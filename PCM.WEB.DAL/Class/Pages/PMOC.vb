Imports PCM.WEB.MODELS
Imports System.DirectoryServices
Imports System.DirectoryServices.ActiveDirectory
Imports PCM.WEB.DAL.SQLHelper
Imports System.Data.SqlClient
Imports System.Text

Public Class PMOC

    Private sConnection As String

    Sub New(ByVal sCon As String)
        sConnection = sCon
    End Sub

    Public Sub UpdateStatus(ByVal lCodigoPMOCOrdemServico As Long,
                            ByVal iCodigoEmpresa As Integer,
                            ByVal iCodigoUsuario As Integer,
                            ByVal iCodigoUnidade As Integer,
                            ByVal iStatus As Integer)

        Try

            'Váriaveis Locais
            Dim oSqlParameter(4) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código PMOC Ordem Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_pmoc_ordem_servico"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoPMOCOrdemServico : i += 1

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
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Status
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "status"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iStatus

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_pmoc_ordem_servico_status", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#Region "::: PMOC :::"

    Public Function LoadPMOCMesFechado(ByVal iCodigoEmpresa As Integer,
                                       ByVal iCodigoUnidade As Integer,
                                       ByVal sData As String,
                                       Optional ByVal iStatus As Integer = -1) As List(Of PMOCMesFechado)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of PMOCMesFechado)
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

            'Seta Parametros - Status
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "status"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iStatus : i += 1

            'Seta Parametros - Data
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = CDate(sData)

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pmoc_mes_fechado", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New PMOCMesFechado

                oInfo.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oInfo.unidade = oSqlDataReader.Item("nome_fantasia")
                oInfo.andar = oSqlDataReader.Item("andar")
                oInfo.tipo_ar_condicionado = oSqlDataReader.Item("tipo_ar_condicionado")
                oInfo.codigo_tipo_ar_condicionado = oSqlDataReader.Item("codigo_tipo_ar_condicionado")
                oInfo.codigo_pmoc_ordem_servico = oSqlDataReader.Item("codigo_pmoc_ordem_servico")
                oInfo.codigo_equipamento = oSqlDataReader.Item("codigo_equipamento")
                oInfo.quantidade_executada = oSqlDataReader.Item("quantidade_executada")
                oInfo.quantidade_necessaria = oSqlDataReader.Item("quantidade_necessaria")
                oInfo.data_ultima = oSqlDataReader.Item("data_ultima")
                oInfo.data_proxima = oSqlDataReader.Item("data_proxima")
                oInfo.data_proxima_formatada = oSqlDataReader.Item("data_proxima_formatada")
                oInfo.tag = oSqlDataReader.Item("tag")
                oInfo.descricao = oSqlDataReader.Item("descricao")
                oInfo.status = oSqlDataReader.Item("status")
                oInfo.descricao_status = oSqlDataReader.Item("descricao_status")
                oInfo.periodicidade = oSqlDataReader.Item("periodicidade")
                oInfo.cor = oSqlDataReader.Item("cor")
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

    Public Function LoadPMOC2(ByVal iCodigoEmpresa As Integer,
                              ByVal iCodigoUnidade As Integer,
                              Optional ByVal iStatus As Integer = -1) As List(Of PMOC2)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of PMOC2)
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

            'Seta Parametros - Status
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "status"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iStatus

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pmoc2", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New PMOC2

                oInfo.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oInfo.unidade = oSqlDataReader.Item("nome_fantasia")
                oInfo.tipo_ar_condicionado = oSqlDataReader.Item("tipo_ar_condicionado")
                oInfo.codigo_tipo_ar_condicionado = oSqlDataReader.Item("codigo_tipo_ar_condicionado")
                oInfo.codigo_pmoc_ordem_servico = oSqlDataReader.Item("codigo_pmoc_ordem_servico")
                oInfo.codigo_equipamento = oSqlDataReader.Item("codigo_equipamento")
                oInfo.data_ultima = oSqlDataReader.Item("data_ultima")
                oInfo.data_proxima = oSqlDataReader.Item("data_proxima")
                oInfo.data_proxima_formatada = oSqlDataReader.Item("data_proxima_formatada")
                oInfo.tag = oSqlDataReader.Item("tag")
                oInfo.descricao = oSqlDataReader.Item("descricao")
                oInfo.status = oSqlDataReader.Item("status")
                oInfo.descricao_status = oSqlDataReader.Item("descricao_status")
                oInfo.periodicidade = oSqlDataReader.Item("periodicidade")
                oInfo.cor = oSqlDataReader.Item("cor")
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

    Public Function LoadPMOC(ByVal iCodigoEmpresa As Integer,
                             ByVal iCodigoUnidade As Integer,
                             Optional ByVal iStatus As Integer = -1) As List(Of PMOCUnidade)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of PMOCUnidade)
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

            'Seta Parametros - Status
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "status"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iStatus

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pmoc_unidade", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New PMOCUnidade

                oInfo.codigo_unidade = oSqlDataReader.Item("codigo")
                oInfo.unidade = oSqlDataReader.Item("nome_fantasia")
                oInfo.registro = False
                oInfo.tipo_ar_condicionado = LoadPMOCTipoArCondicionado(iCodigoEmpresa:=iCodigoEmpresa,
                                                                        iCodigoUnidade:=oSqlDataReader.Item("codigo"),
                                                                        oInfoUnidade:=oInfo,
                                                                        iStatus:=iStatus)

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
    
    Public Function LoadPMOCTipoArCondicionado(ByVal iCodigoEmpresa As Integer,
                                               ByVal iCodigoUnidade As Integer,
                                               ByRef oInfoUnidade As PMOCUnidade,
                                               Optional ByVal iStatus As Integer = -1) As List(Of PMOCTipoArCondicionado)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of PMOCTipoArCondicionado)
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

            'Seta Parametros - Status
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "status"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iStatus

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pmoc_unidade_tipo_ar_condicionado", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New PMOCTipoArCondicionado

                oInfo.codigo_tipo_ar_condicionado = oSqlDataReader.Item("codigo_tipo_ar_condicionado")
                oInfo.tipo_ar_condicionado = oSqlDataReader.Item("tipo_ar_condicionado")
                oInfo.equipamento = LoadPMOCEquipamento(iCodigoEmpresa:=iCodigoEmpresa,
                                                        iCodigoUnidade:=iCodigoUnidade,
                                                        iCodigoTipoArCondicionado:=oSqlDataReader.Item("codigo_tipo_ar_condicionado"),
                                                        iStatus:=iStatus)
                oInfoUnidade.registro = IIf(oInfo.equipamento.Count > 0, True, oInfoUnidade.registro)
                oInfo.registro = IIf(oInfo.equipamento.Count > 0, True, False)

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

    Public Function LoadPMOCEquipamento(ByVal iCodigoEmpresa As Integer,
                                        ByVal iCodigoUnidade As Integer,
                                        ByVal iCodigoTipoArCondicionado As Integer,
                                        Optional ByVal iStatus As Integer = -1) As List(Of PMOCEquipamento)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of PMOCEquipamento)
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

            'Seta Parametros - Código Tipo Ar Condicionado
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_tipo_ar_condicionado"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoTipoArCondicionado : i += 1

            'Seta Parametros - Status
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "status"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iStatus

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pmoc_unidade_tipo_ar_condicionao_equipamento", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New PMOCEquipamento

                oInfo.codigo_pmoc_ordem_servico = oSqlDataReader.Item("codigo_pmoc_ordem_servico")
                oInfo.codigo_equipamento = oSqlDataReader.Item("codigo_equipamento")
                oInfo.data = oSqlDataReader.Item("data")
                oInfo.tag = oSqlDataReader.Item("tag")
                oInfo.descricao = oSqlDataReader.Item("descricao")
                oInfo.status = oSqlDataReader.Item("status")
                oInfo.descricao_status = oSqlDataReader.Item("descricao_status")
                oInfo.periodicidade = oSqlDataReader.Item("periodicidade")
                oInfo.cor = oSqlDataReader.Item("cor")

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

    Public Function LoadPMOCStatus(ByVal iCodigoEmpresa As Integer,
                                   ByVal iCodigoUnidade As Integer) As PMOCStatus

        Try

            'Váriaveis Locais
            Dim oReturn As New PMOCStatus
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
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pmoc_status", oSqlParameter)

            While oSqlDataReader.Read

                oReturn.quantidade_pendente = oSqlDataReader.Item("quantidade_pendente")
                oReturn.quantidade_concluido = oSqlDataReader.Item("quantidade_concluido")
                oReturn.quantidade_atrasado = oSqlDataReader.Item("quantidade_atrasado")
                oReturn.quantidade_em_andamento = oSqlDataReader.Item("quantidade_em_andamento")

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

    Public Function LoadPMOCStatusMesFechado(ByVal iCodigoEmpresa As Integer,
                                             ByVal iCodigoUnidade As Integer,
                                             ByVal iIntervalo As Integer) As PMOCStatus

        Try

            'Váriaveis Locais
            Dim oReturn As New PMOCStatus
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

            'Seta Parametros - Intervalo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "intervalo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iIntervalo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pmoc_status_mes_fechado", oSqlParameter)

            While oSqlDataReader.Read

                oReturn.quantidade_pendente = oSqlDataReader.Item("quantidade_pendente")
                oReturn.quantidade_concluido = oSqlDataReader.Item("quantidade_concluido")
                oReturn.quantidade_atrasado = oSqlDataReader.Item("quantidade_atrasado")
                oReturn.quantidade_em_andamento = oSqlDataReader.Item("quantidade_em_andamento")

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

