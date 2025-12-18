Imports System.ComponentModel
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net.NetworkInformation
Imports OfficeOpenXml
Imports OfficeOpenXml.Style
Imports PCM.WEB.DAL.SQLHelper
Imports PCM.WEB.MODELS

Public Class Governanca

    Private sConnection As String

    Sub New(ByVal sCon As String)
        sConnection = sCon
    End Sub

#Region "::: CHECKLIST :::"

    Public Function LoadGovernanca(ByVal iCodigoEmpresa As Integer,
                                   ByVal iCodigoUnidade As Integer,
                                   ByVal iCodigoTipoGovernanca As Integer,
                                   ByVal iCodigoFuncionario As Integer,
                                   ByVal iStatus As Integer,
                                   ByVal sStatusFrontOffice As String,
                                   ByVal sStatusRoom As String) As List(Of MODELS.Governanca)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of MODELS.Governanca)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(6) As SqlParameter
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

            'Seta Parametros - Código Tipo Governança
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_tipo_governanca"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoTipoGovernanca : i += 1

            'Seta Parametros - Código Funcionário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_funcionario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoFuncionario : i += 1

            'Seta Parametros - Status
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "status"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iStatus : i += 1

            'Seta Parametros - Front Office Status
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "front_office_status"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sStatusFrontOffice : i += 1

            'Seta Parametros - Room Status
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "room_status"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sStatusRoom

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_governanca_checklist", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New MODELS.Governanca

                oInfo.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oInfo.unidade = oSqlDataReader.Item("unidade")
                oInfo.bloco = oSqlDataReader.Item("bloco")
                oInfo.andar = oSqlDataReader.Item("andar")
                oInfo.codigo_apartamento = oSqlDataReader.Item("codigo_apartamento")
                oInfo.codigo_vistoria = oSqlDataReader.Item("codigo_vistoria")
                oInfo.codigo_checklist = oSqlDataReader.Item("codigo_checklist")
                oInfo.apartamento = oSqlDataReader.Item("apartamento")
                oInfo.data_proxima = oSqlDataReader.Item("data_proxima")
                oInfo.css_class = oSqlDataReader.Item("css_class")
                oInfo.status = oSqlDataReader.Item("status")
                oInfo.room_status = oSqlDataReader.Item("room_status")
                oInfo.front_office_status = oSqlDataReader.Item("front_office_status")
                oInfo.codigo_tipo_governanca = iCodigoTipoGovernanca
                oInfo.nao_perturbe = oSqlDataReader.Item("nao_perturbe")

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

    Public Function LoadGovernancaDados(ByVal iCodigoEmpresa As Integer,
                                        ByVal iCodigoUnidade As Integer,
                                        ByVal lCodigoVistoria As Long) As GovernancaDados

        Try

            'Váriaveis Locais
            Dim oReturn As New GovernancaDados
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

            'Seta Parametros - Código Vistoria
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_vistoria"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoVistoria

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_governanca_dados", oSqlParameter)

            While oSqlDataReader.Read

                oReturn.codigo = oSqlDataReader.Item("codigo")
                oReturn.apartamento = oSqlDataReader.Item("apartamento")
                oReturn.camareira = oSqlDataReader.Item("camareira")
                oReturn.codigo_funcionario = oSqlDataReader.Item("codigo_funcionario")
                oReturn.codigo_funcionario_responsavel_vistoria = oSqlDataReader.Item("codigo_funcionario_responsavel_vistoria")
                oReturn.data = oSqlDataReader.Item("data")
                oReturn.hora_inicio = oSqlDataReader.Item("hora_inicio")
                oReturn.hora_termino = oSqlDataReader.Item("hora_termino")
                oReturn.status = oSqlDataReader.Item("status")
                oReturn.apontaCamareira = oSqlDataReader.Item("aponta_camareira")

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

