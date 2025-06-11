using BussinessObject.Models;
using DataAccessLayer;

namespace Repository;

public class CategoryRepository
{
    public List<Category> GetAll() => CategoryDAO.Instance.GetAll();
    public Category? GetById(short id) => CategoryDAO.Instance.GetById(id);
    public void Add(Category category) => CategoryDAO.Instance.Add(category);
    public void Update(Category category) => CategoryDAO.Instance.Update(category);
    public void Delete(short id) => CategoryDAO.Instance.Delete(id);
    public List<Category> Search(string keyword) => CategoryDAO.Instance.Search(keyword);
}
