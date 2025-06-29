using BussinessObject.Contexts;
using BussinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer;

public class SystemAccountDAO
{
    private static SystemAccountDAO? instance;
    private static readonly object lockObj = new();

    private SystemAccountDAO() { }

    public static SystemAccountDAO Instance
    {
        get
        {
            lock (lockObj)
            {
                return instance ??= new SystemAccountDAO();
            }
        }
    }

    public List<SystemAccount> GetAll()
    {
        using var context = new FUNewsManagementContext();
        return context.SystemAccounts.Include(a => a.NewsArticles).ToList();
    }

    public SystemAccount? GetById(short id)
    {
        using var context = new FUNewsManagementContext();
        return context.SystemAccounts.Include(a => a.NewsArticles).FirstOrDefault(a => a.AccountId == id);
    }

    public void Add(SystemAccount account)
    {
        using var context = new FUNewsManagementContext();
        context.SystemAccounts.Add(account);
        context.SaveChanges();
    }

    public void Update(SystemAccount account)
    {
        using var context = new FUNewsManagementContext();
        var local = context.SystemAccounts.Local.FirstOrDefault(a => a.AccountId == account.AccountId);
        if (local != null)
        {
            context.Entry(local).State = EntityState.Detached;
        }
        context.SystemAccounts.Update(account);
        context.SaveChanges();
    }

    public void Delete(short id)
    {
        using var context = new FUNewsManagementContext();
        var account = context.SystemAccounts.Find(id);
        if (account != null)
        {
            context.SystemAccounts.Remove(account);
            context.SaveChanges();
        }
    }

    public List<SystemAccount> Search(string keyword)
    {
        using var context = new FUNewsManagementContext();
        return context.SystemAccounts.Where(a => a.AccountName!.Contains(keyword) || a.AccountEmail!.Contains(keyword)).ToList();
    }
}
