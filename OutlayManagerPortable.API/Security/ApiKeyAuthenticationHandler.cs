using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace OutlayManagerPortable.API.Security
{
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public const string KEY_HEADER = "ApiKey";
        private readonly IConfiguration _configuration;

        public ApiKeyAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder,
                                           ISystemClock clock,IConfiguration configuration) : base(options, logger, encoder, clock)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(IConfiguration));
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (Request.Headers.ContainsKey(KEY_HEADER))
            {
                var apiKeyValue = Request.Headers[KEY_HEADER];

                if (_configuration[KEY_HEADER] == apiKeyValue)
                {
                    var identity = new ClaimsIdentity("Token");
                    var principal = new ClaimsPrincipal(identity);
                    AuthenticationTicket ticket = new AuthenticationTicket(principal, Scheme.Name);

                    return Task.FromResult(AuthenticateResult.Success(ticket));
                }
            }

            return Task.FromResult(AuthenticateResult.Fail(""));
        }
    }
}
