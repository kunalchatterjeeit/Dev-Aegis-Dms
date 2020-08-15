using System.Collections.Generic;

namespace Entity
{
    public class Search : BaseEntity
    {
        public SearchModeEnum SearchMode { get; set; }
        public string SearchString { get; set; }
        public List<Metadata> Metadatas { get; set; }
    }
}
