Imports PCM.WEB.MODELS
Imports PCM.WEB.DAL.SQLHelper
Imports System.Data.SqlClient

Public Class Tudo

    Private sConnection As String

    Sub New(ByVal sCon As String)
        sConnection = sCon
    End Sub

#Region "::: TUDO EM DIA - CHECKLIST :::"

    ' Lista de locais a executar o checklist (agrupável por setor)
    Public Function LoadTudoChecklist(ByVal codigoEmpresa As Integer,
                                      ByVal codigoUnidade As Integer,
                                      ByVal codigoSetor As Integer,
                                      ByVal status As String) As List(Of TudoLocal)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of TudoLocal)
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("codigo_setor", SqlDbType.Int, codigoSetor),
                CriarParametro("status", SqlDbType.VarChar, status)
            }

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_tudo_checklist", oSqlParameter)

                If oSqlDataReader.HasRows Then

                    While oSqlDataReader.Read

                        Dim oInfo As New TudoLocal

                        oInfo.codigo_unidade = SafeGetInt32(oSqlDataReader, "codigo_unidade")
                        oInfo.unidade = SafeGetString(oSqlDataReader, "unidade")
                        oInfo.codigo_setor = SafeGetInt32(oSqlDataReader, "codigo_setor")
                        oInfo.setor = SafeGetString(oSqlDataReader, "setor")
                        oInfo.codigo = SafeGetInt32(oSqlDataReader, "codigo")
                        oInfo.local = SafeGetString(oSqlDataReader, "local")
                        oInfo.codigo_checklist = SafeGetInt64(oSqlDataReader, "codigo_checklist")
                        oInfo.checklist = SafeGetString(oSqlDataReader, "checklist")
                        oInfo.codigo_periodicidade = SafeGetInt32(oSqlDataReader, "codigo_periodicidade")
                        oInfo.periodicidade = SafeGetString(oSqlDataReader, "periodicidade")
                        oInfo.intervalo = SafeGetInt32(oSqlDataReader, "intervalo")
                        oInfo.data_proxima = SafeGetString(oSqlDataReader, "data_proxima")
                        oInfo.status = SafeGetInt32(oSqlDataReader, "status")
                        oInfo.css_class = SafeGetString(oSqlDataReader, "css_class")
                        oInfo.color = SafeGetString(oSqlDataReader, "color")
                        oInfo.bg_color = SafeGetString(oSqlDataReader, "bg_color")
                        oInfo.codigo_apontamento = SafeGetInt64(oSqlDataReader, "codigo_apontamento")

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

    ' Contadores por status (torre de KPIs)
    Public Function LoadTudoChecklistStatus(ByVal codigoEmpresa As Integer,
                                            ByVal codigoUnidade As Integer,
                                            ByVal codigoSetor As Integer) As TudoStatus

        Try

            'Váriaveis Locais
            Dim oReturn As New TudoStatus
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("codigo_setor", SqlDbType.Int, codigoSetor)
            }

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_tudo_checklist_status", oSqlParameter)

                If oSqlDataReader.HasRows Then

                    While oSqlDataReader.Read

                        oReturn.atrasado = SafeGetInt32(oSqlDataReader, "quantidade_atrasado")
                        oReturn.pendente = SafeGetInt32(oSqlDataReader, "quantidade_pendente")
                        oReturn.manutencao = SafeGetInt32(oSqlDataReader, "quantidade_manutencao")
                        oReturn.nova_vistoria = SafeGetInt32(oSqlDataReader, "quantidade_nova_vistoria")
                        oReturn.realizada = SafeGetInt32(oSqlDataReader, "quantidade_realizada")

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

#End Region

