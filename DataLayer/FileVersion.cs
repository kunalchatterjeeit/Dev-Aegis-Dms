using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace DataLayer
{
    public class FileVersion
    {
        public FileVersion()
        {

        }

        public static Guid? FileVersion_Save(Entity.FileVersion fileVersion)
        {
            Guid? fileGuid = Guid.NewGuid();
            using (DataManager oDm = new DataManager())
            {
                oDm.Add("p_FileVersionId", MySqlDbType.VarChar, fileVersion.FileVersionId);
                oDm.Add("p_FileId", MySqlDbType.VarChar, fileVersion.FileId);
                oDm.Add("p_FileGuid", MySqlDbType.VarChar, fileGuid);
                oDm.Add("p_PhysicalFileName", MySqlDbType.VarChar, fileGuid);
                oDm.Add("p_OriginalFileName", MySqlDbType.VarChar, fileVersion.FileOriginalName);
                oDm.Add("p_FileExtension", MySqlDbType.VarChar, fileVersion.FileExtension);
                oDm.Add("p_Description", MySqlDbType.VarChar, fileVersion.Description); 
                oDm.Add("p_CreatedBy", MySqlDbType.Int32, fileVersion.CreatedBy);
                oDm.Add("p_CreatedOn", MySqlDbType.DateTime, fileVersion.CreatedDate);
                oDm.Add("p_VersionNumber", MySqlDbType.Int32, fileVersion.VersionNumber);

                oDm.CommandType = CommandType.StoredProcedure;
                int retVal = oDm.ExecuteNonQuery("usp_FileVersion_Save");
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

        public static List<Entity.FileVersion> FileVersion_GetByFileGuid(long fileId, int pageIndex, int pageSize)
        {
            List<Entity.FileVersion> files = new List<Entity.FileVersion>();
            using (DataManager oDm = new DataManager())
            {
                oDm.Add("p_FileId", MySqlDbType.VarChar, fileId);
                oDm.Add("p_PageIndex", MySqlDbType.Int32, pageIndex);
                oDm.Add("p_PageSize", MySqlDbType.Int32, pageSize);

                oDm.CommandType = CommandType.StoredProcedure;
                using (MySqlDataReader reader = oDm.ExecuteReader("usp_FileVersion_GetByFileGuid"))
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            DateTime createdDate = new DateTime();
                            files.Add(new Entity.FileVersion()
                            {
                                FileVersionId = Convert.ToInt64(reader["FileVersionId"].ToString()),
                                FileId = Convert.ToInt64(reader["FileId"].ToString()),
                                FileGuid = Guid.Parse(reader["FileGuid"].ToString()),
                                PhysicalFileName = reader["PhysicalFileName"].ToString(),
                                FileOriginalName = reader["OriginalFileName"].ToString(),
                                FileExtension = Path.GetExtension(reader["OriginalFileName"].ToString()),
                                VersionNumber = Convert.ToInt32(reader["VersionNumber"].ToString()),
                                Description = reader["Description"].ToString(),
                                CreatedDate = DateTime.TryParse(reader["CreatedOn"].ToString(), out createdDate) ? createdDate : DateTime.MinValue,
                                TotalCount = Convert.ToInt32(reader["TotalRecord"].ToString())
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
