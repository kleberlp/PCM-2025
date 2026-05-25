Imports PCM.WEB.MODELS
Imports System.DirectoryServices
Imports System.DirectoryServices.ActiveDirectory
Imports PCM.WEB.DAL.SQLHelper
Imports System.Data.SqlClient
Imports System.Text

Public Class Dashboard

    Private sConnection As String

    Sub New(ByVal sCon As String)
        sConnection = sCon
    End Sub

#Region "::: DASHBOARD INFO :::"

    Public Function DashboardInfo(ByVal iCodigoEmpresa As Integer,
                                  ByVal iCodigoUnidade As Integer,
                                  ByVal sData As String) As MODELS.DashboardInfo

        Try

            'Váriaveis Locais
            Dim oReturn As New MODELS.DashboardInfo
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
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Data
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sData

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_dashboard_info", oSqlParameter)

            If oSqlDataReader.HasRows Then

                While oSqlDataReader.Read

                    oReturn.unidade = oSqlDataReader("unidade")
                    oReturn.data_implantacao = oSqlDataReader("data_implantacao")
                    oReturn.quantidade_usuario = oSqlDataReader("quantidade_usuario")
                    oReturn.quantidade_equipamento = oSqlDataReader("quantidade_equipamento")
                    oReturn.equipe_manutencao = oSqlDataReader("equipe_manutencao")
                    oReturn.quantidade_os = oSqlDataReader("quantidade_os")
                    oReturn.hh_apontado = oSqlDataReader("hh_apontado")
                    oReturn.hh_corretiva = oSqlDataReader("hh_corretiva")
                    oReturn.hh_preventiva = oSqlDataReader("hh_preventiva")
                    oReturn.corretiva_preventiva = oSqlDataReader("corretiva_preventiva")
                    oReturn.rotinas_implantadas = oSqlDataReader("rotinas_implantadas")
                    oReturn.laudo = oSqlDataReader("laudo")
                    oReturn.preventiva = oSqlDataReader("preventiva")
                    oReturn.rotina = oSqlDataReader("rotina")
                    oReturn.pmoc = oSqlDataReader("pmoc")
                    oReturn.uh_dia = oSqlDataReader("uh_dia")
                    oReturn.preventivas_implantadas = oSqlDataReader("preventivas_implantadas")
                    oReturn.os_atendimento_dia = oSqlDataReader("os_atendimento_dia")
                    oReturn.os_atendimento_um_dia = oSqlDataReader("os_atendimento_um_dia")
                    oReturn.os_atendimento_tres_dias = oSqlDataReader("os_atendimento_tres_dias")
                    oReturn.os_atendimento_cinco_dias = oSqlDataReader("os_atendimento_cinco_dias")
                    oReturn.os_atendimento_acima_cinco_dias = oSqlDataReader("os_atendimento_acima_cinco_dias")
                    oReturn.os_nao_atendido = oSqlDataReader("os_nao_atendido")
                    oReturn.quantidade_colaborador_proprio = oSqlDataReader("quantidade_colaborador_proprio")
                    oReturn.quantidade_colaborador_terceiro = oSqlDataReader("quantidade_colaborador_terceiro")
                    oReturn.hh_disponivel = oSqlDataReader("hh_disponivel")
                    oReturn.faltas_justificadas = oSqlDataReader("faltas_justificadas")
                    oReturn.faltas_nao_justificadas = oSqlDataReader("faltas_nao_justificadas")
                    oReturn.horas_corretivas = oSqlDataReader("horas_corretivas")
                    oReturn.percentual_corretiva = oSqlDataReader("percentual_corretiva")
                    oReturn.horas_preventivas = oSqlDataReader("horas_preventivas")
                    oReturn.percentual_preventiva = oSqlDataReader("percentual_preventiva")
                    oReturn.horas_pmoc = oSqlDataReader("horas_pmoc")
                    oReturn.percentual_pmoc = oSqlDataReader("percentual_pmoc")
                    oReturn.horas_uh = oSqlDataReader("horas_uh")
                    oReturn.percentual_uh = oSqlDataReader("percentual_uh")
                    oReturn.horas_mapa_manutencao = oSqlDataReader("horas_mapa_manutencao")
                    oReturn.percentual_mapa_manutencao = oSqlDataReader("percentual_mapa_manutencao")
                    oReturn.horas_ociosas = oSqlDataReader("horas_ociosas")
                    oReturn.percentual_horas_ociosas = oSqlDataReader("percentual_horas_ociosas")
                    oReturn.quantidade_os_gerada = oSqlDataReader("quantidade_os_gerada")
                    oReturn.quantidade_os_atendida = oSqlDataReader("quantidade_os_atendida")
                    oReturn.quantidade_os_pendente = oSqlDataReader("quantidade_os_pendente")
                    oReturn.ranking = oSqlDataReader("ranking")

                End While

            Else

                oReturn.unidade = "TODAS AS UNIDADES"
                oReturn.data_implantacao = ""
                oReturn.quantidade_usuario = "0"
                oReturn.quantidade_equipamento = "0"
                oReturn.equipe_manutencao = "0"
                oReturn.quantidade_os = "0"
                oReturn.hh_apontado = "0"
                oReturn.hh_corretiva = "0"
                oReturn.hh_preventiva = "0"
                oReturn.corretiva_preventiva = "0"
                oReturn.rotinas_implantadas = "0"
                oReturn.preventivas_implantadas = "0"
                oReturn.laudo = "0"
                oReturn.preventiva = "0"
                oReturn.rotina = "0"
                oReturn.pmoc = "0"
                oReturn.uh_dia = "0"
                oReturn.os_atendimento_dia = "0"
                oReturn.os_atendimento_um_dia = "0"
                oReturn.os_atendimento_tres_dias = "0"
                oReturn.os_atendimento_cinco_dias = "0"
                oReturn.os_atendimento_acima_cinco_dias = "0"
                oReturn.os_nao_atendido = "0"
                oReturn.quantidade_colaborador_proprio = "0"
                oReturn.quantidade_colaborador_terceiro = "0"
                oReturn.hh_disponivel = "0"
                oReturn.faltas_justificadas = "0"
                oReturn.faltas_nao_justificadas = "0"
                oReturn.horas_corretivas = "0"
                oReturn.percentual_corretiva = "0"
                oReturn.horas_preventivas = "0"
                oReturn.percentual_preventiva = "0"
                oReturn.horas_pmoc = "0"
                oReturn.percentual_pmoc = "0"
                oReturn.horas_uh = "0"
                oReturn.percentual_uh = "0"
                oReturn.horas_mapa_manutencao = "0"
                oReturn.percentual_mapa_manutencao = "0"
                oReturn.horas_ociosas = "0"
                oReturn.percentual_horas_ociosas = "0"
                oReturn.quantidade_os_gerada = "0"
                oReturn.quantidade_os_atendida = "0"
                oReturn.quantidade_os_pendente = "0"
                oReturn.ranking = "0"

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

