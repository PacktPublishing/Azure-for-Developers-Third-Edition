// Java example: Azure Service Bus - Add SQL, Boolean, and Correlation Filters
// Requires Maven: com.azure:azure-messaging-servicebus, com.azure:azure-identity

import com.azure.messaging.servicebus.administration.*;
import com.azure.messaging.servicebus.administration.models.*;
import com.azure.identity.DefaultAzureCredentialBuilder;

public class ServiceBusFiltersExample {
    public static void main(String[] args) {
        String connectionString = "<your_connection_string>";
        String fullyQualifiedNamespace = "<your_servicebus_namespace>.servicebus.windows.net";
        String topicName = "mytopic";
        String subscriptionName = "mysubscription";

        // 1. Using connection string
        ServiceBusAdministrationClient adminClientConnStr = new ServiceBusAdministrationClientBuilder()
                .connectionString(connectionString)
                .buildClient();
        addFilters(adminClientConnStr, topicName, subscriptionName);

        // 2. Using Entra ID (DefaultAzureCredential)
        ServiceBusAdministrationClient adminClientEntra = new ServiceBusAdministrationClientBuilder()
                .fullyQualifiedNamespace(fullyQualifiedNamespace)
                .credential(new DefaultAzureCredentialBuilder().build())
                .buildClient();
        addFilters(adminClientEntra, topicName, subscriptionName);
    }

    static void addFilters(ServiceBusAdministrationClient adminClient, String topic, String subscription) {
        // SQL Filter
        adminClient.createRule(topic, subscription, new CreateRuleOptions()
                .setName("SqlFilterRule")
                .setFilter(new SqlRuleFilter("Color = 'Red'")));
        System.out.println("Added SQL filter: Color = 'Red'");

        // Complex SQL Filter
        adminClient.createRule(topic, subscription, new CreateRuleOptions()
                .setName("ComplexSqlFilterRule")
                .setFilter(new SqlRuleFilter("(Color = 'Red' OR Color = 'Blue') AND Quantity > 10 AND sys.Label = 'Important'")));
        System.out.println("Added complex SQL filter: (Color = 'Red' OR Color = 'Blue') AND Quantity > 10 AND sys.Label = 'Important'");

        // Boolean Filter (True/False)
        adminClient.createRule(topic, subscription, new CreateRuleOptions()
                .setName("TrueFilterRule")
                .setFilter(new TrueRuleFilter()));
        adminClient.createRule(topic, subscription, new CreateRuleOptions()
                .setName("FalseFilterRule")
                .setFilter(new FalseRuleFilter()));
        System.out.println("Added Boolean filters: True and False");

        // Correlation Filter
        adminClient.createRule(topic, subscription, new CreateRuleOptions()
                .setName("CorrelationFilterRule")
                .setFilter(new CorrelationRuleFilter().setCorrelationId("my-correlation-id")));
        System.out.println("Added Correlation filter: CorrelationId = 'my-correlation-id'");
    }
}
