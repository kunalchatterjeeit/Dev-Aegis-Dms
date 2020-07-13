
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
namespace DataLayer
{
    public static class Role
    {
        public static int Role_Save(Entity.Role role)
        {
            using (DataManager oDm = new DataManager())
            {
                oDm.Add("p_RoleId", MySqlDbType.Int64, role.RoleId);
                oDm.Add("p_Name", MySqlDbType.VarChar, role.Name);
                oDm.Add("p_Note", MySqlDbType.VarChar, role.Note);

                oDm.CommandType = CommandType.StoredProcedure;
                return oDm.ExecuteNonQuery("usp_Role_Save");
            }
        }

        public static List<Entity.Role> Role_GetAll(int roleId)
        {
            List<Entity.Role> roles = new List<Entity.Role>();
            using (DataManager oDm = new DataManager())
            {
                oDm.Add("p_RoleId", MySqlDbType.Int64, roleId == 0 ? DBNull.Value : (object)roleId);
                oDm.CommandType = CommandType.StoredProcedure;
                using (MySqlDataReader reader = oDm.ExecuteReader("usp_Role_GetAll"))
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            roles.Add(new Entity.Role()
                            {
                                RoleId = Convert.ToInt32(reader["RoleId"].ToString()),
                                Name = reader["Name"].ToString(),
                                Note = reader["Note"].ToString()
                            });
                        }
                    }
                }
                return roles;
            }
        }

        public static int Role_Delete(int roleId)
        {
            using (DataManager oDm = new DataManager())
            {
                oDm.Add("p_RoleId", MySqlDbType.VarChar, roleId);

                oDm.CommandType = CommandType.StoredProcedure;
                return oDm.ExecuteNonQuery("usp_Role_Delete");
            }
        }

        public static int RolePermission_Save(int roleId, int permissionId, bool isEnabled)
        {
            using (DataManager oDm = new DataManager())
            {
                oDm.Add("p_RoleId", MySqlDbType.Int32, roleId);
                oDm.Add("p_PermissionId", MySqlDbType.Int32, permissionId);
                oDm.Add("p_IsEnabled", MySqlDbType.Bit, isEnabled);

                oDm.CommandType = CommandType.StoredProcedure;
                return oDm.ExecuteNonQuery("usp_RolePermission_Save");
            }
        }

        public static List<Entity.RolePermission> RolePermission_GetByRoleId(int roleId)
        {
            List<Entity.RolePermission> rolePermissions = new List<Entity.RolePermission>();
            using (DataManager oDm = new DataManager())
            {
                oDm.Add("p_RoleId", MySqlDbType.Int32, roleId);

                oDm.CommandType = CommandType.StoredProcedure;
                using (MySqlDataReader reader = oDm.ExecuteReader("usp_Role_GetAll"))
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            rolePermissions.Add(new Entity.RolePermission()
                            {
                                RolePermissionId = Convert.ToInt32(reader["RolePermission"].ToString()),
                                RoleId = Convert.ToInt32(reader["RoleId"].ToString()),
                                PermissionId = Convert.ToInt32(reader["PermissionId"].ToString()),
                                IsEnabled = Convert.ToBoolean(reader["IsEnabled"].ToString())
                            });
                        }
                    }
                }
            }
            return rolePermissions;
        }
    }
}