#End Region

#Region "::: EVOLUÇÃO MENSAL - NOTA :::"

    Public Function ChartEvolucaoMensal(ByVal iCodigoEmpresa As Integer,
                                        ByVal iCodigoUnidade As Integer,
                                        ByVal sData As String) As List(Of MODELS.chartLinhas)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of MODELS.chartLinhas)
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
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_dashboard_chart_evolucao_mensal_serie", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New chartLinhas

                oInfo.serie = oSqlDataReader("descricao")
                oInfo.valor = ChartEvolucaoMensalValor(iCodigoEmpresa:=iCodigoEmpresa,
                                                       iCodigoUnidade:=iCodigoUnidade,
                                                       sField:=oSqlDataReader.Item("serie"),
                                                       sData:=sData)

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

    Public Function ChartEvolucaoMensalValor(ByVal iCodigoEmpresa As Integer,
                                             ByVal iCodigoUnidade As Integer,
                                             ByVal sField As String,
                                             ByVal sData As String) As List(Of String)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of String)
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

            'Seta Parametros - Field
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "field"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sField : i += 1

            'Seta Parametros - Data
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sData

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_dashboard_chart_evolucao_mensal_valor", oSqlParameter)

            While oSqlDataReader.Read

                oReturn.Add(oSqlDataReader.Item("valor"))

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

#Region "::: EVOLUÇÃO MENSAL - ATENDIMENTO - OS :::"

    Public Function ChartEvolucaoMensalAtendimentoOS(ByVal iCodigoEmpresa As Integer,
                                                     ByVal iCodigoUnidade As Integer,
                                                     ByVal sData As String) As List(Of MODELS.chartLinhas)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of MODELS.chartLinhas)
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
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_dashboard_chart_evolucao_mensal_atendimento_os_serie", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New chartLinhas

                oInfo.serie = oSqlDataReader("descricao")
                oInfo.valor = ChartEvolucaoMensalAtendimentoOSValor(iCodigoEmpresa:=iCodigoEmpresa,
                                                                    iCodigoUnidade:=iCodigoUnidade,
                                                                    sField:=oSqlDataReader.Item("serie"),
                                                                    sData:=sData)

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

    Public Function ChartEvolucaoMensalAtendimentoOSValor(ByVal iCodigoEmpresa As Integer,
                                                          ByVal iCodigoUnidade As Integer,
                                                          ByVal sField As String,
                                                          ByVal sData As String) As List(Of String)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of String)
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

            'Seta Parametros - Field
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "field"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sField : i += 1

            'Seta Parametros - Data
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sData

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_dashboard_chart_evolucao_mensal_atendimento_os_valor", oSqlParameter)

            While oSqlDataReader.Read

                oReturn.Add(oSqlDataReader.Item("valor"))

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

