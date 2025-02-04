namespace TeamFyraSidor.Data
{
    public class UserLikedArticle
    {
        public int Id { get; set; }
        public User User { get; set; } = new User();
        public Article Article { get; set; } = new Article();
        public string UserId { get; set; } = string.Empty;
        public int ArticleId { get; set; }

    }
}
