using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class FileType
    {
        public FileType() { }

        public int FileTypeId { get; set; }
        public int FileCategoryId { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
    }
}
