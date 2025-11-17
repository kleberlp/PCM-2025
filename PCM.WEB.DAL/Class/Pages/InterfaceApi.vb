Imports System.Data.SqlClient
Imports PCM.WEB.DAL.SQLHelper
Imports PCM.WEB.MODELS
Imports PCM.WEB.MODELS.Models

Public Class InterfaceApi

    Private sConnection As String

    Sub New(ByVal sCon As String)
        sConnection = sCon
    End Sub

#Region "::: LOGIN :::"

    Public Function Authentication(ByVal sUsername As String,
                                   ByVal sPassword As String) As InterfaceUserInfo

        'Variaveis Locais
        Dim oSqlParameter(1) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New InterfaceUserInfo
        Dim i As Integer = 0

        Try

            'Seta Parametros - Username
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "username"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 150
            oSqlParameter(i).Value = sUsername : i += 1

            'Seta Parametros - Password
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "password"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = Cripitografar(sPassword.ToUpper())

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_interface_login", oSqlParameter)

            If oSqlDataReader.HasRows Then

                While oSqlDataReader.Read

                    oReturn.name = oSqlDataReader.Item("nome")
                    oReturn.codigoEmpresa = oSqlDataReader.Item("codigo_empresa")
                    oReturn.success = True

                End While

            Else

                oReturn.success = False

            End If

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: GREEN PLANET :::"

    Public Function GreenPlanet(ByVal iCodigoEmpresa As Integer,
                                ByVal codigoUnidade As Integer,
                                ByVal sItemMedicao As String,
                                ByVal dStartDate As String,
                                ByVal dEndDate As String) As List(Of InterfaceGreenPlanet)

        'Variaveis Locais
        Dim oSqlParameter(4) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New List(Of InterfaceGreenPlanet)
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Hotel Id
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = codigoUnidade : i += 1

            'Seta Parametros - Item Medição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "item_medicao"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sItemMedicao : i += 1

            'Seta Parametros - Data Início
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "start_date"
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = CDate(dStartDate) : i += 1

            'Seta Parametros - Data Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "end_date"
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = CDate(dEndDate)

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_interface_green_planet", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New InterfaceGreenPlanet

                oInfo.id = oSqlDataReader.Item("id")
                oInfo.unidade = oSqlDataReader.Item("unidade")
                oInfo.data = oSqlDataReader.Item("data")
                oInfo.codigoItemMedicao = oSqlDataReader.Item("codigo_item_medicao")
                oInfo.itemMedicao = oSqlDataReader.Item("item_medicao")
                oInfo.numeroHospedes = oSqlDataReader.Item("numero_hospedes")
                oInfo.UHOcupada = oSqlDataReader.Item("uh_ocupada")
                oInfo.medicao = oSqlDataReader.Item("medicao")
                oInfo.consumo = oSqlDataReader.Item("consumo")

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

#End Region

#Region "::: RESUMO UNIDADES :::"


    Public Function ResumoUnidades(ByVal iCodigoEmpresa As Integer) As List(Of resumoUnidades)


        'Variaveis Locais
        Dim oSqlParameter(0) As SqlParameter
        Dim oReturn As New List(Of resumoUnidades)
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_dashboard_main", oSqlParameter)

                While oSqlDataReader.Read

                    Dim oInfo As New resumoUnidades

                    oInfo.unidade = SafeGetString(oSqlDataReader, "unidade")
                    oInfo.quantidadeOSGerada = SafeGetLong(oSqlDataReader, "quantidade_os_gerada")
                    oInfo.quantidadeOSAtendida = SafeGetLong(oSqlDataReader, "quantidade_os_atendida")
                    oInfo.quantidadeOSPendente = SafeGetLong(oSqlDataReader, "quantidade_os_pendente")
                    oInfo.laudo = SafeGetFloat(oSqlDataReader, "laudo") / 100.0
                    oInfo.preventiva = SafeGetFloat(oSqlDataReader, "preventiva") / 100.0
                    oInfo.rotina = SafeGetFloat(oSqlDataReader, "rotina") / 100.0
                    oInfo.pmoc = SafeGetFloat(oSqlDataReader, "pmoc") / 100.0
                    oInfo.uhDia = SafeGetFloat(oSqlDataReader, "uh_dia") / 100.0
                    oInfo.greenPlanet = SafeGetFloat(oSqlDataReader, "green_planet") / 100.0

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

