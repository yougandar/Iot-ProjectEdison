#!/bin/bash
#Comment - Updates the appsettings file for Edison Api code

#------------------------------------
COSMOSDBENDPOINT="$1",
ADCLIENTID="$2",
ADDOMAIN="$3",
ADTENANTID="$4",
B2CCLIENTID="$5",
B2CDOMAIN="$6",
B2CSIGNINPOLICY="$7",
NOTIFICATIONHUBNAME="$8"
TWILIOACCID="$9"
CONNECTIONSTRING="$10"
#------------------------------------
GIT_URL="https://raw.githubusercontent.com/sysgain/Iot-ProjectEdison/stage/scripts"
GITPATH=`pwd`
GIT_DIRPATH="$GITPATH/ProjectEdison"
GIT_DIRCONFIGPATH="$GIT_DIRPATH/Edison.Web/Edison.Api"
APP="/appsettings.json.bak"
APPDEV="/appsettings.Development.json.bak"
LOG="/tmp/edisonapi.log.`date +%d%m%Y_%T`"
#------------------------------------
ls $GIT_DIRPATH
if [ $? -eq 0 ]
then
        echo "------------------------------------" >> $LOG
        echo "The $GIT_DIRPATH exists" >> $LOG
        cd $GIT_DIRCONFIGPATH
        mv $GIT_DIRCONFIGPATH/* /tmp/
        echo "------------------------------------" >> $LOG
        echo "Updating the $APP file..." >> $LOG
        wget -O $GIT_DIRCONFIGPATH$APP $GIT_URL$APP
        sed -i -e s/COSMOSDBENDPOINT/${COSMOSDBENDPOINT}/ -e s/ADCLIENTID/${ADCLIENTID}/ -e s/ADDOMAIN/${ADDOMAIN}/ -e s/ADTENANTID/${ADTENANTID}/ -e s/B2CCLIENTID/${B2CCLIENTID}/ -e s/B2CDOMAIN/${B2CDOMAIN}/ -e s/B2CSIGNINPOLICY/${B2CSIGNINPOLICY}/ -e s/NOTIFICATIONHUBNAME/${NOTIFICATIONHUBNAME}/ -e s/TWILIOACCID/${TWILIOACCID}/ $GIT_DIRCONFIGPATH$APP
        mv $GIT_DIRCONFIGPATH$APP $GIT_DIRCONFIGPATH/appsettings.json
        echo "------------------------------------" >> $LOG
        echo "Updating the $APPDEV file..." >> $LOG
        wget -O $GIT_DIRCONFIGPATH$APPDEV $GIT_URL$APPDEV
        sed -i -e s/ConnectionString/${CONNECTIONSTRING}/ $GIT_DIRCONFIGPATH$APPDEV
        mv $GIT_DIRCONFIGPATH$APPDEV $GIT_DIRCONFIGPATH/appsettings.Development.json
        echo "------------------------------------" >> $LOG
fi
