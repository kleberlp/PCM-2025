Imports System.IO
Imports PCM.IMAGE.RESIZE.SQLHelper
Imports System.Data.SqlClient
Imports PCM.WEB.DAL


Public Class frmMain

    Private sConnection As String = "Data Source=VMI264701\MSSQLSERVER01;Initial Catalog=PCM;Persist Security Info=False;User ID=sa;Password=p@ssw0rd013459;"

    Sub RedimensionarImagens(ByVal arquivo As String, ByVal newWidth As Integer, ByVal sNewDirectory As String, ByVal sNewPath As String)

        Try
            Dim imagem As New Bitmap(arquivo)

            If imagem.Width <> Width Then

                ' Calcular a nova altura mantendo a proporção
                Dim newHeight As Integer = CInt((imagem.Height / CDbl(imagem.Width)) * newWidth)

                ' Criar um novo bitmap com as dimensões desejadas
                Dim resizedImage As New Bitmap(newWidth, newHeight)

                ' Desenhar a imagem original no novo bitmap
                Using g As Graphics = Graphics.FromImage(resizedImage)
                    g.DrawImage(imagem, 0, 0, newWidth, newHeight)
                End Using

                If Directory.Exists(sNewDirectory) = False Then
                    Directory.CreateDirectory(sNewDirectory)
                End If

                resizedImage.Save(sNewPath)

                txtLog.Text = sNewPath & vbCrLf & txtLog.Text

            End If
            imagem.Dispose()

        Catch ex As Exception
            MsgBox("Erro ao processar o arquivo: " & ex.Message)
        End Try

    End Sub

    Private Sub btnExecute_Click(sender As Object, e As EventArgs) Handles btnExecute.Click
        Call LoadImage()
    End Sub

    Public Sub LoadImage()

        Try

            'Váriaveis Locais
            Dim oSQlDataReader As SqlDataReader
            Dim i As Integer = 0

            'Executa Query
            oSQlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_imagem")

            While oSQlDataReader.Read
                Call RedimensionarImagens(oSQlDataReader.Item("arquivo"),
                                          400,
                                          oSQlDataReader.Item("diretorio"),
                                          oSQlDataReader.Item("new_path"))
            End While

            If oSQlDataReader.IsClosed = False Then
                oSQlDataReader.Close()
                oSQlDataReader = Nothing
            End If

        Catch ex As Exception
            MsgBox("ERRO: " & ex.Message.ToString())
        End Try

    End Sub

End Class
