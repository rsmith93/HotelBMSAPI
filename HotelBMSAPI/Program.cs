using System;
using System.Text.Json.Serialization;
using HotelBMSData.Context;
using HotelBMSRepository;
using HotelBMSRepository.Interfaces;
using HotelBMSServices;
using HotelBMSServices.Interfaces;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<DataContext>(options =>
        options.UseSqlite("Data Source=Data/hotels.db"));
}
else
{
    var dbPath = Path.Combine(
        Environment.GetEnvironmentVariable("HOME") ?? "",
        "site", "wwwroot", "hotels.db");

    builder.Services.AddDbContext<DataContext>(options =>
        options.UseSqlite($"Data Source={dbPath}"));
}

builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IHotelRepository, HotelRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IDbSeedRepository, DbSeedRepository>();
builder.Services.AddScoped<IHotelBMSService, HotelBMSService>();


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.ExampleFilters();
});

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.
//if (builder.Environment.IsDevelopment())
//{  
//    app.MapControllerRoute(
//        name: "testreset",
//        pattern: "api/Test/db/{action=reset}");

//    app.MapControllerRoute(
//        name: "testseed",
//        pattern: "api/Test/db/{action=seed}");
//}


using (var scope = app.Services.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<DataContext>();

        var fullPath = db.Database.GetDbConnection().DataSource;
        var directory = Path.GetDirectoryName(fullPath);

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        db.Database.EnsureCreated();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"DB Init failed: {ex.Message}");
    }
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


