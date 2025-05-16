# Python example: Azure Queue Storage - Send and Receive Message
# Requires: pip install azure-storage-queue azure-identity

from azure.storage.queue import QueueClient
from azure.identity import DefaultAzureCredential

queue_name = "myqueue"
connection_string = "<your_connection_string>"
queue_uri = f"https://<your_account>.queue.core.windows.net/{queue_name}"
message = "Hello from Python!"

# 1. Using connection string
queue_client_conn_str = QueueClient.from_connection_string(connection_string, queue_name)
queue_client_conn_str.create_queue()
queue_client_conn_str.send_message(message)
msgs_conn_str = queue_client_conn_str.receive_messages(messages_per_page=1)
for msg in msgs_conn_str.by_page():
    for m in msg:
        print(f"Received (conn str): {m.content}")

# 2. Using Entra ID (DefaultAzureCredential)
queue_client_entra = QueueClient(queue_uri, credential=DefaultAzureCredential())
queue_client_entra.create_queue()
queue_client_entra.send_message(message)
msgs_entra = queue_client_entra.receive_messages(messages_per_page=1)
for msg in msgs_entra.by_page():
    for m in msg:
        print(f"Received (Entra ID): {m.content}")
