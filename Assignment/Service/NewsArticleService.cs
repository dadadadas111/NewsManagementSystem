using BussinessObject.Models;
using Repository;

namespace Service;

public class NewsArticleService
{
    private readonly NewsArticleRepository _repo = new();

    public List<NewsArticle> GetAll() => _repo.GetAll();
    public NewsArticle? GetById(string id) => _repo.GetById(id);
    public void Add(NewsArticle newsArticle) => _repo.Add(newsArticle);
    public void Update(NewsArticle newsArticle) => _repo.Update(newsArticle);
    public void Delete(string id) => _repo.Delete(id);
    public List<NewsArticle> Search(string keyword) => _repo.Search(keyword);
}
