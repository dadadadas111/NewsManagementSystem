using BussinessObject.Contexts;
using BussinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer;

public class TagDAO
{
    private static TagDAO? instance;
    private static readonly object lockObj = new();

    private TagDAO() { }

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
        using var context = new FUNewsManagementContext();
        return context.Tags.Include(t => t.NewsArticles).ToList();
    }

    public Tag? GetById(int id)
    {
        using var context = new FUNewsManagementContext();
        return context.Tags.Include(t => t.NewsArticles).FirstOrDefault(t => t.TagId == id);
    }

    public void Add(Tag tag)
    {
        using var context = new FUNewsManagementContext();
        context.Tags.Add(tag);
        context.SaveChanges();
    }

    public void Update(Tag tag)
    {
        using var context = new FUNewsManagementContext();
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
        using var context = new FUNewsManagementContext();
        var tag = context.Tags.Include(c => c.NewsArticles).FirstOrDefault(t => t.TagId == id);
        if (tag == null)
        {
            // Tag does not exist, nothing to delete
            return;
        }
        if (tag.NewsArticles != null && tag.NewsArticles.Count > 0)
        {
            throw new InvalidOperationException("Cannot delete tag because it is still used by one or more news articles.");
        }
        try
        {
            // Attach if not tracked
            if (context.Entry(tag).State == EntityState.Detached)
            {
                context.Tags.Attach(tag);
            }
            context.Tags.Remove(tag);
            context.SaveChanges();
        }
        catch (DbUpdateConcurrencyException)
        {
            // The tag was already deleted or modified by another process
            // Optionally log or handle as needed
        }
        catch (InvalidOperationException)
        {
            // Handle any tracking issues
        }
    }

    public List<Tag> Search(string keyword)
    {
        using var context = new FUNewsManagementContext();
        return context.Tags.Where(t => t.TagName!.Contains(keyword) || t.Note!.Contains(keyword)).ToList();
    }
}
