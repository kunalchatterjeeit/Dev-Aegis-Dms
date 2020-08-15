namespace Entity
{
    public class BaseEntity: HttpResponse
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    }
}
