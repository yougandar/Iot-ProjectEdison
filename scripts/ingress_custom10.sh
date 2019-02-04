GITPATH=`pwd`
GIT_DIRPATH="$GITPATH/ProjectEdison/Edison.Web/Kubernetes/qa/Deployment"
GIT_INGRESS="$GIT_DIRPATH/Ingress_Custom"
GIT_INGRESSCUSTOM="Ingress_Custom "
EXT_IP= head -3 services.txt | awk -F ' ' '{print $4}' | tail -1
LOG="/tmp/deployhelm.log.`date +%d%m%Y_%T`"

#1-InstallNGINXIngress

ls $GIT_INGRESS
if [ $? -eq 0 ]
then
        echo "------------------------------------" >> $LOG
        echo "The $GIT_INGRESS exists" >> $LOG
        cd $GIT_INGRESS

#  Install Helm

    curl https://raw.githubusercontent.com/helm/helm/master/scripts/get > get_helm.sh
    chmod +x get_helm.sh
    ./get_helm.sh

# Run helm init for first time on your cluster

    helm init 
    sleep 10
    helm install --name nginx-ingress-admin stable/nginx-ingress --namespace kube-system --set rbac.create=false --set rbac.createRole=false --set rbac.createClusterRole=false --set controller.ingressClass=nginx-admin
    sleep 10
    helm install --name nginx-ingress-api stable/nginx-ingress --namespace kube-system --set rbac.create=false --set rbac.createRole=false --set rbac.createClusterRole=false --set controller.ingressClass=nginx-api
    sleep 20

#2-InstallIngressControllers

    kubectl create -f ./nginx-config-adminportal.yaml
    sleep 20s
    kubectl create -f ./nginx-config-api.yaml
    sleep 20s

#3-ConfigureDns 
    # Public IP address
        IP= "$EXT_IP"

    # Name to associate with public IP address
        DNSNAME="edisoningress"

    # Get the resource-id of the public ip
        PUBLICIPID=$(az network public-ip list --query "[?ipAddress!=null]|[?contains(ipAddress, '$IP')].[id]" --output tsv)

    # Update public ip address with dns name
        az network public-ip update --ids $PUBLICIPID --dns-name $DNSNAME

else
        echo "------------------------------------" >> $LOG
        echo "The $GIT_INGRESS doesn't exist " >> $LOG
fi