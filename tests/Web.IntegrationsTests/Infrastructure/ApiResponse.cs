using System.Net;

namespace InmetaTemplate.Web.IntegrationsTests.Infrastructure;

/// <summary>
/// API response with data
/// </summary>
/// <typeparam name="T">Expected type of data for deserializing</typeparam>
public class ApiResponse<T> : ApiResponse
{
    public T? Data { get; set; }
}

/// <summary>
/// API response with no data (only status code)
/// </summary>
public class ApiResponse
{
    public HttpStatusCode StatusCode { get; set; }
}