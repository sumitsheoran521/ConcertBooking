using ConcertBooking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ConcertBooking.Domain.Models;
using ConcertBooking.Application.Services.Interfaces;
using ConcertBooking.Application.Services.Implementions;
using ConcertBooking.Application.Common;
using ConcertBooking.Infrastructure.Repository;
using Microsoft.AspNetCore.Identity.UI.Services;
using ConcertBooking.Application.Common.Utility;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddDefaultTokenProviders().AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddScoped<IVenueService, VenueService>();
builder.Services.AddScoped<IArtistService, ArtistService>();
builder.Services.AddScoped<IUtilityService, UtilityService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IConcertService, ConcertService>();
builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddScoped<IDbInitial, DbInitial>();
builder.Services.AddScoped<IEmailSender, EmailSender>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

builder.Services.AddRazorPages();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

DataSeed();

void DataSeed()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbSeedRepo = scope.ServiceProvider.GetRequiredService<IDbInitial>();
        dbSeedRepo.DataSeed();
    }
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

 app.Run();
