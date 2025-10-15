Imports PCM.WEB.MODELS
Imports System.DirectoryServices
Imports System.DirectoryServices.ActiveDirectory
Imports PCM.WEB.DAL.SQLHelper
Imports System.Data.SqlClient
Imports System.Text
Imports MS.Internal.Text.TextInterface

Public Class Relatorio

    Private sConnection As String

    Sub New(ByVal sCon As String)
        sConnection = sCon
    End Sub

#Region "::: RELATÓRIOS :::"

    Public Function Meses(ByVal sDataInicio As Date,
                          ByVal sDataTermino As Date) As List(Of String)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of String)

            For i = 0 To DateDiff(DateInterval.Month, sDataInicio, sDataTermino)

                Dim oReturnInfo As String = ""

                oReturnInfo = DatePart(DateInterval.Month, DateAdd(DateInterval.Month, i, sDataInicio)).ToString.PadLeft(2, "0") & "/" & DatePart(DateInterval.Year, DateAdd(DateInterval.Month, i, sDataInicio))

                oReturn.Add(oReturnInfo)

            Next

            'Retorno da Função
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: CUSTO - HORAS TRABALHADAS :::"

    Public Function CustoHorasTrabalhadas(ByVal iCodigoEmpresa As Integer,
                                          ByVal iCodigoUnidade As Integer,
                                          ByVal iCodigoModulo As Integer,
                                          ByVal sDataInicio As String,
                                          ByVal sDataTermino As String,
                                          ByVal iAtivo As Integer) As List(Of CustoHorasTrabalhadas)

        Try

            'Váriaveis Locais
            Dim oRelatorio As New List(Of CustoHorasTrabalhadas)
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

            'Seta Parametros - Código Modulo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_modulo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoModulo : i += 1

            'Seta Parametros - Data Início
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_inicio"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataInicio : i += 1

            'Seta Parametros - Data Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_termino"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataTermino : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iAtivo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_report_manutencao_custo_horas", oSqlParameter)

            While oSqlDataReader.Read

                Dim oRelatorioInfo As New CustoHorasTrabalhadas

                oRelatorioInfo.funcionario = oSqlDataReader("funcionario")
                oRelatorioInfo.codigo_funcionario = oSqlDataReader("codigo_funcionario")
                oRelatorioInfo.codigo_unidade = oSqlDataReader("codigo_unidade")
                oRelatorioInfo.total = oSqlDataReader.Item("total")
                oRelatorioInfo.falta = oSqlDataReader.Item("falta")
                oRelatorioInfo.valores = CustoHorasTrabalhadasMes(iCodigoEmpresa:=iCodigoEmpresa,
                                                                  iCodigoUnidade:=oSqlDataReader("codigo_unidade"),
                                                                  iCodigoFuncionario:=oSqlDataReader.Item("codigo_funcionario"),
                                                                  sDataInicio:=sDataInicio,
                                                                  sDataTermino:=sDataTermino)

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

    Public Function CustoHorasTrabalhadasMes(ByVal iCodigoEmpresa As Integer,
                                             ByVal iCodigoUnidade As Integer,
                                             ByVal iCodigoFuncionario As Integer,
                                             ByVal sDataInicio As String,
                                             ByVal sDataTermino As String) As List(Of CustoHorasTrabalhadasMesValor)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of CustoHorasTrabalhadasMesValor)
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

            'Seta Parametros - Código Funcionário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_funcionario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoFuncionario : i += 1

            'Seta Parametros - Data Início
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_inicio"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataInicio : i += 1

            'Seta Parametros - Data Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_termino"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataTermino

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_report_manutencao_custo_horas_mes", oSqlParameter)

            While oSqlDataReader.Read

                Dim oReturnInfo As New CustoHorasTrabalhadasMesValor

                oReturnInfo.mes = oSqlDataReader("mes")
                oReturnInfo.ano = oSqlDataReader("ano")
                oReturnInfo.valor = oSqlDataReader("valor")
                oReturnInfo.falta = oSqlDataReader("falta")

                oReturn.Add(oReturnInfo)

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

    Public Function ChartCustoHorasTrabalhadas(ByVal iCodigoEmpresa As Integer,
                                               ByVal iCodigoUnidade As Integer,
                                               ByVal iCodigoModulo As Integer,
                                               ByVal sDataInicio As String,
                                               ByVal sDataTermino As String,
                                               ByVal iAtivo As Integer) As List(Of chartCustoHorasTrabalhadas)

        Try

            'Váriaveis Locais
            Dim oRelatorio As New List(Of chartCustoHorasTrabalhadas)
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

            'Seta Parametros - Código Modulo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_modulo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoModulo : i += 1

            'Seta Parametros - Data Início
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_inicio"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataInicio : i += 1

            'Seta Parametros - Data Término  
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_termino"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataTermino : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iAtivo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_chart_manutencao_custo_horas", oSqlParameter)

            While oSqlDataReader.Read

                Dim oRelatorioInfo As New chartCustoHorasTrabalhadas

                oRelatorioInfo.nome = oSqlDataReader("funcionario")
                oRelatorioInfo.valor = ChartCustoHorasTrabalhadasMes(iCodigoEmpresa:=iCodigoEmpresa,
                                                                     iCodigoUnidade:=iCodigoUnidade,
                                                                     sDataInicio:=sDataInicio,
                                                                     sDataTermino:=sDataTermino,
                                                                     iCodigoFuncionario:=oSqlDataReader.Item("codigo"))

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

    Public Function ChartCustoHorasTrabalhadasMes(ByVal iCodigoEmpresa As Integer,
                                                  ByVal iCodigoUnidade As Integer,
                                                  ByVal sDataInicio As String,
                                                  ByVal sDataTermino As String,
                                                  ByVal iCodigoFuncionario As Integer) As List(Of String)

        Try

            'Váriaveis Locais
            Dim oValor As New List(Of String)
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

            'Seta Parametros - Data Início
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_inicio"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataInicio : i += 1

            'Seta Parametros - Data Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_termino"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataTermino : i += 1

            'Seta Parametros - Código Funcionário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_funcionario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoFuncionario

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_chart_manutencao_custo_horas_valor", oSqlParameter)

            While oSqlDataReader.Read

                oValor.Add(oSqlDataReader.Item("valor"))

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oValor

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: FUNCIONÁRIO - HORAS TRABALHADAS :::"

    Public Function FuncionarioHorasTrabalhadas(ByVal iCodigoEmpresa As Integer,
                                                ByVal iCodigoUnidade As Integer,
                                                ByVal iCodigoModulo As Integer,
                                                ByVal sDataInicio As String,
                                                ByVal sDataTermino As String,
                                                ByVal iAgrupadoPorUnidade As Integer,
                                                ByVal iAtivo As Integer) As List(Of FuncionarioHorasTrabalhadas)

        Try

            'Váriaveis Locais
            Dim oRelatorio As New List(Of FuncionarioHorasTrabalhadas)
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

            'Seta Parametros - Código Modulo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_modulo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoModulo : i += 1

            'Seta Parametros - Data Início
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_inicio"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataInicio : i += 1

            'Seta Parametros - Data Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_termino"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataTermino : i += 1

            'Seta Parametros - Agrupado por Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "agrupado_por_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iAgrupadoPorUnidade : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iAtivo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_report_manutencao_funcionario_horas", oSqlParameter)

            While oSqlDataReader.Read

                Dim oRelatorioInfo As New FuncionarioHorasTrabalhadas

                oRelatorioInfo.unidade = oSqlDataReader("unidade")
                oRelatorioInfo.funcionario = oSqlDataReader("funcionario")
                oRelatorioInfo.codigo_funcionario = oSqlDataReader("codigo_funcionario")
                oRelatorioInfo.codigo_unidade = oSqlDataReader("codigo_unidade")
                oRelatorioInfo.total = oSqlDataReader.Item("total")
                oRelatorioInfo.falta = oSqlDataReader.Item("falta")

                oRelatorio.Add(oRelatorioInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            For Each oReturnInfo As FuncionarioHorasTrabalhadas In oRelatorio

                oReturnInfo.valores = FuncionarioHorasTrabalhadasMes(iCodigoEmpresa:=iCodigoEmpresa,
                                                                    iCodigoUnidade:=oReturnInfo.codigo_unidade,
                                                                    iCodigoFuncionario:=oReturnInfo.codigo_funcionario,
                                                                    sDataInicio:=sDataInicio,
                                                                    sDataTermino:=sDataTermino)

            Next

            'Retorno da Função
            Return oRelatorio

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function FuncionarioHorasTrabalhadasMes(ByVal iCodigoEmpresa As Integer,
                                                   ByVal iCodigoUnidade As Integer,
                                                   ByVal iCodigoFuncionario As Integer,
                                                   ByVal sDataInicio As String,
                                                   ByVal sDataTermino As String) As List(Of FuncionarioHorasTrabalhadasMesValor)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of FuncionarioHorasTrabalhadasMesValor)
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

            'Seta Parametros - Código Funcionário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_funcionario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoFuncionario : i += 1

            'Seta Parametros - Data Início
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_inicio"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataInicio : i += 1

            'Seta Parametros - Data Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_termino"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataTermino

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_report_manutencao_funcionario_horas_mes", oSqlParameter)

            While oSqlDataReader.Read

                Dim oReturnInfo As New FuncionarioHorasTrabalhadasMesValor

                oReturnInfo.mes = oSqlDataReader("mes")
                oReturnInfo.ano = oSqlDataReader("ano")
                oReturnInfo.valor = oSqlDataReader("valor")
                oReturnInfo.minutos = oSqlDataReader("minutos")
                oReturnInfo.falta_minutos = oSqlDataReader("falta_minutos")
                oReturnInfo.falta = oSqlDataReader("falta")

                oReturn.Add(oReturnInfo)

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

    Public Function ChartFuncionarioHorasTrabalhadas(ByVal iCodigoEmpresa As Integer,
                                                     ByVal iCodigoUnidade As Integer,
                                                     ByVal iCodigoModulo As Integer,
                                                     ByVal sDataInicio As String,
                                                     ByVal sDataTermino As String,
                                                     ByVal iAtivo As Integer) As List(Of chartFuncionarioHorasTrabalhadas)

        Try

            'Váriaveis Locais
            Dim oRelatorio As New List(Of chartFuncionarioHorasTrabalhadas)
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

            'Seta Parametros - Código Modulo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_modulo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoModulo : i += 1

            'Seta Parametros - Data Início
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_inicio"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataInicio : i += 1

            'Seta Parametros - Data Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_termino"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataTermino : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iAtivo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_chart_manutencao_funcionario_horas", oSqlParameter)

            While oSqlDataReader.Read

                Dim oRelatorioInfo As New chartFuncionarioHorasTrabalhadas

                oRelatorioInfo.nome = oSqlDataReader("funcionario")
                oRelatorioInfo.valor = ChartFuncionarioHorasTrabalhadasMes(iCodigoEmpresa:=iCodigoEmpresa,
                                                                           iCodigoUnidade:=iCodigoUnidade,
                                                                           sDataInicio:=sDataInicio,
                                                                           sDataTermino:=sDataTermino,
                                                                           iCodigoFuncionario:=oSqlDataReader.Item("codigo"))

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

    Public Function ChartFuncionarioHorasTrabalhadasMes(ByVal iCodigoEmpresa As Integer,
                                                        ByVal iCodigoUnidade As Integer,
                                                        ByVal sDataInicio As String,
                                                        ByVal sDataTermino As String,
                                                        ByVal iCodigoFuncionario As Integer) As List(Of String)

        Try

            'Váriaveis Locais
            Dim oValor As New List(Of String)
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

            'Seta Parametros - Data Início
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_inicio"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataInicio : i += 1

            'Seta Parametros - Data Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_termino"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataTermino : i += 1

            'Seta Parametros - Código Funcionário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_funcionario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoFuncionario

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_chart_manutencao_funcionario_horas_valor", oSqlParameter)

            While oSqlDataReader.Read

                oValor.Add(oSqlDataReader.Item("valor"))

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oValor

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: MANUTENÇÃO - SOLICITANTE :::"

    Public Function FuncionarioHorasTrabalhadas(ByVal iCodigoEmpresa As Integer,
                                                ByVal iCodigoUnidade As Integer,
                                                ByVal iCodigoFuncionario As Integer,
                                                ByVal iAtivo As Integer,
                                                ByVal iAno As Integer) As List(Of FuncionarioHoras)

        Try

            'Váriaveis Locais
            Dim oRelatorio As New List(Of FuncionarioHoras)
            Dim oRelatorioTotal As New FuncionarioHoras
            Dim i As Integer = 0

            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, iCodigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, iCodigoUnidade),
                CriarParametro("codigo_funcionario", SqlDbType.Int, iCodigoFuncionario),
                CriarParametro("ativo", SqlDbType.Bit, If(iAtivo = -1, DBNull.Value, If(iAtivo = 1, True, False))),
                CriarParametro("ano", SqlDbType.Int, iAno)
            }

            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_report_manutencao_funcionario_horas_mes_new", oSqlParameter)

                Dim meses As String() = {"janeiro", "fevereiro", "marco", "abril", "maio", "junho", "julho", "agosto", "setembro", "outubro", "novembro", "dezembro"}

                While oSqlDataReader.Read
                    Dim oRelatorioInfo As New FuncionarioHoras
                    Dim totalHorasFuncionario As Integer = 0
                    Dim totalFaltasFuncionario As Integer = 0
                    Dim totalHoras As Integer = 0
                    Dim totalFaltas As Integer = 0

                    ' Processar horas e faltas para cada mês
                    For Each mes In meses
                        ' Processar horas
                        Dim horas = ProcessarCampoHoras(oSqlDataReader, $"{mes}_horas")
                        Dim horasNumerico = GetCampoNumerico(oSqlDataReader, $"{mes}_horas")
                        totalHoras += horasNumerico
                        totalHorasFuncionario += horasNumerico
                        CallByName(oRelatorioInfo, $"{mes}Horas", CallType.Let, horas)
                        CallByName(oRelatorioInfo, "sequence", CallType.Let, 1)
                        CallByName(oRelatorioInfo, "funcionario", CallType.Let, oSqlDataReader.Item("funcionario"))
                        CallByName(oRelatorioInfo, "codigoUnidade", CallType.Let, oSqlDataReader.Item("codigo_unidade"))
                        CallByName(oRelatorioInfo, "codigoFuncionario", CallType.Let, oSqlDataReader.Item("codigo_funcionario"))

                        ' Processar faltas
                        Dim faltas = ProcessarCampoHoras(oSqlDataReader, $"{mes}_faltas")
                        Dim faltasNumerico = GetCampoNumerico(oSqlDataReader, $"{mes}_faltas")
                        totalFaltas += faltasNumerico
                        totalFaltasFuncionario += faltasNumerico
                        CallByName(oRelatorioInfo, $"{mes}Faltas", CallType.Let, faltas)

                        ' Atualizar totais
                        Dim horasAtuais = ConverterParaMinutos(CallByName(oRelatorioTotal, $"{mes}Horas", CallType.Get))
                        Dim novoTotalHoras = horasAtuais + horasNumerico
                        CallByName(oRelatorioTotal, $"{mes}Horas", CallType.Let, ConverterParaHorasFormatadas(novoTotalHoras))

                        Dim faltasAtuais = ConverterParaMinutos(CallByName(oRelatorioTotal, $"{mes}Faltas", CallType.Get))
                        Dim novoTotalFaltas = faltasAtuais + faltasNumerico
                        CallByName(oRelatorioTotal, $"{mes}Faltas", CallType.Let, ConverterParaHorasFormatadas(novoTotalFaltas))
                    Next


                    CallByName(oRelatorioInfo, $"totalHoras", CallType.Let, ConverterParaHorasFormatadas(totalHorasFuncionario))
                    CallByName(oRelatorioInfo, $"totalFaltas", CallType.Let, ConverterParaHorasFormatadas(totalFaltasFuncionario))

                    ' Adicionar informações ao relatório
                    oRelatorio.Add(oRelatorioInfo)
                End While

                ' Adicionar total ao relatório
                oRelatorioTotal.funcionario = "TOTAL"
                oRelatorioTotal.sequence = 2
                oRelatorio.Add(oRelatorioTotal)

                ' Retornar o relatório
                Return oRelatorio

            End Using

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: MANUTENÇÃO - SOLICITANTE :::"

    Public Function FuncionarioHorasTrabalhadasGovernanca(ByVal iCodigoEmpresa As Integer,
                                                          ByVal iCodigoUnidade As Integer,
                                                          ByVal iCodigoFuncionario As Integer,
                                                          ByVal iAtivo As Integer,
                                                          ByVal iAno As Integer) As List(Of FuncionarioHoras)

        Try

            'Váriaveis Locais
            Dim oRelatorio As New List(Of FuncionarioHoras)
            Dim oRelatorioTotal As New FuncionarioHoras
            Dim i As Integer = 0

            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, iCodigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, iCodigoUnidade),
                CriarParametro("codigo_funcionario", SqlDbType.Int, iCodigoFuncionario),
                CriarParametro("ativo", SqlDbType.Int, If(iAtivo = -1, DBNull.Value, If(iAtivo = 1, True, False))),
                CriarParametro("ano", SqlDbType.Int, iAno)
            }

            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_report_manutencao_funcionario_horas_governanca_new", oSqlParameter)

                Dim meses As String() = {"janeiro", "fevereiro", "marco", "abril", "maio", "junho", "julho", "agosto", "setembro", "outubro", "novembro", "dezembro"}

                While oSqlDataReader.Read
                    Dim oRelatorioInfo As New FuncionarioHoras
                    Dim totalHorasFuncionario As Integer = 0
                    Dim totalFaltasFuncionario As Integer = 0
                    Dim totalHoras As Integer = 0
                    Dim totalFaltas As Integer = 0

                    ' Processar horas e faltas para cada mês
                    For Each mes In meses
                        ' Processar horas
                        Dim horas = ProcessarCampoHoras(oSqlDataReader, $"{mes}_horas")
                        Dim horasNumerico = GetCampoNumerico(oSqlDataReader, $"{mes}_horas")
                        totalHoras += horasNumerico
                        totalHorasFuncionario += horasNumerico
                        CallByName(oRelatorioInfo, $"{mes}Horas", CallType.Let, horas)
                        CallByName(oRelatorioInfo, "sequence", CallType.Let, 1)
                        CallByName(oRelatorioInfo, "funcionario", CallType.Let, oSqlDataReader.Item("funcionario"))
                        CallByName(oRelatorioInfo, "codigoUnidade", CallType.Let, oSqlDataReader.Item("codigo_unidade"))
                        CallByName(oRelatorioInfo, "codigoFuncionario", CallType.Let, oSqlDataReader.Item("codigo_funcionario"))

                        ' Processar faltas
                        Dim faltas = ProcessarCampoHoras(oSqlDataReader, $"{mes}_faltas")
                        Dim faltasNumerico = GetCampoNumerico(oSqlDataReader, $"{mes}_faltas")
                        totalFaltas += faltasNumerico
                        totalFaltasFuncionario += faltasNumerico
                        CallByName(oRelatorioInfo, $"{mes}Faltas", CallType.Let, faltas)

                        ' Atualizar totais
                        Dim horasAtuais = ConverterParaMinutos(CallByName(oRelatorioTotal, $"{mes}Horas", CallType.Get))
                        Dim novoTotalHoras = horasAtuais + horasNumerico
                        CallByName(oRelatorioTotal, $"{mes}Horas", CallType.Let, ConverterParaHorasFormatadas(novoTotalHoras))

                        Dim faltasAtuais = ConverterParaMinutos(CallByName(oRelatorioTotal, $"{mes}Faltas", CallType.Get))
                        Dim novoTotalFaltas = faltasAtuais + faltasNumerico
                        CallByName(oRelatorioTotal, $"{mes}Faltas", CallType.Let, ConverterParaHorasFormatadas(novoTotalFaltas))
                    Next


                    CallByName(oRelatorioInfo, $"totalHoras", CallType.Let, ConverterParaHorasFormatadas(totalHorasFuncionario))
                    CallByName(oRelatorioInfo, $"totalFaltas", CallType.Let, ConverterParaHorasFormatadas(totalFaltasFuncionario))

                    ' Adicionar informações ao relatório
                    oRelatorio.Add(oRelatorioInfo)
                End While

                ' Adicionar total ao relatório
                oRelatorioTotal.funcionario = "TOTAL"
                oRelatorioTotal.sequence = 2
                oRelatorio.Add(oRelatorioTotal)

                ' Retornar o relatório
                Return oRelatorio

            End Using

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: FUNCIONÁRIO - HORAS TRABALHADAS - GOVERNANÇA :::"

    Public Function FuncionarioHorasTrabalhadasGovernanca(ByVal iCodigoEmpresa As Integer,
                                                          ByVal iCodigoUnidade As Integer,
                                                          ByVal iCodigoModulo As Integer,
                                                          ByVal sDataInicio As String,
                                                          ByVal sDataTermino As String,
                                                          ByVal iAgrupadoPorUnidade As Integer,
                                                          ByVal iAtivo As Integer) As List(Of FuncionarioHorasTrabalhadas)

        Try

            'Váriaveis Locais
            Dim oRelatorio As New List(Of FuncionarioHorasTrabalhadas)
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

            'Seta Parametros - Código Modulo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_modulo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoModulo : i += 1

            'Seta Parametros - Data Início
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_inicio"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataInicio : i += 1

            'Seta Parametros - Data Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_termino"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataTermino : i += 1

            'Seta Parametros - Agrupado por Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "agrupado_por_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iAgrupadoPorUnidade : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iAtivo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_report_governanca_funcionario_horas", oSqlParameter)

            While oSqlDataReader.Read

                Dim oRelatorioInfo As New FuncionarioHorasTrabalhadas

                oRelatorioInfo.unidade = oSqlDataReader("unidade")
                oRelatorioInfo.funcionario = oSqlDataReader("funcionario")
                oRelatorioInfo.codigo_funcionario = oSqlDataReader("codigo_funcionario")
                oRelatorioInfo.codigo_unidade = oSqlDataReader("codigo_unidade")
                oRelatorioInfo.total = oSqlDataReader.Item("total")
                oRelatorioInfo.falta = oSqlDataReader.Item("falta")

                oRelatorio.Add(oRelatorioInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            For Each oReturnInfo As FuncionarioHorasTrabalhadas In oRelatorio

                oReturnInfo.valores = FuncionarioHorasTrabalhadasGovernancaMes(iCodigoEmpresa:=iCodigoEmpresa,
                                                                               iCodigoUnidade:=oReturnInfo.codigo_unidade,
                                                                               iCodigoFuncionario:=oReturnInfo.codigo_funcionario,
                                                                               sDataInicio:=sDataInicio,
                                                                               sDataTermino:=sDataTermino)

            Next

            'Retorno da Função
            Return oRelatorio

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function FuncionarioHorasTrabalhadasGovernancaMes(ByVal iCodigoEmpresa As Integer,
                                                             ByVal iCodigoUnidade As Integer,
                                                             ByVal iCodigoFuncionario As Integer,
                                                             ByVal sDataInicio As String,
                                                             ByVal sDataTermino As String) As List(Of FuncionarioHorasTrabalhadasMesValor)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of FuncionarioHorasTrabalhadasMesValor)
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

            'Seta Parametros - Código Funcionário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_funcionario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoFuncionario : i += 1

            'Seta Parametros - Data Início
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_inicio"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataInicio : i += 1

            'Seta Parametros - Data Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_termino"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataTermino

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_report_governanca_funcionario_horas_mes", oSqlParameter)

            While oSqlDataReader.Read

                Dim oReturnInfo As New FuncionarioHorasTrabalhadasMesValor

                oReturnInfo.mes = oSqlDataReader("mes")
                oReturnInfo.ano = oSqlDataReader("ano")
                oReturnInfo.valor = oSqlDataReader("valor")
                oReturnInfo.minutos = oSqlDataReader("minutos")
                oReturnInfo.falta_minutos = oSqlDataReader("falta_minutos")
                oReturnInfo.falta = oSqlDataReader("falta")

                oReturn.Add(oReturnInfo)

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

