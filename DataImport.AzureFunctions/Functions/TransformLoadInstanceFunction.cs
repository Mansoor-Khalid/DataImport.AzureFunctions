using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace DataImport.AzureFunctions;

public class TransformLoadInstanceFunction
{
    private readonly ILogger _logger;
    public TransformLoadInstanceFunction(ILogger logger)
    {
        _logger = logger;
    }

    [Function("TransformLoadInstance_QueueFunction")]
    public async Task Run(
    [QueueTrigger("%EdGraphStorageConnectionQueueName%", Connection = "ConnectionStringsStorageConnection")] string dataImportTransformLoadInstanceName)
    {

        //var storageConnectionTransformLoadQueue = Environment.GetEnvironmentVariable("ConnectionStrings__storageConnection");
        //var dataImportTransformLoadQueueName = Environment.GetEnvironmentVariable("EdGraph__storageConnection__QueueName"); //"DataImport-TransformLoad-Queue"

        //_logger.LogInformation($"ConnectionStrings__storageConnection: {storageConnectionTransformLoadQueue}");
        //_logger.LogInformation($"EdGraph__storageConnection__QueueName: {dataImportTransformLoadQueueName}");

        Process process = Extensions.Extensions.GetTransformLoadProcess(dataImportTransformLoadInstanceName, _logger);
        try
        {

            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            string err = process.StandardError.ReadToEnd();
            process.WaitForExit();

            _logger.LogInformation($"{output}");
            _logger.LogError($"{err}");


            _logger.LogInformation($"QueueTrigger TransformLoadInstance_QueueFunction executed at: {DateTime.Now}");

        }
        catch (Exception exception)
        {

            _logger.LogError($"{exception}");
        }

    }


}