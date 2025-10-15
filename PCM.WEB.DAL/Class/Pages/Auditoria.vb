Imports PCM.WEB.MODELS
Imports System.DirectoryServices
Imports System.DirectoryServices.ActiveDirectory
Imports PCM.WEB.DAL.SQLHelper
Imports System.Data.SqlClient
Imports System.Text
Imports System.Net
Imports System.IO

Public Class Auditoria

    Private sConnection As String

    Sub New(ByVal sCon As String)
        sConnection = sCon
    End Sub

#Region "::: AUDITORIA EXTERNA :::"

    Public Sub InsertAuditoria(ByVal iCodigoEmpresa As Integer,
                               ByVal iCodigoUsuario As Integer,
                               ByVal iCodigoUnidade As Integer,
                               ByVal iCodigoFornecedor As Integer,
                               ByVal sDescricao As String,
                               ByVal sData As String,
                               ByVal sDataValidade As String,
                               ByVal sValor As String,
                               ByVal sPathArquivo As String,
                               ByVal sArquivo As String)

        'Variaveis Locais
        Dim oSqlParameter(9) As SqlParameter
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

            'Seta Parametros - Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Fornecedor
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_fornecedor"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoFornecedor : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sDescricao.ToUpper() : i += 1

            'Seta Parametros - Data
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sData : i += 1

            'Seta Parametros - Data Validade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_validade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataValidade : i += 1

            'Seta Parametros - Valor
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "valor"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = CDbl(sValor.Replace("R$ ", "").Replace(".", "")) : i += 1

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
            oSqlParameter(i).Value = sPathArquivo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_auditoria_externa", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub DeleteAuditoria(ByVal iCodigoEmpresa As Integer,
                               ByVal iCodigoUsuario As Integer,
                               ByVal iCodigoUnidade As Integer,
                               ByVal lCodigo As Long)

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
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_auditoria_externa", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InfoAuditoria(ByVal iCodigoEmpresa As Integer,
                             ByVal iCodigoUnidade As Integer,
                             ByVal lCodigo As Long,
                             ByRef oAuditoria As AuditoriaExterna)

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
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_auditoria_externa_dados", oSqlParameter)

            While oSqlDataReader.Read

                oAuditoria = New AuditoriaExterna
                oAuditoria.codigo = oSqlDataReader.Item("codigo")
                oAuditoria.descricao = oSqlDataReader.Item("descricao")
                oAuditoria.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oAuditoria.unidade = oSqlDataReader.Item("unidade")
                oAuditoria.data = oSqlDataReader.Item("data")
                oAuditoria.data_validade = oSqlDataReader.Item("data_validade")
                oAuditoria.fornecedor = oSqlDataReader.Item("fornecedor")
                oAuditoria.path_arquivo = oSqlDataReader.Item("path_arquivo")
                oAuditoria.valor = oSqlDataReader.Item("valor")
                oAuditoria.valor_texto = oSqlDataReader.Item("valor")

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function IndexAuditoria(ByVal iCodigoEmpresa As Integer,
                                   ByVal iCodigoUnidade As Integer,
                                   ByVal sDataInicio As String,
                                   ByVal sDataTermino As String,
                                   ByVal iCodigoFornecedor As Integer) As List(Of AuditoriaExterna)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of AuditoriaExterna)
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

            'Seta Parametros - Código Fornecedor
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_fornecedor"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoFornecedor

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_auditoria_externa", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New AuditoriaExterna

                oInfo.codigo = oSqlDataReader.Item("codigo")
                oInfo.descricao = oSqlDataReader.Item("descricao")
                oInfo.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oInfo.unidade = oSqlDataReader.Item("unidade")
                oInfo.data = oSqlDataReader.Item("data")
                oInfo.data_validade = oSqlDataReader.Item("data_validade")
                oInfo.fornecedor = oSqlDataReader.Item("fornecedor")
                oInfo.path_arquivo = oSqlDataReader.Item("path_arquivo")
                oInfo.valor = oSqlDataReader.Item("valor")
                oInfo.valor_texto = oSqlDataReader.Item("valor")

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

