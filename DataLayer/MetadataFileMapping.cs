using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class MetadataFileMapping
    {
        public MetadataFileMapping() { }

        public static int MetadataFileMapping_Save(Entity.MetadataFileMapping metadataFileMapping)
        {
            using (DataManager oDm = new DataManager())
            {
                oDm.Add("p_MetadataFileMappingId", MySqlDbType.Int64, metadataFileMapping.MetadataFileMappingId);
                oDm.Add("p_MetadataId", MySqlDbType.Int64, metadataFileMapping.MetadataId);
                oDm.Add("p_FileGuid", MySqlDbType.VarChar, metadataFileMapping.FileGuid);
                oDm.Add("p_MetadataContent", MySqlDbType.VarChar, metadataFileMapping.MetadataContent);
                oDm.Add("p_CreatedDate", MySqlDbType.DateTime, metadataFileMapping.CreatedDate);
                oDm.Add("p_CreatedBy", MySqlDbType.Int32, metadataFileMapping.CreatedBy);

                oDm.CommandType = CommandType.StoredProcedure;
                int retValue = oDm.ExecuteNonQuery("usp_MetadataFileMapping_Save");
                oDm.Dispose();
                return retValue;
            }
        }

        public static DataTable MetadataFileMapping_GetAll(Entity.MetadataFileMapping metadataFileMapping)
        {
            using (DataManager oDm = new DataManager())
            {
                oDm.CommandType = CommandType.StoredProcedure;
                DataTable retValue = oDm.ExecuteDataTable("usp_MetadataFileMapping_GetAll");
                oDm.Dispose();
                return retValue;
            }
        }

        public static List<Entity.MetadataSearch> MetadataFileMapping_ByMetadataId(long metadataId)
        {
            List<Entity.MetadataSearch> metadataSearches = new List<Entity.MetadataSearch>();
            using (DataManager oDm = new DataManager())
            {
                oDm.Add("p_MetadataId", MySqlDbType.Int64, metadataId);
                oDm.CommandType = CommandType.StoredProcedure;
                using (MySqlDataReader reader = oDm.ExecuteReader("usp_MetadataFileMapping_ByMetadataId"))
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            metadataSearches.Add(new Entity.MetadataSearch()
                            {
                                MetadataContent = reader["MetaDataContent"].ToString()
                            });
                        }
                        oDm.Dispose();
                    }
                }
            }
            return metadataSearches;
        }
    }
}
