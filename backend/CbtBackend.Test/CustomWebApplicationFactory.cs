using CbtBackend.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CbtBackend.Test;

public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class {
    protected override void ConfigureWebHost(IWebHostBuilder builder) {
        builder.ConfigureServices(services => {
            // remove the db context that is used for production
            var descriptor = services.SingleOrDefault(d => {
                return d.ServiceType == typeof(DbContextOptions<CbtDbContext>);
            });

            if (descriptor != null) {
                services.Remove(descriptor);
            }

            // add memory db context instead
            services.AddDbContext<CbtDbContext>(options => {
                options.UseInMemoryDatabase("InMemoryTestDatabase");
            });
        });
    }

    public HttpClient GetClient() {
        var customTestUrl = Environment.GetEnvironmentVariable("TEST_URL");

        if (customTestUrl is not null) {
            return new() { BaseAddress = new(customTestUrl) };
        } else {
            return CreateClient();
        }
    }
}
