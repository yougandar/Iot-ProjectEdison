GITPATH=`pwd`
GIT_DIRPATH="$GITPATH/ProjectEdison/Edison.Web/Kubernetes/qa/Deployment"
GIT_INGRESS="$GIT_DIRPATH/Ingress_Custom"
GIT_INGRESSCUSTOM="Ingress_Custom "
BASEURL_VALUE=`head -25 input.txt | awk -F "\"" '{print $2}'| tail -1`
ADMINURL=`head -37 input.txt | awk -F "\"" '{print $2}'| tail -1`
ADMINSECRET=`head -38 input.txt | awk -F "\"" '{print $2}'| tail -1`
APISECRET=`head -39 input.txt | awk -F "\"" '{print $2}'| tail -1`
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
    sleep 10 

# Run helm init for first time on your cluster

    helm init 
    sleep 10
    kubectl get po --all-namespaces | grep tiller
    su adminuser
    cd ~
    sudo apt install unzip
    sleep 10
    unzip Kubernetes_certs.zip
    ls
    cat 2289f3206db82816.crt gd_bundle-g2-g1.crt > qloudable-npr.com.chained.crt
    sudo -i

#2-  Create the secret in the cluster

    kubectl create secret tls $ADMINSECRET --cert /home/adminuser/qloudable-npr.com.chained.crt --key /home/adminuser/qloudable-npr.key 
    kubectl create secret tls $APISECRET --cert /home/adminuser/qloudable-npr.com.chained.crt --key /home/adminuser/qloudable-npr.key
    kubectl get secrets

#3-InstallIngressControllers
    cd /var/lib/waagent/custom-script/download/0
    sed -i -e 's/edisonadminportal.eastus.cloudapp.azure.com/'$ADMINURL'/g' ProjectEdison/Edison.Web/Kubernetes/qa/Deployment/Ingress_Custom/nginx-config-adminportal.yaml
    sed -i -e 's/tls-secret-adminportal/'$ADMINSECRET'/g' ProjectEdison/Edison.Web/Kubernetes/qa/Deployment/Ingress_Custom/nginx-config-adminportal.yaml
    sed -i -e 's/edisonapi.eastus.cloudapp.azure.com/'$BASEURL_VALUE'/g' ProjectEdison/Edison.Web/Kubernetes/qa/Deployment/Ingress_Custom/nginx-config-api.yaml
    sed -i -e 's/tls-secret-api/'$APISECRET'/g' ProjectEdison/Edison.Web/Kubernetes/qa/Deployment/Ingress_Custom/nginx-config-api.yaml
    sleep 10

    helm install --name nginx-ingress-admin stable/nginx-ingress --namespace kube-system --set rbac.create=false --set rbac.createRole=false --set rbac.createClusterRole=false --set controller.ingressClass=nginx-admin
    sleep 10
    helm install --name nginx-ingress-api stable/nginx-ingress --namespace kube-system --set rbac.create=false --set rbac.createRole=false --set rbac.createClusterRole=false --set controller.ingressClass=nginx-api
    sleep 20
    kubectl create -f ./nginx-config-adminportal.yaml
    sleep 20s
    kubectl create -f ./nginx-config-api.yaml
    sleep 20s

    kubectl get ing
    kubectl get svc -n kube-system

else
        echo "------------------------------------" >> $LOG
        echo "The $GIT_INGRESS doesn't exist " >> $LOG
fi