#Region "::: EVOLUÇÃO - H.H. :::"

    Public Function ChartEvolucaoHH(ByVal iCodigoEmpresa As Integer,
                                    ByVal iCodigoUnidade As Integer,
                                    ByVal sData As String) As List(Of MODELS.chartLinhas)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of MODELS.chartLinhas)
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
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_dashboard_chart_evolucao_hora_homem_serie", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New chartLinhas

                oInfo.serie = oSqlDataReader("descricao")
                oInfo.valor = ChartEvolucaoHHValor(iCodigoEmpresa:=iCodigoEmpresa,
                                                   iCodigoUnidade:=iCodigoUnidade,
                                                   sField:=oSqlDataReader.Item("serie"),
                                                   sData:=sData)

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

    Public Function ChartEvolucaoHHValor(ByVal iCodigoEmpresa As Integer,
                                         ByVal iCodigoUnidade As Integer,
                                         ByVal sField As String,
                                         ByVal sData As String) As List(Of String)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of String)
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

            'Seta Parametros - Field
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "field"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sField : i += 1

            'Seta Parametros - Data
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sData

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_dashboard_chart_evolucao_hora_homem_valor", oSqlParameter)

            While oSqlDataReader.Read

                oReturn.Add(oSqlDataReader.Item("valor"))

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

#Region "::: EVOLUÇÃO - PMOC :::"

    Public Function ChartEvolucaoPMOC(ByVal iCodigoEmpresa As Integer,
                                      ByVal iCodigoUnidade As Integer,
                                      ByVal sData As String) As List(Of MODELS.chartLinhas)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of MODELS.chartLinhas)
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
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_dashboard_chart_evolucao_pmoc_serie", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New chartLinhas

                oInfo.serie = oSqlDataReader("descricao")
                oInfo.valor = ChartEvolucaoPMOCValor(iCodigoEmpresa:=iCodigoEmpresa,
                                                     iCodigoUnidade:=iCodigoUnidade,
                                                     sField:=oSqlDataReader.Item("serie"),
                                                     sData:=sData)

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

    Public Function ChartEvolucaoPMOCValor(ByVal iCodigoEmpresa As Integer,
                                           ByVal iCodigoUnidade As Integer,
                                           ByVal sField As String,
                                           ByVal sData As String) As List(Of String)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of String)
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

            'Seta Parametros - Field
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "field"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sField : i += 1

            'Seta Parametros - Data
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sData

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_dashboard_chart_evolucao_pmoc_valor", oSqlParameter)

            While oSqlDataReader.Read

                oReturn.Add(oSqlDataReader.Item("valor"))

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

#Region "::: SÉRIES :::"

    Public Function Unidades(ByVal iCodigoEmpresa As Integer) As List(Of String)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of String)
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
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_dashboard_chart_unidades", oSqlParameter)

            While oSqlDataReader.Read

                oReturn.Add(oSqlDataReader("nome_fantasia"))

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

    Public Function Mes(ByVal iCodigoEmpresa As Integer,
                        ByVal iCodigoUnidade As Integer,
                        ByVal sData As String) As List(Of String)

        Try

            'Váriaveis Locais
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(2) As SqlParameter
            Dim oReturn As New List(Of String)
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
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Data
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sData

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_dashboard_chart_unidades_data", oSqlParameter)

            While oSqlDataReader.Read

                oReturn.Add(oSqlDataReader("descricao"))

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

