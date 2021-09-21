using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using SWGOH.Models.Options;
using SWGOH.Tools.Services;
using SWGOH.Tools.Logic;

namespace SWGOH.Tools
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
           {
               var configuration = context.Configuration;
               services.Configure<GCOptions>(configuration.GetSection(GCOptions.Name));
               services.Configure<GameClientOptions>(configuration.GetSection(GameClientOptions.Name));

               services.AddHostedService<BGService>();
               services.AddScoped<GameClient>();
               services.AddScoped<GameClientService>();
           });
    }
}
