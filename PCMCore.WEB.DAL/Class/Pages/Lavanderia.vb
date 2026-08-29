Imports System.Data.SqlClient
Imports PCM.WEB.DAL.SQLHelper
Imports PCM.WEB.MODELS

Public Class Lavanderia

    Private sConnection As String

    Sub New(ByVal sCon As String)
        sConnection = sCon
    End Sub

#Region "::: APONTAMENTO :::"

    Public Sub Apontamento(ByVal codigoEmpresa As Integer,
                           ByVal codigoUnidade As Integer,
                           ByVal codigoCliente As Integer,
                           ByVal data As String,
                           ByVal peso As String,
                           ByVal pesoRelave As String,
                           ByVal equipamento As String,
                           ByVal enxoval As String,
                           ByVal funcionario As String,
                           ByVal codigoUsuario As Integer)

        Try

            'Váriavies Locais
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("codigo_cliente", SqlDbType.Int, codigoCliente),
                CriarParametro("data", SqlDbType.Date, data),
                CriarParametro("peso", SqlDbType.VarChar, peso.Replace(".", "").Replace(",", ".")),
                CriarParametro("peso_relave", SqlDbType.VarChar, pesoRelave.Replace(".", "").Replace(",", ".")),
                CriarParametro("equipamento", SqlDbType.VarChar, equipamento),
                CriarParametro("enxoval", SqlDbType.VarChar, enxoval),
                CriarParametro("funcionario", SqlDbType.VarChar, funcionario),
                CriarParametro("codigo_usuario", SqlDbType.Int, codigoUsuario)
            }

            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_lavanderia_apontamento", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function LoadEquipamento(ByVal codigoEmpresa As Integer,
                                    ByVal codigoUnidade As Integer) As List(Of LavanderiaEquipamento)

        Try

            'Váriavies Locais
            Dim oReturn As New List(Of LavanderiaEquipamento)
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade)
            }

            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_lavanderia_equipamento", oSqlParameter)

                While oSqlDataReader.Read

                    Dim oInfo As New LavanderiaEquipamento

                    oInfo.tag = oSqlDataReader.Item("tag")
                    oInfo.descricao = oSqlDataReader.Item("descricao")
                    oInfo.codigoEquipamento = oSqlDataReader.Item("codigo_equipamento")
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

    Public Function LoadEnxoval(ByVal codigoEmpresa As Integer,
                                ByVal codigoUnidade As Integer,
                                ByVal codigoCliente As Integer) As List(Of LavanderiaEnxoval)

        Try

            'Váriavies Locais
            Dim oReturn As New List(Of LavanderiaEnxoval)
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("codigo_cliente", SqlDbType.Int, codigoCliente)
            }

            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_lavanderia_enxoval", oSqlParameter)

                While oSqlDataReader.Read

                    Dim oInfo As New LavanderiaEnxoval

                    oInfo.codigoEnxoval = oSqlDataReader.Item("codigo_enxoval")
                    oInfo.descricao = oSqlDataReader.Item("descricao")
                    oInfo.quantidade = oSqlDataReader.Item("quantidade")
                    oInfo.quantidadeRelave = oSqlDataReader.Item("quantidade_relave")

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

    Public Function LoadFuncionario(ByVal codigoEmpresa As Integer,
                                    ByVal codigoUnidade As Integer) As List(Of LavanderiaFuncionario)

        Try

            'Váriavies Locais
            Dim oReturn As New List(Of LavanderiaFuncionario)
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade)
            }

            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_lavanderia_funcionario", oSqlParameter)

                While oSqlDataReader.Read

                    Dim oInfo As New LavanderiaFuncionario

                    oInfo.codigo = oSqlDataReader.Item("codigo")
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

#End Region

