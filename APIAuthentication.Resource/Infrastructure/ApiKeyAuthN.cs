using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace APIAuthentication.Resource.Infrastructure
{
    public class ApiKeyAuthN : AuthenticationHandler<ApiKeyAuthNOptions>
    {
        public ApiKeyAuthN(IOptionsMonitor<ApiKeyAuthNOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var apiKey = ParseApiKey();

            if (string.IsNullOrEmpty(apiKey))
                return Task.FromResult(AuthenticateResult.NoResult());

            if (string.Compare(apiKey, Options.ApiKey, StringComparison.Ordinal) == 0)
            {
                var principal = BuildPrincipal(Scheme.Name, Options.ApiKey, "APIKey");
                return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(principal, Scheme.Name)));
            }

            return Task.FromResult(AuthenticateResult.Fail($"Invalid API Key provided."));
        }

        static ClaimsPrincipal BuildPrincipal(string schemeName, string name, string issuer, params Claim[] claims)
        {
            var identity = new ClaimsIdentity(schemeName);

            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, name, ClaimValueTypes.String, issuer));
            identity.AddClaim(new Claim(ClaimTypes.Name, name, ClaimValueTypes.String, issuer));

            identity.AddClaims(claims);

            var principal = new ClaimsPrincipal(identity);
            return principal;
        }

        protected string ParseApiKey()
        {
            if (Request.Query.TryGetValue(Options.QueryStringKey, out var value))
                return value.FirstOrDefault();

            return string.Empty;
        }
    }
}