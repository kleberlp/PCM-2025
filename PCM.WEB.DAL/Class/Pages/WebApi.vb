Imports PCM.WEB.MODELS
Imports System.DirectoryServices
Imports System.DirectoryServices.ActiveDirectory
Imports PCM.WEB.DAL.SQLHelper
Imports System.Data.SqlClient
Imports System.Text
Imports System.IO

Public Class WebApi

    Private sConnection As String

    Sub New(ByVal sCon As String)
        sConnection = sCon
    End Sub

#Region "::: LOGIN :::"

    Public Function Perfil(ByVal sEmail As String) As List(Of String)

        'Variaveis Locais
        Dim oSqlParameter(0) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New List(Of String)
        Dim i As Integer = 0

        Try

            'Seta Parametros - E-mail
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "email"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 150
            oSqlParameter(i).Value = sEmail

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_api_validate_login_perfil", oSqlParameter)

            While oSqlDataReader.Read

                'Seta Retorno da Função
                oReturn.Add(oSqlDataReader.Item("codigo"))

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function Login(ByVal sEmail As String,
                          ByVal sSenha As String) As ApiLogin

        'Variaveis Locais
        Dim oSqlParameter(1) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New ApiLogin
        Dim i As Integer = 0

        Try

            'Seta Retorno da Função
            oReturn.message = 0

            'Seta Parametros - E-mail
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "email"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 150
            oSqlParameter(i).Value = sEmail : i += 1

            'Seta Parametros - Senha
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "senha"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = Cripitografar(sSenha.ToUpper)

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_api_validate_login", oSqlParameter)

            While oSqlDataReader.Read

                'Seta Retorno da Função
                oReturn.message = 1
                oReturn.codigo_empresa = oSqlDataReader.Item("codigo_empresa")
                oReturn.unidade = oSqlDataReader.Item("unidade")
                oReturn.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oReturn.codigo_usuario = oSqlDataReader.Item("codigo_usuario")
                oReturn.codigo_funcionario = oSqlDataReader.Item("codigo_funcionario")
                oReturn.nome = oSqlDataReader.Item("nome")
                oReturn.ativo = IIf(oSqlDataReader.Item("ativo"), 1, 0)
                oReturn.perfil = Perfil(sEmail:=sEmail)

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function


#End Region

