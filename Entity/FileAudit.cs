using System;

namespace Entity
{
    public class FileAudit
    {
        public long FileAuditId { get; set; }
        public string Log { get; set; }
        public DateTime CreatedDate { get; set; }
        public long FileId { get; set; }
        public int UserId { get; set; }
        public Guid? FileGuid { get; set; }
    }
}
