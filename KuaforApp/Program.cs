using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using KuaforApp.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure PostgreSQL connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Add HttpClient factory
builder.Services.AddHttpClient();

// Configure CORS for AI service
builder.Services.AddCors(options =>
{
    options.AddPolicy("AIServicePolicy",
        builder =>
        {
            builder.WithOrigins("http://localhost:5000")
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Use CORS before authentication and authorization
app.UseCors("AIServicePolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

// Rolleri ve admin kullanıcılarını başlat
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // Rolleri oluştur (eğer yoksa)
    string[] roleNames = { "Admin", "User" };
    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    // Admin kullanıcı bilgileri
    var adminUsers = new List<(string FirstName, string LastName, string Email, string Password)>
    {
        ("Ali", "Aydın", "B221210373@sakarya.edu.tr", "Test.1234"),
        ("Mustafa", "Erdoğan", "B221210308@sakarya.edu.tr", "Test.1234")
    };

    // Her bir admin kullanıcısı için kontrol ve güncelleme işlemi
    foreach (var adminData in adminUsers)
    {
        // Kullanıcıyı e-posta adresine göre kontrol et
        var existingUser = await userManager.FindByEmailAsync(adminData.Email);

        if (existingUser != null)
        {
            // Kullanıcı zaten varsa, rolünü kontrol et
            var userRoles = await userManager.GetRolesAsync(existingUser);
            
            if (!userRoles.Contains("Admin"))
            {
                // Mevcut rollerini kaldır
                await userManager.RemoveFromRolesAsync(existingUser, userRoles);
                
                // Admin rolünü ekle
                await userManager.AddToRoleAsync(existingUser, "Admin");
                
                // Role property'sini güncelle
                existingUser.Role = UserRole.Admin;
                await context.SaveChangesAsync();
                
                Console.WriteLine($"Kullanıcı {existingUser.Email} admin rolüne yükseltildi.");
            }
        }
        else
        {
            // Yeni admin kullanıcısı oluştur
            var adminUser = new ApplicationUser
            {
                UserName = adminData.Email,
                Email = adminData.Email,
                FirstName = adminData.FirstName,
                LastName = adminData.LastName,
                Role = UserRole.Admin,
                CreatedAt = DateTime.UtcNow,
                EmailConfirmed = true // Admin e-postalarını otomatik onayla
            };

            var result = await userManager.CreateAsync(adminUser, adminData.Password);
            if (result.Succeeded)
            {
                // Admin rolünü ata
                await userManager.AddToRoleAsync(adminUser, "Admin");
                Console.WriteLine($"Yeni admin kullanıcısı oluşturuldu: {adminUser.Email}");
            }
            else
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                Console.WriteLine($"Kullanıcı oluşturma hatası {adminUser.Email}: {errors}");
            }
        }
    }
}

app.Run();
