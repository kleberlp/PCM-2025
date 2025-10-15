Imports System.Data.SqlClient
Imports System.Net
Imports System.Text
Imports PCM.JSON.SQLHelper
Imports PCM.WEB.DAL
Imports PCM.WEB.MODELS

Public Class Form1

    Public Structure interfaceInfo
        Public sJSON As Long
        Public sMethod As String
        Public sEndpoint As String
    End Structure

    Private oHashtable As New Hashtable

    Private Async Sub btnExecute_Click(sender As Object, e As EventArgs) Handles btnExecute.Click

        Try


            Dim oAPI As New Api("Data Source=NOTE-KLEBER;Initial Catalog=PCM;Persist Security Info=False;User ID=sa;Password=p@ssw0rd013459;")

            Dim oStatus As New pwaUHStatusUpdate

            oStatus.codigoEmpresa = 1
            oStatus.codigoUnidade = 7
            oStatus.codigoUsuario = 1394
            oStatus.codigoApartamento = 1507
            oStatus.status = 5

            Call oAPI.updateUHStatusPost(uhStatus:=oStatus)

            'Call ExecuteJSONWebApi(sProcedure:=txtQuery.Text)
            MsgBox("FIM")

        Catch ex As Exception
            MsgBox(ex.Message.ToString())
        End Try

    End Sub


    Private Sub ExecuteJSONWebApi(ByVal sProcedure As String)

        Try

            'Váriaveis Locais
            Dim oSqlDataReader As SqlDataReader
            Dim i As Integer = 0

            'Executa Query
            oSqlDataReader = ExecuteReader(gDatabase, CommandType.Text, sProcedure)

            While oSqlDataReader.Read

                Dim oInfo As New interfaceInfo

                oInfo.sJSON = oSqlDataReader.Item("json")
                oInfo.sEndpoint = oSqlDataReader.Item("endpoint")
                oInfo.sMethod = oSqlDataReader.Item("method")

                oHashtable.Add(i.ToString(), oInfo)

                i += 1

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close()

            For Each oInterface As interfaceInfo In oHashtable.Values

                Try

                    'Params
                    Dim reqString() As Byte
                    Dim responseFromApi As String

                    Dim client As WebClient = New WebClient()

                    client.Headers("Content-type") = "application/json"
                    'client.Headers("username") = "webmethods"
                    'client.Headers("password") = "P@ssw0rd013459#"
                    client.Encoding = Encoding.UTF8
                    Dim jsonReq As String = oInterface.sJSON
                    reqString = Encoding.Default.GetBytes(jsonReq)

                    'responseFromApi = client.UploadString(oSqlDataReader.Item("endpoint"), "POST", jsonReq)
                    client.UploadData(oInterface.sEndpoint, oInterface.sMethod, reqString)
                    'responseFromApi = Encoding.Default.GetString(resByte)
                    txtLog.Text &= "SEND: " & txtEndpoint.Text.Trim & vbCrLf
                    'txtLog.Text &= "RESPONSE: " & responseFromApi.ToString() & vbCrLf & vbCrLf

                Catch ex As Exception
                    txtLog.Text &= "-------------------------------------------------------------------------------------------------------"
                    txtLog.Text &= ex.Message.ToString & vbCrLf
                    txtLog.Text &= "-------------------------------------------------------------------------------------------------------" & vbCrLf & vbCrLf

                End Try

            Next

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub


End Class
