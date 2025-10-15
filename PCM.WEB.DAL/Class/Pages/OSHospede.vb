Imports System.Data.SqlClient
Imports PCM.WEB.DAL.SQLHelper
Imports PCM.WEB.MODELS

Public Class OSHospede

    Private sConnection As String

    Sub New(ByVal sCon As String)
        sConnection = sCon
    End Sub

#Region "::: OS HOSPEDE :::"

    Public Sub InsertOSHospede(ByVal uniqueId As String,
                               ByVal codigoEquipamento As Long,
                               ByVal description As String,
                               ByVal ip As String)

        'Variaveis Locais
        Dim oSqlParameter As SqlParameter() = {
                CriarParametro("uniqueId", SqlDbType.VarChar, uniqueId),
                CriarParametro("codigoEquipamento", SqlDbType.BigInt, codigoEquipamento),
                CriarParametro("description", SqlDbType.VarChar, description),
                CriarParametro("ip", SqlDbType.VarChar, ip)
            }

        Try

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_pcm_ordem_servico_hospede", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function InfoApartamento(ByVal uniqueId As String) As OSHospedeApartamento

        'Variaveis Locais
        Dim oSqlParameter As SqlParameter() = {
                CriarParametro("uniqueId", SqlDbType.VarChar, uniqueId)
            }

        Dim oReturn As New OSHospedeApartamento

        Try

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pcm_ordem_servico_hospede_apartamento_info", oSqlParameter)

                While oSqlDataReader.Read

                    oReturn = New OSHospedeApartamento
                    oReturn.codigoEmpresa = oSqlDataReader.Item("codigo_empresa")
                    oReturn.codigoUnidade = oSqlDataReader.Item("codigo_unidade")
                    oReturn.codigoApartamento = oSqlDataReader.Item("codigo_apartamento")
                    oReturn.unidade = oSqlDataReader.Item("unidade")
                    oReturn.apartamento = oSqlDataReader.Item("apartamento")
                    oReturn.bloco = oSqlDataReader.Item("bloco")
                    oReturn.andar = oSqlDataReader.Item("andar")

                End While

            End Using

            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

End Class