#Region "::: DASHBOARD :::"

    Public Function DashboardInfo(ByVal codigoEmpresa As Integer,
                                  ByVal codigoUnidade As Integer,
                                  ByVal dataInicio As String,
                                  ByVal dataTermino As String) As MODELS.dashboardGovernanca

        Try

            'Váriaveis Locais
            Dim oReturn As New MODELS.dashboardGovernanca
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(3) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = codigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = codigoUnidade : i += 1

            'Seta Parametros - Data Início
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_inicio"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = dataInicio : i += 1

            'Seta Parametros - Data Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_termino"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = dataTermino

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_dashboard_governanca_info", oSqlParameter)

            If oSqlDataReader.HasRows Then

                While oSqlDataReader.Read

                    oReturn.quantidadeCamareira = oSqlDataReader.Item("quantidadeCamareira")
                    oReturn.quantidadeVistoriador = oSqlDataReader.Item("quantidadeVistoriador")
                    oReturn.quantidadeUHs = oSqlDataReader.Item("quantidadeUHs")
                    oReturn.hhDisponivel = oSqlDataReader.Item("hhDisponivel")
                    oReturn.hhUtilizado = oSqlDataReader.Item("hhUtilizado")
                    oReturn.quantidateUHsGovernanca = oSqlDataReader.Item("quantidateUHsGovernanca")
                    oReturn.quantidadeOSGerada = oSqlDataReader.Item("quantidadeOSGerada")

                End While

            Else

                oReturn.quantidadeCamareira = "0"
                oReturn.quantidadeVistoriador = "0"
                oReturn.quantidadeUHs = "0"
                oReturn.hhDisponivel = "00:00"
                oReturn.hhUtilizado = "00:00"
                oReturn.quantidateUHsGovernanca = "0"
                oReturn.quantidadeOSGerada = "0"

            End If


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

    Public Function LoadChartArrumadoxVistoriado(ByVal codigoEmpresa As Integer,
                                                 ByVal codigoUnidade As Integer,
                                                 ByVal dataInicio As String,
                                                 ByVal dataTermino As String) As List(Of dashboardGovernancaArrumadoxVistoriado)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of dashboardGovernancaArrumadoxVistoriado)
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("data_inicio", SqlDbType.Date, dataInicio),
                CriarParametro("data_termino", SqlDbType.Date, dataTermino)
            }

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_dashboard_governanca_chart_arrumado_vistoriado", oSqlParameter)

                If oSqlDataReader.HasRows Then

                    While oSqlDataReader.Read

                        Dim oInfo As New dashboardGovernancaArrumadoxVistoriado

                        oInfo.unidade = oSqlDataReader.Item("unidade")
                        oInfo.quantidadeUHs = oSqlDataReader.Item("quantidade_uh")
                        oInfo.quantidadeArrumados = oSqlDataReader.Item("quantidade_arrumado")
                        oInfo.quantidadeVistoriados = oSqlDataReader.Item("quantidade_vistoriado")

                        oReturn.Add(oInfo)

                    End While

                End If

            End Using

            'Retorno da Função
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function LoadChartArrumacaoDia(ByVal codigoEmpresa As Integer,
                                          ByVal codigoUnidade As Integer,
                                          ByVal dataInicio As String,
                                          ByVal dataTermino As String) As List(Of dashboardGovernancaChartArrumacaoDia)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of dashboardGovernancaChartArrumacaoDia)
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("data_inicio", SqlDbType.Date, dataInicio),
                CriarParametro("data_termino", SqlDbType.Date, dataTermino)
            }

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_dashboard_governanca_chart_arrumacao_dia", oSqlParameter)

                If oSqlDataReader.HasRows Then

                    While oSqlDataReader.Read

                        Dim oInfo As New dashboardGovernancaChartArrumacaoDia

                        oInfo.camareira = oSqlDataReader.Item("camareira")
                        oInfo.quantidade = oSqlDataReader.Item("quantidade")
                        oInfo.data = oSqlDataReader.Item("data")

                        oReturn.Add(oInfo)

                    End While

                End If

            End Using

            'Retorno da Função
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function LoadChartVistoriaDia(ByVal codigoEmpresa As Integer,
                                         ByVal codigoUnidade As Integer,
                                         ByVal dataInicio As String,
                                         ByVal dataTermino As String) As List(Of dashboardGovernancaChartVistoriaDia)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of dashboardGovernancaChartVistoriaDia)
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("data_inicio", SqlDbType.Date, dataInicio),
                CriarParametro("data_termino", SqlDbType.Date, dataTermino)
            }

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_dashboard_governanca_chart_vistoria_dia", oSqlParameter)

                If oSqlDataReader.HasRows Then

                    While oSqlDataReader.Read

                        Dim oInfo As New dashboardGovernancaChartVistoriaDia

                        oInfo.vistoriador = oSqlDataReader.Item("vistoriador")
                        oInfo.quantidade = oSqlDataReader.Item("quantidade")
                        oInfo.data = oSqlDataReader.Item("data")

                        oReturn.Add(oInfo)

                    End While

                End If

            End Using

            'Retorno da Função
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function LoadChartProdutividadeCamareira(ByVal codigoEmpresa As Integer,
                                                    ByVal codigoUnidade As Integer,
                                                    ByVal dataInicio As String,
                                                    ByVal dataTermino As String) As List(Of dashboardGovernancaChartProdutividade)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of dashboardGovernancaChartProdutividade)
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("data_inicio", SqlDbType.Date, dataInicio),
                CriarParametro("data_termino", SqlDbType.Date, dataTermino)
            }

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_dashboard_governanca_chart_produtividade_camareira", oSqlParameter)

                If oSqlDataReader.HasRows Then

                    While oSqlDataReader.Read

                        Dim oInfo As New dashboardGovernancaChartProdutividade

                        oInfo.unidade = oSqlDataReader.Item("unidade")
                        oInfo.percentual = oSqlDataReader.Item("percentual")
                        oInfo.quantidadePendente = oSqlDataReader.Item("quantidade_pendente")
                        oInfo.quantidadeOK = oSqlDataReader.Item("quantidade_ok")
                        oInfo.total = oSqlDataReader.Item("quantidade_ok") + oSqlDataReader.Item("quantidade_pendente")

                        oReturn.Add(oInfo)

                    End While

                End If

            End Using

            'Retorno da Função
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function LoadChartProdutividadeVistoriador(ByVal codigoEmpresa As Integer,
                                                      ByVal codigoUnidade As Integer,
                                                      ByVal dataInicio As String,
                                                      ByVal dataTermino As String) As List(Of dashboardGovernancaChartProdutividade)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of dashboardGovernancaChartProdutividade)
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("data_inicio", SqlDbType.Date, dataInicio),
                CriarParametro("data_termino", SqlDbType.Date, dataTermino)
            }

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_dashboard_governanca_chart_produtividade_vistoriador", oSqlParameter)

                If oSqlDataReader.HasRows Then

                    While oSqlDataReader.Read

                        Dim oInfo As New dashboardGovernancaChartProdutividade

                        oInfo.unidade = oSqlDataReader.Item("unidade")
                        oInfo.percentual = oSqlDataReader.Item("percentual")
                        oInfo.quantidadePendente = oSqlDataReader.Item("quantidade_pendente")
                        oInfo.quantidadeOK = oSqlDataReader.Item("quantidade_ok")
                        oInfo.total = oSqlDataReader.Item("quantidade_ok") + oSqlDataReader.Item("quantidade_pendente")

                        oReturn.Add(oInfo)

                    End While

                End If

            End Using

            'Retorno da Função
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function LoadChartNaoConformidadeDia(ByVal codigoEmpresa As Integer,
                                                ByVal codigoUnidade As Integer,
                                                ByVal dataInicio As String,
                                                ByVal dataTermino As String) As List(Of dashboardGovernancaNCDia)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of dashboardGovernancaNCDia)
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("data_inicio", SqlDbType.Date, dataInicio),
                CriarParametro("data_termino", SqlDbType.Date, dataTermino)
            }

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_dashboard_governanca_chart_nao_conformidade_dia", oSqlParameter)

                If oSqlDataReader.HasRows Then

                    While oSqlDataReader.Read

                        Dim oInfo As New dashboardGovernancaNCDia

                        oInfo.unidade = oSqlDataReader.Item("unidade")
                        oInfo.quantidadeNC = oSqlDataReader.Item("quantidade")
                        oInfo.data = oSqlDataReader.Item("data")

                        oReturn.Add(oInfo)

                    End While

                End If

            End Using

            'Retorno da Função
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function LoadAtendimentoOrdemServico(ByVal codigoEmpresa As Integer,
                                                ByVal codigoUnidade As Integer,
                                                ByVal dataInicio As String,
                                                ByVal dataTermino As String) As List(Of dashboardGovernancaAtendimentoOS)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of dashboardGovernancaAtendimentoOS)
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("data_inicio", SqlDbType.Date, dataInicio),
                CriarParametro("data_termino", SqlDbType.Date, dataTermino)
            }

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_dashboard_governanca_ordem_servico", oSqlParameter)

                If oSqlDataReader.HasRows Then

                    While oSqlDataReader.Read

                        Dim oInfo As New dashboardGovernancaAtendimentoOS

                        oInfo.unidade = oSqlDataReader.Item("unidade")
                        oInfo.quantidadeProprio = oSqlDataReader.Item("quantidade_proprio")
                        oInfo.quantidadeTerceiro = oSqlDataReader.Item("quantidade_terceiro")
                        oInfo.hhDisponivel = oSqlDataReader.Item("hh_disponivel")
                        oInfo.quantidaeOSGerada = oSqlDataReader.Item("quantidade_os_gerada")
                        oInfo.quantidaeOSAtendida = oSqlDataReader.Item("quantidade_os_atendida")
                        oInfo.quantidaeOSPAX = oSqlDataReader.Item("quantidade_os_pax")
                        oInfo.quantidadeOSPendente = oSqlDataReader.Item("quantidade_os_pendente")
                        oInfo.percentualAtendido = oSqlDataReader.Item("percentual_atendido")

                        oReturn.Add(oInfo)

                    End While

                End If

            End Using

            'Retorno da Função
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function LoadNCCamareira(ByVal codigoEmpresa As Integer,
                                    ByVal codigoUnidade As Integer,
                                    ByVal dataInicio As String,
                                    ByVal dataTermino As String) As List(Of dashboardGovernancaNCCamareira)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of dashboardGovernancaNCCamareira)
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("data_inicio", SqlDbType.Date, dataInicio),
                CriarParametro("data_termino", SqlDbType.Date, dataTermino)
            }

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_dashboard_governanca_nc_camareira", oSqlParameter)

                If oSqlDataReader.HasRows Then

                    While oSqlDataReader.Read

                        Dim oInfo As New dashboardGovernancaNCCamareira

                        oInfo.camareira = SafeGetString(oSqlDataReader, "camareira")
                        oInfo.quantidadeNC = SafeGetLong(oSqlDataReader, "quantidade_nc")
                        oInfo.mediaMovel30Dias = SafeGetLong(oSqlDataReader, "media_movel_30dias")
                        oInfo.tendencia = SafeGetString(oSqlDataReader, "tendencia")

                        oReturn.Add(oInfo)

                    End While

                End If

            End Using

            'Retorno da Função
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function RankingCamareira(ByVal codigoEmpresa As Integer,
                                     ByVal codigoUnidade As Integer,
                                     ByVal dataInicio As String,
                                     ByVal dataTermino As String) As List(Of dashboardRankingCamareira)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of dashboardRankingCamareira)
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("data_inicio", SqlDbType.Date, dataInicio),
                CriarParametro("data_termino", SqlDbType.Date, dataTermino)
            }

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_dashboard_governanca_ranking_camareira", oSqlParameter)

                If oSqlDataReader.HasRows Then

                    While oSqlDataReader.Read

                        Dim oInfo As New dashboardRankingCamareira

                        oInfo.camareira = SafeGetString(oSqlDataReader, "camareira")
                        oInfo.percentualNC = SafeGetString(oSqlDataReader, "percentualNC")
                        oInfo.ranking = SafeGetLong(oSqlDataReader, "ranking")
                        oInfo.qtdeNC = SafeGetLong(oSqlDataReader, "qtdeNC")
                        oInfo.qtdeUH = SafeGetLong(oSqlDataReader, "qtdeUH")
                        oInfo.qtdeNCRetrabalho = SafeGetLong(oSqlDataReader, "qtdeNCRetrabalho")
                        oInfo.pesoNC = SafeGetLong(oSqlDataReader, "pesoNC")
                        oInfo.pesoNCRetrabalho = SafeGetLong(oSqlDataReader, "pesoNCRetrabalho")
                        oInfo.cssClass = SafeGetString(oSqlDataReader, "cssClass")

                        oReturn.Add(oInfo)

                    End While

                End If

            End Using

            'Retorno da Função
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function LoadNCDetalhado(ByVal codigoEmpresa As Integer,
                                    ByVal codigoUnidade As Integer,
                                    ByVal dataInicio As String,
                                    ByVal dataTermino As String) As List(Of dashboardGovernancaNCDetalhado)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of dashboardGovernancaNCDetalhado)
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("data_inicio", SqlDbType.Date, dataInicio),
                CriarParametro("data_termino", SqlDbType.Date, dataTermino)
            }

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_dashboard_governanca_nc_detalhado", oSqlParameter)

                If oSqlDataReader.HasRows Then

                    While oSqlDataReader.Read

                        Dim oInfo As New dashboardGovernancaNCDetalhado

                        oInfo.ocorrencia = SafeGetString(oSqlDataReader, "ocorrencia")
                        oInfo.quantidadeNC = SafeGetLong(oSqlDataReader, "quantidade_nc")
                        oInfo.mediaMovel30Dias = SafeGetLong(oSqlDataReader, "media_movel_30dias")
                        oInfo.tendencia = SafeGetString(oSqlDataReader, "tendencia")

                        oReturn.Add(oInfo)

                    End While

                End If

            End Using

            'Retorno da Função
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    'Public Function LoadChartUHxCamareira(ByVal codigoEmpresa As Integer,
    '                                      ByVal codigoUnidade As Integer,
    '                                      ByVal dataInicio As String,
    '                                      ByVal dataTermino As String) As List(Of dashboardGovernancaChartUHxCamareira)

    '    Try

    '        'Váriaveis Locais
    '        Dim oReturn As New List(Of dashboardGovernancaChartUHxCamareira)
    '        Dim oSqlDataReader As SqlDataReader
    '        Dim oSqlParameter(3) As SqlParameter
    '        Dim i As Integer = 0

    '        'Seta Parametros - Código Empresa
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "codigo_empresa"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
    '        oSqlParameter(i).Value = codigoEmpresa : i += 1

    '        'Seta Parametros - Código Unidade
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "codigo_unidade"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
    '        oSqlParameter(i).Value = codigoUnidade : i += 1

    '        'Seta Parametros - Data Início
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "data_inicio"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.Date
    '        oSqlParameter(i).Value = dataInicio : i += 1

    '        'Seta Parametros - Data Término
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "data_termino"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.Date
    '        oSqlParameter(i).Value = dataTermino

    '        'Executa Query
    '        oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_dashboard_governanca_chart_uh_camareira", oSqlParameter)

    '        If oSqlDataReader.HasRows Then

    '            While oSqlDataReader.Read

    '                Dim oInfo As New dashboardGovernancaChartUHxCamareira

    '                oInfo.camareira = oSqlDataReader.Item("camareira")
    '                oInfo.quantidade = oSqlDataReader.Item("quantidade")
    '                oInfo.data = oSqlDataReader.Item("data")

    '                oReturn.Add(oInfo)

    '            End While

    '        End If

    '        'Fecha o oSqlDataReader
    '        If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

    '        'Retorno da Função
    '        Return oReturn

    '    Catch SqlEx As SqlException
    '        Throw SqlEx
    '    Catch ex As Exception
    '        Throw ex
    '    End Try

    'End Function

    'Public Function LoadChartNaoConformidadeTipo(ByVal codigoEmpresa As Integer,
    '                                             ByVal codigoUnidade As Integer,
    '                                             ByVal dataInicio As String,
    '                                             ByVal dataTermino As String) As List(Of dashboardGovernancaChartNaoConformidadeTipo)

    '    Try

    '        'Váriaveis Locais
    '        Dim oReturn As New List(Of dashboardGovernancaChartNaoConformidadeTipo)
    '        Dim oSqlDataReader As SqlDataReader
    '        Dim oSqlParameter(3) As SqlParameter
    '        Dim i As Integer = 0

    '        'Seta Parametros - Código Empresa
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "codigo_empresa"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
    '        oSqlParameter(i).Value = codigoEmpresa : i += 1

    '        'Seta Parametros - Código Unidade
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "codigo_unidade"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
    '        oSqlParameter(i).Value = codigoUnidade : i += 1

    '        'Seta Parametros - Data Início
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "data_inicio"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.Date
    '        oSqlParameter(i).Value = dataInicio : i += 1

    '        'Seta Parametros - Data Término
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "data_termino"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.Date
    '        oSqlParameter(i).Value = dataTermino

    '        'Executa Query
    '        oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_dashboard_governanca_chart_nao_conformidade_tipo", oSqlParameter)

    '        If oSqlDataReader.HasRows Then

    '            While oSqlDataReader.Read

    '                Dim oInfo As New dashboardGovernancaChartNaoConformidadeTipo

    '                oInfo.naoConformidadeTipo = oSqlDataReader.Item("naoConformidadeTipo")
    '                oInfo.quantidade = oSqlDataReader.Item("quantidade")

    '                oReturn.Add(oInfo)

    '            End While

    '        End If

    '        'Fecha o oSqlDataReader
    '        If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

    '        'Retorno da Função
    '        Return oReturn

    '    Catch SqlEx As SqlException
    '        Throw SqlEx
    '    Catch ex As Exception
    '        Throw ex
    '    End Try

    'End Function

    'Public Function LoadDashboardNaoConformidade(ByVal codigoEmpresa As Integer,
    '                                             ByVal codigoUnidade As Integer,
    '                                             ByVal dataInicio As String,
    '                                             ByVal dataTermino As String) As List(Of dashboardGovernancaNaoConformidade)

    '    Try

    '        'Váriaveis Locais
    '        Dim oReturn As New List(Of dashboardGovernancaNaoConformidade)
    '        Dim oSqlDataReader As SqlDataReader
    '        Dim oSqlParameter(3) As SqlParameter
    '        Dim i As Integer = 0

    '        'Seta Parametros - Código Empresa
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "codigo_empresa"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
    '        oSqlParameter(i).Value = codigoEmpresa : i += 1

    '        'Seta Parametros - Código Unidade
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "codigo_unidade"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
    '        oSqlParameter(i).Value = codigoUnidade : i += 1

    '        'Seta Parametros - Data Início
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "data_inicio"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.Date
    '        oSqlParameter(i).Value = dataInicio : i += 1

    '        'Seta Parametros - Data Término
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "data_termino"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.Date
    '        oSqlParameter(i).Value = dataTermino

    '        'Executa Query
    '        oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_dashboard_governanca_nao_conformidade_tipo", oSqlParameter)

    '        If oSqlDataReader.HasRows Then

    '            While oSqlDataReader.Read

    '                Dim oInfo As New dashboardGovernancaNaoConformidade

    '                oInfo.item = oSqlDataReader.Item("item")
    '                oInfo.descricao = oSqlDataReader.Item("descricao")
    '                oInfo.quantidade = oSqlDataReader.Item("quantidade")

    '                oReturn.Add(oInfo)

    '            End While

    '        End If

    '        'Fecha o oSqlDataReader
    '        If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

    '        'Retorno da Função
    '        Return oReturn

    '    Catch SqlEx As SqlException
    '        Throw SqlEx
    '    Catch ex As Exception
    '        Throw ex
    '    End Try

    'End Function

    'Public Function LoadDashboardApontamentos(ByVal iCodigoEmpresa As Integer,
    '                                          ByVal iCodigoUnidade As Integer,
    '                                          ByVal sData As String,
    '                                          ByVal sTipoGovernanca As String,
    '                                          ByVal sCamareira As String,
    '                                          ByVal sNaoConformidade As String) As List(Of GovernancaHistorico)

    '    Try

    '        'Váriaveis Locais
    '        Dim oSqlParameter(5) As SqlParameter
    '        Dim oSqlDataReader As SqlDataReader
    '        Dim oReturn As New List(Of GovernancaHistorico)
    '        Dim i As Integer = 0

    '        'Seta Parametros - Código Empresa
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "codigo_empresa"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
    '        oSqlParameter(i).Value = iCodigoEmpresa : i += 1

    '        'Seta Parametros - Código Unidade
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "codigo_unidade"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.Int
    '        oSqlParameter(i).Value = iCodigoUnidade : i += 1

    '        'Seta Parametros - Data 
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "data"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.Date
    '        oSqlParameter(i).Value = IIf(IsDate(sData), sData, DBNull.Value) : i += 1

    '        'Seta Parametros - Tipo Governança
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "tipo_governanca"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.VarChar
    '        oSqlParameter(i).Size = 150
    '        oSqlParameter(i).Value = sTipoGovernanca : i += 1

    '        'Seta Parametros - Camareira
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "camareira"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.VarChar
    '        oSqlParameter(i).Size = 150
    '        oSqlParameter(i).Value = sCamareira : i += 1

    '        'Seta Parametros - Não Conformidade
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "nao_conformidade"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.VarChar
    '        oSqlParameter(i).Size = 150
    '        oSqlParameter(i).Value = sNaoConformidade

    '        'Executa Query
    '        oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_dashboard_governanca_apontamentos", oSqlParameter)

    '        While oSqlDataReader.Read

    '            Dim oInfo As New GovernancaHistorico

    '            oInfo.codigo = oSqlDataReader("codigo")
    '            oInfo.codigoEmpresa = oSqlDataReader("codigo_empresa")
    '            oInfo.codigoUnidade = oSqlDataReader("codigo_unidade")
    '            oInfo.unidade = oSqlDataReader.Item("unidade")
    '            oInfo.data = oSqlDataReader.Item("data")
    '            oInfo.apartamento = oSqlDataReader("apartamento")
    '            oInfo.camareira = oSqlDataReader("camareira")
    '            oInfo.horaInicio = oSqlDataReader("hora_inicio")
    '            oInfo.horaTermino = oSqlDataReader("hora_termino")
    '            oInfo.tempoGasto = oSqlDataReader("tempo_gasto")
    '            oInfo.tipoGovernanca = oSqlDataReader("tipo_governanca")
    '            oInfo.naoConformidade = oSqlDataReader("nao_conformidade")
    '            oInfo.responsavelVistoria = oSqlDataReader("responsavel_vistoria")
    '            oInfo.horaVistoria = oSqlDataReader("hora_vistoria")
    '            oInfo.tipoGovernanca = oSqlDataReader("tipo_governanca")
    '            oInfo.status = oSqlDataReader("status")
    '            oInfo.cssClass = oSqlDataReader("css_class")

    '            oReturn.Add(oInfo)

    '        End While

    '        'Fecha o oSqlDataReader
    '        If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

    '        'Retorno da Função
    '        Return oReturn

    '    Catch SqlEx As SqlException
    '        Throw SqlEx
    '    Catch ex As Exception
    '        Throw ex
    '    End Try

    'End Function

