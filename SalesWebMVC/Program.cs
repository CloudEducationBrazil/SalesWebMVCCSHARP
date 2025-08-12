using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SalesWebMVC.Data;
using SalesWebMVC.Services;

using System.Globalization;
using Microsoft.AspNetCore.Localization;

var builder = WebApplication.CreateBuilder(args);

// Adicione o contexto do banco de dados usando MySQL
var connectionString = builder.Configuration.GetConnectionString("SalesWebMVCContext");

// Heleno
var enUS = new CultureInfo("en-US");

// Heleno
var localizationOptions = new RequestLocalizationOptions { 
        DefaultRequestCulture = new RequestCulture(enUS),
        SupportedCultures = new List<CultureInfo> {enUS},
        SupportedUICultures = new List<CultureInfo> {enUS}
};

builder.Services.AddDbContext<SalesWebMVCContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 34)), builder => 
    builder.MigrationsAssembly("SalesWebMVC")));
//options.UseSqlServer(builder.Configuration.GetConnectionString("SalesWebMVCContext") ?? throw new InvalidOperationException("Connection string 'SalesWebMVCContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<SeedService>();

builder.Services.AddScoped<SellerService>();

builder.Services.AddScoped<DepartmentService>();

var app = builder.Build();

// Heleno
app.UseRequestLocalization(localizationOptions);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else {
    // Cria escopo para resolver dependências
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;

        try
        {
            var seedService = services.GetRequiredService<SeedService>();
            seedService.Seed();
        }
        catch (Exception ex)
        {
            // Você pode logar o erro ou lançar novamente
            Console.WriteLine("Erro ao rodar SeedService: " + ex.Message);
        }
    }
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
