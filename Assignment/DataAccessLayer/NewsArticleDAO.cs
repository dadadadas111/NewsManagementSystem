using BussinessObject.Contexts;
using BussinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer;

public class NewsArticleDAO
{
    private static NewsArticleDAO? instance;
    private static readonly object lockObj = new();

    private NewsArticleDAO() { }

    public static NewsArticleDAO Instance
    {
        get
        {
            lock (lockObj)
            {
                return instance ??= new NewsArticleDAO();
            }
        }
    }

    public List<NewsArticle> GetAll()
    {
        using var context = new FUNewsManagementContext();
        return context.NewsArticles.Include(n => n.Category).Include(n => n.CreatedBy).Include(n => n.Tags).ToList();
    }

    public NewsArticle? GetById(string id)
    {
        using var context = new FUNewsManagementContext();
        return context.NewsArticles.Include(n => n.Category).Include(n => n.CreatedBy).Include(n => n.Tags).FirstOrDefault(n => n.NewsArticleId == id);
    }

    public void Add(NewsArticle newsArticle)
    {
        using var context = new FUNewsManagementContext();
        var tagIds = newsArticle.Tags.Select(t => t.TagId).Distinct().ToList();
        newsArticle.Tags.Clear();
        var tagsFromDb = context.Tags.Where(t => tagIds.Contains(t.TagId)).ToList();
        foreach (var tag in tagsFromDb)
        {
            newsArticle.Tags.Add(tag);
            context.Entry(tag).State = Microsoft.EntityFrameworkCore.EntityState.Unchanged;
        }
        context.NewsArticles.Add(newsArticle);
        context.SaveChanges();
    }

    public void Update(NewsArticle newsArticle)
    {
        using var context = new FUNewsManagementContext();
        var existing = context.NewsArticles.Include(n => n.Tags).FirstOrDefault(n => n.NewsArticleId == newsArticle.NewsArticleId);
        if (existing != null)
        {
            existing.NewsTitle = newsArticle.NewsTitle;
            existing.Headline = newsArticle.Headline;
            existing.NewsContent = newsArticle.NewsContent;
            existing.NewsSource = newsArticle.NewsSource;
            existing.CategoryId = newsArticle.CategoryId;
            existing.NewsStatus = newsArticle.NewsStatus;
            existing.CreatedById = newsArticle.CreatedById;
            // Update tags
            existing.Tags.Clear();
            if (newsArticle.Tags != null && newsArticle.Tags.Count > 0)
            {
                var tagIds = newsArticle.Tags.Select(t => t.TagId).Distinct().ToList();
                var tagsFromDb = context.Tags.Where(t => tagIds.Contains(t.TagId)).ToList();
                foreach (var tag in tagsFromDb)
                {
                    existing.Tags.Add(tag);
                }
            }
            context.SaveChanges();
        }
    }

    public void Delete(string id)
    {
        using var context = new FUNewsManagementContext();
        var newsArticle = context.NewsArticles.Include(n => n.Tags).Include(n => n.Category).FirstOrDefault(n => n.NewsArticleId == id);
        if (newsArticle != null)
        {
            // Remove all tag relationships first
            newsArticle.Tags.Clear();
            newsArticle.Category = null; // Clear the category 
            context.NewsArticles.Remove(newsArticle);
            context.SaveChanges();
        }
    }

    public List<NewsArticle> Search(string keyword)
    {
        using var context = new FUNewsManagementContext();
        return context.NewsArticles.Where(n => n.NewsTitle!.Contains(keyword) || n.Headline.Contains(keyword)).ToList();
    }

    public List<NewsArticle> GetByPeriod(DateTime startDate, DateTime endDate)
    {
        using var context = new FUNewsManagementContext();
        return context.NewsArticles
            .Where(n => n.CreatedDate >= startDate && n.CreatedDate <= endDate)
            .OrderByDescending(n => n.CreatedDate)
            .Include(n => n.Category)
            .Include(n => n.CreatedBy)
            .Include(n => n.Tags)
            .ToList();
    }
}
