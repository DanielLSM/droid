@echo off
cls
del "%~dp0\Environments\Assets\Neodroid"
mklink /d "%~dp0\Environments\Assets\Neodroid" ..\..\Neodroid
del "%~dp0\Environments\Assets\SteamVR"
mklink /d "%~dp0\Environments\Assets\SteamVR" ..\..\SteamVR
pause