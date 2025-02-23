using CustomForms.Application.Services.Implementations;
using CustomForms.Application.Services.Interfaces;

namespace CustomForms.Configurations.Extentions
{
    internal static class ServiceExtention
    {
        internal static WebApplicationBuilder ConfigureService(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IAccessTokenService, AccessTokenService>();

            return builder;
        }
    }
}
