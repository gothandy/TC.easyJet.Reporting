# TC.easyJet.Reporting

##Command Line Arguments
```
Console.exe azureAccountKey togglApiKey trelloToken
```
 
##azureAccountKey
Goto https://portal.azure.com/ and find the tceasyjetreporting storage account. You will find primary and secondary keys there.

##togglApiKey
Goto https://www.toggl.com/app/profile your personal key is found at the bottom of the page.

##trelloToken
The public Key is `3ba00ca224256611c3ccbac183364259`, use this on the sandbox https://developers.trello.com/sandbox to allow access from your account to this application. You'll need Chrome with Developer tools running to watch the Network traffic and grab the token. Good tool for exploring calls to the API too.

##Debug
For the Console Applications Command Line Arguments need adding to the Debug section of the project Properties where they get added to your .csporj.user files so only stored locally.

##WebJobs
Use Application Settings to set the name/value pairs. Use naming convention given above.
