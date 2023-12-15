using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using System.Linq;
using TurkMedyaCaseApp.Business.NewsAppService;
using TurkMedyaCaseApp.UI.Models;

namespace TurkMedyaCaseApp.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INewsAppService _newsAppService;

        public HomeController(ILogger<HomeController> logger, INewsAppService newsAppService)
        {
            _logger = logger;
            _newsAppService = newsAppService;
        }

        public IActionResult Index(int page = 1, int pageSize = 5, string category = null, string keyword = null)
        {
            
            var allNews = _newsAppService.GetNewsList(page, pageSize, category, keyword);
            var categories = _newsAppService.GetCategories();

            //aynı isimde yer alan birden fazla kategorinin gruplama ve teke düşürülme işlemi yapıldı
            var distinctCategories = categories.GroupBy(c => c.slug).Select(g => g.First()).ToList();

            //geçerli sayfa sayısı
            ViewBag.PageNumber = page;
            //sayfada yer alacak toplam sayfa sayısı
            ViewBag.TotalPages = (int)Math.Ceiling((double)allNews.Count() / pageSize);

            if (string.IsNullOrEmpty(category) || category == "Tüm Kategoriler")
            {
                ViewBag.SelectedCategory = null; //Tüm kategorileri temsil eden durum
            }
            else
            {
                ViewBag.SelectedCategory = category; //Diğer durumda seçilen kategori
            }
            //kategorilerin gruplanmış halinin viewbag ile view tarafına gönderimi
            ViewBag.Categories = distinctCategories;
            //arama için kullanılan keyword değerinin viewbag aracılığıyla view tarafına gönderimi
            ViewBag.Keyword = keyword;

            //pagination yapısının düzenlenmesi ve view tarafına iletilmesi
            var paginatedNews = allNews
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();


            return View(paginatedNews);
        }

        public IActionResult NewsDetail()
        {
            var newsDetail = _newsAppService.GetNewsDetail();
            return View(newsDetail);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
