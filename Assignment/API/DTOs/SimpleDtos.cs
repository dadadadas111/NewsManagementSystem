namespace API.DTOs
{
    public class CategoryDto
    {
        public short CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? CategoryDescription { get; set; }
    }

    public class TagDto
    {
        public int TagId { get; set; }
        public string? TagName { get; set; }
        public string? Note { get; set; }
    }

    public class SystemAccountDto
    {
        public short AccountId { get; set; }
        public string? AccountName { get; set; }
        public string? AccountEmail { get; set; }
        public int? AccountRole { get; set; }
    }
}
