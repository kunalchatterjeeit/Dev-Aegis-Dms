using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public static class Auth
    {
        public static DataTable Login(string userName)
        {
            using (DataManager oDm = new DataManager())
            {
                oDm.Add("p_UserName", MySqlDbType.VarChar, userName);
                oDm.CommandType = CommandType.StoredProcedure;
                return oDm.ExecuteDataTable("usp_Login");
            }
        }

        public static int Login_Save(Entity.Auth auth)
        {
            using (DataManager oDm = new DataManager())
            {
                oDm.Add("p_UserId", MySqlDbType.Int32, auth.UserId);
                oDm.Add("p_IP", MySqlDbType.VarChar, auth.IP);
                oDm.Add("p_Status", MySqlDbType.VarChar, Enum.GetName(typeof(Entity.LoginStatus), auth.Status));
                oDm.Add("p_Client", MySqlDbType.VarChar, auth.Client);
                oDm.Add("p_FailedUserName", MySqlDbType.VarChar, auth.FailedUserName);
                oDm.Add("p_FailedPassword", MySqlDbType.VarChar, auth.FailedPassword);

                oDm.CommandType = CommandType.StoredProcedure;
                return oDm.ExecuteNonQuery("usp_Login_Save");
            }
        }
    }
}