#End Region

#Region "::: APONTAMENTO :::"

    Public Function LoadGovernancaApontamento(ByVal iCodigoEmpresa As Integer,
                                              ByVal iCodigoUnidade As Integer,
                                              ByVal iCodigoApartamento As Integer,
                                              ByVal iCodigoTipoGovernanca As Integer,
                                              ByVal lCodigoGovernancaApontamento As Long) As GovernancaApto

        Try

            Dim oReturn As New GovernancaApto

            oReturn.checklist = LoadGovernancaApontamentoChecklist(iCodigoEmpresa:=iCodigoEmpresa,
                                                                   iCodigoUnidade:=iCodigoUnidade,
                                                                   iCodigoApartamento:=iCodigoApartamento,
                                                                   iCodigoTipoGovernanca:=iCodigoTipoGovernanca,
                                                                   lCodigoGovernancaApontamento:=lCodigoGovernancaApontamento)

            oReturn.enxoval = LoadGovernancaApontamentoEnxoval(iCodigoEmpresa:=iCodigoEmpresa,
                                                               iCodigoUnidade:=iCodigoUnidade,
                                                               iCodigoApartamento:=iCodigoApartamento,
                                                               lCodigoGovernancaApontamento:=lCodigoGovernancaApontamento)

            Return oReturn

        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function LoadGovernancaStatus(ByVal iCodigoEmpresa As Integer,
                                         ByVal iCodigoUnidade As Integer,
                                         ByVal iCodigoTipoGovernanca As Integer) As GovernancaStatus

        Try

            'Váriaveis Locais
            Dim oReturn As New GovernancaStatus
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

            'Seta Parametros - Código Tipo Governança
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_tipo_governanca"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoTipoGovernanca

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_governanca_status", oSqlParameter)

            While oSqlDataReader.Read

                oReturn.pendente = oSqlDataReader.Item("pendente")
                oReturn.concluido = oSqlDataReader.Item("concluido")
                oReturn.aguardandoLiberacao = oSqlDataReader.Item("aguardando_liberacao")
                oReturn.retrabalho = oSqlDataReader.Item("retrabalho")

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

    Public Function LoadGovernancaApontamentoChecklist(ByVal iCodigoEmpresa As Integer,
                                                       ByVal iCodigoUnidade As Integer,
                                                       ByVal iCodigoApartamento As Integer,
                                                       ByVal iCodigoTipoGovernanca As Integer,
                                                       ByVal lCodigoGovernancaApontamento As Long) As List(Of GovernancaApontamentoChecklist)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of GovernancaApontamentoChecklist)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(4) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Apartamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_apartamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoApartamento : i += 1

            'Seta Parametros - Código Tipo Governança
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_tipo_governanca"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoTipoGovernanca : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Governança Apontamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_governanca_apontamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoGovernancaApontamento

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_governanca_apontamento_checklist_item", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New GovernancaApontamentoChecklist

                oInfo.codigo = oSqlDataReader.Item("codigo")
                oInfo.grupo = oSqlDataReader.Item("grupo")
                oInfo.checklist = oSqlDataReader.Item("checklist")
                oInfo.codigo_tipo_item_checklist = oSqlDataReader.Item("codigo_tipo_item_checklist")
                oInfo.descricao = oSqlDataReader.Item("descricao")
                oInfo.resultado = oSqlDataReader.Item("resultado")
                oInfo.resultado_descricao = oSqlDataReader.Item("resultado_descricao")
                oInfo.observacao = oSqlDataReader.Item("observacao")
                oInfo.nova_vistoria = oSqlDataReader.Item("nova_vistoria")

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

    Public Sub InsertGovernancaApontamento(ByVal iCodigoEmpresa As Integer,
                                           ByVal iCodigoUsuario As Integer,
                                           ByVal iCodigoUnidade As Integer,
                                           ByVal iCodigoApartamento As Integer,
                                           ByVal lCodigoChecklist As Long,
                                           ByVal iCodigoTipoGovernanca As Integer,
                                           ByVal iCodigoFuncionario As Integer,
                                           ByVal iCodigoVistoriador As Integer,
                                           ByVal sData As String,
                                           ByVal sHoraInicio As String,
                                           ByVal sHoraTermino As String,
                                           ByVal bNaoPerturbe As Boolean,
                                           ByRef lCodigoGovernancaApontamento As Long)

        Try

            'Váriaveis Locais
            Dim oSqlParameter(12) As SqlParameter
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

            'Seta Parametros - Código Apartamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_apartamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoApartamento : i += 1

            'Seta Parametros - Código Tipo Governança
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_tipo_governanca"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoTipoGovernanca : i += 1

            'Seta Parametros - Código Checklist
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_checklist"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoChecklist : i += 1

            'Seta Parametros - Código Funcionário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_funcionario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoFuncionario : i += 1

            'Seta Parametros - Código Vistoriador
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_vistoriador"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoVistoriador : i += 1

            'Seta Parametros - Data
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sData : i += 1

            'Seta Parametros - Hora Início
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "hora_inicio"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Time
            oSqlParameter(i).Value = IIf(IsDate(sHoraInicio), sHoraInicio, DBNull.Value) : i += 1

            'Seta Parametros - Hora Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "hora_termino"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Time
            oSqlParameter(i).Value = IIf(IsDate(sHoraTermino), sHoraTermino, DBNull.Value) : i += 1

            'Seta Parametros - Não Perturbe
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "nao_perturbe"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bNaoPerturbe : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Output
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_governanca_apontamento", oSqlParameter)

            lCodigoGovernancaApontamento = oSqlParameter(i).Value

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateGovernancaApontamentoVistoria(ByVal iCodigoEmpresa As Integer,
                                                   ByVal iCodigoUnidade As Integer,
                                                   ByVal iCodigoCamareira As Integer,
                                                   ByVal iCodigoVistoriador As Integer,
                                                   ByVal lCodigoApartamento As Long,
                                                   ByVal iCodigoTipoGovernanca As Integer,
                                                   ByVal bNaoPerturbe As Boolean,
                                                   ByVal iCodigoUsuario As Integer,
                                                   ByRef lCodigo As Long,
                                                   ByVal sData As String,
                                                   ByVal sHoraInicio As String,
                                                   ByVal sHoraTermino As String)

        Try

            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, iCodigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, iCodigoUnidade),
                CriarParametro("codigo_apartamento", SqlDbType.BigInt, lCodigoApartamento),
                CriarParametro("nao_perturbe", SqlDbType.Bit, bNaoPerturbe),
                CriarParametro("codigo_funcionario", SqlDbType.Int, IIf(iCodigoCamareira = -1, DBNull.Value, iCodigoCamareira)),
                CriarParametro("codigo_funcionario_vistoria", SqlDbType.Int, iCodigoVistoriador),
                CriarParametro("codigo_tipo_governanca", SqlDbType.SmallInt, iCodigoTipoGovernanca),
                CriarParametro("data_vistoria", SqlDbType.Date, sData),
                CriarParametro("hora_inicio_vistoria", SqlDbType.Time, IIf(IsDate(sHoraInicio), sHoraInicio, DBNull.Value)),
                CriarParametro("hora_termino_vistoria", SqlDbType.Time, IIf(IsDate(sHoraTermino), sHoraTermino, DBNull.Value)),
                CriarParametro("codigo_usuario", SqlDbType.Int, iCodigoUsuario),
                CriarParametro("codigo", SqlDbType.BigInt, lCodigo, ParameterDirection.InputOutput)
            }

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_governanca_apontamento_vistoria", oSqlParameter)

            lCodigo = oSqlParameter(UBound(oSqlParameter)).Value

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InsertGovernancaApontamentoChecklist(ByVal iCodigoEmpresa As Integer,
                                                    ByVal iCodigoUnidade As Integer,
                                                    ByVal lCodigoGovernancaApontamento As Long,
                                                    ByVal lCodigoChecklist As Long,
                                                    ByVal iCodigoChecklistItem As Integer,
                                                    ByVal sResultado As String,
                                                    ByVal sObservacao As String,
                                                    ByVal bNovaVistoria As Boolean)

        Try

            'Váriaveis Locais
            Dim oSqlParameter(7) As SqlParameter
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

            'Seta Parametros - Código Governanca Apontamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_governanca_apontamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoGovernancaApontamento : i += 1

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
            oSqlParameter(i).Value = IIf(iCodigoChecklistItem = -1, DBNull.Value, iCodigoChecklistItem) : i += 1

            'Seta Parametros - Resultado
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "resultado"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = IIf(IsNothing(sResultado), "", sResultado) : i += 1

            'Seta Parametros - Observação
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "observacao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 200
            oSqlParameter(i).Value = IIf(IsNothing(sObservacao), "", sObservacao) : i += 1

            'Seta Parametros - Nova Vistoria
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "nova_vistoria"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = IIf(IsNothing(bNovaVistoria), False, bNovaVistoria)

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_governanca_apontamento_checklist", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InsertGovernancaApontamentoChecklistVistoria(ByVal iCodigoEmpresa As Integer,
                                                            ByVal iCodigoUnidade As Integer,
                                                            ByVal lCodigoGovernancaApontamento As Long,
                                                            ByVal iCodigoChecklistItem As Integer,
                                                            ByVal sResultado As String,
                                                            ByVal sObservacao As String)

        Try

            'Váriaveis Locais
            Dim oSqlParameter(5) As SqlParameter
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

            'Seta Parametros - Código Governanca Apontamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_governanca_apontamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoGovernancaApontamento : i += 1

            'Seta Parametros - Código Checklist Item
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_checklist_item"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoChecklistItem = -1, DBNull.Value, iCodigoChecklistItem) : i += 1

            'Seta Parametros - Resultado
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "resultado"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = IIf(IsNothing(sResultado), "", sResultado) : i += 1

            'Seta Parametros - Observação
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "observacao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 200
            oSqlParameter(i).Value = IIf(IsNothing(sObservacao), "", sObservacao)

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_governanca_apontamento_checklist_vistoria", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateGovernancaApontamento(ByVal iCodigoEmpresa As Integer,
                                           ByVal iCodigoUsuario As Integer,
                                           ByVal iCodigoUnidade As Integer,
                                           ByVal lCodigoGovernancaApontamento As Long)

        Try

            'Váriaveis Locais
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

            'Seta Parametros - Código Governanca Apontamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoGovernancaApontamento

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_governanca_apontamento_status", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub NaoPerturbe(ByVal iCodigoEmpresa As Integer,
                           ByVal iCodigoUsuario As Integer,
                           ByVal iCodigoUnidade As Integer,
                           ByVal lCodigoApartamento As Long,
                           ByVal iCodigoTipoGovernanca As Integer)

        Try

            'Váriaveis Locais
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

            'Seta Parametros - Código Apartamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_apartamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoApartamento : i += 1

            'Seta Parametros - Código Tipo de Governaça
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_tipo_governanca"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoTipoGovernanca : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_governanca_apontamento_nao_perturbe", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region

