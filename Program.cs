
using Microsoft.EntityFrameworkCore;
using UniversityDbApp.Data;

var builder = WebApplication.CreateBuilder(args);

// MVC жүйесін қосу
builder.Services.AddControllersWithViews();

// SQL Server дерекқорына қосылу
var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// Қосымшаны құрастыру
var app = builder.Build();

// HTTP сұраныстарын өңдеу
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Басты маршрут
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
