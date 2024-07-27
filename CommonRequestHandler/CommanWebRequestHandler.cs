using System.Text;

namespace TaskManagementPortal.CommonRequestHandler
{
    public class CommanWebRequestHandler : ICommanWebRequestHandler
    {
        private readonly ILogger<CommanWebRequestHandler> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public CommanWebRequestHandler(IHttpClientFactory httpClientFactory, ILogger<CommanWebRequestHandler> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<HttpResponseMessage> WebRequestExecute(string httpclientname, string url, string method, string input, string functionalityName)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                string myValue = "https://localhost:7117";
                var data = new StringContent(input, Encoding.UTF8, "application/json");
                using var client = _httpClientFactory.CreateClient(httpclientname);
                client.BaseAddress = new Uri($"{myValue}");
                client.Timeout = TimeSpan.FromMinutes(10);
                if (method == "POST")
                    response = await client.PostAsync($"{url}", data);
                else if (method == "GET")
                    response = await client.GetAsync($"{url}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while {functionalityName} => {ex.Message}");
                _logger.LogDebug($"Error occurred while  {functionalityName} InnerException => {ex.InnerException}");
                _logger.LogDebug($"Error occurred while {functionalityName} StackTrace => {ex.StackTrace}");
            }
            return await Task.FromResult(response);
        }

    }
}
