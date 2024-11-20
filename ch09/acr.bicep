resource acr 'Microsoft.ContainerRegistry/registries@2023-07-01' = {
  name: 'afd09cr2'
  location: resourceGroup().location
  sku: {
    name: 'Basic'
  }
  properties: {
    adminUserEnabled: true
  }
}

resource acrpush 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid('acrpush', acr.id)
  scope: acr
  properties: {
    principalId: '326fd6c4-7296-4637-9f1d-10876158a6d2'
    roleDefinitionId: resourceId('Microsoft.Authorization/roleDefinitions', '8311e382-0749-4cb8-b61a-304f252e45ec')
  }
}

resource acrpull 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid('acrpull', acr.id)
  scope: acr
  properties: {
    principalId: '326fd6c4-7296-4637-9f1d-10876158a6d2'
    roleDefinitionId: resourceId('Microsoft.Authorization/roleDefinitions', '7f951dda-4ed3-4680-a7ca-43fe172d538d')
  }
}
