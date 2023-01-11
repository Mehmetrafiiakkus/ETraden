using ETrade.Dal.Abstract;
using ETrade.Dal.Concrete;
using ETrade.Data.Context;
using ETrade.Data.Models.Identity;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddControllersWithViews();

//Dependency Injection
builder.Services.AddDbContext<ETradeContext>();
builder.Services.AddScoped<ICategoryDAl,CategoryDAL>();
builder.Services.AddScoped<IProductDAL, ProductDAL>();
builder.Services.AddScoped<IOrderDAL, OrderDAL>();
//Identity
builder.Services.AddIdentity<AppUser, AppRole>(options =>
{
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);//kitilendiğinde
    options.Lockout.MaxFailedAccessAttempts = 5;//kilitleme süresi ne kadar yanlış girdiği 
    options.Password.RequireNonAlphanumeric = false;//noktalama gibi şeyleri kaldırmak 
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 3;
}).AddEntityFrameworkStores<ETradeContext>()
.AddDefaultTokenProviders().AddRoles<AppRole>();//her kullsnıcı için süre 

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";//Giriş yapılmadıysa autr ile ilgili 
    options.AccessDeniedPath = "/";//Yetkisi yoksa
    options.Cookie = new CookieBuilder
    {
        Name = "AspN" +
        "587etCoreIdentityExampleCookie",
        HttpOnly = false,
        SameSite = SameSiteMode.Lax,//
        SecurePolicy = CookieSecurePolicy.Always//Https üzerinden
    };
    options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
});

//Add Session
builder.Services.AddSession();
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

app.UseRouting();

//Identity ve cookie için 
app.UseAuthentication();//Önce giriş kontrolü
app.UseAuthorization();//Sonra Yetki kontrolü

//Use Session
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
