Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Security.Authentication
Imports System.Security.Policy
Imports System.Text
Imports System.Text.Json.Nodes
Imports Newtonsoft.Json
Imports OfficeOpenXml.FormulaParsing.Excel.Functions.Numeric
Imports Oracle.ManagedDataAccess.Client
Imports PCM.WEB.DAL.SQLHelper
Imports PCM.WEB.MODELS

Public Class Api

    Private sConnection As String
    Private sConnectionIntercity As String

    Sub New(ByVal sCon As String, ByVal sConIntercity As String)
        sConnection = sCon
        sConnectionIntercity = sConIntercity
    End Sub

#Region "::: LOG :::"

    Public Sub insertLogAPI(ByVal iCodigoEmpresa As Integer,
                            ByVal iCodigoUnidade As Integer,
                            ByVal iCodigoUsuario As Integer,
                            ByVal sEndpoint As String,
                            ByVal sRequestBody As String,
                            ByVal sResponseBody As String)

        'Variaveis Locais
        Dim oSqlParameter(5) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Endpoint
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "endpoint"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = sEndpoint : i += 1

            'Seta Parametros - Request Body
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "request_body"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Value = IIf(sRequestBody = "", DBNull.Value, sRequestBody) : i += 1

            'Seta Parametros - Response Body
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "response_body"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Value = IIf(sResponseBody = "", DBNull.Value, sResponseBody)

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_pwa_insert_log_api", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region

#Region "::: FIREBASE :::"

    Public Function insertFirebaseToken(ByVal iCodigoEmpresa As Integer,
                                        ByVal iCodigoUnidade As Integer,
                                        ByVal iCodigoUsuario As Integer,
                                        ByVal sToken As String) As pwaDefaultResponse

        'Variaveis Locais
        Dim oSqlParameter(3) As SqlParameter
        Dim i As Integer = 0
        Dim oReturn As New pwaDefaultResponse

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Token
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "token"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = sToken

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_pwa_insert_firebase_token", oSqlParameter)

            oReturn.success = True
            oReturn.message = ""

        Catch SqlEx As SqlException
            oReturn.success = False
            oReturn.message = SqlEx.Message
        Catch ex As Exception
            oReturn.success = False
            oReturn.message = ex.Message
        End Try

        Return oReturn

    End Function

#End Region

