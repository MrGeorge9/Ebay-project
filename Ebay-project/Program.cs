using Ebay_project.Context;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
var logger = new LoggerConfiguration()
  .ReadFrom.Configuration(builder.Configuration)
  .Enrich.FromLogContext()
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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
