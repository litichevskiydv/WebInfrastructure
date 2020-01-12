namespace Skeleton.Web.Authentication.JwtBearer.Configuration.Issuing
{
    using System;
    using Microsoft.Extensions.Options;

    public class TokenIssueEventHandlerSetup<TEventHandler> : IConfigureOptions<TokensIssuingOptions>
        where TEventHandler : ITokenIssueEventHandler
    {
        private readonly ITokenIssueEventHandler _eventHandler;

        public TokenIssueEventHandlerSetup(TEventHandler eventHandler)
        {
            if (eventHandler == null)
                throw new ArgumentNullException(nameof(eventHandler));

            _eventHandler = eventHandler;
        }

        public void Configure(TokensIssuingOptions options)
        {
            options.WithTokenIssueEventHandler(_eventHandler);
        }
    }
}