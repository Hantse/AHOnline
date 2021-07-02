using System;
using System.Text.Json.Serialization;

namespace Infrastructure.Core.Persistence
{
    public class CoreEntity
    {
        [JsonPropertyName("createAt")]
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;

        [JsonPropertyName("createBy")]
        public string CreateBy { get; set; }

        [JsonPropertyName("deleteAt")]
        public DateTime? DeleteAt { get; set; }

        [JsonPropertyName("deleteBy")]
        public string DeleteBy { get; set; }

        [JsonPropertyName("updateAt")]
        public DateTime? UpdateAt { get; set; }

        [JsonPropertyName("updateBy")]
        public string UpdateBy { get; set; }
    }
}
