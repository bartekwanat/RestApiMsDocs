using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace RestApiMsDocs
{
    class Program
    {
        private static readonly HttpClient _client = new HttpClient();

        public class Repository
        {
            [JsonPropertyName("name")] public string Name { get; set; }
            [JsonPropertyName("description")] public string Description { get; set; }

            [JsonPropertyName("html_url")] public Uri GitHubHomeUrl { get; set; }

            [JsonPropertyName("homepage")] public Uri Homepage { get; set; }

            [JsonPropertyName("watchers")] public int Watchers { get; set; }
        }

        private static async Task<List<Repository>> ProcessRepositories()
        {
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            _client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

            var streamTask = _client.GetStreamAsync("https://api.github.com/orgs/dotnet/repos");
            var repositories = await JsonSerializer.DeserializeAsync<List<Repository>>(await streamTask);
            return repositories;
        }


        static async Task Main(string[] args)
        {
            var repositories = await ProcessRepositories();

            foreach (var repo in repositories)
            {
                Console.WriteLine(repo.Name);
                Console.WriteLine(repo.Description);
                Console.WriteLine(repo.GitHubHomeUrl);
                Console.WriteLine(repo.Homepage);
                Console.WriteLine(repo.Watchers);
                Console.WriteLine();
            }
        }
    }
}