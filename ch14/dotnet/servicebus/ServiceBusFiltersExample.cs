// .NET example: Azure Service Bus - Add SQL, Boolean, and Correlation Filters
// Requires: dotnet add package Azure.Messaging.ServiceBus
// Requires: dotnet add package Azure.Identity (for Entra ID)

using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus.Administration;
using Azure.Identity;

class Program
{
    static async Task Main(string[] args)
    {
        string connectionString = "<your_connection_string>";
        string fullyQualifiedNamespace = "<your_servicebus_namespace>.servicebus.windows.net";
        string topicName = "mytopic";
        string subscriptionName = "mysubscription";

        // 1. Using connection string
        var adminClientConnStr = new ServiceBusAdministrationClient(connectionString);
        await AddFilters(adminClientConnStr, topicName, subscriptionName);

        // 2. Using Entra ID (DefaultAzureCredential)
        var adminClientEntra = new ServiceBusAdministrationClient(fullyQualifiedNamespace, new DefaultAzureCredential());
        await AddFilters(adminClientEntra, topicName, subscriptionName);
    }

    static async Task AddFilters(ServiceBusAdministrationClient adminClient, string topic, string subscription)
    {
        // SQL Filter
        await adminClient.CreateRuleAsync(topic, subscription, "SqlFilterRule", new SqlRuleFilter("Color = 'Red'"));
        Console.WriteLine("Added SQL filter: Color = 'Red'");

        // Complex SQL Filter
        await adminClient.CreateRuleAsync(topic, subscription, "ComplexSqlFilterRule", new SqlRuleFilter("(Color = 'Red' OR Color = 'Blue') AND Quantity > 10 AND sys.Label = 'Important'"));
        Console.WriteLine("Added complex SQL filter: (Color = 'Red' OR Color = 'Blue') AND Quantity > 10 AND sys.Label = 'Important'");

        // Boolean Filter (True/False)
        await adminClient.CreateRuleAsync(topic, subscription, "TrueFilterRule", new TrueRuleFilter());
        await adminClient.CreateRuleAsync(topic, subscription, "FalseFilterRule", new FalseRuleFilter());
        Console.WriteLine("Added Boolean filters: True and False");

        // Correlation Filter
        await adminClient.CreateRuleAsync(topic, subscription, "CorrelationFilterRule", new CorrelationRuleFilter { CorrelationId = "my-correlation-id" });
        Console.WriteLine("Added Correlation filter: CorrelationId = 'my-correlation-id'");
    }
}
