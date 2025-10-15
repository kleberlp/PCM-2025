Imports System.Data
Imports System.Data.SqlClient
Imports System.Xml
Imports PCM.WEB.OS.DAL.SQLHelper
Imports PCM.WEB.OS.MODELS.Models

Public Class Combo

    Private sConnection As String

    Public Sub New(ByVal sConnectionString)
        sConnection = sConnectionString
    End Sub

#Region "::: COMBO :::"

    Public Function LoadCombo(ByVal storedProcedure As String,
                              Optional ByVal codigoEmpresa As Integer = -1,
                              Optional ByVal codigoUnidade As Integer = -1) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlParameter As List(Of SqlParameter) = New List(Of SqlParameter)

        Try

            AddSqlParameter(oSqlParameter, "codigo_empresa", SqlDbType.Int, 0, codigoEmpresa)
            AddSqlParameter(oSqlParameter, "codigo_unidade", SqlDbType.Int, 0, codigoUnidade)

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, storedProcedure, oSqlParameter.ToArray())

                While oSqlDataReader.Read

                    Dim combo As New ListCombo

                    combo.codigo = oSqlDataReader.Item("codigo")
                    combo.descricao = oSqlDataReader.Item("descricao")

                    oCombo.Add(combo)
                End While
            End Using

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function
    Public Function LoadComboString(ByVal storedProcedure As String,
                                    Optional ByVal codigoEmpresa As Integer = -1,
                                    Optional ByVal codigoUnidade As Integer = -1) As List(Of ListComboString)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListComboString)
        Dim oSqlParameter As List(Of SqlParameter) = New List(Of SqlParameter)

        Try

            AddSqlParameter(oSqlParameter, "codigoEmpresa", SqlDbType.Int, 0, codigoEmpresa)
            AddSqlParameter(oSqlParameter, "codigoUnidade", SqlDbType.Int, 0, codigoUnidade)

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, storedProcedure, oSqlParameter.ToArray())

                While oSqlDataReader.Read

                    Dim combo As New ListComboString

                    combo.codigo = oSqlDataReader.Item("codigo")
                    combo.descricao = oSqlDataReader.Item("descricao")

                    oCombo.Add(combo)

                End While

            End Using

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

End Class
