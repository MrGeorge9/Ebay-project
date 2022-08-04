using Ebay_project.Context;
using Ebay_project.Exntensions;
using Ebay_project.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
Extensions.MapSecretsToEnvVariables();

if (env != null && env.Equals("Development"))
{
    builder.Services.AddDbContext<ApplicationContext>(dbBuilder => dbBuilder.UseSqlServer(builder.Configuration.GetConnectionString("LocalDB")));

    var logger = new LoggerConfiguration()
      .ReadFrom.Configuration(builder.Configuration)
      .Enrich.FromLogContext()
      .WriteTo.MSSqlServer(builder.Configuration.GetConnectionString("LocalDB"), autoCreateSqlTable: true, tableName: "Logs")
      .CreateLogger();
    builder.Logging.ClearProviders();
    builder.Logging.AddSerilog(logger);
}

if (env != null && env.Equals("Production"))
{
    var connectionString = builder.Configuration.GetConnectionString("AzureSql");
    var sb = new SqlConnectionStringBuilder(connectionString);
    sb.UserID = builder.Configuration["AzureUser"];
    sb.Password = builder.Configuration["AzureSqlPassword"];
    builder.Services.AddDbContext<ApplicationContext>(builder => builder.UseSqlServer(sb.ConnectionString));

    var logger = new LoggerConfiguration()
     .ReadFrom.Configuration(builder.Configuration)
     .Enrich.FromLogContext()
     .WriteTo.MSSqlServer(sb.ConnectionString, autoCreateSqlTable: true, tableName: "Logs")
     .CreateLogger();
    builder.Logging.ClearProviders();
    builder.Logging.AddSerilog(logger);
}

// Add services to the container.
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
builder.Services.AddSwaggerGen(options => {
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Ebay API",
        Description = "An ASP.NET Core Web API for online webshop Ebay project"
    });
});

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

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = String.Empty;
});

Extensions.FillDatabaseIfEmpty(app);

app.Run();


