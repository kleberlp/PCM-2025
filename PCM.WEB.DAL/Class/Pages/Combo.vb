Imports PCM.WEB.MODELS
Imports PCM.WEB.DAL.SQLHelper
Imports System.Data.SqlClient
Imports MS.Internal.Text.TextInterface
Imports OfficeOpenXml.FormulaParsing.Excel.Functions
Imports SYSPACK.WEB.MODELS

Public Class Combo

    Private sConnection As String

    Sub New(ByVal sCon As String)
        sConnection = sCon
    End Sub

    Public Function AgrupadoPorData() As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)

        Try

            Dim combo As New ListCombo

            combo.codigo = 1
            combo.descricao = "DIA"
            oCombo.Add(combo)
            combo = New ListCombo
            combo.codigo = 2
            combo.descricao = "SEMANA"
            oCombo.Add(combo)
            combo = New ListCombo
            combo.codigo = 3
            combo.descricao = "MÊS"
            oCombo.Add(combo)

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function Andar(ByVal iCodigoEmpresa As Integer,
                          Optional ByVal iCodigoUnidade As Integer = -1) As List(Of ListComboString)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListComboString)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(2) As SqlParameter
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
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_cadastro_basico_andar", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListComboString

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function AndarApartamento(ByVal iCodigoEmpresa As Integer,
                                     ByVal iCodigoUnidade As Integer,
                                     Optional ByVal sBloco As String = "") As List(Of ListComboString)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListComboString)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(2) As SqlParameter
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

            'Seta Parametros - Bloco
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "bloco"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = sBloco

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_cadastro_basico_andar_apartamento", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListComboString

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function Ano(ByVal iCodigoEmpresa As Integer) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(0) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_cadastro_basico_ano", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function TipoFiltroManutencao() As List(Of ListComboString)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListComboString)
        Dim oSqlDataReader As SqlDataReader
        Dim i As Integer = 0

        Try

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_static_tipo_filtro_manutencao")

            While oSqlDataReader.Read

                Dim combo As New ListComboString

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function MesAno(ByVal iCodigoEmpresa As Integer) As List(Of ListComboString)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListComboString)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(0) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_cadastro_basico_mes_ano_pmoc", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListComboString

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function Apartamento(ByVal iCodigoEmpresa As Integer,
                                ByVal iCodigoUnidade As Integer,
                                Optional ByVal iCodigoSetor As Integer = -1) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(2) As SqlParameter
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

            'Seta Parametros - Código Setor
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_setor"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoSetor

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_cadastro_basico_apartamento", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ApartamentoDedetizacao(ByVal iCodigoEmpresa As Integer,
                                           ByVal iCodigoUnidade As Integer,
                                           ByVal iCodigoUHAtividade As Integer) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(2) As SqlParameter
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

            'Seta Parametros - Código UH Atividade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_uh_atividade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = iCodigoUHAtividade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_uh_dedetizacao_apartamento", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ArCondicionado(ByVal iCodigoEmpresa As Integer,
                                   ByVal iCodigoUnidade As Integer) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(1) As SqlParameter
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
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_cadastro_basico_ar_condicionado", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ArCondicionadoPMOC(ByVal iCodigoEmpresa As Integer,
                                       ByVal iCodigoUnidade As Integer) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(1) As SqlParameter
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
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_cadastro_basico_ar_condicionado_pmoc", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function Atividade(ByVal iCodigoEmpresa As Integer,
                              Optional ByVal iCodigoUnidade As Integer = -1) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(1) As SqlParameter
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
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_cadastro_basico_atividade", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function AuditoriaQualidade(ByVal iCodigoEmpresa As Integer,
                                       ByVal iCodigoUnidade As Integer,
                                       ByVal iCodigoModulo As Integer) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(2) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Módulo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_modulo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoModulo : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_cadastro_basico_auditoria_qualidade", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function AuditoriaCorporativo(ByVal iCodigoEmpresa As Integer,
                                         ByVal iCodigoUnidade As Integer,
                                         ByVal iCodigoModulo As Integer) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(2) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Módulo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_modulo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoModulo : i += 1

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_cadastro_basico_auditoria_corporativo", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function Bloco(ByVal iCodigoEmpresa As Integer,
                          Optional ByVal iCodigoUnidade As Integer = -1) As List(Of ListComboString)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListComboString)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(1) As SqlParameter
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
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_cadastro_basico_bloco", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListComboString

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function BlocoApartamento(ByVal iCodigoEmpresa As Integer,
                                     ByVal iCodigoUnidade As Integer) As List(Of ListComboString)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListComboString)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(1) As SqlParameter
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
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_cadastro_basico_bloco_apartamento", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListComboString

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function Categoria(ByVal iCodigoEmpresa As Integer,
                              Optional ByVal iCodigoUnidade As Integer = -1) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(1) As SqlParameter
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
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_cadastro_basico_categoria", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function Cliente(ByVal codigoEmpresa As Integer,
                            Optional ByVal codigoUnidade As Integer = -1) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade)
            }

        Try

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_cadastro_basico_cliente", oSqlParameter)

                While oSqlDataReader.Read

                    Dim combo As New ListCombo
                    combo.codigo = oSqlDataReader.Item("codigo")
                    combo.descricao = oSqlDataReader.Item("descricao")
                    oCombo.Add(combo)

                End While

            End Using

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function Checklist2(ByVal iCodigoEmpresa As Integer) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(0) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_cadastro_basico_checklist", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function Checklist(ByVal iCodigoEmpresa As Integer,
                              ByVal iCodigoUnidade As Integer,
                              ByVal iCodigoTipoChecklist As Integer) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(2) As SqlParameter
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

            'Seta Parametros - Código Tipo Checklist
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_tipo_checklist"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoTipoChecklist

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_cadastro_basico_checklist", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function Modulo(ByVal iCodigoEmpresa As Integer,
                           ByVal iCodigoUsuario As Integer) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(1) As SqlParameter
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
            oSqlParameter(i).Value = iCodigoUsuario

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_cadastro_basico_modulo", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function Departamento(ByVal iCodigoEmpresa As Integer,
                                 ByVal iCodigoUsuario As Integer) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(1) As SqlParameter
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
            oSqlParameter(i).Value = iCodigoUsuario

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_cadastro_basico_departamento", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function Empresa() As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader

        Try

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_cadastro_basico_empresa")

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function EmpresaPMOC(ByVal iCodigoEmpresa As Integer,
                                ByVal iCodigoUnidade As Integer) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(1) As SqlParameter
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
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_cadastro_basico_empresa_pmoc", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function Equipamento(ByVal iCodigoEmpresa As Integer,
                                Optional ByVal iCodigoUnidade As Integer = -1,
                                Optional ByVal iCodigoSetor As Integer = -1,
                                Optional ByVal iCodigoApartamento As Integer = -1,
                                Optional ByVal lCodigoProgramada As Long = -1,
                                Optional ByVal iCodigoFamiliaEquipamento As Integer = -1) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(5) As SqlParameter
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

            'Seta Parametros - Código Programada
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_programada"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoProgramada : i += 1

            'Seta Parametros - Código Família Equipamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_familia_equipamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoFamiliaEquipamento

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_cadastro_basico_equipamento", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function EquipamentoAEB(ByVal iCodigoEmpresa As Integer,
                                   Optional ByVal iCodigoUnidade As Integer = -1,
                                   Optional ByVal iCodigoSetor As Integer = -1,
                                   Optional ByVal iCodigoApartamento As Integer = -1,
                                   Optional ByVal lCodigoProgramada As Long = -1) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(4) As SqlParameter
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

            'Seta Parametros - Código Programada
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_programada"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoProgramada

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_cadastro_basico_aeb_equipamento", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function Enxoval(ByVal codigoEmpresa As Integer,
                            Optional ByVal codigoUnidade As Integer = -1,
                            Optional ByVal codigoCliente As Integer = -1) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("codigo_cliente", SqlDbType.Int, codigoCliente)
            }

        Try

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_cadastro_basico_enxoval", oSqlParameter)

                While oSqlDataReader.Read

                    Dim combo As New ListCombo
                    combo.codigo = oSqlDataReader.Item("codigo")
                    combo.descricao = oSqlDataReader.Item("descricao")
                    oCombo.Add(combo)

                End While

            End Using

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function FamiliaEquipamento(ByVal iCodigoEmpresa As Integer,
                                       ByVal iCodigoUnidade As Integer) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(1) As SqlParameter
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
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_cadastro_basico_familia_equipamento", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function FormaCalculoGreenPlanet(ByVal iCodigoEmpresa As Integer) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(0) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_static_formula_calculo_green_planet", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function FormaLeitura() As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader

        Try

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_static_forma_leitura_item_medicao")

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function Fornecedor(ByVal iCodigoEmpresa As Integer,
                               ByVal iCodigoUnidade As Integer) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(1) As SqlParameter
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
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_cadastro_basico_fornecedor", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function FormaLancamentoPreventiva() As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader

        Try

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_static_forma_lancamento_preventiva")

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function Funcao(ByVal iCodigoEmpresa As Integer,
                           ByVal iCodigoUnidade As Integer) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(1) As SqlParameter
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
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_cadastro_basico_funcao", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function Funcionario(ByVal iCodigoEmpresa As Integer,
                                ByVal iCodigoUnidade As Integer,
                                ByVal iCodigoModulo As Integer) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(2) As SqlParameter
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

            'Seta Parametros - Código Módulo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_modulo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoModulo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_cadastro_basico_funcionario", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function GrupoChecklist(ByVal iCodigoEmpresa As Integer,
                                   ByVal iCodigoUnidade As Integer) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(1) As SqlParameter
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
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_cadastro_basico_grupo_checklist", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function GrupoItemMedicao(ByVal iCodigoEmpresa As Integer,
                                     ByVal iCodigoUnidade As Integer) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(1) As SqlParameter
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
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_cadastro_basico_grupo_item_medicao", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function GrupoProduto(ByVal iCodigoEmpresa As Integer,
                                 ByVal iCodigoUnidade As Integer) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(1) As SqlParameter
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
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_cadastro_basico_grupo_produto", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ItensGerais(ByVal iCodigoEmpresa As Integer,
                                Optional ByVal iCodigoUnidade As Integer = -1) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(1) As SqlParameter
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
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_cadastro_basico_itens_gerais", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ItemMedicao(ByVal iCodigoEmpresa As Integer,
                                ByVal iCodigoUnidade As Integer,
                                ByVal iCodigoGrupoItemMedicao As Integer) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(2) As SqlParameter
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
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_cadastro_basico_item_medicao", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function JustificativaApontamento(ByVal iCodigoEmpresa As Integer,
                                             Optional ByVal iCodigoUnidade As Integer = -1) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(1) As SqlParameter
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
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_cadastro_basico_justificativa_apontamento", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function JustificativaCancelamento(ByVal iCodigoEmpresa As Integer) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(1) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_cadastro_basico_justificativa_cancelamento_ordem_servico", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function JustificativaFalta(ByVal iCodigoEmpresa As Integer,
                                       Optional ByVal iCodigoUnidade As Integer = -1) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(1) As SqlParameter
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
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_cadastro_basico_justificativa_falta", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function Municipio(ByVal sUF As String) As List(Of ListComboString)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListComboString)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(0) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - UF
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "uf"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 2
            oSqlParameter(i).Value = sUF

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_static_municipio", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListComboString

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function Opcoes() As List(Of ListComboString)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListComboString)

        Try

            Dim combo As New ListComboString
            combo.codigo = "SIM"
            combo.descricao = "SIM"
            oCombo.Add(combo)

            combo = New ListComboString
            combo.codigo = "NÃO"
            combo.descricao = "NÃO"
            oCombo.Add(combo)

            combo = New ListComboString
            combo.codigo = "N/A"
            combo.descricao = "N/A"
            oCombo.Add(combo)

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function Perfil(ByVal iCodigoEmpresa As Integer,
                           ByVal iCodigoUsuario As Integer) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(1) As SqlParameter
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
            oSqlParameter(i).Value = iCodigoUsuario

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_cadastro_basico_perfil", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function Periodicidade(ByVal bChecklist As Boolean) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(0) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Checklist
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "checklist"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bChecklist

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_static_periodicidade", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function PotenciaArCondicionado() As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader

        Try

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_static_potencia_ar_condicionado")

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function Prioridade(ByVal iCodigoEmpresa As Integer,
                               Optional ByVal iCodigoUnidade As Integer = -1) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(1) As SqlParameter
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
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_cadastro_basico_prioridade", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function Produto(ByVal iCodigoEmpresa As Integer,
                            ByVal iCodigoUnidade As Integer) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(1) As SqlParameter
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
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_cadastro_basico_produto", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ProdutoLote(ByVal iCodigoEmpresa As Integer,
                                ByVal iCodigoUnidade As Integer,
                                ByVal lCodigoProduto As Long) As List(Of ListComboString)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListComboString)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(2) As SqlParameter
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

            'Seta Parametros - Código Produto
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_produto"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoProduto

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_cadastro_basico_produto_lote", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListComboString

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function Programada(ByVal iCodigoEmpresa As Integer,
                               ByVal iCodigoUnidade As Integer,
                               ByVal iCodigoTipoOrdemServico As Integer) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(2) As SqlParameter
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

            'Seta Parametros - Código Tipo Ordem de Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_tipo_ordem_servico"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoTipoOrdemServico

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_pcm_programada", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function Preventiva(ByVal iCodigoEmpresa As Integer,
                               ByVal iCodigoUnidade As Integer) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(1) As SqlParameter
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
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_pcm_preventiva", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function Rotina(ByVal iCodigoEmpresa As Integer,
                           ByVal iCodigoUnidade As Integer) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(1) As SqlParameter
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
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_pcm_rotina", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ChecklistItemAuditavel(ByVal iCodigoEmpresa As Integer,
                                           ByVal iCodigoUnidade As Integer,
                                           ByVal iCodigoModulo As Integer) As List(Of ListComboString)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListComboString)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(2) As SqlParameter
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

            'Seta Parametros - Código Modulo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_modulo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoModulo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_checklist_item_auditavel", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListComboString

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function RelatorioItensAuditaveis(ByVal iCodigoEmpresa As Integer,
                                             ByVal iCodigoUnidade As Integer,
                                             ByVal iCodigoModulo As Integer) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(2) As SqlParameter
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

            'Seta Parametros - Código Modulo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_modulo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoModulo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_cadastro_basico_relatorio_itens_auditaveis", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function FormaAnaliseConsumoEnxoval() As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader

        Try

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_static_forma_analise_consumo_enxoval")

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ResponsavelApartamento() As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader

        Try

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_static_responsavel_apartamento")

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ResponsavelDepartamento(ByVal iCodigoEmpresa As Integer,
                                            ByVal iCodigoUnidade As Integer,
                                            ByVal iCodigoDepartamento As Integer) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(2) As SqlParameter
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
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Departamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_departamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoDepartamento

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_cadastro_basico_responsavel_departamento", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function Setor(ByVal iCodigoEmpresa As Integer,
                          Optional ByVal iCodigoUnidade As Integer = -1) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(1) As SqlParameter
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
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_cadastro_basico_setor", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function Tarefa(ByVal iCodigoEmpresa As Integer,
                           ByVal iCodigoUnidade As Integer) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(1) As SqlParameter
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
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_qualidade_tarefa", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function TipoApartamento(ByVal iCodigoEmpresa As Integer,
                                    ByVal iCodigoUnidade As Integer) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(1) As SqlParameter
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
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_cadastro_basico_tipo_apartamento", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function TipoArCondicionado(ByVal iCodigoEmpresa As Integer,
                                       ByVal iCodigoUnidade As Integer) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(1) As SqlParameter
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
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_cadastro_basico_tipo_ar_condicionado", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function TipoCama(ByVal iCodigoEmpresa As Integer,
                             ByVal iCodigoUnidade As Integer) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(1) As SqlParameter
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
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_cadastro_basico_tipo_cama", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function TipoChecklist2(ByVal iCodigoEmpresa As Integer,
                                  ByVal iCodigoUnidade As Integer) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(1) As SqlParameter
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
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_cadastro_basico_tipo_checklist", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function TipoChecklist(ByVal iCodigoEmpresa As Integer,
                                  ByVal iCodigoUsuario As Integer) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(1) As SqlParameter
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
            oSqlParameter(i).Value = iCodigoUsuario

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_static_tipo_checklist", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function TipoDespesa(ByVal iCodigoEmpresa As Integer,
                                ByVal iCodigoUnidade As Integer) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(1) As SqlParameter
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
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_cadastro_basico_tipo_despesa", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function TipoEmpresa() As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader

        Try

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_static_tipo_empresa")

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function TipoFuncionario() As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader

        Try

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_static_tipo_funcionario")

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function TipoItemChecklist(ByVal iCodigoEmpresa As Integer) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(0) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_static_tipo_item_checklist", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function TipoNormaTecnica() As List(Of ListComboString)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListComboString)

        Try

            Dim combo As New ListComboString

            combo.codigo = "MANUAL"
            combo.descricao = "MANUAL"
            oCombo.Add(combo)

            combo = New ListComboString
            combo.codigo = "PIM"
            combo.descricao = "PIM"
            oCombo.Add(combo)

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function TipoOrdemServico(ByVal iCodigoEmpresa As Integer,
                                     Optional ByVal iOrdemServico As Integer = -1,
                                     Optional ByVal iProgramada As Integer = -1,
                                     Optional ByVal iRotina As Integer = -1) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(3) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Ordem Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ordem_servico"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = IIf(iOrdemServico = -1, DBNull.Value, IIf(iOrdemServico = 1, True, False)) : i += 1

            'Seta Parametros - Programada
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "programada"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = IIf(iProgramada = -1, DBNull.Value, IIf(iProgramada = 1, True, False)) : i += 1

            'Seta Parametros - Rotina
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "rotina"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = IIf(iRotina = -1, DBNull.Value, IIf(iRotina = 1, True, False))

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_static_tipo_ordem_servico", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function TipoServico() As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader

        Try

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_static_tipo_servico")

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function TipoServicoPMOC() As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader

        Try

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_static_tipo_servico_pmoc")

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function TipoUnidade() As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader

        Try

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_static_tipo_unidade")

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function UF() As List(Of ListComboString)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListComboString)
        Dim oSqlDataReader As SqlDataReader

        Try

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_static_uf")

            While oSqlDataReader.Read

                Dim combo As New ListComboString

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function UnidadePMOC(ByVal iCodigoEmpresa As Integer,
                                ByVal iCodigoUnidade As Integer) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(2) As SqlParameter
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
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_cadastro_basico_unidade_pmoc", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function Unidade(ByVal iCodigoEmpresa As Integer,
                            ByVal iCodigoUsuario As Integer,
                            ByVal bCadastro As Boolean) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(2) As SqlParameter
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

            'Seta Parametros - Cadastro
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "cadastro"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bCadastro

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_cadastro_basico_unidade", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function CadastroBasicoUpload() As List(Of ListComboString)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListComboString)
        Dim oSqlDataReader As SqlDataReader
        Dim i As Integer = 0

        Try

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_static_cadastro_basico_upload")

            While oSqlDataReader.Read

                Dim combo As New ListComboString

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function CadastroBasicoPMOC() As List(Of ListComboString)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListComboString)

        Try


            Dim combo As New ListComboString

            combo.codigo = "sp_insert_excel_pmoc"
            combo.descricao = "PMOC"

            oCombo.Add(combo)

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function Semana() As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim i As Integer = 0

        Try

            For i = 1 To 52

                Dim combo As New ListCombo

                combo.codigo = i
                combo.descricao = "SEM." & i.ToString()

                oCombo.Add(combo)

            Next

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function TipoProgramada() As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim i As Integer = 0

        Try

            Dim combo As New ListCombo

            combo.codigo = 3
            combo.descricao = "PREVENTIVA"
            oCombo.Add(combo)
            combo = New ListCombo
            combo.codigo = 7
            combo.descricao = "ROTINA"
            oCombo.Add(combo)

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function AnoPMOC(ByVal iCodigoEmpresa As Integer,
                            ByVal iCodigoUnidade As Integer) As List(Of ListComboString)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListComboString)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(1) As SqlParameter
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
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_pmoc_ano", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListComboString

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function TipoPMOC() As List(Of ListComboString)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListComboString)
        Dim oSqlDataReader As SqlDataReader
        Dim i As Integer = 0

        Try

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_pmoc_tipo")

            While oSqlDataReader.Read

                Dim combo As New ListComboString

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function DataPMOC(ByVal iCodigoEmpresa As Integer,
                             ByVal iCodigoUnidade As Integer) As List(Of ListComboString)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListComboString)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(1) As SqlParameter
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
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_pmoc_data", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListComboString

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function DataDashboard(ByVal iCodigoEmpresa As Integer,
                                  ByVal iCodigoUnidade As Integer) As List(Of ListComboString)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListComboString)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(1) As SqlParameter
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
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_dashboard_data", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListComboString

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function DataDashboardGovernanca(ByVal iCodigoEmpresa As Integer,
                                            ByVal iCodigoUnidade As Integer) As List(Of ListComboString)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListComboString)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(1) As SqlParameter
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
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_dashboard_governanca_data", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListComboString

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function DataProgramada(ByVal iCodigoEmpresa As Integer,
                                   ByVal iCodigoUnidade As Integer) As List(Of ListComboString)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListComboString)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(1) As SqlParameter
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
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_programada_data", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListComboString

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function DataGovernanca(ByVal iCodigoEmpresa As Integer,
                                   ByVal iCodigoUnidade As Integer) As List(Of ListComboString)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListComboString)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(1) As SqlParameter
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
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_data_governanca", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListComboString

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function Usuario(ByVal iCodigoEmpresa As Integer,
                            ByVal iCodigoUnidade As Integer) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(1) As SqlParameter
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
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_cadastro_basico_usuario", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function UsuarioDepartamento(ByVal iCodigoEmpresa As Integer,
                                        ByVal iCodigoUnidade As Integer,
                                        ByVal iCodigoDepartamento As Integer) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(2) As SqlParameter
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

            'Seta Parametros - Código Departamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_departamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoDepartamento

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_cadastro_basico_usuario", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function SimNao() As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)

        Try

            Dim combo As New ListCombo
            combo.codigo = 1
            combo.descricao = "SIM"
            oCombo.Add(combo)

            combo = New ListCombo
            combo.codigo = 0
            combo.descricao = "NÃO"
            oCombo.Add(combo)

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function HorasFaltas() As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)

        Try

            Dim combo As New ListCombo
            combo.codigo = 0
            combo.descricao = "FALTAS"
            oCombo.Add(combo)

            combo = New ListCombo
            combo.codigo = 1
            combo.descricao = "HORAS"
            oCombo.Add(combo)

            combo = New ListCombo
            combo.codigo = 2
            combo.descricao = "HORAS / FALTAS"
            oCombo.Add(combo)

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function Solicitante(ByVal iCodigoEmpresa As Integer,
                                ByVal iCodigoUsuario As Integer,
                                ByVal iCodigoUnidade As Integer) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(2) As SqlParameter
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
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_cadastro_basico_solicitante", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function StatusAtividade() As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader

        Try

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_static_status_atividade")

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function StatusAuditoriaQualidade(ByVal bHistorico As Boolean) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(0) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Histórico
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "historico"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bHistorico : i += 1

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_static_status_auditoria_qualidade", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function StatusAuditoriaCorporativo(ByVal bHistorico As Boolean) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(0) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Histórico
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "historico"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bHistorico : i += 1

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_static_status_auditoria_corporativo", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function StatusEstoque() As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader

        Try

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_static_status_estoque")

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function StatusManutencao() As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader

        Try

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_static_status_manutencao")

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function StatusRequisicao() As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader

        Try

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_static_status_requisicao")

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function StatusRequisicaoCompra() As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader

        Try

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_static_status_requisicao_compra")

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function StatusPMOC() As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader

        Try

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_static_status_pmoc")

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function StatusUHAtividade() As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader

        Try

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_static_status_uh_atividade")

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function StatusPreventiva() As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)

        Try

            Dim combo As New ListCombo
            combo.codigo = -5 : combo.descricao = "PREVENTIVA EM ATRASO" : oCombo.Add(combo)
            combo = New ListCombo : combo.codigo = -2 : combo.descricao = "PREVENTIVA REALIZADA" : oCombo.Add(combo)
            combo = New ListCombo : combo.codigo = -1 : combo.descricao = "PREVENTIVA A REALIZAR NO MÊS CORRENTE" : oCombo.Add(combo)

            'Retorno da Função
            Return oCombo

        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function StatusOrdemCompra() As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader

        Try

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_static_status_ordem_compra")

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function StatusFrontOffice(ByVal iCodigoEmpresa As Integer,
                                      ByVal iCodigoUnidade As Integer) As List(Of ListComboString)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListComboString)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(1) As SqlParameter
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
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_static_status_front_office", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListComboString

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function StatusRoom(ByVal iCodigoEmpresa As Integer,
                               ByVal iCodigoUnidade As Integer) As List(Of ListComboString)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListComboString)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(1) As SqlParameter
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
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_static_status_room", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListComboString

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function FuncionarioGovernanca(ByVal iCodigoEmpresa As Integer,
                                          ByVal iCodigoUnidade As Integer) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(2) As SqlParameter
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
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_governanca_funcionario", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function FuncionarioGovernancaCamareira(ByVal iCodigoEmpresa As Integer,
                                                   ByVal iCodigoUnidade As Integer) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(2) As SqlParameter
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
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_governanca_funcionario_camareira", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function FuncionarioGovernancaVistoriador(ByVal iCodigoEmpresa As Integer,
                                                     ByVal iCodigoUnidade As Integer) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader
        Dim oSqlParameter(2) As SqlParameter
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
            oSqlParameter(i).Value = iCodigoUnidade

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_governanca_funcionario_vistoriador", oSqlParameter)

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function TipoGovernanca() As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlDataReader As SqlDataReader

        Try

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_tipo_governanca")

            While oSqlDataReader.Read

                Dim combo As New ListCombo

                combo.codigo = oSqlDataReader.Item("codigo")
                combo.descricao = oSqlDataReader.Item("descricao")

                oCombo.Add(combo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function LoadCombo(ByVal storedProcedure As String,
                              Optional ByVal codigoEmpresa As Integer = -1,
                              Optional ByVal codigoUnidade As Integer = -1) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)

        Try

            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade)
            }

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, storedProcedure, oSqlParameter)

                While oSqlDataReader.Read

                    Dim combo As New ListCombo

                    combo.codigo = oSqlDataReader.Item("codigo")
                    combo.descricao = oSqlDataReader.Item("descricao")

                    oCombo.Add(combo)

                End While

            End Using

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function LoadComboString(ByVal storedProcedure As String,
                                    Optional ByVal codigoEmpresa As Integer = -1,
                                    Optional ByVal codigoUnidade As Integer = -1) As List(Of ListComboString)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListComboString)

        Try

            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade)
            }

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, storedProcedure, oSqlParameter)

                While oSqlDataReader.Read

                    Dim combo As New ListComboString

                    combo.codigo = oSqlDataReader.Item("codigo")
                    combo.descricao = oSqlDataReader.Item("descricao")

                    oCombo.Add(combo)

                End While

            End Using

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function LavagemRelatorioAgrupadoPor() As List(Of ListComboString)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListComboString)

        Try

            Dim combo As New ListComboString
            combo.codigo = "codigo_equipamento"
            combo.descricao = "EQUIPAMENTO"
            oCombo.Add(combo)

            combo = New ListComboString
            combo.codigo = "cliente"
            combo.descricao = "CLIENTE"
            oCombo.Add(combo)

            combo = New ListComboString
            combo.codigo = "funcionario"
            combo.descricao = "FUNCIONÁRIO"
            oCombo.Add(combo)

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function MesLavanderia(ByVal codigoEmpresa As Integer,
                                  ByVal codigoUnidade As Integer) As List(Of ListComboString)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListComboString)
        Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade)
            }

        Try

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_lavagem_mes", oSqlParameter)

                While oSqlDataReader.Read

                    Dim combo As New ListComboString
                    combo.codigo = oSqlDataReader.Item("codigo")
                    combo.descricao = oSqlDataReader.Item("descricao")
                    oCombo.Add(combo)

                End While

            End Using

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function UOM(ByVal codigoEmpresa As Integer) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlParameter As SqlParameter() = {
            CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa)
        }

        Try

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_cadastro_basico_unidade_medida", oSqlParameter)

                While oSqlDataReader.Read

                    Dim combo As New ListCombo

                    combo.codigo = oSqlDataReader.Item("codigo")
                    combo.descricao = oSqlDataReader.Item("descricao")

                    oCombo.Add(combo)

                End While

            End Using

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function OrdemCompra(ByVal codigoEmpresa As Integer,
                                ByVal codigoUnidade As Integer) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlParameter As SqlParameter() = {
            CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
            CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade)
        }

        Try

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_estoque_ordem_compra", oSqlParameter)

                While oSqlDataReader.Read

                    Dim combo As New ListCombo With {
                        .codigo = SafeGetLong(oSqlDataReader, "codigo"),
                        .descricao = SafeGetString(oSqlDataReader, "descricao")
                    }

                    oCombo.Add(combo)

                End While

            End Using

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function OrdemCompraProduto(ByVal codigoOrdemCompra As Long) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlParameter As SqlParameter() = {
            CriarParametro("codigo_ordem_compra", SqlDbType.BigInt, codigoOrdemCompra)
        }

        Try

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_estoque_ordem_compra_produto", oSqlParameter)

                While oSqlDataReader.Read

                    Dim combo As New ListCombo With {
                        .codigo = SafeGetLong(oSqlDataReader, "codigo"),
                        .descricao = SafeGetString(oSqlDataReader, "descricao")
                    }

                    oCombo.Add(combo)

                End While

            End Using

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function CentroCusto(ByVal codigoEmpresa As Integer,
                                ByVal codigoUnidade As Integer) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlParameter As SqlParameter() = {
            CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
            CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade)
        }

        Try

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_cadatro_basico_centro_custo", oSqlParameter)

                While oSqlDataReader.Read

                    Dim combo As New ListCombo With {
                        .codigo = SafeGetLong(oSqlDataReader, "codigo"),
                        .descricao = SafeGetString(oSqlDataReader, "descricao")
                    }

                    oCombo.Add(combo)

                End While

            End Using

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ProdutoLoteSaida(ByVal codigoEmpresa As Integer,
                                     ByVal codigoProduto As Long) As List(Of ListComboString)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListComboString)
        Dim oSqlParameter As SqlParameter() = {
            CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
            CriarParametro("codigo_produto", SqlDbType.BigInt, codigoProduto)
        }

        Try

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_estoque_produto_lote_saida", oSqlParameter)

                While oSqlDataReader.Read

                    Dim combo As New ListComboString With {
                        .codigo = SafeGetString(oSqlDataReader, "codigo"),
                        .descricao = SafeGetString(oSqlDataReader, "descricao")
                    }

                    oCombo.Add(combo)

                End While

            End Using

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function OrdemServico(ByVal codigoEmpresa As Integer,
                                 ByVal codigoUnidade As Integer) As List(Of ListCombo)

        'Váriaveis Locais
        Dim oCombo As New List(Of ListCombo)
        Dim oSqlParameter As SqlParameter() = {
            CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
            CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade)
        }

        Try

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_combo_estoque_ordem_servico", oSqlParameter)

                While oSqlDataReader.Read

                    Dim combo As New ListCombo With {
                        .codigo = SafeGetLong(oSqlDataReader, "codigo"),
                        .descricao = SafeGetString(oSqlDataReader, "descricao")
                    }

                    oCombo.Add(combo)

                End While

            End Using

            'Retorno da Função
            Return oCombo

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

End Class
