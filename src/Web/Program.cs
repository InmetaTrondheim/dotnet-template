using Hellang.Middleware.ProblemDetails;
using Template._1.Application;
using Template._1.Infrastructure;
using Template._1.Infrastructure.Data;
using Template._1.Web.Extensions;
#if (!ExcludeAuthentication)
using Template._1.Web.Helpers;
#endif
using Template._1.Web.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

#if (!ExcludeAuthentication)
builder.Services.AddAuthenticationAndAuthorization(builder.Configuration);
#endif

#if (!ExcludeAuthentication)
builder.Services.AddSwaggerGen(c => c.AddOauth(builder.Configuration));
#else
builder.Services.AddSwaggerGen();
#endif

builder.Services.AddControllers();

builder.Services.AddApplicationInsightsTelemetry();

builder.Services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>();

builder.Services.AddProblemDetails(builder.Environment);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration, builder.Environment.IsDevelopment());

var app = builder.Build();

app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<UnhandledExceptionLoggingMiddleware>();

app.UseProblemDetails();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
#if (!ExcludeAuthentication)
    app.UseSwaggerUI(c => c.UseOauth(builder.Configuration));
#else
    app.UseSwaggerUI();
#endif
}

app.UseHttpsRedirection();

#if (!ExcludeAuthentication)
app.UseAuthentication();
app.UseAuthorization();
#endif

app.MapHealthChecks("/hc");

#if (!ExcludeAuthentication)
app.MapControllers()
    .RequireAuthorization(AuthPolicies.RequireApiScope);
#else
app.MapControllers();
#endif

app.Run();

//For integration tests to use this class
namespace Template._1.Web
{
    public partial class Program { }
}