#Region "::: PRINCIPAIS OCORRÊNCIAS :::"

    Public Function PrincipaisOcorrencias(ByVal codigoEmpresa As Integer,
                                          ByVal hotelId As String,
                                          ByVal codigoModulo As Integer,
                                          ByVal startDate As String,
                                          ByVal endDate As String) As List(Of interfacePrincipaisOcorrencias)


        'Variaveis Locais
        Dim oReturn As New List(Of interfacePrincipaisOcorrencias)
        Dim i As Integer = 0

        Try

            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("hotelId", SqlDbType.VarChar, hotelId),
                CriarParametro("codigo_modulo", SqlDbType.Int, codigoModulo),
                CriarParametro("startDate", SqlDbType.Date, startDate),
                CriarParametro("endDate", SqlDbType.Date, endDate)
            }

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_interface_principais_ocorrencias", oSqlParameter)

                While oSqlDataReader.Read

                    Dim oInfo As New interfacePrincipaisOcorrencias

                    oInfo.ranking = SafeGetString(oSqlDataReader, "item")
                    oInfo.descricao = SafeGetLong(oSqlDataReader, "local")
                    oInfo.quantidade = SafeGetLong(oSqlDataReader, "quantidade")

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

#Region "::: ORDEM DE SERVIÇO :::"

    Public Function OrdemServico(ByVal codigoEmpresa As Integer,
                                 ByVal codigoUnidade As Integer,
                                 ByVal dataAberturaOSInicio As String,
                                 ByVal dataAberturaOSTermino As String,
                                 ByVal dataConclusaoOSInicio As String,
                                 ByVal dataConclusaoOSTermino As String,
                                 ByVal status As String,
                                 ByVal page As Integer) As interfaceOrdemServico


        'Variaveis Locais
        Dim oDetails As New List(Of interfaceOrdemServicoDetails)
        Dim oReturn As New interfaceOrdemServico

        Try

            Dim culture = Globalization.CultureInfo.GetCultureInfo("pt-BR")

            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("data_abertura_os_inicio", SqlDbType.Date, If(dataAberturaOSInicio Is Nothing, DBNull.Value, CType(DateTime.ParseExact(dataAberturaOSInicio.Trim(), "dd/MM/yyyy", culture), Object))),
                CriarParametro("data_abertura_os_termino", SqlDbType.Date, If(dataAberturaOSTermino Is Nothing, DBNull.Value, CType(DateTime.ParseExact(dataAberturaOSTermino.Trim(), "dd/MM/yyyy", culture), Object))),
                CriarParametro("data_conclusao_os_inicio", SqlDbType.Date, If(dataConclusaoOSInicio Is Nothing, DBNull.Value, CType(DateTime.ParseExact(dataConclusaoOSInicio.Trim(), "dd/MM/yyyy", culture), Object))),
                CriarParametro("data_conclusao_os_termino", SqlDbType.Date, If(dataConclusaoOSTermino Is Nothing, DBNull.Value, CType(DateTime.ParseExact(dataConclusaoOSTermino.Trim(), "dd/MM/yyyy", culture), Object))),
                CriarParametro("status", SqlDbType.VarChar, status),
                CriarParametro("page", SqlDbType.SmallInt, page)
            }

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_interface_ordem_servico", oSqlParameter)

                While oSqlDataReader.Read

                    Dim oInfo As New interfaceOrdemServicoDetails With {
                        .unidade = SafeGetString(oSqlDataReader, "unidade"),
                        .categoria = SafeGetString(oSqlDataReader, "categoria"),
                        .numeroOrdemServico = SafeGetString(oSqlDataReader, "numero_ordem_servico"),
                        .dataAbertura = SafeGetString(oSqlDataReader, "data_abertura"),
                        .setor = SafeGetString(oSqlDataReader, "setor"),
                        .local = SafeGetString(oSqlDataReader, "local"),
                        .equipamento = SafeGetString(oSqlDataReader, "equipamento"),
                        .prioridade = SafeGetString(oSqlDataReader, "prioridade"),
                        .tipoServico = SafeGetString(oSqlDataReader, "tipo_servico"),
                        .descricao = SafeGetString(oSqlDataReader, "descricao"),
                        .status = SafeGetString(oSqlDataReader, "status"),
                        .solicitante = SafeGetString(oSqlDataReader, "solicitante"),
                        .prazoExecucao = SafeGetString(oSqlDataReader, "prazo_execucao"),
                        .executor = SafeGetString(oSqlDataReader, "executor"),
                        .departamento = SafeGetString(oSqlDataReader, "departamento"),
                        .justificativaApontamento = SafeGetString(oSqlDataReader, "justificativa_apontamento"),
                        .dataVinculo = SafeGetString(oSqlDataReader, "data_vinculo"),
                        .prazoExecucaoInicial = SafeGetString(oSqlDataReader, "prazo_execucao_inicial"),
                        .justificativaAlteracaoPrazo = SafeGetString(oSqlDataReader, "justificativa_alteracao_prazo"),
                        .usuarioAlteracao = SafeGetString(oSqlDataReader, "usuario_alteracao"),
                        .dataAlteracao = SafeGetString(oSqlDataReader, "data_alteracao"),
                        .justificativaCancelamento = SafeGetString(oSqlDataReader, "justificativa_cancelamento"),
                        .dataTermino = SafeGetString(oSqlDataReader, "data_termino")
                    }

                    oDetails.Add(oInfo)

                End While

                oSqlDataReader.NextResult()

                While oSqlDataReader.Read

                    With oReturn
                        .page = page
                        .totalPage = SafeGetLong(oSqlDataReader, "totalPage")
                        .totalRegistros = SafeGetLong(oSqlDataReader, "totalRegistro")
                        .os = oDetails
                    End With

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