#Region "::: COMBO :::"

    Public Function Combo(ByVal iCodigoEmpresa As Integer,
                          ByVal iCodigoUnidade As Integer,
                          ByVal sTabela As String,
                          ByVal sCodigoAux1 As String,
                          ByVal sCodigoAux2 As String) As List(Of pwaCombo)

        'Variaveis Locais
        Dim oSqlParameter(4) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New List(Of pwaCombo)
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Tabela
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "tabela"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sTabela : i += 1

            'Seta Parametros - Código Aux 1
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_aux_1"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sCodigoAux1 : i += 1

            'Seta Parametros - Código Aux 2
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_aux_2"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sCodigoAux2

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_pwa_select_combo", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New pwaCombo

                oInfo.descricao = oSqlDataReader.Item("descricao")
                oInfo.codigo = oSqlDataReader.Item("codigo")

                oReturn.Add(oInfo)
            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : 
            oSqlDataReader = Nothing

            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: PICTURE :::"

    Public Function insertFile(ByVal iCodigoEmpresa As Integer,
                               ByVal iCodigoUnidade As Integer,
                               ByVal iCodigoUsuario As Integer,
                               ByVal lCodigoDocumento As Long,
                               ByVal sTipo As String,
                               ByVal oArquivo As pwaArquivoInsert) As pwaDefaultResponse

        'Variaveis Locais
        Dim oSqlParameter(8) As SqlParameter
        Dim i As Integer = 0
        Dim oReturn As New pwaDefaultResponse()
        Dim sImagePath As String = ""

        Try

            'Verifica se carregou Imagem
            If (oArquivo.arquivo <> "") Then

                'Váriaveis Locais
                Dim sEmpresa As String = iCodigoEmpresa.ToString
                Dim sSemana As String = DatePart(DateInterval.WeekOfYear, Now())
                Dim sYear As String = DatePart(DateInterval.Year, Now())
                Dim sDataHora As String = Format(Now(), "dd_HHmmss")
                Dim sPath As String = Path.Combine("C:\SIM\PCM\SITE\IMAGE\", sTipo, sEmpresa, iCodigoUnidade.ToString(), lCodigoDocumento.ToString())
                Dim sImageName As String = "TMP_" & Format(Now().ToString("yyyyMMdd_HHmmssfff")) & ".png"

                'Verifica se o diretório existe
                If (Directory.Exists(sPath) = False) Then
                    'Cria Diretório
                    Directory.CreateDirectory(sPath)
                End If

                'Salva Imagem
                sImagePath = Path.Combine(sPath, sImageName)
                Dim imageBytes As Byte() = Convert.FromBase64String(oArquivo.arquivo)
                File.WriteAllBytes(sImagePath, imageBytes)
                Dim imagem As New Bitmap(sImagePath)

                Dim dFator As Double = IIf(imagem.Width > imagem.Height, 400.0 / imagem.Width, 400.0 / imagem.Height)


                Dim imagemNew As New Bitmap(imagem, Math.Ceiling(imagem.Width * dFator), Math.Ceiling(imagem.Height * dFator))
                imagemNew.Save(Path.Combine(sPath, sImageName.Replace("TMP_", "")))
                imagemNew.Dispose()

                Try
                    File.Delete(sImagePath)
                Catch ex As Exception

                End Try

                sImagePath = Path.Combine(sPath, sImageName.Replace("TMP_", ""))

            End If

            'Seta Parametros - Código Documento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_documento"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoDocumento : i += 1

            'Seta Parametros - Tipo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "tipo"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sTipo : i += 1

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código Checklist
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_checklist"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oArquivo.codigoChecklist : i += 1

            'Seta Parametros - Código Item Checklist
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_item_checklist"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oArquivo.codigoItemChecklist : i += 1

            'Seta Parametros - Imagem
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "arquivo"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = IIf(sImagePath = "", DBNull.Value, sImagePath)

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_pwa_insert_pcm_picture", oSqlParameter)

            oReturn.success = True
            oReturn.message = ""

        Catch SqlEx As SqlException
            oReturn.success = False
            oReturn.message = SqlEx.Message.ToString()
        Catch ex As Exception
            oReturn.success = False
            oReturn.message = ex.Message.ToString()
        End Try

        Return oReturn

    End Function

    Public Function insertFileGuid(ByVal iCodigoEmpresa As Integer,
                                   ByVal iCodigoUnidade As Integer,
                                   ByVal iCodigoUsuario As Integer,
                                   ByVal lCodigoDocumento As Long,
                                   ByVal sTipo As String,
                                   ByVal oArquivo As pwaArquivoInsert) As pwaDefaultResponse

        'Variaveis Locais
        Dim oReturn As New pwaDefaultResponse()
        Dim sImagePath As String = ""

        Try

            'Verifica se carregou Imagem
            If (oArquivo.arquivo <> "") Then

                'Váriaveis Locais
                Dim sEmpresa As String = iCodigoEmpresa.ToString
                Dim sPath As String = Path.Combine("C:\SIM\PCM\SITE\IMAGE\", sTipo, sEmpresa, iCodigoUnidade.ToString(), lCodigoDocumento.ToString())
                Dim sImageName As String = oArquivo.guid & ".png"

                'Verifica se o diretório existe
                If (Directory.Exists(sPath) = False) Then
                    'Cria Diretório
                    Directory.CreateDirectory(sPath)
                End If

                'Salva Imagem
                sImagePath = Path.Combine(sPath, sImageName)
                Dim imageBytes As Byte() = Convert.FromBase64String(oArquivo.arquivo)
                File.WriteAllBytes(sImagePath, imageBytes)
                Dim imagem As New Bitmap(sImagePath)

                Dim dFator As Double = IIf(imagem.Width > imagem.Height, 400.0 / imagem.Width, 400.0 / imagem.Height)


                Dim imagemNew As New Bitmap(imagem, Math.Ceiling(imagem.Width * dFator), Math.Ceiling(imagem.Height * dFator))
                imagem.Dispose()
                imagemNew.Save(Path.Combine(sPath, sImageName))
                imagemNew.Dispose()

                sImagePath = Path.Combine(sPath, sImageName)

            End If


            'Variaveis Locais
            Dim oSqlParameter As SqlParameter() = {
                    CriarParametro("codigo_documento", SqlDbType.BigInt, lCodigoDocumento),
                    CriarParametro("tipo", SqlDbType.VarChar, sTipo),
                    CriarParametro("codigo_empresa", SqlDbType.SmallInt, iCodigoEmpresa),
                    CriarParametro("codigo_unidade", SqlDbType.Int, iCodigoUnidade),
                    CriarParametro("codigo_usuario", SqlDbType.Int, iCodigoUsuario),
                    CriarParametro("codigo_checklist", SqlDbType.Int, oArquivo.codigoChecklist),
                    CriarParametro("codigo_item_checklist", SqlDbType.Int, oArquivo.codigoItemChecklist),
                    CriarParametro("arquivo", SqlDbType.VarChar, IIf(sImagePath = "", DBNull.Value, sImagePath)),
                    CriarParametro("guid", SqlDbType.VarChar, oArquivo.guid)
                }


            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_pwa_insert_pcm_picture_guid_error", oSqlParameter)

            oReturn.success = True
            oReturn.message = ""

        Catch SqlEx As SqlException
            oReturn.success = False
            oReturn.message = SqlEx.Message.ToString()
        Catch ex As Exception
            oReturn.success = False
            oReturn.message = ex.Message.ToString()
        End Try

        Return oReturn

    End Function

    Public Function getFile(ByVal iCodigoEmpresa As Integer,
                            ByVal iCodigoUnidade As Integer,
                            ByVal lCodigoDocumento As Long,
                            ByVal sTipo As String) As List(Of pwaImagem)

        'Variaveis Locais
        Dim oSqlParameter(3) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim i As Integer = 0
        Dim oReturn As New List(Of pwaImagem)

        Try

            'Seta Parametros - Código Documento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_documento"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoDocumento : i += 1

            'Seta Parametros - Tipo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "tipo"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sTipo : i += 1

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_pwa_select_pcm_picture", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New pwaImagem

                oInfo.url = oSqlDataReader.Item("url")
                oInfo.extensao = oSqlDataReader.Item("extensao")

                oReturn.Add(oInfo)
            End While

            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : 
            oSqlDataReader = Nothing

            Return oReturn

        Catch SqlEx As SqlException
            Return Nothing
        Catch ex As Exception
            Return Nothing
        End Try

    End Function

#End Region

#Region "::: LOGIN :::"

    Public Function Login(ByVal sEmail As String,
                          ByVal sSenha As String) As pwaLogin

        'Variaveis Locais
        Dim oSqlParameter(1) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New pwaLogin
        Dim i As Integer = 0

        Try

            'Seta Parametros - E-mail
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "email"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 150
            oSqlParameter(i).Value = sEmail : i += 1

            'Seta Parametros - Senha
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "senha"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = Cripitografar(sSenha.ToUpper)

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_pwa_valida_login", oSqlParameter)

            While oSqlDataReader.Read

                If oSqlDataReader.Item("success") Then

                    oReturn.codigoEmpresa = oSqlDataReader.Item("codigo_empresa")
                    oReturn.empresa = oSqlDataReader.Item("empresa")
                    oReturn.unidade = oSqlDataReader.Item("unidade")
                    oReturn.codigoUnidade = oSqlDataReader.Item("codigo_unidade")
                    oReturn.codigoUsuario = oSqlDataReader.Item("codigo_usuario")
                    oReturn.codigoFuncionario = oSqlDataReader.Item("codigo_funcionario")
                    oReturn.nome = oSqlDataReader.Item("nome")
                    oReturn.ativo = IIf(oSqlDataReader.Item("ativo"), 1, 0)
                    oReturn.apontamento = IIf(oSqlDataReader.Item("apontamento"), 1, 0)
                    oReturn.marca = Marcas(iCodigoEmpresa:=oSqlDataReader.Item("codigo_empresa"),
                                           iCodigoUsuario:=oSqlDataReader.Item("codigo_unidade"))
                    oReturn.localizacao = Localizacao(iCodigoEmpresa:=oSqlDataReader.Item("codigo_empresa"),
                                                      iCodigoUsuario:=oSqlDataReader.Item("codigo_usuario"))
                    oReturn.form = Form(iCodigoEmpresa:=oSqlDataReader.Item("codigo_empresa"),
                                        iCodigoUnidade:=oSqlDataReader.Item("codigo_unidade"),
                                        iCodigoUsuario:=oSqlDataReader.Item("codigo_usuario"))
                    oReturn.unidades = Unidades(iCodigoEmpresa:=oSqlDataReader.Item("codigo_empresa"),
                                                iCodigoUsuario:=oSqlDataReader.Item("codigo_usuario"))
                    oReturn.statusGovernanca = StatusGovernanca(iCodigoEmpresa:=oSqlDataReader.Item("codigo_empresa"),
                                                                iCodigoUnidade:=oSqlDataReader.Item("codigo_unidade"))
                    oReturn.success = oSqlDataReader.Item("success")
                    oReturn.message = oSqlDataReader.Item("message")

                Else

                    oReturn.success = oSqlDataReader.Item("success")
                    oReturn.message = oSqlDataReader.Item("message")

                End If

            End While

            oSqlDataReader.NextResult()

            oReturn.endpoint = New List(Of pwaEndpoint)

            While oSqlDataReader.Read

                Dim oInfo As New pwaEndpoint
                oInfo.url = SafeGetString(oSqlDataReader, "url")
                oInfo.descricao = SafeGetString(oSqlDataReader, "descricao")

                oReturn.endpoint.Add(oInfo)

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function Form(ByVal iCodigoEmpresa As Integer,
                         ByVal iCodigoUnidade As Integer,
                         ByVal iCodigoUsuario As Integer) As List(Of pwaForm)

        'Variaveis Locais
        Dim oSqlParameter(2) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New List(Of pwaForm)
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_pwa_select_login_form", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New pwaForm

                oInfo.ordem = oSqlDataReader.Item("ordem")
                oInfo.tag = oSqlDataReader.Item("tag")
                oInfo.descricao = oSqlDataReader.Item("descricao")
                oInfo.cssClass = oSqlDataReader.Item("css_class")
                oInfo.totalOk = oSqlDataReader.Item("total_ok")
                oInfo.total = oSqlDataReader.Item("total")

                'Seta Retorno da Função
                oReturn.Add(oInfo)

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function StatusGovernanca(ByVal iCodigoEmpresa As Integer,
                                     ByVal iCodigoUnidade As Integer) As List(Of pwaStatus)

        'Variaveis Locais
        Dim oSqlParameter(1) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New List(Of pwaStatus)
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_pwa_select_login_status_governanca", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New pwaStatus

                oInfo.codigo = oSqlDataReader.Item("codigo")
                oInfo.descricao = oSqlDataReader.Item("descricao")
                oInfo.cssClass = oSqlDataReader.Item("css_class")

                'Seta Retorno da Função
                oReturn.Add(oInfo)

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function DashboardOrdemServico(ByVal iCodigoEmpresa As Integer,
                                          ByVal iCodigoUnidade As Integer,
                                          ByVal iCodigoUsuario As Integer) As pwaDashboardOrdemServico

        'Variaveis Locais
        Dim oSqlParameter(2) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New pwaDashboardOrdemServico
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_pwa_select_dashboard_ordem_servico", oSqlParameter)

            While oSqlDataReader.Read

                oReturn.atrasado = oSqlDataReader.Item("atrasado")
                oReturn.pendente = oSqlDataReader.Item("pendente")
                oReturn.vinculado = oSqlDataReader.Item("vinculado")
                oReturn.emAndamento = oSqlDataReader.Item("andamento")

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function Unidades(ByVal iCodigoEmpresa As Integer,
                             ByVal iCodigoUsuario As Integer) As List(Of pwaUnidades)

        'Variaveis Locais
        Dim oSqlParameter(1) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New List(Of pwaUnidades)
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_pwa_select_login_unidades", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New pwaUnidades

                oInfo.codigo = oSqlDataReader.Item("codigo")
                oInfo.descricao = oSqlDataReader.Item("descricao")
                oInfo.marca = oSqlDataReader.Item("marca")
                oInfo.uf = oSqlDataReader.Item("uf")
                oInfo.municipio = oSqlDataReader.Item("municipio")
                oInfo.urlLogo = oSqlDataReader.Item("url_logo")

                'Seta Retorno da Função
                oReturn.Add(oInfo)

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function Marcas(ByVal iCodigoEmpresa As Integer,
                           ByVal iCodigoUsuario As Integer) As List(Of String)

        'Variaveis Locais
        Dim oSqlParameter(1) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New List(Of String)
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_pwa_select_login_marcas", oSqlParameter)

            While oSqlDataReader.Read

                'Seta Retorno da Função
                oReturn.Add(oSqlDataReader.Item("marca"))

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function Localizacao(ByVal iCodigoEmpresa As Integer,
                                ByVal iCodigoUsuario As Integer) As List(Of pwaLocalizacao)

        'Variaveis Locais
        Dim oSqlParameter(1) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New List(Of pwaLocalizacao)
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_pwa_select_login_localizacao_uf", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New pwaLocalizacao

                oInfo.uf = oSqlDataReader.Item("uf")
                oInfo.municipio = LocalizacaoMunicipio(iCodigoEmpresa:=iCodigoEmpresa,
                                                       iCodigoUsuario:=iCodigoUsuario,
                                                       sUF:=oSqlDataReader.Item("uf"))

                'Seta Retorno da Função
                oReturn.Add(oInfo)

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function LocalizacaoMunicipio(ByVal iCodigoEmpresa As Integer,
                                         ByVal iCodigoUsuario As Integer,
                                         ByVal sUF As String) As List(Of String)

        'Variaveis Locais
        Dim oSqlParameter(2) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New List(Of String)
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - UF
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "uf"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 2
            oSqlParameter(i).Value = sUF

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_pwa_select_login_localizacao_municipio", oSqlParameter)

            While oSqlDataReader.Read

                'Seta Retorno da Função
                oReturn.Add(oSqlDataReader.Item("municipio"))

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function UpdatePassword(ByVal sEmail As String,
                                   ByVal sSenha As String) As pwaDefaultResponse

        'Variaveis Locais
        Dim oSqlParameter(1) As SqlParameter
        Dim oReturn As New pwaDefaultResponse
        Dim i As Integer = 0

        Try

            'Seta Parametros - E-mail
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "email"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 150
            oSqlParameter(i).Value = sEmail : i += 1

            'Seta Parametros - Senha
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "senha"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = Cripitografar(sSenha.ToUpper)

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_pwa_update_password", oSqlParameter)

            oReturn.success = True
            oReturn.message = "Senha atualizada com sucesso!"

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: PCM ORDEM SERVIÇO :::"

    Public Function getListOrdemServicoList(ByVal iCodigoEmpresa As Integer,
                                            ByVal iCodigoUnidade As Integer,
                                            ByVal iCodigoUsuario As Integer,
                                            ByVal iCodigoFuncionario As Integer,
                                            ByVal iPage As Integer) As pwaOrdemServicoList

        'Variaveis Locais
        Dim oSqlParameter(4) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New pwaOrdemServicoList
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código Funcionário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_funcionario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoFuncionario : i += 1

            'Seta Parametros - Código Funcionário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "page"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iPage

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_pwa_select_ordem_servico_list", oSqlParameter)

            While oSqlDataReader.Read

                oReturn.totalPages = oSqlDataReader.Item("total_pages")
                oReturn.totalResults = oSqlDataReader.Item("total_results")
                oReturn.page = oSqlDataReader.Item("page")
                oReturn.results = getListOrdemServico(iCodigoEmpresa:=iCodigoEmpresa,
                                                      iCodigoUnidade:=iCodigoUnidade,
                                                      iCodigoUsuario:=iCodigoUsuario,
                                                      iCodigoFuncionario:=iCodigoFuncionario,
                                                      iPage:=iPage)
                oReturn.status = getListStatusOrdemServico()
                oReturn.statusHotel = getListStatusHotel(iCodigoEmpresa:=iCodigoEmpresa,
                                                         iCodigoUnidade:=iCodigoUnidade)
                oReturn.prioridade = getListPrioridade(iCodigoEmpresa:=iCodigoEmpresa,
                                                       iCodigoUnidade:=iCodigoUnidade)

            End While


            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function getListStatusOrdemServico() As List(Of pwaStatus)

        'Variaveis Locais
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New List(Of pwaStatus)

        Try

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_pwa_select_static_status_ordem_servico")

            While oSqlDataReader.Read

                Dim oInfo As New pwaStatus

                oInfo.codigo = oSqlDataReader.Item("codigo")
                oInfo.descricao = oSqlDataReader.Item("descricao")
                oInfo.cssClass = oSqlDataReader.Item("css_class")

                oReturn.Add(oInfo)

            End While


            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function getListStatusHotel(ByVal iCodigoEmpresa As Integer,
                                       ByVal iCodigoUnidade As Integer) As List(Of pwaStatus)

        'Variaveis Locais
        Dim oSqlParameter(1) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New List(Of pwaStatus)
        Dim i As Integer

        Try


            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_pwa_select_static_status_hotel", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New pwaStatus

                oInfo.codigo = oSqlDataReader.Item("codigo")
                oInfo.descricao = oSqlDataReader.Item("descricao")
                oInfo.cssClass = oSqlDataReader.Item("css_class")

                oReturn.Add(oInfo)

            End While


            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function getListPrioridade(ByVal iCodigoEmpresa As Integer,
                                      ByVal iCodigoUnidade As Integer) As List(Of pwaLista)

        'Variaveis Locais
        Dim oSqlParameter(1) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New List(Of pwaLista)
        Dim i As Integer

        Try


            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_pwa_select_static_prioridade", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New pwaLista

                oInfo.codigo = oSqlDataReader.Item("codigo")
                oInfo.descricao = oSqlDataReader.Item("descricao")
                oInfo.cssClass = oSqlDataReader.Item("css_class")

                oReturn.Add(oInfo)

            End While


            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function getListOrdemServico(ByVal iCodigoEmpresa As Integer,
                                        ByVal iCodigoUnidade As Integer,
                                        ByVal iCodigoUsuario As Integer,
                                        ByVal iCodigoFuncionario As Integer,
                                        ByVal iPage As Integer) As List(Of pwaOrdemServico)

        'Variaveis Locais
        Dim oSqlParameter(4) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New List(Of pwaOrdemServico)
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código Funcionário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_funcionario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoFuncionario : i += 1

            'Seta Parametros - Código Funcionário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "page"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iPage

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_pwa_select_ordem_servico", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New pwaOrdemServico

                oInfo.sequencia = oSqlDataReader.Item("sequencia")
                oInfo.codigoOrdemServico = oSqlDataReader.Item("codigo_ordem_servico")
                oInfo.ordemServico = oSqlDataReader.Item("ordem_servico")
                oInfo.descricao = oSqlDataReader.Item("descricao")
                oInfo.dataAbertura = oSqlDataReader.Item("data_abertura")
                oInfo.dataNecessidade = oSqlDataReader.Item("data_necessidade")
                oInfo.setor = oSqlDataReader.Item("setor")
                oInfo.local = oSqlDataReader.Item("local")
                oInfo.solicitante = oSqlDataReader.Item("solicitante")
                oInfo.prioridade = oSqlDataReader.Item("prioridade")
                oInfo.codigoEquipamento = oSqlDataReader.Item("codigo_equipamento")
                oInfo.equipamento = oSqlDataReader.Item("equipamento")
                oInfo.vinculado = oSqlDataReader.Item("vinculado")
                oInfo.codigoSetor = oSqlDataReader.Item("codigo_setor")
                oInfo.codigoLocal = oSqlDataReader.Item("codigo_local")
                oInfo.codigoPrioridade = oSqlDataReader.Item("codigo_prioridade")

                'Status
                oInfo.status = New pwaStatus
                oInfo.status.codigo = oSqlDataReader.Item("status_codigo")
                oInfo.status.descricao = oSqlDataReader.Item("status_descricao")
                oInfo.status.cssClass = oSqlDataReader.Item("status_css_class")

                'Status - Opera
                oInfo.statusOpera = New pwaStatus
                oInfo.statusOpera.codigo = oSqlDataReader.Item("status_opera_codigo")
                oInfo.statusOpera.descricao = oSqlDataReader.Item("status_opera_descricao")
                oInfo.statusOpera.cssClass = oSqlDataReader.Item("status_opera_css_class")

                oInfo.arquivo = getFile(iCodigoEmpresa:=iCodigoEmpresa,
                                        iCodigoUnidade:=iCodigoUnidade,
                                        lCodigoDocumento:=oSqlDataReader.Item("codigo_ordem_servico"),
                                        sTipo:="ORDEM SERVIÇO")

                oReturn.Add(oInfo)

            End While


            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function insertOrdemServico(ByVal oOrdemServico As pwaOrdemServicoInsert) As pwaOrdemServicoResponse

        'Variaveis Locais
        Dim oSqlParameter(11) As SqlParameter
        Dim oReturn As New pwaOrdemServicoResponse
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = oOrdemServico.codigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oOrdemServico.codigoUnidade : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oOrdemServico.codigoUsuario : i += 1

            'Seta Parametros - Código Setor
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_setor"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(oOrdemServico.codigoSetor = -1, DBNull.Value, oOrdemServico.codigoSetor) : i += 1

            'Seta Parametros - Código Local
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_local"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(oOrdemServico.codigoLocal = -1, DBNull.Value, oOrdemServico.codigoLocal) : i += 1

            'Seta Parametros - Código Equipamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_equipamento"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = IIf(oOrdemServico.codigoEquipamento = -1, DBNull.Value, oOrdemServico.codigoEquipamento) : i += 1

            'Seta Parametros - Código Prioridade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_prioridade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oOrdemServico.codigoPrioridade : i += 1

            'Seta Parametros - Data Necessidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "data_necessidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = oOrdemServico.dataNecessidade : i += 1

            'Seta Parametros - Data
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = oOrdemServico.descricao : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Output
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt : i += 1

            'Seta Parametros - Ordem de Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Output
            oSqlParameter(i).ParameterName = "ordem_servico"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_pwa_insert_ordem_servico", oSqlParameter)

            oReturn.success = True
            oReturn.message = ""
            oReturn.codigo = oSqlParameter(i - 1).Value
            oReturn.ordemServico = oSqlParameter(i).Value

        Catch SqlEx As SqlException
            oReturn.success = False
            oReturn.message = SqlEx.Message.ToString()
        Catch ex As Exception
            oReturn.success = False
            oReturn.message = ex.Message.ToString()
        End Try

        Return oReturn

    End Function

    Public Function insertOrdemServicoApontamento(ByVal oOrdemServicoApontamento As pwaOrdemServicoApontamento) As pwaDefaultResponse

        'Variaveis Locais
        Dim oSqlParameter(10) As SqlParameter
        Dim oReturn As New pwaDefaultResponse
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = oOrdemServicoApontamento.codigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oOrdemServicoApontamento.codigoUnidade : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oOrdemServicoApontamento.codigoUsuario : i += 1

            'Seta Parametros - Código Funcionário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_funcionario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oOrdemServicoApontamento.codigoFuncionario : i += 1

            'Seta Parametros - Código Categoria
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_categoria"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oOrdemServicoApontamento.codigoCategoria : i += 1

            'Seta Parametros - Código PCM Ordem Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_pcm_ordem_servico"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = oOrdemServicoApontamento.codigoOrdemServico : i += 1

            'Seta Parametros - Data Início
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "data_inicio"
            oSqlParameter(i).SqlDbType = SqlDbType.DateTime
            oSqlParameter(i).Value = oOrdemServicoApontamento.dataInicio : i += 1

            'Seta Parametros - Data Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "data_termino"
            oSqlParameter(i).SqlDbType = SqlDbType.DateTime
            oSqlParameter(i).Value = oOrdemServicoApontamento.dataTermino : i += 1

            'Seta Parametros - Solução
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "solucao"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 250
            oSqlParameter(i).Value = oOrdemServicoApontamento.solucao : i += 1

            'Seta Parametros - Concluído
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "concluido"
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = oOrdemServicoApontamento.concluido : i += 1

            'Seta Parametros - Código Justificativa Apontamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_justificativa_apontamento"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oOrdemServicoApontamento.codigoJustificativa

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_pwa_insert_pcm_ordem_servico_apontamento", oSqlParameter)

            oReturn.success = True
            oReturn.message = "Apontamento concluído com sucesso!"

        Catch SqlEx As SqlException
            oReturn.success = False
            oReturn.message = SqlEx.Message.ToString()
        Catch ex As Exception
            oReturn.success = False
            oReturn.message = ex.Message.ToString()
        End Try

        Return oReturn

    End Function

    Public Sub ordemServicoStatus(ByVal iCodigoEmpresa As Integer,
                                  ByVal iCodigoUnidade As Integer,
                                  ByVal lCodigoPCMOrdemServico As Long,
                                  ByVal iCodigoUsuario As Integer,
                                  ByVal iStatus As Integer)

        'Variaveis Locais
        Dim oSqlParameter(4) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código PCM Ordem Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_pcm_ordem_servico"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoPCMOrdemServico : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Status
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "status"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iStatus

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_pwa_update_ordem_servico_status", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function getListEstoque(ByVal codigoEmpresa As Integer,
                                   ByVal codigoUnidade As Integer,
                                   ByVal codigoUsuario As Integer,
                                   ByVal page As Integer) As pwaEstoqueList

        'Váriavies Locais
        Dim oReturn As New pwaEstoqueList
        Dim oResult As New List(Of pwaEstoque)

        Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("codigo_usuario", SqlDbType.Int, codigoUsuario),
                CriarParametro("page", SqlDbType.Int, page)
            }

        Try

            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_pwa_select_estoque_list", oSqlParameter)

                While oSqlDataReader.Read

                    oReturn.totalPages = oSqlDataReader.Item("total_pages")
                    oReturn.totalResults = oSqlDataReader.Item("total_results")
                    oReturn.page = oSqlDataReader.Item("page")

                End While

                oSqlDataReader.NextResult()


                While oSqlDataReader.Read

                    Dim oInfo As New pwaEstoque With {
                        .sequencia = SafeGetLong(oSqlDataReader, "sequencia"),
                        .codigoProduto = SafeGetLong(oSqlDataReader, "codigo_produto"),
                        .produto = SafeGetLong(oSqlDataReader, "produto"),
                        .descricao = SafeGetLong(oSqlDataReader, "descricao"),
                        .quantidade = SafeGetLong(oSqlDataReader, "quantidade"),
                        .saldoEstoque = SafeGetLong(oSqlDataReader, "saldo")
                    }

                    oResult.Add(oInfo)

                End While

            End Using

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: PCM - MANUTENÇÃO PROGRAMADA :::"

    Public Function getListManutencaoProgramadaList(ByVal iCodigoEmpresa As Integer,
                                                    ByVal iCodigoUsuario As Integer,
                                                    ByVal iCodigoUnidade As Integer,
                                                    ByVal sTipo As String,
                                                    ByVal bOffline As Boolean,
                                                    ByVal iPage As Integer) As pwaManutencaoProgramadaList

        'Variaveis Locais
        Dim oSqlParameter(4) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New pwaManutencaoProgramadaList
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Tipo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "tipo"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sTipo : i += 1

            'Seta Parametros - Página
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "page"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iPage

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_pwa_select_programada_list", oSqlParameter)

            While oSqlDataReader.Read

                oReturn.totalPages = oSqlDataReader.Item("total_pages")
                oReturn.totalResults = oSqlDataReader.Item("total_results")
                oReturn.page = oSqlDataReader.Item("page")
                oReturn.results = getListManutencaoProgramada(iCodigoEmpresa:=iCodigoEmpresa,
                                                              iCodigoUnidade:=oSqlDataReader.Item("codigo_unidade"),
                                                              iCodigoUsuario:=iCodigoUsuario,
                                                              sTipo:=sTipo,
                                                              bOffline:=bOffline,
                                                              iPage:=iPage)

            End While


            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function getListManutencaoProgramada(ByVal iCodigoEmpresa As Integer,
                                                ByVal iCodigoUnidade As Integer,
                                                ByVal iCodigoUsuario As Integer,
                                                ByVal sTipo As String,
                                                ByVal bOffline As Boolean,
                                                ByVal iPage As Integer) As List(Of pwaManutencaoProgramada)

        'Variaveis Locais
        Dim oSqlParameter(4) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New List(Of pwaManutencaoProgramada)
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Tipo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "tipo"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sTipo : i += 1

            'Seta Parametros - Página
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "page"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iPage

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_pwa_select_programada", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New pwaManutencaoProgramada

                oInfo.codigoPCMProgramadaOrdemServico = oSqlDataReader.Item("codigo_pcm_programada_ordem_servico")
                oInfo.codigoPCMProgramada = oSqlDataReader.Item("codigo_pcm_programada")
                oInfo.codigoChecklist = oSqlDataReader.Item("codigo_checklist")
                oInfo.descricao = oSqlDataReader.Item("descricao")
                oInfo.dataUltimaManutencao = oSqlDataReader.Item("data_ultima_manutencao")
                oInfo.dataProximaManutencao = oSqlDataReader.Item("data_proxima_manutencao")
                oInfo.setor = oSqlDataReader.Item("setor")
                oInfo.familiaEquipamento = oSqlDataReader.Item("familia_equipamento")
                oInfo.equipamento = oSqlDataReader.Item("equipamento")
                oInfo.codigoEquipamento = oSqlDataReader.Item("codigo_equipamento")

                'Status
                oInfo.status = New pwaStatus
                oInfo.status.codigo = oSqlDataReader.Item("status_codigo")
                oInfo.status.descricao = oSqlDataReader.Item("status_descricao")
                oInfo.status.cssClass = oSqlDataReader.Item("status_css_class")

                If bOffline Then

                    oInfo.checklist = New pwaChecklist
                    oInfo.checklist = getCheckList(iCodigoEmpresa:=iCodigoEmpresa,
                                                   iCodigoUnidade:=iCodigoUnidade,
                                                   lCodigoChecklist:=oSqlDataReader.Item("codigo_checklist"),
                                                   sTipo:=sTipo,
                                                   lCodigoDocumento:=-1,
                                                   iIntervalo:=-1,
                                                   lCodigoEquipamento:=-1)

                End If

                oReturn.Add(oInfo)

            End While


            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function insertManutencaoProgramadaApontamento(ByVal oManutencaoProgramada As pwaManutencaoProgramadaApontamento) As pwaApontamentoResponse

        'Variaveis Locais
        Dim oSqlParameter(13) As SqlParameter
        Dim oReturn As New pwaApontamentoResponse
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = oManutencaoProgramada.codigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oManutencaoProgramada.codigoUnidade : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oManutencaoProgramada.codigoUsuario : i += 1

            'Seta Parametros - Código Funcionário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_funcionario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oManutencaoProgramada.codigoFuncionario : i += 1

            'Seta Parametros - Código PCM Programada
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_pcm_programada"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = oManutencaoProgramada.codigoPCMProgramada : i += 1

            'Seta Parametros - Data Início
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "data_inicio"
            oSqlParameter(i).SqlDbType = SqlDbType.DateTime
            oSqlParameter(i).Value = oManutencaoProgramada.dataInicio : i += 1

            'Seta Parametros - Data Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "data_termino"
            oSqlParameter(i).SqlDbType = SqlDbType.DateTime
            oSqlParameter(i).Value = oManutencaoProgramada.dataTermino : i += 1

            'Seta Parametros - Descrição da Solução
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "descricao_solucao"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 250
            oSqlParameter(i).Value = oManutencaoProgramada.solucao : i += 1

            'Seta Parametros - Concluído
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "concluido"
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = oManutencaoProgramada.concluido : i += 1

            'Seta Parametros - Valor
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "valor"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = oManutencaoProgramada.valor : i += 1

            'Seta Parametros - Quantidade Equipamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "quantidade_equipamento"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oManutencaoProgramada.quantidadeEquipamento : i += 1

            'Seta Parametros - Código Justificativa Apontamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_justificativa_apontamento"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(oManutencaoProgramada.codigoJustificativa = -1, DBNull.Value, oManutencaoProgramada.codigoJustificativa) : i += 1

            'Seta Parametros - Observação
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "observacao"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 250
            oSqlParameter(i).Value = oManutencaoProgramada.observacao : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.InputOutput
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = oManutencaoProgramada.codigoPCMProgramadaOrdemServico

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_pwa_insert_ordem_servico_programada_apontamento", oSqlParameter)

            oReturn.success = True
            oReturn.message = ""
            oReturn.codigo = oSqlParameter(i).Value

        Catch SqlEx As SqlException
            oReturn.success = False
            oReturn.message = SqlEx.Message.ToString()
        Catch ex As Exception
            oReturn.success = False
            oReturn.message = ex.Message.ToString()
        End Try

        Return oReturn

    End Function

