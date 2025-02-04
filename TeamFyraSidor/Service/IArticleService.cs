using Azure;
using Microsoft.AspNetCore.Mvc.Rendering;
using TeamFyraSidor.Data;
using TeamFyraSidor.Models.ViewModels;

namespace TeamFyraSidor.Service
{
    public interface IArticleService
    {
        IQueryable<ArticleCategoryLikedVM> GetArticles();

        IQueryable<Article> GetArticlesByCategoryId(int categoryId);
        IQueryable<Article> GetHomePages();

        Task<Article> AddArticle(Article article);
        Article EditArticle(Article article);
        void DeleteArticle(int id);
        Article FindArticle(int id);

        SelectList GetCategorySelectList();
        List<Category> GetCategories();
        void CreateCategory(Category category);
        Category FindCategory(int id);
        void DeleteCategory(int id);

        IQueryable<ArticleCategoryLikedVM> GetSearchResult(string searchString);
        Task<Response<Azure.Storage.Blobs.Models.BlobContentInfo>> UploadFileToContainer(IFormFile file);

        void AddLikeToArticle(int articleId, string userId);
        void RemoveLikeFromArticle(int articleId, string userId);
        void AddViewToArticle(int articleId);
        Task UpdateArticleAsync(Article article);
        Task UpdateEditorsChoiseAsync(List<string> articles);
    }
}
