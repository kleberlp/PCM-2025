-- =============================================
-- Tabela de auditoria do PCM Interface ATRIO
-- Executar uma vez no banco PCM
-- =============================================
IF NOT EXISTS (
    SELECT 1 FROM sys.tables WHERE name = 'tb_interface_atrio_auditoria'
)
BEGIN
    CREATE TABLE [dbo].[tb_interface_atrio_auditoria] (
        [id]                     BIGINT IDENTITY(1,1) NOT NULL,
        [processo]               VARCHAR(100)         NOT NULL,  -- 'HOUSEKEEPING' | 'RESERVATIONS'
        [dt_inicio]              DATETIME             NOT NULL,
        [dt_fim]                 DATETIME             NULL,
        [status]                 VARCHAR(20)          NOT NULL,  -- 'EXECUTANDO' | 'CONCLUIDO' | 'ERRO'
        [registros_processados]  INT                  NULL,
        [mensagem_erro]          VARCHAR(MAX)         NULL,
        CONSTRAINT [PK_tb_interface_atrio_auditoria] PRIMARY KEY CLUSTERED ([id] ASC)
    );

    CREATE NONCLUSTERED INDEX [IX_auditoria_processo_dtinicio]
        ON [dbo].[tb_interface_atrio_auditoria] ([processo], [dt_inicio] DESC);

    PRINT 'Tabela tb_interface_atrio_auditoria criada com sucesso.';
END
ELSE
BEGIN
    PRINT 'Tabela tb_interface_atrio_auditoria já existe.';
END
GO
