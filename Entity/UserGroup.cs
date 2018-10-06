using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class UserGroup
    {
        public UserGroup()
        { }

        public int UserGroupId { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }


        public Int64 UserGroupFileMappingId { get; set; }
        public Guid? FileGuid { get; set; }
        public Int64 UserGroupUserMappingId { get; set; }        
        public int UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int Status { get; set; }
    }
}
