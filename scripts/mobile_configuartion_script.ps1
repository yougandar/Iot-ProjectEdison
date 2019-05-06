#store the variables to clone github repository
#$githubUrl = "https://github.com/litebulb/ProjectEdison.git"
#$location = "E:\ProjectEdison"
#$date= ((Get-Date).ToString('dd MMM yyyy hh_mm_ss' ))
#New-Item -ItemType Directory -Path "$location\IPE-$date"

$folderName = (Get-Date).tostring("dd-MM-yyyyThh-mm-ss")            
New-Item -itemType Directory -Path C:\Tmp -Name $folderName

#clone the github repository
$githubUrl = "https://github.com/litebulb/ProjectEdison.git"
$location = "C:\Tmp\$folderName"
git clone $githubUrl $location
Start-Sleep -s 180

$Path = "C:\Users\omanduri\Desktop\values.txt"
$values = Get-Content $Path | Out-String | ConvertFrom-StringData
$values.TENANTID
$values.DOMAIN
$values.SIGNUPSIGNINPOLICYID 
$values.AdAppName
$values.ADCLIENTID 
$values.BASEURL_VALUE
$values.notificationhubConnectionString
$values.notificationhubName
$values.B2CCLIENTID
$values.package


# Create variables to store the path and endpoints.
$path1 = "$location\Edison.Mobile\Edison.Mobile.Common\Edison.Mobile.Common\Auth\AuthConfig.cs"
$path2 = "$location\Edison.Mobile\Edison.Mobile.Common\Edison.Mobile.Common\Shared\Constants.cs"
$path3 = "$location\Edison.Mobile\Edison.Mobile.User.Client\Edison.Mobile.User.Client.Core\Shared\Constants.cs"
$path4 = "$location\Edison.Mobile\Edison.Mobile.User.Client\Droid\Shared\Constants.cs"
$path5 = "$location\Edison.Mobile\Edison.Mobile.User.Client\iOS\Shared\Constants.cs"
$path6 = "$location\Edison.Mobile\Edison.Mobile.Admin.Client\Edison.Mobile.Admin.Client.Core\Shared\DeviceConfig.cs"
$path7 = "$location\Edison.Mobile\Edison.Mobile.Admin.Client\Droid\Shared\Constants.cs"
$path8 = "$location\Edison.Mobile\Edison.Mobile.Admin.Client\iOS\Shared\Constants.cs"
$path9 = "$location\Edison.Mobile\Edison.Mobile.User.Client\Droid\Properties\AndroidManifest.xml"
$path10 = "$location\Edison.Mobile\Edison.Mobile.User.Client\iOS\Info.plist"
$path11 = "$location\Edison.Mobile\Edison.Mobile.Admin.Client\Droid\Properties\AndroidManifest.xml"


# Update Edison.mobile.common\AuthConfig.cs file values
(Get-Content -path $path1 -Raw) -replace '1114b48d-24b1-4492-970a-d07d610a741c', $values.TENANTID | Set-Content -Path $path1
(Get-Content -path $path1 -Raw) -replace 'edisondevb2c', $values.DOMAIN | Set-Content -Path $path1
(Get-Content -path $path1 -Raw) -replace 'b2c_1_edision_signinandsignup', $values.SIGNUPSIGNINPOLICYID | Set-Content -Path $path1
(Get-Content -path $path1 -Raw) -replace 'edisonadmin', $values.AdAppName | Set-Content -Path $path1
(Get-Content -path $path1 -Raw) -replace '2373be1e-6d0b-4e38-9115-e0bd01dadd61', $values.ADCLIENTID | Set-Content -Path $path1

# Update Edison.Mobile.Common\constants.cs file values
(Get-Content -path $path2 -Raw) -replace 'edisonapidev.eastus.cloudapp.azure.com', $values.BASEURL_VALUE | Set-Content -Path $path2

# Update Edison.Mobile.User.Client.Core\Constants.cs file values
(Get-Content -path $path3 -Raw) -replace 'Endpoint=sb://edisondev.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=CNCM1xn79hHuUUj6GiAct1JJe5kdzGuPmzBOaVoSGsA=', $values.notificationhubConnectionString | Set-Content -Path $path3
(Get-Content -path $path3 -Raw) -replace 'edisondevnotificationhub', $values.notificationhubName | Set-Content -Path $path3

# Update Edison.Mobile.User.Client.Droid\Constants.cs file values
(Get-Content -path $path4 -Raw) -replace '19cb746c-3066-4cd8-8cd2-e0ce1176ae33', $values.B2CCLIENTID | Set-Content -Path $path4

# Update Edison.Mobile.User.Client.iOS\Constants.cs file values
(Get-Content -path $path5 -Raw) -replace '19cb746c-3066-4cd8-8cd2-e0ce1176ae33', $values.B2CCLIENTID | Set-Content -Path $path5

# Update Edison.Mobile.Admin.Client.Core\DeviceConfig.cs file values
(Get-Content -path $path6 -Raw) -replace 'edisonapidev.eastus.cloudapp.azure.com', $values.BASEURL_VALUE | Set-Content -Path $path6

# Update Edison.Mobile.Admin.Client.Droid\Constants.cs file values
(Get-Content -path $path7 -Raw) -replace '2373be1e-6d0b-4e38-9115-e0bd01dadd61', $values.ADCLIENTID| Set-Content -Path $path7

# Update Edison.Mobile.Admin.Client.iOS\Constants.cs file values
(Get-Content -path $path8 -Raw) -replace '2373be1e-6d0b-4e38-9115-e0bd01dadd61', $values.ADCLIENTID| Set-Content -Path $path8

# Update Edison.Mobile.User.Client.Droid\AndroidManifest.xml file values
(Get-Content -path $path9 -Raw) -replace 'edisondevb2c', $values.DOMAIN | Set-Content -Path $path9
(Get-Content -path $path9 -Raw) -replace 'com.bluemetal.Edison_Mobile_User_Client', $values.package  | Set-Content -Path $path9

# Update Edison.Mobile.User.Client.iOS\Info.plist file values
(Get-Content -path $path10 -Raw) -replace 'edisondevb2c', $values.DOMAIN | Set-Content -Path $path10

# Update Edison.Mobile.Admin.Client.Droid\AndroidManifest.xml file values 
(Get-Content -path $path11 -Raw) -replace '2373be1e-6d0b-4e38-9115-e0bd01dadd61', $values.ADCLIENTID | Set-Content -Path $path11
(Get-Content -path $path11 -Raw) -replace 'edisonbluemetal', $values.DOMAIN | Set-Content -Path $path11