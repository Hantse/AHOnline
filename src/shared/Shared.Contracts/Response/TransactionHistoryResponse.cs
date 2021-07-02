using System.Text.Json.Serialization;

namespace Shared.Contracts.Response
{
    public class TransactionHistoryResponse
    {
        [JsonPropertyName("realmName")]
        public string RealmName { get; set; }

        [JsonPropertyName("realmFaction")]
        public string RealmFaction { get; set; }

        [JsonPropertyName("averageTime")]
        public double AverageTime { get; set; }

        [JsonPropertyName("upperTime")]
        public long UpperTime { get; set; }

        [JsonPropertyName("lowerTime")]
        public long LowerTime { get; set; }

        [JsonPropertyName("maxItem")]
        public long MaxItem { get; set; }
    }
}
