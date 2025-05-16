# Python example: Azure Service Bus - Send and Receive Message
# Requires: pip install azure-servicebus azure-identity

from azure.servicebus import ServiceBusClient, ServiceBusMessage
from azure.identity import DefaultAzureCredential

queue_name = "myqueue"
connection_string = "<your_connection_string>"
fully_qualified_namespace = "<your_servicebus_namespace>.servicebus.windows.net"
message = "Hello from Python!"

# 1. Using connection string
with ServiceBusClient.from_connection_string(connection_string) as client:
    sender = client.get_queue_sender(queue_name)
    with sender:
        sender.send_messages(ServiceBusMessage(message))
    receiver = client.get_queue_receiver(queue_name)
    with receiver:
        for msg in receiver.receive_messages(max_message_count=1, max_wait_time=5):
            print(f"Received (conn str): {str(msg)}")
            receiver.complete_message(msg)

# 2. Using Entra ID (DefaultAzureCredential)
with ServiceBusClient(fully_qualified_namespace, credential=DefaultAzureCredential()) as client:
    sender = client.get_queue_sender(queue_name)
    with sender:
        sender.send_messages(ServiceBusMessage(message))
    receiver = client.get_queue_receiver(queue_name)
    with receiver:
        for msg in receiver.receive_messages(max_message_count=1, max_wait_time=5):
            print(f"Received (Entra ID): {str(msg)}")
            receiver.complete_message(msg)
