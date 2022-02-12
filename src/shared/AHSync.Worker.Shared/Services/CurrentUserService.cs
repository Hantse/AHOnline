using Infrastructure.Core.Entities;
using System.Threading.Tasks;

namespace AHSync.Worker.Shared.Services
{
    public interface ICurrentUserService
    {
        Profile GetProfile();
        Profile SetProfile(Profile p);
        Task WaitUserType();
    }

    public class CurrentUserService : ICurrentUserService
    {
        private static Profile CurrentProfile;

        public Profile GetProfile()
        {
            return CurrentProfile;
        }

        public Profile SetProfile(Profile p)
        {
            CurrentProfile = p;
            return CurrentProfile;
        }

        public async Task WaitUserType()
        {
            if (CurrentProfile.SubscriptionType == "FREE")
            {
                await Task.Delay(2000);
            }
            else if (CurrentProfile.SubscriptionType == "PREMIUM")
            {
                await Task.Delay(1050);
            }
            else
            {
                await Task.Delay(350);
            }
        }
    }
}
