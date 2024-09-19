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
    option.Conventions.AuthorizeFolder("/");
    option.Conventions.AllowAnonymousToPage("/Auth/Welcome");
    option.Conventions.AllowAnonymousToPage("/Auth/Register");
    option.Conventions.AllowAnonymousToPage("/Auth/Login");
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Welcome"; 
        options.AccessDeniedPath = "/Auth/AccessDenied";
        options.Cookie.IsEssential = true;
        options.Cookie.HttpOnly = true;
        options.SlidingExpiration = false;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(1);
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
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.MapGet("/", context =>
{
    if (context.User.Identity != null && context.User.Identity.IsAuthenticated)
        context.Response.Redirect("/Index");
    else
        context.Response.Redirect("/Auth/Welcome");

    return Task.CompletedTask;
});

app.Run();