#End Region

#Region "::: TAREFA :::"

    Public Function getListTarefa(ByVal iCodigoEmpresa As Integer,
                                  ByVal iCodigoUnidade As Integer,
                                  ByVal iPage As Integer,
                                  ByVal bOffline As Boolean) As pwaTarefaList

        'Variaveis Locais
        Dim oSqlParameter(3) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New pwaTarefaList
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Página
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "page"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iPage

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_pwa_select_tarefa_list", oSqlParameter)

            While oSqlDataReader.Read

                oReturn.totalPages = oSqlDataReader.Item("total_pages")
                oReturn.totalResults = oSqlDataReader.Item("total_results")
                oReturn.page = oSqlDataReader.Item("page")
                oReturn.results = getTarefa(iCodigoEmpresa:=iCodigoEmpresa,
                                            iCodigoUnidade:=iCodigoUnidade,
                                            iPage:=iPage,
                                            bOffline:=bOffline)

            End While


            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function getTarefa(ByVal iCodigoEmpresa As Integer,
                              ByVal iCodigoUnidade As Integer,
                              ByVal iPage As Integer,
                              ByVal bOffline As Boolean) As List(Of pwaTarefa)

        'Variaveis Locais
        Dim oSqlParameter(3) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New List(Of pwaTarefa)
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Página
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "page"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iPage

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_pwa_select_tarefa", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New pwaTarefa

                oInfo.codigoQATarefaOrdemServico = oSqlDataReader.Item("codigo_qa_tarefa_ordem_servico")
                oInfo.codigoQATarefa = oSqlDataReader.Item("codigo_qa_tarefa")
                oInfo.codigoChecklist = oSqlDataReader.Item("codigo_checklist")
                oInfo.descricao = oSqlDataReader.Item("descricao")
                oInfo.dataUltimaTarefa = IIf(IsDBNull(oSqlDataReader.Item("data_ultima_tarefa")), "", oSqlDataReader.Item("data_ultima_tarefa"))
                oInfo.dataProximaTarefa = oSqlDataReader.Item("data_proxima_tarefa")

                'Status
                oInfo.status = New pwaStatus
                oInfo.status.codigo = oSqlDataReader.Item("status_codigo")
                oInfo.status.descricao = oSqlDataReader.Item("status_descricao")
                oInfo.status.cssClass = oSqlDataReader.Item("status_css_class")

                If bOffline Then

                    oInfo.checklist = New pwaChecklist
                    oInfo.checklist = getCheckList(iCodigoEmpresa:=iCodigoEmpresa,
                                                   iCodigoUnidade:=iCodigoUnidade,
                                                   lCodigoChecklist:=oSqlDataReader.Item("codigo_checklist"),
                                                   sTipo:="TAREFA",
                                                   lCodigoDocumento:=-1,
                                                   iIntervalo:=-1,
                                                   lCodigoEquipamento:=-1)

                End If

                oReturn.Add(oInfo)

            End While


            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function insertTarefaApontamento(ByVal oTarefaApontamento As pwaTarefaApontamento) As pwaApontamentoResponse

        'Variaveis Locais
        Dim oSqlParameter(8) As SqlParameter
        Dim oReturn As New pwaApontamentoResponse
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = oTarefaApontamento.codigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oTarefaApontamento.codigoUnidade : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oTarefaApontamento.codigoUsuario : i += 1

            'Seta Parametros - Código PCM Programada
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_qa_tarefa"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = oTarefaApontamento.codigoQATarefa : i += 1

            'Seta Parametros - Data Início
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "data_inicio"
            oSqlParameter(i).SqlDbType = SqlDbType.DateTime
            oSqlParameter(i).Value = oTarefaApontamento.dataInicio : i += 1

            'Seta Parametros - Data Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "data_termino"
            oSqlParameter(i).SqlDbType = SqlDbType.DateTime
            oSqlParameter(i).Value = oTarefaApontamento.dataTermino : i += 1

            'Seta Parametros - Concluído
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "concluido"
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = oTarefaApontamento.concluido : i += 1

            'Seta Parametros - Observação
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "observacao"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 250
            oSqlParameter(i).Value = oTarefaApontamento.observacao : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.InputOutput
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = oTarefaApontamento.codigoQATarefaOrdemServico

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_pwa_insert_ordem_servico_tarefa_apontamento", oSqlParameter)

            oReturn.success = True
            oReturn.message = ""
            oReturn.codigo = oSqlParameter(i).Value

        Catch SqlEx As SqlException
            oReturn.success = False
            oReturn.message = SqlEx.Message.ToString()
        Catch ex As Exception
            oReturn.success = False
            oReturn.message = ex.Message.ToString()
        End Try

        Return oReturn

    End Function

#End Region

#Region "::: PMOC :::"

    Public Function getListPMOCList(ByVal iCodigoEmpresa As Integer,
                                    ByVal iCodigoUnidade As Integer,
                                    ByVal iIntervalo As Integer,
                                    ByVal iPage As Integer,
                                    ByVal bOffline As Boolean) As pwaPMOCList

        'Variaveis Locais
        Dim oSqlParameter(3) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New pwaPMOCList
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Intervalo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "intervalo"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iIntervalo : i += 1

            'Seta Parametros - Página
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "page"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iPage

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_pwa_select_pmoc_list", oSqlParameter)

            While oSqlDataReader.Read

                oReturn.totalPages = oSqlDataReader.Item("total_pages")
                oReturn.totalResults = oSqlDataReader.Item("total_results")
                oReturn.page = oSqlDataReader.Item("page")
                oReturn.results = getListPMOC(iCodigoEmpresa:=iCodigoEmpresa,
                                              iCodigoUnidade:=iCodigoUnidade,
                                              iIntervalo:=iIntervalo,
                                              iPage:=iPage,
                                              bOffline:=bOffline)

            End While


            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function getListPMOC(ByVal iCodigoEmpresa As Integer,
                                ByVal iCodigoUnidade As Integer,
                                ByVal iIntervalo As Integer,
                                ByVal iPage As Integer,
                                ByVal bOffline As Boolean) As List(Of pwaPMOC)

        'Variaveis Locais
        Dim oSqlParameter(3) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New List(Of pwaPMOC)
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Intervalo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "intervalo"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iIntervalo : i += 1

            'Seta Parametros - Página
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "page"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iPage

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_pwa_select_pmoc", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New pwaPMOC

                oInfo.codigoPMOCOrdemServico = oSqlDataReader.Item("codigo_pmoc_ordem_servico")
                oInfo.codigoArCondicionado = oSqlDataReader.Item("codigo_ar_condicionado")
                oInfo.codigoChecklist = oSqlDataReader.Item("codigo_checklist")
                oInfo.tag = oSqlDataReader.Item("tag")
                oInfo.arCondicionado = oSqlDataReader.Item("descricao")
                oInfo.setor = oSqlDataReader.Item("setor")
                oInfo.local = oSqlDataReader.Item("local")
                oInfo.tipoArCondicionado = oSqlDataReader.Item("tipo_ar_condicionado")
                oInfo.dataUltimoPMOC = oSqlDataReader.Item("data_ultimo_pmoc")
                oInfo.dataProximoPMOC = oSqlDataReader.Item("data_proximo_pmoc")

                'Status
                oInfo.status = New pwaStatus
                oInfo.status.codigo = oSqlDataReader.Item("status_codigo")
                oInfo.status.descricao = oSqlDataReader.Item("status_descricao")
                oInfo.status.cssClass = oSqlDataReader.Item("status_css_class")

                If bOffline Then

                    oInfo.checklist = New pwaChecklist
                    oInfo.checklist = getCheckList(iCodigoEmpresa:=iCodigoEmpresa,
                                                   iCodigoUnidade:=iCodigoUnidade,
                                                   lCodigoChecklist:=oSqlDataReader.Item("codigo_checklist"),
                                                   sTipo:="PMOC",
                                                   lCodigoDocumento:=-1,
                                                   iIntervalo:=-1)

                End If

                oReturn.Add(oInfo)

            End While


            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function insertPMOCApontamento(ByVal oPMOCApontamento As pwaPMOCApontamento) As pwaApontamentoResponse

        'Variaveis Locais
        Dim oSqlParameter(11) As SqlParameter
        Dim oReturn As New pwaApontamentoResponse
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = oPMOCApontamento.codigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oPMOCApontamento.codigoUnidade : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oPMOCApontamento.codigoUsuario : i += 1

            'Seta Parametros - Código Funcionário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_funcionario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oPMOCApontamento.codigoFuncionario : i += 1

            'Seta Parametros - Código Ar Condicionado
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_equipamento"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = oPMOCApontamento.codigoArCondicionado : i += 1

            'Seta Parametros - Data Início
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "data_inicio"
            oSqlParameter(i).SqlDbType = SqlDbType.DateTime
            oSqlParameter(i).Value = oPMOCApontamento.dataInicio : i += 1

            'Seta Parametros - Data Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "data_termino"
            oSqlParameter(i).SqlDbType = SqlDbType.DateTime
            oSqlParameter(i).Value = oPMOCApontamento.dataTermino : i += 1

            'Seta Parametros - Observação
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "observacao"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 250
            oSqlParameter(i).Value = oPMOCApontamento.observacao : i += 1

            'Seta Parametros - Intervalo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "intervalo"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oPMOCApontamento.intervalo : i += 1

            'Seta Parametros - Concluído
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "concluido"
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = oPMOCApontamento.concluido : i += 1

            'Seta Parametros - Código Justificativa Apontamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_justificativa_apontamento"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(oPMOCApontamento.codigoJustificativaApontamento <= 0, DBNull.Value, oPMOCApontamento.codigoJustificativaApontamento) : i += 1

            'Seta Parametros - Código PMOC Ordem Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.InputOutput
            oSqlParameter(i).ParameterName = "codigo_pmoc_ordem_servico"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = oPMOCApontamento.codigoPMOCOrdemServico

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_pwa_insert_pmoc_apontamento", oSqlParameter)

            oReturn.success = True
            oReturn.message = ""
            oReturn.codigo = oSqlParameter(i).Value

        Catch SqlEx As SqlException
            oReturn.success = False
            oReturn.message = SqlEx.Message.ToString()
        Catch ex As Exception
            oReturn.success = False
            oReturn.message = ex.Message.ToString()
        End Try

        Return oReturn

    End Function

#End Region

