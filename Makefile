UNITY_CLI?=cmd //c "C:\Program Files\Unity\Hub\Editor\2017.4.28f1\Editor\Unity.exe"
LOG_FILE?=unity.log
PROJ_PATH?=$(shell pwd)
PACKAGE_NAME?=MyPlugin.unitypackage
EXPORT_PACKAGES:= Assets/Editors Assets/Plugins Assets/Scripts

.PHONY: package

all:

package:
	$(UNITY_CLI) \
		-exportPackage $(EXPORT_PACKAGES) $(PACKAGE_NAME) \
		-projectPath $(PROJ_PATH) \
		-batchmode \
		-nographics \
		-logfile $(LOG_FILE) \
		-quit