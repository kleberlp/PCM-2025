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

O serviço atende **N hotéis (propriedades) em um único serviço** através da lista
`Properties`. Cada item combina as credenciais Oracle com o destino no PCM
(empresa/conexão) e as flags de sync. Basta acrescentar um novo objeto ao array
para monitorar mais um hotel — **não é preciso instalar um segundo serviço**.

```jsonc
{
  "Properties": [
    {
      "Name": "ATRIO A2501",        // rótulo p/ log/auditoria/e-mail
      "Enabled": true,              // false => propriedade ignorada no ciclo
      "BaseUrl": "https://mtcu11pr.hospitality-api.us-ashburn-1.ocs.oraclecloud.com",
      "AppKey": "...",
      "EnterpriseId": "ATRIO",
      "HotelId": "A2501",
      "Scope": "urn:opc:hgbu:ws:__myscopes__",
      "ClientId": "...",
      "ClientSecret": "...",
      "TokenEndpoint": "/oauth/v1/tokens",
      "TokenExpiryBufferSeconds": 60,
      "CodigoEmpresa": 905,         // empresa PCM de destino
      "ConnectionString": "Server=...;Database=PCM;...",
      "SyncHousekeeping": true,
      "SyncReservations": false
    },
    { "Name": "NOVO HOTEL", "Enabled": false, "...": "..." }
  ],
  "ServiceSettings": { "IntervalMinutes": 5 }  // intervalo entre ciclos (global)
}
```

| Chave | Descrição |
|---|---|
| `Properties[]` | Lista de propriedades (hotéis) processadas em cada ciclo |
| `Properties[].Name` | Rótulo amigável (log/auditoria/e-mail) |
| `Properties[].Enabled` | Liga/desliga a propriedade |
| `Properties[].BaseUrl / AppKey / EnterpriseId / HotelId / Scope / ClientId / ClientSecret` | Credenciais Oracle OHIP daquele hotel |
| `Properties[].CodigoEmpresa` | Empresa PCM de destino |
| `Properties[].ConnectionString` | Conexão SQL (PCM) daquele hotel |
| `Properties[].SyncHousekeeping / SyncReservations` | Flags de sync por propriedade |
| `ServiceSettings:IntervalMinutes` | Intervalo entre ciclos (compartilhado) |

O token OAuth é cacheado **por propriedade** (cada hotel tem credenciais próprias),
então um único serviço mantém sessões independentes para todos os hotéis.

> **Compatibilidade:** se a seção `Properties` não existir, o serviço monta
> automaticamente **uma** propriedade a partir das seções legadas
> `OracleHospitality` + `ServiceSettings` (formato antigo continua funcionando).

> **Atenção:** Em produção, use **User Secrets** ou **variáveis de ambiente** para ClientId e ClientSecret.
> Nunca suba credenciais no repositório.

### Variáveis de ambiente (produção)
```
Properties__0__ClientSecret=sua-secret-aqui
Properties__0__ClientId=seu-client-id-aqui
Properties__1__ClientSecret=secret-do-segundo-hotel
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