#Region "::: UH EM DIA :::"

    Public Function getListUHDiaList(ByVal iCodigoEmpresa As Integer,
                                     ByVal iCodigoUnidade As Integer,
                                     ByVal iPage As Integer,
                                     ByVal bOffline As Boolean) As pwaUHDiaList

        'Variaveis Locais
        Dim oSqlParameter(2) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New pwaUHDiaList
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Página
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "page"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iPage

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_pwa_select_pcm_uh_dia_list", oSqlParameter)

            While oSqlDataReader.Read

                oReturn.totalPages = oSqlDataReader.Item("total_pages")
                oReturn.totalResults = oSqlDataReader.Item("total_results")
                oReturn.page = oSqlDataReader.Item("page")
                oReturn.results = getListUHDia(iCodigoEmpresa:=iCodigoEmpresa,
                                               iCodigoUnidade:=iCodigoUnidade,
                                               iPage:=iPage,
                                               bOffline:=bOffline)

            End While


            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function getListUHDia(ByVal iCodigoEmpresa As Integer,
                                 ByVal iCodigoUnidade As Integer,
                                 ByVal iPage As Integer,
                                 ByVal bOffline As Boolean) As List(Of pwaUHDia)

        'Variaveis Locais
        Dim oSqlParameter(2) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New List(Of pwaUHDia)
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Página
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "page"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iPage

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_pwa_select_pcm_uh_dia", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New pwaUHDia

                oInfo.codigoApartamento = oSqlDataReader.Item("codigo_apartamento")
                oInfo.codigoChecklist = oSqlDataReader.Item("codigo_checklist")
                oInfo.uh = oSqlDataReader.Item("uh")
                oInfo.andar = oSqlDataReader.Item("andar")
                oInfo.bloco = oSqlDataReader.Item("bloco")
                oInfo.dataUltimaUHDia = oSqlDataReader.Item("data_ultimo_uh_dia")
                oInfo.dataProximaUHDia = oSqlDataReader.Item("data_proximo_uh_dia")

                'Status
                oInfo.status = New pwaStatus
                oInfo.status.codigo = oSqlDataReader.Item("status_codigo")
                oInfo.status.descricao = oSqlDataReader.Item("status_descricao")
                oInfo.status.cssClass = oSqlDataReader.Item("status_css_class")

                'Status
                oInfo.statusOpera = New pwaStatus
                oInfo.statusOpera.codigo = oSqlDataReader.Item("status_opera_codigo")
                oInfo.statusOpera.descricao = oSqlDataReader.Item("status_opera_descricao")
                oInfo.statusOpera.cssClass = oSqlDataReader.Item("status_opera_css_class")

                If bOffline Then

                    oInfo.checklist = New pwaChecklist
                    oInfo.checklist = getCheckList(iCodigoEmpresa:=iCodigoEmpresa,
                                                   iCodigoUnidade:=iCodigoUnidade,
                                                   lCodigoChecklist:=oSqlDataReader.Item("codigo_checklist"),
                                                   sTipo:="UH",
                                                   lCodigoDocumento:=-1,
                                                   iIntervalo:=-1)

                End If

                oReturn.Add(oInfo)

            End While


            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function insertUHDiaApontamento(ByVal oUHDiaApontamento As pwaUHDiaApontamento) As pwaApontamentoResponse

        'Variaveis Locais
        Dim oSqlParameter(9) As SqlParameter
        Dim oReturn As New pwaApontamentoResponse
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = oUHDiaApontamento.codigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oUHDiaApontamento.codigoUnidade : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oUHDiaApontamento.codigoUsuario : i += 1

            'Seta Parametros - Código Funcionário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_funcionario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oUHDiaApontamento.codigoFuncionario : i += 1

            'Seta Parametros - Código Apartamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_apartamento"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = oUHDiaApontamento.codigoApartamento : i += 1

            'Seta Parametros - Data Início
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "data_inicio"
            oSqlParameter(i).SqlDbType = SqlDbType.DateTime
            oSqlParameter(i).Value = oUHDiaApontamento.dataInicio : i += 1

            'Seta Parametros - Data Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "data_termino"
            oSqlParameter(i).SqlDbType = SqlDbType.DateTime
            oSqlParameter(i).Value = oUHDiaApontamento.dataTermino : i += 1

            'Seta Parametros - Observação
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "observacao"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 250
            oSqlParameter(i).Value = oUHDiaApontamento.observacao : i += 1

            'Seta Parametros - Concluído
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "concluido"
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = oUHDiaApontamento.concluido : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Output
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_pwa_insert_pcm_uh_dia_apontamento", oSqlParameter)

            oReturn.success = True
            oReturn.message = ""
            oReturn.codigo = oSqlParameter(i).Value

        Catch SqlEx As SqlException
            oReturn.success = False
            oReturn.message = SqlEx.Message.ToString()
        Catch ex As Exception
            oReturn.success = False
            oReturn.message = ex.Message.ToString()
        End Try

        Return oReturn

    End Function

#End Region

#Region "::: UH DIA - STATUS :::"

    Public Function getListUHStatusList(ByVal codigoEmpresa As Integer,
                                        ByVal codigoUnidade As Integer,
                                        ByVal page As Integer) As pwaUHStatusList

        'Variaveis Locais
        Dim oReturn As New pwaUHStatusList
        Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("page", SqlDbType.Int, page)
            }

        Try
            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_pwa_select_uh_status_list", oSqlParameter)

                While oSqlDataReader.Read

                    oReturn.totalPages = oSqlDataReader.Item("total_pages")
                    oReturn.totalResults = oSqlDataReader.Item("total_results")
                    oReturn.page = oSqlDataReader.Item("page")
                    oReturn.results = New List(Of pwaUHStatus)
                    oReturn.statusUH = New List(Of pwaStatusUH)
                    oReturn.statusGovernanca = New List(Of pwaStatusGovernanca)

                End While

                oSqlDataReader.NextResult()

                While oSqlDataReader.Read

                    Dim oInfo As New pwaUHStatus With {
                        .andar = SafeGetString(oSqlDataReader, "andar"),
                        .bloco = SafeGetString(oSqlDataReader, "bloco"),
                        .codigoApartamento = SafeGetLong(oSqlDataReader, "codigo_apartamento"),
                        .uh = SafeGetString(oSqlDataReader, "uh"),
                        .statusGovernanca = New pwaStatus With {
                            .codigo = oSqlDataReader.Item("status_codigo"),
                            .descricao = oSqlDataReader.Item("status_descricao"),
                            .cssClass = oSqlDataReader.Item("status_css_class")
                         },
                         .statusUH = New pwaStatus With {
                            .codigo = oSqlDataReader.Item("status_opera_codigo"),
                            .descricao = oSqlDataReader.Item("status_opera_descricao"),
                            .cssClass = oSqlDataReader.Item("status_opera_css_class")
                         }
                    }

                    oReturn.results.Add(oInfo)

                End While

                While oSqlDataReader.Read

                    Dim oStatusUH As New pwaStatusUH With {
                        .codigo = oSqlDataReader.Item("status"),
                        .descricao = oSqlDataReader.Item("descricao")
                    }

                    oReturn.statusUH.Add(oStatusUH)

                End While

                oSqlDataReader.NextResult()

                While oSqlDataReader.Read

                    Dim oStatusGovernanca As New pwaStatusGovernanca With {
                        .codigo = oSqlDataReader.Item("status"),
                        .descricao = oSqlDataReader.Item("descricao")
                    }

                    oReturn.statusGovernanca.Add(oStatusGovernanca)

                End While

            End Using

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Async Function updateUHStatus(ByVal uhStatus As pwaUHStatusUpdate) As Task(Of pwaApontamentoResponse)

        'Variaveis Locais
        Dim oReturn As New pwaApontamentoResponse

        Dim oSqlParameter As SqlParameter() = {
            CriarParametro("codigo_empresa", SqlDbType.Int, uhStatus.codigoEmpresa),
            CriarParametro("codigo_unidade", SqlDbType.Int, uhStatus.codigoUnidade),
            CriarParametro("codigo_usuario", SqlDbType.Int, uhStatus.codigoUsuario),
            CriarParametro("codigo_apartamento", SqlDbType.BigInt, uhStatus.codigoApartamento),
            CriarParametro("status", SqlDbType.VarChar, uhStatus.status)
        }

        Try

            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_pwa_update_status_uh", oSqlParameter)

                While oSqlDataReader.Read

                    oReturn.success = True
                    oReturn.message = ""

                    ' Cria um objeto anônimo para ser serializado em JSON
                    Dim dados = New With {
                        .hotelId = SafeGetString(oSqlDataReader, "hotelId"),
                        .uh = SafeGetString(oSqlDataReader, "uh"),
                        .status = SafeGetString(oSqlDataReader, "status")
                    }

                    ' Serializa o objeto para o formato JSON
                    Dim jsonBody As String = JsonConvert.SerializeObject(dados)

                    ' Cria o conteúdo da requisição informando o tipo MIME como "application/json"
                    Dim content As New StringContent(jsonBody, Encoding.UTF8, "application/json")

                    ' Instancia o HttpClient que será usado para enviar a requisição
                    Using client As New HttpClient()

                        Try

                            ' Se for necessário adicionar cabeçalhos customizados, como autenticação, utilize:
                            ' client.DefaultRequestHeaders.Add("Authorization", "Bearer SEU_TOKEN")

                            ' Executa a requisição POST para a URL da API
                            Dim response = Await client.PostAsync(oSqlDataReader.Item("endpoint"), content)

                            ' Verifica se o status da resposta indica sucesso (códigos 2xx).
                            response.EnsureSuccessStatusCode()

                            ' Lê a resposta da API como uma string
                            Dim responseString As String = Await response.Content.ReadAsStringAsync()

                        Catch ex As Exception
                            oReturn.success = False
                            oReturn.message = ex.Message.ToString()
                        End Try

                    End Using

                End While


            End Using

        Catch SqlEx As SqlException
            oReturn.success = False
            oReturn.message = SqlEx.Message.ToString()
        Catch ex As Exception
            oReturn.success = False
            oReturn.message = ex.Message.ToString()
        End Try

        Return oReturn

    End Function

    Public Async Function updateUHStatusPost(ByVal uhStatus As pwaUHStatusUpdate) As Task(Of pwaApontamentoResponse)

        'Variaveis Locais
        Dim oReturn As New pwaApontamentoResponse

        Dim oSqlParameter As SqlParameter() = {
            CriarParametro("codigo_empresa", SqlDbType.Int, uhStatus.codigoEmpresa),
            CriarParametro("codigo_unidade", SqlDbType.Int, uhStatus.codigoUnidade),
            CriarParametro("codigo_usuario", SqlDbType.Int, uhStatus.codigoUsuario),
            CriarParametro("codigo_apartamento", SqlDbType.BigInt, uhStatus.codigoApartamento),
            CriarParametro("status", SqlDbType.VarChar, uhStatus.status)
        }

        Try

            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_pwa_update_status_uh", oSqlParameter)

                While oSqlDataReader.Read

                    oReturn.success = True
                    oReturn.message = ""


                    Dim handler = New HttpClientHandler() With {
                        .AutomaticDecompression = DecompressionMethods.GZip Or DecompressionMethods.Deflate
                    }

                    ' Força TLS 1.2
                    handler.SslProtocols = SslProtocols.Tls12

                    'Somente para teste: ignora validação de certificado.
                    'Remova em produção.
                    handler.ServerCertificateCustomValidationCallback = Function(sender, cert, chain, sslPolicyErrors) True

                    Using client As New HttpClient(handler)
                        ' Monta Basic Auth
                        Dim credentials = $"{SafeGetString(oSqlDataReader, "username")}:{SafeGetString(oSqlDataReader, "password")}"
                        Dim auth = Convert.ToBase64String(Encoding.ASCII.GetBytes(credentials))
                        client.DefaultRequestHeaders.Authorization = New AuthenticationHeaderValue("Basic", auth)

                        client.DefaultRequestHeaders.Accept.Clear()
                        client.DefaultRequestHeaders.Accept.Add(New MediaTypeWithQualityHeaderValue("application/json"))

                        ' Corpo vazio JSON
                        Dim content As New StringContent("{}", Encoding.UTF8, "application/json")

                        ' Chamada POST
                        Dim resp = Await client.PostAsync(SafeGetString(oSqlDataReader, "endpoint"), content)
                        resp.EnsureSuccessStatusCode()

                        ' Lê resposta
                        Dim body As String = Await resp.Content.ReadAsStringAsync()

                    End Using

                End While


            End Using

        Catch SqlEx As SqlException
            oReturn.success = False
            oReturn.message = SqlEx.Message.ToString()
        Catch ex As Exception
            oReturn.success = False
            oReturn.message = ex.Message.ToString()
        End Try

        Return oReturn

    End Function

    Public Function updateUHStatusIntercity(ByVal uhStatus As pwaUHStatusUpdate) As pwaApontamentoResponse

        'Variaveis Locais
        Dim oReturn As New pwaApontamentoResponse

        Dim oSqlParameter As SqlParameter() = {
            CriarParametro("codigo_empresa", SqlDbType.Int, uhStatus.codigoEmpresa),
            CriarParametro("codigo_unidade", SqlDbType.Int, uhStatus.codigoUnidade),
            CriarParametro("codigo_usuario", SqlDbType.Int, uhStatus.codigoUsuario),
            CriarParametro("codigo_apartamento", SqlDbType.BigInt, uhStatus.codigoApartamento),
            CriarParametro("status", SqlDbType.VarChar, uhStatus.status)
        }

        Try

            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_pwa_update_status_uh_intercity", oSqlParameter)

                While oSqlDataReader.Read

                    oReturn.success = True
                    oReturn.message = ""

                    Call updateUHStatusGovernancaIntercity(SafeGetString(oSqlDataReader, "hotelId"),
                                                           SafeGetString(oSqlDataReader, "uh"),
                                                           SafeGetString(oSqlDataReader, "status"))

                End While

            End Using

        Catch SqlEx As SqlException
            oReturn.success = False
            oReturn.message = SqlEx.Message.ToString()
        Catch ex As Exception
            oReturn.success = False
            oReturn.message = ex.Message.ToString()
        End Try

        Return oReturn

    End Function

    Public Sub updateUHStatusGovernancaIntercity(hotelId As String, uh As String, status As String)

        Using connection As New OracleConnection(sConnectionIntercity)
            connection.Open()

            Using command As New OracleCommand(
                "UPDATE UH
                 SET IDSTATUSGOV = :NovoStatus
                 WHERE IDHOTEL = :HotelId
                   AND LTRIM(RTRIM(CODUH)) = :CodUH",
                connection)

                command.Parameters.Add(New OracleParameter(":NovoStatus", status))
                command.Parameters.Add(New OracleParameter(":HotelId", hotelId))
                command.Parameters.Add(New OracleParameter(":CodUH", uh))

                command.ExecuteNonQuery()
            End Using
        End Using

    End Sub

    Public Sub updateUHStatusGovernanca(ByVal uhStatus As pwaUHStatusUpdate)

        Dim oSqlParameter As SqlParameter() = {
            CriarParametro("codigo_empresa", SqlDbType.Int, uhStatus.codigoEmpresa),
            CriarParametro("codigo_unidade", SqlDbType.Int, uhStatus.codigoUnidade),
            CriarParametro("codigo_usuario", SqlDbType.Int, uhStatus.codigoUsuario),
            CriarParametro("codigo_apartamento", SqlDbType.BigInt, uhStatus.codigoApartamento),
            CriarParametro("status", SqlDbType.VarChar, uhStatus.status)
        }

        Try

            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_pwa_update_status_uh_governanca", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region

