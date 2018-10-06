
using MySql.Data.MySqlClient;
using System;
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

        public static DataTable Role_GetAll()
        {
            using (DataManager oDm = new DataManager())
            {
                oDm.CommandType = CommandType.StoredProcedure;
                return oDm.ExecuteDataTable("usp_Role_GetAll");
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

        public static DataTable RolePermission_GetByRoleId(int roleId)
        {
            using (DataManager oDm = new DataManager())
            {
                oDm.Add("p_RoleId", MySqlDbType.Int32, roleId);

                oDm.CommandType = CommandType.StoredProcedure;
                return oDm.ExecuteDataTable("usp_RolePermission_GetByRoleId");
            }
        }
    }
}
