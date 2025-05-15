// Java example: Connecting to Azure Table Storage using Connection String, SAS Token, and Managed Identity
// Requires Maven dependencies: com.azure:azure-data-tables, com.azure:azure-identity

import com.azure.data.tables.TableClient;
import com.azure.data.tables.TableClientBuilder;
import com.azure.data.tables.TableServiceClient;
import com.azure.data.tables.TableServiceClientBuilder;
import com.azure.core.credential.AzureSasCredential;
import com.azure.identity.DefaultAzureCredentialBuilder;

public class TableStorageConnectExample {
    public static void main(String[] args) {
        String tableName = "myTable";

        // 1. Connect using a connection string
        String connectionString = "<your_connection_string>";
        TableServiceClient serviceClientConnStr = new TableServiceClientBuilder()
                .connectionString(connectionString)
                .buildClient();
        TableClient tableClientConnStr = serviceClientConnStr.getTableClient(tableName);
        System.out.println("Connected to Table Storage using connection string. Table: " + tableName);

        // 2. Connect using a SAS token
        String storageUri = "https://<your_account>.table.core.windows.net";
        String sasToken = "<your_sas_token>";
        TableServiceClient serviceClientSas = new TableServiceClientBuilder()
                .endpoint(storageUri)
                .credential(new AzureSasCredential(sasToken))
                .buildClient();
        TableClient tableClientSas = serviceClientSas.getTableClient(tableName);
        System.out.println("Connected to Table Storage using SAS token. Table: " + tableName);

        // 3. Connect using Managed Identity (DefaultAzureCredential)
        // Make sure your environment supports Managed Identity (e.g., Azure VM, App Service, etc.)
        TableServiceClient serviceClientMsi = new TableServiceClientBuilder()
                .endpoint(storageUri)
                .credential(new DefaultAzureCredentialBuilder().build())
                .buildClient();
        TableClient tableClientMsi = serviceClientMsi.getTableClient(tableName);
        System.out.println("Connected to Table Storage using Managed Identity. Table: " + tableName);
    }
}
