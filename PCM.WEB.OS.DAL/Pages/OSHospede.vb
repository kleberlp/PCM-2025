Imports System.Data
Imports System.Data.SqlClient
Imports PCM.WEB.OS.MODELS
Imports PCM.WEB.OS.DAL.SQLHelper

Public Class OSHospede

    Private sConnection As String

    Public Sub New(ByVal sConnectionString)
        sConnection = sConnectionString
    End Sub

#Region "::: OS HOSPEDE :::"

    Public Sub InsertOSHospede(ByVal uniqueId As String,
                               ByVal codigoEquipamento As Long,
                               ByVal descricao As String,
                               ByVal filePath As String,
                               ByVal ip As String)

        Dim oSqlParameter As List(Of SqlParameter) = New List(Of SqlParameter)

        Try

            AddSqlParameter(oSqlParameter, "uniqueId", SqlDbType.VarChar, 50, uniqueId)
            AddSqlParameter(oSqlParameter, "codigo_equipamento", SqlDbType.VarChar, 0, codigoEquipamento)
            AddSqlParameter(oSqlParameter, "descricao", SqlDbType.VarChar, 250, descricao)
            AddSqlParameter(oSqlParameter, "file_path", SqlDbType.VarChar, 5000, filePath)
            AddSqlParameter(oSqlParameter, "ip", SqlDbType.VarChar, 50, ip)

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_pcm_ordem_servico_hospede", oSqlParameter.ToArray())

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function InfoApartamento(ByVal uniqueId As String) As OSHospedeApartamento

        Dim oSqlParameter As List(Of SqlParameter) = New List(Of SqlParameter)

        Try

            AddSqlParameter(oSqlParameter, "uniqueId", SqlDbType.VarChar, 50, uniqueId)

            Dim oReturn As New OSHospedeApartamento

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pcm_ordem_servico_hospede_apartamento_info", oSqlParameter.ToArray())

                While oSqlDataReader.Read

                    oReturn = New OSHospedeApartamento
                    oReturn.codigoEmpresa = SafeGetLong(oSqlDataReader, "codigo_empresa")
                    oReturn.codigoUnidade = SafeGetLong(oSqlDataReader, "codigo_unidade")
                    oReturn.codigoApartamento = SafeGetLong(oSqlDataReader, "codigo_apartamento")
                    oReturn.unidade = SafeGetString(oSqlDataReader, "unidade")
                    oReturn.apartamento = SafeGetString(oSqlDataReader, "apartamento")
                    oReturn.bloco = SafeGetString(oSqlDataReader, "bloco")
                    oReturn.andar = SafeGetString(oSqlDataReader, "andar")

                End While

            End Using

            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function LoadRatingQuestions(ByVal uniqueId As String) As List(Of PerguntasAvaliacao)

        Dim oSqlParameter As List(Of SqlParameter) = New List(Of SqlParameter)

        Try

            AddSqlParameter(oSqlParameter, "uniqueId", SqlDbType.VarChar, 50, uniqueId)

            Dim oReturn As New List(Of PerguntasAvaliacao)

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_perguntas_avaliacao", oSqlParameter.ToArray())

                While oSqlDataReader.Read

                    Dim oInfo As New PerguntasAvaliacao
                    oInfo.codigo = SafeGetLong(oSqlDataReader, "codigo")
                    oInfo.descricao = SafeGetString(oSqlDataReader, "descricao")

                    oReturn.Add(oInfo)

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
