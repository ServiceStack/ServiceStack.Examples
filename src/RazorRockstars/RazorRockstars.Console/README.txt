Requires sqlite3.dll module in same /bin directory as .exe or available in the OS System $PATH

In VS.NET this is done by copying the sqlite3.dll for your architecture into your projects root path:

  - \sqlite\x86\sqlite3.dll -> \
or 
  - \sqlite\x64\sqlite3.dll -> \

Then go to \sqlite3.dll properties and change the Build Action to: 'Copy if Newer'