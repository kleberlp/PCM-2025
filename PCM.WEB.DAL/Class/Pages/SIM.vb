Imports PCM.WEB.MODELS
Imports System.DirectoryServices
Imports System.DirectoryServices.ActiveDirectory
Imports PCM.WEB.DAL.SQLHelper
Imports System.Data.SqlClient
Imports System.Text
Imports System.Net
Imports System.IO
Imports System.Data.OleDb

Public Class SIM

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

#Region "::: EMPRESA :::"

    Public Sub InsertEmpresa(ByVal iCodigoTipoEmpresa As Integer,
                             ByVal sNomeFantasia As String,
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
                             ByVal dValor As Double,
                             ByVal sDataInicio As String,
                             ByVal sURL As String,
                             ByVal sEmail As String,
                             ByVal sSenha As String,
                             ByVal sLogoMin As String,
                             ByVal sLogoMax As String,
                             ByVal bAtivo As Boolean,
                             ByVal bAuditoria As Boolean,
                             ByVal bAEB As Boolean,
                             ByVal bEstoque As Boolean,
                             ByVal bFinancas As Boolean,
                             ByVal bGovernanca As Boolean,
                             ByVal bGreenPlanet As Boolean,
                             ByVal bLogBook As Boolean,
                             ByVal bLaudo As Boolean,
                             ByVal bOrdemServico As Boolean,
                             ByVal bPreventiva As Boolean,
                             ByVal bRotina As Boolean,
                             ByVal bPMOC As Boolean,
                             ByVal bUH As Boolean)

        'Variaveis Locais
        Dim oSqlParameter(33) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Tipo Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_tipo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoTipoEmpresa : i += 1

            'Seta Parametros - Nome Fantasia
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "nome_fantasia"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 120
            oSqlParameter(i).Value = sNomeFantasia : i += 1

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

            'Seta Parametros - Valor
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "valor"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = dValor : i += 1

            'Seta Parametros - Data Início
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_inicio"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataInicio : i += 1

            'Seta Parametros - URL
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "url"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 150
            oSqlParameter(i).Value = sURL : i += 1

            'Seta Parametros - E-mail
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "email"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 150
            oSqlParameter(i).Value = sEmail : i += 1

            'Seta Parametros - Senha
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "senha"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = Cripitografar(sSenha.ToUpper) : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo : i += 1

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

            'Seta Parametros - Auditoria
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "auditoria"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAuditoria : i += 1

            'Seta Parametros - AEB
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "aeb"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAEB : i += 1

            'Seta Parametros - Estoque
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "estoque"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bEstoque : i += 1

            'Seta Parametros - Finanças
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "financas"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bFinancas : i += 1

            'Seta Parametros - Governança
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "governanca"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bGovernanca : i += 1

            'Seta Parametros - Green Planet
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "green_planet"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bGreenPlanet : i += 1

            'Seta Parametros - Log Book
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "log_book"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bLogBook : i += 1

            'Seta Parametros - Laudo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "pcm_laudo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bLaudo : i += 1

            'Seta Parametros - Ordem Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "pcm_ordem_servico"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bOrdemServico : i += 1

            'Seta Parametros - Preventiva
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "pcm_preventiva"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bPreventiva : i += 1

            'Seta Parametros - Rotina
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "pcm_rotina"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bRotina : i += 1

            'Seta Parametros - PMOC
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "pmoc"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bPMOC : i += 1

            'Seta Parametros - UH
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "uh"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bUH

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_administracao_insert_cadastro_basico_empresa", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateEmpresa(ByVal iCodigoTipoEmpresa As Integer,
                             ByVal sNomeFantasia As String,
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
                             ByVal dValor As Double,
                             ByVal sDataInicio As String,
                             ByVal bAtivo As Boolean,
                             ByVal sLogoMin As String,
                             ByVal sLogoMax As String,
                             ByVal iCodigo As Integer)

        'Variaveis Locais
        Dim oSqlParameter(18) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Tipo Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_tipo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoTipoEmpresa : i += 1

            'Seta Parametros - Nome Fantasia
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "nome_fantasia"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 120
            oSqlParameter(i).Value = sNomeFantasia : i += 1

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

            'Seta Parametros - Valor
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "valor"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = dValor : i += 1

            'Seta Parametros - Data Início
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_inicio"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataInicio : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo : i += 1

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

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_administracao_update_cadastro_basico_empresa", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub DeleteEmpresa(ByVal iCodigoUsuario As Integer,
                             ByVal iCodigo As Integer)

        'Variaveis Locais
        Dim oSqlParameter(0) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_administracao_delete_cadastro_basico_empresa", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InfoEmpresa(ByVal iCodigo As Integer,
                           ByRef oEmpresa As Empresa)

        'Variaveis Locais
        Dim oSqlParameter(0) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_administracao_select_cadastro_basico_empresa_dados", oSqlParameter)

            While oSqlDataReader.Read

                oEmpresa = New Empresa
                oEmpresa.codigo = oSqlDataReader.Item("codigo")
                oEmpresa.codigo_tipo_empresa = oSqlDataReader.Item("codigo_tipo_empresa")
                oEmpresa.tipo = oSqlDataReader.Item("tipo")
                oEmpresa.cnpj = oSqlDataReader.Item("cnpj")
                oEmpresa.nome_fantasia = oSqlDataReader.Item("nome_fantasia")
                oEmpresa.inscricao_estadual = oSqlDataReader.Item("inscricao_estadual")
                oEmpresa.inscricao_municipal = oSqlDataReader.Item("inscricao_municipal")
                oEmpresa.cep = oSqlDataReader.Item("cep")
                oEmpresa.uf = oSqlDataReader.Item("uf")
                oEmpresa.municipio = oSqlDataReader.Item("municipio")
                oEmpresa.logradouro = oSqlDataReader.Item("logradouro")
                oEmpresa.numero = oSqlDataReader.Item("numero")
                oEmpresa.bairro = oSqlDataReader.Item("bairro")
                oEmpresa.complemento = oSqlDataReader.Item("complemento")
                oEmpresa.telefone = oSqlDataReader.Item("telefone")
                oEmpresa.valor = oSqlDataReader.Item("valor")
                oEmpresa.data_inicio = oSqlDataReader.Item("data_inicio")
                oEmpresa.ativo = oSqlDataReader.Item("ativo")
                oEmpresa.url = oSqlDataReader.Item("url")

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function IndexEmpresa(ByVal iCodigoEmpresa As Integer) As List(Of Empresa)

        Try

            'Váriaveis Locais
            Dim oEmpresa As New List(Of Empresa)
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
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_administracao_select_cadastro_basico_empresa", oSqlParameter)

            While oSqlDataReader.Read

                Dim oEmpresaInfo As New Empresa

                oEmpresaInfo.codigo = oSqlDataReader.Item("codigo")
                oEmpresaInfo.tipo = oSqlDataReader.Item("tipo")
                oEmpresaInfo.cnpj = oSqlDataReader.Item("cnpj")
                oEmpresaInfo.nome_fantasia = oSqlDataReader.Item("nome_fantasia")
                oEmpresaInfo.uf = oSqlDataReader.Item("uf")
                oEmpresaInfo.municipio = oSqlDataReader.Item("municipio")
                oEmpresaInfo.valor = oSqlDataReader.Item("valor")
                oEmpresaInfo.ativo = oSqlDataReader.Item("ativo")

                oEmpresa.Add(oEmpresaInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oEmpresa

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ValidaEmpresa(ByVal iCodigo As Integer,
                                  ByVal sCNPJ As String) As Boolean

        Try

            'Váriaveis Locais
            Dim oSqlParameter(1) As SqlParameter
            Dim i As Integer = 0
            Dim iReturn As Integer

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
            iReturn = ExecuteScalar(sConnection, CommandType.StoredProcedure, "sp_validate_cadastro_basico_empresa", oSqlParameter)

            'Retorno da Função
            Return (IIf(iReturn > 0, False, True))

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: HOME :::"

    Public Function ResumoEmpresa(ByVal iPeriodo As Integer) As List(Of ResumoEmpresa)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of ResumoEmpresa)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(0) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Período
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "periodo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iPeriodo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_administracao_select_resumo_empresa", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New ResumoEmpresa

                oInfo.empresa = oSqlDataReader.Item("empresa")
                oInfo.unidade = oSqlDataReader.Item("unidade")
                oInfo.quantidade_unidade = oSqlDataReader.Item("quantidade_unidade")
                oInfo.numero_os_aberto = oSqlDataReader.Item("numero_os_aberto")
                oInfo.numero_os_concluido = oSqlDataReader.Item("numero_os_concluido")
                oInfo.saldo = oSqlDataReader.Item("saldo")
                oInfo.horas_corretivas = oSqlDataReader.Item("horas_corretivas")
                oInfo.horas_preventivas = oSqlDataReader.Item("horas_preventivas")
                oInfo.horas_pmoc = oSqlDataReader.Item("horas_pmoc")
                oInfo.horas_uh = oSqlDataReader.Item("horas_uh")

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

    Public Function ChartFaturamentoSegmento() As List(Of Chart)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of Chart)
            Dim oInfo As New Chart
            Dim oSqlDataReader As SqlDataReader

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_administracao_chart_faturamento_segmento")

            While oSqlDataReader.Read

                oInfo = New Chart

                oInfo.descricao = oSqlDataReader.Item("descricao")
                oInfo.valor = oSqlDataReader.Item("valor")

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

    Public Function ChartEvolucaoFaturamento() As List(Of Chart)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of Chart)
            Dim oInfo As New Chart
            Dim oSqlDataReader As SqlDataReader

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_administracao_chart_evolucao_faturamento")

            While oSqlDataReader.Read

                oInfo = New Chart

                oInfo.descricao = oSqlDataReader.Item("descricao")
                oInfo.valor = oSqlDataReader.Item("valor")

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

End Class
