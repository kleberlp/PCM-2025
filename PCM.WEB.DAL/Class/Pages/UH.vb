Imports PCM.WEB.MODELS
Imports System.DirectoryServices
Imports System.DirectoryServices.ActiveDirectory
Imports PCM.WEB.DAL.SQLHelper
Imports System.Data.SqlClient
Imports System.Text

Public Class UH

    Private sConnection As String

    Sub New(ByVal sCon As String)
        sConnection = sCon
    End Sub

#Region "::: CHECKLIST :::"

    Public Function LoadChecklistStatus(ByVal iCodigoEmpresa As Integer,
                                        ByVal iCodigoUnidade As Integer,
                                        ByVal iCodigoUsuario As Integer) As UHChecklistStatus

        Try

            'Váriaveis Locais
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(2) As SqlParameter
            Dim oReturn As New UHChecklistStatus
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
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_uh_checklist_status", oSqlParameter)

            While oSqlDataReader.Read

                oReturn.quantidade_atrasado = oSqlDataReader.Item("quantidade_atrasado")
                oReturn.quantidade_pendente = oSqlDataReader.Item("quantidade_pendente")
                oReturn.quantidade_nova_vistoria = oSqlDataReader.Item("quantidade_nova_vistoria")
                oReturn.quantidade_manutencao = oSqlDataReader.Item("quantidade_manutencao")
                oReturn.quantidade_realizada = oSqlDataReader.Item("quantidade_realizada")

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Seta Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    'Public Function LoadChecklist(ByVal iCodigoEmpresa As Integer,
    '                              ByVal iCodigoUnidade As Integer,
    '                              ByVal iCodigoUsuario As Integer,
    '                              Optional ByVal iStatus As Integer = -1) As List(Of UHChecklist)

    '    Try

    '        'Váriaveis Locais
    '        Dim oUHChecklist As New List(Of UHChecklist)
    '        Dim oSqlDataReader As SqlDataReader
    '        Dim oSqlParameter(2) As SqlParameter
    '        Dim i As Integer = 0

    '        'Seta Parametros - Código Empresa
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "codigo_empresa"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
    '        oSqlParameter(i).Value = iCodigoEmpresa : i += 1

    '        'Seta Parametros - Código Usuário
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "codigo_usuario"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.Int
    '        oSqlParameter(i).Value = iCodigoUsuario : i += 1

    '        'Seta Parametros - Código Unidade
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "codigo_unidade"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.Int
    '        oSqlParameter(i).Value = iCodigoUnidade

    '        'Executa Query
    '        oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_uh_checklist_unidade", oSqlParameter)

    '        While oSqlDataReader.Read

    '            Dim oInfo As New UHChecklist

    '            oInfo.codigo_unidade = oSqlDataReader.Item("codigo")
    '            oInfo.unidade = oSqlDataReader.Item("nome_fantasia")
    '            oInfo.bloco = LoadUHBlocoChecklist(iCodigoEmpresa:=iCodigoEmpresa,
    '                                               iCodigoUnidade:=oSqlDataReader.Item("codigo"),
    '                                               iStatus:=iStatus)

    '            If (oInfo.bloco.Count > 0) Then oUHChecklist.Add(oInfo)

    '        End While

    '        'Fecha o oSqlDataReader
    '        If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

    '        'Retorno da Função
    '        Return oUHChecklist

    '    Catch SqlEx As SqlException
    '        Throw SqlEx
    '    Catch ex As Exception
    '        Throw ex
    '    End Try

    'End Function

    'Public Function LoadUHBlocoChecklist(ByVal iCodigoEmpresa As Integer,
    '                                     ByVal iCodigoUnidade As Integer,
    '                                     ByVal iStatus As Integer) As List(Of UHBloco)

    '    Try

    '        'Váriaveis Locais
    '        Dim oUHBloco As New List(Of UHBloco)
    '        Dim oSqlDataReader As SqlDataReader
    '        Dim oSqlParameter(1) As SqlParameter
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
    '        oSqlParameter(i).Value = iCodigoUnidade

    '        'Executa Query
    '        oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_uh_checklist_bloco", oSqlParameter)

    '        While oSqlDataReader.Read

    '            Dim oInfo As New UHBloco

    '            oInfo.codigo = oSqlDataReader.Item("codigo")
    '            oInfo.descricao = oSqlDataReader.Item("descricao")
    '            oInfo.andar = LoadUHAndarChecklist(iCodigoEmpresa:=iCodigoEmpresa,
    '                                               iCodigoUnidade:=iCodigoUnidade,
    '                                               iCodigoBloco:=oSqlDataReader.Item("codigo"),
    '                                               iStatus:=iStatus)

    '            If (oInfo.andar.Count > 0) Then oUHBloco.Add(oInfo)

    '        End While

    '        'Fecha o oSqlDataReader
    '        If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

    '        'Retorno da Função
    '        Return oUHBloco

    '    Catch SqlEx As SqlException
    '        Throw SqlEx
    '    Catch ex As Exception
    '        Throw ex
    '    End Try

    'End Function

    'Public Function LoadUHAndarChecklist(ByVal iCodigoEmpresa As Integer,
    '                                     ByVal iCodigoUnidade As Integer,
    '                                     ByVal iCodigoBloco As Integer,
    '                                     ByVal iStatus As Integer) As List(Of UHAndar)

    '    Try

    '        'Váriaveis Locais
    '        Dim oUHAndar As New List(Of UHAndar)
    '        Dim oSqlDataReader As SqlDataReader
    '        Dim oSqlParameter(2) As SqlParameter
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

    '        'Seta Parametros - Código Bloco
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "bloco"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.Int
    '        oSqlParameter(i).Value = iCodigoBloco

    '        'Executa Query
    '        oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_uh_checklist_andar", oSqlParameter)

    '        While oSqlDataReader.Read

    '            Dim oInfo As New UHAndar

    '            oInfo.codigo = oSqlDataReader.Item("codigo")
    '            oInfo.descricao = oSqlDataReader.Item("descricao")
    '            oInfo.uh = LoadUHChecklist(iCodigoEmpresa:=iCodigoEmpresa,
    '                                       iCodigoUnidade:=iCodigoUnidade,
    '                                       iCodigoBloco:=iCodigoBloco,
    '                                       iCodigoAndar:=oSqlDataReader.Item("codigo"),
    '                                       iStatus:=iStatus)

    '            If (oInfo.uh.Count > 0) Then oUHAndar.Add(oInfo)

    '        End While

    '        'Fecha o oSqlDataReader
    '        If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

    '        'Retorno da Função
    '        Return oUHAndar

    '    Catch SqlEx As SqlException
    '        Throw SqlEx
    '    Catch ex As Exception
    '        Throw ex
    '    End Try

    'End Function

    Public Function LoadUHChecklist(ByVal iCodigoEmpresa As Integer,
                                    ByVal iCodigoUnidade As Integer,
                                    ByVal sStatus As String) As List(Of MODELS.UH)

        Try

            'Váriaveis Locais
            Dim oUH As New List(Of MODELS.UH)
            Dim i As Integer = 0


            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, iCodigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, iCodigoUnidade),
                CriarParametro("status", SqlDbType.VarChar, sStatus)
                }

            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_uh_checklist", oSqlParameter)

                While oSqlDataReader.Read

                    Dim UHInfo As New MODELS.UH

                    UHInfo.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                    UHInfo.unidade = oSqlDataReader.Item("unidade")
                    UHInfo.bloco = oSqlDataReader.Item("bloco")
                    UHInfo.andar = oSqlDataReader.Item("andar")
                    UHInfo.codigo_apartamento = oSqlDataReader.Item("codigo_apartamento")
                    UHInfo.codigo_vistoria = oSqlDataReader.Item("codigo_vistoria")
                    UHInfo.apartamento = oSqlDataReader.Item("apartamento")
                    UHInfo.data_proxima = oSqlDataReader.Item("data_proxima")
                    UHInfo.css_class = oSqlDataReader.Item("css_class")
                    UHInfo.status = oSqlDataReader.Item("status")
                    UHInfo.room_status = oSqlDataReader.Item("room_status")
                    UHInfo.front_office_status = oSqlDataReader.Item("front_office_status")

                    oUH.Add(UHInfo)

                End While

            End Using

            'Retorno da Função
            Return oUH

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Sub LoadUHStatus(ByVal iCodigoEmpresa As Integer,
                            ByVal iCodigoUnidade As Integer,
                            ByRef lQuantidadePendente As Long,
                            ByRef lQuantidadeNovaVistoria As Long,
                            ByRef lQuantidadeRealizada As Long)

        Try

            'Váriaveis Locais
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
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_uh_checklist_status", oSqlParameter)

            While oSqlDataReader.Read

                lQuantidadePendente = oSqlDataReader.Item("quantidade_pendente")
                lQuantidadeNovaVistoria = oSqlDataReader.Item("quantidade_nova_vistoria")
                lQuantidadeRealizada = oSqlDataReader.Item("quantidade_realizada")
                
            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region

