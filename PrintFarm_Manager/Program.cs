using Data_PrintFarm_Manager;
using Microsoft.EntityFrameworkCore;
using PrintFarm_Manager_Core.Interfaces;
using PrintFarm_Manager_Core.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<PrintFarmDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IPrinterService, PrinterService>();
builder.Services.AddScoped<IPrintJobService, PrintJobService>();
builder.Services.AddScoped<IFilamentService, FilamentService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Printer/Index");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Printer}/{action=Index}/{id?}");

app.Run();
