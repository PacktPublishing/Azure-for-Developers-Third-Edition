// .NET example: Azure Service Bus - Send and Receive Message
// Requires: dotnet add package Azure.Messaging.ServiceBus
// Requires: dotnet add package Azure.Identity (for Entra ID)

using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Azure.Identity;

class Program
{
    static async Task Main(string[] args)
    {
        string queueName = "myqueue";
        string connectionString = "<your_connection_string>";
        string fullyQualifiedNamespace = "<your_servicebus_namespace>.servicebus.windows.net";
        string message = "Hello from .NET!";

        // 1. Using connection string
        await using (var clientConnStr = new ServiceBusClient(connectionString))
        {
            ServiceBusSender sender = clientConnStr.CreateSender(queueName);
            await sender.SendMessageAsync(new ServiceBusMessage(message));
            ServiceBusReceiver receiver = clientConnStr.CreateReceiver(queueName);
            ServiceBusReceivedMessage receivedMessage = await receiver.ReceiveMessageAsync();
            if (receivedMessage != null)
                Console.WriteLine($"Received (conn str): {receivedMessage.Body}");
        }

        // 2. Using Entra ID (DefaultAzureCredential)
        await using (var clientEntra = new ServiceBusClient(fullyQualifiedNamespace, new DefaultAzureCredential()))
        {
            ServiceBusSender sender = clientEntra.CreateSender(queueName);
            await sender.SendMessageAsync(new ServiceBusMessage(message));
            ServiceBusReceiver receiver = clientEntra.CreateReceiver(queueName);
            ServiceBusReceivedMessage receivedMessage = await receiver.ReceiveMessageAsync();
            if (receivedMessage != null)
                Console.WriteLine($"Received (Entra ID): {receivedMessage.Body}");
        }
    }
}
