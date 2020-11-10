using System.Net.Http;
using System.Threading.Tasks;

namespace IdentityServerProject.Client1.Services
{
    public interface IApiResourceHttpClient
    {
        Task<HttpClient> GetHttpClient();
    }
}
