using BussinessObject.Models;
using System.Linq;
using API.DTOs;

namespace API.DTOs
{
    public static class NewsArticleMapper
    {
        public static NewsArticleDto ToDto(NewsArticle entity)
        {
            return new NewsArticleDto
            {
                NewsArticleId = entity.NewsArticleId,
                NewsTitle = entity.NewsTitle,
                Headline = entity.Headline,
                NewsContent = entity.NewsContent,
                NewsSource = entity.NewsSource,
                NewsStatus = entity.NewsStatus,
                CreatedDate = entity.CreatedDate,
                ModifiedDate = entity.ModifiedDate,
                Category = entity.Category == null ? null : new CategoryDto
                {
                    CategoryId = entity.Category.CategoryId,
                    CategoryName = entity.Category.CategoryName
                },
                CreatedBy = entity.CreatedBy == null ? null : new SystemAccountDto
                {
                    AccountId = entity.CreatedBy.AccountId,
                    AccountName = entity.CreatedBy.AccountName
                },
                Tags = entity.Tags?.Select(t => new TagDto
                {
                    TagId = t.TagId,
                    TagName = t.TagName
                }).ToList() ?? new List<TagDto>()
            };
        }
    }
}