#Region "::: GERAL :::"

    Public Function RankingUnidades(ByVal iCodigoEmpresa As Integer,
                                    ByVal sData As String) As List(Of MODELS.RankingUnidades)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of MODELS.RankingUnidades)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(1) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Data
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sData

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_dashboard_ranking_unidades", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New RankingUnidades

                oInfo.ranking = oSqlDataReader("ranking")
                oInfo.unidade = oSqlDataReader("unidade")
                oInfo.nota = oSqlDataReader("nota")

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

    Public Function Percentual(ByVal iCodigoEmpresa As Integer) As MODELS.PercentualNota

        Try

            'Váriaveis Locais
            Dim oReturn As New MODELS.PercentualNota
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(1) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_configuracao_percentual_nota", oSqlParameter)

            While oSqlDataReader.Read

                oReturn.laudo = oSqlDataReader("laudo")
                oReturn.preventiva = oSqlDataReader("preventiva")
                oReturn.rotina = oSqlDataReader("rotina")
                oReturn.pmoc = oSqlDataReader("pmoc")
                oReturn.uh_dia = oSqlDataReader("uh_dia")
                oReturn.os_atendimento_dia = oSqlDataReader("os_atendimento_dia")
                oReturn.hh_nao_apontado = oSqlDataReader("hh_nao_apontado")
                oReturn.os_pendente = oSqlDataReader("os_pendente")
                oReturn.hh_extra = oSqlDataReader("hh_extra")
                oReturn.preventiva_corretiva = oSqlDataReader("preventiva_corretiva")
                oReturn.green_planet = oSqlDataReader("green_planet")

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

    Public Function NotasUnidades(ByVal iCodigoEmpresa As Integer,
                                  ByVal sData As String) As List(Of MODELS.NotasUnidades)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of MODELS.NotasUnidades)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(1) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Data
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sData

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_dashboard_notas_unidades", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New NotasUnidades

                oInfo.unidade = oSqlDataReader("unidade")
                oInfo.nota_final = oSqlDataReader("nota_final")
                oInfo.laudo = oSqlDataReader("laudo")
                oInfo.laudo_peso = oSqlDataReader("laudo_peso")
                oInfo.preventiva = oSqlDataReader("preventiva")
                oInfo.preventiva_peso = oSqlDataReader("preventiva_peso")
                oInfo.rotina = oSqlDataReader("rotina")
                oInfo.rotina_peso = oSqlDataReader("rotina_peso")
                oInfo.pmoc = oSqlDataReader("pmoc")
                oInfo.pmoc_peso = oSqlDataReader("pmoc_peso")
                oInfo.os_atendimento_dia = oSqlDataReader("os_atendimento_dia")
                oInfo.os_atendimento_dia_peso = oSqlDataReader("os_atendimento_dia_peso")
                oInfo.uh_dia = oSqlDataReader("uh_dia")
                oInfo.uh_dia_peso = oSqlDataReader("uh_dia_peso")
                oInfo.hh_nao_apontado = oSqlDataReader("hh_nao_apontado")
                oInfo.hh_nao_apontado_peso = oSqlDataReader("hh_nao_apontado_peso")
                oInfo.os_pendente = oSqlDataReader("os_pendente")
                oInfo.os_pendente_peso = oSqlDataReader("os_pendente_peso")
                oInfo.hh_extra = oSqlDataReader("hh_extra")
                oInfo.hh_extra_peso = oSqlDataReader("hh_extra_peso")
                oInfo.preventiva_corretiva = oSqlDataReader("preventiva_corretiva")
                oInfo.preventiva_corretiva_peso = oSqlDataReader("preventiva_corretiva_peso")
                oInfo.green_planet = oSqlDataReader("green_planet")
                oInfo.green_planet_peso = oSqlDataReader("green_planet_peso")

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

    Public Function MetricaUnidades(ByVal iCodigoEmpresa As Integer,
                                    ByVal sData As String,
                                    ByVal sField As String) As List(Of MODELS.MetricaUnidades)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of MODELS.MetricaUnidades)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(2) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Data
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sData : i += 1

            'Seta Parametros - Field
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "field"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sField

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_dashboard_metrica_unidades", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New MetricaUnidades

                oInfo.unidade = oSqlDataReader("unidade")
                oInfo.quantidade_ok = oSqlDataReader("quantidade_ok")
                oInfo.quantidade_pendente = oSqlDataReader("quantidade_pendente")
                oInfo.total = oSqlDataReader("total")
                oInfo.percentual_atendido = oSqlDataReader("percentual_atendido")

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

    Public Function AtendimentoOrdemServico(ByVal iCodigoEmpresa As Integer,
                                            ByVal sData As String) As List(Of MODELS.AtendimentoOrdemServico)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of MODELS.AtendimentoOrdemServico)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(1) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Data
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sData

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_dashboard_ordem_servico", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New AtendimentoOrdemServico

                oInfo.unidade = oSqlDataReader("unidade")
                oInfo.quantidade_colaborador_proprio = oSqlDataReader("quantidade_colaborador_proprio")
                oInfo.quantidade_colaborador_terceiro = oSqlDataReader("quantidade_colaborador_terceiro")
                oInfo.hh_disponivel = oSqlDataReader("hh_disponivel")
                oInfo.quantidade_os_gerada = oSqlDataReader("quantidade_os_gerada")
                oInfo.quantidade_os_atendida = oSqlDataReader("quantidade_os_atendida")
                oInfo.quantidade_os_por_funcionario = oSqlDataReader("quantidade_os_por_funcionario")
                oInfo.quantidade_os_pendente = oSqlDataReader("quantidade_os_pendente")
                oInfo.percentual_os_gerada = oSqlDataReader("percentual_os_gerada")
                oInfo.os_atendimento_dia = oSqlDataReader("os_atendimento_dia")
                oInfo.os_atendimento_um_dia = oSqlDataReader("os_atendimento_um_dia")
                oInfo.os_atendimento_tres_dias = oSqlDataReader("os_atendimento_tres_dias")
                oInfo.os_atendimento_cinco_dias = oSqlDataReader("os_atendimento_cinco_dias")
                oInfo.os_atendimento_acima_cinco_dias = oSqlDataReader("os_atendimento_acima_cinco_dias")
                oInfo.os_nao_atendido = oSqlDataReader("os_nao_atendido")

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

    Public Function ApontamentoHoras(ByVal iCodigoEmpresa As Integer,
                                     ByVal sData As String) As List(Of MODELS.ApontamentoHoras)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of MODELS.ApontamentoHoras)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(1) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Data
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sData

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_dashboard_apontamento_horas", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New ApontamentoHoras

                oInfo.unidade = oSqlDataReader.Item("unidade")
                oInfo.quantidade_colaborador_proprio = oSqlDataReader.Item("quantidade_colaborador_proprio")
                oInfo.quantidade_colaborador_terceiro = oSqlDataReader.Item("quantidade_colaborador_terceiro")
                oInfo.hh_disponivel = oSqlDataReader.Item("hh_disponivel")
                oInfo.faltas_justificadas = oSqlDataReader.Item("faltas_justificadas")
                oInfo.faltas_nao_justificadas = oSqlDataReader.Item("faltas_nao_justificadas")
                oInfo.horas_corretivas = oSqlDataReader.Item("horas_corretivas")
                oInfo.percentual_horas_corretivas = oSqlDataReader.Item("percentual_horas_corretivas")
                oInfo.horas_preventivas = oSqlDataReader.Item("horas_preventivas")
                oInfo.percentual_horas_preventivas = oSqlDataReader.Item("percentual_horas_preventivas")
                oInfo.horas_pmoc = oSqlDataReader.Item("horas_pmoc")
                oInfo.percentual_horas_pmoc = oSqlDataReader.Item("percentual_horas_pmoc")
                oInfo.horas_uh_dia = oSqlDataReader.Item("horas_uh")
                oInfo.percentual_horas_uh_dia = oSqlDataReader.Item("percentual_horas_uh")
                oInfo.total_apontamento = oSqlDataReader.Item("total_apontado")
                oInfo.saldo_hh_disponivel = oSqlDataReader.Item("saldo_hh_disponivel")
                oInfo.css_saldo_hh_disponivel = oSqlDataReader.Item("css_saldo_hh_disponivel")
                oInfo.percentual_ociosidade = oSqlDataReader.Item("percentual_ociosidade")
                oInfo.hora_extra = oSqlDataReader.Item("hora_extra")
                oInfo.css_row = oSqlDataReader.Item("css_row")

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

    Public Function ChartGeral(ByVal iCodigoEmpresa As Integer,
                               ByVal sData As String) As List(Of MODELS.chartLinhas)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of MODELS.chartLinhas)
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
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_dashboard_chart_geral_serie", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New chartLinhas

                oInfo.serie = oSqlDataReader("descricao")
                oInfo.valor = ChartGeralValor(iCodigoEmpresa:=iCodigoEmpresa,
                                              sData:=sData,
                                              sField:=oSqlDataReader.Item("serie"))

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

    Public Function ChartGeralValor(ByVal iCodigoEmpresa As Integer,
                                    ByVal sData As String,
                                    ByVal sField As String) As List(Of String)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of String)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(2) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Data
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sData : i += 1

            'Seta Parametros - Field
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "field"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sField

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_dashboard_chart_geral_valor", oSqlParameter)

            While oSqlDataReader.Read

                oReturn.Add(oSqlDataReader.Item("valor"))

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

    Public Function ChartMetrica(ByVal iCodigoEmpresa As Integer,
                                 ByVal sData As String,
                                 ByVal sField As String,
                                 ByVal sDescricao As String) As List(Of MODELS.chartLinhas)

        Try

            Dim oReturn As New List(Of MODELS.chartLinhas)
            Dim oInfo As New chartLinhas

            oInfo.serie = sDescricao
            oInfo.valor = ChartMetricaValor(iCodigoEmpresa:=iCodigoEmpresa,
                                            sData:=sData,
                                            sField:=sField)

            oReturn.Add(oInfo)

            'Retorno da Função
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ChartMetricaValor(ByVal iCodigoEmpresa As Integer,
                                      ByVal sData As String,
                                      ByVal sField As String) As List(Of String)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of String)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(2) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Data
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sData : i += 1

            'Seta Parametros - Field
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "field"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sField

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_dashboard_chart_metrica_valor", oSqlParameter)

            While oSqlDataReader.Read

                oReturn.Add(oSqlDataReader.Item("valor"))

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

    Public Function ChartNumeroOS(ByVal iCodigoEmpresa As Integer,
                                  ByVal sData As String) As List(Of MODELS.chartLinhas)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of MODELS.chartLinhas)
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
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_dashboard_chart_numero_os_serie", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New chartLinhas

                oInfo.serie = oSqlDataReader("descricao")
                oInfo.valor = ChartNumeroOSValor(iCodigoEmpresa:=iCodigoEmpresa,
                                                 sData:=sData,
                                                 sField:=oSqlDataReader.Item("serie"))

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

    Public Function ChartNumeroOSValor(ByVal iCodigoEmpresa As Integer,
                                       ByVal sData As String,
                                       ByVal sField As String) As List(Of String)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of String)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(2) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Data
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sData : i += 1

            'Seta Parametros - Field
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "field"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sField

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_dashboard_chart_numero_os_valor", oSqlParameter)

            While oSqlDataReader.Read

                oReturn.Add(oSqlDataReader.Item("valor"))

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

    Public Function ChartTempoMedioAtendimento(ByVal iCodigoEmpresa As Integer,
                                               ByVal sData As String) As List(Of MODELS.chartLinhas)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of MODELS.chartLinhas)
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
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_dashboard_chart_tempo_medio_atendimento_serie", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New chartLinhas

                oInfo.serie = oSqlDataReader("descricao")
                oInfo.valor = ChartTempoMedioAtendimentoValor(iCodigoEmpresa:=iCodigoEmpresa,
                                                              sData:=sData,
                                                              sField:=oSqlDataReader.Item("serie"))

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

    Public Function ChartTempoMedioAtendimentoValor(ByVal iCodigoEmpresa As Integer,
                                                    ByVal sData As String,
                                                    ByVal sField As String) As List(Of String)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of String)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(2) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Data
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sData : i += 1

            'Seta Parametros - Field
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "field"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sField

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_dashboard_chart_tempo_medio_atendimento_valor", oSqlParameter)

            While oSqlDataReader.Read

                oReturn.Add(oSqlDataReader.Item("valor"))

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

    Public Function ChartMediaOSColaborador(ByVal iCodigoEmpresa As Integer,
                                            ByVal sData As String) As List(Of MODELS.chartLinhas)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of MODELS.chartLinhas)
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
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_dashboard_chart_media_os_colaborador_serie", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New chartLinhas

                oInfo.serie = oSqlDataReader("descricao")
                oInfo.valor = ChartMediaOSColaboradorValor(iCodigoEmpresa:=iCodigoEmpresa,
                                                           sData:=sData,
                                                           sField:=oSqlDataReader.Item("serie"))

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

    Public Function ChartMediaOSColaboradorValor(ByVal iCodigoEmpresa As Integer,
                                                 ByVal sData As String,
                                                 ByVal sField As String) As List(Of String)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of String)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(2) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Data
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sData : i += 1

            'Seta Parametros - Field
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "field"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sField

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_dashboard_chart_media_os_colaborador_valor", oSqlParameter)

            While oSqlDataReader.Read

                oReturn.Add(oSqlDataReader.Item("valor"))

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

    Public Function ManutencaoLaudo(ByVal iCodigoEmpresa As Integer,
                                    ByVal iCodigoUnidade As Integer,
                                    ByVal sData As String,
                                    ByVal iCodigoModulo As Integer,
                                    ByVal iCodigoTipoOrdemServico As Integer,
                                    ByVal bRotina As Boolean) As List(Of ManutencaoLaudo)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of ManutencaoLaudo)
            Dim oSqlDataReader As SqlDataReader
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

            'Seta Parametros - Código Módulo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_modulo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoModulo : i += 1

            'Seta Parametros - Data
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sData : i += 1

            'Seta Parametros - Código Tipo Ordem Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_tipo_ordem_servico"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoTipoOrdemServico : i += 1

            'Seta Parametros - Rotina
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "rotina"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bRotina

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_dashboard_manutencao_laudo", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New ManutencaoLaudo

                oInfo.nome_fantasia = oSqlDataReader("nome_fantasia")
                oInfo.manutencao = oSqlDataReader("manutencao")
                oInfo.equipamento = oSqlDataReader("equipamento")
                oInfo.data_validade = oSqlDataReader("data_validade")
                oInfo.mes_1 = oSqlDataReader.Item("mes_1")

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

