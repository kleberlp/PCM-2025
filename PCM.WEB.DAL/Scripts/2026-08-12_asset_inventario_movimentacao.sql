/**********************************************************************************************
    Script      : Inventário de Ativo Fixo - contagem duplicada + movimentação (pontos 5 e 8)
    Data        : 12/08/2026
    Descricao   : 1) Nova SP que verifica se o ativo já foi contado no inventário e em qual
                     local — a tela de bipagem pergunta ao usuário se deseja movimentar.
                  2) Ajuste da sp_insert_asset_inventory_count: novo parâmetro @movimentar e
                     movimentação do ativo conforme o local inventariado (ponto 8).

    IMPORTANTE: as SPs vivem no BANCO. A sp_insert_asset_inventory_count abaixo é um TEMPLATE —
    copie o corpo REAL da SP do banco e acrescente os blocos marcados. Os nomes de tabelas/
    colunas marcados com  -- >>> AJUSTE  devem ser conferidos com o schema real.

    Chamadas do código web (já publicadas):
      - sp_select_asset_inventory_count_location (@codigo_inventario, @asset_code,
        @codigo_setor, @codigo_apartamento) -> already_counted / same_location / local_atual
      - sp_insert_asset_inventory_count ganha @movimentar bit = 0 (parâmetro NOVO; a DAL
        sempre envia — a SP precisa aceitá-lo mesmo que ignore)
**********************************************************************************************/

/*--------------------------------------------------------------------------------------------
    1) VERIFICAÇÃO DE CONTAGEM DUPLICADA (ponto 5)  — aliases de saída fixos
--------------------------------------------------------------------------------------------*/
IF OBJECT_ID('dbo.sp_select_asset_inventory_count_location', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_select_asset_inventory_count_location];
GO

CREATE PROCEDURE [dbo].[sp_select_asset_inventory_count_location]
@codigo_inventario  bigint,
@asset_code         varchar(50),
@codigo_setor       int,
@codigo_apartamento int = NULL
AS
BEGIN

    SET NOCOUNT ON;

    SELECT TOP 1
        CAST(1 AS bit)                                              AS already_counted,
        CAST(CASE WHEN c.codigo_setor = @codigo_setor
                   AND ISNULL(c.codigo_apartamento, -1) = ISNULL(@codigo_apartamento, -1)
                  THEN 1 ELSE 0 END AS bit)                         AS same_location,
        CONCAT(ISNULL(s.descricao, ''),
               CASE WHEN a.codigo IS NULL THEN '' ELSE CONCAT(' / ', a.apartamento) END) AS local_atual  -- >>> AJUSTE colunas de setor/apartamento
    FROM
        tb_asset_inventario_count c                                                       -- >>> AJUSTE: tabela de contagem do inventário
        LEFT JOIN tb_cad_setor s       ON s.codigo = c.codigo_setor                       -- >>> AJUSTE
        LEFT JOIN tb_cad_apartamento a ON a.codigo = c.codigo_apartamento                 -- >>> AJUSTE
    WHERE c.codigo_inventario = @codigo_inventario
    AND   c.asset_code = @asset_code                                                      -- >>> AJUSTE se a coluna tiver outro nome
    ORDER BY c.codigo DESC;

    -- Nenhuma linha = ainda não contado (a DAL trata reader vazio como not counted)

END
GO

/*--------------------------------------------------------------------------------------------
    2) CONTAGEM COM MOVIMENTAÇÃO (pontos 5 e 8) — TEMPLATE sobre a SP real do banco
--------------------------------------------------------------------------------------------
    >>> AJUSTE: copie o corpo REAL da sp_insert_asset_inventory_count e acrescente:

    a) O parâmetro novo na assinatura (obrigatório aceitar, a DAL sempre envia):
         @movimentar bit = 0

    b) Contagem duplicada em outro local com @movimentar = 1:
         - atualizar a linha de contagem existente para o novo local
           (ou inserir nova linha e marcar a anterior, conforme a modelagem)

         UPDATE tb_asset_inventario_count                                -- >>> AJUSTE
         SET    codigo_setor = @codigo_setor,
                codigo_apartamento = @codigo_apartamento,
                codigo_usuario_update = @codigo_usuario,                 -- >>> AJUSTE
                data_update = GETDATE()                                  -- >>> AJUSTE
         WHERE  codigo_inventario = @codigo_inventario
         AND    asset_code = @asset_code;

    c) PONTO 8 — movimentar o ATIVO conforme o inventariado (para ativo cadastrado):
       sempre que a contagem confirmar o ativo em um local diferente do cadastro,
       atualizar o local do ativo e registrar a movimentação:

         UPDATE tb_cad_asset                                             -- >>> AJUSTE: tabela do cadastro de ativos
         SET    codigo_setor = @codigo_setor,
                codigo_apartamento = @codigo_apartamento
         WHERE  codigo_empresa = @codigo_empresa
         AND    codigo_unidade = @codigo_unidade
         AND    asset_code = @asset_code
         AND   (ISNULL(codigo_setor, -1) <> @codigo_setor
                OR ISNULL(codigo_apartamento, -1) <> ISNULL(@codigo_apartamento, -1));

         IF @@ROWCOUNT > 0
         BEGIN
             INSERT INTO tb_asset_movimentacao                           -- >>> AJUSTE: tabela de movimentação
                 (codigo_empresa, codigo_unidade, asset_code, codigo_tipo_movimentacao,
                  codigo_setor_destino, codigo_apartamento_destino, data_movimentacao,
                  observacao, codigo_usuario)
             VALUES
                 (@codigo_empresa, @codigo_unidade, @asset_code,
                  (SELECT codigo FROM tb_stc_tipo_movimentacao_asset WHERE descricao = 'INVENTÁRIO'),  -- >>> AJUSTE: tipo de movimentação por inventário
                  @codigo_setor, @codigo_apartamento, GETDATE(),
                  CONCAT('Movimentado pelo inventário ', @codigo_inventario), @codigo_usuario);
         END
--------------------------------------------------------------------------------------------*/
GO
