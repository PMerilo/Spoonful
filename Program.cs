using Microsoft.AspNetCore.Identity;
using Spoonful.Models;
using Microsoft.EntityFrameworkCore;
using Spoonful.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Configuration;
using Spoonful.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<AuthDbContext>();
builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
   opt.TokenLifespan = TimeSpan.FromHours(2));

//Services
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<MenuItemService>();
builder.Services.AddScoped<MealKitService>();
builder.Services.AddScoped<RecipeService>();
builder.Services.AddScoped<EmailService>();


builder.Services.AddIdentity<CustomerUser, IdentityRole>().AddEntityFrameworkStores<AuthDbContext>().AddDefaultTokenProviders();
builder.Services.ConfigureApplicationCookie(config =>
{
    config.LoginPath = "/Account/Login";
});
builder.Services.AddAuthentication("MyCookieAuth").AddCookie("MyCookieAuth", options =>
{
    options.Cookie.Name = "MyCookieAuth";
});

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); // TODO: CHECK IF REQUIRED
builder.Services.AddDistributedMemoryCache(); 
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(30);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithRedirects("/errors/{0}");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseSession();



app.MapRazorPages();

app.Run();
