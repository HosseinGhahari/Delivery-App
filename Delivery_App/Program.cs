using Delivery_Application;
using Delivery_Application_Contracts.Delivery;
using Delivery_Application_Contracts.Destination;
using Delivery_Domain.DeliveryAgg;
using Delivery_Domain.DestinationAgg;
using Delivery_Infrastructure.Context;
using Delivery_Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddTransient<IDestinationRepository,DestinationRepository>();
builder.Services.AddTransient<IDestinationApplication,DestinationApplication>();
builder.Services.AddTransient<IDeliveryRepository,DeliveryRepository>();
builder.Services.AddTransient<IDeliveryApplication,DeliveryApplication>();


builder.Services.AddDbContext<DeliveryContext>(options =>
options.UseSqlServer(builder.Configuration
.GetConnectionString("Delivery_App")));

var app = builder.Build();

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

app.UseAuthorization();

app.MapRazorPages();

app.Run();
