namespace Web.Application.Services
{
    using System;
    using System.Security.Claims;
    using Microsoft.Extensions.Logging;
    using Skeleton.Web.Authentication.JwtBearer;

    public class TokenIssueEventHandler: ITokenIssueEventHandler
    {
        readonly ILogger<TokenIssueEventHandler> _logger;

        public TokenIssueEventHandler(ILogger<TokenIssueEventHandler> logger)
        {
            _logger = logger;
        }

        public void IssueSuccessEventHandle(string token, Claim[] user)
        {
            _logger.LogDebug("Token issued successfully. Token: {0}", token);
        }

        public void LoginNotFoundEventHandle(string login)
        {
            _logger.LogDebug("Token not issued. Login {0} not found.", login);
        }

        public void IncorrectPasswordEventHandle(string login, string password)
        {
            _logger.LogDebug("Token not issued. Incorrect password {0} for login {1}.", password, login);
        }
    }
}
