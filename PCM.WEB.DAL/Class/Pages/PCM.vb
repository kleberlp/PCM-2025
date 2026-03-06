Imports PCM.WEB.MODELS
Imports System.DirectoryServices
Imports System.DirectoryServices.ActiveDirectory
Imports PCM.WEB.DAL.SQLHelper
Imports System.Data.SqlClient
Imports System.Text
Imports System.IO

Public Class PCM

    Private sConnection As String

    Sub New(ByVal sCon As String)
        sConnection = sCon
    End Sub

#Region "::: APONTAMENTO :::"

    Public Sub LoadApontamento(ByVal iCodigoEmpresa As Integer,
                               ByVal iCodigoUsuario As Integer,
                               ByVal iCodigoUnidade As Integer,
                               ByVal lCodigoPCMProgramada As Long,
                               ByRef oApontamento As Apontamento)

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

            'Seta Parametros - Código PCM Programada
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_pcm_programada"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoPCMProgramada

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pcm_apontamento", oSqlParameter)

            While oSqlDataReader.Read

                oApontamento = New Apontamento

                oApontamento.codigo_pcm_programada = oSqlDataReader.Item("codigo_pcm_programada")
                oApontamento.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oApontamento.unidade = oSqlDataReader.Item("unidade")
                oApontamento.setor = oSqlDataReader.Item("setor")
                oApontamento.equipamento = oSqlDataReader.Item("equipamento")
                oApontamento.servico = oSqlDataReader.Item("servico")
                oApontamento.codigo_categoria = oSqlDataReader.Item("codigo_categoria")
                oApontamento.categoria = oSqlDataReader.Item("categoria")
                oApontamento.codigo_tipo_ordem_servico = oSqlDataReader.Item("codigo_tipo_ordem_servico")
                oApontamento.tipo_ordem_servico = oSqlDataReader.Item("tipo_ordem_servico")
                oApontamento.codigo_tipo_servico = oSqlDataReader.Item("codigo_tipo_servico")
                oApontamento.tipo_servico = oSqlDataReader.Item("tipo_servico")
                oApontamento.codigo_funcionario = oSqlDataReader.Item("codigo_funcionario")
                oApontamento.aponta_horas = oSqlDataReader.Item("aponta_horas")
                oApontamento.exige_laudo = oSqlDataReader.Item("exige_laudo")
                oApontamento.codigo_pcm_ordem_servico = 0
                oApontamento.codigo_pcm_programada_ordem_servico = 0

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub LoadApontamentoProgramadaInfo(ByVal iCodigoEmpresa As Integer,
                                             ByVal iCodigoUnidade As Integer,
                                             ByVal lCodigoPCMApontamento As Long,
                                             ByRef oApontamento As Apontamento)

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

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoPCMApontamento : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pcm_apontamento_programada_dados", oSqlParameter)

            While oSqlDataReader.Read

                oApontamento = New Apontamento

                oApontamento.unidade = oSqlDataReader.Item("unidade")
                oApontamento.setor = oSqlDataReader.Item("setor")
                oApontamento.equipamento = oSqlDataReader.Item("equipamento")
                oApontamento.servico = oSqlDataReader.Item("servico")
                oApontamento.categoria = oSqlDataReader.Item("categoria")
                oApontamento.tipo_servico = oSqlDataReader.Item("tipo_servico")
                oApontamento.tipo_ordem_servico = oSqlDataReader.Item("tipo_ordem_servico")
                oApontamento.descricao_solucao = oSqlDataReader.Item("descricao_solucao")
                oApontamento.codigo_funcionario = oSqlDataReader.Item("codigo_funcionario")
                oApontamento.codigo_fornecedor = oSqlDataReader.Item("codigo_fornecedor")
                oApontamento.codigo_tipo_servico = oSqlDataReader.Item("codigo_tipo_servico")
                oApontamento.codigo_apontamento = oSqlDataReader.Item("codigo_apontamento")
                oApontamento.codigo_pcm_programada_ordem_servico = oSqlDataReader.Item("codigo_pcm_programada_ordem_servico")
                oApontamento.codigo_pcm_programada = oSqlDataReader.Item("codigo_pcm_programada")
                oApontamento.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oApontamento.aponta_horas = oSqlDataReader.Item("aponta_horas")
                oApontamento.data_inicio = oSqlDataReader.Item("data_inicio")
                oApontamento.data_termino = oSqlDataReader.Item("data_termino")
                oApontamento.hora_inicio = oSqlDataReader.Item("hora_inicio")
                oApontamento.hora_termino = oSqlDataReader.Item("hora_termino")

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InsertApontamento(ByVal iCodigoEmpresa As Integer,
                                 ByVal iCodigoUsuario As Integer,
                                 ByVal iCodigoUnidade As Integer,
                                 ByVal lCodigoPCMProgramadaOrdemServico As Long,
                                 ByVal lCodigoPCMProgramada As Long,
                                 ByVal iCodigoFornecedor As Integer,
                                 ByVal iCodigoFuncionario As Integer,
                                 ByVal sDataInicio As String,
                                 ByVal sDataTermino As String,
                                 ByVal sHoraInicio As String,
                                 ByVal sHoraTermino As String,
                                 ByVal dValor As Double,
                                 ByVal iQuantidadeEquipamento As Integer,
                                 ByVal sDescricaoSolucao As String,
                                 ByVal sArquivo As String,
                                 ByVal bConcluido As Boolean,
                                 ByRef lCodigoPCMOrdemServico As Long,
                                 ByRef lCodigoPCMApontamento As Long)

        Try

            'Váriaveis Locais
            Dim oSqlParameter(17) As SqlParameter
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

            'Seta Parametros - Código PCM Programada - Ordem de Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_pcm_programada_ordem_servico"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoPCMProgramadaOrdemServico : i += 1

            'Seta Parametros - Código PCM Programada
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_pcm_programada"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoPCMProgramada : i += 1

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
            oSqlParameter(i).Value = IIf(IsDate(sHoraTermino), sHoraTermino, DBNull.Value) : i += 1

            'Seta Parametros - Valor
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "valor"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = dValor : i += 1

            'Seta Parametros - Quantidade Equipamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "quantidade_equipamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iQuantidadeEquipamento : i += 1

            'Seta Parametros - Descrição Solução
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao_solucao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 5000
            oSqlParameter(i).Value = sDescricaoSolucao : i += 1

            'Seta Parametros - Concluído
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "concluido"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bConcluido : i += 1

            'Seta Parametros - Arquivo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "arquivo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 250
            oSqlParameter(i).Value = sArquivo : i += 1

            'Seta Parametros - Código PCM Programada Ordem Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_pcm_ordem_servico"
            oSqlParameter(i).Direction = ParameterDirection.Output
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt : i += 1

            'Seta Parametros - Código PCM Apontamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_pcm_apontamento"
            oSqlParameter(i).Direction = ParameterDirection.Output
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_pcm_apontamento", oSqlParameter)

            lCodigoPCMOrdemServico = oSqlParameter(i - 1).Value
            lCodigoPCMApontamento = oSqlParameter(i).Value

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateApontamento(ByVal iCodigoEmpresa As Integer,
                                 ByVal iCodigoUsuario As Integer,
                                 ByVal iCodigoUnidade As Integer,
                                 ByVal lCodigoApontamento As Long,
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

            'Seta Parametros - Código Apontamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_apontamento"
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
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_pcm_apontamento", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub DeleteApontamento(ByVal iCodigoEmpresa As Integer,
                                   ByVal iCodigoUsuario As Integer,
                                   ByVal iCodigoUnidade As Integer,
                                   ByVal lCodigo As Long)

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
            oSqlParameter(i).Value = lCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_pcm_apontamento", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function LoadApontamentoCheckListExcluir(ByVal iCodigoEmpresa As Integer,
                                                    ByVal iCodigoUnidade As Integer,
                                                    ByVal lCodigoPCMApontamento As Long) As List(Of ApontamentoChecklist)

        Try

            'Váriaveis Locais
            Dim oApontamentoChecklist As New List(Of ApontamentoChecklist)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(2) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código PCM Apontamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_pcm_apontamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoPCMApontamento : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pcm_apontamento_checklist_excluir", oSqlParameter)

            While oSqlDataReader.Read

                Dim oApontamentoChecklistInfo As New ApontamentoChecklist

                oApontamentoChecklistInfo.codigo = oSqlDataReader.Item("codigo")
                oApontamentoChecklistInfo.descricao = oSqlDataReader.Item("descricao")
                oApontamentoChecklistInfo.resultado = oSqlDataReader.Item("resultado")
                oApontamentoChecklistInfo.observacao = oSqlDataReader.Item("observacao")

                oApontamentoChecklist.Add(oApontamentoChecklistInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oApontamentoChecklist

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Sub InsertApontamentoChecklist(ByVal iCodigoEmpresa As Integer,
                                          ByVal lCodigoApontamento As Long,
                                          ByVal iCodigoUnidade As Integer,
                                          ByVal sDescricao As String,
                                          ByVal iCodigo As Integer,
                                          ByVal bConforme As Boolean,
                                          ByVal dValor As Double,
                                          ByVal sComentario As String)

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

            'Seta Parametros - Código Apontamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_pcm_apontamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoApontamento : i += 1

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

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_checklist"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo : i += 1

            'Seta Parametros - Conforme
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "conforme"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bConforme : i += 1

            'Seta Parametros - Valor
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "valor"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = dValor : i += 1

            'Seta Parametros - Comentário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "comentario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 200
            oSqlParameter(i).Value = sComentario

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_pcm_apontamento_checklist", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region

#Region "::: APONTAMENTO OS :::"

    Public Sub LoadApontamentoOS(ByVal iCodigoEmpresa As Integer,
                                 ByVal iCodigoUsuario As Integer,
                                 ByVal lCodigoPCMOrdemServico As Long,
                                 ByVal iCodigoUnidade As Integer,
                                 ByRef oApontamento As Apontamento)

        Try

            'Váriaveis Locais
            Dim oSqlDataReader As SqlDataReader
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

            'Seta Parametros - Código PCM Ordem Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_pcm_ordem_servico"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoPCMOrdemServico

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pcm_apontamento_os", oSqlParameter)

            While oSqlDataReader.Read

                oApontamento = New Apontamento

                oApontamento.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oApontamento.unidade = oSqlDataReader.Item("unidade")
                oApontamento.ordem_servico = oSqlDataReader.Item("ordem_servico")
                oApontamento.setor = oSqlDataReader.Item("setor")
                oApontamento.equipamento = oSqlDataReader.Item("equipamento")
                oApontamento.apartamento = oSqlDataReader.Item("apartamento")
                oApontamento.servico = oSqlDataReader.Item("servico")
                oApontamento.codigo_categoria = oSqlDataReader.Item("codigo_categoria")
                oApontamento.codigo_departamento = oSqlDataReader.Item("codigo_departamento")
                oApontamento.codigo_tipo_servico = oSqlDataReader.Item("codigo_tipo_servico")
                oApontamento.codigo_tipo_ordem_servico = oSqlDataReader.Item("codigo_tipo_ordem_servico")
                oApontamento.codigo_fornecedor = 0
                oApontamento.codigo_funcionario = oSqlDataReader.Item("codigo_funcionario")
                oApontamento.aponta_horas = oSqlDataReader.Item("aponta_horas")
                oApontamento.aponta_horas_qualidade = oSqlDataReader.Item("aponta_horas_qualidade")
                oApontamento.exige_laudo = oSqlDataReader.Item("exige_laudo")
                oApontamento.codigo = oSqlDataReader.Item("codigo_pcm_ordem_servico")
                oApontamento.ar_condicionado = oSqlDataReader.Item("ar_condicionado")

                If IsNothing(oApontamento.fotos) Then
                    oApontamento.fotos = New List(Of String)
                End If

                If IsDBNull(oSqlDataReader.Item("fotos_folder")) = False AndAlso Directory.Exists(oSqlDataReader.Item("fotos_folder").replace("~", "C:\SIM\PCM\PCM.WEB\") & "\") Then

                    For Each sFile As String In System.IO.Directory.GetFiles(oSqlDataReader.Item("fotos_folder").replace("~", "C:\SIM\PCM\PCM.WEB\") & "\") 'C:\SIM\PCM\SITE\APP\") & "\")

                        Dim oFileInfo As New FileInfo(sFile)

                        oApontamento.fotos.Add(oSqlDataReader.Item("fotos_folder") & oFileInfo.Name)

                    Next

                End If

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub LoadApontamentoOSInfo(ByVal iCodigoEmpresa As Integer,
                                     ByVal iCodigoUnidade As Integer,
                                     ByVal lCodigoPCMApontamento As Long,
                                     ByRef oApontamento As Apontamento)

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

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoPCMApontamento : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pcm_apontamento_excluir", oSqlParameter)

            While oSqlDataReader.Read

                oApontamento = New Apontamento

                oApontamento.unidade = oSqlDataReader.Item("unidade")
                oApontamento.setor = oSqlDataReader.Item("setor")
                oApontamento.equipamento = oSqlDataReader.Item("equipamento")
                oApontamento.servico = oSqlDataReader.Item("servico")
                oApontamento.descricao_solucao = oSqlDataReader.Item("descricao_solucao")
                oApontamento.codigo_categoria = oSqlDataReader.Item("codigo_categoria")
                oApontamento.codigo_tipo_servico = oSqlDataReader.Item("codigo_tipo_servico")
                oApontamento.codigo_tipo_ordem_servico = oSqlDataReader.Item("codigo_tipo_ordem_servico")
                oApontamento.codigo = oSqlDataReader.Item("codigo_pcm_ordem_servico")
                oApontamento.codigo_pcm_programada = oSqlDataReader.Item("codigo_pcm_programada")
                oApontamento.codigo_funcionario = oSqlDataReader.Item("codigo_funcionario")
                oApontamento.codigo_fornecedor = oSqlDataReader.Item("codigo_fornecedor")
                oApontamento.funcionario = oSqlDataReader.Item("funcionario")
                oApontamento.fornecedor = oSqlDataReader.Item("fornecedor")
                oApontamento.data_inicio = oSqlDataReader.Item("data_inicio")
                oApontamento.data_termino = oSqlDataReader.Item("data_termino")
                oApontamento.aponta_horas = oSqlDataReader.Item("aponta_horas")
                oApontamento.hora_inicio = oSqlDataReader.Item("hora_inicio")
                oApontamento.hora_termino = oSqlDataReader.Item("hora_termino")
                oApontamento.codigo_apontamento = oSqlDataReader.Item("codigo_apontamento")
                oApontamento.codigo_unidade = oSqlDataReader.Item("codigo_unidade")

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InsertApontamentoOS(ByVal iCodigoEmpresa As Integer,
                                   ByVal iCodigoUsuario AS Integer,
                                   ByVal lCodigoPCMOrdemServico As Long,
                                   ByVal iCodigoUnidade As Integer,
                                   ByVal iCodigoCategoria As Integer,
                                   ByVal iCodigoFornecedor As Integer,
                                   ByVal iCodigoFuncionario As Integer,
                                   ByVal sDataInicio As String,
                                   ByVal sDataTermino As String,
                                   ByVal sHoraInicio As String,
                                   ByVal sHoraTermino As String,
                                   ByVal iCodigoTipoServico As Integer,
                                   ByVal iCodigoTipoOrdemServico As Integer,
                                   ByVal dValor As Double,
                                   ByVal sDescricaoSolucao As String,
                                   ByVal sArquivo As String,
                                   ByVal bConcluido As Boolean,
                                   ByVal iCodigoJustificativaApontamento As Integer,
                                   ByVal sObservacaoApontamento As String,
                                   ByVal bDesativarApontamento As Boolean,
                                   ByRef lCodigo As Long)

        Try

            'Váriaveis Locais
            Dim oSqlParameter(20) As SqlParameter
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

            'Seta Parametros - Código PCM Ordem Servico
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_pcm_ordem_servico"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = IIf(lCodigoPCMOrdemServico = -1, DBNull.Value, lCodigoPCMOrdemServico) : i += 1

            'Seta Parametros - Código Categoria
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_categoria"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = If(iCodigoCategoria = -1, DBNull.Value, iCodigoCategoria) : i += 1

            'Seta Parametros - Código Tipo Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_tipo_servico"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoTipoServico = -1, DBNull.Value, iCodigoTipoServico) : i += 1

            'Seta Parametros - Código Tipo Ordem Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_tipo_ordem_servico"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoTipoOrdemServico = -1, DBNull.Value, iCodigoTipoOrdemServico) : i += 1

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
            oSqlParameter(i).Value = IIf(IsDate(sHoraTermino), sHoraTermino, DBNull.Value) : i += 1

            'Seta Parametros - Valor
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "valor"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = dValor : i += 1

            'Seta Parametros - Descrição Solução
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao_solucao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 5000
            oSqlParameter(i).Value = sDescricaoSolucao : i += 1

            'Seta Parametros - Concluído
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "concluido"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bConcluido : i += 1

            'Seta Parametros - Justificativa Apontamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_justificativa_apontamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoJustificativaApontamento = -1, DBNull.Value, iCodigoJustificativaApontamento) : i += 1

            'Seta Parametros - Desativar Apontamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "desativar_equipamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bDesativarApontamento : i += 1

            'Seta Parametros - Observação
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "observacao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = sObservacaoApontamento : i += 1

            'Seta Parametros - Arquivo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "arquivo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 250
            oSqlParameter(i).Value = sArquivo : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Output
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_pcm_apontamento_os", oSqlParameter)

            lCodigo = oSqlParameter(i).Value

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateApontamentoOS(ByVal iCodigoEmpresa As Integer,
                                   ByVal iCodigoUsuario As Integer,
                                   ByVal iCodigoCategoria As Integer,
                                   ByVal iCodigoFornecedor As Integer,
                                   ByVal iCodigoFuncionario As Integer,
                                   ByVal sDataInicio As String,
                                   ByVal sDataTermino As String,
                                   ByVal sHoraInicio As String,
                                   ByVal sHoraTermino As String,
                                   ByVal iCodigoTipoServico As Integer,
                                   ByVal iCodigoTipoOrdemServico As Integer,
                                   ByVal dValor As Double,
                                   ByVal sDescricaoSolucao As String,
                                   ByVal lCodigoApontamento As Long,
                                   ByVal iCodigoUnidade As Integer)

        Try

            'Váriaveis Locais
            Dim oSqlParameter(15) As SqlParameter
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

            'Seta Parametros - Código Apontamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_apontamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoApontamento : i += 1

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

            'Seta Parametros - Código Tipo Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_tipo_servico"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoTipoServico = -1, DBNull.Value, iCodigoTipoServico) : i += 1

            'Seta Parametros - Código Tipo Ordem Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_tipo_ordem_servico"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoTipoOrdemServico = -1, DBNull.Value, iCodigoTipoOrdemServico) : i += 1

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
            oSqlParameter(i).Value = IIf(IsDate(sHoraTermino), sHoraTermino, DBNull.Value) : i += 1

            'Seta Parametros - Valor
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "valor"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = dValor : i += 1

            'Seta Parametros - Descrição Solução
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao_solucao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 5000
            oSqlParameter(i).Value = sDescricaoSolucao : i += 1

            'Seta Parametros - Arquivo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "arquivo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 250
            oSqlParameter(i).Value = DBNull.Value

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_pcm_apontamento_os", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub DeleteApontamentoOS(ByVal iCodigoEmpresa As Integer,
                                   ByVal iCodigoUsuario AS Integer,
                                   ByVal iCodigoUnidade As Integer,
                                   ByVal lCodigo As Long)

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
            oSqlParameter(i).Value = lCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_pcm_apontamento_os", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region

#Region "::: AGENDA - PROGRAMADA :::"

    Public Function LoadAgenda(ByVal iCodigoEmpresa As Integer,
                               ByVal iCodigoUsuario AS Integer,
                               ByVal iCodigoUnidade As Integer,
                               ByVal iCodigoSetor As Integer,
                               ByVal lCodigoEquipamento As Long,
                               ByVal iCodigoTipoOrdemServico As Integer,
                               ByVal sStatus As String,
                               ByVal sDataInicio As String,
                               ByVal sDataTermino As String) As List(Of AgendaOS)

        Try

            'Váriaveis Locais
            Dim oAgenda As New List(Of AgendaOS)
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

            'Seta Parametros - Código Setor
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_setor"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoSetor : i += 1

            'Seta Parametros - Código Equipamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_equipamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoEquipamento : i += 1

            'Seta Parametros - Código Tipo Ordem Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_tipo_ordem_servico"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoTipoOrdemServico : i += 1

            'Seta Parametros - Status
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "status"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Value = sStatus : i += 1

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
            oSqlParameter(i).Value = IIf(IsDate(sDataTermino), sDataTermino, DBNull.Value)

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pcm_agenda", oSqlParameter)

            While oSqlDataReader.Read

                Dim oPCMOrdemServico As New PCMOrdemServico
                Dim oAgendaInfo As New AgendaOS

                oPCMOrdemServico.codigo_pcm_ordem_servico = oSqlDataReader.Item("codigo_pcm_ordem_servico")
                oPCMOrdemServico.codigo_pcm_programada = oSqlDataReader.Item("codigo_pcm_programada")
                oPCMOrdemServico.item = oSqlDataReader.Item("item")
                oPCMOrdemServico.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oPCMOrdemServico.unidade = oSqlDataReader.Item("unidade")
                oPCMOrdemServico.setor = oSqlDataReader.Item("setor")
                oPCMOrdemServico.equipamento = oSqlDataReader.Item("equipamento")
                oPCMOrdemServico.apartamento = oSqlDataReader.Item("apartamento")
                oPCMOrdemServico.local = oSqlDataReader.Item("local")
                oPCMOrdemServico.prioridade = oSqlDataReader.Item("prioridade")
                oPCMOrdemServico.periodicidade = oSqlDataReader.Item("periodicidade")
                oPCMOrdemServico.intervalo = oSqlDataReader.Item("intervalo")
                oPCMOrdemServico.servico = oSqlDataReader.Item("servico")
                oPCMOrdemServico.status = oSqlDataReader.Item("status")
                oPCMOrdemServico.codigo_status = oSqlDataReader.Item("codigo_status")
                oPCMOrdemServico.codigo_tipo_ordem_servico = oSqlDataReader.Item("codigo_tipo_ordem_servico")
                oPCMOrdemServico.data = oSqlDataReader.Item("data")
                oPCMOrdemServico.solicitante = oSqlDataReader.Item("solicitante")
                oAgendaInfo.ordem_servico = oPCMOrdemServico
                oAgendaInfo.apontamento = LoadAgendaApontamento(lCodigoPCMProgramadaOrdemServico:=oSqlDataReader.Item("codigo_pcm_ordem_servico"),
                                                                iCodigoEmpresa:=iCodigoEmpresa,
                                                                iCodigoUnidade:=oSqlDataReader.Item("codigo_unidade"))

                oAgenda.Add(oAgendaInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oAgenda

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function LoadAgendaApontamento(ByVal lCodigoPCMProgramadaOrdemServico As Long,
                                          ByVal iCodigoEmpresa As Integer,
                                          ByVal iCodigoUnidade As Integer) As List(Of PCMOrdemServicoApontamento)

        Try

            'Váriaveis Locais
            Dim oPCMProgramadaApontamento As New List(Of PCMOrdemServicoApontamento)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(2) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código PCM Programada
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_pcm_programada_ordem_servico"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoPCMProgramadaOrdemServico : i += 1

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
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pcm_agenda_apontamento", oSqlParameter)

            While oSqlDataReader.Read

                Dim oPCMProgramadaApontamentoInfo As New PCMOrdemServicoApontamento

                oPCMProgramadaApontamentoInfo.codigo = oSqlDataReader.Item("codigo")
                oPCMProgramadaApontamentoInfo.data_inicio = oSqlDataReader.Item("data_inicio")
                oPCMProgramadaApontamentoInfo.data_termino = oSqlDataReader.Item("data_termino")
                oPCMProgramadaApontamentoInfo.executor = oSqlDataReader.Item("executor")
                oPCMProgramadaApontamentoInfo.descricao_solucao = oSqlDataReader.Item("descricao_solucao")

                oPCMProgramadaApontamento.Add(oPCMProgramadaApontamentoInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oPCMProgramadaApontamento

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Sub UpdateStatus(ByVal lCodigoPCMProgramadaOrdemServico As Long,
                            ByVal iCodigoEmpresa As Integer,
                            ByVal iCodigoUsuario As Integer,
                            ByVal iCodigoUnidade As Integer,
                            ByVal iStatus As Integer)

        Try

            'Váriaveis Locais
            Dim oSqlParameter(4) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código PCM Programada Ordem Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_pcm_programada_ordem_servico"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoPCMProgramadaOrdemServico : i += 1

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
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_pcm_programada_ordem_servico_status", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region

#Region "::: AGENDA - ROTINA :::"

    Public Function LoadAgendaRotina(ByVal iCodigoEmpresa As Integer,
                                     ByVal iCodigoUsuario AS Integer,
                                     ByVal iCodigoUnidade As Integer,
                                     ByVal iCodigoSetor As Integer,
                                     ByVal lCodigoEquipamento As Long,
                                     ByVal sStatus As String,
                                     ByVal sDataInicio As String,
                                     ByVal sDataTermino As String) As List(Of AgendaOS)

        Try

            'Váriaveis Locais
            Dim oAgenda As New List(Of AgendaOS)
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

            'Seta Parametros - Código Setor
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_setor"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoSetor : i += 1

            'Seta Parametros - Código Equipamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_equipamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoEquipamento : i += 1

            'Seta Parametros - Status
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "status"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Value = sStatus : i += 1

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
            oSqlParameter(i).Value = IIf(IsDate(sDataTermino), sDataTermino, DBNull.Value)

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pcm_agenda_rotina", oSqlParameter)

            While oSqlDataReader.Read

                Dim oPCMOrdemServico As New PCMOrdemServico
                Dim oAgendaInfo As New AgendaOS

                oPCMOrdemServico.codigo_pcm_ordem_servico = oSqlDataReader.Item("codigo_pcm_ordem_servico")
                oPCMOrdemServico.codigo_pcm_programada = oSqlDataReader.Item("codigo_pcm_programada")
                oPCMOrdemServico.item = oSqlDataReader.Item("item")
                oPCMOrdemServico.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oPCMOrdemServico.unidade = oSqlDataReader.Item("unidade")
                oPCMOrdemServico.setor = oSqlDataReader.Item("setor")
                oPCMOrdemServico.equipamento = oSqlDataReader.Item("equipamento")
                oPCMOrdemServico.apartamento = oSqlDataReader.Item("apartamento")
                oPCMOrdemServico.local = oSqlDataReader.Item("local")
                oPCMOrdemServico.prioridade = oSqlDataReader.Item("prioridade")
                oPCMOrdemServico.periodicidade = oSqlDataReader.Item("periodicidade")
                oPCMOrdemServico.intervalo = oSqlDataReader.Item("intervalo")
                oPCMOrdemServico.servico = oSqlDataReader.Item("servico")
                oPCMOrdemServico.status = oSqlDataReader.Item("status")
                oPCMOrdemServico.codigo_status = oSqlDataReader.Item("codigo_status")
                oPCMOrdemServico.codigo_tipo_ordem_servico = oSqlDataReader.Item("codigo_tipo_ordem_servico")
                oPCMOrdemServico.data = oSqlDataReader.Item("data")
                oPCMOrdemServico.solicitante = oSqlDataReader.Item("solicitante")
                oAgendaInfo.ordem_servico = oPCMOrdemServico
                oAgendaInfo.apontamento = LoadAgendaApontamento(lCodigoPCMProgramadaOrdemServico:=oSqlDataReader.Item("codigo_pcm_ordem_servico"),
                                                                iCodigoEmpresa:=iCodigoEmpresa,
                                                                iCodigoUnidade:=oSqlDataReader.Item("codigo_unidade"))

                oAgenda.Add(oAgendaInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oAgenda

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: AGENDA OS :::"

    Public Function LoadAgendaOS(ByVal iCodigoEmpresa As Integer,
                                 ByVal iCodigoUsuario AS Integer,
                                 ByVal iCodigoUnidade As Integer,
                                 ByVal sOrdemServico As String,
                                 ByVal iCodigoSetor As Integer,
                                 ByVal lCodigoEquipamento As Long,
                                 ByVal sStatus As String,
                                 ByVal iCodigoPrioridade As Integer,
                                 ByVal sDataInicio As String,
                                 ByVal sDataTermino As String) As List(Of AgendaOS)

        Try

            'Váriaveis Locais
            Dim oAgenda As New List(Of AgendaOS)
            Dim oSqlDataReader As SqlDataReader
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

            'Seta Parametros - Ordem Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ordem_servico"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = CLng(IIf(IsNumeric(sOrdemServico) = False, "-1", sOrdemServico)) : i += 1

            'Seta Parametros - Código Setor
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_setor"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoSetor : i += 1

            'Seta Parametros - Código Equipamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_equipamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoEquipamento : i += 1

            'Seta Parametros - Prioridade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_prioridade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoPrioridade : i += 1

            'Seta Parametros - Status
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "status"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Value = sStatus : i += 1

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
            oSqlParameter(i).Value = IIf(IsDate(sDataTermino), sDataTermino, DBNull.Value)

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pcm_agenda_os", oSqlParameter)

            While oSqlDataReader.Read

                Dim oPCMOrdemServico As New PCMOrdemServico
                Dim oAgendaOSInfo As New AgendaOS

                oPCMOrdemServico.codigo_pcm_ordem_servico = oSqlDataReader.Item("codigo_pcm_ordem_servico")
                oPCMOrdemServico.item = oSqlDataReader.Item("item")
                oPCMOrdemServico.ordem_servico = oSqlDataReader.Item("ordem_servico")
                oPCMOrdemServico.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oPCMOrdemServico.unidade = oSqlDataReader.Item("unidade")
                oPCMOrdemServico.setor = oSqlDataReader.Item("setor")
                oPCMOrdemServico.equipamento = oSqlDataReader.Item("equipamento")
                oPCMOrdemServico.apartamento = oSqlDataReader.Item("apartamento")
                oPCMOrdemServico.local = oSqlDataReader.Item("local")
                oPCMOrdemServico.prioridade = oSqlDataReader.Item("prioridade")
                oPCMOrdemServico.servico = oSqlDataReader.Item("servico")
                oPCMOrdemServico.status = oSqlDataReader.Item("status")
                oPCMOrdemServico.codigo_status = oSqlDataReader.Item("codigo_status")
                oPCMOrdemServico.codigo_tipo_ordem_servico = oSqlDataReader.Item("codigo_tipo_ordem_servico")
                oPCMOrdemServico.data = oSqlDataReader.Item("data")
                oPCMOrdemServico.solicitante = oSqlDataReader.Item("solicitante")
                oAgendaOSInfo.ordem_servico = oPCMOrdemServico
                oAgendaOSInfo.apontamento = LoadAgendaOSApontamento(lCodigoPCMOrdemServico:=oSqlDataReader.Item("codigo_pcm_ordem_servico"),
                                                                    iCodigoEmpresa:=iCodigoEmpresa,
                                                                    iCodigoUnidade:=oSqlDataReader.Item("codigo_unidade"))

                oAgenda.Add(oAgendaOSInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oAgenda

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function LoadAgendaOSApontamento(ByVal lCodigoPCMOrdemServico As Long,
                                            ByVal iCodigoEmpresa As Integer,
                                            ByVal iCodigoUnidade As Integer) As List(Of PCMOrdemServicoApontamento)

        Try

            'Váriaveis Locais
            Dim oPCMOrdemServicoApontamento As New List(Of PCMOrdemServicoApontamento)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(2) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código PCM Ordem Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_pcm_ordem_servico"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoPCMOrdemServico : i += 1

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
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pcm_agenda_os_apontamento", oSqlParameter)

            While oSqlDataReader.Read

                Dim oPCMOrdemServicoApontamentoInfo As New PCMOrdemServicoApontamento

                oPCMOrdemServicoApontamentoInfo.codigo = oSqlDataReader.Item("codigo")
                oPCMOrdemServicoApontamentoInfo.data_inicio = oSqlDataReader.Item("data_inicio")
                oPCMOrdemServicoApontamentoInfo.data_termino = oSqlDataReader.Item("data_termino")
                oPCMOrdemServicoApontamentoInfo.executor = oSqlDataReader.Item("executor")
                oPCMOrdemServicoApontamentoInfo.descricao_solucao = oSqlDataReader.Item("descricao_solucao")

                oPCMOrdemServicoApontamento.Add(oPCMOrdemServicoApontamentoInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oPCMOrdemServicoApontamento

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: FALTA :::"

    Public Sub InsertFalta(ByVal iCodigoEmpresa As Integer,
                           ByVal iCodigoUnidade As Integer,
                           ByVal iCodigoFuncionario As Integer,
                           ByVal iCodigoJustificativaFalta As Integer,
                           ByVal sDataInicio As String,
                           ByVal sHoraInicio As String,
                           ByVal sDataTermino As String,
                           ByVal sHoraTermino As String,
                           ByVal sObservacao As String,
                           ByVal sPathArquivo As String,
                           ByVal sArquivo As String,
                           ByVal iCodigoUsuario As Integer)

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

            'Seta Parametros - Unidade
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

            'Seta Parametros - Código Justificativa Falta
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_justificativa_falta"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoJustificativaFalta : i += 1

            'Seta Parametros - Data Início
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_inicio"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataInicio : i += 1

            'Seta Parametros - Hora Início
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "hora_inicio"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Time
            oSqlParameter(i).Value = sHoraInicio : i += 1

            'Seta Parametros - Data Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_termino"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataTermino : i += 1

            'Seta Parametros - Hora Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "hora_termino"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Time
            oSqlParameter(i).Value = sHoraTermino : i += 1

            'Seta Parametros - Observação
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "observacao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = sObservacao : i += 1

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

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_pcm_falta", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub DeleteFalta(ByVal iCodigoEmpresa As Integer,
                           ByVal iCodigoUnidade As Integer,
                           ByVal lCodigo As Long,
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
            oSqlParameter(i).Value = iCodigoEmpresa  : i += 1

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
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigo : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_pcm_falta", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InfoFalta(ByVal iCodigoEmpresa As Integer,
                         ByVal iCodigoUnidade As Integer,
                         ByVal lCodigo As Long,
                         ByRef oPCMFalta As PCMFalta)

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
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pcm_falta_dados", oSqlParameter)

            While oSqlDataReader.Read

                oPCMFalta = New PCMFalta

                oPCMFalta.codigo = oSqlDataReader.Item("codigo")
                oPCMFalta.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oPCMFalta.unidade = oSqlDataReader.Item("unidade")
                oPCMFalta.codigo_funcionario = oSqlDataReader.Item("codigo_funcionario")
                oPCMFalta.funcionario = oSqlDataReader.Item("funcionario")
                oPCMFalta.codigo_justificativa_falta = oSqlDataReader.Item("codigo_justificativa_falta")
                oPCMFalta.justificativa_falta = oSqlDataReader.Item("justificativa_falta")
                oPCMFalta.data_inicio = oSqlDataReader.Item("data_inicio")
                oPCMFalta.hora_inicio = oSqlDataReader.Item("hora_inicio")
                oPCMFalta.data_termino = oSqlDataReader.Item("data_termino")
                oPCMFalta.hora_termino = oSqlDataReader.Item("hora_termino")
                oPCMFalta.observacao = oSqlDataReader.Item("observacao")

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function IndexFalta(ByVal iCodigoEmpresa As Integer,
                               ByVal iCodigoUnidade As Integer,
                               ByVal sDataInicio As String,
                               ByVal sDataTermino As String,
                               ByVal iCodigoFuncionario As Long,
                               ByVal iCodigoJustificativaFalta As Integer) As List(Of PCMFalta)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of PCMFalta)
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

            'Seta Parametros - Data Início
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_inicio"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = IIf(sDataInicio = "", DBNull.Value, sDataInicio) : i += 1

            'Seta Parametros - Data Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_termino"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = IIf(sDataTermino = "", DBNull.Value, sDataTermino) : i += 1

            'Seta Parametros - Código Funcionário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_funcionario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoFuncionario : i += 1

            'Seta Parametros - Código Justificativa Falta
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_justificativa_falta"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoJustificativaFalta

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pcm_falta", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New PCMFalta

                oInfo.codigo = oSqlDataReader.Item("codigo")
                oInfo.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oInfo.unidade = oSqlDataReader.Item("unidade")
                oInfo.codigo_funcionario = oSqlDataReader.Item("codigo_funcionario")
                oInfo.funcionario = oSqlDataReader.Item("funcionario")
                oInfo.codigo_justificativa_falta = oSqlDataReader.Item("codigo_justificativa_falta")
                oInfo.justificativa_falta = oSqlDataReader.Item("justificativa_falta")
                oInfo.data_inicio = oSqlDataReader.Item("data_inicio")
                oInfo.hora_inicio = oSqlDataReader.Item("hora_inicio")
                oInfo.data_termino = oSqlDataReader.Item("data_termino")
                oInfo.hora_termino = oSqlDataReader.Item("hora_termino")
                oInfo.observacao = oSqlDataReader.Item("observacao")
                oInfo.path_arquivo = oSqlDataReader.Item("path_arquivo")

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

#Region "::: ROTINA :::"

    Public Function ManutencaoRotina(ByVal iCodigoEmpresa As Integer,
                                     ByVal iCodigoUnidade As Integer,
                                     ByVal iCodigoModulo As Integer,
                                     ByVal iMes As Integer,
                                     ByVal iAno As Integer) As List(Of ManutencaoRotina)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of ManutencaoRotina)
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

            'Seta Parametros - Mês
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "mes"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iMes : i += 1

            'Seta Parametros - Ano
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ano"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iAno

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pcm_programada_rotina", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New ManutencaoRotina

                oInfo.codigo_empresa = oSqlDataReader("codigo_empresa")
                oInfo.codigo_unidade = oSqlDataReader("codigo_unidade")
                oInfo.unidade = oSqlDataReader("unidade")
                oInfo.codigo_pcm_programada = oSqlDataReader("codigo_pcm_programada")
                oInfo.descricao = oSqlDataReader("descricao")
                oInfo.codigo_periodicidade = oSqlDataReader("codigo_periodicidade")
                oInfo.periodicidade = oSqlDataReader("periodicidade")
                oInfo.intervalo = oSqlDataReader("intervalo")
                oInfo.data_ultima_os = oSqlDataReader("data_ultima_os")
                oInfo.data_proxima_os = oSqlDataReader("data_proxima_os")
                oInfo.data_inicio = oSqlDataReader("data_inicio")
                oInfo.data_termino = oSqlDataReader("data_termino")
                oInfo.total = oSqlDataReader("total")
                oInfo.total_realizado = oSqlDataReader("total_realizado")
                oInfo.percentual = IIf(oSqlDataReader.Item("total_realizado") = 0, 0, Math.Round(oSqlDataReader.Item("total_realizado") / oSqlDataReader.Item("total") * 100.0, 2))
                oInfo.dia_1 = oSqlDataReader.Item("1")
                oInfo.dia_2 = oSqlDataReader.Item("2")
                oInfo.dia_3 = oSqlDataReader.Item("3")
                oInfo.dia_4 = oSqlDataReader.Item("4")
                oInfo.dia_5 = oSqlDataReader.Item("5")
                oInfo.dia_6 = oSqlDataReader.Item("6")
                oInfo.dia_7 = oSqlDataReader.Item("7")
                oInfo.dia_8 = oSqlDataReader.Item("8")
                oInfo.dia_9 = oSqlDataReader.Item("9")
                oInfo.dia_10 = oSqlDataReader.Item("10")
                oInfo.dia_11 = oSqlDataReader.Item("11")
                oInfo.dia_12 = oSqlDataReader.Item("12")
                oInfo.dia_13 = oSqlDataReader.Item("13")
                oInfo.dia_14 = oSqlDataReader.Item("14")
                oInfo.dia_15 = oSqlDataReader.Item("15")
                oInfo.dia_16 = oSqlDataReader.Item("16")
                oInfo.dia_17 = oSqlDataReader.Item("17")
                oInfo.dia_18 = oSqlDataReader.Item("18")
                oInfo.dia_19 = oSqlDataReader.Item("19")
                oInfo.dia_20 = oSqlDataReader.Item("20")
                oInfo.dia_21 = oSqlDataReader.Item("21")
                oInfo.dia_22 = oSqlDataReader.Item("22")
                oInfo.dia_23 = oSqlDataReader.Item("23")
                oInfo.dia_24 = oSqlDataReader.Item("24")
                oInfo.dia_25 = oSqlDataReader.Item("25")
                oInfo.dia_26 = oSqlDataReader.Item("26")
                oInfo.dia_27 = oSqlDataReader.Item("27")
                oInfo.dia_28 = oSqlDataReader.Item("28")
                oInfo.dia_29 = oSqlDataReader.Item("29")
                oInfo.dia_30 = oSqlDataReader.Item("30")
                oInfo.dia_31 = oSqlDataReader.Item("31")

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

    Public Function LoadManutencaoRotinaStatus(ByVal iCodigoEmpresa As Integer,
                                               ByVal iCodigoUnidade As Integer,
                                               ByVal iCodigoModulo As Integer) As PCMRotinaStatus

        Try

            'Váriaveis Locais
            Dim oReturn As New PCMRotinaStatus
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
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoModulo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pcm_manutencao_rotina_status", oSqlParameter)

            While oSqlDataReader.Read

                oReturn.atrasado = oSqlDataReader.Item("atrasado")
                oReturn.pendente = oSqlDataReader.Item("pendente")
                oReturn.concluido = oSqlDataReader.Item("concluido")
                oReturn.andamento = oSqlDataReader.Item("em_andamento")

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

    Public Function LoadManutencaoRotina(ByVal iCodigoEmpresa As Integer,
                                         ByVal iCodigoUnidade As Integer,
                                         ByVal iCodigoModulo As Integer,
                                         Optional ByVal iStatus As Integer = -1) As List(Of PCMRotina)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of PCMRotina)
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

            'Seta Parametros - Código Módulo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_modulo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoModulo : i += 1

            'Seta Parametros - Status
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "status"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iStatus

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pcm_manutencao_rotina", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New PCMRotina

                oInfo.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oInfo.unidade = oSqlDataReader.Item("nome_fantasia")
                oInfo.rotina = LoadManutencaoRotinaDia(iCodigoEmpresa:=iCodigoEmpresa,
                                                       iCodigoUnidade:=oSqlDataReader.Item("codigo_unidade"),
                                                       iCodigoModulo:=oSqlDataReader.Item("codigo_modulo"),
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

    Public Function LoadManutencaoRotinaDia(ByVal iCodigoEmpresa As Integer,
                                            ByVal iCodigoUnidade As Integer,
                                            ByVal iCodigoModulo As Integer,
                                            ByVal iStatus As Integer) As List(Of PCMRotinaDia)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of PCMRotinaDia)
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

            'Seta Parametros - Código Módulo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_modulo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoModulo : i += 1

            'Seta Parametros - Status
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "status"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iStatus

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pcm_manutencao_rotina_dia", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New PCMRotinaDia

                oInfo.codigo_pcm_programada = oSqlDataReader.Item("codigo_pcm_programada")
                oInfo.codigo_pcm_programada_ordem_servico = oSqlDataReader.Item("codigo_pcm_programada_ordem_servico")
                oInfo.descricao = oSqlDataReader.Item("descricao")
                oInfo.status = oSqlDataReader.Item("status")
                oInfo.descricao_status = oSqlDataReader.Item("descricao_status")
                oInfo.cor = oSqlDataReader.Item("cor")
                oInfo.data = oSqlDataReader.Item("data")
                oInfo.dataUltimaManutencao = oSqlDataReader.Item("data_ultima_manutencao")
                oInfo.intervalo = oSqlDataReader.Item("intervalo")
                oInfo.periodicidade = oSqlDataReader.Item("periodicidade")

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

#Region "::: PROGRAMADA :::"

    Public Sub UpdateStatusOS(ByVal lCodigoPCMOrdemServico As Long,
                              ByVal iCodigoEmpresa As Integer,
                              ByVal iCodigoUsuario As Integer,
                              ByVal iCodigoUnidade As Integer,
                              ByVal iStatus As Integer)

        Try

            'Váriaveis Locais
            Dim oSqlParameter(4) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código PCM Ordem Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_pcm_ordem_servico"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoPCMOrdemServico : i += 1

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
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_pcm_ordem_servico_status", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub DeleteOrdemServicoProgramada(ByVal iCodigoEmpresa As Integer,
                                            ByVal iCodigoUnidade As Integer,
                                            ByVal lCodigoPCMProgramadaOrdemServico As Long,
                                            ByVal iCodigoUsuario As Integer)

        Try

            'Váriaveis Locais
            Dim oSqlParameter(3) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código PCM Programada Ordem Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_pcm_programada_ordem_servico"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoPCMProgramadaOrdemServico : i += 1

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
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_pcm_programada_ordem_servico", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InsertOrdemServicoProgramada(ByVal iCodigoEmpresa As Integer,
                                            ByVal iCodigoUsuario As Integer,
                                            ByVal iCodigoUnidade As Integer,
                                            ByVal lCodigoPCMProgramada As Long,
                                            ByVal dValor As Double,
                                            ByVal iQuantidadeEquipamento As Integer,
                                            ByVal sDescricaoSolucao As String,
                                            ByVal bConcluido As Boolean,
                                            ByVal sData As String,
                                            ByVal sDataTermino As String,
                                            ByRef lCodigoPCMProgramadaOrdemServico As Long)

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

            'Seta Parametros - Código PCM Programada
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_pcm_programada"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoPCMProgramada : i += 1

            'Seta Parametros - Valor
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "valor"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = dValor : i += 1

            'Seta Parametros - Quantidade Equipamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "quantidade_equipamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iQuantidadeEquipamento : i += 1

            'Seta Parametros - Descrição Solução
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao_solucao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 5000
            oSqlParameter(i).Value = sDescricaoSolucao : i += 1

            'Seta Parametros - Arquivo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "arquivo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = DBNull.Value : i += 1

            'Seta Parametros - Path Arquivo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "path_arquivo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = DBNull.Value : i += 1

            'Seta Parametros - Concluído
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "concluido"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bConcluido : i += 1

            'Seta Parametros - Data
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sData : i += 1

            'Seta Parametros - Data Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_termino"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataTermino : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Output
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_pcm_programada_ordem_servico", oSqlParameter)

            lCodigoPCMProgramadaOrdemServico = oSqlParameter(i).Value

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateOrdemServicoProgramada(ByVal iCodigoEmpresa As Integer,
                                            ByVal iCodigoUsuario As Integer,
                                            ByVal iCodigoUnidade As Integer,
                                            ByVal lCodigoPCMProgramada As Long,
                                            ByVal dValor As Double,
                                            ByVal iQuantidadeEquipamento As Integer,
                                            ByVal sDescricaoSolucao As String,
                                            ByVal bConcluido As Boolean,
                                            ByVal lCodigoPCMProgramadaOrdemServico As Long)

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

            'Seta Parametros - Código PCM Programada
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_pcm_programada"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoPCMProgramada : i += 1

            'Seta Parametros - Valor
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "valor"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = dValor : i += 1

            'Seta Parametros - Quantidade Equipamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "quantidade_equipamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iQuantidadeEquipamento : i += 1

            'Seta Parametros - Descrição Solução
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao_solucao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 5000
            oSqlParameter(i).Value = sDescricaoSolucao : i += 1

            'Seta Parametros - Arquivo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "arquivo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = DBNull.Value : i += 1

            'Seta Parametros - Path Arquivo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "path_arquivo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = DBNull.Value : i += 1

            'Seta Parametros - Concluído
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "concluido"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bConcluido : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoPCMProgramadaOrdemServico

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_pcm_programada_ordem_servico", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InsertApontamentoProgramada(ByVal iCodigoEmpresa As Integer,
                                           ByVal iCodigoUsuario As Integer,
                                           ByVal iCodigoUnidade As Integer,
                                           ByVal lCodigoPCMProgramadaOrdemServico As Long,
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

            'Seta Parametros - Código PCM Programada - Ordem de Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_pcm_programada_ordem_servico"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoPCMProgramadaOrdemServico : i += 1

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
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_pcm_apontamento", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InsertChecklistProgramada(ByVal iCodigoEmpresa As Integer,
                                         ByVal iCodigoUsuario As Integer,
                                         ByVal iCodigoUnidade As Integer,
                                         ByVal lCodigoPCMProgramadaOrdemServico As Long,
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

            'Seta Parametros - Código PCM Programada - Ordem de Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_pcm_programada_ordem_servico"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoPCMProgramadaOrdemServico : i += 1

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
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_pcm_programada_ordem_servico_checklist", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function LoadPCMProgramadaCheckList(ByVal iCodigoEmpresa As Integer,
                                               ByVal lCodigoPCMProgramada As Long,
                                               ByVal iCodigoUnidade As Integer) As List(Of PCMProgramadaOrdemServicoChecklist)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of PCMProgramadaOrdemServicoChecklist)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(2) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código PCM Programada
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_pcm_programada"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoPCMProgramada : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pcm_apontamento_checklist", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New PCMProgramadaOrdemServicoChecklist

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

    Public Function LoadPCMProgramadaHistorico(ByVal iCodigoEmpresa As Integer,
                                               ByVal iCodigoUnidade As Integer,
                                               ByVal iCodigoUsuario As Integer,
                                               ByVal iCodigoModulo As Integer,
                                               ByVal lCodigoProgramada As Long,
                                               ByVal sDataInicio As String,
                                               ByVal sDataTermino As String,
                                               ByVal sFormulario As String) As List(Of PCMProgramadaHistorico)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of PCMProgramadaHistorico)
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

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código Módulo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_modulo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoModulo : i += 1

            'Seta Parametros - Código Programada
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_programada"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoProgramada : i += 1

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

            'Seta Parametros - Formulário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "formulario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sFormulario

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pcm_programada_historico", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New PCMProgramadaHistorico

                oInfo.unidade = oSqlDataReader.Item("unidade")
                oInfo.data = oSqlDataReader.Item("data")
                oInfo.descricao = oSqlDataReader.Item("descricao")
                oInfo.tipo_servico = oSqlDataReader.Item("tipo_servico")
                oInfo.tipo_ordem_servico = oSqlDataReader.Item("tipo_ordem_servico")
                oInfo.quantidade_equipamento = oSqlDataReader.Item("quantidade_equipamento")
                oInfo.valor = oSqlDataReader.Item("valor")
                oInfo.codigo = oSqlDataReader.Item("codigo")
                oInfo.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oInfo.formulario = oSqlDataReader.Item("formulario")

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