#Region "::: TUDO EM DIA - APONTAMENTO :::"

    ' Cabeçalho do apontamento + contexto do local
    Public Function LoadTudoApontamento(ByVal codigoEmpresa As Integer,
                                        ByVal codigoUnidade As Integer,
                                        ByVal codigoApartamento As Integer,
                                        ByVal codigo As Long) As TudoApontamento

        Try

            Dim oReturn As New TudoApontamento
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("codigo_apartamento", SqlDbType.Int, codigoApartamento),
                CriarParametro("codigo", SqlDbType.BigInt, codigo)
            }

            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_tudo_apontamento", oSqlParameter)

                If oSqlDataReader.HasRows Then

                    While oSqlDataReader.Read

                        oReturn.codigo = SafeGetInt64(oSqlDataReader, "codigo")
                        oReturn.codigo_apartamento = SafeGetInt32(oSqlDataReader, "codigo_apartamento")
                        oReturn.local = SafeGetString(oSqlDataReader, "local")
                        oReturn.setor = SafeGetString(oSqlDataReader, "setor")
                        oReturn.codigo_checklist = SafeGetInt64(oSqlDataReader, "codigo_checklist")
                        oReturn.checklist = SafeGetString(oSqlDataReader, "checklist")
                        oReturn.periodicidade = SafeGetString(oSqlDataReader, "periodicidade")
                        oReturn.intervalo = SafeGetInt32(oSqlDataReader, "intervalo")
                        oReturn.data_proxima = SafeGetString(oSqlDataReader, "data_proxima")
                        oReturn.codigo_funcionario_responsavel = SafeGetInt32(oSqlDataReader, "codigo_funcionario_responsavel")
                        oReturn.data_inicio = SafeGetString(oSqlDataReader, "data_inicio")
                        oReturn.data_termino = SafeGetString(oSqlDataReader, "data_termino")
                        oReturn.hora_inicio = SafeGetString(oSqlDataReader, "hora_inicio")
                        oReturn.hora_termino = SafeGetString(oSqlDataReader, "hora_termino")
                        oReturn.nova_vistoria = SafeGetBoolean(oSqlDataReader, "nova_vistoria")

                    End While

                End If

            End Using

            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    ' Itens do checklist (template quando codigo = -1; respondidos caso contrário)
    Public Function LoadTudoApontamentoChecklist(ByVal codigoEmpresa As Integer,
                                                 ByVal codigoUnidade As Integer,
                                                 ByVal codigoApartamento As Integer,
                                                 ByVal codigo As Long) As List(Of TudoApontamentoChecklist)

        Try

            Dim oReturn As New List(Of TudoApontamentoChecklist)
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("codigo_apartamento", SqlDbType.Int, codigoApartamento),
                CriarParametro("codigo", SqlDbType.BigInt, codigo)
            }

            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_tudo_apontamento_checklist_item", oSqlParameter)

                If oSqlDataReader.HasRows Then

                    While oSqlDataReader.Read

                        Dim oInfo As New TudoApontamentoChecklist

                        oInfo.grupo = SafeGetString(oSqlDataReader, "grupo")
                        oInfo.codigo = SafeGetInt32(oSqlDataReader, "codigo")
                        oInfo.checklist = SafeGetString(oSqlDataReader, "checklist")
                        oInfo.descricao = SafeGetString(oSqlDataReader, "descricao")
                        oInfo.opcao = SafeGetString(oSqlDataReader, "opcao")
                        oInfo.observacao = SafeGetString(oSqlDataReader, "observacao")
                        oInfo.nova_vistoria = SafeGetBoolean(oSqlDataReader, "nova_vistoria")

                        oReturn.Add(oInfo)

                    End While

                End If

            End Using

            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    ' Insere o cabeçalho do apontamento (retorna codigo e codigo_checklist)
    Public Sub InsertTudoApontamento(ByVal codigoEmpresa As Integer,
                                     ByVal codigoUsuario As Integer,
                                     ByVal codigoUnidade As Integer,
                                     ByVal codigoApartamento As Integer,
                                     ByVal codigoFuncionario As Integer,
                                     ByVal dataInicio As DateTime,
                                     ByVal dataTermino As DateTime,
                                     ByVal horaInicio As String,
                                     ByVal horaTermino As String,
                                     ByRef codigo As Long,
                                     ByRef codigoChecklist As Long)

        Try

            Dim pCodigo As SqlParameter = CriarParametro("codigo", SqlDbType.BigInt, codigo, ParameterDirection.InputOutput)
            Dim pChecklist As SqlParameter = CriarParametro("codigo_checklist", SqlDbType.BigInt, DBNull.Value, ParameterDirection.Output)

            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_usuario", SqlDbType.Int, codigoUsuario),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("codigo_apartamento", SqlDbType.Int, codigoApartamento),
                CriarParametro("codigo_funcionario_responsavel", SqlDbType.Int, codigoFuncionario),
                CriarParametro("data_inicio", SqlDbType.Date, dataInicio),
                CriarParametro("data_termino", SqlDbType.Date, dataTermino),
                CriarParametro("hora_inicio", SqlDbType.Time, If(String.IsNullOrEmpty(horaInicio), DBNull.Value, TimeSpan.Parse(horaInicio))),
                CriarParametro("hora_termino", SqlDbType.Time, If(String.IsNullOrEmpty(horaTermino), DBNull.Value, TimeSpan.Parse(horaTermino))),
                pCodigo,
                pChecklist
            }

            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_tudo_apontamento", oSqlParameter)

            codigo = If(IsDBNull(pCodigo.Value), 0L, Convert.ToInt64(pCodigo.Value))
            codigoChecklist = If(IsDBNull(pChecklist.Value), 0L, Convert.ToInt64(pChecklist.Value))

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    ' Insere um item respondido (abre OS quando "NÃO")
    Public Sub InsertTudoApontamentoChecklist(ByVal codigoEmpresa As Integer,
                                              ByVal codigoUnidade As Integer,
                                              ByVal codigoTudoApontamento As Long,
                                              ByVal codigoChecklist As Long,
                                              ByVal codigoChecklistItem As Integer,
                                              ByVal descricaoChecklist As String,
                                              ByVal opcao As String,
                                              ByVal observacao As String,
                                              ByVal novaVistoria As Boolean)

        Try

            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("codigo_tudo_apontamento", SqlDbType.BigInt, codigoTudoApontamento),
                CriarParametro("codigo_checklist", SqlDbType.BigInt, codigoChecklist),
                CriarParametro("codigo_checklist_item", SqlDbType.Int, codigoChecklistItem),
                CriarParametro("descricao_checklist", SqlDbType.VarChar, If(String.IsNullOrEmpty(descricaoChecklist), "", descricaoChecklist)),
                CriarParametro("opcao", SqlDbType.VarChar, opcao),
                CriarParametro("observacao", SqlDbType.VarChar, If(String.IsNullOrEmpty(observacao), "", observacao)),
                CriarParametro("nova_vistoria", SqlDbType.Bit, novaVistoria)
            }

            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_tudo_apontamento_checklist", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    ' Recalcula o status (por OS aberta) e a próxima data
    Public Sub UpdateTudoStatus(ByVal codigoEmpresa As Integer,
                                ByVal codigoUnidade As Integer,
                                ByVal codigoTudoApontamento As Long)

        Try

            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("codigo_tudo_apontamento", SqlDbType.BigInt, codigoTudoApontamento)
            }

            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_status_tudo_dia", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region