#Region "::: ENXOVAL :::"

    Public Function LoadGovernancaApontamentoEnxoval(ByVal iCodigoEmpresa As Integer,
                                                     ByVal iCodigoUnidade As Integer,
                                                     ByVal iCodigoApartamento As Integer,
                                                     ByVal lCodigoGovernancaApontamento As Long) As List(Of GovernancaApontamentoEnxoval)

        Try

            'Váriaveis Locais
            Dim oGovernancaApontamentoEnxoval As New List(Of GovernancaApontamentoEnxoval)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(3) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Apartamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_apartamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoApartamento : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Governança Apontamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_governanca_apontamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoGovernancaApontamento

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_governanca_apontamento_enxoval", oSqlParameter)

            While oSqlDataReader.Read

                Dim oGovernancaApontamentoEnxovalInfo As New GovernancaApontamentoEnxoval

                oGovernancaApontamentoEnxovalInfo.codigo_enxoval = oSqlDataReader.Item("codigo_enxoval")
                oGovernancaApontamentoEnxovalInfo.enxoval = oSqlDataReader.Item("enxoval")
                oGovernancaApontamentoEnxovalInfo.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oGovernancaApontamentoEnxovalInfo.quantidade = oSqlDataReader.Item("quantidade")
                oGovernancaApontamentoEnxovalInfo.unidade = oSqlDataReader.Item("unidade")

                oGovernancaApontamentoEnxoval.Add(oGovernancaApontamentoEnxovalInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oGovernancaApontamentoEnxoval

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Sub InsertGovernancaApontamentoEnxoval(ByVal iCodigoEmpresa As Integer,
                                                  ByVal iCodigoUnidade As Integer,
                                                  ByVal lCodigoGovernancaApontamento As Long,
                                                  ByVal iCodigoEnxoval As Integer,
                                                  ByVal iQuantidade As Integer)

        Try

            'Váriaveis Locais
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

            'Seta Parametros - Código Governanca Apontamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_governanca_apontamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoGovernancaApontamento : i += 1

            'Seta Parametros - Código Enxocal
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_enxoval"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoEnxoval : i += 1

            'Seta Parametros - Quantidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "quantidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iQuantidade

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_governanca_apontamento_enxoval", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region

#Region "::: FUNCIONÁRIO :::"

    Public Sub InsertFuncionario(ByVal iCodigoEmpresa As Integer,
                                 ByVal iCodigoUsuario As Integer,
                                 ByVal iCodigoUnidade As Integer,
                                 ByVal sNome As String,
                                 ByVal sCPF As String,
                                 ByVal iCodigoFuncao As Integer,
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
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_governanca_funcionario", oSqlParameter)

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
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_governanca_funcionario", oSqlParameter)

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
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_governanca_funcionario", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InfoFuncionario(ByVal iCodigoEmpresa As Integer,
                               ByVal iCodigo As Integer,
                               ByRef oFuncionario As GovFuncionario)

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
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_governanca_funcionario_dados", oSqlParameter)

            While oSqlDataReader.Read

                oFuncionario = New GovFuncionario
                oFuncionario.codigo = oSqlDataReader.Item("codigo")
                oFuncionario.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oFuncionario.unidade = oSqlDataReader.Item("unidade")
                oFuncionario.nome = oSqlDataReader.Item("nome")
                oFuncionario.cpf = oSqlDataReader.Item("cpf")
                oFuncionario.funcao = oSqlDataReader.Item("funcao")
                oFuncionario.codigo_funcao = oSqlDataReader.Item("codigo_funcao")
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
                                     ByVal sNome As String,
                                     ByVal iCodigoFuncao As Integer,
                                     ByVal iCodigoTipoFuncionario As Integer,
                                     ByVal iAtivo As Integer) As List(Of GovFuncionario)

        Try

            'Váriaveis Locais
            Dim oFuncionario As New List(Of GovFuncionario)
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
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_governanca_funcionario", oSqlParameter)

            While oSqlDataReader.Read

                Dim oFuncionarioInfo As New GovFuncionario

                oFuncionarioInfo.cpf = oSqlDataReader.Item("cpf")
                oFuncionarioInfo.unidade = oSqlDataReader.Item("unidade")
                oFuncionarioInfo.nome = oSqlDataReader.Item("nome")
                oFuncionarioInfo.funcao = oSqlDataReader.Item("funcao")
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
            iReturn = ExecuteScalar(sConnection, CommandType.StoredProcedure, "sp_validate_governanca_funcionario", oSqlParameter)

            'Retorno da Função
            Return (IIf(iReturn > 0, False, True))

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: PLANEJAMENTO :::"

    Public Function LoadLastUploadStatus(ByVal codigoEmpresa As Integer,
                                         ByVal codigoUnidade As Integer) As String

        Try

            ' Define parameters
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade)
            }

            'Executa Query
            Return CType(ExecuteScalar(sConnection, CommandType.StoredProcedure, "sp_select_governanca_planejamento_last_update", oSqlParameter), String)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Sub InsertPlanejamento(ByVal iCodigoEmpresa As Integer,
                                  ByVal iCodigoUnidade As Integer,
                                  ByVal iCodigoTipoGovernanca As Integer,
                                  ByVal sData As String,
                                  ByVal iCodigoFuncionario As Integer,
                                  ByVal iCodigoUsuario As Integer,
                                  ByRef lCodigo As Long)

        Try

            'Váriaveis Locais
            Dim oSqlParameter(6) As SqlParameter
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

            'Seta Parametros - Código Tipo Governança
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_tipo_governanca"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoTipoGovernanca : i += 1

            'Seta Parametros - Data
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sData : i += 1

            'Seta Parametros - Código Funcionário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_funcionario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoFuncionario : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Output
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_governanca_planejamento", oSqlParameter)

            lCodigo = oSqlParameter(i).Value

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InsertPlanejamentoUH(ByVal iCodigoEmpresa As Integer,
                                    ByVal lCodigoPlanejamento As Long,
                                    ByVal iCodigoApartamento As Integer)

        Try

            'Váriaveis Locais
            Dim oSqlParameter(2) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código Planejamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_planejamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoPlanejamento : i += 1

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Apartamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_apartamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoApartamento

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_governanca_planejamento_apartamento", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function LoadPlanejamento(ByVal iCodigoEmpresa As Integer,
                                     ByVal iCodigoUnidade As Integer,
                                     ByVal iCodigoTipoGovernanca As Integer,
                                     ByVal sData As String,
                                     ByVal iCodigoFuncionario As Integer,
                                     ByVal iUHAssociada As Integer,
                                     ByVal sBloco As String,
                                     ByVal sAndar As String,
                                     ByVal sStatusFrontOffice As String,
                                     ByVal sStatusRoom As String) As List(Of GovernancaPlanejamento)

        Try

            'Váriaveis Locais
            Dim oSqlParameter(9) As SqlParameter
            Dim oSqlDataReader As SqlDataReader
            Dim oReturn As New List(Of GovernancaPlanejamento)
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

            'Seta Parametros - Código Tipo Governança
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_tipo_governanca"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoTipoGovernanca : i += 1

            'Seta Parametros - Data
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = IIf(IsDate(sData), sData, DBNull.Value) : i += 1

            'Seta Parametros - Código Funcionário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_funcionario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoFuncionario : i += 1

            'Seta Parametros - UH Associada
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "uh_associada"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iUHAssociada : i += 1

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
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sAndar : i += 1

            'Seta Parametros - Front Office Status
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "front_office_status"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sStatusFrontOffice : i += 1

            'Seta Parametros - Room Status
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "room_status"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sStatusRoom

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_governanca_planejamento", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New GovernancaPlanejamento

                oInfo.codigoApartamento = oSqlDataReader("codigo_apartamento")
                oInfo.apartamento = oSqlDataReader("apartamento")
                oInfo.tipoApartamento = oSqlDataReader("tipo_apartamento")
                oInfo.tipoCama = oSqlDataReader("tipo_cama")
                oInfo.bloco = oSqlDataReader("bloco")
                oInfo.andar = oSqlDataReader("andar")
                oInfo.quantidadeCama = oSqlDataReader("quantidade_cama")
                oInfo.funcionario = oSqlDataReader("funcionario")
                oInfo.selecionado = IIf(oSqlDataReader("selecionado"), "checked", "")
                oInfo.statusFrontOffice = oSqlDataReader("front_office_status")
                oInfo.tipoGovernanca = oSqlDataReader("tipo_governanca")
                oInfo.cssClassTipoGovernaca = oSqlDataReader("css_class_tipo_governanca")
                oInfo.statusRoom = oSqlDataReader("room_status")

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

    Public Function LoadPlanejamento2(ByVal codigoEmpresa As Integer,
                                      ByVal codigoUnidade As Integer,
                                      ByVal data As String,
                                      ByVal codigoTipoGovernanca As Integer,
                                      ByVal bloco As String,
                                      ByVal andar As String,
                                      ByVal statusFrontOffice As String,
                                      ByVal statusRoom As String) As List(Of GovernancaPlanejamento)

        Try

            'Variaveis Locais
            Dim oReturn As New List(Of GovernancaPlanejamento)
            Dim oSqlParameter As SqlParameter() = {
                    CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                    CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                    CriarParametro("data", SqlDbType.Date, data),
                    CriarParametro("codigo_tipo_governanca", SqlDbType.SmallInt, codigoTipoGovernanca),
                    CriarParametro("bloco", SqlDbType.VarChar, bloco),
                    CriarParametro("andar", SqlDbType.VarChar, andar),
                    CriarParametro("front_office_status", SqlDbType.VarChar, statusFrontOffice),
                    CriarParametro("room_status", SqlDbType.VarChar, statusRoom)
                }

            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_governanca_planejamento2", oSqlParameter)

                While oSqlDataReader.Read

                    Dim oInfo As New GovernancaPlanejamento

                    oInfo.codigoApartamento = SafeGetLong(oSqlDataReader, "codigo_apartamento")
                    oInfo.apartamento = SafeGetString(oSqlDataReader, "apartamento")
                    oInfo.tipoApartamento = SafeGetString(oSqlDataReader, "tipo_apartamento")
                    oInfo.tipoCama = SafeGetString(oSqlDataReader, "tipo_cama")
                    oInfo.bloco = SafeGetString(oSqlDataReader, "bloco")
                    oInfo.andar = SafeGetString(oSqlDataReader, "andar")
                    oInfo.quantidadeCama = SafeGetString(oSqlDataReader, "quantidade_cama")
                    oInfo.funcionario = SafeGetString(oSqlDataReader, "funcionario")
                    oInfo.selecionado = SafeGetString(oSqlDataReader, "selecionado")
                    oInfo.statusFrontOffice = SafeGetString(oSqlDataReader, "front_office_status")
                    oInfo.tipoGovernanca = SafeGetString(oSqlDataReader, "tipo_governanca")
                    oInfo.dataChegada = SafeGetString(oSqlDataReader, "data_chegada")
                    oInfo.dataSaida = SafeGetString(oSqlDataReader, "data_saida")
                    oInfo.cssClassTipoGovernaca = SafeGetString(oSqlDataReader, "css_class_tipo_governanca")
                    oInfo.statusRoom = SafeGetString(oSqlDataReader, "room_status")

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

    Public Sub ClearPlanejamento(ByVal codigoEmpresa As Integer,
                                 ByVal codigoUnidade As Integer,
                                 ByVal data As String,
                                 ByVal json As String)

        Try

            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("data", SqlDbType.Date, IIf(IsDate(data), data, DBNull.Value)),
                CriarParametro("json", SqlDbType.VarChar, json)
            }

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_governanca_planejamento_clear", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateTipoGovernanca(ByVal codigoEmpresa As Integer,
                                    ByVal codigoUnidade As Integer,
                                    ByVal data As String,
                                    ByVal codigoTipoGovernanca As Integer,
                                    ByVal json As String)

        Try

            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("data", SqlDbType.Date, IIf(IsDate(data), data, DBNull.Value)),
                CriarParametro("codigo_tipo_governanca", SqlDbType.Int, codigoTipoGovernanca),
                CriarParametro("json", SqlDbType.VarChar, json)
            }

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_governanca_planejamento_tipo_governanca", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateCamareira(ByVal codigoEmpresa As Integer,
                               ByVal codigoUnidade As Integer,
                               ByVal data As String,
                               ByVal codigoCamareira As Integer,
                               ByVal json As String)

        Try

            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("data", SqlDbType.Date, IIf(IsDate(data), data, DBNull.Value)),
                CriarParametro("codigo_camareira", SqlDbType.Int, codigoCamareira),
                CriarParametro("json", SqlDbType.VarChar, json)
            }

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_governanca_planejamento_camareira", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region

