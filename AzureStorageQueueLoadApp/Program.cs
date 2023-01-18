
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using System;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;


var connectionString = "DefaultEndpointsProtocol=https;AccountName=egakscooldevscus;AccountKey=INKSwint2Pdd0MsXukfXJFIKesLNiThQCIVI4SyL6oIGCge1q7qhtxXEWCptIQmWRzEceBzjaLli+ASt02opNA==;EndpointSuffix=core.windows.net";

// Create a unique name for the queue
string queueName = "js-queue-items";

// Instantiate a QueueClient to create and interact with the queue
QueueClient queueClient = new QueueClient(connectionString, queueName, new QueueClientOptions
{
    MessageEncoding = QueueMessageEncoding.Base64
});

Console.WriteLine("\nAdding messages to the queue...");

for (int i = 0;i < 1000; i++)
{
    await queueClient.SendMessageAsync($"message number # {i}");
    Console.WriteLine($"\n wrote message number # {i}. yahhh!!!");
}
Console.WriteLine("\nCompleted 1000 message");

// Send several messages to the queue
//await queueClient.SendMessageAsync("First message");
//Console.WriteLine("\nCompleted first message");
//await queueClient.SendMessageAsync("Second message");

// Save the receipt so we can update this message later
//SendReceipt receipt = await queueClient.SendMessageAsync("Third message");

Console.ReadKey();
