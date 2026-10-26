using NationalParkWebApp_1033.Models;
using NationalParkWebApp_1033.Repository.IRepository;

namespace NationalParkWebApp_1033.Repository
{
    public class NationalParkRepository:Repository<NationalPark>, INationalParkRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public NationalParkRepository(IHttpClientFactory httpClientFactory):base(httpClientFactory) 
        {
            _httpClientFactory = httpClientFactory;
        }
    }
}
