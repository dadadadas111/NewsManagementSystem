using BussinessObject.Contexts;
using BussinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer;

public class NewsArticleDAO
{
    private static NewsArticleDAO? instance;
    private static readonly object lockObj = new();
    private readonly FUNewsManagementContext context;

    private NewsArticleDAO()
    {
        context = new FUNewsManagementContext();
    }

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
        return context.NewsArticles.Include(n => n.Category).Include(n => n.CreatedBy).Include(n => n.Tags).ToList();
    }

    public NewsArticle? GetById(string id)
    {
        return context.NewsArticles.Include(n => n.Category).Include(n => n.CreatedBy).Include(n => n.Tags).FirstOrDefault(n => n.NewsArticleId == id);
    }

    public void Add(NewsArticle newsArticle)
    {
        // Fix: Attach tags from the current context to avoid tracking conflicts
        var tagIds = newsArticle.Tags.Select(t => t.TagId).Distinct().ToList();
        newsArticle.Tags.Clear();
        var tagsFromDb = context.Tags.Where(t => tagIds.Contains(t.TagId)).ToList();
        foreach (var tag in tagsFromDb)
        {
            newsArticle.Tags.Add(tag);
        }
        context.NewsArticles.Add(newsArticle);
        context.SaveChanges();
    }

    public void Update(NewsArticle newsArticle)
    {
        context.NewsArticles.Update(newsArticle);
        context.SaveChanges();
    }

    public void Delete(string id)
    {
        var newsArticle = context.NewsArticles.Find(id);
        if (newsArticle != null)
        {
            context.NewsArticles.Remove(newsArticle);
            context.SaveChanges();
        }
    }

    public List<NewsArticle> Search(string keyword)
    {
        return context.NewsArticles.Where(n => n.NewsTitle!.Contains(keyword) || n.Headline.Contains(keyword)).ToList();
    }
}
