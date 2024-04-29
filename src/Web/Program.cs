using Hellang.Middleware.ProblemDetails;
using InmetaTemplate.Application;
using InmetaTemplate.Infrastructure;
using InmetaTemplate.Infrastructure.Data;
using InmetaTemplate.Web.Extensions;
using InmetaTemplate.Web.Helpers;
using InmetaTemplate.Web.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAuthenticationAndAuthorization(builder.Configuration);

builder.Services.AddSwaggerGen(c => c.AddOauth(builder.Configuration));

builder.Services.AddControllers();

builder.Services.AddApplicationInsightsTelemetry();

builder.Services.AddHealthChecks()
    .AddDbContextCheck<InmetaTemplateDbContext>();

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
    app.UseSwaggerUI(c => c.UseOauth(builder.Configuration));
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapHealthChecks("/hc");

app.MapControllers()
    .RequireAuthorization(AuthPolicies.RequireApiScope);

app.Run();

//For integration tests to use this class
namespace InmetaTemplate.Web
{
    public partial class Program { }
}