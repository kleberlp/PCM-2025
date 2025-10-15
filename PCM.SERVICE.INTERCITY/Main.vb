Imports System.Data.SqlClient
Imports System.Net
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Net.Mail
Imports System.Text
Imports System.Threading
Imports Newtonsoft.Json
Imports PCM.SERVICE.INTERCITY.SQLHelper

Public Class Main

    'Váriaveis
    Private oThread As Thread
    Private oInterfaceTotvs As New List(Of interfaceTotvs)

    Private Class interfaceTotvs
        Public Property codigoEmpresa As Integer
        Public Property hostname As String
        Public Property endpointReserva As String
        Public Property username As String
        Public Property password As String
        Public Property intervalo As Integer
        Public Property hotelID As List(Of String)
        Public Property lastUpdate As Date
    End Class

    Public Class InterfaceStatusUH
        Public Property row As List(Of InterfaceStatusUHDetails)
    End Class

    Public Class InterfaceStatusUHDetails
        Public Property IDHOTEL As Integer
        Public Property UH As String
        Public Property STATUSDAUH As String
        Public Property STATUSDAGOV As String
    End Class

    Protected Overrides Sub OnStart(ByVal args() As String)

        'Váriaveis Locais
        oThread = New Thread(AddressOf PCM_SERVICE)
        oThread.Start()

    End Sub

    Protected Overrides Sub OnStop()
        ' Adicione código aqui para realizar qualquer limpeza necessária para parar seu serviço.
    End Sub

    Private Async Sub PCM_SERVICE()

        Try

            'Carrega Informações de Interface Wish
            Call LoadConfigInterfaceTotvs()

            While True

                Try

                    For Each oInterface As interfaceTotvs In oInterfaceTotvs

                        If (IsNothing(oInterface.lastUpdate) = False) Then
                            WriteLog("LAST UPDATE: " & oInterface.lastUpdate.ToString())
                        End If

                        If IsNothing(oInterface.lastUpdate) OrElse (DateAdd(DateInterval.Minute, oInterface.intervalo, oInterface.lastUpdate) < Now()) Then

                            WriteLog("GET HOTELS INTERCITY")

                            For Each sHotelID As String In oInterface.hotelID

                                Try

                                    Dim resultGetHotels As Boolean = Await GetHotelsTotvs(sHostname:=oInterface.hostname,
                                                                                          sUsername:=oInterface.username,
                                                                                          sPassword:=oInterface.password,
                                                                                          iCodigoEmpresa:=oInterface.codigoEmpresa,
                                                                                          sHotelID:=sHotelID)

                                    WriteLog("RESULT GETHOTELS " & sHotelID & ": " & resultGetHotels.ToString)

                                Catch ex As Exception

                                    WriteLog("ERROR GETHOTELS " & sHotelID & ": " & ex.Message.ToString())

                                End Try

                                Try

                                    If (oInterface.endpointReserva <> "") Then

                                        Dim resultGetHotels As Boolean = Await GetHotelsReservas(sHostname:=oInterface.endpointReserva,
                                                                                             sUsername:=oInterface.username,
                                                                                             sPassword:=oInterface.password,
                                                                                             iCodigoEmpresa:=oInterface.codigoEmpresa,
                                                                                             sHotelID:=sHotelID)

                                        WriteLog("RESULT GETRESERVAS " & sHotelID & ": " & resultGetHotels.ToString)

                                    End If

                                Catch ex As Exception

                                    WriteLog("ERROR GETRESERVAS " & sHotelID & ": " & ex.Message.ToString())

                                End Try

                            Next


                            WriteLog("TÉRMINO GET HOTELS")


                            oInterface.lastUpdate = Now()

                        End If

                    Next

                Catch ex As Exception
                    WriteLog("wish: " & ex.Message.ToString())
                End Try

                Thread.Sleep(_Minute)

            End While

        Catch ex As Exception
            WriteLog(ex.Message)
        End Try

    End Sub