#Region "::: GOVERNANÇA :::"

    Public Function getListGovernancaList(ByVal codigoEmpresa As Integer,
                                          ByVal codigoUnidade As Integer,
                                          ByVal codigoFuncionario As Integer,
                                          ByVal codigoUsuario As Integer,
                                          ByVal page As Integer,
                                          ByVal offline As Boolean) As pwaGovernancaList


        'Variaveis Locais
        Dim oReturn As New pwaGovernancaList
        Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("codigo_funcionario", SqlDbType.Int, codigoFuncionario),
                CriarParametro("codigo_usuario", SqlDbType.Int, codigoUsuario),
                CriarParametro("page", SqlDbType.Int, page)
            }

        Try

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_pwa_select_governanca_list", oSqlParameter)

                While oSqlDataReader.Read

                    oReturn.totalPages = oSqlDataReader.Item("total_pages")
                    oReturn.totalResults = oSqlDataReader.Item("total_results")
                    oReturn.page = oSqlDataReader.Item("page")
                    oReturn.results = New List(Of pwaGovernanca)
                    oReturn.statusUH = New List(Of pwaStatusUH)
                    oReturn.statusGovernanca = New List(Of pwaStatusGovernanca)
                    oReturn.apontaCamareira = oSqlDataReader.Item("aponta_camareira")
                    oReturn.alteraStatus = oSqlDataReader.Item("change_status")

                End While

                oSqlDataReader.NextResult()

                While oSqlDataReader.Read

                    Dim oInfo As New pwaGovernanca With {
                        .codigoDocumento = oSqlDataReader.Item("codigo"),
                        .codigoApartamento = oSqlDataReader.Item("codigo_apartamento"),
                        .codigoChecklist = oSqlDataReader.Item("codigo_checklist"),
                        .uh = oSqlDataReader.Item("uh"),
                        .andar = oSqlDataReader.Item("andar"),
                        .bloco = oSqlDataReader.Item("bloco"),
                        .codigoTipoGovernanca = oSqlDataReader.Item("codigo_tipo_governanca"),
                        .tipoGovernanca = oSqlDataReader.Item("tipo_governanca"),
                        .naoPerturbe = oSqlDataReader.Item("nao_perturbe"),
                        .dataUltimaGovernaca = oSqlDataReader.Item("data_ultima_governanca"),
                        .alertaCheckInOut = oSqlDataReader.Item("alert_checkin_out"),
                        .poolCondominio = IIf(oSqlDataReader.Item("pool_condominio") = 2, "P", "C"),
                        .status = New pwaStatus With {
                            .codigo = oSqlDataReader.Item("status_codigo"),
                            .descricao = oSqlDataReader.Item("status_descricao"),
                            .cssClass = oSqlDataReader.Item("status_css_class")
                         },
                         .statusOpera = New pwaStatus With {
                            .codigo = oSqlDataReader.Item("status_opera_codigo"),
                            .descricao = oSqlDataReader.Item("status_opera_descricao"),
                            .cssClass = oSqlDataReader.Item("status_opera_css_class")
                         },
                         .statusGovernanca = New pwaStatus With {
                            .codigo = oSqlDataReader.Item("status_governanca_codigo"),
                            .descricao = oSqlDataReader.Item("status_governanca_descricao"),
                            .cssClass = oSqlDataReader.Item("status_governanca_css_class")
                         },
                         .statusUH = New pwaStatus With {
                            .codigo = oSqlDataReader.Item("status_uh_codigo"),
                            .descricao = oSqlDataReader.Item("status_uh_descricao"),
                            .cssClass = oSqlDataReader.Item("status_uh_css_class")
                         }
                    }

                    If offline Then

                        oInfo.checklist = New pwaChecklist
                        oInfo.checklist = getCheckList(iCodigoEmpresa:=codigoEmpresa,
                                                       iCodigoUnidade:=codigoUnidade,
                                                       lCodigoChecklist:=oSqlDataReader.Item("codigo_checklist"),
                                                       sTipo:="GOVERNANCA",
                                                       lCodigoDocumento:=-1,
                                                       iIntervalo:=-1)

                    End If

                    oReturn.results.Add(oInfo)

                End While

                oSqlDataReader.NextResult()

                While oSqlDataReader.Read

                    Dim oStatusUH As New pwaStatusUH With {
                        .codigo = oSqlDataReader.Item("status"),
                        .descricao = oSqlDataReader.Item("descricao")
                    }

                    oReturn.statusUH.Add(oStatusUH)

                End While

                oSqlDataReader.NextResult()

                While oSqlDataReader.Read

                    Dim oStatusGovernanca As New pwaStatusGovernanca With {
                        .codigo = oSqlDataReader.Item("status"),
                        .descricao = oSqlDataReader.Item("descricao")
                    }

                    oReturn.statusGovernanca.Add(oStatusGovernanca)

                End While

            End Using

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function insertGovernancaApontamento(ByVal oGovernanca As pwaGovernancaApontamento) As pwaApontamentoResponse

        'Variaveis Locais
        Dim oSqlParameter(13) As SqlParameter
        Dim oReturn As New pwaApontamentoResponse
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = oGovernanca.codigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oGovernanca.codigoUnidade : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oGovernanca.codigoUsuario : i += 1

            'Seta Parametros - Código Funcionário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_funcionario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oGovernanca.codigoFuncionario : i += 1

            'Seta Parametros - Código Camareira
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_camareira"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oGovernanca.codigoCamareira : i += 1

            'Seta Parametros - Código Checklist
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_checklist"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = oGovernanca.codigoChecklist : i += 1

            'Seta Parametros - Código Apartamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_apartamento"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = oGovernanca.codigoApartamento : i += 1

            'Seta Parametros - Código Tipo Governança
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_tipo_governanca"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = oGovernanca.codigoTipoGovernanca : i += 1

            'Seta Parametros - Data Início
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "data_inicio"
            oSqlParameter(i).SqlDbType = SqlDbType.DateTime
            oSqlParameter(i).Value = oGovernanca.dataInicio : i += 1

            'Seta Parametros - Data Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "data_termino"
            oSqlParameter(i).SqlDbType = SqlDbType.DateTime
            oSqlParameter(i).Value = oGovernanca.dataTermino : i += 1

            'Seta Parametros - Observação
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "observacao"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 250
            oSqlParameter(i).Value = oGovernanca.observacao : i += 1

            'Seta Parametros - Não Perturbe
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "nao_perturbe"
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = oGovernanca.naoPerturbe : i += 1

            'Seta Parametros - Concluído
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "concluido"
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = oGovernanca.concluido : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Output
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_pwa_insert_governanca_apontamento", oSqlParameter)

            oReturn.success = True
            oReturn.message = ""
            oReturn.codigo = oSqlParameter(i).Value

        Catch SqlEx As SqlException
            oReturn.success = False
            oReturn.message = SqlEx.Message.ToString()
        Catch ex As Exception
            oReturn.success = False
            oReturn.message = ex.Message.ToString()
        End Try

        Return oReturn

    End Function

#End Region

#Region "::: DEDETIZAÇÃO :::"

    Public Function getListDedetizacaoList(ByVal iCodigoEmpresa As Integer,
                                           ByVal iCodigoUnidade As Integer,
                                           ByVal iPage As Integer) As pwaDedetizacaoList

        'Variaveis Locais
        Dim oSqlParameter(2) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New pwaDedetizacaoList
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Página
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "page"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iPage

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_pwa_select_dedetizacao_list", oSqlParameter)

            While oSqlDataReader.Read

                oReturn.totalPages = oSqlDataReader.Item("total_pages")
                oReturn.totalResults = oSqlDataReader.Item("total_results")
                oReturn.page = oSqlDataReader.Item("page")
                oReturn.results = getListDedetizacao(iCodigoEmpresa:=iCodigoEmpresa,
                                                     iCodigoUnidade:=iCodigoUnidade,
                                                     iPage:=iPage)

            End While


            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function getListDedetizacao(ByVal iCodigoEmpresa As Integer,
                                       ByVal iCodigoUnidade As Integer,
                                       ByVal iPage As Integer) As List(Of pwaDedetizacao)

        'Variaveis Locais
        Dim oSqlParameter(2) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New List(Of pwaDedetizacao)
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Página
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "page"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iPage

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_pwa_select_dedetizacao", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New pwaDedetizacao

                oInfo.codigoApartamento = oSqlDataReader.Item("codigo_apartamento")
                oInfo.uh = oSqlDataReader.Item("uh")
                oInfo.andar = oSqlDataReader.Item("andar")
                oInfo.bloco = oSqlDataReader.Item("bloco")
                oInfo.dataUltimaDedetizacao = oSqlDataReader.Item("data_ultima_dedetizacao")
                oInfo.dataProximaDedetizacao = oSqlDataReader.Item("data_proxima_dedetizacao")

                'Status
                oInfo.statusOpera = New pwaStatus
                oInfo.statusOpera.codigo = oSqlDataReader.Item("status_opera_codigo")
                oInfo.statusOpera.descricao = oSqlDataReader.Item("status_opera_descricao")
                oInfo.statusOpera.cssClass = oSqlDataReader.Item("status_opera_css_class")

                oReturn.Add(oInfo)

            End While


            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function insertDedetizacaoApontamento(ByVal oDedetizacaoApontamento As pwaDedetizacaoApontamento) As pwaDefaultResponse

        'Variaveis Locais
        Dim oSqlParameter(9) As SqlParameter
        Dim oReturn As New pwaDefaultResponse
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = oDedetizacaoApontamento.codigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oDedetizacaoApontamento.codigoUnidade : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oDedetizacaoApontamento.codigoUsuario : i += 1

            'Seta Parametros - Código Funcionário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_funcionario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oDedetizacaoApontamento.codigoFuncionario : i += 1

            'Seta Parametros - Código Apartamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_apartamento"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = oDedetizacaoApontamento.codigoApartamento : i += 1

            'Seta Parametros - Observação
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "observacao"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 250
            oSqlParameter(i).Value = oDedetizacaoApontamento.observacao : i += 1

            'Seta Parametros - Concluído
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "concluido"
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = oDedetizacaoApontamento.concluido

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_pwa_insert_dedetizacao_apontamento", oSqlParameter)

            oReturn.success = True
            oReturn.message = ""

        Catch SqlEx As SqlException
            oReturn.success = False
            oReturn.message = SqlEx.Message.ToString()
        Catch ex As Exception
            oReturn.success = False
            oReturn.message = ex.Message.ToString()
        End Try

        Return oReturn

    End Function

#End Region

#Region "::: GREEN PLANET :::"

    Public Function getGreenPlanetList(ByVal iCodigoEmpresa As Integer,
                                       ByVal iCodigoUnidade As Integer,
                                       ByVal sData As String) As pwaGreenPlanetList

        'Variaveis Locais
        Dim oSqlParameter(2) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New pwaGreenPlanetList
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Data
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "data"
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = IIf(IsDate(sData) = False, DBNull.Value, sData)

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_pwa_select_green_planet", oSqlParameter)

            While oSqlDataReader.Read

                oReturn.data = oSqlDataReader.Item("data")
                oReturn.numeroHospede = oSqlDataReader.Item("numero_hospede")
                oReturn.uhOcupada = oSqlDataReader.Item("uh_ocupada")
                oReturn.itemMedicao = getGreenPlanetItemMedicao(iCodigoEmpresa:=iCodigoEmpresa,
                                                                iCodigoUnidade:=iCodigoUnidade,
                                                                sData:=oSqlDataReader.Item("data"))

            End While


            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function getGreenPlanetItemMedicao(ByVal iCodigoEmpresa As Integer,
                                              ByVal iCodigoUnidade As Integer,
                                              ByVal sData As String) As List(Of pwaGreenPlanetItemMedicao)

        'Variaveis Locais
        Dim oSqlParameter(2) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New List(Of pwaGreenPlanetItemMedicao)
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Data
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "data"
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = IIf(IsDate(sData) = False, DBNull.Value, sData)

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_pwa_select_green_planet_item_medicao", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New pwaGreenPlanetItemMedicao

                oInfo.codigo = oSqlDataReader.Item("codigo")
                oInfo.descricao = oSqlDataReader.Item("descricao")
                oInfo.numeroCasasDecimais = oSqlDataReader.Item("numero_casas_decimais")
                oInfo.allowPicture = oSqlDataReader.Item("allow_picture")
                oInfo.valor = oSqlDataReader.Item("valor")

                oReturn.Add(oInfo)

            End While


            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function insertGreenPlanetApontamento(ByVal oGreenPlanet As pwaGreenPlanetApontamento) As pwaDefaultResponse

        'Variaveis Locais
        Dim oSqlParameter(7) As SqlParameter
        Dim oReturn As New pwaDefaultResponse
        Dim i As Integer = 0

        Try

            For Each oItemMedicao As pwaGreenPlanetItemMedicao In oGreenPlanet.itemMedicao

                'Seta Váriavel
                i = 0

                'Seta Parametros - Código Empresa
                oSqlParameter(i) = New SqlParameter
                oSqlParameter(i).Direction = ParameterDirection.Input
                oSqlParameter(i).ParameterName = "codigo_empresa"
                oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
                oSqlParameter(i).Value = oGreenPlanet.codigoEmpresa : i += 1

                'Seta Parametros - Código Unidade
                oSqlParameter(i) = New SqlParameter
                oSqlParameter(i).Direction = ParameterDirection.Input
                oSqlParameter(i).ParameterName = "codigo_unidade"
                oSqlParameter(i).SqlDbType = SqlDbType.Int
                oSqlParameter(i).Value = oGreenPlanet.codigoUnidade : i += 1

                'Seta Parametros - Código Usuário
                oSqlParameter(i) = New SqlParameter
                oSqlParameter(i).Direction = ParameterDirection.Input
                oSqlParameter(i).ParameterName = "codigo_usuario"
                oSqlParameter(i).SqlDbType = SqlDbType.Int
                oSqlParameter(i).Value = oGreenPlanet.codigoUsuario : i += 1

                'Seta Parametros - Data
                oSqlParameter(i) = New SqlParameter
                oSqlParameter(i).Direction = ParameterDirection.Input
                oSqlParameter(i).ParameterName = "data"
                oSqlParameter(i).SqlDbType = SqlDbType.Date
                oSqlParameter(i).Value = oGreenPlanet.data : i += 1

                'Seta Parametros - Número Hospede
                oSqlParameter(i) = New SqlParameter
                oSqlParameter(i).Direction = ParameterDirection.Input
                oSqlParameter(i).ParameterName = "numero_hospede"
                oSqlParameter(i).SqlDbType = SqlDbType.Int
                oSqlParameter(i).Value = oGreenPlanet.numeroHospede : i += 1

                'Seta Parametros - UH Ocupada
                oSqlParameter(i) = New SqlParameter
                oSqlParameter(i).Direction = ParameterDirection.Input
                oSqlParameter(i).ParameterName = "uh_ocupada"
                oSqlParameter(i).SqlDbType = SqlDbType.Int
                oSqlParameter(i).Value = oGreenPlanet.uhOcupada : i += 1

                'Seta Parametros - Código Item Medição
                oSqlParameter(i) = New SqlParameter
                oSqlParameter(i).Direction = ParameterDirection.Input
                oSqlParameter(i).ParameterName = "codigo_item_medicao"
                oSqlParameter(i).SqlDbType = SqlDbType.Int
                oSqlParameter(i).Value = oItemMedicao.codigo : i += 1

                'Seta Parametros - Valor
                oSqlParameter(i) = New SqlParameter
                oSqlParameter(i).Direction = ParameterDirection.Input
                oSqlParameter(i).ParameterName = "valor"
                oSqlParameter(i).SqlDbType = SqlDbType.Float
                oSqlParameter(i).Value = oItemMedicao.valor

                'Executa Query
                ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_pwa_insert_green_planet_apontamento", oSqlParameter)

            Next

            oReturn.success = True
            oReturn.message = ""

        Catch SqlEx As SqlException
            oReturn.success = False
            oReturn.message = SqlEx.Message.ToString()
        Catch ex As Exception
            oReturn.success = False
            oReturn.message = ex.Message.ToString()
        End Try

        Return oReturn

    End Function

#End Region

