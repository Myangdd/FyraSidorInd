using Azure.Storage.Blobs;
using Azure;
using Microsoft.EntityFrameworkCore;
using TeamFyraSidor.Data;
using TeamFyraSidor.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Internal;

namespace TeamFyraSidor.Service
{
    public class ArticleService : IArticleService
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;

        public ArticleService(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            _db = dbContext;
            _configuration = configuration;
        }

        public IQueryable<ArticleCategoryLikedVM> GetArticles()
        {
            var articles = _db.Articles.Join(_db.Categories,
                                            article => article.CategoryId,
                                            category => category.Id,
                                            (article, category) => new ArticleCategoryLikedVM
                                            {
                                                Category = category,
                                                Article = article,
                                            })
                                       .GroupJoin(_db.UserLikedArticles,
                                            article => article.Article.Id,
                                            liked => liked.Article.Id,
                                            (article, liked) => new ArticleCategoryLikedVM
                                            {
                                                Category = article.Category,
                                                Article = article.Article,
                                                UserLikedArticles = liked.ToList()
                                            });
            return articles;
        }

        public IQueryable<Article> GetHomePages()
        {
            var articles = _db.Articles.OrderByDescending(article => article.Id);
            return articles;
        }
        //World 7
        //Sports 8
        //Travel 9
        //Economy 10
        //Local 11
        //Sweden 12
        public IQueryable<Article> GetArticlesByCategoryId(int categoryId)
        {
            var articles = _db.Articles.Include(a => a.Category).Where(a => a.CategoryId == categoryId);

            return articles;
        }

        public async Task<Article> AddArticle(Article article)
        {
            if (article != null && article.File != null)
            {
                // If you get a result back, it was successfull
                var result = await UploadFileToContainer(article.File);
                if (result != null)
                {
                    // Set ImageLink
                    article.ImageLink = $"https://fyrasidorstorage.blob.core.windows.net/imagecontainer/{article.File.FileName}";
                    article.LinkText = article.Headline;
                    _db.Articles.Add(article);
                    _db.SaveChanges();
                }
            }

            return article!;
        }

        public Article EditArticle(Article article)
        {
            article.UpdateStamp = DateTime.Now;
            _db.Articles.Update(article);
            _db.SaveChanges();
            return article;
        }

        public void DeleteArticle(int id)
        {
            var article = _db.Articles.Find(id);
            if (article != null)
            {
                _db.Articles.Remove(article);
                _db.SaveChanges();
            }
        }

        public async Task<Response<Azure.Storage.Blobs.Models.BlobContentInfo>> UploadFileToContainer(IFormFile file)
        {
            
            string connectionString = _configuration["AzureBlobConnectionString"]!;
            string containerName = _configuration["AzureBlobContainerName"]!;

            // Create Blob Services and Clients
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = containerClient.GetBlobClient(file.FileName);

            // Upload the image to the container
            using (var stream = file.OpenReadStream())
            {
                var result = await blobClient.UploadAsync(stream, true);
                return result;
            }


        }

        public IQueryable<ArticleCategoryLikedVM> GetSearchResult(string searchString)
        {
            var articles = GetArticles(); // Get list of articles

            // Check so Search Input box was not empty
            if (!String.IsNullOrEmpty(searchString))
            {
                // Get all articles where search string is found in headline or content summary.
                // Convert all strings to upper to eliminate capitalized differences.
                // Order by date.
                articles = articles.Where(a => a.Article.ContentSummary.ToUpper().Contains(searchString.ToUpper()) ||
                                               a.Article.Headline.ToUpper().Contains(searchString.ToUpper()))
                                   .OrderByDescending(d => d.Article.DateStamp);
            }
            return articles;
        }

        public SelectList GetCategorySelectList()
        {
            SelectList categoryList = new SelectList(_db.Categories.ToList(), "Id", "Name");
            return categoryList;
        }

        public Article FindArticle(int id)
        {
            var article = _db.Articles.Find(id);

            return article!;
        }


        public List<Category> GetCategories()
        {
            var categories = _db.Categories.ToList();
            return categories;
        }

        public void CreateCategory(Category category)
        {
            _db.Categories.Add(category);
            _db.SaveChanges();
        }

        public Category FindCategory(int id)
        {
            var category = _db.Categories.Find(id);
            return category!;
        }

        public void DeleteCategory(int id)
        {
            var category = FindCategory(id);
            if (category != null)
            {
                _db.Categories.Remove(category);
                _db.SaveChanges();
            }
        }

        public void AddLikeToArticle(int articleId, string userId)
        {
            var article = FindArticle(articleId);
            // check so userId exist
            if (_db.Users.Find(userId) != null)
            {
                var user = _db.Users.Find(userId);

                // Check if user already liked the article. Continue if not.
                if (_db.UserLikedArticles.Where(x => x.User.Id == userId && x.Article.Id == articleId).FirstOrDefault() == null)
                {
                    // Add user and article to the table
                    var userLikedArticle = new UserLikedArticle { UserId = user!.Id, Article = article, User = user! };
                    _db.UserLikedArticles.Add(userLikedArticle);
                    _db.SaveChanges();

                    // Update the Likes count according to the UserLikedArticles Table.
                    article.Likes = _db.UserLikedArticles.Where(x => x.Article.Id == articleId).Count();
                    _db.Articles.Update(article);
                    _db.SaveChanges();
                }
            }
        }

        public void RemoveLikeFromArticle(int articleId, string userId)
        {
            var article = FindArticle(articleId);
            var user = _db.Users.Find(userId);
            // Check if user already liked the article. Continue if so.
            if (_db.UserLikedArticles.Where(x => x.User.Id == userId && x.Article.Id == articleId) != null)
            {
                // Get the row in the table
                var userLikedArticle = _db.UserLikedArticles.Where(x => x.User.Id == userId && x.Article.Id == articleId).FirstOrDefault();
                if (userLikedArticle != null)
                {
                    _db.UserLikedArticles.Remove(userLikedArticle);
                }
                _db.SaveChanges();
                article.Likes = _db.UserLikedArticles.Where(x => x.Article.Id == articleId).Count();
                _db.Articles.Update(article);
                _db.SaveChanges();
            }
        }

        public void AddViewToArticle(int articleId)
        {
            var article = FindArticle(articleId);
            article.Views++;
            _db.Articles.Update(article);
            _db.SaveChanges();
        }

        public async Task UpdateArticleAsync(Article article)
        {
            _db.Articles.Update(article);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateEditorsChoiseAsync(List<string> articles)
        {
            var allArticles = GetArticles().Select(a => a.Article).ToList();
            foreach (var art in allArticles)
            {
                if (art.EditorsChoise == true)
                {
                    art.EditorsChoise = false;
                    await UpdateArticleAsync(art);
                }
            }
            foreach (var articleString in articles)
            {

                var article = FindArticle(Convert.ToInt32(articleString));
                article.EditorsChoise = true;
                await UpdateArticleAsync(article);
            }
        }
    }
}
