using HRManagment.Mappings;
using HRManagment.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using HRManagment.ViewModels;
using HRManagment.Services;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddScoped<DropDownService>();
builder.Services.AddScoped<IEmployeeService, EmployeeServices>();
builder.Services.AddScoped<IEmployeeValidationService, EmployeeValidationService>();

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Logging.ClearProviders();
builder.Logging.AddDebug();
builder.Logging.AddConsole();

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<HRManagmentContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
