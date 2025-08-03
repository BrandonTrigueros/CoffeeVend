using CoffeeDispenserWebApp.Repositories;
using CoffeeDispenserWebApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Register repositories
builder.Services.AddSingleton<ICoffeeRepository, CoffeeRepository>();
builder.Services.AddSingleton<ICoinRepository, CoinRepository>();

// Register services
builder.Services.AddScoped<IChangeCalculatorService, ChangeCalculatorService>();

// Add anti-forgery token for security
builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Coffee}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "coinList",
    pattern: "coins",
    defaults: new { controller = "Coffee", action = "CoinList" });

app.Run();
