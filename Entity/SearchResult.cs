using System;

namespace Entity
{
    public class SearchResult: BaseEntity
    {
        public Guid FileGuid { get; set; }
        public string FileName { get; set; }
        public string EntryDate { get; set; }
        public string FileCategoryName { get; set; }
        public string FileTypeName { get; set; }
        public string FileExtension { get; set; }
    }
}
