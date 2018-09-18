using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class Metadata
    {
        public Metadata() { }

        public Int64 MetadataId { get; set; }
        public string Name { get; set; }
        public int FileCategoryId { get; set; }
        public int FileTypeId { get; set; }
        public string Note { get; set; }
    }
}
