using System.Net;

namespace Web.IntegrationsTests.Infrastructure;

public class ApiResponse<T>
{
    public HttpStatusCode StatusCode { get; set; }
    public T? Data { get; set; }
}

public class ApiResponse
{
    public HttpStatusCode StatusCode { get; set; }
}