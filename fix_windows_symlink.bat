@echo off
cls
del "%~dp0\Environments\Assets\Neodroid"
mklink /d "%~dp0\Environments\Assets\Neodroid" ..\..\Neodroid
pause