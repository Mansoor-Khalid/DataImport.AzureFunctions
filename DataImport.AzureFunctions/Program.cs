using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Reflection;

var host = new HostBuilder()
.ConfigureFunctionsWorkerDefaults()
////#if DEBUG
.ConfigureAppConfiguration(config =>
{
    config
        .AddJsonFile("appsettings.json", true, true)
        .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
        .AddEnvironmentVariables()
        .Build();

    if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
    {
        string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        int fCount = Directory.GetFiles(path, "*", SearchOption.AllDirectories).Length;
    }
})
////#endif
.ConfigureServices(services =>
{
    services.AddSingleton(sp => sp.GetRequiredService<ILoggerFactory>().CreateLogger("DefaultLogger"));
})
.Build();

host.Run();
