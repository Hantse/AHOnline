using Infrastructure.Core.Persistence;

namespace Infrastructure.Core.Entities
{
    public class Profile : CoreEntity
    {
        public string Username { get; set; }
        public int Discriminator { get; set; }
        public long UserId { get; set; }
        public string Email { get; set; }
        public string SubscriptionType { get; set; }
        public string CustomerId { get; set; }
    }
}
