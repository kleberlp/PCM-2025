'==============================================================================================='
'Classe:        Manual                                                                          '
'Objetivo:      Manual integrado (botao "?" do cabecalho e telas de cadastro do help).          '
'                                                                                               '
'               Estrutura e procedures em PCM.WEB.DAL\Scripts\2026-08-27_manual_integrado.sql.  '
'               O conteudo migrado do Supabase entra por SQL\migrar_manual_supabase.py.         '
'==============================================================================================='
Imports System.Data.SqlClient
Imports PCM.WEB.DAL.SQLHelper
Imports PCM.WEB.MODELS

' O modelo e esta classe se chamam Manual, e dentro do namespace PCM.WEB.DAL o nome PCM
' bate na classe PCM desta mesma camada — PCM.WEB.MODELS.Manual nao resolveria. O alias
' diz de qual Manual se trata em cada assinatura.
Imports ManualInfo = PCM.WEB.MODELS.Manual

Public Class Manual

    Private sConnection As String

    Sub New(ByVal sCon As String)
        sConnection = sCon
    End Sub

#Region "::: Leitura :::"

    '-------------------------------------------------------------------------------------------'
    'DESCRICAO     :   Manual da tela em que o usuario esta. A tela se identifica por controller '
    '                  e action, que e o que ela sabe de si mesma sem carregar nada. Sem manual  '
    '                  proprio, a procedure cai no manual do modulo (action vazia).              '
    '-------------------------------------------------------------------------------------------'
    Public Function ManualTela(ByVal iCodigoEmpresa As Integer,
                               ByVal sController As String,
                               ByVal sAction As String) As ManualInfo

        'Variaveis Locais
        Dim oSqlParameter(2) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oManual As New ManualInfo
        Dim i As Integer = 0

        Try

            'Seta Parametros - Codigo Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Controller
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "controller"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = If(sController, "") : i += 1

            'Seta Parametros - Action
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "action"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = If(sAction, "")

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_manual_tela", oSqlParameter)

            LerManual(oSqlDataReader, oManual)

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Funcao
            Return oManual

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    '-------------------------------------------------------------------------------------------'
    'DESCRICAO     :   Um manual pelo codigo — tela de manutencao e link "ver tambem" do painel. '
    '-------------------------------------------------------------------------------------------'
    Public Function InfoManual(ByVal iCodigoEmpresa As Integer,
                               ByVal iCodigo As Integer,
                               Optional ByVal bSomenteAtivo As Boolean = False) As ManualInfo

        'Variaveis Locais
        Dim oSqlParameter(2) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oManual As New ManualInfo
        Dim i As Integer = 0

        Try

            'Seta Parametros - Codigo Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Codigo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo : i += 1

            'Seta Parametros - Somente Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "somente_ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bSomenteAtivo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_manual", oSqlParameter)

            LerManual(oSqlDataReader, oManual)

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Telas adicionais (o mesmo manual servindo telas irmas)
            If oManual.codigo > 0 Then oManual.telas = TelasManual(oManual.codigo)

            'Retorno da Funcao
            Return oManual

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    '-------------------------------------------------------------------------------------------'
    'DESCRICAO     :   Telas ALEM da principal atendidas pelo manual. Base antiga, sem a         '
    '                  tb_manual_tela e a procedure, devolve lista vazia: o cadastro continua    '
    '                  funcionando com uma tela so em vez de quebrar.                            '
    '-------------------------------------------------------------------------------------------'
    Public Function TelasManual(ByVal iCodigo As Integer) As List(Of ManualTela)

        'Variaveis Locais
        Dim oSqlParameter(0) As SqlParameter
        Dim oSqlDataReader As SqlDataReader = Nothing
        Dim oLista As New List(Of ManualTela)

        Try

            'Seta Parametros - Codigo
            oSqlParameter(0) = New SqlParameter
            oSqlParameter(0).ParameterName = "codigo"
            oSqlParameter(0).Direction = ParameterDirection.Input
            oSqlParameter(0).SqlDbType = SqlDbType.Int
            oSqlParameter(0).Value = iCodigo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_manual_tela_lista", oSqlParameter)

            While oSqlDataReader.Read()

                Dim oTela As New ManualTela

                oTela.controller = SafeGetString(oSqlDataReader, "controller")
                oTela.action = SafeGetString(oSqlDataReader, "action")

                oLista.Add(oTela)

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch ex As Exception
            ' Script 2026-08-28 ainda nao rodado: segue com a tela principal apenas
            If oSqlDataReader IsNot Nothing AndAlso oSqlDataReader.IsClosed = False Then oSqlDataReader.Close()
        End Try

        'Retorno da Funcao
        Return oLista

    End Function

    '-------------------------------------------------------------------------------------------'
    'DESCRICAO     :   Le os dois result sets das procedures do manual: o cabecalho e as secoes. '
    '                  Cabecalho sem linha = tela sem manual, e o objeto volta vazio: o painel   '
    '                  diz que a tela ainda nao tem manual em vez de dar erro.                   '
    '-------------------------------------------------------------------------------------------'
    Private Sub LerManual(ByRef oSqlDataReader As SqlDataReader,
                          ByRef oManual As ManualInfo)

        If oSqlDataReader.Read() Then

            oManual.codigo = CInt(SafeGetLong(oSqlDataReader, "codigo"))
            oManual.tipo = SafeGetString(oSqlDataReader, "tipo")
            oManual.controller = SafeGetString(oSqlDataReader, "controller")
            oManual.action = SafeGetString(oSqlDataReader, "action")
            oManual.processo_codigo = CInt(SafeGetLong(oSqlDataReader, "codigo_manual_processo"))
            oManual.processo_titulo = SafeGetString(oSqlDataReader, "processo_titulo")
            oManual.titulo = SafeGetString(oSqlDataReader, "titulo")
            oManual.subtitulo = SafeGetString(oSqlDataReader, "subtitulo")
            oManual.ativo = SafeGetBoolean(oSqlDataReader, "ativo")

        End If

        If oManual.codigo = 0 Then Exit Sub

        oSqlDataReader.NextResult()

        While oSqlDataReader.Read()

            Dim oItem As New ManualItem

            oItem.codigo = CInt(SafeGetLong(oSqlDataReader, "codigo"))
            oItem.sequencia = CInt(SafeGetLong(oSqlDataReader, "sequencia"))
            oItem.titulo = SafeGetString(oSqlDataReader, "titulo")
            oItem.conteudo = SafeGetString(oSqlDataReader, "conteudo")
            oItem.tipo_nota = SafeGetString(oSqlDataReader, "tipo_nota")
            oItem.nota = SafeGetString(oSqlDataReader, "nota")
            oItem.imagem = SafeGetString(oSqlDataReader, "imagem")
            oItem.video = SafeGetString(oSqlDataReader, "video")

            oManual.itens.Add(oItem)

        End While

    End Sub

    '-------------------------------------------------------------------------------------------'
    'DESCRICAO     :   Grade da manutencao: de que tela (ou processo) e o manual e quantas       '
    '                  secoes tem.                                                              '
    '-------------------------------------------------------------------------------------------'
    Public Function IndexManual(ByVal iCodigoEmpresa As Integer,
                                Optional ByVal sTitulo As String = "") As List(Of ManualGrid)

        'Variaveis Locais
        Dim oSqlParameter(1) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oLista As New List(Of ManualGrid)
        Dim i As Integer = 0

        Try

            'Seta Parametros - Codigo Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Titulo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "titulo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.NVarChar
            oSqlParameter(i).Size = 200
            oSqlParameter(i).Value = If(sTitulo, "")

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_manual_index", oSqlParameter)

            While oSqlDataReader.Read()

                Dim oInfo As New ManualGrid

                oInfo.codigo = CInt(SafeGetLong(oSqlDataReader, "codigo"))
                oInfo.tipo = SafeGetString(oSqlDataReader, "tipo")
                oInfo.titulo = SafeGetString(oSqlDataReader, "titulo")
                oInfo.subtitulo = SafeGetString(oSqlDataReader, "subtitulo")
                oInfo.tela = SafeGetString(oSqlDataReader, "tela")
                oInfo.secoes = CInt(SafeGetLong(oSqlDataReader, "secoes"))
                oInfo.ativo = SafeGetBoolean(oSqlDataReader, "ativo")

                oLista.Add(oInfo)

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Funcao
            Return oLista

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    '-------------------------------------------------------------------------------------------'
    'DESCRICAO     :   Manuais de processo, para o combo "ver tambem" da tela de cadastro.       '
    '-------------------------------------------------------------------------------------------'
    Public Function ComboProcesso(ByVal iCodigoEmpresa As Integer) As List(Of ListCombo)

        'Variaveis Locais
        Dim oSqlParameter(0) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oLista As New List(Of ListCombo)

        Try

            'Seta Parametros - Codigo Empresa
            oSqlParameter(0) = New SqlParameter
            oSqlParameter(0).ParameterName = "codigo_empresa"
            oSqlParameter(0).Direction = ParameterDirection.Input
            oSqlParameter(0).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(0).Value = iCodigoEmpresa

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_manual_combo_processo", oSqlParameter)

            While oSqlDataReader.Read()

                Dim oCombo As New ListCombo

                oCombo.codigo = CInt(SafeGetLong(oSqlDataReader, "codigo"))
                oCombo.descricao = SafeGetString(oSqlDataReader, "descricao")

                oLista.Add(oCombo)

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Funcao
            Return oLista

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: Manutencao :::"

    '-------------------------------------------------------------------------------------------'
    'DESCRICAO     :   Grava o manual inteiro — cabecalho e secoes — numa transacao da propria   '
    '                  procedure. Editar manual e mexer no texto e na ordem das secoes ao mesmo  '
    '                  tempo; gravar tudo de uma vez evita meio-caminho gravado se o navegador   '
    '                  cair no meio da edicao. As secoes vao em JSON e substituem as anteriores. '
    '-------------------------------------------------------------------------------------------'
    Public Function SaveManual(ByVal iCodigoEmpresa As Integer,
                               ByVal oManual As ManualInfo,
                               ByVal sUsuario As String) As Integer

        'Variaveis Locais
        Dim oSqlParameter(11) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Codigo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oManual.codigo : i += 1

            'Seta Parametros - Codigo Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Tipo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "tipo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 1
            oSqlParameter(i).Value = If(oManual.tipo = "P", "P", "S") : i += 1

            'Seta Parametros - Controller
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "controller"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = If(oManual.controller, "") : i += 1

            'Seta Parametros - Action
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "action"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = If(oManual.action, "") : i += 1

            'Seta Parametros - Manual do Processo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_manual_processo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oManual.processo_codigo : i += 1

            'Seta Parametros - Titulo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "titulo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.NVarChar
            oSqlParameter(i).Size = 200
            oSqlParameter(i).Value = If(oManual.titulo, "") : i += 1

            'Seta Parametros - Subtitulo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "subtitulo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.NVarChar
            oSqlParameter(i).Size = 300
            oSqlParameter(i).Value = If(oManual.subtitulo, "") : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = oManual.ativo : i += 1

            'Seta Parametros - Secoes (JSON)
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "itens"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.NVarChar
            oSqlParameter(i).Size = -1
            oSqlParameter(i).Value = Newtonsoft.Json.JsonConvert.SerializeObject(oManual.itens) : i += 1

            'Seta Parametros - Usuario
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = If(sUsuario, "") : i += 1

            'Seta Parametros - Telas adicionais (JSON) — o mesmo manual servindo telas irmas
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "telas"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.NVarChar
            oSqlParameter(i).Size = -1
            oSqlParameter(i).Value = Newtonsoft.Json.JsonConvert.SerializeObject(If(oManual.telas, New List(Of ManualTela)))

            'Executa Query
            Return CInt(ExecuteScalar(sConnection, CommandType.StoredProcedure, "sp_save_manual", oSqlParameter))

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    '-------------------------------------------------------------------------------------------'
    'DESCRICAO     :   Exclui o manual. As secoes saem junto pelo ON DELETE CASCADE.             '
    '-------------------------------------------------------------------------------------------'
    Public Sub DeleteManual(ByVal iCodigo As Integer)

        'Variaveis Locais
        Dim oSqlParameter(0) As SqlParameter

        Try

            'Seta Parametros - Codigo
            oSqlParameter(0) = New SqlParameter
            oSqlParameter(0).ParameterName = "codigo"
            oSqlParameter(0).Direction = ParameterDirection.Input
            oSqlParameter(0).SqlDbType = SqlDbType.Int
            oSqlParameter(0).Value = iCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_manual", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region

End Class
