using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

namespace DataLayer
{
    public class UserGroup
    {
        public UserGroup()
        { }

        public static int UserGroup_Save(Entity.UserGroup UserGroup)
        {
            using (DataManager oDm = new DataManager())
            {
                oDm.Add("p_UserGroupId", MySqlDbType.Int32, UserGroup.UserGroupId);
                oDm.Add("p_Name", MySqlDbType.VarChar, UserGroup.Name);
                oDm.Add("p_Note", MySqlDbType.VarChar, UserGroup.Note);

                oDm.CommandType = CommandType.StoredProcedure;
                return oDm.ExecuteNonQuery("usp_UserGroup_Save");
            }
        }

        public static List<Entity.UserGroup> UserGroup_GetAll(Entity.UserGroup UserGroup)
        {
            List<Entity.UserGroup> userGroups = new List<Entity.UserGroup>();
            using (DataManager oDm = new DataManager())
            {
                if (UserGroup.UserGroupId == 0)
                {
                    oDm.Add("p_UserGroupId", MySqlDbType.Int32, DBNull.Value);
                }
                else
                {
                    oDm.Add("p_UserGroupId", MySqlDbType.Int32, UserGroup.UserGroupId);
                }
                oDm.CommandType = CommandType.StoredProcedure;
                using (MySqlDataReader reader = oDm.ExecuteReader("usp_UserGroup_GetAll"))
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            userGroups.Add(new Entity.UserGroup()
                            {
                                UserGroupId = Convert.ToInt32(reader["UserGroupId"].ToString()),
                                Name = reader["Name"].ToString(),
                                Note = reader["Note"].ToString()
                            });
                        }
                    }
                }
                return userGroups;
            }
        }

        public static int UserGroup_Delete(int UserGroupId)
        {
            using (DataManager oDm = new DataManager())
            {
                oDm.Add("p_UserGroupId", MySqlDbType.Int32, UserGroupId);

                oDm.CommandType = CommandType.StoredProcedure;
                return oDm.ExecuteNonQuery("usp_UserGroup_Delete");
            }
        }

        public static int UserGroupUserMapping_Save(Entity.UserGroup userGroup)
        {
            using (DataManager oDm = new DataManager())
            {
                oDm.Add("p_UserGroupUserMappingId", MySqlDbType.Int64, userGroup.UserGroupUserMappingId);
                oDm.Add("p_UserGroupId", MySqlDbType.Int32, userGroup.UserGroupId);
                oDm.Add("p_UserId", MySqlDbType.Int32, userGroup.UserId);
                oDm.Add("p_CreatedDate", MySqlDbType.DateTime, userGroup.CreatedDate);
                oDm.Add("p_CreatedBy", MySqlDbType.Int32, userGroup.CreatedBy);
                oDm.Add("p_Status", MySqlDbType.Int32, userGroup.Status);

                oDm.CommandType = CommandType.StoredProcedure;
                return oDm.ExecuteNonQuery("usp_UserGroupUserMapping_Save");
            }
        }

        public static int UserGroupFileMapping_Save(Entity.UserGroup userGroup)
        {
            using (DataManager oDm = new DataManager())
            {
                oDm.Add("p_UserGroupFileMappingId", MySqlDbType.Int64, userGroup.UserGroupFileMappingId);
                oDm.Add("p_UserGroupId", MySqlDbType.Int32, userGroup.UserGroupId);
                oDm.Add("p_FileGuid", MySqlDbType.VarChar, userGroup.FileGuid);
                oDm.Add("p_CreatedDate", MySqlDbType.DateTime, userGroup.CreatedDate);
                oDm.Add("p_CreatedBy", MySqlDbType.Int32, userGroup.CreatedBy);
                oDm.Add("p_Status", MySqlDbType.Int32, userGroup.Status);

                oDm.CommandType = CommandType.StoredProcedure;
                return oDm.ExecuteNonQuery("usp_UserGroupFileMapping_Save");
            }
        }

        public static List<Entity.UserGroup> UserGroup_GetByUserId(int userId)
        {
            List<Entity.UserGroup> userGroups = new List<Entity.UserGroup>();
            using (DataManager oDm = new DataManager())
            {
                oDm.Add("p_UserId", MySqlDbType.Int32, userId);
                oDm.CommandType = CommandType.StoredProcedure;
                using (MySqlDataReader reader = oDm.ExecuteReader("usp_UserGroup_GetByUserId"))
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            userGroups.Add(new Entity.UserGroup()
                            {
                                UserGroupUserMappingId = Convert.ToInt64(reader["UserGroupUserMappingId"].ToString()),
                                UserGroupId = Convert.ToInt32(reader["UserGroupId"].ToString())
                            });
                        }
                    }
                }
                return userGroups;
            }
        }
    }
}