#Region "::: APONTAMENTO :::"

    Public Sub LoadDadosApontamento(ByVal iCodigoEmpresa As Integer,
                                    ByVal iCodigoUnidade As Integer,
                                    ByVal iCodigoApartamento As Integer,
                                    ByVal lCodigo As Long,
                                    ByRef oUHApontamento As UHApontamento)

        Try

            'Váriaveis Locais
            Dim oSqlParameter(3) As SqlParameter
            Dim oSqlDataReader As SqlDataReader
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
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoApartamento : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_uh_apontamento", oSqlParameter)

            While oSqlDataReader.Read

                oUHApontamento = New MODELS.UHApontamento
                oUHApontamento.unidade = oSqlDataReader.Item("unidade")
                oUHApontamento.apartamento = oSqlDataReader.Item("apartamento")
                oUHApontamento.codigo_funcionario_responsavel_unidade = oSqlDataReader.Item("codigo_funcionario_responsavel_unidade")
                oUHApontamento.funcionario_responsavel_unidade = oSqlDataReader.Item("funcionario_responsavel_unidade")
                oUHApontamento.codigo_funcionario_responsavel_vistoria = oSqlDataReader.Item("codigo_funcionario_responsavel_vistoria")
                oUHApontamento.funcionario_responsavel_vistoria = oSqlDataReader.Item("funcionario_responsavel_vistoria")
                oUHApontamento.aponta_horas = oSqlDataReader.Item("aponta_horas")
                oUHApontamento.data_inicio = oSqlDataReader.Item("data_inicio")
                oUHApontamento.hora_inicio = oSqlDataReader.Item("hora_inicio")
                oUHApontamento.data_termino = oSqlDataReader.Item("data_termino")
                oUHApontamento.hora_termino = oSqlDataReader.Item("hora_termino")
                oUHApontamento.nova_vistoria = oSqlDataReader.Item("nova_vistoria")
                oUHApontamento.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oUHApontamento.codigo_apartamento = oSqlDataReader.Item("codigo_apartamento")
                oUHApontamento.codigo_apontamento = oSqlDataReader.Item("codigo_apontamento")                

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function LoadUHApontamentoChecklist(ByVal iCodigoEmpresa As Integer,
                                               ByVal iCodigoUnidade As Integer,
                                               ByVal iCodigoApartamento As Integer,
                                               ByVal lCodigo As Long) As List(Of UHApontamentoChecklist)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of UHApontamentoChecklist)
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

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_uh_apontamento_checklist_item", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New UHApontamentoChecklist

                oInfo.grupo = oSqlDataReader.Item("grupo")
                oInfo.codigo = oSqlDataReader.Item("codigo")
                oInfo.checklist = oSqlDataReader.Item("checklist")
                oInfo.descricao = oSqlDataReader.Item("descricao")
                oInfo.opcao = oSqlDataReader.Item("opcao")
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

    Public Sub InsertUHApontamento(ByVal iCodigoEmpresa As Integer,
                                   ByVal iCodigoUsuario As Integer,
                                   ByVal iCodigoUnidade As Integer,
                                   ByVal iCodigoApartamento As Integer,
                                   ByVal iCodigoFuncionarioResponsavelVistoria As Integer,
                                   ByVal iCodigoFuncionarioResponsavelUnidade As Integer,
                                   ByVal sDataInicio As String,
                                   ByVal sDataTermino As String,
                                   ByVal sHoraInicio As String,
                                   ByVal sHoraTermino As String,
                                   ByRef lCodigoUHApontamento As Long,
                                   ByRef lCodigoChecklist As Long)

        Try

            'Váriaveis Locais
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

            'Seta Parametros - Código Apartamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_apartamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoApartamento : i += 1

            'Seta Parametros - Código Funcionário Responsável Vistoria
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_funcionario_responsavel_vistoria"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = iCodigoFuncionarioResponsavelVistoria : i += 1

            'Seta Parametros - Código Funcionário Responsável Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_funcionario_responsavel_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = iCodigoFuncionarioResponsavelUnidade : i += 1

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
            oSqlParameter(i).Value = sHoraInicio : i += 1

            'Seta Parametros - Hora Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "hora_termino"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Time
            oSqlParameter(i).Value = sHoraTermino : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.InputOutput
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoUHApontamento : i += 1

            'Seta Parametros - Código Checklist
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_checklist"
            oSqlParameter(i).Direction = ParameterDirection.Output
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_uh_apontamento", oSqlParameter)

            lCodigoUHApontamento = oSqlParameter(i - 1).Value
            lCodigoChecklist = oSqlParameter(i).Value

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InsertUHApontamentoChecklist(ByVal iCodigoEmpresa As Integer,
                                            ByVal iCodigoUnidade As Integer,
                                            ByVal lCodigoUHApontamento As Long,
                                            ByVal lCodigoChecklist As Long,
                                            ByVal iCodigoChecklistItem As Integer,
                                            ByVal sDescricaoChecklist As String,
                                            ByVal sOpcao As String,
                                            ByVal sObservacao As String,
                                            ByVal bNovaVistoria As Boolean)

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

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código UH Apontamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_uh_apontamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoUHApontamento : i += 1

            'Seta Parametros - Código Checklist
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_checklist"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(lCodigoChecklist = -1, DBNull.Value, lCodigoChecklist) : i += 1

            'Seta Parametros - Código Checklist Item
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_checklist_item"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoChecklistItem = -1, DBNull.Value, iCodigoChecklistItem) : i += 1

            'Seta Parametros - Descrição Checklist
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao_checklist"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = sDescricaoChecklist : i += 1

            'Seta Parametros - Opção
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "opcao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 5
            oSqlParameter(i).Value = IIf(IsNothing(sOpcao), "", sOpcao) : i += 1

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
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_uh_apontamento_checklist", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateUHStatus(ByVal iCodigoEmpresa As Integer,
                              ByVal iCodigoUnidade As Integer,
                              ByVal lCodigoUHApontamento As Long)

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

            'Seta Parametros - Código UH Apontamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_uh_apontamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoUHApontamento

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_status_uh_dia", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function DeleteUHApontamentoChecklist(ByVal iCodigoEmpresa As Integer,
                                                 ByVal iCodigoUnidade As Integer,
                                                 ByVal iCodigoApartamento As Integer,
                                                 ByVal lCodigo As Long) As String

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

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigo

            'Executa Query
            Return ExecuteScalar(sConnection, CommandType.StoredProcedure, "sp_delete_uh_apontamento", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: CHECKLIST - HISTÓRICO :::"

    Public Function LoadChecklistHistorico(ByVal iCodigoEmpresa As Integer,
                                           ByVal iCodigoUnidade As Integer,
                                           ByVal sDataInicio As String,
                                           ByVal sDataTermino As String,
                                           ByVal iCodigoApartamento As Integer) As List(Of UHChecklistHistorico)

        Try

            'Váriaveis Locais
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(4) As SqlParameter
            Dim oReturn As New List(Of UHChecklistHistorico)
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

            'Seta Parametros - Código Apartamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_apartamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoApartamento

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_uh_checklist_historico", oSqlParameter)

            While oSqlDataReader.Read

                Dim oReturnInfo As New UHChecklistHistorico

                oReturnInfo.data_inicio = oSqlDataReader.Item("data_inicio")
                oReturnInfo.data_termino = oSqlDataReader.Item("data_termino")
                oReturnInfo.unidade = oSqlDataReader.Item("unidade")
                oReturnInfo.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oReturnInfo.codigo_apartamento = oSqlDataReader.Item("codigo_apartamento")
                oReturnInfo.apartamento = oSqlDataReader.Item("apartamento")
                oReturnInfo.responsavel_vistoria = oSqlDataReader.Item("responsavel_vistoria")
                oReturnInfo.tempo_gasto = oSqlDataReader.Item("tempo_gasto")
                oReturnInfo.codigo = oSqlDataReader.Item("codigo")

                oReturn.Add(oReturnInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Seta Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: ATIVIDADE :::"

    Public Function LoadUHBlocoAtividade(ByVal iCodigoEmpresa As Integer,
                                         ByVal iCodigoUnidade As Integer,
                                         ByVal lCodigoAtividade As Long,
                                         ByVal iCodigoApartamento As Integer,
                                         ByVal iStatus As Integer,
                                         ByVal iStatusUH As Integer,
                                         ByRef iQuantidadeAtrasado As Integer,
                                         ByRef iQuantidadePendente As Integer,
                                         ByRef iQuantidadeRealizada As Integer) As List(Of UHAtividadeBloco)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of UHAtividadeBloco)
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

            'Seta Parametros - Código Atividade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_atividade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoAtividade : i += 1

            'Seta Parametros - Código Apartamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_apartamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoApartamento : i += 1

            'Seta Parametros - Status
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "status"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iStatus : i += 1

            'Seta Parametros - Status UH
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "status_uh"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iStatusUH

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_uh_atividade_bloco", oSqlParameter)

            While oSqlDataReader.Read

                Dim oReturnInfo As New UHAtividadeBloco

                oReturnInfo.codigo = oSqlDataReader.Item("codigo")
                oReturnInfo.descricao = oSqlDataReader.Item("descricao")
                oReturnInfo.andar = LoadUHAndarAtividade(iCodigoEmpresa:=iCodigoEmpresa,
                                                         iCodigoUnidade:=iCodigoUnidade,
                                                         lCodigoAtividade:=lCodigoAtividade,
                                                         iCodigoApartamento:=iCodigoApartamento,
                                                         sBloco:=oSqlDataReader.Item("codigo"),
                                                         iStatus:=iStatus,
                                                         iStatusUH:=iStatusUH,
                                                         iQuantidadeAtrasado:=iQuantidadeAtrasado,
                                                         iQuantidadePendente:=iQuantidadePendente,
                                                         iQuantidadeRealizada:=iQuantidadeRealizada)

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

    Public Function LoadUHAndarAtividade(ByVal iCodigoEmpresa As Integer,
                                         ByVal iCodigoUnidade As Integer,
                                         ByVal lCodigoAtividade As Long,
                                         ByVal iCodigoApartamento As Integer,
                                         ByVal sBloco As String,
                                         ByVal iStatus As Integer,
                                         ByVal iStatusUH As Integer,
                                         ByRef iQuantidadeAtrasado As Integer,
                                         ByRef iQuantidadePendente As Integer,
                                         ByRef iQuantidadeRealizada As Integer) As List(Of UHAtividadeAndar)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of UHAtividadeAndar)
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

            'Seta Parametros - Código Atividade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_atividade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoAtividade : i += 1

            'Seta Parametros - Bloco
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "bloco"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sBloco : i += 1

            'Seta Parametros - Código Apartamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_apartamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoApartamento : i += 1

            'Seta Parametros - Status
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "status"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iStatus : i += 1

            'Seta Parametros - Status UH
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "status_uh"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iStatusUH

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_uh_atividade_andar", oSqlParameter)

            While oSqlDataReader.Read

                Dim oReturnInfo As New UHAtividadeAndar

                oReturnInfo.codigo = oSqlDataReader.Item("codigo")
                oReturnInfo.descricao = oSqlDataReader.Item("descricao")
                oReturnInfo.uh = LoadUHAtividade(iCodigoEmpresa:=iCodigoEmpresa,
                                                 iCodigoUnidade:=iCodigoUnidade,
                                                 lCodigoAtividade:=lCodigoAtividade,
                                                 iCodigoApartamento:=iCodigoApartamento,
                                                 sBloco:=sBloco,
                                                 iAndar:=oSqlDataReader.Item("codigo"),
                                                 iStatus:=iStatus,
                                                 iStatusUH:=iStatusUH,
                                                 iQuantidadeAtrasado:=iQuantidadeAtrasado,
                                                 iQuantidadePendente:=iQuantidadePendente,
                                                 iQuantidadeRealizada:=iQuantidadeRealizada)

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

    Public Function LoadUHAtividade(ByVal iCodigoEmpresa As Integer,
                                    ByVal iCodigoUnidade As Integer,
                                    ByVal lCodigoAtividade As Long,
                                    ByVal iCodigoApartamento As Integer,
                                    ByVal sBloco As String,
                                    ByVal iAndar As Integer,
                                    ByVal iStatus As Integer,
                                    ByVal iStatusUH As Integer,
                                    ByRef iQuantidadeAtrasado As Integer,
                                    ByRef iQuantidadePendente As Integer,
                                    ByRef iQuantidadeRealizada As Integer) As List(Of UHAtividade)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of UHAtividade)
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

            'Seta Parametros - Código Atividade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_atividade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoAtividade : i += 1

            'Seta Parametros - Código Apartamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_apartamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoApartamento : i += 1

            'Seta Parametros - Status
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "status"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iStatus : i += 1

            'Seta Parametros - Bloco
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "bloco"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sBloco : i += 1

            'Seta Parametros - Andar
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "andar"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iAndar : i += 1

            'Seta Parametros - Status UH
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "status_uh"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iStatusUH

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_uh_atividade", oSqlParameter)

            While oSqlDataReader.Read

                Dim oReturnInfo As New UHAtividade

                oReturnInfo.codigo_apartamento = oSqlDataReader.Item("codigo_apartamento")
                oReturnInfo.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oReturnInfo.codigo_atividade = oSqlDataReader.Item("codigo_atividade")
                oReturnInfo.apartamento = oSqlDataReader.Item("apartamento")
                oReturnInfo.data_inicio = oSqlDataReader.Item("data_inicio")
                oReturnInfo.data_termino = oSqlDataReader.Item("data_termino")
                oReturnInfo.color = oSqlDataReader.Item("color")
                oReturnInfo.status = oSqlDataReader.Item("status")
                oReturnInfo.descricao_status = oSqlDataReader.Item("descricao_status")
                oReturnInfo.room_status = oSqlDataReader.Item("room_status")

                Select Case oSqlDataReader.Item("status")
                    Case 1 : iQuantidadePendente += 1
                    Case 2 : iQuantidadeRealizada += 1
                    Case 3 : iQuantidadeAtrasado += 1
                End Select

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

    Public Function LoadUHAtividadeApontamento(ByVal iCodigoEmpresa As Integer,
                                               ByVal iCodigoUnidade As Integer,
                                               ByVal lCodigoAtividade As Long,
                                               ByVal iCodigoApartamento As Integer) As UHAtividadeApontamento

        Try

            'Váriaveis Locais
            Dim oReturn As New UHAtividadeApontamento
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

            'Seta Parametros - Código Atividade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_atividade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoAtividade : i += 1

            'Seta Parametros - Código Apartamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_apartamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoApartamento 

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_uh_atividade_apontamento", oSqlParameter)

            While oSqlDataReader.Read

                oReturn.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oReturn.unidade = oSqlDataReader.Item("unidade")
                oReturn.codigo_atividade = oSqlDataReader.Item("codigo_atividade")
                oReturn.atividade = oSqlDataReader.Item("atividade")
                oReturn.descricao = oSqlDataReader.Item("descricao")
                oReturn.codigo_tipo_servico = oSqlDataReader.Item("codigo_tipo_servico")
                oReturn.categoria = oSqlDataReader.Item("categoria")
                oReturn.itens_gerais = oSqlDataReader.Item("itens_gerais")
                oReturn.apartamento = oSqlDataReader.Item("apartamento")
                oReturn.codigo_apartamento = oSqlDataReader.Item("codigo_apartamento")
                oReturn.data_inicio = oSqlDataReader.Item("data_inicio")
                oReturn.hora_inicio = oSqlDataReader.Item("hora_inicio")
                oReturn.data_termino = oSqlDataReader.Item("data_termino")
                oReturn.hora_termino = oSqlDataReader.Item("hora_termino")
                oReturn.observacao = oSqlDataReader.Item("observacao")
                oReturn.codigo_funcionario = oSqlDataReader.Item("codigo_funcionario")
                oReturn.codigo_fornecedor = oSqlDataReader.Item("codigo_fornecedor")
                oReturn.status_uh = oSqlDataReader.Item("status_uh")

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

    Public Sub DeleteUHAtividadeApontamento(ByVal iCodigoEmpresa As Integer,
                                            ByVal iCodigoUnidade As Integer,
                                            ByVal lCodigoAtividade As Long,
                                            ByVal iCodigoApartamento As Integer)

        Try

            'Váriaveis Locais
            Dim oReturn As New UHAtividadeApontamento
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

            'Seta Parametros - Código Atividade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_atividade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoAtividade : i += 1

            'Seta Parametros - Código Apartamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_apartamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoApartamento

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_uh_atividade_apontamento", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateUHAtividadeApartamento(ByVal iCodigoEmpresa As Integer,
                                            ByVal iCodigoUnidade As Integer,
                                            ByVal lCodigoAtividade As Long,
                                            ByVal iCodigoApartamento As Integer,
                                            ByVal sObservacao As String,
                                            ByVal sDataInicio As String,
                                            ByVal sHoraInicio As String,
                                            ByVal sDataTermino As String,
                                            ByVal sHoraTermino As String,
                                            ByVal sCodigoFuncionario As String,
                                            ByVal iCodigoFornecedor As Integer,
                                            ByVal iStatusUH As Integer)

        Try

            'Váriaveis Locais
            Dim oReturn As New UHAtividadeApontamento
            Dim oSqlParameter(11) As SqlParameter
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

            'Seta Parametros - Código Atividade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_atividade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoAtividade : i += 1

            'Seta Parametros - Código Apartamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_apartamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoApartamento : i += 1

            'Seta Parametros - Observação
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "observacao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 5000
            oSqlParameter(i).Value = sObservacao : i += 1

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
            oSqlParameter(i).Value = IIf(IsDate(sHoraInicio), sHoraInicio, DBNull.Value) : i += 1

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
            oSqlParameter(i).Value = IIf(IsDate(sHoraTermino), sHoraTermino, DBNull.Value) : i += 1

            'Seta Parametros - Código Funcionário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_funcionario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = IIf(sCodigoFuncionario = "", DBNull.Value, sCodigoFuncionario) : i += 1

            'Seta Parametros - Código Fornecedor
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_fornecedor"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoFornecedor = -1, DBNull.Value, iCodigoFornecedor) : i += 1

            'Seta Parametros - Status
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "status_uh"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iStatusUH

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_uh_atividade_apartamento", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region

