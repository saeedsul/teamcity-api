using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using People.Api.Services;
using People.Data.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configure In-Memory Database
builder.Services.AddDbContext<Context>(options =>
    options.UseInMemoryDatabase("PeopleDb"));

// Register the PeopleService
builder.Services.AddScoped<IPeopleService, PeopleService>(); 

// Add Controllers service
builder.Services.AddControllers();

// Add Swagger/OpenAPI for self-documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add health checks
builder.Services.AddHealthChecks();


var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();    
//}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<Context>();
    context.Database.EnsureCreated();
}

// Health Check Endpoint
app.MapHealthChecks("/health")
   .WithName("Health Check")
   .WithTags("Monitoring");

// Map controllers
app.MapControllers();

app.Run();