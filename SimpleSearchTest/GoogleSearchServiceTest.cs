using Moq;
using NUnit.Framework;
using SimpleSearch.Models;
using SimpleSearch.Services;
using System;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSearchTest
{
    public class GoogleSearchServiceTest
    {
        private Mock<IRequestService<string, string>> _webRequestService;
        private ISearchService<SearchModel, string> _searchService;
        private const string _validUrlString = "http://www.google.com/search?num=101&q=property+title";

        [SetUp]
        public void Setup()
        {
            _webRequestService = new Mock<IRequestService<string, string>>();
            _searchService = new GoogleSearchService(_webRequestService.Object);
        }

        [Test]
        public async Task TestSearchParamsEmpty()
        {
            var searchModel = new SearchModel();
            try
            {
                await _searchService.SearchAsync(searchModel);
            }
            catch(ArgumentNullException ex)
            {
                Assert.Pass(ex.Message);
            }

            Assert.Fail();
        }

        [Test]
        public async Task TestSearchKeywordEmpty()
        {
            var searchModel = new SearchModel();
            searchModel.SearchUrl = "https://www.infotrack.com.au";
            try
            {
                await _searchService.SearchAsync(searchModel);
            }
            catch (ArgumentNullException ex)
            {
                Assert.Pass(ex.Message);
            }

            Assert.Fail();
        }

        [Test]
        public async Task TestSearchUrlEmpty()
        {
            var searchModel = new SearchModel();
            searchModel.SearchKeyword = "property title";
            try
            {
                await _searchService.SearchAsync(searchModel);
            }
            catch (ArgumentNullException ex)
            {
                Assert.Pass(ex.Message);
            }

            Assert.Fail();
        }

        [Test]
        public async Task TestSearchResponseEmpty()
        {
            var searchModel = new SearchModel()
            {
                SearchKeyword = "property title",
                SearchUrl = "https://www.infotrack.com.au"
            };

            _webRequestService.Setup(r => r.SendRequestAsync(_validUrlString)).ReturnsAsync(string.Empty);

            try
            {
                var result = await _searchService.SearchAsync(searchModel);
            }
            catch(ArgumentNullException ex)
            {
                Assert.Pass(ex.Message);
            }

            Assert.Fail();
        }

        [Test]
        public async Task TestInvalidSearchUrl()
        {
            var searchModel = new SearchModel()
            {
                SearchKeyword = "property title",
                SearchUrl = "www.infotrack.com.au"
            };

            try
            {
                var result = await _searchService.SearchAsync(searchModel);
            }
            catch (UriFormatException ex)
            {
                Assert.Pass(ex.Message);
            }
            catch (Exception ex)
            {
                Assert.Fail();
            }

            Assert.Fail();
        }

        [Test]
        public async Task TestSearchResponseNoMatch()
        {
            var searchModel = new SearchModel()
            {
                SearchKeyword = "property title",
                SearchUrl = "https://www.infotrack.com.au"
            };

            _webRequestService.Setup(r => r.SendRequestAsync(_validUrlString)).ReturnsAsync("Dummy Response");

            var result = await _searchService.SearchAsync(searchModel);

            Assert.True(result.Contains('0'));
        }

        [Test]
        public async Task TestSearchResponseMatch()
        {
            var searchModel = new SearchModel()
            {
                SearchKeyword = "property title",
                SearchUrl = "https://www.infotrack.com.au"
            };

            StringBuilder sbResponse = new StringBuilder("<a href=\"/url?q=https://www.infotrack.com.au/");
            sbResponse.Append(" <a href=\"/url?q=https://www.officeworks.com.au/");
            sbResponse.Append(" <a href=\"/url?q=https://www.infotrack.com.au/");

            _webRequestService.Setup(r => r.SendRequestAsync(_validUrlString)).ReturnsAsync(sbResponse.ToString());

            var result = await _searchService.SearchAsync(searchModel);
            Assert.True(result.Equals("1, 3"));
        }
    }
}