using LungCancer.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1️⃣ Servisleri ekle
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient(); // IHttpClientFactory için
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



builder.Services.AddDistributedMemoryCache();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();



builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


 // Middleware olarak ekle


// 2️⃣ Uygulamayı build et
var app = builder.Build();
app.UseSession();
// 3️⃣ Middleware ayarları
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// 4️⃣ Controller route ayarları
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Predict controller için ayrı route (opsiyonel)
// app.MapControllerRoute(
//     name: "predict",
//     pattern: "Predict/{action=Index}/{id?}",
//     defaults: new { controller = "Predict", action = "Index" }
// );

app.Run();
