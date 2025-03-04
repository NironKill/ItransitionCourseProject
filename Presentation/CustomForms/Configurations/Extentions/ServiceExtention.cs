using CustomForms.Application.Services.Implementations;
using CustomForms.Application.Services.Interfaces;
using CustomForms.Persistence.Services.Jira;

namespace CustomForms.Configurations.Extentions
{
    internal static class ServiceExtention
    {
        internal static WebApplicationBuilder ConfigureService(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IAccessTokenService, AccessTokenService>();
            builder.Services.AddScoped<IJiraService, JiraService>();
            builder.Services.AddScoped<IJiraApiService, JiraApiService>();
            builder.Services.AddScoped<IApiService, ApiService>();

            return builder;
        }
    }
}
