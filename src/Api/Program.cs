using Api.Logger;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging((hostBuilderContext, logging) =>
                {
                    logging.ClearProviders();
                    logging.AddRoundTheCodeFileLogger(options =>
                    {
                        hostBuilderContext.Configuration.GetSection("Logging").GetSection("RoundTheCodeFile").GetSection("Options").Bind(options);
                    });
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls("http://0.0.0.0:5001");
                });
    }
}
