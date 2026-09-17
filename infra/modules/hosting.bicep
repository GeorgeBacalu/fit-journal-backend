targetScope = 'resourceGroup'

@minLength(3)
param name string
param location string = resourceGroup().location
param tags object = {}
param applicationInsightsConnectionString string
param logAnalyticsWorkspaceId string
param keyVaultName string
param storageAccountName string
param sqlServerName string
param sqlDatabaseName string
param openAiAccountName string
param openAiEndpoint string
param openAiDeploymentName string

var suffix = take(uniqueString(subscription().id, resourceGroup().id, name), 6)
var apiServiceName = 'api'
var webServiceName = 'web'
var apiAppName = toLower(take('app-${name}-api-${suffix}', 60))
var webAppName = toLower(take('app-${name}-web-${suffix}', 60))
var apiUrl = 'https://${apiAppName}.azurewebsites.net'
var webUrl = 'https://${webAppName}.azurewebsites.net'
var sqlConnectionString = 'Server=tcp:${sqlServerName}${environment().suffixes.sqlServerHostname},1433;Database=${sqlDatabaseName};Authentication=Active Directory Default;Encrypt=True;TrustServerCertificate=False;'

resource storage 'Microsoft.Storage/storageAccounts@2023-05-01' existing = {
  name: storageAccountName
}

resource keyVault 'Microsoft.KeyVault/vaults@2023-07-01' existing = {
  name: keyVaultName
}

resource openAi 'Microsoft.CognitiveServices/accounts@2024-10-01' existing = {
  name: openAiAccountName
}

resource plan 'Microsoft.Web/serverfarms@2023-12-01' = {
  name: 'asp-${name}-${suffix}'
  location: location
  kind: 'linux'
  tags: tags
  sku: {
    name: 'B1'
    tier: 'Basic'
    capacity: 1
  }
  properties: {
    reserved: true
  }
}

resource apiApp 'Microsoft.Web/sites@2023-12-01' = {
  name: apiAppName
  location: location
  kind: 'app,linux'
  tags: union(tags, { 'azd-service-name': apiServiceName })
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: plan.id
    httpsOnly: true
    clientAffinityEnabled: false
    publicNetworkAccess: 'Enabled'
    siteConfig: {
      linuxFxVersion: 'DOTNETCORE|10.0'
      alwaysOn: true
      ftpsState: 'Disabled'
      minTlsVersion: '1.2'
      http20Enabled: true
      healthCheckPath: '/health/ready'
      appSettings: [
        { name: 'ASPNETCORE_ENVIRONMENT', value: 'Production' }
        { name: 'WEBSITE_HEALTHCHECK_MAXPINGFAILURES', value: '3' }
        { name: 'APPLICATIONINSIGHTS_CONNECTION_STRING', value: applicationInsightsConnectionString }
        { name: 'ApplicationInsightsAgent_EXTENSION_VERSION', value: '~3' }
        { name: 'XDT_MicrosoftApplicationInsights_Mode', value: 'recommended' }
        { name: 'ConnectionStrings__FitJournalDb', value: sqlConnectionString }
        { name: 'Database__ApplyMigrations', value: 'true' }
        { name: 'Auth__Issuer', value: apiUrl }
        { name: 'Auth__Audience', value: apiUrl }
        { name: 'Auth__Secret', value: '@Microsoft.KeyVault(VaultName=${keyVaultName};SecretName=jwt-signing-secret)' }
        { name: 'Auth__AccessTokenLifetimeMinutes', value: '15' }
        { name: 'Auth__RefreshTokenLifetimeDays', value: '30' }
        { name: 'ExternalAuth__FrontendUrl', value: webUrl }
        { name: 'ExternalAuth__Google__ClientId', value: '@Microsoft.KeyVault(VaultName=${keyVaultName};SecretName=google-client-id)' }
        { name: 'ExternalAuth__Google__ClientSecret', value: '@Microsoft.KeyVault(VaultName=${keyVaultName};SecretName=google-client-secret)' }
        { name: 'ExternalAuth__Microsoft__ClientId', value: '@Microsoft.KeyVault(VaultName=${keyVaultName};SecretName=microsoft-client-id)' }
        { name: 'ExternalAuth__Microsoft__ClientSecret', value: '@Microsoft.KeyVault(VaultName=${keyVaultName};SecretName=microsoft-client-secret)' }
        { name: 'Email__User', value: '@Microsoft.KeyVault(VaultName=${keyVaultName};SecretName=smtp-user)' }
        { name: 'Email__Password', value: '@Microsoft.KeyVault(VaultName=${keyVaultName};SecretName=smtp-password)' }
        { name: 'Email__MailBoxName', value: '@Microsoft.KeyVault(VaultName=${keyVaultName};SecretName=smtp-mailbox-name)' }
        { name: 'Email__SmtpHost', value: 'smtp.gmail.com' }
        { name: 'Email__SmtpPort', value: '587' }
        { name: 'AzureStorage__ServiceUri', value: 'https://${storageAccountName}.blob.${environment().suffixes.storage}' }
        { name: 'AzureOpenAI__Endpoint', value: openAiEndpoint }
        { name: 'AzureOpenAI__Deployment', value: openAiDeploymentName }
      ]
    }
  }
}

