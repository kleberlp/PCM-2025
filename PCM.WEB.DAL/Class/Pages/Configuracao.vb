Imports System.Data.SqlClient
Imports PCM.WEB.DAL.SQLHelper
Imports PCM.WEB.MODELS

Public Class Configuracao

    Private sConnection As String

    Sub New(ByVal sCon As String)
        sConnection = sCon
    End Sub

#Region "::: INTERFACE OPERA :::"

    Public Function LoadConfiguracaoOpera(ByVal codigoEmpresa As Integer,
                                          ByVal codigoUnidade As Integer) As interfaceOpera

        'Variaveis Locais
        Dim oReturn As New interfaceOpera

        Try

            'Váriaveis Locais
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade)
            }

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_configuracao_interface_opera", oSqlParameter)

                While oSqlDataReader.Read

                    oReturn.hostname = oSqlDataReader.Item("hostname")
                    oReturn.username = oSqlDataReader.Item("username")
                    oReturn.password = oSqlDataReader.Item("password")
                    oReturn.appKey = oSqlDataReader.Item("app_key")
                    oReturn.clientId = oSqlDataReader.Item("client_id")
                    oReturn.clientSecret = oSqlDataReader.Item("client_secret")
                    oReturn.intervalo = oSqlDataReader.Item("intervalo")

                End While

            End Using

            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Sub UpdateConfiguracaoOpera(ByVal codigoEmpresa As Integer,
                                       ByVal codigoUnidade As Integer,
                                       ByVal codigoUsuario As Integer,
                                       ByVal hostname As String,
                                       ByVal username As String,
                                       ByVal password As String,
                                       ByVal appKey As String,
                                       ByVal clientId As String,
                                       ByVal clientSecret As String,
                                       ByVal intervalo As Integer)

        'Váriaveis Locais
        Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, IIf(codigoUnidade = -1, DBNull.Value, codigoUnidade)),
                CriarParametro("codigo_usuario", SqlDbType.Int, codigoUsuario),
                CriarParametro("hostname", SqlDbType.VarChar, hostname),
                CriarParametro("username", SqlDbType.VarChar, username),
                CriarParametro("password", SqlDbType.VarChar, password),
                CriarParametro("app_key", SqlDbType.VarChar, appKey),
                CriarParametro("client_id", SqlDbType.VarChar, clientId),
                CriarParametro("client_secret", SqlDbType.VarChar, clientSecret),
                CriarParametro("intervalo", SqlDbType.Int, intervalo)
            }

        Try

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_configuracao_interface_opera", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region

End Class
