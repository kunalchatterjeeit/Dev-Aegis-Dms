using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

namespace DataLayer
{
    public class FileType
    {
        public FileType() { }

        public static int FileType_Save(Entity.FileType fileType)
        {
            using (DataManager oDm = new DataManager())
            {
                oDm.Add("p_FileTypeId", MySqlDbType.Int32, fileType.FileTypeId);
                oDm.Add("p_FileCategoryId", MySqlDbType.Int32, fileType.FileCategoryId);
                oDm.Add("p_Name", MySqlDbType.VarChar, fileType.Name);
                oDm.Add("p_Note", MySqlDbType.VarChar, fileType.Note);

                oDm.CommandType = CommandType.StoredProcedure;
                return oDm.ExecuteNonQuery("usp_FileType_Save");
            }
        }

        public static DataTable FileType_GetAll(Entity.FileType fileType)
        {
            using (DataManager oDm = new DataManager())
            {
                if (fileType.FileCategoryId == 0)
                    oDm.Add("p_FileCategoryId", MySqlDbType.Int32, DBNull.Value);
                else
                    oDm.Add("p_FileCategoryId", MySqlDbType.Int32, fileType.FileCategoryId);

                if (string.IsNullOrEmpty(fileType.Name))
                    oDm.Add("p_Name", MySqlDbType.VarChar, DBNull.Value);
                else
                    oDm.Add("p_Name", MySqlDbType.VarChar, fileType.Name);

                if (fileType.FileTypeId == 0)
                    oDm.Add("p_FileTypeId", MySqlDbType.Int32, DBNull.Value);
                else
                    oDm.Add("p_FileTypeId", MySqlDbType.Int32, fileType.FileTypeId);

                oDm.CommandType = CommandType.StoredProcedure;
                return oDm.ExecuteDataTable("usp_FileType_GetAll");
            }
        }

        public static int FileType_Delete(int fileTypeId)
        {
            using (DataManager oDm = new DataManager())
            {
                oDm.Add("p_FileTypeId", MySqlDbType.VarChar, fileTypeId);

                oDm.CommandType = CommandType.StoredProcedure;
                return oDm.ExecuteNonQuery("usp_FileType_Delete");
            }
        }
    }
}