#Region "::: FUNCIONÁRIO - OCIOSIDADE :::"

    Public Function FuncionarioOciosidade(ByVal iCodigoEmpresa As Integer,
                                          ByVal iCodigoUnidade As Integer,
                                          ByVal iCodigoModulo As Integer,
                                          ByVal sDataInicio As String,
                                          ByVal sDataTermino As String,
                                          ByVal iAtivo As Integer) As List(Of FuncionarioOciosidade)

        Try

            'Váriaveis Locais
            Dim oRelatorio As New List(Of FuncionarioOciosidade)
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

            'Seta Parametros - Data Início
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_inicio"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataInicio : i += 1

            'Seta Parametros - Data Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_termino"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataTermino : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iAtivo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_report_manutencao_funcionario_ociosidade", oSqlParameter)

            While oSqlDataReader.Read

                Dim oRelatorioInfo As New FuncionarioOciosidade

                oRelatorioInfo.funcionario = oSqlDataReader("funcionario")
                oRelatorioInfo.tipo_funcionario = oSqlDataReader("tipo_funcionario")
                oRelatorioInfo.codigo_funcionario = oSqlDataReader("codigo_funcionario")
                oRelatorioInfo.codigo_unidade = oSqlDataReader("codigo_unidade")
                oRelatorioInfo.total = oSqlDataReader.Item("total")
                oRelatorioInfo.valores = FuncionarioOciosidadeMes(iCodigoEmpresa:=iCodigoEmpresa,
                                                                  iCodigoUnidade:=oSqlDataReader("codigo_unidade"),
                                                                  iCodigoFuncionario:=oSqlDataReader.Item("codigo_funcionario"),
                                                                  sDataInicio:=sDataInicio,
                                                                  sDataTermino:=sDataTermino)

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

    Public Function FuncionarioOciosidadeMes(ByVal iCodigoEmpresa As String,
                                             ByVal iCodigoUnidade As Integer,
                                             ByVal iCodigoFuncionario As Integer,
                                             ByVal sDataInicio As String,
                                             ByVal sDataTermino As String) As List(Of FuncionarioOciosidadeMesValor)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of FuncionarioOciosidadeMesValor)
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

            'Seta Parametros - Código Funcionário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_funcionario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoFuncionario : i += 1

            'Seta Parametros - Data Início
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_inicio"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataInicio : i += 1

            'Seta Parametros - Data Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_termino"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataTermino

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_report_manutencao_funcionario_ociosidade_mes", oSqlParameter)

            While oSqlDataReader.Read

                Dim oReturnInfo As New FuncionarioOciosidadeMesValor

                oReturnInfo.mes = oSqlDataReader("mes")
                oReturnInfo.ano = oSqlDataReader("ano")
                oReturnInfo.valor = oSqlDataReader("valor")
                oReturnInfo.horas = oSqlDataReader("horas")

                oReturn.Add(oReturnInfo)

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

    Public Function ChartFuncionarioOciosidade(ByVal iCodigoEmpresa As Integer,
                                               ByVal iCodigoUnidade As Integer,
                                               ByVal iCodigoModulo As Integer,
                                               ByVal sDataInicio As String,
                                               ByVal sDataTermino As String,
                                               ByVal iAtivo As Integer) As List(Of chartFuncionarioOciosidade)

        Try

            'Váriaveis Locais
            Dim oRelatorio As New List(Of chartFuncionarioOciosidade)
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

            'Seta Parametros - Data Início
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_inicio"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataInicio : i += 1

            'Seta Parametros - Data Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_termino"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataTermino : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iAtivo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_chart_manutencao_funcionario_ociosidade", oSqlParameter)

            While oSqlDataReader.Read

                Dim oRelatorioInfo As New chartFuncionarioOciosidade

                oRelatorioInfo.nome = oSqlDataReader("funcionario")
                oRelatorioInfo.valor = ChartFuncionarioOciosidadeMes(iCodigoEmpresa:=iCodigoEmpresa,
                                                                     iCodigoUnidade:=iCodigoUnidade,
                                                                     sDataInicio:=sDataInicio,
                                                                     sDataTermino:=sDataTermino,
                                                                     iCodigoFuncionario:=oSqlDataReader.Item("codigo"))

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

    Public Function ChartFuncionarioOciosidadeMes(ByVal iCodigoEmpresa As Integer,
                                                  ByVal iCodigoUnidade As Integer,
                                                  ByVal sDataInicio As String,
                                                  ByVal sDataTermino As String,
                                                  ByVal iCodigoFuncionario As Integer) As List(Of String)

        Try

            'Váriaveis Locais
            Dim oValor As New List(Of String)
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

            'Seta Parametros - Data Início
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_inicio"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataInicio : i += 1

            'Seta Parametros - Data Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_termino"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataTermino : i += 1

            'Seta Parametros - Código Funcionário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_funcionario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoFuncionario

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_chart_manutencao_funcionario_ociosidade_valor", oSqlParameter)

            While oSqlDataReader.Read

                oValor.Add(oSqlDataReader.Item("valor"))

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oValor

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: GREEN PLANET :::"

    Public Function GreenPlanet(ByVal iCodigoEmpresa As Integer,
                                ByVal iCodigoUnidade As Integer,
                                ByVal sDataInicio As String,
                                ByVal sDataTermino As String,
                                ByVal iAgrupadoPor As Integer,
                                ByVal iCodigoFormaCalculoGreenPlanet As Integer,
                                ByVal iCodigoGrupoItemMedicao As Integer,
                                ByVal iCodigoItemMedicao As Integer) As List(Of MODELS.GreenPlanet)

        Try

            'Váriaveis Locais
            Dim oRelatorio As New List(Of MODELS.GreenPlanet)
            Dim oSqlDataReader As SqlDataReader
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

            'Seta Parametros - Agrupado Por
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "agrupado_por"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iAgrupadoPor : i += 1

            'Seta Parametros - Forma Cálculo Green Planet
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_forma_calculo_green_planet"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoFormaCalculoGreenPlanet : i += 1

            'Seta Parametros - Código Grupo Item Medição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_grupo_item_medicao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoGrupoItemMedicao : i += 1

            'Seta Parametros - Código Item Medição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_item_medicao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoItemMedicao : i += 1

            'Seta Parametros - Data Início
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_inicio"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataInicio : i += 1

            'Seta Parametros - Data Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_termino"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataTermino

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_report_green_planet", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New MODELS.GreenPlanet

                oInfo.codigo = oSqlDataReader("codigo")
                oInfo.codigo_unidade = oSqlDataReader("codigo_unidade")
                oInfo.codigo_fomula_calculo = oSqlDataReader("codigo_formula_calculo")
                oInfo.data_inicio = oSqlDataReader("data_inicio")
                oInfo.data_termino = oSqlDataReader("data_termino")
                oInfo.descricao = oSqlDataReader("descricao")
                oInfo.data = oSqlDataReader.Item("data")
                oInfo.quantidade_hospedes = oSqlDataReader.Item("quantidade_hospede")
                oInfo.quartos_ocupados = oSqlDataReader.Item("quartos_ocupados")
                oInfo.valor = oSqlDataReader.Item("valor")
                oInfo.valor_texto = oSqlDataReader.Item("valor")
                oInfo.erro_quantidade_hospede = oSqlDataReader.Item("erro_quantidade_hospede")
                oInfo.erro_quartos_ocupados = oSqlDataReader.Item("erro_quartos_ocupados")
                oInfo.erro_valor = oSqlDataReader.Item("erro_valor")
                oInfo.unidade_medida = oSqlDataReader.Item("unidade_medida")
                oInfo.medicao = oSqlDataReader.Item("medicao")
                oInfo.foto = oSqlDataReader.Item("foto")

                oRelatorio.Add(oInfo)

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

    Public Function GreenPlanetLancamento(ByVal iCodigoEmpresa As Integer,
                                          ByVal iCodigoUnidade As Integer,
                                          ByVal iCodigoItemMedicao As Integer,
                                          ByVal sDataInicio As String,
                                          ByVal sDataTermino As String,
                                          ByVal iCodigoFormaCalculoGreenPlanet As Integer,
                                          ByVal sErro As String) As List(Of MODELS.GreenPlanetLancamento)

        Try

            'Váriaveis Locais
            Dim oRelatorio As New List(Of MODELS.GreenPlanetLancamento)
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

            'Seta Parametros - Forma Cálculo Green Planet
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_forma_calculo_green_planet"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoFormaCalculoGreenPlanet : i += 1

            'Seta Parametros - Código Item Medição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_item_medicao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoItemMedicao : i += 1

            'Seta Parametros - Data Início
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_inicio"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataInicio : i += 1

            'Seta Parametros - Data Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_termino"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataTermino : i += 1

            'Seta Parametros - Erro
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "erro"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sErro

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_report_green_planet_lancamento", oSqlParameter)

            While oSqlDataReader.Read

                Dim oRelatorioInfo As New MODELS.GreenPlanetLancamento

                oRelatorioInfo.codigo = oSqlDataReader("codigo")
                oRelatorioInfo.item_medicao = oSqlDataReader("descricao")
                oRelatorioInfo.data = oSqlDataReader.Item("data")
                oRelatorioInfo.quantidade_hospedes = oSqlDataReader.Item("quantidade_hospede")
                oRelatorioInfo.quartos_ocupados = oSqlDataReader.Item("quartos_ocupados")
                oRelatorioInfo.valor = oSqlDataReader.Item("valor")
                oRelatorioInfo.valor_anterior = oSqlDataReader.Item("valor_anterior")
                oRelatorioInfo.consumo = oSqlDataReader.Item("consumo")
                oRelatorioInfo.unidade_medida = oSqlDataReader.Item("unidade_medida")

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

    Public Function ChartGreenPlanetData(ByVal iCodigoEmpresa As Integer,
                                         ByVal iCodigoUnidade As Integer,
                                         ByVal sDataInicio As String,
                                         ByVal sDataTermino As String,
                                         ByVal iAgrupadoPor As Integer,
                                         ByVal iCodigoFormaCalculoGreenPlanet As Integer,
                                         ByVal iCodigoGrupoItemMedicao As Integer,
                                         ByVal iCodigoItemMedicao As Integer) As List(Of String)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of String)
            Dim oSqlDataReader As SqlDataReader
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

            'Seta Parametros - Agrupado Por
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "agrupado_por"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iAgrupadoPor : i += 1

            'Seta Parametros - Forma Cálculo Green Planet
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_forma_calculo_green_planet"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoFormaCalculoGreenPlanet : i += 1

            'Seta Parametros - Código Grupo Item Medição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_grupo_item_medicao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoGrupoItemMedicao : i += 1

            'Seta Parametros - Código Item Medição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_item_medicao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoItemMedicao : i += 1

            'Seta Parametros - Data Início
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_inicio"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataInicio : i += 1

            'Seta Parametros - Data Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_termino"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataTermino

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_chart_green_planet_data", oSqlParameter)

            While oSqlDataReader.Read

                oReturn.Add(oSqlDataReader.Item("data"))

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

    Public Function ChartGreenPlanet(ByVal iCodigoEmpresa As Integer,
                                     ByVal iCodigoUnidade As Integer,
                                     ByVal sDataInicio As String,
                                     ByVal sDataTermino As String,
                                     ByVal iAgrupadoPor As Integer,
                                     ByVal iCodigoFormaCalculoGreenPlanet As Integer,
                                     ByVal iCodigoGrupoItemMedicao As Integer,
                                     ByVal iCodigoItemMedicao As Integer) As List(Of MODELS.chartGreenPlanet)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of MODELS.chartGreenPlanet)
            Dim oSqlDataReader As SqlDataReader
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

            'Seta Parametros - Agrupado Por
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "agrupado_por"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iAgrupadoPor : i += 1

            'Seta Parametros - Forma Cálculo Green Planet
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_forma_calculo_green_planet"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoFormaCalculoGreenPlanet : i += 1

            'Seta Parametros - Código Grupo Item Medição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_grupo_item_medicao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoGrupoItemMedicao : i += 1

            'Seta Parametros - Código Item Medição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_item_medicao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoItemMedicao : i += 1

            'Seta Parametros - Data Início
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_inicio"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataInicio : i += 1

            'Seta Parametros - Data Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_termino"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataTermino

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_chart_green_planet", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New chartGreenPlanet

                oInfo.item_medicao = oSqlDataReader("item_medicao")
                oInfo.valor = ChartGreenPlanetValor(iCodigoEmpresa:=iCodigoEmpresa,
                                                    iCodigoUnidade:=iCodigoUnidade,
                                                    sDataInicio:=sDataInicio,
                                                    sDataTermino:=sDataTermino,
                                                    iAgrupadoPor:=iAgrupadoPor,
                                                    iCodigoFormaCalculoGreenPlanet:=iCodigoFormaCalculoGreenPlanet,
                                                    iCodigoGrupoItemMedicao:=iCodigoGrupoItemMedicao,
                                                    iCodigoItemMedicao:=oSqlDataReader.Item("codigo_item_medicao"))

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

    Public Function ChartGreenPlanetValor(ByVal iCodigoEmpresa As Integer,
                                          ByVal iCodigoUnidade As Integer,
                                          ByVal sDataInicio As String,
                                          ByVal sDataTermino As String,
                                          ByVal iAgrupadoPor As Integer,
                                          ByVal iCodigoFormaCalculoGreenPlanet As Integer,
                                          ByVal iCodigoGrupoItemMedicao As Integer,
                                          ByVal iCodigoItemMedicao As Integer) As List(Of String)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of String)
            Dim oSqlDataReader As SqlDataReader
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

            'Seta Parametros - Agrupado Por
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "agrupado_por"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iAgrupadoPor : i += 1

            'Seta Parametros - Forma Cálculo Green Planet
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_forma_calculo_green_planet"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoFormaCalculoGreenPlanet : i += 1

            'Seta Parametros - Código Grupo Item Medição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_grupo_item_medicao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoGrupoItemMedicao : i += 1

            'Seta Parametros - Código Item Medição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_item_medicao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoItemMedicao : i += 1

            'Seta Parametros - Data Início
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_inicio"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataInicio : i += 1

            'Seta Parametros - Data Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_termino"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataTermino

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_chart_green_planet_valor", oSqlParameter)

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

    Public Function ChartGreenPlanetUnidadeMedida(ByVal iCodigoEmpresa As Integer,
                                                  ByVal iCodigoUnidade As Integer,
                                                  ByVal iCodigoGrupoItemMedicao As Integer) As String

        'Váriaveis Locais
        Dim oSqlParameter(2) As SqlParameter
        Dim sReturn As String
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

            'Seta Parametros - Código Grupo Item Medição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_grupo_item_medicao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoGrupoItemMedicao

            'Executa Query
            sReturn = ExecuteScalar(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_item_medicao_unidade_medida", oSqlParameter)

            'Retorno da Função
            Return sReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: MANUTENÇÃO - ABERTO x CONCLUIDO :::"

    Public Function ManutencaoAbertoConcluido(ByVal iCodigoEmpresa As Integer,
                                              ByVal iCodigoUnidade As Integer,
                                              ByVal sDataInicio As String,
                                              ByVal sDataTermino As String,
                                              ByRef oRelatorioTotal As ManutencaoAbertoConcluido) As List(Of ManutencaoAbertoConcluido)

        Try

            'Váriaveis Locais
            Dim oRelatorio As New List(Of ManutencaoAbertoConcluido)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(3) As SqlParameter
            Dim i As Integer = 0
            oRelatorioTotal = New ManutencaoAbertoConcluido


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
            oSqlParameter(i).Value = sDataInicio : i += 1

            'Seta Parametros - Data Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_termino"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataTermino

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_report_manutencao_aberto_concluido", oSqlParameter)

            While oSqlDataReader.Read

                Dim oRelatorioInfo As New ManutencaoAbertoConcluido

                oRelatorioInfo.dia = oSqlDataReader("dia")
                oRelatorioInfo.aberto = oSqlDataReader("aberto")
                oRelatorioInfo.concluido = oSqlDataReader.Item("concluido")
                oRelatorioInfo.saldo = oSqlDataReader.Item("saldo")

                oRelatorioTotal.aberto += oSqlDataReader.Item("aberto")
                oRelatorioTotal.concluido += oSqlDataReader.Item("concluido")
                oRelatorioTotal.saldo = oSqlDataReader.Item("saldo")

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

    Public Function ChartManutencaoAbertoConcluido(ByVal iCodigoEmpresa As Integer,
                                                   ByVal iCodigoUnidade As Integer,
                                                   ByVal sDataInicio As String,
                                                   ByVal sDataTermino As String,
                                                   Optional ByVal bQualidade As Boolean = False) As List(Of chartManutencaoAbertoConcluido)

        Try

            'Váriaveis Locais
            Dim oRelatorio As New List(Of chartManutencaoAbertoConcluido)
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

            'Seta Parametros - Data Início
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_inicio"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataInicio : i += 1

            'Seta Parametros - Data Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_termino"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataTermino : i += 1

            'Seta Parametros - Qualidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "qualidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bQualidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_chart_manutencao_aberto_concluido", oSqlParameter)

            While oSqlDataReader.Read

                Dim oRelatorioInfo As New chartManutencaoAbertoConcluido

                oRelatorioInfo.data = oSqlDataReader("dia")
                oRelatorioInfo.aberto = oSqlDataReader("aberto")
                oRelatorioInfo.concluido = oSqlDataReader("concluido")
                oRelatorioInfo.saldo = oSqlDataReader("saldo")

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

#End Region

#Region "::: LOG BOOK :::"

    Public Function LogBook(ByVal iCodigoEmpresa As Integer,
                            ByVal iCodigoUnidade As Integer,
                            ByVal sDataInicio As String,
                            ByVal sDataTermino As String) As List(Of RelatorioLogBook)

        Try

            'Váriaveis Locais
            Dim oRelatorio As New List(Of RelatorioLogBook)
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

            'Seta Parametros - Data Início
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_inicio"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataInicio : i += 1

            'Seta Parametros - Data Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_termino"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataTermino

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_report_log_book", oSqlParameter)

            While oSqlDataReader.Read

                Dim oRelatorioInfo As New RelatorioLogBook

                oRelatorioInfo.data = oSqlDataReader("data")
                oRelatorioInfo.usuario = oSqlDataReader("usuario")
                oRelatorioInfo.descricao = oSqlDataReader.Item("descricao")

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

#End Region

#Region "::: MANUTENÇÃO - CATEGORIA :::"

    Public Function ManutencaoCategoria(ByVal iCodigoEmpresa As Integer,
                                                ByVal iCodigoUnidade As Integer,
                                                ByVal sDataInicio As String,
                                                ByVal sDataTermino As String) As List(Of ManutencaoCategoria)

        Try

            'Váriaveis Locais
            Dim oRelatorio As New List(Of ManutencaoCategoria)
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

            'Seta Parametros - Data Início
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_inicio"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataInicio : i += 1

            'Seta Parametros - Data Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_termino"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataTermino

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_report_manutencao_categoria", oSqlParameter)

            While oSqlDataReader.Read

                Dim oRelatorioInfo As New ManutencaoCategoria

                oRelatorioInfo.item = oSqlDataReader("item")
                oRelatorioInfo.categoria = oSqlDataReader("descricao")
                oRelatorioInfo.total = oSqlDataReader.Item("total")
                oRelatorioInfo.valores = ManutencaoCategoriaMes(iCodigoEmpresa:=iCodigoEmpresa,
                                                                iCodigoUnidade:=iCodigoUnidade,
                                                                iCodigoCategoria:=oSqlDataReader.Item("codigo_categoria"),
                                                                sDataInicio:=sDataInicio,
                                                                sDataTermino:=sDataTermino)

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

    Public Function ManutencaoCategoriaMes(ByVal iCodigoEmpresa As Integer,
                                           ByVal iCodigoUnidade As Integer,
                                           ByVal iCodigoCategoria As Integer,
                                           ByVal sDataInicio As String,
                                           ByVal sDataTermino As String) As List(Of ManutencaoCategoriaMesValor)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of ManutencaoCategoriaMesValor)
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

            'Seta Parametros - Código Categoria
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_categoria"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoCategoria : i += 1

            'Seta Parametros - Data Início
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_inicio"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataInicio : i += 1

            'Seta Parametros - Data Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_termino"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataTermino

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_report_manutencao_categoria_mes", oSqlParameter)

            While oSqlDataReader.Read

                Dim oReturnInfo As New ManutencaoCategoriaMesValor

                oReturnInfo.valor = oSqlDataReader("valor")

                oReturn.Add(oReturnInfo)

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

    Public Function ChartManutencaoCategoria(ByVal iCodigoEmpresa As Integer,
                                             ByVal iCodigoUnidade As Integer,
                                             ByVal sDataInicio As String,
                                             ByVal sDataTermino As String) As List(Of chartManutencaoCategoria)

        Try

            'Váriaveis Locais
            Dim oRelatorio As New List(Of chartManutencaoCategoria)
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

            'Seta Parametros - Data Início
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_inicio"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataInicio : i += 1

            'Seta Parametros - Data Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_termino"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataTermino

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_chart_manutencao_categoria", oSqlParameter)

            While oSqlDataReader.Read

                Dim oRelatorioInfo As New chartManutencaoCategoria

                oRelatorioInfo.categoria = oSqlDataReader("categoria")
                oRelatorioInfo.valor = ChartManutencaoCategoriaMes(iCodigoEmpresa:=iCodigoEmpresa,
                                                                   iCodigoUnidade:=iCodigoUnidade,
                                                                   sDataInicio:=sDataInicio,
                                                                   sDataTermino:=sDataTermino,
                                                                   iCodigoCategoria:=oSqlDataReader.Item("codigo"))

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

    Public Function ChartManutencaoCategoriaMes(ByVal iCodigoEmpresa As Integer,
                                                ByVal iCodigoUnidade As Integer,
                                                ByVal sDataInicio As String,
                                                ByVal sDataTermino As String,
                                                ByVal iCodigoCategoria As Integer) As List(Of String)

        Try

            'Váriaveis Locais
            Dim oValor As New List(Of String)
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

            'Seta Parametros - Data Início
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_inicio"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataInicio : i += 1

            'Seta Parametros - Data Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_termino"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataTermino : i += 1

            'Seta Parametros - Código Categoria
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_categoria"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoCategoria

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_chart_manutencao_categoria_valor", oSqlParameter)

            While oSqlDataReader.Read

                oValor.Add(oSqlDataReader.Item("valor"))

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oValor

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: MANUTENÇÃO - EQUIPAMENTO :::"

    Public Function ManutencaoEquipamento(ByVal iCodigoEmpresa As Integer,
                                          ByVal iCodigoUnidade As Integer,
                                          ByVal sDataInicio As String,
                                          ByVal sDataTermino As String) As List(Of ManutencaoEquipamento)

        Try

            'Váriaveis Locais
            Dim oRelatorio As New List(Of ManutencaoEquipamento)
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

            'Seta Parametros - Data Início
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_inicio"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataInicio : i += 1

            'Seta Parametros - Data Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_termino"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataTermino

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_report_manutencao_equipamento", oSqlParameter)

            While oSqlDataReader.Read

                Dim oRelatorioInfo As New ManutencaoEquipamento

                oRelatorioInfo.item = oSqlDataReader("item")
                oRelatorioInfo.equipamento = oSqlDataReader("descricao")
                oRelatorioInfo.codigo_equipamento = oSqlDataReader("codigo_equipamento")
                oRelatorioInfo.codigo_unidade = oSqlDataReader("codigo_unidade")
                oRelatorioInfo.total = oSqlDataReader.Item("total")
                oRelatorioInfo.valores = ManutencaoEquipamentoMes(iCodigoEmpresa:=iCodigoEmpresa,
                                                                  iCodigoUnidade:=iCodigoUnidade,
                                                                  lCodigoEquipamento:=oSqlDataReader.Item("codigo_equipamento"),
                                                                  sDataInicio:=sDataInicio,
                                                                  sDataTermino:=sDataTermino)

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

    Public Function ManutencaoEquipamentoMes(ByVal iCodigoEmpresa As Integer,
                                             ByVal iCodigoUnidade As Integer,
                                             ByVal lCodigoEquipamento As Long,
                                             ByVal sDataInicio As String,
                                             ByVal sDataTermino As String) As List(Of ManutencaoEquipamentoMesValor)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of ManutencaoEquipamentoMesValor)
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

            'Seta Parametros - Código Equipamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_equipamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoEquipamento : i += 1

            'Seta Parametros - Data Início
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_inicio"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataInicio : i += 1

            'Seta Parametros - Data Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_termino"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataTermino

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_report_manutencao_equipamento_mes", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New ManutencaoEquipamentoMesValor

                oInfo.mes = oSqlDataReader("mes")
                oInfo.ano = oSqlDataReader("ano")
                oInfo.valor = oSqlDataReader("valor")

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

    Public Function ChartManutencaoEquipamento(ByVal iCodigoEmpresa As Integer,
                                               ByVal iCodigoUnidade As Integer,
                                               ByVal sDataInicio As String,
                                               ByVal sDataTermino As String) As List(Of chartManutencaoEquipamento)

        Try

            'Váriaveis Locais
            Dim oRelatorio As New List(Of chartManutencaoEquipamento)
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

            'Seta Parametros - Data Início
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_inicio"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataInicio : i += 1

            'Seta Parametros - Data Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_termino"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataTermino

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_chart_manutencao_equipamento", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New chartManutencaoEquipamento

                oInfo.equipamento = oSqlDataReader("equipamento")
                oInfo.valor = ChartManutencaoEquipamentoMes(iCodigoEmpresa:=iCodigoEmpresa,
                                                            iCodigoUnidade:=iCodigoUnidade,
                                                            sDataInicio:=sDataInicio,
                                                            sDataTermino:=sDataTermino,
                                                            lCodigoEquipamento:=oSqlDataReader.Item("codigo"))

                oRelatorio.Add(oInfo)

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

    Public Function ChartManutencaoEquipamentoMes(ByVal iCodigoEmpresa As Integer,
                                                  ByVal iCodigoUnidade As Integer,
                                                  ByVal sDataInicio As String,
                                                  ByVal sDataTermino As String,
                                                  ByVal lCodigoEquipamento As Long) As List(Of String)

        Try

            'Váriaveis Locais
            Dim oValor As New List(Of String)
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

            'Seta Parametros - Data Início
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_inicio"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataInicio : i += 1

            'Seta Parametros - Data Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_termino"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataTermino : i += 1

            'Seta Parametros - Código Equipamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_equipamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoEquipamento

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_chart_manutencao_equipamento_valor", oSqlParameter)

            While oSqlDataReader.Read

                oValor.Add(oSqlDataReader.Item("valor"))

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oValor

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: MANUTENÇÃO - EXECUTOR :::"

    Public Function ManutencaoExecutor(ByVal iCodigoEmpresa As Integer,
                                       ByVal iCodigoUnidade As Integer,
                                       ByVal iAno As Integer,
                                       ByRef oRelatorioTotal As ManutencaoExecutor) As List(Of ManutencaoExecutor)

        Try

            'Váriaveis Locais
            Dim oRelatorio As New List(Of ManutencaoExecutor)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(2) As SqlParameter
            Dim i As Integer = 0
            oRelatorioTotal = New ManutencaoExecutor


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

            'Seta Parametros - Ano
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ano"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iAno

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_report_manutencao_executor", oSqlParameter)

            While oSqlDataReader.Read

                Dim oRelatorioInfo As New ManutencaoExecutor

                oRelatorioInfo.executor = oSqlDataReader("executor")
                oRelatorioInfo.janeiro = oSqlDataReader.Item("janeiro")
                oRelatorioInfo.fevereiro = oSqlDataReader.Item("fevereiro")
                oRelatorioInfo.marco = oSqlDataReader.Item("marco")
                oRelatorioInfo.abril = oSqlDataReader.Item("abril")
                oRelatorioInfo.maio = oSqlDataReader.Item("maio")
                oRelatorioInfo.junho = oSqlDataReader.Item("junho")
                oRelatorioInfo.julho = oSqlDataReader.Item("julho")
                oRelatorioInfo.agosto = oSqlDataReader.Item("agosto")
                oRelatorioInfo.setembro = oSqlDataReader.Item("setembro")
                oRelatorioInfo.outubro = oSqlDataReader.Item("outubro")
                oRelatorioInfo.novembro = oSqlDataReader.Item("novembro")
                oRelatorioInfo.dezembro = oSqlDataReader.Item("dezembro")
                oRelatorioInfo.total = oSqlDataReader.Item("total")

                oRelatorioTotal.janeiro += oSqlDataReader.Item("janeiro")
                oRelatorioTotal.fevereiro += oSqlDataReader.Item("fevereiro")
                oRelatorioTotal.marco += oSqlDataReader.Item("marco")
                oRelatorioTotal.abril += oSqlDataReader.Item("abril")
                oRelatorioTotal.maio += oSqlDataReader.Item("maio")
                oRelatorioTotal.junho += oSqlDataReader.Item("junho")
                oRelatorioTotal.julho += oSqlDataReader.Item("julho")
                oRelatorioTotal.agosto += oSqlDataReader.Item("agosto")
                oRelatorioTotal.setembro += oSqlDataReader.Item("setembro")
                oRelatorioTotal.outubro += oSqlDataReader.Item("outubro")
                oRelatorioTotal.novembro += oSqlDataReader.Item("novembro")
                oRelatorioTotal.dezembro += oSqlDataReader.Item("dezembro")
                oRelatorioTotal.total += oSqlDataReader.Item("total")

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

    Public Function ChartManutencaoExecutor(ByVal iCodigoEmpresa As Integer,
                                            ByVal iCodigoUnidade As Integer,
                                            ByVal iAno As Integer) As List(Of chartManutencaoExecutor)

        Try

            'Váriaveis Locais
            Dim oRelatorio As New List(Of chartManutencaoExecutor)
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

            'Seta Parametros - Ano
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ano"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iAno

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_chart_manutencao_executor", oSqlParameter)

            While oSqlDataReader.Read

                Dim oRelatorioInfo As New chartManutencaoExecutor

                oRelatorioInfo.executor = oSqlDataReader("executor")
                oRelatorioInfo.valor = ChartManutencaoExecutorMes(iCodigoEmpresa, iCodigoUnidade, iAno, oSqlDataReader.Item("codigo"))

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

    Public Function ChartManutencaoExecutorMes(ByVal iCodigoEmpresa As Integer,
                                               ByVal iCodigoUnidade As Integer,
                                               ByVal iAno As Integer,
                                               ByVal iCodigoExecutor As Integer) As List(Of String)

        Try

            'Váriaveis Locais
            Dim oValor As New List(Of String)
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

            'Seta Parametros - Ano
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ano"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iAno : i += 1

            'Seta Parametros - Código Executor
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_executor"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoExecutor

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_chart_manutencao_executor_valor", oSqlParameter)

            While oSqlDataReader.Read

                oValor.Add(oSqlDataReader.Item("valor"))

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oValor

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: MANUTENÇÃO - SETOR :::"

    Public Function ManutencaoSetor(ByVal iCodigoEmpresa As Integer,
                                    ByVal iCodigoUnidade As Integer,
                                    ByVal iAno As Integer,
                                    ByRef oRelatorioTotal As ManutencaoSetor) As List(Of ManutencaoSetor)

        Try

            'Váriaveis Locais
            Dim oRelatorio As New List(Of ManutencaoSetor)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(2) As SqlParameter
            Dim i As Integer = 0
            oRelatorioTotal = New ManutencaoSetor


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

            'Seta Parametros - Ano
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ano"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iAno

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_report_manutencao_setor", oSqlParameter)

            While oSqlDataReader.Read

                Dim oRelatorioInfo As New ManutencaoSetor

                oRelatorioInfo.item = oSqlDataReader("item")
                oRelatorioInfo.setor = oSqlDataReader("setor")
                oRelatorioInfo.janeiro = oSqlDataReader.Item("janeiro")
                oRelatorioInfo.fevereiro = oSqlDataReader.Item("fevereiro")
                oRelatorioInfo.marco = oSqlDataReader.Item("marco")
                oRelatorioInfo.abril = oSqlDataReader.Item("abril")
                oRelatorioInfo.maio = oSqlDataReader.Item("maio")
                oRelatorioInfo.junho = oSqlDataReader.Item("junho")
                oRelatorioInfo.julho = oSqlDataReader.Item("julho")
                oRelatorioInfo.agosto = oSqlDataReader.Item("agosto")
                oRelatorioInfo.setembro = oSqlDataReader.Item("setembro")
                oRelatorioInfo.outubro = oSqlDataReader.Item("outubro")
                oRelatorioInfo.novembro = oSqlDataReader.Item("novembro")
                oRelatorioInfo.dezembro = oSqlDataReader.Item("dezembro")
                oRelatorioInfo.total = oSqlDataReader.Item("total")

                oRelatorioTotal.janeiro += oSqlDataReader.Item("janeiro")
                oRelatorioTotal.fevereiro += oSqlDataReader.Item("fevereiro")
                oRelatorioTotal.marco += oSqlDataReader.Item("marco")
                oRelatorioTotal.abril += oSqlDataReader.Item("abril")
                oRelatorioTotal.maio += oSqlDataReader.Item("maio")
                oRelatorioTotal.junho += oSqlDataReader.Item("junho")
                oRelatorioTotal.julho += oSqlDataReader.Item("julho")
                oRelatorioTotal.agosto += oSqlDataReader.Item("agosto")
                oRelatorioTotal.setembro += oSqlDataReader.Item("setembro")
                oRelatorioTotal.outubro += oSqlDataReader.Item("outubro")
                oRelatorioTotal.novembro += oSqlDataReader.Item("novembro")
                oRelatorioTotal.dezembro += oSqlDataReader.Item("dezembro")
                oRelatorioTotal.total += oSqlDataReader.Item("total")

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

    Public Function ChartManutencaoSetor(ByVal iCodigoEmpresa As Integer,
                                         ByVal iCodigoUnidade As Integer,
                                         ByVal iAno As Integer) As List(Of chartManutencaoSetor)

        Try

            'Váriaveis Locais
            Dim oRelatorio As New List(Of chartManutencaoSetor)
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

            'Seta Parametros - Ano
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ano"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iAno

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_chart_manutencao_setor", oSqlParameter)

            While oSqlDataReader.Read

                Dim oRelatorioInfo As New chartManutencaoSetor

                oRelatorioInfo.setor = oSqlDataReader("setor")
                oRelatorioInfo.valor = ChartManutencaoSetorMes(iCodigoEmpresa, iCodigoUnidade, iAno, oSqlDataReader.Item("codigo"))

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

    Public Function ChartManutencaoSetorMes(ByVal iCodigoEmpresa As Integer,
                                            ByVal iCodigoUnidade As Integer,
                                            ByVal iAno As Integer,
                                            ByVal iCodigoSetor As Integer) As List(Of String)

        Try

            'Váriaveis Locais
            Dim oValor As New List(Of String)
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

            'Seta Parametros - Ano
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ano"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iAno : i += 1

            'Seta Parametros - Código Setor
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_setor"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoSetor

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_chart_manutencao_setor_valor", oSqlParameter)

            While oSqlDataReader.Read

                oValor.Add(oSqlDataReader.Item("valor"))

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oValor

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: MANUTENÇÃO :::"

    Public Function LoadManutencao(ByVal iCodigoEmpresa As Integer,
                                   ByVal iCodigoUnidade As Integer,
                                   ByVal sTipo As String,
                                   ByVal iAno As Integer) As List(Of Manutencao)

        Try

            'Váriaveis Locais
            Dim oRelatorio As New List(Of Manutencao)
            Dim oRelatorioTotal As New Manutencao
            Dim i As Integer = 0

            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, iCodigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, iCodigoUnidade),
                CriarParametro("tipo", SqlDbType.VarChar, sTipo),
                CriarParametro("ano", SqlDbType.Int, iAno)
                }

            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_report_manutencao", oSqlParameter)

                Dim meses As String() = {"janeiro", "fevereiro", "marco", "abril", "maio", "junho", "julho", "agosto", "setembro", "outubro", "novembro", "dezembro"}

                While oSqlDataReader.Read

                    Dim oRelatorioInfo As New Manutencao
                    Dim total As Long = 0
                    Dim valor As Long = 0

                    CallByName(oRelatorioInfo, $"descricao", CallType.Let, oSqlDataReader.Item("descricao"))
                    CallByName(oRelatorioInfo, $"unidade", CallType.Let, oSqlDataReader.Item("unidade"))
                    ' Processar horas e faltas para cada mês
                    For Each mes In meses
                        CallByName(oRelatorioInfo, $"{mes}", CallType.Let, GetCampoNumerico(oSqlDataReader, $"{mes}"))
                        total += GetCampoNumerico(oSqlDataReader, $"{mes}")
                        valor = CallByName(oRelatorioTotal, $"{mes}", CallType.Get) + GetCampoNumerico(oSqlDataReader, $"{mes}")
                        CallByName(oRelatorioTotal, $"{mes}", CallType.Let, valor)
                    Next
                    CallByName(oRelatorioInfo, $"total", CallType.Let, total)

                    valor = CallByName(oRelatorioTotal, $"total", CallType.Get) + total
                    CallByName(oRelatorioTotal, $"total", CallType.Let, valor)

                    ' Adicionar informações ao relatório
                    oRelatorio.Add(oRelatorioInfo)

                End While

                oRelatorioTotal.descricao = "TOTAL"
                oRelatorio.Add(oRelatorioTotal)


                ' Retornar o relatório
                Return oRelatorio

            End Using

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: MANUTENÇÃO - SOLICITANTE :::"

    Public Function ManutencaoSolicitante(ByVal iCodigoEmpresa As Integer,
                                          ByVal iCodigoUnidade As Integer,
                                          ByVal iCodigoDepartamento As Integer,
                                          ByVal iAno As Integer) As List(Of ManutencaoSolicitante)

        Try

            'Váriaveis Locais
            Dim oRelatorio As New List(Of ManutencaoSolicitante)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(3) As SqlParameter
            Dim oRelatorioTotal As New ManutencaoSolicitante
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

            'Seta Parametros - Código Departamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_departamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoDepartamento : i += 1

            'Seta Parametros - Ano
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ano"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iAno

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_report_manutencao_solicitante", oSqlParameter)

            While oSqlDataReader.Read

                Dim oRelatorioInfo As New ManutencaoSolicitante

                oRelatorioInfo.solicitante = oSqlDataReader("solicitante")
                oRelatorioInfo.departamento = oSqlDataReader("departamento")
                oRelatorioInfo.janeiro = oSqlDataReader.Item("janeiro")
                oRelatorioInfo.fevereiro = oSqlDataReader.Item("fevereiro")
                oRelatorioInfo.marco = oSqlDataReader.Item("marco")
                oRelatorioInfo.abril = oSqlDataReader.Item("abril")
                oRelatorioInfo.maio = oSqlDataReader.Item("maio")
                oRelatorioInfo.junho = oSqlDataReader.Item("junho")
                oRelatorioInfo.julho = oSqlDataReader.Item("julho")
                oRelatorioInfo.agosto = oSqlDataReader.Item("agosto")
                oRelatorioInfo.setembro = oSqlDataReader.Item("setembro")
                oRelatorioInfo.outubro = oSqlDataReader.Item("outubro")
                oRelatorioInfo.novembro = oSqlDataReader.Item("novembro")
                oRelatorioInfo.dezembro = oSqlDataReader.Item("dezembro")
                oRelatorioInfo.total = oSqlDataReader.Item("total")

                oRelatorioTotal.janeiro += oSqlDataReader.Item("janeiro")
                oRelatorioTotal.fevereiro += oSqlDataReader.Item("fevereiro")
                oRelatorioTotal.marco += oSqlDataReader.Item("marco")
                oRelatorioTotal.abril += oSqlDataReader.Item("abril")
                oRelatorioTotal.maio += oSqlDataReader.Item("maio")
                oRelatorioTotal.junho += oSqlDataReader.Item("junho")
                oRelatorioTotal.julho += oSqlDataReader.Item("julho")
                oRelatorioTotal.agosto += oSqlDataReader.Item("agosto")
                oRelatorioTotal.setembro += oSqlDataReader.Item("setembro")
                oRelatorioTotal.outubro += oSqlDataReader.Item("outubro")
                oRelatorioTotal.novembro += oSqlDataReader.Item("novembro")
                oRelatorioTotal.dezembro += oSqlDataReader.Item("dezembro")
                oRelatorioTotal.total += oSqlDataReader.Item("total")

                oRelatorio.Add(oRelatorioInfo)

            End While

            oRelatorioTotal.solicitante = "TOTAL"

            oRelatorio.Add(oRelatorioTotal)

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

    Public Function ChartManutencaoSolicitante(ByVal iCodigoEmpresa As Integer,
                                               ByVal iCodigoUnidade As Integer,
                                               ByVal iAno As Integer) As List(Of chartManutencaoSolicitante)

        Try

            'Váriaveis Locais
            Dim oRelatorio As New List(Of chartManutencaoSolicitante)
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

            'Seta Parametros - Ano
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ano"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iAno

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_chart_manutencao_solicitante", oSqlParameter)

            While oSqlDataReader.Read

                Dim oRelatorioInfo As New chartManutencaoSolicitante

                oRelatorioInfo.solicitante = oSqlDataReader("solicitante")
                oRelatorioInfo.valor = ChartManutencaoSolicitanteMes(iCodigoEmpresa, iCodigoUnidade, iAno, oSqlDataReader.Item("codigo"))

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

    Public Function ChartManutencaoSolicitanteMes(ByVal iCodigoEmpresa As Integer,
                                                  ByVal iCodigoUnidade As Integer,
                                                  ByVal iAno As Integer,
                                                  ByVal iCodigoSolicitante As Integer) As List(Of String)

        Try

            'Váriaveis Locais
            Dim oValor As New List(Of String)
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

            'Seta Parametros - Ano
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ano"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iAno : i += 1

            'Seta Parametros - Código Solicitante
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_solicitante"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoSolicitante

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_chart_manutencao_solicitante_valor", oSqlParameter)

            While oSqlDataReader.Read

                oValor.Add(oSqlDataReader.Item("valor"))

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oValor

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: MANUTENÇÃO - TIPO ORDEM SERVIÇO :::"

    Public Function ManutencaoTipoOrdemServico(ByVal iCodigoEmpresa As Integer,
                                               ByVal iCodigoUnidade As Integer,
                                               ByVal iAno As Integer,
                                               ByRef oRelatorioTotal As ManutencaoTipoOrdemServico) As List(Of ManutencaoTipoOrdemServico)

        Try

            'Váriaveis Locais
            Dim oRelatorio As New List(Of ManutencaoTipoOrdemServico)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(2) As SqlParameter
            Dim i As Integer = 0
            oRelatorioTotal = New ManutencaoTipoOrdemServico


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

            'Seta Parametros - Ano
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ano"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iAno

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_report_manutencao_tipo_ordem_servico", oSqlParameter)

            While oSqlDataReader.Read

                Dim oRelatorioInfo As New ManutencaoTipoOrdemServico

                oRelatorioInfo.mes = oSqlDataReader("mes")
                oRelatorioInfo.corretiva = oSqlDataReader.Item("corretiva")
                oRelatorioInfo.melhoria = oSqlDataReader.Item("melhoria")
                oRelatorioInfo.preditiva = oSqlDataReader.Item("preditiva")
                oRelatorioInfo.preventiva = oSqlDataReader.Item("preventiva")
                oRelatorioInfo.checklist = oSqlDataReader.Item("checklist")
                oRelatorioInfo.total = oSqlDataReader.Item("total")

                oRelatorioTotal.corretiva += oSqlDataReader.Item("corretiva")
                oRelatorioTotal.melhoria += oSqlDataReader.Item("melhoria")
                oRelatorioTotal.preditiva += oSqlDataReader.Item("preditiva")
                oRelatorioTotal.preventiva += oSqlDataReader.Item("preventiva")
                oRelatorioTotal.checklist += oSqlDataReader.Item("checklist")
                oRelatorioTotal.total += oSqlDataReader.Item("total")

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

    Public Function ChartManutencaoTipoOrdemServico(ByVal iCodigoEmpresa As Integer,
                                                    ByVal iCodigoUnidade As Integer,
                                                    ByVal iAno As Integer) As List(Of chartManutencaoTipoOrdemServico)

        Try

            'Váriaveis Locais
            Dim oChart As New List(Of chartManutencaoTipoOrdemServico)
            Dim oChartInfo As New chartManutencaoTipoOrdemServico
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

            'Seta Parametros - Ano
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ano"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iAno

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_chart_manutencao_tipo_ordem_servico", oSqlParameter)

            While oSqlDataReader.Read

                oChartInfo = New chartManutencaoTipoOrdemServico

                oChartInfo.tipo = oSqlDataReader.Item("tipo")
                oChartInfo.valor = oSqlDataReader.Item("valor")

                oChart.Add(oChartInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oChart

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: MANUTENÇÃO - TEMPO MÉDIO ATENDIMENTO :::"

    Public Function ManutencaoTempoMedioAtendimento(ByVal iCodigoEmpresa As Integer,
                                                    ByVal iCodigoUnidade As Integer,
                                                    ByVal iAno As Integer) As List(Of ManutencaoTempoMedioAtendimento)

        Try

            'Váriaveis Locais
            Dim oRelatorio As New List(Of ManutencaoTempoMedioAtendimento)
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

            'Seta Parametros - Ano
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ano"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iAno

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_report_manutencao_tempo_medio_atendimento", oSqlParameter)

            While oSqlDataReader.Read

                Dim oRelatorioInfo As New ManutencaoTempoMedioAtendimento

                oRelatorioInfo.mes = oSqlDataReader("mes")
                oRelatorioInfo.no_dia = oSqlDataReader.Item("no_dia")
                oRelatorioInfo.um_dia = oSqlDataReader.Item("um_dia")
                oRelatorioInfo.tres_dias = oSqlDataReader.Item("tres_dias")
                oRelatorioInfo.cinco_dias = oSqlDataReader.Item("cinco_dias")
                oRelatorioInfo.acima_cinco_dias = oSqlDataReader.Item("acima_cinco_dias")

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

    Public Function ChartManutencaoTempoMedioAtendimento(ByVal iCodigoEmpresa As Integer,
                                                         ByVal iCodigoUnidade As Integer,
                                                         ByVal iAno As Integer,
                                                         Optional ByVal bQualidade As Boolean = False) As List(Of chartManutencaoTempoMedioAtendimento)

        Try

            'Váriaveis Locais
            Dim oChart As New List(Of chartManutencaoTempoMedioAtendimento)
            Dim oChartInfo As New chartManutencaoTempoMedioAtendimento
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

            'Seta Parametros - Ano
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ano"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iAno : i += 1

            'Seta Parametros - Qualidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "qualidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bQualidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_chart_manutencao_tempo_medio_atendimento", oSqlParameter)

            While oSqlDataReader.Read

                oChartInfo = New chartManutencaoTempoMedioAtendimento

                oChartInfo.tipo = oSqlDataReader.Item("tipo")
                oChartInfo.valor = oSqlDataReader.Item("valor")

                oChart.Add(oChartInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oChart

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: PREVENTIVA - MENSAL :::"

    Public Function PreventivaMes(ByVal iCodigoEmpresa As Integer,
                                  ByVal iCodigoUnidade As Integer) As List(Of PreventivaMes)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of PreventivaMes)
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
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_report_preventiva_mes", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New PreventivaMes

                oInfo.ano = oSqlDataReader.Item("ano")
                oInfo.mes = oSqlDataReader.Item("mes")
                oInfo.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oInfo.unidade = oSqlDataReader.Item("unidade")
                oInfo.descricao = oSqlDataReader.Item("descricao")
                oInfo.linha = oSqlDataReader.Item("linha")

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

#Region "::: PMOC :::"

    Public Function LoadPMOC(ByVal iCodigoEmpresa As Integer,
                             ByVal iCodigoUnidade As Integer,
                             ByVal iAno As Integer?,
                             ByVal sTipo As String) As List(Of PMOCAno)

        Try
            ' Variáveis Locais
            Dim anosAgrupados = New Dictionary(Of Integer, PMOCAno)()


            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, iCodigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, iCodigoUnidade),
                CriarParametro("tipo", SqlDbType.VarChar, If(String.IsNullOrEmpty(sTipo), DBNull.Value, sTipo)),
                CriarParametro("ano", SqlDbType.Int, If(iAno.HasValue, iAno, DBNull.Value))
                }

            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_report_pmoc_consolidado", oSqlParameter)

                While oSqlDataReader.Read()

                    Dim ano = Convert.ToInt32(oSqlDataReader("ano"))
                    Dim codigoUnidade = Convert.ToInt32(oSqlDataReader("codigo_unidade"))
                    Dim unidade = oSqlDataReader("unidade").ToString()
                    Dim bimestre = Convert.ToInt32(oSqlDataReader("bimestre"))
                    Dim descricao = oSqlDataReader("descricao").ToString()
                    Dim startDate = Convert.ToString(oSqlDataReader("start_date"))
                    Dim endDate = Convert.ToString(oSqlDataReader("end_date"))

                    ' Criar ou buscar o objeto do ano
                    If Not anosAgrupados.ContainsKey(ano) Then
                        anosAgrupados(ano) = New PMOCAno With {
                        .ano = ano,
                        .unidade = New List(Of PMOCAnoUnidade)()
                    }
                    End If

                    Dim pmocAno = anosAgrupados(ano)

                    ' Criar ou buscar a unidade
                    Dim unidadeObj = pmocAno.unidade.FirstOrDefault(Function(u) u.codigo_unidade = codigoUnidade)
                    If unidadeObj Is Nothing Then
                        unidadeObj = New PMOCAnoUnidade With {
                        .codigo_unidade = codigoUnidade,
                        .unidade = unidade,
                        .mes = New List(Of PMOCAnoUnidadeMes)(),
                        .bimestre = New List(Of PMOCAnoUnidadeBimestral)()
                    }
                        pmocAno.unidade.Add(unidadeObj)
                    End If

                    ' Adicionar o bimestre
                    unidadeObj.bimestre.Add(New PMOCAnoUnidadeBimestral With {
                    .bimestre = bimestre,
                    .descricao = descricao,
                    .startDate = startDate,
                    .endDate = endDate
                })
                End While

            End Using

            ' Retornar os dados agrupados
            Return anosAgrupados.Values.ToList()

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function PMOCAno(ByVal iCodigoEmpresa As Integer,
                            ByVal iCodigoUnidade As Integer) As List(Of PMOCAno)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of PMOCAno)
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
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_report_pmoc_ano", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New PMOCAno

                oInfo.ano = oSqlDataReader.Item("ano")
                oInfo.unidade = PMOCAnoUnidade(iCodigoEmpresa:=iCodigoEmpresa,
                                               iCodigoUnidade:=iCodigoUnidade,
                                               iAno:=oSqlDataReader.Item("ano"))

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

    Public Function PMOCAnoUnidade(ByVal iCodigoEmpresa As Integer,
                                   ByVal iCodigoUnidade As Integer,
                                   ByVal iAno As Integer) As List(Of PMOCAnoUnidade)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of PMOCAnoUnidade)
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

            'Seta Parametros - Ano
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ano"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iAno

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_report_pmoc_ano_unidade", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New PMOCAnoUnidade

                oInfo.unidade = oSqlDataReader("unidade")
                oInfo.codigo_unidade = oSqlDataReader("codigo_unidade")
                oInfo.mes = PMOCAnoUnidadeMes(iCodigoEmpresa:=iCodigoEmpresa,
                                              iCodigoUnidade:=oSqlDataReader.Item("codigo_unidade"),
                                              iAno:=iAno)

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

    Public Function PMOCAnoUnidadeBimestre(ByVal iCodigoEmpresa As Integer,
                                           ByVal iCodigoUnidade As Integer,
                                           ByVal iAno As Integer) As List(Of PMOCAnoUnidadeBimestral)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of PMOCAnoUnidadeBimestral)
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

            'Seta Parametros - Ano
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ano"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iAno

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_report_pmoc_ano_unidade_bimestre", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New PMOCAnoUnidadeBimestral

                oInfo.bimestre = oSqlDataReader("bimestre")
                oInfo.descricao = oSqlDataReader("descricao")
                oInfo.startDate = oSqlDataReader("start_date")
                oInfo.endDate = oSqlDataReader("end_date")

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

    Public Function PMOCAnoUnidadeMes(ByVal iCodigoEmpresa As Integer,
                                      ByVal iCodigoUnidade As Integer,
                                      ByVal iAno As Integer) As List(Of PMOCAnoUnidadeMes)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of PMOCAnoUnidadeMes)
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

            'Seta Parametros - Ano
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ano"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iAno

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_report_pmoc_ano_unidade_mes", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New PMOCAnoUnidadeMes

                oInfo.mes = oSqlDataReader("mes")
                oInfo.ano = oSqlDataReader("ano")
                oInfo.descricao = oSqlDataReader("descricao")
                oInfo.startDate = oSqlDataReader("start_date")
                oInfo.endDate = oSqlDataReader("end_date")

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

    Public Function PMOCRefresh(ByVal iCodigoEmpresa As Integer,
                                ByVal iCodigoUnidade As Integer,
                                ByVal sData As String) As String

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

            'Seta Parametros - Data
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sData

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_report_pmoc_mensal", oSqlParameter)

            'Retorno da Função
            Return "Relatório gerado com Sucesso!"

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: FUNCIONÁRIO - HORAS TRABALHADAS :::"

    Public Function NaoConformidade(ByVal iCodigoEmpresa As Integer,
                                    ByVal iCodigoUnidade As Integer,
                                    ByVal iCodigoModulo As Integer,
                                    ByVal iCodigoTipo As Integer,
                                    ByVal lCodigoManutencaoProgramada As Long,
                                    ByVal sData As String) As List(Of NaoConformidade)

        Try

            'Váriaveis Locais
            Dim oRelatorio As New List(Of NaoConformidade)
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

            'Seta Parametros - Tipo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_tipo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoTipo : i += 1

            'Seta Parametros - Código Manutenção Programada
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_pcm_programada"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoManutencaoProgramada : i += 1

            'Seta Parametros - Data 
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sData

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_report_nao_conformidade", oSqlParameter)

            While oSqlDataReader.Read

                Dim oRelatorioInfo As New NaoConformidade

                oRelatorioInfo.codigo_unidade = oSqlDataReader("codigo_unidade")
                oRelatorioInfo.unidade = oSqlDataReader("unidade")
                oRelatorioInfo.tipo = oSqlDataReader("tipo")
                oRelatorioInfo.codigo_pcm_programada = oSqlDataReader("codigo_pcm_programada")
                oRelatorioInfo.manutencao_programada = oSqlDataReader("manutencao_programada")
                oRelatorioInfo.descricao = oSqlDataReader("descricao")
                oRelatorioInfo.mes_1 = oSqlDataReader("mes_1")
                oRelatorioInfo.mes_2 = oSqlDataReader("mes_2")
                oRelatorioInfo.mes_3 = oSqlDataReader("mes_3")
                oRelatorioInfo.mes_4 = oSqlDataReader("mes_4")
                oRelatorioInfo.mes_5 = oSqlDataReader("mes_5")
                oRelatorioInfo.mes_6 = oSqlDataReader("mes_6")
                oRelatorioInfo.mes_7 = oSqlDataReader("mes_7")
                oRelatorioInfo.mes_8 = oSqlDataReader("mes_8")
                oRelatorioInfo.mes_9 = oSqlDataReader("mes_9")
                oRelatorioInfo.mes_10 = oSqlDataReader("mes_10")
                oRelatorioInfo.mes_11 = oSqlDataReader("mes_11")
                oRelatorioInfo.mes_12 = oSqlDataReader("mes_12")

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

