using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public static class Role
    {
        public static int RoleSave(Entity.Role role)
        {
            return DataLayer.Role.Role_Save(role);
        }

        public static DataTable RoleGetAll()
        {
            return DataLayer.Role.Role_GetAll();
        }

        public static int RoleDelete(int roleId)
        {
            return DataLayer.Role.Role_Delete(roleId);
        }

        public static int RolePermissionSave(int roleId, int permissionId, bool isEnabled)
        {
            return DataLayer.Role.RolePermission_Save(roleId, permissionId, isEnabled);
        }

        public static DataTable RolePermissionGetByRoleId(int roleId)
        {
            return DataLayer.Role.RolePermission_GetByRoleId(roleId);
        }
    }
}
