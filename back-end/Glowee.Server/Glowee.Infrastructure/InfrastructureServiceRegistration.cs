using Glowee.Application.Contracts.Email;
using Glowee.Application.Contracts.FileProcessing;
using Glowee.Application.Contracts.Logging;
using Glowee.Application.Contracts.Storage;
using Glowee.Application.Models.Email;
using Glowee.Application.Models.Storage;
using Glowee.Infrastructure.EmailService;
using Glowee.Infrastructure.FileProcessing;
using Glowee.Infrastructure.Logging;
using Glowee.Infrastructure.Storage;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Glowee.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));

            services.Configure<BlobStorageContainerOptions>(configuration.GetSection("AzureStorage:Containers"));
            services.Configure<DefaultFiles>(configuration.GetSection("AzureStorage:DefaultFiles"));

            var storageConnectionString = configuration["AzureStorage:ConnectionString"];
            services.AddAzureClients(builder =>
            {
                builder.AddBlobServiceClient(storageConnectionString);
            });

            services.AddScoped<IImageProcessor, ImageProcessor>();

            services.AddSingleton<IBlobContainerResolver, BlobContainerResolver>();
            services.AddScoped<IBlobStorageService, BlobStorageService>();
            services.AddScoped<IChatLogoStorageService, ChatLogoStorageService>();
            services.AddScoped<IMessageAttachmentStorageService, MessageAttachmentStorageService>();
            services.AddScoped<IPostMediaStorageService, PostMediaStorageSevice>();
            services.AddScoped<IProfileImageStorageService, ProfileImageStorageService>();
            services.AddScoped<IThumbnailStorageService, ThumbnailStorageService>();

            return services;
        }
    }
}