#Region "::: QUALIDADE :::"

    Public Function MesQualidade(ByVal iCodigoEmpresa As Integer,
                                 ByVal iCodigoUnidade As Integer,
                                 ByVal sData As String) As List(Of String)

        Try

            'Váriaveis Locais
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(2) As SqlParameter
            Dim oReturn As New List(Of String)
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
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Data
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sData

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_dashboard_chart_unidades_data_qualidade", oSqlParameter)

            While oSqlDataReader.Read

                oReturn.Add(oSqlDataReader("descricao"))

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

    Public Function QualidadeAuditoria(ByVal iCodigoEmpresa As Integer,
                                       ByVal sData As String) As List(Of MODELS.QualidadeAuditoria)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of MODELS.QualidadeAuditoria)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(1) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Data
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sData

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_dashboard_qualidade_auditoria", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New QualidadeAuditoria

                oInfo.unidade = oSqlDataReader("unidade")
                oInfo.quantidade_auditoria = oSqlDataReader("quantidade_auditoria")
                oInfo.quantidade_concluida = oSqlDataReader("quantidade_concluida")
                oInfo.quantidade_atrasada = oSqlDataReader("quantidade_atrasada")
                oInfo.quantidade_em_andamento = oSqlDataReader("quantidade_em_andamento")
                oInfo.percentual_execucao = oSqlDataReader("percentual_execucao")

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

    Public Function QualidadeNotasUnidade(ByVal iCodigoEmpresa As Integer,
                                          ByVal iCodigoUnidade As Integer,
                                          ByVal sData As String) As List(Of MODELS.QualidadeNotasUnidade)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of MODELS.QualidadeNotasUnidade)
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

            'Seta Parametros - Data
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sData

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_dashboard_qualidade_notas_unidade", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New QualidadeNotasUnidade

                oInfo.codigo_auditoria_interna = oSqlDataReader("codigo_auditoria_interna")
                oInfo.auditoria = oSqlDataReader("auditoria")
                oInfo.quantidade_auditoria = oSqlDataReader("quantidade_auditoria")
                oInfo.quantidade_concluida = oSqlDataReader("quantidade_concluida")
                oInfo.total_perguntas = oSqlDataReader("total_perguntas")
                oInfo.total_conformes = oSqlDataReader("total_conformes")
                oInfo.total_nao_conformes = oSqlDataReader("total_nao_conformes")
                oInfo.total_na = oSqlDataReader("total_na")
                oInfo.atrasada = oSqlDataReader("atrasada")
                oInfo.em_andamento = oSqlDataReader("em_andamento")
                oInfo.media_nota = oSqlDataReader("media_nota")

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

    Public Function QualidadePlanoAcaoUnidade(ByVal iCodigoEmpresa As Integer,
                                              ByVal iCodigoUnidade As Integer,
                                              ByVal sData As String) As List(Of MODELS.QualidadePlanoAcaoUnidade)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of MODELS.QualidadePlanoAcaoUnidade)
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

            'Seta Parametros - Data
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sData

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_dashboard_qualidade_plano_acao_unidade", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New QualidadePlanoAcaoUnidade

                oInfo.departamento_responsavel = oSqlDataReader("departamento_responsavel")
                oInfo.quantidade_plano_acao = oSqlDataReader("quantidade_plano_acao")
                oInfo.quantidade_concluido = oSqlDataReader("quantidade_concluido")
                oInfo.quantidade_pendente = oSqlDataReader("quantidade_pendente")
                oInfo.quantidade_atrasada = oSqlDataReader("quantidade_atrasada")
                oInfo.tempo_medio_atraso = oSqlDataReader("tempo_medio_atraso")
                oInfo.quantidade_em_andamento = oSqlDataReader("quantidade_em_andamento")
                oInfo.tempo_medio_em_andamento = oSqlDataReader("tempo_medio_em_andamento")

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

    Public Function QualidadePlanoAcaoJustificativaUnidade(ByVal iCodigoEmpresa As Integer,
                                                           ByVal iCodigoUnidade As Integer,
                                                           ByVal sData As String) As List(Of MODELS.QualidadePlanoAcaoJustificativaUnidade)
         
        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of MODELS.QualidadePlanoAcaoJustificativaUnidade)
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

            'Seta Parametros - Data
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sData

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_dashboard_qualidade_plano_acao_justificativa_unidade", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New QualidadePlanoAcaoJustificativaUnidade

                oInfo.justificativa = oSqlDataReader("justificativa")
                oInfo.quantidade = oSqlDataReader("quantidade")
                oInfo.departamento_responsavel = oSqlDataReader("departamento_responsavel")
                oInfo.quantidade_concluida = oSqlDataReader("quantidade_concluido")
                oInfo.quantidade_atrasada = oSqlDataReader("quantidade_atrasada")
                oInfo.tempo_medio_em_andamento = oSqlDataReader("tempo_medio_em_andamento")

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

    Public Function QualidadePlanoAcaoRecorrenciaUnidade(ByVal iCodigoEmpresa As Integer,
                                                         ByVal iCodigoUnidade As Integer,
                                                         ByVal iFiltro As Integer) As List(Of MODELS.QualidadePlanoAcaoRecorrenciaUnidade)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of MODELS.QualidadePlanoAcaoRecorrenciaUnidade)
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

            'Seta Parametros - Filtro
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "filtro"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iFiltro

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_dashboard_qualidade_plano_acao_recorrencia_unidade", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New QualidadePlanoAcaoRecorrenciaUnidade

                oInfo.quantidade = oSqlDataReader("quantidade_plano_acao")
                oInfo.item_plano_acao = oSqlDataReader("item_plano_acao")
                oInfo.departamento = oSqlDataReader("departamento_responsavel")
                oInfo.tempo_gasto = oSqlDataReader("tempo_gasto")
                oInfo.valor_gasto = oSqlDataReader("valor_gasto")

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

    Public Function ChartQualidadeNotasUnidade(ByVal iCodigoEmpresa As Integer,
                                               ByVal iCodigoUnidade As Integer,
                                               ByVal sData As String) As List(Of MODELS.chartLinhas)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of MODELS.chartLinhas)
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

            'Seta Parametros - Data
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sData

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_dashboard_chart_qualidade_notas_unidade", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New chartLinhas

                oInfo.serie = oSqlDataReader("descricao")
                oInfo.valor = ChartQualidadeNotasUnidadeValor(iCodigoEmpresa:=iCodigoEmpresa,
                                                              iCodigoUnidade:=iCodigoUnidade,
                                                              sData:=sData,
                                                              sField:=oSqlDataReader.Item("serie"))

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

    Public Function ChartQualidadeNotasUnidadeValor(ByVal iCodigoEmpresa As Integer,
                                                    ByVal iCodigoUnidade As Integer,
                                                    ByVal sData As String,
                                                    ByVal sField As String) As List(Of String)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of String)
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

            'Seta Parametros - Data
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sData : i += 1

            'Seta Parametros - Field
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "field"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sField

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_dashboard_chart_qualidade_notas_unidade_valor", oSqlParameter)

            While oSqlDataReader.Read

                oReturn.Add(oSqlDataReader.Item("valor"))

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

    Public Function QualidadeNotas(ByVal iCodigoEmpresa As Integer,
                                   ByVal sData As String) As List(Of MODELS.QualidadeNotas)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of MODELS.QualidadeNotas)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(1) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Data
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sData

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_dashboard_qualidade_notas", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New QualidadeNotas

                oInfo.unidade = oSqlDataReader("unidade")
                oInfo.codigo_unidade = oSqlDataReader("codigo_unidade")
                oInfo.quantidade_auditoria = oSqlDataReader("quantidade_auditoria")
                oInfo.quantidade_concluida = oSqlDataReader("quantidade_concluida")
                oInfo.total_perguntas = oSqlDataReader("total_perguntas")
                oInfo.total_conformes = oSqlDataReader("total_conformes")
                oInfo.total_nao_conformes = oSqlDataReader("total_nao_conformes")
                oInfo.total_na = oSqlDataReader("total_na")
                oInfo.atrasada = oSqlDataReader("atrasada")
                oInfo.em_andamento = oSqlDataReader("em_andamento")
                oInfo.media_nota = oSqlDataReader("media_nota")

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


    Public Function QualidadePlanoAcao(ByVal iCodigoEmpresa As Integer,
                                       ByVal sData As String) As List(Of MODELS.QualidadePlanoAcaoTodasUnidades)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of MODELS.QualidadePlanoAcaoTodasUnidades)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(2) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Data
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sData

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_dashboard_qualidade_plano_acao", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New QualidadePlanoAcaoTodasUnidades

                oInfo.unidade = oSqlDataReader("unidade")
                oInfo.quantidade_plano_acao = oSqlDataReader("quantidade_plano_acao")
                oInfo.quantidade_concluido = oSqlDataReader("quantidade_concluido")
                oInfo.quantidade_pendente = oSqlDataReader("quantidade_pendente")
                oInfo.quantidade_atrasada = oSqlDataReader("quantidade_atrasada")
                oInfo.tempo_medio_atraso = oSqlDataReader("tempo_medio_atraso")
                oInfo.quantidade_em_andamento = oSqlDataReader("quantidade_em_andamento")
                oInfo.tempo_medio_em_andamento = oSqlDataReader("tempo_medio_em_andamento")

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

    Public Function QualidadePlanoAcaoJustificativa(ByVal iCodigoEmpresa As Integer,
                                                    ByVal sData As String) As List(Of MODELS.QualidadePlanoAcaoJustificativaTodasUnidades)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of MODELS.QualidadePlanoAcaoJustificativaTodasUnidades)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(1) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Data
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sData

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_dashboard_qualidade_plano_acao_justificativa", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New QualidadePlanoAcaoJustificativaTodasUnidades

                oInfo.unidade = oSqlDataReader("unidade")
                oInfo.justificativa = oSqlDataReader("justificativa")
                oInfo.quantidade = oSqlDataReader("quantidade")
                oInfo.departamento_responsavel = oSqlDataReader("departamento_responsavel")
                oInfo.quantidade_concluida = oSqlDataReader("quantidade_concluido")
                oInfo.quantidade_atrasada = oSqlDataReader("quantidade_atrasada")
                oInfo.tempo_medio_em_andamento = oSqlDataReader("tempo_medio_em_andamento")

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

    Public Function ChartQualidadeNotas(ByVal iCodigoEmpresa As Integer,
                                        ByVal sData As String) As List(Of MODELS.chartLinhas)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of MODELS.chartLinhas)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(1) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Data
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sData

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_dashboard_chart_qualidade_notas", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New chartLinhas

                oInfo.serie = oSqlDataReader("descricao")
                oInfo.valor = ChartQualidadeNotasValor(iCodigoEmpresa:=iCodigoEmpresa,
                                                       sData:=sData,
                                                       sField:=oSqlDataReader.Item("serie"))

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

    Public Function ChartQualidadeNotasValor(ByVal iCodigoEmpresa As Integer,
                                             ByVal sData As String,
                                             ByVal sField As String) As List(Of String)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of String)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(2) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Data
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sData : i += 1

            'Seta Parametros - Field
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "field"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sField

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_dashboard_chart_qualidade_notas_valor", oSqlParameter)

            While oSqlDataReader.Read

                oReturn.Add(oSqlDataReader.Item("valor"))

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

