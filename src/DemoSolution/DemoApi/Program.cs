using HtTemplate.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.AddCustomFeatureManagement();

builder.Services.AddCustomServices();
builder.Services.AddCustomOasGeneration();

builder.Services.AddControllers();
// above this line is configuring the services this application will use.
// what is a service? Some code that owns some state and the process associated with that state.
var app = builder.Build();
// after this line is "http middleware" - code that looks at the incoming request, and 
// helps decide if and how a response should be made.

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // generate the open api specification from reflecting on our code.
    app.UseSwaggerUI(); // that if they go to /swagger/index.html
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers(); // before our application starts, use "reflection" to find all the routes.
// route table - phone lookup table.
// GET /status - StatusController -> GetTheStatus
// GET /status/int/int/int -> GetTheStatus and provide the values
app.Run();