#Region "::: AUDITORIA CORPORATIVO :::"

    Public Function IndexAuditoriaCorporativo(ByVal iCodigoEmpresa As Integer,
                                              ByVal iCodigoUnidade As Integer,
                                              ByVal sDataInicio As String,
                                              ByVal sDataTermino As String,
                                              ByVal iCodigoAuditoriaInterna As Integer,
                                              ByVal iCodigoModulo As Integer) As List(Of MODELS.Auditoria)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of MODELS.Auditoria)
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

            'Seta Parametros - Código Auditoria Interna
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_auditoria_interna"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoAuditoriaInterna : i += 1

            'Seta Parametros - Código Módulo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_modulo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoModulo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_auditoria_corporativo", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New MODELS.Auditoria

                oInfo.codigo_empresa = oSqlDataReader("codigo_empresa")
                oInfo.codigo_unidade = oSqlDataReader("codigo_unidade")
                oInfo.unidade = oSqlDataReader("unidade")
                oInfo.codigo_auditoria_interna = oSqlDataReader("codigo_auditoria_interna")
                oInfo.codigo_auditoria = oSqlDataReader("codigo_auditoria")
                oInfo.codigo_checklist = oSqlDataReader("codigo_checklist")
                oInfo.descricao = oSqlDataReader("descricao")
                oInfo.status = oSqlDataReader("status")
                oInfo.descricao_status = oSqlDataReader("descricao_status")
                oInfo.css_class = oSqlDataReader("css_class")
                oInfo.data = oSqlDataReader("data")
                oInfo.usuario = oSqlDataReader("usuario")
                oInfo.pontos_possiveis = oSqlDataReader("pontos_possiveis")
                oInfo.pontos_realizados = oSqlDataReader("pontos_realizados")
                oInfo.conforme = oSqlDataReader("conforme")
                oInfo.nao_conforme = oSqlDataReader("nao_conforme")
                oInfo.nao_respondido = oSqlDataReader("nao_respondido")
                oInfo.nao_aplicavel = oSqlDataReader("nao_aplicavel")
                oInfo.nota = oSqlDataReader("nota")

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

    Public Sub InsertAuditoriaCorporativo(ByVal iCodigoEmpresa As Integer,
                                          ByVal iCodigoUsuario As Integer,
                                          ByVal iCodigoUnidade As Integer,
                                          ByVal sNumeroDocumento As String,
                                          ByVal iCodigoAuditoriaCorporativo As Integer,
                                          ByRef lCodigo As Long)

        'Variaveis Locais
        Dim oSqlParameter(5) As SqlParameter
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

            'Seta Parametros - Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Nº Documento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "numero_documento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sNumeroDocumento : i += 1

            'Seta Parametros - Código Auditoria Interna
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_auditoria_interna"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoAuditoriaCorporativo : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Output
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_auditoria_corporativo", oSqlParameter)

            lCodigo = oSqlParameter(i).Value

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub DeleteAuditoriaCorporativo(ByVal iCodigoEmpresa As Integer,
                                          ByVal iCodigoUsuario As Integer,
                                          ByVal iCodigoUnidade As Integer,
                                          ByVal lCodigo As Long)

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
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_auditoria_corporativo", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region

