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

        public static DataTable FileCategory_GetAll(Entity.FileCategory fileCategory)
        {
            using (DataManager oDm = new DataManager())
            {
                if (fileCategory.FileCategoryId == 0)
                {
                    oDm.Add("p_FileCategoryId", MySqlDbType.Int64, DBNull.Value);
                }
                else
                {
                    oDm.Add("p_FileCategoryId", MySqlDbType.Int64, fileCategory.FileCategoryId);
                }
                oDm.CommandType = CommandType.StoredProcedure;
                return oDm.ExecuteDataTable("usp_FileCategory_GetAll");
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
