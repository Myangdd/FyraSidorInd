using Microsoft.AspNetCore.Mvc;
using TeamFyraSidor.Data;
using TeamFyraSidor.Service;

namespace TeamFyraSidor.ViewComponents
{
    public class LatestNewsView : ViewComponent
    {
        private readonly IArticleService _articleService;

        public LatestNewsView(IArticleService articleService)
        {
            _articleService = articleService;
        }

        public IViewComponentResult Invoke()
        {
            var latestNews = _articleService.GetArticles();
            
            return View("Default",latestNews);
        }
    }
}
