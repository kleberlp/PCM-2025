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

public class ServiceSettings
{
    public const string Section = "ServiceSettings";

    public int IntervalMinutes { get; set; } = 5;
    public int CodigoEmpresa { get; set; } = 1;
    public string ConnectionString { get; set; } = string.Empty;

    /// <summary>Habilita/desabilita sync de housekeeping.</summary>
    public bool SyncHousekeeping { get; set; } = true;

    /// <summary>Habilita/desabilita sync de reservas.</summary>
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
