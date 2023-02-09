using Microsoft.AspNetCore.Identity;
using Spoonful.Models;
using Microsoft.EntityFrameworkCore;
using Spoonful.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<AuthDbContext>();

//Services
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<MenuItemService>();
builder.Services.AddScoped<VoucherService>();
builder.Services.AddScoped<MealKitService>();
builder.Services.AddScoped<RecipeService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<CustomerUserService>();
builder.Services.AddScoped<DiaryService>();
builder.Services.AddScoped<ShoppingListService>();

//builder.Services.Configure<EmailConfiguration>(builder.Configuration.GetSection("EmailConfiguration"));
var emailConfig = builder.Configuration
        .GetSection("EmailConfiguration")
        .Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);
builder.Services.AddScoped<IEmailService, EmailService>();


builder.Services.AddIdentity<CustomerUser, IdentityRole>().AddEntityFrameworkStores<AuthDbContext>();
builder.Services.ConfigureApplicationCookie(config =>
{
    config.LoginPath = "/User/Login";
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

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseSession();

app.UseStatusCodePagesWithReExecute("/Error");

app.MapRazorPages();

app.Run();
