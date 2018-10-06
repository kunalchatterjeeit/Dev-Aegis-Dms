using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class File
    {
        public File() { }

        public static Guid? File_Save(Entity.File file)
        {
            Guid? fileGuid = Guid.NewGuid();
            using (DataManager oDm = new DataManager())
            {
                oDm.Add("p_FileTypeId", MySqlDbType.Int32, file.FileTypeId);
                oDm.Add("p_FileGuid", MySqlDbType.VarChar, fileGuid);
                oDm.Add("p_PhysicalFileName", MySqlDbType.VarChar, fileGuid);
                oDm.Add("p_FileOriginalName", MySqlDbType.VarChar, file.FileOriginalName);
                oDm.Add("p_FileExtension", MySqlDbType.VarChar, file.FileExtension);
                oDm.Add("p_EntryDate", MySqlDbType.DateTime, file.EntryDate);
                oDm.Add("p_IsFullTextCopied", MySqlDbType.Bit, file.IsFullTextCopied);
                oDm.Add("p_IsAttachment", MySqlDbType.Bit, file.IsAttachment);
                oDm.Add("p_MainFileGuid", MySqlDbType.VarChar, file.MainFileGuid);
                oDm.Add("p_CreatedDate", MySqlDbType.DateTime, file.CreatedDate);
                oDm.Add("p_CreatedBy", MySqlDbType.Int32, file.CreatedBy);
                oDm.Add("p_FileStatus", MySqlDbType.Int32, file.FileStatus);

                oDm.CommandType = CommandType.StoredProcedure;
                int retVal = oDm.ExecuteNonQuery("usp_File_Save");

                if (retVal > 0)
                {
                    return fileGuid;
                }
                else
                {
                    return null;
                }
            }
        }

        public static Guid File_Update(Entity.File file)
        {
            Guid fileGuid = new Guid();
            using (DataManager oDm = new DataManager())
            {
                oDm.Add("p_FileTypeId", MySqlDbType.Int32, file.FileTypeId);
                oDm.Add("p_FileGuid", MySqlDbType.VarChar, fileGuid);
                oDm.Add("p_PhysicalFileName", MySqlDbType.VarChar, file.PhysicalFileName);
                oDm.Add("p_FileOriginalName", MySqlDbType.VarChar, file.FileOriginalName);
                oDm.Add("p_EntryDate", MySqlDbType.DateTime, file.EntryDate);
                oDm.Add("p_IsFullTextCopied", MySqlDbType.Bit, file.IsFullTextCopied);
                oDm.Add("p_IsAttachment", MySqlDbType.Bit, file.IsAttachment);
                oDm.Add("p_MainFileGuid", MySqlDbType.VarChar, file.MainFileGuid);
                oDm.Add("p_LastModifiedDate", MySqlDbType.DateTime, file.CreatedDate);
                oDm.Add("p_LastModifiedBy", MySqlDbType.Int32, file.CreatedBy);
                oDm.Add("p_FileStatus", MySqlDbType.Int32, file.FileStatus);

                oDm.CommandType = CommandType.StoredProcedure;
                int retVal = oDm.ExecuteNonQuery("usp_File_Update");

                if (retVal > 0)
                {
                    return fileGuid;
                }
                else
                {
                    return fileGuid; //check
                }
            }
        }

        public static DataTable File_GetAll(Entity.File file)
        {
            using (DataManager oDm = new DataManager())
            {
                oDm.CommandType = CommandType.StoredProcedure;
                return oDm.ExecuteDataTable("usp_File_GetAll");
            }
        }

        public static DataTable GetPendingContentTransferFiles()
        {
            using (DataManager oDm = new DataManager())
            {
                oDm.CommandType = CommandType.StoredProcedure;
                return oDm.ExecuteDataTable("usp_GetPendingContentTransferFiles");
            }
        }

        public static int File_Content_Save(Guid fileGuid, string content)
        {
            using (DataManager oDm = new DataManager())
            {
                oDm.Add("p_FileGuid", MySqlDbType.VarChar, fileGuid);
                oDm.Add("p_Content", MySqlDbType.LongText, content);

                oDm.CommandType = CommandType.StoredProcedure;
                return oDm.ExecuteNonQuery("usp_File_Content_Save");
            }
        }

        public static DataTable File_SearchByMetadata(string condition, int userId)
        {
            using (DataManager oDm = new DataManager())
            {
                oDm.Add("p_Condition", MySqlDbType.VarChar, condition);
                oDm.Add("p_UserId", MySqlDbType.Int32, userId);

                oDm.CommandType = CommandType.StoredProcedure;
                return oDm.ExecuteDataTable("usp_File_SearchByMetadata");
            }
        }

        public static DataTable File_SearchByPhrase(string phrase, int userId)
        {
            using (DataManager oDm = new DataManager())
            {
                oDm.Add("p_phrase", MySqlDbType.VarChar, phrase);
                oDm.Add("p_UserId", MySqlDbType.Int32, userId);

                oDm.CommandType = CommandType.StoredProcedure;
                return oDm.ExecuteDataTable("usp_File_SearchByPhrase");
            }
        }

        public static DataTable File_GetByFileGuid(string fileGuid)
        {
            using (DataManager oDm = new DataManager())
            {
                oDm.Add("p_FileGuid", MySqlDbType.VarChar, fileGuid);

                oDm.CommandType = CommandType.StoredProcedure;
                return oDm.ExecuteDataTable("usp_File_GetByFileGuid");
            }
        }

        public static int File_StatusChange(Guid fileGuid, int status)
        {
            using (DataManager oDm = new DataManager())
            {
                oDm.Add("p_FileGuid", MySqlDbType.VarChar, fileGuid);
                oDm.Add("p_Status", MySqlDbType.Int32, status);

                oDm.CommandType = CommandType.StoredProcedure;
                return oDm.ExecuteNonQuery("usp_File_StatusChange");
            }
        }

        public static int File_Delete(Guid fileGuid)
        {
            using (DataManager oDm = new DataManager())
            {
                oDm.Add("p_FileGuid", MySqlDbType.VarChar, fileGuid);

                oDm.CommandType = CommandType.StoredProcedure;
                return oDm.ExecuteNonQuery("usp_File_Delete");
            }
        }
    }
}
