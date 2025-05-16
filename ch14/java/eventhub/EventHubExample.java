// Java example: Azure Event Hub - Send and Receive Event
// Requires Maven: com.azure:azure-messaging-eventhubs, com.azure:azure-identity

import com.azure.messaging.eventhubs.*;
import com.azure.identity.DefaultAzureCredentialBuilder;
import java.util.Collections;

public class EventHubExample {
    public static void main(String[] args) {
        String eventHubName = "myeventhub";
        String connectionString = "<your_connection_string>";
        String fullyQualifiedNamespace = "<your_eventhub_namespace>.servicebus.windows.net";
        String message = "Hello from Java!";
        String consumerGroup = EventHubClientBuilder.DEFAULT_CONSUMER_GROUP_NAME;

        // 1. Using connection string
        EventHubProducerClient producerConnStr = new EventHubClientBuilder()
                .connectionString(connectionString, eventHubName)
                .buildProducerClient();
        producerConnStr.send(new EventData(message));
        producerConnStr.close();

        EventHubConsumerClient consumerConnStr = new EventHubClientBuilder()
                .connectionString(connectionString, eventHubName)
                .consumerGroup(consumerGroup)
                .buildConsumerClient();
        for (PartitionEvent event : consumerConnStr.receive(false, 1)) {
            System.out.println("Received (conn str): " + event.getData().getBodyAsString());
            break;
        }
        consumerConnStr.close();

        // 2. Using Entra ID (DefaultAzureCredential)
        EventHubProducerClient producerEntra = new EventHubClientBuilder()
                .fullyQualifiedNamespace(fullyQualifiedNamespace)
                .eventHubName(eventHubName)
                .credential(new DefaultAzureCredentialBuilder().build())
                .buildProducerClient();
        producerEntra.send(new EventData(message));
        producerEntra.close();

        EventHubConsumerClient consumerEntra = new EventHubClientBuilder()
                .fullyQualifiedNamespace(fullyQualifiedNamespace)
                .eventHubName(eventHubName)
                .consumerGroup(consumerGroup)
                .credential(new DefaultAzureCredentialBuilder().build())
                .buildConsumerClient();
        for (PartitionEvent event : consumerEntra.receive(false, 1)) {
            System.out.println("Received (Entra ID): " + event.getData().getBodyAsString());
            break;
        }
        consumerEntra.close();
    }
}
