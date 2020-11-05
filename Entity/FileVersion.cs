using System;

namespace Entity
{
    public class FileVersion : BaseEntity
    {
        public long FileVersionId { get; set; }
        public long FileId { get; set; }
        public Guid? FileGuid { get; set; }
        public string PhysicalFileName { get; set; }
        public string FileOriginalName { get; set; }
        public string FileExtension { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string LastModifiedByName { get; set; }
        public int VersionNumber { get; set; }
    }
}
