'==============================================================================================='
'Classe:        Manual                                                                          '
'Objetivo:      Manual integrado (botao "?" do cabecalho e telas de cadastro do help).          '
'                                                                                               '
'               Diferente do restante do PCM, o conteudo do manual NAO mora no SQL Server:      '
'               mora no banco PostgreSQL (Supabase) informado na connection string              '
'               "HelpConnection" do Web.config. Por isso esta classe usa Npgsql e SQL           '
'               parametrizado em vez de stored procedures.                                      '
'==============================================================================================='
Imports Npgsql

Public Class Manual

    Private sConnection As String

    Sub New(ByVal sCon As String)
        sConnection = sCon
    End Sub

#Region "::: Leitura :::"

    '-------------------------------------------------------------------------------------------'
    'Manual da tela em que o usuario esta. A tela se identifica por controller + action, que e  '
    'o que ela sabe de si mesma sem carregar nada. A busca tenta a tela exata e, se nao houver, '
    'cai no manual do modulo (mesmo controller com action vazia) — melhor mostrar a visao do    '
    'modulo do que abrir vazio.                                                                 '
    '-------------------------------------------------------------------------------------------'
    Public Function ManualTela(ByVal sController As String, ByVal sAction As String) As PCM.WEB.MODELS.Manual

        Dim iCodigo As Integer = 0

        Using oConnection As New NpgsqlConnection(sConnection)

            oConnection.Open()

            Using oCommand As New NpgsqlCommand(
                "SELECT h.help_id " &
                "FROM tb_pcm_help h " &
                "WHERE h.active = true " &
                "  AND h.kind = 'S' " &
                "  AND lower(COALESCE(h.controller, '')) = lower(@controller) " &
                "  AND (lower(COALESCE(h.action, '')) = lower(@action) OR COALESCE(h.action, '') = '') " &
                "ORDER BY CASE WHEN lower(COALESCE(h.action, '')) = lower(@action) THEN 0 ELSE 1 END, h.help_id " &
                "LIMIT 1", oConnection)

                oCommand.Parameters.AddWithValue("controller", If(sController, ""))
                oCommand.Parameters.AddWithValue("action", If(sAction, ""))

                Dim oResult As Object = oCommand.ExecuteScalar()
                If oResult IsNot Nothing AndAlso IsDBNull(oResult) = False Then iCodigo = Convert.ToInt32(oResult)

            End Using

            If iCodigo = 0 Then Return New PCM.WEB.MODELS.Manual

            Return LerManual(oConnection, iCodigo, bSomenteAtivo:=True)

        End Using

    End Function

    '-------------------------------------------------------------------------------------------'
    'Um manual pelo codigo (tela de manutencao e link "ver tambem" do painel).                  '
    '-------------------------------------------------------------------------------------------'
    Public Function InfoManual(ByVal iCodigo As Integer, Optional ByVal bSomenteAtivo As Boolean = False) As PCM.WEB.MODELS.Manual

        Using oConnection As New NpgsqlConnection(sConnection)
            oConnection.Open()
            Return LerManual(oConnection, iCodigo, bSomenteAtivo)
        End Using

    End Function

    Private Function LerManual(ByVal oConnection As NpgsqlConnection, ByVal iCodigo As Integer, ByVal bSomenteAtivo As Boolean) As PCM.WEB.MODELS.Manual

        Dim oManual As New PCM.WEB.MODELS.Manual

        Using oCommand As New NpgsqlCommand(
            "SELECT h.help_id, " &
            "       h.kind, " &
            "       COALESCE(h.controller, '')       AS controller, " &
            "       COALESCE(h.action, '')           AS action, " &
            "       COALESCE(h.processo_help_id, 0)  AS processo_help_id, " &
            "       COALESCE(p.title, '')            AS processo_titulo, " &
            "       h.title, " &
            "       COALESCE(h.subtitle, '')         AS subtitle, " &
            "       h.active " &
            "FROM tb_pcm_help h " &
            "LEFT JOIN tb_pcm_help p ON p.help_id = h.processo_help_id AND p.active = true " &
            "WHERE h.help_id = @codigo" & If(bSomenteAtivo, " AND h.active = true", ""), oConnection)

            oCommand.Parameters.AddWithValue("codigo", iCodigo)

            Using oReader As NpgsqlDataReader = oCommand.ExecuteReader()
                If oReader.Read() Then
                    oManual.codigo = Convert.ToInt32(oReader.Item("help_id"))
                    oManual.tipo = oReader.Item("kind").ToString()
                    oManual.controller = oReader.Item("controller").ToString()
                    oManual.action = oReader.Item("action").ToString()
                    oManual.processo_codigo = Convert.ToInt32(oReader.Item("processo_help_id"))
                    oManual.processo_titulo = oReader.Item("processo_titulo").ToString()
                    oManual.titulo = oReader.Item("title").ToString()
                    oManual.subtitulo = oReader.Item("subtitle").ToString()
                    oManual.ativo = Convert.ToBoolean(oReader.Item("active"))
                End If
            End Using

        End Using

        If oManual.codigo = 0 Then Return oManual

        Using oCommand As New NpgsqlCommand(
            "SELECT i.item_id, " &
            "       i.sequence, " &
            "       i.title, " &
            "       COALESCE(i.content, '')   AS content, " &
            "       COALESCE(i.note_type, '') AS note_type, " &
            "       COALESCE(i.note, '')      AS note, " &
            "       COALESCE(i.image, '')     AS image, " &
            "       COALESCE(i.video, '')     AS video " &
            "FROM tb_pcm_help_item i " &
            "WHERE i.help_id = @codigo AND i.active = true " &
            "ORDER BY i.sequence, i.item_id", oConnection)

            oCommand.Parameters.AddWithValue("codigo", oManual.codigo)

            Using oReader As NpgsqlDataReader = oCommand.ExecuteReader()
                While oReader.Read()
                    Dim oItem As New PCM.WEB.MODELS.ManualItem
                    oItem.codigo = Convert.ToInt32(oReader.Item("item_id"))
                    oItem.sequencia = Convert.ToInt32(oReader.Item("sequence"))
                    oItem.titulo = oReader.Item("title").ToString()
                    oItem.conteudo = oReader.Item("content").ToString()
                    oItem.tipo_nota = oReader.Item("note_type").ToString()
                    oItem.nota = oReader.Item("note").ToString()
                    oItem.imagem = oReader.Item("image").ToString()
                    oItem.video = oReader.Item("video").ToString()
                    oManual.itens.Add(oItem)
                End While
            End Using

        End Using

        Return oManual

    End Function

    '-------------------------------------------------------------------------------------------'
    'Grade da manutencao: de que tela (ou processo) e o manual e quantas secoes tem.            '
    '-------------------------------------------------------------------------------------------'
    Public Function IndexManual(Optional ByVal sTitulo As String = "") As List(Of PCM.WEB.MODELS.ManualGrid)

        Dim oLista As New List(Of PCM.WEB.MODELS.ManualGrid)

        Using oConnection As New NpgsqlConnection(sConnection)

            oConnection.Open()

            Using oCommand As New NpgsqlCommand(
                "SELECT h.help_id, " &
                "       h.kind, " &
                "       h.title, " &
                "       COALESCE(h.subtitle, '') AS subtitle, " &
                "       CASE WHEN h.kind = 'P' THEN '' " &
                "            ELSE COALESCE(h.controller, '') || CASE WHEN COALESCE(h.action, '') = '' THEN '' ELSE '/' || h.action END END AS tela, " &
                "       (SELECT COUNT(*) FROM tb_pcm_help_item i WHERE i.help_id = h.help_id AND i.active = true) AS secoes, " &
                "       h.active " &
                "FROM tb_pcm_help h " &
                "WHERE (@titulo = '' OR h.title ILIKE '%' || @titulo || '%') " &
                "ORDER BY h.title", oConnection)

                oCommand.Parameters.AddWithValue("titulo", If(sTitulo, ""))

                Using oReader As NpgsqlDataReader = oCommand.ExecuteReader()
                    While oReader.Read()
                        Dim oInfo As New PCM.WEB.MODELS.ManualGrid
                        oInfo.codigo = Convert.ToInt32(oReader.Item("help_id"))
                        oInfo.tipo = oReader.Item("kind").ToString()
                        oInfo.titulo = oReader.Item("title").ToString()
                        oInfo.subtitulo = oReader.Item("subtitle").ToString()
                        oInfo.tela = oReader.Item("tela").ToString()
                        oInfo.secoes = Convert.ToInt32(oReader.Item("secoes"))
                        oInfo.ativo = Convert.ToBoolean(oReader.Item("active"))
                        oLista.Add(oInfo)
                    End While
                End Using

            End Using

        End Using

        Return oLista

    End Function

    '-------------------------------------------------------------------------------------------'
    'Manuais de processo, para o combo "ver tambem" da tela de cadastro.                        '
    '-------------------------------------------------------------------------------------------'
    Public Function ComboProcesso() As List(Of PCM.WEB.MODELS.ListCombo)

        Dim oLista As New List(Of PCM.WEB.MODELS.ListCombo)

        Using oConnection As New NpgsqlConnection(sConnection)

            oConnection.Open()

            Using oCommand As New NpgsqlCommand(
                "SELECT h.help_id, h.title " &
                "FROM tb_pcm_help h " &
                "WHERE h.kind = 'P' AND h.active = true " &
                "ORDER BY h.title", oConnection)

                Using oReader As NpgsqlDataReader = oCommand.ExecuteReader()
                    While oReader.Read()
                        Dim oCombo As New PCM.WEB.MODELS.ListCombo
                        oCombo.codigo = Convert.ToInt32(oReader.Item("help_id"))
                        oCombo.descricao = oReader.Item("title").ToString()
                        oLista.Add(oCombo)
                    End While
                End Using

            End Using

        End Using

        Return oLista

    End Function

