# Python example: Azure Service Bus - Add SQL, Boolean, and Correlation Filters
# Requires: pip install azure-servicebus azure-identity

from azure.servicebus.management import ServiceBusAdministrationClient, SqlRuleFilter, TrueRuleFilter, FalseRuleFilter, CorrelationRuleFilter
from azure.identity import DefaultAzureCredential

connection_string = "<your_connection_string>"
fully_qualified_namespace = "<your_servicebus_namespace>.servicebus.windows.net"
topic_name = "mytopic"
subscription_name = "mysubscription"

# 1. Using connection string
admin_client_conn_str = ServiceBusAdministrationClient.from_connection_string(connection_string)

# SQL Filter
admin_client_conn_str.create_rule(topic_name, subscription_name, "SqlFilterRule", filter=SqlRuleFilter("Color = 'Red'"))
print("Added SQL filter: Color = 'Red'")

# Complex SQL Filter
admin_client_conn_str.create_rule(topic_name, subscription_name, "ComplexSqlFilterRule", filter=SqlRuleFilter("(Color = 'Red' OR Color = 'Blue') AND Quantity > 10 AND sys.Label = 'Important'"))
print("Added complex SQL filter: (Color = 'Red' OR Color = 'Blue') AND Quantity > 10 AND sys.Label = 'Important'")

# Boolean Filter (True/False)
admin_client_conn_str.create_rule(topic_name, subscription_name, "TrueFilterRule", filter=TrueRuleFilter())
admin_client_conn_str.create_rule(topic_name, subscription_name, "FalseFilterRule", filter=FalseRuleFilter())
print("Added Boolean filters: True and False")

# Correlation Filter
admin_client_conn_str.create_rule(topic_name, subscription_name, "CorrelationFilterRule", filter=CorrelationRuleFilter(correlation_id="my-correlation-id"))
print("Added Correlation filter: CorrelationId = 'my-correlation-id'")

# 2. Using Entra ID (DefaultAzureCredential)
admin_client_entra = ServiceBusAdministrationClient(fully_qualified_namespace, credential=DefaultAzureCredential())

admin_client_entra.create_rule(topic_name, subscription_name, "SqlFilterRule", filter=SqlRuleFilter("Color = 'Red'"))
admin_client_entra.create_rule(topic_name, subscription_name, "TrueFilterRule", filter=TrueRuleFilter())
admin_client_entra.create_rule(topic_name, subscription_name, "FalseFilterRule", filter=FalseRuleFilter())
admin_client_entra.create_rule(topic_name, subscription_name, "CorrelationFilterRule", filter=CorrelationRuleFilter(correlation_id="my-correlation-id"))
admin_client_entra.create_rule(topic_name, subscription_name, "ComplexSqlFilterRule", filter=SqlRuleFilter("(Color = 'Red' OR Color = 'Blue') AND Quantity > 10 AND sys.Label = 'Important'"))
print("Added all filters using Entra ID, including complex SQL filter")