#Region "::: CADASTRO BÁSICO :::"

    Public Function Unidade(ByVal iCodigoEmpresa As Integer) As List(Of APIUnidade)

        'Variaveis Locais
        Dim oSqlParameter(1) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New List(Of APIUnidade)
        Dim oInfo As APIUnidade
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_api_select_cadastro_basico_unidade", oSqlParameter)

            While oSqlDataReader.Read

                'Seta Retorno da Função
                oInfo = New APIUnidade

                oInfo.codigo = oSqlDataReader.Item("codigo")
                oInfo.descricao = oSqlDataReader.Item("descricao")
                oInfo.image = oSqlDataReader.Item("image")

                oReturn.Add(oInfo)

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function UnidadeUsuario(ByVal iCodigoEmpresa As Integer,
                                   ByVal iCodigoUsuario As Integer) As List(Of APIUnidade)

        'Variaveis Locais
        Dim oSqlParameter(1) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New List(Of APIUnidade)
        Dim oInfo As APIUnidade
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_api_select_cadastro_basico_unidade_usuario", oSqlParameter)

            While oSqlDataReader.Read

                'Seta Retorno da Função
                oInfo = New APIUnidade

                oInfo.codigo = oSqlDataReader.Item("codigo")
                oInfo.descricao = oSqlDataReader.Item("descricao")
                oInfo.image = oSqlDataReader.Item("image")

                oReturn.Add(oInfo)

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function combo(ByVal sStorageProdedure As String) As List(Of APIComboBox)

        'Variaveis Locais
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New List(Of APIComboBox)
        Dim oInfo As APIComboBox
        Dim i As Integer = 0

        Try

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.Text, sStorageProdedure)

            While oSqlDataReader.Read

                'Seta Retorno da Função
                oInfo = New APIComboBox

                oInfo.codigo = oSqlDataReader.Item("codigo")
                oInfo.descricao = oSqlDataReader.Item("descricao")

                oReturn.Add(oInfo)

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function combo(ByVal iCodigoEmpresa As Integer,
                          ByVal iCodigoUnidade As Integer,
                          ByVal sQuery As String) As List(Of APIComboBox)

        'Variaveis Locais
        Dim oSqlParameter(2) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New List(Of APIComboBox)
        Dim oInfo As APIComboBox
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, sQuery, oSqlParameter)

            While oSqlDataReader.Read

                'Seta Retorno da Função
                oInfo = New APIComboBox

                oInfo.codigo = oSqlDataReader.Item("codigo")
                oInfo.descricao = oSqlDataReader.Item("descricao")

                oReturn.Add(oInfo)

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function combo1(ByVal iCodigoEmpresa As Integer,
                           ByVal iCodigoUnidade As Integer,
                           ByVal iCodigo1 As Integer,
                           ByVal sQuery As String) As List(Of APIComboBox)

        'Variaveis Locais
        Dim oSqlParameter(2) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New List(Of APIComboBox)
        Dim oInfo As APIComboBox
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código 1
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo1"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo1


            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, sQuery, oSqlParameter)

            While oSqlDataReader.Read

                'Seta Retorno da Função
                oInfo = New APIComboBox

                oInfo.codigo = oSqlDataReader.Item("codigo")
                oInfo.descricao = oSqlDataReader.Item("descricao")

                oReturn.Add(oInfo)

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function combo2(ByVal iCodigoEmpresa As Integer,
                           ByVal iCodigoUnidade As Integer,
                           ByVal iCodigo1 As Integer,
                           ByVal iCodigo2 As Integer,
                           ByVal sQuery As String) As List(Of APIComboBox)

        'Variaveis Locais
        Dim oSqlParameter(3) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New List(Of APIComboBox)
        Dim oInfo As APIComboBox
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código 1
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo1"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo1 : i += 1

            'Seta Parametros - Código 2
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo2"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo2


            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, sQuery, oSqlParameter)

            While oSqlDataReader.Read

                'Seta Retorno da Função
                oInfo = New APIComboBox

                oInfo.codigo = oSqlDataReader.Item("codigo")
                oInfo.descricao = oSqlDataReader.Item("descricao")

                oReturn.Add(oInfo)

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function combo3(ByVal iCodigoEmpresa As Integer,
                           ByVal iCodigoUnidade As Integer,
                           ByVal iCodigo1 As Integer,
                           ByVal iCodigo2 As Integer,
                           ByVal iCodigo3 As Integer,
                           ByVal sQuery As String) As List(Of APIComboBox)

        'Variaveis Locais
        Dim oSqlParameter(4) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New List(Of APIComboBox)
        Dim oInfo As APIComboBox
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código 1
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo1"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo1 : i += 1

            'Seta Parametros - Código 2
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo2"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo2 : i += 1

            'Seta Parametros - Código 3
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo3"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo3


            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, sQuery, oSqlParameter)

            While oSqlDataReader.Read

                'Seta Retorno da Função
                oInfo = New APIComboBox

                oInfo.codigo = oSqlDataReader.Item("codigo")
                oInfo.descricao = oSqlDataReader.Item("descricao")

                oReturn.Add(oInfo)

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: PMOC :::"

    Public Function getPMOC(ByVal iCodigoEmpresa As Integer,
                            ByVal iCodigoUnidade As Integer,
                            ByVal iIntervalo As Integer) As APIPMOC

        Dim oReturn As New APIPMOC

        Try

            oReturn.results = PMOCList(iCodigoEmpresa:=iCodigoEmpresa,
                                       iCodigoUnidade:=iCodigoUnidade,
                                       iIntervalo:=iIntervalo)

            oReturn.page = 1
            oReturn.total_results = oReturn.results.Count
            oReturn.total_pages = 1

            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function PMOCList(ByVal iCodigoEmpresa As Integer,
                             ByVal iCodigoUnidade As Integer,
                             ByVal iIntervalo As Integer) As List(Of ApiPMOCList)

        'Variaveis Locais
        Dim oSqlParameter(2) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New List(Of ApiPMOCList)
        Dim oInfo As ApiPMOCList
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Intervalo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "intervalo"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iIntervalo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_api_select_pmoc_list", oSqlParameter)

            While oSqlDataReader.Read

                oInfo = New ApiPMOCList

                'Seta Retorno da Função
                oInfo.codigo_empresa = oSqlDataReader("codigo_empresa")
                oInfo.unidade = oSqlDataReader("unidade")
                oInfo.codigo_unidade = oSqlDataReader("codigo_unidade")
                oInfo.codigo_equipamento = oSqlDataReader("codigo_equipamento")
                oInfo.tag = oSqlDataReader("tag")
                oInfo.descricao = oSqlDataReader("descricao")
                oInfo.local = oSqlDataReader("local")
                oInfo.codigo_tipo_ar_condicionado = oSqlDataReader("codigo_tipo_ar_condicionado")
                oInfo.tipo_ar_condicionado = oSqlDataReader("tipo_ar_condicionado")
                oInfo.data_proxima_manutencao = oSqlDataReader("data_proxima_manutencao")
                oInfo.data_ultima_manutencao = oSqlDataReader("data_ultima_manutencao")
                oInfo.status = oSqlDataReader("status")
                oInfo.descricao_status = oSqlDataReader("descricao_status")
                oInfo.codigo_checklist = oSqlDataReader("codigo_checklist")
                oInfo.intervalo = oSqlDataReader("intervalo")

                oReturn.Add(oInfo)

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function getPMOCChecklist(ByVal iCodigoEmpresa As Integer,
                                     ByVal iCodigoUnidade As Integer,
                                     ByVal lCodigoEquipamento As Long,
                                     ByVal sDataManutencao As String) As APIPMOCChecklist

        Dim oReturn As New APIPMOCChecklist

        Try

            oReturn.results = PMOCChecklistList(iCodigoEmpresa:=iCodigoEmpresa,
                                                iCodigoUnidade:=iCodigoUnidade,
                                                lCodigoEquipamento:=lCodigoEquipamento,
                                                sDataManutencao:=sDataManutencao)

            oReturn.page = 1
            oReturn.total_results = oReturn.results.Count
            oReturn.total_pages = 1

            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function PMOCChecklistList(ByVal iCodigoEmpresa As Integer,
                                      ByVal iCodigoUnidade As Integer,
                                      ByVal lCodigoEquipamento As Long,
                                      ByVal sDataManutencao As String) As List(Of ApiPMOCChecklistList)

        'Variaveis Locais
        Dim oSqlParameter(3) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New List(Of ApiPMOCChecklistList)
        Dim oInfo As ApiPMOCChecklistList
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_equipamento"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoEquipamento : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "data_manutencao"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sDataManutencao

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_api_select_pmoc_checklist_list", oSqlParameter)

            While oSqlDataReader.Read

                oInfo = New ApiPMOCChecklistList

                'Seta Retorno da Função
                oInfo.codigo_empresa = oSqlDataReader.Item("codigo_empresa")
                oInfo.codigo_tipo_ar_condicionado = oSqlDataReader.Item("codigo_tipo_ar_condicionado")
                oInfo.codigo = oSqlDataReader.Item("codigo")
                oInfo.grupo_checklist = oSqlDataReader.Item("grupo_checklist")
                oInfo.codigo_checklist = oSqlDataReader.Item("codigo_checklist")
                oInfo.descricao = oSqlDataReader.Item("descricao")
                oInfo.codigo_tipo_item_checklist = oSqlDataReader.Item("codigo_tipo_item_checklist")
                oInfo.valor_minimo = oSqlDataReader.Item("valor_minimo")
                oInfo.valor_maximo = oSqlDataReader.Item("valor_maximo")
                oInfo.resultado = oSqlDataReader.Item("resultado")
                oInfo.observacao = oSqlDataReader.Item("observacao")
                oInfo.unidade_medida = oSqlDataReader.Item("unidade_medida")
                oInfo.codigo_unidade_medida = oSqlDataReader.Item("codigo_unidade_medida")
                oInfo.data_manutencao = oSqlDataReader.Item("data_manutencao")

                oReturn.Add(oInfo)

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Sub PMOCOrdemServico(ByVal iCodigoEmpresa As Integer,
                                ByVal iCodigoUnidade As Integer,
                                ByVal lCodigoEquipamento As Long,
                                ByVal sData As String,
                                ByVal iStatus As Integer,
                                ByVal iCodigoUsuario As Integer,
                                ByRef lCodigo As Long)

        'Variaveis Locais
        Dim oSqlParameter(6) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Equipamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_equipamento"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoEquipamento : i += 1

            'Seta Parametros - Data
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "data"
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sData : i += 1

            'Seta Parametros - Status
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "status"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iStatus : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Output
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_api_insert_pmoc_ordem_servico", oSqlParameter)

            'Seta Váriavel
            lCodigo = oSqlParameter(i).Value

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub PMOCOrdemServicoChecklist(ByVal lCodigoPmocOrdemServico As Long,
                                         ByVal iCodigoEmpresa As Integer,
                                         ByVal iCodigoUnidade As Integer,
                                         ByVal iCodigoTipoArCondicionado As Integer,
                                         ByVal iCodigoChecklist As Integer,
                                         ByVal sResultado As String,
                                         ByVal sObservacao As String)

        'Variaveis Locais
        Dim oSqlParameter(6) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código PMOC Ordem Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_pmoc_ordem_servico"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoPmocOrdemServico : i += 1

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Tipo Ar Condicionado
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_tipo_ar_condicionado"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoTipoArCondicionado : i += 1

            'Seta Parametros - Código Checklist
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_checklist"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoChecklist : i += 1

            'Seta Parametros - Resultado
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "resultado"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Value = sResultado : i += 1

            'Seta Parametros - Observação
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "observacao"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = sObservacao

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_api_insert_pmoc_ordem_servico_checklist", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub PMOCOrdemServicoChecklistHoras(ByVal lCodigoPmocOrdemServico As Long,
                                              ByVal iCodigoEmpresa As Integer,
                                              ByVal iCodigoUnidade As Integer,
                                              ByVal iCodigoPmocOrdemServicoChecklist As Integer,
                                              ByVal sInicio As String,
                                              ByVal sTermino As String)

        'Variaveis Locais
        Dim oSqlParameter(6) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código PMOC Ordem Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_pmoc_ordem_servico"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoPmocOrdemServico : i += 1

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código PMOC Ordem Serviço Checklist
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_pmoc_ordem_servico_checklist"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoPmocOrdemServicoChecklist : i += 1

            'Seta Parametros - Início
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "data_inicio"
            oSqlParameter(i).SqlDbType = SqlDbType.DateTime
            oSqlParameter(i).Value = sInicio : i += 1

            'Seta Parametros - Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "data_termino"
            oSqlParameter(i).SqlDbType = SqlDbType.DateTime
            oSqlParameter(i).Value = sTermino

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_api_insert_pmoc_ordem_servico_checklist_horas", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub PMOCApontamento(ByVal lCodigoPmocOrdemServico As Long,
                               ByVal iCodigoEmpresa As Integer,
                               ByVal iCodigoUnidade As Integer,
                               ByVal sInicio As String,
                               ByVal sTermino As String,
                               ByVal iCodigoUsuario As Integer)

        'Variaveis Locais
        Dim oSqlParameter(6) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código PMOC Ordem Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_pmoc_ordem_servico"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoPmocOrdemServico : i += 1

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Início
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "inicio"
            oSqlParameter(i).SqlDbType = SqlDbType.DateTime
            oSqlParameter(i).Value = sInicio : i += 1

            'Seta Parametros - Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "termino"
            oSqlParameter(i).SqlDbType = SqlDbType.DateTime
            oSqlParameter(i).Value = sTermino

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_api_insert_pmoc_apontamento", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region

