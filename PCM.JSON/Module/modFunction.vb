Imports System.Data.SqlClient
Imports PCM.JSON.SQLHelper
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

#End Region

End Module
