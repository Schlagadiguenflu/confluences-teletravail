using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO;

namespace Api.Logger
{
    [ProviderAlias("RoundTheCodeFile")]
    public class RoundTheCodeFileLoggerProvider : ILoggerProvider
    {
        private IWebHostEnvironment _hostEnvironment;
        public readonly RoundTheCodeFileLoggerOptions Options;

        public RoundTheCodeFileLoggerProvider(IWebHostEnvironment environment,IOptions<RoundTheCodeFileLoggerOptions> _options)
        {
            _hostEnvironment = environment;
            Options = _options.Value;
            string path = Path.Combine(_hostEnvironment.ContentRootPath, Options.FolderPath);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new RoundTheCodeFileLogger(this);
        }

        public void Dispose()
        {
        }
    }
}
