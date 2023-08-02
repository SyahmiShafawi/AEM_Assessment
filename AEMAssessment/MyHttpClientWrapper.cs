using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AEMAssessment
{
    public class MyHttpClientWrapper : IDisposable
    {
        private readonly HttpClient httpClient;
        public static IConfiguration _configuration;
        private string baseURL;
        public MyHttpClientWrapper(IConfiguration configuration)
        {
            httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromSeconds(10);

            _configuration = configuration;
            baseURL = _configuration.GetValue<string>("BaseURL");
        }

        public async Task<string> PostDataAsync(string url, string jsonPayload)
        {
            try
            {
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync(baseURL + url, content))
                {
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsStringAsync();
                }
            }
            catch (HttpRequestException ex)
            {
                // Handle specific exception or log the error
                throw new Exception("Error occurred during the HTTP POST request.", ex);
            }
        }

        public async Task<string> GetResponseAsync(string url, string apikey)
        {
            try
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apikey);

                using (var response = await httpClient.GetAsync(baseURL + url))
                {
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsStringAsync();
                }
            }
            catch (HttpRequestException ex)
            {
                // Handle specific exception or log the error
                throw new Exception("Error occurred during the HTTP request.", ex);
            }
        }

        public void Dispose()
        {
            httpClient.Dispose();
        }
    }
}
