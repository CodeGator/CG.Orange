
### CG.Orange.EfCore - README

This project contains an EFCORE data access layer for the **CG.Orange** microservice.

The project supports the following options for storage:

* SQL-Server - for use in a standard production environment. 
* SQLite - for use in a QA environment, or a minimal development or production environment.
* In-Memory - for demonstration purposes.

#### Notes

To add/remove/change EFCORE migrations follow these steps:
    
1. Set the CG.Orange.EfCore project as the startup project, in Visual Studio.
2. Open the Package Manager Console window, in Visual Studio.
3. Set the 'Default project' to CG.Orange.EfCore, in the Package Manager Console window.
4. Use the normal add-migration commands, in the Package Manager Console window. Remember to use the -outputDir Migrations argument, so the migrations end up in the right sub folder.
5. Remember to set the start project back to the CG.Orange.Host project (or whatever project you normally start with), when you're done.

<br />

Configuration for the **CG.Orange** microservice data-access layer uses the following format:

```
{
  "DAL": {
    "DropDatabaseOnStartup": true,
    "MigrateDatabaseOnStartup": true,
    "Strategy": "SQLite",
    "SQLServer": {
      "ConnectionString": "Server=localhost;Database=CG.Orange;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
    },
    "SQLite": {
      "ConnectionString": "Data Source=orange.db;"
    },
    "InMemory": {
      "DatabaseName": "orange"
    }
  }
}
```

* `DAL` is the default name of the section. If you change this name you must also change the `sectionName` parameter of the `WebApplicationBuilder.AddDataLayer` method.

* `DropDatabaseOnStartup` is a flag that tells the DAL startup logic to drop the database on application startup. This is obviously a feature that should be used with caution. Note that this feature does nothing in a production environment. 

* `MigrateDatabaseOnStartup` is a flag that tells the DAL startup logic to apply all pending migrations on application startup. This is obviously a feature that should be used with caution. Note that this feature does nothing in a production environment. 

* `Strategy` is the name of the storage strategy that should be used at runtime. 

* `SQLServer` is the section for the SQLServer storage strategy.
    * `ConnectionString` is an EFCORE, SQLServer compliant database connection string.

* `SQLite` is the section for the SQLite storage strategy.
    * `ConnectionString` is an EFCORE, SQLite compliant database connection string.

* `InMemory` is the section for the in-memory storage strategy.
    * `DatabaseName` is a unique name for the in-memory database.




