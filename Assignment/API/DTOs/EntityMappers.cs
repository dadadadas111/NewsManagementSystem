using BussinessObject.Models;
using System.Linq;

namespace API.DTOs
{
    public static class CategoryMapper
    {
        public static CategoryDto ToDto(Category entity)
        {
            return new CategoryDto
            {
                CategoryId = entity.CategoryId,
                CategoryName = entity.CategoryName,
                CategoryDescription = entity.CategoryDesciption
            };
        }
    }

    public static class TagMapper
    {
        public static TagDto ToDto(Tag entity)
        {
            return new TagDto
            {
                TagId = entity.TagId,
                TagName = entity.TagName,
                Note = entity.Note,
            };
        }
    }

    public static class SystemAccountMapper
    {
        public static SystemAccountDto ToDto(SystemAccount entity)
        {
            return new SystemAccountDto
            {
                AccountId = entity.AccountId,
                AccountName = entity.AccountName,
                AccountEmail = entity.AccountEmail,
                AccountRole = entity.AccountRole,
            };
        }
    }
}
