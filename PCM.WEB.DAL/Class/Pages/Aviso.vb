'==============================================================================================='
'Classe:        Aviso                                                                           '
'Objetivo:      Avisos aos clientes — popup exibido no login, com secoes em carrossel,          '
'               auditoria e avaliacao opcionais.                                                '
'                                                                                               '
'               Estrutura e procedures em PCM.WEB.DAL\Scripts\2026-08-28_avisos_clientes.sql.   '
'==============================================================================================='
Imports System.Data.SqlClient
Imports PCM.WEB.DAL.SQLHelper
Imports PCM.WEB.MODELS

' O modelo e esta classe se chamam Aviso — mesmo caso do Manual: o alias diz de qual
' Aviso se trata em cada assinatura (dentro do namespace da DAL, o prefixo PCM bate
' na classe PCM desta mesma camada e o nome qualificado nao resolveria).
Imports AvisoInfo = PCM.WEB.MODELS.Aviso

Public Class Aviso

    Private sConnection As String

    Sub New(ByVal sCon As String)
        sConnection = sCon
    End Sub

#Region "::: Manutencao :::"

    '-------------------------------------------------------------------------------------------'
    'DESCRICAO     :   Grade da manutencao: periodo, alvo, flags e numeros de cada aviso.        '
    '-------------------------------------------------------------------------------------------'
    Public Function IndexAviso(ByVal iCodigoEmpresa As Integer) As List(Of AvisoGrid)

        'Variaveis Locais
        Dim oSqlParameter(0) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oLista As New List(Of AvisoGrid)

        Try

            'Seta Parametros - Codigo Empresa
            oSqlParameter(0) = New SqlParameter
            oSqlParameter(0).ParameterName = "codigo_empresa"
            oSqlParameter(0).Direction = ParameterDirection.Input
            oSqlParameter(0).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(0).Value = iCodigoEmpresa

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_aviso_index", oSqlParameter)

            While oSqlDataReader.Read

                Dim oLinha As New AvisoGrid

                oLinha.codigo = CInt(SafeGetLong(oSqlDataReader, "codigo"))
                oLinha.titulo = SafeGetString(oSqlDataReader, "titulo")
                oLinha.data_inicio = SafeGetString(oSqlDataReader, "data_inicio")
                oLinha.data_termino = SafeGetString(oSqlDataReader, "data_termino")
                oLinha.codigo_empresa = CInt(SafeGetLong(oSqlDataReader, "codigo_empresa"))
                oLinha.codigo_unidade = CInt(SafeGetLong(oSqlDataReader, "codigo_unidade"))
                oLinha.unidade = SafeGetString(oSqlDataReader, "unidade")
                oLinha.auditado = SafeGetBoolean(oSqlDataReader, "auditado")
                oLinha.avaliado = SafeGetBoolean(oSqlDataReader, "avaliado")
                oLinha.ativo = SafeGetBoolean(oSqlDataReader, "ativo")
                oLinha.situacao = CInt(SafeGetLong(oSqlDataReader, "situacao"))
                oLinha.visualizacoes = CInt(SafeGetLong(oSqlDataReader, "visualizacoes"))
                oLinha.avaliacoes = CInt(SafeGetLong(oSqlDataReader, "avaliacoes"))
                oLinha.media_avaliacao = Convert.ToDecimal(oSqlDataReader.Item("media_avaliacao"))

                oLista.Add(oLinha)

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
    'DESCRICAO     :   Um aviso (cabecalho + secoes) para a tela de edicao.                      '
    '-------------------------------------------------------------------------------------------'
    Public Function InfoAviso(ByVal iCodigo As Integer) As AvisoInfo

        'Variaveis Locais
        Dim oSqlParameter(0) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oAviso As New AvisoInfo

        Try

            'Seta Parametros - Codigo
            oSqlParameter(0) = New SqlParameter
            oSqlParameter(0).ParameterName = "codigo"
            oSqlParameter(0).Direction = ParameterDirection.Input
            oSqlParameter(0).SqlDbType = SqlDbType.Int
            oSqlParameter(0).Value = iCodigo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_aviso", oSqlParameter)

            If oSqlDataReader.Read() Then

                oAviso.codigo = CInt(SafeGetLong(oSqlDataReader, "codigo"))
                oAviso.titulo = SafeGetString(oSqlDataReader, "titulo")
                oAviso.data_inicio = SafeGetString(oSqlDataReader, "data_inicio")
                oAviso.data_termino = SafeGetString(oSqlDataReader, "data_termino")
                oAviso.codigo_empresa = CInt(SafeGetLong(oSqlDataReader, "codigo_empresa"))
                oAviso.codigo_unidade = CInt(SafeGetLong(oSqlDataReader, "codigo_unidade"))
                oAviso.auditado = SafeGetBoolean(oSqlDataReader, "auditado")
                oAviso.avaliado = SafeGetBoolean(oSqlDataReader, "avaliado")
                oAviso.ativo = SafeGetBoolean(oSqlDataReader, "ativo")

            End If

            If oAviso.codigo > 0 Then

                oSqlDataReader.NextResult()

                While oSqlDataReader.Read()

                    Dim oSecao As New AvisoSecao

                    oSecao.codigo = CInt(SafeGetLong(oSqlDataReader, "codigo"))
                    oSecao.sequencia = CInt(SafeGetLong(oSqlDataReader, "sequencia"))
                    oSecao.titulo = SafeGetString(oSqlDataReader, "titulo")
                    oSecao.conteudo = SafeGetString(oSqlDataReader, "conteudo")

                    oAviso.secoes.Add(oSecao)

                End While

            End If

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Funcao
            Return oAviso

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    '-------------------------------------------------------------------------------------------'
    'DESCRICAO     :   Grava o aviso inteiro — cabecalho e secoes (JSON) — na transacao da       '
    '                  propria procedure, como o manual: salvar tudo de uma vez evita meio-      '
    '                  caminho gravado se o navegador cair durante a edicao.                     '
    '-------------------------------------------------------------------------------------------'
    Public Function SaveAviso(ByVal oAviso As AvisoInfo,
                              ByVal sUsuario As String) As Integer

        'Variaveis Locais
        Dim oSqlParameter(10) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Codigo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oAviso.codigo : i += 1

            'Seta Parametros - Codigo Empresa (-1 = todas)
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = oAviso.codigo_empresa : i += 1

            'Seta Parametros - Codigo Unidade (-1 = todas)
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oAviso.codigo_unidade : i += 1

            'Seta Parametros - Titulo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "titulo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.NVarChar
            oSqlParameter(i).Size = 200
            oSqlParameter(i).Value = If(oAviso.titulo, "") : i += 1

            'Seta Parametros - Data Inicio
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_inicio"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = oAviso.data_inicio : i += 1

            'Seta Parametros - Data Termino
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_termino"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = oAviso.data_termino : i += 1

            'Seta Parametros - Auditado
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "auditado"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = oAviso.auditado : i += 1

            'Seta Parametros - Avaliado
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "avaliado"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = oAviso.avaliado : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = oAviso.ativo : i += 1

            'Seta Parametros - Secoes (JSON)
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "secoes"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.NVarChar
            oSqlParameter(i).Size = -1
            oSqlParameter(i).Value = Newtonsoft.Json.JsonConvert.SerializeObject(oAviso.secoes) : i += 1

            'Seta Parametros - Usuario
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = If(sUsuario, "")

            'Executa Query
            Return CInt(ExecuteScalar(sConnection, CommandType.StoredProcedure, "sp_save_aviso", oSqlParameter))

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    '-------------------------------------------------------------------------------------------'
    'DESCRICAO     :   Exclui o aviso (secoes e log caem pelo CASCADE).                          '
    '-------------------------------------------------------------------------------------------'
    Public Sub DeleteAviso(ByVal iCodigo As Integer)

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
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_aviso", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region

