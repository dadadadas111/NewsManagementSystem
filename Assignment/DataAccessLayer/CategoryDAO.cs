using BussinessObject.Contexts;
using BussinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer;

public class CategoryDAO
{
    private static CategoryDAO? instance;
    private static readonly object lockObj = new();

    private CategoryDAO() { }

    public static CategoryDAO Instance
    {
        get
        {
            lock (lockObj)
            {
                return instance ??= new CategoryDAO();
            }
        }
    }

    public List<Category> GetAll()
    {
        using var context = new FUNewsManagementContext();
        return context.Categories.Include(c => c.ParentCategory).Include(c => c.NewsArticles).ToList();
    }

    public Category? GetById(short id)
    {
        using var context = new FUNewsManagementContext();
        return context.Categories.Include(c => c.ParentCategory).Include(c => c.NewsArticles).FirstOrDefault(c => c.CategoryId == id);
    }

    public void Add(Category category)
    {
        using var context = new FUNewsManagementContext();
        context.Categories.Add(category);
        context.SaveChanges();
    }

    public void Update(Category category)
    {
        using var context = new FUNewsManagementContext();
        var existing = context.Categories.Find(category.CategoryId);
        if (existing != null)
        {
            existing.CategoryName = category.CategoryName;
            existing.CategoryDesciption = category.CategoryDesciption;
            // Update other properties as needed
            context.SaveChanges();
        }
    }

    public void Delete(short id)
    {
        using var context = new FUNewsManagementContext();
        var category = context.Categories.Include(c => c.NewsArticles).FirstOrDefault(c => c.CategoryId == id);
        if (category != null)
        {
            context.Categories.Remove(category);
            context.SaveChanges();
        }
    }

    public List<Category> Search(string keyword)
    {
        using var context = new FUNewsManagementContext();
        return context.Categories.Where(c => c.CategoryName.Contains(keyword) || c.CategoryDesciption.Contains(keyword)).ToList();
    }

    public bool IsCategoryInUse(short categoryId)
    {
        using var context = new FUNewsManagementContext();
        return context.NewsArticles.AsNoTracking().Any(a => a.CategoryId == categoryId);
    }
}
