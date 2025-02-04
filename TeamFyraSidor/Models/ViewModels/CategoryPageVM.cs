using TeamFyraSidor.Data;

namespace TeamFyraSidor.Models.ViewModels
{
    public class CategoryPageVM
    {
        public Article TopArticle { get; set; } = new Article();
        public List<Article> Pop3Articles { get; set; } = new List<Article>();
        public List<Article> LatestArticles { get; set; } = new List<Article>();

    }
}
