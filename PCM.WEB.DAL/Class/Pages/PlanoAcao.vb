Imports PCM.WEB.MODELS
Imports System.DirectoryServices
Imports System.DirectoryServices.ActiveDirectory
Imports PCM.WEB.DAL.SQLHelper
Imports System.Data.SqlClient
Imports System.Text
Imports System.IO

Public Class PlanoAcao

    Private sConnection As String

    Sub New(ByVal sCon As String)
        sConnection = sCon
    End Sub

#Region "::: PLANO DE AÇÃO :::"

    Public Sub InsertPlanoAcao(ByVal iCodigoEmpresa As Integer,
                               ByVal iCodigoUsuario As Integer,
                               ByVal iCodigoUnidade As Integer,
                               ByVal iCodigoUsuarioSolicitante As Integer,
                               ByVal iCodigoDepartamento As Integer,
                               ByVal iCodigoResponsavel As Integer,
                               ByVal sDataNecessidade As String,
                               ByVal sDescricao As String,
                               ByVal iCodigoPrioridade As Integer,
                               ByRef lCodigo As Long,
                               ByRef sPlanoAcao As String,
                               ByRef sTo As String,
                               ByRef sBody As String)
        Try

            'Váriaveis Locais
            Dim oSqlParameter(8) As SqlParameter
            Dim oSqlDataReader As SqlDataReader
            Dim i As Integer = 0

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Usuário Solicitante
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario_solicitante"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoUsuarioSolicitante = -1, DBNull.Value, iCodigoUsuarioSolicitante) : i += 1

            'Seta Parametros - Código Departamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_departamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoDepartamento = -1, DBNull.Value, iCodigoDepartamento) : i += 1

            'Seta Parametros - Código Responsável
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_responsavel"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoResponsavel = -1, DBNull.Value, iCodigoResponsavel) : i += 1

            'Seta Parametros - Data Necessidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_necessidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataNecessidade : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 5000
            oSqlParameter(i).Value = sDescricao.Trim.ToUpper : i += 1

            'Seta Parametros - Código Prioridade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_prioridade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoPrioridade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_insert_pcm_plano_acao", oSqlParameter)

            While oSqlDataReader.Read

                'Seta Váriaveis
                lCodigo = oSqlDataReader.Item("codigo")
                sPlanoAcao = oSqlDataReader.Item("plano_acao")
                sTo = oSqlDataReader.Item("to")
                sBody = oSqlDataReader.Item("body")

            End While

            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdatePlanoAcao(ByVal iCodigoEmpresa As Integer,
                               ByVal iCodigoUsuario As Integer,
                               ByVal iCodigoUnidade As Integer,
                               ByVal iCodigoUsuarioSolicitante As Integer,
                               ByVal iCodigoDepartamento As Integer,
                               ByVal iCodigoResponsavel As Integer,
                               ByVal sDataNecessidade As String,
                               ByVal sDescricao As String,
                               ByVal iCodigoPrioridade As Integer,
                               ByVal sImagem As String,
                               ByVal sArquivo As String,
                               ByVal lCodigo As Long,
                               ByVal iCodigoUnidadeOld As Integer)

        Try

            'Váriaveis Locais
            Dim oSqlParameter(10) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Usuário Solicitante
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario_solicitante"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoUsuarioSolicitante = -1, DBNull.Value, iCodigoUsuarioSolicitante) : i += 1

            'Seta Parametros - Código Departamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_departamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoDepartamento = -1, DBNull.Value, iCodigoDepartamento) : i += 1

            'Seta Parametros - Código Responsável
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_responsavel"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoResponsavel = -1, DBNull.Value, iCodigoResponsavel) : i += 1

            'Seta Parametros - Data Necessidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_necessidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataNecessidade : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 5000
            oSqlParameter(i).Value = sDescricao.Trim.ToUpper : i += 1

            'Seta Parametros - Código Prioridade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_prioridade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoPrioridade : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigo : i += 1

            'Seta Parametros - Código Unidade Old
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade_old"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidadeOld

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_pcm_plano_acao", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub DeletePlanoAcao(ByVal iCodigoEmpresa As Integer,
                               ByVal iCodigoUsuario As Integer,
                               ByVal lCodigo As Long,
                               ByVal iCodigoUnidade As Integer)

        'Variaveis Locais
        Dim oSqlParameter(3) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigo : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_pcm_plano_acao", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InfoPlanoAcao(ByVal iCodigoEmpresa As Integer,
                             ByVal lCodigo As Long,
                             ByVal iCodigoUnidade As Integer,
                             ByRef oPlanoAcao As MODELS.PlanoAcao)

        'Variaveis Locais
        Dim oSqlParameter(2) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oPicture As New Picture(sConnection)
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigo : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pcm_plano_acao_dados", oSqlParameter)

            While oSqlDataReader.Read

                oPlanoAcao = New MODELS.PlanoAcao
                oPlanoAcao.codigo = oSqlDataReader("codigo")
                oPlanoAcao.codigo_unidade = oSqlDataReader("codigo_unidade")
                oPlanoAcao.unidade = oSqlDataReader("unidade")
                oPlanoAcao.numero_documento = oSqlDataReader("numero_documento")
                oPlanoAcao.data_abertura = oSqlDataReader("data_abertura")
                oPlanoAcao.data_execucao = oSqlDataReader("data_execucao")
                oPlanoAcao.descricao = oSqlDataReader("descricao")
                oPlanoAcao.codigo_usuario_solicitante = oSqlDataReader("codigo_usuario_solicitante")
                oPlanoAcao.solicitante = oSqlDataReader("solicitante")
                oPlanoAcao.codigo_prioridade = oSqlDataReader("codigo_prioridade")
                oPlanoAcao.prioridade = oSqlDataReader("prioridade")
                oPlanoAcao.codigo_departamento = oSqlDataReader("codigo_departamento")
                oPlanoAcao.departamento = oSqlDataReader("departamento")
                oPlanoAcao.codigo_responsavel = oSqlDataReader("codigo_responsavel")
                oPlanoAcao.responsavel = oSqlDataReader("responsavel")
                oPlanoAcao.status = oSqlDataReader("status")
                oPlanoAcao.data_necessidade = oSqlDataReader("data_necessidade")
                oPlanoAcao.fotos = oPicture.PictureList(iCodigoEmpresa:=iCodigoEmpresa,
                                                           iCodigoUnidade:=oSqlDataReader.Item("codigo_unidade"),
                                                           lCodigo:=oSqlDataReader.Item("codigo"),
                                                           iCodigoItemChecklist:=-1,
                                                           sTipo:="PA")

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function IndexPlanoAcao(ByVal iCodigoEmpresa As Integer,
                                   ByVal iCodigoUsuario As Integer,
                                   Optional ByVal iCodigoUnidade As Integer = 0,
                                   Optional ByVal iCodigoDepartamento As Integer = -1,
                                   Optional ByVal sDataInicio As String = "",
                                   Optional ByVal sDataTermino As String = "",
                                   Optional ByVal sPlanoAcao As String = "",
                                   Optional ByVal iCodigoPrioridade As Integer = 0,
                                   Optional ByVal iCodigoUsuarioSolicitante As Integer = 0,
                                   Optional ByVal sExecutor As String = "",
                                   Optional ByVal iStatus As Integer = 0,
                                   Optional ByVal iCodigoJustificativaApontamento As Integer = -1) As List(Of MODELS.PlanoAcao)

        Try

            'Váriaveis Locais
            Dim oPlanoAcao As New List(Of MODELS.PlanoAcao)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(11) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Departamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_departamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoDepartamento : i += 1

            'Seta Parametros - Data Início
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_inicio"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = IIf(sDataInicio = "", DBNull.Value, sDataInicio) : i += 1

            'Seta Parametros - Data Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_termino"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = IIf(sDataTermino = "", DBNull.Value, sDataTermino) : i += 1

            'Seta Parametros - Plano de Ação
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "plano_acao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sPlanoAcao : i += 1

            'Seta Parametros - Código Prioridade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_prioridade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoPrioridade : i += 1

            'Seta Parametros - Código Usuário Solicitante
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario_solicitante"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuarioSolicitante : i += 1

            'Seta Parametros - Executor
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "executor"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Value = 100
            oSqlParameter(i).Value = sExecutor : i += 1

            'Seta Parametros - Status
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "status"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iStatus : i += 1

            'Seta Parametros - Código Justificativa - Apontamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_justificativa_apontamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoJustificativaApontamento

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pcm_plano_acao", oSqlParameter)

            While oSqlDataReader.Read

                Dim oPlanoAcaoInfo As New MODELS.PlanoAcao

                oPlanoAcaoInfo.codigo = oSqlDataReader("codigo")
                oPlanoAcaoInfo.codigo_unidade = oSqlDataReader("codigo_unidade")
                oPlanoAcaoInfo.data_necessidade = oSqlDataReader("data_necessidade")
                oPlanoAcaoInfo.unidade = oSqlDataReader("unidade")
                oPlanoAcaoInfo.solicitante = oSqlDataReader("solicitante")
                oPlanoAcaoInfo.data_execucao = oSqlDataReader("data_execucao")
                oPlanoAcaoInfo.numero_documento = oSqlDataReader("numero_documento")
                oPlanoAcaoInfo.percentual = oSqlDataReader("percentual")
                oPlanoAcaoInfo.descricao = oSqlDataReader("descricao")
                oPlanoAcaoInfo.departamento = oSqlDataReader("departamento")
                oPlanoAcaoInfo.responsavel = oSqlDataReader("responsavel")
                oPlanoAcaoInfo.codigo_prioridade = oSqlDataReader("codigo_prioridade")
                oPlanoAcaoInfo.prioridade = oSqlDataReader("prioridade")
                oPlanoAcaoInfo.status = oSqlDataReader("status")
                oPlanoAcaoInfo.status_descricao = oSqlDataReader("status_descricao")
                oPlanoAcaoInfo.css_class = oSqlDataReader("css_class")
                oPlanoAcaoInfo.dias = oSqlDataReader("dias")

                oPlanoAcao.Add(oPlanoAcaoInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oPlanoAcao

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function PlanoAcaoStatus(ByVal iCodigoEmpresa As Integer,
                                    ByVal iCodigoUnidade As Integer,
                                    ByVal iCodigoUsuario As Integer) As MODELS.PlanoAcaoStatus

        Try

            'Váriaveis Locais
            Dim oReturn As MODELS.PlanoAcaoStatus
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(2) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pcm_plano_acao_status", oSqlParameter)

            oReturn = New MODELS.PlanoAcaoStatus
            oReturn.pendente = "0"
            oReturn.concluido = "0"
            oReturn.atrasado = "0"
            oReturn.em_andamento = "0"

            While oSqlDataReader.Read

                Select Case oSqlDataReader.Item("status")
                    Case 1 : oReturn.pendente = oSqlDataReader.Item("quantidade")
                    Case 2 : oReturn.concluido = oSqlDataReader.Item("quantidade")
                    Case 3 : oReturn.atrasado = oSqlDataReader.Item("quantidade")
                    Case 4 : oReturn.em_andamento = oSqlDataReader.Item("quantidade")
                End Select

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Sub UpdatePlanoAcaoStatus(ByVal iCodigoEmpresa As Integer,
                                        ByVal iCodigoUsuario As Integer,
                                        ByVal lCodigo As Long,
                                        ByVal iCodigoUnidade As Integer,
                                        ByVal iStatus As Integer)

        'Váriaveis Locais
        Dim oSqlParameter(4) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Ordem de Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_pcm_ordem_servico"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigo : i += 1

            'Seta Parametros - Status
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "status"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iStatus

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_pcm_plano_acao_status", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function UpdatePlanoAcaoDataNecessidade(ByVal iCodigoEmpresa As Integer,
                                                   ByVal iCodigoUsuario As Integer,
                                                   ByVal lCodigo As Long,
                                                   ByVal iCodigoUnidade As Integer,
                                                   ByVal sDataNecessidade As String) As Boolean

        'Váriaveis Locais
        Dim oSqlParameter(4) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Ordem de Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_pcm_ordem_servico"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigo : i += 1

            'Seta Parametros - Data Necessidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_necessidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.DateTime
            oSqlParameter(i).Value = sDataNecessidade

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_pcm_plano_acao_data_necessidade", oSqlParameter)

            Return True

        Catch SqlEx As SqlException
            Return False
        Catch ex As Exception
            Return False
        End Try

    End Function

    Public Function PlanoAcaoApontamento(ByVal iCodigoEmpresa As Integer,
                                         ByVal iCodigoUnidade As Integer,
                                         ByVal lCodigoPlanoAcao As Long) As List(Of MODELS.PlanoAcaoApontamento)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of MODELS.PlanoAcaoApontamento)
            Dim oPicture As New Picture(sConnection)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(2) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código PCM Plano Ação
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_pcm_plano_acao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoPlanoAcao

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pcm_plano_acao_apontamento", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New MODELS.PlanoAcaoApontamento

                oInfo.item = oSqlDataReader.Item("item")
                oInfo.justificativa_apontamento = oSqlDataReader.Item("justificativa_apontamento")
                oInfo.executor = oSqlDataReader.Item("executor")
                oInfo.data = oSqlDataReader.Item("data")
                oInfo.descricao = oSqlDataReader.Item("descricao")
                oInfo.observacao = oSqlDataReader.Item("observacao")
                oInfo.percentual = oSqlDataReader.Item("percentual")
                oInfo.fotos = oPicture.PictureList(iCodigoEmpresa:=iCodigoEmpresa,
                                                   iCodigoUnidade:=oSqlDataReader.Item("codigo_unidade"),
                                                   lCodigo:=oSqlDataReader.Item("codigo"),
                                                   iCodigoItemChecklist:=-1,
                                                   sTipo:="PA_APONTAMENTO")

                oReturn.Add(oInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: APONTAMENTO :::"

    Public Sub LoadApontamentoPlanoAcao(ByVal iCodigoEmpresa As Integer,
                                        ByVal iCodigoUsuario As Integer,
                                        ByVal lCodigoPCMPlanoAcao As Long,
                                        ByVal iCodigoUnidade As Integer,
                                        ByRef oPlanoAcaoApontamento As PlanoAcaoApontamento)

        Try

            'Váriaveis Locais
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(4) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código PCM Plano de Ação
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_pcm_plano_acao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoPCMPlanoAcao

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pcm_apontamento_plano_acao", oSqlParameter)

            While oSqlDataReader.Read

                oPlanoAcaoApontamento = New PlanoAcaoApontamento

                oPlanoAcaoApontamento.codigo = oSqlDataReader.Item("codigo")
                oPlanoAcaoApontamento.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oPlanoAcaoApontamento.unidade = oSqlDataReader.Item("unidade")
                oPlanoAcaoApontamento.plano_acao = oSqlDataReader.Item("plano_acao")
                oPlanoAcaoApontamento.prioridade = oSqlDataReader.Item("prioridade")
                oPlanoAcaoApontamento.departamento = oSqlDataReader.Item("departamento")
                oPlanoAcaoApontamento.responsavel = oSqlDataReader.Item("responsavel")
                oPlanoAcaoApontamento.descricao = oSqlDataReader.Item("descricao")
                oPlanoAcaoApontamento.percentual = oSqlDataReader.Item("percentual")

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InsertApontamentoPlanoAcao(ByVal iCodigoEmpresa As Integer,
                                          ByVal iCodigoUsuario As Integer,
                                          ByVal lCodigoPCMPlanoAcao As Long,
                                          ByVal iCodigoUnidade As Integer,
                                          ByVal sData As String,
                                          ByVal sDescricao As String,
                                          ByVal dValor As Double,
                                          ByVal bConcluido As Boolean,
                                          ByVal iCodigoJustificativaApontamento As Integer,
                                          ByVal dPercentual As Double,
                                          ByVal sObservacao As String,
                                          ByRef lCodigo As Long)

        Try

            'Váriaveis Locais
            Dim oSqlParameter(11) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código PCM Plano Ação
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_pcm_plano_acao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoPCMPlanoAcao : i += 1

            'Seta Parametros - Data
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sData : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = sDescricao.ToUpper() : i += 1

            'Seta Parametros - Valor
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "valor"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = dValor : i += 1

            'Seta Parametros - Concluído
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "concluido"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bConcluido : i += 1

            'Seta Parametros - Justificativa Apontamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_justificativa_apontamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoJustificativaApontamento = -1, DBNull.Value, iCodigoJustificativaApontamento) : i += 1

            'Seta Parametros - Percentual
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "percentual"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = dPercentual : i += 1

            'Seta Parametros - Observação
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "observacao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = sObservacao : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Output
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_pcm_apontamento_plano_acao", oSqlParameter)

            lCodigo = oSqlParameter(i).Value

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region

End Class