#End Region

#Region "::: RELATORIO DINAMICO :::"

    Public Function RelatorioDinamico(ByVal iCodigoEmpresa As Integer,
                                      ByVal iCodigoUnidade As Integer,
                                      ByVal iCodigoModulo As Integer,
                                      ByVal lCodigoRelatorioItensAuditavies As Long) As List(Of RelatorioAuditoria)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of RelatorioAuditoria)
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

            'Seta Parametros - Código Módulo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_modulo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoModulo : i += 1

            'Seta Parametros - Código Relatório Itens Auditáveis
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_relatorio_itens_auditaveis"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoRelatorioItensAuditavies

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_relatorio_dinamico_checklist", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New RelatorioAuditoria

                oInfo.checklist = oSqlDataReader.Item("checklist")
                oInfo.codigo_tipo_item_checklist = oSqlDataReader.Item("codigo_tipo_item_checklist")
                oInfo.tipo_item_checklist = oSqlDataReader.Item("tipo_item_checklist")
                oInfo.unidade_medida = oSqlDataReader.Item("unidade_medida")
                oInfo.valor_minimo = oSqlDataReader.Item("valor_minimo")
                oInfo.valor_maximo = oSqlDataReader.Item("valor_maximo")

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

    Public Function RelatorioDinamicoData(ByVal iCodigoEmpresa As Integer,
                                          ByVal iCodigoUnidade As Integer,
                                          ByVal iCodigoModulo As Integer,
                                          ByVal lCodigoRelatorioItensAuditaveis As Long,
                                          ByVal sDataInicio As String,
                                          ByVal sDataTermino As String) As List(Of RelatorioAuditoriaDataValor)

        Try

            Dim oReturn As New List(Of RelatorioAuditoriaDataValor)
            Dim oInfo As RelatorioAuditoriaDataValor
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

            'Seta Parametros - Data Início
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_inicio"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataInicio : i += 1

            'Seta Parametros - Data Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_termino"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataTermino : i += 1

            'Seta Parametros - Código Relatório Itens Auditaveis
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_relatorio_itens_auditaveis"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoRelatorioItensAuditaveis

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_relatorio_dinamico_resultado", oSqlParameter)

            While oSqlDataReader.Read

                oInfo = New RelatorioAuditoriaDataValor

                oInfo.codigo_tipo_item = oSqlDataReader.Item("codigo_tipo_item")
                oInfo.data = oSqlDataReader.Item("data")
                oInfo.resultado = oSqlDataReader.Item("resultado")
                oInfo.unidade_medida = oSqlDataReader.Item("unidade_medida")
                oInfo.valor_minimo = oSqlDataReader.Item("valor_minimo")
                oInfo.valor_maximo = oSqlDataReader.Item("valor_maximo")
                oInfo.observacao = oSqlDataReader.Item("observacao")
                oInfo.css_class = oSqlDataReader.Item("css_class")

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

    Public Function RelatorioAuditoriaDataResultado(ByVal iCodigoEmpresa As Integer,
                                                    ByVal iCodigoUnidade As Integer,
                                                    ByVal lCodigoChecklist As Long,
                                                    ByVal iCodigoChecklistItem As Integer,
                                                    ByVal sData As String) As List(Of RelatorioAuditoriaDataValor)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of RelatorioAuditoriaDataValor)
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

            'Seta Parametros - Código Checklist
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_checklist"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = lCodigoChecklist : i += 1

            'Seta Parametros - Código Checklist Item
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_checklist_item"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoChecklistItem : i += 1

            'Seta Parametros - Data
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sData

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_auditoria_relatorio_resultado", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New RelatorioAuditoriaDataValor

                oInfo.resultado = oSqlDataReader.Item("resultado")
                oInfo.unidade_medida = oSqlDataReader.Item("unidade_medida")
                oInfo.valor_minimo = oSqlDataReader.Item("valor_minimo")
                oInfo.valor_maximo = oSqlDataReader.Item("valor_maximo")
                oInfo.observacao = oSqlDataReader.Item("observacao")

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

