targetScope = 'subscription'

@minLength(3)
@maxLength(24)
param environmentName string
param location string
param principalId string
param principalName string
@allowed(['User', 'Group', 'Application'])
param principalType string = 'User'
@secure()
param jwtSecret string
@secure()
param googleClientId string
@secure()
param googleClientSecret string
@secure()
param microsoftClientId string
@secure()
param microsoftClientSecret string
@secure()
param smtpUser string
@secure()
param smtpPassword string
@secure()
param smtpMailboxName string
param gitLabOidcSubject string
param gitLabOidcIssuer string = 'https://gitlab.com'

var resourceSuffix = take(uniqueString(subscription().id, environmentName, location), 6)
var resourceGroupName = 'rg-${environmentName}'
var tags = {
  'azd-env-name': environmentName
  application: 'fit-journal'
  environment: environmentName
}

resource resourceGroup 'Microsoft.Resources/resourceGroups@2024-03-01' = {
  name: resourceGroupName
  location: location
  tags: tags
}

module monitoring './modules/monitoring.bicep' = {
  name: 'monitoring-${resourceSuffix}'
  scope: resourceGroup
  params: {
    name: environmentName
    location: location
    tags: tags
  }
}

module data './modules/data.bicep' = {
  name: 'data-${resourceSuffix}'
  scope: resourceGroup
  params: {
    name: environmentName
    location: location
    tags: tags
    principalId: principalId
    principalName: principalName
    principalType: principalType
    jwtSecret: jwtSecret
    googleClientId: googleClientId
    googleClientSecret: googleClientSecret
    microsoftClientId: microsoftClientId
    microsoftClientSecret: microsoftClientSecret
    smtpUser: smtpUser
    smtpPassword: smtpPassword
    smtpMailboxName: smtpMailboxName
  }
}

module ai './modules/ai.bicep' = {
  name: 'ai-${resourceSuffix}'
  scope: resourceGroup
  params: {
    name: environmentName
    location: location
    tags: tags
  }
}

module hosting './modules/hosting.bicep' = {
  name: 'hosting-${resourceSuffix}'
  scope: resourceGroup
  params: {
    name: environmentName
    location: location
    tags: tags
    applicationInsightsConnectionString: monitoring.outputs.applicationInsightsConnectionString
    logAnalyticsWorkspaceId: monitoring.outputs.logAnalyticsWorkspaceId
    keyVaultName: data.outputs.keyVaultName
    storageAccountName: data.outputs.storageAccountName
    sqlServerName: data.outputs.sqlServerName
    sqlDatabaseName: data.outputs.sqlDatabaseName
    openAiAccountName: ai.outputs.openAiAccountName
    openAiEndpoint: ai.outputs.openAiEndpoint
    openAiDeploymentName: ai.outputs.openAiDeploymentName
  }
}

module delivery './modules/delivery.bicep' = {
  name: 'delivery-${resourceSuffix}'
  scope: resourceGroup
  params: {
    name: environmentName
    location: location
    tags: tags
    apiAppName: hosting.outputs.apiAppName
    webAppName: hosting.outputs.webAppName
    gitLabOidcSubject: gitLabOidcSubject
    gitLabOidcIssuer: gitLabOidcIssuer
  }
}

output AZURE_RESOURCE_GROUP string = resourceGroup.name
output AZURE_LOCATION string = location
output APPLICATIONINSIGHTS_CONNECTION_STRING string = monitoring.outputs.applicationInsightsConnectionString
output AZURE_KEY_VAULT_NAME string = data.outputs.keyVaultName
output AZURE_STORAGE_ACCOUNT_NAME string = data.outputs.storageAccountName
output SQL_SERVER string = data.outputs.sqlServerName
output SQL_DATABASE string = data.outputs.sqlDatabaseName
output SQL_GRANT_DDLADMIN string = 'true'
output SERVICE_API_NAME string = hosting.outputs.apiAppName
output API_URL string = hosting.outputs.apiUrl
output WEB_URL string = hosting.outputs.webUrl
output AZURE_OPENAI_ENDPOINT string = ai.outputs.openAiEndpoint
output AZURE_OPENAI_DEPLOYMENT string = ai.outputs.openAiDeploymentName
output AZURE_CLIENT_ID string = delivery.outputs.deliveryClientId
output AZURE_TENANT_ID string = tenant().tenantId
output AZURE_SUBSCRIPTION_ID string = subscription().subscriptionId
