using AutoMapper;
using FluentValidation;
using GlassLewisChallange.API.Middlewares;
using GlassLewisChallange.Application;
using GlassLewisChallange.Application.Common.Mapping;
using GlassLewisChallange.Application.Companies.Create;
using GlassLewisChallange.Infrastructure;
using GlassLewisChallange.Persistance;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddValidatorsFromAssemblyContaining<CreateCompanyValidator>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "GlassLewis API", Version = "v1" });

    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "API Key needed to access the endpoints. X-API-KEY: your_key",
        In = ParameterLocation.Header,
        Name = "X-API-KEY",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "ApiKeyScheme"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            Array.Empty<string>()
        }
    });
});

var mapperConfig = new MapperConfiguration(cfg =>
{
    cfg.AddMaps(typeof(MappingProfile).Assembly); // Application assembly
});
mapperConfig.AssertConfigurationIsValid();
builder.Services.AddSingleton(mapperConfig.CreateMapper());

builder.Services.AddPersistance(builder.Configuration);
builder.Services.AddInfrastructure();
builder.Services.AddApplication();


var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<ApiKeyMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
