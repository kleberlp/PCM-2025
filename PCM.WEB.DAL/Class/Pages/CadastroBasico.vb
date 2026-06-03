Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.DirectoryServices
Imports System.DirectoryServices.ActiveDirectory
Imports System.IO
Imports System.Net
Imports System.Net.WebRequestMethods
Imports System.Text
Imports MS.Internal.Text.TextInterface
Imports Newtonsoft.Json
Imports OfficeOpenXml
Imports OfficeOpenXml.FormulaParsing.Excel.Functions
Imports PCM.WEB.DAL.SQLHelper
Imports PCM.WEB.MODELS

Public Class CadastroBasico

    Private sConnection As String

    Sub New(ByVal sCon As String)
        sConnection = sCon
    End Sub

#Region "::: CEP :::"

    Public Function CEP(ByVal sCEP As String) As List(Of CEP)

        Try

            'Váriaveis Locais
            Dim oCEP As New List(Of CEP)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(0) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "cep"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sCEP

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_static_dados_cep", oSqlParameter)

            While oSqlDataReader.Read

                Dim oCEPInfo As New CEP

                oCEPInfo.tipo_logradouro = oSqlDataReader.Item("tipo_logradouro")
                oCEPInfo.logradouro = oSqlDataReader.Item("logradouro")
                oCEPInfo.bairro = oSqlDataReader.Item("bairro")
                oCEPInfo.uf = oSqlDataReader.Item("uf")
                oCEPInfo.municipio = oSqlDataReader.Item("municipio")

                oCEP.Add(oCEPInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCEP

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function CEPInternet(ByVal sCEP As String) As List(Of CEP)

        'Variaveis Locais
        Dim oCEP As New List(Of CEP)
        Dim oDataSet As DataSet
        Dim sResultado As String

        Try

            'Cria a requisão HTTP
            Dim oWebRequest As WebRequest = WebRequest.Create("http://cep.republicavirtual.com.br/web_cep.php?cep=" + sCEP.Replace("-", "").Trim() + "&formato=xml")

            'Executa a requisão do XML do endereço, utilizando Proxy
            Dim oXmlStream As Stream = oWebRequest.GetResponse().GetResponseStream()

            oDataSet = New DataSet()
            'Usa o Stream obtido pela requisão como fonte do DataSet
            oDataSet.ReadXml(oXmlStream)

            If Not IsNothing(oDataSet) Then

                If (oDataSet.Tables(0).Rows.Count > 0) Then

                    sResultado = oDataSet.Tables(0).Rows(0).Item("resultado").ToString()

                    Select Case sResultado

                        Case "1"

                            Dim oCEPInfo As New CEP

                            oCEPInfo.uf = oDataSet.Tables(0).Rows(0).Item("uf").ToString().Trim().ToUpper()
                            oCEPInfo.municipio = oDataSet.Tables(0).Rows(0).Item("cidade").ToString().Trim().ToUpper()
                            oCEPInfo.bairro = oDataSet.Tables(0).Rows(0).Item("bairro").ToString().Trim().ToUpper()
                            oCEPInfo.tipo_logradouro = oDataSet.Tables(0).Rows(0).Item("tipo_logradouro").ToString().Trim().ToUpper()
                            oCEPInfo.logradouro = oDataSet.Tables(0).Rows(0).Item("logradouro").ToString().Trim().ToUpper()

                            oCEP.Add(oCEPInfo)

                        Case "2"
                            Dim oCEPInfo As New CEP

                            oCEPInfo.uf = oDataSet.Tables(0).Rows(0).Item("uf").ToString().Trim().ToUpper()
                            oCEPInfo.municipio = oDataSet.Tables(0).Rows(0).Item("cidade").ToString().Trim().ToUpper()

                            oCEP.Add(oCEPInfo)

                    End Select

                End If

            End If

            'Retorno da Função
            Return oCEP

        Catch ex As Exception
            Throw New Exception("Falha ao Buscar o Cep" & vbCrLf & ex.ToString)
        End Try

    End Function

#End Region

#Region "::: APARTAMENTO :::"

    Public Sub InsertApartamento(ByVal iCodigoEmpresa As Integer,
                                 ByVal iCodigoUsuario As Integer,
                                 ByVal iCodigoUnidade As Integer,
                                 ByVal iCodigoResponsavelApartamento As Integer,
                                 ByVal iCodigoSetor As Integer,
                                 ByVal iCodigoTipoApartamento As Integer,
                                 ByVal sApartamento As String,
                                 ByVal sBloco As String,
                                 ByVal iAndar As Integer,
                                 ByVal sDescritivo As String,
                                 ByVal iCodigoTipoCama As Integer,
                                 ByVal iQuantidadeCama As Integer,
                                 ByVal dMetragem As Double,
                                 ByVal dCargaTermica As Double,
                                 ByVal sDescricaoAtividade As String,
                                 ByVal iNumeroPessoasFixas As Integer,
                                 ByVal iNumeroPessoasVolantes As Integer,
                                 ByVal bAtivo As Boolean,
                                 ByVal sDataUltimaManutencao As String)

        'Variaveis Locais
        Dim oSqlParameter(19) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Responsável Apartamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_responsavel_apartamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoResponsavelApartamento : i += 1

            'Seta Parametros - Setor
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_setor"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoSetor : i += 1

            'Seta Parametros - Tipo Apartamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_tipo_apartamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoTipoApartamento : i += 1

            'Seta Parametros - Apartamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "apartamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sApartamento : i += 1

            'Seta Parametros - Bloco
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "bloco"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sBloco : i += 1

            'Seta Parametros - Andar
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "andar"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iAndar : i += 1

            'Seta Parametros - Código Tipo Cama
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_tipo_cama"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoTipoCama : i += 1

            'Seta Parametros - Quantidade Cama
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "quantidade_cama"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iQuantidadeCama : i += 1

            'Seta Parametros - Metragem
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "metragem"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = dMetragem : i += 1

            'Seta Parametros - Carga Térmica
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "carga_termica"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = dCargaTermica : i += 1

            'Seta Parametros - Descrição da Atividade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao_atividade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 150
            oSqlParameter(i).Value = sDescricaoAtividade : i += 1

            'Seta Parametros - Número de Pessoas Fixas
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "numero_pessoas_fixas"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iNumeroPessoasFixas : i += 1

            'Seta Parametros - Número de Pessoas Volantes
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "numero_pessoas_volantes"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iNumeroPessoasVolantes : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo : i += 1

            'Seta Parametros - Descritivo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descritivo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 2000
            oSqlParameter(i).Value = sDescritivo : i += 1

            'Seta Parametros - Data Ultima Manutenção
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_ultima_manutencao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = IIf(IsDate(sDataUltimaManutencao), sDataUltimaManutencao, DBNull.Value)

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_cadastro_basico_apartamento", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateApartamento(ByVal iCodigoEmpresa As Integer,
                                 ByVal iCodigoUsuario As Integer,
                                 ByVal iCodigoUnidade As Integer,
                                 ByVal iCodigoResponsavelApartamento As Integer,
                                 ByVal iCodigoSetor As Integer,
                                 ByVal iCodigoTipoApartamento As Integer,
                                 ByVal sApartamento As String,
                                 ByVal sBloco As String,
                                 ByVal iAndar As Integer,
                                 ByVal sDescritivo As String,
                                 ByVal iCodigoTipoCama As Integer,
                                 ByVal iQuantidadeCama As Integer,
                                 ByVal dMetragem As Double,
                                 ByVal dCargaTermica As Double,
                                 ByVal sDescricaoAtividade As String,
                                 ByVal iNumeroPessoasFixas As Integer,
                                 ByVal iNumeroPessoasVolantes As Integer,
                                 ByVal bAtivo As Boolean,
                                 ByVal sDataUltimaManutencao As String,
                                 ByVal iCodigo As Integer)

        'Variaveis Locais
        Dim oSqlParameter(19) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Responsável Apartamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_responsavel_apartamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoResponsavelApartamento : i += 1

            'Seta Parametros - Setor
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_setor"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoSetor : i += 1

            'Seta Parametros - Tipo Apartamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_tipo_apartamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoTipoApartamento : i += 1

            'Seta Parametros - Apartamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "apartamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sApartamento : i += 1

            'Seta Parametros - Bloco
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "bloco"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sBloco : i += 1

            'Seta Parametros - Andar
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "andar"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iAndar : i += 1

            'Seta Parametros - Descritivo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descritivo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 2000
            oSqlParameter(i).Value = sDescritivo : i += 1

            'Seta Parametros - Código Tipo Cama
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_tipo_cama"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoTipoCama : i += 1

            'Seta Parametros - Quantidade Cama
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "quantidade_cama"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iQuantidadeCama : i += 1

            'Seta Parametros - Metragem
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "metragem"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = dMetragem : i += 1

            'Seta Parametros - Carga Térmica
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "carga_termica"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = dCargaTermica : i += 1

            'Seta Parametros - Descrição da Atividade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao_atividade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 150
            oSqlParameter(i).Value = sDescricaoAtividade : i += 1

            'Seta Parametros - Número de Pessoas Fixas
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "numero_pessoas_fixas"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iNumeroPessoasFixas : i += 1

            'Seta Parametros - Número de Pessoas Volantes
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "numero_pessoas_volantes"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iNumeroPessoasVolantes : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo : i += 1

            'Seta Parametros - Data Ultima Manutenção
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_ultima_manutencao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = IIf(IsDate(sDataUltimaManutencao), sDataUltimaManutencao, DBNull.Value) : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_cadastro_basico_apartamento", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub DeleteApartamento(ByVal iCodigoEmpresa As Integer,
                                 ByVal iCodigoUsuario As Integer,
                                 ByVal iCodigo As Integer)

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

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_cadastro_basico_apartamento", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InfoApartamento(ByVal iCodigoEmpresa As Integer,
                               ByVal iCodigo As Integer,
                               ByRef oApartamento As Apartamento)

        'Variaveis Locais
        Dim oSqlParameter(1) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_apartamento_dados", oSqlParameter)

            While oSqlDataReader.Read

                oApartamento = New Apartamento
                oApartamento.codigo = oSqlDataReader.Item("codigo")
                oApartamento.apartamento = oSqlDataReader.Item("apartamento")
                oApartamento.bloco = oSqlDataReader.Item("bloco")
                oApartamento.andar = oSqlDataReader.Item("andar")
                oApartamento.descritivo = oSqlDataReader.Item("descritivo")
                oApartamento.unidade = oSqlDataReader.Item("unidade")
                oApartamento.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oApartamento.codigo_responsavel_apartamento = oSqlDataReader.Item("codigo_responsavel_apartamento")
                oApartamento.setor = oSqlDataReader.Item("setor")
                oApartamento.codigo_setor = oSqlDataReader.Item("codigo_setor")
                oApartamento.tipo_apartamento = oSqlDataReader.Item("tipo_apartamento")
                oApartamento.codigo_tipo_apartamento = oSqlDataReader.Item("codigo_tipo_apartamento")
                oApartamento.tipo_cama = oSqlDataReader.Item("tipo_cama")
                oApartamento.codigo_tipo_cama = oSqlDataReader.Item("codigo_tipo_cama")
                oApartamento.quantidade_cama = oSqlDataReader.Item("quantidade_cama")
                oApartamento.responsavel_apartamento = oSqlDataReader.Item("responsavel_apartamento")
                oApartamento.codigo_responsavel_apartamento = oSqlDataReader.Item("codigo_responsavel_apartamento")
                oApartamento.metragem = oSqlDataReader.Item("metragem")
                oApartamento.carga_termica = oSqlDataReader.Item("carga_termica")
                oApartamento.descricao_atividade = oSqlDataReader.Item("descricao_atividade")
                oApartamento.numero_pessoas_fixas = oSqlDataReader.Item("numero_pessoas_fixas")
                oApartamento.numero_pessoas_volantes = oSqlDataReader.Item("numero_pessoas_volantes")
                oApartamento.codigo_tipo_unidade = oSqlDataReader.Item("codigo_tipo_unidade")
                oApartamento.ativo = oSqlDataReader.Item("ativo")
                oApartamento.data_ultima_manutencao = oSqlDataReader.Item("data_ultima_manutencao")

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function LoadApartamento(ByVal codigoEmpresa As Integer,
                                    ByVal codigoUsuario As Integer,
                                    ByVal codigoUnidade As Integer,
                                    ByVal codigoResponsavelApartamento As Integer,
                                    ByVal bloco As String,
                                    ByVal andar As String,
                                    ByVal ativo As Integer) As List(Of Apartamento)

        Try

            'Variaveis Locais
            Dim oReturn As New List(Of Apartamento)
            Dim oSqlParameter As SqlParameter() = {
                    CriarParametro("codigoEmpresa", SqlDbType.SmallInt, codigoEmpresa),
                    CriarParametro("codigoUsuario", SqlDbType.Int, codigoUsuario),
                    CriarParametro("codigoUnidade", SqlDbType.Int, codigoUnidade),
                    CriarParametro("codigoResponsavelApartamento", SqlDbType.SmallInt, codigoResponsavelApartamento),
                    CriarParametro("bloco", SqlDbType.VarChar, bloco),
                    CriarParametro("andar", SqlDbType.VarChar, andar),
                    CriarParametro("ativo", SqlDbType.Bit, IIf(ativo = 1, True, IIf(ativo = 0, False, DBNull.Value)))
                }


            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_apartamento", oSqlParameter)

                While oSqlDataReader.Read

                    Dim oInfo As New Apartamento

                    oInfo.apartamento = SafeGetString(oSqlDataReader, "apartamento")
                    oInfo.bloco = SafeGetString(oSqlDataReader, "bloco")
                    oInfo.andar = SafeGetString(oSqlDataReader, "andar")
                    oInfo.descritivo = SafeGetString(oSqlDataReader, "descritivo")
                    oInfo.codigo = SafeGetLong(oSqlDataReader, "codigo")
                    oInfo.codigo_unidade = SafeGetLong(oSqlDataReader, "codigo_unidade")
                    oInfo.unidade = SafeGetString(oSqlDataReader, "unidade")
                    oInfo.setor = SafeGetString(oSqlDataReader, "setor")
                    oInfo.tipo_apartamento = SafeGetString(oSqlDataReader, "tipo_apartamento")
                    oInfo.codigo_tipo_apartamento = SafeGetLong(oSqlDataReader, "codigo_tipo_apartamento")
                    oInfo.tipo_cama = SafeGetString(oSqlDataReader, "tipo_cama")
                    oInfo.codigo_tipo_cama = SafeGetLong(oSqlDataReader, "codigo_tipo_cama")
                    oInfo.quantidade_cama = SafeGetLong(oSqlDataReader, "quantidade_cama")
                    oInfo.codigo_responsavel_apartamento = SafeGetLong(oSqlDataReader, "codigo_responsavel_apartamento")
                    oInfo.responsavel_apartamento = SafeGetString(oSqlDataReader, "responsavel_apartamento")
                    oInfo.ativoTexto = SafeGetBooleanSimNao(oSqlDataReader, "ativo")

                    oReturn.Add(oInfo)

                End While

            End Using

            'Retorno da Função
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ValidaApartamento(ByVal iCodigoEmpresa As Integer,
                                      ByVal iCodigoUnidade As Integer,
                                      ByVal sApartamento As String,
                                      ByVal iCodigo As Integer) As Boolean

        Try

            'Váriaveis Locais
            Dim oSqlParameter(3) As SqlParameter
            Dim i As Integer = 0
            Dim iReturn As Integer

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo : i += 1

            'Seta Parametros - Apartamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "apartamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sApartamento

            'Executa Query
            iReturn = ExecuteScalar(sConnection, CommandType.StoredProcedure, "sp_validate_cadastro_basico_apartamento", oSqlParameter)

            'Retorno da Função
            Return (IIf(iReturn > 0, False, True))

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#Region "::: OPERA :::"

    Public Sub InterfaceHotelRoomsOpera(ByVal iCodigoEmpresa As Integer,
                                        ByVal iCodigoUnidade As Integer,
                                        ByVal oHotelRoomsOpera As HotelRoomsOpera)

        'Variaveis Locais
        Dim oSqlParameter(6) As SqlParameter
        Dim i As Integer = 0

        Try

            For Each oRoom As Room In oHotelRoomsOpera.hotelRoomsDetails.room

                i = 0

                'Seta Parametros - Código Empresa
                oSqlParameter(i) = New SqlParameter
                oSqlParameter(i).ParameterName = "codigo_empresa"
                oSqlParameter(i).Direction = ParameterDirection.Input
                oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
                oSqlParameter(i).Value = iCodigoEmpresa : i += 1

                'Seta Parametros - Unidade
                oSqlParameter(i) = New SqlParameter
                oSqlParameter(i).ParameterName = "codigo_unidade"
                oSqlParameter(i).Direction = ParameterDirection.Input
                oSqlParameter(i).SqlDbType = SqlDbType.Int
                oSqlParameter(i).Value = iCodigoUnidade : i += 1

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
                oSqlParameter(i).Value = oRoom.floorDescription : i += 1

                'Seta Parametros - Room Status
                oSqlParameter(i) = New SqlParameter
                oSqlParameter(i).ParameterName = "room_status"
                oSqlParameter(i).Direction = ParameterDirection.Input
                oSqlParameter(i).SqlDbType = SqlDbType.VarChar
                oSqlParameter(i).Size = 200
                oSqlParameter(i).Value = oRoom.housekeeping.roomStatus.roomStatus : i += 1

                'Seta Parametros - Front Office Status
                oSqlParameter(i) = New SqlParameter
                oSqlParameter(i).ParameterName = "front_office_status"
                oSqlParameter(i).Direction = ParameterDirection.Input
                oSqlParameter(i).SqlDbType = SqlDbType.VarChar
                oSqlParameter(i).Size = 200
                oSqlParameter(i).Value = oRoom.housekeeping.roomStatus.frontOfficeStatus

                'Executa Query
                ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_interface_cadastro_basico_apartamento_opera", oSqlParameter)

            Next

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region

#End Region