#Region "::: PLANEJAMENTO HISTÓRICO :::"

    Public Function LoadPlanejamentoHistorico(ByVal codigoEmpresa As Integer,
                                              ByVal codigoUnidade As Integer,
                                              ByVal dataInicio As String,
                                              ByVal dataTermino As String,
                                              ByVal camareira As Integer,
                                              ByVal codigoTipoGovernanca As Integer,
                                              ByVal bloco As String,
                                              ByVal andar As String,
                                              ByVal apartamento As String) As List(Of GovernancaPlanejamentoHistorico)

        Try

            'Variaveis Locais
            Dim oReturn As New List(Of GovernancaPlanejamentoHistorico)
            Dim oSqlParameter As SqlParameter() = {
                    CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                    CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                    CriarParametro("data_inicio", SqlDbType.Date, IIf(IsDate(dataInicio), dataInicio, DBNull.Value)),
                    CriarParametro("data_termino", SqlDbType.Date, IIf(IsDate(dataTermino), dataTermino, DBNull.Value)),
                    CriarParametro("codigo_tipo_governanca", SqlDbType.SmallInt, codigoTipoGovernanca),
                    CriarParametro("codigo_camareira", SqlDbType.SmallInt, camareira),
                    CriarParametro("bloco", SqlDbType.VarChar, bloco),
                    CriarParametro("andar", SqlDbType.VarChar, andar),
                    CriarParametro("apartamento", SqlDbType.VarChar, apartamento)
                }

            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_governanca_planejamento_historico", oSqlParameter)

                While oSqlDataReader.Read

                    Dim oInfo As New GovernancaPlanejamentoHistorico With {
                        .codigoApartamento = SafeGetLong(oSqlDataReader, "codigo_apartamento"),
                        .data = SafeGetString(oSqlDataReader, "data"),
                        .tipoGovernanca = SafeGetString(oSqlDataReader, "tipo_governanca"),
                        .apartamento = SafeGetString(oSqlDataReader, "apartamento"),
                        .tipoApartamento = SafeGetString(oSqlDataReader, "tipo_apartamento"),
                        .bloco = SafeGetString(oSqlDataReader, "bloco"),
                        .andar = SafeGetString(oSqlDataReader, "andar"),
                        .quantidadeCama = SafeGetString(oSqlDataReader, "quantidade_cama"),
                        .camareira = SafeGetString(oSqlDataReader, "camareira"),
                        .executado = SafeGetString(oSqlDataReader, "executado"),
                        .quantidadeNC = SafeGetString(oSqlDataReader, "quantidade_nc"),
                        .vistoriado = SafeGetString(oSqlDataReader, "vistoriado")
                    }

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