#Region "::: ORDEM SERVIÇO :::"

    Public Function getOrdemServico(ByVal iCodigoEmpresa As Integer,
                                    ByVal iCodigoUnidade As Integer,
                                    ByVal iCodigoFuncionario As Integer,
                                    ByVal iCodigoUsuario As Integer) As APIOrdemServico

        Dim oReturn As New APIOrdemServico

        Try

            oReturn.results = OrdemServicoList(iCodigoEmpresa:=iCodigoEmpresa,
                                               iCodigoUnidade:=iCodigoUnidade,
                                               iCodigoFuncionario:=iCodigoFuncionario,
                                               iCodigoUsuario:=iCodigoUsuario)

            oReturn.page = 1
            oReturn.total_results = oReturn.results.Count
            oReturn.total_pages = 1

            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function OrdemServicoList(ByVal iCodigoEmpresa As Integer,
                                     ByVal iCodigoUnidade As Integer,
                                     ByVal iCodigoFuncionario As Integer,
                                     ByVal iCodigoUsuario As Integer) As List(Of ApiOrdemServicoList)

        'Variaveis Locais
        Dim oSqlParameter(3) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New List(Of ApiOrdemServicoList)
        Dim oInfo As ApiOrdemServicoList
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Funcionário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_funcionario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoFuncionario : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_api_select_ordem_servico_list", oSqlParameter)

            While oSqlDataReader.Read

                oInfo = New ApiOrdemServicoList

                'Seta Retorno da Função
                oInfo.codigo = oSqlDataReader("codigo")
                oInfo.unidade = oSqlDataReader("unidade")
                oInfo.codigo_unidade = oSqlDataReader("codigo_unidade")
                oInfo.ordem_servico = oSqlDataReader("ordem_servico")
                oInfo.descricao = oSqlDataReader("descricao")
                oInfo.data = oSqlDataReader("data")
                oInfo.local = oSqlDataReader("local")
                oInfo.solicitante = oSqlDataReader("solicitante")
                oInfo.prioridade = oSqlDataReader("prioridade")
                oInfo.descricao_status = oSqlDataReader("descricao_status")
                oInfo.status = oSqlDataReader("status")
                oInfo.codigo_equipamento = oSqlDataReader("codigo_equipamento")
                oInfo.equipamento = oSqlDataReader("equipamento")
                oInfo.codigo_funcionario = oSqlDataReader("codigo_funcionario")
                oInfo.status_opera = oSqlDataReader("status_opera")

                oReturn.Add(oInfo)

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Sub OrdemServicoInput(ByRef oApiOrdemServicoInput As ApiOrdemServicoInput,
                                 ByRef oApiOrdemServicoInputResponse As ApiOrdemServicoInputResponse)

        'Variaveis Locais
        Dim oSqlParameter(10) As SqlParameter
        Dim i As Integer = 0
        Dim sImagePath As String = ""

        Try

            'Seta Váriavel
            oApiOrdemServicoInputResponse = New ApiOrdemServicoInputResponse

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oApiOrdemServicoInput.codigo_empresa : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oApiOrdemServicoInput.codigo_usuario : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oApiOrdemServicoInput.codigo_unidade : i += 1

            'Seta Parametros - Data
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "data"
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = oApiOrdemServicoInput.data : i += 1

            'Seta Parametros - Código Setor
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_setor"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oApiOrdemServicoInput.codigo_setor : i += 1

            'Seta Parametros - Código Apartamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_apartamento"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(oApiOrdemServicoInput.codigo_apartamento = -1, DBNull.Value, oApiOrdemServicoInput.codigo_apartamento) : i += 1

            'Seta Parametros - Código Equipamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_equipamento"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = IIf(oApiOrdemServicoInput.codigo_equipamento = -1, DBNull.Value, oApiOrdemServicoInput.codigo_equipamento) : i += 1

            'Seta Parametros - Código Prioridade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_prioridade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oApiOrdemServicoInput.codigo_prioridade : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = oApiOrdemServicoInput.descricao : i += 1

            'Seta Parametros - Ordem de Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Output
            oSqlParameter(i).ParameterName = "ordem_servico"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50 : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Output
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_api_insert_pcm_ordem_servico_v3", oSqlParameter)

            oApiOrdemServicoInputResponse.codigo = oSqlParameter(i).Value
            oApiOrdemServicoInputResponse.ordem_servico = oSqlParameter(i - 1).Value
            oApiOrdemServicoInputResponse.message = ""

        Catch SqlEx As SqlException
            oApiOrdemServicoInputResponse.codigo = 0
            oApiOrdemServicoInputResponse.ordem_servico = ""
            oApiOrdemServicoInputResponse.message = SqlEx.Message
        Catch ex As Exception
            oApiOrdemServicoInputResponse.codigo = 0
            oApiOrdemServicoInputResponse.ordem_servico = ""
            oApiOrdemServicoInputResponse.message = ex.Message
        End Try

    End Sub

    Public Sub InsertApontamentoOS(ByVal iCodigoEmpresa As Integer,
                                   ByVal iCodigoUsuario As Integer,
                                   ByVal lCodigoPCMOrdemServico As Long,
                                   ByVal iCodigoUnidade As Integer,
                                   ByVal iCodigoCategoria As Integer,
                                   ByVal sDataInicio As String,
                                   ByVal sDataTermino As String,
                                   ByVal sDescricaoSolucao As String,
                                   ByVal bConcluido As Boolean,
                                   ByVal iCodigoJustificativaApontamento As Integer,
                                   ByVal sImagem As String,
                                   ByVal lCodigoEquipamento As Long,
                                   ByRef lCodigo As Long)

        Try

            'Váriaveis Locais
            Dim oSqlParameter(15) As SqlParameter
            Dim i As Integer = 0
            Dim sImagePath As String = ""

            'Verifica se carregou Imagem
            If (sImagem <> "") Then

                'Váriaveis Locais

                Dim sEmpresa As String = iCodigoEmpresa.ToString
                Dim sSemana As String = DatePart(DateInterval.WeekOfYear, Now())
                Dim sYear As String = DatePart(DateInterval.Year, Now())
                Dim sDataHora As String = Format(Now(), "dd_HHmmss")
                Dim sPath As String = Path.Combine("C:\SIM\PCM\SITE\IMAGE\", sEmpresa, sYear, sSemana)
                Dim sImageName As String = "APTO_" & Format(Now(), "dd_HHmmss") & ".png"

                'Verifica se o diretório existe
                If (Directory.Exists(sPath) = False) Then
                    'Cria Diretório
                    Directory.CreateDirectory(sPath)
                End If

                'Salva Imagem
                sImagePath = System.IO.Path.Combine(sPath, sImageName)
                Dim imageBytes As Byte() = Convert.FromBase64String(sImagem)
                File.WriteAllBytes(sImagePath, imageBytes)

            End If

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
            oSqlParameter(i).Value = lCodigoPCMOrdemServico : i += 1

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
            oSqlParameter(i).Value = CDate(sDataInicio) : i += 1

            'Seta Parametros - Data Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_termino"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = IIf(IsDate(sDataTermino), sDataTermino, DBNull.Value) : i += 1

            'Seta Parametros - Hora Início
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "hora_inicio"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.DateTime
            oSqlParameter(i).Value = CDate(sDataInicio) : i += 1

            'Seta Parametros - Hora Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "hora_termino"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.DateTime
            oSqlParameter(i).Value = IIf(IsDate(sDataTermino), sDataTermino, DBNull.Value) : i += 1

            'Seta Parametros - Valor
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "valor"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = 0 : i += 1

            'Seta Parametros - Descrição Solução
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao_solucao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 5000
            oSqlParameter(i).Value = sDescricaoSolucao : i += 1

            'Seta Parametros - Imagem
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "imagem"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = IIf(sImagePath = "", DBNull.Value, sImagePath) : i += 1

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

            'Seta Parametros - Código Equipamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_equipamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = IIf(lCodigoEquipamento = -1, DBNull.Value, lCodigoEquipamento) : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Output
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_api_insert_pcm_ordem_servico_apontamento_v2", oSqlParameter)

            lCodigo = oSqlParameter(i).Value

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub OrdemServicoVincular(ByVal iCodigoEmpresa As Integer,
                                    ByVal iCodigoUnidade As Integer,
                                    ByVal lCodigoPCMOrdemServico As Long,
                                    ByVal iCodigoUsuario As Integer)

        'Variaveis Locais
        Dim oSqlParameter(3) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código PCM Ordem Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_pcm_ordem_servico"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoPCMOrdemServico : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_api_update_ordem_servico_vincular", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub OrdemServicoStatus(ByVal iCodigoEmpresa As Integer,
                                  ByVal iCodigoUnidade As Integer,
                                  ByVal lCodigoPCMOrdemServico As Long,
                                  ByVal iCodigoUsuario As Integer,
                                  ByVal iStatus As Integer)

        'Variaveis Locais
        Dim oSqlParameter(4) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código PCM Ordem Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_pcm_ordem_servico"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoPCMOrdemServico : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Status
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "status"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iStatus

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_api_update_ordem_servico_status", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function getEquipamentoInfo(ByVal iCodigoEmpresa As Integer,
                                       ByVal iCodigoUnidade As Integer,
                                       ByVal lCodigoEquipamento As Long) As ApiInfoEquipamentoResponse

        'Variaveis Locais
        Dim oSqlParameter(2) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New ApiInfoEquipamentoResponse
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Equipamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_equipamento"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoEquipamento

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_api_select_ordem_servico_equipamento_info", oSqlParameter)

            While oSqlDataReader.Read

                'Seta Retorno da Função
                oReturn.codigo_setor = oSqlDataReader("codigo_setor")
                oReturn.setor = oSqlDataReader("setor")
                oReturn.codigo_apartamento = oSqlDataReader("codigo_apartamento")
                oReturn.apartamento = oSqlDataReader("apartamento")
                oReturn.codigo_equipamento = oSqlDataReader("codigo_equipamento")
                oReturn.equipamento = oSqlDataReader("equipamento")

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: PROGRAMADA :::"

    Public Function getProgramada(ByVal iCodigoEmpresa As Integer,
                                  ByVal iCodigoUnidade As Integer,
                                  ByVal iCodigoUsuario As Integer,
                                  ByVal sTipo As String) As APIProgramada

        Dim oReturn As New APIProgramada

        Try

            oReturn.results = ProgramadaList(iCodigoEmpresa:=iCodigoEmpresa,
                                             iCodigoUnidade:=iCodigoUnidade,
                                             iCodigoUsuario:=iCodigoUsuario,
                                             sTipo:=sTipo)

            oReturn.page = 1
            oReturn.total_results = oReturn.results.Count
            oReturn.total_pages = 1

            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ProgramadaList(ByVal iCodigoEmpresa As Integer,
                                   ByVal iCodigoUnidade As Integer,
                                   ByVal iCodigoUsuario As Integer,
                                   ByVal sTipo As String) As List(Of APIProgramadaList)

        'Variaveis Locais
        Dim oSqlParameter(3) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New List(Of APIProgramadaList)
        Dim oInfo As APIProgramadaList
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Tipo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "tipo"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sTipo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_api_select_programada_list", oSqlParameter)

            While oSqlDataReader.Read

                oInfo = New APIProgramadaList

                'Seta Retorno da Função
                oInfo.codigo_empresa = oSqlDataReader("codigo_empresa")
                oInfo.unidade = oSqlDataReader("unidade")
                oInfo.codigo_unidade = oSqlDataReader("codigo_unidade")
                oInfo.codigo_pcm_programada = oSqlDataReader("codigo_pcm_programada")
                oInfo.codigo_checklist = oSqlDataReader("codigo_checklist")
                oInfo.descricao = oSqlDataReader("descricao")
                oInfo.data = oSqlDataReader("data")
                oInfo.data_ultima_manutencao = oSqlDataReader("data_ultima_manutencao")
                oInfo.data_validade = oSqlDataReader("data_validade")
                oInfo.codigo_equipamento = oSqlDataReader("codigo_equipamento")
                oInfo.equipamento = oSqlDataReader("equipamento")
                oInfo.status = oSqlDataReader("status")
                oInfo.descricao_status = oSqlDataReader("descricao_status")

                oReturn.Add(oInfo)

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function getProgramadaChecklist(ByVal iCodigoEmpresa As Integer,
                                           ByVal iCodigoUnidade As Integer,
                                           ByVal lCodigoPCMProgramada As Long,
                                           ByVal sTipo As String) As APIProgramadaChecklist

        Dim oReturn As New APIProgramadaChecklist

        Try

            oReturn.results = ProgramadaChecklistList(iCodigoEmpresa:=iCodigoEmpresa,
                                                      iCodigoUnidade:=iCodigoUnidade,
                                                      lCodigoPCMProgramada:=lCodigoPCMProgramada,
                                                      sTipo:=sTipo)

            oReturn.page = 1
            oReturn.total_results = oReturn.results.Count
            oReturn.total_pages = 1

            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ProgramadaChecklistList(ByVal iCodigoEmpresa As Integer,
                                            ByVal iCodigoUnidade As Integer,
                                            ByVal lCodigoPCMProgramada As Long,
                                            ByVal sTipo As String) As List(Of ApiProgramadaChecklistList)

        'Variaveis Locais
        Dim oSqlParameter(3) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New List(Of ApiProgramadaChecklistList)
        Dim oInfo As ApiProgramadaChecklistList
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código PCM Programada
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_pcm_programada"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoPCMProgramada : i += 1

            'Seta Parametros - Tipo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "tipo"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sTipo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_api_select_programada_checklist_list", oSqlParameter)

            While oSqlDataReader.Read

                oInfo = New ApiProgramadaChecklistList

                'Seta Retorno da Função
                oInfo.codigo_empresa = oSqlDataReader.Item("codigo_empresa")
                oInfo.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oInfo.codigo_pcm_programada = oSqlDataReader.Item("codigo_pcm_programada")
                oInfo.codigo = oSqlDataReader.Item("codigo")
                oInfo.descricao_programada = oSqlDataReader.Item("descricao_programada")
                oInfo.grupo_checklist = oSqlDataReader.Item("grupo_checklist")
                oInfo.codigo_checklist = oSqlDataReader.Item("codigo_checklist")
                oInfo.descricao = oSqlDataReader.Item("descricao")
                oInfo.codigo_tipo_item_checklist = oSqlDataReader.Item("codigo_tipo_item_checklist")
                oInfo.valor_minimo = oSqlDataReader.Item("valor_minimo")
                oInfo.valor_maximo = oSqlDataReader.Item("valor_maximo")
                oInfo.resultado = oSqlDataReader.Item("resultado")
                oInfo.observacao = oSqlDataReader.Item("observacao")
                oInfo.unidade_medida = oSqlDataReader.Item("unidade_medida")
                oInfo.data = oSqlDataReader.Item("data")
                oInfo.unidade = oSqlDataReader.Item("unidade")

                oReturn.Add(oInfo)

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Sub ProgramadaOrdemServico(ByVal iCodigoEmpresa As Integer,
                                      ByVal iCodigoUnidade As Integer,
                                      ByVal lCodigoPCMProgramada As Long,
                                      ByVal sData As String,
                                      ByVal bConcluido As Boolean,
                                      ByVal sSolucao As String,
                                      ByVal dValor As Double,
                                      ByVal iQuantidadeEquipamento As Integer,
                                      ByVal iStatus As Integer,
                                      ByVal iCodigoUsuario As Integer,
                                      ByRef lCodigo As Long)

        'Variaveis Locais
        Dim oSqlParameter(10) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código PCM Programada
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_pcm_programada"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoPCMProgramada : i += 1

            'Seta Parametros - Data
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "data"
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sData : i += 1

            'Seta Parametros - Concluido
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "concluido"
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bConcluido : i += 1

            'Seta Parametros - Solução
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "solucao"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = sSolucao.ToUpper() : i += 1

            'Seta Parametros - Valor
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "valor"
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = dValor : i += 1

            'Seta Parametros - Quantidade Equipamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "quantidade_equipamento"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iQuantidadeEquipamento : i += 1

            'Seta Parametros - Status
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "status"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iStatus : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Output
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_api_insert_programada_ordem_servico", oSqlParameter)

            'Seta Váriavel
            lCodigo = oSqlParameter(i).Value

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub ProgramadaOrdemServicoChecklist(ByVal lCodigoProgramadaOrdemServico As Long,
                                               ByVal lCodigoPCMProgramada As Long,
                                               ByVal iCodigoEmpresa As Integer,
                                               ByVal iCodigoUnidade As Integer,
                                               ByVal iCodigoChecklist As Integer,
                                               ByVal sResultado As String,
                                               ByVal sObservacao As String,
                                               ByVal iCodigoUsuario As Integer)

        'Variaveis Locais
        Dim oSqlParameter(7) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Programada Ordem Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_programada_ordem_servico"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoProgramadaOrdemServico : i += 1

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código PCM Programada
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_pcm_programada"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoPCMProgramada : i += 1

            'Seta Parametros - Código Checklist
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_checklist"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoChecklist : i += 1

            'Seta Parametros - Resultado
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "resultado"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Value = sResultado : i += 1

            'Seta Parametros - Observação
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "observacao"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = sObservacao : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_api_insert_programada_ordem_servico_checklist", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub ProgramadaOrdemServicoChecklistHoras(ByVal lCodigoProgramadaOrdemServico As Long,
                                                    ByVal iCodigoEmpresa As Integer,
                                                    ByVal iCodigoUnidade As Integer,
                                                    ByVal iCodigoProgramadaOrdemServicoChecklist As Integer,
                                                    ByVal sInicio As String,
                                                    ByVal sTermino As String)

        'Variaveis Locais
        Dim oSqlParameter(6) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Programada Ordem Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_programada_ordem_servico"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoProgramadaOrdemServico : i += 1

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Programada Ordem Serviço Checklist
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_programada_ordem_servico_checklist"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoProgramadaOrdemServicoChecklist : i += 1

            'Seta Parametros - Início
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "data_inicio"
            oSqlParameter(i).SqlDbType = SqlDbType.DateTime
            oSqlParameter(i).Value = sInicio : i += 1

            'Seta Parametros - Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "data_termino"
            oSqlParameter(i).SqlDbType = SqlDbType.DateTime
            oSqlParameter(i).Value = sTermino

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_api_insert_programada_ordem_servico_checklist_horas", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub ProgramadaApontamento(ByVal lCodigoProgramadaOrdemServico As Long,
                                     ByVal iCodigoEmpresa As Integer,
                                     ByVal iCodigoUnidade As Integer,
                                     ByVal sInicio As String,
                                     ByVal sTermino As String,
                                     ByVal iCodigoUsuario As Integer)

        'Variaveis Locais
        Dim oSqlParameter(6) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Programada Ordem Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_programada_ordem_servico"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoProgramadaOrdemServico : i += 1

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Início
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "inicio"
            oSqlParameter(i).SqlDbType = SqlDbType.DateTime
            oSqlParameter(i).Value = sInicio : i += 1

            'Seta Parametros - Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "termino"
            oSqlParameter(i).SqlDbType = SqlDbType.DateTime
            oSqlParameter(i).Value = sTermino

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_api_insert_programada_apontamento", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region

