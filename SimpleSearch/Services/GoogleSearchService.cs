using SimpleSearch.Constants;
using SimpleSearch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace SimpleSearch.Services
{
    public class GoogleSearchService : ISearchService<SearchModel, string>
    {
        private readonly string _googleSearchUrl = string.Empty;
        private readonly IRequestService<string, string> _webRequestService;

        public GoogleSearchService(IRequestService<string, string> webService)
        {
            _googleSearchUrl = "http://www.google.com/search?num=" + (SearchConstants.NumberSearchResults + 1) + "&q={0}";
            _webRequestService = webService;
        }

        public async Task<string> SearchAsync(SearchModel searchParams)
        {
            string googleUrl = string.Format(_googleSearchUrl, HttpUtility.UrlEncode(searchParams.SearchKeyword));

            string responseHtml = await _webRequestService.SendRequestAsync(googleUrl);

            return GetPositionIndicesAsString(responseHtml, new Uri(searchParams.SearchUrl));

            
        }

        private string GetPositionIndicesAsString(string responseHtml, Uri url)
        {
            if(string.IsNullOrEmpty(responseHtml))
            {
                throw new ArgumentNullException("Search Response cannot be null");
            }

            IEnumerable<int> indexList = Enumerable.Empty<int>();

            MatchCollection matches = Regex.Matches(responseHtml, SearchConstants.URLRegex);

            for (int i = 0; i < matches.Count; i++)
            {
                string match = matches[i].Groups[2].Value;
                if (match.Contains(url.Host))
                {
                    indexList = indexList.Append(i + 1);
                }
            }

            return indexList.Any() ? string.Join(", ", indexList) : "0";
        }
    }
}
