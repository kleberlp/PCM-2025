using PCM.INTERFACE.INTERCITY;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

var builder = Host.CreateApplicationBuilder(args);

//Windows Service (NET 8)
builder.Services.AddWindowsService();

// -------------------------------------------------------------------
// EMPRESAS (N empresas em um único serviço)
//   Lê a seção "Empresas". Se ausente, monta UMA empresa a partir do
//   "codigoEmpresa" legado + as ConnectionStrings compartilhadas.
//   Conexões vazias na empresa herdam as compartilhadas.
// -------------------------------------------------------------------
var cfg = builder.Configuration;

var sqlDefault = cfg.GetConnectionString("DefaultConnection");
var oracleDefault = cfg.GetConnectionString("ConnectionStringIntercity");

var empresas = cfg.GetSection("Empresas").Get<List<EmpresaSettings>>() ?? new List<EmpresaSettings>();

if (empresas.Count == 0)
{
    empresas.Add(new EmpresaSettings
    {
        Nome = "default",
        Enabled = true,
        CodigoEmpresa = cfg.GetValue<int>("codigoEmpresa"),
        DefaultConnection = sqlDefault,
        ConnectionStringIntercity = oracleDefault
    });
}

foreach (var e in empresas)
{
    if (string.IsNullOrWhiteSpace(e.DefaultConnection)) e.DefaultConnection = sqlDefault;
    if (string.IsNullOrWhiteSpace(e.ConnectionStringIntercity)) e.ConnectionStringIntercity = oracleDefault;
}

builder.Services.AddSingleton(empresas);

//Worker
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
