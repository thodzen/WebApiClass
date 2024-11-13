using FluentValidation;
using Marten;
using Software.Api.Catalog;
using Software.Api.Configuration;
using Software.Api.User;
using Software.Api.Vendors;

var builder = WebApplication.CreateBuilder(args);



builder.AddCustomFeatureManagement();

builder.Services.AddCustomServices();
builder.Services.AddCustomOasGeneration();

builder.Services.AddControllers();

// TODO: Refactor This to an Extension Method.
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("IsSoftwareCenter", policy =>
    {
        policy.RequireRole("software-center");
    })
    .AddPolicy("IsSoftwareManager", policy =>
    {
        policy.RequireRole("software-center-admin");
    });

var connectionString = builder.Configuration.GetConnectionString("software") ??
    throw new Exception("No Connection String");

builder.Services.AddMarten(config =>
{
    config.Connection(connectionString);
}).UseLightweightSessions();

builder.Services.AddValidatorsFromAssemblyContaining<CatalogCreateModelValidator>();

builder.Services.AddScoped<CatalogManager>(); // 99% of the time you want "Scoped"
builder.Services.AddScoped<VendorManager>();
builder.Services.AddScoped<ILookupVendors, VendorManager>();
builder.Services.AddScoped<IProvideUserInformation, UserManager>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run(); // a web server up and running and listening for requests.


public partial class Program { }