#Region "::: LAUDO :::"

    Public Sub InsertLaudo(ByVal iCodigoEmpresa As Integer,
                           ByVal iCodigoUsuario As Integer,
                           ByVal iCodigoUnidade As Integer,
                           ByVal iCodigoModulo As Integer,
                           ByVal lCodigoPCMProgramada As Long,
                           ByVal iCodigoFornecedor As Integer,
                           ByVal sData As String,
                           ByVal sDataValidade As String,
                           ByVal sValor As String,
                           ByVal sEquipamento As String,
                           ByVal sPathArquivo As String,
                           ByVal sArquivo As String)

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

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Código Módulo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_modulo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoModulo : i += 1

            'Seta Parametros - Código PCM Programada
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_pcm_programada"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoPCMProgramada : i += 1

            'Seta Parametros - Fornecedor
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_fornecedor"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoFornecedor : i += 1

            'Seta Parametros - Data
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sData : i += 1

            'Seta Parametros - Data Validade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_validade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataValidade : i += 1

            'Seta Parametros - Valor
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "valor"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = CDbl(IIf(IsNumeric(sValor.Replace("R$ ", "").Replace(".", "")), sValor.Replace("R$ ", "").Replace(".", ""), 0)) : i += 1

            'Seta Parametros - Equipamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "equipamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 500
            oSqlParameter(i).Value = sEquipamento : i += 1

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
            oSqlParameter(i).Value = sPathArquivo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_auditoria_laudo", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub DeleteLaudo(ByVal iCodigoEmpresa As Integer,
                           ByVal iCodigoUsuario As Integer,
                           ByVal iCodigoUnidade As Integer,
                           ByVal lCodigo As Long)

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
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_auditoria_laudo", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InfoLaudo(ByVal iCodigoEmpresa As Integer,
                         ByVal iCodigoUnidade As Integer,
                         ByVal lCodigo As Long,
                         ByRef oLaudo As Laudo)

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
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_auditoria_laudo_dados", oSqlParameter)

            While oSqlDataReader.Read

                oLaudo = New Laudo
                oLaudo.codigo = oSqlDataReader.Item("codigo")
                oLaudo.descricao = oSqlDataReader.Item("descricao")
                oLaudo.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oLaudo.unidade = oSqlDataReader.Item("unidade")
                oLaudo.data = oSqlDataReader.Item("data")
                oLaudo.data_validade = oSqlDataReader.Item("data_validade")
                oLaudo.fornecedor = oSqlDataReader.Item("fornecedor")
                oLaudo.path_arquivo = oSqlDataReader.Item("path_arquivo")
                oLaudo.valor = oSqlDataReader.Item("valor")
                oLaudo.valor_texto = oSqlDataReader.Item("valor")
                oLaudo.equipamento = InfoLaudoEquipamento(iCodigoEmpresa:=iCodigoEmpresa,
                                                          iCodigoUnidade:=iCodigoUnidade,
                                                          lCodigo:=lCodigo)

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function InfoLaudoEquipamento(ByVal iCodigoEmpresa As Integer,
                                         ByVal iCodigoUnidade As Integer,
                                         ByVal lCodigo As Long) As List(Of LaudoEquipamento)

        'Variaveis Locais
        Dim oSqlParameter(2) As SqlParameter
        Dim oReturn As New List(Of LaudoEquipamento)
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
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_auditoria_laudo_dados_equipamento", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New LaudoEquipamento
                oInfo.tag = oSqlDataReader.Item("tag")
                oInfo.descricao = oSqlDataReader.Item("descricao")
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

    Public Function IndexLaudo(ByVal iCodigoEmpresa As Integer,
                               ByVal iCodigoUnidade As Integer,
                               ByVal iCodigoModulo As Integer,
                               ByVal sDataInicio As String,
                               ByVal sDataTermino As String,
                               ByVal lCodigoPCMProgramada As Long,
                               ByVal iCodigoFornecedor As Integer,
                               ByVal lCodigoEquipamento As Long) As List(Of Laudo)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of Laudo)
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

            'Seta Parametros - Código Módulo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_modulo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoModulo : i += 1

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
            oSqlParameter(i).Value = iCodigoFornecedor : i += 1

            'Seta Parametros - Código Equipamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_equipamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoEquipamento

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_auditoria_laudo", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New Laudo

                oInfo.codigo = oSqlDataReader.Item("codigo")
                oInfo.descricao = oSqlDataReader.Item("descricao")
                oInfo.codigo_pcm_programada = oSqlDataReader.Item("codigo_pcm_programada")
                oInfo.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oInfo.unidade = oSqlDataReader.Item("unidade")
                oInfo.data = oSqlDataReader.Item("data")
                oInfo.data_validade = oSqlDataReader.Item("data_validade")
                oInfo.fornecedor = oSqlDataReader.Item("fornecedor")
                oInfo.path_arquivo = oSqlDataReader.Item("path_arquivo")
                oInfo.valor = oSqlDataReader.Item("valor")
                oInfo.valor_texto = oSqlDataReader.Item("valor")
                oInfo.data_input = oSqlDataReader.Item("data_input")
                'oInfo.equipamento = InfoLaudoEquipamento(iCodigoEmpresa:=iCodigoEmpresa,
                '                                         iCodigoUnidade:=oSqlDataReader.Item("codigo_unidade"),
                '                                         lCodigo:=oSqlDataReader.Item("codigo"))

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

