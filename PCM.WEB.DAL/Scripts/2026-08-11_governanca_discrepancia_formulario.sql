/**********************************************************************************************
    Script      : Formulário/perfil da tela Discrepancias (GH15) - Governança
    Data        : 11/08/2026
    Descricao   : Registro do formulário 'gov_discrepancias' no cadastro de formulários/perfis,
                  para o item aparecer no menu (sidebar) e o LoadPerfil da tela funcionar.

    IMPORTANTE: as tabelas/SPs de perfil vivem no BANCO — replicar aqui EXATAMENTE o que foi
    feito manualmente para o formulário 'rel_gov_discrepancias' (RE18). Este script é um
    TEMPLATE: ajuste os nomes reais marcados com  -- >>> AJUSTE .

    O código lê o formulário em três pontos:
      1) sp_select_cadastro_basico_perfil_direito_usuario_visualizar
         -> precisa passar a RETORNAR a coluna 'gov_discrepancias' ('S'/'N'), lida no login
            (Account.vb / FormularioVisualizar) e usada pelo _Sidebar via
            Session["gov_discrepancias"].
         ATENÇÃO: o login QUEBRA se a coluna não existir no retorno — aplicar este script
         ANTES de publicar a versão com o código novo.
      2) sp usada por LoadPerfil (direitos inserir/editar/excluir por formulário)
         -> o formulário 'gov_discrepancias' precisa existir no cadastro para o GET
            /Governanca/Discrepancias resolver os direitos.
      3) Cadastro de perfis (tela Administração) -> o novo formulário deve aparecer para ser
         liberado por perfil.
**********************************************************************************************/

-- 1) Registrar o formulário  -- >>> AJUSTE: nome real da tabela de formulários
--    (usar o registro de 'rel_gov_discrepancias' como modelo de valores)
IF NOT EXISTS (SELECT 1 FROM tb_cad_formulario WHERE formulario = 'gov_discrepancias')     -- >>> AJUSTE
BEGIN
    INSERT INTO tb_cad_formulario (formulario, descricao, modulo)                          -- >>> AJUSTE colunas
    SELECT 'gov_discrepancias', 'Governança - Discrepâncias', modulo
    FROM   tb_cad_formulario
    WHERE  formulario = 'gov_apontamento';                                                 -- copia o módulo da tela base
END
GO

-- 2) Liberar o formulário para os perfis (mesma liberação da tela base como ponto de partida)
--    -- >>> AJUSTE: nome real da tabela de perfil x formulário/direitos
INSERT INTO tb_cad_perfil_direito (codigo_empresa, codigo_perfil, formulario, visualizar, inserir, editar, excluir)  -- >>> AJUSTE colunas
SELECT pd.codigo_empresa, pd.codigo_perfil, 'gov_discrepancias', pd.visualizar, pd.inserir, pd.editar, pd.excluir
FROM   tb_cad_perfil_direito pd                                                            -- >>> AJUSTE
WHERE  pd.formulario = 'gov_apontamento'
AND    NOT EXISTS (SELECT 1 FROM tb_cad_perfil_direito x
                   WHERE x.codigo_empresa = pd.codigo_empresa
                   AND   x.codigo_perfil  = pd.codigo_perfil
                   AND   x.formulario     = 'gov_discrepancias');
GO

-- 3) sp_select_cadastro_basico_perfil_direito_usuario_visualizar
--    -- >>> AJUSTE: incluir 'gov_discrepancias' no retorno (mesmo mecanismo usado para
--    'rel_gov_discrepancias': se a SP pivota os formulários, acrescentar a coluna no PIVOT;
--    se monta colunas fixas, acrescentar o MAX(CASE WHEN formulario = 'gov_discrepancias'...)).
GO
