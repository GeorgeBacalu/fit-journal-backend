targetScope = 'resourceGroup'

@minLength(3)
param name string
param location string = resourceGroup().location
param tags object = {}

var suffix = take(uniqueString(subscription().id, resourceGroup().id, name), 6)
var accountName = toLower(take('oai-${name}-${suffix}', 64))
var deploymentName = 'gpt-4.1-mini'

resource openAi 'Microsoft.CognitiveServices/accounts@2024-10-01' = {
  name: accountName
  location: location
  kind: 'OpenAI'
  tags: tags
  sku: {
    name: 'S0'
  }
  properties: {
    customSubDomainName: accountName
    disableLocalAuth: true
    publicNetworkAccess: 'Enabled'
    networkAcls: {
      defaultAction: 'Allow'
    }
  }
}

resource modelDeployment 'Microsoft.CognitiveServices/accounts/deployments@2024-10-01' = {
  parent: openAi
  name: deploymentName
  sku: {
    name: 'GlobalStandard'
    capacity: 10
  }
  properties: {
    model: {
      format: 'OpenAI'
      name: 'gpt-4.1-mini'
      version: '2025-04-14'
    }
    raiPolicyName: 'Microsoft.DefaultV2'
    versionUpgradeOption: 'OnceCurrentVersionExpired'
  }
}

output openAiAccountName string = openAi.name
output openAiEndpoint string = openAi.properties.endpoint
output openAiDeploymentName string = modelDeployment.name
