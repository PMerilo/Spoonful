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
using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages(options =>
{
	options.Conventions.AuthorizeFolder("/Admin", "RequireAdministratorRole");

	//Allow Anonymous
	options.Conventions.AllowAnonymousToPage("/Index");
    options.Conventions.AllowAnonymousToPage("/Error");
    options.Conventions.AllowAnonymousToPage("/NotificationTester");
    options.Conventions.AllowAnonymousToPage("/Account/CreateAdmin");
    options.Conventions.AllowAnonymousToPage("/Account/CreateDriver");
    options.Conventions.AllowAnonymousToPage("/Account/ExternalLogin");
    options.Conventions.AllowAnonymousToPage("/Account/2FA");
    options.Conventions.AllowAnonymousToPage("/notificationHub");





});
// Add ToastNotification
builder.Services.AddNotyf(config =>
{
	config.DurationInSeconds = 5;
	config.IsDismissable = true;
	config.Position = NotyfPosition.TopRight;
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
builder.Services.AddAntiforgery(options => options.HeaderName = "XSRF-TOKEN");

//Identity Tokens
builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
   opt.TokenLifespan = TimeSpan.FromHours(2));


//Services
builder.Services.AddScoped<TicketingService>();

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
builder.Services.AddScoped<MessagingService>();
builder.Services.AddScoped<VoucherEmailService>();
builder.Services.AddScoped<DeliveryService>();
builder.Services.AddScoped<CustomerUserService>();
builder.Services.AddScoped<DiaryService>();
builder.Services.AddScoped<ShoppingListService>();

//EmailConfig and service
//builder.Services.Configure<EmailConfiguration>(builder.Configuration.GetSection("EmailConfiguration"));
var emailConfig = builder.Configuration
        .GetSection("EmailConfiguration")
        .Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);
builder.Services.AddScoped<IEmailService, EmailService>();
var smsConfig = builder.Configuration
        .GetSection("SMSConfiguration")
        .Get<SMSoptions>();
builder.Services.AddSingleton(smsConfig);
builder.Services.AddScoped<ISmsSender, SMSSender>();

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
    config.AccessDeniedPath = "/error/403";
    config.SlidingExpiration = true;
});


builder.Services.AddAuthentication().AddGoogle(googleOptions =>
{
    googleOptions.ClientId = builder.Configuration["Authentication:Google:client_id"];
    googleOptions.ClientSecret = builder.Configuration["Authentication:Google:client_secret"];
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

builder.Services.Configure<IdentityOptions>(options =>
{
    // Default Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // Default User settings.
    options.User.RequireUniqueEmail = true;

    options.SignIn.RequireConfirmedEmail = true;

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

app.UseNotyf();
app.MapRazorPages();
app.MapHub<NotificationHub>("/notificationHub");
app.MapHub<ChatHub>("/chatHub");

app.Run();
