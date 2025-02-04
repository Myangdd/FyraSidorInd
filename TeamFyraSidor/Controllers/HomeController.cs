using Azure;
using Azure.AI.TextAnalytics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Linq;
using TeamFyraSidor.Data;
using TeamFyraSidor.Models;
using TeamFyraSidor.Models.ViewModels;
using TeamFyraSidor.Service;

namespace TeamFyraSidor.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IArticleService _articleService;
        private readonly IElpriceService _elpriceService;
        private readonly string _endPoint;
        private readonly string _apiKey;

        public HomeController(ILogger<HomeController> logger, IArticleService articleService,  
            IElpriceService elpriceService,IConfiguration configuration)
        {
            _logger = logger;
            _articleService = articleService;
            _elpriceService = elpriceService;
            _endPoint = configuration["LanguageAI:EndPoint"];
            _apiKey = configuration["LanguageAI:ApiKey"];
        }
        
        public IActionResult Index()
        {
            var articles = _articleService.GetHomePages();

            var model = new HomePageVM();

            model.MostPopular = articles.OrderByDescending(a => a.Views).ToList();

            model.Latest = articles.OrderByDescending(x => x.DateStamp).ToList();

            model.EditorChoice = articles.OrderByDescending(a => a.Likes).ThenBy(a => a.Views).Where(a => a.EditorsChoise == true).ToList();

            return View(model);
        }
        [HttpGet]
        public IActionResult LanguageAI()
        {
            return View();
        }
        [HttpPost]
        public IActionResult LanguageAI(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                ViewBag.Error = "Text cannot be empty.";
                return View();
            }

            var client = new TextAnalyticsClient(new Uri(_endPoint), new AzureKeyCredential(_apiKey));
            DocumentSentiment response = client.AnalyzeSentiment(text); // Analyze sentiment and get response

            if (response == null)
            {
                ViewBag.Error = "Could not analyze sentiment.";
                return View();
            }

            // Pass data to the ViewBag
            ViewBag.Sentiment = response.Sentiment.ToString();
            ViewBag.ConfidenceScores = response.ConfidenceScores;

            return View();
        }

        public IActionResult Privacy()
        {
            var articles = _articleService.GetArticles().ToList();
            return View(articles);
        }

        public IActionResult CookiePolicy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Local()
        {
            var articles = _articleService.GetArticlesByCategoryId(11).OrderByDescending(a => a.DateStamp);

            var model = new CategoryPageVM();
            model.TopArticle = articles.FirstOrDefault()!;

            var others = articles.Skip(1).OrderByDescending(a => a.Views);

            model.Pop3Articles = others.Take(3).ToList();

            model.LatestArticles = others.Skip(3).OrderByDescending(a => a.DateStamp).ToList();

            return View(model);
        }

        public IActionResult Sweden()
        {
            var articles = _articleService.GetArticlesByCategoryId(12).OrderByDescending(a => a.DateStamp);

            var model = new CategoryPageVM();
            model.TopArticle = articles.FirstOrDefault()!;

            var others = articles.Skip(1).OrderByDescending(a => a.Views);

            model.Pop3Articles = others.Take(3).ToList();

            model.LatestArticles = others.Skip(3).OrderByDescending(a => a.DateStamp).ToList();

            return View(model);
        }

        public IActionResult World()
        {
            var articles = _articleService.GetArticlesByCategoryId(7).OrderByDescending(a => a.DateStamp);

            var model = new CategoryPageVM();
            model.TopArticle = articles.FirstOrDefault()!;

            var others = articles.Skip(1).OrderByDescending(a => a.Views);

            model.Pop3Articles = others.Take(3).ToList();

            model.LatestArticles = others.Skip(3).OrderByDescending(a => a.DateStamp).ToList();

            return View(model);
        }

        public IActionResult Travel()
        {
            var articles = _articleService.GetArticlesByCategoryId(9).OrderByDescending(a => a.DateStamp);

            var model = new CategoryPageVM();
            model.TopArticle = articles.FirstOrDefault()!;
                      
            var others = articles.Skip(1).OrderByDescending(a => a.Views);

            model.Pop3Articles = others.Take(3).ToList();

            model.LatestArticles = others.Skip(3).OrderByDescending(a => a.DateStamp).ToList();

            return View(model);
        }

        public IActionResult Economy()
        {
            var articles = _articleService.GetArticlesByCategoryId(10).OrderByDescending(a => a.DateStamp);

            var model = new CategoryPageVM();
            model.TopArticle = articles.FirstOrDefault()!;

            var others = articles.Skip(1).OrderByDescending(a => a.Views);

            model.Pop3Articles = others.Take(3).ToList();

            model.LatestArticles = others.Skip(3).OrderByDescending(a => a.DateStamp).ToList();

            return View(model);
        }

        public IActionResult Sports()
        {
            var articles = _articleService.GetArticlesByCategoryId(8).OrderByDescending(a => a.DateStamp);

            var model = new CategoryPageVM();
            model.TopArticle = articles.FirstOrDefault()!;

            var others = articles.Skip(1).OrderByDescending(a => a.Views);

            model.Pop3Articles = others.Take(3).ToList();

            model.LatestArticles = others.Skip(3).OrderByDescending(a => a.DateStamp).ToList();

            return View(model);
        }


        public async Task<IActionResult> Search(string searchString)
        {
            var articles = _articleService.GetSearchResult(searchString);
            return View(await articles.ToListAsync());
        }

        public async Task<IActionResult> GetElPriceTodayData()
        {
            var data = await _elpriceService.GetElPriceTodayAsync();
            return Json(data);
        }

        public IActionResult BuySubscription()
        {
            return View();
        }


    }
} 
