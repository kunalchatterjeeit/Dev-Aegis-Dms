using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class Metadata
    {
        public Metadata()
        { }

        public static int Metadata_Save(Entity.Metadata metadata)
        {
            using (DataManager oDm = new DataManager())
            {
                oDm.Add("p_MetadataId", MySqlDbType.Int64, metadata.MetadataId);
                oDm.Add("p_FileTypeId", MySqlDbType.Int32, metadata.FileTypeId);
                oDm.Add("p_Name", MySqlDbType.VarChar, metadata.Name);
                oDm.Add("p_Note", MySqlDbType.VarChar, metadata.Note);

                oDm.CommandType = CommandType.StoredProcedure;
                return oDm.ExecuteNonQuery("usp_Metadata_Save");
            }
        }

        public static List<Entity.Metadata> Metadata_GetAll(Entity.Metadata metadata)
        {
            List<Entity.Metadata> metadatas = new List<Entity.Metadata>();
            using (DataManager oDm = new DataManager())
            {
                if (metadata.MetadataId == 0)
                    oDm.Add("p_MetaDataId", MySqlDbType.Int64, DBNull.Value);
                else
                    oDm.Add("p_MetaDataId", MySqlDbType.Int64, metadata.MetadataId);

                if (metadata.FileCategoryId == 0)
                    oDm.Add("p_FileCategoryId", MySqlDbType.Int32, DBNull.Value);
                else
                    oDm.Add("p_FileCategoryId", MySqlDbType.Int32, metadata.FileCategoryId);

                if (metadata.FileTypeId == 0)
                    oDm.Add("p_FileTypeId", MySqlDbType.Int32, DBNull.Value);
                else
                    oDm.Add("p_FileTypeId", MySqlDbType.Int32, metadata.FileTypeId);

                if (string.IsNullOrEmpty(metadata.Name))
                    oDm.Add("p_Name", MySqlDbType.VarChar, DBNull.Value);
                else
                    oDm.Add("p_Name", MySqlDbType.VarChar, metadata.Name);

                oDm.CommandType = CommandType.StoredProcedure;
                using (MySqlDataReader reader = oDm.ExecuteReader("usp_Metadata_GetAll"))
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            metadatas.Add(new Entity.Metadata()
                            {
                                MetadataId = Convert.ToInt32(reader["MetaDataId"].ToString()),
                                FileTypeId = Convert.ToInt32(reader["FileTypeId"].ToString()),
                                FileCategoryId = Convert.ToInt32(reader["FileCategoryId"].ToString()),
                                Name = reader["Name"].ToString(),
                                Note = reader["Note"].ToString()
                            });
                        }
                    }
                }
                return metadatas;
            }
        }

        public static int Metadata_Delete(Int64 metadataId)
        {
            using (DataManager oDm = new DataManager())
            {
                oDm.Add("p_MetadataId", MySqlDbType.Int64, metadataId);

                oDm.CommandType = CommandType.StoredProcedure;
                return oDm.ExecuteNonQuery("usp_Metadata_Delete");
            }
        }
    }
}
