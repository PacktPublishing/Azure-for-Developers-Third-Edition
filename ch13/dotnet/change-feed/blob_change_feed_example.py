# Python example: Reading Azure Blob Storage Change Feed
# Requires: pip install azure-storage-blob-changefeed

from azure.storage.blob import BlobServiceClient
from azure.storage.blob.changefeed import BlobChangeFeedClient

# Replace with your Blob service connection string
connection_string = "<your_connection_string>"

# Create a BlobServiceClient
service_client = BlobServiceClient.from_connection_string(connection_string)

# Create a BlobChangeFeedClient
change_feed_client = BlobChangeFeedClient(service_client)

# Read change feed events
for event in change_feed_client.list_changes():
    print(f"Event Type: {event.event_type}")
    print(f"Subject: {event.subject}")
    print(f"Event Time: {event.event_time}")
    print("---")
