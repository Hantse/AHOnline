using Infrastructure.Core.Persistence;

namespace Infrastructure.Core.Entities
{
    public class DiscordSync : CoreEntity
    {
        public string Username { get; set; }
        public int Discriminator { get; set; }
        public long UserId { get; set; }
    }
}