#Region "::: ROTINA :::"

    Public Function Rotina(ByVal codigoEmpresa As Integer,
                           ByVal codigoUnidade As Integer,
                           ByVal dataInicio As String,
                           ByVal dataTermino As String,
                           ByVal page As Integer) As interfaceRotina


        'Variaveis Locais
        Dim oDetails As New List(Of interfaceRotinaDetails)
        Dim oReturn As New interfaceRotina
        Dim grupos As New Dictionary(Of String, interfaceRotinaDetails)(StringComparer.OrdinalIgnoreCase)

        Try

            Dim culture = Globalization.CultureInfo.GetCultureInfo("pt-BR")

            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("data_inicio", SqlDbType.Date, If(dataInicio Is Nothing, DBNull.Value, CType(DateTime.ParseExact(dataInicio.Trim(), "dd/MM/yyyy", culture), Object))),
                CriarParametro("data_termino", SqlDbType.Date, If(dataTermino Is Nothing, DBNull.Value, CType(DateTime.ParseExact(dataTermino.Trim(), "dd/MM/yyyy", culture), Object))),
                CriarParametro("page", SqlDbType.SmallInt, page)
            }

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_interface_rotina", oSqlParameter)

                oReturn.rotina = New List(Of interfaceRotinaDetails)

                While oSqlDataReader.Read()

                    Dim codigo As Long = SafeGetLong(oSqlDataReader, "codigo")
                    Dim unidade As String = SafeGetString(oSqlDataReader, "unidade")
                    Dim setor As String = SafeGetString(oSqlDataReader, "setor")
                    Dim descricaoRotina As String = SafeGetString(oSqlDataReader, "rotina")
                    Dim categoria As String = SafeGetString(oSqlDataReader, "categoria")
                    Dim tipoServico As String = SafeGetString(oSqlDataReader, "tipo_servico")

                    ' Chave do grupo
                    Dim key As String = $"{codigo}"

                    ' Cria o grupo se ainda não existir
                    Dim detalhe As interfaceRotinaDetails = Nothing
                    If Not grupos.TryGetValue(key, detalhe) Then
                        detalhe = New interfaceRotinaDetails With {
                            .codigo = codigo,
                            .unidade = unidade,
                            .setor = setor,
                            .rotina = descricaoRotina,
                            .categoria = categoria,
                            .tipoServico = tipoServico,
                            .apontamentos = New List(Of interfaceRotinaDetailsExecutor)
                        }
                        grupos.Add(key, detalhe)
                    End If

                    detalhe.apontamentos = New List(Of interfaceRotinaDetailsExecutor)

                    'Só adiciona se houver algum dado relevante (opcional)
                    detalhe.apontamentos.Add(New interfaceRotinaDetailsExecutor With {
                        .nome = SafeGetString(oSqlDataReader, "nome"),
                        .inicio = SafeGetString(oSqlDataReader, "data_inicio"),
                        .termino = SafeGetString(oSqlDataReader, "data_termino"),
                        .justificativa = SafeGetString(oSqlDataReader, "justificativa")})

                    oReturn.rotina.Add(detalhe)

                End While

                oSqlDataReader.NextResult()

                While oSqlDataReader.Read

                    With oReturn
                        .page = page
                        .totalPage = SafeGetLong(oSqlDataReader, "totalPage")
                        .totalRegistros = SafeGetLong(oSqlDataReader, "totalRegistro")
                    End With

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

End Class
