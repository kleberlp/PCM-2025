Imports System.Data.SqlClient
Imports System.IO
Imports System.Net
Imports System.Net.Mail
Imports System.Runtime.Remoting.Messaging
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.Web.Script.Serialization
Imports FirebaseAdmin
Imports FirebaseAdmin.Messaging
Imports Google.Apis.Auth.OAuth2
Imports PCM.SERVICE.MESSAGE.SQLHelper
Imports System.Net.Mail.MailMessage

Public Class Main

    ' Classe para armazenar os dados de email
    Public Class EmailData
        Public Property Email As String
        Public Property Body As String
        Public Property Laudo As String
    End Class

    'Váriaveis
    Private oThread As Thread

    Protected Overrides Sub OnStart(ByVal args() As String)

        'Váriaveis Locais
        oThread = New Thread(AddressOf PCM_SERVICE)
        oThread.Start()

    End Sub

    Protected Overrides Sub OnStop()
        ' Adicione código aqui para realizar qualquer limpeza necessária para parar seu serviço.
    End Sub

    Private Sub PCM_SERVICE()

        FirebaseApp.Create(New AppOptions With
        {
            .Credential = GoogleCredential.FromFile(System.AppDomain.CurrentDomain.BaseDirectory() & "\secrets.json")
        })

        Call LoadEmailLaudo()

        While True

            'Try
            '    Call SendMessageFirebase()
            'Catch ex As Exception
            '    WriteLog("SendMessageFirebase:" & ex.Message)
            'End Try

            Try
                Call PWASendMessageFirebase()
            Catch ex As Exception
                WriteLog("PWASendMessageFirebase:" & ex.Message)
            End Try

            Try
                Call SendEmail()
            Catch ex As Exception
                WriteLog("SendEmail:" & ex.Message)
            End Try

            ' Verifica se é domingo e se são 21:00:00
            If Now.DayOfWeek = DayOfWeek.Sunday AndAlso TimeSerial(Now.Hour, Now.Minute, Now.Second) = TimeSerial(21, 0, 0) Then
                Call LoadEmailLaudo()
            End If

            Thread.Sleep(_Minute)

        End While

    End Sub

#Region ":::: EMAIL LAUDO :::"

    Private Sub LoadEmailLaudo()

        Dim emailList As New List(Of EmailData) ' Lista para armazenar dados do banco

        'Carregar os dados em memória
        Using oSqlDataReader As SqlDataReader = ExecuteReader(gDatabase, CommandType.StoredProcedure, "sp_select_service_email_laudo")

            While oSqlDataReader.Read

                emailList.Add(New EmailData With {
                            .Email = oSqlDataReader.Item("email").ToString(),
                            .Body = oSqlDataReader.Item("body").ToString(),
                            .Laudo = oSqlDataReader.Item("laudo").ToString()
                        })

            End While

        End Using

        ' Envio dos emails
        For Each emailData As EmailData In emailList

            Try

                Call SendEmailLaudo(sEmail:=emailData.Email,
                                    sBody:=emailData.Body,
                                    sLaudo:=emailData.Laudo)

            Catch ex As Exception
                WriteLog("SendEmailLaudo:" & ex.Message)
            End Try

        Next

    End Sub

    Private Sub SendEmailLaudo(ByVal sEmail As String,
                                ByVal sBody As String,
                                ByVal sLaudo As String)

        Dim sRemetente As String = "no-reply@pcmbysim.com.br"
        Dim oEmail As New MailMessage()

        For Each sTo As String In sEmail.Split(";")
            oEmail.To.Add(sTo)
        Next
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        oEmail.From = New MailAddress(sRemetente, "PCM by SIM", System.Text.Encoding.UTF8)
        oEmail.Subject = String.Concat("Vencimento do Laudo: ", sLaudo)
        oEmail.SubjectEncoding = System.Text.Encoding.UTF8
        oEmail.Body = sBody
        oEmail.BodyEncoding = System.Text.Encoding.UTF8
        oEmail.IsBodyHtml = True
        oEmail.Priority = MailPriority.High

        Dim oSmtpClient As New SmtpClient()
        oSmtpClient.Credentials = New System.Net.NetworkCredential(sRemetente, "$Noreply@2026$")
        oSmtpClient.Port = 465
        oSmtpClient.Host = "smtpout.secureserver.net"
        oSmtpClient.EnableSsl = True

        Try
            oSmtpClient.Send(oEmail)
        Catch ex As Exception
            Debug.Print(ex.Message)
        End Try

    End Sub

#End Region