#Region " ::: TOTVS :::"

    Private Sub LoadConfigInterfaceTotvs()

        'Váriaveis Locais
        Dim oSqlDataReader As SqlDataReader

        Try

            'Executa Query  
            oSqlDataReader = ExecuteReader(gDatabase, CommandType.StoredProcedure, "sp_select_configuracao_interface_totvs")

            While oSqlDataReader.Read

                Dim oInfo As New interfaceTotvs

                oInfo.codigoEmpresa = oSqlDataReader.Item("codigo_empresa")
                oInfo.hostname = oSqlDataReader.Item("hostname")
                oInfo.endpointReserva = oSqlDataReader.Item("endpointReservas")
                oInfo.username = oSqlDataReader.Item("username")
                oInfo.password = oSqlDataReader.Item("password")
                oInfo.intervalo = oSqlDataReader.Item("intervalo")
                oInfo.lastUpdate = DateAdd(DateInterval.Minute, oSqlDataReader.Item("intervalo") * -1, Now())
                oInfo.hotelID = New List(Of String)
                oInfo.hotelID = LoadConfigInterfaceTotvsHotels(iCodigoEmpresa:=oSqlDataReader.Item("codigo_empresa"))

                oInterfaceTotvs.Add(oInfo)

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Private Function LoadConfigInterfaceTotvsHotels(ByVal iCodigoEmpresa As Integer) As List(Of String)

        'Váriaveis Locais
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(0) As SqlParameter
        Dim oReturn As New List(Of String)
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa

            'Executa Query
            oSqlDataReader = ExecuteReader(gDatabase, CommandType.StoredProcedure, "sp_select_configuracao_interface_totvs_hotel", oSqlParameter)

            While oSqlDataReader.Read

                oReturn.Add(oSqlDataReader.Item("hotel_id"))

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            Return oReturn

        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Async Function GetHotelsTotvs(ByVal sHostname As String,
                                         ByVal sUsername As String,
                                         ByVal sPassword As String,
                                         ByVal iCodigoEmpresa As Integer,
                                         ByVal sHotelID As String) As Task(Of Boolean)

        Using client As New HttpClient()

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

            Dim requestMessage As HttpRequestMessage

            requestMessage = New HttpRequestMessage(HttpMethod.Get, sHostname & "?hotelId=" + sHotelID)

            ' Adiciona a autenticação Basic no cabeçalho
            Dim byteArray As Byte() = Encoding.UTF8.GetBytes(sUsername & ":" & sPassword)

            Dim credentials As String = Convert.ToBase64String(Encoding.ASCII.GetBytes(sUsername & ":" & sPassword))

            requestMessage.Headers.Authorization = New AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray))

            Dim result = Await client.SendAsync(requestMessage)

            If result.IsSuccessStatusCode Then

                WriteLog("GET HOTELS INTERCITY " & sHotelID & ": True")

                ' Lida com a resposta da API de acordo com o conteúdo retornado
                Dim content = Await result.Content.ReadAsStringAsync()

                Call InterfaceHotelRoomsTotvs(iCodigoEmpresa:=iCodigoEmpresa,
                                              sHotelID:=sHotelID,
                                              sJSON:=content)

            Else

                Call SendEmail("bruno@simservices.com.br;suporte@simservices.com.br;comercial@simservices.com.br;pcmbysim@simservices.com.br",
                               "VPN Intercity - OUT",
                               "VPN Intercity - OUT")

                Return False

            End If

            Return True

        End Using

    End Function

    Public Async Function GetHotelsReservas(ByVal sHostname As String,
                                            ByVal sUsername As String,
                                            ByVal sPassword As String,
                                            ByVal iCodigoEmpresa As Integer,
                                            ByVal sHotelID As String) As Task(Of Boolean)

        Using client As New HttpClient()

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

            Dim requestMessage As HttpRequestMessage

            requestMessage = New HttpRequestMessage(HttpMethod.Get, sHostname & "?hotelId=" + sHotelID)

            ' Adiciona a autenticação Basic no cabeçalho
            Dim byteArray As Byte() = Encoding.UTF8.GetBytes(sUsername & ":" & sPassword)

            Dim credentials As String = Convert.ToBase64String(Encoding.ASCII.GetBytes(sUsername & ":" & sPassword))

            requestMessage.Headers.Authorization = New AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray))

            Dim result = Await client.SendAsync(requestMessage)

            If result.IsSuccessStatusCode Then

                WriteLog("RESERVAS " & sHotelID & ": True")

                ' Lida com a resposta da API de acordo com o conteúdo retornado
                Dim content = Await result.Content.ReadAsStringAsync()

                Call InterfaceHotelReservas(iCodigoEmpresa:=iCodigoEmpresa,
                                            sHotelID:=sHotelID,
                                            sJSON:=content)

            Else

                Call SendEmail("bruno@simservices.com.br;suporte@simservices.com.br;comercial@simservices.com.br;pcmbysim@simservices.com.br",
                               "VPN Intercity - OUT",
                               "VPN Intercity - OUT")

                Return False

            End If

            Return True

        End Using

    End Function

    Private Sub SendEmail(ByVal sEmail As String,
                          ByVal sBody As String,
                          ByVal sSubject As String)

        Dim sRemetente As String = "pcm@simservices.com.br"
        Dim oEmail As New MailMessage()

        For Each sTo As String In sEmail.Split(";")
            oEmail.To.Add(sTo)
        Next

        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        oEmail.From = New MailAddress(sRemetente, "PCM by SIM", System.Text.Encoding.UTF8)
        oEmail.Subject = sSubject
        oEmail.SubjectEncoding = System.Text.Encoding.UTF8
        oEmail.Body = sBody
        oEmail.BodyEncoding = System.Text.Encoding.UTF8
        oEmail.IsBodyHtml = True
        oEmail.Priority = MailPriority.High
        Dim oSmtpClient As New SmtpClient()
        oSmtpClient.Credentials = New System.Net.NetworkCredential(sRemetente, "p@ssw0rd013459")
        oSmtpClient.Port = 587
        oSmtpClient.Host = "smtp.office365.com"
        oSmtpClient.EnableSsl = True

        Try
            oSmtpClient.Send(oEmail)
        Catch ex As Exception
            Debug.Print(ex.Message)
        End Try

    End Sub

    Public Sub InterfaceHotelRoomsTotvs(ByVal iCodigoEmpresa As Integer,
                                        ByVal sHotelID As String,
                                        ByVal sJSON As String)

        'Variaveis Locais
        Dim oSqlParameter(2) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Hotel ID
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "hotel_id"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sHotelID : i += 1

            'Seta Parametros - JSON
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "json"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Value = sJSON

            'Executa Query
            ExecuteNonQuery(gDatabase, CommandType.StoredProcedure, "sp_interface_cadastro_basico_apartamento_totvs", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InterfaceHotelReservas(ByVal iCodigoEmpresa As Integer,
                                      ByVal sHotelID As String,
                                      ByVal sJSON As String)

        'Variaveis Locais
        Dim oSqlParameter(2) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Hotel ID
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "hotel_id"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sHotelID : i += 1

            'Seta Parametros - JSON
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "json"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Value = sJSON

            'Executa Query
            ExecuteNonQuery(gDatabase, CommandType.StoredProcedure, "sp_interface_cadastro_basico_apartamento_reservas", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region

End Class
