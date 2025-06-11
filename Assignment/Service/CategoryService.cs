using BussinessObject.Models;
using Repository;

namespace Service;

public class CategoryService
{
    private readonly CategoryRepository _repo = new();

    public List<Category> GetAll() => _repo.GetAll();
    public Category? GetById(short id) => _repo.GetById(id);
    public void Add(Category category) => _repo.Add(category);
    public void Update(Category category) => _repo.Update(category);
    public void Delete(short id) => _repo.Delete(id);
    public List<Category> Search(string keyword) => _repo.Search(keyword);
}
