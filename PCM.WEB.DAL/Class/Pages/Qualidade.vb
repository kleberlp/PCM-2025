Imports System.Data.SqlClient
Imports System.Net.Mail
Imports PCM.WEB.DAL.SQLHelper
Imports PCM.WEB.MODELS

Public Class Qualidade

    Private sConnection As String

    Sub New(ByVal sCon As String)
        sConnection = sCon
    End Sub

#Region "::: AUDITORIA INTERNA :::"

    Public Sub InsertAuditoria(ByVal iCodigoEmpresa As Integer,
                               ByVal iCodigoUnidade As Integer,
                               ByVal iCodigoModulo As Integer,
                               ByVal sDescricao As String,
                               ByVal lCodigoChecklist As Long,
                               ByVal iCodigoPeriodicidade As Integer,
                               ByVal iIntervalo As Integer,
                               ByVal bAtivo As Boolean,
                               ByVal iCodigoUsuario As Integer)

        'Variaveis Locais
        Dim oSqlParameter(8) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sDescricao.ToUpper() : i += 1

            'Seta Parametros - Código Checklist
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_checklist"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoChecklist : i += 1

            'Seta Parametros - Código Periodicidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_periodicidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoPeriodicidade : i += 1

            'Seta Parametros - Intervalo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "intervalo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iIntervalo : i += 1

            'Seta Parametros - Código Módulo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_modulo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = iCodigoModulo : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_cadastro_basico_auditoria_interna", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateAuditoria(ByVal iCodigoEmpresa As Integer,
                               ByVal iCodigoUnidade As Integer,
                               ByVal iCodigoModulo As Integer,
                               ByVal sDescricao As String,
                               ByVal lCodigoChecklist As Long,
                               ByVal iCodigoPeriodicidade As Integer,
                               ByVal iIntervalo As Integer,
                               ByVal bAtivo As Boolean,
                               ByVal iCodigoUsuario As Integer,
                               ByVal iCodigo As Integer)

        'Variaveis Locais
        Dim oSqlParameter(9) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sDescricao.ToUpper() : i += 1

            'Seta Parametros - Código Checklist
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_checklist"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoChecklist : i += 1

            'Seta Parametros - Código Periodicidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_periodicidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoPeriodicidade : i += 1

            'Seta Parametros - Intervalo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "intervalo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iIntervalo : i += 1

            'Seta Parametros - Código Módulo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_modulo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoModulo : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo : i += 1

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
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_cadastro_basico_auditoria_interna", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub DeleteAuditoria(ByVal iCodigoEmpresa As Integer,
                               ByVal iCodigoUsuario As Integer,
                               ByVal iCodigoUnidade As Integer,
                               ByVal iCodigo As Integer)

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

            'Seta Parametros - Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_cadastro_basico_auditoria_interna", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InfoAuditoria(ByVal iCodigoEmpresa As Integer,
                             ByVal iCodigoUnidade As Integer,
                             ByVal iCodigo As Integer,
                             ByRef oAuditoriaQualidade As AuditoriaQualidade)

        'Variaveis Locais
        Dim oSqlParameter(2) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_auditoria_interna_dados", oSqlParameter)

            While oSqlDataReader.Read

                oAuditoriaQualidade = New AuditoriaQualidade
                oAuditoriaQualidade.codigo = oSqlDataReader.Item("codigo")
                oAuditoriaQualidade.descricao = oSqlDataReader.Item("descricao")
                oAuditoriaQualidade.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oAuditoriaQualidade.unidade = oSqlDataReader.Item("unidade")
                oAuditoriaQualidade.codigo_checklist = oSqlDataReader.Item("codigo_checklist")
                oAuditoriaQualidade.checklist = oSqlDataReader.Item("checklist")
                oAuditoriaQualidade.codigo_periodicidade = oSqlDataReader.Item("codigo_periodicidade")
                oAuditoriaQualidade.periodicidade = oSqlDataReader.Item("periodicidade")
                oAuditoriaQualidade.intervalo = oSqlDataReader.Item("intervalo")
                oAuditoriaQualidade.ativo = oSqlDataReader.Item("ativo")

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function IndexAuditoria(ByVal iCodigoEmpresa As Integer,
                                   ByVal iCodigoUnidade As Integer,
                                   ByVal iCodigoModulo As Integer) As List(Of AuditoriaQualidade)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of AuditoriaQualidade)
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

            'Seta Parametros - Código Módulo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_modulo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoModulo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_auditoria_interna", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New AuditoriaQualidade

                oInfo.codigo = oSqlDataReader.Item("codigo")
                oInfo.descricao = oSqlDataReader.Item("descricao")
                oInfo.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oInfo.unidade = oSqlDataReader.Item("unidade")
                oInfo.codigo_checklist = oSqlDataReader.Item("codigo_checklist")
                oInfo.checklist = oSqlDataReader.Item("checklist")
                oInfo.ativo = oSqlDataReader.Item("ativo")

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

    Public Function ValidaAuditoria(ByVal iCodigoEmpresa As Integer,
                                    ByVal iCodigoUnidade As Integer,
                                    ByVal sDescricao As String,
                                    ByVal iCodigo As Integer) As Boolean

        Try

            'Váriaveis Locais
            Dim oSqlParameter(3) As SqlParameter
            Dim i As Integer = 0
            Dim iReturn As Integer

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

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sDescricao

            'Executa Query
            iReturn = ExecuteScalar(sConnection, CommandType.StoredProcedure, "sp_validate_cadastro_basico_apartamento", oSqlParameter)

            'Retorno da Função
            Return (IIf(iReturn > 0, False, True))

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: AUDITORIA :::"

    Public Function LoadPlanoAcaoStatus(ByVal iCodigoEmpresa As Integer,
                                        ByVal iCodigoUnidade As Integer) As QAAuditoriaStatus

        Try

            'Váriaveis Locais
            Dim oReturn As New QAAuditoriaStatus
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(1) As SqlParameter
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
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_qualidade_plano_acao_status", oSqlParameter)

            While oSqlDataReader.Read

                oReturn.atrasado = oSqlDataReader.Item("atrasado")
                oReturn.pendente = oSqlDataReader.Item("pendente")
                oReturn.concluido = oSqlDataReader.Item("concluido")
                oReturn.em_andamento = oSqlDataReader.Item("em_andamento")

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

    Public Function LoadAuditoriaStatus(ByVal iCodigoEmpresa As Integer,
                                        ByVal iCodigoUnidade As Integer) As QAAuditoriaStatus

        Try

            'Váriaveis Locais
            Dim oReturn As New QAAuditoriaStatus
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(1) As SqlParameter
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
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_qualidade_auditoria_status", oSqlParameter)

            While oSqlDataReader.Read

                oReturn.atrasado = oSqlDataReader.Item("atrasado")
                oReturn.pendente = oSqlDataReader.Item("pendente")
                oReturn.concluido = oSqlDataReader.Item("concluido")
                oReturn.em_andamento = oSqlDataReader.Item("em_andamento")

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

    Public Function LoadAuditoria(ByVal iCodigoEmpresa As Integer,
                                  ByVal iCodigoUnidade As Integer,
                                  Optional ByVal iStatus As Integer = -1) As List(Of QAAuditoria)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of QAAuditoria)
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

            'Seta Parametros - Status
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "status"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iStatus

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_qualidade_auditoria", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New QAAuditoria

                oInfo.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oInfo.unidade = oSqlDataReader.Item("nome_fantasia")
                oInfo.auditoria = LoadAuditoriaDia(iCodigoEmpresa:=iCodigoEmpresa,
                                                   iCodigoUnidade:=oSqlDataReader.Item("codigo_unidade"),
                                                   iStatus:=iStatus)

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

    Public Function LoadAuditoriaDia(ByVal iCodigoEmpresa As Integer,
                                     ByVal iCodigoUnidade As Integer,
                                     ByVal iStatus As Integer) As List(Of QAAuditoriaDia)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of QAAuditoriaDia)
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

            'Seta Parametros - Status
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "status"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iStatus

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_qualidade_auditoria_dia", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New QAAuditoriaDia

                oInfo.codigo_auditoria_interna = oSqlDataReader.Item("codigo_auditoria_interna")
                oInfo.codigo_auditoria = oSqlDataReader.Item("codigo_auditoria")
                oInfo.codigo_checklist = oSqlDataReader.Item("codigo_checklist")
                oInfo.descricao = oSqlDataReader.Item("descricao")
                oInfo.status = oSqlDataReader.Item("status")
                oInfo.descricao_status = oSqlDataReader.Item("descricao_status")
                oInfo.css_class = oSqlDataReader.Item("css_class")
                oInfo.data = oSqlDataReader.Item("data")
                oInfo.usuario = oSqlDataReader.Item("usuario")
                oInfo.pontos_possiveis = oSqlDataReader.Item("pontos_possiveis")
                oInfo.pontos_realizados = oSqlDataReader.Item("pontos_realizados")
                oInfo.conforme = oSqlDataReader.Item("conforme")
                oInfo.nao_conforme = oSqlDataReader.Item("nao_conforme")
                oInfo.nao_respondido = oSqlDataReader.Item("nao_respondido")
                oInfo.nao_aplicavel = oSqlDataReader.Item("nao_aplicavel")
                oInfo.nota = oSqlDataReader.Item("nota")

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

    Public Sub ReabrirAuditoriaQualidade(ByVal iCodigoEmpresa As Integer,
                                         ByVal iCodigoUsuario As Integer,
                                         ByVal iCodigoUnidade As Integer,
                                         ByVal lCodigo As Long)

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

            'Seta Parametros - Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_auditoria_qualidade_reabrir", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub DeleteAuditoriaQualidade(ByVal iCodigoEmpresa As Integer,
                                        ByVal iCodigoUsuario As Integer,
                                        ByVal iCodigoUnidade As Integer,
                                        ByVal lCodigo As Long)

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

            'Seta Parametros - Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_auditoria_qualidade", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region

