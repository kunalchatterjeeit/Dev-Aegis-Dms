using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class FileContent
    {
        public FileContent() { }
        public Guid? FileGuid { get; set; }
        public string Content { get; set; }
    }
}
