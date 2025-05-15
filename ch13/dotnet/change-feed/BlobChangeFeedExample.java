// Java example: Reading Azure Blob Storage Change Feed
// Requires Maven dependency: com.azure:azure-storage-blob-changefeed

import com.azure.storage.blob.BlobServiceClient;
import com.azure.storage.blob.BlobServiceClientBuilder;
import com.azure.storage.blob.changefeed.BlobChangefeedClient;
import com.azure.storage.blob.changefeed.BlobChangefeedClientBuilder;
import com.azure.storage.blob.changefeed.models.BlobChangefeedEvent;

public class BlobChangeFeedExample {
    public static void main(String[] args) {
        // Replace with your Blob service endpoint and credentials
        String connectionString = "<your_connection_string>";

        // Create a BlobServiceClient
        BlobServiceClient serviceClient = new BlobServiceClientBuilder()
                .connectionString(connectionString)
                .buildClient();

        // Create a BlobChangefeedClient
        BlobChangefeedClient changefeedClient = new BlobChangefeedClientBuilder(serviceClient).buildClient();

        // Read change feed events
        for (BlobChangefeedEvent event : changefeedClient.getEvents()) {
            System.out.println("Event Type: " + event.getEventType());
            System.out.println("Subject: " + event.getSubject());
            System.out.println("Event Time: " + event.getEventTime());
            System.out.println("---");
        }
    }
}
