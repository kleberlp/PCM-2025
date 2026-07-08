using PCM.INTERFACE.ATRIO.Models;
using System.Collections.Concurrent;

namespace PCM.INTERFACE.ATRIO.Services;

/// <summary>
/// Cache de token OAuth por propriedade (singleton). Cada hotel tem credenciais
/// próprias, então o token é armazenado e renovado por chave de propriedade,
/// permitindo que um único serviço atenda vários hotéis.
/// </summary>
public interface IOracleTokenCache
{
    Task<string> GetOrRefreshAsync(
        string key,
        Func<CancellationToken, Task<OracleTokenState>> refresh,
        CancellationToken ct = default);
}

public class OracleTokenCache : IOracleTokenCache
{
    private readonly ConcurrentDictionary<string, OracleTokenState> _tokens = new();
    private readonly ConcurrentDictionary<string, SemaphoreSlim> _locks = new();

    public async Task<string> GetOrRefreshAsync(
        string key,
        Func<CancellationToken, Task<OracleTokenState>> refresh,
        CancellationToken ct = default)
    {
        if (_tokens.TryGetValue(key, out var current) && current.IsValid())
            return current.AccessToken;

        var gate = _locks.GetOrAdd(key, _ => new SemaphoreSlim(1, 1));
        await gate.WaitAsync(ct);
        try
        {
            if (_tokens.TryGetValue(key, out current) && current.IsValid())
                return current.AccessToken;

            var fresh = await refresh(ct);
            _tokens[key] = fresh;
            return fresh.AccessToken;
        }
        finally
        {
            gate.Release();
        }
    }
}
