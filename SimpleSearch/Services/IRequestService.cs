using System.Threading.Tasks;

namespace SimpleSearch.Services
{
    public interface IRequestService<TRequest, TResponse>
    {
        Task<TResponse> SendRequestAsync(TRequest request);
    }
}
