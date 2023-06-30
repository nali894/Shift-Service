using Microsoft.ApplicationInsights.Extensibility;
using Serilog;
using Serilog.Sinks.ApplicationInsights;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using ShiftService;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json");
builder.WebHost.UseUrls("http://*:3045");
builder.Host.UseSerilog();
// Agrega servicios al contenedor.

ConfigureLogging(builder);

builder.Services.AddControllers();
// Obtén más información sobre la configuración de Swagger/OpenAPI en https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var azureAdConfiguration = builder.Configuration.GetSection("AzureAd");
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//        .AddMicrosoftIdentityWebApi(azureAdConfiguration);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["AzureAd:Instance"] + builder.Configuration["AzureAd:TenantId"]+"/v2.0";
        options.Audience = builder.Configuration["AzureAd:ClientId"];
    });


// Configuración de Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
});


var app = builder.Build();

// Configuración de Swagger UI
app.UseSwagger(option =>
{
    option.RouteTemplate = "api/{documentName}/swagger.json";
});

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/api/v1/swagger.json", "API V1");
    c.RoutePrefix = "api";
});


// app cors
app.UseCors("corsapp");

app.UseAuthentication();

app.UseAuthorization();


app.MapControllers();

app.Run();

Log.CloseAndFlush();


void ConfigureLogging(WebApplicationBuilder builder)
{
    // Configuración de Application Insights
    builder.Services.AddApplicationInsightsTelemetry();
    TelemetryConfiguration telemetryConfiguration = TelemetryConfiguration.Active;
    telemetryConfiguration.InstrumentationKey = builder.Configuration["ApplicationInsights:InstrumentationKey"];

    // Configuración de Serilog para Application Insights
    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .WriteTo.ApplicationInsights(telemetryConfiguration, TelemetryConverter.Traces)
        .CreateLogger();

}
