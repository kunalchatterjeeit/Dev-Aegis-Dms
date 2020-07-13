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
                oDm.Add("p_Password", MySqlDbType.VarChar, string.IsNullOrEmpty(user.Password) ? DBNull.Value : (object)user.Password);
                oDm.Add("p_CreatedBy", MySqlDbType.Int32, user.CreatedBy);
                oDm.Add("p_Status", MySqlDbType.Int32, user.Status);
                oDm.Add("p_EmailId", MySqlDbType.VarChar, user.EmailId);
                oDm.Add("p_DesignationId", MySqlDbType.Int32, user.DesignationId);

                oDm.CommandType = CommandType.StoredProcedure;
                retValue = oDm.ExecuteNonQuery("usp_User_Save");
                return retValue;
            }
        }

        public static List<Entity.User> User_GetAll(Entity.User user)
        {
            List<Entity.User> users = new List<Entity.User>();
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
                using (MySqlDataReader reader = oDm.ExecuteReader("usp_User_GetAll"))
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            users.Add(new Entity.User()
                            {
                                UserId = Convert.ToInt32(reader["UserId"].ToString()),
                                Name = reader["Name"].ToString(),
                                Username = reader["Username"].ToString(),
                                DesignationName = reader["DesignationName"].ToString(),
                                DesignationId = Convert.ToInt32(reader["DesignationId"].ToString()),
                                EmailId = reader["Email"].ToString(),
                                Status = (Entity.UserStatus)Enum.Parse(typeof(Entity.UserStatus), reader["Status"].ToString()),
                                LoginStatus = (string.IsNullOrEmpty(reader["LoginStatus"].ToString())) ? Entity.ApplicationLoginStatus.InActive : (Entity.ApplicationLoginStatus)Enum.Parse(typeof(Entity.ApplicationLoginStatus), reader["LoginStatus"].ToString()),
                                CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString()),
                            });
                        }
                    }
                }
            }
            return users;
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

        public static int User_LoginStatusChange(int userId, int loginStatus, int modifiedBy)
        {
            using (DataManager oDm = new DataManager())
            {
                oDm.Add("p_UserId", MySqlDbType.Int32, userId);
                oDm.Add("p_LoginStatus", MySqlDbType.Int32, loginStatus);
                oDm.Add("p_ModifiedBy", MySqlDbType.Int32, modifiedBy);

                oDm.CommandType = CommandType.StoredProcedure;
                return oDm.ExecuteNonQuery("usp_User_LoginStatusChange");
            }
        }

        public static List<int> UserRole_GetByUserId(int userId)
        {
            List<int> userRoles = new List<int>();
            using (DataManager oDm = new DataManager())
            {
                oDm.Add("p_UserId", MySqlDbType.Int32, userId);
                oDm.CommandType = CommandType.StoredProcedure;
                using (MySqlDataReader reader = oDm.ExecuteReader("usp_UserRole_GetByUserId"))
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            userRoles.Add(Convert.ToInt32(reader["RoleId"].ToString()));
                        }
                    }
                }
                return userRoles;
            }
        }
    }
}
