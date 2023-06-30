using MySqlConnector;

namespace ShiftService
{
    public static class Setting
    {
        public static MySqlConnectionStringBuilder GetStringBuilder()
        {

            MySqlConnectionStringBuilder? builder = new MySqlConnectionStringBuilder
            {
                Server = Environment.GetEnvironmentVariable("HostName"),
                Database = Environment.GetEnvironmentVariable("Database"),
                UserID = Environment.GetEnvironmentVariable("ID"),
                Password = Environment.GetEnvironmentVariable("Password"),
                SslMode = MySqlSslMode.Required
            };

            return builder;
        }

    }
}