#Region "::: DEDETIZAÇÃO :::"

    Public Function LoadDedetizacaoStatus(ByVal iCodigoEmpresa As Integer,
                                          ByVal iCodigoUnidade As Integer) As UHDedetizacaoStatus

        Try

            'Váriaveis Locais
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(1) As SqlParameter
            Dim oReturn As New UHDedetizacaoStatus
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
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_uh_dedetizacao_status", oSqlParameter)

            While oSqlDataReader.Read

                oReturn.quantidade_atrasado = oSqlDataReader.Item("quantidade_atrasado")
                oReturn.quantidade_pendente = oSqlDataReader.Item("quantidade_pendente")
                oReturn.quantidade_realizada = oSqlDataReader.Item("quantidade_realizada")

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Seta Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function LoadDedetizacao(ByVal iCodigoEmpresa As Integer,
                                    ByVal iCodigoUnidade As Integer,
                                    ByVal iCodigoUHAtividade As Integer,
                                    ByVal iCodigoUsuario As Integer,
                                    Optional ByVal iStatus As Integer = -1) As List(Of UHDedetizacaoList)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of UHDedetizacaoList)
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

            'Seta Parametros - Código UH Atividade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_uh_atividade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUHAtividade  : i += 1

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
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iStatus

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_uh_dedetizacao", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New UHDedetizacaoList

                oInfo.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oInfo.codigo_apartamento = oSqlDataReader.Item("codigo_apartamento")
                oInfo.codigo_uh_atividade = oSqlDataReader.Item("codigo_uh_atividade")
                oInfo.unidade = oSqlDataReader.Item("unidade")
                oInfo.bloco = oSqlDataReader.Item("bloco")
                oInfo.andar = oSqlDataReader.Item("andar")
                oInfo.apartamento = oSqlDataReader.Item("apartamento")
                oInfo.data = oSqlDataReader.Item("data")
                oInfo.data_proxima = oSqlDataReader.Item("data_proxima")
                oInfo.css_class = oSqlDataReader.Item("css_class")
                oInfo.icon = oSqlDataReader.Item("icon")
                oInfo.codigo = oSqlDataReader.Item("codigo")
                oInfo.status = oSqlDataReader.Item("status")
                oInfo.room_status = oSqlDataReader.Item("room_status")
                oInfo.front_office_status = oSqlDataReader.Item("front_office_status")

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

    'Public Function LoadDedetizacao(ByVal iCodigoEmpresa As Integer,
    '                                ByVal iCodigoUnidade As Integer,
    '                                ByVal iCodigoUHAtividade As Integer,
    '                                ByVal iCodigoUsuario As Integer,
    '                                Optional ByVal iStatus As Integer = -1) As List(Of UHDedetizacao)

    '    Try

    '        'Váriaveis Locais
    '        Dim oReturn As New List(Of UHDedetizacao)
    '        Dim oSqlDataReader As SqlDataReader
    '        Dim oSqlParameter(3) As SqlParameter
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

    '        'Seta Parametros - Código UH Atividade
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "codigo_uh_atividade"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.Int
    '        oSqlParameter(i).Value = iCodigoUHAtividade : i += 1

    '        'Seta Parametros - Código Usuário
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "codigo_usuario"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.Int
    '        oSqlParameter(i).Value = iCodigoUsuario

    '        'Executa Query
    '        oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_uh_dedetizacao_unidade", oSqlParameter)

    '        While oSqlDataReader.Read

    '            Dim oInfo As New UHDedetizacao

    '            oInfo.codigo_unidade = oSqlDataReader.Item("codigo")
    '            oInfo.unidade = oSqlDataReader.Item("nome_fantasia")
    '            oInfo.bloco = LoadUHBlocoDedetizacao(iCodigoEmpresa:=iCodigoEmpresa,
    '                                                 iCodigoUnidade:=oSqlDataReader.Item("codigo"),
    '                                                 iCodigoUHAtividade:=iCodigoUHAtividade,
    '                                                 iStatus:=iStatus)

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

    'Public Function LoadUHBlocoDedetizacao(ByVal iCodigoEmpresa As Integer,
    '                                       ByVal iCodigoUnidade As Integer,
    '                                       ByVal iCodigoUHAtividade As Integer,
    '                                       ByVal iStatus As Integer) As List(Of UHBlocoDedetizacao)

    '    Try

    '        'Váriaveis Locais
    '        Dim oReturn As New List(Of UHBlocoDedetizacao)
    '        Dim oSqlDataReader As SqlDataReader
    '        Dim oSqlParameter(2) As SqlParameter
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

    '        'Seta Parametros - Código UH Atividade
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "codigo_uh_atividade"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.Int
    '        oSqlParameter(i).Value = iCodigoUHAtividade

    '        'Executa Query
    '        oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_uh_dedetizacao_bloco", oSqlParameter)

    '        While oSqlDataReader.Read

    '            Dim oInfo As New UHBlocoDedetizacao

    '            oInfo.codigo = oSqlDataReader.Item("codigo")
    '            oInfo.descricao = oSqlDataReader.Item("descricao")
    '            oInfo.andar = LoadUHAndarDedetizacao(iCodigoEmpresa:=iCodigoEmpresa,
    '                                                 iCodigoUnidade:=iCodigoUnidade,
    '                                                 iCodigoUHAtividade:=iCodigoUHAtividade,
    '                                                 sBloco:=oSqlDataReader.Item("codigo"),
    '                                                 iStatus:=iStatus)

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

    'Public Function LoadUHAndarDedetizacao(ByVal iCodigoEmpresa As Integer,
    '                                       ByVal iCodigoUnidade As Integer,
    '                                       ByVal iCodigoUHAtividade As Integer,
    '                                       ByVal sBloco As String,
    '                                       ByVal iStatus As Integer) As List(Of UHAndarDedetizacao)

    '    Try

    '        'Váriaveis Locais
    '        Dim oReturn As New List(Of UHAndarDedetizacao)
    '        Dim oSqlDataReader As SqlDataReader
    '        Dim oSqlParameter(3) As SqlParameter
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

    '        'Seta Parametros - Código UH Atividade
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "codigo_uh_atividade"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.Int
    '        oSqlParameter(i).Value = iCodigoUHAtividade : i += 1

    '        'Seta Parametros - Código Bloco
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "bloco"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.VarChar
    '        oSqlParameter(i).Size = 50
    '        oSqlParameter(i).Value = sBloco

    '        'Executa Query
    '        oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_uh_dedetizacao_andar", oSqlParameter)

    '        While oSqlDataReader.Read

    '            Dim oInfo As New UHAndarDedetizacao

    '            oInfo.codigo = oSqlDataReader.Item("codigo")
    '            oInfo.descricao = oSqlDataReader.Item("descricao")
    '            oInfo.uh = LoadUHDedetizacao(iCodigoEmpresa:=iCodigoEmpresa,
    '                                         iCodigoUnidade:=iCodigoUnidade,
    '                                         iCodigoUHAtividade:=iCodigoUHAtividade,
    '                                         sBloco:=sBloco,
    '                                         iCodigoAndar:=oSqlDataReader.Item("codigo"),
    '                                         iStatus:=iStatus)

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

    'Public Function LoadUHDedetizacao(ByVal iCodigoEmpresa As Integer,
    '                                  ByVal iCodigoUnidade As Integer,
    '                                  ByVal iCodigoUHAtividade As Integer,
    '                                  ByVal sBloco As String,
    '                                  ByVal iCodigoAndar As Integer,
    '                                  ByVal iStatus As Integer) As List(Of UHDedetizacaoApartamento)

    '    Try

    '        'Váriaveis Locais
    '        Dim oReturn As New List(Of UHDedetizacaoApartamento)
    '        Dim oSqlDataReader As SqlDataReader
    '        Dim oSqlParameter(5) As SqlParameter
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

    '        'Seta Parametros - Código UH Atividade
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "codigo_uh_atividade"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.Int
    '        oSqlParameter(i).Value = iCodigoUHAtividade : i += 1

    '        'Seta Parametros - Código Bloco
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "bloco"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.VarChar
    '        oSqlParameter(i).Size = 50
    '        oSqlParameter(i).Value = sBloco : i += 1

    '        'Seta Parametros - Código Andar
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "andar"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.Int
    '        oSqlParameter(i).Value = iCodigoAndar : i += 1

    '        'Seta Parametros - Status
    '        oSqlParameter(i) = New SqlParameter
    '        oSqlParameter(i).ParameterName = "status"
    '        oSqlParameter(i).Direction = ParameterDirection.Input
    '        oSqlParameter(i).SqlDbType = SqlDbType.Int
    '        oSqlParameter(i).Value = iStatus

    '        'Executa Query
    '        oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_uh_dedetizacao_apartamento", oSqlParameter)

    '        While oSqlDataReader.Read

    '            Dim oInfo As New UHDedetizacaoApartamento

    '            oInfo.codigo_uh_atividade = iCodigoUHAtividade
    '            oInfo.codigo_apartamento = oSqlDataReader.Item("codigo_apartamento")
    '            oInfo.codigo = oSqlDataReader.Item("codigo")
    '            oInfo.apartamento = oSqlDataReader.Item("apartamento")
    '            oInfo.data = oSqlDataReader.Item("data")
    '            oInfo.color = oSqlDataReader.Item("color")
    '            oInfo.icon = oSqlDataReader.Item("icon")
    '            oInfo.status = oSqlDataReader.Item("status")
    '            oInfo.room_status = oSqlDataReader.Item("room_status")
    '            oInfo.front_office_status = oSqlDataReader.Item("front_office_status")

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

    Public Function LoadUHDedetizacaoApontamento(ByVal iCodigoEmpresa As Integer,
                                                 ByVal iCodigoUnidade As Integer,
                                                 ByVal iCodigoUHAtividade As Integer,
                                                 ByVal iCodigoApartamento As Integer,
                                                 ByVal lCodigo As Long) As UHDedetizacaoApontamento

        Try

            'Váriaveis Locais
            Dim oReturn As New UHDedetizacaoApontamento
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

            'Seta Parametros - Código UH Atividade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_uh_atividade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUHAtividade : i += 1

            'Seta Parametros - Código Apartamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_apartamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoApartamento : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_uh_dedetizacao_apontamento", oSqlParameter)

            While oSqlDataReader.Read

                oReturn.codigo_uh_atividade = oSqlDataReader.Item("codigo_uh_atividade")
                oReturn.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oReturn.codigo_tipo_servico = oSqlDataReader.Item("codigo_tipo_servico")
                oReturn.codigo_apartamento = oSqlDataReader.Item("codigo_apartamento")
                oReturn.codigo = oSqlDataReader.Item("codigo")
                oReturn.icon = oSqlDataReader.Item("icon")
                oReturn.uh_local = oSqlDataReader.Item("uh_local")
                oReturn.unidade = oSqlDataReader.Item("unidade")
                oReturn.descricao = oSqlDataReader.Item("descricao")
                oReturn.codigo_funcionario = oSqlDataReader.Item("codigo_funcionario")
                oReturn.codigo_fornecedor = oSqlDataReader.Item("codigo_fornecedor")
                oReturn.data = oSqlDataReader.Item("data")
                oReturn.observacao = oSqlDataReader.Item("observacao")

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

    Public Function LoadUHAtividade(ByVal iCodigoEmpresa As Integer,
                                    ByVal iCodigoUnidade As Integer,
                                    ByVal iCodigoUHAtividade As Integer) As UHAtividadeInfo

        Try

            'Váriaveis Locais
            Dim oReturn As New UHAtividadeInfo
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

            'Seta Parametros - Código UH Atividade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_uh_atividade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUHAtividade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_uh_atividade_info", oSqlParameter)

            While oSqlDataReader.Read

                oReturn.codigo_uh_atividade = oSqlDataReader.Item("codigo_uh_atividade")
                oReturn.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oReturn.codigo_tipo_servico = oSqlDataReader.Item("codigo_tipo_servico")
                oReturn.icon = oSqlDataReader.Item("icon")
                oReturn.unidade = oSqlDataReader.Item("unidade")
                oReturn.descricao = oSqlDataReader.Item("descricao")

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

    Public Sub UHDedetizacaoApontamento(ByVal iCodigoEmpresa As Integer,
                                        ByVal iCodigoUnidade As Integer,
                                        ByVal iCodigoUHAtividade As Integer,
                                        ByVal lCodigo As Long,
                                        ByVal iCodigoApartamento As Integer,
                                        ByVal iCodigoFuncionario As Integer,
                                        ByVal iCodigoFornecedor As Integer,
                                        ByVal sData As String,
                                        ByVal sObservacao As String,
                                        ByVal iCodigoUsuario As Integer)

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

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código UH Atividade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_uh_atividade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUHAtividade : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigo : i += 1

            'Seta Parametros - Código Apartamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_apartamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoApartamento : i += 1

            'Seta Parametros - Código Funcionário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_funcionario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoFuncionario = -1, DBNull.Value, iCodigoFuncionario) : i += 1

            'Seta Parametros - Código Fornecedor
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_fornecedor"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoFornecedor = -1, DBNull.Value, iCodigoFornecedor) : i += 1

            'Seta Parametros - Data
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sData : i += 1

            'Seta Parametros - Observação
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "observacao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = sObservacao.ToUpper : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_uh_dedetizacao_apontamento", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region

