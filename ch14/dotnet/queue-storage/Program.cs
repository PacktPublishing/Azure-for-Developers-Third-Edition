// .NET example: Azure Queue Storage - Send and Receive Message
// Requires: dotnet add package Azure.Storage.Queues
// Requires: dotnet add package Azure.Identity (for Entra ID)

using System;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using Azure.Identity;

class Program
{
    static async Task Main(string[] args)
    {
        string queueName = "myqueue";
        string connectionString = "<your_connection_string>";
        string queueUri = "https://<your_account>.queue.core.windows.net/" + queueName;
        string message = "Hello from .NET!";

        // 1. Using connection string
        var queueClientConnStr = new QueueClient(connectionString, queueName);
        await queueClientConnStr.CreateIfNotExistsAsync();
        await queueClientConnStr.SendMessageAsync(message);
        var receivedConnStr = await queueClientConnStr.ReceiveMessagesAsync(maxMessages: 1);
        foreach (var msg in receivedConnStr.Value)
        {
            Console.WriteLine($"Received (conn str): {msg.MessageText}");
        }

        // 2. Using Entra ID (DefaultAzureCredential)
        var queueClientEntra = new QueueClient(new Uri(queueUri), new DefaultAzureCredential());
        await queueClientEntra.CreateIfNotExistsAsync();
        await queueClientEntra.SendMessageAsync(message);
        var receivedEntra = await queueClientEntra.ReceiveMessagesAsync(maxMessages: 1);
        foreach (var msg in receivedEntra.Value)
        {
            Console.WriteLine($"Received (Entra ID): {msg.MessageText}");
        }
    }
}
