using TeamFyraSidor.Data;

namespace TeamFyraSidor.Models.ViewModels
{
    public class HomePageVM
    {
        public Article Article { get; set; } = new Article();
        public List<Article> Latest { get; set; } = new List<Article>();
        public List<Article> MostPopular { get; set; } = new List<Article>();
        public List<Article> EditorChoice { get; set; } = new List<Article>();

        //public Category? Category { get; set; }

    }
}
