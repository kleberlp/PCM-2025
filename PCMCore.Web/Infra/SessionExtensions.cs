using Microsoft.AspNetCore.Http;

namespace PCM.WEB
{
    /// <summary>
    /// A Session do Core só fala int/string/bytes; a do MVC5 guardava object.
    /// Estes helpers cobrem o que o login do PCM pivota (bool das permissões
    /// de formulário: cad_*, adm_*, audit_*), mantendo a leitura com a mesma
    /// cara nos controllers migrados: Session.GetBool("cad_aviso").
    /// </summary>
    public static class SessionExtensions
    {
        public static void SetBool(this ISession session, string chave, bool valor)
        {
            session.SetInt32(chave, valor ? 1 : 0);
        }

        /// <summary>Chave ausente = false — o mesmo efeito do cast de null no MVC5 protegido por guarda.</summary>
        public static bool GetBool(this ISession session, string chave)
        {
            return session.GetInt32(chave) == 1;
        }
    }
}
