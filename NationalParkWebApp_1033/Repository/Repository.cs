using NationalParkWebApp_1033.Repository.IRepository;
using Newtonsoft.Json;
using System.Text;

namespace NationalParkWebApp_1033.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public Repository(IHttpClientFactory httpClientFactory)
        {
           _httpClientFactory = httpClientFactory; 
        }
        public async Task<bool> CreateAsync(string url, T objToCreate)
        {
            var request=new HttpRequestMessage(HttpMethod.Post, url);
            if(objToCreate != null)
            {
                request.Content=new StringContent(JsonConvert.SerializeObject(objToCreate),
                    Encoding.UTF8,"application/json");
            }
            var Client = _httpClientFactory.CreateClient();
            HttpResponseMessage httpResponse=await Client.SendAsync(request);
            if (httpResponse.StatusCode == System.Net.HttpStatusCode.Created)
                return true;return false;
        }

        public async Task<bool> DeleteAsync(string url, int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, url+"/"+id.ToString());
            var Client = _httpClientFactory.CreateClient();
            HttpResponseMessage httpResponse = await Client.SendAsync(request);
            if (httpResponse.StatusCode == System.Net.HttpStatusCode.Created)
                return true; return false;
        }

        public async Task<IEnumerable<T>> GetAllAsync(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var Client = _httpClientFactory.CreateClient();
            HttpResponseMessage httpResponse = await Client.SendAsync(request);
            if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string JsonString = await httpResponse.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<T>>(JsonString);
            }
            return null;
        }

        public async Task<T> GetAsync(string url, int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url + "/" + id.ToString());
            var Client = _httpClientFactory.CreateClient();
            HttpResponseMessage httpResponse = await Client.SendAsync(request);
            if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string JsonString = await httpResponse.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(JsonString);
            }
            return null;
        }

        public async Task<bool> UpdateAsync(string url, T objToUpdate)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, url);
            if (objToUpdate != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(objToUpdate),
                    Encoding.UTF8, "application/json");
            }
            var Client = _httpClientFactory.CreateClient();
            HttpResponseMessage httpResponse = await Client.SendAsync(request);
            if (httpResponse.StatusCode == System.Net.HttpStatusCode.Created)
                return true; return false;
        }
    }
}
