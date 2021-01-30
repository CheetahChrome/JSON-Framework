using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSON_Display.Extensions
{
    public static class DatabaseExtensions
    {
        public static async Task<bool> IsConnectionViable(this string connectionStr)
        {
            await using var sqlConn = new SqlConnection(connectionStr);
            return await sqlConn.IsConnectionViable();
        }

        public static async Task<bool> IsConnectionViable(this SqlConnection connection)
        {
            var isConnected = false;

            try
            {
                await connection.OpenAsync();
                isConnected = (connection.State == ConnectionState.Open);
            }
            catch (Exception)
            {
                // ignored
            }

            return isConnected;
        }
    }
}
