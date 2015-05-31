Webserver for testing purposes only
----

To start:
---
1.  Build the solution
2.  Edit StaticWebServer.exe.config in the output folder
  
        <appSettings>
          <add key="Endpoint" value="http://localhost:9527/"/>
          <add key="ResourceFile" value="response.json"/>
          <add key="ContentType" value="application/json"/>
        </appSettings>
  
3.  Edit resource file
4.  Run StaticWebServer.exe. Press Enter if you want to stop the server
