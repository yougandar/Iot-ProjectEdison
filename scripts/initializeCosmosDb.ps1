
$tenantId="dcf9e4d3-f44a-4c28-be12-8245c0d35668"
$azureAccountName="nvtuluva@sysgain.com"
$azurePassword="ush1b@123456"
$cosmosDBAccountKey="cciNaze7pqnUYPYZ30rg9sGduiIWHR5qEtdATWi4uXT20SpAEcFjltYckUTPHO31oYTFIJYJnic5k3qaZ2EIAQ=="
$cosmosDbAccountName="edisondb"
$cosmosDbName="edison"
$adminportalUri="https://edisonadmin.cloudapp.azure.com"
$objectId="277b4abb-f68a-462e-b04d-ef2f4e3a5337"
      
 
    Set-ExecutionPolicy -ExecutionPolicy RemoteSigned  -Force
    $password = ConvertTo-SecureString $azurePassword -AsPlainText -Force
    $psCred = New-Object System.Management.Automation.PSCredential($azureAccountName, $password)
    start-Sleep -s 20
    Login-AzureRmAccount -TenantId $tenantId -Credential $psCred 
    start-Sleep -s 20
 
    $primaryKey = ConvertTo-SecureString -String $cosmosDBAccountKey -AsPlainText -Force
    $cosmosDbContext = New-CosmosDbContext -Account $cosmosDbAccountName -Database $cosmosDbName -Key $primaryKey
    start-Sleep -s 20
 
    # Create CosmosDB
    New-CosmosDbDatabase -Context $cosmosDbContext -Id $cosmosDbName
    start-Sleep -s 30
 
    # Create CosmosDB Collections
    New-CosmosDbCollection -Context $cosmosDbContext -Id 'EventClusters'  -OfferThroughput 400
    start-Sleep -s 20
    New-CosmosDbCollection -Context $cosmosDbContext -Id 'Responses' -OfferThroughput 400
    start-Sleep -s 20
    New-CosmosDbCollection -Context $cosmosDbContext -Id 'Devices' -OfferThroughput 400
    start-Sleep -s 30
    New-CosmosDbCollection -Context $cosmosDbContext -Id 'ActionPlans' -OfferThroughput 400
    start-Sleep -s 30
    New-CosmosDbCollection -Context $cosmosDbContext -Id 'Notifications' -OfferThroughput 400
    start-Sleep -s 30
    New-CosmosDbCollection -Context $cosmosDbContext -Id 'Bot' -OfferThroughput 400
    start-Sleep -s 30
    New-CosmosDbCollection -Context $cosmosDbContext -Id 'ChatReports' -OfferThroughput 400
    start-Sleep -s 30
    New-CosmosDbCollection -Context $cosmosDbContext -Id 'Sagas' -OfferThroughput 400
    start-Sleep -s 30

    #Update Azure AD applications reply urls
    Connect-AzureAd -TenantId $tenantId -Credential $psCred -InformationAction Ignore
    $adminURL=$adminportalUri
    $replyURLList = @($adminURL);  
    Write-Host '', 'Configuring and setting the Azure AD reply URLs' -ForegroundColor Green
    Set-AzureADApplication -ObjectId $objectId -HomePage $adminportalUri -ReplyUrls $replyURLList -Verbose
  