#Region "::: HISTÓRICO :::"

    Public Function Historico(ByVal codigoEmpresa As Integer,
                              ByVal codigoUnidade As Integer,
                              ByVal codigoCliente As Integer,
                              ByVal codigoEquipamento As Long,
                              ByVal dataInicio As String,
                              ByVal dataTermino As String,
                              ByVal codigoFuncionario As Integer) As List(Of HistoricoApontamentoLavanderia)

        Try

            'Váriavies Locais
            Dim oReturn As New List(Of HistoricoApontamentoLavanderia)
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("codigo_cliente", SqlDbType.Int, codigoCliente),
                CriarParametro("codigo_equipamento", SqlDbType.BigInt, codigoEquipamento),
                CriarParametro("codigo_funcionario", SqlDbType.BigInt, codigoFuncionario),
                CriarParametro("data_inicio", SqlDbType.Date, dataInicio),
                CriarParametro("data_termino", SqlDbType.Date, dataTermino)
            }

            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_lavanderia_apontamento_historico", oSqlParameter)

                While oSqlDataReader.Read

                    Dim info As New HistoricoApontamentoLavanderia

                    info.codigoEmpresa = oSqlDataReader.Item("codigo_empresa")
                    info.codigoUnidade = oSqlDataReader.Item("codigo_unidade")
                    info.codigo = oSqlDataReader.Item("codigo")
                    info.data = oSqlDataReader.Item("data")
                    info.cliente = oSqlDataReader.Item("cliente")
                    info.funcionario = oSqlDataReader.Item("funcionario")
                    info.horaInicio = oSqlDataReader.Item("hora_inicio").ToString().Substring(0, 5)
                    info.horaTermino = oSqlDataReader.Item("hora_termino").ToString().Substring(0, 5)
                    info.tempoGasto = oSqlDataReader.Item("tempo_gasto")
                    info.peso = oSqlDataReader.Item("peso")
                    info.pesoRelavagem = oSqlDataReader.Item("peso_relavagem")
                    info.pesoTotal = oSqlDataReader.Item("peso_total")

                    oReturn.Add(info)

                End While

            End Using

            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function HistoricoDetalhe(ByVal codigoEmpresa As Integer,
                                     ByVal codigoUnidade As Integer,
                                     ByVal codigo As Long) As HistoricoApontamentoLavanderiaDetalhe

        Try

            'Váriavies Locais
            Dim oReturn As New HistoricoApontamentoLavanderiaDetalhe
            Dim oEnxoval As New List(Of HistoricoApontamentoLavanderiaEnxoval)
            Dim oEquipamento As New List(Of HistoricoApontamentoLavanderiaEquipamento)
            Dim oFuncionario As New List(Of HistoricoApontamentoLavanderiaTipoFuncionario)

            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("codigo", SqlDbType.BigInt, codigo)
            }

            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_lavanderia_apontamento_detalhe_historico", oSqlParameter)

                While oSqlDataReader.Read

                    Dim info As New HistoricoApontamentoLavanderiaEnxoval

                    info.enxoval = oSqlDataReader.Item("enxoval")
                    info.peso = oSqlDataReader.Item("peso")
                    info.pesoRelave = oSqlDataReader.Item("peso_relavagem")
                    info.pesoTotal = oSqlDataReader.Item("peso_total")

                    oEnxoval.Add(info)

                End While

                oSqlDataReader.NextResult()

                While oSqlDataReader.Read

                    Dim infoEquipamento As New HistoricoApontamentoLavanderiaEquipamento

                    infoEquipamento.equipamento = oSqlDataReader.Item("equipamento")
                    infoEquipamento.quantidade = oSqlDataReader.Item("quantidade")

                    oEquipamento.Add(infoEquipamento)

                End While

                oSqlDataReader.NextResult()

                While oSqlDataReader.Read

                    Dim infoFuncionario As New HistoricoApontamentoLavanderiaTipoFuncionario

                    infoFuncionario.tipoFuncionario = oSqlDataReader.Item("descricao")
                    infoFuncionario.quantidade = oSqlDataReader.Item("quantidade")

                    oFuncionario.Add(infoFuncionario)

                End While

            End Using

            oReturn.enxoval = oEnxoval
            oReturn.equipamento = oEquipamento
            oReturn.funcionario = oFuncionario

            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Sub DeleteApontamento(ByVal codigoEmpresa As Integer,
                                 ByVal codigoUnidade As Integer,
                                 ByVal codigo As Long)

        Try

            'Váriavies Locais
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("codigo", SqlDbType.BigInt, codigo)
            }

            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_lavanderia_apontamento", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region

