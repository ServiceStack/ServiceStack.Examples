REM INSTALL THIS WINDOWS SERVICE:
REM 1. Build in Release mode

SET INSTALL_UTL="C:\Windows\Microsoft.NET\Framework\v2.0.50727\InstallUtil.exe"

%INSTALL_UTL% bin\Release\WinServiceAppHost.exe