#End Region

#Region "::: APONTAMENTO :::"

    Public Sub InsertApontamento(ByVal iCodigoEmpresa As Integer,
                                 ByVal iCodigoUnidade As Integer,
                                 ByVal iCodigoTipoGovernanca As Integer,
                                 ByVal sData As String,
                                 ByVal iCodigoFuncionario As Integer,
                                 ByVal iCodigoUsuario As Integer,
                                 ByVal iCodigoApartamento As Integer)

        Try

            'Váriaveis Locais
            Dim oSqlParameter(6) As SqlParameter
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

            'Seta Parametros - Código Tipo Governança
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_tipo_governanca"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoTipoGovernanca : i += 1

            'Seta Parametros - Data
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sData : i += 1

            'Seta Parametros - Código Funcionário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_funcionario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoFuncionario : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código Apartamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_apartamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoApartamento

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_governanca_apontamento_massa", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function LoadApontamento(ByVal iCodigoEmpresa As Integer,
                                    ByVal iCodigoUnidade As Integer,
                                    ByVal iCodigoTipoGovernanca As Integer,
                                    ByVal sData As String,
                                    ByVal iCodigoFuncionario As Integer,
                                    ByVal sBloco As String,
                                    ByVal sAndar As String,
                                    ByVal sStatusFrontOffice As String,
                                    ByVal sStatusRoom As String) As List(Of GovernancaApontamentoApartamento)

        Try

            'Váriaveis Locais
            Dim oSqlParameter(8) As SqlParameter
            Dim oSqlDataReader As SqlDataReader
            Dim oReturn As New List(Of GovernancaApontamentoApartamento)
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

            'Seta Parametros - Código Tipo Governança
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_tipo_governanca"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoTipoGovernanca : i += 1

            'Seta Parametros - Data
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = IIf(IsDate(sData), sData, DBNull.Value) : i += 1

            'Seta Parametros - Código Funcionário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_funcionario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoFuncionario : i += 1

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
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sAndar : i += 1

            'Seta Parametros - Front Office Status
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "front_office_status"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sStatusFrontOffice : i += 1

            'Seta Parametros - Room Status
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "room_status"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sStatusRoom

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_governanca_apontamento", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New GovernancaApontamentoApartamento

                oInfo.codigoApartamento = oSqlDataReader("codigo_apartamento")
                oInfo.apartamento = oSqlDataReader("apartamento")
                oInfo.tipoApartamento = oSqlDataReader("tipo_apartamento")
                oInfo.tipoCama = oSqlDataReader("tipo_cama")
                oInfo.bloco = oSqlDataReader("bloco")
                oInfo.andar = oSqlDataReader("andar")
                oInfo.quantidadeCama = oSqlDataReader("quantidade_cama")
                oInfo.funcionario = oSqlDataReader("funcionario")
                oInfo.selecionado = IIf(oSqlDataReader("selecionado"), "checked", "")
                oInfo.statusFrontOffice = oSqlDataReader("front_office_status")
                oInfo.tipoGovernanca = oSqlDataReader("tipo_governanca")
                oInfo.cssClassTipoGovernaca = oSqlDataReader("css_class_tipo_governanca")
                oInfo.statusRoom = oSqlDataReader("room_status")

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