#Region "::: AR CONDICIONADO :::"

    Public Sub InsertArCondicionado(ByVal iCodigoEmpresa As Integer,
                                    ByVal iCodigoUsuario As Integer,
                                    ByVal iCodigoUnidade As Integer,
                                    ByVal iCodigoTipoArCondicionado As Integer,
                                    ByVal iCodigoDepartamento As Integer,
                                    ByVal iCodigoEmpresaPMOC As Integer,
                                    ByVal lCodigoArCondicionadoPMOC As Long,
                                    ByVal sTag As String,
                                    ByVal sDescricao As String,
                                    ByVal iCodigoSetor As Integer,
                                    ByVal iCodigoApartamento As Integer,
                                    ByVal sFabricante As String,
                                    ByVal sEnderecoFabricante As String,
                                    ByVal sContatoFabricante As String,
                                    ByVal sModelo As String,
                                    ByVal sNumeroFabricacao As String,
                                    ByVal sAnoFabricacao As String,
                                    ByVal sCaracteristicas As String,
                                    ByVal sDataUltimaManutencao As String,
                                    ByVal dPotencia As Double,
                                    ByVal iCodigoPotenciaArCondicionado As Integer,
                                    ByVal iAndar As Integer,
                                    ByVal sArquivo As String,
                                    ByVal bAtivo As Boolean)

        'Variaveis Locais
        Dim oSqlParameter(22) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Tipo Ar Condicionado
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_tipo_ar_condicionado"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoTipoArCondicionado : i += 1

            'Seta Parametros - Código Departamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_departamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoDepartamento : i += 1

            'Seta Parametros - Tag
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "tag"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sTag.ToUpper : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 200
            oSqlParameter(i).Value = sDescricao.ToUpper : i += 1

            'Seta Parametros - Código Setor
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_setor"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoSetor = -1, DBNull.Value, iCodigoSetor) : i += 1

            'Seta Parametros - Código Apartamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_apartamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoApartamento = -1, DBNull.Value, iCodigoApartamento) : i += 1

            'Seta Parametros - Fabricante
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "fabricante"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sFabricante : i += 1

            'Seta Parametros - Endereço Fabricante
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "endereco_fabricante"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 200
            oSqlParameter(i).Value = sEnderecoFabricante.ToUpper : i += 1

            'Seta Parametros - Contato Fabricante
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "contato_fabricante"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sContatoFabricante.ToUpper : i += 1

            'Seta Parametros - Modelo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "modelo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sModelo.ToUpper : i += 1

            'Seta Parametros - Nº Fabricação
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "numero_fabricacao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sNumeroFabricacao.ToUpper : i += 1

            'Seta Parametros - Ano Fabricação
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ano_fabricacao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(IsNumeric(sAnoFabricacao), sAnoFabricacao, DBNull.Value) : i += 1

            'Seta Parametros - Características
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "caracteristicas"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = sCaracteristicas : i += 1

            'Seta Parametros - Data Próxima Manutenção
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_proxima_manutencao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataUltimaManutencao : i += 1

            'Seta Parametros - Andar
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "andar"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = IIf(iAndar = -1, DBNull.Value, iAndar) : i += 1

            'Seta Parametros - Potência
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "potencia"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = dPotencia : i += 1

            'Seta Parametros - Código Potência Ar Condicionado
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_potencia_ar_condicionado"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = IIf(iCodigoPotenciaArCondicionado = -1, DBNull.Value, iCodigoPotenciaArCondicionado) : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_cadastro_basico_ar_condicionado", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateArCondicionado(ByVal iCodigoEmpresa As Integer,
                                    ByVal iCodigoUsuario As Integer,
                                    ByVal iCodigoUnidade As Integer,
                                    ByVal iCodigoTipoArCondicionado As Integer,
                                    ByVal iCodigoDepartamento As Integer,
                                    ByVal iCodigoEmpresaPMOC As Integer,
                                    ByVal lCodigoArCondicionadoPMOC As Long,
                                    ByVal sTag As String,
                                    ByVal sDescricao As String,
                                    ByVal iCodigoSetor As Integer,
                                    ByVal iCodigoApartamento As Integer,
                                    ByVal sFabricante As String,
                                    ByVal sEnderecoFabricante As String,
                                    ByVal sContatoFabricante As String,
                                    ByVal sModelo As String,
                                    ByVal sNumeroFabricacao As String,
                                    ByVal sAnoFabricacao As String,
                                    ByVal sCaracteristicas As String,
                                    ByVal sDataUltimaManutencao As String,
                                    ByVal dPotencia As Double,
                                    ByVal iCodigoPotenciaArCondicionado As Integer,
                                    ByVal iAndar As Integer,
                                    ByVal sArquivo As String,
                                    ByVal bAtivo As Boolean,
                                    ByVal lCodigo As Long)

        'Variaveis Locais
        Dim oSqlParameter(23) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código Tipo Ar Condicionado
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_tipo_ar_condicionado"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoTipoArCondicionado : i += 1

            'Seta Parametros - Código Departamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_departamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoDepartamento : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Empresa PMOC
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa_pmoc"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = IIf(iCodigoEmpresaPMOC = 0, DBNull.Value, iCodigoEmpresaPMOC) : i += 1

            'Seta Parametros - Código Ar Condicionado PMOC
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_ar_condicionado_pmoc"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = IIf(lCodigoArCondicionadoPMOC = 0, DBNull.Value, lCodigoArCondicionadoPMOC) : i += 1

            'Seta Parametros - Tag
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "tag"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sTag.ToUpper : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 200
            oSqlParameter(i).Value = sDescricao.ToUpper : i += 1

            'Seta Parametros - Código Setor
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_setor"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoSetor = -1, DBNull.Value, iCodigoSetor) : i += 1

            'Seta Parametros - Código Apartamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_apartamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoApartamento = -1, DBNull.Value, iCodigoApartamento) : i += 1

            'Seta Parametros - Fabricante
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "fabricante"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sFabricante : i += 1

            'Seta Parametros - Endereço Fabricante
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "endereco_fabricante"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 200
            oSqlParameter(i).Value = sEnderecoFabricante.ToUpper : i += 1

            'Seta Parametros - Contato Fabricante
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "contato_fabricante"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sContatoFabricante.ToUpper : i += 1

            'Seta Parametros - Modelo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "modelo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sModelo.ToUpper : i += 1

            'Seta Parametros - Nº Fabricação
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "numero_fabricacao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sNumeroFabricacao.ToUpper : i += 1

            'Seta Parametros - Ano Fabricação
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ano_fabricacao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(IsNumeric(sAnoFabricacao), sAnoFabricacao, DBNull.Value) : i += 1

            'Seta Parametros - Características
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "caracteristicas"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = sCaracteristicas : i += 1

            'Seta Parametros - Data Próxima Manutenção
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_proxima_manutencao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataUltimaManutencao : i += 1

            'Seta Parametros - Potência
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "potencia"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = dPotencia : i += 1

            'Seta Parametros - Código Potência Ar Condicionado
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_potencia_ar_condicionado"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = IIf(iCodigoPotenciaArCondicionado = -1, DBNull.Value, iCodigoPotenciaArCondicionado) : i += 1

            'Seta Parametros - Andar
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "andar"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = IIf(iAndar = -1, DBNull.Value, iAndar) : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = lCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_cadastro_basico_ar_condicionado", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub DeleteArCondicionado(ByVal iCodigoEmpresa As Integer,
                                    ByVal iCodigoUsuario As Integer,
                                    ByVal lCodigo As Long)

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

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_cadastro_basico_ar_condicionado", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InfoArCondicionado(ByVal iCodigoEmpresa As Integer,
                                  ByVal lCodigo As Long,
                                  ByRef oArCondicionado As ArCondicionado)

        'Variaveis Locais
        Dim oSqlParameter(1) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_ar_condicionado_dados", oSqlParameter)

            While oSqlDataReader.Read

                oArCondicionado = New ArCondicionado
                oArCondicionado.codigo = oSqlDataReader("codigo")
                oArCondicionado.tag = oSqlDataReader.Item("tag")
                oArCondicionado.codigo_unidade = oSqlDataReader("codigo_unidade")
                oArCondicionado.unidade = oSqlDataReader("unidade")
                oArCondicionado.codigo_tipo_ar_condicionado = oSqlDataReader("codigo_tipo_ar_condicionado")
                oArCondicionado.codigo_departamento = oSqlDataReader("codigo_departamento")
                oArCondicionado.tipo_ar_condicionado = oSqlDataReader("tipo_ar_condicionado")
                oArCondicionado.descricao = oSqlDataReader("descricao")
                oArCondicionado.codigo_setor = oSqlDataReader("codigo_setor")
                oArCondicionado.setor = oSqlDataReader("setor")
                oArCondicionado.codigo_apartamento = oSqlDataReader("codigo_apartamento")
                oArCondicionado.apartamento = oSqlDataReader("apartamento")
                oArCondicionado.fabricante = oSqlDataReader("fabricante")
                oArCondicionado.endereco_fabricante = oSqlDataReader("endereco_fabricante")
                oArCondicionado.contato_fabricante = oSqlDataReader("contato_fabricante")
                oArCondicionado.modelo = oSqlDataReader("modelo")
                oArCondicionado.numero_fabricacao = oSqlDataReader("numero_fabricacao")
                oArCondicionado.ano_fabricacao = oSqlDataReader("ano_fabricacao")
                oArCondicionado.caracteristicas = oSqlDataReader("caracteristicas")
                oArCondicionado.data_proxima_manutencao = oSqlDataReader("data_proxima_manutencao")
                oArCondicionado.codigo_potencia_ar_condicionado = oSqlDataReader.Item("codigo_potencia_ar_condicionado")
                oArCondicionado.potencia = oSqlDataReader.Item("potencia")
                oArCondicionado.potencia_ar_condicionado = oSqlDataReader.Item("potencia_ar_condicionado")
                oArCondicionado.ativo = oSqlDataReader.Item("ativo")
                oArCondicionado.codigo_empresa_pmoc = oSqlDataReader.Item("codigo_empresa_pmoc")
                oArCondicionado.codigo_unidade_pmoc = oSqlDataReader.Item("codigo_unidade_pmoc")
                oArCondicionado.codigo_ar_condicionado_pmoc = oSqlDataReader.Item("codigo_ar_condicionado_pmoc")
                oArCondicionado.andar = oSqlDataReader.Item("andar")

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function IndexArCondicionado(ByVal iCodigoEmpresa As Integer,
                                        ByVal iCodigoUsuario As Integer,
                                        ByVal iCodigoUnidade As Integer,
                                        ByVal sTAG As String,
                                        ByVal iCodigoTipoArCondicionado As Integer,
                                        ByVal iCodigoDepartamento As Integer,
                                        ByVal iCodigoSetor As Integer,
                                        ByVal iCodigoApartamento As Integer,
                                        ByVal iAtivo As Integer) As List(Of ArCondicionado)

        Try

            'Váriaveis Locais
            Dim oArCondicionado As New List(Of ArCondicionado)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(8) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - TAG
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "tag"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sTAG : i += 1

            'Seta Parametros - Código Tipo Ar Condicionado
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_tipo_ar_condicionado"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoTipoArCondicionado : i += 1

            'Seta Parametros - Código Departamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_departamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoDepartamento : i += 1

            'Seta Parametros - Código Setor
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_setor"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoSetor : i += 1

            'Seta Parametros - Código Apartamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_apartamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoApartamento : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = IIf(iAtivo = 1, True, IIf(iAtivo = 0, False, DBNull.Value))

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_ar_condicionado", oSqlParameter)

            While oSqlDataReader.Read

                Dim oArCondicionadoInfo As New ArCondicionado

                oArCondicionadoInfo.codigo = oSqlDataReader.Item("codigo")
                oArCondicionadoInfo.tag = oSqlDataReader.Item("tag")
                oArCondicionadoInfo.unidade = oSqlDataReader.Item("unidade")
                oArCondicionadoInfo.tipo_ar_condicionado = oSqlDataReader("tipo_ar_condicionado")
                oArCondicionadoInfo.setor = oSqlDataReader.Item("setor")
                oArCondicionadoInfo.apartamento = oSqlDataReader("apartamento")
                oArCondicionadoInfo.potencia = oSqlDataReader.Item("potencia")
                oArCondicionadoInfo.modelo = oSqlDataReader.Item("modelo")
                oArCondicionadoInfo.potencia_ar_condicionado = oSqlDataReader.Item("potencia_ar_condicionado")
                'oArCondicionadoInfo.data_proxima_manutencao = oSqlDataReader("data_proxima_manutencao")
                oArCondicionadoInfo.descricao = oSqlDataReader.Item("descricao")
                oArCondicionadoInfo.qrcode = oSqlDataReader.Item("qrcode")
                oArCondicionadoInfo.ativo = oSqlDataReader.Item("ativo")
                oArCondicionadoInfo.texto_ativo = IIf(oSqlDataReader.Item("ativo") = True, "SIM", "NÃO")

                oArCondicionado.Add(oArCondicionadoInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oArCondicionado

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ValidaArCondicionado(ByVal iCodigoEmpresa As Integer,
                                         ByVal iCodigoUnidade As Integer,
                                         ByVal sTag As String,
                                         ByVal lCodigo As Long) As Boolean

        Try

            'Váriaveis Locais
            Dim oSqlParameter(3) As SqlParameter
            Dim i As Integer = 0
            Dim iReturn As Integer

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigo : i += 1

            'Seta Parametros - Tag
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "tag"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sTag

            'Executa Query
            iReturn = ExecuteScalar(sConnection, CommandType.StoredProcedure, "sp_validate_cadastro_basico_ar_condicionado", oSqlParameter)

            'Retorno da Função
            Return (IIf(iReturn > 0, False, True))

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ValidaArCondicionadoPMOC(ByVal iCodigoEmpresa As Integer,
                                             ByVal iCodigoUnidade As Integer,
                                             ByVal lCodigoArCondicionadoPMOC As Long,
                                             ByVal lCodigo As Long) As Boolean

        Try

            'Váriaveis Locais
            Dim oSqlParameter(3) As SqlParameter
            Dim i As Integer = 0
            Dim iReturn As Integer

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigo : i += 1

            'Seta Parametros - Código Ar Condicionado PMOC
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_ar_condicionado_pmoc"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoArCondicionadoPMOC

            'Executa Query
            iReturn = ExecuteScalar(sConnection, CommandType.StoredProcedure, "sp_validate_cadastro_basico_ar_condicionado_pmoc", oSqlParameter)

            'Retorno da Função
            Return (IIf(iReturn > 0, False, True))

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function VerificaArCondicionadoPMOC(ByVal iCodigoEmpresa As Integer,
                                               ByVal iCodigoUnidade As Integer) As Integer

        Try

            'Váriaveis Locais
            Dim oSqlParameter(1) As SqlParameter
            Dim i As Integer = 0
            Dim iReturn As Integer

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            iReturn = ExecuteScalar(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_unidade_dados_pmoc", oSqlParameter)

            'Retorno da Função
            Return iReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: ATIVIDADE :::"

    Public Sub InsertAtividade(ByVal iCodigoEmpresa As Integer,
                               ByVal iCodigoUsuario As Integer,
                               ByVal iCodigoUnidade As Integer,
                               ByVal iCodigoCategoria As Integer,
                               ByVal lCodigoEquipamento As Long,
                               ByVal sTitulo As String,
                               ByVal sDescricao As String,
                               ByVal iCodigoTipoServico As Integer,
                               ByVal sDataPrevisaoTermino As String,
                               ByVal sBloco As String,
                               ByVal sAndar As String,
                               ByVal bAtivo As Boolean)

        'Variaveis Locais
        Dim oSqlParameter(11) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Categoria
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_categoria"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoCategoria : i += 1

            'Seta Parametros - Código Equipamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_equipamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = IIf(lCodigoEquipamento = -1, DBNull.Value, lCodigoEquipamento) : i += 1

            'Seta Parametros - Título
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "titulo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 200
            oSqlParameter(i).Value = sTitulo.ToUpper : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = sDescricao.ToUpper : i += 1

            'Seta Parametros - Código Tipo Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_tipo_servico"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoTipoServico : i += 1

            'Seta Parametros - Data Previsão Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_previsao_termino"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataPrevisaoTermino : i += 1

            'Seta Parametros - Bloco
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "bloco"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = sBloco : i += 1

            'Seta Parametros - Andar
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "andar"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = sAndar : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_cadastro_basico_atividade", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateAtividade(ByVal iCodigoEmpresa As Integer,
                               ByVal iCodigoUsuario As Integer,
                               ByVal iCodigoUnidade As Integer,
                               ByVal iCodigoUnidadeOld As Integer,
                               ByVal iCodigoCategoria As Integer,
                               ByVal lCodigoEquipamento As Long,
                               ByVal sTitulo As String,
                               ByVal sDescricao As String,
                               ByVal iCodigoTipoServico As Integer,
                               ByVal sDataPrevisaoTermino As String,
                               ByVal sBloco As String,
                               ByVal sAndar As String,
                               ByVal bAtivo As Boolean,
                               ByVal lCodigo As Long)

        'Variaveis Locais
        Dim oSqlParameter(14) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Categoria
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_categoria"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoCategoria : i += 1

            'Seta Parametros - Código Equipamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_equipamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = IIf(lCodigoEquipamento = -1, DBNull.Value, lCodigoEquipamento) : i += 1

            'Seta Parametros - Título
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "titulo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 200
            oSqlParameter(i).Value = sTitulo.ToUpper : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = sDescricao.ToUpper : i += 1

            'Seta Parametros - Código Tipo Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_tipo_servico"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoTipoServico : i += 1

            'Seta Parametros - Data Previsão Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_previsao_termino"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataPrevisaoTermino : i += 1

            'Seta Parametros - Bloco
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "bloco"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = sBloco : i += 1

            'Seta Parametros - Andar
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "andar"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = sAndar : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigo : i += 1

            'Seta Parametros - Código Unidade Old
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade_old"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidadeOld

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_cadastro_basico_atividade", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub DeleteAtividade(ByVal iCodigoEmpresa As Integer,
                               ByVal iCodigoUsuario As Integer,
                               ByVal lCodigo As Long,
                               ByVal iCodigoUnidade As Integer)

        'Variaveis Locais
        Dim oSqlParameter(3) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigo : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_cadastro_basico_atividade", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InfoAtividade(ByVal iCodigoEmpresa As Integer,
                             ByVal lCodigo As Long,
                             ByVal iCodigoUnidade As Integer,
                             ByRef oAtividade As Atividade)

        'Variaveis Locais
        Dim oSqlParameter(2) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigo : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_atividade_dados", oSqlParameter)

            While oSqlDataReader.Read

                oAtividade = New Atividade
                oAtividade.codigo = oSqlDataReader.Item("codigo")
                oAtividade.unidade = oSqlDataReader.Item("unidade")
                oAtividade.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oAtividade.categoria = oSqlDataReader.Item("categoria")
                oAtividade.codigo_categoria = oSqlDataReader.Item("codigo_categoria")
                oAtividade.equipamento = oSqlDataReader.Item("equipamento")
                oAtividade.codigo_equipamento = oSqlDataReader.Item("codigo_equipamento")
                oAtividade.titulo = oSqlDataReader.Item("titulo")
                oAtividade.descricao = oSqlDataReader.Item("descricao")
                oAtividade.codigo_tipo_servico = oSqlDataReader.Item("codigo_tipo_servico")
                oAtividade.tipo_servico = oSqlDataReader.Item("tipo_servico")
                oAtividade.data_inicio = oSqlDataReader.Item("data_inicio")
                oAtividade.data_previsao_termino = oSqlDataReader.Item("data_previsao_termino")
                oAtividade.data_termino = oSqlDataReader.Item("data_termino")
                oAtividade.bloco = oSqlDataReader.Item("bloco")
                oAtividade.andar = oSqlDataReader.Item("andar")
                oAtividade.ativo = oSqlDataReader.Item("ativo")

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function IndexAtividade(ByVal iCodigoEmpresa As Integer,
                                   ByVal iCodigoUsuario As Integer,
                                   ByVal iCodigoUnidade As Integer) As List(Of Atividade)

        Try

            'Váriaveis Locais
            Dim oAtividade As New List(Of Atividade)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(2) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_atividade", oSqlParameter)

            While oSqlDataReader.Read

                Dim oAtividadeInfo As New Atividade

                oAtividadeInfo.codigo = oSqlDataReader.Item("codigo")
                oAtividadeInfo.unidade = oSqlDataReader.Item("unidade")
                oAtividadeInfo.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oAtividadeInfo.categoria = oSqlDataReader.Item("categoria")
                oAtividadeInfo.codigo_categoria = oSqlDataReader.Item("codigo_categoria")
                oAtividadeInfo.equipamento = oSqlDataReader.Item("equipamento")
                oAtividadeInfo.codigo_equipamento = oSqlDataReader.Item("codigo_equipamento")
                oAtividadeInfo.titulo = oSqlDataReader.Item("titulo")
                oAtividadeInfo.descricao = oSqlDataReader.Item("descricao")
                oAtividadeInfo.codigo_tipo_servico = oSqlDataReader.Item("codigo_tipo_servico")
                oAtividadeInfo.tipo_servico = oSqlDataReader.Item("tipo_servico")
                oAtividadeInfo.data_inicio = oSqlDataReader.Item("data_inicio")
                oAtividadeInfo.data_previsao_termino = oSqlDataReader.Item("data_previsao_termino")
                oAtividadeInfo.data_termino = oSqlDataReader.Item("data_termino")
                oAtividadeInfo.bloco = oSqlDataReader.Item("bloco")
                oAtividadeInfo.andar = oSqlDataReader.Item("andar")
                oAtividadeInfo.ativo = oSqlDataReader.Item("ativo")

                oAtividade.Add(oAtividadeInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oAtividade

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: AUDITORIA CORPORATIVO :::"

    Public Sub InsertAuditoriaCorporativo(ByVal iCodigoEmpresa As Integer,
                                          ByVal iCodigoUnidade As Integer,
                                          ByVal sDescricao As String,
                                          ByVal iCodigoModulo As Integer,
                                          ByVal iCodigoChecklist As Integer,
                                          ByVal bAtivo As Boolean,
                                          ByVal bGerarPlanoAcao As Boolean,
                                          ByVal iCodigoUsuario As Integer)

        'Variaveis Locais
        Dim oSqlParameter(7) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sDescricao.ToUpper() : i += 1

            'Seta Parametros - Código Módulo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_modulo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoModulo : i += 1

            'Seta Parametros - Código Checklist
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_checklist"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoChecklist : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo : i += 1

            'Seta Parametros - Gerar Plano Ação
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "gerar_plano_acao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bGerarPlanoAcao : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_cadastro_basico_auditoria_corporativo", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateAuditoriaCorporativo(ByVal iCodigoEmpresa As Integer,
                                          ByVal iCodigoUnidade As Integer,
                                          ByVal sDescricao As String,
                                          ByVal iCodigoChecklist As Integer,
                                          ByVal bAtivo As Boolean,
                                          ByVal bGerarPlanoAcao As Boolean,
                                          ByVal iCodigoUsuario As Integer,
                                          ByVal iCodigo As Integer)

        'Variaveis Locais
        Dim oSqlParameter(7) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sDescricao.ToUpper() : i += 1

            'Seta Parametros - Código Checklist
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_checklist"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoChecklist : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo : i += 1

            'Seta Parametros - Gerar Plano Ação
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "gerar_plano_acao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bGerarPlanoAcao : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_cadastro_basico_auditoria_corporativo", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub DeleteAuditoriaCorporativo(ByVal iCodigoEmpresa As Integer,
                                          ByVal iCodigoUsuario As Integer,
                                          ByVal iCodigoUnidade As Integer,
                                          ByVal iCodigo As Integer)

        'Variaveis Locais
        Dim oSqlParameter(3) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_cadastro_basico_auditoria_corporativo", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InfoAuditoriaCorporativo(ByVal iCodigoEmpresa As Integer,
                                        ByVal iCodigoUnidade As Integer,
                                        ByVal iCodigo As Integer,
                                        ByRef oAuditoriaCorporativo As AuditoriaCorporativo)

        'Variaveis Locais
        Dim oSqlParameter(2) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_auditoria_corporativo_dados", oSqlParameter)

            While oSqlDataReader.Read

                oAuditoriaCorporativo = New AuditoriaCorporativo
                oAuditoriaCorporativo.codigo = oSqlDataReader.Item("codigo")
                oAuditoriaCorporativo.descricao = oSqlDataReader.Item("descricao")
                oAuditoriaCorporativo.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oAuditoriaCorporativo.unidade = oSqlDataReader.Item("unidade")
                oAuditoriaCorporativo.codigo_checklist = oSqlDataReader.Item("codigo_checklist")
                oAuditoriaCorporativo.checklist = oSqlDataReader.Item("checklist")
                oAuditoriaCorporativo.ativo = oSqlDataReader.Item("ativo")
                oAuditoriaCorporativo.gerar_plano_acao = oSqlDataReader.Item("gerar_plano_acao")

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function IndexAuditoriaComporativo(ByVal iCodigoEmpresa As Integer,
                                              ByVal iCodigoUnidade As Integer,
                                              ByVal iCodigoModulo As Integer) As List(Of AuditoriaCorporativo)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of AuditoriaCorporativo)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(2) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Módulo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_modulo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoModulo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_auditoria_corporativo", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New AuditoriaCorporativo

                oInfo.codigo = oSqlDataReader.Item("codigo")
                oInfo.descricao = oSqlDataReader.Item("descricao")
                oInfo.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oInfo.unidade = oSqlDataReader.Item("unidade")
                oInfo.codigo_checklist = oSqlDataReader.Item("codigo_checklist")
                oInfo.checklist = oSqlDataReader.Item("checklist")
                oInfo.ativo = oSqlDataReader.Item("ativo")
                oInfo.gerar_plano_acao = oSqlDataReader.Item("gerar_plano_acao")

                oReturn.Add(oInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: AUDITORIA INTERNA :::"

    Public Sub InsertAuditoriaQualidade(ByVal iCodigoEmpresa As Integer,
                                      ByVal iCodigoUnidade As Integer,
                                      ByVal sDescricao As String,
                                      ByVal iCodigoModulo As Integer,
                                      ByVal iCodigoChecklist As Integer,
                                      ByVal iCodigoPeriodicidade As Integer,
                                      ByVal iIntervalo As Integer,
                                      ByVal bAtivo As Boolean,
                                      ByVal iCodigoUsuario As Integer)

        'Variaveis Locais
        Dim oSqlParameter(8) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sDescricao.ToUpper() : i += 1

            'Seta Parametros - Código Módulo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_modulo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoModulo : i += 1

            'Seta Parametros - Código Checklist
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_checklist"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoChecklist : i += 1

            'Seta Parametros - Código Periodicidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_periodicidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoPeriodicidade = -1, DBNull.Value, iCodigoPeriodicidade) : i += 1

            'Seta Parametros - Intervalo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "intervalo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iIntervalo : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_cadastro_basico_auditoria_qualidade", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateAuditoriaQualidade(ByVal iCodigoEmpresa As Integer,
                                      ByVal iCodigoUnidade As Integer,
                                      ByVal sDescricao As String,
                                      ByVal iCodigoModulo As Integer,
                                      ByVal iCodigoChecklist As Integer,
                                      ByVal iCodigoPeriodicidade As Integer,
                                      ByVal iIntervalo As Integer,
                                      ByVal bAtivo As Boolean,
                                      ByVal iCodigoUsuario As Integer,
                                      ByVal iCodigo As Integer)

        'Variaveis Locais
        Dim oSqlParameter(9) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sDescricao.ToUpper() : i += 1

            'Seta Parametros - Código Módulo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_modulo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoModulo : i += 1

            'Seta Parametros - Código Checklist
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_checklist"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoChecklist : i += 1

            'Seta Parametros - Código Periodicidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_periodicidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoPeriodicidade : i += 1

            'Seta Parametros - Intervalo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "intervalo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iIntervalo : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_cadastro_basico_auditoria_qualidade", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub DeleteAuditoriaQualidade(ByVal iCodigoEmpresa As Integer,
                                      ByVal iCodigoUsuario As Integer,
                                      ByVal iCodigoUnidade As Integer,
                                      ByVal iCodigo As Integer)

        'Variaveis Locais
        Dim oSqlParameter(3) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_cadastro_basico_auditoria_qualidade", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InfoAuditoriaQualidade(ByVal iCodigoEmpresa As Integer,
                                    ByVal iCodigoUnidade As Integer,
                                    ByVal iCodigo As Integer,
                                    ByRef oAuditoriaQualidade As AuditoriaQualidade)

        'Variaveis Locais
        Dim oSqlParameter(2) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_auditoria_qualidade_dados", oSqlParameter)

            While oSqlDataReader.Read

                oAuditoriaQualidade = New AuditoriaQualidade
                oAuditoriaQualidade.codigo = oSqlDataReader.Item("codigo")
                oAuditoriaQualidade.descricao = oSqlDataReader.Item("descricao")
                oAuditoriaQualidade.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oAuditoriaQualidade.unidade = oSqlDataReader.Item("unidade")
                oAuditoriaQualidade.codigo_checklist = oSqlDataReader.Item("codigo_checklist")
                oAuditoriaQualidade.checklist = oSqlDataReader.Item("checklist")
                oAuditoriaQualidade.codigo_modulo = oSqlDataReader.Item("codigo_modulo")
                oAuditoriaQualidade.modulo = oSqlDataReader.Item("modulo")
                oAuditoriaQualidade.codigo_periodicidade = oSqlDataReader.Item("codigo_periodicidade")
                oAuditoriaQualidade.periodicidade = oSqlDataReader.Item("periodicidade")
                oAuditoriaQualidade.intervalo = oSqlDataReader.Item("intervalo")
                oAuditoriaQualidade.ativo = oSqlDataReader.Item("ativo")

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function IndexAuditoriaQualidade(ByVal iCodigoEmpresa As Integer,
                                          ByVal iCodigoUnidade As Integer,
                                          ByVal iCodigoModulo As Integer) As List(Of AuditoriaQualidade)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of AuditoriaQualidade)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(2) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Módulo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_modulo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoModulo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_auditoria_qualidade", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New AuditoriaQualidade

                oInfo.codigo = oSqlDataReader.Item("codigo")
                oInfo.descricao = oSqlDataReader.Item("descricao")
                oInfo.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oInfo.unidade = oSqlDataReader.Item("unidade")
                oInfo.codigo_checklist = oSqlDataReader.Item("codigo_checklist")
                oInfo.checklist = oSqlDataReader.Item("checklist")
                oInfo.codigo_modulo = oSqlDataReader.Item("codigo_modulo")
                oInfo.modulo = oSqlDataReader.Item("modulo")
                oInfo.codigo_periodicidade = oSqlDataReader.Item("codigo_periodicidade")
                oInfo.periodicidade = oSqlDataReader.Item("periodicidade")
                oInfo.intervalo = oSqlDataReader.Item("intervalo")
                oInfo.ativo = oSqlDataReader.Item("ativo")

                oReturn.Add(oInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: CATEGORIA :::"

    Public Sub InsertCategoria(ByVal iCodigoEmpresa As Integer,
                               ByVal iCodigoUsuario As Integer,
                               ByVal iCodigoUnidade As Integer,
                               ByVal sDescricao As String,
                               ByVal bAtivo As Boolean)

        'Variaveis Locais
        Dim oSqlParameter(4) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sDescricao : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_cadastro_basico_categoria", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateCategoria(ByVal iCodigoEmpresa As Integer,
                               ByVal iCodigoUsuario As Integer,
                               ByVal iCodigoUnidade As Integer,
                               ByVal sDescricao As String,
                               ByVal bAtivo As Boolean,
                               ByVal iCodigo As Integer)

        'Variaveis Locais
        Dim oSqlParameter(5) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sDescricao : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_cadastro_basico_categoria", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub DeleteCategoria(ByVal iCodigoEmpresa As Integer,
                               ByVal iCodigoUsuario As Integer,
                               ByVal iCodigo As Integer)

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

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_cadastro_basico_categoria", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InfoCategoria(ByVal iCodigoEmpresa As Integer,
                             ByVal iCodigo As Integer,
                             ByRef oCategoria As Categoria)

        'Variaveis Locais
        Dim oSqlParameter(1) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_categoria_dados", oSqlParameter)

            While oSqlDataReader.Read

                oCategoria = New Categoria
                oCategoria.codigo = oSqlDataReader.Item("codigo")
                oCategoria.descricao = oSqlDataReader.Item("descricao")
                oCategoria.ativo = oSqlDataReader.Item("ativo")
                oCategoria.unidade = oSqlDataReader.Item("unidade")
                oCategoria.codigo_unidade = oSqlDataReader.Item("codigo_unidade")

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function IndexCategoria(ByVal iCodigoEmpresa As Integer,
                                   ByVal iCodigoUsuario As Integer,
                                   ByVal iCodigoUnidade As Integer) As List(Of Categoria)

        Try

            'Váriaveis Locais
            Dim oCategoria As New List(Of Categoria)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(2) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_categoria", oSqlParameter)

            While oSqlDataReader.Read

                Dim oCategoriaInfo As New Categoria

                oCategoriaInfo.descricao = oSqlDataReader.Item("descricao")
                oCategoriaInfo.ativo = oSqlDataReader.Item("ativo")
                oCategoriaInfo.codigo = oSqlDataReader.Item("codigo")
                oCategoriaInfo.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oCategoriaInfo.unidade = oSqlDataReader.Item("unidade")

                oCategoria.Add(oCategoriaInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCategoria

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ValidaCategoria(ByVal iCodigoEmpresa As Integer,
                                    ByVal iCodigoUnidade As Integer,
                                    ByVal sDescricao As String,
                                    ByVal iCodigo As Integer) As Boolean

        Try

            'Váriaveis Locais
            Dim oSqlParameter(3) As SqlParameter
            Dim i As Integer = 0
            Dim iReturn As Integer

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sDescricao

            'Executa Query
            iReturn = ExecuteScalar(sConnection, CommandType.StoredProcedure, "sp_validate_cadastro_basico_categoria", oSqlParameter)

            'Retorno da Função
            Return (IIf(iReturn > 0, False, True))

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: CHECKLIST :::"

    Public Sub InsertChecklist(ByVal iCodigoEmpresa As Integer,
                               ByVal iCodigoUsuario As Integer,
                               ByVal iCodigoUnidade As Integer,
                               ByVal iCodigoModulo As Integer,
                               ByVal iCodigoTipoChecklist As Integer,
                               ByVal sDescricao As String,
                               ByRef lCodigo As Long)

        'Variaveis Locais
        Dim oSqlParameter(6) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = IIf(iCodigoUnidade = -1, DBNull.Value, iCodigoUnidade) : i += 1

            'Seta Parametros - Código Módulo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_modulo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoModulo : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código Tipo Checklist
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_tipo_checklist"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoTipoChecklist : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sDescricao : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Output
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_cadastro_basico_checklist", oSqlParameter)

            lCodigo = oSqlParameter(i).Value

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InsertChecklistItem(ByVal lCodigoChecklist As Long,
                                   ByVal iCodigoEmpresa As Integer,
                                   ByVal sChecklist As String,
                                   ByVal iCodigoTipoItemChecklist As Integer,
                                   ByVal sGrupo As String,
                                   ByVal iPesoGrupo As Integer,
                                   ByVal sSubgrupo As String,
                                   ByVal iPesoSubgrupo As Integer,
                                   ByVal sDescricao As String,
                                   ByVal bAllowPicture As Boolean,
                                   ByVal dValorMinimo As Double,
                                   ByVal dValorMaximo As Double,
                                   ByVal sUnidadeMedida As String,
                                   ByVal iTempoEstimado As Integer,
                                   ByVal bAuditado As Boolean,
                                   ByVal bOrdemServico As Boolean,
                                   ByVal iCodigoPeriodicidade As Integer,
                                   ByVal iIntervalo As Integer,
                                   ByVal iPeso As Integer,
                                   ByVal iCodigoDepartamento As Integer)

        'Variaveis Locais
        Dim oSqlParameter(21) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Checklist
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_checklist"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoChecklist : i += 1

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Checklist
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "checklist"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sChecklist : i += 1

            'Seta Parametros - Código Tipo Item Checklist
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_tipo_item_checklist"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoTipoItemChecklist : i += 1

            'Seta Parametros - Grupo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "grupo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sGrupo : i += 1

            'Seta Parametros - Peso Grupo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "peso_grupo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iPesoGrupo : i += 1

            'Seta Parametros - Subgrupo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "subgrupo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sSubgrupo : i += 1

            'Seta Parametros - Peso Sub Grupo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "peso_subgrupo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iPesoSubgrupo : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = sDescricao : i += 1

            'Seta Parametros - Allow Picture
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "allow_picture"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAllowPicture : i += 1

            'Seta Parametros - Valor Mínimo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "valor_minimo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = dValorMinimo : i += 1

            'Seta Parametros - Valor Máximo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "valor_maximo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = dValorMaximo : i += 1

            'Seta Parametros - Unidade de Medida
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "unidade_medida"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 10
            oSqlParameter(i).Value = sUnidadeMedida : i += 1

            'Seta Parametros - Tempo Estimado
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "tempo_estimado"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iTempoEstimado : i += 1

            'Seta Parametros - Auditado
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "auditado"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAuditado : i += 1

            'Seta Parametros - Ordem Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ordem_servico"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bOrdemServico : i += 1

            'Seta Parametros - Código Periodicidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_periodicidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoPeriodicidade = -1, DBNull.Value, iCodigoPeriodicidade) : i += 1

            'Seta Parametros - Intervalo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "intervalo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iIntervalo : i += 1

            'Seta Parametros - Peso
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "peso"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iPeso = -1, DBNull.Value, iPeso) : i += 1

            'Seta Parametros - Código Departamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_departamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoDepartamento = -1, DBNull.Value, iCodigoDepartamento)

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_cadastro_basico_checklist_item", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateChecklist(ByVal iCodigoEmpresa As Integer,
                               ByVal iCodigoUsuario As Integer,
                               ByVal iCodigoTipoChecklist As Integer,
                               ByVal sDescricao As String,
                               ByVal lCodigo As Long)

        'Variaveis Locais
        Dim oSqlParameter(5) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código Tipo Checklist
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_tipo_checklist"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoTipoChecklist : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sDescricao : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_cadastro_basico_checklist", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub DeleteChecklist(ByVal iCodigoEmpresa As Integer,
                               ByVal iCodigoUsuario As Integer,
                               ByVal iCodigo As Integer)

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

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_cadastro_basico_checklist", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InfoChecklist(ByVal iCodigoEmpresa As Integer,
                             ByVal lCodigo As Long,
                             ByRef oChecklistHeader As ChecklistHeader)

        'Variaveis Locais
        Dim oSqlParameter(1) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_checklist_dados", oSqlParameter)

            While oSqlDataReader.Read

                oChecklistHeader = New ChecklistHeader

                oChecklistHeader.codigoUnidade = oSqlDataReader.Item("codigo_unidade")
                oChecklistHeader.unidade = oSqlDataReader.Item("unidade")
                oChecklistHeader.codigo = oSqlDataReader.Item("codigo")
                oChecklistHeader.codigoTipoChecklist = oSqlDataReader.Item("codigo_tipo_checklist")
                oChecklistHeader.tipoChecklist = oSqlDataReader.Item("tipo_checklist")
                oChecklistHeader.descricao = oSqlDataReader.Item("descricao")

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function LoadChecklist(ByVal codigoEmpresa As Integer,
                                  ByVal codigoUnidade As Integer,
                                  ByVal codigoModulo As Integer,
                                  ByVal codigoTipoChecklist As Integer,
                                  ByVal descricao As String,
                                  ByVal codigoUsuario As Integer) As List(Of ChecklistHeader)

        Try


            Dim oReturn As New List(Of ChecklistHeader)
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("codigo_modulo", SqlDbType.Int, codigoModulo),
                CriarParametro("codigo_tipo_checklist", SqlDbType.SmallInt, codigoTipoChecklist),
                CriarParametro("descricao", SqlDbType.VarChar, descricao),
                CriarParametro("codigo_usuario", SqlDbType.Int, codigoUsuario)
            }

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_checklist", oSqlParameter)


                While oSqlDataReader.Read

                    Dim oInfo As New ChecklistHeader

                    oInfo.codigoUnidade = SafeGetLong(oSqlDataReader, "codigo_unidade")
                    oInfo.unidade = SafeGetString(oSqlDataReader, "unidade")
                    oInfo.codigo = SafeGetLong(oSqlDataReader, "codigo")
                    oInfo.descricao = SafeGetString(oSqlDataReader, "descricao")
                    oInfo.codigoTipoChecklist = SafeGetLong(oSqlDataReader, "codigo_tipo_checklist")
                    oInfo.tipoChecklist = SafeGetString(oSqlDataReader, "tipo_checklist")

                    oReturn.Add(oInfo)

                End While


            End Using

            'Retorno da Função
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function LoadChecklistItem(ByVal codigoEmpresa As Integer,
                                      ByVal codigoUnidade As Integer,
                                      ByVal codigoChecklist As Long,
                                      ByVal codigoTipoChecklist As Integer,
                                      ByVal codigoUsuario As Integer) As ChecklistResponse

        Try

            Dim oReturn As New ChecklistResponse

            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("codigo_checklist", SqlDbType.Int, codigoChecklist),
                CriarParametro("codigo_tipo_checklist", SqlDbType.SmallInt, codigoTipoChecklist),
                CriarParametro("codigo_usuario", SqlDbType.Int, codigoUsuario)
            }

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_checklist_item_v2", oSqlParameter)

                'Estrutura
                While oSqlDataReader.Read()

                    Dim col As New ColumnStructure()

                    col.data = oSqlDataReader("dataMember").ToString()
                    col.title = oSqlDataReader("colunaExcel").ToString()
                    col.visible = Convert.ToBoolean(oSqlDataReader("visivel"))
                    col.width = If(IsDBNull(oSqlDataReader("largura")), Nothing, oSqlDataReader("largura").ToString())
                    col.align = If(IsDBNull(oSqlDataReader("alinhamento")), Nothing, oSqlDataReader("alinhamento").ToString())
                    col.frozen = Convert.ToBoolean(oSqlDataReader("frozen"))
                    col.orderable = Convert.ToBoolean(oSqlDataReader("ordenavel"))
                    col.wrap = Convert.ToBoolean(oSqlDataReader("quebraLinha"))

                    oReturn.columns.Add(col)

                End While

                ' Ir para o próximo result set
                oSqlDataReader.NextResult()

                'Dados
                While oSqlDataReader.Read()

                    Dim row As New Dictionary(Of String, Object)

                    For i As Integer = 0 To oSqlDataReader.FieldCount - 1
                        row(oSqlDataReader.GetName(i)) =
                        If(oSqlDataReader.IsDBNull(i), Nothing, oSqlDataReader.GetValue(i))
                    Next

                    oReturn.data.Add(row)

                End While

                ' Ir para o próximo result set
                oSqlDataReader.NextResult()

                'Dados
                While oSqlDataReader.Read()

                    Dim group As New GroupDefinition()

                    group.column = oSqlDataReader("column").ToString()
                    group.level = oSqlDataReader("level").ToString()
                    group.collapsible = Convert.ToBoolean(oSqlDataReader("collapsible"))
                    group.showCount = Convert.ToBoolean(oSqlDataReader("showCount"))
                    group.cssClass = oSqlDataReader("cssClass").ToString()

                    oReturn.groupBy.Add(group)

                End While


            End Using

            'Retorno da Função
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function IndexChecklistItem(ByVal iCodigoEmpresa As Integer,
                                       ByVal lCodigoChecklist As Long) As List(Of ChecklistItem)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of ChecklistItem)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(1) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código Checklist
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_checklist"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoChecklist : i += 1

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_checklist_item", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New ChecklistItem

                oInfo.codigo_checklist = oSqlDataReader.Item("codigo_checklist")
                oInfo.codigo = oSqlDataReader.Item("codigo")
                oInfo.checklist = oSqlDataReader.Item("checklist")
                oInfo.tipo_item_checklist = oSqlDataReader.Item("tipo_item_checklist")
                oInfo.codigo_tipo_item_checklist = oSqlDataReader.Item("codigo_tipo_item_checklist")
                oInfo.grupo = oSqlDataReader.Item("grupo")
                oInfo.peso_grupo = oSqlDataReader.Item("peso_grupo")
                oInfo.subgrupo = oSqlDataReader.Item("subgrupo")
                oInfo.peso_subgrupo = oSqlDataReader.Item("peso_subgrupo")
                oInfo.descricao = oSqlDataReader.Item("descricao")
                oInfo.numero_digitos = oSqlDataReader.Item("numero_digitos")
                oInfo.allow_picture = oSqlDataReader.Item("allow_picture")
                oInfo.valor_minimo = oSqlDataReader.Item("valor_minimo")
                oInfo.valor_maximo = oSqlDataReader.Item("valor_maximo")
                oInfo.unidade_medida = oSqlDataReader.Item("unidade_medida")
                oInfo.tempo_estimado = oSqlDataReader.Item("tempo_estimado")
                oInfo.auditado = oSqlDataReader.Item("auditado")
                oInfo.periodicidade = oSqlDataReader.Item("periodicidade")
                oInfo.codigo_periodicidade = oSqlDataReader.Item("codigo_periodicidade")
                oInfo.intervalo = oSqlDataReader.Item("intervalo")
                oInfo.codigo_departamento = oSqlDataReader.Item("codigo_departamento")
                oInfo.ordem_servico = oSqlDataReader.Item("ordem_servico")

                oReturn.Add(oInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function



    Public Function ValidaChecklist(ByVal iCodigoEmpresa As Integer,
                                    ByVal sDescricao As String,
                                    ByVal iCodigo As Integer) As Boolean

        Try

            'Váriaveis Locais
            Dim oSqlParameter(2) As SqlParameter
            Dim i As Integer = 0
            Dim iReturn As Integer

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sDescricao

            'Executa Query
            iReturn = ExecuteScalar(sConnection, CommandType.StoredProcedure, "sp_validate_cadastro_basico_checklist", oSqlParameter)

            'Retorno da Função
            Return (IIf(iReturn > 0, False, True))

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function LoadInterfaceExcel(ByVal codigoEmpresa As Integer,
                                       ByVal tipoChecklist As Integer) As List(Of InterfaceExcelColumn)

        Dim oReturn As New List(Of InterfaceExcelColumn)

        Dim oSqlParameter As SqlParameter() = {
            CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
            CriarParametro("codigo_tipo_checklist", SqlDbType.SmallInt, tipoChecklist)
        }

        Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_interface_excel_checklist", oSqlParameter)

            While oSqlDataReader.Read()

                Dim col As New InterfaceExcelColumn()

                col.ColunaExcel = SafeGetString(oSqlDataReader, "colunaExcel").ToString()
                col.DataMember = SafeGetString(oSqlDataReader, "dataMember")
                col.Obrigatorio = SafeGetBoolean(oSqlDataReader, "obrigatorio")
                col.Visivel = SafeGetBoolean(oSqlDataReader, "visivel")
                col.Linha = SafeGetLong(oSqlDataReader, "linha")
                col.Coluna = SafeGetLong(oSqlDataReader, "coluna")
                col.Largura = SafeGetString(oSqlDataReader, "largura")
                col.TipoValidacao = SafeGetString(oSqlDataReader, "tipoValidacao")
                col.FonteLista = SafeGetString(oSqlDataReader, "fonteLista")

                oReturn.Add(col)

            End While

        End Using

        Return oReturn

    End Function

    Public Sub BulkInsertChecklistExcel(ByVal uniqueId As String,
                                        ByVal filePath As String,
                                        ByVal codigoEmpresa As Integer,
                                        ByVal codigoUnidade As Integer,
                                        ByVal interfaceName As String,
                                        ByVal codigoUsuario As Integer,
                                        ByVal tabelaInsert As String,
                                        Optional ByVal storedProcedureUpdate As String = "")

        Dim query As String = ""
        Dim rowDelete As String = ""
        Dim columnDelete As String = ""
        Dim worksheetImport As New List(Of String)
        Dim structureExcel As New List(Of InterfaceExcelColumn)

        Try


            Dim oQueryParameters As SqlParameter() = {
                CriarParametro("uniqueId", SqlDbType.VarChar, uniqueId),
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_usuario", SqlDbType.Int, codigoUsuario),
                CriarParametro("worksheet", SqlDbType.VarChar, "[$WORKSHEET]"),
                CriarParametro("interface", SqlDbType.VarChar, interfaceName)
            }

            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_interface_query_excel", oQueryParameters)

                ' Resultset 1: query e deletes
                While oSqlDataReader.Read()
                    query = SafeGetString(oSqlDataReader, "query")
                    query = query.Replace(vbCrLf, "").Replace(vbTab, "").Replace(vbLf, "")
                    rowDelete = SafeGetString(oSqlDataReader, "rowDelete")
                    columnDelete = SafeGetString(oSqlDataReader, "columnDelete")
                End While

                ' Resultset 2: estrutura
                oSqlDataReader.NextResult()

                While oSqlDataReader.Read()
                    Dim info As New InterfaceExcelColumn
                    info.Linha = SafeGetLong(oSqlDataReader, "linha")
                    info.Coluna = SafeGetLong(oSqlDataReader, "coluna")
                    info.ColunaExcel = SafeGetString(oSqlDataReader, "colunaExcel")
                    info.DataMember = SafeGetString(oSqlDataReader, "dataMember")
                    info.Visivel = SafeGetBoolean(oSqlDataReader, "visivel")
                    structureExcel.Add(info)
                End While

            End Using

            If String.IsNullOrEmpty(query) Then
                Throw New Exception("Query de importação não encontrada.")
            End If

            If structureExcel.Count = 0 Then
                Throw New Exception("Estrutura de importação não encontrada.")
            End If

            Dim oDelParams As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_usuario", SqlDbType.Int, codigoUsuario),
                CriarParametro("tabela", SqlDbType.VarChar, tabelaInsert)
            }

            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_interface_table_tmp", oDelParams)

            ExcelPackage.License.SetNonCommercialOrganization("<ACTI>")

            Using oExcelPackage As New ExcelPackage(New FileInfo(filePath))

                For Each oWorksheet As ExcelWorksheet In oExcelPackage.Workbook.Worksheets

                    Dim worksheetFound As Boolean = True

                    ' Aplica deletes se existirem
                    If (rowDelete <> "") Then
                        For Each r In rowDelete.Split(","c)
                            Dim rr As Integer
                            If Integer.TryParse(r, rr) Then oWorksheet.DeleteRow(rr)
                        Next
                    End If

                    If (columnDelete <> "") Then
                        For Each c In columnDelete.Split(","c)
                            Dim cc As Integer
                            If Integer.TryParse(c, cc) Then oWorksheet.DeleteColumn(cc)
                        Next
                    End If

                    ' Confere header
                    For Each wsStructure As InterfaceExcelColumn In structureExcel.Where(Function(x) x.Visivel)

                        Dim cellValue As String = ""
                        If oWorksheet.Cells(wsStructure.Linha, wsStructure.Coluna).Value IsNot Nothing Then
                            cellValue = oWorksheet.Cells(wsStructure.Linha, wsStructure.Coluna).Value.ToString()
                        End If

                        If cellValue.Trim().ToUpper() <> wsStructure.ColunaExcel.Replace("[", "").Replace("]", "").Trim().ToUpper() Then
                            worksheetFound = False
                            Exit For
                        End If

                    Next

                    If worksheetFound Then
                        worksheetImport.Add(oWorksheet.Name)
                    End If

                Next

            End Using

            If worksheetImport.Count = 0 Then
                Throw New Exception("Nenhuma aba do Excel corresponde à estrutura esperada.")
            End If

            ' Para cada worksheet encontrada, BulkCopy
            For Each worksheetName As String In worksheetImport

                Dim querySheet As String = query.Replace("$WORKSHEET", worksheetName & "$")

                Using oOleDbConnection As New OleDbConnection(
                "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & filePath &
                ";Extended Properties='Excel 12.0 Xml;HDR=YES;IMEX=1';")

                    oOleDbConnection.Open()

                    Using oSqlConnection As New SqlConnection(sConnection),
                      oOleDbCommand As New OleDbCommand(querySheet, oOleDbConnection),
                      oOleDbReader As OleDbDataReader = oOleDbCommand.ExecuteReader(),
                      oBulk As New SqlBulkCopy(oSqlConnection)

                        oSqlConnection.Open()

                        oBulk.DestinationTableName = tabelaInsert
                        oBulk.BulkCopyTimeout = 5000

                        ' Mapeamento fixo (campos de controle)
                        'oBulk.ColumnMappings.Add("codigo_empresa", "codigo_empresa")
                        'oBulk.ColumnMappings.Add("uniqueId", "uniqueId")
                        'oBulk.ColumnMappings.Add("codigo_usuario", "codigo_usuario")

                        '' Mapeamento dinâmico baseado na estrutura
                        'For Each s In structureExcel
                        '    oBulk.ColumnMappings.Add(s.ColunaExcel.Replace("[", "").Replace("]", ""), s.DataMember)
                        'Next

                        oBulk.WriteToServer(oOleDbReader)

                    End Using

                End Using

            Next

            ' Atualiza campos de controle que não vêm do Excel (uniqueId, usuario, etc)
            If (storedProcedureUpdate <> "") Then

                Dim oUpdParams As SqlParameter() = {
                    CriarParametro("uniqueId", SqlDbType.VarChar, uniqueId)
                }

                ExecuteNonQuery(sConnection, CommandType.StoredProcedure, storedProcedureUpdate, oUpdParams)

            End If

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function LoadChecklistItemTmp(ByVal uniqueId As String, ByVal codigoEmpresa As Integer, ByVal codigoTipoChecklist As Integer) As ChecklistResponse

        Dim oReturn As New ChecklistResponse()

        Try

            Dim oQueryParameters As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("uniqueId", SqlDbType.VarChar, uniqueId),
                CriarParametro("codigo_tipo_checklist", SqlDbType.SmallInt, codigoTipoChecklist)
            }

            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_checklist_item_tmp", oQueryParameters)

                'Estrutura
                While oSqlDataReader.Read()

                    Dim col As New ColumnStructure()

                    col.data = oSqlDataReader("dataMember").ToString()
                    col.title = oSqlDataReader("colunaExcel").ToString()
                    col.visible = Convert.ToBoolean(oSqlDataReader("visivel"))
                    col.width = If(IsDBNull(oSqlDataReader("largura")), Nothing, oSqlDataReader("largura").ToString())
                    col.align = If(IsDBNull(oSqlDataReader("alinhamento")), Nothing, oSqlDataReader("alinhamento").ToString())
                    col.frozen = Convert.ToBoolean(oSqlDataReader("frozen"))
                    col.orderable = Convert.ToBoolean(oSqlDataReader("ordenavel"))
                    col.wrap = Convert.ToBoolean(oSqlDataReader("quebraLinha"))

                    oReturn.columns.Add(col)

                End While

                ' Ir para o próximo result set
                oSqlDataReader.NextResult()

                'Dados
                While oSqlDataReader.Read()

                    Dim row As New Dictionary(Of String, Object)

                    For i As Integer = 0 To oSqlDataReader.FieldCount - 1

                        Dim columnName As String = oSqlDataReader.GetName(i)

                        If columnName = "errorData" Then
                            Dim errorJson As String = If(oSqlDataReader.IsDBNull(i), "[]", oSqlDataReader.GetString(i))
                            row("errorData") = JsonConvert.DeserializeObject(Of List(Of InterfaceError))(errorJson)
                        Else
                            row(columnName) = If(oSqlDataReader.IsDBNull(i), Nothing, oSqlDataReader.GetValue(i))
                        End If

                    Next

                    oReturn.data.Add(row)

                End While

                ' Ir para o próximo result set
                oSqlDataReader.NextResult()

                'Dados
                While oSqlDataReader.Read()

                    Dim group As New GroupDefinition()

                    group.column = oSqlDataReader("column").ToString()
                    group.level = oSqlDataReader("level").ToString()
                    group.collapsible = Convert.ToBoolean(oSqlDataReader("collapsible"))
                    group.showCount = Convert.ToBoolean(oSqlDataReader("showCount"))
                    group.cssClass = oSqlDataReader("cssClass").ToString()

                    oReturn.groupBy.Add(group)

                End While


            End Using

            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function LoadChecklistExcel(ByVal codigoEmpresa As Integer,
                                       ByVal uniqueId As String,
                                       ByVal codigoTipoChecklist As Integer) As DataSet

        Dim oReturn As DataSet

        Try

            Dim oQueryParameters As SqlParameter() = {
                CriarParametro("uniqueId", SqlDbType.VarChar, uniqueId),
                CriarParametro("codigo_tipo_checklist", SqlDbType.Int, codigoTipoChecklist),
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa)
            }

            oReturn = ExecuteDataset(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_checklist_item_excel", oQueryParameters)

            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Sub InsertChecklist(ByVal codigoEmpresa As Integer,
                               ByVal codigoUnidade As Integer,
                               ByVal codigoModulo As Integer,
                               ByVal codigoTipoChecklist As Integer,
                               ByVal descricao As String,
                               ByVal uniqueId As String,
                               ByVal codigoUsuario As Integer)

        Try

            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, IIf(codigoUnidade = -1, DBNull.Value, codigoUnidade)),
                CriarParametro("codigo_modulo", SqlDbType.Int, codigoModulo),
                CriarParametro("codigo_tipo_checklist", SqlDbType.SmallInt, codigoTipoChecklist),
                CriarParametro("descricao", SqlDbType.VarChar, descricao),
                CriarParametro("uniqueId", SqlDbType.VarChar, uniqueId),
                CriarParametro("codigo_usuario", SqlDbType.Int, codigoUsuario)
            }

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_cadastro_basico_checklist_v2", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateChecklist(ByVal codigoEmpresa As Integer,
                               ByVal codigoUnidade As Integer,
                               ByVal descricao As String,
                               ByVal uniqueId As String,
                               ByVal codigoUsuario As Integer,
                               ByVal codigo As Long)

        Try

            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, IIf(codigoUnidade = -1, DBNull.Value, codigoUnidade)),
                CriarParametro("codigo", SqlDbType.BigInt, codigo),
                CriarParametro("descricao", SqlDbType.VarChar, descricao),
                CriarParametro("uniqueId", SqlDbType.VarChar, uniqueId),
                CriarParametro("codigo_usuario", SqlDbType.Int, codigoUsuario)
            }

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_cadastro_basico_checklist_v2", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function LoadChecklistInfo(ByVal codigoEmpresa As Integer,
                                      ByVal codigo As Long,
                                      ByVal codigoUsuario As Integer) As ChecklistInfo

        Try

            Dim oReturn As New ChecklistInfo
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo", SqlDbType.BigInt, codigo),
                CriarParametro("codigo_usuario", SqlDbType.Int, codigoUsuario)
            }

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_checklist_dados", oSqlParameter)

                While oSqlDataReader.Read

                    With oReturn
                        .codigo = SafeGetLong(oSqlDataReader, "codigo")
                        .uniqueId = SafeGetString(oSqlDataReader, "uniqueId")
                        .codigoUnidade = SafeGetLong(oSqlDataReader, "codigo_unidade")
                        .codigoTipoChecklist = SafeGetLong(oSqlDataReader, "codigo_tipo_checklist")
                        .descricao = SafeGetString(oSqlDataReader, "descricao")
                    End With

                End While

            End Using

            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: CLIENTE :::"

    Public Sub InsertCliente(ByVal codigoEmpresa As Integer,
                             ByVal codigoUsuario As Integer,
                             ByVal codigoUnidade As Integer,
                             ByVal nomeFantasia As String,
                             ByVal razaoSocial As String,
                             ByVal cnpj As String,
                             ByVal inscricaoEstadual As String,
                             ByVal inscricaoMunicipal As String,
                             ByVal cep As String,
                             ByVal uf As String,
                             ByVal municipio As String,
                             ByVal logradouro As String,
                             ByVal numero As String,
                             ByVal bairro As String,
                             ByVal complemento As String,
                             ByVal telefone As String,
                             ByVal email As String,
                             ByVal ativo As Boolean)

        'Variaveis Locais
        Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_usuario", SqlDbType.Int, codigoUsuario),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("nome_fantasia", SqlDbType.VarChar, nomeFantasia),
                CriarParametro("razao_social", SqlDbType.VarChar, razaoSocial),
                CriarParametro("cnpj", SqlDbType.VarChar, cnpj),
                CriarParametro("inscricao_estadual", SqlDbType.VarChar, inscricaoEstadual),
                CriarParametro("inscricao_municipal", SqlDbType.VarChar, inscricaoMunicipal),
                CriarParametro("cep", SqlDbType.VarChar, cep),
                CriarParametro("uf", SqlDbType.VarChar, uf),
                CriarParametro("municipio", SqlDbType.VarChar, municipio),
                CriarParametro("logradouro", SqlDbType.VarChar, logradouro),
                CriarParametro("numero", SqlDbType.VarChar, numero),
                CriarParametro("bairro", SqlDbType.VarChar, bairro),
                CriarParametro("complemento", SqlDbType.VarChar, complemento),
                CriarParametro("telefone", SqlDbType.VarChar, telefone),
                CriarParametro("email", SqlDbType.VarChar, email),
                CriarParametro("ativo", SqlDbType.Bit, ativo)
            }

        Try

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_cadastro_basico_cliente", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateCliente(ByVal codigoEmpresa As Integer,
                             ByVal codigoUsuario As Integer,
                             ByVal codigoUnidade As Integer,
                             ByVal nomeFantasia As String,
                             ByVal razaoSocial As String,
                             ByVal cnpj As String,
                             ByVal inscricaoEstadual As String,
                             ByVal inscricaoMunicipal As String,
                             ByVal cep As String,
                             ByVal uf As String,
                             ByVal municipio As String,
                             ByVal logradouro As String,
                             ByVal numero As String,
                             ByVal bairro As String,
                             ByVal complemento As String,
                             ByVal telefone As String,
                             ByVal email As String,
                             ByVal ativo As Boolean,
                             ByVal codigo As Integer)

        'Variaveis Locais
        Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_usuario", SqlDbType.Int, codigoUsuario),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("nome_fantasia", SqlDbType.VarChar, nomeFantasia),
                CriarParametro("razao_social", SqlDbType.VarChar, razaoSocial),
                CriarParametro("cnpj", SqlDbType.VarChar, cnpj),
                CriarParametro("inscricao_estadual", SqlDbType.VarChar, inscricaoEstadual),
                CriarParametro("inscricao_municipal", SqlDbType.VarChar, inscricaoMunicipal),
                CriarParametro("cep", SqlDbType.VarChar, cep),
                CriarParametro("uf", SqlDbType.VarChar, uf),
                CriarParametro("municipio", SqlDbType.VarChar, municipio),
                CriarParametro("logradouro", SqlDbType.VarChar, logradouro),
                CriarParametro("numero", SqlDbType.VarChar, numero),
                CriarParametro("bairro", SqlDbType.VarChar, bairro),
                CriarParametro("complemento", SqlDbType.VarChar, complemento),
                CriarParametro("telefone", SqlDbType.VarChar, telefone),
                CriarParametro("email", SqlDbType.VarChar, email),
                CriarParametro("ativo", SqlDbType.Bit, ativo),
                CriarParametro("codigo", SqlDbType.Int, codigo)
            }

        Try

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_cadastro_basico_cliente", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub DeleteCliente(ByVal codigoEmpresa As Integer,
                             ByVal codigoUsuario As Integer,
                             ByVal codigo As Integer)

        'Variaveis Locais
        Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_usuario", SqlDbType.Int, codigoUsuario),
                CriarParametro("codigo", SqlDbType.Int, codigo)
            }

        Try

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_cadastro_basico_cliente", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function InfoCliente(ByVal codigoEmpresa As Integer,
                                ByVal codigo As Integer) As Cliente

        'Variaveis Locais
        Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo", SqlDbType.Int, codigo)
            }
        Dim oReturn As New Cliente

        Try

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_cliente_dados", oSqlParameter)

                While oSqlDataReader.Read

                    oReturn = New Cliente
                    oReturn.codigo = oSqlDataReader.Item("codigo")
                    oReturn.codigoUnidade = oSqlDataReader.Item("codigo_unidade")
                    oReturn.unidade = oSqlDataReader.Item("unidade")
                    oReturn.nomeFantasia = oSqlDataReader.Item("nome_fantasia")
                    oReturn.razaoSocial = oSqlDataReader.Item("razao_social")
                    oReturn.cnpj = oSqlDataReader.Item("cnpj")
                    oReturn.inscricaoEstadual = oSqlDataReader.Item("inscricao_estadual")
                    oReturn.inscricaoMunicipal = oSqlDataReader.Item("inscricao_municipal")
                    oReturn.cep = oSqlDataReader.Item("cep")
                    oReturn.uf = oSqlDataReader.Item("uf")
                    oReturn.municipio = oSqlDataReader.Item("municipio")
                    oReturn.logradouro = oSqlDataReader.Item("logradouro")
                    oReturn.numero = oSqlDataReader.Item("numero")
                    oReturn.bairro = oSqlDataReader.Item("bairro")
                    oReturn.complemento = oSqlDataReader.Item("complemento")
                    oReturn.telefone = oSqlDataReader.Item("telefone")
                    oReturn.email = oSqlDataReader.Item("email")
                    oReturn.ativo = oSqlDataReader.Item("ativo")

                End While

            End Using

            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function IndexCliente(ByVal codigoEmpresa As Integer,
                                 ByVal codigoUnidade As Integer,
                                 ByVal nomeFantasia As String) As List(Of Cliente)

        Try

            'Váriavies Locais
            Dim oReturn As New List(Of Cliente)
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("nome_fantasia", SqlDbType.VarChar, nomeFantasia)
            }

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_cliente", oSqlParameter)

                While oSqlDataReader.Read

                    Dim oClienteInfo As New Cliente

                    oClienteInfo.codigo = oSqlDataReader.Item("codigo")
                    oClienteInfo.codigoUnidade = oSqlDataReader.Item("codigo_unidade")
                    oClienteInfo.unidade = oSqlDataReader.Item("unidade")
                    oClienteInfo.nomeFantasia = oSqlDataReader.Item("nome_fantasia")
                    oClienteInfo.razaoSocial = oSqlDataReader.Item("razao_social")
                    oClienteInfo.cnpj = oSqlDataReader.Item("cnpj")
                    oClienteInfo.inscricaoEstadual = oSqlDataReader.Item("inscricao_estadual")
                    oClienteInfo.inscricaoMunicipal = oSqlDataReader.Item("inscricao_municipal")
                    oClienteInfo.cep = oSqlDataReader.Item("cep")
                    oClienteInfo.uf = oSqlDataReader.Item("uf")
                    oClienteInfo.municipio = oSqlDataReader.Item("municipio")
                    oClienteInfo.logradouro = oSqlDataReader.Item("logradouro")
                    oClienteInfo.numero = oSqlDataReader.Item("numero")
                    oClienteInfo.bairro = oSqlDataReader.Item("bairro")
                    oClienteInfo.complemento = oSqlDataReader.Item("complemento")
                    oClienteInfo.telefone = oSqlDataReader.Item("telefone")
                    oClienteInfo.email = oSqlDataReader.Item("email")
                    oClienteInfo.ativo = oSqlDataReader.Item("ativo")

                    oReturn.Add(oClienteInfo)

                End While

            End Using

            'Retorno da Função
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ValidaCliente(ByVal codigoEmpresa As Integer,
                                  ByVal codigoUnidade As Integer,
                                  ByVal cnpj As String,
                                  ByVal codigo As Integer) As Boolean

        Try

            'Váriavies Locais
            Dim iReturn As Integer
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("cnpj", SqlDbType.VarChar, cnpj),
                CriarParametro("codigo", SqlDbType.Int, codigo)
            }

            'Executa Query
            iReturn = ExecuteScalar(sConnection, CommandType.StoredProcedure, "sp_validate_cadastro_basico_cliente", oSqlParameter)

            'Retorno da Função
            Return (IIf(iReturn > 0, False, True))

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: CLIENTE - ACORDO COMERCIAL :::"

    Public Function LoadClienteEnxoval(ByVal codigoEmpresa As Integer,
                                       ByVal codigoUnidade As Integer,
                                       ByVal codigoCliente As Integer) As List(Of ClienteAcordoComercial)

        Try

            'Váriavies Locais
            Dim oReturn As New List(Of ClienteAcordoComercial)
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("codigo_cliente", SqlDbType.Int, codigoCliente)
            }

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_cliente_enxoval", oSqlParameter)

                While oSqlDataReader.Read

                    Dim oInfo As New ClienteAcordoComercial

                    oInfo.enxoval = oSqlDataReader.Item("enxoval")
                    oInfo.quantidade = oSqlDataReader.Item("quantidade")
                    oInfo.valorUnitario = oSqlDataReader.Item("valor_unitario")
                    oInfo.codigoEnxoval = oSqlDataReader.Item("codigo_enxoval")

                    oReturn.Add(oInfo)

                End While

            End Using

            'Retorno da Função
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Sub DeleteClienteEnxoval(ByVal codigoEmpresa As Integer,
                                    ByVal codigoUnidade As Integer,
                                    ByVal codigoCliente As Integer,
                                    ByVal codigoEnxoval As Integer)

        Try

            'Váriavies Locais
            Dim oReturn As New List(Of ClienteAcordoComercial)
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("codigo_cliente", SqlDbType.Int, codigoCliente),
                CriarParametro("codigo_enxoval", SqlDbType.Int, codigoEnxoval)
            }

            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_cadastro_basico_cliente_enxoval", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InsertClienteEnxoval(ByVal codigoEmpresa As Integer,
                                    ByVal codigoUnidade As Integer,
                                    ByVal codigoCliente As Integer,
                                    ByVal codigoEnxoval As Integer,
                                    ByVal quantidade As Integer,
                                    ByVal valorUnitario As Double)

        Try

            'Váriavies Locais
            Dim oReturn As New List(Of ClienteAcordoComercial)
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("codigo_cliente", SqlDbType.Int, codigoCliente),
                CriarParametro("codigo_enxoval", SqlDbType.Int, codigoEnxoval),
                CriarParametro("quantidade", SqlDbType.Int, quantidade),
                CriarParametro("valor_unitario", SqlDbType.Float, valorUnitario)
            }

            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_cadastro_basico_cliente_enxoval", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region

