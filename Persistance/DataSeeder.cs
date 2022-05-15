using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PullDataApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace PullDataApi.Persistance
{
    public class DataSeeder : IHostedService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private string _fileUri;
        public DataSeeder(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            _fileUri = "Files\\posts.json";
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").Equals("Development"))
                    dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();
                SeedData(dbContext);
            }            

            return Task.CompletedTask;
        }

        private void SeedData(DataContext context)
        {
            var posts = File.ReadAllText(_fileUri);
            var postList = JsonSerializer.Deserialize<List<Post>>(posts,
                new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            context.Posts.AddRange(postList);
            context.SaveChanges();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
