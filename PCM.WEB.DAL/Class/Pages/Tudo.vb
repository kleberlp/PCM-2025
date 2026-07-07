Imports PCM.WEB.MODELS
Imports PCM.WEB.DAL.SQLHelper
Imports System.Data.SqlClient

Public Class Tudo

    Private sConnection As String

    Sub New(ByVal sCon As String)
        sConnection = sCon
    End Sub

#Region "::: TUDO EM DIA - CHECKLIST :::"

    ' Lista de locais a executar o checklist (agrupável por setor)
    Public Function LoadTudoChecklist(ByVal codigoEmpresa As Integer,
                                      ByVal codigoUnidade As Integer,
                                      ByVal codigoSetor As Integer,
                                      ByVal status As String) As List(Of TudoLocal)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of TudoLocal)
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("codigo_setor", SqlDbType.Int, codigoSetor),
                CriarParametro("status", SqlDbType.VarChar, status)
            }

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_tudo_checklist", oSqlParameter)

                If oSqlDataReader.HasRows Then

                    While oSqlDataReader.Read

                        Dim oInfo As New TudoLocal

                        oInfo.codigo_unidade = SafeGetInt32(oSqlDataReader, "codigo_unidade")
                        oInfo.unidade = SafeGetString(oSqlDataReader, "unidade")
                        oInfo.codigo_setor = SafeGetInt32(oSqlDataReader, "codigo_setor")
                        oInfo.setor = SafeGetString(oSqlDataReader, "setor")
                        oInfo.codigo = SafeGetInt32(oSqlDataReader, "codigo")
                        oInfo.local = SafeGetString(oSqlDataReader, "local")
                        oInfo.codigo_checklist = SafeGetInt64(oSqlDataReader, "codigo_checklist")
                        oInfo.checklist = SafeGetString(oSqlDataReader, "checklist")
                        oInfo.codigo_periodicidade = SafeGetInt32(oSqlDataReader, "codigo_periodicidade")
                        oInfo.periodicidade = SafeGetString(oSqlDataReader, "periodicidade")
                        oInfo.intervalo = SafeGetInt32(oSqlDataReader, "intervalo")
                        oInfo.data_proxima = SafeGetString(oSqlDataReader, "data_proxima")
                        oInfo.status = SafeGetInt32(oSqlDataReader, "status")
                        oInfo.css_class = SafeGetString(oSqlDataReader, "css_class")
                        oInfo.color = SafeGetString(oSqlDataReader, "color")
                        oInfo.bg_color = SafeGetString(oSqlDataReader, "bg_color")
                        oInfo.codigo_apontamento = SafeGetInt64(oSqlDataReader, "codigo_apontamento")

                        oReturn.Add(oInfo)

                    End While

                End If

            End Using

            'Retorno da Função
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    ' Contadores por status (torre de KPIs)
    Public Function LoadTudoChecklistStatus(ByVal codigoEmpresa As Integer,
                                            ByVal codigoUnidade As Integer,
                                            ByVal codigoSetor As Integer) As TudoStatus

        Try

            'Váriaveis Locais
            Dim oReturn As New TudoStatus
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("codigo_setor", SqlDbType.Int, codigoSetor)
            }

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_tudo_checklist_status", oSqlParameter)

                If oSqlDataReader.HasRows Then

                    While oSqlDataReader.Read

                        oReturn.atrasado = SafeGetInt32(oSqlDataReader, "quantidade_atrasado")
                        oReturn.pendente = SafeGetInt32(oSqlDataReader, "quantidade_pendente")
                        oReturn.manutencao = SafeGetInt32(oSqlDataReader, "quantidade_manutencao")
                        oReturn.nova_vistoria = SafeGetInt32(oSqlDataReader, "quantidade_nova_vistoria")
                        oReturn.realizada = SafeGetInt32(oSqlDataReader, "quantidade_realizada")

                    End While

                End If

            End Using

            'Retorno da Função
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

End Class