#Region "::: GOVERNANÇA - HISTÓRICO :::"

    Public Function LoadGovernancaHistorico(ByVal iCodigoEmpresa As Integer,
                                            ByVal iCodigoUnidade As Integer,
                                            ByVal sDataInicio As String,
                                            ByVal sDataTermino As String,
                                            ByVal iCodigoTipoGovernanca As Integer,
                                            ByVal iCodigoCamareira As Integer,
                                            ByVal iCodigoApartamento As Integer) As List(Of GovernancaHistorico)

        Try

            'Váriaveis Locais
            Dim oSqlParameter(6) As SqlParameter
            Dim oSqlDataReader As SqlDataReader
            Dim oReturn As New List(Of GovernancaHistorico)
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

            'Seta Parametros - Data Início
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_inicio"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = IIf(IsDate(sDataInicio), sDataInicio, DBNull.Value) : i += 1

            'Seta Parametros - Data Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_termino"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = IIf(IsDate(sDataTermino), sDataTermino, DBNull.Value) : i += 1

            'Seta Parametros - Código Tipo Governança
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_tipo_governanca"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoTipoGovernanca : i += 1

            'Seta Parametros - Código Camareia
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_camareira"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoCamareira : i += 1

            'Seta Parametros - Código Apartamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_apartamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoApartamento

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_governanca_historico", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New GovernancaHistorico

                oInfo.codigo = oSqlDataReader("codigo")
                oInfo.codigoEmpresa = oSqlDataReader("codigo_empresa")
                oInfo.codigoUnidade = oSqlDataReader("codigo_unidade")
                oInfo.unidade = oSqlDataReader.Item("unidade")
                oInfo.data = oSqlDataReader.Item("data")
                oInfo.apartamento = oSqlDataReader("apartamento")
                oInfo.camareira = oSqlDataReader("camareira")
                oInfo.horaInicio = oSqlDataReader("hora_inicio")
                oInfo.horaTermino = oSqlDataReader("hora_termino")
                oInfo.tempoGasto = oSqlDataReader("tempo_gasto")
                oInfo.tipoGovernanca = oSqlDataReader("tipo_governanca")
                oInfo.naoConformidade = oSqlDataReader("nao_conformidade")
                oInfo.responsavelVistoria = oSqlDataReader("responsavel_vistoria")
                oInfo.horaVistoria = oSqlDataReader("hora_vistoria")
                oInfo.tipoGovernanca = oSqlDataReader("tipo_governanca")
                oInfo.status = oSqlDataReader("status")
                oInfo.cssClass = oSqlDataReader("css_class")

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

    Public Function LoadGovernancaHistoricoDetails(ByVal iCodigoEmpresa As Integer,
                                                   ByVal iCodigoUnidade As Integer,
                                                   ByVal lCodigo As Long) As GovernancaHistoricoDetails

        Try

            'Váriaveis Locais
            Dim oSqlParameter(2) As SqlParameter
            Dim oSqlDataReader As SqlDataReader
            Dim oReturn As New GovernancaHistoricoDetails
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

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_governanca_historico_details", oSqlParameter)

            oReturn.grupo = New List(Of GovernancaHistoricoChecklistGrupo)
            oReturn.enxoval = New List(Of GovernancaHistoricoEnxoval)

            While oSqlDataReader.Read

                Dim oInfoGrupo As New GovernancaHistoricoChecklistGrupo
                oInfoGrupo.grupo = oSqlDataReader.Item("grupo")
                oInfoGrupo.subgrupo = New List(Of GovernancaHistoricoChecklistSubGrupo)
                oReturn.grupo.Add(oInfoGrupo)

            End While

            oSqlDataReader.NextResult()

            While oSqlDataReader.Read

                Dim oInfoSubgrupo As New GovernancaHistoricoChecklistSubGrupo

                oInfoSubgrupo.grupo = oSqlDataReader.Item("grupo")
                oInfoSubgrupo.subgrupo = oSqlDataReader.Item("subgrupo")
                oInfoSubgrupo.checklist = New List(Of GovernancaHistoricoChecklist)
                Dim oGrupo As GovernancaHistoricoChecklistGrupo = oReturn.grupo.Find(Function(g) g.grupo = oSqlDataReader.Item("grupo"))
                oGrupo.subgrupo.Add(oInfoSubgrupo)

            End While

            oSqlDataReader.NextResult()

            While oSqlDataReader.Read

                Dim oInfoChecklist As New GovernancaHistoricoChecklist

                oInfoChecklist.checklist = oSqlDataReader.Item("checklist")
                oInfoChecklist.resposta = oSqlDataReader.Item("resultado")
                oInfoChecklist.observacao = oSqlDataReader.Item("observacao")
                oInfoChecklist.foto = oSqlDataReader.Item("foto")
                oInfoChecklist.cssClass = oSqlDataReader.Item("css_class")

                Dim oSubgrupo As GovernancaHistoricoChecklistSubGrupo = oReturn.grupo.Find(Function(g) g.grupo = oSqlDataReader.Item("grupo")).subgrupo.Find(Function(s) s.subgrupo = oSqlDataReader.Item("subgrupo"))

                oSubgrupo.checklist.Add(oInfoChecklist)

            End While

            oSqlDataReader.NextResult()

            While oSqlDataReader.Read

                Dim oInfoEnxoval As New GovernancaHistoricoEnxoval

                oInfoEnxoval.enxoval = oSqlDataReader.Item("enxoval")
                oInfoEnxoval.quantidade = oSqlDataReader.Item("quantidade")
                oInfoEnxoval.peso = oSqlDataReader.Item("peso")
                oInfoEnxoval.totalPeso = oSqlDataReader.Item("total_peso")

                oReturn.enxoval.Add(oInfoEnxoval)

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

    Public Sub DeleteGovernanca(ByVal iCodigoEmpresa As Integer,
                                ByVal iCodigoUnidade As Integer,
                                ByVal lCodigo As Long)

        Try

            'Váriaveis Locais
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

            'Seta Parametros - Código 
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_governanca", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region