#Region "::: AUDITORIA HISTÓRICO :::"

    Public Function AuditoriaHistorico(ByVal iCodigoEmpresa As Integer,
                                       ByVal iCodigoUnidade As Integer,
                                       ByVal sDataInicio As String,
                                       ByVal sDataTermino As String,
                                       ByVal iCodigoAuditoriaInterna As Integer,
                                       ByVal iStatus As Integer) As List(Of QAAuditoriaHistorico)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of MODELS.QAAuditoriaHistorico)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(5) As SqlParameter
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

            'Seta Parametros - Código Auditoria Interna
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_auditoria_interna"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoAuditoriaInterna : i += 1

            'Seta Parametros - Status
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "status"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iStatus

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_qualidade_auditoria_historico", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New QAAuditoriaHistorico


                oInfo.codigo_empresa = oSqlDataReader("codigo_empresa")
                oInfo.codigo_unidade = oSqlDataReader("codigo_unidade")
                oInfo.unidade = oSqlDataReader("unidade")
                oInfo.codigo_auditoria_interna = oSqlDataReader("codigo_auditoria_interna")
                oInfo.codigo_auditoria = oSqlDataReader("codigo_auditoria")
                oInfo.codigo_checklist = oSqlDataReader("codigo_checklist")
                oInfo.descricao = oSqlDataReader("descricao")
                oInfo.usuario = oSqlDataReader("usuario")
                oInfo.status = oSqlDataReader("status")
                oInfo.descricao_status = oSqlDataReader("descricao_status")
                oInfo.css_class = oSqlDataReader("css_class")
                oInfo.data = oSqlDataReader("data")
                oInfo.pontos_possiveis = oSqlDataReader("pontos_possiveis")
                oInfo.pontos_realizados = oSqlDataReader("pontos_realizados")
                oInfo.conforme = oSqlDataReader("conforme")
                oInfo.nao_conforme = oSqlDataReader("nao_conforme")
                oInfo.nao_respondido = oSqlDataReader("nao_respondido")
                oInfo.nao_aplicavel = oSqlDataReader("nao_aplicavel")
                oInfo.nota = oSqlDataReader("nota")

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

    Public Function AuditoriaCronograma(ByVal iCodigoEmpresa As Integer,
                                        ByVal iCodigoUnidade As Integer,
                                        ByVal iMes As Integer,
                                        ByVal iAno As Integer) As List(Of QAAuditoriaCronograma)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of QAAuditoriaCronograma)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(3) As SqlParameter
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

            'Seta Parametros - Mês
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "mes"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iMes : i += 1

            'Seta Parametros - Ano
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ano"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iAno

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_qualidade_auditoria_cronograma", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New QAAuditoriaCronograma

                oInfo.codigo_empresa = oSqlDataReader("codigo_empresa")
                oInfo.codigo_unidade = oSqlDataReader("codigo_unidade")
                oInfo.unidade = oSqlDataReader("unidade")
                oInfo.codigo_auditoria_interna = oSqlDataReader("codigo_auditoria_interna")
                oInfo.descricao = oSqlDataReader("descricao")
                oInfo.codigo_periodicidade = oSqlDataReader("codigo_periodicidade")
                oInfo.periodicidade = oSqlDataReader("periodicidade")
                oInfo.intervalo = oSqlDataReader("intervalo")
                oInfo.codigo_auditoria = oSqlDataReader("codigo_auditoria")
                oInfo.data_ultima_auditoria = oSqlDataReader("data_ultima_auditoria")
                oInfo.data_proxima_auditoria = oSqlDataReader("data_proxima_auditoria")
                oInfo.data_inicio = oSqlDataReader("data_inicio")
                oInfo.data_termino = oSqlDataReader("data_termino")
                oInfo.total = oSqlDataReader("total")
                oInfo.total_realizado = oSqlDataReader("total_realizado")
                oInfo.percentual = Math.Round(oSqlDataReader.Item("total_realizado") / oSqlDataReader.Item("total") * 100.0, 2)
                oInfo.dia_1 = oSqlDataReader.Item("1")
                oInfo.dia_2 = oSqlDataReader.Item("2")
                oInfo.dia_3 = oSqlDataReader.Item("3")
                oInfo.dia_4 = oSqlDataReader.Item("4")
                oInfo.dia_5 = oSqlDataReader.Item("5")
                oInfo.dia_6 = oSqlDataReader.Item("6")
                oInfo.dia_7 = oSqlDataReader.Item("7")
                oInfo.dia_8 = oSqlDataReader.Item("8")
                oInfo.dia_9 = oSqlDataReader.Item("9")
                oInfo.dia_10 = oSqlDataReader.Item("10")
                oInfo.dia_11 = oSqlDataReader.Item("11")
                oInfo.dia_12 = oSqlDataReader.Item("12")
                oInfo.dia_13 = oSqlDataReader.Item("13")
                oInfo.dia_14 = oSqlDataReader.Item("14")
                oInfo.dia_15 = oSqlDataReader.Item("15")
                oInfo.dia_16 = oSqlDataReader.Item("16")
                oInfo.dia_17 = oSqlDataReader.Item("17")
                oInfo.dia_18 = oSqlDataReader.Item("18")
                oInfo.dia_19 = oSqlDataReader.Item("19")
                oInfo.dia_20 = oSqlDataReader.Item("20")
                oInfo.dia_21 = oSqlDataReader.Item("21")
                oInfo.dia_22 = oSqlDataReader.Item("22")
                oInfo.dia_23 = oSqlDataReader.Item("23")
                oInfo.dia_24 = oSqlDataReader.Item("24")
                oInfo.dia_25 = oSqlDataReader.Item("25")
                oInfo.dia_26 = oSqlDataReader.Item("26")
                oInfo.dia_27 = oSqlDataReader.Item("27")
                oInfo.dia_28 = oSqlDataReader.Item("28")
                oInfo.dia_29 = oSqlDataReader.Item("29")
                oInfo.dia_30 = oSqlDataReader.Item("30")
                oInfo.dia_31 = oSqlDataReader.Item("31")

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

    Public Sub InsertApontamento(ByVal iCodigoEmpresa As Integer,
                                 ByVal iCodigoUnidade As Integer,
                                 ByVal iCodigoUsuario As Integer,
                                 ByVal iCodigoAuditoriaInterna As Integer,
                                 ByRef lCodigoAuditoria As Long)

        Try

            'Váriaveis Locais
            Dim oSqlParameter(4) As SqlParameter
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
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código Auditoria Interna
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_auditoria_interna"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoAuditoriaInterna : i += 1

            'Seta Parametros - Código Auditoria
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_auditoria"
            oSqlParameter(i).Direction = ParameterDirection.Output
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_qualidade_auditoria_apontamento", oSqlParameter)

            lCodigoAuditoria = oSqlParameter(i).Value

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateApontamento(ByVal iCodigoEmpresa As Integer,
                                 ByVal iCodigoUnidade As Integer,
                                 ByVal iCodigoUsuario As Integer,
                                 ByVal bConcluido As Boolean,
                                 ByVal iCodigoAuditoriaQualidade As Integer,
                                 ByVal lCodigoAuditoria As Long)

        Try

            'Váriaveis Locais
            Dim oSqlParameter(5) As SqlParameter
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
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Concluído
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "concluido"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bConcluido : i += 1

            'Seta Parametros - Código Auditoria Interna
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_auditoria_interna"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoAuditoriaQualidade : i += 1

            'Seta Parametros - Código Auditoria
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_auditoria"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoAuditoria

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_qualidade_auditoria_apontamento", oSqlParameter)

            lCodigoAuditoria = oSqlParameter(i).Value

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InsertApontamentoChecklist(ByVal iCodigoEmpresa As Integer,
                                          ByVal iCodigoUnidade As Integer,
                                          ByVal iCodigoUsuario As Integer,
                                          ByVal lCodigoAuditoria As Long,
                                          ByVal lCodigoChecklist As Long,
                                          ByVal iCodigoItemChecklist As Integer,
                                          ByVal sResultado As String,
                                          ByVal sPrazo As String,
                                          ByVal sObservacao As String)

        Try

            'Váriaveis Locais
            Dim oSqlParameter(8) As SqlParameter
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
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código Auditoria
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_auditoria"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoAuditoria : i += 1

            'Seta Parametros - Código Checklist
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_checklist"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoChecklist : i += 1

            'Seta Parametros - Código Item Checklist
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_item_checklist"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoItemChecklist : i += 1

            'Seta Parametros - Resultado
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "resultado"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = IIf(IsNothing(sResultado), DBNull.Value, sResultado) : i += 1

            'Seta Parametros - Prazo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "prazo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = IIf(IsNumeric(sPrazo), sPrazo, DBNull.Value) : i += 1

            'Seta Parametros - Observação
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "observacao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = IIf(IsNothing(sObservacao), DBNull.Value, sObservacao)

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_qualidade_auditoria_apontamento_checklist", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub LoadApontamento(ByVal iCodigoEmpresa As Integer,
                               ByVal iCodigoUnidade As Integer,
                               ByVal iCodigoAuditoriaInterna As Integer,
                               ByVal lCodigoAuditoria As Long,
                               ByRef oQAApontamento As QAApontamento)

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

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Auditoria Interna
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_auditoria_interna"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoAuditoriaInterna : i += 1

            'Seta Parametros - Código Auditoria
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_auditoria"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoAuditoria


            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_qualidade_auditoria_apontamento", oSqlParameter)

            While oSqlDataReader.Read

                oQAApontamento = New QAApontamento

                oQAApontamento.codigo_empresa = oSqlDataReader.Item("codigo_empresa")
                oQAApontamento.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oQAApontamento.unidade = oSqlDataReader.Item("unidade")
                oQAApontamento.codigo_auditoria_interna = oSqlDataReader.Item("codigo_auditoria_interna")
                oQAApontamento.codigo_auditoria = oSqlDataReader.Item("codigo_auditoria")
                oQAApontamento.codigo_checklist = oSqlDataReader.Item("codigo_checklist")
                oQAApontamento.descricao = oSqlDataReader.Item("descricao")
                oQAApontamento.numero_documento = oSqlDataReader.Item("numero_documento")
                oQAApontamento.usuario = oSqlDataReader.Item("usuario")
                oQAApontamento.data_inicio = oSqlDataReader.Item("data_inicio")
                oQAApontamento.data_termino = oSqlDataReader.Item("data_termino")
                oQAApontamento.nota = oSqlDataReader.Item("nota")
                oQAApontamento.status = oSqlDataReader.Item("status")
                oQAApontamento.checklist = LoadApontamentoChecklist(iCodigoEmpresa:=iCodigoEmpresa,
                                                                    iCodigoUnidade:=iCodigoUnidade,
                                                                    iCodigoAuditoriaInterna:=iCodigoAuditoriaInterna,
                                                                    lCodigoAuditoria:=lCodigoAuditoria)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function LoadApontamentoChecklist(ByVal iCodigoEmpresa As Integer,
                                             ByVal iCodigoUnidade As Integer,
                                             ByVal iCodigoAuditoriaInterna As Integer,
                                             ByVal lCodigoAuditoria As Long) As List(Of QAApontamentoChecklist)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of QAApontamentoChecklist)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(3) As SqlParameter
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

            'Seta Parametros - Código Auditoria Interna
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_auditoria_interna"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoAuditoriaInterna : i += 1

            'Seta Parametros - Código Auditoria
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_auditoria"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoAuditoria


            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_qualidade_auditoria_apontamento_checklist", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New QAApontamentoChecklist

                oInfo.checklist = oSqlDataReader.Item("checklist")
                oInfo.codigo_tipo_item_checklist = oSqlDataReader.Item("codigo_tipo_item_checklist")
                oInfo.codigo_checklist = oSqlDataReader.Item("codigo_checklist")
                oInfo.codigo_item_checklist = oSqlDataReader.Item("codigo_item_checklist")
                oInfo.grupo = oSqlDataReader.Item("grupo")
                oInfo.subgrupo = oSqlDataReader.Item("subgrupo")
                oInfo.descricao = oSqlDataReader.Item("descricao")
                oInfo.numero_digitos = oSqlDataReader.Item("numero_digitos")
                oInfo.allow_picture = oSqlDataReader.Item("allow_picture")
                oInfo.unidade_medida = oSqlDataReader.Item("unidade_medida")
                oInfo.resultado = oSqlDataReader.Item("resultado")
                oInfo.observacao = oSqlDataReader.Item("observacao")
                oInfo.responsavel = ""
                oInfo.prazo = oSqlDataReader.Item("prazo")
                oInfo.quantidade_ordem_servico = oSqlDataReader.Item("quantidade_ordem_servico")
                oInfo.possui_foto = oSqlDataReader.Item("possui_foto")

                oReturn.Add(oInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: PLANO DE AÇÃO :::"

    Public Sub LoadPlanoAcaoView(ByVal iCodigoEmpresa As Integer,
                                 ByVal iCodigoUnidade As Integer,
                                 ByVal lCodigoAuditoria As Long,
                                 ByVal iCodigoDepartamento As Integer,
                                 ByRef oQAApontamento As QAApontamento)

        Try

            'Váriaveis Locais
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

            'Seta Parametros - Código Auditoria
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_auditoria"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoAuditoria


            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_qualidade_plano_acao_view", oSqlParameter)

            While oSqlDataReader.Read

                oQAApontamento = New QAApontamento

                oQAApontamento.codigo_empresa = oSqlDataReader.Item("codigo_empresa")
                oQAApontamento.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oQAApontamento.unidade = oSqlDataReader.Item("unidade")
                oQAApontamento.codigo_auditoria_interna = oSqlDataReader.Item("codigo_auditoria_interna")
                oQAApontamento.codigo_auditoria = oSqlDataReader.Item("codigo_auditoria")
                oQAApontamento.codigo_checklist = oSqlDataReader.Item("codigo_checklist")
                oQAApontamento.codigo_departamento = iCodigoDepartamento
                oQAApontamento.descricao = oSqlDataReader.Item("descricao")
                oQAApontamento.status = oSqlDataReader.Item("status")
                oQAApontamento.checklist = LoadPlanoAcaoViewChecklist(iCodigoEmpresa:=iCodigoEmpresa,
                                                                      iCodigoUnidade:=iCodigoUnidade,
                                                                      lCodigoAuditoria:=lCodigoAuditoria,
                                                                      iCodigoDepartamento:=iCodigoDepartamento)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function LoadPlanoAcaoViewChecklist(ByVal iCodigoEmpresa As Integer,
                                               ByVal iCodigoUnidade As Integer,
                                               ByVal lCodigoAuditoria As Long,
                                               ByVal iCodigoDepartamento As Integer) As List(Of QAApontamentoChecklist)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of QAApontamentoChecklist)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(3) As SqlParameter
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

            'Seta Parametros - Código Departamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_departamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoDepartamento : i += 1

            'Seta Parametros - Código Auditoria
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_auditoria"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoAuditoria


            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_qualidade_plano_acao_view_checklist", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New QAApontamentoChecklist

                oInfo.checklist = oSqlDataReader.Item("checklist")
                oInfo.codigo_tipo_item_checklist = oSqlDataReader.Item("codigo_tipo_item_checklist")
                oInfo.codigo_checklist = oSqlDataReader.Item("codigo_checklist")
                oInfo.codigo_item_checklist = oSqlDataReader.Item("codigo_item_checklist")
                oInfo.grupo = oSqlDataReader.Item("grupo")
                oInfo.subgrupo = oSqlDataReader.Item("subgrupo")
                oInfo.descricao = oSqlDataReader.Item("descricao")
                oInfo.allow_picture = oSqlDataReader.Item("allow_picture")
                oInfo.unidade_medida = oSqlDataReader.Item("unidade_medida")
                oInfo.resultado = oSqlDataReader.Item("resultado")
                oInfo.observacao = oSqlDataReader.Item("observacao")
                oInfo.responsavel = oSqlDataReader.Item("responsavel")
                oInfo.prazo = oSqlDataReader.Item("prazo")

                oReturn.Add(oInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Sub SendEmailPlanoAcao(ByVal iCodigoEmpresa As Integer,
                                  ByVal iCodigoUsuario As Integer,
                                  ByVal iCodigoUnidade As Integer,
                                  ByVal lCodigoAuditoria As Long,
                                  ByVal sTipo As String)

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

            'Seta Parametros - Código Auditoria
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_auditoria"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoAuditoria : i += 1

            'Seta Parametros - Tipo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "tipo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sTipo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pcm_ordem_servico_plano_acao_email", oSqlParameter)

            While oSqlDataReader.Read

                Dim oMailMessage As New MailMessage()

                For Each sEmail As String In oSqlDataReader.Item("to").ToString.Split(";")
                    oMailMessage.To.Add(sEmail)
                Next

                oMailMessage.From = New MailAddress("no-reply@pcmbysim.com.br", "PCM by SIM", System.Text.Encoding.UTF8)
                oMailMessage.Subject = oSqlDataReader.Item("subject")
                oMailMessage.SubjectEncoding = System.Text.Encoding.UTF8
                oMailMessage.Body = oSqlDataReader.Item("body")
                oMailMessage.BodyEncoding = System.Text.Encoding.UTF8
                oMailMessage.IsBodyHtml = True
                oMailMessage.Priority = MailPriority.Normal
                Dim oSmtpClient As New SmtpClient()
                oSmtpClient.Credentials = New System.Net.NetworkCredential("no-reply@pcmbysim.com.br", "$Noreply@2026$")
                oSmtpClient.Port = 465
                oSmtpClient.Host = "smtpout.secureserver.net"
                oSmtpClient.EnableSsl = True

                Try
                    oSmtpClient.Send(oMailMessage)
                Catch ex As Exception

                End Try

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function IndexOrdemServico(ByVal iCodigoEmpresa As Integer,
                                      ByVal iCodigoUsuario As Integer,
                                      Optional ByVal iCodigoUnidade As Integer = 0,
                                      Optional ByVal sDataInicio As String = "",
                                      Optional ByVal sDataTermino As String = "",
                                      Optional ByVal sOrdemServico As String = "",
                                      Optional ByVal iCodigoAuditoria As Integer = 0,
                                      Optional ByVal iCodigoPrioridade As Integer = 0,
                                      Optional ByVal iCodigoDepartamento As Integer = 0,
                                      Optional ByVal iCodigoSolicitante As Integer = 0,
                                      Optional ByVal sExecutor As String = "",
                                      Optional ByVal iStatus As Integer = 0,
                                      Optional ByVal lCodigoAuditoria As Long = -1,
                                      Optional ByVal iCodigoJustificativaApontamento As Integer = -1) As List(Of MODELS.OrdemServico)

        Try

            'Váriaveis Locais
            Dim oOrdemServico As New List(Of MODELS.OrdemServico)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(14) As SqlParameter
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

            'Seta Parametros - Ordem Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ordem_servico"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sOrdemServico : i += 1

            'Seta Parametros - Auditoria Interna
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_auditoria_interna"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoAuditoria : i += 1

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

            'Seta Parametros - Código Prioridade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_prioridade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoPrioridade : i += 1

            'Seta Parametros - Código Departamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_departamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = iCodigoDepartamento : i += 1

            'Seta Parametros - Código Solicitante
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_solicitante"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoSolicitante : i += 1

            'Seta Parametros - Código Justificativa Apontamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_justificativa_apontamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoJustificativaApontamento : i += 1

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

            'Seta Parametros - Código Auditoria
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_auditoria"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = lCodigoAuditoria

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pcm_ordem_servico_plano_acao", oSqlParameter)

            While oSqlDataReader.Read

                Dim oOrdemServicoInfo As New MODELS.OrdemServico

                oOrdemServicoInfo.codigo = oSqlDataReader("codigo")
                oOrdemServicoInfo.codigo_unidade = oSqlDataReader("codigo_unidade")
                oOrdemServicoInfo.unidade = oSqlDataReader("unidade")
                oOrdemServicoInfo.categoria = oSqlDataReader("categoria")
                oOrdemServicoInfo.codigo_setor = oSqlDataReader("codigo_setor")
                oOrdemServicoInfo.setor = oSqlDataReader("setor")
                oOrdemServicoInfo.codigo_equipamento = oSqlDataReader("codigo_equipamento")
                oOrdemServicoInfo.equipamento = oSqlDataReader("equipamento")
                oOrdemServicoInfo.departamento = oSqlDataReader("departamento")
                oOrdemServicoInfo.local = oSqlDataReader("local")
                oOrdemServicoInfo.solicitante = oSqlDataReader("solicitante")
                oOrdemServicoInfo.executor = oSqlDataReader("executor")
                oOrdemServicoInfo.data_execucao = oSqlDataReader("data_execucao")
                oOrdemServicoInfo.numero_documento = oSqlDataReader("numero_documento")
                oOrdemServicoInfo.ordem_servico_cliente = oSqlDataReader("ordem_servico_cliente")
                oOrdemServicoInfo.data = Format(oSqlDataReader("data"), "dd/MM/yyyy HH:mm")
                oOrdemServicoInfo.data_necessidade = Format(oSqlDataReader("data_necessidade"), "dd/MM/yyyy")
                oOrdemServicoInfo.dias = Format(oSqlDataReader("dias"))
                oOrdemServicoInfo.descricao = oSqlDataReader("descricao")
                oOrdemServicoInfo.codigo_prioridade = oSqlDataReader("codigo_prioridade")
                oOrdemServicoInfo.prioridade = oSqlDataReader("prioridade")
                oOrdemServicoInfo.codigo_tipo_servico = oSqlDataReader("codigo_tipo_servico")
                oOrdemServicoInfo.tipo_servico = oSqlDataReader("tipo_servico")
                oOrdemServicoInfo.codigo_tipo_ordem_servico = oSqlDataReader("codigo_tipo_ordem_servico")
                oOrdemServicoInfo.tipo_ordem_servico = oSqlDataReader("tipo_ordem_servico")
                oOrdemServicoInfo.status = oSqlDataReader("status")
                oOrdemServicoInfo.status_descricao = oSqlDataReader("status_descricao")
                oOrdemServicoInfo.justificativa_apontamento = oSqlDataReader("justificativa_apontamento")
                oOrdemServicoInfo.auditoria = oSqlDataReader("auditoria")

                oOrdemServico.Add(oOrdemServicoInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oOrdemServico

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: TAREFA :::"

    Public Function LoadQualidadeTarefaStatus(ByVal iCodigoEmpresa As Integer,
                                              ByVal iCodigoUnidade As Integer,
                                              ByVal iCodigoModulo As Integer) As QATarefaStatus

        Try

            'Váriaveis Locais
            Dim oReturn As New QATarefaStatus
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

            'Seta Parametros - Código Módulo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_modulo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoModulo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_qa_tarefa_status", oSqlParameter)

            While oSqlDataReader.Read

                oReturn.atrasado = oSqlDataReader.Item("atrasado")
                oReturn.pendente = oSqlDataReader.Item("pendente")
                oReturn.concluido = oSqlDataReader.Item("concluido")
                oReturn.em_andamento = oSqlDataReader.Item("em_andamento")

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

    Public Function LoadQualidadeTarefa(ByVal iCodigoEmpresa As Integer,
                                        ByVal iCodigoUnidade As Integer,
                                        ByVal iCodigoModulo As Integer,
                                        Optional ByVal iStatus As Integer = -1) As List(Of QATarefa)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of QATarefa)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(3) As SqlParameter
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

            'Seta Parametros - Código Módulo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_modulo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoModulo : i += 1

            'Seta Parametros - Status
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "status"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iStatus

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_qa_tarefa_lista", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New QATarefa

                oInfo.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oInfo.unidade = oSqlDataReader.Item("nome_fantasia")
                oInfo.tarefa = LoadQualidadeTarefaDia(iCodigoEmpresa:=iCodigoEmpresa,
                                                      iCodigoUnidade:=oSqlDataReader.Item("codigo_unidade"),
                                                      iCodigoModulo:=oSqlDataReader.Item("codigo_modulo"),
                                                      iStatus:=iStatus)

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

    Public Function LoadQualidadeTarefaDia(ByVal iCodigoEmpresa As Integer,
                                           ByVal iCodigoUnidade As Integer,
                                           ByVal iCodigoModulo As Integer,
                                           ByVal iStatus As Integer) As List(Of QATarefaDia)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of QATarefaDia)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(3) As SqlParameter
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

            'Seta Parametros - Código Módulo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_modulo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoModulo : i += 1

            'Seta Parametros - Status
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "status"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iStatus

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_qa_tarefa_lista_dia", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New QATarefaDia

                oInfo.codigo_qa_tarefa = oSqlDataReader.Item("codigo_qa_tarefa")
                oInfo.codigo_qa_tarefa_ordem_servico = oSqlDataReader.Item("codigo_qa_tarefa_ordem_servico")
                oInfo.descricao = oSqlDataReader.Item("descricao")
                oInfo.status = oSqlDataReader.Item("status")
                oInfo.descricao_status = oSqlDataReader.Item("descricao_status")
                oInfo.cor = oSqlDataReader.Item("cor")
                oInfo.data = oSqlDataReader.Item("data")

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

    Public Function LoadQualidadeTarefaMes(ByVal iCodigoEmpresa As Integer,
                                           ByVal iCodigoUnidade As Integer,
                                           ByVal iCodigoModulo As Integer,
                                           Optional ByVal iStatus As Integer = -1) As List(Of QATarefa)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of QATarefa)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(3) As SqlParameter
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

            'Seta Parametros - Código Módulo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_modulo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoModulo : i += 1

            'Seta Parametros - Status
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "status"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iStatus

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_qualidade_tarefa_mes", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New QATarefa

                oInfo.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oInfo.unidade = oSqlDataReader.Item("nome_fantasia")
                oInfo.tarefa = LoadQualidadeTarefaMesDia(iCodigoEmpresa:=iCodigoEmpresa,
                                                         iCodigoUnidade:=oSqlDataReader.Item("codigo_unidade"),
                                                         iCodigoModulo:=oSqlDataReader.Item("codigo_modulo"),
                                                         iStatus:=iStatus)

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

    Public Function LoadQualidadeTarefaMesDia(ByVal iCodigoEmpresa As Integer,
                                              ByVal iCodigoUnidade As Integer,
                                              ByVal iCodigoModulo As Integer,
                                              ByVal iStatus As Integer) As List(Of QATarefaDia)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of QATarefaDia)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(3) As SqlParameter
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

            'Seta Parametros - Código Módulo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_modulo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoModulo : i += 1

            'Seta Parametros - Status
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "status"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iStatus

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_qualidade_tarefa_mes_dia", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New QATarefaDia

                oInfo.codigo_qa_tarefa = oSqlDataReader.Item("codigo_qa_tarefa")
                oInfo.codigo_qa_tarefa_ordem_servico = oSqlDataReader.Item("codigo_qa_tarefa_ordem_servico")
                oInfo.descricao = oSqlDataReader.Item("descricao")
                oInfo.status = oSqlDataReader.Item("status")
                oInfo.descricao_status = oSqlDataReader.Item("descricao_status")
                oInfo.cor = oSqlDataReader.Item("cor")
                oInfo.data = oSqlDataReader.Item("data")

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

    Public Function QualidadeTarefa(ByVal iCodigoEmpresa As Integer,
                                    ByVal iCodigoUnidade As Integer,
                                    ByVal iCodigoModulo As Integer,
                                    ByVal iMes As Integer,
                                    ByVal iAno As Integer) As List(Of QualidadeTarefa)

        Try



            'Váriaveis Locais
            Dim oReturn As New List(Of QualidadeTarefa)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(4) As SqlParameter
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

            'Seta Parametros - Código Módulo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_modulo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoModulo : i += 1

            'Seta Parametros - Mês
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "mes"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iMes : i += 1

            'Seta Parametros - Ano
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ano"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iAno

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_qualidade_tarefa", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New QualidadeTarefa

                oInfo.codigo_empresa = oSqlDataReader("codigo_empresa")
                oInfo.codigo_unidade = oSqlDataReader("codigo_unidade")
                oInfo.unidade = oSqlDataReader("unidade")
                oInfo.codigo_qa_tarefa = oSqlDataReader("codigo_qa_tarefa")
                oInfo.descricao = oSqlDataReader("descricao")
                oInfo.codigo_periodicidade = oSqlDataReader("codigo_periodicidade")
                oInfo.periodicidade = oSqlDataReader("periodicidade")
                oInfo.intervalo = oSqlDataReader("intervalo")
                oInfo.data_ultima_os = oSqlDataReader("data_ultima_os")
                oInfo.data_proxima_os = oSqlDataReader("data_proxima_os")
                oInfo.data_inicio = oSqlDataReader("data_inicio")
                oInfo.data_termino = oSqlDataReader("data_termino")
                oInfo.total = oSqlDataReader("total")
                oInfo.total_realizado = oSqlDataReader("total_realizado")
                oInfo.percentual = IIf(oSqlDataReader.Item("total_realizado") = 0, 0, Math.Round(oSqlDataReader.Item("total_realizado") / oSqlDataReader.Item("total") * 100.0, 2))
                oInfo.dia_1 = oSqlDataReader.Item("1")
                oInfo.dia_2 = oSqlDataReader.Item("2")
                oInfo.dia_3 = oSqlDataReader.Item("3")
                oInfo.dia_4 = oSqlDataReader.Item("4")
                oInfo.dia_5 = oSqlDataReader.Item("5")
                oInfo.dia_6 = oSqlDataReader.Item("6")
                oInfo.dia_7 = oSqlDataReader.Item("7")
                oInfo.dia_8 = oSqlDataReader.Item("8")
                oInfo.dia_9 = oSqlDataReader.Item("9")
                oInfo.dia_10 = oSqlDataReader.Item("10")
                oInfo.dia_11 = oSqlDataReader.Item("11")
                oInfo.dia_12 = oSqlDataReader.Item("12")
                oInfo.dia_13 = oSqlDataReader.Item("13")
                oInfo.dia_14 = oSqlDataReader.Item("14")
                oInfo.dia_15 = oSqlDataReader.Item("15")
                oInfo.dia_16 = oSqlDataReader.Item("16")
                oInfo.dia_17 = oSqlDataReader.Item("17")
                oInfo.dia_18 = oSqlDataReader.Item("18")
                oInfo.dia_19 = oSqlDataReader.Item("19")
                oInfo.dia_20 = oSqlDataReader.Item("20")
                oInfo.dia_21 = oSqlDataReader.Item("21")
                oInfo.dia_22 = oSqlDataReader.Item("22")
                oInfo.dia_23 = oSqlDataReader.Item("23")
                oInfo.dia_24 = oSqlDataReader.Item("24")
                oInfo.dia_25 = oSqlDataReader.Item("25")
                oInfo.dia_26 = oSqlDataReader.Item("26")
                oInfo.dia_27 = oSqlDataReader.Item("27")
                oInfo.dia_28 = oSqlDataReader.Item("28")
                oInfo.dia_29 = oSqlDataReader.Item("29")
                oInfo.dia_30 = oSqlDataReader.Item("30")
                oInfo.dia_31 = oSqlDataReader.Item("31")

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