#Region "::: CRONOGRAMA PREVENTIVA :::"

    Public Function CronogramaManutencaoPreventiva(ByVal iCodigoEmpresa As Integer,
                                                   ByVal iCodigoUnidade As Integer,
                                                   ByVal iCodigoModulo As Integer,
                                                   ByVal iCodigoUsuario As Integer,
                                                   ByVal iCodigoSetor As Integer,
                                                   ByVal sData As String,
                                                   ByVal iCodigoTipoOrdemServico As Integer,
                                                   ByVal bRotina As Boolean,
                                                   ByVal iStatus As Integer) As List(Of CronogramaManutencaoPreventiva)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of CronogramaManutencaoPreventiva)
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

            'Seta Parametros - Código Módulo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_modulo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoModulo : i += 1

            'Seta Parametros - Código Setor
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_setor"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoSetor : i += 1

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
            oSqlParameter(i).Value = bRotina : i += 1

            'Seta Parametros - Status
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "status"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iStatus : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pcm_cronograma_manutencao_preventiva", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New CronogramaManutencaoPreventiva

                oInfo.nome_fantasia = oSqlDataReader("nome_fantasia")
                oInfo.manutencao = oSqlDataReader("manutencao")
                oInfo.data_ultima_manutencao = oSqlDataReader("data_ultima_manutencao")
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
                oInfo.codigo_pcm_programada = oSqlDataReader.Item("codigo_pcm_programada")
                oInfo.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oInfo.codigo_pcm_programada_ordem_servico = oSqlDataReader.Item("codigo_pcm_programada_ordem_servico")

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

