namespace honaapi.DTOs.product
{
    public class SearchProductDTO
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public int PageNumber { get; set; } = 0;
        public int PageSize { get; set; } = 0;
        public string? KeyWord { get; set; }
        public string? OrderBy { get; set; } = "Name";
        public string? OrderByDirection { get; set; } = "ASC";
    }
}
