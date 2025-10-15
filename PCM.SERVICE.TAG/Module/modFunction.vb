Imports System.Data.SqlClient
Imports PCM.SERVICE.TAG.SQLHelper
Imports System.IO

Module modFunction

#Region "::: FUNCTION :::"

    Public Function DataBaseExecuteScalar(ByVal sStringConexao As String,
                                          ByVal sQuery As String) As Object

        Try

            'Obtem o DataSet
            Return ExecuteScalar(sStringConexao, CommandType.Text, sQuery)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Sub WriteLog(ByVal sError As String)

        Dim oStreamWriter As StreamWriter

        oStreamWriter = New IO.StreamWriter(System.AppDomain.CurrentDomain.BaseDirectory() & "\" & Format(Now.Date, "yyyy_MM_dd") & ".log", True)
        oStreamWriter.WriteLine(Now & ": " & sError)
        oStreamWriter.Close()

    End Sub

    Public Function CriarParametro(nome As String, tipo As SqlDbType, valor As Object, Optional ByVal direction As ParameterDirection = ParameterDirection.Input) As SqlParameter

        Dim parametro As New SqlParameter With {
            .ParameterName = nome,
            .Direction = direction,
            .SqlDbType = tipo,
            .Value = If(valor Is Nothing, DBNull.Value, valor)
        }
        Return parametro

    End Function

    Public Function SafeGetString(ByVal reader As SqlDataReader, ByVal columnName As String) As String

        If reader.IsDBNull(reader.GetOrdinal(columnName)) Then
            Return String.Empty
        Else
            Return reader.Item(reader.GetOrdinal(columnName)).ToString()
        End If

    End Function

    Public Function SafeGetFloat(ByVal reader As SqlDataReader, ByVal columnName As String) As Double

        If reader.IsDBNull(reader.GetOrdinal(columnName)) Then
            Return 0
        Else
            Return reader.Item(reader.GetOrdinal(columnName))
        End If

    End Function

    Public Function SafeGetLong(ByVal reader As SqlDataReader, ByVal columnName As String) As Long

        If reader.IsDBNull(reader.GetOrdinal(columnName)) Then
            Return 0
        Else
            Return reader.Item(reader.GetOrdinal(columnName))
        End If

    End Function

    Public Function SafeGetBoolean(ByVal reader As SqlDataReader, ByVal columnName As String) As Boolean

        If reader.IsDBNull(reader.GetOrdinal(columnName)) Then
            Return False
        Else
            Return reader.GetSqlBoolean(reader.GetOrdinal(columnName))
        End If

    End Function

#End Region

End Module