#Region "::: MANUTENÇÃO PREVENTIVA :::"

    Public Function ManutencaoPreventiva(ByVal iCodigoEmpresa As Integer,
                                         ByVal iCodigoUnidade As Integer,
                                         ByVal iCodigoModulo As Integer,
                                         ByVal iCodigoUsuario As Integer,
                                         ByVal iStatus As Integer,
                                         ByRef oManutencaoPreventivaStatus As ManutencaoPreventivaStatus) As List(Of ManutencaoPreventiva)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of ManutencaoPreventiva)
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

            'Seta Parametros - Status
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "status"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iStatus : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pcm_manutencao_preventiva", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New ManutencaoPreventiva

                oInfo.codigoUnidade = oSqlDataReader("codigo_unidade")
                oInfo.codigoPCMProgramada = oSqlDataReader("codigo_pcm_programada")
                oInfo.codigoPCMOrdemServicoProgramada = oSqlDataReader("codigo_pcm_ordem_servico_programada")
                oInfo.unidade = oSqlDataReader("unidade")
                oInfo.preventiva = oSqlDataReader("preventiva")
                oInfo.equipamento = oSqlDataReader("equipamento")
                oInfo.dataUltimaManutencao = oSqlDataReader("data_ultima_manutencao")
                oInfo.dataProximaManutencao = oSqlDataReader("data_proxima_manutencao")
                oInfo.periodicidade = oSqlDataReader("periodicidade")
                oInfo.intervalo = oSqlDataReader("intervalo")
                oInfo.status = oSqlDataReader("status")
                oInfo.statusID = oSqlDataReader("status_id")
                oInfo.cssClass = oSqlDataReader("css_class")

                Select Case oSqlDataReader.Item("status_id")
                    Case 1 : oManutencaoPreventivaStatus.pendente += 1
                    Case 2 : oManutencaoPreventivaStatus.concluido += 1
                    Case 3 : oManutencaoPreventivaStatus.atrasado += 1
                    Case 4 : oManutencaoPreventivaStatus.emAndamento += 1
                End Select

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

    Public Function OrdemServicoPreventiva(ByVal iCodigoEmpresa As Integer,
                                           ByVal iCodigoUnidade As Integer,
                                           ByVal lCodigoPCMProgramadaOrdemServico As Long) As PCMProgramadaOrdemServico

        Try

            'Váriaveis Locais
            Dim oReturn As New PCMProgramadaOrdemServico
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
            oSqlParameter(i).Value = lCodigoPCMProgramadaOrdemServico : i += 1

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pcm_manutencao_preventiva_ordem_servico", oSqlParameter)

            While oSqlDataReader.Read

                oReturn.codigo = oSqlDataReader.Item("codigo")
                oReturn.codigo_pcm_programada = oSqlDataReader.Item("codigo_pcm_programada")
                oReturn.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oReturn.unidade = oSqlDataReader.Item("unidade")
                oReturn.codigo_setor = oSqlDataReader.Item("codigo_setor")
                oReturn.setor = oSqlDataReader.Item("setor")
                oReturn.codigo_equipamento = oSqlDataReader.Item("codigo_equipamento")
                oReturn.equipamento = oSqlDataReader.Item("equipamento")
                oReturn.codigo_tipo_servico = oSqlDataReader.Item("codigo_tipo_servico")
                oReturn.categoria = oSqlDataReader.Item("categoria")
                oReturn.tipo_servico = oSqlDataReader.Item("tipo_servico")
                oReturn.tipo_ordem_servico = oSqlDataReader.Item("tipo_ordem_servico")
                oReturn.servico = oSqlDataReader.Item("servico")
                oReturn.data = oSqlDataReader.Item("data")
                oReturn.descricao_solucao = oSqlDataReader.Item("descricao_solucao")
                oReturn.status = oSqlDataReader.Item("status")
                oReturn.valor = oSqlDataReader.Item("valor")
                oReturn.valor_texto = oSqlDataReader.Item("valor")
                oReturn.quantidade_equipamento = oSqlDataReader.Item("quantidade_equipamento")
                oReturn.aponta_horas = oSqlDataReader.Item("aponta_horas")
                oReturn.apontamento = OrdemServicoPreventivaApontamento(iCodigoEmpresa:=iCodigoEmpresa,
                                                                        iCodigoUnidade:=iCodigoUnidade,
                                                                        lCodigoPCMProgramadaOrdemServico:=lCodigoPCMProgramadaOrdemServico)
                oReturn.checklist = OrdemServicoPreventivaChecklist(iCodigoEmpresa:=iCodigoEmpresa,
                                                                    iCodigoUnidade:=iCodigoUnidade,
                                                                    lCodigoPCMProgramadaOrdemServico:=lCodigoPCMProgramadaOrdemServico)

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

    Public Function OrdemServicoPreventivaApontamento(ByVal iCodigoEmpresa As Integer,
                                                      ByVal iCodigoUnidade As Integer,
                                                      ByVal lCodigoPCMProgramadaOrdemServico As Long) As List(Of PCMProgramadaOrdemServicoApontamento)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of PCMProgramadaOrdemServicoApontamento)
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

            'Seta Parametros - Código PCM Programada Ordem Servico
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_pcm_programada_ordem_servico"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = lCodigoPCMProgramadaOrdemServico

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pcm_manutencao_preventiva_ordem_servico_apontamento", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New PCMProgramadaOrdemServicoApontamento

                oInfo.codigo_pcm_programada_ordem_servico = oSqlDataReader.Item("codigo_pcm_programada_ordem_servico")
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

    Public Function OrdemServicoPreventivaChecklist(ByVal iCodigoEmpresa As Integer,
                                                    ByVal iCodigoUnidade As Integer,
                                                    ByVal lCodigoPCMProgramadaOrdemServico As Long) As List(Of PCMProgramadaOrdemServicoChecklist)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of PCMProgramadaOrdemServicoChecklist)
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

            'Seta Parametros - Código PCM Programada Ordem Servico
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_pcm_programada_ordem_servico"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = lCodigoPCMProgramadaOrdemServico

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pcm_manutencao_preventiva_ordem_servico_checklist", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New PCMProgramadaOrdemServicoChecklist

                oInfo.codigo_pcm_programada_ordem_servico = oSqlDataReader.Item("codigo_pcm_programada_ordem_servico")
                oInfo.codigo = oSqlDataReader.Item("codigo")
                oInfo.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oInfo.codigo_tipo_item_checklist = oSqlDataReader.Item("codigo_tipo_item_checklist")
                oInfo.tipo_checklist = oSqlDataReader.Item("tipo_item_checklist")
                oInfo.grupo = oSqlDataReader.Item("grupo")
                oInfo.subgrupo = oSqlDataReader.Item("subgrupo")
                oInfo.checklist = oSqlDataReader.Item("checklist")
                oInfo.descricao = oSqlDataReader.Item("descricao")
                oInfo.valor_minimo = oSqlDataReader.Item("valor_minimo")
                oInfo.valor_maximo = oSqlDataReader.Item("valor_maximo")
                oInfo.resultado = oSqlDataReader.Item("resultado")
                oInfo.unidade_medida = oSqlDataReader.Item("unidade_medida")
                oInfo.observacao = oSqlDataReader.Item("observacao")
                oInfo.allow_picture = oSqlDataReader.Item("allow_picture")
                oInfo.possui_foto = oSqlDataReader.Item("possui_foto")

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

