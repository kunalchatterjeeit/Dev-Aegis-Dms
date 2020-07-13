using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class Role
    {
        public int RoleSave(Entity.Role role)
        {
            return DataLayer.Role.Role_Save(role);
        }

        public List<Entity.Role> RoleGetAll(int roleId)
        {
            return DataLayer.Role.Role_GetAll(roleId);
        }

        public int RoleDelete(int roleId)
        {
            return DataLayer.Role.Role_Delete(roleId);
        }

        public int RolePermissionSave(int roleId, int permissionId, bool isEnabled)
        {
            return DataLayer.Role.RolePermission_Save(roleId, permissionId, isEnabled);
        }

        public List<Entity.RolePermission> RolePermissionGetByRoleId(int roleId)
        {
            return DataLayer.Role.RolePermission_GetByRoleId(roleId);
        }
    }
}