#Region "::: APONTAMENTO :::"

    Public Function OrdemServicoPMOC(ByVal iCodigoEmpresa As Integer,
                                     ByVal iCodigoUnidade As Integer,
                                     ByVal lCodigoPMOCOrdemServico As Long) As PMOCOrdemServico

        Try

            'Váriaveis Locais
            Dim oReturn As New PMOCOrdemServico
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

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoPMOCOrdemServico : i += 1

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pmoc_ordem_servico", oSqlParameter)

            While oSqlDataReader.Read

                oReturn.codigo_pmoc_ordem_servico = oSqlDataReader.Item("codigo")
                oReturn.codigo_equipamento = oSqlDataReader.Item("codigo_equipamento")
                oReturn.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oReturn.unidade = oSqlDataReader.Item("unidade")
                oReturn.setor = oSqlDataReader.Item("setor")
                oReturn.codigo_equipamento = oSqlDataReader.Item("codigo_equipamento")
                oReturn.equipamento = oSqlDataReader.Item("equipamento")
                oReturn.apartamento = oSqlDataReader.Item("apartamento")
                oReturn.tipo_ar_condicionado = oSqlDataReader.Item("tipo_ar_condicionado")
                oReturn.data = oSqlDataReader.Item("data")
                oReturn.status = oSqlDataReader.Item("status")
                oReturn.aponta_horas = oSqlDataReader.Item("aponta_horas")
                oReturn.apontamento = OrdemServicoPMOCApontamento(iCodigoEmpresa:=iCodigoEmpresa,
                                                                  iCodigoUnidade:=iCodigoUnidade,
                                                                  lCodigoPMOCOrdemServico:=lCodigoPMOCOrdemServico)
                oReturn.checklist = OrdemServicoPMOCChecklist(iCodigoEmpresa:=iCodigoEmpresa,
                                                              iCodigoUnidade:=iCodigoUnidade,
                                                              lCodigoPMOCOrdemServico:=lCodigoPMOCOrdemServico)

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

    Public Function OrdemServicoPMOCApontamento(ByVal iCodigoEmpresa As Integer,
                                                ByVal iCodigoUnidade As Integer,
                                                ByVal lCodigoPMOCOrdemServico As Long) As List(Of PMOCOrdemServicoApontamento)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of PMOCOrdemServicoApontamento)
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

            'Seta Parametros - Código PMOC Ordem Servico
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_pmoc_ordem_servico"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = lCodigoPMOCOrdemServico

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pmoc_ordem_servico_apontamento", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New PMOCOrdemServicoApontamento

                oInfo.codigo_pmoc_ordem_servico = oSqlDataReader.Item("codigo_pmoc_ordem_servico")
                oInfo.codigo = oSqlDataReader.Item("codigo")
                oInfo.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oInfo.codigo_executor = oSqlDataReader.Item("codigo_executor")
                oInfo.executor = oSqlDataReader.Item("executor")
                oInfo.data_inicio = oSqlDataReader.Item("data_inicio")
                oInfo.data_termino = oSqlDataReader.Item("data_termino")

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

    Public Function OrdemServicoPMOCChecklist(ByVal iCodigoEmpresa As Integer,
                                              ByVal iCodigoUnidade As Integer,
                                              ByVal lCodigoPMOCOrdemServico As Long) As List(Of PMOCOrdemServicoChecklist)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of PMOCOrdemServicoChecklist)
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

            'Seta Parametros - Código PMOC Ordem Servico
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_pmoc_ordem_servico"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = lCodigoPMOCOrdemServico

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pmoc_ordem_servico_checklist", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New PMOCOrdemServicoChecklist

                oInfo.codigo = oSqlDataReader.Item("codigo")
                oInfo.descricao = oSqlDataReader.Item("descricao")
                oInfo.codigo_tipo_item_checklist = oSqlDataReader.Item("codigo_tipo_item_checklist")
                oInfo.grupo = oSqlDataReader.Item("grupo")
                oInfo.subgrupo = oSqlDataReader.Item("subgrupo")
                oInfo.checklist = oSqlDataReader.Item("checklist")
                oInfo.resultado = oSqlDataReader.Item("resultado")
                oInfo.valor_minimo = oSqlDataReader.Item("valor_minimo")
                oInfo.valor_maximo = oSqlDataReader.Item("valor_maximo")
                oInfo.observacao = oSqlDataReader.Item("observacao")
                oInfo.unidade_medida = oSqlDataReader.Item("unidade_medida")
                oInfo.allow_picture = oSqlDataReader.Item("allow_picture")
                oInfo.numero = oSqlDataReader.Item("numero")
                oInfo.texto = oSqlDataReader.Item("texto")
                oInfo.data = oSqlDataReader.Item("data")
                oInfo.hora = oSqlDataReader.Item("hora")

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

    Public Sub UpdateOrdemServicoPMOC(ByVal iCodigoEmpresa As Integer,
                                      ByVal iCodigoUsuario As Integer,
                                      ByVal iCodigoUnidade As Integer,
                                      ByVal lCodigoPMOCOrdemServico As Long,
                                      ByVal sArquivo As String,
                                      ByVal sPathArquivo As String,
                                      ByVal bConcluido As Boolean)

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

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoPMOCOrdemServico : i += 1

            'Seta Parametros - Arquivo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "arquivo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = sArquivo : i += 1

            'Seta Parametros - Path Arquivo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "path_arquivo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = sPathArquivo : i += 1

            'Seta Parametros - Concluído
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "concluido"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bConcluido

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_pmoc_ordem_servico", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub DeleteOrdemServicoPMOC(ByVal iCodigoEmpresa As Integer,
                                      ByVal iCodigoUnidade As Integer,
                                      ByVal lCodigoPMOCOrdemServico As Long,
                                      ByVal iCodigoUsuario As Integer)

        Try

            'Váriaveis Locais
            Dim oSqlParameter(3) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoPMOCOrdemServico : i += 1

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
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_pmoc_ordem_servico", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InsertOrdemServicoPMOC(ByVal iCodigoEmpresa As Integer,
                                      ByVal iCodigoUsuario As Integer,
                                      ByVal iCodigoUnidade As Integer,
                                      ByVal lCodigoEquipamento As Long,
                                      ByVal sArquivo As String,
                                      ByVal sPathArquivo As String,
                                      ByVal bConcluido As Boolean,
                                      ByRef lCodigoPMOCOrdemServico As Long)

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

            'Seta Parametros - Código Equipamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_equipamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoEquipamento : i += 1

            'Seta Parametros - Arquivo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "arquivo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = sArquivo : i += 1

            'Seta Parametros - Path Arquivo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "path_arquivo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = sPathArquivo : i += 1

            'Seta Parametros - Concluído
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "concluido"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bConcluido : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Output
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_pmoc_ordem_servico", oSqlParameter)

            lCodigoPMOCOrdemServico = oSqlParameter(i).Value

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InsertOrdemServicoPMOC(ByVal iCodigoEmpresa As Integer,
                                      ByVal iCodigoUsuario As Integer,
                                      ByVal iCodigoUnidade As Integer,
                                      ByVal lCodigoEquipamento As Long,
                                      ByVal iIntervalo As Integer,
                                      ByVal sArquivo As String,
                                      ByVal sPathArquivo As String,
                                      ByVal bConcluido As Boolean,
                                      ByRef lCodigoPMOCOrdemServico As Long)

        Try

            'Váriaveis Locais
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

            'Seta Parametros - Código Equipamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_equipamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoEquipamento : i += 1

            'Seta Parametros - Intervalo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "intervalo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iIntervalo : i += 1

            'Seta Parametros - Arquivo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "arquivo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = sArquivo : i += 1

            'Seta Parametros - Path Arquivo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "path_arquivo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = sPathArquivo : i += 1

            'Seta Parametros - Concluído
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "concluido"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bConcluido : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Output
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_pmoc_ordem_servico_intervalo", oSqlParameter)

            lCodigoPMOCOrdemServico = oSqlParameter(i).Value

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InsertApontamentoPMOC(ByVal iCodigoEmpresa As Integer,
                                     ByVal iCodigoUsuario As Integer,
                                     ByVal iCodigoUnidade As Integer,
                                     ByVal lCodigoPMOCOrdemServico As Long,
                                     ByVal iCodigoFornecedor As Integer,
                                     ByVal iCodigoFuncionario As Integer,
                                     ByVal sDataInicio As String,
                                     ByVal sDataTermino As String,
                                     ByVal sHoraInicio As String,
                                     ByVal sHoraTermino As String)

        Try

            'Váriaveis Locais
            Dim oSqlParameter(9) As SqlParameter
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

            'Seta Parametros - Código PMOC - Ordem de Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_pmoc_ordem_servico"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoPMOCOrdemServico : i += 1

            'Seta Parametros - Código Fornecedor
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_fornecedor"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoFornecedor = -1, DBNull.Value, iCodigoFornecedor) : i += 1

            'Seta Parametros - Código Funcionário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_funcionario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoFuncionario = -1, DBNull.Value, iCodigoFuncionario) : i += 1

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
            oSqlParameter(i).Value = IIf(IsDate(sHoraTermino), sHoraTermino, DBNull.Value)

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_pmoc_apontamento", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InsertApontamentoPMOCJustificativa(ByVal iCodigoEmpresa As Integer,
                                                  ByVal iCodigoUsuario As Integer,
                                                  ByVal iCodigoUnidade As Integer,
                                                  ByVal lCodigoEquipamento As Long,
                                                  ByVal iCodigoJustificativaApontamento As Integer)

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

            'Seta Parametros - Código Equipamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_equipamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoEquipamento : i += 1

            'Seta Parametros - Código Justificativa Apontamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_justificativa_apontamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoJustificativaApontamento

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_pmoc_apontamento_justificativa", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InsertChecklistPMOC(ByVal iCodigoEmpresa As Integer,
                                   ByVal iCodigoUsuario As Integer,
                                   ByVal iCodigoUnidade As Integer,
                                   ByVal lCodigoPMOCOrdemServico As Long,
                                   ByVal iCodigoChecklistItem As Integer,
                                   ByVal sResultado As String,
                                   ByVal sObservacao As String)

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

            'Seta Parametros - Código PMOC - Ordem de Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_pmoc_ordem_servico"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoPMOCOrdemServico : i += 1

            'Seta Parametros - Código Checklist Item
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_checklist_item"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoChecklistItem : i += 1

            'Seta Parametros - Resultado
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "resultado"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 5000
            oSqlParameter(i).Value = sResultado : i += 1

            'Seta Parametros - Observação
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "observacao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = sObservacao

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_pmoc_ordem_servico_checklist", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub LoadApontamentoPMOC(ByVal iCodigoEmpresa As Integer,
                                   ByVal lCodigoEquipamento As Long,
                                   ByVal iCodigoUnidade As Integer,
                                   ByRef oPMOCApontamento As PMOCApontamento)

        Try

            'Váriaveis Locais
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(2) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Equipamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_equipamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoEquipamento : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade


            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pmoc_apontamento", oSqlParameter)

            While oSqlDataReader.Read

                oPMOCApontamento = New PMOCApontamento

                oPMOCApontamento.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oPMOCApontamento.unidade = oSqlDataReader.Item("unidade")
                oPMOCApontamento.setor = oSqlDataReader.Item("setor")
                oPMOCApontamento.codigo_equipamento = oSqlDataReader.Item("codigo_equipamento")
                oPMOCApontamento.equipamento = oSqlDataReader.Item("equipamento")
                oPMOCApontamento.tipo_ar_condicionado = oSqlDataReader.Item("tipo_ar_condicionado")
                oPMOCApontamento.apartamento = oSqlDataReader.Item("apartamento")
                oPMOCApontamento.aponta_horas = oSqlDataReader.Item("aponta_horas")

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function LoadApontamentoCheckListPMOC(ByVal iCodigoEmpresa As Integer,
                                                 ByVal lCodigoEquipamento As Long,
                                                 ByVal sDataManutencao As String,
                                                 ByVal iCodigoUnidade As Integer) As List(Of PMOCOrdemServicoChecklist)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of PMOCOrdemServicoChecklist)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(3) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Equipamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_equipamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoEquipamento : i += 1

            'Seta Parametros - Data Manutenção
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_manutencao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataManutencao : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pmoc_apontamento_checklist", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New PMOCOrdemServicoChecklist

                oInfo.codigo = oSqlDataReader.Item("codigo")
                oInfo.descricao = oSqlDataReader.Item("descricao")
                oInfo.codigo_tipo_item_checklist = oSqlDataReader.Item("codigo_tipo_item_checklist")
                oInfo.grupo = oSqlDataReader.Item("grupo")
                oInfo.subgrupo = oSqlDataReader.Item("subgrupo")
                oInfo.checklist = oSqlDataReader.Item("checklist")
                oInfo.resultado = oSqlDataReader.Item("resultado")
                oInfo.valor_minimo = oSqlDataReader.Item("valor_minimo")
                oInfo.valor_maximo = oSqlDataReader.Item("valor_maximo")
                oInfo.observacao = oSqlDataReader.Item("observacao")
                oInfo.unidade_medida = oSqlDataReader.Item("unidade_medida")
                oInfo.allow_picture = oSqlDataReader.Item("allow_picture")

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

    Public Function LoadApontamentoCheckListPMOC(ByVal iCodigoEmpresa As Integer,
                                                 ByVal lCodigoEquipamento As Long,
                                                 ByVal iIntervalo As Integer,
                                                 ByVal iCodigoUnidade As Integer) As List(Of PMOCOrdemServicoChecklist)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of PMOCOrdemServicoChecklist)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(3) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Equipamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_equipamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoEquipamento : i += 1

            'Seta Parametros - Intervalo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "intervalo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iIntervalo : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pmoc_apontamento_checklist_intervalo", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New PMOCOrdemServicoChecklist

                oInfo.codigo = oSqlDataReader.Item("codigo")
                oInfo.descricao = oSqlDataReader.Item("descricao")
                oInfo.codigo_tipo_item_checklist = oSqlDataReader.Item("codigo_tipo_item_checklist")
                oInfo.grupo = oSqlDataReader.Item("grupo")
                oInfo.subgrupo = oSqlDataReader.Item("subgrupo")
                oInfo.checklist = oSqlDataReader.Item("checklist")
                oInfo.resultado = oSqlDataReader.Item("resultado")
                oInfo.valor_minimo = oSqlDataReader.Item("valor_minimo")
                oInfo.valor_maximo = oSqlDataReader.Item("valor_maximo")
                oInfo.observacao = oSqlDataReader.Item("observacao")
                oInfo.unidade_medida = oSqlDataReader.Item("unidade_medida")
                oInfo.allow_picture = oSqlDataReader.Item("allow_picture")

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

    Public Sub LoadApontamentoPMOCInfo(ByVal iCodigoEmpresa As Integer,
                                       ByVal lCodigoPMOCOrdemServico As Long,
                                       ByVal iCodigoUnidade As Integer,
                                       ByVal lCodigoPMOCApontamento As Long,
                                       ByRef oPMOCApontamento As PMOCApontamento)

        Try

            'Váriaveis Locais
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(3) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código PMOC Ordem Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_pmoc_ordem_servico"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoPMOCOrdemServico : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoPMOCApontamento : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pmoc_apontamento_dados", oSqlParameter)

            While oSqlDataReader.Read

                oPMOCApontamento = New PMOCApontamento

                oPMOCApontamento.codigo_pmoc_ordem_servico = oSqlDataReader.Item("codigo_pmoc_ordem_servico")
                oPMOCApontamento.unidade = oSqlDataReader.Item("unidade")
                oPMOCApontamento.setor = oSqlDataReader.Item("setor")
                oPMOCApontamento.codigo_equipamento = oSqlDataReader.Item("codigo_equipamento")
                oPMOCApontamento.equipamento = oSqlDataReader.Item("equipamento")
                oPMOCApontamento.funcionario = oSqlDataReader.Item("funcionario")
                oPMOCApontamento.fornecedor = oSqlDataReader.Item("fornecedor")
                oPMOCApontamento.apartamento = oSqlDataReader.Item("apartamento")
                oPMOCApontamento.tipo_ar_condicionado = oSqlDataReader.Item("tipo_ar_condicionado")
                oPMOCApontamento.codigo_funcionario = oSqlDataReader.Item("codigo_funcionario")
                oPMOCApontamento.codigo_fornecedor = oSqlDataReader.Item("codigo_fornecedor")
                oPMOCApontamento.data_inicio = oSqlDataReader.Item("data_inicio")
                oPMOCApontamento.data_termino = oSqlDataReader.Item("data_termino")
                oPMOCApontamento.hora_inicio = oSqlDataReader.Item("hora_inicio")
                oPMOCApontamento.hora_termino = oSqlDataReader.Item("hora_termino")
                oPMOCApontamento.aponta_horas = oSqlDataReader.Item("aponta_horas")
                oPMOCApontamento.concluido = oSqlDataReader.Item("concluido")
                oPMOCApontamento.codigo = oSqlDataReader.Item("codigo_apontamento")
                oPMOCApontamento.codigo_unidade = oSqlDataReader.Item("codigo_unidade")

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateApontamentoPMOC(ByVal iCodigoEmpresa As Integer,
                                     ByVal iCodigoUsuario As Integer,
                                     ByVal iCodigoUnidade As Integer,
                                     ByVal lCodigoPMOCOrdemServico As Long,
                                     ByVal lCodigoApontamento As Long,
                                     ByVal iCodigoFornecedor As Integer,
                                     ByVal iCodigoFuncionario As Integer,
                                     ByVal sDataInicio As String,
                                     ByVal sDataTermino As String,
                                     ByVal sHoraInicio As String,
                                     ByVal sHoraTermino As String)

        Try

            'Váriaveis Locais
            Dim oSqlParameter(10) As SqlParameter
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

            'Seta Parametros - Código PMOC Ordem Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_pmoc_ordem_servico"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoPMOCOrdemServico : i += 1

            'Seta Parametros - Código Apontamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoApontamento : i += 1

            'Seta Parametros - Código Fornecedor
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_fornecedor"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoFornecedor = -1, DBNull.Value, iCodigoFornecedor) : i += 1

            'Seta Parametros - Código Funcionário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_funcionario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoFuncionario = -1, DBNull.Value, iCodigoFuncionario) : i += 1

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
            oSqlParameter(i).Value = IIf(IsDate(sHoraTermino), sHoraTermino, DBNull.Value)

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_pmoc_apontamento", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub DeleteApontamentoPMOC(ByVal iCodigoEmpresa As Integer,
                                     ByVal iCodigoUsuario As Integer,
                                     ByVal iCodigoUnidade As Integer,
                                     ByVal lCodigoPMOCOrdemServico As Long,
                                     ByVal lCodigo As Long)

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

            'Seta Parametros - Código PMOC Ordem Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_pmoc_ordem_servico"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoPMOCOrdemServico : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_pmoc_apontamento", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region

