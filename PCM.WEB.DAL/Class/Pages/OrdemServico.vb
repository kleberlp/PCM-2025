Imports PCM.WEB.MODELS
Imports System.DirectoryServices
Imports System.DirectoryServices.ActiveDirectory
Imports PCM.WEB.DAL.SQLHelper
Imports System.Data.SqlClient
Imports System.Text
Imports System.IO
Imports OfficeOpenXml
Imports System.ComponentModel
Imports OfficeOpenXml.Style

Public Class OrdemServico

    Private sConnection As String

    Sub New(ByVal sCon As String)
        sConnection = sCon
    End Sub

#Region "::: ORDEM DE SERVIÇO :::"

    Public Sub InsertOrdemServico(ByVal iCodigoEmpresa As Integer,
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
                                    ByRef lCodigo As Long,
                                    ByRef sOrdemServico As String,
                                    ByRef sTo As String,
                                    ByRef sToken As String,
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
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_insert_pcm_ordem_servico", oSqlParameter)

            While oSqlDataReader.Read

                'Seta Váriaveis
                lCodigo = oSqlDataReader.Item("codigo")
                sOrdemServico = oSqlDataReader.Item("ordem_servico")
                sTo = oSqlDataReader.Item("to")
                sToken = oSqlDataReader.Item("token")
                sBody = oSqlDataReader.Item("body")

            End While

            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateOrdemServico(ByVal iCodigoEmpresa As Integer,
                                  ByVal iCodigoUsuario As Integer,
                                  ByVal iCodigoUnidade As Integer,
                                  ByVal iCodigoSetor As Integer,
                                  ByVal lCodigoEquipamento As Long,
                                  ByVal iCodigoApartamento As Integer,
                                  ByVal sData As String,
                                  ByVal iCodigoJustificativaAlteracaoData As Integer,
                                  ByVal iCodigoPrioridade As Integer,
                                  ByVal sDescricao As String,
                                  ByVal sImagem As String,
                                  ByVal sArquivo As String,
                                  ByVal lCodigo As Long,
                                  ByVal iCodigoUnidadeOld As Integer)

        Try

            'Váriaveis Locais
            Dim oSqlParameter(13) As SqlParameter
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

            'Seta Parametros - Código Justificativa Alteração Data
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_justificativa_alteracao_data"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoJustificativaAlteracaoData = -1, DBNull.Value, iCodigoJustificativaAlteracaoData) : i += 1

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
            oSqlParameter(i).Value = sArquivo : i += 1

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
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_pcm_ordem_servico", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub DeleteOrdemServico(ByVal iCodigoEmpresa As Integer,
                                  ByVal iCodigoUsuario AS Integer,
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
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_pcm_ordem_servico", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub CancelartOrdemServico(ByVal iCodigoEmpresa As Integer,
                                     ByVal iCodigoUsuario As Integer,
                                     ByVal lCodigo As Long,
                                     ByVal iCodigoUnidade As Integer,
                                     ByVal iCodigoJustificativaCancelamento As Integer)

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
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Justificativa Cancelamento OS
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_justificativa_cancelamento_ordem_servico"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoJustificativaCancelamento

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_pcm_ordem_servico", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InfoOrdemServico(ByVal iCodigoEmpresa As Integer,
                                ByVal lCodigo As Long,
                                ByVal iCodigoUnidade As Integer,
                                ByVal sTipo As String,
                                ByRef oOrdemServico As MODELS.OrdemServico)

        'Variaveis Locais
        Dim oSqlParameter(2) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oPicture As New Picture(sConnection)
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
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pcm_ordem_servico_dados", oSqlParameter)

            While oSqlDataReader.Read

                oOrdemServico = New MODELS.OrdemServico
                oOrdemServico.codigo = oSqlDataReader("codigo")
                oOrdemServico.codigo_unidade = oSqlDataReader("codigo_unidade")
                oOrdemServico.unidade = oSqlDataReader("unidade")
                oOrdemServico.codigo_setor = oSqlDataReader("codigo_setor")
                oOrdemServico.setor = oSqlDataReader("setor")
                oOrdemServico.categoria = oSqlDataReader("categoria")
                oOrdemServico.codigo_equipamento = oSqlDataReader("codigo_equipamento")
                oOrdemServico.equipamento = oSqlDataReader("equipamento")
                oOrdemServico.codigo_apartamento = oSqlDataReader("codigo_apartamento")
                oOrdemServico.apartamento = oSqlDataReader("apartamento")
                oOrdemServico.numero_documento = oSqlDataReader("numero_documento")
                oOrdemServico.data = oSqlDataReader("data")
                oOrdemServico.data_necessidade = oSqlDataReader("data_necessidade")
                oOrdemServico.data_necessidade_inicial = oSqlDataReader("data_necessidade_inicial")
                oOrdemServico.descricao = oSqlDataReader("descricao")
                oOrdemServico.solicitante = oSqlDataReader("solicitante")
                oOrdemServico.codigo_prioridade = oSqlDataReader("codigo_prioridade")
                oOrdemServico.prioridade = oSqlDataReader("prioridade")
                oOrdemServico.arquivo = oSqlDataReader.Item("arquivo")
                oOrdemServico.status = oSqlDataReader("status")
                oOrdemServico.horas = oSqlDataReader("horas")
                oOrdemServico.imagem = oSqlDataReader("imagem")
                oOrdemServico.justificativa_cancelamento = oSqlDataReader("justificativa_cancelamento")
                oOrdemServico.fotos = oPicture.PictureList(iCodigoEmpresa:=iCodigoEmpresa,
                                                           iCodigoUnidade:=oSqlDataReader.Item("codigo_unidade"),
                                                           lCodigo:=oSqlDataReader.Item("codigo"),
                                                           iCodigoItemChecklist:=-1,
                                                           sTipo:=sTipo)

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function OrdemServicoAtribuir(ByVal iCodigoEmpresa As Integer,
                                         ByVal iCodigoUsuario AS Integer,
                                         Optional ByVal iCodigoUnidade As Integer = 0,
                                         Optional ByVal sDataInicio As String = "",
                                         Optional ByVal sDataTermino As String = "",
                                         Optional ByVal sOrdemServico As String = "",
                                         Optional ByVal iCodigoSetor As Integer = 0,
                                         Optional ByVal iCodigoPrioridade As Integer = 0,
                                         Optional ByVal lCodigoEquipamento As Long = 0,
                                         Optional ByVal iCodigoSolicitante As Integer = 0,
                                         Optional ByVal iCodigoApartamento As Integer = 0,
                                         Optional ByVal sExecutor As String = "") As List(Of MODELS.OrdemServico)

        Try

            'Váriaveis Locais
            Dim oOrdemServico As New List(Of MODELS.OrdemServico)
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

            'Seta Parametros - Ordem Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ordem_servico"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sOrdemServico : i += 1

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

            'Seta Parametros - Executor
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "executor"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Value = 100
            oSqlParameter(i).Value = sExecutor

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pcm_ordem_servico_atribuir", oSqlParameter)

            While oSqlDataReader.Read

                Dim oOrdemServicoInfo As New MODELS.OrdemServico

                oOrdemServicoInfo.codigo = oSqlDataReader("codigo")
                oOrdemServicoInfo.codigo_unidade = oSqlDataReader("codigo_unidade")
                oOrdemServicoInfo.unidade = oSqlDataReader("unidade")
                oOrdemServicoInfo.categoria = oSqlDataReader("categoria")
                oOrdemServicoInfo.codigo_setor = oSqlDataReader("codigo_setor")
                oOrdemServicoInfo.setor = oSqlDataReader("setor")
                oOrdemServicoInfo.codigo_equipamento = oSqlDataReader("codigo_equipamento")
                oOrdemServicoInfo.equipamento = oSqlDataReader("equipamento")
                oOrdemServicoInfo.local = oSqlDataReader("local")
                oOrdemServicoInfo.solicitante = oSqlDataReader("solicitante")
                oOrdemServicoInfo.executor = oSqlDataReader("executor")
                oOrdemServicoInfo.data_execucao = oSqlDataReader("data_execucao")
                oOrdemServicoInfo.numero_documento = oSqlDataReader("numero_documento")
                oOrdemServicoInfo.data = oSqlDataReader("data")
                oOrdemServicoInfo.descricao = oSqlDataReader("descricao")
                oOrdemServicoInfo.codigo_prioridade = oSqlDataReader("codigo_prioridade")
                oOrdemServicoInfo.prioridade = oSqlDataReader("prioridade")
                oOrdemServicoInfo.codigo_tipo_servico = oSqlDataReader("codigo_tipo_servico")
                oOrdemServicoInfo.tipo_servico = oSqlDataReader("tipo_servico")
                oOrdemServicoInfo.codigo_tipo_ordem_servico = oSqlDataReader("codigo_tipo_ordem_servico")
                oOrdemServicoInfo.tipo_ordem_servico = oSqlDataReader("tipo_ordem_servico")
                oOrdemServicoInfo.status = oSqlDataReader("status")
                oOrdemServicoInfo.status_descricao = oSqlDataReader("status_descricao")

                oOrdemServico.Add(oOrdemServicoInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oOrdemServico

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function IndexOrdemServico(ByVal iCodigoEmpresa As Integer,
                                      ByVal iCodigoUsuario As Integer,
                                      Optional ByVal iCodigoUnidade As Integer = 0,
                                      Optional ByVal iCodigoDepartamento As Integer = -1,
                                      Optional ByVal sDataInicio As String = "",
                                      Optional ByVal sDataTermino As String = "",
                                      Optional ByVal sOrdemServico As String = "",
                                      Optional ByVal sOrdemServicoCliente As String = "",
                                      Optional ByVal iCodigoSetor As Integer = 0,
                                      Optional ByVal iCodigoPrioridade As Integer = 0,
                                      Optional ByVal lCodigoEquipamento As Long = 0,
                                      Optional ByVal iCodigoSolicitante As Integer = 0,
                                      Optional ByVal iCodigoApartamento As Integer = 0,
                                      Optional ByVal iCodigoResponsavelApartamento As Integer = -1,
                                      Optional ByVal sExecutor As String = "",
                                      Optional ByVal iStatus As Integer = 0,
                                      Optional ByVal iCodigoJustificativaApontamento As Integer = -1,
                                      Optional ByVal sDataInicioExecucao As String = "",
                                      Optional ByVal sDataTerminoExecucao As String = "",
                                      Optional ByVal iHospede As Integer = -1) As List(Of MODELS.OrdemServico)

        Try

            'Váriaveis Locais
            Dim oOrdemServico As New List(Of MODELS.OrdemServico)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(19) As SqlParameter
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

            'Seta Parametros - Código Departamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_departamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoDepartamento : i += 1

            'Seta Parametros - Ordem Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ordem_servico"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sOrdemServico : i += 1

            'Seta Parametros - Ordem Serviço Cliente
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ordem_servico_cliente"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sOrdemServicoCliente : i += 1

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

            'Seta Parametros - Código Responsável Apartamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_responsavel_apartamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoResponsavelApartamento : i += 1

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

            'Seta Parametros - Executor
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "executor"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Value = 100
            oSqlParameter(i).Value = sExecutor : i += 1

            'Seta Parametros - Status
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "status"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iStatus : i += 1

            'Seta Parametros - Hóspede
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "hospede"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = IIf(iHospede = -1, DBNull.Value, IIf(iHospede = 1, True, False)) : i += 1

            'Seta Parametros - Código Justificativa - Apontamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_justificativa_apontamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoJustificativaApontamento : i += 1

            'Seta Parametros - Data Início
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_inicio_execucao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = IIf(sDataInicioExecucao = "", DBNull.Value, sDataInicioExecucao) : i += 1

            'Seta Parametros - Data Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_termino_execucao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = IIf(sDataTerminoExecucao = "", DBNull.Value, sDataTerminoExecucao)


            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pcm_ordem_servico", oSqlParameter)

            While oSqlDataReader.Read

                Dim oOrdemServicoInfo As New MODELS.OrdemServico

                oOrdemServicoInfo.codigo = oSqlDataReader("codigo")
                oOrdemServicoInfo.codigo_unidade = oSqlDataReader("codigo_unidade")
                oOrdemServicoInfo.unidade = oSqlDataReader("unidade")
                oOrdemServicoInfo.categoria = oSqlDataReader("categoria")
                oOrdemServicoInfo.codigo_setor = oSqlDataReader("codigo_setor")
                oOrdemServicoInfo.setor = oSqlDataReader("setor")
                oOrdemServicoInfo.codigo_equipamento = oSqlDataReader("codigo_equipamento")
                oOrdemServicoInfo.equipamento = oSqlDataReader("equipamento")
                oOrdemServicoInfo.local = oSqlDataReader("local")
                oOrdemServicoInfo.solicitante = oSqlDataReader("solicitante")
                oOrdemServicoInfo.executor = oSqlDataReader("executor")
                oOrdemServicoInfo.data_execucao = oSqlDataReader("data_execucao")
                oOrdemServicoInfo.numero_documento = oSqlDataReader("numero_documento")
                oOrdemServicoInfo.ordem_servico_cliente = oSqlDataReader("ordem_servico_cliente")
                oOrdemServicoInfo.data = Format(oSqlDataReader("data"), "dd/MM/yyyy HH:mm")
                oOrdemServicoInfo.data_necessidade = Format(oSqlDataReader("data_necessidade"), "dd/MM/yyyy")
                oOrdemServicoInfo.data_necessidade_inicial = oSqlDataReader("data_necessidade_inicial")
                oOrdemServicoInfo.dias = oSqlDataReader("dias")
                oOrdemServicoInfo.css_class = oSqlDataReader("css_class")
                oOrdemServicoInfo.descricao = oSqlDataReader("descricao")
                oOrdemServicoInfo.codigo_prioridade = oSqlDataReader("codigo_prioridade")
                oOrdemServicoInfo.prioridade = oSqlDataReader("prioridade")
                oOrdemServicoInfo.codigo_tipo_servico = oSqlDataReader("codigo_tipo_servico")
                oOrdemServicoInfo.tipo_servico = oSqlDataReader("tipo_servico")
                oOrdemServicoInfo.codigo_tipo_ordem_servico = oSqlDataReader("codigo_tipo_ordem_servico")
                oOrdemServicoInfo.tipo_ordem_servico = oSqlDataReader("tipo_ordem_servico")
                oOrdemServicoInfo.status = oSqlDataReader("status")
                oOrdemServicoInfo.status_descricao = oSqlDataReader("status_descricao")
                oOrdemServicoInfo.justificativa_apontamento = oSqlDataReader("justificativa_apontamento")

                oOrdemServico.Add(oOrdemServicoInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oOrdemServico

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ExcelOrdemServico(ByVal iCodigoEmpresa As Integer,
                                      ByVal iCodigoUsuario As Integer,
                                      Optional ByVal iCodigoUnidade As Integer = 0,
                                      Optional ByVal iCodigoDepartamento As Integer = -1,
                                      Optional ByVal sDataInicio As String = "",
                                      Optional ByVal sDataTermino As String = "",
                                      Optional ByVal sOrdemServico As String = "",
                                      Optional ByVal sOrdemServicoCliente As String = "",
                                      Optional ByVal iCodigoSetor As Integer = 0,
                                      Optional ByVal iCodigoPrioridade As Integer = 0,
                                      Optional ByVal lCodigoEquipamento As Long = 0,
                                      Optional ByVal iCodigoSolicitante As Integer = 0,
                                      Optional ByVal iCodigoApartamento As Integer = 0,
                                      Optional ByVal iCodigoResponsavelApartamento As Integer = -1,
                                      Optional ByVal sExecutor As String = "",
                                      Optional ByVal iStatus As Integer = 0,
                                      Optional ByVal iCodigoJustificativaApontamento As Integer = -1) As ExcelPackage
        Try


            ExcelPackage.License.SetNonCommercialOrganization("<ACTI>")

            'Váriaveis Locais
            Dim oDataSet As DataSet
            Dim oSqlParameter(16) As SqlParameter
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

            'Seta Parametros - Código Departamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_departamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoDepartamento : i += 1

            'Seta Parametros - Ordem Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ordem_servico"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sOrdemServico : i += 1

            'Seta Parametros - Ordem Serviço Cliente
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ordem_servico_cliente"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sOrdemServicoCliente : i += 1

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

            'Seta Parametros - Código Responsável Apartamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_responsavel_apartamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoResponsavelApartamento : i += 1

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

            'Seta Parametros - Executor
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "executor"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Value = 100
            oSqlParameter(i).Value = sExecutor : i += 1

            'Seta Parametros - Status
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "status"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iStatus : i += 1

            'Seta Parametros - Código Justificativa - Apontamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_justificativa_apontamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoJustificativaApontamento

            'Executa Query
            oDataSet = ExecuteDataset(sConnection, CommandType.StoredProcedure, "sp_select_pcm_ordem_servico_excel", oSqlParameter)

            'ExcelPackage.LicenseContext = LicenseContext.NonCommercial

            Dim oExcelPackage As New ExcelPackage()

            Dim oWSExcel = oExcelPackage.Workbook.Worksheets.Add("OS")
            oWSExcel.Cells("A1").LoadFromDataTable(oDataSet.Tables(0), True)

            oWSExcel.Cells("A1:" & NumeroParaColuna(oDataSet.Tables(0).Columns.Count) & "1").Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium)
            oWSExcel.Cells("A1:" & NumeroParaColuna(oDataSet.Tables(0).Columns.Count) & "1").Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid
            oWSExcel.Cells("A1:" & NumeroParaColuna(oDataSet.Tables(0).Columns.Count) & "1").Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DarkBlue)
            oWSExcel.Cells("A1:" & NumeroParaColuna(oDataSet.Tables(0).Columns.Count) & "1").Style.Font.Color.SetColor(System.Drawing.Color.White)

            oWSExcel.Cells("A:C").Style.HorizontalAlignment = ExcelHorizontalAlignment.Center
            oWSExcel.Cells("F:F").Style.HorizontalAlignment = ExcelHorizontalAlignment.Center
            oWSExcel.Cells("L:R").Style.HorizontalAlignment = ExcelHorizontalAlignment.Center

            oWSExcel.Cells.AutoFitColumns()

            'Retorno da Função
            Return oExcelPackage

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Function NumeroParaColuna(numero As Integer) As String
        Dim coluna As String = ""

        Do While numero > 0
            Dim modulo As Integer = (numero - 1) Mod 26
            coluna = Convert.ToChar(65 + modulo).ToString() & coluna
            numero = (numero - modulo) \ 26
        Loop

        Return coluna
    End Function

    Public Sub UpdateOrdemServicoStatus(ByVal iCodigoEmpresa As Integer,
                                        ByVal iCodigoUsuario As Integer,
                                        ByVal lCodigo As Long,
                                        ByVal iCodigoUnidade As Integer,
                                        ByVal iStatus As Integer)

        'Váriaveis Locais
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

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Ordem de Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_pcm_ordem_servico"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigo : i += 1

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

    Public Function VincularFuncionarioOS(ByVal iCodigoEmpresa As Integer,
                                          ByVal iCodigoUsuario As Integer,
                                          ByVal lCodigo As Long,
                                          ByVal iCodigoUnidade As Integer,
                                          ByVal iStatus As Integer,
                                          ByVal sCodigoFuncionario As String,
                                          ByVal sNomeFuncionario As String) As Boolean

        'Váriaveis Locais
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

            'Seta Parametros - Código Ordem de Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_pcm_ordem_servico"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigo : i += 1

            'Seta Parametros - Status
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "status"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iStatus : i += 1

            'Seta Parametros - Nome Funcionario
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "funcionario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 8000
            oSqlParameter(i).Value = sNomeFuncionario : i += 1

            'Seta Parametros - Código Funcionario
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_funcionario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 8000
            oSqlParameter(i).Value = sCodigoFuncionario

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_pcm_ordem_servico_status_vincular_funcionario", oSqlParameter)

            Return True

        Catch SqlEx As SqlException
            Return False
        Catch ex As Exception
            Return False
        End Try

    End Function

    Public Function UpdateOrdemServicoDataNecessidade(ByVal iCodigoEmpresa As Integer,
                                                      ByVal iCodigoUsuario As Integer,
                                                      ByVal lCodigo As Long,
                                                      ByVal iCodigoUnidade As Integer,
                                                      ByVal sDataNecessidade As String) As Boolean

        'Váriaveis Locais
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

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Ordem de Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_pcm_ordem_servico"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigo : i += 1

            'Seta Parametros - Data Necessidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_necessidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.DateTime
            oSqlParameter(i).Value = sDataNecessidade

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_pcm_ordem_servico_data_necessidade", oSqlParameter)

            Return True

        Catch SqlEx As SqlException
            Return False
        Catch ex As Exception
            Return False
        End Try

    End Function

    Public Function OrdemServicoApontamento(ByVal iCodigoEmpresa As Integer,
                                            ByVal iCodigoUnidade As Integer,
                                            ByVal lCodigoOrdemServico As Long) As List(Of MODELS.OrdemServicoApontamento)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of MODELS.OrdemServicoApontamento)
            Dim oPicture As New Picture(sConnection)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(12) As SqlParameter
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

            'Seta Parametros - Código Ordem Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_ordem_servico"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoOrdemServico

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pcm_ordem_servico_apontamento", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New MODELS.OrdemServicoApontamento

                oInfo.item = oSqlDataReader.Item("item")
                oInfo.executor = oSqlDataReader.Item("executor")
                oInfo.solucao = oSqlDataReader.Item("descricao_solucao")
                oInfo.justificativa = oSqlDataReader.Item("justificativa")
                oInfo.data_inicio = oSqlDataReader.Item("data_inicio")
                oInfo.data_termino = oSqlDataReader.Item("data_termino")
                oInfo.horas = oSqlDataReader.Item("horas")
                oInfo.imagem_ordem_servico = oSqlDataReader.Item("imagem_ordem_servico")
                oInfo.imagem_apontamento = oSqlDataReader.Item("imagem_apontamento")
                oInfo.usuario = SafeGetString(oSqlDataReader, "usuario")
                oInfo.fotos = oPicture.PictureList(iCodigoEmpresa:=iCodigoEmpresa,
                                                   iCodigoUnidade:=iCodigoUnidade,
                                                   lCodigo:=oSqlDataReader.Item("codigo_ordem_servico"),
                                                   iCodigoItemChecklist:=-1,
                                                   sTipo:="OS_APONTAMENTO")

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

    Public Function ValidateOrdemServico(ByVal iCodigoEmpresa As Integer,
                                         ByVal iCodigoUnidade As Integer,
                                         ByVal lCodigoEquipamento As Long) As Boolean

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

            'Seta Parametros - Código Equipamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_equipamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoEquipamento

            'Executa Query
            Return CType(ExecuteScalar(sConnection, CommandType.StoredProcedure, "sp_validate_pcm_ordem_servico_equipamento", oSqlParameter), Boolean)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

End Class
