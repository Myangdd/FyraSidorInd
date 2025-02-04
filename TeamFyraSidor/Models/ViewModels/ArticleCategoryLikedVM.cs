using TeamFyraSidor.Data;

namespace TeamFyraSidor.Models.ViewModels
{
    public class ArticleCategoryLikedVM
    {
        public Article Article { get; set; } = new Article();
        public Category Category { get; set; } = new Category();
        public List<UserLikedArticle> UserLikedArticles { get; set; } = new List<UserLikedArticle>();
    }
}
