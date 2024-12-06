using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<StoreContext>(options => { options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")); });
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Seeding Data :
/*
 This code is responsible for seeding data into the database.
  It ensures the database is migrated to the latest version and then calls a method to seed initial data.

  If an exception occurs during the seeding process, it catches the exception,
   changes console colors to highlight the error,
   logs the exception, resets the colors,
   and rethrows the exception for further handling.
 */
try
{
// We use a 'using' here because we want the resources used disposed of when the scope ends , because we are using this service outside D.I.
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<StoreContext>();
    await context.Database.MigrateAsync();
    // At this stage we have a Database :
    await StoreContextSeed.SeedAsync(context);
}
catch (Exception e)
{
    Console.BackgroundColor = ConsoleColor.Red;
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine(e);
    Console.ResetColor();
    throw;
}

app.Run();