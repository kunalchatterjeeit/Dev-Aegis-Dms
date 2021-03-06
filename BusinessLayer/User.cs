﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BusinessLayer
{
    public class User : Disposable
    {
        public User() { }

        public static int UserSave(Entity.User user)
        {
            return DataLayer.User.User_Save(user);
        }

        public static DataTable UserGetAll(Entity.User user)
        {
            return DataLayer.User.User_GetAll(user);
        }

        public static int UserStatusChange(int userId, int status, int modifiedBy)
        {
            return DataLayer.User.User_StatusChange(userId, status, modifiedBy);
        }

        public static int UserRole_Save(int userId, int roleId, bool isEnable)
        {
            return DataLayer.User.UserRole_Save(userId, roleId, isEnable);
        }

        ~User()
        {
            Dispose(false);
        }
    }
}
