using Microsoft.AspNetCore.Authentication;

namespace APIAuthentication.Resource.Infrastructure
{
    public class ApiKeyAuthNOptions : AuthenticationSchemeOptions
    {
        public string ApiKey { get; set; }

        public string QueryStringKey { get; set; }
    }
}
