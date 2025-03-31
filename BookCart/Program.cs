using BookCart.Data;
using BookCart.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BookCartDbContext>(
    o=>o.UseSqlServer(builder.Configuration
    .GetConnectionString("BookCartConnectionString")));

builder.Services.AddDistributedMemoryCache();   // lưu cache trong bộ nhớ (Session sử dụng)
builder.Services.AddSession(cfg => {
    cfg.Cookie.Name = "HutechAspNetCore";
    cfg.IdleTimeout = TimeSpan.FromMinutes(60);
});

// Đăng ký IHttpContextAccessor vào container DI
builder.Services.AddHttpContextAccessor();

// Add services to the container.
builder.Services.AddControllersWithViews();
//builder.Services.AddRazorPages()
//    .AddRazorRuntimeCompilation();

builder.Services.AddScoped<IPayPalService, PayPalService>();

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

app.UseAuthorization();

app.UseSession();   // Đăng ký middleware session vào pipeline

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.Run();