#End Region

#Region "::: Manutencao :::"

    '-------------------------------------------------------------------------------------------'
    'Grava o manual inteiro — cabecalho e secoes — em uma transacao. Editar manual e mexer no   '
    'texto e na ordem das secoes ao mesmo tempo; gravar tudo de uma vez evita meio-caminho      '
    'gravado se o navegador cair no meio da edicao. As secoes substituem as anteriores.         '
    '-------------------------------------------------------------------------------------------'
    Public Function SaveManual(ByVal oManual As PCM.WEB.MODELS.Manual, ByVal sUsuario As String) As Integer

        'Um manual e de uma coisa so: processo nao tem tela, e tela nao aceita apontar para si.
        Dim bProcesso As Boolean = (oManual.tipo = "P")
        Dim sController As String = If(bProcesso, "", If(oManual.controller, "").Trim())
        Dim sAction As String = If(bProcesso, "", If(oManual.action, "").Trim())
        Dim iProcesso As Integer = If(bProcesso, 0, oManual.processo_codigo)

        If bProcesso = False AndAlso sController = "" Then
            Throw New Exception("Informe a tela (controller) do manual.")
        End If

        Using oConnection As New NpgsqlConnection(sConnection)

            oConnection.Open()

            Using oTransaction As NpgsqlTransaction = oConnection.BeginTransaction()

                Try

                    If oManual.codigo = 0 Then

                        Using oCommand As New NpgsqlCommand(
                            "INSERT INTO tb_pcm_help (kind, controller, action, processo_help_id, title, subtitle, active, username, date_input) " &
                            "VALUES (@kind, NULLIF(@controller, ''), NULLIF(@action, ''), NULLIF(@processo, 0), @titulo, NULLIF(@subtitulo, ''), @ativo, @usuario, now()) " &
                            "RETURNING help_id", oConnection, oTransaction)

                            oCommand.Parameters.AddWithValue("kind", If(bProcesso, "P", "S"))
                            oCommand.Parameters.AddWithValue("controller", sController)
                            oCommand.Parameters.AddWithValue("action", sAction)
                            oCommand.Parameters.AddWithValue("processo", iProcesso)
                            oCommand.Parameters.AddWithValue("titulo", If(oManual.titulo, "").Trim())
                            oCommand.Parameters.AddWithValue("subtitulo", If(oManual.subtitulo, "").Trim())
                            oCommand.Parameters.AddWithValue("ativo", oManual.ativo)
                            oCommand.Parameters.AddWithValue("usuario", If(sUsuario, ""))

                            oManual.codigo = Convert.ToInt32(oCommand.ExecuteScalar())

                        End Using

                    Else

                        Using oCommand As New NpgsqlCommand(
                            "UPDATE tb_pcm_help " &
                            "SET kind = @kind, " &
                            "    controller = NULLIF(@controller, ''), " &
                            "    action = NULLIF(@action, ''), " &
                            "    processo_help_id = NULLIF(@processo, 0), " &
                            "    title = @titulo, " &
                            "    subtitle = NULLIF(@subtitulo, ''), " &
                            "    active = @ativo, " &
                            "    username_update = @usuario, " &
                            "    date_update = now() " &
                            "WHERE help_id = @codigo", oConnection, oTransaction)

                            oCommand.Parameters.AddWithValue("kind", If(bProcesso, "P", "S"))
                            oCommand.Parameters.AddWithValue("controller", sController)
                            oCommand.Parameters.AddWithValue("action", sAction)
                            oCommand.Parameters.AddWithValue("processo", If(iProcesso = oManual.codigo, 0, iProcesso))
                            oCommand.Parameters.AddWithValue("titulo", If(oManual.titulo, "").Trim())
                            oCommand.Parameters.AddWithValue("subtitulo", If(oManual.subtitulo, "").Trim())
                            oCommand.Parameters.AddWithValue("ativo", oManual.ativo)
                            oCommand.Parameters.AddWithValue("usuario", If(sUsuario, ""))
                            oCommand.Parameters.AddWithValue("codigo", oManual.codigo)

                            oCommand.ExecuteNonQuery()

                        End Using

                        Using oCommand As New NpgsqlCommand("DELETE FROM tb_pcm_help_item WHERE help_id = @codigo", oConnection, oTransaction)
                            oCommand.Parameters.AddWithValue("codigo", oManual.codigo)
                            oCommand.ExecuteNonQuery()
                        End Using

                    End If

                    Dim iSequencia As Integer = 0

                    For Each oItem As PCM.WEB.MODELS.ManualItem In If(oManual.itens, New List(Of PCM.WEB.MODELS.ManualItem))

                        'Secao sem titulo e linha em branco do editor, nao conteudo.
                        If If(oItem.titulo, "").Trim() = "" Then Continue For

                        iSequencia += 1

                        Using oCommand As New NpgsqlCommand(
                            "INSERT INTO tb_pcm_help_item (help_id, sequence, title, content, note_type, note, image, video, active) " &
                            "VALUES (@help, @sequencia, @titulo, NULLIF(@conteudo, ''), NULLIF(@tipo_nota, ''), NULLIF(@nota, ''), NULLIF(@imagem, ''), NULLIF(@video, ''), true)",
                            oConnection, oTransaction)

                            oCommand.Parameters.AddWithValue("help", oManual.codigo)
                            oCommand.Parameters.AddWithValue("sequencia", iSequencia)
                            oCommand.Parameters.AddWithValue("titulo", oItem.titulo.Trim())
                            oCommand.Parameters.AddWithValue("conteudo", If(oItem.conteudo, ""))
                            oCommand.Parameters.AddWithValue("tipo_nota", If(oItem.tipo_nota, ""))
                            oCommand.Parameters.AddWithValue("nota", If(oItem.nota, ""))
                            oCommand.Parameters.AddWithValue("imagem", If(oItem.imagem, ""))
                            oCommand.Parameters.AddWithValue("video", If(oItem.video, ""))

                            oCommand.ExecuteNonQuery()

                        End Using

                    Next

                    oTransaction.Commit()

                Catch ex As Exception
                    oTransaction.Rollback()
                    Throw
                End Try

            End Using

        End Using

        Return oManual.codigo

    End Function

    '-------------------------------------------------------------------------------------------'
    'Exclui o manual. As secoes saem junto pelo ON DELETE CASCADE.                              '
    '-------------------------------------------------------------------------------------------'
    Public Sub DeleteManual(ByVal iCodigo As Integer)

        Using oConnection As New NpgsqlConnection(sConnection)

            oConnection.Open()

            Using oCommand As New NpgsqlCommand("DELETE FROM tb_pcm_help WHERE help_id = @codigo", oConnection)
                oCommand.Parameters.AddWithValue("codigo", iCodigo)
                oCommand.ExecuteNonQuery()
            End Using

        End Using

    End Sub

#End Region

End Class
