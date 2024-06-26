# Inmeta .NET Core dotnet new Template

The purpose of this template is to provide a fully fledged backend in .NET Core using CQRS. The template has everything setup and ready to go, like authentication, error handling, logging, validation, data access, mapping and example classes for CQRS pattern to get you startet. Also integration tests using testcontainers are in place.

## Prerequisites

The following prerequisites are required to build and run the solution locally:

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (latest version)
- [Local MSSQL Database Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

It is possible to swap out the database connection string in appsettings.json for using a remote database when running locally.

## Create a new project using Docker

To create a project with this template using Docker, you can run the following commands

### On Windows:
```bash
docker run -v <Full Path Repos Directory>/<Project Name>:/out ghcr.io/inmetatrondheim/dotnet-template:main --force -n <Project Name> 
```

Example:
```bash
docker run -v C:/Repos/MyProject:/out ghcr.io/inmetatrondheim/dotnet-template:main --force -n MyProject 
```

### On Linux:
```bash
mkdir <ProjectName>
cd <ProjectName>
docker run -v $(pwd):/out ghcr.io/inmetatrondheim/dotnet-template:main --force -n <ProjectName>
```
**It is recommended to use Pascal Case (no space with capital first letter of every word) for the project name to follow the naming convensions for namespaces. Also preferably avoid spacial characters.**

If you want a project with no authentication, you can add the ```--ExcludeAuthentication``` flag at the end of the ```docker run ...``` command.

## Create a new project using Nuget

You can skip this section and jump down to [Customizing your project](#customizing-your-project) if you have already created a project using docker.

### Installing the template

To use this template, you can use the nuget package in the Inmeta Trondheim nuget feed:
1. In Github, [create a personal access token (classic)](https://docs.github.com/en/authentication/keeping-your-account-and-data-secure/managing-your-personal-access-tokens#creating-a-personal-access-token-classic) with the scope ```read:packages```. Note that the account must have access to the packages. This step can be skipped if you are using Github workflows.
2. Add the nuget source:
```dotnet nuget add source https://nuget.pkg.github.com/InmetaTrondheim/index.json -n InmetaTrondheim -u <username> -p <access_token>```.
If this is done through workflows, than username can be replaced with ```${{ github.actor }}``` and the access token with ```${{ secrets.GITHUB_TOKEN }} ```
3. Install the template:
```dotnet new install Inmeta.Netcore.Template```

Another approach is to clone this repo and create a nuget package locally. [Nuget CLI](https://learn.microsoft.com/en-us/nuget/reference/nuget-exe-cli-reference?tabs=windows#installing-nugetexe) is required since we are using a .nuspec file:

```bash 
nuget pack -NoDefaultExcludes -OutputFileNamesWithoutVersion
dotnet new install .\Inmeta.Netcore.Template.nupkg
```

### Updating the template

To update the template to the newest version, you can run: 
```bash 
dotnet new update
```
This requires that the nuget source has been added, see the [Installing the template](#installing-the-template) section for adding the source.


If changes been made to the template code or files, and a new version is ready, the nuget package version has to be upped to deploy a new version. This is done in the [InmetaTemplat.nuspec](./InmetaTemplate.nuspec) file. Pushing the changes will automatically trigger a github workflow that publishes the new version to the organization's nuget feed.

### Creating the project

When the template is installed, you can create a new project using the template with the command:
```bash 
dotnet new inmeta-template -n "MyProject"
```

If you want a project with no authentication, you can use the ```--ExcludeAuthentication``` flag:
```bash 
dotnet new inmeta-template --ExcludeAuthentication -n "MyProject"
```

**It is recommended to use Pascal Case (no space with capital first letter of every word) for the project name to follow the naming convensions for namespaces. Also preferably avoid spacial characters.**

## Customizing your project

After creating a project using the ```dotnet new``` command, you should have a project that is ready and runs. But this is only a demo project and customizations has to be done.

### Database

By default, when running locally, the api tries to connect to a local sql server. When deploying the application, the connection string of the database needs to be set using the config variable ```ConnectionStrings:ApplicationDb```

### Authentication

Authentication is included by default. To exclude authentication in the new project, use the ```--ExcludeAuthentication``` flag when creating the project using the template.

Out of the box, the authentication for the API is checking for tokens coming from [Duende's demo server](https://demo.duendesoftware.com/), which is an identity provider made for demo/test purposes.
To customize this, the following app settings needs to be changed:
- ```Authority```: The url to the authority to use when making OpenIdConnect calls.
- ```Audience```: The audience to check for if aud claim is emitted, if not than leave it as ```null``` to skip audience validation.
- ```ClientId```/```ClientSecret```: Client Id and secret of client that is set up in the identity provider, these are used for swagger integration. If swagger is not enabled in staging/production, these do not need to be set. However if you wish to test the authentication locally using swagger, than these are required. They can be safely stored in [User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-8.0&tabs=windows) to avoid checking them into version control. Another easier approach is to disable authentication under development, if you do not wish to deal with secrets.
- ```ScopeClaimType```: The name of the claim type for scopes. This is customizeable because they can differ from identity provider to another. Duende for example uses "scope" while Microsoft Entra used "scp".
- ```ScopeClaimValue```: The Api scope for this api, to verify that the token used to make calls has the appropriate scope and is allowed to call this specific api. Note that there is no support for multiple scopes out of the box (for when you want specific scopes like for example "api:read" and "api:write"). These can be very application specific and needs to be implemented as needed.

### Application insights

Application insights is added to the project, and everything that is needed for it to work is to add the connection string using configuration ```ApplicationInsights:ConnectionString``` in appsettings or the environment variable ```APPLICATIONINSIGHTS_CONNECTION_STRING```

### Domain events and handlers

In this template, we have one event handler as an example, [TodoItemCreatedEventHandler.cs](./src/Application/TodoItems/EventHandlers/TodoItemCreatedEventHandler.cs) The only thing this does is log a message. This event is fired every time a Todo item is created, see [CreateTodoItem.cs](./src/Application/TodoItems/Commands/CreateTodoItem.cs) for an example on how the event is emitted. Follow this example to create other event handlers to react to commands or queries executed by the application.
