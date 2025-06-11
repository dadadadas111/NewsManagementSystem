using BussinessObject.Models;
using DataAccessLayer;

namespace Repository;

public class TagRepository
{
    public List<Tag> GetAll() => TagDAO.Instance.GetAll();
    public Tag? GetById(int id) => TagDAO.Instance.GetById(id);
    public void Add(Tag tag) => TagDAO.Instance.Add(tag);
    public void Update(Tag tag) => TagDAO.Instance.Update(tag);
    public void Delete(int id) => TagDAO.Instance.Delete(id);
    public List<Tag> Search(string keyword) => TagDAO.Instance.Search(keyword);
}