#Region "::: CRONOGRAMA :::"

    Public Function CronogramaPMOC(ByVal iCodigoEmpresa As Integer,
                                   ByVal iCodigoUnidade As Integer,
                                   ByVal sData As String) As List(Of PMOCCronograma)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of PMOCCronograma)
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
            oSqlParameter(i).Value = sData

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pmoc_cronograma", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New PMOCCronograma

                oInfo.unidade = oSqlDataReader.Item("unidade")
                oInfo.tag = oSqlDataReader.Item("tag")
                oInfo.descricao = oSqlDataReader.Item("descricao")
                oInfo.mes_1 = oSqlDataReader.Item("mes_1")
                oInfo.mes_2 = oSqlDataReader.Item("mes_2")
                oInfo.mes_3 = oSqlDataReader.Item("mes_3")
                oInfo.mes_4 = oSqlDataReader.Item("mes_4")
                oInfo.mes_5 = oSqlDataReader.Item("mes_5")
                oInfo.mes_6 = oSqlDataReader.Item("mes_6")
                oInfo.mes_7 = oSqlDataReader.Item("mes_7")
                oInfo.mes_8 = oSqlDataReader.Item("mes_8")
                oInfo.mes_9 = oSqlDataReader.Item("mes_9")
                oInfo.mes_10 = oSqlDataReader.Item("mes_10")
                oInfo.mes_11 = oSqlDataReader.Item("mes_11")
                oInfo.mes_12 = oSqlDataReader.Item("mes_12")

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