#Region "::: DEDETIZAÇÃO - HISTÓRICO :::"

    Public Function LoadDedetizacaoHistorico(ByVal codigoEmpresa As Integer,
                                             ByVal codigoUnidade As Integer,
                                             ByVal dataInicio As String,
                                             ByVal dataTermino As String,
                                             ByVal codigoApartamento As Integer) As List(Of UHDedetizacaoHistorico)

        Try


            Dim oReturn As New List(Of UHDedetizacaoHistorico)

            Dim oParameters As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("data_inicio", SqlDbType.Date, IIf(IsDate(dataInicio), dataInicio, DBNull.Value)),
                CriarParametro("data_termino", SqlDbType.Date, IIf(IsDate(dataTermino), dataTermino, DBNull.Value)),
                CriarParametro("codigo_apartamento", SqlDbType.Int, codigoApartamento)
            }

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_uh_dedetizacao_historico", oParameters)

                While oSqlDataReader.Read

                    Dim oInfo As New UHDedetizacaoHistorico With {
                        .codigo = SafeGetLong(oSqlDataReader, "codigo"),
                        .codigoEmpresa = SafeGetLong(oSqlDataReader, "codigo_empresa"),
                        .codigoUnidade = SafeGetLong(oSqlDataReader, "codigo_unidade"),
                        .codigoUHAtividade = SafeGetLong(oSqlDataReader, "codigo_uh_atividade"),
                        .unidade = SafeGetString(oSqlDataReader, "unidade"),
                        .data = SafeGetDate(oSqlDataReader, "data"),
                        .apartamento = SafeGetString(oSqlDataReader, "apartamento"),
                        .colaborador = SafeGetString(oSqlDataReader, "colaborador"),
                        .observacao = SafeGetString(oSqlDataReader, "observacao")
                    }

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

    Public Sub DeleteDedetizacao(ByVal codigoEmpresa As Integer,
                                 ByVal codigoUnidade As Integer,
                                 ByVal codigoUHAtividade As Integer,
                                 ByVal codigo As Long,
                                 ByVal codigoUsuario As Integer)

        Try

            Dim oParameters As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("codigo_uh_atividade", SqlDbType.Int, codigoUHAtividade),
                CriarParametro("codigo", SqlDbType.BigInt, codigo),
                CriarParametro("codigo_usuario", SqlDbType.Int, codigoUsuario)
            }

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_uh_dedetizacao", oParameters)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region

