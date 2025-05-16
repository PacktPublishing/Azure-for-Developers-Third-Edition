// Java example: Azure Queue Storage - Send and Receive Message
// Requires Maven: com.azure:azure-storage-queue, com.azure:azure-identity

import com.azure.storage.queue.QueueClient;
import com.azure.storage.queue.QueueClientBuilder;
import com.azure.identity.DefaultAzureCredentialBuilder;

public class QueueStorageExample {
    public static void main(String[] args) {
        String queueName = "myqueue";
        String connectionString = "<your_connection_string>";
        String queueEndpoint = "https://<your_account>.queue.core.windows.net/" + queueName;
        String message = "Hello from Java!";

        // 1. Using connection string
        QueueClient queueClientConnStr = new QueueClientBuilder()
                .connectionString(connectionString)
                .queueName(queueName)
                .buildClient();
        queueClientConnStr.createIfNotExists();
        queueClientConnStr.sendMessage(message);
        queueClientConnStr.receiveMessages(1).forEach(msg ->
                System.out.println("Received (conn str): " + msg.getMessageText()));

        // 2. Using Entra ID (DefaultAzureCredential)
        QueueClient queueClientEntra = new QueueClientBuilder()
                .endpoint(queueEndpoint)
                .credential(new DefaultAzureCredentialBuilder().build())
                .buildClient();
        queueClientEntra.createIfNotExists();
        queueClientEntra.sendMessage(message);
        queueClientEntra.receiveMessages(1).forEach(msg ->
                System.out.println("Received (Entra ID): " + msg.getMessageText()));
    }
}
