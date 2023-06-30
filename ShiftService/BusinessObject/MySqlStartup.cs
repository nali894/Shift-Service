using MySql.Data.MySqlClient;
using System.Data;

namespace ShiftService
{
    public static class MySqlStartup
    {
        private static MySqlConnector.MySqlConnectionStringBuilder builder = Setting.GetStringBuilder();

        public static DataTable CallStoredProcedure_Read(string strStoredProcedure, Dictionary<string, dynamic>? lstParameters= null)
        {
           
            DataTable dt = new DataTable();

            using (MySqlConnection conn = new MySqlConnection(builder.ConnectionString))
            {
                MySqlCommand cmd = GetMySqlCommand(conn, lstParameters, strStoredProcedure);

                if (cmd != null)
                {
                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                    }
                }
            }
            return dt;
        }

        public static bool CallStoredProcedure_Update(Dictionary<string, dynamic> lstParameters, string strStoredProcedure)
        {
            bool bResdult = false;
            using (MySqlConnection conn = new MySqlConnection(builder.ConnectionString))
            {
                using (MySqlCommand cmd = GetMySqlCommand(conn, lstParameters, strStoredProcedure))
                {
                    if (cmd != null)
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        bResdult = true;
                        conn.Close();
                    }
                }
            }

            return bResdult;
        }

        private static MySqlCommand GetMySqlCommand(MySqlConnection conn,Dictionary<string, dynamic> lstParameters, string strStoredProcedure)
        {
            MySqlCommand cmd = null;           
            using (cmd = new MySqlCommand(strStoredProcedure, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;


                if (lstParameters !=null)
                {
                    foreach (KeyValuePair<string, dynamic> parameter in lstParameters)
                    {
                        cmd.Parameters.AddWithValue($"{parameter.Key}", $"{parameter.Value}");
                    }
                }
                                  
            }
            
            return cmd;        
        }

    }
}