using BussinessObject.Models;
using DataAccessLayer;

namespace Repository;

public class SystemAccountRepository
{
    public List<SystemAccount> GetAll() => SystemAccountDAO.Instance.GetAll();
    public SystemAccount? GetById(short id) => SystemAccountDAO.Instance.GetById(id);
    public void Add(SystemAccount account) => SystemAccountDAO.Instance.Add(account);
    public void Update(SystemAccount account) => SystemAccountDAO.Instance.Update(account);
    public void Delete(short id) => SystemAccountDAO.Instance.Delete(id);
    public List<SystemAccount> Search(string keyword) => SystemAccountDAO.Instance.Search(keyword);
}
