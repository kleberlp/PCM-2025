using PCM.INTERFACE.DAL;
using PCM.INTERFACE.INTERCITY;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

var builder = Host.CreateApplicationBuilder(args);

//Windows Service (NET 8)
builder.Services.AddWindowsService();

//SqlHelper como Singleton
builder.Services.AddSingleton<ISqlHelper>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var logger = sp.GetRequiredService<ILogger<SqlHelper>>();

    return new SqlHelper(
        config,
        logger,
        "DefaultConnection"
    );
});

//InterfaceApiOracle como Singleton
builder.Services.AddSingleton<InterfaceApiOracle>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var sqlHelper = sp.GetRequiredService<ISqlHelper>();
    var logger = sp.GetRequiredService<ILogger<InterfaceApiOracle>>();

    return new InterfaceApiOracle(
        oracleConnectionString: config.GetConnectionString("ConnectionStringIntercity"),
        sqlHelper: sqlHelper,
        logger: logger
    );
});

//Worker
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