resource webApp 'Microsoft.Web/sites@2023-12-01' = {
  name: webAppName
  location: location
  kind: 'app,linux'
  tags: union(tags, { 'azd-service-name': webServiceName })
  properties: {
    serverFarmId: plan.id
    httpsOnly: true
    clientAffinityEnabled: false
    publicNetworkAccess: 'Enabled'
    siteConfig: {
      linuxFxVersion: 'NODE|22-lts'
      alwaysOn: true
      ftpsState: 'Disabled'
      minTlsVersion: '1.2'
      http20Enabled: true
      healthCheckPath: '/'
      appCommandLine: 'pm2 serve /home/site/wwwroot --no-daemon --spa'
      appSettings: [
        { name: 'NODE_ENV', value: 'production' }
        { name: 'SCM_DO_BUILD_DURING_DEPLOYMENT', value: 'true' }
        { name: 'WEBSITE_NODE_DEFAULT_VERSION', value: '~22' }
        { name: 'WEBSITE_HEALTHCHECK_MAXPINGFAILURES', value: '3' }
        { name: 'APPLICATIONINSIGHTS_CONNECTION_STRING', value: applicationInsightsConnectionString }
      ]
    }
  }
}

resource storageBlobContributor 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(storage.id, apiApp.id, 'Storage Blob Data Contributor')
  scope: storage
  properties: {
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', 'ba92f5b4-2d11-453d-a403-e96b0029c9fe')
    principalId: apiApp.identity.principalId
    principalType: 'ServicePrincipal'
  }
}

resource keyVaultSecretsUser 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(keyVault.id, apiApp.id, 'Key Vault Secrets User')
  scope: keyVault
  properties: {
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', '4633458b-17de-408a-b874-0445c86b69e6')
    principalId: apiApp.identity.principalId
    principalType: 'ServicePrincipal'
  }
}

resource openAiUser 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(openAi.id, apiApp.id, 'Cognitive Services OpenAI User')
  scope: openAi
  properties: {
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', '5e0bd9bd-7b93-4f28-af87-19fc36ad61bd')
    principalId: apiApp.identity.principalId
    principalType: 'ServicePrincipal'
  }
}

resource apiDiagnostics 'Microsoft.Insights/diagnosticSettings@2021-05-01-preview' = {
  name: 'send-to-log-analytics'
  scope: apiApp
  properties: {
    workspaceId: logAnalyticsWorkspaceId
    logs: [
      { categoryGroup: 'allLogs', enabled: true }
    ]
    metrics: [
      { category: 'AllMetrics', enabled: true }
    ]
  }
}

resource webDiagnostics 'Microsoft.Insights/diagnosticSettings@2021-05-01-preview' = {
  name: 'send-to-log-analytics'
  scope: webApp
  properties: {
    workspaceId: logAnalyticsWorkspaceId
    logs: [
      { categoryGroup: 'allLogs', enabled: true }
    ]
    metrics: [
      { category: 'AllMetrics', enabled: true }
    ]
  }
}

output apiAppName string = apiApp.name
output webAppName string = webApp.name
output apiUrl string = apiUrl
output webUrl string = webUrl