#Region "::: RELATORIO DINAMICO :::"

    Public Function RelatorioDinamicoDia(ByVal iCodigoEmpresa As Integer,
                                         ByVal iCodigoUnidade As Integer,
                                         ByVal iCodigoModulo As Integer,
                                         ByVal sData As String,
                                         ByVal lCodigoRelatorioItensAuditavies As Long) As List(Of RelatorioDinamicoDia)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of RelatorioDinamicoDia)
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

            'Seta Parametros - Mês
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "mes"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = CDate(sData).Month : i += 1

            'Seta Parametros - Ano
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ano"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = CDate(sData).Year : i += 1

            'Seta Parametros - Código Relatório Itens Auditáveis
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_relatorio_itens_auditaveis"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoRelatorioItensAuditavies

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_report_dinamico", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New RelatorioDinamicoDia

                oInfo.checklist = oSqlDataReader.Item("checklist")
                oInfo.descricao = oSqlDataReader.Item("descricao")
                oInfo.grupo = oSqlDataReader.Item("grupo")
                oInfo.subgrupo = oSqlDataReader.Item("subgrupo")
                oInfo.valorMinimo = oSqlDataReader.Item("valor_minimo")
                oInfo.valorMaximo = oSqlDataReader.Item("valor_maximo")
                oInfo.codigoTipoItemChecklist = oSqlDataReader.Item("codigo_tipo_item_checklist")
                oInfo.dia1 = oSqlDataReader.Item("1")
                oInfo.dia2 = oSqlDataReader.Item("2")
                oInfo.dia3 = oSqlDataReader.Item("3")
                oInfo.dia4 = oSqlDataReader.Item("4")
                oInfo.dia5 = oSqlDataReader.Item("5")
                oInfo.dia6 = oSqlDataReader.Item("6")
                oInfo.dia7 = oSqlDataReader.Item("7")
                oInfo.dia8 = oSqlDataReader.Item("8")
                oInfo.dia9 = oSqlDataReader.Item("9")
                oInfo.dia10 = oSqlDataReader.Item("10")
                oInfo.dia11 = oSqlDataReader.Item("11")
                oInfo.dia12 = oSqlDataReader.Item("12")
                oInfo.dia13 = oSqlDataReader.Item("13")
                oInfo.dia14 = oSqlDataReader.Item("14")
                oInfo.dia15 = oSqlDataReader.Item("15")
                oInfo.dia16 = oSqlDataReader.Item("16")
                oInfo.dia17 = oSqlDataReader.Item("17")
                oInfo.dia18 = oSqlDataReader.Item("18")
                oInfo.dia19 = oSqlDataReader.Item("19")
                oInfo.dia20 = oSqlDataReader.Item("20")
                oInfo.dia21 = oSqlDataReader.Item("21")
                oInfo.dia22 = oSqlDataReader.Item("22")
                oInfo.dia23 = oSqlDataReader.Item("23")
                oInfo.dia24 = oSqlDataReader.Item("24")
                oInfo.dia25 = oSqlDataReader.Item("25")
                oInfo.dia26 = oSqlDataReader.Item("26")
                oInfo.dia27 = oSqlDataReader.Item("27")
                oInfo.dia28 = oSqlDataReader.Item("28")
                oInfo.dia29 = oSqlDataReader.Item("29")
                oInfo.dia30 = oSqlDataReader.Item("30")
                oInfo.dia31 = oSqlDataReader.Item("31")

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

