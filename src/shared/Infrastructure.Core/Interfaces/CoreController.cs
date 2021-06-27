using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Infrastructure.Core.Interfaces
{
    [ApiController]
    [Route("[controller]")]
    public class CoreController : ControllerBase
    {
        #region Get Name Identifier
        private const string OBJECT_ID_CLAIM = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
        public string UserId
        {
            get
            {
                return User.Claims.First(c => c.Type.Equals(OBJECT_ID_CLAIM, StringComparison.InvariantCultureIgnoreCase)).Value;
            }
        }

        public string GetObjectIdAsNull
        {
            get
            {
                try
                {
                    return User.Claims.First(c => c.Type.Equals(OBJECT_ID_CLAIM, StringComparison.InvariantCultureIgnoreCase)).Value;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
        #endregion

        #region Get Claims
        private const string EMAIL_CLAIMS = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress";
        public string GetEmail
        {
            get
            {
                var claim = User.Claims.First(c => c.Type.Equals(EMAIL_CLAIMS, StringComparison.InvariantCultureIgnoreCase)).Value;
                return claim;
            }
        }

        private const string ROLES_CLAIMS = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
        public string GetRoles
        {
            get
            {
                var claim = User.Claims.First(c => c.Type.Equals(ROLES_CLAIMS, StringComparison.InvariantCultureIgnoreCase)).Value;
                return claim;
            }
        }
        #endregion
    }
}
