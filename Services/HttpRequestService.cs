using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace PullDataApi.Services
{
    public class HttpRequestService : IHostedService
    {
        private readonly HttpClient _client;
        private string _dirUri;
        private string _fileUri;

        public HttpRequestService()
        {
            _client = new HttpClient()
            {
                BaseAddress = new Uri("https://jsonplaceholder.typicode.com")
            };
            _fileUri = "..\\Files\\{0}.json";
            _dirUri = "..\\Files";
        }      

        // Tasks to do when starting up the service
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Log.Information("Starting up HttpRequestService...");
            if (!Directory.Exists(_dirUri))
            {
                Log.Information($"Creating directory ({_dirUri}) to save retrieved data");
                Directory.CreateDirectory(_dirUri);
            }

            await RetrieveData("users");
            await RetrieveData("posts");
            await RetrieveData("todos");
            await RetrieveData("comments");
        }

        private async Task RetrieveData(string appender)
        {
            var url = $"{_client.BaseAddress}{appender}";
            var response = await _client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                Log.Information($"Retrieving {appender} from External Api");
                var stringResponse = await response.Content.ReadAsStringAsync();
                Log.Information("Saving data to file");
                File.WriteAllText(string.Format(_fileUri, appender), stringResponse);
            }
            else
            {
                Log.Error($"Unable to retrieve data: {response.ReasonPhrase}");
                throw new HttpRequestException(response.ReasonPhrase);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Log.Information("Data retrieving finished. HttpRequestService finishing...");
            return Task.CompletedTask;
        }
    }
}