#Region "::: TAREFA :::"

    Public Function getTarefa(ByVal iCodigoEmpresa As Integer,
                              ByVal iCodigoUnidade As Integer) As APITarefa

        Dim oReturn As New APITarefa

        Try

            oReturn.results = TarefaList(iCodigoEmpresa:=iCodigoEmpresa,
                                         iCodigoUnidade:=iCodigoUnidade)

            oReturn.page = 1
            oReturn.total_results = oReturn.results.Count
            oReturn.total_pages = 1

            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function TarefaList(ByVal iCodigoEmpresa As Integer,
                               ByVal iCodigoUnidade As Integer) As List(Of APITarefaList)

        'Variaveis Locais
        Dim oSqlParameter(3) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New List(Of APITarefaList)
        Dim oInfo As APITarefaList
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_api_select_qualidade_tarefa_list", oSqlParameter)

            While oSqlDataReader.Read

                oInfo = New APITarefaList

                'Seta Retorno da Função
                oInfo.codigo_empresa = oSqlDataReader("codigo_empresa")
                oInfo.unidade = oSqlDataReader("unidade")
                oInfo.codigo_unidade = oSqlDataReader("codigo_unidade")
                oInfo.codigo_qa_tarefa = oSqlDataReader("codigo_qa_tarefa")
                oInfo.codigo_checklist = oSqlDataReader("codigo_checklist")
                oInfo.descricao = oSqlDataReader("descricao")
                oInfo.data = oSqlDataReader("data")
                oInfo.data_ultima_tarefa = oSqlDataReader("data_ultima_tarefa")
                oInfo.status = oSqlDataReader("status")
                oInfo.descricao_status = oSqlDataReader("descricao_status")

                oReturn.Add(oInfo)

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: UH :::"

    Public Function getUH(ByVal iCodigoEmpresa As Integer,
                          ByVal iCodigoUnidade As Integer) As APIUH

        Dim oReturn As New APIUH

        Try

            oReturn.results = UHList(iCodigoEmpresa:=iCodigoEmpresa,
                                     iCodigoUnidade:=iCodigoUnidade)

            oReturn.page = 1
            oReturn.total_results = oReturn.results.Count
            oReturn.total_pages = 1

            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function UHList(ByVal iCodigoEmpresa As Integer,
                           ByVal iCodigoUnidade As Integer) As List(Of APIUHList)

        'Variaveis Locais
        Dim oSqlParameter(2) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New List(Of APIUHList)
        Dim oInfo As APIUHList
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_api_select_uh_list", oSqlParameter)

            While oSqlDataReader.Read

                oInfo = New APIUHList

                'Seta Retorno da Função
                oInfo.codigo_empresa = oSqlDataReader("codigo_empresa")
                oInfo.codigo_unidade = oSqlDataReader("codigo_unidade")
                oInfo.codigo_apartamento = oSqlDataReader("codigo_apartamento")
                oInfo.codigo_checklist = oSqlDataReader("codigo_checklist")
                oInfo.codigo = oSqlDataReader("codigo")
                oInfo.unidade = oSqlDataReader("unidade")
                oInfo.status = oSqlDataReader("status")
                oInfo.data_ultima_vistoria = oSqlDataReader("data_ultima_vistoria")
                oInfo.data_proxima_vistoria = oSqlDataReader("data_proxima_vistoria")
                oInfo.bloco = oSqlDataReader("bloco")
                oInfo.andar = oSqlDataReader("andar")
                oInfo.apartamento = oSqlDataReader("apartamento")
                oInfo.status_opera = oSqlDataReader("status_opera")
                oInfo.nova_vistoria = oSqlDataReader("nova_vistoria")
                oInfo.codigo_apontamento_origem = oSqlDataReader("codigo_apontamento_origem")

                oReturn.Add(oInfo)

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function getUHChecklist(ByVal iCodigoEmpresa As Integer,
                                   ByVal iCodigoUnidade As Integer,
                                   ByVal lCodigoApartamento As Long,
                                   ByVal lCodigoApontamentoOrigem As Long) As APIUHChecklist

        Dim oReturn As New APIUHChecklist

        Try

            oReturn.results = UHChecklistList(iCodigoEmpresa:=iCodigoEmpresa,
                                              iCodigoUnidade:=iCodigoUnidade,
                                              lCodigoApartamento:=lCodigoApartamento,
                                              lCodigoApontamentoOrigem:=lCodigoApontamentoOrigem)

            oReturn.page = 1
            oReturn.total_results = oReturn.results.Count
            oReturn.total_pages = 1

            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function UHChecklistList(ByVal iCodigoEmpresa As Integer,
                                    ByVal iCodigoUnidade As Integer,
                                    ByVal lCodigoApartamento As Long,
                                    ByVal lCodigoApontamentoOrigem As Long) As List(Of ApiUHChecklistList)

        'Variaveis Locais
        Dim oSqlParameter(3) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New List(Of ApiUHChecklistList)
        Dim oInfo As ApiUHChecklistList
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Apartamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_apartamento"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoApartamento : i += 1

            'Seta Parametros - Código Apontamento Oridem
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_apontamento_origem"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoApontamentoOrigem

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_api_select_uh_checklist_list", oSqlParameter)

            While oSqlDataReader.Read

                oInfo = New ApiUHChecklistList

                'Seta Retorno da Função
                oInfo.codigo_empresa = oSqlDataReader.Item("codigo_empresa")
                oInfo.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oInfo.codigo_apartamento = oSqlDataReader.Item("codigo_apartamento")
                oInfo.codigo = oSqlDataReader.Item("codigo")
                oInfo.apartamento = oSqlDataReader.Item("apartamento")
                oInfo.grupo_checklist = oSqlDataReader.Item("grupo_checklist")
                oInfo.codigo_checklist = oSqlDataReader.Item("codigo_checklist")
                oInfo.descricao = oSqlDataReader.Item("descricao")
                oInfo.opcao = oSqlDataReader.Item("opcao")
                oInfo.observacao = oSqlDataReader.Item("observacao")
                oInfo.nova_vistoria = oSqlDataReader.Item("nova_vistoria")

                oReturn.Add(oInfo)

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Sub UHOrdemServico(ByVal iCodigoEmpresa As Integer,
                              ByVal iCodigoUnidade As Integer,
                              ByVal lCodigoApartamento As Long,
                              ByVal sDataInicio As String,
                              ByVal sDataTermino As String,
                              ByVal iCodigoUsuario As Integer,
                              ByVal iCodigoResponsavelUnidade As Integer,
                              ByRef lCodigo As Long)

        'Variaveis Locais
        Dim oSqlParameter(7) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Apartamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_apartamento"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoApartamento : i += 1

            'Seta Parametros - Data Início
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "data_inicio"
            oSqlParameter(i).SqlDbType = SqlDbType.DateTime
            oSqlParameter(i).Value = sDataInicio : i += 1

            'Seta Parametros - Data Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "data_termino"
            oSqlParameter(i).SqlDbType = SqlDbType.DateTime
            oSqlParameter(i).Value = sDataTermino : i += 1

            'Seta Parametros - Código Responsável Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_responsavel_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoResponsavelUnidade : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Output
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_api_insert_UH_ordem_servico", oSqlParameter)

            'Seta Váriavel
            lCodigo = oSqlParameter(i).Value

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UHOrdemServicoChecklist(ByVal lCodigoUHOrdemServico As Long,
                                               ByVal lCodigoPCMUH As Long,
                                               ByVal iCodigoEmpresa As Integer,
                                               ByVal iCodigoUnidade As Integer,
                                               ByVal iCodigoChecklist As Integer,
                                               ByVal sResultado As String,
                                               ByVal sObservacao As String,
                                               ByVal iCodigoUsuario As Integer)

        'Variaveis Locais
        Dim oSqlParameter(7) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código UH Ordem Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_apartamento"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoUHOrdemServico : i += 1

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código PCM UH
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_pcm_UH"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoPCMUH : i += 1

            'Seta Parametros - Código Checklist
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_checklist"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoChecklist : i += 1

            'Seta Parametros - Resultado
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "resultado"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Value = sResultado : i += 1

            'Seta Parametros - Observação
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "observacao"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = sObservacao : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_api_insert_UH_ordem_servico_checklist", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UHOrdemServicoChecklistHoras(ByVal lCodigoUHOrdemServico As Long,
                                                    ByVal iCodigoEmpresa As Integer,
                                                    ByVal iCodigoUnidade As Integer,
                                                    ByVal iCodigoUHOrdemServicoChecklist As Integer,
                                                    ByVal sInicio As String,
                                                    ByVal sTermino As String)

        'Variaveis Locais
        Dim oSqlParameter(6) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código UH Ordem Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_UH_ordem_servico"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoUHOrdemServico : i += 1

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código UH Ordem Serviço Checklist
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_UH_ordem_servico_checklist"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUHOrdemServicoChecklist : i += 1

            'Seta Parametros - Início
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "data_inicio"
            oSqlParameter(i).SqlDbType = SqlDbType.DateTime
            oSqlParameter(i).Value = sInicio : i += 1

            'Seta Parametros - Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "data_termino"
            oSqlParameter(i).SqlDbType = SqlDbType.DateTime
            oSqlParameter(i).Value = sTermino

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_api_insert_UH_ordem_servico_checklist_horas", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InsertUHApontamento(ByVal iCodigoEmpresa As Integer,
                                   ByVal iCodigoUsuario As Integer,
                                   ByVal iCodigoUnidade As Integer,
                                   ByVal lCodigoApartamento As Long,
                                   ByVal iCodigoFuncionarioResponsavelUnidade As Integer,
                                   ByVal sDataInicio As String,
                                   ByVal sDataTermino As String,
                                   ByRef lCodigoUHApontamento As Long)

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

            'Seta Parametros - Código Apartamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_apartamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoApartamento : i += 1

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
            oSqlParameter(i).SqlDbType = SqlDbType.DateTime
            oSqlParameter(i).Value = CDate(sDataInicio) : i += 1

            'Seta Parametros - Data Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_termino"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.DateTime
            oSqlParameter(i).Value = CDate(sDataTermino) : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.InputOutput
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoUHApontamento

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_api_insert_uh_apontamento", oSqlParameter)

            lCodigoUHApontamento = oSqlParameter(i).Value

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InsertUHApontamentoChecklist(ByVal iCodigoEmpresa As Integer,
                                            ByVal iCodigoUnidade As Integer,
                                            ByVal lCodigoUHApontamento As Long,
                                            ByVal iCodigoChecklist As Integer,
                                            ByVal iOpcao As Integer,
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
            oSqlParameter(i).Value = IIf(iCodigoChecklist = -1, DBNull.Value, iCodigoChecklist) : i += 1

            'Seta Parametros - Opção
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "opcao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iOpcao : i += 1

            'Seta Parametros - Observação
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "observacao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 200
            oSqlParameter(i).Value = IIf(IsNothing(sObservacao), "", sObservacao)

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_api_insert_uh_apontamento_checklist", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region

#Region "::: GREEN PLANET :::"

    Public Function getGreenPlanet(ByVal iCodigoEmpresa As Integer,
                          ByVal iCodigoUnidade As Integer) As APIGreenPlanet

        Dim oReturn As New APIGreenPlanet

        Try

            oReturn.results = GreenPlanetList(iCodigoEmpresa:=iCodigoEmpresa,
                                              iCodigoUnidade:=iCodigoUnidade)

            oReturn.page = 1
            oReturn.total_results = oReturn.results.Count
            oReturn.total_pages = 1

            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function GreenPlanetList(ByVal iCodigoEmpresa As Integer,
                                    ByVal iCodigoUnidade As Integer) As List(Of APIGreenPlanetList)

        'Variaveis Locais
        Dim oSqlParameter(2) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New List(Of APIGreenPlanetList)
        Dim oInfo As APIGreenPlanetList
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_api_select_green_planet_list", oSqlParameter)

            While oSqlDataReader.Read

                oInfo = New APIGreenPlanetList

                'Seta Retorno da Função
                oInfo.codigo_empresa = oSqlDataReader("codigo_empresa")
                oInfo.codigo_unidade = oSqlDataReader("codigo_unidade")
                oInfo.codigo_item_medicao = oSqlDataReader("codigo_item_medicao")
                oInfo.unidade = oSqlDataReader("unidade")
                oInfo.data = oSqlDataReader("data")
                oInfo.item_medicao = oSqlDataReader("item_medicao")
                oInfo.quantidade_hospede = oSqlDataReader("quantidade_hospede")
                oInfo.ocupacao_quartos = oSqlDataReader("ocupacao_quartos")
                oInfo.valor = oSqlDataReader("valor")

                oReturn.Add(oInfo)

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function GreenPlanetInput(ByVal iCodigoEmpresa As Integer,
                                     ByVal iCodigoUnidade As Integer,
                                     ByVal iCodigoItemMedicao As Long,
                                     ByVal iCodigoUsuario As Integer,
                                     ByVal iQuantidadeHospede As String,
                                     ByVal iOcupacaoQuartos As String,
                                     ByVal sValor As String)

        'Variaveis Locais
        Dim oSqlParameter(6) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Item Medição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_item_medicao"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoItemMedicao : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Qtde. Hospede
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "quantidade_hospede"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iQuantidadeHospede : i += 1

            'Seta Parametros - Ocupação Quartos
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "ocupacao_quartos"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iOcupacaoQuartos : i += 1

            'Seta Parametros - Valor
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "valor"
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = CDbl(sValor)

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_api_insert_green_planet", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: FIREBASE TOKEN :::"

    Public Sub InsertToken(ByVal iCodigoEmpresa As Integer,
                           ByVal iCodigoUnidade As Integer,
                           ByVal sToken As String)

        'Variaveis Locais
        Dim oSqlParameter(2) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Token
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "token"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = sToken

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_api_insert_firebase_token", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InsertTokenNew(ByVal iCodigoEmpresa As Integer,
                              ByVal iCodigoUsuario As Integer,
                              ByVal sToken As String)

        'Variaveis Locais
        Dim oSqlParameter(2) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Token
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "token"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = sToken

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_api_insert_firebase_token_new", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region

