using BussinessObject.Contexts;
using BussinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer;

public class TagDAO
{
    private static TagDAO? instance;
    private static readonly object lockObj = new();
    private readonly FUNewsManagementContext context;

    private TagDAO()
    {
        context = new FUNewsManagementContext();
    }

    public static TagDAO Instance
    {
        get
        {
            lock (lockObj)
            {
                return instance ??= new TagDAO();
            }
        }
    }

    public List<Tag> GetAll()
    {
        return context.Tags.Include(t => t.NewsArticles).ToList();
    }

    public Tag? GetById(int id)
    {
        return context.Tags.Include(t => t.NewsArticles).FirstOrDefault(t => t.TagId == id);
    }

    public void Add(Tag tag)
    {
        context.Tags.Add(tag);
        context.SaveChanges();
    }

    public void Update(Tag tag)
    {
        var local = context.Tags.Local.FirstOrDefault(t => t.TagId == tag.TagId);
        if (local != null)
        {
            context.Entry(local).State = EntityState.Detached;
        }
        context.Tags.Update(tag);
        context.SaveChanges();
    }

    public void Delete(int id)
    {
        var tag = context.Tags.Find(id);
        if (tag != null)
        {
            context.Tags.Remove(tag);
            context.SaveChanges();
        }
    }

    public List<Tag> Search(string keyword)
    {
        return context.Tags.Where(t => t.TagName!.Contains(keyword) || t.Note!.Contains(keyword)).ToList();
    }
}
