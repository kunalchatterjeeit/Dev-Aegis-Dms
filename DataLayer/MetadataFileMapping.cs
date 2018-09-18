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
                return oDm.ExecuteNonQuery("usp_MetadataFileMapping_Save");
            }
        }

        public static DataTable MetadataFileMapping_GetAll(Entity.MetadataFileMapping metadataFileMapping)
        {
            using (DataManager oDm = new DataManager())
            {
                oDm.CommandType = CommandType.StoredProcedure;
                return oDm.ExecuteDataTable("usp_MetadataFileMapping_GetAll");
            }
        }

        public static DataTable MetadataFileMapping_ByMetadataId(Int64 metadataId)
        {
            using (DataManager oDm = new DataManager())
            {
                oDm.Add("p_MetadataId", MySqlDbType.Int64, metadataId);
                oDm.CommandType = CommandType.StoredProcedure;
                return oDm.ExecuteDataTable("usp_MetadataFileMapping_ByMetadataId");
            }
        }
    }
}
