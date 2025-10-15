Imports System.Data.SqlClient
Imports System.Drawing
Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Text
Imports System.Threading
Imports Newtonsoft.Json
Imports PCM.SERVICE.SQLHelper

Public Class Main

    'Váriaveis
    Private oThread As Thread
    Private oInterfaceOpera As New List(Of interfaceOpera)
    Private oInterfaceWish As New List(Of interfaceWish)
    Private lastUpdateTAG As Date

    Private Class interfaceOpera
        Public Property codigoEmpresa As Integer
        Public Property hostname As String
        Public Property username As String
        Public Property password As String
        Public Property appKey As String
        Public Property clientID As String
        Public Property clientSecret As String
        Public Property intervalo As Integer
        Public Property token As returnAuthenticationOpera
        Public Property expirationDateToken As Date
        Public Property hotelID As List(Of String)
        Public Property lastUpdate As Date
    End Class

    Private Class interfaceWish
        Public Property codigoEmpresa As Integer
        Public Property hostname As String
        Public Property username As String
        Public Property password As String
        Public Property intervalo As Integer
        Public Property hotelID As List(Of String)
        Public Property lastUpdate As Date
    End Class

    Public Class returnAuthenticationOpera
        Public Property expires_in As Integer
        Public Property token_type As String
        Public Property oracle_tk_context As String
        Public Property refresh_token As String
        Public Property oracle_grant_type As String
        Public Property access_token As String
    End Class

    Public Class Housekeepingroom
        Public Property housekeepingRoomInfo As Housekeepingroominfo
        Public Property links As List(Of Object)
    End Class

    Public Class Housekeepingroominfo
        Public Property housekeepingRooms As Housekeepingrooms
        Public Property totalPages As Integer
        Public Property offset As Integer
        Public Property limit As Integer
        Public Property hasMore As Boolean
        Public Property totalResults As Integer
    End Class

    Public Class Housekeepingrooms
        Public Property room As List(Of Room)
        Public Property hotelId As String
    End Class


    Public Class WishUH
        Public Property row As List(Of WishUHInfo)
    End Class

    Public Class WishUHInfo
        Public Property IDHOTEL As Integer
        Public Property UH As String
        Public Property CATEGORIA As String
        Public Property STATUSDAUH As String
        Public Property STATUSDAGOV As String
    End Class


    Public Class Room
        Public Property roomType As Roomtype
        Public Property floor As String
        Public Property smokingPreference As String
        Public Property roomId As String
        Public Property housekeeping As Housekeeping
        Public Property discrepancy As List(Of String)
    End Class

    Public Class Roomtype
        Public Property pseudoRoom As Boolean
        Public Property roomClass As String
        Public Property roomType As String
    End Class

    Public Class Housekeeping
        Public Property housekeepingRoomStatus As Housekeepingroomstatus
        Public Property roomPersons As Roompersons
    End Class

    Public Class Housekeepingroomstatus
        Public Property reservationStatusList As List(Of String)
        Public Property housekeepingRoomStatus As String
        Public Property frontOfficeStatus As String
        Public Property housekeepingStatus As String
    End Class

    Public Class Roompersons
        Public Property frontOfficePersons As Integer
        Public Property houseKeepingPersons As Integer
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

            'Seta Váriavel
            lastUpdateTAG = DateAdd(DateInterval.Minute, -2, Now())

            'Carrega Informações de Interface Opera
            Call LoadConfigInterfaceOpera()

            'Carrega Informações de Interface Wish
            Call LoadConfigInterfaceWish()

            While True

                If DateDiff(DateInterval.Minute, lastUpdateTAG, Now()) > 0 Then

                    'Equipamento
                    Try
                        Call UpdateTAGEquipamento()
                    Catch ex As Exception
                        WriteLog("UPDATE TAG EQUIPAMENTO: " & ex.Message)
                    End Try

                    'Rotina
                    Try
                        Call UpdateTAGRotina()
                    Catch ex As Exception
                        WriteLog("UPDATE TAG ROTINA: " & ex.Message)
                    End Try

                    lastUpdateTAG = Now()

                End If

                Try

                    For Each oInterface In oInterfaceOpera

                        If IsNothing(oInterface.token) = True OrElse DateDiff(DateInterval.Second, Now, oInterface.expirationDateToken) < 0 Then

                            Try

                                WriteLog("AUTENTICAÇÃO OPERA")

                                'Carrega novos dados de Integração
                                Call LoadConfigInterfaceOperaUpdate(oInterfaceOpera:=oInterface)

                                'Carrega o Resultado
                                Dim result As returnAuthenticationOpera = Await AuthenticationOpera(sHostname:=oInterface.hostname,
                                                                                                    sUsername:=oInterface.username,
                                                                                                    sPassword:=oInterface.password,
                                                                                                    sClientID:=oInterface.clientID,
                                                                                                    sClientSecret:=oInterface.clientSecret,
                                                                                                    sAppKey:=oInterface.appKey)

                                If result Is Nothing Then

                                    WriteLog("NÃO AUTENTICADO")

                                    Exit For

                                Else


                                    oInterface.token = result
                                    oInterface.expirationDateToken = DateAdd(DateInterval.Second, oInterface.token.expires_in, Now())

                                    WriteLog("TOKEN: " & oInterface.token.access_token)
                                    WriteLog("TOKEN EXPIRATION: " & oInterface.token.expires_in)
                                    WriteLog("TOKEN EXPIRATION DATE: " & oInterface.expirationDateToken)

                                    Thread.Sleep(5000)

                                End If

                            Catch ex As Exception

                                WriteLog("ERRO: " & ex.Message)

                            Finally

                                WriteLog("TÉRMINO AUTENTICAÇÃO OPERA")

                            End Try

                        End If


                        If (IsNothing(oInterface.lastUpdate) = False) Then
                            WriteLog("LAST UPDATE: " & oInterface.lastUpdate.ToString())
                        End If


                        If IsNothing(oInterface.lastUpdate) OrElse (DateAdd(DateInterval.Minute, oInterface.intervalo, oInterface.lastUpdate) < Now()) Then

                            WriteLog("GET HOTELS")

                            For Each sHotelID As String In oInterface.hotelID

                                Try

                                    Dim resultGetHotels As Boolean = Await GetHotelsOpera(sHostname:=oInterface.hostname,
                                                                                          sTokenType:=oInterface.token.token_type,
                                                                                          sTokenAccess:=oInterface.token.access_token,
                                                                                          sAppKey:=oInterface.appKey,
                                                                                          iCodigoEmpresa:=oInterface.codigoEmpresa,
                                                                                          sHotelID:=sHotelID)

                                    WriteLog("RESULT GETHOTELS " & sHotelID & ": " & resultGetHotels.ToString)

                                Catch ex As Exception

                                    WriteLog("ERROR GETHOTELS " & sHotelID & ": " & ex.Message.ToString())

                                End Try

                            Next


                            WriteLog("TÉRMINO GET HOTELS")


                            oInterface.lastUpdate = Now()

                        End If

                    Next

                Catch ex As Exception
                    WriteLog("OPERA: " & ex.Message.ToString())
                End Try

                Try

                    For Each oInterface As interfaceWish In oInterfaceWish

                        If (IsNothing(oInterface.lastUpdate) = False) Then
                            WriteLog("LAST UPDATE: " & oInterface.lastUpdate.ToString())
                        End If

                        If IsNothing(oInterface.lastUpdate) OrElse (DateAdd(DateInterval.Minute, oInterface.intervalo, oInterface.lastUpdate) < Now()) Then

                            WriteLog("GET HOTELS WISH")

                            For Each sHotelID As String In oInterface.hotelID

                                Try

                                    Dim resultGetHotels As Boolean = Await GetHotelsWish(sHostname:=oInterface.hostname,
                                                                                         sUsername:=oInterface.username,
                                                                                         sPassword:=oInterface.password,
                                                                                         iCodigoEmpresa:=oInterface.codigoEmpresa,
                                                                                         sHotelID:=sHotelID)

                                    WriteLog("RESULT GETHOTELS " & sHotelID & ": " & resultGetHotels.ToString)

                                Catch ex As Exception

                                    WriteLog("ERROR GETHOTELS " & sHotelID & ": " & ex.Message.ToString())

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

    Private Sub UpdateTAGEquipamento()

        'Váriaveis Locais
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(3) As SqlParameter
        Dim oHashtable As New Hashtable
        Dim iCount As Long = 0

        Try

            'Executa Query
            oSqlDataReader = ExecuteReader(gDatabase, CommandType.StoredProcedure, "sp_select_cadastro_basico_equipamento_tag")

            While oSqlDataReader.Read

                Dim oEquipamento As Equipamento

                oEquipamento.iCodigoEmpresa = oSqlDataReader.Item("codigo_empresa")
                oEquipamento.iCodigoUnidade = oSqlDataReader.Item("codigo_unidade")
                oEquipamento.lCodigo = oSqlDataReader.Item("codigo")
                oEquipamento.sCode = oSqlDataReader.Item("code")

                oHashtable.Add(iCount.ToString(), oEquipamento)

                iCount += 1

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            For Each oEquipamentoHash As Equipamento In oHashtable.Values

                Dim sURL As String = oEquipamentoHash.sCode
                Dim oImage As Drawing.Image = Nothing
                Dim i As Integer = 0

                If Not String.IsNullOrWhiteSpace(sURL) Then

                    Dim oWebRequest As WebRequest = WebRequest.Create(sURL)

                    Using oWebResponse As WebResponse = oWebRequest.GetResponse
                        Using oStream As Stream = oWebResponse.GetResponseStream
                            oImage = New Bitmap(Drawing.Image.FromStream(oStream))
                        End Using
                    End Using
                End If

                'Seta Parametros - Código Empresa
                oSqlParameter(i) = New SqlParameter
                oSqlParameter(i).ParameterName = "codigo_empresa"
                oSqlParameter(i).Direction = ParameterDirection.Input
                oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
                oSqlParameter(i).Value = oEquipamentoHash.iCodigoEmpresa : i += 1

                'Seta Parametros - Código Unidade
                oSqlParameter(i) = New SqlParameter
                oSqlParameter(i).ParameterName = "codigo_unidade"
                oSqlParameter(i).Direction = ParameterDirection.Input
                oSqlParameter(i).SqlDbType = SqlDbType.Int
                oSqlParameter(i).Value = oEquipamentoHash.iCodigoUnidade : i += 1

                'Seta Parametros - Código
                oSqlParameter(i) = New SqlParameter
                oSqlParameter(i).ParameterName = "codigo"
                oSqlParameter(i).Direction = ParameterDirection.Input
                oSqlParameter(i).SqlDbType = SqlDbType.BigInt
                oSqlParameter(i).Value = oEquipamentoHash.lCodigo : i += 1

                'Carrega Imagem
                Dim oMemoryStream As MemoryStream = New MemoryStream()
                oImage.Save(oMemoryStream, Imaging.ImageFormat.Jpeg)
                Dim bytBLOBData(oMemoryStream.Length - 1) As Byte
                oMemoryStream.Position = 0
                oMemoryStream.Read(bytBLOBData, 0, oMemoryStream.Length)

                'Seta Parametros - TAG
                oSqlParameter(i) = New SqlParameter
                oSqlParameter(i).ParameterName = "tag_imagem"
                oSqlParameter(i).Direction = ParameterDirection.Input
                oSqlParameter(i).SqlDbType = SqlDbType.Image
                oSqlParameter(i).Value = bytBLOBData

                'Executa Query
                ExecuteNonQuery(gDatabase, CommandType.StoredProcedure, "sp_update_cadastro_basico_equipamento_tag", oSqlParameter)

            Next

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Private Sub UpdateTAGRotina()

        'Váriaveis Locais
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(3) As SqlParameter
        Dim oHashtable As New Hashtable
        Dim iCount As Long = 0

        Try

            'Executa Query
            oSqlDataReader = ExecuteReader(gDatabase, CommandType.StoredProcedure, "sp_select_pcm_programada_tag")

            While oSqlDataReader.Read

                Dim oRotina As Rotina

                oRotina.iCodigoEmpresa = oSqlDataReader.Item("codigo_empresa")
                oRotina.iCodigoUnidade = oSqlDataReader.Item("codigo_unidade")
                oRotina.lCodigo = oSqlDataReader.Item("codigo")
                oRotina.sCode = oSqlDataReader.Item("code")

                oHashtable.Add(iCount.ToString(), oRotina)

                iCount += 1

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            For Each oRotinaHash As Rotina In oHashtable.Values

                Dim sURL As String = oRotinaHash.sCode
                Dim oImage As Drawing.Image = Nothing
                Dim i As Integer = 0

                If Not String.IsNullOrWhiteSpace(sURL) Then

                    Dim oWebRequest As WebRequest = WebRequest.Create(sURL)

                    Using oWebResponse As WebResponse = oWebRequest.GetResponse
                        Using oStream As Stream = oWebResponse.GetResponseStream
                            oImage = New Bitmap(Drawing.Image.FromStream(oStream))
                        End Using
                    End Using
                End If

                'Seta Parametros - Código Empresa
                oSqlParameter(i) = New SqlParameter
                oSqlParameter(i).ParameterName = "codigo_empresa"
                oSqlParameter(i).Direction = ParameterDirection.Input
                oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
                oSqlParameter(i).Value = oRotinaHash.iCodigoEmpresa : i += 1

                'Seta Parametros - Código Unidade
                oSqlParameter(i) = New SqlParameter
                oSqlParameter(i).ParameterName = "codigo_unidade"
                oSqlParameter(i).Direction = ParameterDirection.Input
                oSqlParameter(i).SqlDbType = SqlDbType.Int
                oSqlParameter(i).Value = oRotinaHash.iCodigoUnidade : i += 1

                'Seta Parametros - Código
                oSqlParameter(i) = New SqlParameter
                oSqlParameter(i).ParameterName = "codigo"
                oSqlParameter(i).Direction = ParameterDirection.Input
                oSqlParameter(i).SqlDbType = SqlDbType.BigInt
                oSqlParameter(i).Value = oRotinaHash.lCodigo : i += 1

                'Carrega Imagem
                Dim oMemoryStream As MemoryStream = New MemoryStream()
                oImage.Save(oMemoryStream, Imaging.ImageFormat.Jpeg)
                Dim bytBLOBData(oMemoryStream.Length - 1) As Byte
                oMemoryStream.Position = 0
                oMemoryStream.Read(bytBLOBData, 0, oMemoryStream.Length)

                'Seta Parametros - TAG
                oSqlParameter(i) = New SqlParameter
                oSqlParameter(i).ParameterName = "tag_imagem"
                oSqlParameter(i).Direction = ParameterDirection.Input
                oSqlParameter(i).SqlDbType = SqlDbType.Image
                oSqlParameter(i).Value = bytBLOBData

                'Executa Query
                ExecuteNonQuery(gDatabase, CommandType.StoredProcedure, "sp_update_pcm_programada_tag", oSqlParameter)

            Next

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Private Sub UpdateDashboard()

        'Váriaveis Locais
        Dim oSqlDataReader As SqlDataReader
        Dim oHashtable As New Hashtable
        Dim iCount As Long = 0

        Try

            'Executa Query
            ExecuteNonQuery(gDatabase, CommandType.StoredProcedure, "sp_service_dashboard")

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#Region " ::: OPERA :::"

    Private Sub LoadConfigInterfaceOpera()

        'Váriaveis Locais
        Dim oSqlDataReader As SqlDataReader

        Try

            'Executa Query
            oSqlDataReader = ExecuteReader(gDatabase, CommandType.StoredProcedure, "sp_select_configuracao_interface_opera_all")

            While oSqlDataReader.Read

                Dim oInfo As New interfaceOpera

                oInfo.codigoEmpresa = oSqlDataReader.Item("codigo_empresa")
                oInfo.hostname = oSqlDataReader.Item("hostname")
                oInfo.username = oSqlDataReader.Item("username")
                oInfo.password = oSqlDataReader.Item("password")
                oInfo.appKey = oSqlDataReader.Item("app_key")
                oInfo.clientID = oSqlDataReader.Item("client_id")
                oInfo.clientSecret = oSqlDataReader.Item("client_secret")
                oInfo.intervalo = oSqlDataReader.Item("intervalo")
                oInfo.lastUpdate = DateAdd(DateInterval.Minute, oSqlDataReader.Item("intervalo") * -1, Now())
                oInfo.hotelID = New List(Of String)
                oInfo.hotelID = LoadConfigInterfaceOperaHotels(iCodigoEmpresa:=oSqlDataReader.Item("codigo_empresa"))

                oInterfaceOpera.Add(oInfo)

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Private Sub LoadConfigInterfaceWish()

        'Váriaveis Locais
        Dim oSqlDataReader As SqlDataReader

        Try

            'Executa Query
            oSqlDataReader = ExecuteReader(gDatabase, CommandType.StoredProcedure, "sp_select_configuracao_interface_wish_all")

            While oSqlDataReader.Read

                Dim oInfo As New interfaceWish

                oInfo.codigoEmpresa = oSqlDataReader.Item("codigo_empresa")
                oInfo.hostname = oSqlDataReader.Item("hostname")
                oInfo.username = oSqlDataReader.Item("username")
                oInfo.password = oSqlDataReader.Item("password")
                oInfo.intervalo = oSqlDataReader.Item("intervalo")
                oInfo.lastUpdate = DateAdd(DateInterval.Minute, oSqlDataReader.Item("intervalo") * -1, Now())
                oInfo.hotelID = New List(Of String)
                oInfo.hotelID = LoadConfigInterfaceOperaHotels(iCodigoEmpresa:=oSqlDataReader.Item("codigo_empresa"))

                oInterfaceWish.Add(oInfo)

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Private Sub LoadConfigInterfaceOperaUpdate(ByRef oInterfaceOpera As interfaceOpera)

        'Váriaveis Locais
        Dim oSqlParameter(0) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = oInterfaceOpera.codigoEmpresa

            'Executa Query
            oSqlDataReader = ExecuteReader(gDatabase, CommandType.StoredProcedure, "sp_select_configuracao_interface_opera_empresa", oSqlParameter)

            While oSqlDataReader.Read

                oInterfaceOpera.codigoEmpresa = oSqlDataReader.Item("codigo_empresa")
                oInterfaceOpera.hostname = oSqlDataReader.Item("hostname")
                oInterfaceOpera.username = oSqlDataReader.Item("username")
                oInterfaceOpera.password = oSqlDataReader.Item("password")
                oInterfaceOpera.appKey = oSqlDataReader.Item("app_key")
                oInterfaceOpera.clientID = oSqlDataReader.Item("client_id")
                oInterfaceOpera.clientSecret = oSqlDataReader.Item("client_secret")
                oInterfaceOpera.intervalo = oSqlDataReader.Item("intervalo")
                oInterfaceOpera.lastUpdate = DateAdd(DateInterval.Minute, oSqlDataReader.Item("intervalo") * -1, Now())
                oInterfaceOpera.hotelID = New List(Of String)
                oInterfaceOpera.hotelID = LoadConfigInterfaceOperaHotels(iCodigoEmpresa:=oSqlDataReader.Item("codigo_empresa"))

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Private Function LoadConfigInterfaceWishHotels(ByVal iCodigoEmpresa As Integer) As List(Of String)

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
            oSqlDataReader = ExecuteReader(gDatabase, CommandType.StoredProcedure, "sp_select_configuracao_interface_wish_hotel", oSqlParameter)

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

    Private Function LoadConfigInterfaceOperaHotels(ByVal iCodigoEmpresa As Integer) As List(Of String)

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
            oSqlDataReader = ExecuteReader(gDatabase, CommandType.StoredProcedure, "sp_select_configuracao_interface_opera_hotel", oSqlParameter)

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

    Public Async Function AuthenticationOpera(ByVal sHostname As String,
                                              ByVal sUsername As String,
                                              ByVal sPassword As String,
                                              ByVal sClientID As String,
                                              ByVal sClientSecret As String,
                                              ByVal sAppKey As String) As Task(Of returnAuthenticationOpera)

        Using client As New HttpClient()

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

            Dim authenticationString = $"{sClientID}:{sClientSecret}"
            Dim base64String = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(authenticationString))

            Dim requestMessage = New HttpRequestMessage(HttpMethod.Post, sHostname & "/oauth/v1/tokens")
            requestMessage.Headers.Authorization = New AuthenticationHeaderValue("Basic", base64String)

            ' Define os dados que serão enviados no corpo da solicitação
            Dim dados = New Dictionary(Of String, String)() From {
                {"username", sUsername},
                {"password", sPassword},
                {"grant_type", "password"}
            }
            Dim conteudo = New FormUrlEncodedContent(dados)
            requestMessage.Headers.Add("x-app-key", sAppKey)
            requestMessage.Content = conteudo
            Dim result = Await client.SendAsync(requestMessage)

            If result.IsSuccessStatusCode Then
                ' Lida com a resposta da API de acordo com o conteúdo retornado
                Dim content = Await result.Content.ReadAsStringAsync()
                Dim resultado = JsonConvert.DeserializeObject(Of returnAuthenticationOpera)(content)
                Return resultado
            Else
                Return Nothing
            End If

        End Using

    End Function

    Public Async Function GetHotelsOpera(ByVal sHostname As String,
                                         ByVal sTokenType As String,
                                         ByVal sTokenAccess As String,
                                         ByVal sAppKey As String,
                                         ByVal iCodigoEmpresa As Integer,
                                         ByVal sHotelID As String) As Task(Of Boolean)

        Using client As New HttpClient()

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

            Dim offset = 0
            Dim totalResults = 0

            While offset <= totalResults

                Dim requestMessage As HttpRequestMessage

                requestMessage = New HttpRequestMessage(HttpMethod.Get, sHostname & "/hsk/v1/hotels/" + sHotelID + "/housekeepingOverview?limit=200&offset=" & offset.ToString())

                requestMessage.Headers.Authorization = New AuthenticationHeaderValue(sTokenType, sTokenAccess)
                requestMessage.Headers.Add("x-hotelid", sHotelID)
                requestMessage.Headers.Add("x-app-key", sAppKey)

                Dim result = Await client.SendAsync(requestMessage)

                If result.IsSuccessStatusCode Then

                    ' Lida com a resposta da API de acordo com o conteúdo retornado
                    Dim content = Await result.Content.ReadAsStringAsync()
                    Dim resultado = JsonConvert.DeserializeObject(Of Housekeepingroom)(content)

                    Call InterfaceHotelRoomsOpera(iCodigoEmpresa:=iCodigoEmpresa,
                                                  sHotelID:=sHotelID,
                                                  oHouseKeepingRooms:=resultado.housekeepingRoomInfo.housekeepingRooms)

                    totalResults = resultado.housekeepingRoomInfo.totalResults

                Else

                    Return False

                End If

                offset += 200

            End While

            Return True

        End Using

    End Function

    Public Async Function GetHotelsWish(ByVal sHostname As String,
                                        ByVal sUsername As String,
                                        ByVal sPassword As String,
                                        ByVal iCodigoEmpresa As Integer,
                                        ByVal sHotelID As String) As Task(Of Boolean)

        Using client As New HttpClient()

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

            Dim requestMessage As HttpRequestMessage

            requestMessage = New HttpRequestMessage(HttpMethod.Post, sHostname & "/busca-uh?uh=&idhotel=" + sHotelID)

            ' Adiciona a autenticação Basic no cabeçalho
            Dim byteArray As Byte() = Encoding.UTF8.GetBytes(sUsername & ":" & sPassword)

            Dim credentials As String = Convert.ToBase64String(Encoding.ASCII.GetBytes(sUsername & ":" & sPassword))

            requestMessage.Headers.Authorization = New AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray))

            Dim result = Await client.SendAsync(requestMessage)

            If result.IsSuccessStatusCode Then

                ' Lida com a resposta da API de acordo com o conteúdo retornado
                Dim content = Await result.Content.ReadAsStringAsync()
                Dim resultado = JsonConvert.DeserializeObject(Of WishUH)(content)

                Call InterfaceHotelRoomsWish(iCodigoEmpresa:=iCodigoEmpresa,
                                             sHotelID:=sHotelID,
                                             oWishUH:=resultado)

            Else

                Return False

            End If

            Return True

        End Using

    End Function

    Public Sub InterfaceHotelRoomsOpera(ByVal iCodigoEmpresa As Integer,
                                        ByVal sHotelID As String,
                                        ByVal oHouseKeepingRooms As Housekeepingrooms)

        'Variaveis Locais
        Dim oSqlParameter(6) As SqlParameter
        Dim i As Integer = 0

        Try

            For Each oRoom As Room In oHouseKeepingRooms.room

                i = 0

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

                'Seta Parametros - Room ID
                oSqlParameter(i) = New SqlParameter
                oSqlParameter(i).ParameterName = "room_id"
                oSqlParameter(i).Direction = ParameterDirection.Input
                oSqlParameter(i).SqlDbType = SqlDbType.VarChar
                oSqlParameter(i).Size = 20
                oSqlParameter(i).Value = oRoom.roomId : i += 1

                'Seta Parametros - Floor
                oSqlParameter(i) = New SqlParameter
                oSqlParameter(i).ParameterName = "floor"
                oSqlParameter(i).Direction = ParameterDirection.Input
                oSqlParameter(i).SqlDbType = SqlDbType.VarChar
                oSqlParameter(i).Size = 20
                oSqlParameter(i).Value = oRoom.floor : i += 1

                'Seta Parametros - Floor Description
                oSqlParameter(i) = New SqlParameter
                oSqlParameter(i).ParameterName = "floor_description"
                oSqlParameter(i).Direction = ParameterDirection.Input
                oSqlParameter(i).SqlDbType = SqlDbType.VarChar
                oSqlParameter(i).Size = 200
                oSqlParameter(i).Value = oRoom.floor : i += 1

                'Seta Parametros - Room Status
                oSqlParameter(i) = New SqlParameter
                oSqlParameter(i).ParameterName = "room_status"
                oSqlParameter(i).Direction = ParameterDirection.Input
                oSqlParameter(i).SqlDbType = SqlDbType.VarChar
                oSqlParameter(i).Size = 200
                oSqlParameter(i).Value = oRoom.housekeeping.housekeepingRoomStatus.housekeepingRoomStatus : i += 1

                'Seta Parametros - Front Office Status
                oSqlParameter(i) = New SqlParameter
                oSqlParameter(i).ParameterName = "front_office_status"
                oSqlParameter(i).Direction = ParameterDirection.Input
                oSqlParameter(i).SqlDbType = SqlDbType.VarChar
                oSqlParameter(i).Size = 200
                oSqlParameter(i).Value = oRoom.housekeeping.housekeepingRoomStatus.frontOfficeStatus

                'Executa Query
                ExecuteNonQuery(gDatabase, CommandType.StoredProcedure, "sp_interface_cadastro_basico_apartamento_opera", oSqlParameter)

            Next

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InterfaceHotelRoomsWish(ByVal iCodigoEmpresa As Integer,
                                       ByVal sHotelID As String,
                                       ByVal oWishUH As WishUH)

        'Variaveis Locais
        Dim oSqlParameter(6) As SqlParameter
        Dim i As Integer = 0

        Try

            For Each oRoom As WishUHInfo In oWishUH.row

                i = 0

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

                'Seta Parametros - Room ID
                oSqlParameter(i) = New SqlParameter
                oSqlParameter(i).ParameterName = "room_id"
                oSqlParameter(i).Direction = ParameterDirection.Input
                oSqlParameter(i).SqlDbType = SqlDbType.VarChar
                oSqlParameter(i).Size = 20
                oSqlParameter(i).Value = oRoom.UH : i += 1

                'Seta Parametros - Floor
                oSqlParameter(i) = New SqlParameter
                oSqlParameter(i).ParameterName = "floor"
                oSqlParameter(i).Direction = ParameterDirection.Input
                oSqlParameter(i).SqlDbType = SqlDbType.VarChar
                oSqlParameter(i).Size = 20
                oSqlParameter(i).Value = "" : i += 1

                'Seta Parametros - Floor Description
                oSqlParameter(i) = New SqlParameter
                oSqlParameter(i).ParameterName = "floor_description"
                oSqlParameter(i).Direction = ParameterDirection.Input
                oSqlParameter(i).SqlDbType = SqlDbType.VarChar
                oSqlParameter(i).Size = 200
                oSqlParameter(i).Value = "" : i += 1

                'Seta Parametros - Room Status
                oSqlParameter(i) = New SqlParameter
                oSqlParameter(i).ParameterName = "room_status"
                oSqlParameter(i).Direction = ParameterDirection.Input
                oSqlParameter(i).SqlDbType = SqlDbType.VarChar
                oSqlParameter(i).Size = 200
                oSqlParameter(i).Value = oRoom.STATUSDAGOV : i += 1

                'Seta Parametros - Front Office Status
                oSqlParameter(i) = New SqlParameter
                oSqlParameter(i).ParameterName = "front_office_status"
                oSqlParameter(i).Direction = ParameterDirection.Input
                oSqlParameter(i).SqlDbType = SqlDbType.VarChar
                oSqlParameter(i).Size = 200
                oSqlParameter(i).Value = oRoom.STATUSDAUH

                'Executa Query
                ExecuteNonQuery(gDatabase, CommandType.StoredProcedure, "sp_interface_cadastro_basico_apartamento_opera", oSqlParameter)

            Next

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region

End Class
