namespace Core.Specifications
{
    public class ProductSpecParams
    {
        private const int MaxPageSize = 50;   // Neil's arbitrary max

        private int _pageSize = 6;            // Default page size

        // Paging
        public int PageIndex { get; set; } = 1;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        // Filtering
        public string? Brand { get; set; }
        public string? Type { get; set; }

        // Sorting
        public string? Sort { get; set; }

        // Search
        private string? _search;
        public string Search
        {
            get => _search ?? string.Empty;
            set => _search = value.ToLower();
        }

    }
}