#Region "::: DASHBOARD GOVERNANÇA :::"

    Public Function DashboardGovernancaInfo(ByVal codigoEmpresa As Integer,
                                            ByVal codigoUnidade As Integer,
                                            ByVal data As String) As List(Of DashboardGovernancaInfo)

        Try


            'Váriaveis Locais
            Dim _return As New List(Of DashboardGovernancaInfo)

            Dim _sqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("data", SqlDbType.Date, data)
            }

            'Executa Query
            Using _sqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_govenanca_desempenho", _sqlParameter)

                If _sqlDataReader.HasRows Then

                    While _sqlDataReader.Read

                        Dim _info As New DashboardGovernancaInfo With {
                            .unidade = SafeGetString(_sqlDataReader, "unidade"),
                            .quantidadeCamareiras = SafeGetString(_sqlDataReader, "qtd_camareiras"),
                            .quantidadeSupervisores = SafeGetString(_sqlDataReader, "qtd_surpevisores"),
                            .uhsArrumadas = SafeGetString(_sqlDataReader, "uhs_arrumadas"),
                            .uhsPermanencia = SafeGetString(_sqlDataReader, "uhs_permanencia"),
                            .uhsSaida = SafeGetString(_sqlDataReader, "uhs_saida"),
                            .uhsVistoriadas = SafeGetString(_sqlDataReader, "uhs_vistoriadas"),
                            .percentualVistoria = SafeGetString(_sqlDataReader, "percentual_vistoria"),
                            .quantidadeOSManutencao = SafeGetString(_sqlDataReader, "qtd_os_manutencao"),
                            .quantidadeNC = SafeGetString(_sqlDataReader, "qtd_nc"),
                            .quantidadeRetrabalho = SafeGetString(_sqlDataReader, "qtd_retrabalho"),
                            .quantidadeAlteracaoStatus = SafeGetString(_sqlDataReader, "qtd_alteracao_status")
                        }

                        _return.Add(_info)

                    End While

                End If

            End Using

            Return _return

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

End Class
