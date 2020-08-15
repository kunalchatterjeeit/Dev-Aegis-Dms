using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

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
                oDm.Add("p_SizeInKb", MySqlDbType.Decimal, file.SizeInKb);

                oDm.CommandType = CommandType.StoredProcedure;
                int retVal = oDm.ExecuteNonQuery("usp_File_Save");
                oDm.Dispose();
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
                oDm.Dispose();
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
                DataTable retValue = oDm.ExecuteDataTable("usp_File_GetAll");
                oDm.Dispose();
                return retValue;
            }
        }

        public static DataTable GetPendingContentTransferFiles()
        {
            using (DataManager oDm = new DataManager())
            {
                oDm.CommandType = CommandType.StoredProcedure;
                DataTable retValue = oDm.ExecuteDataTable("usp_GetPendingContentTransferFiles");
                oDm.Dispose();
                return retValue;
            }
        }

        public static int File_Content_Save(Guid fileGuid, string content)
        {
            using (DataManager oDm = new DataManager())
            {
                oDm.Add("p_FileGuid", MySqlDbType.VarChar, fileGuid);
                oDm.Add("p_Content", MySqlDbType.LongText, content);

                oDm.CommandType = CommandType.StoredProcedure;
                int retValue = oDm.ExecuteNonQuery("usp_File_Content_Save");
                oDm.Dispose();
                return retValue;
            }
        }

        public static List<Entity.SearchResult> File_SearchByMetadata(string condition, int userId, int pageIndex, int pageSize)
        {
            List<Entity.SearchResult> files = new List<Entity.SearchResult>();
            using (DataManager oDm = new DataManager())
            {
                oDm.Add("p_Condition", MySqlDbType.VarChar, condition);
                oDm.Add("p_UserId", MySqlDbType.Int32, userId);
                oDm.Add("p_PageIndex", MySqlDbType.Int32, pageIndex);
                oDm.Add("p_PageSize", MySqlDbType.Int32, pageSize);

                oDm.CommandType = CommandType.StoredProcedure;
                using (MySqlDataReader reader = oDm.ExecuteReader("usp_File_SearchByMetadata"))
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            DateTime entryDate = new DateTime();
                            files.Add(new Entity.SearchResult()
                            {
                                FileGuid = Guid.Parse(reader["FileGuid"].ToString()),
                                FileCategoryName = reader["FileCategory"].ToString(),
                                FileTypeName = reader["FileType"].ToString(),
                                FileName = reader["FileName"].ToString(),
                                EntryDate = DateTime.TryParse(reader["EntryDate"].ToString(), out entryDate) ? entryDate.ToString("dd/MM/yyyy") : string.Empty,
                                FileExtension = Path.GetExtension(reader["FileName"].ToString()),
                                TotalCount = Convert.ToInt32(reader["TotalRecord"].ToString())
                            });
                        }
                        oDm.Dispose();
                    }
                }
            }
            return files;
        }

        public static List<Entity.SearchResult> File_SearchByPhrase(string phrase, int userId, int pageIndex, int pageSize)
        {
            List<Entity.SearchResult> files = new List<Entity.SearchResult>();
            using (DataManager oDm = new DataManager())
            {
                oDm.Add("p_phrase", MySqlDbType.VarChar, phrase);
                oDm.Add("p_UserId", MySqlDbType.Int32, userId);
                oDm.Add("p_PageIndex", MySqlDbType.Int32, pageIndex);
                oDm.Add("p_PageSize", MySqlDbType.Int32, pageSize);

                oDm.CommandType = CommandType.StoredProcedure;
                using (MySqlDataReader reader = oDm.ExecuteReader("usp_File_SearchByPhrase"))
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            DateTime entryDate = new DateTime();
                            files.Add(new Entity.SearchResult()
                            {
                                FileGuid = Guid.Parse(reader["FileGuid"].ToString()),
                                FileCategoryName = reader["FileCategory"].ToString(),
                                FileTypeName = reader["FileType"].ToString(),
                                FileName = reader["FileName"].ToString(),
                                EntryDate = DateTime.TryParse(reader["EntryDate"].ToString(), out entryDate) ? entryDate.ToString("dd/MM/yyyy") : string.Empty,
                                FileExtension = Path.GetExtension(reader["FileName"].ToString()),
                                TotalCount = Convert.ToInt32(reader["TotalRecord"].ToString())
                            });
                        }
                        oDm.Dispose();
                    }
                }
            }
            return files;
        }

        public static Entity.File File_GetByFileGuid(string fileGuid)
        {
            Entity.File file = new Entity.File();
            using (DataManager oDm = new DataManager())
            {
                oDm.Add("p_FileGuid", MySqlDbType.VarChar, fileGuid);

                oDm.CommandType = CommandType.StoredProcedure;
                using (MySqlDataReader reader = oDm.ExecuteReader("usp_File_GetByFileGuid"))
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            DateTime entryDate = new DateTime();

                            file.FileGuid = Guid.Parse(reader["FileGuid"].ToString());
                            file.FileTypeId = Convert.ToInt32(reader["FileTypeId"].ToString());
                            file.PhysicalFileName = reader["PhysicalFileName"].ToString();
                            file.FileOriginalName = reader["FileOriginalName"].ToString();
                            file.EntryDate = DateTime.TryParse(reader["EntryDate"].ToString(), out entryDate) ? entryDate : DateTime.MinValue;
                            file.FileExtension = Path.GetExtension(reader["FileExtension"].ToString());
                            file.IsFullTextCopied = Convert.ToBoolean(reader["IsFullTextCopied"].ToString());
                            file.IsAttachment = Convert.ToBoolean(reader["IsAttachment"].ToString());
                            //file.MainFileGuid = new Guid(reader["MainFileGuid"].ToString());
                            file.FileStatus = (int)Enum.Parse(typeof(Entity.FileStatus), reader["Status"].ToString());
                            file.CreatedByName = reader["CreatedBy"].ToString();
                            file.CreatedDate = DateTime.TryParse(reader["CreatedDate"].ToString(), out entryDate) ? entryDate : DateTime.MinValue;
                            file.LastModifiedByName = Path.GetExtension(reader["LastModifiedBy"].ToString());
                            file.ModifiedDate = DateTime.TryParse(reader["LastModifiedDate"].ToString(), out entryDate) ? entryDate : DateTime.MinValue;
                        }
                        oDm.Dispose();
                    }
                }
            }
            return file;
        }

        public static int File_StatusChange(Guid fileGuid, int status)
        {
            using (DataManager oDm = new DataManager())
            {
                oDm.Add("p_FileGuid", MySqlDbType.VarChar, fileGuid);
                oDm.Add("p_Status", MySqlDbType.Int32, status);

                oDm.CommandType = CommandType.StoredProcedure;
                int retValue = oDm.ExecuteNonQuery("usp_File_StatusChange");
                oDm.Dispose();
                return retValue;
            }
        }

        public static int File_Delete(Guid fileGuid)
        {
            using (DataManager oDm = new DataManager())
            {
                oDm.Add("p_FileGuid", MySqlDbType.VarChar, fileGuid);

                oDm.CommandType = CommandType.StoredProcedure;
                int retValue = oDm.ExecuteNonQuery("usp_File_Delete");
                oDm.Dispose();
                return retValue;
            }
        }

        public static DataTable MetadataFileMapping_GetByFileGuid(string fileGuid)
        {
            using (DataManager oDm = new DataManager())
            {
                oDm.Add("p_FileGuid", MySqlDbType.VarChar, fileGuid);

                oDm.CommandType = CommandType.StoredProcedure;
                DataTable retValue = oDm.ExecuteDataTable("usp_MetadataFileMapping_GetByFileGuid");
                oDm.Dispose();
                return retValue;
            }
        }

        public static DataTable PdfSeperator_GetAll()
        {
            using (DataManager oDm = new DataManager())
            {
                oDm.CommandType = CommandType.StoredProcedure;
                DataTable retValue = oDm.ExecuteDataTable("usp_PdfSeperator_GetAll");
                oDm.Dispose();
                return retValue;
            }
        }

        public static List<Entity.File> File_GetPendingContentSave()
        {
            List<Entity.File> files = new List<Entity.File>();
            using (DataManager oDm = new DataManager())
            {
                oDm.CommandType = CommandType.StoredProcedure;
                using (MySqlDataReader reader = oDm.ExecuteReader("usp_File_GetPendingContentSave"))
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {

                            DateTime entryDate = new DateTime();
                            files.Add(new Entity.File()
                            {
                                FileGuid = Guid.Parse(reader["FileGuid"].ToString()),
                                FileTypeId = Convert.ToInt32(reader["FileTypeId"].ToString()),
                                PhysicalFileName = reader["PhysicalFileName"].ToString(),
                                FileOriginalName = reader["FileOriginalName"].ToString(),
                                EntryDate = DateTime.TryParse(reader["EntryDate"].ToString(), out entryDate) ? entryDate : DateTime.MinValue,
                                FileExtension = Path.GetExtension(reader["FileExtension"].ToString()),
                                IsFullTextCopied = Convert.ToBoolean(reader["IsFullTextCopied"].ToString()),
                                IsAttachment = Convert.ToBoolean(reader["IsAttachment"].ToString()),
                                FileStatus = (int)Enum.Parse(typeof(Entity.FileStatus), reader["Status"].ToString()),
                                CreatedByName = reader["CreatedBy"].ToString(),
                                CreatedDate = DateTime.TryParse(reader["CreatedDate"].ToString(), out entryDate) ? entryDate : DateTime.MinValue,
                                LastModifiedByName = Path.GetExtension(reader["LastModifiedBy"].ToString()),
                                ModifiedDate = DateTime.TryParse(reader["LastModifiedDate"].ToString(), out entryDate) ? entryDate : DateTime.MinValue,
                            });
                        }
                        oDm.Dispose();
                    }
                }
            }
            return files;
        }
    }
}