#Region "::: Popup do login :::"

    '-------------------------------------------------------------------------------------------'
    'DESCRICAO     :   Avisos pendentes do usuario: vigentes hoje, do alvo dele e ainda nao      '
    '                  dispensados. Base sem as tabelas (script nao rodado) devolve lista        '
    '                  vazia — o login nunca quebra por causa do aviso.                          '
    '-------------------------------------------------------------------------------------------'
    Public Function AvisosPendentes(ByVal iCodigoEmpresa As Integer,
                                    ByVal iCodigoUnidade As Integer,
                                    ByVal iCodigoUsuario As Integer) As List(Of AvisoInfo)

        'Variaveis Locais
        Dim oSqlParameter(2) As SqlParameter
        Dim oSqlDataReader As SqlDataReader = Nothing
        Dim oLista As New List(Of AvisoInfo)
        Dim i As Integer = 0

        Try

            'Seta Parametros - Codigo Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Codigo Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Codigo Usuario
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_aviso_login", oSqlParameter)

            While oSqlDataReader.Read

                Dim oAviso As New AvisoInfo

                oAviso.codigo = CInt(SafeGetLong(oSqlDataReader, "codigo"))
                oAviso.titulo = SafeGetString(oSqlDataReader, "titulo")
                oAviso.data_termino = SafeGetString(oSqlDataReader, "data_termino")
                oAviso.auditado = SafeGetBoolean(oSqlDataReader, "auditado")
                oAviso.avaliado = SafeGetBoolean(oSqlDataReader, "avaliado")
                oAviso.avaliacao = CInt(SafeGetLong(oSqlDataReader, "avaliacao"))

                oLista.Add(oAviso)

            End While

            'Secoes de todos os avisos pendentes, no segundo result set
            oSqlDataReader.NextResult()

            While oSqlDataReader.Read

                Dim iCodigoAviso As Integer = CInt(SafeGetLong(oSqlDataReader, "codigo_aviso"))

                For Each oAviso As AvisoInfo In oLista

                    If oAviso.codigo = iCodigoAviso Then

                        Dim oSecao As New AvisoSecao
                        oSecao.sequencia = CInt(SafeGetLong(oSqlDataReader, "sequencia"))
                        oSecao.titulo = SafeGetString(oSqlDataReader, "titulo")
                        oSecao.conteudo = SafeGetString(oSqlDataReader, "conteudo")

                        oAviso.secoes.Add(oSecao)
                        Exit For

                    End If

                Next

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch ex As Exception
            'Script 2026-08-28 ainda nao rodado: login segue sem avisos
            If oSqlDataReader IsNot Nothing AndAlso oSqlDataReader.IsClosed = False Then oSqlDataReader.Close()
            oLista.Clear()
        End Try

        'Retorno da Funcao
        Return oLista

    End Function

    '-------------------------------------------------------------------------------------------'
    'DESCRICAO     :   Registros por usuario: visualizacao, avaliacao e dispensa (upsert na SP). '
    '-------------------------------------------------------------------------------------------'
    Public Sub RegistrarVisualizacao(ByVal iCodigoAviso As Integer, ByVal iCodigoEmpresa As Integer, ByVal iCodigoUsuario As Integer)
        ExecutarRegistro("sp_update_aviso_visualizacao", iCodigoAviso, iCodigoEmpresa, iCodigoUsuario, -1)
    End Sub

    Public Sub RegistrarAvaliacao(ByVal iCodigoAviso As Integer, ByVal iCodigoEmpresa As Integer, ByVal iCodigoUsuario As Integer, ByVal iAvaliacao As Integer)
        ExecutarRegistro("sp_update_aviso_avaliacao", iCodigoAviso, iCodigoEmpresa, iCodigoUsuario, iAvaliacao)
    End Sub

    Public Sub RegistrarDispensa(ByVal iCodigoAviso As Integer, ByVal iCodigoEmpresa As Integer, ByVal iCodigoUsuario As Integer)
        ExecutarRegistro("sp_update_aviso_dispensa", iCodigoAviso, iCodigoEmpresa, iCodigoUsuario, -1)
    End Sub

    Private Sub ExecutarRegistro(ByVal sProcedure As String,
                                 ByVal iCodigoAviso As Integer,
                                 ByVal iCodigoEmpresa As Integer,
                                 ByVal iCodigoUsuario As Integer,
                                 ByVal iAvaliacao As Integer)

        'Variaveis Locais
        Dim iParametros As Integer = If(iAvaliacao >= 0, 3, 2)
        Dim oSqlParameter(iParametros) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Codigo Aviso
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_aviso"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoAviso : i += 1

            'Seta Parametros - Codigo Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Codigo Usuario
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            If iAvaliacao >= 0 Then
                'Seta Parametros - Avaliacao
                oSqlParameter(i) = New SqlParameter
                oSqlParameter(i).ParameterName = "avaliacao"
                oSqlParameter(i).Direction = ParameterDirection.Input
                oSqlParameter(i).SqlDbType = SqlDbType.TinyInt
                oSqlParameter(i).Value = iAvaliacao
            End If

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, sProcedure, oSqlParameter)

        Catch ex As Exception
            'Registro do aviso e melhor-esforco: nunca derruba a navegacao do usuario
        End Try

    End Sub

