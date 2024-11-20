import os
import json
from azure.identity import AzureCliCredential
from azure.mgmt.resource import ResourceManagementClient
from azure.mgmt.resource.resources.models import DeploymentMode

credential = AzureCliCredential()
subscription_id = os.environ["AZURE_SUBSCRIPTION_ID"]

resource_client = ResourceManagementClient(credential, subscription_id)

with open("aci.json", "r") as template_file:
    template_body = json.load(template_file)

rg_deployment_result = resource_client.deployments.begin_create_or_update(
    "<resource_group_name>",
    "<deployment_name>",
    {
        "properties": {
            "template": template_body,
            "parameters": {
                "parCustomerId": {
                    "value": "18363875"
                },
            },
            "mode": DeploymentMode.incremental
        }
    }
)