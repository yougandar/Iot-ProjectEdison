#!/bin/bash
#downloading deploy1.sh
sudo wget -P /var/lib/waagent/custom-script/download/0 https://raw.githubusercontent.com/sysgain/Iot-ProjectEdison/stage/scripts/deploy1.sh
#downloading configupdate2.sh
sudo wget -P /var/lib/waagent/custom-script/download/0 https://raw.githubusercontent.com/sysgain/Iot-ProjectEdison/stage/scripts/configupdate2.sh

#downloading commonupdate3.sh
sudo wget -P /var/lib/waagent/custom-script/download/0 https://raw.githubusercontent.com/sysgain/Iot-ProjectEdison/stage/scripts/commonupdate3.sh
#downloading edisonwebenvupdate4.sh
sudo wget -P /var/lib/waagent/custom-script/download/0 https://raw.githubusercontent.com/sysgain/Iot-ProjectEdison/stage/scripts/edisonwebenvupdate4.sh
#downloading updateappsettings5.sh
 sudo wget -P /var/lib/waagent/custom-script/download/0 https://raw.githubusercontent.com/sysgain/Iot-ProjectEdison/stage/scripts/updateappsettings5.sh
#downloading imagesupdate6.sh
sudo wget -P /var/lib/waagent/custom-script/download/0 https://raw.githubusercontent.com/sysgain/Iot-ProjectEdison/stage/scripts/imagesupdate6.sh
#downloading set-kubernetes-config7.sh
sudo wget -P /var/lib/waagent/custom-script/download/0/projectedison/edison.web/kubernetes/qa/config https://raw.githubusercontent.com/sysgain/Iot-ProjectEdison/stage/scripts/set-kubernetes-config7.sh
 

cd /var/lib/waagent/custom-script/download/0
chmod +x deploy1.sh configupdate2.sh commonupdate3.sh edisonwebenvupdate4.sh updateappsettings5.sh imagesupdate6.sh
cd /var/lib/waagent/custom-script/download/0/projectedison/edison.web/kubernetes/qa/config 
chmod +x set-kubernetes-config7.sh