#Region "::: RELATÓRIO - CONTROLE LAVAGEM :::"

    Public Function RelatorioControleLavagem(ByVal codigoEmpresa As Integer,
                                             ByVal codigoUnidade As Integer,
                                             ByVal codigoCliente As Integer,
                                             ByVal codigoEquipamento As Long,
                                             ByVal codigoFuncionario As Integer,
                                             ByVal data As String,
                                             ByVal agrupadoPor As String) As List(Of RelatorioControleLavagem)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of RelatorioControleLavagem)

            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("codigo_cliente", SqlDbType.Int, codigoCliente),
                CriarParametro("codigo_equipamento", SqlDbType.BigInt, codigoEquipamento),
                CriarParametro("codigo_funcionario", SqlDbType.BigInt, codigoFuncionario),
                CriarParametro("data", SqlDbType.Date, IIf(IsDate(data), data, DBNull.Value)),
                CriarParametro("agrupado_por", SqlDbType.VarChar, agrupadoPor)
            }

            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_report_lavagem_controle_geral", oSqlParameter)

                While oSqlDataReader.Read

                    Dim oInfo As New RelatorioControleLavagem

                    oInfo.descricao = oSqlDataReader.Item("descricao")
                    oInfo.dia = oSqlDataReader.Item("dia")
                    oInfo.quiloLavagem = oSqlDataReader.Item("quilo_lavagem")
                    oInfo.quiloRelave = oSqlDataReader.Item("quilo_relave")
                    oInfo.percentualRelave = oSqlDataReader.Item("percentual_relave")
                    oInfo.cssClassPercentualRelave = oSqlDataReader.Item("css_relave")
                    oInfo.quiloHH = oSqlDataReader.Item("quilo_hh")
                    oInfo.maquinadas = oSqlDataReader.Item("maquinadas")
                    oInfo.total = oSqlDataReader.Item("total")

                    oReturn.Add(oInfo)

                End While

                ' Retornar o relatório
                Return oReturn

            End Using

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    'Public Function RelatorioControleLavagemOld(ByVal codigoEmpresa As Integer,
    '                                         ByVal codigoUnidade As Integer,
    '                                         ByVal codigoCliente As Integer,
    '                                         ByVal codigoEquipamento As Long,
    '                                         ByVal codigoFuncionario As Integer,
    '                                         ByVal data As String,
    '                                         ByVal agrupadoPor As String) As List(Of RelatorioControleLavagem)

    '    Try

    '        'Váriaveis Locais
    '        Dim oRelatorio As New List(Of RelatorioControleLavagem)
    '        Dim oRelatorioTotal As New RelatorioControleLavagem
    '        Dim i As Integer = 0

    '        Dim oSqlParameter As SqlParameter() = {
    '            CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
    '            CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
    '            CriarParametro("codigo_cliente", SqlDbType.Int, codigoCliente),
    '            CriarParametro("codigo_equipamento", SqlDbType.BigInt, codigoEquipamento),
    '            CriarParametro("codigo_funcionario", SqlDbType.BigInt, codigoFuncionario),
    '            CriarParametro("data", SqlDbType.Date, IIf(IsDate(data), data, DBNull.Value)),
    '            CriarParametro("agrupado_por", SqlDbType.VarChar, agrupadoPor)
    '        }

    '        Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_report_lavagem_controle", oSqlParameter)

    '            Dim dias As String() = {"dia1", "dia2", "dia3", "dia4", "dia5", "dia6", "dia7", "dia8", "dia9", "dia10", "dia11", "dia12", "dia13", "dia14", "dia15", "dia16",
    '                                    "dia17", "dia18", "dia19", "dia20", "dia21", "dia22", "dia23", "dia24", "dia25", "dia26", "dia27", "dia28", "dia29", "dia30", "dia31"}

    '            If oSqlDataReader.HasRows Then

    '                While oSqlDataReader.Read

    '                    Dim oRelatorioInfo As New RelatorioControleLavagem
    '                    Dim total As Double = 0
    '                    Dim totalRelave As Double = 0

    '                    ' Processar horas e faltas para cada mês
    '                    For Each mes In dias
    '                        ' Processar horas
    '                        Dim valor = GetCampoDouble(oSqlDataReader, $"{mes}")
    '                        Dim valorRelave = GetCampoDouble(oSqlDataReader, $"{mes}Relave")
    '                        total += valor
    '                        totalRelave += valorRelave
    '                        CallByName(oRelatorioInfo, $"{mes}", CallType.Let, valor)
    '                        CallByName(oRelatorioInfo, $"{mes}Relave", CallType.Let, valorRelave)
    '                        CallByName(oRelatorioInfo, "sequence", CallType.Let, 1)
    '                        CallByName(oRelatorioInfo, "descricao", CallType.Let, oSqlDataReader.Item("descricao"))

    '                        ' Atualizar totais
    '                        CallByName(oRelatorioTotal, $"{mes}", CallType.Let, valor)
    '                        CallByName(oRelatorioTotal, $"{mes}Relave", CallType.Let, valorRelave)

    '                    Next


    '                    CallByName(oRelatorioInfo, $"total", CallType.Let, total)
    '                    CallByName(oRelatorioInfo, $"totalRelave", CallType.Let, totalRelave)

    '                    totalGeral += total
    '                    totalGeralRelave += totalRelave

    '                    ' Adicionar informações ao relatório
    '                    oRelatorio.Add(oRelatorioInfo)

    '                End While

    '                ' Adicionar total ao relatório
    '                oRelatorioTotal.descricao = "TOTAL"
    '                oRelatorioTotal.sequence = 2
    '                CallByName(oRelatorioTotal, $"total", CallType.Let, totalGeral)
    '                CallByName(oRelatorioTotal, $"totalRelave", CallType.Let, totalGeralRelave)

    '                oRelatorio.Add(oRelatorioTotal)

    '            End If

    '            ' Retornar o relatório
    '            Return oRelatorio

    '        End Using

    '    Catch SqlEx As SqlException
    '        Throw SqlEx
    '    Catch ex As Exception
    '        Throw ex
    '    End Try

    'End Function

#End Region

End Class
