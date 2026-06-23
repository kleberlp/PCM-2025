Imports PCM.WEB.MODELS
Imports System.DirectoryServices
Imports System.DirectoryServices.ActiveDirectory
Imports PCM.WEB.DAL.SQLHelper
Imports System.Data.SqlClient
Imports System.Text

Public Class Home

    Private sConnection As String

    Sub New(ByVal sCon As String)
        sConnection = sCon
    End Sub

    Public Function LoadLinkIOS() As String

        Try

            'Executa Query
            Return ExecuteScalar(sConnection, CommandType.StoredProcedure, "sp_select_ios_link")

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Sub DadosOrdemServico(ByVal iCodigoEmpresa As Integer,
                                 ByVal iCodigoUnidade As Integer,
                                 ByVal iCodigoModulo As Integer,
                                 ByVal bQualidade As Boolean,
                                 ByRef oInfoOrdemServico As InfoOrdemServico)

        'Variaveis Locais
        Dim oSqlParameter(3) As SqlParameter
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

            'Seta Parametros - Código Módulo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_modulo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoModulo : i += 1

            'Seta Parametros - Qualidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "qualidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bQualidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pcm_ordem_servico_home", oSqlParameter)

            While oSqlDataReader.Read

                oInfoOrdemServico = New InfoOrdemServico

                oInfoOrdemServico.unidade = oSqlDataReader.Item("unidade")
                oInfoOrdemServico.quantidade_pendente_condo = oSqlDataReader.Item("quantidade_pendente_condo")
                oInfoOrdemServico.quantidade_concluida_condo = oSqlDataReader.Item("quantidade_concluida_condo")
                oInfoOrdemServico.quantidade_andamento_condo = oSqlDataReader.Item("quantidade_andamento_condo")
                oInfoOrdemServico.quantidade_vinculada_condo = oSqlDataReader.Item("quantidade_vinculada_condo")
                oInfoOrdemServico.quantidade_pendente_pool = oSqlDataReader.Item("quantidade_pendente_pool")
                oInfoOrdemServico.quantidade_concluida_pool = oSqlDataReader.Item("quantidade_concluida_pool")
                oInfoOrdemServico.quantidade_andamento_pool = oSqlDataReader.Item("quantidade_andamento_pool")
                oInfoOrdemServico.quantidade_vinculada_pool = oSqlDataReader.Item("quantidade_vinculada_pool")
                oInfoOrdemServico.nota_auditoria_externa = oSqlDataReader.Item("nota_auditoria_externa")
                oInfoOrdemServico.nota_avaliacao_mensal = oSqlDataReader.Item("nota_avaliacao_mensal")
                oInfoOrdemServico.ranking = oSqlDataReader.Item("ranking")
                oInfoOrdemServico.arquivo_auditoria_externa = oSqlDataReader.Item("arquivo_auditoria_externa")
                oInfoOrdemServico.log_book = oSqlDataReader.Item("log_book")

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function InfoOrdemServico(ByVal iCodigoEmpresa As Integer,
                                     ByVal iCodigoUnidade As Integer,
                                     ByVal iCodigoModulo As Integer,
                                     ByVal bQualidade As Boolean) As InfoOrdemServicoNew

        'Variaveis Locais
        Dim oSqlParameter(3) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New InfoOrdemServicoNew
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

            'Seta Parametros - Código Módulo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_modulo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoModulo : i += 1

            'Seta Parametros - Qualidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "qualidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bQualidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pcm_ordem_servico_info", oSqlParameter)

            While oSqlDataReader.Read

                oReturn = New InfoOrdemServicoNew

                oReturn.unidade = oSqlDataReader.Item("unidade")
                oReturn.quantidade_atrasada_condo = SafeGetLong(oSqlDataReader, "quantidade_atrasada_condo")
                oReturn.quantidade_pendente_condo = SafeGetLong(oSqlDataReader, "quantidade_pendente_condo")
                oReturn.quantidade_concluida_condo = SafeGetLong(oSqlDataReader, "quantidade_concluida_condo")
                oReturn.quantidade_andamento_condo = SafeGetLong(oSqlDataReader, "quantidade_andamento_condo")
                oReturn.quantidade_vinculada_condo = SafeGetLong(oSqlDataReader, "quantidade_vinculada_condo")
                oReturn.quantidade_backlog_condo = SafeGetLong(oSqlDataReader, "quantidade_backlog_condo")
                oReturn.quantidade_atrasada_pool = SafeGetLong(oSqlDataReader, "quantidade_atrasada_pool")
                oReturn.quantidade_pendente_pool = SafeGetLong(oSqlDataReader, "quantidade_pendente_pool")
                oReturn.quantidade_concluida_pool = SafeGetLong(oSqlDataReader, "quantidade_concluida_pool")
                oReturn.quantidade_andamento_pool = SafeGetLong(oSqlDataReader, "quantidade_andamento_pool")
                oReturn.quantidade_vinculada_pool = SafeGetLong(oSqlDataReader, "quantidade_vinculada_pool")
                oReturn.quantidade_backlog_pool = SafeGetLong(oSqlDataReader, "quantidade_backlog_pool")
                oReturn.nota_auditoria_externa = SafeGetLong(oSqlDataReader, "nota_auditoria_externa")
                oReturn.nota_avaliacao_mensal = SafeGetLong(oSqlDataReader, "nota_avaliacao_mensal")
                oReturn.ranking = oSqlDataReader.Item("ranking")
                oReturn.arquivo_auditoria_externa = oSqlDataReader.Item("arquivo_auditoria_externa")
                oReturn.log_book = oSqlDataReader.Item("log_book")

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

    Public Sub DadosUnidade(ByVal iCodigoEmpresa As Integer,
                            ByVal iCodigoUnidade As Integer,
                            ByRef iCodigoEmpresaPMOC As Integer,
                            ByRef iCodigoUnidadePMOC As Integer,
                            ByRef iCodigoTipoUnidade As Integer,
                            ByRef sHotelOpera As String)

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

            'Seta Parametros - Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_unidade_dados_pmoc", oSqlParameter)

            While oSqlDataReader.Read

                iCodigoEmpresaPMOC = oSqlDataReader.Item("codigo_empresa_pmoc")
                iCodigoUnidadePMOC = oSqlDataReader.Item("codigo_unidade_pmoc")
                iCodigoTipoUnidade = oSqlDataReader.Item("codigo_tipo_unidade")
                sHotelOpera = oSqlDataReader.Item("hotel_opera")

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function DadosOrdemServico(ByVal iCodigoEmpresa As Integer,
                                      ByVal iCodigoUnidade As Integer) As InfoOrdemServico

        'Variaveis Locais
        Dim oSqlParameter(1) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As InfoOrdemServico
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
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pcm_ordem_servico_home", oSqlParameter)

            While oSqlDataReader.Read

                oReturn = New InfoOrdemServico

                oReturn.quantidade_pendente_condo = oSqlDataReader.Item("quantidade_pendente_condo")
                oReturn.quantidade_concluida_condo = oSqlDataReader.Item("quantidade_concluida_condo")
                oReturn.quantidade_andamento_condo = oSqlDataReader.Item("quantidade_andamento_condo")
                oReturn.quantidade_vinculada_condo = oSqlDataReader.Item("quantidade_vinculada_condo")
                oReturn.quantidade_pendente_pool = oSqlDataReader.Item("quantidade_pendente_pool")
                oReturn.quantidade_concluida_pool = oSqlDataReader.Item("quantidade_concluida_pool")
                oReturn.quantidade_andamento_pool = oSqlDataReader.Item("quantidade_andamento_pool")
                oReturn.quantidade_vinculada_pool = oSqlDataReader.Item("quantidade_vinculada_pool")
                oReturn.nota_auditoria_externa = oSqlDataReader.Item("nota_auditoria_externa")
                oReturn.nota_avaliacao_mensal = oSqlDataReader.Item("nota_avaliacao_mensal")
                oReturn.ranking = oSqlDataReader.Item("ranking")
                oReturn.arquivo_auditoria_externa = oSqlDataReader.Item("arquivo_auditoria_externa")
                oReturn.log_book = oSqlDataReader.Item("log_book")

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

    Public Sub ChartResumoServicos(ByVal iCodigoEmpresa As Integer,
                                   ByVal iCodigoUnidade As Integer,
                                   ByRef oChart As ChartResumoServicos)

        'Variaveis Locais
        Dim oSqlParameter(1) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim i As Integer = 0

        Try

            oChart = New ChartResumoServicos
            oChart.label = New List(Of String)
            oChart.os = New List(Of String)
            oChart.programada = New List(Of String)
            oChart.pmoc = New List(Of String)
            oChart.uh = New List(Of String)

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
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_dashboard_chart_resumo_servicos", oSqlParameter)

            While oSqlDataReader.Read

                oChart.label.Add(oSqlDataReader.Item("label"))
                oChart.os.Add(oSqlDataReader.Item("os"))
                oChart.programada.Add(oSqlDataReader.Item("programada"))
                oChart.pmoc.Add(oSqlDataReader.Item("pmoc"))
                oChart.uh.Add(oSqlDataReader.Item("uh"))

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function ChartGauge(ByVal iCodigoEmpresa As Integer,
                               ByVal iCodigoUnidade As Integer,
                               ByVal iCodigoModulo As Integer) As ChartGauge


        'Variaveis Locais
        Dim oSqlParameter(2) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New ChartGauge
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
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Módulo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_modulo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoModulo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_chart_dashboard_gauge", oSqlParameter)

            While oSqlDataReader.Read

                Select Case oSqlDataReader.Item("label")
                    Case "LAUDO"
                        oReturn.laudo = oSqlDataReader.Item("valor")
                        oReturn.cor_laudo = IIf(oSqlDataReader("valor") < 65, "rgba(255, 0, 0, 1)", IIf(oSqlDataReader("valor") > 85, "rgba(34, 177, 76, 1)", "rgba(255, 201, 14, 1)"))
                    Case "PREV." : oReturn.preventiva = oSqlDataReader.Item("valor")
                        oReturn.cor_preventiva = IIf(oSqlDataReader("valor") < 65, "rgba(255, 0, 0, 1)", IIf(oSqlDataReader("valor") > 85, "rgba(34, 177, 76, 1)", "rgba(255, 201, 14, 1)"))
                    Case "UH EM DIA" : oReturn.uh_dia = oSqlDataReader.Item("valor")
                        oReturn.cor_uh_dia = IIf(oSqlDataReader("valor") < 65, "rgba(255, 0, 0, 1)", IIf(oSqlDataReader("valor") > 85, "rgba(34, 177, 76, 1)", "rgba(255, 201, 14, 1)"))
                    Case "PMOC" : oReturn.pmoc = oSqlDataReader.Item("valor")
                        oReturn.cor_pmoc = IIf(oSqlDataReader("valor") < 65, "rgba(255, 0, 0, 1)", IIf(oSqlDataReader("valor") > 85, "rgba(34, 177, 76, 1)", "rgba(255, 201, 14, 1)"))
                    Case "ROTINA" : oReturn.rotina = oSqlDataReader.Item("valor")
                        oReturn.cor_rotina = IIf(oSqlDataReader("valor") < 65, "rgba(255, 0, 0, 1)", IIf(oSqlDataReader("valor") > 85, "rgba(34, 177, 76, 1)", "rgba(255, 201, 14, 1)"))
                    Case "GREEN_PLANET" : oReturn.green_planet = oSqlDataReader.Item("valor")
                        oReturn.cor_green_planet = IIf(oSqlDataReader("valor") < 65, "rgba(255, 0, 0, 1)", IIf(oSqlDataReader("valor") > 85, "rgba(34, 177, 76, 1)", "rgba(255, 201, 14, 1)"))
                End Select

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

    Public Function DashboardInfo(ByVal iCodigoEmpresa As Integer) As List(Of DashboardInfoMain)


        'Variaveis Locais
        Dim oSqlParameter(0) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New List(Of DashboardInfoMain)
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_dashboard_main", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New DashboardInfoMain

                oInfo.unidade = oSqlDataReader.Item("unidade")
                oInfo.quantidadeOSGerada = oSqlDataReader.Item("quantidade_os_gerada")
                oInfo.quantidadeOSAtendida = oSqlDataReader.Item("quantidade_os_atendida")
                oInfo.quantidadeOSPendente = oSqlDataReader.Item("quantidade_os_pendente")
                oInfo.laudo = oSqlDataReader.Item("laudo") / 100.0
                oInfo.preventiva = oSqlDataReader.Item("preventiva") / 100.0
                oInfo.rotina = oSqlDataReader.Item("rotina") / 100.0
                oInfo.pmoc = oSqlDataReader.Item("pmoc") / 100.0
                oInfo.uhDia = oSqlDataReader.Item("uh_dia") / 100.0
                oInfo.greenPlanet = oSqlDataReader.Item("green_planet") / 100.0
                oInfo.corLaudo = IIf(oSqlDataReader.Item("laudo") >= 90, "bg-success text-white", IIf(oSqlDataReader.Item("laudo") < 80, "bg-danger text-white", "bg-warning"))
                oInfo.corPreventiva = IIf(oSqlDataReader.Item("preventiva") >= 90, "bg-success text-white", IIf(oSqlDataReader.Item("preventiva") < 80, "bg-danger text-white", "bg-warning"))
                oInfo.corRotina = IIf(oSqlDataReader.Item("rotina") >= 90, "bg-success text-white", IIf(oSqlDataReader.Item("rotina") < 80, "bg-danger text-white", "bg-warning"))
                oInfo.corPMOC = IIf(oSqlDataReader.Item("pmoc") >= 90, "bg-success text-white", IIf(oSqlDataReader.Item("pmoc") < 80, "bg-danger text-white", "bg-warning"))
                oInfo.corUHDia = IIf(oSqlDataReader.Item("uh_dia") >= 90, "bg-success text-white", IIf(oSqlDataReader.Item("uh_dia") < 80, "bg-danger text-white", "bg-warning"))
                oInfo.corGreenPlanet = IIf(oSqlDataReader.Item("green_planet") >= 90, "bg-success text-white", IIf(oSqlDataReader.Item("green_planet") < 80, "bg-danger text-white", "bg-warning"))

                oReturn.Add(oInfo)

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

    Public Function PrincipaisOcorrencias(ByVal iCodigoEmpresa As Integer,
                                          ByVal iCodigoUnidade As Integer,
                                          ByVal iCodigoModulo As Integer,
                                          ByVal iQuantidade As Integer,
                                          ByVal bQualidade As Boolean) As List(Of PrincipaisOcorrencias)


        'Variaveis Locais
        Dim oSqlParameter(4) As SqlParameter
        Dim oReturn As New List(Of PrincipaisOcorrencias)
        Dim oSqlDataReader As SqlDataReader
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
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Módulo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_modulo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoModulo : i += 1

            'Seta Parametros - Quantidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "quantidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iQuantidade : i += 1

            'Seta Parametros - Qualidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "qualidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bQualidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_home_principais_ocorrencias", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New PrincipaisOcorrencias

                oInfo.item = oSqlDataReader.Item("item")
                oInfo.local = oSqlDataReader.Item("local")
                oInfo.quantidade = oSqlDataReader.Item("quantidade")
                oInfo.horas = oSqlDataReader.Item("horas")

                oReturn.Add(oInfo)

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

    Public Function LoadNotificacao(ByVal iCodigoEmpresa As Integer,
                                    ByVal iCodigoUnidade As Integer,
                                    ByVal iCodigoUsuario As Integer) As List(Of Notificacao)


        'Variaveis Locais
        Dim oSqlParameter(2) As SqlParameter
        Dim oReturn As New List(Of Notificacao)
        Dim oSqlDataReader As SqlDataReader
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

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pcm_notificacao", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New Notificacao

                oInfo.codigo = oSqlDataReader.Item("codigo")
                oInfo.codigo_empresa = oSqlDataReader.Item("codigo_empresa")
                oInfo.modulo = oSqlDataReader.Item("modulo")
                oInfo.descricao = oSqlDataReader.Item("descricao")
                oInfo.data_input = oSqlDataReader.Item("data_input")
                oInfo.data_necessidade = oSqlDataReader.Item("data_necessidade")
                oInfo.css = oSqlDataReader.Item("css")
                oInfo.link = oSqlDataReader.Item("link")
                oInfo.visto = oSqlDataReader.Item("visto")

                oReturn.Add(oInfo)

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

    Public Function LoadNotificacaoOSHospede(ByVal codigoEmpresa As Integer,
                                             ByVal codigoUnidade As Integer,
                                             ByVal codigoUsuario As Integer) As List(Of NotificacaoOSHoespede)


        'Variaveis Locais
        Dim oReturn As New List(Of NotificacaoOSHoespede)

        Try

            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("codigo_usuario", SqlDbType.Int, codigoUsuario)
            }

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pcm_notificacao_os_hospede", oSqlParameter)

                While oSqlDataReader.Read

                    Dim oInfo As New NotificacaoOSHoespede

                    oInfo.message = SafeGetString(oSqlDataReader, "message")
                    oInfo.type = SafeGetString(oSqlDataReader, "type")
                    oInfo.delay = SafeGetLong(oSqlDataReader, "delay")
                    oInfo.timer = SafeGetLong(oSqlDataReader, "timer")
                    oInfo.url = SafeGetString(oSqlDataReader, "url")

                    oReturn.Add(oInfo)

                End While

            End Using

            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Sub SetNotificacao(ByVal iCodigoEmpresa As Integer,
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

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_pcm_notificacao_visto", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

End Class