#Region "::: STATUS UH :::"

    Public Sub UploadStatuUHExcel(ByVal codigoEmpresa As Integer,
                                  ByVal codigoUnidade As Integer,
                                  ByVal codigoUsuario As Integer,
                                  ByVal file As String,
                                  ByVal worksheet As String,
                                  ByRef oDetails As List(Of GovernancaStatusUHDetalhe),
                                  ByRef oDetailsError As List(Of GovernancaStatusUHDetalheError))

        ' Local variables
        Dim sQuery As String
        Dim iChangeFile As Integer
        Dim oOleDbConnection As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & file & ";Extended Properties='Excel 12.0 Xml;HDR=YES';")

        Try

            ' Step 1: Delete temporary inventory data
            Dim oDeleteParameters As SqlParameter() = {
                CriarParametro("codigo_usuario", SqlDbType.Int, codigoUsuario),
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade)
            }

            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_governanca_status_uh_tmp", oDeleteParameters)

            ' Step 2: Get inventory query
            Dim oQueryParameters As SqlParameter() = {
                CriarParametro("tipo", SqlDbType.VarChar, "STATUS UH"),
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("codigo_usuario", SqlDbType.Int, codigoUsuario),
                CriarParametro("worksheet", SqlDbType.VarChar, worksheet)
            }

            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pcm_query", oQueryParameters)

                While oSqlDataReader.Read

                    sQuery = SafeGetString(oSqlDataReader, "query")
                    sQuery = sQuery.Replace(vbCrLf, "").Replace(vbTab, "").Replace(vbLf, "")

                    iChangeFile = SafeGetLong(oSqlDataReader, "changeFile")

                End While

                If iChangeFile = 1 Then

                    'ExcelPackage.LicenseContext = LicenseContext.NonCommercial

                    Using oExcelPackage As New ExcelPackage(New FileInfo(file))

                        Dim oExcelExport = oExcelPackage.Workbook.Worksheets(worksheet)

                        oExcelExport.InsertRow(1, 1)

                        oExcelExport.Cells("A1").Value = "APARTAMENTO"
                        oExcelExport.Cells("E1").Value = "STATUS_UH"
                        oExcelExport.Cells("G1").Value = "STATUS_GOV"
                        oExcelExport.Cells("M1").Value = "DATA_CHEGADA"
                        oExcelExport.Cells("N1").Value = "DATA_SAIDA"

                        oExcelPackage.Save()

                    End Using

                End If

            End Using

            oOleDbConnection.Open()
            Using oSqlConnection As New SqlConnection(sConnection),
              oOleDbCommand As New OleDbCommand(sQuery, oOleDbConnection),
              oOleDbDataReader As OleDbDataReader = oOleDbCommand.ExecuteReader(),
              oSqlBulkCopy As New SqlBulkCopy(oSqlConnection)

                oSqlConnection.Open()
                oSqlBulkCopy.BulkCopyTimeout = 500
                oSqlBulkCopy.DestinationTableName = "dbo.tb_gov_status_uh_tmp"
                oSqlBulkCopy.WriteToServer(oOleDbDataReader)

            End Using

        Catch ex As Exception
            Throw ex
        Finally
            oOleDbConnection.Close()
        End Try

        Try

            ' Step 4: Retrieve and process inventory details
            Dim oDetailsParameters As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("codigo_usuario", SqlDbType.Int, codigoUsuario)
            }

            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_governanca_status_uh_tmp", oDetailsParameters)

                While oSqlDataReader.Read()

                    If IsDBNull(oSqlDataReader("erro")) Then
                        Dim oInfo As New GovernancaStatusUHDetalhe With {
                        .bloco = SafeGetString(oSqlDataReader, "bloco"),
                        .andar = SafeGetString(oSqlDataReader, "andar"),
                        .apartamento = SafeGetString(oSqlDataReader, "apartamento"),
                        .statusUH = SafeGetString(oSqlDataReader, "status_uh"),
                        .statusGov = SafeGetString(oSqlDataReader, "status_gov"),
                        .newStatusGov = SafeGetString(oSqlDataReader, "new_status_uh"),
                        .newStatusUH = SafeGetString(oSqlDataReader, "new_status_gov")
                    }
                        oDetails.Add(oInfo)
                    Else
                        Dim oInfo As New GovernancaStatusUHDetalheError With {
                        .apartamento = SafeGetString(oSqlDataReader, "apartamento"),
                        .erro = SafeGetString(oSqlDataReader, "erro")
                    }
                        oDetailsError.Add(oInfo)
                    End If

                End While

            End Using

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateStatusUH(ByVal codigoEmpresa As Integer,
                              ByVal codigoUnidade As Integer,
                              ByVal codigoUsuario As Integer)

        Try

            ' Define parameters
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("codigo_usuario", SqlDbType.Int, codigoUsuario)
            }

            ' Execute query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_governanca_status_uh_tmp", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region

#Region "::: LAVANDERIA :::"

    Public Sub ApontamentoLavanderia(ByVal codigoEmpresa As Integer,
                                     ByVal codigoUnidade As Integer,
                                     ByVal tipo As Integer,
                                     ByVal quantidadeHospede As String,
                                     ByVal ocupacaoQuartos As String,
                                     ByVal data As String,
                                     ByVal peso As String,
                                     ByVal enxoval As String,
                                     ByVal codigoUsuario As Integer)

        Try

            'Váriavies Locais
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("tipo", SqlDbType.Int, tipo),
                CriarParametro("quantidade_hospede", SqlDbType.Int, IIf(IsNumeric(quantidadeHospede), quantidadeHospede, DBNull.Value)),
                CriarParametro("ocupacao_quartos", SqlDbType.Int, IIf(IsNumeric(ocupacaoQuartos), ocupacaoQuartos, DBNull.Value)),
                CriarParametro("data", SqlDbType.Date, data),
                CriarParametro("peso", SqlDbType.VarChar, peso.Replace(".", "").Replace(",", ".")),
                CriarParametro("enxoval", SqlDbType.VarChar, enxoval),
                CriarParametro("codigo_usuario", SqlDbType.Int, codigoUsuario)
            }

            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_governanca_lavanderia_apontamento", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function LoadEnxoval(ByVal codigoEmpresa As Integer,
                                ByVal codigoUnidade As Integer,
                                ByVal tipo As Integer,
                                ByVal data As String,
                                ByVal dataInicio As String,
                                ByVal dataTermino As String) As List(Of LavanderiaEnxoval)

        Try

            'Váriavies Locais
            Dim oReturn As New List(Of LavanderiaEnxoval)

            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("tipo", SqlDbType.SmallInt, tipo),
                CriarParametro("data", SqlDbType.Date, data),
                CriarParametro("data_inicio", SqlDbType.Date, IIf(IsDate(dataInicio), dataInicio, DBNull.Value)),
                CriarParametro("data_termino", SqlDbType.Date, IIf(IsDate(dataTermino), dataTermino, DBNull.Value))
            }

            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_governanca_lavanderia_enxoval", oSqlParameter)

                While oSqlDataReader.Read

                    Dim oInfo As New LavanderiaEnxoval

                    oInfo.codigoEnxoval = oSqlDataReader.Item("codigo_enxoval")
                    oInfo.descricao = oSqlDataReader.Item("descricao")
                    oInfo.quantidade = oSqlDataReader.Item("quantidade")

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

    Public Function LoadLavanderiaInfo(ByVal codigoEmpresa As Integer,
                                       ByVal codigoUnidade As Integer,
                                       ByVal tipo As Integer,
                                       ByVal data As String) As LavanderiaEnxovalInfo

        Try

            'Váriavies Locais
            Dim oReturn As New LavanderiaEnxovalInfo
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("tipo", SqlDbType.Int, tipo),
                CriarParametro("data", SqlDbType.Date, data)
            }

            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_governanca_lavanderia_enxoval_info", oSqlParameter)

                While oSqlDataReader.Read

                    oReturn.peso = SafeGetString(oSqlDataReader, "peso").Replace(".", ",")
                    oReturn.impresso = SafeGetBoolean(oSqlDataReader, "impresso")
                    oReturn.quantidadeHospede = SafeGetLong(oSqlDataReader, "quantidade_hospede")
                    oReturn.ocupacaoQuartos = SafeGetLong(oSqlDataReader, "ocupacao_quartos")
                    oReturn.impresso = SafeGetLong(oSqlDataReader, "impresso")

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

#Region "::: LOG HISTÓRIO ALTERAÇÃO STATUS UH :::"


    Public Function LoadLogAlteracaoStatusGov(ByVal codigoEmpresa As Integer,
                                              ByVal codigoUnidade As Integer,
                                              ByVal dataInicio As String,
                                              ByVal dataTermino As String,
                                              ByVal status As String,
                                              ByVal apartamento As String) As List(Of GovernancaLogAlteracaoStatusUH)

        'Variaveis Locais
        Dim oReturn As New List(Of GovernancaLogAlteracaoStatusUH)

        Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("data_inicio", SqlDbType.Date, IIf(IsDate(dataInicio), dataInicio, DBNull.Value)),
                CriarParametro("data_termino", SqlDbType.Date, IIf(IsDate(dataTermino), dataTermino, DBNull.Value)),
                CriarParametro("status", SqlDbType.VarChar, status),
                CriarParametro("apartamento", SqlDbType.VarChar, apartamento)
            }

        Try

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_governanca_log_alteracao_status", oSqlParameter)

                While oSqlDataReader.Read

                    Dim oInfo As New GovernancaLogAlteracaoStatusUH With
                        {
                        .unidade = SafeGetString(oSqlDataReader, "unidade"),
                        .bloco = SafeGetString(oSqlDataReader, "bloco"),
                        .andar = SafeGetString(oSqlDataReader, "andar"),
                        .apartamento = SafeGetString(oSqlDataReader, "apartamento"),
                        .status = SafeGetString(oSqlDataReader, "status"),
                        .usuario = SafeGetString(oSqlDataReader, "usuario"),
                        .dataAlteracao = SafeGetString(oSqlDataReader, "data_alteracao")
                    }

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

#End Region

End Class