#Region " ::: MESSAGE FIREBASE :::"

    Private Sub SendMessageFirebase()

        'Váriaveis Locais
        Dim oSqlDataReader As SqlDataReader
        Dim oHashTable As New Hashtable()

        Try

            'Executa Query
            oSqlDataReader = ExecuteReader(gDatabase, CommandType.StoredProcedure, "sp_select_firebase_message")

            While oSqlDataReader.Read

                Dim oFibMessage As New FibMessage

                oFibMessage.sTitle = oSqlDataReader.Item("title")
                oFibMessage.sMessage = oSqlDataReader.Item("message")
                oFibMessage.sPriority = oSqlDataReader.Item("priority")
                oFibMessage.sToken = oSqlDataReader.Item("token")
                oFibMessage.lCodigo = oSqlDataReader.Item("codigo")

                oHashTable.Add(oSqlDataReader.Item("codigo"), oFibMessage)

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            For Each oMessage As FibMessage In oHashTable.Values

                SendMessage(sTitle:=oMessage.sTitle,
                            sMessage:=oMessage.sMessage,
                            sPriority:=oMessage.sPriority,
                            sToken:=oMessage.sToken)

                UpdateMessage(oMessage.lCodigo)

            Next

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateMessage(ByVal lCodigo As Long)

        'Váriaveis Locais
        Dim oSqlParameter(0) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigo

            'Executa Query
            ExecuteNonQuery(gDatabase, CommandType.StoredProcedure, "sp_update_firebase_message", oSqlParameter)

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub SendMessage(ByVal sTitle As String,
                           ByVal sMessage As String,
                           ByVal sPriority As String,
                           ByVal sToken As String)

        Try

            Dim sDeviceTokens As List(Of String) = sToken.Split("|PCMbySIM|").ToList()
            Dim sPredicado As Predicate(Of String) = Function(nombre) nombre.Equals("PCMbySIM")

            sDeviceTokens.RemoveAll(sPredicado)
            sDeviceTokens.Remove("")

            If (sDeviceTokens.Count > 0) Then

                Dim timeStamp As String = DateTime.Now.ToFileTime().ToString()

                Dim data = New With
                {
                    .registration_ids = sDeviceTokens.ToArray(),
                    .priority = "high",
                    .content_available = True,
                    .data = New With
                    {
                        .message = sMessage.ToUpper(),
                        .title = sTitle,
                        .is_background = True,
                        .timeStamp = timeStamp
                    }
                }

                SendNotification(data)

            End If

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub SendNotification(ByVal data As Object)
        Try

            Dim serializer = New JavaScriptSerializer()
            Dim json = serializer.Serialize(data)
            Dim byteArray As Byte() = Encoding.UTF8.GetBytes(json)

            SendNotification(byteArray)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub SendNotification(ByVal byteArray As Byte())

        Try

            Dim server_api_key As String = "AAAAhu64c6k:APA91bG9TSbnmR9nKnlR5qcrl6J5QcML-gVnur-D2YYvdua5WRxqwLtOVS6CmNJN0qQaLPGjqmePxj7Azg7e5AYqvqMmjM-Fa3mZseqFXoVCB1MzA3t9mqJs9psxZ0JBJ8OUbW2yNnpg"
            Dim sender_id As String = "579530683305"

            Dim oWebRequest As WebRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send")
            oWebRequest.Method = "post"
            oWebRequest.ContentType = "application/json"
            oWebRequest.Headers.Add(String.Format("Authorization: key={0}", server_api_key))
            oWebRequest.Headers.Add(String.Format("Sender: id={0}", sender_id))

            oWebRequest.ContentLength = byteArray.Length
            Dim dataStream As Stream = oWebRequest.GetRequestStream()
            dataStream.Write(byteArray, 0, byteArray.Length)
            dataStream.Close()

            Dim oWebResponse As WebResponse = oWebRequest.GetResponse()
            dataStream = oWebResponse.GetResponseStream()
            Dim oStreamReader As StreamReader = New StreamReader(dataStream)

            Dim sResponseFromServer As String = oStreamReader.ReadToEnd()

            oStreamReader.Close()
            dataStream.Close()
            oWebResponse.Close()

        Catch Ex As Exception
            Throw Ex
        End Try

    End Sub

#End Region

