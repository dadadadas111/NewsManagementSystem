using System;
using System.Collections.Generic;

namespace API.DTOs
{
    public class NewsArticleDto
    {
        public string NewsArticleId { get; set; }
        public string NewsTitle { get; set; }
        public string Headline { get; set; }
        public string? NewsContent { get; set; }
        public string? NewsSource { get; set; }
        public bool? NewsStatus { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public CategoryDto? Category { get; set; }
        public SystemAccountDto? CreatedBy { get; set; }
        public List<TagDto> Tags { get; set; } = new List<TagDto>();
    }
}