#Region "::: REQUISIÇÃO :::"

    Public Sub RequisicaoInput(ByRef oApiRequisicaoInput As ApiRequisicaoInput,
                               ByRef oApiRequisicaoInputResponse As ApiRequisicaoInputResponse)

        'Variaveis Locais
        Dim oSqlParameter(9) As SqlParameter
        Dim i As Integer = 0
        Dim sImagePath As String = ""

        Try

            'Seta Váriavel
            oApiRequisicaoInputResponse = New ApiRequisicaoInputResponse

            'Verifica se carregou Imagem
            If (oApiRequisicaoInput.imagem <> "") Then


                'Váriaveis Locais
                Dim sEmpresa As String = oApiRequisicaoInput.codigo_empresa.ToString
                Dim sSemana As String = DatePart(DateInterval.WeekOfYear, Now())
                Dim sYear As String = DatePart(DateInterval.Year, Now())
                Dim sDataHora As String = Format(Now(), "dd_HHmmss")
                Dim sPath As String = Path.Combine("C:\SIM\PCM\SITE\IMAGE\", sEmpresa, sYear, sSemana)
                Dim sImageName As String = "REQ_OS_" & Format(Now(), "dd_HHmmss") & ".png"

                'Verifica se o diretório existe
                If (Directory.Exists(sPath) = False) Then
                    'Cria Diretório
                    Directory.CreateDirectory(sPath)
                End If

                'Salva Imagem
                sImagePath = System.IO.Path.Combine(sPath, sImageName)
                Dim imageBytes As Byte() = Convert.FromBase64String(oApiRequisicaoInput.imagem)
                File.WriteAllBytes(sImagePath, imageBytes)

            End If

            oApiRequisicaoInput.imagem = ""

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oApiRequisicaoInput.codigo_empresa : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oApiRequisicaoInput.codigo_usuario : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oApiRequisicaoInput.codigo_unidade : i += 1

            'Seta Parametros - Código Setor
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_setor"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oApiRequisicaoInput.codigo_setor : i += 1

            'Seta Parametros - Código Apartamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_apartamento"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(oApiRequisicaoInput.codigo_apartamento = -1, DBNull.Value, oApiRequisicaoInput.codigo_apartamento) : i += 1

            'Seta Parametros - Código Equipamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_equipamento"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = IIf(oApiRequisicaoInput.codigo_equipamento = -1, DBNull.Value, oApiRequisicaoInput.codigo_equipamento) : i += 1

            'Seta Parametros - Código Prioridade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_prioridade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oApiRequisicaoInput.codigo_prioridade : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = oApiRequisicaoInput.descricao : i += 1

            'Seta Parametros - Imagem
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "imagem"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = IIf(sImagePath = "", DBNull.Value, sImagePath) : i += 1

            'Seta Parametros - Requisicação
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Output
            oSqlParameter(i).ParameterName = "requisicao"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_api_insert_pcm_requisicao", oSqlParameter)

            oApiRequisicaoInput.requisicao = oSqlParameter(i).Value
            oApiRequisicaoInputResponse.requisicao = oSqlParameter(i).Value
            oApiRequisicaoInputResponse.message = ""

        Catch SqlEx As SqlException
            oApiRequisicaoInputResponse.requisicao = ""
            oApiRequisicaoInputResponse.message = SqlEx.Message
        Catch ex As Exception
            oApiRequisicaoInputResponse.requisicao = ""
            oApiRequisicaoInputResponse.message = ex.Message
        End Try

    End Sub

#End Region

#Region "::: CHECKLIST :::"

    Public Function GrupoChecklist(ByVal iCodigoEmpresa As Integer,
                                   ByVal iCodigoUnidade As Integer,
                                   ByVal iCodigoChecklist As Integer) As List(Of APIGrupoChecklist)

        'Variaveis Locais
        Dim oSqlParameter(2) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New List(Of APIGrupoChecklist)
        Dim oInfo As APIGrupoChecklist
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Checklist
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_checklist"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoChecklist

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_api_select_checklist_grupo", oSqlParameter)

            While oSqlDataReader.Read

                'Seta Retorno da Função
                oInfo = New APIGrupoChecklist

                oInfo.descricao = oSqlDataReader.Item("descricao")

                oReturn.Add(oInfo)

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function SubGrupoChecklist(ByVal iCodigoEmpresa As Integer,
                                      ByVal iCodigoUnidade As Integer,
                                      ByVal iCodigoChecklist As Integer,
                                      ByVal sGrupoChecklist As String) As List(Of APIGrupoChecklist)

        'Variaveis Locais
        Dim oSqlParameter(2) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New List(Of APIGrupoChecklist)
        Dim oInfo As APIGrupoChecklist
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Checklist
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_checklist"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoChecklist

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_api_select_checklist_grupo", oSqlParameter)

            While oSqlDataReader.Read

                'Seta Retorno da Função
                oInfo = New APIGrupoChecklist

                oInfo.descricao = oSqlDataReader.Item("descricao")

                oReturn.Add(oInfo)

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function Checklist(ByVal iCodigoEmpresa As Integer,
                              ByVal iCodigoUnidade As Integer,
                              ByVal iCodigoUsuario As Integer,
                              ByVal lCodigoChecklist As Long,
                              ByVal lCodigo As Long,
                              ByVal iIntervalo As Integer) As List(Of APIChecklist)

        'Variaveis Locais
        Dim oSqlParameter(5) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New List(Of APIChecklist)
        Dim oInfo As APIChecklist
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código Checklist
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_checklist"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoChecklist : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigo : i += 1

            'Seta Parametros - Intervalo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "intervalo"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iIntervalo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_api_select_checklist", oSqlParameter)

            While oSqlDataReader.Read

                'Seta Retorno da Função
                oInfo = New APIChecklist

                oInfo.codigo_auditoria = 0
                oInfo.codigo_empresa = oSqlDataReader.Item("codigo_empresa")
                oInfo.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oInfo.codigo_checklist = oSqlDataReader.Item("codigo_checklist")
                oInfo.codigo = oSqlDataReader.Item("codigo")
                oInfo.codigo_usuario = oSqlDataReader.Item("codigo_usuario")
                oInfo.tipo = oSqlDataReader.Item("tipo")
                oInfo.codigo_checklist_item = oSqlDataReader.Item("codigo_checklist_item")
                oInfo.grupo_checklist = oSqlDataReader.Item("grupo_checklist")
                oInfo.sub_grupo_checklist = oSqlDataReader.Item("sub_grupo_checklist")
                oInfo.checklist = oSqlDataReader.Item("checklist")
                oInfo.descricao = oSqlDataReader.Item("descricao")
                oInfo.codigo_tipo_item_checklist = oSqlDataReader.Item("codigo_tipo_item_checklist")
                oInfo.numero_digitos = oSqlDataReader.Item("numero_digitos")
                oInfo.allow_picture = oSqlDataReader.Item("allow_picture")
                oInfo.valor_minimo = oSqlDataReader.Item("valor_minimo")
                oInfo.valor_maximo = oSqlDataReader.Item("valor_maximo")
                oInfo.unidade_medida = oSqlDataReader.Item("unidade_medida")
                oInfo.resultado = oSqlDataReader.Item("resultado")
                oInfo.observacao = oSqlDataReader.Item("observacao")
                oInfo.error_last = oSqlDataReader.Item("error_last")
                oInfo.picture = ChecklistPicture(iCodigoEmpresa:=iCodigoEmpresa,
                                                 iCodigoUnidade:=iCodigoUnidade,
                                                 iCodigoUsuario:=iCodigoUsuario,
                                                 lCodigoChecklist:=lCodigoChecklist,
                                                 iCodigoChecklistItem:=oSqlDataReader.Item("codigo_checklist_item"),
                                                 lCodigo:=lCodigo)

                oReturn.Add(oInfo)

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ChecklistPicture(ByVal iCodigoEmpresa As Integer,
                                     ByVal iCodigoUnidade As Integer,
                                     ByVal iCodigoUsuario As Integer,
                                     ByVal lCodigoChecklist As Long,
                                     ByVal iCodigoChecklistItem As Integer,
                                     ByVal lCodigo As Long) As List(Of APIChecklistPictureWeb)

        'Variaveis Locais
        Dim oSqlParameter(5) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New List(Of APIChecklistPictureWeb)
        Dim oInfo As APIChecklistPictureWeb
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código Checklist
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_checklist"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoChecklist : i += 1

            'Seta Parametros - Código Checklist Item
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_checklist_item"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoChecklistItem : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_api_select_checklist_picture", oSqlParameter)

            While oSqlDataReader.Read

                'Seta Retorno da Função
                oInfo = New APIChecklistPictureWeb

                oInfo.codigo_checklist_item = oSqlDataReader.Item("codigo_checklist_item")
                oInfo.image = oSqlDataReader.Item("image")
                oInfo.observacao = oSqlDataReader.Item("observacao")

                oReturn.Add(oInfo)

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: AUDITORIA :::"

    Public Function createAuditoria(ByVal iCodigoEmpresa As Integer,
                                    ByVal iCodigoUnidade As Integer,
                                    ByVal iCodigoUsuario As Integer,
                                    ByVal sNumeroDocumento As String,
                                    ByVal lCodigoAuditoria As Long) As APICreateAuditoriaResponse

        'Variaveis Locais
        Dim oSqlParameter(4) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New APICreateAuditoriaResponse
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Código Auditoria
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_auditoria"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoAuditoria : i += 1

            'Seta Parametros - Número Documento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "numero_documento"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sNumeroDocumento

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_api_insert_auditoria", oSqlParameter)

            While oSqlDataReader.Read

                'Seta Retorno da Função
                oReturn.codigo = oSqlDataReader.Item("codigo")
                oReturn.codigo_checklist = oSqlDataReader.Item("codigo_checklist")
                oReturn.checklist = Checklist(iCodigoEmpresa:=iCodigoEmpresa,
                                              iCodigoUnidade:=iCodigoUnidade,
                                              iCodigoUsuario:=iCodigoUsuario,
                                              lCodigoChecklist:=oSqlDataReader.Item("codigo_checklist"),
                                              lCodigo:=oSqlDataReader.Item("codigo"),
                                              iIntervalo:=0)

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function getAuditoria(ByVal iCodigoEmpresa As Integer,
                                 ByVal iCodigoUnidade As Integer) As APIAuditoria

        Dim oReturn As New APIAuditoria

        Try

            oReturn.results = AuditoriaList(iCodigoEmpresa:=iCodigoEmpresa,
                                            iCodigoUnidade:=iCodigoUnidade)

            oReturn.page = 1
            oReturn.total_results = oReturn.results.Count
            oReturn.total_pages = 1

            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function AuditoriaList(ByVal iCodigoEmpresa As Integer,
                                  ByVal iCodigoUnidade As Integer) As List(Of ApiAuditoriaList)

        'Variaveis Locais
        Dim oSqlParameter(1) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New List(Of ApiAuditoriaList)
        Dim oInfo As ApiAuditoriaList
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_api_select_auditoria_list", oSqlParameter)

            While oSqlDataReader.Read

                oInfo = New ApiAuditoriaList

                'Seta Retorno da Função
                oInfo.codigo = oSqlDataReader("codigo")
                oInfo.codigo_checklist = oSqlDataReader("codigo_checklist")
                oInfo.numero_documento = oSqlDataReader("numero_documento")
                oInfo.codigo_unidade = oSqlDataReader("codigo_unidade")
                oInfo.unidade = oSqlDataReader("unidade")
                oInfo.data = oSqlDataReader("data")
                oInfo.executor = oSqlDataReader("executor")
                oInfo.status = oSqlDataReader("status")

                oReturn.Add(oInfo)

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Sub InsertChecklist(ByVal oAPIChecklistInsert As APIChecklistInsert,
                               ByRef lCodigo As Long)

        'Variaveis Locais
        Dim oSqlParameter(14) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = oAPIChecklistInsert.codigo_empresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oAPIChecklistInsert.codigo_unidade : i += 1

            'Seta Parametros - Código Checklist
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_checklist"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oAPIChecklistInsert.codigo_checklist : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oAPIChecklistInsert.codigo_usuario : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = oAPIChecklistInsert.codigo : i += 1

            'Seta Parametros - Tipo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "tipo"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = oAPIChecklistInsert.tipo : i += 1

            'Seta Parametros - Data Início
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "data_inicio"
            oSqlParameter(i).SqlDbType = SqlDbType.DateTime
            oSqlParameter(i).Value = oAPIChecklistInsert.data_inicio : i += 1

            'Seta Parametros - Data Término
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "data_termino"
            oSqlParameter(i).SqlDbType = SqlDbType.DateTime
            oSqlParameter(i).Value = IIf(IsDate(oAPIChecklistInsert.data_termino), oAPIChecklistInsert.data_termino, DBNull.Value) : i += 1

            'Seta Parametros - Concluído
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "concluido"
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = oAPIChecklistInsert.concluido : i += 1

            'Seta Parametros - Solução
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "solucao"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            If IsNothing(oAPIChecklistInsert.solucao) Then
                oSqlParameter(i).Value = ""
            Else
                oSqlParameter(i).Value = oAPIChecklistInsert.solucao.ToUpper()
            End If : i += 1

            'Seta Parametros - Valor
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "valor"
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = oAPIChecklistInsert.valor : i += 1

            'Seta Parametros - Código Responsável
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_responsavel"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oAPIChecklistInsert.codigo_responsavel : i += 1

            'Seta Parametros - Quantidade Equipamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "quantidade_equipamento"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oAPIChecklistInsert.quantidade_equipamento : i += 1

            'Seta Parametros - Concluído
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Output
            oSqlParameter(i).ParameterName = "codigo_return"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_api_insert_checklist", oSqlParameter)

            lCodigo = oSqlParameter(i).Value

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InsertChecklistItem(ByVal iCodigoEmpresa As Integer,
                                   ByVal iCodigoUnidade As Integer,
                                   ByVal lCodigoChecklist As Long,
                                   ByVal sTipo As String,
                                   ByVal lCodigo As Long,
                                   ByVal iCodigoItemChecklist As Integer,
                                   ByVal sResultado As String,
                                   ByVal sObservacao As String,
                                   ByVal iPrazo As Integer)

        'Variaveis Locais
        Dim oSqlParameter(8) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Checklist
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_checklist"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoChecklist : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigo : i += 1

            'Seta Parametros - Tipo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "tipo"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sTipo : i += 1

            'Seta Parametros - Código Item Checklist
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_item_checklist"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoItemChecklist : i += 1

            'Seta Parametros - Resultado
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "resultado"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Value = sResultado : i += 1

            'Seta Parametros - Prazo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "prazo"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iPrazo : i += 1

            'Seta Parametros - Observação
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "observacao"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 1000
            oSqlParameter(i).Value = sObservacao

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_api_insert_checklist_item", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InsertChecklistPicture(ByVal oAPIChecklistPicture As APIChecklistPicture)

        'Variaveis Locais
        Dim oSqlParameter(6) As SqlParameter
        Dim i As Integer = 0
        Dim sImagePath As String = ""

        Try

            'Verifica se carregou Imagem
            If (oAPIChecklistPicture.foto <> "") Then

                'Váriaveis Locais
                Dim sEmpresa As String = oAPIChecklistPicture.codigo_empresa.ToString
                Dim sSemana As String = DatePart(DateInterval.WeekOfYear, Now())
                Dim sYear As String = DatePart(DateInterval.Year, Now())
                Dim sDataHora As String = Format(Now(), "dd_HHmmss")
                Dim sPath As String = Path.Combine("C:\SIM\PCM\SITE\IMAGE\CHECKLIST", sEmpresa, sYear, sSemana)
                Dim sImageName As String = System.IO.Path.GetTempFileName.Replace(System.IO.Path.GetTempPath, "").Replace(".tmp", ".png")

                'Verifica se o diretório existe
                If (Directory.Exists(sPath) = False) Then
                    'Cria Diretório
                    Directory.CreateDirectory(sPath)
                End If

                'Salva Imagem
                sImagePath = System.IO.Path.Combine(sPath, sImageName)
                Dim imageBytes As Byte() = Convert.FromBase64String(oAPIChecklistPicture.foto)
                File.WriteAllBytes(sImagePath, imageBytes)

            End If

            oAPIChecklistPicture.foto = ""

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oAPIChecklistPicture.codigo_empresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oAPIChecklistPicture.codigo_unidade : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = oAPIChecklistPicture.codigo : i += 1

            'Seta Parametros - Código Checklist
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_checklist"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oAPIChecklistPicture.codigo_checklist : i += 1

            'Seta Parametros - Código Item Checklist
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_item_checklist"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oAPIChecklistPicture.codigo_item_checklist : i += 1

            'Seta Parametros - Imagem
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "imagem"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = IIf(sImagePath = "", DBNull.Value, sImagePath) : i += 1

            'Seta Parametros - Observação
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "observacao"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = oAPIChecklistPicture.observacao

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_api_insert_checklist_foto", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region

