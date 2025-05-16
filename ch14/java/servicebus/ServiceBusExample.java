// Java example: Azure Service Bus - Send and Receive Message
// Requires Maven: com.azure:azure-messaging-servicebus, com.azure:azure-identity

import com.azure.messaging.servicebus.*;
import com.azure.identity.DefaultAzureCredentialBuilder;

public class ServiceBusExample {
    public static void main(String[] args) {
        String queueName = "myqueue";
        String connectionString = "<your_connection_string>";
        String fullyQualifiedNamespace = "<your_servicebus_namespace>.servicebus.windows.net";
        String message = "Hello from Java!";

        // 1. Using connection string
        try (ServiceBusClientBuilder builder = new ServiceBusClientBuilder()) {
            ServiceBusSenderClient sender = builder.connectionString(connectionString).sender().queueName(queueName).buildClient();
            sender.sendMessage(new ServiceBusMessage(message));
            ServiceBusReceiverClient receiver = builder.connectionString(connectionString).receiver().queueName(queueName).buildClient();
            ServiceBusReceivedMessage receivedMessage = receiver.receiveMessages(1).stream().findFirst().orElse(null);
            if (receivedMessage != null)
                System.out.println("Received (conn str): " + receivedMessage.getBody().toString());
            sender.close();
            receiver.close();
        }

        // 2. Using Entra ID (DefaultAzureCredential)
        ServiceBusClientBuilder builderEntra = new ServiceBusClientBuilder()
                .fullyQualifiedNamespace(fullyQualifiedNamespace)
                .credential(new DefaultAzureCredentialBuilder().build());
        ServiceBusSenderClient senderEntra = builderEntra.sender().queueName(queueName).buildClient();
        senderEntra.sendMessage(new ServiceBusMessage(message));
        ServiceBusReceiverClient receiverEntra = builderEntra.receiver().queueName(queueName).buildClient();
        ServiceBusReceivedMessage receivedMessageEntra = receiverEntra.receiveMessages(1).stream().findFirst().orElse(null);
        if (receivedMessageEntra != null)
            System.out.println("Received (Entra ID): " + receivedMessageEntra.getBody().toString());
        senderEntra.close();
        receiverEntra.close();
    }
}
