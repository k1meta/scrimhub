targetScope = 'subscription'

@description('Azure region. Default is West Europe, near the Balkans.')
param location string = 'westeurope'

@description('Short name used for the resource group and shared resources.')
param appName string = 'scrimhub'

@minLength(1)
param sqlAdminLogin string

@secure()
@minLength(8)
param sqlAdminPassword string

param apiAppName string = 'scrimhub-api'
param realtimeAppName string = 'scrimhub-realtime'
param webAppName string = 'scrimhub-web'
param appServiceSkuName string = 'B1'
param appServiceSkuTier string = 'Basic'
param apiLinuxFxVersion string = 'DOTNETCORE|10.0'

@description('Confirm this stack in the portal before deploy. Node 26 may not be listed yet.')
param realtimeLinuxFxVersion string = 'NODE|22-lts'

resource rg 'Microsoft.Resources/resourceGroups@2024-03-01' = {
  name: '${appName}-rg'
  location: location
}

module resources 'resources.bicep' = {
  name: 'resources'
  scope: rg
  params: {
    location: location
    appName: appName
    sqlAdminLogin: sqlAdminLogin
    sqlAdminPassword: sqlAdminPassword
    apiAppName: apiAppName
    realtimeAppName: realtimeAppName
    webAppName: webAppName
    appServiceSkuName: appServiceSkuName
    appServiceSkuTier: appServiceSkuTier
    apiLinuxFxVersion: apiLinuxFxVersion
    realtimeLinuxFxVersion: realtimeLinuxFxVersion
  }
}

output resourceGroupName string = rg.name
output apiHostName string = resources.outputs.apiHostName
output realtimeHostName string = resources.outputs.realtimeHostName
output webHostName string = resources.outputs.webHostName
output keyVaultName string = resources.outputs.keyVaultName
output sqlServerName string = resources.outputs.sqlServerName
