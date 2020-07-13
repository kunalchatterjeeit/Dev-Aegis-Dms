using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

namespace DataLayer
{
    public class FileCategory
    {
        public FileCategory()
        { }

        public static int FileCategory_Save(Entity.FileCategory fileCategory)
        {
            using (DataManager oDm = new DataManager())
            {
                oDm.Add("p_FileCategoryId", MySqlDbType.Int64, fileCategory.FileCategoryId);
                oDm.Add("p_Name", MySqlDbType.VarChar, fileCategory.Name);
                oDm.Add("p_Note", MySqlDbType.VarChar, fileCategory.Note);

                oDm.CommandType = CommandType.StoredProcedure;
                return oDm.ExecuteNonQuery("usp_FileCategory_Save");
            }
        }

        public static List<Entity.FileCategory> FileCategory_GetAll(Entity.FileCategory fileCategory)
        {
            List<Entity.FileCategory> fileCategories = new List<Entity.FileCategory>();
            using (DataManager oDm = new DataManager())
            {
                oDm.Add("p_FileCategoryId", MySqlDbType.Int64, fileCategory.FileCategoryId > 0 ? (object)fileCategory.FileCategoryId : DBNull.Value);
                oDm.CommandType = CommandType.StoredProcedure;
                using (MySqlDataReader reader = oDm.ExecuteReader("usp_FileCategory_GetAll"))
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            fileCategories.Add(new Entity.FileCategory()
                            {
                                FileCategoryId = Convert.ToInt32(reader["FileCategoryId"].ToString()),
                                Name = reader["Name"].ToString(),
                                Note = reader["Note"].ToString()
                            });
                        }
                    }
                }
                return fileCategories;
            }
        }

        public static int FileCategory_Delete(int fileCategoryId)
        {
            using (DataManager oDm = new DataManager())
            {
                oDm.Add("p_FileCategoryId", MySqlDbType.VarChar, fileCategoryId);

                oDm.CommandType = CommandType.StoredProcedure;
                return oDm.ExecuteNonQuery("usp_FileCategory_Delete");
            }
        }
    }
}
