using Dapper;
using Microsoft.Extensions.Hosting;
using TAG.SERVICE.DAL;
using TAG.SERVICE.Services;
using TAG.SERVICE.Workers;

// As SPs devolvem colunas snake_case (codigo_empresa) -> propriedades PascalCase
DefaultTypeMap.MatchNamesWithUnderscores = true;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<TagWorker>();

builder.Services.AddSingleton<ITagRepository, TagRepository>();
builder.Services.AddHttpClient<TagImageService>();

builder.Services.AddWindowsService();

var host = builder.Build();
host.Run();