#Region "::: NORMAS E PROCEDIMENTOS :::"

    Public Sub InsertNormasProcedimentos(ByVal iCodigoEmpresa As Integer,
                                         ByVal iCodigoUsuario As Integer,
                                         ByVal iCodigoUnidade As Integer,
                                         ByVal sDescricao As String,
                                         ByVal sTipo As String,
                                         ByVal sComentario As String,
                                         ByVal sPathArquivo As String,
                                         ByVal sArquivo As String,
                                         ByVal bAtivo As Boolean)

        'Variaveis Locais
        Dim oSqlParameter(8) As SqlParameter
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

            'Seta Parametros - Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoUnidade = -1, DBNull.Value, iCodigoUnidade) : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sDescricao : i += 1

            'Seta Parametros - Tipo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "tipo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sTipo : i += 1

            'Seta Parametros - Comentário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "comentario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 5000
            oSqlParameter(i).Value = sComentario : i += 1

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

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_cadastro_basico_normas_procedimentos", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateNormasProcedimentos(ByVal iCodigoEmpresa As Integer,
                                         ByVal iCodigoUsuario As Integer,
                                         ByVal iCodigoUnidade As Integer,
                                         ByVal sDescricao As String,
                                         ByVal sTipo As String,
                                         ByVal sComentario As String,
                                         ByVal sArquivo As String,
                                         ByVal sPathArquivo As String,
                                         ByVal bAtivo As Boolean,
                                         ByVal iCodigo As Integer)

        'Variaveis Locais
        Dim oSqlParameter(9) As SqlParameter
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

            'Seta Parametros - Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = IIf(iCodigoUnidade = -1, DBNull.Value, iCodigoUnidade) : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sDescricao : i += 1

            'Seta Parametros - Tipo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "tipo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sTipo : i += 1

            'Seta Parametros - Comentário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "comentario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 5000
            oSqlParameter(i).Value = sComentario : i += 1

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

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_cadastro_basico_normas_procedimentos", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub DeleteNormasProcedimentos(ByVal iCodigoEmpresa As Integer,
                                         ByVal iCodigoUsuario AS Integer,
                                         ByVal iCodigo As Integer)

        'Variaveis Locais
        Dim oSqlParameter(2) As SqlParameter
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
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_cadastro_basico_normas_procedimentos", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InfoNormasProcedimentos(ByVal iCodigoEmpresa As Integer,
                                       ByVal iCodigo As Integer,
                                       ByRef oNormasProcedimentos As NormasProcedimentos)

        'Variaveis Locais
        Dim oSqlParameter(1) As SqlParameter
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
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_normas_procedimentos_dados", oSqlParameter)

            While oSqlDataReader.Read

                oNormasProcedimentos = New NormasProcedimentos
                oNormasProcedimentos.codigo = oSqlDataReader.Item("codigo")
                oNormasProcedimentos.descricao = oSqlDataReader.Item("descricao")
                oNormasProcedimentos.tipo = oSqlDataReader.Item("tipo")
                oNormasProcedimentos.comentario = oSqlDataReader.Item("comentario")
                oNormasProcedimentos.arquivo = oSqlDataReader.Item("arquivo")
                oNormasProcedimentos.path_arquivo = oSqlDataReader.Item("path_arquivo")
                oNormasProcedimentos.ativo = oSqlDataReader.Item("ativo")
                oNormasProcedimentos.unidade = oSqlDataReader.Item("unidade")
                oNormasProcedimentos.codigo_unidade = oSqlDataReader.Item("codigo_unidade")

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function IndexNormasProcedimentos(ByVal iCodigoEmpresa As Integer,
                                             ByVal iCodigoUsuario AS Integer,
                                             ByVal iCodigoUnidade As Integer) As List(Of NormasProcedimentos)

        Try

            'Váriaveis Locais
            Dim oNormasProcedimentos As New List(Of NormasProcedimentos)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(2) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

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
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_normas_procedimentos", oSqlParameter)

            While oSqlDataReader.Read

                Dim oNormasProcedimentosInfo As New NormasProcedimentos

                oNormasProcedimentosInfo.descricao = oSqlDataReader.Item("descricao")
                oNormasProcedimentosInfo.tipo = oSqlDataReader.Item("tipo")
                oNormasProcedimentosInfo.ativo = oSqlDataReader.Item("ativo")
                oNormasProcedimentosInfo.codigo = oSqlDataReader.Item("codigo")
                oNormasProcedimentosInfo.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oNormasProcedimentosInfo.unidade = oSqlDataReader.Item("unidade")
                oNormasProcedimentosInfo.path_arquivo = oSqlDataReader.Item("path_arquivo")
                oNormasProcedimentosInfo.comentario = oSqlDataReader.Item("comentario")

                oNormasProcedimentos.Add(oNormasProcedimentosInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oNormasProcedimentos

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ValidaNormasProcedimentos(ByVal iCodigoEmpresa As Integer,
                                              ByVal iCodigoUnidade As Integer,
                                              ByVal sDescricao As String,
                                              ByVal iCodigo As Integer) As Boolean

        Try

            'Váriaveis Locais
            Dim oSqlParameter(3) As SqlParameter
            Dim i As Integer = 0
            Dim iReturn As Integer

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
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sDescricao

            'Executa Query
            iReturn = ExecuteScalar(sConnection, CommandType.StoredProcedure, "sp_validate_cadastro_basico_normas_procedimentos", oSqlParameter)

            'Retorno da Função
            Return (IIf(iReturn > 0, False, True))

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: AUDITORIA :::"

    Public Function RelatorioAuditoria(ByVal iCodigoEmpresa As Integer,
                                       ByVal iCodigoUnidade As Integer,
                                       ByVal sChecklist As String()) As List(Of RelatorioAuditoria)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of RelatorioAuditoria)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(3) As SqlParameter
            Dim i As Integer = 0

            For Each sCodigo As String In sChecklist

                i = 0

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
                oSqlParameter(i).SqlDbType = SqlDbType.BigInt
                oSqlParameter(i).Value = sCodigo.Split("|")(0) : i += 1

                'Seta Parametros - Código Checklist Item
                oSqlParameter(i) = New SqlParameter
                oSqlParameter(i).ParameterName = "codigo_checklist_item"
                oSqlParameter(i).Direction = ParameterDirection.Input
                oSqlParameter(i).SqlDbType = SqlDbType.Int
                oSqlParameter(i).Value = sCodigo.Split("|")(1)

                'Executa Query
                oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_auditoria_relatorio_checklist", oSqlParameter)

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

            Next

            'Retorno da Função
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function RelatorioAuditoriaData(ByVal iCodigoEmpresa As Integer,
                                           ByVal iCodigoUnidade As Integer,
                                           ByVal sChecklist As String(),
                                           ByVal sDataInicio As String,
                                           ByVal sDataTermino As String) As List(Of RelatorioAuditoriaDataValor)

        Try

            Dim oReturn As New List(Of RelatorioAuditoriaDataValor)
            Dim oInfo As RelatorioAuditoriaDataValor

            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(5) As SqlParameter
            Dim i As Integer = 0

            For Each sCodigo As String In sChecklist

                i = 0

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

                'Seta Parametros - Código Checklist
                oSqlParameter(i) = New SqlParameter
                oSqlParameter(i).ParameterName = "codigo_checklist"
                oSqlParameter(i).Direction = ParameterDirection.Input
                oSqlParameter(i).SqlDbType = SqlDbType.BigInt
                oSqlParameter(i).Value = sCodigo.Split("|")(0) : i += 1

                'Seta Parametros - Código Checklist Item
                oSqlParameter(i) = New SqlParameter
                oSqlParameter(i).ParameterName = "codigo_checklist_item"
                oSqlParameter(i).Direction = ParameterDirection.Input
                oSqlParameter(i).SqlDbType = SqlDbType.Int
                oSqlParameter(i).Value = sCodigo.Split("|")(1)

                'Executa Query
                oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_auditoria_relatorio_resultado", oSqlParameter)

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

            Next

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

                oReturn.Add(oInfo )

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

