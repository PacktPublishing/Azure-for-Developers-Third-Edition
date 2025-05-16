# Python example: Azure Event Hub - Send and Receive Event
# Requires: pip install azure-eventhub azure-identity

from azure.eventhub import EventHubProducerClient, EventData, EventHubConsumerClient
from azure.identity import DefaultAzureCredential

connection_string = "<your_connection_string>"
eventhub_name = "myeventhub"
fully_qualified_namespace = "<your_eventhub_namespace>.servicebus.windows.net"
consumer_group = "$Default"
message = "Hello from Python!"

# 1. Using connection string
producer_conn_str = EventHubProducerClient.from_connection_string(conn_str=connection_string, eventhub_name=eventhub_name)
with producer_conn_str:
    event_data_batch = producer_conn_str.create_batch()
    event_data_batch.add(EventData(message))
    producer_conn_str.send_batch(event_data_batch)

consumer_conn_str = EventHubConsumerClient.from_connection_string(conn_str=connection_string, consumer_group=consumer_group, eventhub_name=eventhub_name)
received = False
def on_event_conn_str(partition_context, event):
    global received
    print(f"Received (conn str): {event.body_as_str()}")
    received = True
    consumer_conn_str.close()

with consumer_conn_str:
    consumer_conn_str.receive(on_event=on_event_conn_str, starting_position="-1", max_wait_time=5)

# 2. Using Entra ID (DefaultAzureCredential)
producer_entra = EventHubProducerClient(fully_qualified_namespace=fully_qualified_namespace, eventhub_name=eventhub_name, credential=DefaultAzureCredential())
with producer_entra:
    event_data_batch = producer_entra.create_batch()
    event_data_batch.add(EventData(message))
    producer_entra.send_batch(event_data_batch)

consumer_entra = EventHubConsumerClient(fully_qualified_namespace=fully_qualified_namespace, eventhub_name=eventhub_name, consumer_group=consumer_group, credential=DefaultAzureCredential())
received = False
def on_event_entra(partition_context, event):
    global received
    print(f"Received (Entra ID): {event.body_as_str()}")
    received = True
    consumer_entra.close()

with consumer_entra:
    consumer_entra.receive(on_event=on_event_entra, starting_position="-1", max_wait_time=5)
