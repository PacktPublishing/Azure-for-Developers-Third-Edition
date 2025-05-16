// .NET example: Azure Event Hub - Send and Receive Event
// Requires: dotnet add package Azure.Messaging.EventHubs
// Requires: dotnet add package Azure.Messaging.EventHubs.Producer
// Requires: dotnet add package Azure.Messaging.EventHubs.Consumer
// Requires: dotnet add package Azure.Identity (for Entra ID)

using System;
using System.Threading.Tasks;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Identity;

class Program
{
    static async Task Main(string[] args)
    {
        string eventHubName = "myeventhub";
        string connectionString = "<your_connection_string>";
        string fullyQualifiedNamespace = "<your_eventhub_namespace>.servicebus.windows.net";
        string message = "Hello from .NET!";
        string consumerGroup = EventHubConsumerClient.DefaultConsumerGroupName;

        // 1. Using connection string
        await using (var producerConnStr = new EventHubProducerClient(connectionString, eventHubName))
        {
            using EventDataBatch batch = await producerConnStr.CreateBatchAsync();
            batch.TryAdd(new EventData(message));
            await producerConnStr.SendAsync(batch);
        }
        await using (var consumerConnStr = new EventHubConsumerClient(consumerGroup, connectionString, eventHubName))
        {
            await foreach (PartitionEvent evt in consumerConnStr.ReadEventsAsync())
            {
                Console.WriteLine($"Received (conn str): {evt.Data.EventBody}");
                break;
            }
        }

        // 2. Using Entra ID (DefaultAzureCredential)
        await using (var producerEntra = new EventHubProducerClient(fullyQualifiedNamespace, eventHubName, new DefaultAzureCredential()))
        {
            using EventDataBatch batch = await producerEntra.CreateBatchAsync();
            batch.TryAdd(new EventData(message));
            await producerEntra.SendAsync(batch);
        }
        await using (var consumerEntra = new EventHubConsumerClient(consumerGroup, fullyQualifiedNamespace, eventHubName, new DefaultAzureCredential()))
        {
            await foreach (PartitionEvent evt in consumerEntra.ReadEventsAsync())
            {
                Console.WriteLine($"Received (Entra ID): {evt.Data.EventBody}");
                break;
            }
        }
    }
}
