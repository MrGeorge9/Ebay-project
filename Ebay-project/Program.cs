using Ebay_project.Context;
using Ebay_project.Exntensions;
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
Extensions.MapSecretsToEnvVariables();
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
builder.Services.AddTransient<IBuyerService, BuyerService>();
builder.Services.AddTransient<IAdminService, AdminService>();
builder.Services.AddTransient<IAuthService, JwtService>();
builder.Services.AddTransient<IPublicService, PublicService>();

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

Extensions.FillDatabaseIfEmpty(app);

app.Run();
