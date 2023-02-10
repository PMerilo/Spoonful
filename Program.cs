using Microsoft.AspNetCore.Identity;
using Spoonful.Models;
using Microsoft.EntityFrameworkCore;
using Spoonful.Services;
using Spoonful.Utility;
using Stripe;

using Microsoft.AspNetCore.Identity.UI.Services;
using System.Configuration;
using Spoonful.Settings;
using Spoonful.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages(options =>
{
	options.Conventions.AuthorizeFolder("/Admin", "RequireAdministratorRole");

	//Allow Anonymous
	options.Conventions.AllowAnonymousToPage("/Index");
    options.Conventions.AllowAnonymousToPage("/Error");
    options.Conventions.AllowAnonymousToPage("/NotificationTester");
    options.Conventions.AllowAnonymousToPage("/notificationHub");



});

//SignalR
builder.Services.AddSignalR(hubOptions =>
{
    hubOptions.EnableDetailedErrors = true;
});
builder.Services.AddSingleton(typeof(IUserIdProvider), typeof(MyUserIdProvider));

//Database
builder.Services.AddDbContext<AuthDbContext>();

//Stripe
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
builder.Services.AddControllers();

//Identity Tokens
builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
   opt.TokenLifespan = TimeSpan.FromHours(2));


//Services
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<MenuItemService>();
builder.Services.AddScoped<VoucherService>();
builder.Services.AddScoped<MealKitService>();
builder.Services.AddScoped<RecipeService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<BlogService>();
builder.Services.AddScoped<MealOrderService>();
builder.Services.AddScoped<InvoiceMealKitService>();
//Logs Services
builder.Services.AddScoped<MealKitSubscriptionLogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<CustomerUserService>();
//EmailConfig and service
var emailConfig = builder.Configuration
        .GetSection("EmailConfiguration")
        .Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);
builder.Services.AddScoped<IEmailService, EmailService>();

var GoogleAddressAutoCorrect = builder.Configuration
        .GetSection("GoogleAddressAutoCorrect")
        .Get<GoogleAddressAutoCorrectConfiguration>();

builder.Services.AddSingleton(GoogleAddressAutoCorrect);

//Identity
builder.Services.AddIdentity<CustomerUser, IdentityRole>().AddEntityFrameworkStores<AuthDbContext>().AddDefaultTokenProviders();
builder.Services.ConfigureApplicationCookie(config =>
{
    config.LoginPath = "/Account/Login";
    config.LogoutPath = "/Account/Logout";
    config.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    config.SlidingExpiration = true;
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

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();

	options.AddPolicy("RequireAdministratorRole",
		 policy => policy.RequireRole(Roles.Admin, Roles.RootUser));
    options.AddPolicy("RequireCustomerRole",
         policy => policy.RequireRole(Roles.Customer, Roles.RootUser));
    options.AddPolicy("RequireDriverRole",
         policy => policy.RequireRole(Roles.Driver, Roles.RootUser));
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

string key = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();
StripeConfiguration.ApiKey = key;

app.UseAuthentication();

app.UseAuthorization();

app.UseSession();
app.MapControllers();


app.MapRazorPages();
app.MapHub<NotificationHub>("/notificationHub");

app.Run();