#Region "::: PMOC ANDAR :::"

    Public Function PMOCAndar(ByVal iCodigoEmpresa As Integer,
                              ByVal iCodigoUnidade As Integer,
                              ByVal sData As String) As List(Of PMOCAndar)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of PMOCAndar)
            Dim oDataSet As DataSet
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
            oDataSet = ExecuteDataset(sConnection, CommandType.StoredProcedure, "sp_select_pmoc_andar", oSqlParameter)

            Dim oDatatable As DataTable = oDataSet.Tables(0)

            If oDatatable IsNot Nothing Then

                For i = 0 To oDatatable.Columns.Count - 1

                    Dim oInfo As New PMOCAndar

                    oInfo.andar = oDatatable.Columns(i).ColumnName
                    oInfo.equipamentos = New List(Of PMOCAndarEquipamento)

                    For Each row As DataRow In oDatatable.Rows

                        Dim oInfoEquipamento As New PMOCAndarEquipamento

                        oInfoEquipamento.id = row("id")
                        oInfoEquipamento.value = IIf(IsDBNull(row(i)), "", row(i))

                        oInfo.equipamentos.Add(oInfoEquipamento)

                    Next

                    oReturn.Add(oInfo)

                Next

            End If

            'Retorno da Função
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: CRONOGRAMA :::"

    Public Function PMOCHistorico(ByVal iCodigoEmpresa As Integer,
                                  ByVal iCodigoUnidade As Integer,
                                  ByVal iAno As Integer) As List(Of PMOCCronograma)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of PMOCCronograma)
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
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pmoc_historico_mes_fechado", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New PMOCCronograma

                oInfo.unidade = oSqlDataReader.Item("unidade")
                oInfo.tag = oSqlDataReader.Item("tag")
                oInfo.descricao = oSqlDataReader.Item("descricao")
                oInfo.mes_1 = oSqlDataReader.Item("mes_1")
                oInfo.mes_2 = oSqlDataReader.Item("mes_2")
                oInfo.mes_3 = oSqlDataReader.Item("mes_3")
                oInfo.mes_4 = oSqlDataReader.Item("mes_4")
                oInfo.mes_5 = oSqlDataReader.Item("mes_5")
                oInfo.mes_6 = oSqlDataReader.Item("mes_6")
                oInfo.mes_7 = oSqlDataReader.Item("mes_7")
                oInfo.mes_8 = oSqlDataReader.Item("mes_8")
                oInfo.mes_9 = oSqlDataReader.Item("mes_9")
                oInfo.mes_10 = oSqlDataReader.Item("mes_10")
                oInfo.mes_11 = oSqlDataReader.Item("mes_11")
                oInfo.mes_12 = oSqlDataReader.Item("mes_12")

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

