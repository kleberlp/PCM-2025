Imports System.Data.SqlClient
Imports System.Drawing
Imports System.IO
Imports System.Net
Imports System.Threading
Imports PCM.SERVICE.TAG.SQLHelper

Public Class Main

    'Váriaveis
    Private oThread As Thread
    Private lastUpdateTAG As Date

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

                    'Rotina
                    Try
                        Call UpdateTAGApartamento()
                    Catch ex As Exception
                        WriteLog("UPDATE TAG APARTAMENTO: " & ex.Message)
                    End Try

                    lastUpdateTAG = Now()

                End If

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

    Private Sub UpdateTAGApartamento()

        'Váriaveis Locais
        Dim oHashtable As New Hashtable
        Dim iCount As Long = 0

        Try

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(gDatabase, CommandType.StoredProcedure, "sp_select_pcm_os_hospede_apartamento_tag")

                While oSqlDataReader.Read

                    oHashtable.Add(oSqlDataReader.Item("uniqueId"), oSqlDataReader.Item("code"))
                    iCount += 1

                End While

            End Using

            For Each uniqueId As String In oHashtable.Keys

                Dim sURL As String = oHashtable.Item(uniqueId)
                Dim oImage As Drawing.Image = Nothing

                If Not String.IsNullOrWhiteSpace(sURL) Then

                    Dim oWebRequest As WebRequest = WebRequest.Create(sURL)

                    Using oWebResponse As WebResponse = oWebRequest.GetResponse
                        Using oStream As Stream = oWebResponse.GetResponseStream
                            oImage = New Bitmap(Drawing.Image.FromStream(oStream))
                        End Using
                    End Using
                End If

                'Carrega Imagem
                Dim oMemoryStream As MemoryStream = New MemoryStream()
                oImage.Save(oMemoryStream, Imaging.ImageFormat.Jpeg)
                Dim bytBLOBData(oMemoryStream.Length - 1) As Byte
                oMemoryStream.Position = 0
                oMemoryStream.Read(bytBLOBData, 0, oMemoryStream.Length)

                'Variaveis Locais
                Dim oSqlParameter As SqlParameter() = {
                    CriarParametro("uniqueId", SqlDbType.VarChar, uniqueId),
                    CriarParametro("tag", SqlDbType.Image, bytBLOBData)
                }

                'Executa Query
                ExecuteNonQuery(gDatabase, CommandType.StoredProcedure, "sp_update_pcm_os_hospede_apartamento_tag", oSqlParameter)

            Next

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

End Class
 