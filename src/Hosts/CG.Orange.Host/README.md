
### CG.Orange.Host - README

This project a Blazor host for the **CG.Orange** microservice.

#### Notes

### Secrets

These secrets must be configured, either in the microservice's configuration, or, in the local environment variables. It is possible to simply add them to the local appSettings, but that's isn't recommended because anyone can open that file and read your secrets.

#### Business Logic

* BLL:SharedPassword - The shared cryptographic password for the microservice.

* BLL:SharedSalt - The shared cryptographic SALT for the microservice.

#### Data Access

Depending on which database is currently in use:

* DAL:InMemory:DatabaseName - The name of the in-memory database (Not really a secret, but placed here for consistency).

* DAL:SQLServer:ConnectionString - The connection string for the SQLServer database.

* DAL:SQLite:ConnectionString - The connection string for the SQLite database.

#### Identity

* Identity:Authority - The url for the identity authority (Not really a secret, but placed here for consistency).

* Identity:ClientId - The client id for this microservice (Not really a secret, but placed here for consistency).

* Identity:ClientSecret - The client secret for this microservice.

#### Plugins

* Plugins:Modules:[Index]:Options:KeyVaultUri - The Azure key vault URI for your secrets.

