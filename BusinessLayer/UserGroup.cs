using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BusinessLayer
{
    public class UserGroup : Disposable
    {
        public UserGroup()
        { }

        public int UserGroupSave(Entity.UserGroup UserGroup)
        {
            return DataLayer.UserGroup.UserGroup_Save(UserGroup);
        }

        public List<Entity.UserGroup> UserGroupGetAll(Entity.UserGroup UserGroup)
        {
            return DataLayer.UserGroup.UserGroup_GetAll(UserGroup);
        }

        public int UserGroupDelete(int UserGroupId)
        {
            return DataLayer.UserGroup.UserGroup_Delete(UserGroupId);
        }

        public int UserGroupUserMapping_Save(Entity.UserGroup userGroup)
        {
            return DataLayer.UserGroup.UserGroupUserMapping_Save(userGroup);
        }

        public static int UserGroupFileMapping_Save(Entity.UserGroup userGroup)
        {
            return DataLayer.UserGroup.UserGroupFileMapping_Save(userGroup);
        }

        public List<Entity.UserGroup> UserGroup_GetByUserId(int userId)
        {
            return DataLayer.UserGroup.UserGroup_GetByUserId(userId);
        }

        public int UserGroupFileMapping_Delete(string fileGuid)
        {
            return DataLayer.UserGroup.UserGroupFileMapping_Delete(fileGuid);
        }

        ~UserGroup()
        {
            Dispose(false);
        }
    }
}
