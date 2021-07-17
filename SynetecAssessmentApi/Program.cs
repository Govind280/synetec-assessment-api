using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SynetecAssessmentApi.Persistence;

namespace SynetecAssessmentApi
{
    /// <summary>
    /// Main Entry Class
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Entry method for startup
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<AppDbContext>();

                DbContextGenerator.Initialize(services);
            }

            host.Run();
        }

        /// <summary>
        /// CreateHostBuilder method
        /// </summary>
        /// <param name="args"></param>
        /// <returns><see cref="IHostBuilder"/></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
