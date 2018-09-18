using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class FileMetadata : Disposable
    {
        public FileMetadata()
        { }

        public static int MetadataSave(Entity.Metadata metadata)
        {
            return DataLayer.Metadata.Metadata_Save(metadata);
        }

        public static DataTable MetadataGetAll(Entity.Metadata metadata)
        {
            return DataLayer.Metadata.Metadata_GetAll(metadata);
        }

        public static int MetadataDelete(Int64 metadataId)
        {
            return DataLayer.Metadata.Metadata_Delete(metadataId);
        }

        ~FileMetadata()
        {
            Dispose(false);
        }
    }
}
