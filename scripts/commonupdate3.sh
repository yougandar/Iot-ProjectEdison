#!/bin/bash
#Comment - Installs the required packages for building images
#Author - Vivek

#------------------------------------
CosmosDbSRT=$1
AzureServiceBusCONN=$2
ServiceBusRabbitMQUSR=$3
ServiceBusRabbitMQPWD=$4
AzureAdSRT=$5
SignalCONN=$6
IoTHubControllerSRT=$7
NotificationHubSRT=$8
ApplicationInsightsKey=$9
BotAppPassword=${10}
BotStorageep=${11}
TuserName=${12}
Tpassword=${13}
TauthToken=${14}

#------------------------------------
export CosmosDbSRT
export AzureServiceBusCONN
export ServiceBusRabbitMQUSR
export ServiceBusRabbitMQPWD
export AzureAdSRT
export SignalCONN
export IoTHubControllerSRT
export NotificationHubSRT
export ApplicationInsightsKey
export BotAppPassword
export BotStorageep
export TuserName
export Tpassword
export TauthToken
#------------------------------------
GITPATH=`pwd`
GIT_DIRPATH="$GITPATH/ProjectEdison"
GIT_DIRCONFIGPATH="$GIT_DIRPATH/Edison.Web/Kubernetes/qa/Config/Secrets"
LOG="/tmp/common.log.`date +%d%m%Y_%T`"
#------------------------------------
echo $AzureServiceBusCONN
ls $GIT_DIRPATH
if [ $? -eq 0 ]
then
        echo "------------------------------------" >> $LOG
        echo "The $GIT_DIRPATH exists" >> $LOG
        cd $GIT_DIRCONFIGPATH
        cat common.json | jq '.CosmosDb.AuthKey=env.CosmosDbSRT' | jq '.AzureServiceBus.ConnectionString=env.AzureServiceBusCONN' | jq '.ServiceBusRabbitMQ.Username=env.ServiceBusRabbitMQUSR' | jq '.ServiceBusRabbitMQ.Password=env.ServiceBusRabbitMQPWD' | jq '.RestService.AzureAd.ClientSecret=env.AzureAdSRT' | jq '.SignalR.ConnectionString=env.SignalCONN' | jq '.IoTHubController.IoTHubConnectionString=env.IoTHubControllerSRT' | jq '.NotificationHub.ConnectionString=env.NotificationHubSRT' | jq '.ApplicationInsights.InstrumentationKey=env.ApplicationInsightsKey' | jq '.Bot.MicrosoftAppPassword=env.BotAppPassword' | jq '.Bot.AzureStorageConnectionString=env.BotStorageep' | jq '.Twilio.UserName=env.TuserName' | jq '.Twilio.Password=env.Tpassword' | jq '.Twilio.AuthToken=env.TauthToken' > $GIT_DIRCONFIGPATH/common.secrets
else
        echo "------------------------------------" >> $LOG
        echo "The $GIT_DIRPATH doesn't exists" >> $LOG
fi
