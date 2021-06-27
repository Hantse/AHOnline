using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;

namespace Infrastructure.Core.Persistence
{
    public class LocalDb
    {
        private const string StorageFolder = ".localdb";
        private const string ConnectionStringTemplate = @"data source=(localdb)\MSSQLLocalDB;Initial Catalog={0}";

        public string Name { get; }
        public string Directory { get; }
        public string ConnectionString { get; }
        public string MasterDbConnectionString { get; }

        private readonly string sysFile, dataFile, logFile;

        public LocalDb(string name, string directory = null, bool makeDatabaseNameUnique = true)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            this.Name = $"LocalDb_{name}";
            if (makeDatabaseNameUnique)
                this.Name = this.Name + "_" + Guid.NewGuid().ToString("N");

            if (directory != null)
            {
                if (!System.IO.Directory.Exists(directory))
                    throw new ArgumentException("Directory does not exist: " + directory);

                this.Directory = directory;
            }
            else
            {
                this.Directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, StorageFolder);

                if (!System.IO.Directory.Exists(this.Directory))
                    System.IO.Directory.CreateDirectory(this.Directory);
            }

            this.ConnectionString = string.Format(ConnectionStringTemplate, this.Name);
            this.MasterDbConnectionString = string.Format(ConnectionStringTemplate, "master");

            this.sysFile = Path.Combine(this.Directory, this.Name + "_sys.mdf");
            this.dataFile = Path.Combine(this.Directory, this.Name + "_data.ndf");
            this.logFile = Path.Combine(this.Directory, this.Name + "_log.ldf");
        }

        public Task ExecuteScript(string path)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            if (!File.Exists(path))
                throw new ArgumentException($"File '{path}' does not exist.");

            return ExecuteScriptContent(File.ReadAllText(path));
        }

        public Task ExecuteScriptContent(string content) =>
            ExecuteNonQuery(this.ConnectionString, content);

        public Task Create() =>
            ExecuteNonQuery(this.MasterDbConnectionString, $@"
                CREATE DATABASE {this.Name}
                CONTAINMENT = NONE
                ON PRIMARY 
                ( NAME = N'XXXXX_sys', FILENAME = N'{this.sysFile}',
                    SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB ), 
                    FILEGROUP [Tables] DEFAULT
                ( NAME = N'XXXXX_data', FILENAME = N'{this.dataFile}',
                    SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
                    LOG ON 
                ( NAME = N'XXXXX_log', FILENAME = N'{this.logFile}',
                    SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )");

        private bool destroyed = false;

        public async Task Destroy()
        {
            if (!destroyed)
            {
                SqlConnection.ClearAllPools();

                await ExecuteNonQuery(this.MasterDbConnectionString, $@"
                    ALTER DATABASE {this.Name} SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                    DROP DATABASE {this.Name};");

                foreach (var file in new[] { this.sysFile, this.dataFile, this.logFile })
                {
                    try
                    {
                        if (File.Exists(file))
                            File.Delete(file);
                    }
                    catch (Exception e)
                    {
                        throw new Exception($"Could not delete the '{file}' file, please delete it manually", e);
                    }
                }

                destroyed = true;
            }
        }

        /// <summary>
        /// Calling this method before destroy will return you all VARCHAR => NVARCHAR and NVARCHAR => VARCHAR implicit conversions
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<QueryWithImplicitConvertion>> DetectVarcharImplicitConversions()
        {
            var detectedImplicitConvertions = new List<QueryWithImplicitConvertion>();

            using (var connection = new SqlConnection(this.ConnectionString))
            {
                await connection.OpenAsync();

                using (var command = connection.CreateCommand())
                {
                    // https://blog.sqlauthority.com/2017/01/29/find-all-queries-with-implicit-conversion-in-sql-server-interview-question-of-the-week-107/
                    command.CommandText =
                        @"SELECT TOP(100)   qp.query_plan
                        FROM                sys.dm_exec_query_stats AS qs WITH (NOLOCK)
                        CROSS APPLY         sys.dm_exec_sql_text(plan_handle) AS t 
                        CROSS APPLY         sys.dm_exec_query_plan(plan_handle) AS qp 
                        WHERE               CAST(query_plan AS NVARCHAR(MAX)) LIKE ('%CONVERT_IMPLICIT%')
                                        AND t.[dbid] = DB_ID()
                        ORDER BY            qs.total_worker_time DESC OPTION (RECOMPILE)";
                    command.CommandType = CommandType.Text;

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var implicitConversions = QueryWithImplicitConvertion.FromQueryPlan(reader.GetString(0));
                            if (implicitConversions != null)
                                detectedImplicitConvertions.Add(implicitConversions);
                        }
                    }
                }
            }

            return detectedImplicitConvertions;
        }

        private static async Task<int> ExecuteNonQuery(string connectionString, string text)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = text;
                        command.CommandType = CommandType.Text;

                        return await command.ExecuteNonQueryAsync();
                    }
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
        }
    }
}
