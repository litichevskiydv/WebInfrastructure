namespace Web.Application.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Endpoint for account information
    /// </summary>
    public class AccountController : Controller
    {
        /// <summary>
        /// Get all claims for current user
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("UserInfo")]
        public IEnumerable<string> UserInfo()
        {
            var identity = (ClaimsIdentity)User.Identity;
            return identity.Claims
                .Select(x => $"{x.Type}:{x.Value}")
                .ToArray();
        }
    }
}