#Region "::: MAPA DE MANUTENÇÃO :::"

    Public Function getListMapa(ByVal iCodigoEmpresa As Integer,
                                ByVal iCodigoUnidade As Integer,
                                ByVal iPage As Integer) As pwaMapaList

        'Variaveis Locais
        Dim oSqlParameter(2) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New pwaMapaList
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Página
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "page"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iPage

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_pwa_select_mapa_list", oSqlParameter)

            While oSqlDataReader.Read

                oReturn.totalPages = oSqlDataReader.Item("total_pages")
                oReturn.totalResults = oSqlDataReader.Item("total_results")
                oReturn.page = oSqlDataReader.Item("page")
                oReturn.results = getMapa(iCodigoEmpresa:=iCodigoEmpresa,
                                          iCodigoUnidade:=iCodigoUnidade,
                                          iPage:=iPage)

            End While


            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function getMapa(ByVal iCodigoEmpresa As Integer,
                            ByVal iCodigoUnidade As Integer,
                            ByVal iPage As Integer) As List(Of pwaMapa)

        'Variaveis Locais
        Dim oSqlParameter(2) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New List(Of pwaMapa)
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Página
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "page"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iPage

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_pwa_select_mapa", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New pwaMapa

                oInfo.codigo = oSqlDataReader.Item("codigo")
                oInfo.descricao = oSqlDataReader.Item("descricao")

                oReturn.Add(oInfo)

            End While


            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function getListMapaManutencaoList(ByVal iCodigoEmpresa As Integer,
                                              ByVal iCodigoUnidade As Integer,
                                              ByVal lCodigo As Long,
                                              ByVal iPage As Integer) As pwaMapaManutencaoList

        'Variaveis Locais
        Dim oSqlParameter(3) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New pwaMapaManutencaoList
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigo : i += 1

            'Seta Parametros - Página
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "page"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iPage

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_pwa_select_mapa_manutencao_list", oSqlParameter)

            While oSqlDataReader.Read

                oReturn.totalPages = oSqlDataReader.Item("total_pages")
                oReturn.totalResults = oSqlDataReader.Item("total_results")
                oReturn.page = oSqlDataReader.Item("page")
                oReturn.results = getListMapaManutencao(iCodigoEmpresa:=iCodigoEmpresa,
                                                        iCodigoUnidade:=iCodigoUnidade,
                                                        lCodigo:=lCodigo,
                                                        iPage:=iPage)

            End While


            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function getListMapaManutencao(ByVal iCodigoEmpresa As Integer,
                                          ByVal iCodigoUnidade As Integer,
                                          ByVal lCodigo As Long,
                                          ByVal iPage As Integer) As List(Of pwaMapaManutencao)

        'Variaveis Locais
        Dim oSqlParameter(3) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New List(Of pwaMapaManutencao)
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigo : i += 1

            'Seta Parametros - Página
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "page"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iPage

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_pwa_select_mapa_manutencao", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New pwaMapaManutencao

                oInfo.codigoAtividade = oSqlDataReader.Item("codigo_atividade")
                oInfo.codigoApartamento = oSqlDataReader.Item("codigo_apartamento")
                oInfo.uh = oSqlDataReader.Item("uh")
                oInfo.andar = oSqlDataReader.Item("andar")
                oInfo.bloco = oSqlDataReader.Item("bloco")
                oInfo.dataPrevisaoTermino = oSqlDataReader.Item("data_previsao_termino")

                'Status
                oInfo.statusOpera = New pwaStatus
                oInfo.statusOpera.codigo = oSqlDataReader.Item("status_opera_codigo")
                oInfo.statusOpera.descricao = oSqlDataReader.Item("status_opera_descricao")
                oInfo.statusOpera.cssClass = oSqlDataReader.Item("status_opera_css_class")

                oReturn.Add(oInfo)

            End While


            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function insertMapaManutencaoApontamento(ByVal oMapaManutencaoApontamento As pwaMapaManutencaoApontamento) As pwaDefaultResponse

        'Variaveis Locais
        Dim oSqlParameter(9) As SqlParameter
        Dim oReturn As New pwaDefaultResponse
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = oMapaManutencaoApontamento.codigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oMapaManutencaoApontamento.codigoUnidade : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oMapaManutencaoApontamento.codigoUsuario : i += 1

            'Seta Parametros - Código Funcionário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_funcionario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oMapaManutencaoApontamento.codigoFuncionario : i += 1

            'Seta Parametros - Código Atividade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_atividade"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = oMapaManutencaoApontamento.codigoAtividade : i += 1

            'Seta Parametros - Código Apartamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_apartamento"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = oMapaManutencaoApontamento.codigoApartamento : i += 1

            'Seta Parametros - Observação
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "observacao"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 250
            oSqlParameter(i).Value = oMapaManutencaoApontamento.observacao : i += 1

            'Seta Parametros - Concluído
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "concluido"
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = oMapaManutencaoApontamento.concluido

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_pwa_insert_mapa_manutencao_apontamento", oSqlParameter)

            oReturn.success = True
            oReturn.message = ""

        Catch SqlEx As SqlException
            oReturn.success = False
            oReturn.message = SqlEx.Message.ToString()
        Catch ex As Exception
            oReturn.success = False
            oReturn.message = ex.Message.ToString()
        End Try

        Return oReturn

    End Function

#End Region

#Region "::: QUALIDADE - AUDITORIA :::"

    Public Function getListQualidadeAuditoria(ByVal iCodigoEmpresa As Integer,
                                              ByVal iCodigoUnidade As Integer,
                                              ByVal iCodigoUsuario As Integer,
                                              ByVal iPage As Integer,
                                              ByVal bOffline As Boolean) As pwaQualidadeAuditoriaList

        'Variaveis Locais
        Dim oSqlParameter(3) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New pwaQualidadeAuditoriaList
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Página
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "page"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iPage

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_pwa_select_qualidade_auditoria_list", oSqlParameter)

            While oSqlDataReader.Read

                oReturn.totalPages = oSqlDataReader.Item("total_pages")
                oReturn.totalResults = oSqlDataReader.Item("total_results")
                oReturn.page = oSqlDataReader.Item("page")
                oReturn.results = getQualidadeAuditoria(iCodigoEmpresa:=iCodigoEmpresa,
                                                        iCodigoUnidade:=iCodigoUnidade,
                                                        iCodigoUsuario:=iCodigoUsuario,
                                                        iPage:=iPage,
                                                        bOffline:=bOffline)

            End While


            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function getQualidadeAuditoria(ByVal iCodigoEmpresa As Integer,
                                          ByVal iCodigoUnidade As Integer,
                                          ByVal iCodigoUsuario As Integer,
                                          ByVal iPage As Integer,
                                          ByVal bOffline As Boolean) As List(Of pwaQualidadeAuditoria)

        'Variaveis Locais
        Dim oSqlParameter(3) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New List(Of pwaQualidadeAuditoria)
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Página
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "page"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iPage

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_pwa_select_qualidade_auditoria", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New pwaQualidadeAuditoria
                Dim oStatus As New pwaStatus

                oInfo.codigo = oSqlDataReader("codigo")
                oInfo.codigoAuditoriaInterna = oSqlDataReader("codigo_auditoria_interna")
                oInfo.codigoChecklist = oSqlDataReader("codigo_checklist")
                oInfo.dataUltimaAuditoria = oSqlDataReader("data_ultima_auditoria")
                oInfo.dataProximaAuditoria = oSqlDataReader("data_proxima_auditoria")
                oInfo.descricao = oSqlDataReader("descricao")
                oInfo.pontosPossiveis = oSqlDataReader("pontos_possiveis")
                oInfo.pontosRealizados = oSqlDataReader("pontos_realizados")
                oInfo.pontosConformes = oSqlDataReader("pontos_conformes")
                oInfo.pontosNaoConformes = oSqlDataReader("pontos_nao_conformes")
                oInfo.naoRespondido = oSqlDataReader("nao_respondido")
                oInfo.naoAplicaveis = oSqlDataReader("nao_aplicaveis")

                oStatus.codigo = oSqlDataReader("status")
                oStatus.descricao = oSqlDataReader("status_descricao")
                oStatus.cssClass = oSqlDataReader("status_css_class")

                oInfo.status = oStatus

                If bOffline Then

                    oInfo.checklist = New pwaChecklist
                    oInfo.checklist = getCheckList(iCodigoEmpresa:=iCodigoEmpresa,
                                                   iCodigoUnidade:=iCodigoUnidade,
                                                   lCodigoChecklist:=oSqlDataReader.Item("codigo_checklist"),
                                                   sTipo:="AUDITORIA_QUALIDADE",
                                                   lCodigoDocumento:=-1,
                                                   iIntervalo:=-1)

                End If

                oReturn.Add(oInfo)

            End While


            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function insertQualidadeAuditoriaApontamento(ByVal oQualidadeAuditoriaApontamento As pwaQualidadeAuditoriaApontamento) As pwaApontamentoResponse

        'Variaveis Locais
        Dim oSqlParameter(9) As SqlParameter
        Dim oReturn As New pwaApontamentoResponse
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = oQualidadeAuditoriaApontamento.codigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oQualidadeAuditoriaApontamento.codigoUnidade : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oQualidadeAuditoriaApontamento.codigoUsuario : i += 1

            'Seta Parametros - Código Funcionário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_funcionario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oQualidadeAuditoriaApontamento.codigoFuncionario : i += 1

            'Seta Parametros - Código Auditoria Interna
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_auditoria_interna"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oQualidadeAuditoriaApontamento.codigoAuditoriaInterna : i += 1

            'Seta Parametros - Data Início
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "data_inicio"
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = oQualidadeAuditoriaApontamento.dataInicio : i += 1

            'Seta Parametros - Data Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "data_termino"
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = oQualidadeAuditoriaApontamento.dataTermino : i += 1

            'Seta Parametros - Observação
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "observacao"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 250
            oSqlParameter(i).Value = oQualidadeAuditoriaApontamento.observacao : i += 1

            'Seta Parametros - Concluído
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "concluido"
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = oQualidadeAuditoriaApontamento.concluido : i += 1

            'Seta Parametros - Código Auditoria
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.InputOutput
            oSqlParameter(i).ParameterName = "codigo_auditoria"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = oQualidadeAuditoriaApontamento.codigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_pwa_insert_qualidade_auditoria_apontamento", oSqlParameter)

            oReturn.success = True
            oReturn.message = ""
            oReturn.codigo = oSqlParameter(i).Value

        Catch SqlEx As SqlException
            oReturn.success = False
            oReturn.message = SqlEx.Message.ToString()
        Catch ex As Exception
            oReturn.success = False
            oReturn.message = ex.Message.ToString()
        End Try

        Return oReturn

    End Function

#End Region

#Region "::: AUDITORIA - CORPORATIVO :::"

    Public Function getListAuditoriaCorporativo(ByVal iCodigoEmpresa As Integer,
                                                ByVal iCodigoUnidade As Integer,
                                                ByVal iCodigoUsuario As Integer,
                                                ByVal iPage As Integer,
                                                ByVal bOffline As Boolean) As pwaAuditoriaCorporativoList

        'Variaveis Locais
        Dim oSqlParameter(3) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New pwaAuditoriaCorporativoList
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Página
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "page"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iPage

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_pwa_select_auditoria_corporativo_list", oSqlParameter)

            While oSqlDataReader.Read

                oReturn.totalPages = oSqlDataReader.Item("total_pages")
                oReturn.totalResults = oSqlDataReader.Item("total_results")
                oReturn.page = oSqlDataReader.Item("page")
                oReturn.results = getAuditoriaCorporativo(iCodigoEmpresa:=iCodigoEmpresa,
                                                          iCodigoUnidade:=iCodigoUnidade,
                                                          iCodigoUsuario:=iCodigoUsuario,
                                                          iPage:=iPage,
                                                          bOffline:=bOffline)

            End While


            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function getAuditoriaCorporativo(ByVal iCodigoEmpresa As Integer,
                                            ByVal iCodigoUnidade As Integer,
                                            ByVal iCodigoUsuario As Integer,
                                            ByVal iPage As Integer,
                                            ByVal bOffline As Boolean) As List(Of pwaAuditoriaCorporativo)

        'Variaveis Locais
        Dim oSqlParameter(3) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New List(Of pwaAuditoriaCorporativo)
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Página
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "page"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iPage

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_pwa_select_auditoria_corporativo", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New pwaAuditoriaCorporativo
                Dim oStatus As New pwaStatus

                oInfo.codigo = oSqlDataReader("codigo")
                oInfo.codigoAuditoriaInterna = oSqlDataReader("codigo_auditoria_interna")
                oInfo.codigoChecklist = oSqlDataReader("codigo_checklist")
                oInfo.dataUltimaAuditoria = oSqlDataReader("data_ultima_auditoria")
                oInfo.dataProximaAuditoria = oSqlDataReader("data_proxima_auditoria")
                oInfo.descricao = oSqlDataReader("descricao")
                oInfo.pontosPossiveis = oSqlDataReader("pontos_possiveis")
                oInfo.pontosRealizados = oSqlDataReader("pontos_realizados")
                oInfo.pontosConformes = oSqlDataReader("pontos_conformes")
                oInfo.pontosNaoConformes = oSqlDataReader("pontos_nao_conformes")
                oInfo.naoRespondido = oSqlDataReader("nao_respondido")
                oInfo.naoAplicaveis = oSqlDataReader("nao_aplicaveis")

                oStatus.codigo = oSqlDataReader("status")
                oStatus.descricao = oSqlDataReader("status_descricao")
                oStatus.cssClass = oSqlDataReader("status_css_class")

                oInfo.status = oStatus

                oInfo.checklist = New pwaChecklist

                'If iCodigoEmpresa = 905 Then
                oInfo.checklist = getCheckListFull(iCodigoEmpresa:=iCodigoEmpresa,
                                                   iCodigoUnidade:=iCodigoUnidade,
                                                   lCodigoChecklist:=oSqlDataReader.Item("codigo_checklist"),
                                                   sTipo:="AUDITORIA_CORPORATIVO",
                                                   lCodigoDocumento:=-1,
                                                   iIntervalo:=-1)

                'Else
                '    oInfo.checklist = getCheckList(iCodigoEmpresa:=iCodigoEmpresa,
                '                                   iCodigoUnidade:=iCodigoUnidade,
                '                                   lCodigoChecklist:=oSqlDataReader.Item("codigo_checklist"),
                '                                   sTipo:="AUDITORIA_CORPORATIVO",
                '                                   lCodigoDocumento:=-1,
                '                                   iIntervalo:=-1)

                'End If

                oReturn.Add(oInfo)

            End While


            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function insertAuditoriaCorporativoApontamento(ByVal oAuditoriaCorporativoApontamento As pwaAuditoriaCorporativoApontamento) As pwaApontamentoResponse

        'Variaveis Locais
        Dim oSqlParameter(10) As SqlParameter
        Dim oReturn As New pwaApontamentoResponse
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = oAuditoriaCorporativoApontamento.codigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oAuditoriaCorporativoApontamento.codigoUnidade : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oAuditoriaCorporativoApontamento.codigoUsuario : i += 1

            'Seta Parametros - Código Funcionário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_funcionario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oAuditoriaCorporativoApontamento.codigoFuncionario : i += 1

            'Seta Parametros - Código Auditoria Interna
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_auditoria_interna"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oAuditoriaCorporativoApontamento.codigoAuditoriaInterna : i += 1

            'Seta Parametros - Nº Documento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "numero_documento"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = oAuditoriaCorporativoApontamento.numeroDocumento : i += 1

            'Seta Parametros - Data Início
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "data_inicio"
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = oAuditoriaCorporativoApontamento.dataInicio : i += 1

            'Seta Parametros - Data Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "data_termino"
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = oAuditoriaCorporativoApontamento.dataTermino : i += 1

            'Seta Parametros - Observação
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "observacao"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 250
            oSqlParameter(i).Value = oAuditoriaCorporativoApontamento.observacao : i += 1

            'Seta Parametros - Concluído
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "concluido"
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = oAuditoriaCorporativoApontamento.concluido : i += 1

            'Seta Parametros - Código Auditoria
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.InputOutput
            oSqlParameter(i).ParameterName = "codigo_auditoria"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = oAuditoriaCorporativoApontamento.codigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_pwa_insert_auditoria_corporativo_apontamento", oSqlParameter)

            oReturn.success = True
            oReturn.message = ""
            oReturn.codigo = oSqlParameter(i).Value

        Catch SqlEx As SqlException
            oReturn.success = False
            oReturn.message = SqlEx.Message.ToString()
        Catch ex As Exception
            oReturn.success = False
            oReturn.message = ex.Message.ToString()
        End Try

        Return oReturn

    End Function