#Region "::: TAREFA - ORDEM DE SERVIÇO :::"

    Public Function TarefaOrdemServico(ByVal iCodigoEmpresa As Integer,
                                       ByVal iCodigoUnidade As Integer,
                                       ByVal lCodigoQATarefaOrdemServico As Long) As QATarefaOrdemServico

        Try

            'Váriaveis Locais
            Dim oReturn As New QATarefaOrdemServico
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

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoQATarefaOrdemServico : i += 1

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_qa_tarefa_ordem_servico", oSqlParameter)

            While oSqlDataReader.Read

                oReturn.codigo = oSqlDataReader.Item("codigo")
                oReturn.codigo_qa_tarefa = oSqlDataReader.Item("codigo_qa_tarefa")
                oReturn.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oReturn.unidade = oSqlDataReader.Item("unidade")
                oReturn.servico = oSqlDataReader.Item("servico")
                oReturn.data = oSqlDataReader.Item("data")
                oReturn.descricao_solucao = oSqlDataReader.Item("descricao_solucao")
                oReturn.status = oSqlDataReader.Item("status")
                oReturn.apontamento = TarefaOrdemServicoApontamento(iCodigoEmpresa:=iCodigoEmpresa,
                                                                    iCodigoUnidade:=iCodigoUnidade,
                                                                    lCodigoQATarefaOrdemServico:=lCodigoQATarefaOrdemServico)
                oReturn.checklist = TarefaOrdemServicoChecklist(iCodigoEmpresa:=iCodigoEmpresa,
                                                                iCodigoUnidade:=iCodigoUnidade,
                                                                lCodigoQATarefaOrdemServico:=lCodigoQATarefaOrdemServico)

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

    Public Function TarefaOrdemServicoApontamento(ByVal iCodigoEmpresa As Integer,
                                                  ByVal iCodigoUnidade As Integer,
                                                  ByVal lCodigoQATarefaOrdemServico As Long) As List(Of QATarefaApontamento)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of QATarefaApontamento)
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

            'Seta Parametros - Código QA Tarefa Ordem Servico
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_qa_tarefa_ordem_servico"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = lCodigoQATarefaOrdemServico

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_qa_tarefa_ordem_servico_apontamento", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New QATarefaApontamento

                oInfo.codigo = oSqlDataReader.Item("codigo")
                oInfo.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oInfo.codigo_funcionario = oSqlDataReader.Item("codigo_funcionario")
                oInfo.funcionario = oSqlDataReader.Item("funcionario")
                oInfo.descricao_solucao = oSqlDataReader.Item("descricao_solucao")
                oInfo.data_inicio = oSqlDataReader.Item("data_inicio")
                oInfo.data_termino = oSqlDataReader.Item("data_termino")
                oInfo.hora_inicio = oSqlDataReader.Item("hora_inicio")
                oInfo.hora_termino = oSqlDataReader.Item("hora_termino")

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

    Public Function TarefaOrdemServicoChecklist(ByVal iCodigoEmpresa As Integer,
                                                ByVal iCodigoUnidade As Integer,
                                                ByVal lCodigoQATarefaOrdemServico As Long) As List(Of QATarefaApontamentoChecklist)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of QATarefaApontamentoChecklist)
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

            'Seta Parametros - Código QA Tarefa Ordem Servico
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_qa_tarefa_ordem_servico"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoQATarefaOrdemServico

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_qa_tarefa_ordem_servico_checklist", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New QATarefaApontamentoChecklist

                oInfo.codigo_qa_tarefa_ordem_servico = oSqlDataReader.Item("codigo_qa_tarefa_ordem_servico")
                oInfo.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oInfo.codigo = oSqlDataReader.Item("codigo")
                oInfo.codigo_tipo_item_checklist = oSqlDataReader.Item("codigo_tipo_item_checklist")
                oInfo.tipo_checklist = oSqlDataReader.Item("tipo_item_checklist")
                oInfo.grupo = oSqlDataReader.Item("grupo")
                oInfo.subgrupo = oSqlDataReader.Item("subgrupo")
                oInfo.checklist = oSqlDataReader.Item("checklist")
                oInfo.descricao = oSqlDataReader.Item("descricao")
                oInfo.valor_minimo = oSqlDataReader.Item("valor_minimo")
                oInfo.valor_maximo = oSqlDataReader.Item("valor_maximo")
                oInfo.resultado = oSqlDataReader.Item("resultado")
                oInfo.unidade_medida = oSqlDataReader.Item("unidade_medida")
                oInfo.observacao = oSqlDataReader.Item("observacao")
                oInfo.allow_picture = oSqlDataReader.Item("allow_picture")

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

    Public Sub TarefaOrdemServicoDelete(ByVal iCodigoEmpresa As Integer,
                                        ByVal iCodigoUsuario As Integer,
                                        ByVal iCodigoUnidade As Integer,
                                        ByVal lCodigoQATarefaOrdemServico As Long)

        Try

            'Váriaveis Locais
            Dim oSqlParameter(3) As SqlParameter
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

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoQATarefaOrdemServico

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_qa_tarefa_ordem_servico", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region

