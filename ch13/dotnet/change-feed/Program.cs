// .NET example: Reading Azure Blob Storage Change Feed
// Requires: dotnet add package Azure.Storage.Blobs.ChangeFeed

using System;
using System.Threading.Tasks;
using Azure.Storage.Blobs.ChangeFeed;
using Azure.Storage.Blobs;

class Program
{
    static async Task Main(string[] args)
    {
        // Replace with your Blob service endpoint and credentials
        string blobServiceEndpoint = "https://<your_account>.blob.core.windows.net";
        string connectionString = "<your_connection_string>";

        // Create a BlobServiceClient
        var serviceClient = new BlobServiceClient(connectionString);

        // Create a BlobChangeFeedClient
        var changeFeedClient = new BlobChangeFeedClient(serviceClient);

        // Read change feed events
        await foreach (var changeFeedEvent in changeFeedClient.GetChangesAsync())
        {
            Console.WriteLine($"Event Type: {changeFeedEvent.EventType}");
            Console.WriteLine($"Subject: {changeFeedEvent.Subject}");
            Console.WriteLine($"Event Time: {changeFeedEvent.EventTime}");
            Console.WriteLine("---");
        }
    }
}
