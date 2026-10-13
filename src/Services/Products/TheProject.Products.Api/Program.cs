
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using TheProject.BuildingBlocks.Slices;
using TheProject.Products.Api.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<ProductsDbContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("products"))
    .UseSnakeCaseNamingConvention();
});


builder.Services.AddSlices(typeof(Program).Assembly);



var app = builder.Build();

app.UseExceptionHandler();
app.UseStatusCodePages();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

}


app.MapGroup("")
   .ProducesProblem(StatusCodes.Status500InternalServerError)
   .MapEndpoints();

app.Run();