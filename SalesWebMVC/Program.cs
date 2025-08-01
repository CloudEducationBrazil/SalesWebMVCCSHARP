using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SalesWebMVC.Data;

var builder = WebApplication.CreateBuilder(args);

// Adicione o contexto do banco de dados usando MySQL
var connectionString = builder.Configuration.GetConnectionString("SalesWebMVCContext");

builder.Services.AddDbContext<SalesWebMVCContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 34)), builder => 
    builder.MigrationsAssembly("SalesWebMVC")));
//options.UseSqlServer(builder.Configuration.GetConnectionString("SalesWebMVCContext") ?? throw new InvalidOperationException("Connection string 'SalesWebMVCContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<SeedService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else {
    // Cria escopo para resolver depend�ncias
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
            // Voc� pode logar o erro ou lan�ar novamente
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
