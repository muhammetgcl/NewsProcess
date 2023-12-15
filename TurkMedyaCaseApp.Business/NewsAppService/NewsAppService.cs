using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TurkMedyaCaseApp.DTO.Models;

namespace TurkMedyaCaseApp.Business.NewsAppService
{
    public class NewsAppService : INewsAppService
    {
        private readonly HttpClient _httpClient;


        public NewsAppService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        //Haber listesini verilen url'den çekmek için kullanılan method
        public IEnumerable<NewsListResponseModel.ItemList> GetNewsList(int page = 1, int pageSize = 5, string category = null, string keyword = null)
        {
            string apiUrl = "https://www.turkmedya.com.tr/anasayfa.json";
            string json = _httpClient.GetStringAsync(apiUrl).Result;
            var responseModel = JsonConvert.DeserializeObject<NewsListResponseModel.Root>(json);

            //burda json'dan dönen sectionType'a göre ayrılan değerler vardı, içeriklerini ve farklarını kestiremediğim için değerleri birleştirerek işlem yaptım.
            List<NewsListResponseModel.ItemList> filteredNews = responseModel.data
            .SelectMany(d => d.itemList)
            .Where(news =>
            //Kategori seçilmemişse veya kategori içeriyorsa
            (string.IsNullOrEmpty(category) || news.category.slug.Contains(category, StringComparison.OrdinalIgnoreCase)) &&
            //Anahtar kelime seçilmemişse veya başlık içeriyorsa
            (string.IsNullOrEmpty(keyword) || news.title.Contains(keyword, StringComparison.OrdinalIgnoreCase)))
            .ToList();

            
            return filteredNews;
        }

        //Kategori listesini dinamik olarak view tarafında listelemek için kullanılan method
        public List<NewsListResponseModel.Category> GetCategories()
        {
            string newsListUri = "https://www.turkmedya.com.tr/anasayfa.json";
            string newsJsonData = _httpClient.GetStringAsync(newsListUri).Result;
            var newsList = JsonConvert.DeserializeObject<NewsListResponseModel.Root>(newsJsonData);

            //İsmi boş olmayan ve "\" olmayan değerlerin seçilmesi işlemi yapıldı. 
            return newsList.data.SelectMany(d => d.itemList.Select(i => i.category)).Where(category=> !string.IsNullOrEmpty(category.slug) && category.slug != "\"").Distinct().ToList();
        }

        //Haber detayını çekmek için kullanılan method.
        public NewsDetailResponseModel.Root GetNewsDetail()
        {
            string newsDetailUri = "https://www.turkmedya.com.tr/detay.json";
            string newsDetailJsonData = _httpClient.GetStringAsync(newsDetailUri).Result;
            var newsDetail = JsonConvert.DeserializeObject<NewsDetailResponseModel.Root>(newsDetailJsonData);

            return newsDetail;

        }

    }
}
