
## CG.Orange.Data.SqlServer - README

This project contains an EFCORE SQLServer provider for the **CG.Orange** microservice.

### Notes

To add/remove/change EFCORE migrations follow these steps:
    
1. Set the CG.Orange.Data.SqlServer project as the startup project, in Visual Studio.
2. Open the Package Manager Console window, in Visual Studio.
3. Use the normal add-migration commands, in the Package Manager Console window. Remember to use the -p CG.Orange.Data.SqlServer argument, so the migrations will end up in the right project.

So here's an example command line for adding a migration in Visual Studio: 

```
add-migration MyMigration -p CG.Orange.Data.SqlServer
```

Remember to set the start project back to the CG.Orange.Host project (or whatever project you normally start with), when you're done.


