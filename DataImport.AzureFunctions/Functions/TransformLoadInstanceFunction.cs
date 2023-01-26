using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;
using Microsoft.Extensions.Logging;
using System;
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

        try
        {
            Process process = Extensions.Extensions.GetTransformLoadProcess(dataImportTransformLoadInstanceName, _logger);

            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            string err = process.StandardError.ReadToEnd();
            process.WaitForExit();

            _logger.LogInformation($"{output}");
            _logger.LogError($"{err}");

            _logger.LogInformation($"QueueTrigger TransformLoadInstance_QueueFunction execution ended at: {DateTime.Now}");
        }
        catch (Exception exception)
        {

            _logger.LogError($"{exception}");
        }

    }


}