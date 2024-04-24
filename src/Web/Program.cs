using Application;
using Infrastructure;
using Infrastructure.Data;
using Web.Extensions;
using Web.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAuthenticationAndAuthorization(builder.Configuration);

builder.Services.AddSwaggerGen(c => c.AddOauth(builder.Configuration));

builder.Services.AddControllers();

builder.Services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>();

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration, builder.Environment.IsDevelopment());

var app = builder.Build();

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
public partial class Program { }