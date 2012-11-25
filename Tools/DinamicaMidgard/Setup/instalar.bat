@echo off
>tmp.reg echo REGEDIT4
>>tmp.reg echo [HKEY_CLASSES_ROOT\CLSID\{16026687-A316-425F-B9C5-AFC54436B47C}]
>>tmp.reg echo "(Default)"=-
>>tmp.reg echo [-HKEY_CLASSES_ROOT\CLSID\{16026687-A316-425F-B9C5-AFC54436B47C}]
>>tmp.reg echo [HKEY_CLASSES_ROOT\CLSID\{B0315C16-DBAC-4647-A559-F7383CE39D03}]
>>tmp.reg echo "(Default)"=-
>>tmp.reg echo [-HKEY_CLASSES_ROOT\CLSID\{B0315C16-DBAC-4647-A559-F7383CE39D03}]
>>tmp.reg echo [-HKEY_CLASSES_ROOT\ProcessModel.Connect]
>>tmp.reg echo [-HKEY_CLASSES_ROOT\ScreenModel.Connect]
>>tmp.reg echo [-HKEY_LOCAL_MACHINE\Software\Microsoft\Visio\Addins\ProcessModel.Connect]
>>tmp.reg echo [-HKEY_LOCAL_MACHINE\Software\Microsoft\Visio\Addins\ScreenModel.Connect]
>>tmp.reg echo [-HKEY_CURRENT_USER\Software\Microsoft\Office\11.0\Visio\Application]
>>tmp.reg echo -"StencilPath"=-
>>tmp.reg echo [HKEY_CURRENT_USER\Software\Microsoft\Office\11.0\Visio\Application]
>>tmp.reg echo "TemplatePath"=-
>>tmp.reg echo.

regedit /s tmp.reg
del tmp.reg
msiexec /i ModeladorCGI.msi REINSTALLMODE=vomus REBOOT=Supress