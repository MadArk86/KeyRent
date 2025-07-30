using KeyRent.Data;
using KeyRent.Properties.DAL;
using KeyRent.Repositories;
using KeyRent.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using FluentValidation.AspNetCore;
using KeyRent.Mapping;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Konfiguracja dostępu do bazy danych
builder.Services.AddDbContext<RentContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(options =>
    {
        options.AddProfile<UserProfile>();
    });

// Rejestracja serwisów aplikacji
builder.Services.AddScoped<IUserRepository, UserRepository>(); // Rejestracja repozytorium
builder.Services.AddScoped<UserService>(); // Rejestracja serwisu

// Rejestracja FluentValidation
builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<UserViewModelValidator>());

// Rejestracja ASP.NET Core Identity
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    // Opcjonalna konfiguracja opcji Identity
    options.SignIn.RequireConfirmedAccount = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<RentContext>()
.AddDefaultUI() // Dodaje domyślne widoki dla logowania, rejestracji itd.
.AddDefaultTokenProviders();


builder.Services.AddControllersWithViews()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);
builder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "Resources";
});

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("en-US"),
        new CultureInfo("de-DE")
    };
    options.DefaultRequestCulture = new RequestCulture("en-US");
    options.SupportedUICultures = supportedCultures;
});

// Rejestracja Razor Pages dla Identity
builder.Services.AddRazorPages();

// Dodanie obsługi wyjątków bazy danych w trybie deweloperskim
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();
app.UseRequestLocalization();


// Inicjalizacja bazy danych, jeśli nie istnieje
InitializeDatabaseIfNeededAsync(app);

// Konfiguracja obsługi błędów
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Szczegółowe błędy w trybie deweloperskim
}
else
{
    app.UseExceptionHandler("/Home/Error"); // Globalna obsługa błędów w trybie produkcyjnym
    app.UseHsts(); // Wymuszanie HTTPS
}

app.UseHttpsRedirection(); // Automatyczne przekierowanie na HTTPS
app.UseStaticFiles(); // Obsługa plików statycznych (CSS, JS, obrazki)
app.Use(async (context, next) =>
{
    if (context.Request.Path.Equals("/Account/Logout", StringComparison.OrdinalIgnoreCase) &&
        context.Request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase))
    {
        context.Response.Redirect("/Identity/Account/Logout");
        return;
    }
    await next();
});

app.UseRouting(); // Włączenie routingu

// Middleware dla uwierzytelniania i autoryzacji
app.UseAuthentication(); // Dodanie obsługi logowania i autoryzacji
app.UseAuthorization(); // Obsługuje autoryzację użytkowników (jeśli wymagane)

// Mapowanie tras kontrolerów
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Mapowanie tras Razor Pages (dla Identity)
app.MapRazorPages();

app.Run(); // Uruchomienie aplikacji

// Metoda inicjalizująca bazę danych, jeżeli nie istnieje
async Task InitializeDatabaseIfNeededAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var serviceProvider = scope.ServiceProvider;

    try
    {
        var context = serviceProvider.GetRequiredService<RentContext>();
        context.Database.Migrate(); // Automatyczne zastosowanie migracji

        // Inicjalizacja danych (jeśli metoda istnieje)
        DbInitializer.Initialize(context);

        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        string[] roleNames = { "Admin", "User", "Manager" };

        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        //Przypisanie
        var userAdmin = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var user = await userAdmin.FindByEmailAsync("test@o2.pl");

        if (user != null && !await userAdmin.IsInRoleAsync(user, "Admin"))
        {
            await userAdmin.AddToRoleAsync(user, "Admin");
        }
    }


    catch (Exception ex)
    {
        var logger = serviceProvider.GetRequiredService<ILogger<Program>>(); // Pobranie loggera
        logger.LogError(ex, "Wystąpił błąd podczas inicjalizacji bazy danych.");
    }


    
}