#Region "::: MANUTENÇÃO PROGRAMADA :::"

    Public Function ManutencaoProgramadaCalendario(ByVal iCodigoEmpresa As Integer,
                                                   ByVal iCodigoUnidade As Integer,
                                                   ByVal sData As String,
                                                   ByVal iCodigoTipoOrdemServico As Integer,
                                                   ByVal bRotina As Boolean) As List(Of ManutencaoProgramadaAgenda)

        Try

            'Váriaveis Locais
            Dim oManutencaoProgramada As New List(Of ManutencaoProgramadaAgenda)
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
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pcm_manutencao_programada_calendario", oSqlParameter)

            While oSqlDataReader.Read

                Dim oManutencaoProgramadaInfo As New ManutencaoProgramadaAgenda

                oManutencaoProgramadaInfo.nome = oSqlDataReader("manutencao")
                oManutencaoProgramadaInfo.data = oSqlDataReader.Item("data")
                oManutencaoProgramadaInfo.color = oSqlDataReader.Item("cor")
                oManutencaoProgramadaInfo.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oManutencaoProgramadaInfo.codigo_pcm_programada = oSqlDataReader.Item("codigo_pcm_programada")

                oManutencaoProgramada.Add(oManutencaoProgramadaInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oManutencaoProgramada

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
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pcm_manutencao_laudo", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New ManutencaoLaudo

                oInfo.nome_fantasia = oSqlDataReader("nome_fantasia")
                oInfo.manutencao = oSqlDataReader("manutencao")
                oInfo.equipamento = oSqlDataReader("equipamento")
                oInfo.data_validade = oSqlDataReader("data_validade")
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
                oInfo.codigo_pcm_programada = oSqlDataReader.Item("codigo_pcm_programada")
                oInfo.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oInfo.codigo_pcm_programada_ordem_servico = oSqlDataReader.Item("codigo_pcm_programada_ordem_servico")

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

    Public Function ManutencaoProgramada(ByVal iCodigoEmpresa As Integer,
                                         ByVal iCodigoUnidade As Integer,
                                         ByVal sDataInicio As String,
                                         ByVal sDataTermino As String,
                                         ByVal iCodigoTipoOrdemServico As Integer,
                                         ByVal bRotina As Boolean) As List(Of PCMManutencaoProgramada)

        Try

            'Váriaveis Locais
            Dim oListagem As New List(Of PCMManutencaoProgramada)
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
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pcm_manutencao_programada", oSqlParameter)

            While oSqlDataReader.Read

                Dim oListagemInfo As New PCMManutencaoProgramada

                oListagemInfo.manutencao = oSqlDataReader("manutencao")
                oListagemInfo.codigo_pcm_programada = oSqlDataReader("codigo_pcm_programada")
                oListagemInfo.codigo_unidade = oSqlDataReader("codigo_unidade")
                oListagemInfo.mes = ManutencaoProgramadaMes(iCodigoEmpresa:=iCodigoEmpresa,
                                                            iCodigoUnidade:=oSqlDataReader.Item("codigo_unidade"),
                                                            lCodigoPCMProgramada:=oSqlDataReader.Item("codigo_pcm_programada"),
                                                            sDataInicio:=sDataInicio,
                                                            sDataTermino:=sDataTermino)

                oListagem.Add(oListagemInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oListagem

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ManutencaoProgramadaMes(ByVal iCodigoEmpresa As Integer,
                                            ByVal iCodigoUnidade As Integer,
                                            ByVal lCodigoPCMProgramada As Long,
                                            ByVal sDataInicio As String,
                                            ByVal sDataTermino As String) As List(Of PCMManutencaoProgramadaMes)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of PCMManutencaoProgramadaMes)
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

            'Seta Parametros - Código PCM Programada
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_pcm_programada"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoPCMProgramada : i += 1

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
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pcm_manutencao_programada_mes", oSqlParameter)

            While oSqlDataReader.Read

                Dim oReturnInfo As New PCMManutencaoProgramadaMes

                oReturnInfo.mes = oSqlDataReader("mes")
                oReturnInfo.ano = oSqlDataReader("ano")
                oReturnInfo.manutencao = oSqlDataReader("manutencao")

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

#Region "::: MANUTENÇÃO APONTAMENTO :::"

    Public Function ManutencaoApontamento(ByVal iCodigoEmpresa As Integer,
                                          ByVal iCodigoUnidade As Integer,
                                          ByVal iCodigoFuncionario As Integer,
                                          ByVal lCodigoEquipamento As Long,
                                          ByVal sDataInicio As String,
                                          ByVal sDataTermino As String,
                                          ByVal iMes As Integer,
                                          ByVal iAno As Integer,
                                          ByRef sTotal As String) As List(Of ManutencaoApontamento)

        Try

            'Váriaveis Locais
            Dim oManutencaoApontamento As New List(Of ManutencaoApontamento)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(7) As SqlParameter
            Dim i As Integer = 0
            Dim lTotalMinutos As Long = 0


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
            oSqlParameter(i).Value = sDataTermino : i += 1

            'Seta Parametros - Mês
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "mes"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iMes : i += 1

            'Seta Parametros - Ano
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ano"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iAno

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pcm_manutencao_apontamento", oSqlParameter)

            While oSqlDataReader.Read

                Dim oManutencaoApontamentoInfo As New ManutencaoApontamento

                oManutencaoApontamentoInfo.unidade = oSqlDataReader("unidade")
                oManutencaoApontamentoInfo.ordem_servico = oSqlDataReader("ordem_servico")
                oManutencaoApontamentoInfo.executor = oSqlDataReader("executor")
                oManutencaoApontamentoInfo.local = oSqlDataReader.Item("local")
                oManutencaoApontamentoInfo.descricao = oSqlDataReader.Item("descricao")
                oManutencaoApontamentoInfo.data_inicio = oSqlDataReader.Item("data_inicio")
                oManutencaoApontamentoInfo.data_termino = oSqlDataReader.Item("data_termino")
                oManutencaoApontamentoInfo.horas = ConverterParaHorasFormatadas(oSqlDataReader.Item("minutos"))
                oManutencaoApontamentoInfo.erro = oSqlDataReader.Item("erro")
                oManutencaoApontamentoInfo.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oManutencaoApontamentoInfo.codigo = oSqlDataReader.Item("codigo")
                oManutencaoApontamentoInfo.custo = oSqlDataReader.Item("custo")
                lTotalMinutos = lTotalMinutos + oSqlDataReader.Item("minutos")

                oManutencaoApontamento.Add(oManutencaoApontamentoInfo)

            End While

            sTotal = CInt(System.Math.Floor(lTotalMinutos / 60)) & ":" & Format(lTotalMinutos Mod 60, "00")

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oManutencaoApontamento

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ManutencaoApontamento2(ByVal iCodigoEmpresa As Integer,
                                           ByVal iCodigoUnidade As Integer,
                                           ByVal iCodigoFuncionario As Integer,
                                           ByVal lCodigoEquipamento As Long,
                                           ByVal sDataInicio As String,
                                           ByVal sDataTermino As String,
                                           ByRef sTotal As String) As List(Of ManutencaoApontamento)

        Try

            'Váriaveis Locais
            Dim oManutencaoApontamento As New List(Of ManutencaoApontamento)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(5) As SqlParameter
            Dim i As Integer = 0
            Dim lTotalMinutos As Long = 0


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
            oSqlParameter(i).Value = IIf(sDataInicio = "", DBNull.Value, sDataInicio) : i += 1

            'Seta Parametros - Data Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_termino"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = IIf(sDataTermino = "", DBNull.Value, sDataTermino)

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pcm_manutencao_apontamento2", oSqlParameter)

            While oSqlDataReader.Read

                Dim oManutencaoApontamentoInfo As New ManutencaoApontamento

                oManutencaoApontamentoInfo.ordem_servico = oSqlDataReader("ordem_servico")
                oManutencaoApontamentoInfo.executor = oSqlDataReader("executor")
                oManutencaoApontamentoInfo.local = oSqlDataReader.Item("local")
                oManutencaoApontamentoInfo.descricao = oSqlDataReader.Item("descricao")
                oManutencaoApontamentoInfo.data_inicio = oSqlDataReader.Item("data_inicio")
                oManutencaoApontamentoInfo.data_termino = oSqlDataReader.Item("data_termino")
                oManutencaoApontamentoInfo.horas = oSqlDataReader.Item("horas")
                oManutencaoApontamentoInfo.erro = oSqlDataReader.Item("erro")
                oManutencaoApontamentoInfo.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oManutencaoApontamentoInfo.codigo = oSqlDataReader.Item("codigo")
                oManutencaoApontamentoInfo.custo = oSqlDataReader.Item("custo")
                lTotalMinutos = lTotalMinutos + oSqlDataReader.Item("minutos")

                oManutencaoApontamento.Add(oManutencaoApontamentoInfo)

            End While

            sTotal = CInt(System.Math.Floor(lTotalMinutos / 60)) & ":" & Format(lTotalMinutos Mod 60, "00")

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oManutencaoApontamento

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: REQUISIÇÃO :::"

    Public Sub InsertRequisicao(ByVal iCodigoEmpresa As Integer,
                                ByVal iCodigoUsuario As Integer,
                                ByVal iCodigoUnidade As Integer,
                                ByVal iCodigoSetor As Integer,
                                ByVal lCodigoEquipamento As Long,
                                ByVal iCodigoApartamento As Integer,
                                ByVal sData As String,
                                ByVal iCodigoPrioridade As Integer,
                                ByVal sDescricao As String,
                                ByVal sImagem As String,
                                ByVal sArquivo As String,
                                ByRef sNumeroRequisicao As String,
                                ByRef sTo As String,
                                ByRef sBody As String)
        Try

            'Váriaveis Locais
            Dim oSqlParameter(10) As SqlParameter
            Dim oSqlDataReader As SqlDataReader
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

            'Seta Parametros - Código Apartamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_apartamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoApartamento = -1, DBNull.Value, iCodigoApartamento) : i += 1

            'Seta Parametros - Data
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sData : i += 1

            'Seta Parametros - Código Prioridade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_prioridade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoPrioridade : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 5000
            oSqlParameter(i).Value = sDescricao.Trim.ToUpper : i += 1

            'Seta Parametros - Imagem
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "imagem"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 250
            oSqlParameter(i).Value = sImagem : i += 1

            'Seta Parametros - Arquivo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "arquivo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sArquivo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_insert_pcm_requisicao", oSqlParameter)

            While oSqlDataReader.Read

                'Seta Váriaveis
                sNumeroRequisicao = oSqlDataReader.Item("numero_requisicao")
                sTo = oSqlDataReader.Item("to")
                sBody = oSqlDataReader.Item("body")

            End While

            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function RequisicaoAprovarReprovarIndex(ByVal iCodigoEmpresa As Integer,
                                                   ByVal iCodigoUsuario As Integer,
                                                   Optional ByVal iCodigoUnidade As Integer = 0,
                                                   Optional ByVal sDataInicio As String = "",
                                                   Optional ByVal sDataTermino As String = "",
                                                   Optional ByVal sNumeroRequisicao As String = "",
                                                   Optional ByVal iCodigoSetor As Integer = -1,
                                                   Optional ByVal iCodigoPrioridade As Integer = -1,
                                                   Optional ByVal lCodigoEquipamento As Long = -1,
                                                   Optional ByVal iCodigoSolicitante As Integer = -1,
                                                   Optional ByVal iCodigoApartamento As Integer = -1,
                                                   Optional ByVal iStatus As Integer = -1) As List(Of MODELS.RequisicaoAprovarReprovar)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of MODELS.RequisicaoAprovarReprovar)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(11) As SqlParameter
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

            'Seta Parametros - Nº Requisição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "numero_requisicao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sNumeroRequisicao : i += 1

            'Seta Parametros - Data Início
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_inicio"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = IIf(sDataInicio = "", DBNull.Value, sDataInicio) : i += 1

            'Seta Parametros - Data Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_termino"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = IIf(sDataTermino = "", DBNull.Value, sDataTermino) : i += 1

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

            'Seta Parametros - Código Prioridade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_prioridade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoPrioridade : i += 1

            'Seta Parametros - Código Equipamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_equipamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoEquipamento : i += 1

            'Seta Parametros - Código Solicitante
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_solicitante"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoSolicitante : i += 1

            'Seta Parametros - Status
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "status"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iStatus

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pcm_requisicao", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New MODELS.RequisicaoAprovarReprovar

                oInfo.codigo = oSqlDataReader("codigo")
                oInfo.codigo_unidade = oSqlDataReader("codigo_unidade")
                oInfo.unidade = oSqlDataReader("unidade")
                oInfo.codigo_setor = oSqlDataReader("codigo_setor")
                oInfo.setor = oSqlDataReader("setor")
                oInfo.codigo_equipamento = oSqlDataReader("codigo_equipamento")
                oInfo.equipamento = oSqlDataReader("equipamento")
                oInfo.local = oSqlDataReader("local")
                oInfo.solicitante = oSqlDataReader("solicitante")
                oInfo.numero_requisicao = oSqlDataReader("numero_requisicao")
                oInfo.data = oSqlDataReader("data")
                oInfo.descricao = oSqlDataReader("descricao")
                oInfo.codigo_prioridade = oSqlDataReader("codigo_prioridade")
                oInfo.prioridade = oSqlDataReader("prioridade")
                oInfo.status = oSqlDataReader("status")
                oInfo.status_descricao = oSqlDataReader("status_descricao")
                oInfo.status_css = oSqlDataReader("status_css")

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

    Public Sub RequisicaoAprovarReprovar(ByVal iCodigoEmpresa As Integer,
                                         ByVal iCodigoUsuario As Integer,
                                         ByVal iCodigoUnidade As Integer,
                                         ByVal lCodigo As Long,
                                         ByVal iStatus As Integer,
                                         ByVal sJustificativa As String)

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
            oSqlParameter(i).Value = lCodigo : i += 1

            'Seta Parametros - Status
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "status"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iStatus : i += 1

            'Seta Parametros - Justificativa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "justificativa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = sJustificativa.Trim.ToUpper()

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_pcm_requisicao_aprovar_reprovar", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region

