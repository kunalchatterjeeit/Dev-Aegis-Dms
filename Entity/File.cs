using System;
using System.Collections.Generic;

namespace Entity
{
    public class File
    {
        public File() { }

        public long FileId { get; set; }
        public Guid? FileGuid { get; set; }
        public int FileCategoryId { get; set; }
        public int FileTypeId { get; set; }
        public string PhysicalFileName { get; set; }
        public string FileOriginalName { get; set; }
        public string FileExtension { get; set; }
        public DateTime EntryDate { get; set; }
        public bool IsFullTextCopied { get; set; }
        public bool IsAttachment { get; set; }
        public Guid? MainFileGuid { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string LastModifiedByName { get; set; }
        public int AccessedBy { get; set; }
        public int FileStatus { get; set; }
        public string FileCompletePath { get; set; }
        public decimal SizeInKb { get; set; }
        public List<UserGroup> UserGroups { get; set; }
        public List<MetadataFileMapping> MetadataFileMappings { get; set; }
        public List<Metadata> Metadatas { get; set; }
        public List<int> SelectedUserGroups { get; set; }
        public int VersionNumber { get; set; }
    }

    public enum FileStatus
    {
        Active = 1,
        Inactive = 0,
        Suspended = 2
    }
}
