using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();

    // Добавление описания для Swagger
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Sales Service API",
        Version = "v1",
        Description = "API для расчета кредита",
        Contact = new OpenApiContact
        {
            Name = "Мой GitHub",
            Url = new Uri("https://github.com/mmmargarret/creditcalculation.git")
        },
    });
});
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
var app = builder.Build();

// 
    app.UseSwagger();
    app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
