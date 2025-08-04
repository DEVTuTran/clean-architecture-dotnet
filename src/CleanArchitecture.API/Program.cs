using CleanArchitecture.Infrastructure;
using CleanArchitecture.Application.Mappings;
using Microsoft.EntityFrameworkCore;
using CleanArchitecture.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CleanArchitecture.API.Middleware;
using CleanArchitecture.Infrastructure.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Clean Architecture API",
        Version = "v1",
        Description = "A .NET 8 Clean Architecture API with Firebase Authentication",
        Contact = new OpenApiContact
        {
            Name = "Tu Tran",
            Email = "dev.tutran@gmail.com",
            Url = new Uri("https://github.com/DevTranVanTu")
        },
        License = new OpenApiLicense
        {
            Name = "MIT",
            Url = new Uri("https://github.com/DevTranVanTu")
        }
    });

    // Add JWT Bearer authentication
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });

    // Add Firebase ID Token authentication
    c.AddSecurityDefinition("Firebase", new OpenApiSecurityScheme
    {
        Description = "Firebase ID Token in header. Example: \"X-Firebase-ID-Token: {token}\"",
        Name = "X-Firebase-ID-Token",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Firebase"
                }
            },
            new string[] {}
        }
    });
});

// Add MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(CleanArchitecture.Application.ApplicationAssembly).Assembly);
});

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(OrganizationMappingProfile), typeof(UserMappingProfile));

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    ));

// Configure JWT Authentication
var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKey = jwtSettings["SecretKey"] ?? "your-secret-key-here-minimum-16-characters";
var issuer = jwtSettings["Issuer"] ?? "CleanArchitecture";
var audience = jwtSettings["Audience"] ?? "CleanArchitecture";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

// Add Infrastructure services
builder.Services.AddInfrastructure();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Clean Architecture API v1");
        c.RoutePrefix = "api-docs";

        // Add basic authentication for Swagger UI in Development
        var swaggerConfig = builder.Configuration.GetSection("Swagger");
        var username = swaggerConfig["UserName"];
        var password = swaggerConfig["Password"];

        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
        {
            c.DocumentTitle = "Clean Architecture API - Development";
            c.ConfigObject.AdditionalItems.Add("persistAuthorization", "true");
            c.HeadContent = @"
                <script>
                    (function() {
                        const username = '" + username + @"';
                        const password = '" + password + @"';
                        
                        function checkAuth() {
                            if (!sessionStorage.getItem('swagger-auth')) {
                                const inputUsername = prompt('Enter username:');
                                if (inputUsername === null) {
                                    window.location.href = '/';
                                    return;
                                }
                                
                                const inputPassword = prompt('Enter password:');
                                if (inputPassword === null) {
                                    window.location.href = '/';
                                    return;
                                }
                                
                                if (inputUsername === username && inputPassword === password) {
                                    sessionStorage.setItem('swagger-auth', 'true');
                                } else {
                                    alert('Invalid credentials');
                                    window.location.href = '/';
                                }
                            }
                        }
                        
                        // Run immediately
                        checkAuth();
                        
                        // Also run when DOM is ready
                        if (document.readyState === 'loading') {
                            document.addEventListener('DOMContentLoaded', checkAuth);
                        } else {
                            checkAuth();
                        }
                    })();
                </script>";
        }
    });
}
else if (app.Environment.IsEnvironment("Staging"))
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Clean Architecture API v1 - Staging");
        c.RoutePrefix = "api-docs";
        c.DocumentTitle = "Clean Architecture API - Staging";
    });
}

app.UseHttpsRedirection();

// Add Firebase authentication middleware
app.UseFirebaseAuth();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Seed database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await DbInitializer.SeedAsync(context);
}

app.Run();