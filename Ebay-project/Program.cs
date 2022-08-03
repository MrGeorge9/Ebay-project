using Ebay_project.Context;
using Ebay_project.Models;
using Ebay_project.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
MapSecretsToEnvVariables();
var logger = new LoggerConfiguration()
  .ReadFrom.Configuration(builder.Configuration)
  .CreateLogger();

if (env != null && env.Equals("Development"))
{
    builder.Services.AddDbContext<ApplicationContext>(dbBuilder => dbBuilder.UseSqlServer(builder.Configuration.GetConnectionString("LocalDB")));
}
if (env != null && env.Equals("Production"))
{
    var connectionString = builder.Configuration.GetConnectionString("AzureSql");
    var sb = new SqlConnectionStringBuilder(connectionString);
    sb.UserID = builder.Configuration["AzureUser"];
    sb.Password = builder.Configuration["AzureSqlPassword"];
    builder.Services.AddDbContext<ApplicationContext>(builder => builder.UseSqlServer(sb.ConnectionString));
}

// Add services to the container.
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("TokenGenerationKey")))
        };
    });
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IAuthService, JwtService>();

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

app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints => endpoints.MapControllers());

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

FillDatabaseIfEmpty(app);

app.Run();

static void MapSecretsToEnvVariables()
{
    var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
    foreach (var child in config.GetChildren())
    {
        Environment.SetEnvironmentVariable(child.Key, child.Value);
    }
}

static void FillDatabaseIfEmpty(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

    if (db.Users.IsNullOrEmpty())
    {
        User user = new User()
        {
            Name = "George",
            Password = "Uhorka",
            Wallet = 1500,
            Role = "Admin"
        };

        User user2 = new User()
        {
            Name = "James",
            Password = "Salama",
            Wallet = 220,
            Role = "Buyer"
        };
        db.Users.Add(user);
        db.Users.Add(user2);
        db.SaveChanges();
    }
}