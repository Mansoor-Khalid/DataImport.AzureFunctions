using Azure.Storage.Queues;
using DataImport.AzureFunctions.Extensions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;
using Microsoft.Extensions.Logging;

namespace DataImport.AzureFunctions;

public class TransformLoadTimerFunction
{
    private readonly ILogger _logger;
    public TransformLoadTimerFunction(ILogger logger)
    {
        _logger = logger;
    }

    [Function("TransformLoad_TimerFunction")]
    public void Run([TimerTrigger("%EdGraphTransformLoadTimerTrigger%", /*RunOnStartup = true,*/ UseMonitor = true)] TimerInfo timerInfo, FunctionContext context)
    {
        QueueClient queueClient = Extensions.Extensions.GetQueue();

        _logger.LogInformation("Scan DbServer for DataImport instances");
        var dataImportDbs = DbExtensions.ScanDataImportDatabases();
        _logger.LogInformation($"Scan DbServer for DataImport instances found: {string.Join(", ", dataImportDbs)}");


        foreach (var dbName in dataImportDbs)
        {
            var isPendingFiles = DbExtensions.ScanDataImportPendingFiles(dbName);
            if (!isPendingFiles) continue;
            queueClient.SendMessageAsync($"{dbName}");
        }

        _logger.LogInformation($"TransformLoadTimerFunction Function Ran. Next timer schedule = {timerInfo.ScheduleStatus.Next}");
    }


}