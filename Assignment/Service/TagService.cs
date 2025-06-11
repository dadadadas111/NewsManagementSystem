using BussinessObject.Models;
using Repository;

namespace Service;

public class TagService
{
    private readonly TagRepository _repo = new();

    public List<Tag> GetAll() => _repo.GetAll();
    public Tag? GetById(int id) => _repo.GetById(id);
    public void Add(Tag tag) => _repo.Add(tag);
    public void Update(Tag tag) => _repo.Update(tag);
    public void Delete(int id) => _repo.Delete(id);
    public List<Tag> Search(string keyword) => _repo.Search(keyword);
}
