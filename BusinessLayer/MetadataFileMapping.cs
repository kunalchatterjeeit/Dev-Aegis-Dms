using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class MetadataFileMapping:Disposable
    {
        public MetadataFileMapping() { }

        public static int MetadataFileMappingSave(Entity.MetadataFileMapping metadataFileMapping)
        {
            return DataLayer.MetadataFileMapping.MetadataFileMapping_Save(metadataFileMapping);
        }

        public static DataTable MetadataFileMappingGetAll(Entity.MetadataFileMapping metadataFileMapping)
        {
            return DataLayer.MetadataFileMapping.MetadataFileMapping_GetAll(metadataFileMapping);
        }

        public static DataTable MetadataFileMappingByMetadataId(Int64 metadataId)
        {
            return DataLayer.MetadataFileMapping.MetadataFileMapping_ByMetadataId(metadataId);
        }

        ~MetadataFileMapping()
        {
            Dispose(false);
        }
    }
}
