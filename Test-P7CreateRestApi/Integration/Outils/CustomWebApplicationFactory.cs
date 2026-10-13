using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using P7CreateRestApi.Data;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Test_P7CreateRestApi.Integration.Outils
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        private readonly SqliteConnection _connection;

        public CustomWebApplicationFactory(SqliteConnection connection)
        {
            _connection = connection;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Replace the SQL Server database of the API by the SQLite one of the test
                var descriptor = services.Single(d => d.ServiceType == typeof(DbContextOptions<LocalDbContext>));
                services.Remove(descriptor);

                services.AddDbContext<LocalDbContext>(options => options.UseSqlite(_connection));
            });
        }

        public async Task<HttpClient> CreateClientWithTokenAsync(string username, string password)
        {
            HttpClient client = CreateClient();

            HttpResponseMessage response = await client.PostAsJsonAsync("/Login/Access", new { username, password });
            response.EnsureSuccessStatusCode();

            TokenResponse? tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse!.Token);

            return client;
        }

        private class TokenResponse
        {
            public string Token { get; set; } = string.Empty;
        }
    }
}