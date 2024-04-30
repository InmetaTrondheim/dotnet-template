# InmetaTemplate .NET Core Backend Project

## Build

Run `dotnet build` to build the solution.

## Run

Note that in order to run the application locally, a [Microsoft SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) needs to be installed. Another solution would be to run one using docker or using a remote server, and changing the connection string in [appsettings.json](./src/Web/appsettings.json)

To run the application using Visual Studio, select the ```http``` or ```https``` launch profile and press F5.

To run the application without Visual Studio (using CLI):

```bash
cd .\src\Web\
dotnet run --launch-profile "https"
```

## Test

The solution contains unit and integration tests.

Note that in order to run the tests, Docker Desktop needs to be installed and running.

To run the tests with Visual Studio:
Use the Test menu > Run All Tests, or press Ctrl + R, A

To run the tests without Visual Studio:
```bash
dotnet test
```

## Infrastructure

This project is using [CQRS and mediator pattern](https://medium.com/@darshana-edirisinghe/cqrs-and-mediator-design-patterns-f11d2e9e9c2e) and [Clean Architecture](https://www.dandoescode.com/blog/clean-architecture-an-introduction). It is separated into 4 projects, Domain, Application, Infrastructure and Web. Integration testing is implemented using the [Outside-In Test-Driven Development](https://www.codecademy.com/article/tdd-outside-in) method. Testcontainers are utilized to setup a real database when running the tests.

## Entity Framework Core

Entity Framework Core is used as the ORM in the Infrastructure layer to access data. [BaseEntity.cs](./src/Domain/Common/BaseEntity.cs) is used as a base class for all entities. This is to ensure that all entities has an id, dates and soft delete. It is also used to dispatch domain events using MediatR.

### Interceptors

#### [EntityDateInterceptor](./src/Infrastructure/Data/Interceptors/EntityDateInterceptor.cs)

Ensures that all entities that derive from [BaseEntity.cs](./src/Domain/Common/BaseEntity.cs) gets the correct date when added or modified when ```SaveChanges``` is called.

#### [DispatchDomainEventsInterceptor](./src/Infrastructure/Data/Interceptors/DispatchDomainEventsInterceptor.cs)

Ensures that events are correctly sent for all entities that derive from [BaseEntity.cs](./src/Domain/Common/BaseEntity.cs) and has Domain events added and ready to be sent. This happens when ```SaveChanges``` is called. Events are also cleared.

## Authentication

Bearer authentication is used to validate that the calling user is authorized. The following app settings are used for this to work proparly:
- ```Authority```: The url to the authority to use when making OpenIdConnect calls.
- ```Audience```: The audience to check for if aud claim is emitted, if not than leave it as ```null``` to skip audience validation.
- ```ClientId```/```ClientSecret```: Client Id and secret of client that is set up in the identity provider, these are used for swagger integration. If swagger is not enabled in staging/production, these do not need to be set. However if you wish to test the authentication locally using swagger, than these are required. They can be safely stored in UserSecrets to avoid checking them into version control. Another easier approach is to disable authentication under development, if you do not wish to deal with secrets.
- ```ScopeClaimType```: The name of the claim type for scopes. This is customizeable because they can differ from identity provider to another. Duende for example uses "scope" while Microsoft Entra used "scp".
- ```ScopeClaimValue```: The Api scope for this api, to verify that the token used to make calls has the appropriate scope and is allowed to call this specific api. Note that there is no support for multiple scopes out of the box (for when you want specific scopes like for example "api:read" and "api:write"). These can be very application specific and needs to be implemented as needed.

## Authorization

There is no authorization in place other than checking the token for the scope ```api``` on every authorized call.

## Error handling

For error handling, we use [Hellang Problem Details](https://github.com/khellang/Middleware). This makes it possible to easily transform an exception into a gived HTTP status code.
We build on top of this to have a general api exception (ApiException) that can be thrown from anywhere and decide what status code the Web layer should return. 
The ApiException takes an ApiError as argument, with HTTP Status Code, a custom error code and a message. The custom error code is there for the frontend to use when more spesific or corner-case errors are thrown. 
For example, one API method may return bad request for multiple reasons, and frontend can use the error codes to distinguish between those bad request responses.
Error codes must be unique and follow the same pattern to make it easier to use, as done in the CommonErrors.cs file. 
The Error codes are hard coded without constants by design to ensure they are not changed, as tests are in place to ensure they are present.
There is a test for each error to ensure it is present and unchanged, as changing or removing may cause breaking changes if for example Frontend is using them.
It is important to create a test when creating new errors in the CommonErrors.cs file.

## Integration Tests

Integration tests use the [Outside-In Test-Driven Development](https://www.codecademy.com/article/tdd-outside-in) method. This means that the entrypoint for each test is through the API, just like a normal user. To achieve this, the API runs just like it would run otherwise, only that this happens in-memory before the tests run. After this, the tests will call the API and verify the response. To get it as close to the real application as possible, we use [Testcontainers](https://testcontainers.com/) to initialize a real database using docker that the application uses. This is done in a CollectionFixture, which means that all tests will share the same database and data will persist untill all tests have completed. It is important to keep this in mind when creating tests.