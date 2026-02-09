using MESSAGE.SERVICE.DAL;
using MESSAGE.SERVICE.Services;
using MESSAGE.SERVICE.Settings;
using MESSAGE.SERVICE.Workers;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<MessageWorker>();

builder.Services.AddSingleton<IMessageRepository, MessageRepository>();
builder.Services.AddSingleton<WhatsAppService>();
builder.Services.AddSingleton<EmailService>();

builder.Services.Configure<WhatsAppSettings>(
    builder.Configuration.GetSection("WhatsApp"));

builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("Email"));

builder.Services.AddWindowsService();

var host = builder.Build();
host.Run();
