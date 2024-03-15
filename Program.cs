using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Operation Id
    c.CustomOperationIds(e => $"{e.HttpMethod.ToLower()}-{e.ActionDescriptor.RouteValues["controller"].ToLower()}");
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Rainfall Api",
        Version = "1.0",
        Contact = new OpenApiContact
        {
            Name = "Sorted",
            Url = new Uri("https://www.sorted.com")
        },
        Description = "An API which provides rainfall reading data",
    });
});
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(options =>
    {
        // Server Details
        options.PreSerializeFilters.Add((swagger, httpReq) =>
        {
            swagger.Servers = new List<OpenApiServer>
            {
                new OpenApiServer
                {
                    Url = "http://localhost:3000",
                    Description = "Rainfall Api"
                }
            };
        });
    });
    app.UseSwaggerUI(c => {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "swaggertest API V1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