#Region "::: AUDITORIA - QUALIDADE :::"

    Public Function getAuditoriaQualidade(ByVal iCodigoEmpresa As Integer,
                                          ByVal iCodigoUnidade As Integer,
                                          ByVal iCodigoUsuario As Integer) As APIAuditoriaQualidade

        Dim oReturn As New APIAuditoriaQualidade

        Try

            oReturn.results = AuditoriaQualidadeList(iCodigoEmpresa:=iCodigoEmpresa,
                                                     iCodigoUnidade:=iCodigoUnidade,
                                                     iCodigoUsuario:=iCodigoUsuario)

            oReturn.page = 1
            oReturn.total_results = oReturn.results.Count
            oReturn.total_pages = 1

            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function AuditoriaQualidadeList(ByVal iCodigoEmpresa As Integer,
                                           ByVal iCodigoUnidade As Integer,
                                           ByVal iCodigoUsuario As Integer) As List(Of ApiAuditoriaQualidadeList)

        'Variaveis Locais
        Dim oSqlParameter(2) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New List(Of ApiAuditoriaQualidadeList)
        Dim oInfo As ApiAuditoriaQualidadeList
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_api_select_auditoria_qualidade_list", oSqlParameter)

            While oSqlDataReader.Read

                oInfo = New ApiAuditoriaQualidadeList

                'Seta Retorno da Função
                oInfo.codigo_empresa = oSqlDataReader("codigo_empresa")
                oInfo.codigo_unidade = oSqlDataReader("codigo_unidade")
                oInfo.codigo_auditoria_interna = oSqlDataReader("codigo_auditoria_interna")
                oInfo.codigo_checklist = oSqlDataReader("codigo_checklist")
                oInfo.codigo = oSqlDataReader("codigo")
                oInfo.unidade = oSqlDataReader("unidade")
                oInfo.status = oSqlDataReader("status")
                oInfo.data_ultima_auditora = oSqlDataReader("data_ultima_auditoria")
                oInfo.data_proxima_auditoria = oSqlDataReader("data_proxima_auditoria")
                oInfo.descricao = oSqlDataReader("descricao")
                oInfo.pontos_possiveis = oSqlDataReader("pontos_possiveis")
                oInfo.pontos_realizados = oSqlDataReader("pontos_realizados")
                oInfo.pontos_conformes = oSqlDataReader("pontos_conformes")
                oInfo.pontos_nao_conformes = oSqlDataReader("pontos_nao_conformes")
                oInfo.nao_respondido = oSqlDataReader("nao_respondido")
                oInfo.nao_aplicaveis = oSqlDataReader("nao_aplicaveis")

                oReturn.Add(oInfo)

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: PICTURE :::"

    Public Sub InsertPicture(ByVal oAPIPicture As APIPicture)

        Try

            'Váriaveis Locais
            Dim oSqlParameter(5) As SqlParameter
            Dim i As Integer = 0
            Dim sImagePath As String = ""
            Dim sPath As String

            If oAPIPicture.tipo = "PMOC" Then
                sPath = Path.Combine("C:\SIM\PCM\SITE\IMAGE\", oAPIPicture.tipo, oAPIPicture.codigo_empresa.ToString(), oAPIPicture.codigo_unidade.ToString(), oAPIPicture.codigo.ToString(), Now.Year, Now.Month)
            Else
                sPath = Path.Combine("C:\SIM\PCM\SITE\IMAGE\", oAPIPicture.tipo, oAPIPicture.codigo_empresa.ToString(), oAPIPicture.codigo_unidade.ToString(), oAPIPicture.codigo.ToString())
            End If

            Dim sImageName As String = System.IO.Path.GetTempFileName.Replace(System.IO.Path.GetTempPath, "").Replace(".tmp", ".png")

            'Verifica se o diretório existe
            If (Directory.Exists(sPath) = False) Then
                'Cria Diretório
                Directory.CreateDirectory(sPath)
            End If

            'Salva Imagem
            sImagePath = System.IO.Path.Combine(sPath, sImageName)
            Dim imageBytes As Byte() = Convert.FromBase64String(oAPIPicture.base64)
            File.WriteAllBytes(sImagePath, imageBytes)

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oAPIPicture.codigo_empresa : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = oAPIPicture.codigo_unidade : i += 1

            'Seta Parametros - Código Documento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_documento"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = oAPIPicture.codigo : i += 1

            'Seta Parametros - Código Item Checklist
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "codigo_item_checklist"
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = oAPIPicture.codigo_item_checklist : i += 1

            'Seta Parametros - Tipo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "tipo"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = oAPIPicture.tipo : i += 1

            'Seta Parametros - Imagem
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "imagem"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 5000
            oSqlParameter(i).Value = IIf(sImagePath = "", DBNull.Value, sImagePath)

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_api_insert_picture", oSqlParameter)

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region

    Public Sub InsertLogAPI(ByVal sEndPoint As String,
                            ByVal sMethod As String,
                            ByVal sRequestBody As String,
                            ByVal sResponseBody As String,
                            ByVal sUsername As String)

        'Váriaveis Locais
        Dim i As Integer = 0
        Dim oSqlParameter(4) As SqlParameter

        Try

            'Seta Parametros - EndPoint
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "endpoint"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 250
            oSqlParameter(i).Value = sEndPoint : i += 1

            'Seta Parametros - Method
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "method"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 10
            oSqlParameter(i).Value = sMethod : i += 1

            'Seta Parametros - RequestBody
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "request_body"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Value = sRequestBody : i += 1

            'Seta Parametros - Response Body
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "response_body"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Value = sResponseBody : i += 1

            'Seta Parametros - User Name
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "username"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sUsername

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_api_insert_log", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function LoadAPIRefresh(ByVal sEndpoint As String) As List(Of String)

        'Váriaveis Locais
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(0) As SqlParameter
        Dim i As Integer = 0
        Dim oReturn As New List(Of String)

        Try

            'Seta Parametros - User Name
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "endpoint"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sEndpoint

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_api_select_log_refresh", oSqlParameter)

            While oSqlDataReader.Read
                oReturn.Add(oSqlDataReader.Item("request_body"))
            End While

            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

End Class
