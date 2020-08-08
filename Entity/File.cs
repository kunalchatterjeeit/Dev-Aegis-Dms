using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class File
    {
        public File() { }

        public Guid? FileGuid { get; set; }
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
        public DateTime ModifiedDate { get; set; }
        public string LastModifiedByName { get; set; }
        public int AccessedBy { get; set; }
        public int FileStatus { get; set; }
        public string FileCompletePath { get; set; }
        public decimal SizeInKb { get; set; }
    }

    public enum FileStatus
    {
        Active = 1,
        Inactive = 0,
        Suspended = 2
    }
}
