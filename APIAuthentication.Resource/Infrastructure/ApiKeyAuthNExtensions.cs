using APIAuthentication.Resource.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using System;

namespace Microsoft.AspNetCore.Authentication
{
    public static class ApiKeyAuthNExtensions
    {
        public static AuthenticationBuilder AddApiKey(this AuthenticationBuilder builder, Action<ApiKeyAuthNOptions>? configureOptions)
            => AddApiKey(builder, ApiKeyAuthNDefaults.SchemaName, configureOptions);

        public static AuthenticationBuilder AddApiKey(this AuthenticationBuilder builder, string authenticationScheme, Action<ApiKeyAuthNOptions>? configureOptions)
            => builder.AddScheme<ApiKeyAuthNOptions, ApiKeyAuthN>(authenticationScheme, configureOptions);
    }
}