#End Region

#Region "::: PCM - CHECKLIST :::"

    Public Function getCheckListFull(ByVal iCodigoEmpresa As Integer,
                                     ByVal iCodigoUnidade As Integer,
                                     ByVal lCodigoChecklist As Long,
                                     ByVal sTipo As String,
                                     ByVal lCodigoDocumento As Long,
                                     ByVal iIntervalo As Integer,
                                     Optional ByVal lCodigoEquipamento As Long = -1) As pwaChecklist

        Dim oSqlParameter(6) As SqlParameter
        Dim oReader As SqlDataReader = Nothing
        Dim i As Integer = 0

        Dim oReturn As New pwaChecklist With {
        .codigoEmpresa = iCodigoEmpresa,
        .codigoUnidade = iCodigoUnidade,
        .codigoChecklist = lCodigoChecklist,
        .grupo = New List(Of pwaChecklistGrupo)()
    }

        ' Dicionários para montagem rápida (sem loops chamando o banco)
        Dim gruposMap As New Dictionary(Of String, pwaChecklistGrupo)(StringComparer.OrdinalIgnoreCase)
        Dim subgruposMap As New Dictionary(Of String, pwaChecklistSubGrupo)(StringComparer.OrdinalIgnoreCase)
        Dim arquivosPorItem As New Dictionary(Of Integer, List(Of pwaImagem))()

        Try
            ' Parametros
            oSqlParameter(i) = New SqlParameter("codigo_empresa", SqlDbType.SmallInt) With {.Value = iCodigoEmpresa} : i += 1
            oSqlParameter(i) = New SqlParameter("codigo_unidade", SqlDbType.Int) With {.Value = iCodigoUnidade} : i += 1
            oSqlParameter(i) = New SqlParameter("codigo_checklist", SqlDbType.BigInt) With {.Value = lCodigoChecklist} : i += 1
            oSqlParameter(i) = New SqlParameter("tipo", SqlDbType.VarChar, 50) With {.Value = sTipo} : i += 1
            oSqlParameter(i) = New SqlParameter("codigo_documento", SqlDbType.BigInt) With {.Value = lCodigoDocumento} : i += 1
            oSqlParameter(i) = New SqlParameter("intervalo", SqlDbType.SmallInt) With {.Value = iIntervalo} : i += 1
            oSqlParameter(i) = New SqlParameter("codigo_equipamento", SqlDbType.BigInt) With {.Value = lCodigoEquipamento}

            ' 1 ida ao banco, 4 resultsets
            oReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_pwa_select_pcm_checklist_full", oSqlParameter)

            ' ==========================================================
            ' RESULTSET 1: GRUPOS
            ' colunas esperadas:
            ' descricao, possui_subgrupo, totalOk, total, status, codigo_apartamento, codigo_tipo_governanca
            ' ==========================================================
            While oReader.Read()
                Dim desc As String = If(Convert.IsDBNull(oReader("descricao")), "", Convert.ToString(oReader("descricao")))

                Dim g As New pwaChecklistGrupo With {
                .descricao = desc,
                .total = If(Convert.IsDBNull(oReader("total")), 0, Convert.ToInt32(oReader("total"))),
                .totalOk = If(Convert.IsDBNull(oReader("totalOk")), 0, Convert.ToInt32(oReader("totalOk"))),
                .subgrupo = New List(Of pwaChecklistSubGrupo)(),
                .checklist = New List(Of pwaChecklistItem)()
            }

                oReturn.grupo.Add(g)
                If Not gruposMap.ContainsKey(desc) Then gruposMap.Add(desc, g)
            End While

            ' Vai para o próximo resultset
            If Not oReader.NextResult() Then Return oReturn

            ' ==========================================================
            ' RESULTSET 2: SUBGRUPOS
            ' colunas esperadas:
            ' grupo, subgrupo, totalOk, total
            ' ==========================================================
            While oReader.Read()
                Dim grupoDesc As String = If(Convert.IsDBNull(oReader("grupo")), "", Convert.ToString(oReader("grupo")))
                Dim subDesc As String = If(Convert.IsDBNull(oReader("subgrupo")), "", Convert.ToString(oReader("subgrupo")))

                Dim s As New pwaChecklistSubGrupo With {
                .descricao = subDesc,
                .total = If(Convert.IsDBNull(oReader("total")), 0, Convert.ToInt32(oReader("total"))),
                .totalOk = If(Convert.IsDBNull(oReader("totalOk")), 0, Convert.ToInt32(oReader("totalOk"))),
                .checklist = New List(Of pwaChecklistItem)()
            }

                ' Adiciona no grupo pai
                If gruposMap.ContainsKey(grupoDesc) Then
                    gruposMap(grupoDesc).subgrupo.Add(s)
                End If

                ' Chave única do subgrupo (grupo|subgrupo) para localizar depois
                Dim key As String = $"{grupoDesc}||{subDesc}"
                If Not subgruposMap.ContainsKey(key) Then subgruposMap.Add(key, s)
            End While

            If Not oReader.NextResult() Then Return oReturn

            ' ==========================================================
            ' RESULTSET 3: ITENS
            ' colunas esperadas:
            ' grupo, subgrupo, codigo_tipo_checklist, codigo, checklist, descricao, numero_digitos,
            ' allow_picture, uom, resultado, observacao, ordem_servico, prazo, color
            ' ==========================================================
            While oReader.Read()
                Dim grupoDesc As String = If(Convert.IsDBNull(oReader("grupo")), "", Convert.ToString(oReader("grupo")))
                Dim subDesc As String = If(Convert.IsDBNull(oReader("subgrupo")), "", Convert.ToString(oReader("subgrupo")))

                Dim codigoItem As Integer = If(Convert.IsDBNull(oReader("codigo")), 0, Convert.ToInt32(oReader("codigo")))

                Dim item As New pwaChecklistItem With {
                .codigoTipoChecklist = If(Convert.IsDBNull(oReader("codigo_tipo_checklist")), 0, Convert.ToInt32(oReader("codigo_tipo_checklist"))),
                .codigo = codigoItem,
                .checklist = If(Convert.IsDBNull(oReader("checklist")), "", Convert.ToString(oReader("checklist"))),
                .descricao = If(Convert.IsDBNull(oReader("descricao")), "", Convert.ToString(oReader("descricao"))),
                .numeroDigitos = If(Convert.IsDBNull(oReader("numero_digitos")), 0, Convert.ToInt32(oReader("numero_digitos"))),
                .allowPicture = If(Convert.IsDBNull(oReader("allow_picture")), 0, Convert.ToInt32(oReader("allow_picture"))),
                .uom = If(Convert.IsDBNull(oReader("uom")), "", Convert.ToString(oReader("uom"))),
                .resultado = If(Convert.IsDBNull(oReader("resultado")), "", Convert.ToString(oReader("resultado"))),
                .observacao = If(Convert.IsDBNull(oReader("observacao")), "", Convert.ToString(oReader("observacao"))),
                .ordemServico = If(Convert.IsDBNull(oReader("ordem_servico")), False, Convert.ToBoolean(oReader("ordem_servico"))),
                .prazo = If(oReader.GetOrdinal("prazo") >= 0 AndAlso Not Convert.IsDBNull(oReader("prazo")), Convert.ToString(oReader("prazo")), ""),
                .color = If(Convert.IsDBNull(oReader("color")), "#000000", Convert.ToString(oReader("color"))),
                .associarEquipamento = SafeGetLong(oReader, "associar_equipamento"),
                .codigoEquipamento = SafeGetLong(oReader, "codigo_equipamento"),
                .arquivo = New List(Of pwaImagem)()
            }

                ' Alocar item no lugar certo:
                ' - se tem subgrupo (subDesc <> ""), vai para subgrupo.checklist
                ' - senão, vai para grupo.checklist
                If Not String.IsNullOrWhiteSpace(subDesc) Then
                    Dim key As String = $"{grupoDesc}||{subDesc}"
                    If subgruposMap.ContainsKey(key) Then
                        subgruposMap(key).checklist.Add(item)
                    ElseIf gruposMap.ContainsKey(grupoDesc) Then
                        ' fallback
                        gruposMap(grupoDesc).checklist.Add(item)
                    End If
                Else
                    If gruposMap.ContainsKey(grupoDesc) Then
                        gruposMap(grupoDesc).checklist.Add(item)
                    End If
                End If

                ' (opcional) já cria “slot” do dicionário de arquivos pra anexar depois sem if
                If Not arquivosPorItem.ContainsKey(codigoItem) Then
                    arquivosPorItem.Add(codigoItem, item.arquivo)
                End If
            End While

            If Not oReader.NextResult() Then Return oReturn

            ' ==========================================================
            ' RESULTSET 4: ARQUIVOS
            ' colunas esperadas:
            ' codigo_item, url, extensao
            ' ==========================================================
            While oReader.Read()
                Dim codigoItem As Integer = If(Convert.IsDBNull(oReader("codigo_item")), 0, Convert.ToInt32(oReader("codigo_item")))
                If codigoItem <= 0 Then Continue While

                Dim img As New pwaImagem With {
                .url = If(Convert.IsDBNull(oReader("url")), "", Convert.ToString(oReader("url"))),
                .extensao = If(Convert.IsDBNull(oReader("extensao")), "", Convert.ToString(oReader("extensao")))
            }

                If arquivosPorItem.ContainsKey(codigoItem) Then
                    arquivosPorItem(codigoItem).Add(img)
                End If
            End While

            Return oReturn

        Catch SqlEx As SqlException
            Throw
        Catch ex As Exception
            Throw
        Finally
            If oReader IsNot Nothing AndAlso Not oReader.IsClosed Then
                oReader.Close()
            End If
            oReader = Nothing
        End Try

    End Function


    Public Function getCheckList(ByVal iCodigoEmpresa As Integer,
                                 ByVal iCodigoUnidade As Integer,
                                 ByVal lCodigoChecklist As Long,
                                 ByVal sTipo As String,
                                 ByVal lCodigoDocumento As Long,
                                 ByVal iIntervalo As Integer,
                                 Optional ByVal lCodigoEquipamento As Long = -1) As pwaChecklist

        'Variaveis Locais
        Dim oSqlParameter(6) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New pwaChecklist
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Checklist
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_checklist"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoChecklist : i += 1

            'Seta Parametros - Tipo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "tipo"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sTipo : i += 1

            'Seta Parametros - Código Documento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_documento"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoDocumento : i += 1

            'Seta Parametros - Intervalo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "intervalo"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iIntervalo : i += 1

            'Seta Parametros - Código Equipamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_equipamento"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoEquipamento

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_pwa_select_pcm_checklist_grupo", oSqlParameter)

            If oSqlDataReader.HasRows Then

                oReturn.codigoEmpresa = iCodigoEmpresa
                oReturn.codigoUnidade = iCodigoUnidade
                oReturn.codigoChecklist = lCodigoChecklist
                oReturn.grupo = New List(Of pwaChecklistGrupo)

                While oSqlDataReader.Read

                    Dim oInfo As New pwaChecklistGrupo

                    oInfo.descricao = oSqlDataReader.Item("grupo")
                    oInfo.total = oSqlDataReader.Item("total")
                    oInfo.totalOk = oSqlDataReader.Item("total_ok")

                    If oSqlDataReader.Item("possui_subgrupo") > 0 Then

                        oInfo.checklist = New List(Of pwaChecklistItem)
                        oInfo.subgrupo = getCheckListSubgrupo(iCodigoEmpresa:=iCodigoEmpresa,
                                                              iCodigoUnidade:=iCodigoUnidade,
                                                              lCodigoChecklist:=lCodigoChecklist,
                                                              sGrupo:=oSqlDataReader.Item("grupo"),
                                                              sTipo:=sTipo,
                                                              lCodigoDocumento:=lCodigoDocumento,
                                                              iIntervalo:=iIntervalo,
                                                              iStatus:=oSqlDataReader.Item("status"),
                                                              iCodigoApartamento:=oSqlDataReader.Item("codigo_apartamento"),
                                                              iCodigoTipoGovernanca:=oSqlDataReader.Item("codigo_tipo_governanca"),
                                                              lCodigoEquipamento:=lCodigoEquipamento)

                    Else

                        oInfo.subgrupo = New List(Of pwaChecklistSubGrupo)
                        oInfo.checklist = getCheckListItem(iCodigoEmpresa:=iCodigoEmpresa,
                                                           iCodigoUnidade:=iCodigoUnidade,
                                                           lCodigoChecklist:=lCodigoChecklist,
                                                           sGrupo:=oSqlDataReader.Item("grupo"),
                                                           sSubgrupo:="",
                                                           sTipo:=sTipo,
                                                           lCodigoDocumento:=lCodigoDocumento,
                                                           iIntervalo:=iIntervalo,
                                                           iStatus:=oSqlDataReader.Item("status"),
                                                           iCodigoApartamento:=oSqlDataReader.Item("codigo_apartamento"),
                                                           iCodigoTipoGovernanca:=oSqlDataReader.Item("codigo_tipo_governanca"),
                                                           lCodigoEquipamento:=lCodigoEquipamento)

                    End If

                    oReturn.grupo.Add(oInfo)

                End While

            End If


            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function getCheckListSubgrupo(ByVal iCodigoEmpresa As Integer,
                                         ByVal iCodigoUnidade As Integer,
                                         ByVal lCodigoChecklist As Long,
                                         ByVal sGrupo As String,
                                         ByVal sTipo As String,
                                         ByVal lCodigoDocumento As Long,
                                         ByVal iIntervalo As Integer,
                                         ByVal iStatus As Integer,
                                         ByVal iCodigoApartamento As Integer,
                                         ByVal iCodigoTipoGovernanca As Integer,
                                         ByVal lCodigoEquipamento As Long) As List(Of pwaChecklistSubGrupo)

        'Variaveis Locais
        Dim oSqlParameter(10) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New List(Of pwaChecklistSubGrupo)
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Checklist
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_checklist"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoChecklist : i += 1

            'Seta Parametros - Grupo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "grupo"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sGrupo : i += 1

            'Seta Parametros - Tipo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "tipo"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sTipo : i += 1

            'Seta Parametros - Código Documento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_documento"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoDocumento : i += 1

            'Seta Parametros - Intervalo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "intervalo"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iIntervalo : i += 1

            'Seta Parametros - Status
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "status"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iStatus : i += 1

            'Seta Parametros - Código Apartamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_apartamento"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoApartamento : i += 1

            'Seta Parametros - Código Tipo Governança
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_tipo_governanca"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoTipoGovernanca : i += 1

            'Seta Parametros - Código Equipamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_equipamento"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoEquipamento

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_pwa_select_pcm_checklist_subgrupo", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New pwaChecklistSubGrupo

                oInfo.descricao = oSqlDataReader.Item("subgrupo")
                oInfo.total = oSqlDataReader.Item("total")
                oInfo.totalOk = oSqlDataReader.Item("total_ok")
                oInfo.checklist = getCheckListItem(iCodigoEmpresa:=iCodigoEmpresa,
                                                   iCodigoUnidade:=iCodigoUnidade,
                                                   lCodigoChecklist:=lCodigoChecklist,
                                                   sGrupo:=sGrupo,
                                                   sSubgrupo:=oSqlDataReader.Item("subgrupo"),
                                                   sTipo:=sTipo,
                                                   lCodigoDocumento:=lCodigoDocumento,
                                                   iIntervalo:=iIntervalo,
                                                   iStatus:=iStatus,
                                                   iCodigoApartamento:=iCodigoApartamento,
                                                   iCodigoTipoGovernanca:=iCodigoTipoGovernanca,
                                                   lCodigoEquipamento:=lCodigoEquipamento)

                oReturn.Add(oInfo)

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function getCheckListItem(ByVal iCodigoEmpresa As Integer,
                                     ByVal iCodigoUnidade As Integer,
                                     ByVal lCodigoChecklist As Long,
                                     ByVal sGrupo As String,
                                     ByVal sSubgrupo As String,
                                     ByVal sTipo As String,
                                     ByVal lCodigoDocumento As Long,
                                     ByVal iIntervalo As Integer,
                                     ByVal iStatus As Integer,
                                     ByVal iCodigoApartamento As Integer,
                                     ByVal iCodigoTipoGovernanca As Integer,
                                     ByVal lCodigoEquipamento As Long) As List(Of pwaChecklistItem)

        'Variaveis Locais
        Dim oSqlParameter(11) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New List(Of pwaChecklistItem)
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Checklist
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_checklist"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoChecklist : i += 1

            'Seta Parametros - Grupo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "grupo"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sGrupo : i += 1

            'Seta Parametros - Subgrupo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "subgrupo"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sSubgrupo : i += 1

            'Seta Parametros - Tipo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "tipo"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sTipo : i += 1

            'Seta Parametros - Código Documento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_documento"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoDocumento : i += 1

            'Seta Parametros - Intervalo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "intervalo"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iIntervalo : i += 1

            'Seta Parametros - Status
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "status"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iStatus : i += 1

            'Seta Parametros - Código Apartamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_apartamento"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoApartamento : i += 1

            'Seta Parametros - Código Tipo Governança
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_tipo_governanca"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoTipoGovernanca : i += 1

            'Seta Parametros - Código Equipamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_equipamento"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoEquipamento

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_pwa_select_pcm_checklist", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New pwaChecklistItem

                oInfo.codigoTipoChecklist = oSqlDataReader.Item("codigo_tipo_checklist")
                oInfo.codigo = oSqlDataReader.Item("codigo")
                oInfo.checklist = oSqlDataReader.Item("checklist")
                oInfo.descricao = oSqlDataReader.Item("descricao")
                oInfo.numeroDigitos = oSqlDataReader.Item("numero_digitos")
                oInfo.allowPicture = oSqlDataReader.Item("allow_picture")
                oInfo.uom = oSqlDataReader.Item("uom")
                oInfo.resultado = oSqlDataReader.Item("resultado")
                oInfo.observacao = oSqlDataReader.Item("observacao")
                oInfo.ordemServico = oSqlDataReader.Item("ordem_servico")
                oInfo.color = oSqlDataReader.Item("color")
                oInfo.associarEquipamento = oSqlDataReader.Item("associar_equipamento")
                oInfo.arquivo = getCheckListItemArquivo(iCodigoEmpresa:=iCodigoEmpresa,
                                                        iCodigoUnidade:=iCodigoUnidade,
                                                        lCodigoChecklist:=lCodigoChecklist,
                                                        iCodigoChecklistItem:=oSqlDataReader.Item("codigo"),
                                                        sTipo:=sTipo,
                                                        lCodigoDocumento:=lCodigoDocumento)

                oReturn.Add(oInfo)

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function getCheckListItemArquivo(ByVal iCodigoEmpresa As Integer,
                                            ByVal iCodigoUnidade As Integer,
                                            ByVal lCodigoChecklist As Long,
                                            ByVal iCodigoChecklistItem As Integer,
                                            ByVal sTipo As String,
                                            ByVal lCodigoDocumento As Long) As List(Of pwaImagem)

        'Variaveis Locais
        Dim oSqlParameter(4) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New List(Of pwaImagem)
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Checklist Item
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_checklist_item"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoChecklistItem : i += 1

            'Seta Parametros - Tipo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "tipo"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sTipo : i += 1

            'Seta Parametros - Código Documento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_documento"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoDocumento

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_pwa_select_pcm_checklist_arquivo", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New pwaImagem

                oInfo.url = oSqlDataReader.Item("url")
                oInfo.extensao = oSqlDataReader.Item("extensao")

                oReturn.Add(oInfo)

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function insertApontamentoChecklist(ByVal lCodigoDocumento As Long,
                                               ByVal sTipo As String,
                                               ByVal oChecklistApontamento As pwaChecklistApontamento) As pwaApontamentoResponse

        'Variaveis Locais
        Dim oSqlParameter(9) As SqlParameter
        Dim oReturn As New pwaApontamentoResponse
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Documento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_documento"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoDocumento : i += 1

            'Seta Parametros - Tipo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "tipo"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sTipo : i += 1

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = oChecklistApontamento.codigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oChecklistApontamento.codigoUnidade : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oChecklistApontamento.codigoUsuario : i += 1

            'Seta Parametros - Código Checklist
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_checklist"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = oChecklistApontamento.codigoChecklist : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oChecklistApontamento.codigo : i += 1

            'Seta Parametros - Resultado
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "resultado"
            oSqlParameter(i).SqlDbType = SqlDbType.NVarChar
            oSqlParameter(i).Value = oChecklistApontamento.resultado : i += 1

            'Seta Parametros - Observação
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "observacao"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 250
            oSqlParameter(i).Value = oChecklistApontamento.observacao : i += 1

            'Seta Parametros - Intervalo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "intervalo"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = oChecklistApontamento.intervalo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_pwa_insert_checklist", oSqlParameter)

            oReturn.success = True
            oReturn.message = ""

        Catch SqlEx As SqlException
            oReturn.success = False
            oReturn.message = SqlEx.Message.ToString()
        Catch ex As Exception
            oReturn.success = False
            oReturn.message = ex.Message.ToString()
        End Try

        Return oReturn

    End Function

    Public Sub insertApontamentoChecklistNew(ByVal lCodigoDocumento As Long,
                                             ByVal iCodigoEmpresa As Integer,
                                             ByVal iCodigoUnidade As Integer,
                                             ByVal iCodigoUsuario As Integer,
                                             ByVal lCodigoChecklist As Long,
                                             ByVal iIntervalo As Integer,
                                             ByVal sTipo As String,
                                             ByVal oChecklist As pwaChecklistItem,
                                             Optional ByVal bExoval As Boolean = False)

        'Variaveis Locais
        Dim oSqlParameter(11) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Documento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_documento"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoDocumento : i += 1

            'Seta Parametros - Tipo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "tipo"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sTipo : i += 1

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código Checklist
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_checklist"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoChecklist : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oChecklist.codigo : i += 1

            'Seta Parametros - Resultado
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "resultado"
            oSqlParameter(i).SqlDbType = SqlDbType.NVarChar
            oSqlParameter(i).Value = oChecklist.resultado : i += 1

            'Seta Parametros - Observação
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "observacao"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 250
            oSqlParameter(i).Value = oChecklist.observacao : i += 1

            'Seta Parametros - Prazo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "prazo"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 250
            oSqlParameter(i).Value = oChecklist.prazo : i += 1

            'Seta Parametros - Intervalo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "intervalo"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iIntervalo : i += 1

            'Seta Parametros - Enxoval
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "enxoval"
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bExoval

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_pwa_insert_checklist", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub insertApontamentoPlanoAcao(ByVal lCodigoDocumento As Long,
                                          ByVal iCodigoEmpresa As Integer,
                                          ByVal iCodigoUnidade As Integer,
                                          ByVal iCodigoUsuario As Integer,
                                          ByVal sTipo As String)

        'Variaveis Locais
        Dim oSqlParameter(4) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Documento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_documento"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoDocumento : i += 1

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Tipo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "tipo"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sTipo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_pwa_insert_plano_acao", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region

