#!/bin/bash
#Comment - connect to the cluster
#------------------------------------
TenantId=`head -32 input.txt | awk -F "\"" '{print $2}'| tail -1`
AzureUserId=`head -33 input.txt | awk -F "\"" '{print $2}'| tail -1`
AzurePwd=`head -34 input.txt | awk -F "\"" '{print $2}'| tail -1`
SubscriptionId=`head -35 input.txt | awk -F "\"" '{print $2}'| tail -1`
ResourceGroupName=`head -36 input.txt | awk -F "\"" '{print $2}'| tail -1`
ClusterName=`head -37 input.txt | awk -F "\"" '{print $2}'| tail -1`
#-----------------------------------------------------------
GITPATH=`pwd`
GIT_DIRPATH="$GITPATH/ProjectEdison/Edison.Web/Kubernetes/qa/Config"
LOG="/tmp/Config.log.`date +%d%m%Y_%T`"
#--------------------------------------------------------
az login --tenant $TenantId --username $AzureUserId --password $AzurePwd
az account set -s $SubscriptionId
echo "Use Url http://localhost:8001/api/v1/namespaces/kube-system/services/kubernetes-dashboard/proxy/#"
az aks get-credentials --resource-group $ResourceGroupName --name $ClusterName
kubectl proxy
sleep 20s
# move the kubernetes script to config path for execution
ls $GIT_DIRPATH
if [ $? -eq 0 ]
then
        echo "------------------------------------" >> $LOG
        echo "The $GIT_DIRPATH exists" >> $LOG
        mv set-kubernetes-config8.sh $GIT_DIRPATH
        cd $GIT_DIRPATH
else
        echo "------------------------------------" >> $LOG
        echo "The $GIT_DIRPATH doesn't exist " >> $LOG
fi


