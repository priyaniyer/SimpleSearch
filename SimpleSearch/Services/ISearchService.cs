using SimpleSearch.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleSearch.Services
{
    public interface ISearchService<TSearchParams, TSearchResult>
    {
        Task<TSearchResult> SearchAsync(TSearchParams searchParams);
    }
}
