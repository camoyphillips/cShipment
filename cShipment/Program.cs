using cShipment;
using cShipment.Data;
using cShipment.Controllers;
using cShipment.Interfaces;
using cShipment.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity; 

var builder = WebApplication.CreateBuilder(args);

// Add services to the container (MVC + API)
builder.Services.AddControllersWithViews();

// --- Identity Services ---
// Configure EF Core with SQL Server for Identity
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Add Default Identity
// Uses IdentityUser by default
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
// --- End Identity Services ---


// Register custom service interfaces and implementations
builder.Services.AddScoped<IDriverService, DriverService>();
builder.Services.AddScoped<ITruckService, TruckService>();
builder.Services.AddScoped<IShipmentService, ShipmentService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IDriverShipmentService, DriverShipmentService>();


// Swagger API Explorer
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    
    app.UseHsts();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "cShipment API v1");
        options.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.UseAuthentication(); 


app.UseAuthorization(); 

// Map Razor Pages for Identity UI (needed for /Identity/Account/Login, /Register etc.)
app.MapRazorPages(); 

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();