#Vincente

An application for reporting on the activity in Trello and Toggl.

##AppSettings
To keep your keys private on your development environment you can use machine.config to hold the relevant AppSettings. Find it `C:\Windows\Microsoft.NET\Framework\v4.0.30319\Config` and remember to use Run As administrator. Once in Azure use the AppSettings functionality.
```
  <appSettings>
    <add key="azureConnectionString" value=""/>
    <add key="togglApiKey" value=""/>
    <add key="trelloToken" value=""/>
  </appSettings>
```
###azureConnectionString
Goto https://portal.azure.com/ or use Server Explorer for each storage account. You will find primary and secondary keys there.

###togglApiKey
Goto https://www.toggl.com/app/profile your personal key is found at the bottom of the page.

###trelloToken
The public Key is `3ba00ca224256611c3ccbac183364259`, use this on the sandbox https://developers.trello.com/sandbox to allow access from your account to this application. You'll need Chrome with Developer tools running to watch the Network traffic and grab the token. Good tool for exploring calls to the API too.

##Azure Configuration
1. Create Resource Group
2. Create Storage Account (note connection string)
3. Publish WebApp
4. Update AppSettings
5. Setup Scheduler Job (see below)

### Scheduler Job
All WebJobs are published as OnDemand, this allows the Scheduler Job Collection to be 
added to the Resource Group. Add a new Scheduler with Action Settings as follows:

```
Action: Https
Method: Post
Url: https://vincentewebapp.scm.azurewebsites.net/api/triggeredjobs/TrelloConsoleApp/run
Authentication Settings: Basic (get username and password from publish profile)
```

##Continuous Deployment
Setup from master to VincenteWebApp.



