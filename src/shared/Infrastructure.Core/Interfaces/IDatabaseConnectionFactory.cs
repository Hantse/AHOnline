using System.Data;

namespace Infrastructure.Core.Interfaces
{
    public interface IDatabaseConnectionFactory
    {
        IDbConnection GetConnection();
    }
}
