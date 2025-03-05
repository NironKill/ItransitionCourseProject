using CustomForms.Application.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace CustomForms.Application.Services.Implementations
{
    public class ApiService : IApiService
    {
        private readonly IConfiguration _configuration;

        public ApiService(IConfiguration configuration) => _configuration = configuration;

        public string GetApiConfiguration(string option)
        {
            string connString = _configuration[option];

            if (!string.IsNullOrEmpty(connString))
                return connString;

            string envString = Environment.GetEnvironmentVariable(option);

            return envString;
        }
    }
}