#End Region

#Region "::: Auditoria :::"

    '-------------------------------------------------------------------------------------------'
    'DESCRICAO     :   Resumo + linhas de auditoria de um aviso.                                 '
    '-------------------------------------------------------------------------------------------'
    Public Function AuditoriaAviso(ByVal iCodigo As Integer) As AvisoAuditoriaResumo

        'Variaveis Locais
        Dim oSqlParameter(0) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oResumo As New AvisoAuditoriaResumo

        Try

            'Seta Parametros - Codigo
            oSqlParameter(0) = New SqlParameter
            oSqlParameter(0).ParameterName = "codigo"
            oSqlParameter(0).Direction = ParameterDirection.Input
            oSqlParameter(0).SqlDbType = SqlDbType.Int
            oSqlParameter(0).Value = iCodigo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_aviso_auditoria", oSqlParameter)

            If oSqlDataReader.Read() Then

                oResumo.titulo = SafeGetString(oSqlDataReader, "titulo")
                oResumo.visualizacoes = CInt(SafeGetLong(oSqlDataReader, "visualizacoes"))
                oResumo.usuarios = CInt(SafeGetLong(oSqlDataReader, "usuarios"))
                oResumo.dispensaram = CInt(SafeGetLong(oSqlDataReader, "dispensaram"))
                oResumo.avaliaram = CInt(SafeGetLong(oSqlDataReader, "avaliaram"))
                oResumo.media_avaliacao = Convert.ToDecimal(oSqlDataReader.Item("media_avaliacao"))

            End If

            oSqlDataReader.NextResult()

            While oSqlDataReader.Read

                Dim oLinha As New AvisoAuditoriaLinha

                oLinha.usuario = SafeGetString(oSqlDataReader, "usuario")
                oLinha.ultima_visualizacao = SafeGetString(oSqlDataReader, "ultima_visualizacao")
                oLinha.exibicoes = CInt(SafeGetLong(oSqlDataReader, "exibicoes"))
                oLinha.avaliacao = CInt(SafeGetLong(oSqlDataReader, "avaliacao"))
                oLinha.nao_ver_mais = SafeGetBoolean(oSqlDataReader, "nao_ver_mais")
                oLinha.data_dispensa = SafeGetString(oSqlDataReader, "data_dispensa")

                oResumo.linhas.Add(oLinha)

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Funcao
            Return oResumo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

End Class
