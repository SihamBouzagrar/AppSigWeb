using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReservationHotel.Data;
using ReservationHotel.Models;

var builder = WebApplication.CreateBuilder(args);

// Ajout des services
builder.Services.AddControllersWithViews(); // Si tu veux mixer API + MVC

// Database SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<ReservationHotel.Services.HotelService>();   // ← Important
builder.Services.AddScoped<ReservationHotel.Services.ReservationService>();
// Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Configuration des mots de passe (assouplie)
    options.Password.RequireDigit = false;                    // Pas besoin de chiffre
    options.Password.RequireLowercase = false;                // Pas besoin de minuscule
    options.Password.RequireUppercase = false;                // ← Supprimé (Uppercase)
    options.Password.RequireNonAlphanumeric = false;          // ← Supprimé (caractère spécial)
    options.Password.RequiredLength = 6;                      // Longueur minimale
    options.Password.RequiredUniqueChars = 1;                 // Moins strict

    // Autres options utiles
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = false;             // Pas de confirmation email pour le moment
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();   // ← Ajoute ceci
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

var app = builder.Build();

// ====================== SEED ADMIN PAR DÉFAUT ======================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        // Création des rôles
        if (!await roleManager.RoleExistsAsync("Admin"))
            await roleManager.CreateAsync(new IdentityRole("Admin"));

        if (!await roleManager.RoleExistsAsync("Client"))
            await roleManager.CreateAsync(new IdentityRole("Client"));

        // Création de l'Admin par défaut
        string adminEmail = "admin@hotel.com";
        var admin = await userManager.FindByEmailAsync(adminEmail);

        if (admin == null)
        {
            admin = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FullName = "Administrateur",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(admin, "Admin123!");
            
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "Admin");
                Console.WriteLine("✅ Admin par défaut créé : admin@hotel.com / Admin123!");
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erreur Seed : {ex.Message}");
    }
}
// ===================================================================

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();