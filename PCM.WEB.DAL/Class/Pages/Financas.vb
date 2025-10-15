Imports PCM.WEB.MODELS
Imports System.DirectoryServices
Imports System.DirectoryServices.ActiveDirectory
Imports PCM.WEB.DAL.SQLHelper
Imports System.Data.SqlClient
Imports System.Text

Public Class Financas

    Private sConnection As String

    Sub New(ByVal sCon As String)
        sConnection = sCon
    End Sub

#Region "::: CONTROLE GASTO :::"

    Public Sub ControleGastoIndex(ByVal iCodigoEmpresa As Integer,
                                  ByVal iCodigoUsuario AS Integer,
                                  ByVal iCodigoUnidade As Integer,
                                  ByVal iAno As Integer,
                                  ByRef oControleGasto As ControleGasto)

        'Variaveis Locais
        Dim oSqlParameter(3) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
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

            'Seta Parametros - Ano
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ano"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iAno

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_financas_controle_gasto", oSqlParameter)

            While oSqlDataReader.Read

                oControleGasto = New ControleGasto
                oControleGasto.ano = oSqlDataReader.Item("ano")
                oControleGasto.mes = oSqlDataReader.Item("mes")
                oControleGasto.previsao_gasto_janeiro = oSqlDataReader.Item("previsao_gasto_janeiro")
                oControleGasto.gasto_janeiro = oSqlDataReader.Item("gasto_janeiro")
                oControleGasto.saldo_janeiro = oSqlDataReader.Item("saldo_janeiro")
                oControleGasto.previsao_gasto_fevereiro = oSqlDataReader.Item("previsao_gasto_fevereiro")
                oControleGasto.gasto_fevereiro = oSqlDataReader.Item("gasto_fevereiro")
                oControleGasto.saldo_fevereiro = oSqlDataReader.Item("saldo_fevereiro")
                oControleGasto.previsao_gasto_marco = oSqlDataReader.Item("previsao_gasto_marco")
                oControleGasto.gasto_marco = oSqlDataReader.Item("gasto_marco")
                oControleGasto.saldo_marco = oSqlDataReader.Item("saldo_marco")
                oControleGasto.previsao_gasto_abril = oSqlDataReader.Item("previsao_gasto_abril")
                oControleGasto.gasto_abril = oSqlDataReader.Item("gasto_abril")
                oControleGasto.saldo_abril = oSqlDataReader.Item("saldo_abril")
                oControleGasto.previsao_gasto_maio = oSqlDataReader.Item("previsao_gasto_maio")
                oControleGasto.gasto_maio = oSqlDataReader.Item("gasto_maio")
                oControleGasto.saldo_maio = oSqlDataReader.Item("saldo_maio")
                oControleGasto.previsao_gasto_junho = oSqlDataReader.Item("previsao_gasto_junho")
                oControleGasto.gasto_junho = oSqlDataReader.Item("gasto_junho")
                oControleGasto.saldo_junho = oSqlDataReader.Item("saldo_junho")
                oControleGasto.previsao_gasto_julho = oSqlDataReader.Item("previsao_gasto_julho")
                oControleGasto.gasto_julho = oSqlDataReader.Item("gasto_julho")
                oControleGasto.saldo_julho = oSqlDataReader.Item("saldo_julho")
                oControleGasto.previsao_gasto_agosto = oSqlDataReader.Item("previsao_gasto_agosto")
                oControleGasto.gasto_agosto = oSqlDataReader.Item("gasto_agosto")
                oControleGasto.saldo_agosto = oSqlDataReader.Item("saldo_agosto")
                oControleGasto.previsao_gasto_setembro = oSqlDataReader.Item("previsao_gasto_setembro")
                oControleGasto.gasto_setembro = oSqlDataReader.Item("gasto_setembro")
                oControleGasto.saldo_setembro = oSqlDataReader.Item("saldo_setembro")
                oControleGasto.previsao_gasto_outubro = oSqlDataReader.Item("previsao_gasto_outubro")
                oControleGasto.gasto_outubro = oSqlDataReader.Item("gasto_outubro")
                oControleGasto.saldo_outubro = oSqlDataReader.Item("saldo_outubro")
                oControleGasto.previsao_gasto_novembro = oSqlDataReader.Item("previsao_gasto_novembro")
                oControleGasto.gasto_novembro = oSqlDataReader.Item("gasto_novembro")
                oControleGasto.saldo_novembro = oSqlDataReader.Item("saldo_novembro")
                oControleGasto.previsao_gasto_dezembro = oSqlDataReader.Item("previsao_gasto_dezembro")
                oControleGasto.gasto_dezembro = oSqlDataReader.Item("gasto_dezembro")
                oControleGasto.saldo_dezembro = oSqlDataReader.Item("saldo_dezembro")

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InsertControleGasto(ByVal iCodigoEmpresa As Integer,
                                   ByVal iCodigoUsuario AS Integer,
                                   ByVal iCodigoUnidade As Integer,
                                   ByVal iAno As Integer,
                                   ByVal dPrevisaoGastoJaneiro As Double,
                                   ByVal dPrevisaoGastoFevereiro As Double,
                                   ByVal dPrevisaoGastoMarco As Double,
                                   ByVal dPrevisaoGastoAbril As Double,
                                   ByVal dPrevisaoGastoMaio As Double,
                                   ByVal dPrevisaoGastoJunho As Double,
                                   ByVal dPrevisaoGastoJulho As Double,
                                   ByVal dPrevisaoGastoAgosto As Double,
                                   ByVal dPrevisaoGastoSetembro As Double,
                                   ByVal dPrevisaoGastoOutubro As Double,
                                   ByVal dPrevisaoGastoNovembro As Double,
                                   ByVal dPrevisaoGastoDezembro As Double)

        'Variaveis Locais
        Dim oSqlParameter(15) As SqlParameter
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

            'Seta Parametros - Ano
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ano"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iAno : i += 1

            'Seta Parametros - Previsão de Gasto - Janeiro
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "previsao_gasto_janeiro"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = dPrevisaoGastoJaneiro : i += 1

            'Seta Parametros - Previsão de Gasto - Fevereiro
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "previsao_gasto_fevereiro"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = dPrevisaoGastoFevereiro : i += 1

            'Seta Parametros - Previsão de Gasto - Março
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "previsao_gasto_marco"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = dPrevisaoGastoMarco : i += 1

            'Seta Parametros - Previsão de Gasto - Abril
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "previsao_gasto_abril"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = dPrevisaoGastoAbril : i += 1

            'Seta Parametros - Previsão de Gasto - Maio
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "previsao_gasto_maio"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = dPrevisaoGastoMaio : i += 1

            'Seta Parametros - Previsão de Gasto - Junho
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "previsao_gasto_junho"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = dPrevisaoGastoJunho : i += 1

            'Seta Parametros - Previsão de Gasto - Julho
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "previsao_gasto_julho"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = dPrevisaoGastoJulho : i += 1

            'Seta Parametros - Previsão de Gasto - Agosto
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "previsao_gasto_agosto"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = dPrevisaoGastoAgosto : i += 1

            'Seta Parametros - Previsão de Gasto - Setembro
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "previsao_gasto_setembro"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = dPrevisaoGastoSetembro : i += 1

            'Seta Parametros - Previsão de Gasto - Outubro
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "previsao_gasto_outubro"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = dPrevisaoGastoOutubro : i += 1

            'Seta Parametros - Previsão de Gasto - Novembro
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "previsao_gasto_novembro"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = dPrevisaoGastoNovembro : i += 1

            'Seta Parametros - Previsão de Gasto - Dezembro
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "previsao_gasto_dezembro"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = dPrevisaoGastoDezembro

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_financas_controle_gasto", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region

