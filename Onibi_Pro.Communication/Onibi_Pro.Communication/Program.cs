using MongoDB.Driver;

using Onibi_Pro.Communication.BgWorkers;
using Onibi_Pro.Communication.Hubs;
using Onibi_Pro.Communication.Models;
using Onibi_Pro.Communication.Repositories;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddSingleton<INotificationRepository, NotificationRepository>();
        builder.Services.AddSingleton<IMessageRepository, MessageRepository>();

        builder.Services.AddSignalR();
        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddHostedService<EventNotifier>();
        builder.Services.Configure<NotificationBgWorkerConfig>(
            builder.Configuration.GetSection(NotificationBgWorkerConfig.Key));

        builder
          .Services
          .Configure<CommunicationDatabaseSettings>(
            builder.Configuration.GetSection(CommunicationDatabaseSettings.Key)
          );

        builder.Services.AddSingleton<IMongoClient>(_ =>
        {
            var connectionString =
                builder
                    .Configuration
                    .GetSection($"{CommunicationDatabaseSettings.Key}:{nameof(CommunicationDatabaseSettings.ConnectionString)}")?
                    .Value;

            return new MongoClient(connectionString);
        });

        builder.Services.AddCors();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.MapHub<NotificationsHub>("NotificationsHub");
        app.MapHub<ChatsHub>("ChatsHub");

        app.Run();
    }
}