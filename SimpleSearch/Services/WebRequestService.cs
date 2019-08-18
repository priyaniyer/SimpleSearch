using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace SimpleSearch.Services
{
    public class WebRequestService : IRequestService<string, string>
    {
        public async Task<string> SendRequestAsync(string searchUrl)
        {
            if (string.IsNullOrEmpty(searchUrl))
            {
                throw new ArgumentNullException("Invalid Search Url");
            }

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(searchUrl);

            using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
