using CustomForms.Persistence.Settings;

namespace CustomForms.Configurations
{
    internal static class Connection
    {
        internal static string GetConfigurationDB(string defaultConnection)
        {
            if (!string.IsNullOrEmpty(defaultConnection))
                return defaultConnection;

            string envString = Environment.GetEnvironmentVariable(DataBaseSet.Configuration);

            return envString;
        }
    }
}