#Region "::: DEPARTAMENTO :::"

    Public Sub InsertDepartamento(ByVal iCodigoEmpresa As Integer,
                                  ByVal iCodigoUsuario As Integer,
                                  ByVal sDescricao As String,
                                  ByVal bAtivo As Boolean)

        'Variaveis Locais
        Dim oSqlParameter(3) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sDescricao : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_cadastro_basico_departamento", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateDepartamento(ByVal iCodigoEmpresa As Integer,
                                  ByVal iCodigoUsuario As Integer,
                                  ByVal sDescricao As String,
                                  ByVal bAtivo As Boolean,
                                  ByVal iCodigo As Integer)

        'Variaveis Locais
        Dim oSqlParameter(4) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sDescricao : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_cadastro_basico_departamento", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub DeleteDepartamento(ByVal iCodigoEmpresa As Integer,
                                  ByVal iCodigoUsuario As Integer,
                                  ByVal iCodigo As Integer)

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

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_cadastro_basico_departamento", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InfoDepartamento(ByVal iCodigoEmpresa As Integer,
                                ByVal iCodigo As Integer,
                                ByRef oDepartamento As Departamento)

        'Variaveis Locais
        Dim oSqlParameter(1) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_departamento_dados", oSqlParameter)

            While oSqlDataReader.Read

                oDepartamento = New Departamento
                oDepartamento.codigo = oSqlDataReader.Item("codigo")
                oDepartamento.descricao = oSqlDataReader.Item("descricao")
                oDepartamento.ativo = oSqlDataReader.Item("ativo")

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function IndexDepartamento(ByVal iCodigoEmpresa As Integer,
                                      ByVal iCodigoUsuario As Integer) As List(Of Departamento)

        Try

            'Váriaveis Locais
            Dim oDepartamento As New List(Of Departamento)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(1) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_departamento", oSqlParameter)

            While oSqlDataReader.Read

                Dim oDepartamentoInfo As New Departamento

                oDepartamentoInfo.descricao = oSqlDataReader.Item("descricao")
                oDepartamentoInfo.ativo = oSqlDataReader.Item("ativo")
                oDepartamentoInfo.codigo = oSqlDataReader.Item("codigo")

                oDepartamento.Add(oDepartamentoInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Departamento
            Return oDepartamento

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ValidaDepartamento(ByVal iCodigoEmpresa As Integer,
                                       ByVal sDescricao As String,
                                       ByVal iCodigo As Integer) As Boolean

        Try

            'Váriaveis Locais
            Dim oSqlParameter(2) As SqlParameter
            Dim i As Integer = 0
            Dim iReturn As Integer

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sDescricao

            'Executa Query
            iReturn = ExecuteScalar(sConnection, CommandType.StoredProcedure, "sp_validate_cadastro_basico_departamento", oSqlParameter)

            'Retorno da Departamento
            Return (IIf(iReturn > 0, False, True))

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: DEPARTAMENTO - GESTOR :::"

    Public Sub InsertDepartamentoGestor(ByVal iCodigoEmpresa As Integer,
                                        ByVal iCodigoDepartamento As Integer,
                                        ByVal iCodigoUnidade As Integer,
                                        ByVal iCodigoUsuario As Integer)

        'Variaveis Locais
        Dim oSqlParameter(3) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Departamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_departamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoDepartamento : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_cadastro_basico_departamento_gestor", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateDepartamentoGestor(ByVal iCodigoEmpresa As Integer,
                                        ByVal iCodigoDepartamento As Integer,
                                        ByVal iCodigoUnidade As Integer,
                                        ByVal iCodigoUsuario As Integer,
                                        ByVal iCodigo As Integer)

        'Variaveis Locais
        Dim oSqlParameter(4) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Departamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_departamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoDepartamento : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_cadastro_basico_departamento_gestor", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub DeleteDepartamentoGestor(ByVal iCodigoEmpresa As Integer,
                                        ByVal iCodigo As Integer)

        'Variaveis Locais
        Dim oSqlParameter(1) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_cadastro_basico_departamento_gestor", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InfoDepartamentoGestor(ByVal iCodigoEmpresa As Integer,
                                      ByVal iCodigo As Integer,
                                      ByRef oDepartamentoGestor As DepartamentoGestor)

        'Variaveis Locais
        Dim oSqlParameter(1) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_departamento_gestor_dados", oSqlParameter)

            While oSqlDataReader.Read

                oDepartamentoGestor = New DepartamentoGestor
                oDepartamentoGestor.codigo_departamento = oSqlDataReader.Item("codigo_departamento")
                oDepartamentoGestor.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oDepartamentoGestor.codigo_usuario = oSqlDataReader.Item("codigo_usuario")
                oDepartamentoGestor.unidade = oSqlDataReader.Item("unidade")
                oDepartamentoGestor.departamento = oSqlDataReader.Item("departamento")
                oDepartamentoGestor.nome = oSqlDataReader.Item("nome")
                oDepartamentoGestor.codigo = oSqlDataReader.Item("codigo")

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function IndexDepartamentoGestor(ByVal iCodigoEmpresa As Integer,
                                            ByVal iCodigoUnidade As Integer) As List(Of DepartamentoGestor)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of DepartamentoGestor)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(1) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_departamento_gestor", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New DepartamentoGestor

                oInfo.codigo_departamento = oSqlDataReader.Item("codigo_departamento")
                oInfo.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oInfo.codigo_usuario = oSqlDataReader.Item("codigo_usuario")
                oInfo.unidade = oSqlDataReader.Item("unidade")
                oInfo.departamento = oSqlDataReader.Item("departamento")
                oInfo.nome = oSqlDataReader.Item("nome")
                oInfo.codigo = oSqlDataReader.Item("codigo")

                oReturn.Add(oInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Departamento
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ValidaDepartamentoGestor(ByVal iCodigoEmpresa As Integer,
                                             ByVal iCodigoDepartamento As Integer,
                                             ByVal iCodigoUsuario As Integer,
                                             ByVal iCodigo As Integer) As Boolean

        Try

            'Váriaveis Locais
            Dim oSqlParameter(3) As SqlParameter
            Dim i As Integer = 0
            Dim iReturn As Integer

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo : i += 1

            'Seta Parametros - Código Departamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_departamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoDepartamento : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario

            'Executa Query
            iReturn = ExecuteScalar(sConnection, CommandType.StoredProcedure, "sp_validate_cadastro_basico_departamento_gestor", oSqlParameter)

            'Retorno da Departamento
            Return (IIf(iReturn > 0, False, True))

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: DEDETIZAÇÃO :::"

    Public Sub InsertDedetizacao(ByVal iCodigoEmpresa As Integer,
                                 ByVal iCodigoUnidade As Integer,
                                 ByVal iPeriodicidade As Integer,
                                 ByVal iAlerta As Integer,
                                 ByVal iCodigoTipoServico As Integer,
                                 ByVal sDataInicio As String,
                                 ByRef iCodigo As Integer)

        'Variaveis Locais
        Dim oSqlParameter(6) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Periodicidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "periodicidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iPeriodicidade : i += 1

            'Seta Parametros - Alerta
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "alerta"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iAlerta : i += 1

            'Seta Parametros - Código Tipo de Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_tipo_servico"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoTipoServico : i += 1

            'Seta Parametros - Data Início
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_inicio"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataInicio : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Output
            oSqlParameter(i).SqlDbType = SqlDbType.Int

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_cadastro_basico_dedetizacao", oSqlParameter)

            iCodigo = oSqlParameter(i).Value

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InsertDedetizacaoApartamento(ByVal iCodigoEmpresa As Integer,
                                            ByVal iCodigoUnidade As Integer,
                                            ByVal iCodigo As Integer,
                                            ByVal lCodigoApartamento As Long)

        'Variaveis Locais
        Dim oSqlParameter(3) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo : i += 1

            'Seta Parametros - Código Apartamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_apartamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoApartamento

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_cadastro_basico_dedetizacao_apartamento", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function InfoDedetizacao(ByVal iCodigoEmpresa As Integer,
                                    ByVal iCodigoUnidade As Integer) As Dedetizacao

        'Variaveis Locais
        Dim oSqlParameter(1) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New Dedetizacao
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_dedetizacao_dados", oSqlParameter)

            While oSqlDataReader.Read

                oReturn.codigo = oSqlDataReader.Item("codigo")
                oReturn.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oReturn.periodicidade = oSqlDataReader.Item("periodicidade")
                oReturn.alerta = oSqlDataReader.Item("alerta")
                oReturn.codigo_tipo_servico = oSqlDataReader.Item("codigo_tipo_servico")
                oReturn.data_inicio = oSqlDataReader.Item("data_inicio")

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function DedetizacaoApartamento(ByVal iCodigoEmpresa As Integer,
                                           ByVal iCodigoUnidade As Integer) As List(Of DedetizacaoApartamento)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of DedetizacaoApartamento)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(1) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_dedetizacao_apartamento", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New DedetizacaoApartamento

                oInfo.codigo_apartamento = oSqlDataReader.Item("codigo_apartamento")
                oInfo.setor = oSqlDataReader.Item("setor")
                oInfo.apartamento = oSqlDataReader.Item("apartamento")
                oInfo.tipo_apartamento = oSqlDataReader.Item("tipo_apartamento")
                oInfo.bloco = oSqlDataReader.Item("bloco")
                oInfo.andar = oSqlDataReader.Item("andar")
                oInfo.selected = oSqlDataReader.Item("selected")

                oReturn.Add(oInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: EQUIPAMENTO :::"

    Public Sub InsertEquipamento(ByVal iCodigoEmpresa As Integer,
                                 ByVal iCodigoUsuario As Integer,
                                 ByVal iCodigoUnidade As Integer,
                                 ByVal iCodigoFamiliaEquipamento As Integer,
                                 ByVal iCodigoDepartamento As Integer,
                                 ByVal sTag As String,
                                 ByVal sDescricao As String,
                                 ByVal iCodigoSetor As Integer,
                                 ByVal iCodigoApartamento As Integer,
                                 ByVal sFabricante As String,
                                 ByVal sEnderecoFabricante As String,
                                 ByVal sContatoFabricante As String,
                                 ByVal sModelo As String,
                                 ByVal sNumeroFabricacao As String,
                                 ByVal sAnoFabricacao As String,
                                 ByVal sCaracteristicas As String,
                                 ByVal sProgramada As String,
                                 ByVal sDescricaoOperacao As String,
                                 ByVal sInstrucaoUtilizacao As String,
                                 ByVal sProcedimentoEmergencia As String,
                                 ByVal sTreinamentoOperador As String,
                                 ByVal sCondicaoSeguranca As String,
                                 ByVal sIndicacaoConclusiva As String,
                                 ByVal sPathArquivo As String,
                                 ByVal sArquivo As String,
                                 ByVal bAtivo As Boolean)

        'Variaveis Locais
        Dim oSqlParameter(25) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Família Equipamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_familia_equipamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoFamiliaEquipamento = -1, DBNull.Value, iCodigoFamiliaEquipamento) : i += 1

            'Seta Parametros - Tag
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "tag"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sTag.ToUpper : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 200
            oSqlParameter(i).Value = sDescricao.ToUpper : i += 1

            'Seta Parametros - Código Setor
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_setor"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoSetor : i += 1

            'Seta Parametros - Código Apartamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_apartamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoApartamento = -1, DBNull.Value, iCodigoApartamento) : i += 1

            'Seta Parametros - Fabricante
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "fabricante"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sFabricante : i += 1

            'Seta Parametros - Endereço Fabricante
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "endereco_fabricante"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 200
            oSqlParameter(i).Value = sEnderecoFabricante.ToUpper : i += 1

            'Seta Parametros - Contato Fabricante
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "contato_fabricante"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sContatoFabricante.ToUpper : i += 1

            'Seta Parametros - Modelo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "modelo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sModelo.ToUpper : i += 1

            'Seta Parametros - Nº Fabricação
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "numero_fabricacao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sNumeroFabricacao.ToUpper : i += 1

            'Seta Parametros - Ano Fabricação
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ano_fabricacao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(IsNumeric(sAnoFabricacao), sAnoFabricacao, DBNull.Value) : i += 1

            'Seta Parametros - Características
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "caracteristicas"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = sCaracteristicas : i += 1

            'Seta Parametros - Programada
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "programada"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = sProgramada : i += 1

            'Seta Parametros - Descrição Operação
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao_operacao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = sDescricaoOperacao.ToUpper() : i += 1

            'Seta Parametros - Instrução Utilização
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "instrucao_utilizacao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = sInstrucaoUtilizacao.ToUpper() : i += 1

            'Seta Parametros - Procedimento Emergência
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "procedimento_emergencia"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = sProcedimentoEmergencia.ToUpper() : i += 1

            'Seta Parametros - Treinamento Operador
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "treinamento_operador"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = sTreinamentoOperador.ToUpper() : i += 1

            'Seta Parametros - Condição de Segurança
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "condicao_seguranca"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = sCondicaoSeguranca.ToUpper() : i += 1

            'Seta Parametros - Indicação Conclusiva
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "indicacao_conclusiva"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = sIndicacaoConclusiva.ToUpper() : i += 1

            'Seta Parametros - Path Arquivo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "path_arquivo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 250
            oSqlParameter(i).Value = sPathArquivo : i += 1

            'Seta Parametros - Arquivo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "arquivo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 250
            oSqlParameter(i).Value = sArquivo : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo : i += 1

            'Seta Parametros - Código Departamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_departamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoDepartamento

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_cadastro_basico_equipamento", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateEquipamento(ByVal iCodigoEmpresa As Integer,
                                 ByVal iCodigoUsuario As Integer,
                                 ByVal iCodigoUnidade As Integer,
                                 ByVal iCodigoFamiliaEquipamento As Integer,
                                 ByVal iCodigoDepartamento As Integer,
                                 ByVal sTag As String,
                                 ByVal sDescricao As String,
                                 ByVal iCodigoSetor As Integer,
                                 ByVal iCodigoApartamento As Integer,
                                 ByVal sFabricante As String,
                                 ByVal sEnderecoFabricante As String,
                                 ByVal sContatoFabricante As String,
                                 ByVal sModelo As String,
                                 ByVal sNumeroFabricacao As String,
                                 ByVal sAnoFabricacao As String,
                                 ByVal sCaracteristicas As String,
                                 ByVal sProgramada As String,
                                 ByVal sDescricaoOperacao As String,
                                 ByVal sInstrucaoUtilizacao As String,
                                 ByVal sProcedimentoEmergencia As String,
                                 ByVal sTreinamentoOperador As String,
                                 ByVal sCondicaoSeguranca As String,
                                 ByVal sIndicacaoConclusiva As String,
                                 ByVal sPathArquivo As String,
                                 ByVal sArquivo As String,
                                 ByVal bAtivo As Boolean,
                                 ByVal lCodigo As Long)

        'Variaveis Locais
        Dim oSqlParameter(26) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Família Equipamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_familia_equipamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoFamiliaEquipamento = -1, DBNull.Value, iCodigoFamiliaEquipamento) : i += 1

            'Seta Parametros - Tag
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "tag"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sTag.ToUpper : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 200
            oSqlParameter(i).Value = sDescricao.ToUpper : i += 1

            'Seta Parametros - Código Setor
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_setor"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoSetor : i += 1

            'Seta Parametros - Código Apartamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_apartamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoApartamento = -1, DBNull.Value, iCodigoApartamento) : i += 1

            'Seta Parametros - Fabricante
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "fabricante"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sFabricante : i += 1

            'Seta Parametros - Endereço Fabricante
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "endereco_fabricante"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 200
            oSqlParameter(i).Value = sEnderecoFabricante.ToUpper : i += 1

            'Seta Parametros - Contato Fabricante
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "contato_fabricante"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sContatoFabricante.ToUpper : i += 1

            'Seta Parametros - Modelo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "modelo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sModelo.ToUpper : i += 1

            'Seta Parametros - Nº Fabricação
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "numero_fabricacao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sNumeroFabricacao.ToUpper : i += 1

            'Seta Parametros - Ano Fabricação
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ano_fabricacao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(IsNumeric(sAnoFabricacao), sAnoFabricacao, DBNull.Value) : i += 1

            'Seta Parametros - Características
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "caracteristicas"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = sCaracteristicas : i += 1

            'Seta Parametros - Programada
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "programada"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = sProgramada : i += 1

            'Seta Parametros - Descrição Operação
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao_operacao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = sDescricaoOperacao.ToUpper() : i += 1

            'Seta Parametros - Instrução Utilização
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "instrucao_utilizacao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = sInstrucaoUtilizacao.ToUpper() : i += 1

            'Seta Parametros - Procedimento Emergência
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "procedimento_emergencia"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = sProcedimentoEmergencia.ToUpper() : i += 1

            'Seta Parametros - Treinamento Operador
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "treinamento_operador"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = sTreinamentoOperador.ToUpper() : i += 1

            'Seta Parametros - Condição de Segurança
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "condicao_seguranca"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = sCondicaoSeguranca.ToUpper() : i += 1

            'Seta Parametros - Indicação Conclusiva
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "indicacao_conclusiva"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = sIndicacaoConclusiva.ToUpper() : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo : i += 1

            'Seta Parametros - Path Arquivo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "path_arquivo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 250
            oSqlParameter(i).Value = sPathArquivo : i += 1

            'Seta Parametros - Arquivo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "arquivo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 250
            oSqlParameter(i).Value = sArquivo : i += 1

            'Seta Parametros - Código Departamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_departamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoDepartamento = -1, DBNull.Value, iCodigoDepartamento) : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = lCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_cadastro_basico_equipamento", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub DeleteEquipamento(ByVal iCodigoEmpresa As Integer,
                                 ByVal iCodigoUsuario As Integer,
                                 ByVal lCodigo As Long)

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

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_cadastro_basico_equipamento", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InfoEquipamento(ByVal iCodigoEmpresa As Integer,
                               ByVal lCodigo As Long,
                               ByRef oEquipamento As Equipamento)

        'Variaveis Locais
        Dim oSqlParameter(1) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_equipamento_dados", oSqlParameter)

            While oSqlDataReader.Read

                oEquipamento = New Equipamento
                oEquipamento.codigo = oSqlDataReader("codigo")
                oEquipamento.tag = oSqlDataReader.Item("tag")
                oEquipamento.codigo_unidade = oSqlDataReader("codigo_unidade")
                oEquipamento.unidade = oSqlDataReader("unidade")
                oEquipamento.codigo_familia_equipamento = oSqlDataReader("codigo_familia_equipamento")
                oEquipamento.familia = oSqlDataReader("familia")
                oEquipamento.codigo_departamento = oSqlDataReader("codigo_departamento")
                oEquipamento.descricao = oSqlDataReader("descricao")
                oEquipamento.codigo_setor = oSqlDataReader("codigo_setor")
                oEquipamento.setor = oSqlDataReader("setor")
                oEquipamento.codigo_apartamento = oSqlDataReader("codigo_apartamento")
                oEquipamento.apartamento = oSqlDataReader("apartamento")
                oEquipamento.fabricante = oSqlDataReader("fabricante")
                oEquipamento.endereco_fabricante = oSqlDataReader("endereco_fabricante")
                oEquipamento.contato_fabricante = oSqlDataReader("contato_fabricante")
                oEquipamento.modelo = oSqlDataReader("modelo")
                oEquipamento.numero_fabricacao = oSqlDataReader("numero_fabricacao")
                oEquipamento.ano_fabricacao = oSqlDataReader("ano_fabricacao")
                oEquipamento.caracteristicas = oSqlDataReader("caracteristicas")
                oEquipamento.programada = oSqlDataReader.Item("programada")
                oEquipamento.descricao_operacao = oSqlDataReader.Item("descricao_operacao")
                oEquipamento.instrucao_utilizacao = oSqlDataReader.Item("instrucao_utilizacao")
                oEquipamento.procedimento_emergencia = oSqlDataReader.Item("procedimento_emergencia")
                oEquipamento.treinamento_operador = oSqlDataReader.Item("treinamento_operador")
                oEquipamento.condicao_seguranca = oSqlDataReader.Item("condicao_seguranca")
                oEquipamento.indicacao_conclusiva = oSqlDataReader.Item("indicacao_conclusiva")
                oEquipamento.path_arquivo = oSqlDataReader.Item("path_arquivo")
                oEquipamento.arquivo = oSqlDataReader.Item("arquivo")
                oEquipamento.ativo = oSqlDataReader.Item("ativo")

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function IndexEquipamento(ByVal iCodigoEmpresa As Integer,
                                     ByVal iCodigoUsuario As Integer,
                                     ByVal iCodigoUnidade As Integer,
                                     ByVal sTAG As String,
                                     ByVal iCodigoSetor As Integer,
                                     ByVal lCodigoApartamento As Long,
                                     ByVal iCodigoDepartamento As Integer,
                                     ByVal iAtivo As Integer) As List(Of Equipamento)

        Try

            'Váriaveis Locais
            Dim oEquipamento As New List(Of Equipamento)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(7) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - TAG
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "tag"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sTAG : i += 1

            'Seta Parametros - Código Setor
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_setor"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoSetor : i += 1

            'Seta Parametros - Código Apartamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_apartamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoApartamento : i += 1

            'Seta Parametros - Código Departamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_departamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoDepartamento : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = IIf(iAtivo = 1, True, IIf(iAtivo = 0, False, DBNull.Value))

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_equipamento", oSqlParameter)

            While oSqlDataReader.Read

                Dim oEquipamentoInfo As New Equipamento

                oEquipamentoInfo.codigo = oSqlDataReader.Item("codigo")
                oEquipamentoInfo.tag = oSqlDataReader.Item("tag")
                oEquipamentoInfo.unidade = oSqlDataReader.Item("unidade")
                oEquipamentoInfo.familia = oSqlDataReader.Item("familia")
                oEquipamentoInfo.setor = oSqlDataReader.Item("setor")
                oEquipamentoInfo.apartamento = oSqlDataReader.Item("apartamento")
                oEquipamentoInfo.descricao = oSqlDataReader.Item("descricao")
                oEquipamentoInfo.ativo = oSqlDataReader.Item("ativo")

                oEquipamento.Add(oEquipamentoInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oEquipamento

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ValidaEquipamento(ByVal iCodigoEmpresa As Integer,
                                      ByVal iCodigoUnidade As Integer,
                                      ByVal sTag As String,
                                      ByVal iCodigo As Integer) As Boolean

        Try

            'Váriaveis Locais
            Dim oSqlParameter(3) As SqlParameter
            Dim i As Integer = 0
            Dim iReturn As Integer

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo : i += 1

            'Seta Parametros - Tag
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "tag"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sTag

            'Executa Query
            iReturn = ExecuteScalar(sConnection, CommandType.StoredProcedure, "sp_validate_cadastro_basico_equipamento", oSqlParameter)

            'Retorno da Função
            Return (IIf(iReturn > 0, False, True))

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: ENXOVAL :::"

    Public Sub InsertEnxoval(ByVal iCodigoEmpresa As Integer,
                             ByVal iCodigoUsuario As Integer,
                             ByVal iCodigoUnidade As Integer,
                             ByVal sDescricao As String,
                             ByVal dPeso As Double,
                             ByVal bAtivo As Boolean)

        'Variaveis Locais
        Dim oSqlParameter(5) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sDescricao : i += 1

            'Seta Parametros - Peso
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "peso"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = dPeso : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_cadastro_basico_enxoval", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateEnxoval(ByVal iCodigoEmpresa As Integer,
                             ByVal iCodigoUsuario As Integer,
                             ByVal iCodigoUnidade As Integer,
                             ByVal sDescricao As String,
                             ByVal dPeso As Double,
                             ByVal bAtivo As Boolean,
                             ByVal iCodigo As Integer)

        'Variaveis Locais
        Dim oSqlParameter(6) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sDescricao : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo : i += 1

            'Seta Parametros - Peso
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "peso"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = dPeso : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_cadastro_basico_enxoval", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub DeleteEnxoval(ByVal iCodigoEmpresa As Integer,
                             ByVal iCodigoUsuario As Integer,
                             ByVal iCodigo As Integer)

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

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_cadastro_basico_enxoval", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InfoEnxoval(ByVal iCodigoEmpresa As Integer,
                           ByVal iCodigo As Integer,
                           ByRef oEnxoval As Enxoval)

        'Variaveis Locais
        Dim oSqlParameter(1) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_enxoval_dados", oSqlParameter)

            While oSqlDataReader.Read

                oEnxoval = New Enxoval
                oEnxoval.codigo = oSqlDataReader.Item("codigo")
                oEnxoval.descricao = oSqlDataReader.Item("descricao")
                oEnxoval.peso = oSqlDataReader.Item("peso")
                oEnxoval.ativo = oSqlDataReader.Item("ativo")
                oEnxoval.unidade = oSqlDataReader.Item("unidade")
                oEnxoval.codigo_unidade = oSqlDataReader.Item("codigo_unidade")

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function IndexEnxoval(ByVal iCodigoEmpresa As Integer,
                                 ByVal iCodigoUsuario As Integer,
                                 ByVal iCodigoUnidade As Integer,
                                 ByVal sDescricao As String,
                                 ByVal iAtivo As Integer) As List(Of Enxoval)

        Try

            'Váriaveis Locais
            Dim oEnxoval As New List(Of Enxoval)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(4) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 150
            oSqlParameter(i).Value = sDescricao : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = IIf(iAtivo = -1, DBNull.Value, IIf(iAtivo = 1, True, False))

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_enxoval", oSqlParameter)

            While oSqlDataReader.Read

                Dim oEnxovalInfo As New Enxoval

                oEnxovalInfo.descricao = oSqlDataReader.Item("descricao")
                oEnxovalInfo.peso = oSqlDataReader.Item("peso")
                oEnxovalInfo.ativo = oSqlDataReader.Item("ativo")
                oEnxovalInfo.texto_ativo = IIf(oSqlDataReader.Item("ativo"), "SIM", "NÃO")
                oEnxovalInfo.codigo = oSqlDataReader.Item("codigo")
                oEnxovalInfo.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oEnxovalInfo.unidade = oSqlDataReader.Item("unidade")

                oEnxoval.Add(oEnxovalInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oEnxoval

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ValidaEnxoval(ByVal iCodigoEmpresa As Integer,
                                  ByVal iCodigoUnidade As Integer,
                                  ByVal sDescricao As String,
                                  ByVal iCodigo As Integer) As Boolean

        Try

            'Váriaveis Locais
            Dim oSqlParameter(3) As SqlParameter
            Dim i As Integer = 0
            Dim iReturn As Integer

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sDescricao

            'Executa Query
            iReturn = ExecuteScalar(sConnection, CommandType.StoredProcedure, "sp_validate_cadastro_basico_enxoval", oSqlParameter)

            'Retorno da Função
            Return (IIf(iReturn > 0, False, True))

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: FAMÍLIA - EQUIPAMENTO :::"

    Public Sub InsertFamiliaEquipamento(ByVal iCodigoEmpresa As Integer,
                                        ByVal iCodigoUsuario As Integer,
                                        ByVal iCodigoUnidade As Integer,
                                        ByVal sDescricao As String,
                                        ByVal bAtivo As Boolean)

        'Variaveis Locais
        Dim oSqlParameter(4) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 250
            oSqlParameter(i).Value = sDescricao : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_cadastro_basico_familia_equipamento", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateFamiliaEquipamento(ByVal iCodigoEmpresa As Integer,
                                        ByVal iCodigoUsuario As Integer,
                                        ByVal iCodigoUnidade As Integer,
                                        ByVal sDescricao As String,
                                        ByVal bAtivo As Boolean,
                                        ByVal iCodigo As Integer)

        'Variaveis Locais
        Dim oSqlParameter(5) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 250
            oSqlParameter(i).Value = sDescricao : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_cadastro_basico_familia_equipamento", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub DeleteFamiliaEquipamento(ByVal iCodigoEmpresa As Integer,
                                        ByVal iCodigoUsuario As Integer,
                                        ByVal iCodigo As Integer)

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

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_cadastro_basico_familia_equipamento", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InfoFamiliaEquipamento(ByVal iCodigoEmpresa As Integer,
                                      ByVal iCodigo As Integer,
                                      ByRef oFamiliaEquipamento As FamiliaEquipamento)

        'Variaveis Locais
        Dim oSqlParameter(1) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_familia_equipamento_dados", oSqlParameter)

            While oSqlDataReader.Read

                oFamiliaEquipamento = New FamiliaEquipamento
                oFamiliaEquipamento.codigo = oSqlDataReader.Item("codigo")
                oFamiliaEquipamento.descricao = oSqlDataReader.Item("descricao")
                oFamiliaEquipamento.ativo = oSqlDataReader.Item("ativo")
                oFamiliaEquipamento.unidade = oSqlDataReader.Item("unidade")
                oFamiliaEquipamento.codigo_unidade = oSqlDataReader.Item("codigo_unidade")

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function IndexFamiliaEquipamento(ByVal iCodigoEmpresa As Integer,
                                            ByVal iCodigoUsuario As Integer,
                                            ByVal iCodigoUnidade As Integer) As List(Of FamiliaEquipamento)

        Try

            'Váriaveis Locais
            Dim oFamiliaEquipamento As New List(Of FamiliaEquipamento)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(2) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_familia_equipamento", oSqlParameter)

            While oSqlDataReader.Read

                Dim oFamiliaEquipamentoInfo As New FamiliaEquipamento

                oFamiliaEquipamentoInfo.descricao = oSqlDataReader.Item("descricao")
                oFamiliaEquipamentoInfo.ativo = oSqlDataReader.Item("ativo")
                oFamiliaEquipamentoInfo.codigo = oSqlDataReader.Item("codigo")
                oFamiliaEquipamentoInfo.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oFamiliaEquipamentoInfo.unidade = oSqlDataReader.Item("unidade")

                oFamiliaEquipamento.Add(oFamiliaEquipamentoInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oFamiliaEquipamento

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ValidaFamiliaEquipamento(ByVal iCodigoEmpresa As Integer,
                                             ByVal iCodigoUnidade As Integer,
                                             ByVal sDescricao As String,
                                             ByVal iCodigo As Integer) As Boolean

        Try

            'Váriaveis Locais
            Dim oSqlParameter(3) As SqlParameter
            Dim i As Integer = 0
            Dim iReturn As Integer

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 250
            oSqlParameter(i).Value = sDescricao

            'Executa Query
            iReturn = ExecuteScalar(sConnection, CommandType.StoredProcedure, "sp_validate_cadastro_basico_familia_equipamento", oSqlParameter)

            'Retorno da Função
            Return (IIf(iReturn > 0, False, True))

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: FORNECEDOR :::"

    Public Sub InsertFornecedor(ByVal iCodigoEmpresa As Integer,
                                ByVal iCodigoUsuario As Integer,
                                ByVal iCodigoUnidade As Integer,
                                ByVal sNomeFantasia As String,
                                ByVal sRazaoSocial As String,
                                ByVal sCNPJ As String,
                                ByVal sInscricaoEstadual As String,
                                ByVal sInscricaoMunicipal As String,
                                ByVal sCEP As String,
                                ByVal sUF As String,
                                ByVal sMunicipio As String,
                                ByVal sLogradouro As String,
                                ByVal sNumero As String,
                                ByVal sBairro As String,
                                ByVal sComplemento As String,
                                ByVal sTelefone As String,
                                ByVal sEmail As String,
                                ByVal iCodigoCategoria As Integer,
                                ByVal bAtivo As Boolean)

        'Variaveis Locais
        Dim oSqlParameter(18) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoUnidade = -1, DBNull.Value, iCodigoUnidade) : i += 1

            'Seta Parametros - Nome Fantasia
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "nome_fantasia"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 120
            oSqlParameter(i).Value = sNomeFantasia : i += 1

            'Seta Parametros - Razão Social
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "razao_social"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 120
            oSqlParameter(i).Value = sRazaoSocial : i += 1

            'Seta Parametros - CNPJ
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "cnpj"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sCNPJ : i += 1

            'Seta Parametros - Inscrição Estadual
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "inscricao_estadual"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sInscricaoEstadual : i += 1

            'Seta Parametros - Inscrição Municipal
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "inscricao_municipal"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sInscricaoMunicipal : i += 1

            'Seta Parametros - CEP
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "cep"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 10
            oSqlParameter(i).Value = sCEP : i += 1

            'Seta Parametros - UF
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "uf"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 2
            oSqlParameter(i).Value = IIf(sUF = "", DBNull.Value, sUF) : i += 1

            'Seta Parametros - Município
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "municipio"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 60
            oSqlParameter(i).Value = sMunicipio : i += 1

            'Seta Parametros - Logradouro
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "logradouro"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sLogradouro : i += 1

            'Seta Parametros - Número
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "numero"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 60
            oSqlParameter(i).Value = sNumero : i += 1

            'Seta Parametros - Bairro
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "bairro"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sBairro : i += 1

            'Seta Parametros - Complemento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "complemento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sComplemento : i += 1

            'Seta Parametros - Telefone
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "telefone"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sTelefone : i += 1

            'Seta Parametros - Email
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "email"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sEmail : i += 1

            'Seta Parametros - Código Categoria
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_categoria"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoCategoria = -1, DBNull.Value, iCodigoCategoria) : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_cadastro_basico_fornecedor", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateFornecedor(ByVal iCodigoEmpresa As Integer,
                                ByVal iCodigoUsuario As Integer,
                                ByVal iCodigoUnidade As Integer,
                                ByVal sNomeFantasia As String,
                                ByVal sRazaoSocial As String,
                                ByVal sCNPJ As String,
                                ByVal sInscricaoEstadual As String,
                                ByVal sInscricaoMunicipal As String,
                                ByVal sCEP As String,
                                ByVal sUF As String,
                                ByVal sMunicipio As String,
                                ByVal sLogradouro As String,
                                ByVal sNumero As String,
                                ByVal sBairro As String,
                                ByVal sComplemento As String,
                                ByVal sTelefone As String,
                                ByVal sEmail As String,
                                ByVal iCodigoCategoria As Integer,
                                ByVal bAtivo As Boolean,
                                ByVal iCodigo As Integer)

        'Variaveis Locais
        Dim oSqlParameter(19) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo : i += 1

            'Seta Parametros - Nome Fantasia
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "nome_fantasia"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 120
            oSqlParameter(i).Value = sNomeFantasia : i += 1

            'Seta Parametros - Razão Social
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "razao_social"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 120
            oSqlParameter(i).Value = sRazaoSocial : i += 1

            'Seta Parametros - CNPJ
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "cnpj"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sCNPJ : i += 1

            'Seta Parametros - Inscrição Estadual
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "inscricao_estadual"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sInscricaoEstadual : i += 1

            'Seta Parametros - Inscrição Municipal
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "inscricao_municipal"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sInscricaoMunicipal : i += 1

            'Seta Parametros - CEP
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "cep"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 10
            oSqlParameter(i).Value = sCEP : i += 1

            'Seta Parametros - UF
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "uf"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 2
            oSqlParameter(i).Value = IIf(sUF = "", DBNull.Value, sUF) : i += 1

            'Seta Parametros - Município
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "municipio"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 60
            oSqlParameter(i).Value = sMunicipio : i += 1

            'Seta Parametros - Logradouro
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "logradouro"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sLogradouro : i += 1

            'Seta Parametros - Número
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "numero"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 60
            oSqlParameter(i).Value = sNumero : i += 1

            'Seta Parametros - Bairro
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "bairro"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sBairro : i += 1

            'Seta Parametros - Complemento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "complemento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sComplemento : i += 1

            'Seta Parametros - Telefone
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "telefone"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sTelefone : i += 1

            'Seta Parametros - Email
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "email"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sEmail : i += 1

            'Seta Parametros - Código Categoria
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_categoria"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoCategoria = -1, DBNull.Value, iCodigoCategoria) : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_cadastro_basico_fornecedor", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub DeleteFornecedor(ByVal iCodigoEmpresa As Integer,
                             ByVal iCodigoUsuario As Integer,
                             ByVal iCodigo As Integer)

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

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_cadastro_basico_fornecedor", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InfoFornecedor(ByVal iCodigoEmpresa As Integer,
                              ByVal iCodigo As Integer,
                              ByRef oFornecedor As Fornecedor)

        'Variaveis Locais
        Dim oSqlParameter(1) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_fornecedor_dados", oSqlParameter)

            While oSqlDataReader.Read

                oFornecedor = New Fornecedor
                oFornecedor.codigo = oSqlDataReader.Item("codigo")
                oFornecedor.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oFornecedor.unidade = oSqlDataReader.Item("unidade")
                oFornecedor.nome_fantasia = oSqlDataReader.Item("nome_fantasia")
                oFornecedor.razao_social = oSqlDataReader.Item("razao_social")
                oFornecedor.cnpj = oSqlDataReader.Item("cnpj")
                oFornecedor.inscricao_estadual = oSqlDataReader.Item("inscricao_estadual")
                oFornecedor.inscricao_municipal = oSqlDataReader.Item("inscricao_municipal")
                oFornecedor.cep = oSqlDataReader.Item("cep")
                oFornecedor.uf = oSqlDataReader.Item("uf")
                oFornecedor.municipio = oSqlDataReader.Item("municipio")
                oFornecedor.logradouro = oSqlDataReader.Item("logradouro")
                oFornecedor.numero = oSqlDataReader.Item("numero")
                oFornecedor.bairro = oSqlDataReader.Item("bairro")
                oFornecedor.complemento = oSqlDataReader.Item("complemento")
                oFornecedor.telefone = oSqlDataReader.Item("telefone")
                oFornecedor.email = oSqlDataReader.Item("email")
                oFornecedor.codigo_categoria = oSqlDataReader.Item("codigo_categoria")
                oFornecedor.categoria = oSqlDataReader.Item("categoria")
                oFornecedor.ativo = oSqlDataReader.Item("ativo")

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function IndexFornecedor(ByVal iCodigoEmpresa As Integer,
                                    ByVal iCodigoUsuario As Integer,
                                    ByVal iCodigoUnidade As Integer,
                                    Optional ByVal sCNPJ As String = "",
                                    Optional ByVal sNomeFantasia As String = "",
                                    Optional ByVal iCodigoCategoria As Integer = -1,
                                    Optional ByVal sUF As String = "",
                                    Optional ByVal iAtivo As Integer = -1) As List(Of Fornecedor)

        Try

            'Váriaveis Locais
            Dim oFornecedor As New List(Of Fornecedor)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(7) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - CNPJ
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "cnpj"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sCNPJ : i += 1

            'Seta Parametros - Nome Fantasia
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "nome_fantasia"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 150
            oSqlParameter(i).Value = sNomeFantasia : i += 1

            'Seta Parametros - Código Categoria
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_categoria"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoCategoria : i += 1

            'Seta Parametros - UF
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "uf"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 2
            oSqlParameter(i).Value = sUF : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = IIf(iAtivo = -1, DBNull.Value, IIf(iAtivo = 1, True, False))

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_fornecedor", oSqlParameter)

            While oSqlDataReader.Read

                Dim oFornecedorInfo As New Fornecedor

                oFornecedorInfo.cnpj = oSqlDataReader.Item("cnpj")
                oFornecedorInfo.unidade = oSqlDataReader.Item("unidade")
                oFornecedorInfo.nome_fantasia = oSqlDataReader.Item("nome_fantasia")
                oFornecedorInfo.razao_social = oSqlDataReader.Item("razao_social")
                oFornecedorInfo.uf = oSqlDataReader.Item("uf")
                oFornecedorInfo.municipio = oSqlDataReader.Item("municipio")
                oFornecedorInfo.telefone = oSqlDataReader.Item("telefone")
                oFornecedorInfo.email = oSqlDataReader.Item("email")
                oFornecedorInfo.categoria = oSqlDataReader.Item("categoria")
                oFornecedorInfo.ativo = oSqlDataReader.Item("ativo")
                oFornecedorInfo.codigo = oSqlDataReader.Item("codigo")

                oFornecedor.Add(oFornecedorInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oFornecedor

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ValidaFornecedor(ByVal iCodigoEmpresa As Integer,
                                     ByVal iCodigoUnidade As Integer,
                                     ByVal iCodigo As Integer,
                                     ByVal sCNPJ As String) As Boolean

        Try

            'Váriaveis Locais
            Dim oSqlParameter(3) As SqlParameter
            Dim i As Integer = 0
            Dim iReturn As Integer

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo : i += 1

            'Seta Parametros - CNPJ
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "cnpj"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sCNPJ

            'Executa Query
            iReturn = ExecuteScalar(sConnection, CommandType.StoredProcedure, "sp_validate_cadastro_basico_fornecedor", oSqlParameter)

            'Retorno da Função
            Return (IIf(iReturn > 0, False, True))

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: FUNÇÃO :::"

    Public Sub InsertFuncao(ByVal iCodigoEmpresa As Integer,
                            ByVal iCodigoUsuario As Integer,
                            ByVal iCodigoUnidade As Integer,
                            ByVal sDescricao As String,
                            ByVal bAtivo As Boolean)

        'Variaveis Locais
        Dim oSqlParameter(4) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sDescricao : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_cadastro_basico_funcao", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateFuncao(ByVal iCodigoEmpresa As Integer,
                            ByVal iCodigoUsuario As Integer,
                            ByVal iCodigoUnidade As Integer,
                            ByVal sDescricao As String,
                            ByVal bAtivo As Boolean,
                            ByVal iCodigo As Integer)

        'Variaveis Locais
        Dim oSqlParameter(5) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sDescricao : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_cadastro_basico_funcao", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub DeleteFuncao(ByVal iCodigoEmpresa As Integer,
                            ByVal iCodigoUsuario As Integer,
                            ByVal iCodigo As Integer)

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

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_cadastro_basico_funcao", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InfoFuncao(ByVal iCodigoEmpresa As Integer,
                          ByVal iCodigo As Integer,
                          ByRef oFuncao As Funcao)

        'Variaveis Locais
        Dim oSqlParameter(1) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_funcao_dados", oSqlParameter)

            While oSqlDataReader.Read

                oFuncao = New Funcao
                oFuncao.codigo = oSqlDataReader.Item("codigo")
                oFuncao.descricao = oSqlDataReader.Item("descricao")
                oFuncao.ativo = oSqlDataReader.Item("ativo")
                oFuncao.unidade = oSqlDataReader.Item("unidade")
                oFuncao.codigo_unidade = oSqlDataReader.Item("codigo_unidade")

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function IndexFuncao(ByVal iCodigoEmpresa As Integer,
                                ByVal iCodigoUsuario As Integer,
                                ByVal iCodigoUnidade As Integer) As List(Of Funcao)

        Try

            'Váriaveis Locais
            Dim oFuncao As New List(Of Funcao)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(2) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_funcao", oSqlParameter)

            While oSqlDataReader.Read

                Dim oFuncaoInfo As New Funcao

                oFuncaoInfo.descricao = oSqlDataReader.Item("descricao")
                oFuncaoInfo.ativo = oSqlDataReader.Item("ativo")
                oFuncaoInfo.codigo = oSqlDataReader.Item("codigo")
                oFuncaoInfo.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oFuncaoInfo.unidade = oSqlDataReader.Item("unidade")

                oFuncao.Add(oFuncaoInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oFuncao

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ValidaFuncao(ByVal iCodigoEmpresa As Integer,
                                 ByVal iCodigoUnidade As Integer,
                                 ByVal sDescricao As String,
                                 ByVal iCodigo As Integer) As Boolean

        Try

            'Váriaveis Locais
            Dim oSqlParameter(3) As SqlParameter
            Dim i As Integer = 0
            Dim iReturn As Integer

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sDescricao

            'Executa Query
            iReturn = ExecuteScalar(sConnection, CommandType.StoredProcedure, "sp_validate_cadastro_basico_funcao", oSqlParameter)

            'Retorno da Função
            Return (IIf(iReturn > 0, False, True))

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: FUNCIONÁRIO :::"

    Public Sub InsertFuncionario(ByVal iCodigoEmpresa As Integer,
                                 ByVal iCodigoUsuario As Integer,
                                 ByVal iCodigoUnidade As Integer,
                                 ByVal sNome As String,
                                 ByVal sCPF As String,
                                 ByVal iCodigoFuncao As Integer,
                                 ByVal iCodigoModulo As Integer,
                                 ByVal sTelefone As String,
                                 ByVal iCodigoUsuarioVinculado As Integer,
                                 ByVal iCodigoTipoFuncionario As Integer,
                                 ByVal dValorHora As Double,
                                 ByVal bAtivo As Boolean,
                                 ByVal bContabilizaHora As Boolean)

        'Variaveis Locais
        Dim oSqlParameter(12) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Nome
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "nome"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 120
            oSqlParameter(i).Value = sNome : i += 1

            'Seta Parametros - CPF
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "cpf"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sCPF : i += 1

            'Seta Parametros - Código Função
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_funcao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoFuncao = -1, DBNull.Value, iCodigoFuncao) : i += 1

            'Seta Parametros - Código Módulo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_modulo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoModulo : i += 1

            'Seta Parametros - Telefone
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "telefone"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sTelefone : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario_vinculado"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoUsuarioVinculado = -1, DBNull.Value, iCodigoUsuarioVinculado) : i += 1

            'Seta Parametros - Código Tipo Funcionário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_tipo_funcionario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoTipoFuncionario : i += 1

            'Seta Parametros - Valor Hora
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "valor_hora"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = dValorHora : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo : i += 1

            'Seta Parametros - Contabiliza Hora
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "contabiliza_hora"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bContabilizaHora

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_cadastro_basico_funcionario", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateFuncionario(ByVal iCodigoEmpresa As Integer,
                                 ByVal iCodigoUsuario As Integer,
                                 ByVal iCodigoUnidade As Integer,
                                 ByVal sNome As String,
                                 ByVal sCPF As String,
                                 ByVal iCodigoFuncao As Integer,
                                 ByVal iCodigoModulo As Integer,
                                 ByVal sTelefone As String,
                                 ByVal bAtivo As Boolean,
                                 ByVal iCodigoUsuarioVinculado As Integer,
                                 ByVal iCodigoTipoFuncionario As Integer,
                                 ByVal dValorHora As Double,
                                 ByVal bContabilizaHora As Boolean,
                                 ByVal iCodigo As Integer)

        'Variaveis Locais
        Dim oSqlParameter(13) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Nome
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "nome"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 120
            oSqlParameter(i).Value = sNome : i += 1

            'Seta Parametros - CPF
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "cpf"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sCPF : i += 1

            'Seta Parametros - Código Função
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_funcao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoFuncao = -1, DBNull.Value, iCodigoFuncao) : i += 1

            'Seta Parametros - Código Módulo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_modulo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoModulo : i += 1

            'Seta Parametros - Telefone
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "telefone"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sTelefone : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario_vinculado"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoUsuarioVinculado = -1, DBNull.Value, iCodigoUsuarioVinculado) : i += 1

            'Seta Parametros - Código Tipo Funcionário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_tipo_funcionario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoTipoFuncionario : i += 1

            'Seta Parametros - Valor Hora
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "valor_hora"
            oSqlParameter(i).Direction = ParameterDirection.Input

            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = dValorHora : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo : i += 1

            'Seta Parametros - Contabiliza Hora
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "contabiliza_hora"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bContabilizaHora : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_cadastro_basico_funcionario", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub DeleteFuncionario(ByVal iCodigoEmpresa As Integer,
                                 ByVal iCodigoUsuario As Integer,
                                 ByVal iCodigo As Integer)

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

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_cadastro_basico_funcionario", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InfoFuncionario(ByVal iCodigoEmpresa As Integer,
                               ByVal iCodigo As Integer,
                               ByRef oFuncionario As Funcionario)

        'Variaveis Locais
        Dim oSqlParameter(1) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_funcionario_dados", oSqlParameter)

            While oSqlDataReader.Read

                oFuncionario = New Funcionario
                oFuncionario.codigo = oSqlDataReader.Item("codigo")
                oFuncionario.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oFuncionario.unidade = oSqlDataReader.Item("unidade")
                oFuncionario.nome = oSqlDataReader.Item("nome")
                oFuncionario.cpf = oSqlDataReader.Item("cpf")
                oFuncionario.funcao = oSqlDataReader.Item("funcao")
                oFuncionario.codigo_funcao = oSqlDataReader.Item("codigo_funcao")
                oFuncionario.modulo = oSqlDataReader.Item("modulo")
                oFuncionario.codigo_modulo = oSqlDataReader.Item("codigo_modulo")
                oFuncionario.codigo_usuario = oSqlDataReader.Item("codigo_usuario")
                oFuncionario.telefone = oSqlDataReader.Item("telefone")
                oFuncionario.codigo_tipo_funcionario = oSqlDataReader.Item("codigo_tipo_funcionario")
                oFuncionario.valor_hora = oSqlDataReader.Item("valor_hora")
                oFuncionario.ativo = oSqlDataReader.Item("ativo")
                oFuncionario.contabiliza_hora = oSqlDataReader.Item("contabiliza_hora")

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function IndexFuncionario(ByVal iCodigoEmpresa As Integer,
                                     ByVal iCodigoUsuario As Integer,
                                     ByVal iCodigoUnidade As Integer,
                                     ByVal iCodigoModulo As Integer,
                                     ByVal sNome As String,
                                     ByVal iCodigoFuncao As Integer,
                                     ByVal iCodigoTipoFuncionario As Integer,
                                     ByVal iAtivo As Integer) As List(Of Funcionario)

        Try

            'Váriaveis Locais
            Dim oFuncionario As New List(Of Funcionario)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(7) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Módulo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_modulo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoModulo : i += 1

            'Seta Parametros - Nome
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "nome"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 250
            oSqlParameter(i).Value = sNome : i += 1

            'Seta Parametros - Código Função
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_funcao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoFuncao : i += 1

            'Seta Parametros - Código Tipo de Funcionário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_tipo_funcionario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoTipoFuncionario : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = IIf(iAtivo = -1, DBNull.Value, IIf(iAtivo = 1, True, False))

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_funcionario", oSqlParameter)

            While oSqlDataReader.Read

                Dim oFuncionarioInfo As New Funcionario

                oFuncionarioInfo.cpf = oSqlDataReader.Item("cpf")
                oFuncionarioInfo.unidade = oSqlDataReader.Item("unidade")
                oFuncionarioInfo.nome = oSqlDataReader.Item("nome")
                oFuncionarioInfo.funcao = oSqlDataReader.Item("funcao")
                oFuncionarioInfo.modulo = oSqlDataReader.Item("modulo")
                oFuncionarioInfo.ativo = oSqlDataReader.Item("ativo")
                oFuncionarioInfo.tipo_funcionario = oSqlDataReader.Item("tipo_funcionario")
                oFuncionarioInfo.codigo = oSqlDataReader.Item("codigo")
                oFuncionarioInfo.texto_ativo = IIf(oSqlDataReader.Item("ativo"), "SIM", "NÃO")

                oFuncionario.Add(oFuncionarioInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oFuncionario

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ValidaFuncionario(ByVal iCodigoEmpresa As Integer,
                                      ByVal iCodigoUnidade As Integer,
                                      ByVal iCodigo As Integer,
                                      ByVal sCPF As String) As Boolean

        Try

            'Váriaveis Locais
            Dim oSqlParameter(3) As SqlParameter
            Dim i As Integer = 0
            Dim iReturn As Integer

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo : i += 1

            'Seta Parametros - CPF
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "cpf"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sCPF

            'Executa Query
            iReturn = ExecuteScalar(sConnection, CommandType.StoredProcedure, "sp_validate_cadastro_basico_funcionario", oSqlParameter)

            'Retorno da Função
            Return (IIf(iReturn > 0, False, True))

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: GRUPO - CHECKLIST :::"

    Public Sub InsertGrupoChecklist(ByVal iCodigoEmpresa As Integer,
                                    ByVal iCodigoUsuario As Integer,
                                    ByVal iCodigoUnidade As Integer,
                                    ByVal sCodigoGrupo As String,
                                    ByVal sDescricao As String,
                                    ByVal bAtivo As Boolean)

        'Variaveis Locais
        Dim oSqlParameter(5) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Grupo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_grupo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sCodigoGrupo : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sDescricao : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_cadastro_basico_grupo_checklist", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateGrupoChecklist(ByVal iCodigoEmpresa As Integer,
                                    ByVal iCodigoUsuario As Integer,
                                    ByVal iCodigoUnidade As Integer,
                                    ByVal sCodigoGrupo As String,
                                    ByVal sDescricao As String,
                                    ByVal bAtivo As Boolean,
                                    ByVal iCodigo As Integer)

        'Variaveis Locais
        Dim oSqlParameter(6) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Grupo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_grupo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sCodigoGrupo : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sDescricao : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_cadastro_basico_grupo_checklist", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub DeleteGrupoChecklist(ByVal iCodigoEmpresa As Integer,
                                    ByVal iCodigoUsuario As Integer,
                                    ByVal iCodigo As Integer)

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

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_cadastro_basico_grupo_checklist", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InfoGrupoChecklist(ByVal iCodigoEmpresa As Integer,
                                  ByVal iCodigo As Integer,
                                  ByRef oGrupoChecklist As GrupoChecklist)

        'Variaveis Locais
        Dim oSqlParameter(1) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_grupo_checklist_dados", oSqlParameter)

            While oSqlDataReader.Read

                oGrupoChecklist = New GrupoChecklist
                oGrupoChecklist.codigo = oSqlDataReader.Item("codigo")
                oGrupoChecklist.codigo_grupo = oSqlDataReader.Item("codigo_grupo")
                oGrupoChecklist.descricao = oSqlDataReader.Item("descricao")
                oGrupoChecklist.ativo = oSqlDataReader.Item("ativo")
                oGrupoChecklist.unidade = oSqlDataReader.Item("unidade")
                oGrupoChecklist.codigo_unidade = oSqlDataReader.Item("codigo_unidade")

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function IndexGrupoChecklist(ByVal iCodigoEmpresa As Integer,
                                        ByVal iCodigoUsuario As Integer,
                                        ByVal iCodigoUnidade As Integer) As List(Of GrupoChecklist)

        Try

            'Váriaveis Locais
            Dim oGrupoChecklist As New List(Of GrupoChecklist)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(2) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_grupo_checklist", oSqlParameter)

            While oSqlDataReader.Read

                Dim oGrupoChecklistInfo As New GrupoChecklist

                oGrupoChecklistInfo.codigo_grupo = oSqlDataReader.Item("codigo_grupo")
                oGrupoChecklistInfo.descricao = oSqlDataReader.Item("descricao")
                oGrupoChecklistInfo.ativo = oSqlDataReader.Item("ativo")
                oGrupoChecklistInfo.codigo = oSqlDataReader.Item("codigo")
                oGrupoChecklistInfo.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oGrupoChecklistInfo.unidade = oSqlDataReader.Item("unidade")

                oGrupoChecklist.Add(oGrupoChecklistInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oGrupoChecklist

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ValidaGrupoChecklist(ByVal iCodigoEmpresa As Integer,
                                         ByVal iCodigoUnidade As Integer,
                                         ByVal sCodigoGrupo As String,
                                         ByVal iCodigo As Integer) As Boolean

        Try

            'Váriaveis Locais
            Dim oSqlParameter(3) As SqlParameter
            Dim i As Integer = 0
            Dim iReturn As Integer

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo : i += 1

            'Seta Parametros - Código Grupo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_grupo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sCodigoGrupo

            'Executa Query
            iReturn = ExecuteScalar(sConnection, CommandType.StoredProcedure, "sp_validate_cadastro_basico_grupo_checklist", oSqlParameter)

            'Retorno da Função
            Return (IIf(iReturn > 0, False, True))

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: GRUPO - ITEM - MEDIÇÃO :::"

    Public Sub InsertGrupoItemMedicao(ByVal iCodigoEmpresa As Integer,
                                      ByVal iCodigoUsuario As Integer,
                                      ByVal iCodigoUnidade As Integer,
                                      ByVal sDescricao As String,
                                      ByVal bAtivo As Boolean)

        'Variaveis Locais
        Dim oSqlParameter(4) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sDescricao : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_cadastro_basico_grupo_item_medicao", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateGrupoItemMedicao(ByVal iCodigoEmpresa As Integer,
                                      ByVal iCodigoUsuario As Integer,
                                      ByVal iCodigoUnidade As Integer,
                                      ByVal sDescricao As String,
                                      ByVal bAtivo As Boolean,
                                      ByVal iCodigo As Integer)

        'Variaveis Locais
        Dim oSqlParameter(5) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sDescricao : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_cadastro_basico_grupo_item_medicao", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub DeleteGrupoItemMedicao(ByVal iCodigoEmpresa As Integer,
                                      ByVal iCodigoUsuario As Integer,
                                      ByVal iCodigo As Integer)

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

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_cadastro_basico_grupo_item_medicao", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InfoGrupoItemMedicao(ByVal iCodigoEmpresa As Integer,
                                    ByVal iCodigo As Integer,
                                    ByRef oGrupoItemMedicao As GrupoItemMedicao)

        'Variaveis Locais
        Dim oSqlParameter(1) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_grupo_item_medicao_dados", oSqlParameter)

            While oSqlDataReader.Read

                oGrupoItemMedicao = New GrupoItemMedicao
                oGrupoItemMedicao.codigo = oSqlDataReader.Item("codigo")
                oGrupoItemMedicao.descricao = oSqlDataReader.Item("descricao")
                oGrupoItemMedicao.ativo = oSqlDataReader.Item("ativo")
                oGrupoItemMedicao.unidade = oSqlDataReader.Item("unidade")
                oGrupoItemMedicao.codigo_unidade = oSqlDataReader.Item("codigo_unidade")

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function IndexGrupoItemMedicao(ByVal iCodigoEmpresa As Integer,
                                          ByVal iCodigoUsuario As Integer,
                                          ByVal iCodigoUnidade As Integer) As List(Of GrupoItemMedicao)

        Try

            'Váriaveis Locais
            Dim oGrupoItemMedicao As New List(Of GrupoItemMedicao)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(2) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_grupo_item_medicao", oSqlParameter)

            While oSqlDataReader.Read

                Dim oGrupoItemMedicaoInfo As New GrupoItemMedicao

                oGrupoItemMedicaoInfo.descricao = oSqlDataReader.Item("descricao")
                oGrupoItemMedicaoInfo.ativo = oSqlDataReader.Item("ativo")
                oGrupoItemMedicaoInfo.codigo = oSqlDataReader.Item("codigo")
                oGrupoItemMedicaoInfo.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oGrupoItemMedicaoInfo.unidade = oSqlDataReader.Item("unidade")

                oGrupoItemMedicao.Add(oGrupoItemMedicaoInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oGrupoItemMedicao

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ValidaGrupoItemMedicao(ByVal iCodigoEmpresa As Integer,
                                           ByVal iCodigoUnidade As Integer,
                                           ByVal sDescricao As String,
                                           ByVal iCodigo As Integer) As Boolean

        Try

            'Váriaveis Locais
            Dim oSqlParameter(3) As SqlParameter
            Dim i As Integer = 0
            Dim iReturn As Integer

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sDescricao

            'Executa Query
            iReturn = ExecuteScalar(sConnection, CommandType.StoredProcedure, "sp_validate_cadastro_basico_grupo_item_medicao", oSqlParameter)

            'Retorno da Função
            Return (IIf(iReturn > 0, False, True))

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: GRUPO - PRODUTO :::"

    Public Sub InsertGrupoProduto(ByVal codigoEmpresa As Integer,
                                  ByVal codigoUsuario As Integer,
                                  ByVal codigoUnidade As Integer,
                                  ByVal descricao As String,
                                  ByVal ativo As Boolean)

        'Variaveis Locais
        Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_usuario", SqlDbType.Int, codigoUsuario),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("descricao", SqlDbType.VarChar, descricao),
                CriarParametro("ativo", SqlDbType.Bit, ativo)
            }

        Try

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_cadastro_basico_grupo_produto", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateGrupoProduto(ByVal codigoEmpresa As Integer,
                                  ByVal codigoUsuario As Integer,
                                  ByVal codigoUnidade As Integer,
                                  ByVal descricao As String,
                                  ByVal ativo As Boolean,
                                  ByVal codigo As Integer)

        'Variaveis Locais
        Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_usuario", SqlDbType.Int, codigoUsuario),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("descricao", SqlDbType.VarChar, descricao),
                CriarParametro("ativo", SqlDbType.Bit, ativo),
                CriarParametro("codigo", SqlDbType.Int, codigo)
            }

        Try

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_cadastro_basico_grupo_produto", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub DeleteGrupoProduto(ByVal codigoEmpresa As Integer,
                                  ByVal codigoUsuario As Integer,
                                  ByVal codigo As Integer)

        'Variaveis Locais
        Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_usuario", SqlDbType.Int, codigoUsuario),
                CriarParametro("codigo", SqlDbType.Int, codigo)
            }

        Try

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_cadastro_basico_grupo_produto", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function InfoGrupoProduto(ByVal codigoEmpresa As Integer,
                                     ByVal codigo As Integer) As GrupoProduto

        'Variaveis Locais
        Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo", SqlDbType.Int, codigo)
            }
        Dim oReturn As New GrupoProduto

        Try

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_grupo_produto_dados", oSqlParameter)

                While oSqlDataReader.Read

                    oReturn = New GrupoProduto
                    oReturn.codigo = oSqlDataReader.Item("codigo")
                    oReturn.descricao = oSqlDataReader.Item("descricao")
                    oReturn.ativo = oSqlDataReader.Item("ativo")
                    oReturn.unidade = oSqlDataReader.Item("unidade")
                    oReturn.codigoUnidade = oSqlDataReader.Item("codigo_unidade")

                End While

            End Using

            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function IndexGrupoProduto(ByVal codigoEmpresa As Integer,
                                      ByVal codigoUnidade As Integer,
                                      ByVal descricao As String) As List(Of GrupoProduto)

        Try

            'Váriavies Locais
            Dim oReturn As New List(Of GrupoProduto)
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("descricao", SqlDbType.VarChar, descricao)
            }

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_grupo_produto", oSqlParameter)

                While oSqlDataReader.Read

                    Dim oGrupoProdutoInfo As New GrupoProduto

                    oGrupoProdutoInfo.descricao = oSqlDataReader.Item("descricao")
                    oGrupoProdutoInfo.ativo = oSqlDataReader.Item("ativo")
                    oGrupoProdutoInfo.codigo = oSqlDataReader.Item("codigo")
                    oGrupoProdutoInfo.codigoUnidade = oSqlDataReader.Item("codigo_unidade")
                    oGrupoProdutoInfo.unidade = oSqlDataReader.Item("unidade")

                    oReturn.Add(oGrupoProdutoInfo)

                End While

            End Using

            'Retorno da Função
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ValidaGrupoProduto(ByVal codigoEmpresa As Integer,
                                       ByVal codigoUnidade As Integer,
                                       ByVal descricao As String,
                                       ByVal codigo As Integer) As Boolean

        Try

            'Váriavies Locais
            Dim iReturn As Integer
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("descricao", SqlDbType.VarChar, descricao),
                CriarParametro("codigo", SqlDbType.Int, codigo)
            }

            'Executa Query
            iReturn = ExecuteScalar(sConnection, CommandType.StoredProcedure, "sp_validate_cadastro_basico_grupo_produto", oSqlParameter)

            'Retorno da Função
            Return (IIf(iReturn > 0, False, True))

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: ITEM - MEDIÇÂO :::"

    Public Sub InsertItemMedicao(ByVal iCodigoEmpresa As Integer,
                                 ByVal iCodigoUsuario As Integer,
                                 ByVal iCodigoUnidade As Integer,
                                 ByVal iCodigoGrupoItemMedicao As Integer,
                                 ByVal sDescricao As String,
                                 ByVal sMetaConsumo As String,
                                 ByVal iCodigoFormaLeitura As Integer,
                                 ByVal iNumeroDigitos As Integer,
                                 ByVal iNumeroCasasDecimais As Integer,
                                 ByVal sUnidadeMedida As String,
                                 ByVal bAtivo As Boolean)

        'Variaveis Locais
        Dim oSqlParameter(10) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Grupo - Item de Medição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_grupo_item_medicao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoGrupoItemMedicao = -1, DBNull.Value, iCodigoGrupoItemMedicao) : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sDescricao : i += 1

            'Seta Parametros - Meta de Consumo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "meta_consumo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = CDbl(sMetaConsumo.Replace(".", ",")) : i += 1

            'Seta Parametros - Código Forma Leitura
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_forma_leitura"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoFormaLeitura : i += 1

            'Seta Parametros - Nº Digitos
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "numero_digitos"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iNumeroDigitos : i += 1

            'Seta Parametros - Nº Casas Decimais
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "numero_casas_decimais"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iNumeroCasasDecimais : i += 1

            'Seta Parametros - Unidade de Medida
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "unidade_medida"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 10
            oSqlParameter(i).Value = sUnidadeMedida : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_cadastro_basico_item_medicao", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateItemMedicao(ByVal iCodigoEmpresa As Integer,
                                 ByVal iCodigoUsuario As Integer,
                                 ByVal iCodigoUnidade As Integer,
                                 ByVal iCodigoGrupoItemMedicao As Integer,
                                 ByVal sDescricao As String,
                                 ByVal sMetaConsumo As String,
                                 ByVal iCodigoFormaLeitura As Integer,
                                 ByVal iNumeroDigitos As Integer,
                                 ByVal iNumeroCasasDecimais As Integer,
                                 ByVal bAtivo As Boolean,
                                 ByVal sUnidadeMedida As String,
                                 ByVal iCodigo As Integer)

        'Variaveis Locais
        Dim oSqlParameter(11) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Grupo - Item de Medição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_grupo_item_medicao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoGrupoItemMedicao = -1, DBNull.Value, iCodigoGrupoItemMedicao) : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sDescricao : i += 1

            'Seta Parametros - Meta de Consumo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "meta_consumo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = CDbl(sMetaConsumo.Replace(".", ",")) : i += 1

            'Seta Parametros - Código Forma Leitura
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_forma_leitura"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoFormaLeitura : i += 1

            'Seta Parametros - Nº Digitos
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "numero_digitos"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iNumeroDigitos : i += 1

            'Seta Parametros - Nº Casas Decimais
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "numero_casas_decimais"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iNumeroCasasDecimais : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo : i += 1

            'Seta Parametros - Unidade de Medida
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "unidade_medida"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 10
            oSqlParameter(i).Value = sUnidadeMedida : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_cadastro_basico_item_medicao", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub DeleteItemMedicao(ByVal iCodigoEmpresa As Integer,
                                 ByVal iCodigoUsuario As Integer,
                                 ByVal iCodigo As Integer)

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

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_cadastro_basico_item_medicao", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InfoItemMedicao(ByVal iCodigoEmpresa As Integer,
                               ByVal iCodigo As Integer,
                               ByRef oItemMedicao As ItemMedicao)

        'Variaveis Locais
        Dim oSqlParameter(1) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_item_medicao_dados", oSqlParameter)

            While oSqlDataReader.Read

                oItemMedicao = New ItemMedicao
                oItemMedicao.codigo = oSqlDataReader.Item("codigo")
                oItemMedicao.descricao = oSqlDataReader.Item("descricao")
                oItemMedicao.ativo = oSqlDataReader.Item("ativo")
                oItemMedicao.unidade = oSqlDataReader.Item("unidade")
                oItemMedicao.codigo_grupo_item_medicao = oSqlDataReader.Item("codigo_grupo_item_medicao")
                oItemMedicao.grupo_item_medicao = oSqlDataReader.Item("grupo_item_medicao")
                oItemMedicao.meta_consumo = oSqlDataReader.Item("meta_consumo")
                oItemMedicao.forma_leitura = oSqlDataReader.Item("forma_leitura")
                oItemMedicao.codigo_forma_leitura = oSqlDataReader.Item("codigo_forma_leitura")
                oItemMedicao.numero_digitos = oSqlDataReader.Item("numero_digitos")
                oItemMedicao.numero_casas_decimais = oSqlDataReader.Item("numero_casas_decimais")
                oItemMedicao.unidade_medida = oSqlDataReader.Item("unidade_medida")
                oItemMedicao.codigo_unidade = oSqlDataReader.Item("codigo_unidade")

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function IndexItemMedicao(ByVal iCodigoEmpresa As Integer,
                                     ByVal iCodigoUsuario As Integer,
                                     ByVal iCodigoUnidade As Integer) As List(Of ItemMedicao)

        Try

            'Váriaveis Locais
            Dim oItemMedicao As New List(Of ItemMedicao)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(2) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_item_medicao", oSqlParameter)

            While oSqlDataReader.Read

                Dim oItemMedicaoInfo As New ItemMedicao

                oItemMedicaoInfo.descricao = oSqlDataReader.Item("descricao")
                oItemMedicaoInfo.ativo = oSqlDataReader.Item("ativo")
                oItemMedicaoInfo.codigo = oSqlDataReader.Item("codigo")
                oItemMedicaoInfo.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oItemMedicaoInfo.grupo_item_medicao = oSqlDataReader.Item("grupo_item_medicao")
                oItemMedicaoInfo.meta_consumo = oSqlDataReader.Item("meta_consumo")
                oItemMedicaoInfo.forma_leitura = oSqlDataReader.Item("forma_leitura")
                oItemMedicaoInfo.numero_digitos = oSqlDataReader.Item("numero_digitos")
                oItemMedicaoInfo.numero_casas_decimais = oSqlDataReader.Item("numero_casas_decimais")
                oItemMedicaoInfo.unidade_medida = oSqlDataReader.Item("unidade_medida")
                oItemMedicaoInfo.unidade = oSqlDataReader.Item("unidade")

                oItemMedicao.Add(oItemMedicaoInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oItemMedicao

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ValidaItemMedicao(ByVal iCodigoEmpresa As Integer,
                                      ByVal iCodigoUnidade As Integer,
                                      ByVal sDescricao As String,
                                      ByVal iCodigo As Integer) As Boolean

        Try

            'Váriaveis Locais
            Dim oSqlParameter(3) As SqlParameter
            Dim i As Integer = 0
            Dim iReturn As Integer

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sDescricao

            'Executa Query
            iReturn = ExecuteScalar(sConnection, CommandType.StoredProcedure, "sp_validate_cadastro_basico_item_medicao", oSqlParameter)

            'Retorno da Função
            Return (IIf(iReturn > 0, False, True))

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: ITENS GERAIS :::"

    Public Sub InsertItensGerais(ByVal iCodigoEmpresa As Integer,
                                 ByVal iCodigoUsuario As Integer,
                                 ByVal iCodigoUnidade As Integer,
                                 ByVal sDescricao As String,
                                 ByVal bAtivo As Boolean)

        'Variaveis Locais
        Dim oSqlParameter(4) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sDescricao : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_cadastro_basico_itens_gerais", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateItensGerais(ByVal iCodigoEmpresa As Integer,
                                 ByVal iCodigoUsuario As Integer,
                                 ByVal iCodigoUnidade As Integer,
                                 ByVal sDescricao As String,
                                 ByVal bAtivo As Boolean,
                                 ByVal iCodigo As Integer)

        'Variaveis Locais
        Dim oSqlParameter(5) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sDescricao : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_cadastro_basico_itens_gerais", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub DeleteItensGerais(ByVal iCodigoEmpresa As Integer,
                                 ByVal iCodigoUsuario As Integer,
                                 ByVal iCodigo As Integer)

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

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_cadastro_basico_itens_gerais", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InfoItensGerais(ByVal iCodigoEmpresa As Integer,
                               ByVal iCodigo As Integer,
                               ByRef oItensGerais As ItensGerais)

        'Variaveis Locais
        Dim oSqlParameter(1) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_itens_gerais_dados", oSqlParameter)

            While oSqlDataReader.Read

                oItensGerais = New ItensGerais
                oItensGerais.codigo = oSqlDataReader.Item("codigo")
                oItensGerais.descricao = oSqlDataReader.Item("descricao")
                oItensGerais.ativo = oSqlDataReader.Item("ativo")
                oItensGerais.unidade = oSqlDataReader.Item("unidade")
                oItensGerais.codigo_unidade = oSqlDataReader.Item("codigo_unidade")

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function IndexItensGerais(ByVal iCodigoEmpresa As Integer,
                                     ByVal iCodigoUsuario As Integer,
                                     ByVal iCodigoUnidade As Integer) As List(Of ItensGerais)

        Try

            'Váriaveis Locais
            Dim oItensGerais As New List(Of ItensGerais)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(2) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_itens_gerais", oSqlParameter)

            While oSqlDataReader.Read

                Dim oItensGeraisInfo As New ItensGerais

                oItensGeraisInfo.descricao = oSqlDataReader.Item("descricao")
                oItensGeraisInfo.ativo = oSqlDataReader.Item("ativo")
                oItensGeraisInfo.codigo = oSqlDataReader.Item("codigo")
                oItensGeraisInfo.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oItensGeraisInfo.unidade = oSqlDataReader.Item("unidade")

                oItensGerais.Add(oItensGeraisInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oItensGerais

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ValidaItensGerais(ByVal iCodigoEmpresa As Integer,
                                      ByVal iCodigoUnidade As Integer,
                                      ByVal sDescricao As String,
                                      ByVal iCodigo As Integer) As Boolean

        Try

            'Váriaveis Locais
            Dim oSqlParameter(3) As SqlParameter
            Dim i As Integer = 0
            Dim iReturn As Integer

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sDescricao

            'Executa Query
            iReturn = ExecuteScalar(sConnection, CommandType.StoredProcedure, "sp_validate_cadastro_basico_itens_gerais", oSqlParameter)

            'Retorno da Função
            Return (IIf(iReturn > 0, False, True))

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: JUSTIFICATIVA APONTAMENTO :::"

    Public Sub InsertJustificativaApontamento(ByVal iCodigoEmpresa As Integer,
                                              ByVal iCodigoUsuario As Integer,
                                              ByVal sDescricao As String,
                                              ByVal bAtivo As Boolean)

        'Variaveis Locais
        Dim oSqlParameter(3) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sDescricao : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_cadastro_basico_justificativa_apontamento", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateJustificativaApontamento(ByVal iCodigoEmpresa As Integer,
                                              ByVal iCodigoUsuario As Integer,
                                              ByVal sDescricao As String,
                                              ByVal bAtivo As Boolean,
                                              ByVal iCodigo As Integer)

        'Variaveis Locais
        Dim oSqlParameter(4) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sDescricao : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_cadastro_basico_justificativa_apontamento", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub DeleteJustificativaApontamento(ByVal iCodigoEmpresa As Integer,
                                              ByVal iCodigoUsuario As Integer,
                                              ByVal iCodigo As Integer)

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

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_cadastro_basico_justificativa_apontamento", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InfoJustificativaApontamento(ByVal iCodigoEmpresa As Integer,
                                            ByVal iCodigo As Integer,
                                            ByRef oJustificativaApontamento As JustificativaApontamento)

        'Variaveis Locais
        Dim oSqlParameter(1) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_justificativa_apontamento_dados", oSqlParameter)

            While oSqlDataReader.Read

                oJustificativaApontamento = New JustificativaApontamento
                oJustificativaApontamento.codigo = oSqlDataReader.Item("codigo")
                oJustificativaApontamento.descricao = oSqlDataReader.Item("descricao")
                oJustificativaApontamento.ativo = oSqlDataReader.Item("ativo")

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function IndexJustificativaApontamento(ByVal iCodigoEmpresa As Integer,
                                                  ByVal iCodigoUsuario As Integer) As List(Of JustificativaApontamento)

        Try

            'Váriaveis Locais
            Dim oJustificativaApontamento As New List(Of JustificativaApontamento)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(2) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_justificativa_apontamento", oSqlParameter)

            While oSqlDataReader.Read

                Dim oJustificativaApontamentoInfo As New JustificativaApontamento

                oJustificativaApontamentoInfo.descricao = oSqlDataReader.Item("descricao")
                oJustificativaApontamentoInfo.ativo = oSqlDataReader.Item("ativo")
                oJustificativaApontamentoInfo.codigo = oSqlDataReader.Item("codigo")

                oJustificativaApontamento.Add(oJustificativaApontamentoInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da JustificativaApontamento
            Return oJustificativaApontamento

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ValidaJustificativaApontamento(ByVal iCodigoEmpresa As Integer,
                                                   ByVal sDescricao As String,
                                                   ByVal iCodigo As Integer) As Boolean

        Try

            'Váriaveis Locais
            Dim oSqlParameter(2) As SqlParameter
            Dim i As Integer = 0
            Dim iReturn As Integer

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sDescricao

            'Executa Query
            iReturn = ExecuteScalar(sConnection, CommandType.StoredProcedure, "sp_validate_cadastro_basico_justificativa_apontamento", oSqlParameter)

            'Retorno da JustificativaApontamento
            Return (IIf(iReturn > 0, False, True))

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: JUSTIFICATIVA FALTA :::"

    Public Sub InsertJustificativaFalta(ByVal iCodigoEmpresa As Integer,
                                        ByVal iCodigoUsuario As Integer,
                                        ByVal iCodigoUnidade As Integer,
                                        ByVal sDescricao As String,
                                        ByVal bJustificada As Boolean,
                                        ByVal bAtivo As Boolean)

        'Variaveis Locais
        Dim oSqlParameter(5) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sDescricao : i += 1

            'Seta Parametros - Justificada
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "justificada"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bJustificada : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_cadastro_basico_justificativa_falta", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateJustificativaFalta(ByVal iCodigoEmpresa As Integer,
                                        ByVal iCodigoUsuario As Integer,
                                        ByVal iCodigoUnidade As Integer,
                                        ByVal sDescricao As String,
                                        ByVal bJustificada As Boolean,
                                        ByVal bAtivo As Boolean,
                                        ByVal iCodigo As Integer)

        'Variaveis Locais
        Dim oSqlParameter(6) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sDescricao : i += 1

            'Seta Parametros - Justificada
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "justificada"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bJustificada : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_cadastro_basico_justificativa_falta", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub DeleteJustificativaFalta(ByVal iCodigoEmpresa As Integer,
                                        ByVal iCodigoUsuario As Integer,
                                        ByVal iCodigo As Integer)

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

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_cadastro_basico_justificativa_falta", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InfoJustificativaFalta(ByVal iCodigoEmpresa As Integer,
                                      ByVal iCodigo As Integer,
                                      ByRef oJustificativaFalta As JustificativaFalta)

        'Variaveis Locais
        Dim oSqlParameter(1) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_justificativa_falta_dados", oSqlParameter)

            While oSqlDataReader.Read

                oJustificativaFalta = New JustificativaFalta
                oJustificativaFalta.codigo = oSqlDataReader.Item("codigo")
                oJustificativaFalta.descricao = oSqlDataReader.Item("descricao")
                oJustificativaFalta.justificada = oSqlDataReader.Item("justificada")
                oJustificativaFalta.ativo = oSqlDataReader.Item("ativo")
                oJustificativaFalta.unidade = oSqlDataReader.Item("unidade")
                oJustificativaFalta.codigo_unidade = oSqlDataReader.Item("codigo_unidade")

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function IndexJustificativaFalta(ByVal iCodigoEmpresa As Integer,
                                            ByVal iCodigoUsuario As Integer,
                                            ByVal iCodigoUnidade As Integer) As List(Of JustificativaFalta)

        Try

            'Váriaveis Locais
            Dim oJustificativaFalta As New List(Of JustificativaFalta)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(2) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_justificativa_falta", oSqlParameter)

            While oSqlDataReader.Read

                Dim oJustificativaFaltaInfo As New JustificativaFalta

                oJustificativaFaltaInfo.descricao = oSqlDataReader.Item("descricao")
                oJustificativaFaltaInfo.ativo = oSqlDataReader.Item("ativo")
                oJustificativaFaltaInfo.justificada = oSqlDataReader.Item("justificada")
                oJustificativaFaltaInfo.codigo = oSqlDataReader.Item("codigo")
                oJustificativaFaltaInfo.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oJustificativaFaltaInfo.unidade = oSqlDataReader.Item("unidade")

                oJustificativaFalta.Add(oJustificativaFaltaInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da JustificativaFalta
            Return oJustificativaFalta

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ValidaJustificativaFalta(ByVal iCodigoEmpresa As Integer,
                                                   ByVal iCodigoUnidade As Integer,
                                                   ByVal sDescricao As String,
                                                   ByVal iCodigo As Integer) As Boolean

        Try

            'Váriaveis Locais
            Dim oSqlParameter(3) As SqlParameter
            Dim i As Integer = 0
            Dim iReturn As Integer

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sDescricao

            'Executa Query
            iReturn = ExecuteScalar(sConnection, CommandType.StoredProcedure, "sp_validate_cadastro_basico_justificativa_falta", oSqlParameter)

            'Retorno da JustificativaFalta
            Return (IIf(iReturn > 0, False, True))

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: JUSTIFICATIVA CANCELAMENTO - ORDEM SERVIÇO :::"

    Public Sub InsertJustificativaCancelamentoOrdemServico(ByVal iCodigoEmpresa As Integer,
                                                           ByVal iCodigoUsuario As Integer,
                                                           ByVal sDescricao As String,
                                                           ByVal bAtivo As Boolean)

        'Variaveis Locais
        Dim oSqlParameter(4) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sDescricao : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_cadastro_basico_justificativa_cancelamento_ordem_servico", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateJustificativaCancelamentoOrdemServico(ByVal iCodigoEmpresa As Integer,
                                                           ByVal iCodigoUsuario As Integer,
                                                           ByVal sDescricao As String,
                                                           ByVal bAtivo As Boolean,
                                                           ByVal iCodigo As Integer)

        'Variaveis Locais
        Dim oSqlParameter(5) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sDescricao : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_cadastro_basico_justificativa_cancelamento_ordem_servico", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub DeleteJustificativaCancelamentoOrdemServico(ByVal iCodigoEmpresa As Integer,
                                                           ByVal iCodigoUsuario As Integer,
                                                           ByVal iCodigo As Integer)

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

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_cadastro_basico_justificativa_cancelamento_ordem_servico", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InfoJustificativaCancelamentoOrdemServico(ByVal iCodigoEmpresa As Integer,
                                                         ByVal iCodigo As Integer,
                                                         ByRef oJustificativaCancelamentoOrdemServico As JustificativaCancelamentoOrdemServico)

        'Variaveis Locais
        Dim oSqlParameter(1) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_justificativa_cancelamento_ordem_servico_dados", oSqlParameter)

            While oSqlDataReader.Read

                oJustificativaCancelamentoOrdemServico = New JustificativaCancelamentoOrdemServico
                oJustificativaCancelamentoOrdemServico.codigo = oSqlDataReader.Item("codigo")
                oJustificativaCancelamentoOrdemServico.descricao = oSqlDataReader.Item("descricao")
                oJustificativaCancelamentoOrdemServico.ativo = oSqlDataReader.Item("ativo")

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function IndexJustificativaCancelamentoOrdemServico(ByVal iCodigoEmpresa As Integer,
                                                               ByVal iCodigoUsuario As Integer) As List(Of JustificativaCancelamentoOrdemServico)

        Try

            'Váriaveis Locais
            Dim oJustificativaCancelamentoOrdemServico As New List(Of JustificativaCancelamentoOrdemServico)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(1) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_justificativa_cancelamento_ordem_servico", oSqlParameter)

            While oSqlDataReader.Read

                Dim oJustificativaCancelamentoOrdemServicoInfo As New JustificativaCancelamentoOrdemServico

                oJustificativaCancelamentoOrdemServicoInfo.descricao = oSqlDataReader.Item("descricao")
                oJustificativaCancelamentoOrdemServicoInfo.ativo = oSqlDataReader.Item("ativo")
                oJustificativaCancelamentoOrdemServicoInfo.codigo = oSqlDataReader.Item("codigo")

                oJustificativaCancelamentoOrdemServico.Add(oJustificativaCancelamentoOrdemServicoInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da JustificativaCancelamentoOrdemServico
            Return oJustificativaCancelamentoOrdemServico

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ValidaJustificativaCancelamentoOrdemServico(ByVal iCodigoEmpresa As Integer,
                                                                ByVal sDescricao As String,
                                                                ByVal iCodigo As Integer) As Boolean

        Try

            'Váriaveis Locais
            Dim oSqlParameter(2) As SqlParameter
            Dim i As Integer = 0
            Dim iReturn As Integer

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sDescricao

            'Executa Query
            iReturn = ExecuteScalar(sConnection, CommandType.StoredProcedure, "sp_validate_cadastro_basico_justificativa_cancelamento_ordem_servico", oSqlParameter)

            'Retorno da JustificativaCancelamentoOrdemServico
            Return (IIf(iReturn > 0, False, True))

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: PRIORIDADE :::"

    Public Sub InsertPrioridade(ByVal iCodigoEmpresa As Integer,
                                ByVal iCodigoUsuario As Integer,
                                ByVal iCodigoUnidade As Integer,
                                ByVal sDescricao As String,
                                ByVal bEnviaEmail As Boolean,
                                ByVal sEmail As String,
                                ByVal bAtivo As Boolean)

        'Variaveis Locais
        Dim oSqlParameter(6) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sDescricao : i += 1

            'Seta Parametros - Envia E-mail
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "envia_email"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bEnviaEmail : i += 1

            'Seta Parametros - Email
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "email"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 2000
            oSqlParameter(i).Value = sEmail : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_cadastro_basico_prioridade", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdatePrioridade(ByVal iCodigoEmpresa As Integer,
                                ByVal iCodigoUsuario As Integer,
                                ByVal iCodigoUnidade As Integer,
                                ByVal sDescricao As String,
                                ByVal bEnviaEmail As Boolean,
                                ByVal sEmail As String,
                                ByVal bAtivo As Boolean,
                                ByVal iCodigo As Integer)

        'Variaveis Locais
        Dim oSqlParameter(7) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sDescricao : i += 1

            'Seta Parametros - Envia E-mail
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "envia_email"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bEnviaEmail : i += 1

            'Seta Parametros - Email
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "email"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 2000
            oSqlParameter(i).Value = sEmail : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_cadastro_basico_prioridade", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub DeletePrioridade(ByVal iCodigoEmpresa As Integer,
                                ByVal iCodigoUsuario As Integer,
                                ByVal iCodigo As Integer)

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

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_cadastro_basico_prioridade", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InfoPrioridade(ByVal iCodigoEmpresa As Integer,
                              ByVal iCodigo As Integer,
                              ByRef oPrioridade As Prioridade)

        'Variaveis Locais
        Dim oSqlParameter(1) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_prioridade_dados", oSqlParameter)

            While oSqlDataReader.Read

                oPrioridade = New Prioridade
                oPrioridade.codigo = oSqlDataReader.Item("codigo")
                oPrioridade.descricao = oSqlDataReader.Item("descricao")
                oPrioridade.envia_email = oSqlDataReader.Item("envia_email")
                oPrioridade.email = oSqlDataReader.Item("email")
                oPrioridade.ativo = oSqlDataReader.Item("ativo")
                oPrioridade.unidade = oSqlDataReader.Item("unidade")
                oPrioridade.codigo_unidade = oSqlDataReader.Item("codigo_unidade")

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function IndexPrioridade(ByVal iCodigoEmpresa As Integer,
                                    ByVal iCodigoUsuario As Integer,
                                    ByVal iCodigoUnidade As Integer) As List(Of Prioridade)

        Try

            'Váriaveis Locais
            Dim oPrioridade As New List(Of Prioridade)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(2) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_prioridade", oSqlParameter)

            While oSqlDataReader.Read

                Dim oPrioridadeInfo As New Prioridade

                oPrioridadeInfo.descricao = oSqlDataReader.Item("descricao")
                oPrioridadeInfo.ativo = oSqlDataReader.Item("ativo")
                oPrioridadeInfo.envia_email = oSqlDataReader.Item("envia_email")
                oPrioridadeInfo.email = oSqlDataReader.Item("email")
                oPrioridadeInfo.codigo = oSqlDataReader.Item("codigo")
                oPrioridadeInfo.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oPrioridadeInfo.unidade = oSqlDataReader.Item("unidade")

                oPrioridade.Add(oPrioridadeInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oPrioridade

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ValidaPrioridade(ByVal iCodigoEmpresa As Integer,
                                     ByVal iCodigoUnidade As Integer,
                                     ByVal sDescricao As String,
                                     ByVal iCodigo As Integer) As Boolean

        Try

            'Váriaveis Locais
            Dim oSqlParameter(3) As SqlParameter
            Dim i As Integer = 0
            Dim iReturn As Integer

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sDescricao

            'Executa Query
            iReturn = ExecuteScalar(sConnection, CommandType.StoredProcedure, "sp_validate_cadastro_basico_prioridade", oSqlParameter)

            'Retorno da Função
            Return (IIf(iReturn > 0, False, True))

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: PRODUTO :::"

    Public Sub InsertProduto(ByVal codigoEmpresa As Integer,
                             ByVal codigoUsuario As Integer,
                             ByVal codigoUnidade As Integer,
                             ByVal codigoGrupoProduto As Integer,
                             ByVal codigoProduto As String,
                             ByVal descricao As String,
                             ByVal unidadeMedida As String,
                             ByVal pontoReposicao As Integer,
                             ByVal controlaLote As Boolean,
                             ByVal controlaDataValidade As Boolean,
                             ByVal ativo As Boolean)

        'Variaveis Locais
        Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_usuario", SqlDbType.Int, codigoUsuario),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("codigo_grupo_produto", SqlDbType.Int, codigoGrupoProduto),
                CriarParametro("codigo_produto", SqlDbType.VarChar, codigoProduto),
                CriarParametro("descricao", SqlDbType.VarChar, descricao),
                CriarParametro("unidade_medida", SqlDbType.VarChar, unidadeMedida),
                CriarParametro("ponto_reposicao", SqlDbType.Int, pontoReposicao),
                CriarParametro("controla_lote", SqlDbType.Bit, controlaLote),
                CriarParametro("controla_data_validade", SqlDbType.Bit, controlaDataValidade),
                CriarParametro("ativo", SqlDbType.Bit, ativo)
            }

        Try

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_cadastro_basico_produto", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateProduto(ByVal codigoEmpresa As Integer,
                             ByVal codigoUsuario As Integer,
                             ByVal codigoUnidade As Integer,
                             ByVal codigoGrupoProduto As Integer,
                             ByVal codigoProduto As String,
                             ByVal descricao As String,
                             ByVal unidadeMedida As String,
                             ByVal pontoReposicao As Integer,
                             ByVal controlaLote As Boolean,
                             ByVal controlaDataValidade As Boolean,
                             ByVal ativo As Boolean,
                             ByVal codigo As Integer)

        'Variaveis Locais
        Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_usuario", SqlDbType.Int, codigoUsuario),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("codigo_grupo_produto", SqlDbType.Int, codigoGrupoProduto),
                CriarParametro("codigo_produto", SqlDbType.VarChar, codigoProduto),
                CriarParametro("descricao", SqlDbType.VarChar, descricao),
                CriarParametro("unidade_medida", SqlDbType.VarChar, unidadeMedida),
                CriarParametro("ponto_reposicao", SqlDbType.Int, pontoReposicao),
                CriarParametro("controla_lote", SqlDbType.Bit, controlaLote),
                CriarParametro("controla_data_validade", SqlDbType.Bit, controlaDataValidade),
                CriarParametro("ativo", SqlDbType.Bit, ativo),
                CriarParametro("codigo", SqlDbType.Int, codigo)
            }

        Try

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_cadastro_basico_produto", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub DeleteProduto(ByVal codigoEmpresa As Integer,
                             ByVal codigoUsuario As Integer,
                             ByVal codigo As Integer)

        'Variaveis Locais
        Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_usuario", SqlDbType.Int, codigoUsuario),
                CriarParametro("codigo", SqlDbType.Int, codigo)
            }

        Try

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_cadastro_basico_produto", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function InfoProduto(ByVal codigoEmpresa As Integer,
                                ByVal codigo As Integer) As Produto

        'Variaveis Locais
        Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo", SqlDbType.Int, codigo)
            }
        Dim oReturn As New Produto

        Try

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_produto_dados", oSqlParameter)

                While oSqlDataReader.Read

                    oReturn = New Produto
                    oReturn.codigo = oSqlDataReader.Item("codigo")
                    oReturn.codigoUnidade = oSqlDataReader.Item("codigo_unidade")
                    oReturn.unidade = oSqlDataReader.Item("unidade")
                    oReturn.codigoGrupoProduto = oSqlDataReader.Item("codigo_grupo_produto")
                    oReturn.grupoProduto = oSqlDataReader.Item("grupo_produto")
                    oReturn.codigoProduto = oSqlDataReader.Item("codigo_produto")
                    oReturn.descricao = oSqlDataReader.Item("descricao")
                    oReturn.unidadeMedida = oSqlDataReader.Item("unidade_medida")
                    oReturn.pontoReposicao = oSqlDataReader.Item("ponto_reposicao")
                    oReturn.controlaLote = oSqlDataReader.Item("controla_lote")
                    oReturn.controlaDataValidade = oSqlDataReader.Item("controla_data_validade")
                    oReturn.ativo = oSqlDataReader.Item("ativo")

                End While

            End Using

            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function IndexProduto(ByVal codigoEmpresa As Integer,
                                 ByVal codigoUnidade As Integer,
                                 ByVal codigoGrupoProduto As Integer,
                                 ByVal descricao As String) As List(Of Produto)

        Try

            'Váriavies Locais
            Dim oReturn As New List(Of Produto)
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("codigo_grupo_produto", SqlDbType.Int, codigoGrupoProduto),
                CriarParametro("descricao", SqlDbType.VarChar, descricao)
            }

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_produto", oSqlParameter)

                While oSqlDataReader.Read

                    Dim oProdutoInfo As New Produto

                    oProdutoInfo.codigo = oSqlDataReader.Item("codigo")
                    oProdutoInfo.codigoUnidade = oSqlDataReader.Item("codigo_unidade")
                    oProdutoInfo.unidade = oSqlDataReader.Item("unidade")
                    oProdutoInfo.codigoGrupoProduto = oSqlDataReader.Item("codigo_grupo_produto")
                    oProdutoInfo.grupoProduto = oSqlDataReader.Item("grupo_produto")
                    oProdutoInfo.codigoProduto = oSqlDataReader.Item("codigo_produto")
                    oProdutoInfo.descricao = oSqlDataReader.Item("descricao")
                    oProdutoInfo.unidadeMedida = oSqlDataReader.Item("unidade_medida")
                    oProdutoInfo.pontoReposicao = oSqlDataReader.Item("ponto_reposicao")
                    oProdutoInfo.quantidade = oSqlDataReader.Item("quantidade")
                    oProdutoInfo.controlaLote = oSqlDataReader.Item("controla_lote")
                    oProdutoInfo.controlaDataValidade = oSqlDataReader.Item("controla_data_validade")
                    oProdutoInfo.ativo = oSqlDataReader.Item("ativo")

                    oReturn.Add(oProdutoInfo)

                End While

            End Using

            'Retorno da Função
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ValidaProduto(ByVal codigoEmpresa As Integer,
                                  ByVal codigoUnidade As Integer,
                                  ByVal codigoProduto As String,
                                  ByVal codigo As Integer) As Boolean

        Try

            'Váriavies Locais
            Dim iReturn As Integer
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("codigo_produto", SqlDbType.VarChar, codigoProduto),
                CriarParametro("codigo", SqlDbType.Int, codigo)
            }

            'Executa Query
            iReturn = ExecuteScalar(sConnection, CommandType.StoredProcedure, "sp_validate_cadastro_basico_produto", oSqlParameter)

            'Retorno da Função
            Return (IIf(iReturn > 0, False, True))

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: PROGRAMADA :::"

    Public Sub InsertProgramada(ByVal iCodigoEmpresa As Integer,
                                ByVal iCodigoUsuario As Integer,
                                ByVal iCodigoUnidade As Integer,
                                ByVal iCodigoCategoria As Integer,
                                ByVal iCodigoModulo As Integer,
                                ByVal iCodigoSetor As Integer,
                                ByVal sDescricao As String,
                                ByVal dValorPrevisto As String,
                                ByVal iQuantidadeEquipamento As Integer,
                                ByVal iCodigoPeriodicidade As Integer,
                                ByVal iIntervalo As Integer,
                                ByVal iCodigoTipoServico As Integer,
                                ByVal iCodigoTipoOrdemServico As Integer,
                                ByVal bExigeLaudo As Boolean,
                                ByVal bAtivo As Boolean,
                                ByVal bRotina As Boolean,
                                Optional ByVal lCodigoChecklist As Long = -1,
                                Optional ByVal bEnviaEmail As Boolean = False,
                                Optional ByVal sEmail As String = "",
                                Optional ByVal lCodigoEquipamento As Long = -1,
                                Optional ByVal iDiasAlerta As Integer = 0,
                                Optional ByVal iCodigoPrioridade As Integer = -1,
                                Optional ByVal dTempoEstimado As Double = -1,
                                Optional ByVal bSegunda As Boolean = True,
                                Optional ByVal bTerca As Boolean = True,
                                Optional ByVal bQuarta As Boolean = True,
                                Optional ByVal bQuinta As Boolean = True,
                                Optional ByVal bSexta As Boolean = True,
                                Optional ByVal bSabado As Boolean = True,
                                Optional ByVal bDomingo As Boolean = True,
                                Optional ByVal iCodigoFamiliaEquipamento As Integer = -1)

        'Variaveis Locais
        Dim oSqlParameter(31) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Módulo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_modulo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoModulo : i += 1

            'Seta Parametros - Código Categoria
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_categoria"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoCategoria = -1, DBNull.Value, iCodigoCategoria) : i += 1

            'Seta Parametros - Código Setor
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_setor"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoSetor = -1, DBNull.Value, CInt(iCodigoSetor)) : i += 1

            'Seta Parametros - Código Família Equipamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_familia_equipamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoFamiliaEquipamento = -1, DBNull.Value, iCodigoFamiliaEquipamento) : i += 1

            'Seta Parametros - Código Equipamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_equipamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = IIf(lCodigoEquipamento = -1, DBNull.Value, lCodigoEquipamento) : i += 1

            'Seta Parametros - Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = sDescricao.ToUpper : i += 1

            'Seta Parametros - Valor Previsto
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "valor_previsto"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = IIf(IsNumeric(dValorPrevisto), dValorPrevisto, 0) : i += 1

            'Seta Parametros - Quantidade Equipamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "quantidade_equipamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = iQuantidadeEquipamento : i += 1

            'Seta Parametros - Código Periodicidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_periodicidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoPeriodicidade : i += 1

            'Seta Parametros - Intervalo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "intervalo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iIntervalo : i += 1

            'Seta Parametros - Código Checklist
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_checklist"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = IIf(lCodigoChecklist = -1, DBNull.Value, lCodigoChecklist) : i += 1

            'Seta Parametros - Código Tipo Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_tipo_servico"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoTipoServico : i += 1

            'Seta Parametros - Código Tipo Ordem Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_tipo_ordem_servico"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = IIf(iCodigoTipoOrdemServico = -1, DBNull.Value, iCodigoTipoOrdemServico) : i += 1

            'Seta Parametros - Exige Laudo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "exige_laudo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bExigeLaudo : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo : i += 1

            'Seta Parametros - Rotina
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "rotina"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bRotina : i += 1

            'Seta Parametros - Envia E-mail
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "envia_email"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bEnviaEmail : i += 1

            'Seta Parametros - Email
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "email"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 2000
            oSqlParameter(i).Value = sEmail : i += 1

            'Seta Parametros - Dias Alerta
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "dias_alerta"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iDiasAlerta : i += 1

            'Seta Parametros - Código Prioridade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_prioridade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoPrioridade = -1, DBNull.Value, iCodigoPrioridade) : i += 1

            'Seta Parametros - Tempo Estimado
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "tempo_estimado"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = IIf(dTempoEstimado = -1, DBNull.Value, dTempoEstimado) : i += 1

            'Seta Parametros - Segunda
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "segunda"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bSegunda : i += 1

            'Seta Parametros - Terça
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "terca"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bTerca : i += 1

            'Seta Parametros - Quarta
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "quarta"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bQuarta : i += 1

            'Seta Parametros - Quinta
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "quinta"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bQuinta : i += 1

            'Seta Parametros - Sexta
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "sexta"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bSexta : i += 1

            'Seta Parametros - Sábado
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "sabado"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bSabado : i += 1

            'Seta Parametros - Domingo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "domingo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bDomingo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_pcm_programada", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateProgramada(ByVal iCodigoEmpresa As Integer,
                                ByVal iCodigoUsuario As Integer,
                                ByVal iCodigoUnidade As Integer,
                                ByVal iCodigoCategoria As Integer,
                                ByVal iCodigoSetor As Integer,
                                ByVal sDescricao As String,
                                ByVal dValorPrevisto As String,
                                ByVal iQuantidadeEquipamento As Integer,
                                ByVal iCodigoPeriodicidade As Integer,
                                ByVal iIntervalo As Integer,
                                ByVal iCodigoTipoServico As Integer,
                                ByVal iCodigoTipoOrdemServico As Integer,
                                ByVal bExigeLaudo As Boolean,
                                ByVal bAtivo As Boolean,
                                ByVal iCodigoModulo As Integer,
                                ByVal lCodigo As Long,
                                ByVal iCodigoUnidadeOld As Integer,
                                Optional ByVal lCodigoChecklist As Long = -1,
                                Optional ByVal bEnviaEmail As Boolean = False,
                                Optional ByVal sEmail As String = "",
                                Optional ByVal lCodigoEquipamento As Long = -1,
                                Optional ByVal iDiasAlerta As Integer = 0,
                                Optional ByVal iCodigoPrioridade As Integer = -1,
                                Optional ByVal dTempoEstimado As Double = -1,
                                Optional ByVal bSegunda As Boolean = True,
                                Optional ByVal bTerca As Boolean = True,
                                Optional ByVal bQuarta As Boolean = True,
                                Optional ByVal bQuinta As Boolean = True,
                                Optional ByVal bSexta As Boolean = True,
                                Optional ByVal bSabado As Boolean = True,
                                Optional ByVal bDomingo As Boolean = True)

        'Variaveis Locais
        Dim oSqlParameter(30) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Categoria
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_categoria"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoCategoria = -1, DBNull.Value, iCodigoCategoria) : i += 1

            'Seta Parametros - Código Setor
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_setor"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoSetor = -1, DBNull.Value, iCodigoSetor) : i += 1

            'Seta Parametros - Código Equipamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_equipamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = IIf(lCodigoEquipamento = -1, DBNull.Value, lCodigoEquipamento) : i += 1

            'Seta Parametros - Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = sDescricao.ToUpper : i += 1

            'Seta Parametros - Valor Previsto
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "valor_previsto"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = IIf(IsNumeric(dValorPrevisto), dValorPrevisto, 0) : i += 1

            'Seta Parametros - Quantidade Equipamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "quantidade_equipamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = iQuantidadeEquipamento : i += 1

            'Seta Parametros - Código Periodicidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_periodicidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoPeriodicidade : i += 1

            'Seta Parametros - Intervalo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "intervalo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iIntervalo : i += 1

            'Seta Parametros - Código Checklist
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_checklist"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = IIf(lCodigoChecklist = -1, DBNull.Value, lCodigoChecklist) : i += 1

            'Seta Parametros - Código Tipo Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_tipo_servico"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoTipoServico : i += 1

            'Seta Parametros - Código Tipo Ordem Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_tipo_ordem_servico"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoTipoOrdemServico : i += 1

            'Seta Parametros - Exige Laudo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "exige_laudo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bExigeLaudo : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo : i += 1

            'Seta Parametros - Envia E-mail
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "envia_email"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bEnviaEmail : i += 1

            'Seta Parametros - Email
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "email"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 2000
            oSqlParameter(i).Value = sEmail : i += 1

            'Seta Parametros - Dias Alerta
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "dias_alerta"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iDiasAlerta : i += 1

            'Seta Parametros - Código Prioridade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_prioridade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = IIf(iCodigoPrioridade = -1, DBNull.Value, iCodigoPrioridade) : i += 1

            'Seta Parametros - Tempo Estimado
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "tempo_estimado"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = IIf(dTempoEstimado = -1, DBNull.Value, dTempoEstimado) : i += 1

            'Seta Parametros - Segunda
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "segunda"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bSegunda : i += 1

            'Seta Parametros - Terça
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "terca"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bTerca : i += 1

            'Seta Parametros - Quarta
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "quarta"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bQuarta : i += 1

            'Seta Parametros - Quinta
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "quinta"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bQuinta : i += 1

            'Seta Parametros - Sexta
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "sexta"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bSexta : i += 1

            'Seta Parametros - Sábado
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "sabado"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bSabado : i += 1

            'Seta Parametros - Domingo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "domingo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bDomingo : i += 1

            'Seta Parametros - Código Módulo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_modulo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoModulo = -1, DBNull.Value, iCodigoModulo) : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigo : i += 1

            'Seta Parametros - Código Unidade Old
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade_old"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidadeOld

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_pcm_programada", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub DeleteProgramada(ByVal iCodigoEmpresa As Integer,
                                ByVal iCodigoUsuario As Integer,
                                ByVal lCodigo As Long,
                                ByVal iCodigoUnidade As Integer)

        'Variaveis Locais
        Dim oSqlParameter(3) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigo : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_pcm_programada", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InfoProgramada(ByVal iCodigoEmpresa As Integer,
                              ByVal lCodigo As Long,
                              ByVal iCodigoUnidade As Integer,
                              ByRef oProgramada As Programada)

        'Variaveis Locais
        Dim oSqlParameter(2) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigo : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pcm_programada_dados", oSqlParameter)

            While oSqlDataReader.Read

                oProgramada = New Programada
                oProgramada.codigo = oSqlDataReader.Item("codigo")
                oProgramada.unidade = oSqlDataReader.Item("unidade")
                oProgramada.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oProgramada.categoria = oSqlDataReader.Item("categoria")
                oProgramada.codigo_categoria = oSqlDataReader.Item("codigo_categoria")
                oProgramada.codigo_setor = oSqlDataReader.Item("codigo_setor")
                oProgramada.setor = oSqlDataReader.Item("setor")
                oProgramada.equipamento = oSqlDataReader.Item("equipamento")
                oProgramada.codigo_equipamento = oSqlDataReader.Item("codigo_equipamento")
                oProgramada.descricao = oSqlDataReader.Item("descricao")
                oProgramada.valor_previsto = oSqlDataReader.Item("valor_previsto")
                oProgramada.quantidade_equipamento = oSqlDataReader.Item("quantidade_equipamento")
                oProgramada.periodicidade = oSqlDataReader.Item("periodicidade")
                oProgramada.codigo_periodicidade = oSqlDataReader.Item("codigo_periodicidade")
                oProgramada.intervalo = oSqlDataReader.Item("intervalo")
                oProgramada.codigo_checklist = oSqlDataReader.Item("codigo_checklist")
                oProgramada.checklist = oSqlDataReader.Item("checklist")
                oProgramada.codigo_tipo_servico = oSqlDataReader.Item("codigo_tipo_servico")
                oProgramada.tipo_servico = oSqlDataReader.Item("tipo_servico")
                oProgramada.codigo_tipo_ordem_servico = oSqlDataReader.Item("codigo_tipo_ordem_servico")
                oProgramada.tipo_ordem_servico = oSqlDataReader.Item("tipo_ordem_servico")
                oProgramada.envia_email = oSqlDataReader.Item("envia_email")
                oProgramada.email = oSqlDataReader.Item("email")
                oProgramada.dias_alerta = oSqlDataReader.Item("dias_alerta")
                oProgramada.exige_laudo = oSqlDataReader.Item("exige_laudo")
                oProgramada.ativo = oSqlDataReader.Item("ativo")
                oProgramada.codigo_modulo = oSqlDataReader.Item("codigo_modulo")
                oProgramada.modulo = oSqlDataReader.Item("modulo")
                oProgramada.codigo_prioridade = oSqlDataReader.Item("codigo_prioridade")
                oProgramada.tempo_estimado = oSqlDataReader.Item("tempo_estimado")
                oProgramada.segunda = oSqlDataReader.Item("segunda")
                oProgramada.terca = oSqlDataReader.Item("terca")
                oProgramada.quarta = oSqlDataReader.Item("quarta")
                oProgramada.quinta = oSqlDataReader.Item("quinta")
                oProgramada.sexta = oSqlDataReader.Item("sexta")
                oProgramada.sabado = oSqlDataReader.Item("sabado")
                oProgramada.domingo = oSqlDataReader.Item("domingo")

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function IndexProgramada(ByVal iCodigoEmpresa As Integer,
                                    ByVal iCodigoUnidade As Integer,
                                    ByVal iCodigoTipoOrdemServico As Integer,
                                    ByVal iCodigoModulo As Integer,
                                    ByVal bRotina As Boolean,
                                    Optional ByVal sDescricao As String = "",
                                    Optional ByVal sEquipamento As String = "",
                                    Optional ByVal iCodigoPeriodicidade As Integer = -1,
                                    Optional ByVal iAtivo As Integer = -1) As List(Of Programada)

        Try

            'Váriaveis Locais
            Dim oProgramada As New List(Of Programada)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(8) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Tipo Ordem Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_tipo_ordem_servico"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoTipoOrdemServico : i += 1

            'Seta Parametros - Código Modulo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_modulo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoModulo : i += 1

            'Seta Parametros - Rotina
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "rotina"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bRotina : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 150
            oSqlParameter(i).Value = sDescricao : i += 1

            'Seta Parametros - Equipamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "equipamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 150
            oSqlParameter(i).Value = sEquipamento : i += 1

            'Seta Parametros - Código Periodicidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_periodicidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoPeriodicidade : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = IIf(iAtivo = 1, True, IIf(iAtivo = 0, False, DBNull.Value))

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pcm_programada", oSqlParameter)

            While oSqlDataReader.Read

                Dim oProgramadaInfo As New Programada

                oProgramadaInfo.codigo = oSqlDataReader.Item("codigo")
                oProgramadaInfo.unidade = oSqlDataReader.Item("unidade")
                oProgramadaInfo.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oProgramadaInfo.categoria = oSqlDataReader.Item("categoria")
                oProgramadaInfo.setor = oSqlDataReader.Item("setor")
                oProgramadaInfo.equipamento = oSqlDataReader.Item("equipamento")
                oProgramadaInfo.descricao = oSqlDataReader.Item("descricao")
                oProgramadaInfo.valor_previsto = oSqlDataReader.Item("valor_previsto")
                oProgramadaInfo.quantidade_equipamento = oSqlDataReader.Item("quantidade_equipamento")
                oProgramadaInfo.periodicidade = oSqlDataReader.Item("periodicidade")
                oProgramadaInfo.intervalo = oSqlDataReader.Item("intervalo")
                oProgramadaInfo.checklist = oSqlDataReader.Item("checklist")
                oProgramadaInfo.codigo_checklist = oSqlDataReader.Item("codigo_checklist")
                oProgramadaInfo.tipo_servico = oSqlDataReader.Item("tipo_servico")
                oProgramadaInfo.tipo_ordem_servico = oSqlDataReader.Item("tipo_ordem_servico")
                oProgramadaInfo.intervalo = oSqlDataReader.Item("intervalo")
                oProgramadaInfo.exige_laudo = oSqlDataReader.Item("exige_laudo")
                oProgramadaInfo.ativo = oSqlDataReader.Item("ativo")

                oProgramada.Add(oProgramadaInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oProgramada

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: RELATÓRIO :::"

    Public Sub InsertRelatorio(ByVal iCodigoEmpresa As Integer,
                               ByVal iCodigoUsuario As Integer,
                               ByVal sDescricao As String,
                               ByVal bAtivo As Boolean)

        'Variaveis Locais
        Dim oSqlParameter(3) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sDescricao : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_cadastro_basico_relatorio", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateRelatorio(ByVal iCodigoEmpresa As Integer,
                               ByVal iCodigoUsuario As Integer,
                               ByVal sDescricao As String,
                               ByVal bAtivo As Boolean,
                               ByVal iCodigo As Integer)

        'Variaveis Locais
        Dim oSqlParameter(4) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sDescricao : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_cadastro_basico_relatorio", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub DeleteRelatorio(ByVal iCodigoEmpresa As Integer,
                               ByVal iCodigoUsuario As Integer,
                               ByVal iCodigo As Integer)

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

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_cadastro_basico_relatorio", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InfoRelatorio(ByVal iCodigoEmpresa As Integer,
                             ByVal iCodigo As Integer,
                             ByRef oRelatorio As MODELS.Relatorio)

        'Variaveis Locais
        Dim oSqlParameter(1) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_relatorio_dados", oSqlParameter)

            While oSqlDataReader.Read

                oRelatorio = New MODELS.Relatorio
                oRelatorio.codigo = oSqlDataReader.Item("codigo")
                oRelatorio.descricao = oSqlDataReader.Item("descricao")
                oRelatorio.ativo = oSqlDataReader.Item("ativo")

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function IndexRelatorio(ByVal iCodigoEmpresa As Integer) As List(Of MODELS.Relatorio)

        Try

            'Váriaveis Locais
            Dim oRelatorio As New List(Of MODELS.Relatorio)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(0) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_relatorio", oSqlParameter)

            While oSqlDataReader.Read

                Dim oRelatorioInfo As New MODELS.Relatorio

                oRelatorioInfo.descricao = oSqlDataReader.Item("descricao")
                oRelatorioInfo.ativo = oSqlDataReader.Item("ativo")
                oRelatorioInfo.codigo = oSqlDataReader.Item("codigo")

                oRelatorio.Add(oRelatorioInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oRelatorio

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ValidaRelatorio(ByVal iCodigoEmpresa As Integer,
                                    ByVal sDescricao As String,
                                    ByVal iCodigo As Integer) As Boolean

        Try

            'Váriaveis Locais
            Dim oSqlParameter(2) As SqlParameter
            Dim i As Integer = 0
            Dim iReturn As Integer

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sDescricao

            'Executa Query
            iReturn = ExecuteScalar(sConnection, CommandType.StoredProcedure, "sp_validate_cadastro_basico_relatorio", oSqlParameter)

            'Retorno da Função
            Return (IIf(iReturn > 0, False, True))

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: SETOR :::"

    Public Sub InsertSetor(ByVal iCodigoEmpresa As Integer,
                           ByVal iCodigoUsuario As Integer,
                           ByVal iCodigoUnidade As Integer,
                           ByVal sDescricao As String,
                           ByVal sObservacao As String,
                           ByVal dMetragem As Double,
                           ByVal dCargaTermica As Double,
                           ByVal sDescricaoAtividade As String,
                           ByVal iNumeroPessoasFixas As Integer,
                           ByVal iNumeroPessoasVolantes As Integer,
                           ByVal bAtivo As Boolean,
                           ByRef iCodigo As Integer)

        'Variaveis Locais
        Dim oSqlParameter(12) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sDescricao : i += 1

            'Seta Parametros - Observação
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "observacao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 2000
            oSqlParameter(i).Value = sObservacao : i += 1

            'Seta Parametros - Metragem
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "metragem"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = dMetragem : i += 1

            'Seta Parametros - Carga Térmica
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "carga_termica"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = dCargaTermica : i += 1

            'Seta Parametros - Descrição da Atividade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao_atividade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 150
            oSqlParameter(i).Value = sDescricaoAtividade : i += 1

            'Seta Parametros - Número de Pessoas Fixas
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "numero_pessoas_fixas"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iNumeroPessoasFixas : i += 1

            'Seta Parametros - Número de Pessoas Volantes
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "numero_pessoas_volantes"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iNumeroPessoasVolantes : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Output
            oSqlParameter(i).SqlDbType = SqlDbType.Int

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_cadastro_basico_setor", oSqlParameter)

            iCodigo = oSqlParameter(i).Value

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateSetor(ByVal iCodigoEmpresa As Integer,
                           ByVal iCodigoUsuario As Integer,
                           ByVal iCodigoUnidade As Integer,
                           ByVal sDescricao As String,
                           ByVal sObservacao As String,
                           ByVal dMetragem As String,
                           ByVal dCargaTermica As String,
                           ByVal sDescricaoAtividade As String,
                           ByVal iNumeroPessoasFixas As String,
                           ByVal iNumeroPessoasVolantes As String,
                           ByVal bAtivo As Boolean,
                           ByVal iCodigo As Integer)

        'Variaveis Locais
        Dim oSqlParameter(12) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sDescricao : i += 1

            'Seta Parametros - Observação
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "observacao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 2000
            oSqlParameter(i).Value = sObservacao : i += 1

            'Seta Parametros - Metragem
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "metragem"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = IIf(IsNumeric(dMetragem), dMetragem, 0) : i += 1

            'Seta Parametros - Carga Térmica
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "carga_termica"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = IIf(IsNumeric(dCargaTermica), dCargaTermica, 0) : i += 1

            'Seta Parametros - Descrição da Atividade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao_atividade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 150
            oSqlParameter(i).Value = sDescricaoAtividade : i += 1

            'Seta Parametros - Número de Pessoas Fixas
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "numero_pessoas_fixas"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(IsNumeric(iNumeroPessoasFixas), iNumeroPessoasFixas, 0) : i += 1

            'Seta Parametros - Número de Pessoas Volantes
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "numero_pessoas_volantes"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(IsNumeric(iNumeroPessoasVolantes), iNumeroPessoasVolantes, 0) : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_cadastro_basico_setor", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub DeleteSetor(ByVal iCodigoEmpresa As Integer,
                           ByVal iCodigoUsuario As Integer,
                           ByVal iCodigo As Integer)

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

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_cadastro_basico_setor", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InfoSetor(ByVal iCodigoEmpresa As Integer,
                         ByVal iCodigo As Integer,
                         ByRef oSetor As Setor)

        'Variaveis Locais
        Dim oSqlParameter(1) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_setor_dados", oSqlParameter)

            While oSqlDataReader.Read

                oSetor = New Setor
                oSetor.codigo = oSqlDataReader.Item("codigo")
                oSetor.descricao = oSqlDataReader.Item("descricao")
                oSetor.observacao = oSqlDataReader.Item("observacao")
                oSetor.metragem = oSqlDataReader.Item("metragem")
                oSetor.carga_termica = oSqlDataReader.Item("carga_termica")
                oSetor.descricao_atividade = oSqlDataReader.Item("descricao_atividade")
                oSetor.numero_pessoas_fixas = oSqlDataReader.Item("numero_pessoas_fixas")
                oSetor.numero_pessoas_volantes = oSqlDataReader.Item("numero_pessoas_volantes")
                oSetor.ativo = oSqlDataReader.Item("ativo")
                oSetor.unidade = oSqlDataReader.Item("unidade")
                oSetor.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oSetor.local = IndexSetorLocal(iCodigoEmpresa:=iCodigoEmpresa,
                                               iCodigoUnidade:=oSqlDataReader.Item("codigo_unidade"),
                                               iCodigoSetor:=oSqlDataReader.Item("codigo"))

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function IndexSetor(ByVal iCodigoEmpresa As Integer,
                               ByVal iCodigoUsuario As Integer,
                               ByVal iCodigoUnidade As Integer) As List(Of Setor)

        Try

            'Váriaveis Locais
            Dim oSetor As New List(Of Setor)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(2) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_setor", oSqlParameter)

            While oSqlDataReader.Read

                Dim oSetorInfo As New Setor

                oSetorInfo.descricao = oSqlDataReader.Item("descricao")
                oSetorInfo.ativo = oSqlDataReader.Item("ativo")
                oSetorInfo.codigo = oSqlDataReader.Item("codigo")
                oSetorInfo.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oSetorInfo.unidade = oSqlDataReader.Item("unidade")

                oSetor.Add(oSetorInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oSetor

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ValidaSetor(ByVal iCodigoEmpresa As Integer,
                                ByVal iCodigoUnidade As Integer,
                                ByVal sDescricao As String,
                                ByVal iCodigo As Integer) As Boolean

        Try

            'Váriaveis Locais
            Dim oSqlParameter(3) As SqlParameter
            Dim i As Integer = 0
            Dim iReturn As Integer

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sDescricao

            'Executa Query
            iReturn = ExecuteScalar(sConnection, CommandType.StoredProcedure, "sp_validate_cadastro_basico_setor", oSqlParameter)

            'Retorno da Função
            Return (IIf(iReturn > 0, False, True))

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: SETOR / LOCAL :::"

    Public Sub InsertSetorLocal(ByVal iCodigoEmpresa As Integer,
                                ByVal iCodigoUsuario As Integer,
                                ByVal iCodigoUnidade As Integer,
                                ByVal iCodigoSetor As Integer,
                                ByVal sLocal As String)

        'Variaveis Locais
        Dim oSqlParameter(4) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Setor
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_setor"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoSetor : i += 1

            'Seta Parametros - Local
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "local"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sLocal.ToUpper()

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_cadastro_basico_setor_local", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function IndexSetorLocal(ByVal iCodigoEmpresa As Integer,
                                    ByVal iCodigoUnidade As Integer,
                                    ByVal iCodigoSetor As Integer) As List(Of SetorLocal)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of SetorLocal)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(2) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Setor
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_setor"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoSetor

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_setor_local", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New SetorLocal

                oInfo.codigo = oSqlDataReader.Item("codigo")
                oInfo.local = oSqlDataReader.Item("local")
                oInfo.excluido = 0

                oReturn.Add(oInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Sub UpdateSetorLocal(ByVal iCodigoEmpresa As Integer,
                                ByVal iCodigoUsuario As Integer,
                                ByVal iCodigoUnidade As Integer,
                                ByVal iCodigoSetor As Integer,
                                ByVal sLocal As String,
                                ByVal iExcluido As Integer,
                                ByVal iCodigo As Integer)

        'Variaveis Locais
        Dim oSqlParameter(6) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Setor
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_setor"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoSetor : i += 1

            'Seta Parametros - Local
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "local"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sLocal.ToUpper() : i += 1

            'Seta Parametros - Excluido
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "excluido"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iExcluido : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_cadastro_basico_setor_local", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region

#Region "::: TAREFA :::"

    Public Sub InsertTarefa(ByVal iCodigoEmpresa As Integer,
                            ByVal iCodigoUsuario As Integer,
                            ByVal iCodigoUnidade As Integer,
                            ByVal iCodigoModulo As Integer,
                            ByVal sDescricao As String,
                            ByVal iCodigoPeriodicidade As Integer,
                            ByVal iIntervalo As Integer,
                            ByVal bAtivo As Boolean,
                            ByVal lCodigoChecklist As Long,
                            Optional ByVal bSegunda As Boolean = True,
                            Optional ByVal bTerca As Boolean = True,
                            Optional ByVal bQuarta As Boolean = True,
                            Optional ByVal bQuinta As Boolean = True,
                            Optional ByVal bSexta As Boolean = True,
                            Optional ByVal bSabado As Boolean = True,
                            Optional ByVal bDomingo As Boolean = True)

        'Variaveis Locais
        Dim oSqlParameter(15) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Módulo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_modulo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoModulo : i += 1

            'Seta Parametros - Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = sDescricao.ToUpper : i += 1

            'Seta Parametros - Código Periodicidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_periodicidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoPeriodicidade : i += 1

            'Seta Parametros - Intervalo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "intervalo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iIntervalo : i += 1

            'Seta Parametros - Código Checklist
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_checklist"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = IIf(lCodigoChecklist = -1, DBNull.Value, lCodigoChecklist) : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo : i += 1

            'Seta Parametros - Segunda
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "segunda"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bSegunda : i += 1

            'Seta Parametros - Terça
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "terca"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bTerca : i += 1

            'Seta Parametros - Quarta
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "quarta"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bQuarta : i += 1

            'Seta Parametros - Quinta
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "quinta"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bQuinta : i += 1

            'Seta Parametros - Sexta
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "sexta"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bSexta : i += 1

            'Seta Parametros - Sábado
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "sabado"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bSabado : i += 1

            'Seta Parametros - Domingo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "domingo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bDomingo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_qa_tarefa", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateTarefa(ByVal iCodigoEmpresa As Integer,
                            ByVal iCodigoUsuario As Integer,
                            ByVal iCodigoUnidade As Integer,
                            ByVal sDescricao As String,
                            ByVal iCodigoPeriodicidade As Integer,
                            ByVal iIntervalo As Integer,
                            ByVal bAtivo As Boolean,
                            ByVal iCodigoModulo As Integer,
                            ByVal lCodigo As Long,
                            ByVal iCodigoUnidadeOld As Integer,
                            ByVal lCodigoChecklist As Long,
                            Optional ByVal bSegunda As Boolean = True,
                            Optional ByVal bTerca As Boolean = True,
                            Optional ByVal bQuarta As Boolean = True,
                            Optional ByVal bQuinta As Boolean = True,
                            Optional ByVal bSexta As Boolean = True,
                            Optional ByVal bSabado As Boolean = True,
                            Optional ByVal bDomingo As Boolean = True)

        'Variaveis Locais
        Dim oSqlParameter(17) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = sDescricao.ToUpper : i += 1

            'Seta Parametros - Código Periodicidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_periodicidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoPeriodicidade : i += 1

            'Seta Parametros - Intervalo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "intervalo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iIntervalo : i += 1

            'Seta Parametros - Código Checklist
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_checklist"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = IIf(lCodigoChecklist = -1, DBNull.Value, lCodigoChecklist) : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo : i += 1

            'Seta Parametros - Segunda
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "segunda"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bSegunda : i += 1

            'Seta Parametros - Terça
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "terca"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bTerca : i += 1

            'Seta Parametros - Quarta
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "quarta"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bQuarta : i += 1

            'Seta Parametros - Quinta
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "quinta"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bQuinta : i += 1

            'Seta Parametros - Sexta
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "sexta"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bSexta : i += 1

            'Seta Parametros - Sábado
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "sabado"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bSabado : i += 1

            'Seta Parametros - Domingo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "domingo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bDomingo : i += 1

            'Seta Parametros - Código Módulo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_modulo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoModulo = -1, DBNull.Value, iCodigoModulo) : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigo : i += 1

            'Seta Parametros - Código Unidade Old
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade_old"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidadeOld

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_qa_tarefa", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub DeleteTarefa(ByVal iCodigoEmpresa As Integer,
                            ByVal iCodigoUsuario As Integer,
                            ByVal lCodigo As Long,
                            ByVal iCodigoUnidade As Integer)

        'Variaveis Locais
        Dim oSqlParameter(3) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigo : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_qa_tarefa", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InfoTarefa(ByVal iCodigoEmpresa As Integer,
                              ByVal lCodigo As Long,
                              ByVal iCodigoUnidade As Integer,
                              ByRef oTarefa As Tarefa)

        'Variaveis Locais
        Dim oSqlParameter(2) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigo : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_qa_tarefa_dados", oSqlParameter)

            While oSqlDataReader.Read

                oTarefa = New Tarefa
                oTarefa.codigo = oSqlDataReader.Item("codigo")
                oTarefa.unidade = oSqlDataReader.Item("unidade")
                oTarefa.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oTarefa.descricao = oSqlDataReader.Item("descricao")
                oTarefa.periodicidade = oSqlDataReader.Item("periodicidade")
                oTarefa.codigo_periodicidade = oSqlDataReader.Item("codigo_periodicidade")
                oTarefa.intervalo = oSqlDataReader.Item("intervalo")
                oTarefa.codigo_checklist = oSqlDataReader.Item("codigo_checklist")
                oTarefa.checklist = oSqlDataReader.Item("checklist")
                oTarefa.ativo = oSqlDataReader.Item("ativo")
                oTarefa.codigo_modulo = oSqlDataReader.Item("codigo_modulo")
                oTarefa.modulo = oSqlDataReader.Item("modulo")
                oTarefa.segunda = oSqlDataReader.Item("segunda")
                oTarefa.terca = oSqlDataReader.Item("terca")
                oTarefa.quarta = oSqlDataReader.Item("quarta")
                oTarefa.quinta = oSqlDataReader.Item("quinta")
                oTarefa.sexta = oSqlDataReader.Item("sexta")
                oTarefa.sabado = oSqlDataReader.Item("sabado")
                oTarefa.domingo = oSqlDataReader.Item("domingo")

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function IndexTarefa(ByVal iCodigoEmpresa As Integer,
                                ByVal iCodigoUnidade As Integer,
                                ByVal iCodigoModulo As Integer,
                                ByVal sDescricao As String) As List(Of Tarefa)

        Try

            'Váriaveis Locais
            Dim oTarefa As New List(Of Tarefa)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(3) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Modulo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_modulo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoModulo : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = sDescricao

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_qa_tarefa", oSqlParameter)

            While oSqlDataReader.Read

                Dim oTarefaInfo As New Tarefa

                oTarefaInfo.codigo = oSqlDataReader.Item("codigo")
                oTarefaInfo.unidade = oSqlDataReader.Item("unidade")
                oTarefaInfo.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oTarefaInfo.descricao = oSqlDataReader.Item("descricao")
                oTarefaInfo.periodicidade = oSqlDataReader.Item("periodicidade")
                oTarefaInfo.intervalo = oSqlDataReader.Item("intervalo")
                oTarefaInfo.checklist = oSqlDataReader.Item("checklist")
                oTarefaInfo.codigo_checklist = oSqlDataReader.Item("codigo_checklist")
                oTarefaInfo.intervalo = oSqlDataReader.Item("intervalo")
                oTarefaInfo.ativo = oSqlDataReader.Item("ativo")

                oTarefa.Add(oTarefaInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oTarefa

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: TIPO DE APARTAMENTO :::"

    Public Sub InsertTipoApartamento(ByVal codigoEmpresa As Integer,
                                     ByVal codigoUsuario As Integer,
                                     ByVal codigoUnidade As Integer,
                                     ByVal descricao As String,
                                     ByVal codigoChecklistUH As Long,
                                     ByVal codigoPeriodicidadeUH As Integer,
                                     ByVal intervaloUH As Integer,
                                     ByVal codigoChecklistGovernancaPermanencia As Long,
                                     ByVal codigoChecklistGovernancaSaida As Long,
                                     ByVal codigoChecklistGovernancaManutencao As Long,
                                     ByVal codigoChecklistGovernancaPermanenciaVistoria As Long,
                                     ByVal codigoChecklistGovernancaSaidaVistoria As Long,
                                     ByVal codigoChecklistGovernancaManutencaoVistoria As Long,
                                     ByVal ativo As Boolean)

        Try

            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_usuario", SqlDbType.Int, codigoUsuario),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("descricao", SqlDbType.VarChar, descricao),
                CriarParametro("codigo_checklist_uh", SqlDbType.BigInt, IIf(codigoChecklistUH = -1, DBNull.Value, codigoChecklistUH)),
                CriarParametro("codigo_periodicidade_uh", SqlDbType.Int, IIf(codigoPeriodicidadeUH = -1, DBNull.Value, codigoPeriodicidadeUH)),
                CriarParametro("intervalo_uh", SqlDbType.Int, IIf(intervaloUH = -1, DBNull.Value, intervaloUH)),
                CriarParametro("codigo_checklist_governanca_permanencia", SqlDbType.BigInt, IIf(codigoChecklistGovernancaPermanencia = -1, DBNull.Value, codigoChecklistGovernancaPermanencia)),
                CriarParametro("codigo_checklist_governanca_saida", SqlDbType.BigInt, IIf(codigoChecklistGovernancaSaida = -1, DBNull.Value, codigoChecklistGovernancaSaida)),
                CriarParametro("codigo_checklist_governanca_manutencao", SqlDbType.BigInt, IIf(codigoChecklistGovernancaManutencao = -1, DBNull.Value, codigoChecklistGovernancaManutencao)),
                CriarParametro("codigo_checklist_governanca_permanencia_vistoria", SqlDbType.BigInt, IIf(codigoChecklistGovernancaPermanenciaVistoria = -1, DBNull.Value, codigoChecklistGovernancaPermanenciaVistoria)),
                CriarParametro("codigo_checklist_governanca_saida_vistoria", SqlDbType.BigInt, IIf(codigoChecklistGovernancaSaidaVistoria = -1, DBNull.Value, codigoChecklistGovernancaSaidaVistoria)),
                CriarParametro("codigo_checklist_governanca_manutencao_vistoria", SqlDbType.BigInt, IIf(codigoChecklistGovernancaManutencaoVistoria = -1, DBNull.Value, codigoChecklistGovernancaManutencaoVistoria)),
                CriarParametro("ativo", SqlDbType.Bit, ativo)
            }

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_cadastro_basico_tipo_apartamento", oSqlParameter.ToArray())

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateTipoApartamento(ByVal codigoEmpresa As Integer,
                                     ByVal codigoUsuario As Integer,
                                     ByVal codigoUnidade As Integer,
                                     ByVal descricao As String,
                                     ByVal codigoChecklistUH As Long,
                                     ByVal codigoPeriodicidadeUH As Integer,
                                     ByVal intervaloUH As Integer,
                                     ByVal codigoChecklistGovernancaPermanencia As Long,
                                     ByVal codigoChecklistGovernancaSaida As Long,
                                     ByVal codigoChecklistGovernancaManutencao As Long,
                                     ByVal codigoChecklistGovernancaPermanenciaVistoria As Long,
                                     ByVal codigoChecklistGovernancaSaidaVistoria As Long,
                                     ByVal codigoChecklistGovernancaManutencaoVistoria As Long,
                                     ByVal ativo As Boolean,
                                     ByVal codigo As Integer)

        Try

            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_usuario", SqlDbType.Int, codigoUsuario),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("descricao", SqlDbType.VarChar, descricao),
                CriarParametro("codigo_checklist_uh", SqlDbType.BigInt, IIf(codigoChecklistUH = -1, DBNull.Value, codigoChecklistUH)),
                CriarParametro("codigo_periodicidade_uh", SqlDbType.Int, IIf(codigoPeriodicidadeUH = -1, DBNull.Value, codigoPeriodicidadeUH)),
                CriarParametro("intervalo_uh", SqlDbType.Int, IIf(intervaloUH = -1, DBNull.Value, intervaloUH)),
                CriarParametro("codigo_checklist_governanca_permanencia", SqlDbType.BigInt, IIf(codigoChecklistGovernancaPermanencia = -1, DBNull.Value, codigoChecklistGovernancaPermanencia)),
                CriarParametro("codigo_checklist_governanca_saida", SqlDbType.BigInt, IIf(codigoChecklistGovernancaSaida = -1, DBNull.Value, codigoChecklistGovernancaSaida)),
                CriarParametro("codigo_checklist_governanca_manutencao", SqlDbType.BigInt, IIf(codigoChecklistGovernancaManutencao = -1, DBNull.Value, codigoChecklistGovernancaManutencao)),
                CriarParametro("codigo_checklist_governanca_permanencia_vistoria", SqlDbType.BigInt, IIf(codigoChecklistGovernancaPermanenciaVistoria = -1, DBNull.Value, codigoChecklistGovernancaPermanenciaVistoria)),
                CriarParametro("codigo_checklist_governanca_saida_vistoria", SqlDbType.BigInt, IIf(codigoChecklistGovernancaSaidaVistoria = -1, DBNull.Value, codigoChecklistGovernancaSaidaVistoria)),
                CriarParametro("codigo_checklist_governanca_manutencao_vistoria", SqlDbType.BigInt, IIf(codigoChecklistGovernancaManutencaoVistoria = -1, DBNull.Value, codigoChecklistGovernancaManutencaoVistoria)),
                CriarParametro("ativo", SqlDbType.Bit, ativo),
                CriarParametro("codigo", SqlDbType.Int, codigo)
            }

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_cadastro_basico_tipo_apartamento", oSqlParameter.ToArray())

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub DeleteTipoApartamento(ByVal codigoEmpresa As Integer,
                                     ByVal codigoUsuario As Integer,
                                     ByVal codigo As Integer)

        Try

            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_usuario", SqlDbType.Int, codigoUsuario),
                CriarParametro("codigo", SqlDbType.Int, codigo)
            }

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_cadastro_basico_tipo_apartamento", oSqlParameter.ToArray())

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InfoTipoApartamento(ByVal codigoEmpresa As Integer,
                                   ByVal codigo As Integer,
                                   ByRef oTipoApartamento As TipoApartamento)

        Try

            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo", SqlDbType.Int, codigo)
            }

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_tipo_apartamento_dados", oSqlParameter)

                While oSqlDataReader.Read

                    oTipoApartamento = New TipoApartamento
                    oTipoApartamento.codigo = SafeGetLong(oSqlDataReader, "codigo")
                    oTipoApartamento.descricao = SafeGetString(oSqlDataReader, "descricao")
                    oTipoApartamento.codigo_unidade = SafeGetLong(oSqlDataReader, "codigo_unidade")
                    oTipoApartamento.unidade = SafeGetString(oSqlDataReader, "unidade")
                    oTipoApartamento.codigo_checklist_uh = SafeGetLong(oSqlDataReader, "codigo_checklist_uh")
                    oTipoApartamento.codigo_periodicidade_uh = SafeGetLong(oSqlDataReader, "codigo_periodicidade_uh")
                    oTipoApartamento.intervalo_uh = SafeGetLong(oSqlDataReader, "intervalo_uh")
                    oTipoApartamento.codigo_checklist_governanca_permanencia = SafeGetLong(oSqlDataReader, "codigo_checklist_governanca_permanencia")
                    oTipoApartamento.codigo_checklist_governanca_saida = SafeGetLong(oSqlDataReader, "codigo_checklist_governanca_saida")
                    oTipoApartamento.codigo_checklist_governanca_manutencao = SafeGetLong(oSqlDataReader, "codigo_checklist_governanca_manutencao")
                    oTipoApartamento.codigo_checklist_governanca_permanencia_vistoria = SafeGetLong(oSqlDataReader, "codigo_checklist_governanca_permanencia_vistoria")
                    oTipoApartamento.codigo_checklist_governanca_saida_vistoria = SafeGetLong(oSqlDataReader, "codigo_checklist_governanca_saida_vistoria")
                    oTipoApartamento.codigo_checklist_governanca_manutencao_vistoria = SafeGetLong(oSqlDataReader, "codigo_checklist_governanca_manutencao_vistoria")
                    oTipoApartamento.ativo = SafeGetBoolean(oSqlDataReader, "ativo")

                End While

            End Using

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function IndexTipoApartamento(ByVal codigoEmpresa As Integer,
                                         ByVal codigoUsuario As Integer,
                                         ByVal codigoUnidade As Integer,
                                         ByVal descricao As String,
                                         ByVal ativo As Integer) As List(Of TipoApartamento)

        Try

            Dim oTipoApartamento As New List(Of TipoApartamento)
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("codigo_usuario", SqlDbType.Int, codigoUsuario),
                CriarParametro("ativo", SqlDbType.Bit, IIf(ativo = -1, DBNull.Value, IIf(ativo = 1, True, False))),
                CriarParametro("descricao", SqlDbType.VarChar, descricao)
            }

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_tipo_apartamento", oSqlParameter.ToArray())

                While oSqlDataReader.Read

                    Dim oTipoApartamentoInfo As New TipoApartamento

                    oTipoApartamentoInfo.codigo = SafeGetLong(oSqlDataReader, "codigo")
                    oTipoApartamentoInfo.descricao = SafeGetString(oSqlDataReader, "descricao")
                    oTipoApartamentoInfo.codigo_unidade = SafeGetLong(oSqlDataReader, "codigo_unidade")
                    oTipoApartamentoInfo.unidade = SafeGetString(oSqlDataReader, "unidade")
                    oTipoApartamentoInfo.checklist_uh = SafeGetString(oSqlDataReader, "checklist_uh")
                    oTipoApartamentoInfo.checklist_governanca_permanencia = SafeGetString(oSqlDataReader, "checklist_governanca_permanencia")
                    oTipoApartamentoInfo.checklist_governanca_saida = SafeGetString(oSqlDataReader, "checklist_governanca_saida")
                    oTipoApartamentoInfo.checklist_governanca_manutencao = SafeGetString(oSqlDataReader, "checklist_governanca_manutencao")
                    oTipoApartamentoInfo.ativo = SafeGetBoolean(oSqlDataReader, "ativo")
                    oTipoApartamentoInfo.texto_ativo = SafeGetBooleanSimNao(oSqlDataReader, "ativo")

                    oTipoApartamento.Add(oTipoApartamentoInfo)

                End While

            End Using

            'Retorno da Função
            Return oTipoApartamento

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ValidaTipoApartamento(ByVal codigoEmpresa As Integer,
                                          ByVal codigoUnidade As Integer,
                                          ByVal descricao As String,
                                          ByVal codigo As Integer) As Boolean

        Try

            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("codigo", SqlDbType.Int, codigo),
                CriarParametro("descricao", SqlDbType.VarChar, descricao)
            }

            'Executa Query
            Dim iReturn As Integer = ExecuteScalar(sConnection, CommandType.StoredProcedure, "sp_validate_cadastro_basico_tipo_apartamento", oSqlParameter.ToArray())

            'Retorno da Função
            Return (IIf(iReturn > 0, False, True))

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: TIPO DE AR CONDICIONADO :::"

    Public Sub InsertTipoArCondicionado(ByVal iCodigoEmpresa As Integer,
                                        ByVal iCodigoUsuario As Integer,
                                        ByVal sTipo As String,
                                        ByVal sDescricao As String,
                                        ByVal iCodigoPeriodicidade As Integer,
                                        ByVal iIntervalo As Integer,
                                        ByVal lCodigoChecklist As Long,
                                        ByVal bAtivo As Boolean)

        'Variaveis Locais
        Dim oSqlParameter(7) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Tipo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "tipo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sTipo : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 200
            oSqlParameter(i).Value = sDescricao : i += 1

            'Seta Parametros - Código Periodicidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_periodicidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoPeriodicidade : i += 1

            'Seta Parametros - Intervalo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "intervalo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iIntervalo = -1, 0, iIntervalo) : i += 1

            'Seta Parametros - Código Checklist
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_checklist"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = IIf(lCodigoChecklist = -1, DBNull.Value, lCodigoChecklist) : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_cadastro_basico_tipo_ar_condicionado", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateTipoArCondicionado(ByVal iCodigoEmpresa As Integer,
                                        ByVal iCodigoUsuario As Integer,
                                        ByVal sTipo As String,
                                        ByVal sDescricao As String,
                                        ByVal iCodigoPeriodicidade As Integer,
                                        ByVal iIntervalo As Integer,
                                        ByVal lCodigoChecklist As Long,
                                        ByVal bAtivo As Boolean,
                                        ByVal iCodigo As Integer)

        'Variaveis Locais
        Dim oSqlParameter(8) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Tipo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "tipo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sTipo : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 200
            oSqlParameter(i).Value = sDescricao : i += 1

            'Seta Parametros - Código Periodicidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_periodicidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoPeriodicidade : i += 1

            'Seta Parametros - Intervalo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "intervalo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iIntervalo = -1, 0, iIntervalo) : i += 1

            'Seta Parametros - Código Checklist
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_checklist"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = IIf(lCodigoChecklist = -1, DBNull.Value, lCodigoChecklist) : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_cadastro_basico_tipo_ar_condicionado", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub DeleteTipoArCondicionado(ByVal iCodigoEmpresa As Integer,
                                        ByVal iCodigoUsuario As Integer,
                                        ByVal iCodigo As Integer)

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

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_cadastro_basico_tipo_ar_condicionado", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InfoTipoArCondicionado(ByVal iCodigoEmpresa As Integer,
                                      ByVal iCodigo As Integer,
                                      ByRef oTipoArCondicionado As TipoArCondicionado)

        'Variaveis Locais
        Dim oSqlParameter(1) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_tipo_ar_condicionado_dados", oSqlParameter)

            While oSqlDataReader.Read

                oTipoArCondicionado = New TipoArCondicionado

                oTipoArCondicionado.codigo = oSqlDataReader.Item("codigo")
                oTipoArCondicionado.tipo = oSqlDataReader.Item("tipo")
                oTipoArCondicionado.descricao = oSqlDataReader.Item("descricao")
                oTipoArCondicionado.codigo_periodicidade = oSqlDataReader.Item("codigo_periodicidade")
                oTipoArCondicionado.periodicidade = oSqlDataReader.Item("periodicidade")
                oTipoArCondicionado.intervalo = oSqlDataReader.Item("intervalo")
                oTipoArCondicionado.codigo_checklist = oSqlDataReader.Item("codigo_checklist")
                oTipoArCondicionado.checklist = oSqlDataReader.Item("checklist")
                oTipoArCondicionado.ativo = oSqlDataReader.Item("ativo")

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function IndexTipoArCondicionado(ByVal iCodigoEmpresa As Integer) As List(Of TipoArCondicionado)

        Try

            'Váriaveis Locais
            Dim oTipoArCondicionado As New List(Of TipoArCondicionado)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(0) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_tipo_ar_condicionado", oSqlParameter)

            While oSqlDataReader.Read

                Dim oTipoArCondicionadoInfo As New TipoArCondicionado

                oTipoArCondicionadoInfo.codigo = oSqlDataReader.Item("codigo")
                oTipoArCondicionadoInfo.tipo = oSqlDataReader.Item("tipo")
                oTipoArCondicionadoInfo.descricao = oSqlDataReader.Item("descricao")
                oTipoArCondicionadoInfo.codigo_periodicidade = oSqlDataReader.Item("codigo_periodicidade")
                oTipoArCondicionadoInfo.periodicidade = oSqlDataReader.Item("periodicidade")
                oTipoArCondicionadoInfo.intervalo = oSqlDataReader.Item("intervalo")
                oTipoArCondicionadoInfo.codigo_checklist = oSqlDataReader.Item("codigo_checklist")
                oTipoArCondicionadoInfo.checklist = oSqlDataReader.Item("checklist")
                oTipoArCondicionadoInfo.ativo = oSqlDataReader.Item("ativo")

                oTipoArCondicionado.Add(oTipoArCondicionadoInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oTipoArCondicionado

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ValidaTipoArCondicionado(ByVal iCodigoEmpresa As Integer,
                                             ByVal sTipo As String,
                                             ByVal iCodigo As Integer) As Boolean

        Try

            'Váriaveis Locais
            Dim oSqlParameter(2) As SqlParameter
            Dim i As Integer = 0
            Dim iReturn As Integer

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo : i += 1

            'Seta Parametros - Tipo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "tipo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sTipo

            'Executa Query
            iReturn = ExecuteScalar(sConnection, CommandType.StoredProcedure, "sp_validate_cadastro_basico_tipo_ar_condicionado", oSqlParameter)

            'Retorno da Função
            Return (IIf(iReturn > 0, False, True))

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: TIPO DE CAMA :::"

    Public Sub InsertTipoCama(ByVal iCodigoEmpresa As Integer,
                              ByVal iCodigoUsuario As Integer,
                              ByVal iCodigoUnidade As Integer,
                              ByVal sDescricao As String,
                              ByVal bAtivo As Boolean)

        'Variaveis Locais
        Dim oSqlParameter(4) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sDescricao : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_cadastro_basico_tipo_cama", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateTipoCama(ByVal iCodigoEmpresa As Integer,
                              ByVal iCodigoUsuario As Integer,
                              ByVal iCodigoUnidade As Integer,
                              ByVal sDescricao As String,
                              ByVal bAtivo As Boolean,
                              ByVal iCodigo As Integer)

        'Variaveis Locais
        Dim oSqlParameter(5) As SqlParameter
        Dim i As Integer = 0

        Try


            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sDescricao : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_cadastro_basico_tipo_cama", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub DeleteTipoCama(ByVal iCodigoEmpresa As Integer,
                              ByVal iCodigoUsuario As Integer,
                              ByVal iCodigo As Integer)

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

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_cadastro_basico_tipo_cama", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InfoTipoCama(ByVal iCodigoEmpresa As Integer,
                            ByVal iCodigo As Integer,
                            ByRef oTipoCama As TipoCama)

        'Variaveis Locais
        Dim oSqlParameter(1) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_tipo_cama_dados", oSqlParameter)

            While oSqlDataReader.Read

                oTipoCama = New TipoCama
                oTipoCama.codigo = oSqlDataReader.Item("codigo")
                oTipoCama.descricao = oSqlDataReader.Item("descricao")
                oTipoCama.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oTipoCama.unidade = oSqlDataReader.Item("unidade")
                oTipoCama.ativo = oSqlDataReader.Item("ativo")

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function IndexTipoCama(ByVal iCodigoEmpresa As Integer,
                                  ByVal iCodigoUsuario As Integer,
                                  ByVal iCodigoUnidade As Integer) As List(Of TipoCama)

        Try

            'Váriaveis Locais
            Dim oTipoCama As New List(Of TipoCama)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(2) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_tipo_cama", oSqlParameter)

            While oSqlDataReader.Read

                Dim oTipoCamaInfo As New TipoCama

                oTipoCamaInfo.codigo = oSqlDataReader.Item("codigo")
                oTipoCamaInfo.descricao = oSqlDataReader.Item("descricao")
                oTipoCamaInfo.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oTipoCamaInfo.unidade = oSqlDataReader.Item("unidade")
                oTipoCamaInfo.ativo = oSqlDataReader.Item("ativo")

                oTipoCama.Add(oTipoCamaInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oTipoCama

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ValidaTipoCama(ByVal iCodigoEmpresa As Integer,
                                   ByVal iCodigoUnidade As Integer,
                                   ByVal sDescricao As String,
                                   ByVal iCodigo As Integer) As Boolean

        Try

            'Váriaveis Locais
            Dim oSqlParameter(3) As SqlParameter
            Dim i As Integer = 0
            Dim iReturn As Integer

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sDescricao

            'Executa Query
            iReturn = ExecuteScalar(sConnection, CommandType.StoredProcedure, "sp_validate_cadastro_basico_tipo_cama", oSqlParameter)

            'Retorno da Função
            Return (IIf(iReturn > 0, False, True))

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: TIPO DE CHECKLIST :::"

    Public Sub InsertTipoChecklist(ByVal iCodigoEmpresa As Integer,
                                   ByVal iCodigoUsuario As Integer,
                                   ByVal iCodigoUnidade As Integer,
                                   ByVal sDescricao As String,
                                   ByVal bAtivo As Boolean,
                                   ByRef iCodigo As Integer)

        'Variaveis Locais
        Dim oSqlParameter(5) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sDescricao : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Output
            oSqlParameter(i).SqlDbType = SqlDbType.Int

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_cadastro_basico_tipo_checklist", oSqlParameter)

            iCodigo = oSqlParameter(i).Value

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InsertTipoChecklistItem(ByVal iCodigoEmpresa As Integer,
                                       ByVal iCodigoTipoChecklist As Integer,
                                       ByVal iCodigoUnidade As Integer,
                                       ByVal iCodigoChecklist As Integer)

        'Variaveis Locais
        Dim oSqlParameter(3) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Tipo Checklist
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_tipo_checklist"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = CInt(iCodigoTipoChecklist) : i += 1

            'Seta Parametros - Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Checklist
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_checklist"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoChecklist

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_cadastro_basico_tipo_checklist_item", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateTipoChecklist(ByVal iCodigoEmpresa As Integer,
                                   ByVal iCodigoUsuario As Integer,
                                   ByVal iCodigoUnidade As Integer,
                                   ByVal sDescricao As String,
                                   ByVal bAtivo As Boolean,
                                   ByVal iCodigo As Integer)

        'Variaveis Locais
        Dim oSqlParameter(5) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sDescricao : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_cadastro_basico_tipo_checklist", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub DeleteTipoChecklist(ByVal iCodigoEmpresa As Integer,
                                   ByVal iCodigoUsuario As Integer,
                                   ByVal iCodigo As Integer)

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

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_cadastro_basico_tipo_checklist", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InfoTipoChecklist(ByVal iCodigoEmpresa As Integer,
                                 ByVal iCodigo As Integer,
                                 ByRef oTipoChecklist As TipoChecklist)

        'Variaveis Locais
        Dim oSqlParameter(1) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_tipo_checklist_dados", oSqlParameter)

            While oSqlDataReader.Read

                oTipoChecklist = New TipoChecklist
                oTipoChecklist.codigo = oSqlDataReader.Item("codigo")
                oTipoChecklist.descricao = oSqlDataReader.Item("descricao")
                oTipoChecklist.ativo = oSqlDataReader.Item("ativo")
                oTipoChecklist.unidade = oSqlDataReader.Item("unidade")
                oTipoChecklist.codigo_unidade = oSqlDataReader.Item("codigo_unidade")

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function IndexTipoChecklist(ByVal iCodigoEmpresa As Integer,
                                       ByVal iCodigoUsuario As Integer,
                                       ByVal iCodigoUnidade As Integer) As List(Of TipoChecklist)

        Try

            'Váriaveis Locais
            Dim oTipoChecklist As New List(Of TipoChecklist)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(2) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_tipo_checklist", oSqlParameter)

            While oSqlDataReader.Read

                Dim oTipoChecklistInfo As New TipoChecklist

                oTipoChecklistInfo.descricao = oSqlDataReader.Item("descricao")
                oTipoChecklistInfo.ativo = oSqlDataReader.Item("ativo")
                oTipoChecklistInfo.codigo = oSqlDataReader.Item("codigo")
                oTipoChecklistInfo.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oTipoChecklistInfo.unidade = oSqlDataReader.Item("unidade")

                oTipoChecklist.Add(oTipoChecklistInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oTipoChecklist

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ValidaTipoChecklist(ByVal iCodigoEmpresa As Integer,
                                        ByVal iCodigoUnidade As Integer,
                                        ByVal sDescricao As String,
                                        ByVal iCodigo As Integer) As Boolean

        Try

            'Váriaveis Locais
            Dim oSqlParameter(3) As SqlParameter
            Dim i As Integer = 0
            Dim iReturn As Integer

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sDescricao

            'Executa Query
            iReturn = ExecuteScalar(sConnection, CommandType.StoredProcedure, "sp_validate_cadastro_basico_tipo_checklist", oSqlParameter)

            'Retorno da Função
            Return (IIf(iReturn > 0, False, True))

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function IndexTipoChecklistItem(ByVal iCodigoEmpresa As Integer,
                                           ByVal iCodigoTipoChecklist As Integer,
                                           ByVal iCodigoUnidade As Integer) As List(Of TipoChecklistItem)

        Try

            'Váriaveis Locais
            Dim oTipoChecklistItem As New List(Of TipoChecklistItem)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(2) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Tipo Checklist
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_tipo_checklist"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoTipoChecklist : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_tipo_checklist_item", oSqlParameter)

            While oSqlDataReader.Read

                Dim oTipoChecklistItemInfo As New TipoChecklistItem

                oTipoChecklistItemInfo.codigo_tipo_checklist = oSqlDataReader.Item("codigo_tipo_checklist")
                oTipoChecklistItemInfo.codigo = oSqlDataReader.Item("codigo")
                oTipoChecklistItemInfo.codigo_checklist = oSqlDataReader.Item("codigo_checklist")
                oTipoChecklistItemInfo.grupo = oSqlDataReader.Item("grupo")
                oTipoChecklistItemInfo.checklist = oSqlDataReader.Item("checklist")
                oTipoChecklistItemInfo.selecionado = oSqlDataReader.Item("selecionado")

                oTipoChecklistItem.Add(oTipoChecklistItemInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oTipoChecklistItem

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: TIPO DE DESPESA :::"

    Public Sub InsertTipoDespesa(ByVal iCodigoEmpresa As Integer,
                                 ByVal iCodigoUsuario As Integer,
                                 ByVal iCodigoUnidade As Integer,
                                 ByVal sCodigoTipoDespesa As String,
                                 ByVal sDescricao As String,
                                 ByVal bAtivo As Boolean)

        'Variaveis Locais
        Dim oSqlParameter(5) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Tipo de Despesa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_tipo_despesa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sCodigoTipoDespesa : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sDescricao : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_cadastro_basico_tipo_despesa", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateTipoDespesa(ByVal iCodigoEmpresa As Integer,
                                 ByVal iCodigoUsuario As Integer,
                                 ByVal iCodigoUnidade As Integer,
                                 ByVal sCodigoTipoDespesa As String,
                                 ByVal sDescricao As String,
                                 ByVal bAtivo As Boolean,
                                 ByVal iCodigo As Integer)

        'Variaveis Locais
        Dim oSqlParameter(6) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Tipo de Despesa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_tipo_despesa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sCodigoTipoDespesa : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sDescricao : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_cadastro_basico_tipo_despesa", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub DeleteTipoDespesa(ByVal iCodigoEmpresa As Integer,
                                 ByVal iCodigoUsuario As Integer,
                                 ByVal iCodigo As Integer)

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

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_cadastro_basico_tipo_despesa", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InfoTipoDespesa(ByVal iCodigoEmpresa As Integer,
                               ByVal iCodigo As Integer,
                               ByRef oTipoDespesa As TipoDespesa)

        'Variaveis Locais
        Dim oSqlParameter(1) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_tipo_despesa_dados", oSqlParameter)

            While oSqlDataReader.Read

                oTipoDespesa = New TipoDespesa
                oTipoDespesa.codigo = oSqlDataReader.Item("codigo")
                oTipoDespesa.codigo_tipo_despesa = oSqlDataReader.Item("codigo_tipo_despesa")
                oTipoDespesa.descricao = oSqlDataReader.Item("descricao")
                oTipoDespesa.ativo = oSqlDataReader.Item("ativo")
                oTipoDespesa.unidade = oSqlDataReader.Item("unidade")
                oTipoDespesa.codigo_unidade = oSqlDataReader.Item("codigo_unidade")

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function IndexTipoDespesa(ByVal iCodigoEmpresa As Integer,
                                     ByVal iCodigoUsuario As Integer,
                                     ByVal iCodigoUnidade As Integer) As List(Of TipoDespesa)

        Try

            'Váriaveis Locais
            Dim oTipoDespesa As New List(Of TipoDespesa)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(2) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_tipo_despesa", oSqlParameter)

            While oSqlDataReader.Read

                Dim oTipoDespesaInfo As New TipoDespesa

                oTipoDespesaInfo.codigo_tipo_despesa = oSqlDataReader.Item("codigo_tipo_despesa")
                oTipoDespesaInfo.descricao = oSqlDataReader.Item("descricao")
                oTipoDespesaInfo.ativo = oSqlDataReader.Item("ativo")
                oTipoDespesaInfo.codigo = oSqlDataReader.Item("codigo")
                oTipoDespesaInfo.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oTipoDespesaInfo.unidade = oSqlDataReader.Item("unidade")

                oTipoDespesa.Add(oTipoDespesaInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da TipoDespesa
            Return oTipoDespesa

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ValidaTipoDespesa(ByVal iCodigoEmpresa As Integer,
                                      ByVal iCodigoUnidade As Integer,
                                      ByVal sCodigoTipoDespesa As String,
                                      ByVal iCodigo As Integer) As Boolean

        Try

            'Váriaveis Locais
            Dim oSqlParameter(3) As SqlParameter
            Dim i As Integer = 0
            Dim iReturn As Integer

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo : i += 1

            'Seta Parametros - Código Tipo Despesa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_tipo_despesa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sCodigoTipoDespesa

            'Executa Query
            iReturn = ExecuteScalar(sConnection, CommandType.StoredProcedure, "sp_validate_cadastro_basico_tipo_despesa", oSqlParameter)

            'Retorno da TipoDespesa
            Return (IIf(iReturn > 0, False, True))

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: TREINAMENTO :::"

    Public Sub InsertTreinamento(ByVal iCodigoEmpresa As Integer,
                                 ByVal iCodigoUsuario As Integer,
                                 ByVal iCodigoUnidade As Integer,
                                 ByVal iCodigoModulo As Integer,
                                 ByVal sDescricao As String,
                                 ByVal sComentario As String,
                                 ByVal sPathArquivo As String,
                                 ByVal sArquivo As String,
                                 ByVal bAtivo As Boolean)

        'Variaveis Locais
        Dim oSqlParameter(8) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoUnidade = -1, DBNull.Value, iCodigoUnidade) : i += 1

            'Seta Parametros - Código Módulo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_modulo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoModulo = -1, DBNull.Value, iCodigoModulo) : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sDescricao : i += 1

            'Seta Parametros - Comentário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "comentario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 5000
            oSqlParameter(i).Value = sComentario : i += 1

            'Seta Parametros - Arquivo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "arquivo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 250
            oSqlParameter(i).Value = sArquivo : i += 1

            'Seta Parametros - Path Arquivo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "path_arquivo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 250
            oSqlParameter(i).Value = sPathArquivo : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_cadastro_basico_treinamento", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateTreinamento(ByVal iCodigoEmpresa As Integer,
                                 ByVal iCodigoUsuario As Integer,
                                 ByVal iCodigoUnidade As Integer,
                                 ByVal iCodigoModulo As Integer,
                                 ByVal sDescricao As String,
                                 ByVal sComentario As String,
                                 ByVal sArquivo As String,
                                 ByVal sPathArquivo As String,
                                 ByVal bAtivo As Boolean,
                                 ByVal iCodigo As Integer)

        'Variaveis Locais
        Dim oSqlParameter(9) As SqlParameter
        Dim i As Integer = 0

        Try


            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoUnidade = -1, DBNull.Value, iCodigoUnidade) : i += 1

            'Seta Parametros - Código Módulo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_modulo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoModulo = -1, DBNull.Value, iCodigoModulo) : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sDescricao : i += 1

            'Seta Parametros - Comentário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "comentario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 5000
            oSqlParameter(i).Value = sComentario : i += 1

            'Seta Parametros - Arquivo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "arquivo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 250
            oSqlParameter(i).Value = sArquivo : i += 1

            'Seta Parametros - Path Arquivo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "path_arquivo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 250
            oSqlParameter(i).Value = sPathArquivo : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_cadastro_basico_treinamento", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub DeleteTreinamento(ByVal iCodigoEmpresa As Integer,
                                 ByVal iCodigoUsuario As Integer,
                                 ByVal iCodigo As Integer)

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

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_cadastro_basico_treinamento", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InfoTreinamento(ByVal iCodigoEmpresa As Integer,
                               ByVal iCodigo As Integer,
                               ByRef oTreinamento As Treinamento)

        'Variaveis Locais
        Dim oSqlParameter(1) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_treinamento_dados", oSqlParameter)

            While oSqlDataReader.Read

                oTreinamento = New Treinamento
                oTreinamento.codigo = oSqlDataReader.Item("codigo")
                oTreinamento.descricao = oSqlDataReader.Item("descricao")
                oTreinamento.comentario = oSqlDataReader.Item("comentario")
                oTreinamento.arquivo = oSqlDataReader.Item("arquivo")
                oTreinamento.path_arquivo = oSqlDataReader.Item("path_arquivo")
                oTreinamento.ativo = oSqlDataReader.Item("ativo")
                oTreinamento.unidade = oSqlDataReader.Item("unidade")
                oTreinamento.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oTreinamento.modulo = oSqlDataReader.Item("modulo")
                oTreinamento.codigo_modulo = oSqlDataReader.Item("codigo_modulo")

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function IndexTreinamento(ByVal iCodigoEmpresa As Integer,
                                     ByVal iCodigoUsuario As Integer,
                                     ByVal iCodigoUnidade As Integer,
                                     ByVal iCodigoModulo As Integer) As List(Of Treinamento)

        Try

            'Váriaveis Locais
            Dim oNormasProcedimentos As New List(Of Treinamento)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(3) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Módulo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_modulo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoModulo = -1, DBNull.Value, iCodigoModulo)

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_treinamento", oSqlParameter)

            While oSqlDataReader.Read

                Dim oNormasProcedimentosInfo As New Treinamento

                oNormasProcedimentosInfo.descricao = oSqlDataReader.Item("descricao")
                oNormasProcedimentosInfo.ativo = oSqlDataReader.Item("ativo")
                oNormasProcedimentosInfo.codigo = oSqlDataReader.Item("codigo")
                oNormasProcedimentosInfo.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oNormasProcedimentosInfo.unidade = oSqlDataReader.Item("unidade")
                oNormasProcedimentosInfo.codigo_modulo = oSqlDataReader.Item("codigo_modulo")
                oNormasProcedimentosInfo.modulo = oSqlDataReader.Item("modulo")
                oNormasProcedimentosInfo.path_arquivo = oSqlDataReader.Item("path_arquivo")
                oNormasProcedimentosInfo.data_cadastro = oSqlDataReader.Item("data_cadastro")
                oNormasProcedimentosInfo.comentario = oSqlDataReader.Item("comentario")

                oNormasProcedimentos.Add(oNormasProcedimentosInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oNormasProcedimentos

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ValidaTreinamento(ByVal iCodigoEmpresa As Integer,
                                      ByVal iCodigoUnidade As Integer,
                                      ByVal iCodigoModulo As Integer,
                                      ByVal sDescricao As String,
                                      ByVal iCodigo As Integer) As Boolean

        Try

            'Váriaveis Locais
            Dim oSqlParameter(4) As SqlParameter
            Dim i As Integer = 0
            Dim iReturn As Integer

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoUnidade = -1, DBNull.Value, iCodigoUnidade) : i += 1

            'Seta Parametros - Código Módulo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_modulo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoModulo = -1, DBNull.Value, iCodigoModulo) : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sDescricao

            'Executa Query
            iReturn = ExecuteScalar(sConnection, CommandType.StoredProcedure, "sp_validate_cadastro_basico_treinamento", oSqlParameter)

            'Retorno da Função
            Return (IIf(iReturn > 0, False, True))

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: UNIDADE :::"

    Public Sub InsertUnidade(ByVal iCodigoEmpresa As Integer,
                             ByVal iCodigoUsuario As Integer,
                             ByVal sNomeFantasia As String,
                             ByVal sRazaoSocial As String,
                             ByVal sCNPJ As String,
                             ByVal sInscricaoEstadual As String,
                             ByVal sInscricaoMunicipal As String,
                             ByVal sCEP As String,
                             ByVal sUF As String,
                             ByVal sMunicipio As String,
                             ByVal sLogradouro As String,
                             ByVal sNumero As String,
                             ByVal sBairro As String,
                             ByVal sComplemento As String,
                             ByVal sTelefone As String,
                             ByVal iQuantidadeBloco As Integer,
                             ByVal iQuantidadeAndar As Integer,
                             ByVal sLogoMin As String,
                             ByVal sLogoMax As String,
                             ByVal bApontaHoras As Boolean,
                             ByVal bApontaHorasQualidade As Boolean,
                             ByVal iQuantidadeMaximaHorasApontamento As Integer,
                             ByVal dAreaTotal As Double,
                             ByVal dAreaTotalConstruida As Double,
                             ByVal iCodigoTipoUnidade As Integer,
                             ByVal bAtivo As Boolean,
                             ByVal sHotelOpera As String,
                             ByRef iCodigoUnidade As Integer)

        'Variaveis Locais
        Dim oSqlParameter(27) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Nome Fantasia
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "nome_fantasia"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 120
            oSqlParameter(i).Value = sNomeFantasia : i += 1

            'Seta Parametros - Razão Social
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "razao_social"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 120
            oSqlParameter(i).Value = sRazaoSocial : i += 1

            'Seta Parametros - CNPJ
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "cnpj"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sCNPJ : i += 1

            'Seta Parametros - Inscrição Estadual
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "inscricao_estadual"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sInscricaoEstadual : i += 1

            'Seta Parametros - Inscrição Municipal
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "inscricao_municipal"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sInscricaoMunicipal : i += 1

            'Seta Parametros - CEP
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "cep"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 10
            oSqlParameter(i).Value = sCEP : i += 1

            'Seta Parametros - UF
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "uf"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 2
            oSqlParameter(i).Value = sUF : i += 1

            'Seta Parametros - Município
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "municipio"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 60
            oSqlParameter(i).Value = sMunicipio : i += 1

            'Seta Parametros - Logradouro
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "logradouro"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sLogradouro : i += 1

            'Seta Parametros - Número
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "numero"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 60
            oSqlParameter(i).Value = sNumero : i += 1

            'Seta Parametros - Bairro
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "bairro"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sBairro : i += 1

            'Seta Parametros - Complemento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "complemento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sComplemento : i += 1

            'Seta Parametros - Telefone
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "telefone"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sTelefone : i += 1

            'Seta Parametros - Quantidade Bloco
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "quantidade_bloco"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iQuantidadeBloco : i += 1

            'Seta Parametros - Quantidade Andar
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "quantidade_andar"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iQuantidadeAndar : i += 1

            'Seta Parametros - Logo Min
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "logo_min"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 250
            oSqlParameter(i).Value = sLogoMin : i += 1

            'Seta Parametros - Logo Max
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "logo_max"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 250
            oSqlParameter(i).Value = sLogoMax : i += 1

            'Seta Parametros - Aponta Horas
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "aponta_horas"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bApontaHoras : i += 1

            'Seta Parametros - Aponta Horas Qualidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "aponta_horas_qualidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bApontaHorasQualidade : i += 1

            'Seta Parametros - Quantidade Máxima - Horas Apontamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "quantidade_maxima_horas_apontamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iQuantidadeMaximaHorasApontamento : i += 1

            'Seta Parametros - Área Total
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "area_total"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = dAreaTotal : i += 1

            'Seta Parametros - Área Total Construída
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "area_total_construida"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = dAreaTotalConstruida : i += 1

            'Seta Parametros - Código Tipo de Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_tipo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoTipoUnidade : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo : i += 1

            'Seta Parametros - Hotel Opera
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "hotel_opera"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sHotelOpera : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Output
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_cadastro_basico_unidade", oSqlParameter)

            iCodigoUnidade = oSqlParameter(i).Value

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateUnidade(ByVal iCodigoEmpresa As Integer,
                             ByVal iCodigoUsuario As Integer,
                             ByVal sNomeFantasia As String,
                             ByVal sRazaoSocial As String,
                             ByVal sCNPJ As String,
                             ByVal sInscricaoEstadual As String,
                             ByVal sInscricaoMunicipal As String,
                             ByVal sCEP As String,
                             ByVal sUF As String,
                             ByVal sMunicipio As String,
                             ByVal sLogradouro As String,
                             ByVal sNumero As String,
                             ByVal sBairro As String,
                             ByVal sComplemento As String,
                             ByVal sTelefone As String,
                             ByVal iQuantidadeBloco As Integer,
                             ByVal iQuantidadeAndar As Integer,
                             ByVal sLogoMin As String,
                             ByVal sLogoMax As String,
                             ByVal bApontaHoras As Boolean,
                             ByVal bApontaHorasQualidade As Boolean,
                             ByVal iQuantidadeMaximaHorasApontamento As Integer,
                             ByVal dAreaTotal As Double,
                             ByVal dAreaTotalConstruida As Double,
                             ByVal iCodigoTipoUnidade As Integer,
                             ByVal bAtivo As Boolean,
                             ByVal sHotelOpera As String,
                             ByVal iCodigo As Integer)

        'Variaveis Locais
        Dim oSqlParameter(27) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo : i += 1

            'Seta Parametros - Nome Fantasia
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "nome_fantasia"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 120
            oSqlParameter(i).Value = sNomeFantasia : i += 1

            'Seta Parametros - Razão Social
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "razao_social"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 120
            oSqlParameter(i).Value = sRazaoSocial : i += 1

            'Seta Parametros - CNPJ
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "cnpj"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sCNPJ : i += 1

            'Seta Parametros - Inscrição Estadual
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "inscricao_estadual"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sInscricaoEstadual : i += 1

            'Seta Parametros - Inscrição Municipal
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "inscricao_municipal"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sInscricaoMunicipal : i += 1

            'Seta Parametros - CEP
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "cep"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 10
            oSqlParameter(i).Value = sCEP : i += 1

            'Seta Parametros - UF
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "uf"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 2
            oSqlParameter(i).Value = sUF : i += 1

            'Seta Parametros - Município
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "municipio"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 60
            oSqlParameter(i).Value = sMunicipio : i += 1

            'Seta Parametros - Logradouro
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "logradouro"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sLogradouro : i += 1

            'Seta Parametros - Número
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "numero"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 60
            oSqlParameter(i).Value = sNumero : i += 1

            'Seta Parametros - Bairro
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "bairro"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sBairro : i += 1

            'Seta Parametros - Complemento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "complemento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sComplemento : i += 1

            'Seta Parametros - Telefone
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "telefone"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sTelefone : i += 1

            'Seta Parametros - Quantidade Bloco
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "quantidade_bloco"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iQuantidadeBloco : i += 1

            'Seta Parametros - Quantidade Andar
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "quantidade_andar"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iQuantidadeAndar : i += 1

            'Seta Parametros - Logo Min
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "logo_min"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 250
            oSqlParameter(i).Value = sLogoMin : i += 1

            'Seta Parametros - Logo Max
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "logo_max"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 250
            oSqlParameter(i).Value = sLogoMax : i += 1

            'Seta Parametros - Aponta Horas
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "aponta_horas"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bApontaHoras : i += 1

            'Seta Parametros - Aponta Horas Qualidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "aponta_horas_qualidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bApontaHorasQualidade : i += 1

            'Seta Parametros - Quantidade Máxima - Horas Apontamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "quantidade_maxima_horas_apontamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iQuantidadeMaximaHorasApontamento : i += 1

            'Seta Parametros - Área Total
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "area_total"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = dAreaTotal : i += 1

            'Seta Parametros - Área Total Construída
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "area_total_construida"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = dAreaTotalConstruida : i += 1

            'Seta Parametros - Código Tipo de Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_tipo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoTipoUnidade : i += 1

            'Seta Parametros - Hotel Opera
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "hotel_opera"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sHotelOpera : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_cadastro_basico_unidade", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub DeleteUnidade(ByVal iCodigoEmpresa As Integer,
                             ByVal iCodigoUsuario As Integer,
                             ByVal iCodigo As Integer)

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

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_cadastro_basico_unidade", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InfoUnidade(ByVal iCodigoEmpresa As Integer,
                           ByVal iCodigo As Integer,
                           ByRef oUnidade As Unidade)

        'Variaveis Locais
        Dim oSqlParameter(1) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_unidade_dados", oSqlParameter)

            While oSqlDataReader.Read

                oUnidade = New Unidade
                oUnidade.codigo = oSqlDataReader.Item("codigo")
                oUnidade.nome_fantasia = oSqlDataReader.Item("nome_fantasia")
                oUnidade.razao_social = oSqlDataReader.Item("razao_social")
                oUnidade.cnpj = oSqlDataReader.Item("cnpj")
                oUnidade.inscricao_estadual = oSqlDataReader.Item("inscricao_estadual")
                oUnidade.inscricao_municipal = oSqlDataReader.Item("inscricao_municipal")
                oUnidade.cep = oSqlDataReader.Item("cep")
                oUnidade.uf = oSqlDataReader.Item("uf")
                oUnidade.municipio = oSqlDataReader.Item("municipio")
                oUnidade.logradouro = oSqlDataReader.Item("logradouro")
                oUnidade.numero = oSqlDataReader.Item("numero")
                oUnidade.bairro = oSqlDataReader.Item("bairro")
                oUnidade.complemento = oSqlDataReader.Item("complemento")
                oUnidade.telefone = oSqlDataReader.Item("telefone")
                oUnidade.quantidade_bloco = oSqlDataReader.Item("quantidade_bloco")
                oUnidade.quantidade_andar = oSqlDataReader.Item("quantidade_andar")
                oUnidade.imagem = oSqlDataReader.Item("imagem")
                oUnidade.arquivo = oSqlDataReader.Item("arquivo")
                oUnidade.aponta_horas = oSqlDataReader.Item("aponta_horas")
                oUnidade.aponta_horas_qualidade = oSqlDataReader.Item("aponta_horas_qualidade")
                oUnidade.quantidae_maxima_horas_apontamento = oSqlDataReader.Item("quantidade_maxima_horas_apontamento")
                oUnidade.area_total = oSqlDataReader.Item("area_total")
                oUnidade.area_total_construida = oSqlDataReader.Item("area_total_construida")
                oUnidade.codigo_tipo_unidade = oSqlDataReader.Item("codigo_tipo_unidade")
                oUnidade.ativo = oSqlDataReader.Item("ativo")
                oUnidade.hotel_opera = oSqlDataReader.Item("hotel_opera")

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function IndexUnidade(ByVal iCodigoEmpresa As Integer,
                                 ByVal iCodigoUsuario As Integer,
                                 ByVal iCodigoUnidade As Integer) As List(Of Unidade)

        Try

            'Váriaveis Locais
            Dim oUnidade As New List(Of Unidade)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(2) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_unidade", oSqlParameter)

            While oSqlDataReader.Read

                Dim oUnidadeInfo As New Unidade

                oUnidadeInfo.cnpj = oSqlDataReader.Item("cnpj")
                oUnidadeInfo.nome_fantasia = oSqlDataReader.Item("nome_fantasia")
                oUnidadeInfo.razao_social = oSqlDataReader.Item("razao_social")
                oUnidadeInfo.uf = oSqlDataReader.Item("uf")
                oUnidadeInfo.municipio = oSqlDataReader.Item("municipio")
                oUnidadeInfo.ativo = oSqlDataReader.Item("ativo")
                oUnidadeInfo.codigo = oSqlDataReader.Item("codigo")

                oUnidade.Add(oUnidadeInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oUnidade

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ValidaUnidade(ByVal iCodigoEmpresa As Integer,
                                  ByVal iCodigo As Integer,
                                  ByVal sCNPJ As String) As Boolean

        Try

            'Váriaveis Locais
            Dim oSqlParameter(2) As SqlParameter
            Dim i As Integer = 0
            Dim iReturn As Integer

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo : i += 1

            'Seta Parametros - CNPJ
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "cnpj"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sCNPJ

            'Executa Query
            iReturn = ExecuteScalar(sConnection, CommandType.StoredProcedure, "sp_validate_cadastro_basico_unidade", oSqlParameter)

            'Retorno da Função
            Return (IIf(iReturn > 0, False, True))

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function LoadTipoUnidade(ByVal iCodigoEmpresa As Integer,
                                    ByVal iCodigoUnidade As Integer) As Integer

        Try

            'Váriaveis Locais
            Dim oSqlParameter(1) As SqlParameter
            Dim i As Integer = 0
            Dim iReturn As Integer

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            iReturn = ExecuteScalar(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_unidade_tipo_unidade", oSqlParameter)

            'Retorno da Função
            Return iReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: RELATÓRIO - ITENS AUDITÁVEIS :::"

    Public Sub InsertRelatorioItensAuditaveis(ByVal iCodigoEmpresa As Integer,
                                              ByVal iCodigoUsuario As Integer,
                                              ByVal iCodigoUnidade As Integer,
                                              ByVal iCodigoModulo As Integer,
                                              ByVal sDescricao As String,
                                              ByVal bAtivo As Boolean,
                                              ByRef lCodigo As Long)

        'Variaveis Locais
        Dim oSqlParameter(6) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Modulo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_modulo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoModulo : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 200
            oSqlParameter(i).Value = sDescricao.ToUpper : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Output
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_cadastro_basico_relatorio_itens_auditaveis", oSqlParameter)

            lCodigo = oSqlParameter(i).Value

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateRelatorioItensAuditaveis(ByVal iCodigoEmpresa As Integer,
                                              ByVal iCodigoUsuario As Integer,
                                              ByVal iCodigoUnidade As Integer,
                                              ByVal sDescricao As String,
                                              ByVal bAtivo As Boolean,
                                              ByVal lCodigo As Long)

        'Variaveis Locais
        Dim oSqlParameter(4) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 200
            oSqlParameter(i).Value = sDescricao.ToUpper : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_cadastro_basico_relatorio_itens_auditaveis", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InsertRelatorioItensAuditaveisChecklist(ByVal lCodigoRelatorioItensAuditaveis As Long,
                                                       ByVal iCodigoEmpresa As Integer,
                                                       ByVal lCodigoChecklist As Long,
                                                       ByVal iCodigoChecklistItem As Integer)

        'Variaveis Locais
        Dim oSqlParameter(3) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Relatório Itens Auditáveis
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_relatorio_itens_auditaveis"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoRelatorioItensAuditaveis : i += 1

            'Seta Parametros - Código Checklist
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_checklist"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoChecklist : i += 1

            'Seta Parametros - Código Checklist Item
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_checklist_item"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoChecklistItem

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_cadastro_basico_relatorio_itens_auditaveis_checklist", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub DeleteRelatorioItensAuditaveis(ByVal lCodigo As Long,
                                              ByVal iCodigoUsuario As Integer)

        'Variaveis Locais
        Dim oSqlParameter(1) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_cadastro_basico_relatorio_itens_auditaveis", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InfoRelatorioItensAuditaveis(ByVal lCodigo As Long,
                                            ByRef oRelatorioItensAuditaveis As RelatorioItensAuditaveis)

        'Variaveis Locais
        Dim oSqlParameter(0) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_relatorio_itens_auditaveis_dados", oSqlParameter)

            While oSqlDataReader.Read

                oRelatorioItensAuditaveis = New RelatorioItensAuditaveis
                oRelatorioItensAuditaveis.codigo = oSqlDataReader("codigo")
                oRelatorioItensAuditaveis.codigo_unidade = oSqlDataReader("codigo_unidade")
                oRelatorioItensAuditaveis.unidade = oSqlDataReader("unidade")
                oRelatorioItensAuditaveis.descricao = oSqlDataReader("descricao")
                oRelatorioItensAuditaveis.ativo = oSqlDataReader.Item("ativo")

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function IndexRelatorioItensAuditaveis(ByVal iCodigoEmpresa As Integer,
                                                  ByVal iCodigoUnidade As Integer,
                                                  ByVal iCodigoModulo As Integer,
                                                  ByVal sDescricao As String,
                                                  ByVal iAtivo As Integer) As List(Of RelatorioItensAuditaveis)

        Try

            'Váriaveis Locais
            Dim oRelatorioItensAuditaveis As New List(Of RelatorioItensAuditaveis)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(4) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Modulo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_modulo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoModulo : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 200
            oSqlParameter(i).Value = sDescricao : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = IIf(iAtivo = 1, True, IIf(iAtivo = 0, False, DBNull.Value))

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_relatorio_itens_auditaveis", oSqlParameter)

            While oSqlDataReader.Read

                Dim oRelatorioItensAuditaveisInfo As New RelatorioItensAuditaveis

                oRelatorioItensAuditaveisInfo.codigo = oSqlDataReader.Item("codigo")
                oRelatorioItensAuditaveisInfo.unidade = oSqlDataReader.Item("unidade")
                oRelatorioItensAuditaveisInfo.descricao = oSqlDataReader.Item("descricao")
                oRelatorioItensAuditaveisInfo.ativo = oSqlDataReader.Item("ativo")

                oRelatorioItensAuditaveis.Add(oRelatorioItensAuditaveisInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oRelatorioItensAuditaveis

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function IndexRelatorioItensAuditaveisChecklist(ByVal lCodigoRelatorioItensAuditaveis As Long) As List(Of RelatorioItensAuditaveisDetais)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of RelatorioItensAuditaveisDetais)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(0) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código Relatório Itens Auditaveis
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_relatorio_itens_auditaveis"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoRelatorioItensAuditaveis

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_relatorio_itens_auditaveis_checklist_lista", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New RelatorioItensAuditaveisDetais

                oInfo.codigo_checklist = oSqlDataReader.Item("codigo_checklist")
                oInfo.codigo = oSqlDataReader.Item("codigo")
                oInfo.descricao = oSqlDataReader.Item("descricao")
                oInfo.selecionado = oSqlDataReader.Item("selecionado")

                oReturn.Add(oInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function IndexRelatorioItensAuditaveisChecklist(ByVal iCodigoEmpresa As Integer,
                                                           ByVal iCodigoUnidade As Integer,
                                                           ByVal lCodigoRelatorioItensAuditaveis As Long) As List(Of RelatorioItensAuditaveisDetais)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of RelatorioItensAuditaveisDetais)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(2) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Relatório Itens Auditaveis
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_relatorio_itens_auditaveis"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoRelatorioItensAuditaveis

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_relatorio_itens_auditaveis_checklist", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New RelatorioItensAuditaveisDetais

                oInfo.codigo_checklist = oSqlDataReader.Item("codigo_checklist")
                oInfo.codigo = oSqlDataReader.Item("codigo")
                oInfo.descricao = oSqlDataReader.Item("descricao")
                oInfo.selecionado = oSqlDataReader.Item("selecionado")

                oReturn.Add(oInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ValidaRelatorioItensAuditaveis(ByVal iCodigoEmpresa As Integer,
                                                   ByVal iCodigoUnidade As Integer,
                                                   ByVal sDescricao As String,
                                                   ByVal lCodigo As Long) As Boolean

        Try

            'Váriaveis Locais
            Dim oSqlParameter(3) As SqlParameter
            Dim i As Integer = 0
            Dim iReturn As Integer

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigo : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 200
            oSqlParameter(i).Value = sDescricao

            'Executa Query
            iReturn = ExecuteScalar(sConnection, CommandType.StoredProcedure, "sp_validate_cadastro_basico_relatorio_itens_auditaveis", oSqlParameter)

            'Retorno da Função
            Return (IIf(iReturn > 0, False, True))

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: ITEM - OS HOSPEDE :::"

    Public Sub InsertItemOSHospede(ByVal codigoEmpresa As Integer,
                                   ByVal codigoUsuario As Integer,
                                   ByVal codigoUnidade As Integer,
                                   ByVal descricao As String,
                                   ByVal ativo As Boolean)

        'Variaveis Locais
        Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_usuario", SqlDbType.Int, codigoUsuario),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("descricao", SqlDbType.VarChar, descricao),
                CriarParametro("ativo", SqlDbType.Bit, ativo)
            }

        Try

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_cadastro_basico_item_os_hospede", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateItemOSHospede(ByVal codigoEmpresa As Integer,
                                   ByVal codigoUsuario As Integer,
                                   ByVal codigoUnidade As Integer,
                                   ByVal descricao As String,
                                   ByVal ativo As Boolean,
                                   ByVal codigo As Integer)

        'Variaveis Locais
        Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_usuario", SqlDbType.Int, codigoUsuario),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("descricao", SqlDbType.VarChar, descricao),
                CriarParametro("ativo", SqlDbType.Bit, ativo),
                CriarParametro("codigo", SqlDbType.Int, codigo)
            }

        Try

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_cadastro_basico_item_os_hospede", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub DeleteItemOSHospede(ByVal codigoEmpresa As Integer,
                                   ByVal codigoUsuario As Integer,
                                   ByVal codigo As Integer)

        'Variaveis Locais
        Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_usuario", SqlDbType.Int, codigoUsuario),
                CriarParametro("codigo", SqlDbType.Int, codigo)
            }

        Try

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_cadastro_basico_item_os_hospede", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function InfoItemOSHospede(ByVal codigoEmpresa As Integer,
                                      ByVal codigo As Integer) As ItemOSHospede

        'Variaveis Locais
        Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo", SqlDbType.Int, codigo)
            }
        Dim oReturn As New ItemOSHospede

        Try

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_item_os_hospede_dados", oSqlParameter)

                While oSqlDataReader.Read

                    oReturn = New ItemOSHospede
                    oReturn.codigo = oSqlDataReader.Item("codigo")
                    oReturn.descricao = oSqlDataReader.Item("descricao")
                    oReturn.ativo = oSqlDataReader.Item("ativo")
                    oReturn.unidade = oSqlDataReader.Item("unidade")
                    oReturn.codigoUnidade = oSqlDataReader.Item("codigo_unidade")

                End While

            End Using

            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function IndexItemOSHospede(ByVal codigoEmpresa As Integer,
                                       ByVal codigoUnidade As Integer,
                                       ByVal descricao As String) As List(Of ItemOSHospede)

        Try

            'Váriavies Locais
            Dim oReturn As New List(Of ItemOSHospede)
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("descricao", SqlDbType.VarChar, descricao)
            }

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_item_os_hospede", oSqlParameter)

                While oSqlDataReader.Read

                    Dim oItemOSHospedeInfo As New ItemOSHospede

                    oItemOSHospedeInfo.descricao = oSqlDataReader.Item("descricao")
                    oItemOSHospedeInfo.ativo = oSqlDataReader.Item("ativo")
                    oItemOSHospedeInfo.codigo = oSqlDataReader.Item("codigo")
                    oItemOSHospedeInfo.codigoUnidade = oSqlDataReader.Item("codigo_unidade")
                    oItemOSHospedeInfo.unidade = oSqlDataReader.Item("unidade")

                    oReturn.Add(oItemOSHospedeInfo)

                End While

            End Using

            'Retorno da Função
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ValidaItemOSHospede(ByVal codigoEmpresa As Integer,
                                        ByVal codigoUnidade As Integer,
                                        ByVal descricao As String,
                                        ByVal codigo As Integer) As Boolean

        Try

            'Váriavies Locais
            Dim iReturn As Integer
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("descricao", SqlDbType.VarChar, descricao),
                CriarParametro("codigo", SqlDbType.Int, codigo)
            }

            'Executa Query
            iReturn = ExecuteScalar(sConnection, CommandType.StoredProcedure, "sp_validate_cadastro_basico_item_os_hospede", oSqlParameter)

            'Retorno da Função
            Return (IIf(iReturn > 0, False, True))

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

End Class
