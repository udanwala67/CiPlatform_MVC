using CiPlatform.Entitites.Data;
using CiPlatform.Repository.Interface;
using CiPlatform.Repository.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<CiContext>();
builder.Services.AddScoped<ICiRepository, CiRepository>();
builder.Services.AddScoped<IStoryRepository, StoryRepository>();
builder.Services.AddTransient<EmailServices>();
builder.Services.AddScoped<IUserDetailsRepository, UserDetailsRepository>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();    
/*builder.Services.AddControllersWithViews();*/
builder.Services.AddSession();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(option=>
            {
                option.ExpireTimeSpan = TimeSpan.FromMinutes(60 * 1);
                option.LoginPath = "/Home/login";
                option.AccessDeniedPath = "/Home/login";
            });
builder.Services.AddSession(option =>
{
    option.IdleTimeout = TimeSpan.FromMinutes(50 * 1);
    option.Cookie.HttpOnly = true;  
    option.Cookie.IsEssential = true;
});

builder.Services.AddControllersWithViews()
.AddNewtonsoftJson(options =>
options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession(); 
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=login}/{id?}");

app.Run();