#Region "::: RELATORIO ENXOVAL :::"

    Public Function RelatorioConsumoEnxovalDia(ByVal iCodigoEmpresa As Integer,
                                               ByVal iCodigoUnidade As Integer,
                                               ByVal sData As String,
                                               ByVal iCodigoFormaAnalise As Integer) As List(Of RelatorioConsumoEnxoval)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of RelatorioConsumoEnxoval)
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

            'Seta Parametros - Código Forma Análise
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_forma_analise"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoFormaAnalise : i += 1

            'Seta Parametros - Mês
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "mes"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            If IsDate(sData) Then
                oSqlParameter(i).Value = CDate(sData).Month()
            Else
                oSqlParameter(i).Value = 0
            End If : i += 1

            'Seta Parametros - Ano
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ano"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            If IsDate(sData) Then
                oSqlParameter(i).Value = CDate(sData).Year()
            Else
                oSqlParameter(i).Value = 0
            End If : i += 1

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_report_governanca_consumo_enxoval", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New RelatorioConsumoEnxoval

                oInfo.enxoval = oSqlDataReader.Item("enxoval")
                oInfo.peso = oSqlDataReader.Item("peso")

                Dim lTotal As Long = 0

                ' Soma dinâmica das propriedades dia*
                For day As Integer = 1 To 31

                    Dim diaPropertyName As String = $"dia{day}"
                    Dim diaProperty As Object = Nothing

                    If Not IsDBNull(oSqlDataReader.Item(day.ToString())) Then
                        diaProperty = Convert.ToInt32(oSqlDataReader.Item(day.ToString()))
                        oInfo.GetType().GetProperty(diaPropertyName).SetValue(oInfo, diaProperty)
                        lTotal += Convert.ToInt32(diaProperty)
                    End If

                Next

                oInfo.total = lTotal
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

