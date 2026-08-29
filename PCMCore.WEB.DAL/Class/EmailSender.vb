Imports System.Configuration
Imports System.IO
Imports System.Net
Imports System.Net.Mail
Imports System.Text
Imports System.Text.RegularExpressions

' ============================================================
'  EmailSender — camada única de envio de e-mail.
'  (cópia idêntica em: PCM.SERVICE.MESSAGE, PCM.SERVICE.LAUDO,
'   PCM.SERVICE.INTERCITY, PCM.EMAIL.LAUDO e PCM.WEB.DAL —
'   alterou aqui, replique nas demais)
'
'  Provedores (appSettings do App.config / Web.config):
'    Email:Provider = "auto"  -> Brevo se houver Email:ApiKey, senão SMTP
'                     "brevo" -> força a API do Brevo
'                     "smtp"  -> força o SMTP clássico
'
'  A ideia é migrar do SMTP (GoDaddy) para um serviço de mensageria
'  sem recompilar: basta colar a chave da API na configuração.
' ============================================================
Public Module EmailSender

    Private ReadOnly EMAIL_REGEX As New Regex("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")

    Private Function Config(ByVal sChave As String, ByVal sPadrao As String) As String
        Dim sValor As String = ConfigurationManager.AppSettings(sChave)
        If String.IsNullOrWhiteSpace(sValor) Then Return sPadrao
        Return sValor.Trim()
    End Function

    ' Envia para um ou mais destinatários separados por ";".
    ' Lança exceção em falha — o chamador decide logar/seguir.
    Public Sub EnviarEmail(ByVal sPara As String,
                           ByVal sAssunto As String,
                           ByVal sCorpoHtml As String,
                           Optional ByVal ePrioridade As MailPriority = MailPriority.High)

        Dim oDestinos As New List(Of String)
        For Each sEmail As String In sPara.Split(";"c)
            If EMAIL_REGEX.IsMatch(sEmail.Trim()) Then oDestinos.Add(sEmail.Trim())
        Next

        If oDestinos.Count = 0 Then
            Throw New ArgumentException("Nenhum destinatário válido em: " & sPara)
        End If

        Dim sProvider As String = Config("Email:Provider", "auto").ToLower()
        Dim sApiKey As String = Config("Email:ApiKey", "")

        If sProvider = "brevo" OrElse (sProvider = "auto" AndAlso sApiKey <> "") Then
            EnviarViaBrevo(oDestinos, sAssunto, sCorpoHtml, sApiKey)
        Else
            EnviarViaSmtp(oDestinos, sAssunto, sCorpoHtml, ePrioridade)
        End If

    End Sub

    ' --- Brevo (https://developers.brevo.com/reference/sendtransacemail) ---
    Private Sub EnviarViaBrevo(ByVal oDestinos As List(Of String),
                               ByVal sAssunto As String,
                               ByVal sCorpoHtml As String,
                               ByVal sApiKey As String)

        If sApiKey = "" Then
            Throw New InvalidOperationException("Email:Provider=brevo mas Email:ApiKey está vazio na configuração")
        End If

        Dim oJson As New StringBuilder()
        oJson.Append("{""sender"":{""name"":").Append(JsonStr(Config("Email:FromName", "PCM by SIM")))
        oJson.Append(",""email"":").Append(JsonStr(Config("Email:From", "no-reply@pcmbysim.com.br")))
        oJson.Append("},""to"":[")
        For i As Integer = 0 To oDestinos.Count - 1
            If i > 0 Then oJson.Append(",")
            oJson.Append("{""email"":").Append(JsonStr(oDestinos(i))).Append("}")
        Next
        oJson.Append("],""subject"":").Append(JsonStr(sAssunto))
        oJson.Append(",""htmlContent"":").Append(JsonStr(sCorpoHtml)).Append("}")

        Dim abBody As Byte() = Encoding.UTF8.GetBytes(oJson.ToString())

        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

        Dim oRequest As HttpWebRequest = CType(WebRequest.Create("https://api.brevo.com/v3/smtp/email"), HttpWebRequest)
        oRequest.Method = "POST"
        oRequest.ContentType = "application/json"
        oRequest.Accept = "application/json"
        oRequest.Headers.Add("api-key", sApiKey)
        oRequest.Timeout = 30000
        oRequest.ContentLength = abBody.Length

        Using oStream As Stream = oRequest.GetRequestStream()
            oStream.Write(abBody, 0, abBody.Length)
        End Using

        Try
            Using oResponse As HttpWebResponse = CType(oRequest.GetResponse(), HttpWebResponse)
                ' 201 = aceito; qualquer 2xx serve
            End Using
        Catch ex As WebException
            ' Recupera o corpo do erro da API (motivo real: chave inválida,
            ' remetente não verificado, cota estourada...) para o log
            Dim sDetalhe As String = ex.Message
            If ex.Response IsNot Nothing Then
                Using oReader As New StreamReader(ex.Response.GetResponseStream())
                    sDetalhe = oReader.ReadToEnd()
                End Using
            End If
            Throw New Exception("Brevo recusou o envio: " & sDetalhe, ex)
        End Try

    End Sub

    ' Escapa uma string para literal JSON (com as aspas externas)
    Private Function JsonStr(ByVal sValor As String) As String
        Dim oSb As New StringBuilder("""")
        For Each c As Char In sValor
            Select Case c
                Case """"c : oSb.Append("\""")
                Case "\"c : oSb.Append("\\")
                Case ControlChars.Cr : oSb.Append("\r")
                Case ControlChars.Lf : oSb.Append("\n")
                Case ControlChars.Tab : oSb.Append("\t")
                Case Else
                    If AscW(c) < 32 Then
                        oSb.AppendFormat("\u{0:x4}", AscW(c))
                    Else
                        oSb.Append(c)
                    End If
            End Select
        Next
        Return oSb.Append("""").ToString()
    End Function

    ' --- SMTP clássico (fallback), com credenciais na configuração ---
    Private Sub EnviarViaSmtp(ByVal oDestinos As List(Of String),
                              ByVal sAssunto As String,
                              ByVal sCorpoHtml As String,
                              ByVal ePrioridade As MailPriority)

        Dim sRemetente As String = Config("Email:From", "no-reply@pcmbysim.com.br")

        Using oMailMessage As New MailMessage()

            For Each sEmail As String In oDestinos
                oMailMessage.To.Add(sEmail)
            Next

            oMailMessage.From = New MailAddress(sRemetente, Config("Email:FromName", "PCM by SIM"), Encoding.UTF8)
            oMailMessage.Subject = sAssunto
            oMailMessage.SubjectEncoding = Encoding.UTF8
            oMailMessage.Body = sCorpoHtml
            oMailMessage.BodyEncoding = Encoding.UTF8
            oMailMessage.IsBodyHtml = True
            oMailMessage.Priority = ePrioridade

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

            Using oSmtpClient As New SmtpClient()
                oSmtpClient.Credentials = New NetworkCredential(Config("Smtp:User", sRemetente), Config("Smtp:Password", ""))
                ' 587 = STARTTLS, o modo suportado pelo System.Net.Mail.
                ' Na 465 (SSL implícito) cliente e servidor ficam esperando um ao
                ' outro falar primeiro e o envio estoura em timeout.
                oSmtpClient.Port = Integer.Parse(Config("Smtp:Port", "587"))
                oSmtpClient.Host = Config("Smtp:Host", "smtpout.secureserver.net")
                oSmtpClient.EnableSsl = True
                oSmtpClient.Timeout = 30000

                oSmtpClient.Send(oMailMessage)
            End Using

        End Using

    End Sub

End Module
