using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Ebay_Tests.IntegrationTests
{
    public class IntegrationTests
    {
        protected readonly HttpClient _client;
        protected readonly IAuthService authService;
        protected readonly IConfiguration configuration;

        protected IntegrationTests()
        {
            configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json")
                .AddUserSecrets<Program>().Build();
            foreach (var child in configuration.GetChildren())
            {
                Environment.SetEnvironmentVariable(child.Key, child.Value);
            }
            authService = new JwtService(configuration);

            var appFactory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.RemoveAll(typeof(ApplicationContext));
                    services.AddDbContext<ApplicationContext>(options =>
                    {
                        options.UseInMemoryDatabase(databaseName: "IntegrationTests");
                    });
                    var sp = services.BuildServiceProvider();
                    using (var scope = sp.CreateScope())
                    {
                        var scopedServices = scope.ServiceProvider;
                        var appDb = scopedServices.GetRequiredService<ApplicationContext>();
                        appDb.Database.EnsureDeleted();
                        appDb.Database.EnsureCreated();
                        SeedDatabase.SeedDatabaseForTests(appDb);
                    }
                });
            });
            _client = appFactory.CreateClient();
        }
    }
}