#Region "::: CRONOGRAMA :::"

    Public Function CronogramaSemana(ByVal iCodigoEmpresa As Integer,
                                     ByVal iCodigoUnidade As Integer,
                                     ByVal iTipo As Integer) As List(Of MODELS.PCMCronogramaSemana)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of MODELS.PCMCronogramaSemana)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(2) As SqlParameter
            Dim i As Integer = 0
            Dim dTotalSemana(52) As Double

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

            'Seta Parametros - Tipo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "tipo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iTipo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pcm_cronograma_semana", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New MODELS.PCMCronogramaSemana

                oInfo.descricao = oSqlDataReader("descricao")
                oInfo.tempo_estimado = oSqlDataReader("tempo_estimado") & "hr"
                oInfo.codigo_unidade = oSqlDataReader("codigo_unidade")
                oInfo.codigo_pcm_programada = oSqlDataReader("codigo_pcm_programada")
                oInfo.codigo_equipamento = oSqlDataReader("codigo_equipamento")
                oInfo.equipamento = oSqlDataReader("equipamento")
                oInfo.prioridade = oSqlDataReader("prioridade")
                oInfo.intervalo = oSqlDataReader("intervalo")
                oInfo.setor = oSqlDataReader("setor")
                oInfo.periodicidade = oSqlDataReader("periodicidade")
                oInfo.semana1 = oSqlDataReader("1")
                oInfo.semana2 = oSqlDataReader("2")
                oInfo.semana3 = oSqlDataReader("3")
                oInfo.semana4 = oSqlDataReader("4")
                oInfo.semana5 = oSqlDataReader("5")
                oInfo.semana6 = oSqlDataReader("6")
                oInfo.semana7 = oSqlDataReader("7")
                oInfo.semana8 = oSqlDataReader("8")
                oInfo.semana9 = oSqlDataReader("9")
                oInfo.semana10 = oSqlDataReader("10")
                oInfo.semana11 = oSqlDataReader("11")
                oInfo.semana12 = oSqlDataReader("12")
                oInfo.semana13 = oSqlDataReader("13")
                oInfo.semana14 = oSqlDataReader("14")
                oInfo.semana15 = oSqlDataReader("15")
                oInfo.semana16 = oSqlDataReader("16")
                oInfo.semana17 = oSqlDataReader("17")
                oInfo.semana18 = oSqlDataReader("18")
                oInfo.semana19 = oSqlDataReader("19")
                oInfo.semana20 = oSqlDataReader("20")
                oInfo.semana21 = oSqlDataReader("21")
                oInfo.semana22 = oSqlDataReader("22")
                oInfo.semana23 = oSqlDataReader("23")
                oInfo.semana24 = oSqlDataReader("24")
                oInfo.semana25 = oSqlDataReader("25")
                oInfo.semana26 = oSqlDataReader("26")
                oInfo.semana27 = oSqlDataReader("27")
                oInfo.semana28 = oSqlDataReader("28")
                oInfo.semana29 = oSqlDataReader("29")
                oInfo.semana30 = oSqlDataReader("30")
                oInfo.semana31 = oSqlDataReader("31")
                oInfo.semana32 = oSqlDataReader("32")
                oInfo.semana33 = oSqlDataReader("33")
                oInfo.semana34 = oSqlDataReader("34")
                oInfo.semana35 = oSqlDataReader("35")
                oInfo.semana36 = oSqlDataReader("36")
                oInfo.semana37 = oSqlDataReader("37")
                oInfo.semana38 = oSqlDataReader("38")
                oInfo.semana39 = oSqlDataReader("39")
                oInfo.semana40 = oSqlDataReader("40")
                oInfo.semana41 = oSqlDataReader("41")
                oInfo.semana42 = oSqlDataReader("42")
                oInfo.semana43 = oSqlDataReader("43")
                oInfo.semana44 = oSqlDataReader("44")
                oInfo.semana45 = oSqlDataReader("45")
                oInfo.semana46 = oSqlDataReader("46")
                oInfo.semana47 = oSqlDataReader("47")
                oInfo.semana48 = oSqlDataReader("48")
                oInfo.semana49 = oSqlDataReader("49")
                oInfo.semana50 = oSqlDataReader("50")
                oInfo.semana51 = oSqlDataReader("51")
                oInfo.semana52 = oSqlDataReader("52")
                dTotalSemana(1) += IIf(oSqlDataReader("1") <> "", oSqlDataReader("tempo_estimado"), 0)
                dTotalSemana(2) += IIf(oSqlDataReader("2") <> "", oSqlDataReader("tempo_estimado"), 0)
                dTotalSemana(3) += IIf(oSqlDataReader("3") <> "", oSqlDataReader("tempo_estimado"), 0)
                dTotalSemana(4) += IIf(oSqlDataReader("4") <> "", oSqlDataReader("tempo_estimado"), 0)
                dTotalSemana(5) += IIf(oSqlDataReader("5") <> "", oSqlDataReader("tempo_estimado"), 0)
                dTotalSemana(6) += IIf(oSqlDataReader("6") <> "", oSqlDataReader("tempo_estimado"), 0)
                dTotalSemana(7) += IIf(oSqlDataReader("7") <> "", oSqlDataReader("tempo_estimado"), 0)
                dTotalSemana(8) += IIf(oSqlDataReader("8") <> "", oSqlDataReader("tempo_estimado"), 0)
                dTotalSemana(9) += IIf(oSqlDataReader("9") <> "", oSqlDataReader("tempo_estimado"), 0)
                dTotalSemana(10) += IIf(oSqlDataReader("10") <> "", oSqlDataReader("tempo_estimado"), 0)
                dTotalSemana(11) += IIf(oSqlDataReader("11") <> "", oSqlDataReader("tempo_estimado"), 0)
                dTotalSemana(12) += IIf(oSqlDataReader("12") <> "", oSqlDataReader("tempo_estimado"), 0)
                dTotalSemana(13) += IIf(oSqlDataReader("13") <> "", oSqlDataReader("tempo_estimado"), 0)
                dTotalSemana(14) += IIf(oSqlDataReader("14") <> "", oSqlDataReader("tempo_estimado"), 0)
                dTotalSemana(15) += IIf(oSqlDataReader("15") <> "", oSqlDataReader("tempo_estimado"), 0)
                dTotalSemana(16) += IIf(oSqlDataReader("16") <> "", oSqlDataReader("tempo_estimado"), 0)
                dTotalSemana(17) += IIf(oSqlDataReader("17") <> "", oSqlDataReader("tempo_estimado"), 0)
                dTotalSemana(18) += IIf(oSqlDataReader("18") <> "", oSqlDataReader("tempo_estimado"), 0)
                dTotalSemana(19) += IIf(oSqlDataReader("19") <> "", oSqlDataReader("tempo_estimado"), 0)
                dTotalSemana(20) += IIf(oSqlDataReader("20") <> "", oSqlDataReader("tempo_estimado"), 0)
                dTotalSemana(21) += IIf(oSqlDataReader("21") <> "", oSqlDataReader("tempo_estimado"), 0)
                dTotalSemana(22) += IIf(oSqlDataReader("22") <> "", oSqlDataReader("tempo_estimado"), 0)
                dTotalSemana(23) += IIf(oSqlDataReader("23") <> "", oSqlDataReader("tempo_estimado"), 0)
                dTotalSemana(24) += IIf(oSqlDataReader("24") <> "", oSqlDataReader("tempo_estimado"), 0)
                dTotalSemana(25) += IIf(oSqlDataReader("25") <> "", oSqlDataReader("tempo_estimado"), 0)
                dTotalSemana(26) += IIf(oSqlDataReader("26") <> "", oSqlDataReader("tempo_estimado"), 0)
                dTotalSemana(27) += IIf(oSqlDataReader("27") <> "", oSqlDataReader("tempo_estimado"), 0)
                dTotalSemana(28) += IIf(oSqlDataReader("28") <> "", oSqlDataReader("tempo_estimado"), 0)
                dTotalSemana(29) += IIf(oSqlDataReader("29") <> "", oSqlDataReader("tempo_estimado"), 0)
                dTotalSemana(30) += IIf(oSqlDataReader("30") <> "", oSqlDataReader("tempo_estimado"), 0)
                dTotalSemana(31) += IIf(oSqlDataReader("31") <> "", oSqlDataReader("tempo_estimado"), 0)
                dTotalSemana(32) += IIf(oSqlDataReader("32") <> "", oSqlDataReader("tempo_estimado"), 0)
                dTotalSemana(33) += IIf(oSqlDataReader("33") <> "", oSqlDataReader("tempo_estimado"), 0)
                dTotalSemana(34) += IIf(oSqlDataReader("34") <> "", oSqlDataReader("tempo_estimado"), 0)
                dTotalSemana(35) += IIf(oSqlDataReader("35") <> "", oSqlDataReader("tempo_estimado"), 0)
                dTotalSemana(36) += IIf(oSqlDataReader("36") <> "", oSqlDataReader("tempo_estimado"), 0)
                dTotalSemana(37) += IIf(oSqlDataReader("37") <> "", oSqlDataReader("tempo_estimado"), 0)
                dTotalSemana(38) += IIf(oSqlDataReader("38") <> "", oSqlDataReader("tempo_estimado"), 0)
                dTotalSemana(39) += IIf(oSqlDataReader("39") <> "", oSqlDataReader("tempo_estimado"), 0)
                dTotalSemana(40) += IIf(oSqlDataReader("40") <> "", oSqlDataReader("tempo_estimado"), 0)
                dTotalSemana(41) += IIf(oSqlDataReader("41") <> "", oSqlDataReader("tempo_estimado"), 0)
                dTotalSemana(42) += IIf(oSqlDataReader("42") <> "", oSqlDataReader("tempo_estimado"), 0)
                dTotalSemana(43) += IIf(oSqlDataReader("43") <> "", oSqlDataReader("tempo_estimado"), 0)
                dTotalSemana(44) += IIf(oSqlDataReader("44") <> "", oSqlDataReader("tempo_estimado"), 0)
                dTotalSemana(45) += IIf(oSqlDataReader("45") <> "", oSqlDataReader("tempo_estimado"), 0)
                dTotalSemana(46) += IIf(oSqlDataReader("46") <> "", oSqlDataReader("tempo_estimado"), 0)
                dTotalSemana(47) += IIf(oSqlDataReader("47") <> "", oSqlDataReader("tempo_estimado"), 0)
                dTotalSemana(48) += IIf(oSqlDataReader("48") <> "", oSqlDataReader("tempo_estimado"), 0)
                dTotalSemana(49) += IIf(oSqlDataReader("49") <> "", oSqlDataReader("tempo_estimado"), 0)
                dTotalSemana(50) += IIf(oSqlDataReader("50") <> "", oSqlDataReader("tempo_estimado"), 0)
                dTotalSemana(51) += IIf(oSqlDataReader("51") <> "", oSqlDataReader("tempo_estimado"), 0)
                dTotalSemana(52) += IIf(oSqlDataReader("52") <> "", oSqlDataReader("tempo_estimado"), 0)

                oInfo.total_semana1 = dTotalSemana(1) & "hr"
                oInfo.total_semana2 = dTotalSemana(2) & "hr"
                oInfo.total_semana3 = dTotalSemana(3) & "hr"
                oInfo.total_semana4 = dTotalSemana(4) & "hr"
                oInfo.total_semana5 = dTotalSemana(5) & "hr"
                oInfo.total_semana6 = dTotalSemana(6) & "hr"
                oInfo.total_semana7 = dTotalSemana(7) & "hr"
                oInfo.total_semana8 = dTotalSemana(8) & "hr"
                oInfo.total_semana9 = dTotalSemana(9) & "hr"
                oInfo.total_semana10 = dTotalSemana(10) & "hr"
                oInfo.total_semana11 = dTotalSemana(11) & "hr"
                oInfo.total_semana12 = dTotalSemana(12) & "hr"
                oInfo.total_semana13 = dTotalSemana(13) & "hr"
                oInfo.total_semana14 = dTotalSemana(14) & "hr"
                oInfo.total_semana15 = dTotalSemana(15) & "hr"
                oInfo.total_semana16 = dTotalSemana(16) & "hr"
                oInfo.total_semana17 = dTotalSemana(17) & "hr"
                oInfo.total_semana18 = dTotalSemana(18) & "hr"
                oInfo.total_semana19 = dTotalSemana(19) & "hr"
                oInfo.total_semana20 = dTotalSemana(20) & "hr"
                oInfo.total_semana21 = dTotalSemana(21) & "hr"
                oInfo.total_semana22 = dTotalSemana(22) & "hr"
                oInfo.total_semana23 = dTotalSemana(23) & "hr"
                oInfo.total_semana24 = dTotalSemana(24) & "hr"
                oInfo.total_semana25 = dTotalSemana(25) & "hr"
                oInfo.total_semana26 = dTotalSemana(26) & "hr"
                oInfo.total_semana27 = dTotalSemana(27) & "hr"
                oInfo.total_semana28 = dTotalSemana(28) & "hr"
                oInfo.total_semana29 = dTotalSemana(29) & "hr"
                oInfo.total_semana30 = dTotalSemana(30) & "hr"
                oInfo.total_semana31 = dTotalSemana(31) & "hr"
                oInfo.total_semana32 = dTotalSemana(32) & "hr"
                oInfo.total_semana33 = dTotalSemana(33) & "hr"
                oInfo.total_semana34 = dTotalSemana(34) & "hr"
                oInfo.total_semana35 = dTotalSemana(35) & "hr"
                oInfo.total_semana36 = dTotalSemana(36) & "hr"
                oInfo.total_semana37 = dTotalSemana(37) & "hr"
                oInfo.total_semana38 = dTotalSemana(38) & "hr"
                oInfo.total_semana39 = dTotalSemana(39) & "hr"
                oInfo.total_semana40 = dTotalSemana(40) & "hr"
                oInfo.total_semana41 = dTotalSemana(41) & "hr"
                oInfo.total_semana42 = dTotalSemana(42) & "hr"
                oInfo.total_semana43 = dTotalSemana(43) & "hr"
                oInfo.total_semana44 = dTotalSemana(44) & "hr"
                oInfo.total_semana45 = dTotalSemana(45) & "hr"
                oInfo.total_semana46 = dTotalSemana(46) & "hr"
                oInfo.total_semana47 = dTotalSemana(47) & "hr"
                oInfo.total_semana48 = dTotalSemana(48) & "hr"
                oInfo.total_semana49 = dTotalSemana(49) & "hr"
                oInfo.total_semana50 = dTotalSemana(50) & "hr"
                oInfo.total_semana51 = dTotalSemana(51) & "hr"
                oInfo.total_semana52 = dTotalSemana(52) & "hr"
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

    Public Sub CronogramaSemanaData(ByVal iCodigoEmpresa As Integer,
                                    ByVal iCodigoUnidade As Integer,
                                    ByVal lCodigoPCMProgramada As Long,
                                    ByVal iSemana As Integer)

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

            'Seta Parametros - Código PCM Programada
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_pcm_programada"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoPCMProgramada : i += 1

            'Seta Parametros - Semana
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "semana"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iSemana

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_pcm_programada_start", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region

End Class
