using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SimpleSearch.Models;
using SimpleSearch.Services;

namespace SimpleSearch.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISearchService<SearchModel, string> _searchService;
        public HomeController(ISearchService<SearchModel, string> searchService)
        {
            _searchService = searchService;
        }
        public IActionResult Index()
        {
            return View("Search");
        }

        public async Task<IActionResult> Search(SearchResultDTO searchParams)
        {
            string positions = await _searchService.SearchAsync(searchParams);
            return View("Search", new SearchResultDTO(){
                SearchKeyword = searchParams.SearchKeyword,
                SearchUrl = searchParams.SearchUrl,
                SearchResult= positions
            });
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
