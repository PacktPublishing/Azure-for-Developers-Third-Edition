# Python example: Connecting to Azure Table Storage using Connection String, SAS Token, and Managed Identity
# Requires: pip install azure-data-tables azure-identity

from azure.data.tables import TableServiceClient
from azure.core.credentials import AzureSasCredential
from azure.identity import DefaultAzureCredential

# 1. Connect using a connection string
connection_string = "<your_connection_string>"
table_name = "myTable"
service_client_conn_str = TableServiceClient.from_connection_string(conn_str=connection_string)
table_client_conn_str = service_client_conn_str.get_table_client(table_name)
print(f"Connected to Table Storage using connection string. Table: {table_name}")

# 2. Connect using a SAS token
storage_uri = "https://<your_account>.table.core.windows.net"
sas_token = "<your_sas_token>"
service_client_sas = TableServiceClient(endpoint=storage_uri, credential=AzureSasCredential(sas_token))
table_client_sas = service_client_sas.get_table_client(table_name)
print(f"Connected to Table Storage using SAS token. Table: {table_name}")

# 3. Connect using Managed Identity (DefaultAzureCredential)
# Make sure your environment supports Managed Identity (e.g., Azure VM, App Service, etc.)
service_client_msi = TableServiceClient(endpoint=storage_uri, credential=DefaultAzureCredential())
table_client_msi = service_client_msi.get_table_client(table_name)
print(f"Connected to Table Storage using Managed Identity. Table: {table_name}")
