# Inmeta .NET Core Template Project


## CQRS and MediatR


## Entity Framework Core

### Interceptors


## Authentication

## Authorization

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