#Region "::: RELATORIO CAMAREIRA UH :::"

    Public Function RelatorioCamareiraUH(ByVal iCodigoEmpresa As Integer,
                                         ByVal iCodigoUnidade As Integer,
                                         ByVal sData As String,
                                         ByVal iCodigoCamareira As Integer) As List(Of RelatorioCamareiraUH)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of RelatorioCamareiraUH)
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

            'Seta Parametros - Código Camareira
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_camareira"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoCamareira : i += 1

            'Seta Parametros - Mês
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sData

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_report_governanca_camareira_uh", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New RelatorioCamareiraUH

                oInfo.camareira = oSqlDataReader.Item("camareira")

                Dim lTotal As Long = 0

                ' Soma dinâmica das propriedades dia*
                For day As Integer = 1 To 31

                    Dim diaPropertyName As String = $"dia{day}"
                    Dim diaProperty As Object = Nothing

                    If Not IsDBNull(oSqlDataReader.Item(day.ToString())) Then
                        diaProperty = Convert.ToInt32(oSqlDataReader.Item(day.ToString()))
                        oInfo.GetType().GetProperty(diaPropertyName).SetValue(oInfo, diaProperty)
                        lTotal += Convert.ToInt32(diaProperty)
                    End If

                Next

                oInfo.total = lTotal
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

