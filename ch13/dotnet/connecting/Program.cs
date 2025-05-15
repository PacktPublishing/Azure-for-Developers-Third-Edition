// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

using Azure.Data.Tables;
using Azure.Identity;

// Example: Connect using a connection string
string connectionString = "<your_connection_string>";
string tableName = "myTable";
var serviceClientConnStr = new TableServiceClient(connectionString);
var tableClientConnStr = serviceClientConnStr.GetTableClient(tableName);
Console.WriteLine($"Connected to Table Storage using connection string. Table: {tableName}");

// Example: Connect using a SAS token
string storageUri = "https://<your_account>.table.core.windows.net";
string sasToken = "<your_sas_token>";
var serviceClientSas = new TableServiceClient(new Uri(storageUri), new AzureSasCredential(sasToken));
var tableClientSas = serviceClientSas.GetTableClient(tableName);
Console.WriteLine($"Connected to Table Storage using SAS token. Table: {tableName}");

// Example: Connect using Managed Identity (DefaultAzureCredential)
// Make sure your environment supports Managed Identity (e.g., Azure VM, App Service, etc.)
var serviceClientMsi = new TableServiceClient(new Uri(storageUri), new DefaultAzureCredential());
var tableClientMsi = serviceClientMsi.GetTableClient(tableName);
Console.WriteLine($"Connected to Table Storage using Managed Identity. Table: {tableName}");