#Region "::: TAREFA - HISTÓRICO :::"

    Public Function TarefaHistorico(ByVal iCodigoEmpresa As Integer,
                                    ByVal iCodigoUnidade As Integer,
                                    ByVal sDataInicio As String,
                                    ByVal sDataTermino As String,
                                    ByVal lCodigoQATarefa As Long) As List(Of QATarefaOrdemServicoHistorico)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of QATarefaOrdemServicoHistorico)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(4) As SqlParameter
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

            'Seta Parametros - Data Início
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_inicio"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = IIf(IsDate(sDataInicio), sDataInicio, DBNull.Value) : i += 1

            'Seta Parametros - Data Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_termino"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = IIf(IsDate(sDataTermino), sDataTermino, DBNull.Value) : i += 1

            'Seta Parametros - Código Tarefa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_qa_tarefa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoQATarefa

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_qa_tarefa_ordem_servico_historico", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New QATarefaOrdemServicoHistorico

                oInfo.codigo = oSqlDataReader.Item("codigo")
                oInfo.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oInfo.codigo_qa_tarefa = oSqlDataReader.Item("codigo_qa_tarefa")
                oInfo.unidade = oSqlDataReader.Item("unidade")
                oInfo.tarefa = oSqlDataReader.Item("tarefa")
                oInfo.data_execucao = oSqlDataReader.Item("data_execucao")

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

