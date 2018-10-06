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

        public static int UserGroupSave(Entity.UserGroup UserGroup)
        {
            return DataLayer.UserGroup.UserGroup_Save(UserGroup);
        }

        public static DataTable UserGroupGetAll(Entity.UserGroup UserGroup)
        {
            return DataLayer.UserGroup.UserGroup_GetAll(UserGroup);
        }

        public static int UserGroupDelete(int UserGroupId)
        {
            return DataLayer.UserGroup.UserGroup_Delete(UserGroupId);
        }

        public static int UserGroupUserMapping_Save(Entity.UserGroup userGroup)
        {
            return DataLayer.UserGroup.UserGroupUserMapping_Save(userGroup);
        }

        public static int UserGroupFileMapping_Save(Entity.UserGroup userGroup)
        {
            return DataLayer.UserGroup.UserGroupFileMapping_Save(userGroup);
        }

        ~UserGroup()
        {
            Dispose(false);
        }
    }
}
