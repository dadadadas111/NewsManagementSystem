namespace API.DTOs
{
    public class CategoryDto
    {
        public short CategoryId { get; set; }
        public string? CategoryName { get; set; }
    }

    public class TagDto
    {
        public int TagId { get; set; }
        public string? TagName { get; set; }
    }

    public class SystemAccountDto
    {
        public short AccountId { get; set; }
        public string? AccountName { get; set; }
    }
}