#Region "::: RELATORIO RESPONSÁVEL VISTORIA UH :::"

    Public Function RelatorioResponsavelVistoria(ByVal iCodigoEmpresa As Integer,
                                                 ByVal iCodigoUnidade As Integer,
                                                 ByVal sData As String,
                                                 ByVal iCodigoFuncionarioGovernanca As Integer) As List(Of RelatorioResponsavelVistoriaUH)

        Try

            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, iCodigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, iCodigoUnidade),
                CriarParametro("data", SqlDbType.Date, IIf(IsDate(sData), sData, DBNull.Value)),
                CriarParametro("codigo_funcionario", SqlDbType.Int, iCodigoFuncionarioGovernanca)
            }

            'Váriaveis Locais
            Dim oReturn As New List(Of RelatorioResponsavelVistoriaUH)

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_report_governanca_responsavel_vistoria_uh", oSqlParameter)

                While oSqlDataReader.Read

                    Dim oInfo As New RelatorioResponsavelVistoriaUH

                    oInfo.responsavelVistoria = oSqlDataReader.Item("responsavel_vistoria")

                    Dim lTotal As Long = 0

                    ' Soma dinâmica das propriedades dia*
                    For day As Integer = 1 To 31

                        Dim diaPropertyName As String = $"dia{day}"
                        Dim diaProperty As Object = Nothing

                        If Not IsDBNull(oSqlDataReader.Item(day.ToString())) Then
                            diaProperty = Convert.ToInt32(oSqlDataReader.Item(day.ToString()))
                            oInfo.GetType().GetProperty(diaPropertyName).SetValue(oInfo, diaProperty)
                            lTotal += Convert.ToInt32(diaProperty)
                        End If

                    Next

                    oInfo.total = lTotal
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