#Region "::: PCM - NOTIFICAÇÃO :::"

    Public Function getNotificacao(ByVal iCodigoEmpresa As Integer,
                                   ByVal iCodigoUnidade As Integer,
                                   ByVal iCodigoUsuario As Integer) As List(Of pwaListaNotificacao)

        'Variaveis Locais
        Dim oSqlParameter(2) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New List(Of pwaListaNotificacao)
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_pwa_select_pcm_notificacao", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New pwaListaNotificacao

                oInfo.codigo = oSqlDataReader("codigo")
                oInfo.cabecalho = oSqlDataReader("cabecalho")
                oInfo.info = oSqlDataReader("info")
                oInfo.descricao = oSqlDataReader("descricao")
                oInfo.autor = oSqlDataReader("autor")
                oInfo.lido = oSqlDataReader("lido")

                oReturn.Add(oInfo)

            End While


            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Sub readNotificacao(ByVal iCodigoEmpresa As Integer,
                               ByVal iCodigoUnidade As Integer,
                               ByVal lCodigo As Long)

        'Variaveis Locais
        Dim oSqlParameter(2) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_pwa_update_pcm_notificacao", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region

#Region "::: LOG BOOK :::"

    Public Function getLogBook(ByVal iCodigoEmpresa As Integer,
                               ByVal iCodigoUnidade As Integer,
                               ByVal iCodigoUsuario As Integer,
                               ByVal sDataInicio As String,
                               ByVal sDataTermino As String) As List(Of pwaListaLogBook)

        'Variaveis Locais
        Dim oSqlParameter(4) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New List(Of pwaListaLogBook)
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Data Início
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "data_inicio"
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataInicio : i += 1

            'Seta Parametros - Data Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "data_termino"
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataTermino

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_pwa_select_pcm_log_book", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New pwaListaLogBook

                oInfo.codigo = oSqlDataReader("codigo")
                oInfo.usuario = oSqlDataReader("usuario")
                oInfo.data = oSqlDataReader("data")
                oInfo.descricao = oSqlDataReader("descricao")

                oReturn.Add(oInfo)

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Sub insertLogBook(ByVal iCodigoEmpresa As Integer,
                             ByVal iCodigoUnidade As Integer,
                             ByVal iCodigoUsuario As Integer,
                             ByVal sDescricao As String)

        'Variaveis Locais
        Dim oSqlParameter(3) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = sDescricao

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_pwa_insert_log_book", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region

    '#Region "::: AUDITORIA :::"

    '    Public Function getListAuditoriaList(ByVal iCodigoEmpresa As Integer,
    '                                         ByVal iCodigoUnidade As Integer,
    '                                         ByVal iCodigoFuncionario As Integer,
    '                                         ByVal iPage As Integer) As pwaAuditoriaList

    '        'Variaveis Locais
    '        Dim oSqlParameter(3) As SqlParameter
    '        Dim oSqlDataReader As SqlDataReader
    '        Dim oReturn As New pwaAuditoriaList
    '        Dim i As Integer = 0

    '        Try

    '            'Seta Parametros - Código Empresa
    '            oSqlParameter(i) = New SqlParameter
    '            oSqlParameter(i).Direction = ParameterDirection.Input
    '            oSqlParameter(i).ParameterName = "codigo_empresa"
    '            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
    '            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

    '            'Seta Parametros - Código Unidade
    '            oSqlParameter(i) = New SqlParameter
    '            oSqlParameter(i).Direction = ParameterDirection.Input
    '            oSqlParameter(i).ParameterName = "codigo_unidade"
    '            oSqlParameter(i).SqlDbType = SqlDbType.Int
    '            oSqlParameter(i).Value = iCodigoUnidade : i += 1

    '            'Seta Parametros - Código Funcionário
    '            oSqlParameter(i) = New SqlParameter
    '            oSqlParameter(i).Direction = ParameterDirection.Input
    '            oSqlParameter(i).ParameterName = "codigo_funcionario"
    '            oSqlParameter(i).SqlDbType = SqlDbType.Int
    '            oSqlParameter(i).Value = iCodigoFuncionario : i += 1

    '            'Seta Parametros - Código Funcionário
    '            oSqlParameter(i) = New SqlParameter
    '            oSqlParameter(i).Direction = ParameterDirection.Input
    '            oSqlParameter(i).ParameterName = "page"
    '            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
    '            oSqlParameter(i).Value = iPage

    '            'Executa Query
    '            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_pwa_select_auditoria_list", oSqlParameter)

    '            While oSqlDataReader.Read

    '                oReturn.totalPages = oSqlDataReader.Item("total_pages")
    '                oReturn.totalResults = oSqlDataReader.Item("total_results")
    '                oReturn.page = oSqlDataReader.Item("page")
    '                oReturn.results = getListAuditoria(iCodigoEmpresa:=iCodigoEmpresa,
    '                                                   iCodigoUnidade:=iCodigoUnidade,
    '                                                   iCodigoFuncionario:=iCodigoFuncionario,
    '                                                   iPage:=iPage)

    '            End While


    '            'Fecha o SqlDataReader
    '            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

    '            'Retorno
    '            Return oReturn

    '        Catch SqlEx As SqlException
    '            Throw SqlEx
    '        Catch ex As Exception
    '            Throw ex
    '        End Try

    '    End Function

    '    Public Function getListAuditoria(ByVal iCodigoEmpresa As Integer,
    '                                     ByVal iCodigoUnidade As Integer,
    '                                     ByVal iCodigoFuncionario As Integer,
    '                                     ByVal iPage As Integer) As List(Of pwaAuditoria)

    '        'Variaveis Locais
    '        Dim oSqlParameter(3) As SqlParameter
    '        Dim oSqlDataReader As SqlDataReader
    '        Dim oReturn As New List(Of pwaAuditoria)
    '        Dim i As Integer = 0

    '        Try

    '            'Seta Parametros - Código Empresa
    '            oSqlParameter(i) = New SqlParameter
    '            oSqlParameter(i).Direction = ParameterDirection.Input
    '            oSqlParameter(i).ParameterName = "codigo_empresa"
    '            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
    '            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

    '            'Seta Parametros - Código Unidade
    '            oSqlParameter(i) = New SqlParameter
    '            oSqlParameter(i).Direction = ParameterDirection.Input
    '            oSqlParameter(i).ParameterName = "codigo_unidade"
    '            oSqlParameter(i).SqlDbType = SqlDbType.Int
    '            oSqlParameter(i).Value = iCodigoUnidade : i += 1

    '            'Seta Parametros - Código Funcionário
    '            oSqlParameter(i) = New SqlParameter
    '            oSqlParameter(i).Direction = ParameterDirection.Input
    '            oSqlParameter(i).ParameterName = "codigo_funcionario"
    '            oSqlParameter(i).SqlDbType = SqlDbType.Int
    '            oSqlParameter(i).Value = iCodigoFuncionario : i += 1

    '            'Seta Parametros - Código Funcionário
    '            oSqlParameter(i) = New SqlParameter
    '            oSqlParameter(i).Direction = ParameterDirection.Input
    '            oSqlParameter(i).ParameterName = "page"
    '            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
    '            oSqlParameter(i).Value = iPage

    '            'Executa Query
    '            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_pwa_select_auditoria", oSqlParameter)

    '            While oSqlDataReader.Read

    '                Dim oInfo As New pwaAuditoria

    '                oInfo.codigoAuditoria = oSqlDataReader.Item("codigo_auditoria")
    '                oInfo.codigoChecklist = oSqlDataReader.Item("codigo_checklist")
    '                oInfo.numeroDocumento = oSqlDataReader.Item("numero_documento")
    '                oInfo.descricao = oSqlDataReader.Item("descricao")
    '                oInfo.dataCriacao = oSqlDataReader.Item("data_criacao")
    '                oInfo.executor = oSqlDataReader.Item("executor")

    '                'Status
    '                oInfo.status = New pwaStatus
    '                oInfo.status.codigo = oSqlDataReader.Item("status_codigo")
    '                oInfo.status.descricao = oSqlDataReader.Item("status_descricao")
    '                oInfo.status.cssClass = oSqlDataReader.Item("status_css_class")

    '                oReturn.Add(oInfo)

    '            End While


    '            'Fecha o SqlDataReader
    '            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

    '            'Retorno
    '            Return oReturn

    '        Catch SqlEx As SqlException
    '            Throw SqlEx
    '        Catch ex As Exception
    '            Throw ex
    '        End Try

    '    End Function

    '#End Region

End Class
