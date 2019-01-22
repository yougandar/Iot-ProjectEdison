#!/bin/bash
#Comment - connect to the cluster
#Author - komali
#------------------------------------
TenantId=$1
AzureUserId=$2
AzurePwd=$3
SubscriptionId=$4
ResourceGroupName=$5
ClusterName=$6
az login --tenant $TenantId --username $AzureUserId --password $AzurePwd
az account set -s $SubscriptionId
echo "Use Url http://localhost:8001/api/v1/namespaces/kube-system/services/kubernetes-dashboard/proxy/#"
az aks get-credentials --resource-group $ResourceGroupName --name $ClusterName
kubectl proxy
