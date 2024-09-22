using Delivery_Application;
using Delivery_Application_Contracts.Delivery;
using Delivery_Application_Contracts.Destination;
using Delivery_Application_Contracts.User;
using Delivery_Domain.AuthAgg;
using Delivery_Domain.DeliveryAgg;
using Delivery_Domain.DestinationAgg;
using Delivery_Infrastructure.Context;
using Delivery_Infrastructure.DateConversionService;
using Delivery_Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages(option =>
{
    option.Conventions.AuthorizeFolder("/Account");
    option.Conventions.AllowAnonymousToPage("/Account/Welcome");
    option.Conventions.AllowAnonymousToPage("/Account/Register");
    option.Conventions.AllowAnonymousToPage("/Account/Login");
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Welcome";
        options.LogoutPath = "/Account/Welcome"; 
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.Cookie.IsEssential = true;
        options.Cookie.HttpOnly = true;
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    });

builder.Services.AddTransient<IDestinationRepository,DestinationRepository>();
builder.Services.AddTransient<IDestinationApplication,DestinationApplication>();
builder.Services.AddTransient<IDeliveryRepository,DeliveryRepository>();
builder.Services.AddTransient<IDeliveryApplication,DeliveryApplication>();
builder.Services.AddTransient<IDateConversionService,DateConversionService>();
builder.Services.AddTransient<IUserApplication, UserApplication>();

builder.Services.AddIdentity<User, IdentityRole>(option =>
{
    option.Password.RequiredLength = 5;
    option.Password.RequireNonAlphanumeric = false;
    option.Password.RequireUppercase = false;
    option.Password.RequireLowercase = false;
    option.Password.RequireDigit = false;

}).AddEntityFrameworkStores<DeliveryContext>().AddDefaultTokenProviders();

builder.Services.AddDbContext<DeliveryContext>(options =>
options.UseSqlServer(builder.Configuration
.GetConnectionString("Delivery_App")));

var app = builder.Build();

// Creates or ensures the existence of the MainContext database.
using(var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<DeliveryContext>();
    context.Database.EnsureCreated();
    context.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
