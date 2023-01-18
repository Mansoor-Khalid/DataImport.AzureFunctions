using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;
using Microsoft.Extensions.Logging;

namespace DurableFunctions.Chaining;

public class TransformLoadFunction
{
    private readonly ILogger _logger;
    public TransformLoadFunction(ILogger logger)
    {
        _logger = logger;
    }
    
    [Function("TransformLoadFunction_QueueStart")]
    public async Task QueueStart(
        [QueueTrigger("js-queue-items", Connection = "AzureWebJobsStorage")] string myQueueItem,
        [DurableClient] DurableClientContext starter)
    {
        var instanceId = await starter.Client.ScheduleNewOrchestrationInstanceAsync(nameof(TransformLoadFunction));
       
    }

    [Function(nameof(TransformLoadFunction))]
    public async Task RunOrchestrator([OrchestrationTrigger] TaskOrchestrationContext context)
    {
        var toolsTask = context.CallActivityAsync<ToolsResponse>(nameof(FetchToolsFunction.FetchTools));
        //var partsTask = context.CallActivityAsync<PartsResponse>(nameof(FetchPartsFunction.FetchParts));
        //await Task.WhenAll(toolsTask, partsTask);

        //await context.CallActivityAsync<bool>(nameof(HaveCoffeeFunction.HaveCoffee));


        //var buildInput = new BuildShellInput { Tools = toolsTask.Result, Parts = partsTask.Result };
        //var robot = await context.CallActivityAsync<RobotResponse>(nameof(BuildShellFunction.BuildShell), buildInput);
        //robot = await context.CallActivityAsync<RobotResponse>(nameof(ProgramRobotFunction.ProgramRobot), robot);
        //robot = await context.CallActivityAsync<RobotResponse>(nameof(TestRobotFunction.TestRobot), robot);

        //_logger.LogInformation($"Completed robot. Programmed: {robot.IsProgrammed}, tested: {robot.IsTested}, solid: {robot.IsSolid}, epic: {robot.IsEpic}");


        //var tasks = new List<Task<SentimentResult>>();
        //for (var userId = 1; userId <= 100; userId++)
        //{
        //    tasks.Add(context.CallActivityAsync<SentimentResult>(nameof(CalculateUserSentiment), userId));
        //}
        //await Task.WhenAll(tasks);

        //var results = tasks.Select(x => x.Result);
        //await context.CallActivityAsync(nameof(PrintReport), results);
    }
}