#Region "::: HISTÓRICO :::"

    Public Function HistoricoPMOC(ByVal iCodigoEmpresa As Integer,
                                  ByVal iCodigoUnidade As Integer,
                                  ByVal lCodigoEquipamento As Long,
                                  ByVal sDataInicio As String,
                                  ByVal sDataTermino As String) As List(Of PMOCHistorico)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of PMOCHistorico)
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
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pmoc_historico", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New PMOCHistorico

                oInfo.unidade = oSqlDataReader.Item("unidade")
                oInfo.data = oSqlDataReader.Item("data")
                oInfo.equipamento = oSqlDataReader.Item("equipamento")
                oInfo.status = oSqlDataReader.Item("status")
                oInfo.tipo_ar_condicionado = oSqlDataReader.Item("tipo_ar_condicionado")
                oInfo.codigo = oSqlDataReader.Item("codigo")
                oInfo.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oInfo.codigo_empresa = oSqlDataReader.Item("codigo_empresa")

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

#Region "::: PMOC - IMPLANTAÇÃO :::"

    Public Sub InsertPMOC(ByVal iCodigoEmpresa As Integer,
                          ByVal iCodigoUsuario As Integer,
                          ByVal iCodigoUnidade As Integer,
                          ByVal iCodigoResponsavelLegal As Integer,
                          ByVal iCodigoTipoServico As Integer,
                          ByVal iCodigoEmpresaPMOC As Integer,
                          ByVal iCodigoUnidadePMOC As Integer,
                          ByVal sResponsavelTecnicoPMOC As String,
                          ByVal sNumeroARTPMOC As String,
                          ByVal sDataInicioVigenciaARTPMOC As String,
                          ByVal sDataTerminoVigenciaPMOC As String)

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

            'Seta Parametros - Código Responsável Legal
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_responsavel_legal"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoResponsavelLegal = -1, DBNull.Value, iCodigoResponsavelLegal) : i += 1

            'Seta Parametros - Código Tipo de Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_tipo_servico"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoTipoServico : i += 1

            'Seta Parametros - Código Empresa PMOC
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa_pmoc"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = IIf(iCodigoEmpresaPMOC = -1, DBNull.Value, iCodigoEmpresaPMOC) : i += 1

            'Seta Parametros - Código Unidade PMOC
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade_pmoc"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoUnidadePMOC = -1, DBNull.Value, iCodigoUnidadePMOC) : i += 1

            'Seta Parametros - Responsável Tecnico PMOC
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "responsavel_tecnico_pmoc"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Value = sResponsavelTecnicoPMOC.ToUpper : i += 1

            'Seta Parametros - Número ART PMOC
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "numero_art_pmoc"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sNumeroARTPMOC.ToUpper : i += 1

            'Seta Parametros - DAta Início Vigencia ART
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_inicio_vigencia_art_pmoc"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataInicioVigenciaARTPMOC : i += 1

            'Seta Parametros - DAta Término Vigencia ART
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_termino_vigencia_art_pmoc"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataTerminoVigenciaPMOC

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_pmoc", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdatePMOC(ByVal iCodigoEmpresa As Integer,
                          ByVal iCodigoUsuario As Integer,
                          ByVal iCodigoUnidade As Integer,
                          ByVal iCodigoResponsavelLegal As Integer,
                          ByVal iCodigoTipoServico As Integer,
                          ByVal iCodigoEmpresaPMOC As Integer,
                          ByVal iCodigoUnidadePMOC As Integer,
                          ByVal sResponsavelTecnicoPMOC As String,
                          ByVal sNumeroARTPMOC As String,
                          ByVal sDataInicioVigenciaARTPMOC As String,
                          ByVal sDataTerminoVigenciaPMOC As String,
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

            'Seta Parametros - Código Responsável Legal
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_responsavel_legal"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoResponsavelLegal = -1, DBNull.Value, iCodigoResponsavelLegal) : i += 1

            'Seta Parametros - Código Tipo de Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_tipo_servico"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoTipoServico : i += 1

            'Seta Parametros - Código Empresa PMOC
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa_pmoc"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = IIf(iCodigoEmpresaPMOC = -1, DBNull.Value, iCodigoEmpresaPMOC) : i += 1

            'Seta Parametros - Código Unidade PMOC
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade_pmoc"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoUnidadePMOC = -1, DBNull.Value, iCodigoUnidadePMOC) : i += 1

            'Seta Parametros - Responsável Tecnico PMOC
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "responsavel_tecnico_pmoc"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Value = sResponsavelTecnicoPMOC.ToUpper : i += 1

            'Seta Parametros - Número ART PMOC
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "numero_art_pmoc"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sNumeroARTPMOC.ToUpper : i += 1

            'Seta Parametros - DAta Início Vigencia ART
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_inicio_vigencia_art_pmoc"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataInicioVigenciaARTPMOC : i += 1

            'Seta Parametros - DAta Término Vigencia ART
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_termino_vigencia_art_pmoc"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataTerminoVigenciaPMOC : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_pmoc", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub DeletePMOC(ByVal iCodigoEmpresa As Integer,
                          ByVal iCodigoUsuario As Integer,
                          ByVal iCodigo As Integer,
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
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_pmoc", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InfoPMOC(ByVal iCodigoEmpresa As Integer,
                        ByVal iCodigo As Integer,
                        ByVal iCodigoUnidade As Integer,
                        ByRef oPMOC As PMOCCadastro)

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
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pmoc_dados", oSqlParameter)

            While oSqlDataReader.Read

                oPMOC = New PMOCCadastro

                oPMOC.codigo = oSqlDataReader.Item("codigo")
                oPMOC.codigo_empresa = oSqlDataReader.Item("codigo_empresa")
                oPMOC.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oPMOC.unidade = oSqlDataReader.Item("unidade")
                oPMOC.codigo_empresa_pmoc = oSqlDataReader.Item("codigo_empresa_pmoc")
                oPMOC.codigo_unidade_pmoc = oSqlDataReader.Item("codigo_unidade_pmoc")
                oPMOC.codigo_responsavel_legal = oSqlDataReader.Item("codigo_responsavel_legal")
                oPMOC.responsavel_tecnico_pmoc = oSqlDataReader.Item("responsavel_tecnico_pmoc")
                oPMOC.numero_art_pmoc = oSqlDataReader.Item("numero_art_pmoc")
                oPMOC.data_inicio_vigencia_art_pmoc = oSqlDataReader.Item("data_inicio_vigencia_art_pmoc")
                oPMOC.data_termino_vigencia_art_pmoc = oSqlDataReader.Item("data_termino_vigencia_art_pmoc")

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function IndexPMOC(ByVal iCodigoEmpresa As Integer,
                              ByVal iCodigoUsuario As Integer,
                              ByVal iCodigoUnidade As Integer) As List(Of PMOCCadastro)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of PMOCCadastro)
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
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pmoc", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New PMOCCadastro

                oInfo.codigo = oSqlDataReader.Item("codigo")
                oInfo.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oInfo.unidade = oSqlDataReader.Item("unidade")
                oInfo.numero_art_pmoc = oSqlDataReader.Item("numero_art_pmoc")
                oInfo.data_inicio_vigencia_art_pmoc = oSqlDataReader.Item("data_inicio_vigencia_art_pmoc")
                oInfo.data_termino_vigencia_art_pmoc = oSqlDataReader.Item("data_termino_vigencia_art_pmoc")

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
