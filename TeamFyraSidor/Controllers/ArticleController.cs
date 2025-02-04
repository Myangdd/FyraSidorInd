using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TeamFyraSidor.Data;
using TeamFyraSidor.Models;
using TeamFyraSidor.Models.ViewModels;
using TeamFyraSidor.Service;

namespace TeamFyraSidor.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly UserManager<User> _userManager;
        private readonly ISubscriptionService _subscriptionService;
        private readonly IAIService _aIService;

        public ArticleController(IArticleService articleService, UserManager<User> userManager, ISubscriptionService subscriptionService, IAIService aIService)
        {
            _articleService = articleService;
            _userManager = userManager;
            _subscriptionService = subscriptionService;
            _aIService = aIService;
        }

        [Authorize(Roles = "Admin, Editor")]
        public IActionResult Index()
        {
            var articles = _articleService.GetArticles().OrderByDescending(x=>x.Article.DateStamp);
            return View(articles);
        }

        [Authorize(Roles = "Admin, Editor")]
        public IActionResult Details(int id) 
        {
            ViewBag.Categories = _articleService.GetCategorySelectList();
            var article = _articleService.FindArticle(id);
            if (article == null)
            {
                return NotFound();
            }
            return View(article);
        }

        //below are Article CRUD

        [Authorize(Roles = "Admin, Editor")]
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = _articleService.GetCategorySelectList();
            return View();
        }

        [Authorize(Roles = "Admin, Editor")]
        [HttpPost]
        public async Task<IActionResult> Create(Article article)
        {
            if (ModelState.IsValid && article.File != null)
            {
                var result = await _articleService.AddArticle(article);
                return RedirectToAction("Index");
            }
            return View(article);
        }

        [Authorize(Roles = "Admin, Editor")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Categories = _articleService.GetCategorySelectList();
            var article = _articleService.FindArticle(id);
            if (article == null) 
            {
                return NotFound();
            }
            return View(article);
        }

        [Authorize(Roles = "Admin, Editor")]
        [HttpPost]
        public IActionResult Edit(Article article) 
        {
            ViewBag.Categories = _articleService.GetCategorySelectList();
            var result = _articleService.EditArticle(article);
            return View(article);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id) 
        {
            var article = _articleService.FindArticle(id);
            if (article == null)
            {
                return NotFound();
            }
            return View(article);

        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult DeleteConfirmed(int id) 
        {
            _articleService.DeleteArticle(id);
            return RedirectToAction("Index","Article");
        }









        //below are Catagory CRUD
        [Authorize(Roles = "Admin")]
        public IActionResult CategoryList()
        {
            var categories = _articleService.GetCategories();
            return View(categories);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult DetailsCategory(int id)
        {
            var category = _articleService.FindCategory(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                _articleService.CreateCategory(category);
                return RedirectToAction("CategoryList");
            }
            return View(category);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult EditCategory(int id)
        {
            var category = _articleService.FindCategory(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult EditCategory(Category category)
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteCategory(int id)
        {
            var category = _articleService.FindCategory(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult DeleteCategoryConfirmed(int id)
        {
            _articleService.DeleteCategory(id);
            return RedirectToAction("CategoryList");

        }

        
        [HttpPost]
        public IActionResult AddLikeToArticle(int articleId, string userId)
        {
            // Call service to add a like to database,
            // then return json with the new number of likes.
            _articleService.AddLikeToArticle(articleId, userId);
            var article = _articleService.FindArticle(articleId);
            return Json(article.Likes);
        }

        
        [HttpPost]
        public IActionResult RemoveLikeFromArticle(int articleId, string userId)
        {
            // Call service to remove a like to database,
            // then return json with the new number of likes.
            _articleService.RemoveLikeFromArticle(articleId, userId);
            var article = _articleService.FindArticle(articleId);
            return Json(article.Likes);
        }

        
        [HttpPost]
        public IActionResult AddViewToArticle(int articleId)
        {
            // Call service to add a view to database,
            // then return json with the new number of views.
            _articleService.AddViewToArticle(articleId);
            var articleCount = _articleService.FindArticle(articleId).Views;
            return Json(articleCount);
        }

        
        public async Task<IActionResult> DisplayArticle(int id)
        {
            
            var article = _articleService.GetArticles().Where(x => x.Article.Id == id).FirstOrDefault();
            // If the user is an admin always show full article
            if (User.IsInRole("Admin")) 
            {

                return View(article);
            }

            // Check if User is Authenticated
            if (User.Identity!.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                var subscription = _subscriptionService.GetSubscriptionUser(user!.Id);

                // Check if user has a subscription
                if (subscription != null)
                {
                    // Check if expiration is past date and time of now.
                    if (subscription.Expires < DateTime.Now)
                    {
                        await _subscriptionService.CheckIfExpiredAsync(user);
                    }
                }

                // Show Limited Article if user does not have a subscription.
                if (subscription == null)
                {
                    return View("LimitedArticle", article);
                }
                //SubscriptionType Id 1 == Sports Access            Category Id 7 == World      Category Id 10 == Economy
                //SubscriptionType Id 2 == News Access              Category Id 8 == Sports     Category Id 11 == Local
                //SubscriptionType Id 3 == Full Access              Category Id 9 == Travel     Category Id 12 == Sweden
                else if (subscription.SubscriptionTypeId == 3) // If Subscription Type is Full Access
                {

                    return View(article);
                }
                else if (article!.Category.Id == 8 && subscription.SubscriptionTypeId == 1) // If Category is Sports and Subscription Type is Sports Access
                {

                    return View(article);
                }
                else if (article.Category.Id != 8 && subscription.SubscriptionTypeId == 2) // If Category is NOT Sports and Subscription Type is News Access
                {

                    return View(article);
                }
                else // Else show Limited Article (ex: User has Sports Access and Article has the Travel Category)
                {
                    return View("LimitedArticle", article);
                }

            }
            else // If User is not Authenticated.
            {
                return View("LimitedArticle", article);
            }            
        }

        public IActionResult LimitedArticle(ArticleCategoryLikedVM limitedArticle)
        {
            return View(limitedArticle);
        }


        [Authorize(Roles = "Admin,Editor")]
        public IActionResult EditorsChoise()
        {
            var articles = _articleService.GetArticles().OrderByDescending(a => a.Article.DateStamp).Select(a => a.Article);
            return View(articles.ToList());
        }


        [Authorize(Roles = "Admin,Editor")]
        [HttpPost]
        public async Task<IActionResult> EditorsChoise(List<string> articles)
        {
            if (ModelState.IsValid)
            {
                await _articleService.UpdateEditorsChoiseAsync(articles);
                ViewBag.EditorsChoiseStatus = "Editor's Choise Updated Successfully";
            }
            else
            {
                ViewBag.EditorsChoiseStatus = "Something when wrong when trying to update Editor's Choise.";
            }
            var articleList = _articleService.GetArticles().OrderByDescending(a => a.Article.DateStamp).Select(a => a.Article);
            return View(articleList);
        }

        [HttpPost]
        public async Task<IActionResult> GetSummary(int articleId)
        {
            var article = _articleService.FindArticle(articleId);
            if (article != null)
            {
                var summary = await _aIService.GetSummary(article);
                return Json(summary);
            }
            return RedirectToAction("DisplayArticle");
        }
    }
}
