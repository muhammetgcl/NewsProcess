using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TurkMedyaCaseApp.DTO.Models;

namespace TurkMedyaCaseApp.Business.NewsAppService
{
    public interface INewsAppService
    {
        IEnumerable<NewsListResponseModel.ItemList> GetNewsList(int page = 1, int pageSize = 5, string category = null, string keyword = null);
        List<NewsListResponseModel.Category> GetCategories();
        NewsDetailResponseModel.Root GetNewsDetail();
    }
}
