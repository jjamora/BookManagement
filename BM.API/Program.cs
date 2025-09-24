using BM.Core.DTO;
using BM.Core.Repositories;
using BM.Data;
using BM.Data.Repositories;
using BM.Services;
using Microsoft.EntityFrameworkCore;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var conString = builder.Configuration.GetConnectionString("Local") ??
     throw new InvalidOperationException("Connection string 'BookManagement Db' not found.");

builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(conString), ServiceLifetime.Transient);

builder.Services.AddTransient<IRepositoryManager, RepositoryManager>();
builder.Services.AddTransient<IServiceManager, ServiceManager>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    options.IgnoreObsoleteActions();
    options.IgnoreObsoleteProperties();
    options.CustomSchemaIds(type => type.FullName);
});

builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.User.Identity?.Name ?? "anonymous",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 10,
                Window = TimeSpan.FromMinutes(1)
            }));
});

var app = builder.Build();

app.MapGet("/api/books", async (IServiceManager services) =>
{
    try
    {
        return Results.Ok(await services.BookServices.GetAllBooks());
    }
    catch (Exception)
    {
        return Results.StatusCode(StatusCodes.Status500InternalServerError);
    }
});

app.MapGet("api/books/{id}", async (int id, IServiceManager services) =>
{
    try
    {
        return Results.Ok(await services.BookServices.GetBookById(id));
    }
    catch (Exception)
    {
        return Results.StatusCode(StatusCodes.Status500InternalServerError);
    }
});

app.MapPost("/api/books", async (BookDTO dto, IServiceManager services) =>
{
    try
    {
        if (dto == null) return Results.BadRequest();
        await services.BookServices.AddBook(dto!);

        return Results.Ok(dto);
    }
    catch (Exception)
    {
        return Results.StatusCode(StatusCodes.Status500InternalServerError);
    }
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(options =>
{
    options.AllowAnyHeader()
    .AllowAnyOrigin().AllowAnyMethod();
});
app.UseAuthorization();

app.MapControllers();

app.Run();
