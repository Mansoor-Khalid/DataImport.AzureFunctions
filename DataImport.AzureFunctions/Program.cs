using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;
using System.IO.Compression;
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

    //if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
    //{
    //    string pathBase = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    //    var toolPath = Path.Combine(pathBase, "TransformLoadTool");
    //    int fileCount = Directory.GetFiles(toolPath, "*", SearchOption.AllDirectories).Length;
    //    if(fileCount == 1)
    //    {
    //        var zipPath = Path.Combine(toolPath, "DataImport.TranformLoad.zip");
    //        //ZipFile.CreateFromDirectory(path, zipPath);
    //        ZipFile.ExtractToDirectory(zipPath, toolPath);
    //    }
    //}
})
////#endif
.ConfigureServices(services =>
{
    services.AddSingleton(sp => sp.GetRequiredService<ILoggerFactory>().CreateLogger("DefaultLogger"));
})
.Build();

host.Run();
