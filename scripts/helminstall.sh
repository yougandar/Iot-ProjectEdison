GITPATH=`pwd`
GIT_DIRPATH="$GITPATH/ProjectEdison/Edison.Web/Kubernetes/qa/Deployment"
GIT_INGRESS="$GIT_DIRPATH/Ingress_Custom"
GIT_INGRESSCUSTOM="Ingress_Custom"
LOG="/tmp/deploy.log.`date +%d%m%Y_%T`"
TAG=`head -31 input.txt | awk -F "\"" '{print $2}'| tail -1`
ACR_SERVERNAME=`head -28 input.txt | awk -F "\"" '{print $2}'| tail -1`

if [ $? -eq 0 ]
then
        echo "------------------------------------" >> $LOG
        cd ..
        cd Deployment
        sed -i -e 's/latest/'$TAG'/g' edison.kubernetes.deploy.yaml
        sed -i -e 's/edisoncontainerregistry.azurecr.io/'$ACR_SERVERNAME'/g' edison.kubernetes.deploy.yaml
        kubectl create -f edison.kubernetes.deploy.yaml
        sleep 10s
        kubectl create -f edison.kubernetes.services.yaml
        sleep 10s
        kubectl get svc -n kube-system
        sleep 10
        kubectl get pods
        kubectl get svc
       
else
        echo "------------------------------------" >> $LOG

fi
#-----------------------------------------------------------------------------------
ls $GIT_INGRESS
if [ $? -eq 0 ]
then
        echo "------------------------------------" >> $LOG
        echo "The $GIT_INGRESS exists" >> $LOG
        cd $GIT_INGRESSCUSTOM
#  Install Helm
        curl https://raw.githubusercontent.com/helm/helm/master/scripts/get > get_helm.sh
        ./get_helm.sh

# Run helm init for first time on your cluster
helm init 
helm install --name nginx-ingress-admin stable/nginx-ingress --namespace kube-system --set rbac.create=false --set rbac.createRole=false --set rbac.createClusterRole=false --set controller.ingressClass=nginx-admin
helm install --name nginx-ingress-api stable/nginx-ingress --namespace kube-system --set rbac.create=false --set rbac.createRole=false --set rbac.createClusterRole=false --set controller.ingressClass=nginx-api

else
        echo "------------------------------------" >> $LOG
        echo "The $GIT_INGRESS doesn't exist " >> $LOG
