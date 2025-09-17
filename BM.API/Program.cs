using BM.Core.Model;
using BM.Core.Repositories;
using BM.Data;
using BM.Data.Repositories;
using BM.Services;
using Microsoft.EntityFrameworkCore;

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

var app = builder.Build();

app.MapGet("/api/books", async (IServiceManager services) =>
{
    try
    {
        //get all books
        List<Book> list = [..await services.BookServices.GetAllBooks()];
        return Results.Ok(list);
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

app.MapPost("/api/books", async (Book book, IServiceManager services) =>
{
    try
    {
        if (book == null) return Results.BadRequest();
        await services.BookServices.AddBook(book!);

        return Results.Ok(book);
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
