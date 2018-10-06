using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Entity
{
    public class SeperatedFile
    {
        public string FileName { get; set; }
        public int FileCategoryId { get; set; }
        public int FileTypeId { get; set; }
        public int FirstPageIndex { get; set; }
    }
}
