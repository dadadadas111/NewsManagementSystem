using BussinessObject.Models;
using Repository;

namespace Service;

public class SystemAccountService
{
    private readonly SystemAccountRepository _repo = new();

    public List<SystemAccount> GetAll() => _repo.GetAll();
    public SystemAccount? GetById(short id) => _repo.GetById(id);
    public void Add(SystemAccount account) => _repo.Add(account);
    public void Update(SystemAccount account) => _repo.Update(account);
    public void Delete(short id) => _repo.Delete(id);
    public List<SystemAccount> Search(string keyword) => _repo.Search(keyword);
}
