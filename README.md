![logo](logos/codegator-167x79.png)

# CG.Orange: 

[![Build Status](https://dev.azure.com/codegator/CG.Orange/_apis/build/status/CodeGator.CG.Orange?branchName=main)](https://dev.azure.com/codegator/CG.Orange/_build/latest?definitionId=95&branchName=main)
[![Board Status](https://dev.azure.com/codegator/5153b7fd-7c67-4c6f-9911-7e38bc8f421b/eda5ba0a-dc2f-4276-ab1c-b9096f753eeb/_apis/work/boardbadge/74d4b9b8-3e7c-4e49-b1c1-98879fc693de)](https://dev.azure.com/codegator/5153b7fd-7c67-4c6f-9911-7e38bc8f421b/_boards/board/t/eda5ba0a-dc2f-4276-ab1c-b9096f753eeb/Issues/)
![Azure DevOps coverage](https://img.shields.io/azure-devops/coverage/codegator/CG.Orange/95?logo=codecov&logoColor=white&style=flat-square&token=4BBNQPPATD)
[![Github docs](https://img.shields.io/static/v1?label=Documentation&message=online&color=blue)](https://codegator.github.io/CG.Orange/index.html)

#### GitHub Stats

![Alt](https://repobeats.axiom.co/api/embed/7691ab3e13795551e146c7203987f95588918bf4.svg "Repobeats analytics image")

### What does it do?

Orange is an idea for a configuration microservice. The scenario is: You create, or upload, a JSON configuration file, then, later, you connect to the microservice and get that same configuration back.

The JSON you upload can either have secrets embedded in it (JSON is encrypted, at rest), or you can dynamically pull your secrets from external sources, like Azure, or AWS. Either way, clients don't need to know anything about what is, or isn't a secret. Clients simply use an `IConfiguration` object that contains all the settings for that application.

Using Orange is more secure than writing secrets to your appSettings.json file(s). Orange is also more convenient than manually adding secrets to environment settings, in containers or on VM's, or servers.

All updates on the server-side generate an event back to connected clients, so your applications will always know when their configuration has been changed, and can react accordingly.

### What runtime(s) does it support?

* [.NET 7.x](https://dotnet.microsoft.com/en-us/download/dotnet/7.0) or higher

### What OS(s) does it support?

* [Windows](https://en.wikipedia.org/wiki/Microsoft_Windows) 

* [Linux](https://en.wikipedia.org/wiki/Linux) (not tested)

* [macOS](https://en.wikipedia.org/wiki/MacOS) (not tested)

### What database(s) does it support?

* [SqlServer](https://www.microsoft.com/en-us/sql-server/sql-server-2019)

* [SQLite](https://www.sqlite.org/index.html)

* [In-memory (for demo purposes)](https://learn.microsoft.com/en-us/ef/core/providers/in-memory/?tabs=dotnet-core-cli)

### What cloud based secrets does it support?

* [Azure](https://azure.microsoft.com/en-us/products/key-vault/) (tested against Azure key vaults).

### What authentication / authorization does it support?

* Any [OIDC](https://learn.microsoft.com/en-us/azure/active-directory/develop/v2-protocols-oidc) compliant identity service capable of integrating with a client using a url, client identifier, and a secret.

### How do I contact you?

If you've spotted a bug in the code please use the project Issues [HERE](https://github.com/CodeGator/CG.Orange/issues)

We also have a discussion group [HERE](https://github.com/CodeGator/CG.Orange/discussions)

### Is there any documentation?

There is developer documentation [HERE](https://codegator.github.io/CG.Orange/)  (when the blasted CI/CD pipeline works and it gets updated).

We also write about projects like this one on our website, [HERE](http://www.codegator.com)

### Disclaimer

This project and it's contents are experimental in nature. There is no official support. Use at your own risk.