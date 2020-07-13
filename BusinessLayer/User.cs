using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BusinessLayer
{
    public class User : Disposable
    {
        public User() { }

        public int UserSave(Entity.User user)
        {
            return DataLayer.User.User_Save(user);
        }

        public List<Entity.User> UserGetAll(Entity.User user)
        {
            List<Entity.User> users = DataLayer.User.User_GetAll(user);
            foreach (Entity.User u in users)
            {
                foreach (Entity.UserGroup userGroup in new UserGroup().UserGroup_GetByUserId(u.UserId))
                    u.SelectedUserGroups.Add(userGroup.UserGroupId);
                foreach (int roleId in new User().UserRole_GetByUserId(u.UserId))
                    u.SelectedUserRoles.Add(roleId);
            }
            return users;
        }

        public int UserStatusChange(int userId, int status, int modifiedBy)
        {
            return DataLayer.User.User_StatusChange(userId, status, modifiedBy);
        }

        public int UserRole_Save(int userId, int roleId, bool isEnable)
        {
            return DataLayer.User.UserRole_Save(userId, roleId, isEnable);
        }

        public int User_LoginStatusChange(int userId, int loginStatus, int modifiedBy)
        {
            return DataLayer.User.User_LoginStatusChange(userId, loginStatus, modifiedBy);
        }

        public List<int> UserRole_GetByUserId(int userId)
        {
            return DataLayer.User.UserRole_GetByUserId(userId);
        }

        ~User()
        {
            Dispose(false);
        }
    }
}
