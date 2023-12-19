using ElectronicsStore_eCommerce.Models;
using ElectronicsStore_eCommerce.Models.ContextFile;
using ElectronicsStore_eCommerce.Services;
using ElectronicsStore_eCommerce.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<StoreDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("StoreDBContext")));

builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
{
    // Configure password requirements if needed
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;

    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<StoreDBContext>()
.AddDefaultTokenProviders();

builder.Services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest)
   .AddRazorPagesOptions(options =>
   {
       options.Conventions.AuthorizeAreaFolder("Identity", "/User/Manage");
       options.Conventions.AuthorizeAreaPage("Identity", "/User/Logout");
   });

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/User/Login";
    options.LogoutPath = $"/User/Logout";
    options.AccessDeniedPath = $"/User/AccessDenied";
});

builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IOrderManagementService, OrderManagementService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IProductPageService, ProductPageService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IShippingInfoService, ShippingInfoService>();
builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Ensure the roles are created during application startup
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
    await SeedRolesAsync(roleManager);
}

app.MapControllerRoute(
    name: "Admin",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

// Seed roles method
async Task SeedRolesAsync(RoleManager<IdentityRole<int>> roleManager)
{
    string roleName = "Admin";

    if (!await roleManager.RoleExistsAsync(roleName))
    {
        await roleManager.CreateAsync(new IdentityRole<int> { Name = roleName });
    }
}