#Region "::: AUDITORIA HISTÓRICO :::"

    Public Function AuditoriaHistorico(ByVal iCodigoEmpresa As Integer,
                                       ByVal iCodigoUnidade As Integer,
                                       ByVal sDataInicio As String,
                                       ByVal sDataTermino As String,
                                       ByVal iCodigoAuditoriaInterna As Integer,
                                       ByVal iStatus As Integer) As List(Of QAAuditoriaHistorico)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of MODELS.QAAuditoriaHistorico)
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

            'Seta Parametros - Código Auditoria Interna
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_auditoria_interna"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoAuditoriaInterna : i += 1

            'Seta Parametros - Status
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "status"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iStatus

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_auditoria_corporativo_historico", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New QAAuditoriaHistorico


                oInfo.codigo_empresa = oSqlDataReader("codigo_empresa")
                oInfo.codigo_unidade = oSqlDataReader("codigo_unidade")
                oInfo.unidade = oSqlDataReader("unidade")
                oInfo.codigo_auditoria_interna = oSqlDataReader("codigo_auditoria_interna")
                oInfo.codigo_auditoria = oSqlDataReader("codigo_auditoria")
                oInfo.codigo_checklist = oSqlDataReader("codigo_checklist")
                oInfo.descricao = oSqlDataReader("descricao")
                oInfo.usuario = oSqlDataReader("usuario")
                oInfo.status = oSqlDataReader("status")
                oInfo.descricao_status = oSqlDataReader("descricao_status")
                oInfo.css_class = oSqlDataReader("css_class")
                oInfo.data = oSqlDataReader("data")
                oInfo.pontos_possiveis = oSqlDataReader("pontos_possiveis")
                oInfo.pontos_realizados = oSqlDataReader("pontos_realizados")
                oInfo.conforme = oSqlDataReader("conforme")
                oInfo.nao_conforme = oSqlDataReader("nao_conforme")
                oInfo.nao_respondido = oSqlDataReader("nao_respondido")
                oInfo.nao_aplicavel = oSqlDataReader("nao_aplicavel")
                oInfo.nota = oSqlDataReader("nota")

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

    Public Function AuditoriaCronograma(ByVal iCodigoEmpresa As Integer,
                                        ByVal iCodigoUnidade As Integer,
                                        ByVal iMes As Integer,
                                        ByVal iAno As Integer) As List(Of QAAuditoriaCronograma)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of QAAuditoriaCronograma)
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
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_qualidade_auditoria_cronograma", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New QAAuditoriaCronograma

                oInfo.codigo_empresa = oSqlDataReader("codigo_empresa")
                oInfo.codigo_unidade = oSqlDataReader("codigo_unidade")
                oInfo.unidade = oSqlDataReader("unidade")
                oInfo.codigo_auditoria_interna = oSqlDataReader("codigo_auditoria_interna")
                oInfo.descricao = oSqlDataReader("descricao")
                oInfo.codigo_periodicidade = oSqlDataReader("codigo_periodicidade")
                oInfo.periodicidade = oSqlDataReader("periodicidade")
                oInfo.intervalo = oSqlDataReader("intervalo")
                oInfo.codigo_auditoria = oSqlDataReader("codigo_auditoria")
                oInfo.data_ultima_auditoria = oSqlDataReader("data_ultima_auditoria")
                oInfo.data_proxima_auditoria = oSqlDataReader("data_proxima_auditoria")
                oInfo.data_inicio = oSqlDataReader("data_inicio")
                oInfo.data_termino = oSqlDataReader("data_termino")
                oInfo.total = oSqlDataReader("total")
                oInfo.total_realizado = oSqlDataReader("total_realizado")
                oInfo.percentual = Math.Round(oSqlDataReader.Item("total_realizado") / oSqlDataReader.Item("total") * 100.0, 2)
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

#End Region

End Class
