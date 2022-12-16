
### CG.Orange.SqlLite - README

This project contains a SQL-Lite data access layer for the **CG.Orange** microservice.

#### Notes

To add/remove/change EFCORE migrations follow these steps:
    
1. Set the CG.Orange.SqlLite project as the startup project, in Visual Studio.
2. Open the Package Manager Console window, in Visual Studio.
3. Set the 'Default project' to CG.Orange.SqlLite, in the Package Manager Console window.
4. Use the normal add-migration commands, in the Package Manager Console window. Remember to use the -outputDir Migrations argument, so the migrations end up in the right sub folder.
5. Remember to set the start project back to the CG.Orange.Host project (or whatever project you normally start with), when you're done.






