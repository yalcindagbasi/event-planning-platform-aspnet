using Microsoft.EntityFrameworkCore;
using SmartEventPlanningPlatform.Data;
using SmartEventPlanningPlatform.Services;
using project.web.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddScoped<AuthenticationService>();
builder.Services.AddScoped<MessageService>();
builder.Services.AddScoped<AdminService>();
builder.Services.AddScoped<EventService>();
builder.Services.AddScoped<GamificationService>();
builder.Services.AddScoped<UserService>(); // Register UserService
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options =>
    {
        options.LoginPath = "/Account/Login"; // Giriþ sayfasý
        options.AccessDeniedPath = "/Account/AccessDenied"; // Yetkisiz eriþim
    });


// Add services to the container.
builder.Services.AddControllersWithViews();

// Add the DataSeeder before the app is built

var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseAuthentication(); // Kullanýcý kimlik doðrulama
app.UseAuthorization();  // Yetkilendirme kontrolü

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
