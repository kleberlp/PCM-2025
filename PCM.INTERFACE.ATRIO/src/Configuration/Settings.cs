namespace PCM.INTERFACE.ATRIO.Configuration;

public class OracleHospitalitySettings
{
    public const string Section = "OracleHospitality";

    public string BaseUrl { get; set; } = string.Empty;
    public string AppKey { get; set; } = string.Empty;
    public string EnterpriseId { get; set; } = string.Empty;
    public string HotelId { get; set; } = string.Empty;
    public string Scope { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string TokenEndpoint { get; set; } = "/oauth/v1/tokens";
    public int TokenExpiryBufferSeconds { get; set; } = 60;

    public string GetBasicAuthHeader()
    {
        var credentials = $"{ClientId}:{ClientSecret}";
        return Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(credentials));
    }
}

/// <summary>
/// Uma propriedade (hotel) monitorada pelo serviço. Combina a configuração Oracle
/// Hospitality com os dados de destino no PCM (empresa/conexão) e as flags de sync.
/// Vários itens podem ser cadastrados na seção "Properties" para rodar N hotéis
/// dentro de UM único serviço.
/// </summary>
public class PropertySettings : OracleHospitalitySettings
{
    public const string Section = "Properties";

    /// <summary>Nome/identificador amigável da propriedade (usado em log/auditoria/e-mail).</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Quando false, a propriedade é ignorada no ciclo.</summary>
    public bool Enabled { get; set; } = true;

    /// <summary>Empresa (PCM) de destino.</summary>
    public int CodigoEmpresa { get; set; } = 1;

    /// <summary>Conexão do banco PCM de destino desta propriedade.</summary>
    public string ConnectionString { get; set; } = string.Empty;

    /// <summary>Habilita/desabilita sync de housekeeping desta propriedade.</summary>
    public bool SyncHousekeeping { get; set; } = true;

    /// <summary>Habilita/desabilita sync de reservas desta propriedade.</summary>
    public bool SyncReservations { get; set; } = false;

    /// <summary>Chave estável para cache de token (por propriedade/credencial).</summary>
    public string TokenCacheKey =>
        string.IsNullOrWhiteSpace(Name) ? $"{EnterpriseId}:{HotelId}:{ClientId}" : Name;

    /// <summary>Rótulo para logs.</summary>
    public string Label =>
        string.IsNullOrWhiteSpace(Name) ? $"{EnterpriseId}/{HotelId}" : Name;
}

/// <summary>
/// Contexto da propriedade em processamento no escopo atual. É preenchido pelo worker
/// antes de resolver os serviços, permitindo que Auth/HttpClient/Repositórios usem a
/// configuração correta de cada hotel sem exigir um serviço separado por hotel.
/// </summary>
public interface IPropertyContext
{
    PropertySettings Current { get; set; }
}

public class PropertyContext : IPropertyContext
{
    public PropertySettings Current { get; set; } = new();
}

public class ServiceSettings
{
    public const string Section = "ServiceSettings";

    public int IntervalMinutes { get; set; } = 5;

    // -----------------------------------------------------------------------
    // Campos legados (compatibilidade). Quando a seção "Properties" não é
    // informada, o serviço monta uma única propriedade a partir destes valores
    // + da seção "OracleHospitality".
    // -----------------------------------------------------------------------
    public int CodigoEmpresa { get; set; } = 1;
    public string ConnectionString { get; set; } = string.Empty;
    public bool SyncHousekeeping { get; set; } = true;
    public bool SyncReservations { get; set; } = true;
}

public class EmailSettings
{
    public const string Section = "EmailSettings";

    public bool Enabled { get; set; } = false;
    public string SmtpHost { get; set; } = string.Empty;
    public int SmtpPort { get; set; } = 587;
    public bool UseSsl { get; set; } = true;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FromAddress { get; set; } = string.Empty;
    public string FromName { get; set; } = "PCM Interface ATRIO";
    public List<string> Recipients { get; set; } = [];
}
