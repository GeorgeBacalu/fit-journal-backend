targetScope = 'resourceGroup'

@minLength(3)
param name string
param location string = resourceGroup().location
param tags object = {}
param apiAppName string
param webAppName string
param githubBackendSubject string
param githubFrontendSubject string

var suffix = take(uniqueString(subscription().id, resourceGroup().id, name), 6)
var websiteContributorRoleId = subscriptionResourceId('Microsoft.Authorization/roleDefinitions', 'de139f84-1756-47ae-9be6-808fbbe84772')

resource apiApp 'Microsoft.Web/sites@2023-12-01' existing = {
  name: apiAppName
}

resource webApp 'Microsoft.Web/sites@2023-12-01' existing = {
  name: webAppName
}

resource deliveryIdentity 'Microsoft.ManagedIdentity/userAssignedIdentities@2023-01-31' = {
  name: 'id-${name}-github-${suffix}'
  location: location
  tags: tags
}

resource githubBackendFederation 'Microsoft.ManagedIdentity/userAssignedIdentities/federatedIdentityCredentials@2023-01-31' = {
  parent: deliveryIdentity
  name: 'github-backend'
  properties: {
    audiences: [
      'api://AzureADTokenExchange'
    ]
    issuer: 'https://token.actions.githubusercontent.com'
    subject: githubBackendSubject
  }
}

resource githubFrontendFederation 'Microsoft.ManagedIdentity/userAssignedIdentities/federatedIdentityCredentials@2023-01-31' = {
  parent: deliveryIdentity
  name: 'github-frontend'
  properties: {
    audiences: [
      'api://AzureADTokenExchange'
    ]
    issuer: 'https://token.actions.githubusercontent.com'
    subject: githubFrontendSubject
  }
}

resource apiDeploymentRole 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(apiApp.id, deliveryIdentity.id, 'Website Contributor')
  scope: apiApp
  properties: {
    roleDefinitionId: websiteContributorRoleId
    principalId: deliveryIdentity.properties.principalId
    principalType: 'ServicePrincipal'
  }
}

resource webDeploymentRole 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(webApp.id, deliveryIdentity.id, 'Website Contributor')
  scope: webApp
  properties: {
    roleDefinitionId: websiteContributorRoleId
    principalId: deliveryIdentity.properties.principalId
    principalType: 'ServicePrincipal'
  }
}

output deliveryClientId string = deliveryIdentity.properties.clientId
output deliveryPrincipalId string = deliveryIdentity.properties.principalId
