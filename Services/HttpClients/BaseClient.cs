using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace HttpClients
{
    public abstract class BaseClient
    {
        protected BaseClient(string baseAddress)
        {

            Client = new HttpClient
            {
                BaseAddress = new Uri(baseAddress)

            };

            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

        }

        public HttpClient Client { get; set; }
        protected abstract string ServiceAddress { get; }

        
        protected async Task<IActionResult>GetAsync<T>(string url)
        {
           
            var response = await Client.GetAsync(url);
           
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsAsync<T>();

                return new ObjectResult(data){StatusCode = (int)response.StatusCode};
            }

            return  new ObjectResult(response.ReasonPhrase){StatusCode = (int)response.StatusCode};

     
        }



        protected async Task<IActionResult> PostAsync<T>(string url, T value)
        {

            var response = await Client.PostAsJsonAsync(url, value);
            
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsAsync<string>();

                return new ObjectResult(data) { StatusCode = (int)response.StatusCode };
            }

            return new ObjectResult(response.ReasonPhrase) { StatusCode = (int)response.StatusCode };

        }

    }
}
