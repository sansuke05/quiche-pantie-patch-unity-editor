#!/bin/sh

UNITY_PATH="C:\Program Files\Unity\Hub\Editor\2018.4.20f1\Editor\Unity.exe"
LOG_FILE=unity.log
PROJ_PATH="D:\sansuke05\projects\unity\QuichePantiePatchEditor"
PACKAGE_NAME=QuichePantiePatchEditor.unitypackage
EXPORT_PACKAGES="Assets/AliceLaboratory/Editor Assets/Plugins/UniTask"
EXPORT_PARENT_DIR=/d/sansuke05/UnityAssets/editor
EXPORT_DIR=${EXPORT_PARENT_DIR}/QuichePantiePatchEditor_v$1

set -e
git pull

echo "Updating local repository is done"
cmd //c "$UNITY_PATH" -exportPackage $EXPORT_PACKAGES $PACKAGE_NAME -projectPath $PROJ_PATH -batchmode -nographics -logfile $LOG_FILE -quit

mkdir $EXPORT_DIR
mv $PACKAGE_NAME $EXPORT_DIR 
echo "Creating unitypackage is done"