#Region "::: TUDO EM DIA - HISTÓRICO :::"

    Public Function LoadTudoChecklistHistorico(ByVal codigoEmpresa As Integer,
                                               ByVal codigoUnidade As Integer,
                                               ByVal dataInicio As String,
                                               ByVal dataTermino As String,
                                               ByVal codigoApartamento As Integer) As List(Of TudoChecklistHistorico)

        Try

            Dim oReturn As New List(Of TudoChecklistHistorico)
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("data_inicio", SqlDbType.Date, Convert.ToDateTime(dataInicio, New Globalization.CultureInfo("pt-BR"))),
                CriarParametro("data_termino", SqlDbType.Date, Convert.ToDateTime(dataTermino, New Globalization.CultureInfo("pt-BR"))),
                CriarParametro("codigo_apartamento", SqlDbType.Int, codigoApartamento)
            }

            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_tudo_checklist_historico", oSqlParameter)

                If oSqlDataReader.HasRows Then

                    While oSqlDataReader.Read

                        Dim oInfo As New TudoChecklistHistorico

                        oInfo.codigo_unidade = SafeGetInt32(oSqlDataReader, "codigo_unidade")
                        oInfo.unidade = SafeGetString(oSqlDataReader, "unidade")
                        oInfo.setor = SafeGetString(oSqlDataReader, "setor")
                        oInfo.codigo_apartamento = SafeGetInt32(oSqlDataReader, "codigo_apartamento")
                        oInfo.local = SafeGetString(oSqlDataReader, "local")
                        oInfo.codigo = SafeGetInt64(oSqlDataReader, "codigo")
                        oInfo.checklist = SafeGetString(oSqlDataReader, "checklist")
                        oInfo.responsavel = SafeGetString(oSqlDataReader, "responsavel")
                        oInfo.data_inicio = SafeGetString(oSqlDataReader, "data_inicio")
                        oInfo.data_termino = SafeGetString(oSqlDataReader, "data_termino")
                        oInfo.tempo = SafeGetString(oSqlDataReader, "tempo")

                        oReturn.Add(oInfo)

                    End While

                End If

            End Using

            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

End Class
