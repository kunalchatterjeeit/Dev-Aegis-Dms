using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class User
    {
        public User() { }

        public static int User_Save(Entity.User user)
        {
            int retValue = 0;
            using (DataManager oDm = new DataManager())
            {
                oDm.Add("p_UserId", MySqlDbType.Int32, user.UserId);
                oDm.Add("p_Name", MySqlDbType.VarChar, user.Name);
                oDm.Add("p_Username", MySqlDbType.VarChar, user.Username);
                oDm.Add("p_Password", MySqlDbType.VarChar, user.Password);
                oDm.Add("p_CreatedBy", MySqlDbType.Int32, user.CreatedBy);
                oDm.Add("p_Status", MySqlDbType.Int32, user.Status);

                oDm.CommandType = CommandType.StoredProcedure;
                retValue = oDm.ExecuteNonQuery("usp_User_Save");
                return retValue;
            }
        }

        public static DataTable User_GetAll(Entity.User user)
        {
            using (DataManager oDm = new DataManager())
            {
                if (string.IsNullOrEmpty(user.Name))
                    oDm.Add("p_Name", MySqlDbType.VarChar, DBNull.Value);
                else
                    oDm.Add("p_Name", MySqlDbType.VarChar, user.Name);

                if (string.IsNullOrEmpty(user.Username))
                    oDm.Add("p_UserName", MySqlDbType.VarChar, DBNull.Value);
                else
                    oDm.Add("p_UserName", MySqlDbType.VarChar, user.Username);

                if (user.UserId == 0)
                    oDm.Add("p_UserId", MySqlDbType.Int32, DBNull.Value);
                else
                    oDm.Add("p_UserId", MySqlDbType.Int32, user.UserId);

                oDm.CommandType = CommandType.StoredProcedure;
                return oDm.ExecuteDataTable("usp_User_GetAll");
            }
        }

        public static int User_StatusChange(int userId, int status, int modifiedBy)
        {
            using (DataManager oDm = new DataManager())
            {
                oDm.Add("p_UserId", MySqlDbType.Int32, userId);
                oDm.Add("p_Status", MySqlDbType.Int32, status);
                oDm.Add("p_ModifiedBy", MySqlDbType.Int32, modifiedBy);

                oDm.CommandType = CommandType.StoredProcedure;
                return oDm.ExecuteNonQuery("usp_User_StatusChange");
            }
        }

        public static int UserRole_Save(int userId, int roleId, bool isEnabled)
        {
            int retValue = 0;
            using (DataManager oDm = new DataManager())
            {
                oDm.Add("p_UserId", MySqlDbType.Int32, userId);
                oDm.Add("p_RoleId", MySqlDbType.VarChar, roleId);
                oDm.Add("p_IsEnable", MySqlDbType.Bit, isEnabled);

                oDm.CommandType = CommandType.StoredProcedure;
                retValue = oDm.ExecuteNonQuery("usp_UserRole_Save");
                return retValue;
            }
        }
    }
}
