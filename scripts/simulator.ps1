param
(
 [Parameter(Mandatory=$true)]	
 [string]$COSMOSDBEP,

 [Parameter(Mandatory=$true)]	
 [string]$CosmosDbSRT,


 [Parameter(Mandatory=$true)]
 [string]$AzureServiceBusCONN,

 [Parameter(Mandatory=$true)]
 [string]$ServiceBusAccesKey,

 [Parameter(Mandatory=$true)]
 [string]$AzureAdSRT,

 [Parameter(Mandatory=$true)]
 [string]$SignalCONN,

 [Parameter(Mandatory=$true)]
 [string]$ApplicationInsightsKey,

 [Parameter(Mandatory=$true)]
 [string]$IoTHubControllerSRT,

 [Parameter(Mandatory=$true)]
 [string]$BotAppPassword,

 [Parameter(Mandatory=$true)]
 [string]$BOTSECRETTOKEN,

 [Parameter(Mandatory=$true)]
 [string]$BotStorageep,

 
 [Parameter(Mandatory=$true)]
 [string]$NotificationHubSRT
 );

$folderName = (Get-Date).tostring("dd-MM-yyyyThh-mm-ss")            
New-Item -itemType Directory -Path C:\Tmp -Name $folderName

$githubUrl = "https://github.com/litebulb/ProjectEdison.git"
$location = "C:\Tmp\$folderName"
git clone $githubUrl $location
Start-Sleep -s 180
$path1 = "$location\Edison.Devices\Edison.Simulators.Sensors\appsettings.json"
$json = Get-Content $path1 | ConvertFrom-Json

$json.CosmosDb.Endpoint = $COSMOSDBEP
$json.CosmosDb.AuthKey = $CosmosDbSRT
$json.AzureServiceBus.ConnectionString = $AzureServiceBusCONN
$json.ServiceBusAzure.ConnectionString = $ServiceBusAccesKey
$json.RestService.AzureAd.ClientSecret = $AzureAdSRT
$json.SignalR.ConnectionString = $SignalCONN
$json.ApplicationInsights.InstrumentationKey = $ApplicationInsightsKey
$json.Simulator.IoTHubConnectionString = $IoTHubControllerSRT
$json.BotConfigOptions.MicrosoftAppPassword = $BotAppPassword
$json.BotConfigOptions.BotSecret = $BOTSECRETTOKEN
$json.BotConfigOptions.AzureStorageConnectionString = $BotStorageep
$json.NotificationHub.ConnectionString =$NotificationHubSRT

$json | ConvertTo-Json | Out-File C:\Tmp\$folderName