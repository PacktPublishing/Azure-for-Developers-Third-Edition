// Bicep file to deploy an Azure Logic App with HTTP trigger and JSON response
// Change these parameters as needed

@description('Name of the Logic App')
param logicAppName string = 'SimpleHttpWorkflow'

@description('Location for all resources.')
param location string = resourceGroup().location

// Define the Logic App resource
resource logicApp 'Microsoft.Logic/workflows@2019-05-01' = {
  name: logicAppName
  location: location
  properties: {
    state: 'Enabled'
    definition: {
      '$schema': 'https://schema.management.azure.com/providers/Microsoft.Logic/schemas/2016-06-01/workflowdefinition.json#'
      contentVersion: '1.0.0.0'
      parameters: {}
      triggers: {
        manual: {
          type: 'Request'
          kind: 'Http'
          inputs: {
            schema: {}
          }
        }
      }
      actions: {
        'Return_JSON_Response': {
          type: 'Response'
          kind: 'Http'
          inputs: {
            statusCode: 200
            headers: {
              'Content-Type': 'application/json'
            }
            body: {
              message: 'Hello from Logic App!'
              timestamp: '@{utcNow()}'
              requestMethod: '@{triggerOutputs()?.headers?.method}'
            }
          }
          runAfter: {}
        }
      }
      outputs: {}
    }
    parameters: {}
  }
}

// Output the Logic App URL for easy access
output logicAppUrl string = '${logicApp.properties.accessEndpoint}triggers/manual/invoke'
