param parLocation string = resourceGroup().location
param parTimestamp string = utcNow()
param parCustomerId string

resource aci 'Microsoft.ContainerInstance/containerGroups@2021-03-01' = {
  name: 'aci-${parTimestamp}'
  location: parLocation
  properties: {
    containers: [
      {
        name: 'myjob'
        properties: {
          image: '<acr-name>.azurecr.io/<image-name>:latest'
          environmentVariables: [
            {
              name: 'customerId'
              value: parCustomerId
            }
          ]
          resources: {
            requests: {
              cpu: 1 
              memoryInGB: 3
            }
          }
        }
      }
    ]
    restartPolicy: 'Always'
    osType: 'Linux'
  }
}