#Region "::: MAPA DE MANUTENÇÃO - HISTÓRICO :::"

    Public Function LoadMapaManutencaoHistorico(ByVal codigoEmpresa As Integer,
                                                ByVal codigoUnidade As Integer,
                                                ByVal codigoAtividade As Integer,
                                                ByVal dataInicio As String,
                                                ByVal dataTermino As String,
                                                ByVal codigoApartamento As Integer) As List(Of UHMapaManutencaoHistorico)

        Try


            Dim oReturn As New List(Of UHMapaManutencaoHistorico)

            Dim oParameters As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("codigo_atividade", SqlDbType.Int, codigoAtividade),
                CriarParametro("data_inicio", SqlDbType.Date, IIf(IsDate(dataInicio), dataInicio, DBNull.Value)),
                CriarParametro("data_termino", SqlDbType.Date, IIf(IsDate(dataTermino), dataTermino, DBNull.Value)),
                CriarParametro("codigo_apartamento", SqlDbType.Int, codigoApartamento)
            }

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_uh_mapa_manutencao_historico", oParameters)

                While oSqlDataReader.Read

                    Dim oInfo As New UHMapaManutencaoHistorico With {
                        .codigo = SafeGetLong(oSqlDataReader, "codigo"),
                        .codigoEmpresa = SafeGetLong(oSqlDataReader, "codigo_empresa"),
                        .codigoUnidade = SafeGetLong(oSqlDataReader, "codigo_unidade"),
                        .codigoApartamento = SafeGetLong(oSqlDataReader, "codigo_apartamento"),
                        .unidade = SafeGetString(oSqlDataReader, "unidade"),
                        .data = SafeGetDate(oSqlDataReader, "data"),
                        .dataPrevisaoTermino = SafeGetDate(oSqlDataReader, "data_previsao_termino"),
                        .descricao = SafeGetString(oSqlDataReader, "descricao"),
                        .apartamento = SafeGetString(oSqlDataReader, "apartamento"),
                        .colaborador = SafeGetString(oSqlDataReader, "colaborador"),
                        .observacao = SafeGetString(oSqlDataReader, "observacao")
                    }

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

    Public Sub DeleteMapaManutencao(ByVal codigoEmpresa As Integer,
                                    ByVal codigoUnidade As Integer,
                                    ByVal codigoApartamento As Integer,
                                    ByVal codigo As Long,
                                    ByVal codigoUsuario As Integer)

        Try

            Dim oParameters As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("codigo_apartamento", SqlDbType.Int, codigoApartamento),
                CriarParametro("codigo", SqlDbType.BigInt, codigo),
                CriarParametro("codigo_usuario", SqlDbType.Int, codigoUsuario)
            }

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_uh_mapa_manutencao", oParameters)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region

End Class
