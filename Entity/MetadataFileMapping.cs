using System;

namespace Entity
{
    public class MetadataFileMapping
    {
        public MetadataFileMapping() { }

        public Int64 MetadataFileMappingId { get; set; }
        public Int64 MetadataId { get; set; }
        public Guid? FileGuid { get; set; }
        public string MetadataContent { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public string Name { get; set; }
    }
}
