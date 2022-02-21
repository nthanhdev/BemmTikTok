@echo off
setlocal enabledelayedexpansion
for /f "tokens=6*" %%a in ('adb shell "service call iphonesubinfo 1 ^| grep -m 1 \"'\""') do (
set imei1=%%a)
for /f "tokens=6*" %%b in ('adb shell "service call iphonesubinfo 1 ^| grep -m 2 \"'\""') do (
set imei2=%%b)
for /f "tokens=4*" %%c in ('adb shell "service call iphonesubinfo 1 ^| grep -m 3 \"'\""') do (
set imei3=%%c) 
set imei=!imei1!!imei2!!imei3!
echo !imei! > imei.txt
for /f "delims=" %%d in (imei.txt) do (
set DeviceIMEI=%%d
set DeviceIMEI=!DeviceIMEI:'=!
set DeviceIMEI=!DeviceIMEI:.=!
set OIMEI=Phone IMEI  !DeviceIMEI!
)
echo %OIMEI%
pause