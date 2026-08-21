/**********************************************************************************************
    Script      : Ativo Fixo - histórico do ativo (ficha em painel lateral)
    Data        : 21/08/2026
    Descricao   : Linha do tempo mostrada na ficha do ativo, na tela de Ativos. Reúne o que já
                  está gravado: o cadastro, as movimentações e as contagens de inventário.
                  NENHUMA ALTERAÇÃO DE TABELA — procedure nova, sobre dados existentes.

    Fontes:
      tb_cad_asset               -> data de cadastro
      tb_ast_movimentacao        -> movimentações avulsas (origem/destino, tipo, observação)
      tb_ast_inventario_count    -> contagens de inventário (avaliação, observação, foto)

    Retorno (uma linha por evento, da mais recente para a mais antiga):
      data / titulo / detalhe / usuario / marcador
      marcador: 'ok' | 'warn' | 'neutro'  — define a cor do ponto na linha do tempo

    Chamada do código web (já publicada):
      - sp_select_asset_historico (@codigo_empresa, @codigo_asset)
**********************************************************************************************/

IF OBJECT_ID('dbo.sp_select_asset_historico', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_select_asset_historico];
GO

CREATE PROCEDURE [dbo].[sp_select_asset_historico]
@codigo_empresa smallint,
@codigo_asset   bigint
AS
BEGIN

    SET NOCOUNT ON;

    SELECT TOP 50
        FORMAT(h.dt, 'dd/MM/yyyy HH:mm') AS data,
        h.titulo,
        ISNULL(h.detalhe, '')            AS detalhe,
        ISNULL(h.usuario, '')            AS usuario,
        h.marcador
    FROM
    (

        /* ---------- Cadastro do ativo ---------- */
        SELECT
            a.data_input                        AS dt,
            'Cadastrado na unidade'             AS titulo,
            CONVERT(varchar(200), a.asset_code) AS detalhe,
            u.nome                              AS usuario,
            'neutro'                            AS marcador
        FROM
            tb_cad_asset a
            LEFT JOIN tb_cad_usuario u ON u.codigo = a.codigo_usuario_input
                                      AND u.codigo_empresa = a.codigo_empresa
        WHERE a.codigo = @codigo_asset
        AND   a.codigo_empresa = @codigo_empresa

        UNION ALL

        /* ---------- Movimentações avulsas ---------- */
        SELECT
            m.data_movimentacao,
            ISNULL(t.descricao, 'Movimentação'),
            CONVERT(varchar(200),
                CASE
                    WHEN ISNULL(m.origem, '') = '' AND ISNULL(m.destino, '') = '' THEN ISNULL(m.observacao, '')
                    ELSE CONCAT(ISNULL(m.origem, '—'), '  →  ', ISNULL(m.destino, '—'))
                END),
            u.nome,
            'neutro'
        FROM
            tb_ast_movimentacao m
            LEFT JOIN tb_stc_tipo_movimentacao_asset t ON t.codigo = m.codigo_tipo_movimentacao
            LEFT JOIN tb_cad_usuario u ON u.codigo = m.codigo_usuario
                                      AND u.codigo_empresa = m.codigo_empresa
        WHERE m.codigo_asset = @codigo_asset
        AND   m.codigo_empresa = @codigo_empresa

        UNION ALL

        /* ---------- Contagens de inventário ---------- */
        SELECT
            c.data,
            CASE
                WHEN c.status_ok = 0 THEN 'Contado como Não OK'
                WHEN c.status_ok = 1 THEN 'Contado como OK'
                ELSE 'Contado no inventário'
            END,
            CONVERT(varchar(200),
                CASE
                    WHEN ISNULL(c.observacao, '') <> '' THEN c.observacao
                    WHEN ISNULL(c.origem, '') <> ISNULL(c.destino, '')
                         THEN CONCAT(ISNULL(c.origem, '—'), '  →  ', ISNULL(c.destino, '—'))
                    ELSE ''
                END),
            -- o app grava o nome do contador na própria contagem; o usuário do PCM entra como reserva
            ISNULL(NULLIF(c.usuario, ''), u.nome),
            CASE
                WHEN c.status_ok = 0 THEN 'warn'
                WHEN c.status_ok = 1 THEN 'ok'
                ELSE 'neutro'
            END
        FROM
            tb_ast_inventario_count c
            LEFT JOIN tb_cad_usuario u ON u.codigo = c.codigo_usuario
                                      AND u.codigo_empresa = c.codigo_empresa
        WHERE c.codigo_asset = @codigo_asset
        AND   c.codigo_empresa = @codigo_empresa

    ) h
    ORDER BY h.dt DESC;

END
GO
