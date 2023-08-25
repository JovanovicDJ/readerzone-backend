namespace readerzone_api.Dtos
{
    public class PaginationQuery
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchKeyword { get; set; } = string.Empty;
        public List<string> SelectedGenres { get; set; } = new List<string>();
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }

    }
}
