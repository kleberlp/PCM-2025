# Oracle Hospitality Service

Worker Service .NET 8 para integração com a **Oracle OHIP (Oracle Hospitality Integration Platform)**.

---

## Estrutura do Projeto

```
OracleHospitality.Service/
├── OracleHospitality.Service.sln
└── src/
    ├── OracleHospitality.Service.csproj
    ├── Program.cs                          # Entry point, DI, Serilog, Polly
    ├── OracleWorker.cs                     # BackgroundService principal
    ├── appsettings.json                    # Configurações (sem secrets)
    ├── appsettings.Development.json
    ├── Configuration/
    │   └── Settings.cs                     # Classes de config tipadas
    ├── Http/
    │   └── OracleHttpClient.cs             # HttpClient tipado
    ├── Models/
    │   └── OracleModels.cs                 # DTOs da API Oracle
    └── Services/
        ├── OracleAuthService.cs            # Autenticação OAuth2 com cache
        └── OracleReservationService.cs     # Consumo de reservas
```

---

## Como executar

### Desenvolvimento
```bash
cd src
dotnet run
```

### Build Release
```bash
dotnet publish -c Release -o ./publish
```

---

## Instalar como Serviço Windows

1. Adicione o pacote:
```bash
dotnet add package Microsoft.Extensions.Hosting.WindowsServices
```

2. No `Program.cs`, adicione `.UseWindowsService()`:
```csharp
var host = Host.CreateDefaultBuilder(args)
    .UseWindowsService(options => options.ServiceName = "OracleHospitalityService")
    ...
```

3. Publique e registre:
```bash
dotnet publish -c Release -r win-x64 --self-contained -o C:\Services\OracleHospitality
sc create OracleHospitalityService binPath="C:\Services\OracleHospitality\OracleHospitality.Service.exe"
sc start OracleHospitalityService
```

---

## Configurações (`appsettings.json`)

| Chave | Descrição |
|---|---|
| `OracleHospitality:BaseUrl` | URL base da API Oracle |
| `OracleHospitality:AppKey` | Chave da aplicação (x-app-key) |
| `OracleHospitality:EnterpriseId` | ID do enterprise (ATRIO) |
| `OracleHospitality:HotelId` | ID do hotel (A2501) |
| `OracleHospitality:ClientId` | OAuth2 Client ID |
| `OracleHospitality:ClientSecret` | OAuth2 Client Secret |
| `OracleHospitality:Scope` | Escopo OAuth2 |
| `ServiceSettings:IntervalMinutes` | Intervalo entre ciclos |
| `ServiceSettings:ConnectionString` | String de conexão SQL (PCM) |

> **Atenção:** Em produção, use **User Secrets** ou **variáveis de ambiente** para ClientId e ClientSecret.
> Nunca suba credenciais no repositório.

### Variáveis de ambiente (produção)
```
OracleHospitality__ClientSecret=sua-secret-aqui
OracleHospitality__ClientId=seu-client-id-aqui
```

---

## Adicionando novos endpoints Oracle

1. Crie o model em `Models/OracleModels.cs`
2. Crie a interface + implementação em `Services/`
3. Registre no `Program.cs` com `services.AddScoped<INovoServico, NovoServico>()`
4. Injete no `OracleWorker` e chame no `ExecuteAsync`

---

## Dependências

| Pacote | Finalidade |
|---|---|
| `Microsoft.Extensions.Hosting` | Worker Service base |
| `Microsoft.Extensions.Http` | HttpClient com DI |
| `Microsoft.Extensions.Http.Polly` | Polly integrado ao HttpClient |
| `Polly` | Retry + Circuit Breaker |
| `Newtonsoft.Json` | Deserialização JSON |
| `Serilog` | Logging estruturado |
| `Microsoft.Data.SqlClient` | Conexão SQL Server (PCM) |
