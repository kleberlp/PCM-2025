Imports System.Net.Mail
Imports System.Data.SqlClient
Imports PCM.EMAIL.LAUDO.SQLHelper
Imports System.Net

Module modMain

    Private sConnection As String = "Data Source=VMI264701\MSSQLSERVER01;Initial Catalog=PCM;Persist Security Info=False;User ID=sa;Password=p@ssw0rd013459;"

    ' Classe para armazenar os dados de email
    Public Class EmailData
        Public Property Email As String
        Public Property Body As String
        Public Property Laudo As String
    End Class

    Sub Main()

        'Call LoadEmail()

        ' Rotina principal
        While True

            ' Verifica se é domingo e se são 21:00:00
            If Now.DayOfWeek = DayOfWeek.Sunday AndAlso TimeSerial(Now.Hour, Now.Minute, Now.Second) = TimeSerial(21, 0, 0) Then

            End If

            ' Espera um segundo antes de checar novamente para não sobrecarregar o CPU
            System.Threading.Thread.Sleep(1000 * 60 * 60)

        End While

    End Sub


    Private Sub SendEmail(ByVal sEmail As String,
                          ByVal sBody As String,
                          ByVal sLaudo As String)

        Try
            EnviarEmail(sPara:=sEmail,
                        sAssunto:=String.Concat("Vencimento do Laudo: ", sLaudo),
                        sCorpoHtml:=sBody)
        Catch ex As Exception
            Debug.Print(ex.Message)
        End Try

    End Sub

End Module
