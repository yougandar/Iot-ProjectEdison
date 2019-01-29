#!/bin/bash
#Comment - connect to the cluster
#Author - komali
#------------------------------------
TenantId=`head -32 input.txt | awk -F: '{print $2}'`
AzureUserId=`head -33 input.txt | awk -F: '{print $2}'`
AzurePwd=`head -34 input.txt | awk -F: '{print $2}'`
SubscriptionId=`head -35 input.txt | awk -F: '{print $2}'`
ResourceGroupName=`head -36 input.txt | awk -F: '{print $2}'`
ClusterName=`head -37 input.txt | awk -F: '{print $2}'`
az login --tenant $TenantId --username $AzureUserId --password $AzurePwd
az account set -s $SubscriptionId
echo "Use Url http://localhost:8001/api/v1/namespaces/kube-system/services/kubernetes-dashboard/proxy/#"
az aks get-credentials --resource-group $ResourceGroupName --name $ClusterName
kubectl proxy
