#store the variables to clone github repository
$githubUrl = "https://github.com/litebulb/ProjectEdison.git"
$location = "E:\ProjectEdison"
$date= ((Get-Date).ToString('dd MMM yyyy hh_mm_ss' ))
New-Item -ItemType Directory -Path "$location\IPE-$date"

#clone the github repository
git clone $githubUrl $location\IPE-$date

# Create variables to store the path and endpoints.
$path1 = "$location\IPE-$date\Edison.Mobile\Edison.Mobile.Common\Edison.Mobile.Common\Auth\AuthConfig.cs"
$path2 = "$location\IPE-$date\Edison.Mobile\Edison.Mobile.Common\Edison.Mobile.Common\Shared\Constants.cs"
$path3 = "$location\IPE-$date\Edison.Mobile\Edison.Mobile.User.Client\Edison.Mobile.User.Client.Core\Shared\Constants.cs"
$path4 = "$location\IPE-$date\Edison.Mobile\Edison.Mobile.User.Client\Droid\Shared\Constants.cs"
$path5 = "$location\IPE-$date\Edison.Mobile\Edison.Mobile.User.Client\iOS\Shared\Constants.cs"
$path6 = "$location\IPE-$date\Edison.Mobile\Edison.Mobile.Admin.Client\Edison.Mobile.Admin.Client.Core\Shared\DeviceConfig.cs"
$path7 = "$location\IPE-$date\Edison.Mobile\Edison.Mobile.Admin.Client\Droid\Shared\Constants.cs"
$path8 = "$location\IPE-$date\Edison.Mobile\Edison.Mobile.Admin.Client\iOS\Shared\Constants.cs"
$path9 = "$location\IPE-$date\Edison.Mobile\Edison.Mobile.User.Client\Droid\Properties\AndroidManifest.xml"
$path10 = "$location\IPE-$date\Edison.Mobile\Edison.Mobile.User.Client\iOS\Info.plist"
$path11 = "$location\IPE-$date\Edison.Mobile\Edison.Mobile.Admin.Client\Droid\Properties\AndroidManifest.xml"
$adminTenant = "016c0a8a-388e-4fe5-a363-7a3914d2006d"
$userTenant = "newiotp2"
$userPolicy = "B2C_1_signup_signin"
$adAppName = "adminedison"
$adAppId = "a9b2dc1f-1051-4d8b-88df-9f96fad0cbdc"
$apiUri = "botapi.qloudable-npr.com"
$notificationhubConnectionString = "Endpoint=sb://notificationhubnsmchq.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=Qt2AgkORUDNnKbbte7OKImQ+/JevjpsPRt8z2lSLMEg="
$notificationhubName = "NotificationHubmchq"
$userAppId = "7c519e29-9718-4e9e-8fed-5253206dff9b"
$package = "com.sysgain.smartbuildings"

# Update Edison.mobile.common\AuthConfig.cs file values
(Get-Content -path $path1 -Raw) -replace '1114b48d-24b1-4492-970a-d07d610a741c', $adminTenant | Set-Content -Path $path1
(Get-Content -path $path1 -Raw) -replace 'edisondevb2c', $userTenant | Set-Content -Path $path1
(Get-Content -path $path1 -Raw) -replace 'b2c_1_edision_signinandsignup', $userPolicy | Set-Content -Path $path1
(Get-Content -path $path1 -Raw) -replace 'edisonadmin', $adAppName | Set-Content -Path $path1
(Get-Content -path $path1 -Raw) -replace '2373be1e-6d0b-4e38-9115-e0bd01dadd61', $adAppId | Set-Content -Path $path1

# Update Edison.Mobile.Common\constants.cs file values
(Get-Content -path $path2 -Raw) -replace 'edisonapidev.eastus.cloudapp.azure.com', $apiUri | Set-Content -Path $path2

# Update Edison.Mobile.User.Client.Core\Constants.cs file values
(Get-Content -path $path3 -Raw) -replace 'Endpoint=sb://edisondev.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=CNCM1xn79hHuUUj6GiAct1JJe5kdzGuPmzBOaVoSGsA=', $notificationhubConnectionString | Set-Content -Path $path3
(Get-Content -path $path3 -Raw) -replace 'edisondevnotificationhub', $notificationhubName | Set-Content -Path $path3

# Update Edison.Mobile.User.Client.Droid\Constants.cs file values
(Get-Content -path $path4 -Raw) -replace '19cb746c-3066-4cd8-8cd2-e0ce1176ae33', $userAppId | Set-Content -Path $path4

# Update Edison.Mobile.User.Client.iOS\Constants.cs file values
(Get-Content -path $path5 -Raw) -replace '19cb746c-3066-4cd8-8cd2-e0ce1176ae33', $userAppId | Set-Content -Path $path5

# Update Edison.Mobile.Admin.Client.Core\DeviceConfig.cs file values
(Get-Content -path $path6 -Raw) -replace 'edisonapidev.eastus.cloudapp.azure.com', $apiUri | Set-Content -Path $path6

# Update Edison.Mobile.Admin.Client.Droid\Constants.cs file values
(Get-Content -path $path7 -Raw) -replace '2373be1e-6d0b-4e38-9115-e0bd01dadd61', $adAppId | Set-Content -Path $path7

# Update Edison.Mobile.Admin.Client.iOS\Constants.cs file values
(Get-Content -path $path8 -Raw) -replace '2373be1e-6d0b-4e38-9115-e0bd01dadd61', $adAppId | Set-Content -Path $path8

# Update Edison.Mobile.User.Client.Droid\AndroidManifest.xml file values
(Get-Content -path $path9 -Raw) -replace 'edisondevb2c', $userTenant | Set-Content -Path $path9
(Get-Content -path $path9 -Raw) -replace 'com.bluemetal.Edison_Mobile_User_Client', $package | Set-Content -Path $path9

# Update Edison.Mobile.User.Client.iOS\Info.plist file values
(Get-Content -path $path10 -Raw) -replace 'edisondevb2c', $userTenant | Set-Content -Path $path10

# Update Edison.Mobile.Admin.Client.Droid\AndroidManifest.xml file values 
(Get-Content -path $path11 -Raw) -replace '2373be1e-6d0b-4e38-9115-e0bd01dadd61', $adAppId | Set-Content -Path $path11
(Get-Content -path $path11 -Raw) -replace 'edisonbluemetal', $userTenant | Set-Content -Path $path11