#Region " ::: PWA MESSAGE FIREBASE :::"

    Private Sub PWASendMessageFirebase()

        'Váriaveis Locais
        Dim oSqlDataReader As SqlDataReader
        Dim oHashTable As New Hashtable()

        Try

            'Executa Query
            oSqlDataReader = ExecuteReader(gDatabase, CommandType.StoredProcedure, "sp_pwa_select_firebase_message")

            While oSqlDataReader.Read

                Dim oFibMessage As New FibMessagePWA

                oFibMessage.sTitle = oSqlDataReader.Item("title")
                oFibMessage.sBody = oSqlDataReader.Item("body")
                oFibMessage.sUnidade = oSqlDataReader.Item("unidade")
                oFibMessage.sTopic = oSqlDataReader.Item("topic")
                oFibMessage.sToken = oSqlDataReader.Item("token")
                oFibMessage.lCodigo = oSqlDataReader.Item("codigo")

                oHashTable.Add(oSqlDataReader.Item("codigo").ToString & vbCrLf & oSqlDataReader.Item("token"), oFibMessage)

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            For Each oMessage As FibMessagePWA In oHashTable.Values

                PWASendMessage(sTitle:=oMessage.sTitle,
                               sUnidade:=oMessage.sUnidade,
                               sBody:=oMessage.sBody,
                               sTopic:=oMessage.sTopic,
                               sToken:=oMessage.sToken)

                PWAUpdateMessage(oMessage.lCodigo)

            Next

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Private Sub PWASendMessage(ByVal sTitle As String,
                               ByVal sUnidade As String,
                               ByVal sBody As String,
                               ByVal sTopic As String,
                               ByVal sToken As String)

        Dim message As New Messaging.Message With
        {
            .Token = sToken,
            .Notification = New Notification With
            {
                .Title = sTitle,
                .Body = sBody
            },
            .Data = New Dictionary(Of String, String) From
            {
                {"codigo", sTopic},
                {"descricao", sTitle},
                {"unidade", sUnidade}
            }
        }

        Try
            Dim response As String = FirebaseMessaging.DefaultInstance.SendAsync(message).GetAwaiter().GetResult()
            Console.WriteLine("Mensagem enviada com sucesso: " & response)
        Catch ex As Exception
            Console.WriteLine("Erro ao enviar mensagem: " & ex.Message)
        End Try

    End Sub

    Public Sub PWAUpdateMessage(ByVal lCodigo As Long)

        'Váriaveis Locais
        Dim oSqlParameter(0) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigo

            'Executa Query
            ExecuteNonQuery(gDatabase, CommandType.StoredProcedure, "sp_pwa_update_firebase_message", oSqlParameter)

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region

#Region "::: EMAIL :::"

    Private Sub SendEmail()

        'Váriaveis Locais
        Dim oSqlDataReader As SqlDataReader
        Dim oHashTable As New Hashtable()

        Try

            'Executa Query
            oSqlDataReader = ExecuteReader(gDatabase, CommandType.StoredProcedure, "sp_select_email")

            While oSqlDataReader.Read

                Dim oEmail As New Email

                oEmail.sTo = oSqlDataReader.Item("para")
                oEmail.sBody = oSqlDataReader.Item("body")
                oEmail.sOrdemServico = oSqlDataReader.Item("ordem_servico")
                oEmail.lCodigo = oSqlDataReader.Item("codigo")

                oHashTable.Add(oSqlDataReader.Item("codigo"), oEmail)

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            For Each oMessage As Email In oHashTable.Values

                Dim sRemetente As String = "no-reply@pcmbysim.com.br"
                Dim oMailMessage As New MailMessage()

                For Each sEmail As String In oMessage.sTo.Split(";")
                    If IsValidEmail(sEmail) Then
                        oMailMessage.To.Add(sEmail)
                    End If
                Next

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
                oMailMessage.From = New MailAddress(sRemetente, "PCM by SIM", System.Text.Encoding.UTF8)
                oMailMessage.Subject = String.Concat("Ordem de Serviço: ", oMessage.sOrdemServico, " - ", DateTime.Now.ToShortTimeString())
                oMailMessage.SubjectEncoding = System.Text.Encoding.UTF8
                oMailMessage.Body = oMessage.sBody
                oMailMessage.BodyEncoding = System.Text.Encoding.UTF8
                oMailMessage.IsBodyHtml = True
                oMailMessage.Priority = MailPriority.High

                Dim oSmtpClient As New SmtpClient()
                oSmtpClient.Credentials = New System.Net.NetworkCredential(sRemetente, "$Noreply@2026$")
                oSmtpClient.Port = 465
                oSmtpClient.Host = "smtpout.secureserver.net"
                oSmtpClient.EnableSsl = True

                Try
                    oSmtpClient.Send(oMailMessage)
                Catch ex As Exception

                End Try

                UpdateEmail(oMessage.lCodigo)

            Next

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Function IsValidEmail(email As String) As Boolean
        Dim pattern As String = "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"
        Dim regex As New Regex(pattern)
        Return regex.IsMatch(email)
    End Function

    Public Sub UpdateEmail(ByVal lCodigo As Long)

        'Váriaveis Locais
        Dim oSqlParameter(0) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigo

            'Executa Query
            ExecuteNonQuery(gDatabase, CommandType.StoredProcedure, "sp_update_email", oSqlParameter)

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region

End Class
