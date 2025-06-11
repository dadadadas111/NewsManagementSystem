using BussinessObject.Models;
using DataAccessLayer;

namespace Repository;

public class NewsArticleRepository
{
    public List<NewsArticle> GetAll() => NewsArticleDAO.Instance.GetAll();
    public NewsArticle? GetById(string id) => NewsArticleDAO.Instance.GetById(id);
    public void Add(NewsArticle newsArticle) => NewsArticleDAO.Instance.Add(newsArticle);
    public void Update(NewsArticle newsArticle) => NewsArticleDAO.Instance.Update(newsArticle);
    public void Delete(string id) => NewsArticleDAO.Instance.Delete(id);
    public List<NewsArticle> Search(string keyword) => NewsArticleDAO.Instance.Search(keyword);
}
