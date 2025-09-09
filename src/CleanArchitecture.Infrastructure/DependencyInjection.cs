using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CleanArchitecture.Domain.Repositories;
using CleanArchitecture.Infrastructure.Data;
using CleanArchitecture.Infrastructure.Repositories;
using CleanArchitecture.Application.Interfaces.Storage;
using CleanArchitecture.Infrastructure.Services.AWS.Storage;
using CleanArchitecture.Application.Interfaces.Security;
using CleanArchitecture.Infrastructure.Services.Security;
using CleanArchitecture.Application.Interfaces.Email;
using CleanArchitecture.Infrastructure.Services.Email;
using CleanArchitecture.Application.Interfaces.AWS;
using CleanArchitecture.Infrastructure.Services.AWS.Lambda;
using CleanArchitecture.Application.Interfaces.Office;
using CleanArchitecture.Infrastructure.Services.Office;
using CleanArchitecture.Application.Interfaces.Payments;
using CleanArchitecture.Infrastructure.Services.Payments;
using Amazon.S3;
using Amazon.Lambda;
using CleanArchitecture.Infrastructure.Services.Email.Models;
using SendGrid;
using Microsoft.Extensions.Options;
using Amazon;
using CleanArchitecture.Application.Interfaces.Auth;
using CleanArchitecture.Infrastructure.Services.Auth;
using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Infrastructure.Services;

namespace CleanArchitecture.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase("CleanArchitectureDb"));

        services.AddScoped<IProductRepository, ProductRepository>();

        // Configure AWS S3 Options and Services
        services.Configure<AwsS3Options>(configuration.GetSection("AWS"));
        services.AddSingleton<IAmazonS3>(sp =>
        {
            var awsOptions = sp.GetRequiredService<IOptions<AwsS3Options>>().Value;
            return new AmazonS3Client(awsOptions.AccessKey, awsOptions.SecretAccessKey, RegionEndpoint.GetBySystemName(awsOptions.Region));
        });
        services.AddScoped<IS3, S3>();

        // Configure AWS Lambda Services
        services.AddSingleton<IAmazonLambda>(sp =>
        {
            var awsOptions = sp.GetRequiredService<IOptions<AwsS3Options>>().Value;
            return new AmazonLambdaClient(awsOptions.AccessKey, awsOptions.SecretAccessKey, RegionEndpoint.GetBySystemName(awsOptions.Region));
        });
        services.AddScoped<ILambdaService, LambdaService>();

        // Configure SendGrid Email Services
        services.Configure<SendGridConfig>(configuration.GetSection("SendGrid"));
        services.AddScoped<ISendGridClient>(sp =>
        {
            var apiKey = configuration["SendGrid:ApiKey"];
            return new SendGridClient(apiKey);
        });
        services.AddScoped<IMailService, SendGridMailService>();

        // Add Security Services
        services.AddDataProtection(); // Required for IDataProtectionProvider
        services.AddScoped<IProtector, Protector>();

        // Add Firebase Auth Services
        services.AddScoped<IFirebaseUserService, FirebaseUserService>();

        // Add Product Services
        services.AddScoped<IProductService, ProductService>();

        // Add Office Services
        services.AddScoped<IExcelService, ExcelService>(); // Placeholder
        services.AddScoped<IPowerPointService, PowerPointService>(); // Placeholder

        // Add Payment Services
        services.AddScoped<IPaymentService, PaymentService>(); // Placeholder

        return services;
    }
}
