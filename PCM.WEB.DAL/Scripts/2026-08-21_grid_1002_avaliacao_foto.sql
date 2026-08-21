/**********************************************************************************************
    Script      : Inventário aberto - colunas Avaliação e Foto na aba Inventariados
    Data        : 21/08/2026
    Descricao   : A grid da aba "Inventariados" (codigo_grid 1002) monta as colunas a partir de
                  tb_stc_grid_column. Este script cadastra as duas colunas que faltavam para o
                  layout novo, agora que a sp_select_asset_manager_movement já devolve
                  status_ok e foto_path (script 2026-08-21_ativofixo_layout_fase2.sql).

                  NENHUMA ALTERAÇÃO DE TABELA — apenas duas linhas de configuração.

    Ordem final das colunas da grid 1002:
        1. asset_code          Ativo fixo
        2. descricao           Descrição
        3. status_ok           Avaliação          <-- NOVA
        4. foto_path           Foto               <-- NOVA
        5. origem
        6. destino
        7. data                Data inventário
        8. usuario             Usuário
        9. tipo_movimentacao   Tipo movimentação

    Idempotente: só age se a coluna Avaliação ainda não estiver cadastrada, então pode ser
    executado mais de uma vez sem duplicar nem deslocar a ordem novamente.

    OBS.: assume que tb_stc_grid_column.codigo é IDENTITY (os códigos existentes são
    sequenciais entre grids). Se não for, informe o código a usar e o INSERT passa a preenchê-lo.
**********************************************************************************************/

IF NOT EXISTS (SELECT 1 FROM tb_stc_grid_column WHERE codigo_grid = 1002 AND data = 'status_ok')
BEGIN

    -- Abre espaço para as duas colunas novas logo após a descrição
    UPDATE tb_stc_grid_column
    SET    ordem = ordem + 2
    WHERE  codigo_grid = 1002
    AND    ordem >= 3;

    INSERT INTO tb_stc_grid_column
    (
        codigo_grid,
        data,
        title,
        visible,
        orderable,
        align,
        width,
        frozen,
        ordem
    )
    VALUES
        -- Avaliação: a tela desenha o chip OK / Não OK a partir do bit status_ok
        (1002, 'status_ok', 'Avaliação', 1, 1, 'center', 110, 0, 3),

        -- Foto: indica que a contagem tem evidência registrada (não ordenável)
        (1002, 'foto_path', 'Foto', 1, 0, 'center', 70, 0, 4);

END
GO

-- Conferência
SELECT ordem, data, title, align, width
FROM   tb_stc_grid_column
WHERE  codigo_grid = 1002
ORDER  BY ordem;
GO
