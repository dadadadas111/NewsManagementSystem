using BussinessObject.Contexts;
using BussinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer;

public class CategoryDAO
{
    private static CategoryDAO? instance;
    private static readonly object lockObj = new();
    private readonly FUNewsManagementContext context;

    private CategoryDAO()
    {
        context = new FUNewsManagementContext();
    }

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
        return context.Categories.Include(c => c.ParentCategory).Include(c => c.NewsArticles).ToList();
    }

    public Category? GetById(short id)
    {
        return context.Categories.Include(c => c.ParentCategory).Include(c => c.NewsArticles).FirstOrDefault(c => c.CategoryId == id);
    }

    public void Add(Category category)
    {
        context.Categories.Add(category);
        context.SaveChanges();
    }

    public void Update(Category category)
    {
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
        var category = context.Categories.Include(c => c.NewsArticles).FirstOrDefault(c => c.CategoryId == id);
        if (category != null)
        {
            category.NewsArticles.Clear(); // Clear related news articles
            context.Categories.Attach(category);
            context.Categories.Remove(category);
            context.SaveChanges();
        }
    }

    public List<Category> Search(string keyword)
    {
        return context.Categories.Where(c => c.CategoryName.Contains(keyword) || c.CategoryDesciption.Contains(keyword)).ToList();
    }

    public bool IsCategoryInUse(short categoryId)
    {
        return context.NewsArticles.AsNoTracking().Any(a => a.CategoryId == categoryId);
    }
}