#Region "::: TAREFA - APONTAMENTO :::"

    Public Sub LoadDadosTarefa(ByVal iCodigoEmpresa As Integer,
                               ByVal iCodigoUnidade As Integer,
                               ByVal lCodigoQATarefa As Long,
                               ByVal lCodigoQATarefaOrdemServico As Long,
                               ByRef oTarefaDados As QATarefaDados)

        Try

            'Váriaveis Locais
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(3) As SqlParameter
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

            'Seta Parametros - Código QA Tarefa Ordem Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_qa_tarefa_ordem_servico"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoQATarefaOrdemServico : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoQATarefa

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_qa_tarefa_apontamento_dados", oSqlParameter)

            While oSqlDataReader.Read

                oTarefaDados = New QATarefaDados

                oTarefaDados.codigo = oSqlDataReader.Item("codigo")
                oTarefaDados.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oTarefaDados.unidade = oSqlDataReader.Item("unidade")
                oTarefaDados.descricao = oSqlDataReader.Item("descricao")
                oTarefaDados.descricao_solucao = oSqlDataReader.Item("descricao_solucao")
                oTarefaDados.data_inicio = oSqlDataReader.Item("data_inicio")
                oTarefaDados.data_termino = oSqlDataReader.Item("data_termino")

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function LoadQualidadeTarefaApontamentoChecklist(ByVal iCodigoEmpresa As Integer,
                                                            ByVal lCodigoQATarefa As Long,
                                                            ByVal lCodigoQATarefaOrdemServico As Long,
                                                            ByVal iCodigoUnidade As Integer) As List(Of QATarefaApontamentoChecklist)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of QATarefaApontamentoChecklist)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(3) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código QA Tarefa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_qa_tarefa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoQATarefa : i += 1

            'Seta Parametros - Código QA Tarefa - Ordem de Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_qa_tarefa_ordem_servico"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoQATarefaOrdemServico : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_qa_tarefa_apontamento_checklist", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New QATarefaApontamentoChecklist

                oInfo.codigo = oSqlDataReader.Item("codigo")
                oInfo.descricao = oSqlDataReader.Item("descricao")
                oInfo.codigo_tipo_item_checklist = oSqlDataReader.Item("codigo_tipo_item_checklist")
                oInfo.grupo = oSqlDataReader.Item("grupo")
                oInfo.subgrupo = oSqlDataReader.Item("subgrupo")
                oInfo.checklist = oSqlDataReader.Item("checklist")
                oInfo.resultado = oSqlDataReader.Item("resultado")
                oInfo.valor_minimo = oSqlDataReader.Item("valor_minimo")
                oInfo.valor_maximo = oSqlDataReader.Item("valor_maximo")
                oInfo.observacao = oSqlDataReader.Item("observacao")
                oInfo.unidade_medida = oSqlDataReader.Item("unidade_medida")
                oInfo.allow_picture = oSqlDataReader.Item("allow_picture")

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

    Public Sub InsertOrdemServicoTarefa(ByVal iCodigoEmpresa As Integer,
                                        ByVal iCodigoUsuario As Integer,
                                        ByVal iCodigoUnidade As Integer,
                                        ByVal lCodigoQATarefa As Long,
                                        ByVal sSolucao As String,
                                        ByVal bConcluido As Boolean,
                                        ByVal sData As String,
                                        ByVal sDataTermino As String,
                                        ByRef lCodigoQATarefaOrdemServico As Long)

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

            'Seta Parametros - Código QA Tarefa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_qa_tarefa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoQATarefa : i += 1

            'Seta Parametros - Solução
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "solucao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 5000
            oSqlParameter(i).Value = sSolucao : i += 1

            'Seta Parametros - Arquivo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "arquivo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = DBNull.Value : i += 1

            'Seta Parametros - Path Arquivo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "path_arquivo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = DBNull.Value : i += 1

            'Seta Parametros - Concluído
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "concluido"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bConcluido : i += 1

            'Seta Parametros - Data
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sData : i += 1

            'Seta Parametros - Data Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_termino"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataTermino : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Output
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_qa_tarefa_ordem_servico", oSqlParameter)

            lCodigoQATarefaOrdemServico = oSqlParameter(i).Value

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InsertApontamentoTarefa(ByVal iCodigoEmpresa As Integer,
                                       ByVal iCodigoUsuario As Integer,
                                       ByVal iCodigoUnidade As Integer,
                                       ByVal lCodigoQATarefaOrdemServico As Long,
                                       ByVal iCodigoFuncionario As Integer,
                                       ByVal sDescricaoSolucao As String,
                                       ByVal iCodigoJustificativaApontamento As Integer,
                                       ByVal sDataInicio As String,
                                       ByVal sDataTermino As String,
                                       ByVal sHoraInicio As String,
                                       ByVal sHoraTermino As String)

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

            'Seta Parametros - Código QA Tarefa - Ordem de Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_qa_tarefa_ordem_servico"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoQATarefaOrdemServico : i += 1

            'Seta Parametros - Código Funcionário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_funcionario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoFuncionario = -1, DBNull.Value, iCodigoFuncionario) : i += 1

            'Seta Parametros - Descrição da Solução
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao_solucao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = sDescricaoSolucao : i += 1

            'Seta Parametros - Código Justificativa Apontamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_justificativa_apontamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoJustificativaApontamento = -1, DBNull.Value, iCodigoJustificativaApontamento) : i += 1

            'Seta Parametros - Data Início
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_inicio"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataInicio : i += 1

            'Seta Parametros - Data Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_termino"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataTermino : i += 1

            'Seta Parametros - Hora Início
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "hora_inicio"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Time
            oSqlParameter(i).Value = IIf(IsDate(sHoraInicio), sHoraInicio, DBNull.Value) : i += 1

            'Seta Parametros - Hora Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "hora_termino"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Time
            oSqlParameter(i).Value = IIf(IsDate(sHoraTermino), sHoraTermino, DBNull.Value)

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_qa_tarefa_ordem_servico_apontamento", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InsertChecklistTarefa(ByVal iCodigoEmpresa As Integer,
                                     ByVal iCodigoUsuario As Integer,
                                     ByVal iCodigoUnidade As Integer,
                                     ByVal lCodigoQATarefaOrdemServico As Long,
                                     ByVal iCodigoChecklistItem As Integer,
                                     ByVal sResultado As String,
                                     ByVal sObservacao As String)

        Try

            'Váriaveis Locais
            Dim oSqlParameter(6) As SqlParameter
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

            'Seta Parametros - Código QA Tarefa - Ordem de Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_qa_tarefa_ordem_servico"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoQATarefaOrdemServico : i += 1

            'Seta Parametros - Código Checklist Item
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_checklist_item"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoChecklistItem : i += 1

            'Seta Parametros - Resultado
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "resultado"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 5000
            oSqlParameter(i).Value = sResultado : i += 1

            'Seta Parametros - Observação
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "observacao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = sObservacao

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_qa_tarefa_ordem_servico_checklist", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateOrdemServicoTarefa(ByVal iCodigoEmpresa As Integer,
                                        ByVal iCodigoUsuario As Integer,
                                        ByVal iCodigoUnidade As Integer,
                                        ByVal lCodigoQATarefa As Long,
                                        ByVal sDescricaoSolucao As String,
                                        ByVal bConcluido As Boolean,
                                        ByVal sData As String,
                                        ByVal sDataTermino As String,
                                        ByVal lCodigoQATarefaOrdemServico As Long)

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

            'Seta Parametros - Código QA Tarefa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_qa_tarefa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoQATarefa : i += 1

            'Seta Parametros - Descrição Solução
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao_solucao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 5000
            oSqlParameter(i).Value = sDescricaoSolucao : i += 1

            'Seta Parametros - Arquivo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "arquivo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = DBNull.Value : i += 1

            'Seta Parametros - Path Arquivo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "path_arquivo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = DBNull.Value : i += 1

            'Seta Parametros - Data
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sData : i += 1

            'Seta Parametros - Data Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_termino"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataTermino : i += 1

            'Seta Parametros - Concluído
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "concluido"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bConcluido : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoQATarefaOrdemServico

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_qa_tarefa_ordem_servico", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    'Public Sub LoadApontamentoProgramadaInfo(ByVal iCodigoEmpresa As Integer,
    '                                         ByVal iCodigoUnidade As Integer,
    '                                         ByVal lCodigoPCMApontamento As Long,
    '                                         ByRef oApontamento As Apontamento)

    '    Try

    '        'Váriaveis Locais
    '        Dim oSqlDataReader As SqlDataReader
    '        Dim oSqlParameter(2) As SqlParameter
    '        Dim i As Integer = 0

    '        'Seta Parametros - Código Empresa
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "codigo_empresa"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
    '        oSqlParameter(i).Value = iCodigoEmpresa : i += 1

    '        'Seta Parametros - Código
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "codigo"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.BigInt
    '        oSqlParameter(i).Value = lCodigoPCMApontamento : i += 1

    '        'Seta Parametros - Código Unidade
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "codigo_unidade"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.Int
    '        oSqlParameter(i).Value = iCodigoUnidade

    '        'Executa Query
    '        oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pcm_apontamento_programada_dados", oSqlParameter)

    '        While oSqlDataReader.Read

    '            oApontamento = New Apontamento

    '            oApontamento.unidade = oSqlDataReader.Item("unidade")
    '            oApontamento.setor = oSqlDataReader.Item("setor")
    '            oApontamento.equipamento = oSqlDataReader.Item("equipamento")
    '            oApontamento.servico = oSqlDataReader.Item("servico")
    '            oApontamento.categoria = oSqlDataReader.Item("categoria")
    '            oApontamento.tipo_servico = oSqlDataReader.Item("tipo_servico")
    '            oApontamento.tipo_ordem_servico = oSqlDataReader.Item("tipo_ordem_servico")
    '            oApontamento.descricao_solucao = oSqlDataReader.Item("descricao_solucao")
    '            oApontamento.codigo_funcionario = oSqlDataReader.Item("codigo_funcionario")
    '            oApontamento.codigo_fornecedor = oSqlDataReader.Item("codigo_fornecedor")
    '            oApontamento.codigo_tipo_servico = oSqlDataReader.Item("codigo_tipo_servico")
    '            oApontamento.codigo_apontamento = oSqlDataReader.Item("codigo_apontamento")
    '            oApontamento.codigo_pcm_programada_ordem_servico = oSqlDataReader.Item("codigo_pcm_programada_ordem_servico")
    '            oApontamento.codigo_pcm_programada = oSqlDataReader.Item("codigo_pcm_programada")
    '            oApontamento.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
    '            oApontamento.aponta_horas = oSqlDataReader.Item("aponta_horas")
    '            oApontamento.data_inicio = oSqlDataReader.Item("data_inicio")
    '            oApontamento.data_termino = oSqlDataReader.Item("data_termino")
    '            oApontamento.hora_inicio = oSqlDataReader.Item("hora_inicio")
    '            oApontamento.hora_termino = oSqlDataReader.Item("hora_termino")

    '        End While

    '        'Fecha o oSqlDataReader
    '        If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

    '    Catch SqlEx As SqlException
    '        Throw SqlEx
    '    Catch ex As Exception
    '        Throw ex
    '    End Try

    'End Sub

    'Public Sub InsertApontamento(ByVal iCodigoEmpresa As Integer,
    '                             ByVal iCodigoUsuario As Integer,
    '                             ByVal iCodigoUnidade As Integer,
    '                             ByVal lCodigoPCMProgramadaOrdemServico As Long,
    '                             ByVal lCodigoPCMProgramada As Long,
    '                             ByVal iCodigoFornecedor As Integer,
    '                             ByVal iCodigoFuncionario As Integer,
    '                             ByVal sDataInicio As String,
    '                             ByVal sDataTermino As String,
    '                             ByVal sHoraInicio As String,
    '                             ByVal sHoraTermino As String,
    '                             ByVal dValor As Double,
    '                             ByVal iQuantidadeEquipamento As Integer,
    '                             ByVal sDescricaoSolucao As String,
    '                             ByVal sArquivo As String,
    '                             ByVal bConcluido As Boolean,
    '                             ByRef lCodigoPCMOrdemServico As Long,
    '                             ByRef lCodigoPCMApontamento As Long)

    '    Try

    '        'Váriaveis Locais
    '        Dim oSqlParameter(17) As SqlParameter
    '        Dim i As Integer = 0

    '        'Seta Parametros - Código Empresa
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "codigo_empresa"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
    '        oSqlParameter(i).Value = iCodigoEmpresa : i += 1

    '        'Seta Parametros - Código Usuário
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "codigo_usuario"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.Int
    '        oSqlParameter(i).Value = iCodigoUsuario : i += 1

    '        'Seta Parametros - Código Unidade
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "codigo_unidade"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.Int
    '        oSqlParameter(i).Value = iCodigoUnidade : i += 1

    '        'Seta Parametros - Código PCM Programada - Ordem de Serviço
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "codigo_pcm_programada_ordem_servico"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.BigInt
    '        oSqlParameter(i).Value = lCodigoPCMProgramadaOrdemServico : i += 1

    '        'Seta Parametros - Código PCM Programada
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "codigo_pcm_programada"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.BigInt
    '        oSqlParameter(i).Value = lCodigoPCMProgramada : i += 1

    '        'Seta Parametros - Código Fornecedor
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "codigo_fornecedor"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.Int
    '        oSqlParameter(i).Value = IIf(iCodigoFornecedor = -1, DBNull.Value, iCodigoFornecedor) : i += 1

    '        'Seta Parametros - Código Funcionário
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "codigo_funcionario"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.Int
    '        oSqlParameter(i).Value = IIf(iCodigoFuncionario = -1, DBNull.Value, iCodigoFuncionario) : i += 1

    '        'Seta Parametros - Data Início
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "data_inicio"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.Date
    '        oSqlParameter(i).Value = sDataInicio : i += 1

    '        'Seta Parametros - Data Término
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "data_termino"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.Date
    '        oSqlParameter(i).Value = sDataTermino : i += 1

    '        'Seta Parametros - Hora Início
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "hora_inicio"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.Time
    '        oSqlParameter(i).Value = IIf(IsDate(sHoraInicio), sHoraInicio, DBNull.Value) : i += 1

    '        'Seta Parametros - Hora Término
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "hora_termino"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.Time
    '        oSqlParameter(i).Value = IIf(IsDate(sHoraTermino), sHoraTermino, DBNull.Value) : i += 1

    '        'Seta Parametros - Valor
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "valor"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.Float
    '        oSqlParameter(i).Value = dValor : i += 1

    '        'Seta Parametros - Quantidade Equipamento
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "quantidade_equipamento"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.Int
    '        oSqlParameter(i).Value = iQuantidadeEquipamento : i += 1

    '        'Seta Parametros - Descrição Solução
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "descricao_solucao"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.VarChar
    '        oSqlParameter(i).Size = 5000
    '        oSqlParameter(i).Value = sDescricaoSolucao : i += 1

    '        'Seta Parametros - Concluído
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "concluido"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.Bit
    '        oSqlParameter(i).Value = bConcluido : i += 1

    '        'Seta Parametros - Arquivo
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "arquivo"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.VarChar
    '        oSqlParameter(i).Size = 250
    '        oSqlParameter(i).Value = sArquivo : i += 1

    '        'Seta Parametros - Código PCM Programada Ordem Serviço
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "codigo_pcm_ordem_servico"
    '        oSqlParameter(i).Direction = ParameterDirection.Output
    '        oSqlParameter(i).SqlDbType = SqlDbType.BigInt : i += 1

    '        'Seta Parametros - Código PCM Apontamento
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "codigo_pcm_apontamento"
    '        oSqlParameter(i).Direction = ParameterDirection.Output
    '        oSqlParameter(i).SqlDbType = SqlDbType.BigInt

    '        'Executa Query
    '        ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_pcm_apontamento", oSqlParameter)

    '        lCodigoPCMOrdemServico = oSqlParameter(i - 1).Value
    '        lCodigoPCMApontamento = oSqlParameter(i).Value

    '    Catch SqlEx As SqlException
    '        Throw SqlEx
    '    Catch ex As Exception
    '        Throw ex
    '    End Try

    'End Sub

    'Public Sub UpdateApontamento(ByVal iCodigoEmpresa As Integer,
    '                             ByVal iCodigoUsuario As Integer,
    '                             ByVal iCodigoUnidade As Integer,
    '                             ByVal lCodigoApontamento As Long,
    '                             ByVal iCodigoFornecedor As Integer,
    '                             ByVal iCodigoFuncionario As Integer,
    '                             ByVal sDataInicio As String,
    '                             ByVal sDataTermino As String,
    '                             ByVal sHoraInicio As String,
    '                             ByVal sHoraTermino As String)

    '    Try

    '        'Váriaveis Locais
    '        Dim oSqlParameter(9) As SqlParameter
    '        Dim i As Integer = 0

    '        'Seta Parametros - Código Empresa
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "codigo_empresa"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
    '        oSqlParameter(i).Value = iCodigoEmpresa : i += 1

    '        'Seta Parametros - Código Usuário
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "codigo_usuario"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.Int
    '        oSqlParameter(i).Value = iCodigoUsuario : i += 1

    '        'Seta Parametros - Código Unidade
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "codigo_unidade"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.Int
    '        oSqlParameter(i).Value = iCodigoUnidade : i += 1

    '        'Seta Parametros - Código Apontamento
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "codigo_apontamento"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.BigInt
    '        oSqlParameter(i).Value = lCodigoApontamento : i += 1

    '        'Seta Parametros - Código Fornecedor
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "codigo_fornecedor"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.Int
    '        oSqlParameter(i).Value = IIf(iCodigoFornecedor = -1, DBNull.Value, iCodigoFornecedor) : i += 1

    '        'Seta Parametros - Código Funcionário
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "codigo_funcionario"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.Int
    '        oSqlParameter(i).Value = IIf(iCodigoFuncionario = -1, DBNull.Value, iCodigoFuncionario) : i += 1

    '        'Seta Parametros - Data Início
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "data_inicio"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.Date
    '        oSqlParameter(i).Value = sDataInicio : i += 1

    '        'Seta Parametros - Data Término
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "data_termino"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.Date
    '        oSqlParameter(i).Value = sDataTermino : i += 1

    '        'Seta Parametros - Hora Início
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "hora_inicio"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.Time
    '        oSqlParameter(i).Value = IIf(IsDate(sHoraInicio), sHoraInicio, DBNull.Value) : i += 1

    '        'Seta Parametros - Hora Término
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "hora_termino"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.Time
    '        oSqlParameter(i).Value = IIf(IsDate(sHoraTermino), sHoraTermino, DBNull.Value)

    '        'Executa Query
    '        ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_pcm_apontamento", oSqlParameter)

    '    Catch SqlEx As SqlException
    '        Throw SqlEx
    '    Catch ex As Exception
    '        Throw ex
    '    End Try

    'End Sub

    'Public Sub DeleteApontamento(ByVal iCodigoEmpresa As Integer,
    '                               ByVal iCodigoUsuario As Integer,
    '                               ByVal iCodigoUnidade As Integer,
    '                               ByVal lCodigo As Long)

    '    Try

    '        'Váriaveis Locais
    '        Dim oSqlParameter(3) As SqlParameter
    '        Dim i As Integer = 0

    '        'Seta Parametros - Código Empresa
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "codigo_empresa"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
    '        oSqlParameter(i).Value = iCodigoEmpresa : i += 1

    '        'Seta Parametros - Código Usuário
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "codigo_usuario"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.Int
    '        oSqlParameter(i).Value = iCodigoUsuario : i += 1

    '        'Seta Parametros - Código Unidade
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "codigo_unidade"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.Int
    '        oSqlParameter(i).Value = iCodigoUnidade : i += 1

    '        'Seta Parametros - Código
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "codigo"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.BigInt
    '        oSqlParameter(i).Value = lCodigo

    '        'Executa Query
    '        ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_pcm_apontamento", oSqlParameter)

    '    Catch SqlEx As SqlException
    '        Throw SqlEx
    '    Catch ex As Exception
    '        Throw ex
    '    End Try

    'End Sub

    'Public Function LoadApontamentoCheckListExcluir(ByVal iCodigoEmpresa As Integer,
    '                                                ByVal iCodigoUnidade As Integer,
    '                                                ByVal lCodigoPCMApontamento As Long) As List(Of ApontamentoChecklist)

    '    Try

    '        'Váriaveis Locais
    '        Dim oApontamentoChecklist As New List(Of ApontamentoChecklist)
    '        Dim oSqlDataReader As SqlDataReader
    '        Dim oSqlParameter(2) As SqlParameter
    '        Dim i As Integer = 0

    '        'Seta Parametros - Código Empresa
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "codigo_empresa"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
    '        oSqlParameter(i).Value = iCodigoEmpresa : i += 1

    '        'Seta Parametros - Código PCM Apontamento
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "codigo_pcm_apontamento"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.BigInt
    '        oSqlParameter(i).Value = lCodigoPCMApontamento : i += 1

    '        'Seta Parametros - Código Unidade
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "codigo_unidade"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.Int
    '        oSqlParameter(i).Value = iCodigoUnidade

    '        'Executa Query
    '        oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pcm_apontamento_checklist_excluir", oSqlParameter)

    '        While oSqlDataReader.Read

    '            Dim oApontamentoChecklistInfo As New ApontamentoChecklist

    '            oApontamentoChecklistInfo.codigo = oSqlDataReader.Item("codigo")
    '            oApontamentoChecklistInfo.descricao = oSqlDataReader.Item("descricao")
    '            oApontamentoChecklistInfo.resultado = oSqlDataReader.Item("resultado")
    '            oApontamentoChecklistInfo.observacao = oSqlDataReader.Item("observacao")

    '            oApontamentoChecklist.Add(oApontamentoChecklistInfo)

    '        End While

    '        'Fecha o oSqlDataReader
    '        If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

    '        'Retorno da Função
    '        Return oApontamentoChecklist

    '    Catch SqlEx As SqlException
    '        Throw SqlEx
    '    Catch ex As Exception
    '        Throw ex
    '    End Try

    'End Function

    'Public Sub InsertApontamentoChecklist(ByVal iCodigoEmpresa As Integer,
    '                                      ByVal lCodigoApontamento As Long,
    '                                      ByVal iCodigoUnidade As Integer,
    '                                      ByVal sDescricao As String,
    '                                      ByVal iCodigo As Integer,
    '                                      ByVal bConforme As Boolean,
    '                                      ByVal dValor As Double,
    '                                      ByVal sComentario As String)

    '    Try

    '        'Váriaveis Locais
    '        Dim oSqlParameter(7) As SqlParameter
    '        Dim i As Integer = 0

    '        'Seta Parametros - Código Empresa
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "codigo_empresa"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
    '        oSqlParameter(i).Value = iCodigoEmpresa : i += 1

    '        'Seta Parametros - Código Apontamento
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "codigo_pcm_apontamento"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.BigInt
    '        oSqlParameter(i).Value = lCodigoApontamento : i += 1

    '        'Seta Parametros - Código Unidade
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "codigo_unidade"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.Int
    '        oSqlParameter(i).Value = iCodigoUnidade : i += 1

    '        'Seta Parametros - Descrição
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "descricao"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.VarChar
    '        oSqlParameter(i).Size = 50
    '        oSqlParameter(i).Value = sDescricao : i += 1

    '        'Seta Parametros - Código
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "codigo_checklist"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.Int
    '        oSqlParameter(i).Value = iCodigo : i += 1

    '        'Seta Parametros - Conforme
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "conforme"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.Bit
    '        oSqlParameter(i).Value = bConforme : i += 1

    '        'Seta Parametros - Valor
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "valor"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.Float
    '        oSqlParameter(i).Value = dValor : i += 1

    '        'Seta Parametros - Comentário
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "comentario"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.VarChar
    '        oSqlParameter(i).Size = 200
    '        oSqlParameter(i).Value = sComentario

    '        'Executa Query
    '        ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_pcm_apontamento_checklist", oSqlParameter)

    '    Catch SqlEx As SqlException
    '        Throw SqlEx
    '    Catch ex As Exception
    '        Throw ex
    '    End Try

    'End Sub

#End Region

End Class