#Region "::: CONTRATO :::"

    Public Sub InsertContrato(ByVal iCodigoEmpresa As Integer,
                              ByVal iCodigoUsuario As Integer,
                              ByVal iCodigoUnidade As Integer,
                              ByVal sDescricao As String,
                              ByVal sComentario As String,
                              ByVal sPathArquivo As String,
                              ByVal sArquivo As String,
                              ByVal sDataInicio As String,
                              ByVal sDataTermino As String,
                              ByVal bAtivo As Boolean,
                              ByVal bEnviaEmail As Boolean,
                              ByVal iDiasAlerta As Integer,
                              ByVal sEmail As String)

        'Variaveis Locais
        Dim oSqlParameter(12) As SqlParameter
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
            oSqlParameter(i).Value = IIf(IsDate(sDataTermino), sDataTermino, DBNull.Value) : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo : i += 1

            'Seta Parametros - Envia E-mail
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "envia_email"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bEnviaEmail : i += 1

            'Seta Parametros - Email
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "email"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 2000
            oSqlParameter(i).Value = sEmail : i += 1

            'Seta Parametros - Dias Alerta
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "dias_alerta"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iDiasAlerta

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_financeiro_contrato", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateContrato(ByVal iCodigoEmpresa As Integer,
                              ByVal iCodigoUsuario As Integer,
                              ByVal iCodigoUnidade As Integer,
                              ByVal sDescricao As String,
                              ByVal sComentario As String,
                              ByVal sArquivo As String,
                              ByVal sPathArquivo As String,
                              ByVal sDataInicio As String,
                              ByVal sDataTermino As String,
                              ByVal bAtivo As Boolean,
                              ByVal bEnviaEmail As Boolean,
                              ByVal iDiasAlerta As Integer,
                              ByVal sEmail As String,
                              ByVal iCodigo As Integer)

        'Variaveis Locais
        Dim oSqlParameter(13) As SqlParameter
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
            oSqlParameter(i).Value = IIf(IsDate(sDataTermino), sDataTermino, DBNull.Value) : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo : i += 1

            'Seta Parametros - Envia E-mail
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "envia_email"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bEnviaEmail : i += 1

            'Seta Parametros - Email
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "email"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 2000
            oSqlParameter(i).Value = sEmail : i += 1

            'Seta Parametros - Dias Alerta
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "dias_alerta"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iDiasAlerta : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_financeiro_contrato", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub DeleteContrato(ByVal iCodigoEmpresa As Integer,
                              ByVal iCodigoUsuario As Integer,
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
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_financeiro_contrato", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InfoContrato(ByVal iCodigoEmpresa As Integer,
                            ByVal iCodigo As Integer,
                            ByRef oContrato As Contrato)

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
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_financeiro_contrato_dados", oSqlParameter)

            While oSqlDataReader.Read

                oContrato = New Contrato
                oContrato.codigo = oSqlDataReader.Item("codigo")
                oContrato.descricao = oSqlDataReader.Item("descricao")
                oContrato.comentario = oSqlDataReader.Item("comentario")
                oContrato.arquivo = oSqlDataReader.Item("arquivo")
                oContrato.path_arquivo = oSqlDataReader.Item("path_arquivo")
                oContrato.ativo = oSqlDataReader.Item("ativo")
                oContrato.data_inicio = oSqlDataReader.Item("data_inicio")
                oContrato.data_termino = oSqlDataReader.Item("data_termino")
                oContrato.unidade = oSqlDataReader.Item("unidade")
                oContrato.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oContrato.envia_email = oSqlDataReader.Item("envia_email")
                oContrato.dias_alerta = oSqlDataReader.Item("dias_alerta")
                oContrato.email = oSqlDataReader.Item("email")

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function IndexContrato(ByVal iCodigoEmpresa As Integer,
                                  ByVal iCodigoUsuario As Integer,
                                  ByVal iCodigoUnidade As Integer) As List(Of Contrato)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of Contrato)
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
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_financeiro_contrato", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New Contrato

                oInfo.descricao = oSqlDataReader.Item("descricao")
                oInfo.ativo = oSqlDataReader.Item("ativo")
                oInfo.codigo = oSqlDataReader.Item("codigo")
                oInfo.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oInfo.unidade = oSqlDataReader.Item("unidade")
                oInfo.data_inicio = oSqlDataReader.Item("data_inicio")
                oInfo.data_termino = oSqlDataReader.Item("data_termino")
                oInfo.path_arquivo = oSqlDataReader.Item("path_arquivo")
                oInfo.comentario = oSqlDataReader.Item("comentario")
                'oInfo.valor = oSqlDataReader.Item("valor_texto")

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

    Public Function ValidaContrato(ByVal iCodigoEmpresa As Integer,
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
            iReturn = ExecuteScalar(sConnection, CommandType.StoredProcedure, "sp_validate_financeiro_contrato", oSqlParameter)

            'Retorno da Função
            Return (IIf(iReturn > 0, False, True))

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: DESPESA :::"

    Public Sub InsertDespesa(ByVal iCodigoEmpresa As Integer,
                             ByVal iCodigoUsuario As Integer,
                             ByVal iCodigoUnidade As Integer,
                             ByVal iCodigoTipoDespesa As Integer,
                             ByVal sNumeroDocumento As String,
                             ByVal sDescricao As String,
                             ByVal iCodigoFornecedor As Integer,
                             ByVal dValor As Double,
                             ByVal sDataCompetencia As String,
                             ByVal sDataPagamento As String,
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
            oSqlParameter(i).Value = IIf(iCodigoUnidade = -1, DBNull.Value, iCodigoUnidade) : i += 1

            'Seta Parametros - Tipo de Despesa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_tipo_despesa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoTipoDespesa : i += 1

            'Seta Parametros - Código Fornecedor
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_fornecedor"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoFornecedor : i += 1

            'Seta Parametros - Nº Documento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "numero_documento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sNumeroDocumento : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 250
            oSqlParameter(i).Value = sDescricao : i += 1

            'Seta Parametros - Valor
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "valor"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = dValor : i += 1

            'Seta Parametros - Data Competência
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_competencia"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataCompetencia : i += 1

            'Seta Parametros - Data Pagamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_pagamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = IIf(IsDate(sDataPagamento), sDataPagamento, DBNull.Value) : i += 1

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
            oSqlParameter(i).Value = sPathArquivo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_financeiro_despesa", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateDespesa(ByVal iCodigoEmpresa As Integer,
                             ByVal iCodigoUsuario As Integer,
                             ByVal iCodigoUnidade As Integer,
                             ByVal iCodigoTipoDespesa As Integer,
                             ByVal sDescricao As String,
                             ByVal iCodigoFornecedor As Integer,
                             ByVal sNumeroDocumento As String,
                             ByVal dValor As Double,
                             ByVal sDataCompetencia As String,
                             ByVal sDataPagamento As String,
                             ByVal sPathArquivo As String,
                             ByVal sArquivo As String,
                             ByVal lCodigo As Long)

        'Variaveis Locais
        Dim oSqlParameter(12) As SqlParameter
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

            'Seta Parametros - Tipo de Despesa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_tipo_despesa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoTipoDespesa : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sDescricao : i += 1

            'Seta Parametros - Código Fornecedor
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_fornecedor"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoFornecedor : i += 1

            'Seta Parametros - Nº Documento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "numero_documento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sNumeroDocumento : i += 1

            'Seta Parametros - Valor
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "valor"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = dValor : i += 1

            'Seta Parametros - Data Competência
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_competencia"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = sDataCompetencia : i += 1

            'Seta Parametros - Data Pagamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "data_pagamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Date
            oSqlParameter(i).Value = IIf(IsDate(sDataPagamento), sDataPagamento, DBNull.Value) : i += 1

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
            oSqlParameter(i).Value = sPathArquivo
            i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_financeiro_despesa", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub DeleteDespesa(ByVal iCodigoEmpresa As Integer,
                             ByVal iCodigoUsuario As Integer,
                             ByVal lCodigo As Long)

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
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_financeiro_despesa", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InfoDespesa(ByVal iCodigoEmpresa As Integer,
                           ByVal lCodigo As Long,
                           ByRef oDespesa As Despesa)

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
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigo

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_financeiro_despesa_dados", oSqlParameter)

            While oSqlDataReader.Read

                oDespesa = New Despesa
                oDespesa.codigo = oSqlDataReader.Item("codigo")
                oDespesa.numero_documento = oSqlDataReader.Item("numero_documento")
                oDespesa.descricao = oSqlDataReader.Item("descricao")
                oDespesa.codigo_tipo_despesa = oSqlDataReader.Item("codigo_tipo_despesa")
                oDespesa.codigo_fornecedor = oSqlDataReader.Item("codigo_fornecedor")
                oDespesa.valor = oSqlDataReader.Item("valor")
                oDespesa.data_competencia = oSqlDataReader.Item("data_competencia")
                oDespesa.data_pagamento = oSqlDataReader.Item("data_pagamento")
                oDespesa.unidade = oSqlDataReader.Item("unidade")
                oDespesa.tipo_despesa = oSqlDataReader.Item("tipo_despesa")
                oDespesa.fornecedor = oSqlDataReader.Item("fornecedor")
                oDespesa.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oDespesa.arquivo = oSqlDataReader.Item("arquivo")
                oDespesa.path_arquivo = oSqlDataReader.Item("path_arquivo")

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function IndexDespesa(ByVal iCodigoEmpresa As Integer,
                                 ByVal iCodigoUsuario As Integer,
                                 ByVal iCodigoUnidade As Integer) As List(Of Despesa)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of Despesa)
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
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_financeiro_despesa", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New Despesa

                oInfo.numero_documento = oSqlDataReader.Item("numero_documento")
                oInfo.descricao = oSqlDataReader.Item("descricao")
                oInfo.codigo = oSqlDataReader.Item("codigo")
                oInfo.codigo_unidade = oSqlDataReader.Item("codigo_unidade")
                oInfo.unidade = oSqlDataReader.Item("unidade")
                oInfo.tipo_despesa = oSqlDataReader.Item("tipo_despesa")
                oInfo.fornecedor = oSqlDataReader.Item("fornecedor")
                oInfo.valor = oSqlDataReader.Item("valor")
                oInfo.data_competencia = oSqlDataReader.Item("data_competencia")
                oInfo.data_pagamento = oSqlDataReader.Item("data_pagamento")
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

End Class
