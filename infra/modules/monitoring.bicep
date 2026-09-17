targetScope = 'resourceGroup'

@minLength(3)
param name string
param location string = resourceGroup().location
param tags object = {}

var suffix = take(uniqueString(subscription().id, resourceGroup().id, name), 6)

resource workspace 'Microsoft.OperationalInsights/workspaces@2023-09-01' = {
  name: 'log-${name}-${suffix}'
  location: location
  tags: tags
  properties: {
    retentionInDays: 30
    sku: {
      name: 'PerGB2018'
    }
    features: {
      enableLogAccessUsingOnlyResourcePermissions: true
    }
  }
}

resource applicationInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: 'appi-${name}-${suffix}'
  location: location
  kind: 'web'
  tags: tags
  properties: {
    Application_Type: 'web'
    WorkspaceResourceId: workspace.id
    IngestionMode: 'LogAnalytics'
    DisableIpMasking: false
    RetentionInDays: 30
  }
}

output logAnalyticsWorkspaceId string = workspace.id
output applicationInsightsConnectionString string = applicationInsights.properties.ConnectionString