#Region "::: RELATORIO CAMAREIRA NC :::"

    Public Function RelatorioCamareiraNC(ByVal codigoEmpresa As Integer,
                                         ByVal codigoUnidade As Integer,
                                         ByVal data As String,
                                         ByVal codigoCamareira As Integer,
                                         ByVal tipoNC As Integer,
                                         ByVal viewReportNC As Integer) As List(Of RelatorioCamareiraNC)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of RelatorioCamareiraNC)

            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("data", SqlDbType.Date, IIf(IsDate(data), data, DBNull.Value)),
                CriarParametro("codigoCamareira", SqlDbType.Int, codigoCamareira),
                CriarParametro("tipoNC", SqlDbType.Int, tipoNC),
                CriarParametro("viewReportNC", SqlDbType.Int, viewReportNC)
            }

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_report_governanca_camareira_nc", oSqlParameter)

                While oSqlDataReader.Read

                    Dim oInfo As New RelatorioCamareiraNC

                    oInfo.camareira = oSqlDataReader.Item("camareira")
                    oInfo.naoConformidade = oSqlDataReader.Item("nao_conformidade")

                    Dim lTotal As Long = 0

                    ' Soma dinâmica das propriedades dia*
                    For day As Integer = 1 To 31

                        Dim diaPropertyName As String = $"dia{day}"
                        Dim diaProperty As Object = Nothing

                        If Not IsDBNull(oSqlDataReader.Item(day.ToString())) Then
                            diaProperty = oSqlDataReader.Item(day.ToString())
                            oInfo.GetType().GetProperty(diaPropertyName).SetValue(oInfo, diaProperty.ToString())
                            lTotal += Convert.ToInt32(diaProperty)
                        End If

                    Next

                    oInfo.total = lTotal
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
