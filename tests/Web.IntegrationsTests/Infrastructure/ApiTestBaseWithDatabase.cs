using System.Net.Http.Json;
using System.Text;
using Newtonsoft.Json;
using Xunit.Abstractions;

namespace Template._1.Web.IntegrationsTests.Infrastructure;

[Collection(nameof(ApiTestCollection))]
public abstract class ApiTestBaseWithDatabase
{
    private readonly ApiFactory _apiFactory;
    private HttpClient _httpClient;

    protected ApiTestBaseWithDatabase(ITestOutputHelper testOutputHelper, SqlServerTestFixture sqlServerTestFixture)
    {
        _apiFactory = new ApiFactory(testOutputHelper);
        OverrideAppsettings("ConnectionStrings:ApplicationDb", sqlServerTestFixture.ConnectionString);
    }

    protected abstract string ControllerPath { get; }

    protected IServiceProvider ServiceProvider =>
    _apiFactory.Server.Services;

    private void OverrideAppsettings(string key, string value)
    {
        EnsureTestServerNotRunning();
        _apiFactory.OverriddenAppsettings.Add(key, value);
    }

    /// <summary>
    /// Send a GET request to the default controller path and map return
    /// </summary>
    /// <typeparam name="T">Expected return object to map</typeparam>
    /// <param name="customUrl">For overriding the default controller url</param>
    /// <returns>ApiResponse object with status code and returned data</returns>
    protected async Task<ApiResponse<T>> SendGetRequest<T>(string? customUrl = null)
    {
        var response = await Get(customUrl ?? ControllerPath);

        return await MapApiResponse<T>(response);
    }

    /// <summary>
    /// Send a GET request to the default controller path with an id as route variable variable and map return
    /// </summary>
    /// <typeparam name="T">Expected return object to map</typeparam>
    /// <param name="id">Id of item</param>
    /// <param name="customUrl">For overriding the default controller url</param>
    /// <returns>ApiResponse object with status code and returned data</returns>
    protected async Task<ApiResponse<T>> SendGetRequest<T>(int id, string? customUrl = null)
    {
        var url = customUrl ?? ControllerPath;

        var response = await Get($"{url}/{id}");

        return await MapApiResponse<T>(response);
    }

    /// <summary>
    /// Send a POST request to the default controller path and map return
    /// </summary>
    /// <typeparam name="TReq">Request body object</typeparam>
    /// <typeparam name="TRes">Expected return object to map</typeparam>
    /// <param name="customUrl">For overriding the default controller url</param>
    /// <param name="content">Request body</param>
    /// <returns>ApiResponse object with status code and returned data</returns>
    protected async Task<ApiResponse<TRes>> SendPostRequest<TReq, TRes>(TReq content, string? customUrl = null)
    {
        var response = await Post(customUrl ?? ControllerPath, content);

        return await MapApiResponse<TRes>(response);
    }

    /// <summary>
    /// Send a PUT request to the default controller path and map return
    /// </summary>
    /// <typeparam name="TReq">Request body object</typeparam>
    /// <typeparam name="TRes">Expected return object to map</typeparam>
    /// <param name="customUrl">For overriding the default controller url</param>
    /// <param name="content">Request body</param>
    /// <returns>ApiResponse object with status code and returned data</returns>
    protected async Task<ApiResponse<TRes>> SendPutRequest<TReq, TRes>(int id, TReq content, string? customUrl = null)
    {
        var url = customUrl ?? ControllerPath;

        var response = await Put($"{url}/{id}", content);

        return await MapApiResponse<TRes>(response);
    }

    /// <summary>
    /// Send a DELETE request to the default controller path and map return
    /// </summary>
    /// <typeparam name="T">Expected return object to map</typeparam>
    /// <param name="customUrl">For overriding the default controller url</param>
    /// <returns>ApiResponse object with status code and returned data</returns>
    protected async Task<ApiResponse<T>> SendDeleteRequest<T>(string? customUrl = null)
    {
        var response = await Delete(customUrl ?? ControllerPath);

        return await MapApiResponse<T>(response);
    }

    /// <summary>
    /// Send a DELETE request to client that does not return any data
    /// </summary>
    /// <param name="id">Id of item to delete</param>
    /// <param name="customUrl">For overriding the default controller url</param>
    /// <returns>ApiResponse object with status code and returned data (if any)</returns>
    protected async Task<ApiResponse> SendDeleteRequest(int id, string? customUrl = null)
    {
        var url = customUrl ?? ControllerPath;

        var response = await Delete($"{url}/{id}");

        return new ApiResponse { StatusCode = response.StatusCode };
    }

    private static async Task<ApiResponse<T>> MapApiResponse<T>(HttpResponseMessage response)
    {
        var apiResponse = new ApiResponse<T>
        {
            StatusCode = response.StatusCode
        };

        if (response.IsSuccessStatusCode)
        {
            apiResponse.Data = await response.Content.ReadFromJsonAsync<T>();
        }

        return apiResponse;
    }


    private async Task<HttpResponseMessage> Get(string url)
    {
        return await Client.GetAsync(url);
    }

    private async Task<HttpResponseMessage> Post<T>(string url, T data)
    {
        var json = JsonConvert.SerializeObject(data);
        var jsonContent = new StringContent(json, Encoding.UTF8, "application/json");
        return await Client.PostAsync(url, jsonContent);
    }

    private async Task<HttpResponseMessage> Put<T>(string url, T data)
    {
        var jsonContent = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
        return await Client.PutAsync(url, jsonContent);
    }

    private async Task<HttpResponseMessage> Delete(string url)
    {
        return await Client.DeleteAsync(url);
    }

    private HttpClient Client
    {
        get
        {
            if (_httpClient != null)
            {
                return _httpClient;
            }

            _httpClient = _apiFactory.CreateClient();

            return _httpClient;
        }
    }

    private void EnsureTestServerNotRunning()
    {
        if (_httpClient != null)
        {
            throw new InvalidOperationException("Test server has already started, cannot configure setup now");
        }
    }
}
