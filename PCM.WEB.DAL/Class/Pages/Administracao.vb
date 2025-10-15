Imports PCM.WEB.MODELS
Imports System.DirectoryServices
Imports System.DirectoryServices.ActiveDirectory
Imports PCM.WEB.DAL.SQLHelper
Imports System.Data.SqlClient
Imports System.Text
Imports System.Net
Imports System.IO
Imports MS.Internal.Text.TextInterface
Imports OfficeOpenXml.FormulaParsing.Excel.Functions.Math

Public Class Administracao

    Private sConnection As String

    Sub New(ByVal sCon As String)
        sConnection = sCon
    End Sub

#Region "::: USUÁRIO :::"

    Public Sub InsertUsuario(ByVal codigoEmpresa As Integer,
                             ByVal codigoUnidade As Integer,
                             ByVal codigoPerfil As Integer,
                             ByVal codigoDepartamento As Integer,
                             ByVal senha As String,
                             ByVal nome As String,
                             ByVal apelido As String,
                             ByVal telefone As String,
                             ByVal email As String,
                             ByVal emailSenha As String,
                             ByVal aplicativo As Boolean,
                             ByVal website As Boolean,
                             ByVal ativo As Boolean,
                             ByVal colaborador As Boolean,
                             ByVal contabilizaHora As Boolean,
                             ByVal valorHora As Double,
                             ByVal codigoTipoFuncionario As Integer,
                             ByVal codigoFuncao As Integer,
                             ByVal modulo As String,
                             ByVal codigoModuloDefault As Integer,
                             ByRef codigo As Integer)

        Try

            ' Define parameters
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, IIf(codigoUnidade = -1, DBNull.Value, codigoUnidade)),
                CriarParametro("codigo_perfil", SqlDbType.Int, codigoPerfil),
                CriarParametro("codigo_departamento", SqlDbType.Int, IIf(codigoDepartamento = -1, DBNull.Value, codigoDepartamento)),
                CriarParametro("senha", SqlDbType.VarChar, Cripitografar(senha.ToUpper())),
                CriarParametro("nome", SqlDbType.VarChar, nome),
                CriarParametro("apelido", SqlDbType.VarChar, apelido),
                CriarParametro("telefone", SqlDbType.VarChar, telefone),
                CriarParametro("email", SqlDbType.VarChar, email),
                CriarParametro("email_senha", SqlDbType.VarChar, emailSenha),
                CriarParametro("aplicativo", SqlDbType.Bit, aplicativo),
                CriarParametro("website", SqlDbType.Bit, website),
                CriarParametro("ativo", SqlDbType.Bit, ativo),
                CriarParametro("colaborador", SqlDbType.Bit, colaborador),
                CriarParametro("contabiliza_hora", SqlDbType.Bit, contabilizaHora),
                CriarParametro("valor_hora", SqlDbType.Float, valorHora),
                CriarParametro("codigo_tipo_funcionario", SqlDbType.Int, IIf(codigoTipoFuncionario = -1, DBNull.Value, codigoTipoFuncionario)),
                CriarParametro("codigo_funcao", SqlDbType.Int, IIf(codigoFuncao = -1, DBNull.Value, codigoFuncao)),
                CriarParametro("modulo", SqlDbType.VarChar, modulo),
                CriarParametro("codigo_modulo_default", SqlDbType.Int, codigoModuloDefault),
                CriarParametro("codigo", SqlDbType.Int, 0, ParameterDirection.Output)
            }

            'Execute query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_cadastro_basico_usuario_colaborador", oSqlParameter)

            codigo = oSqlParameter(UBound(oSqlParameter)).Value

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateUsuario(ByVal codigoEmpresa As Integer,
                             ByVal codigoUnidade As Integer,
                             ByVal codigoPerfil As Integer,
                             ByVal codigoDepartamento As Integer,
                             ByVal nome As String,
                             ByVal apelido As String,
                             ByVal telefone As String,
                             ByVal email As String,
                             ByVal emailSenha As String,
                             ByVal aplicativo As Boolean,
                             ByVal website As Boolean,
                             ByVal ativo As Boolean,
                             ByVal colaborador As Boolean,
                             ByVal contabilizaHora As Boolean,
                             ByVal valorHora As Double,
                             ByVal codigoTipoFuncionario As Integer,
                             ByVal codigoFuncao As Integer,
                             ByVal modulo As String,
                             ByVal codigoModuloDefault As Integer,
                             ByVal codigo As Integer)

        Try

            ' Define parameters
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, IIf(codigoUnidade = -1, DBNull.Value, codigoUnidade)),
                CriarParametro("codigo_perfil", SqlDbType.Int, codigoPerfil),
                CriarParametro("codigo_departamento", SqlDbType.Int, IIf(codigoDepartamento = -1, DBNull.Value, codigoDepartamento)),
                CriarParametro("nome", SqlDbType.VarChar, nome),
                CriarParametro("apelido", SqlDbType.VarChar, apelido),
                CriarParametro("telefone", SqlDbType.VarChar, telefone),
                CriarParametro("email", SqlDbType.VarChar, email),
                CriarParametro("email_senha", SqlDbType.VarChar, emailSenha),
                CriarParametro("aplicativo", SqlDbType.Bit, aplicativo),
                CriarParametro("website", SqlDbType.Bit, website),
                CriarParametro("ativo", SqlDbType.Bit, ativo),
                CriarParametro("colaborador", SqlDbType.Bit, colaborador),
                CriarParametro("contabiliza_hora", SqlDbType.Bit, contabilizaHora),
                CriarParametro("valor_hora", SqlDbType.Float, valorHora),
                CriarParametro("codigo_tipo_funcionario", SqlDbType.Int, IIf(codigoTipoFuncionario = -1, DBNull.Value, codigoTipoFuncionario)),
                CriarParametro("codigo_funcao", SqlDbType.Int, IIf(codigoFuncao = -1, DBNull.Value, codigoFuncao)),
                CriarParametro("modulo", SqlDbType.VarChar, modulo),
                CriarParametro("codigo_modulo_default", SqlDbType.Int, codigoModuloDefault),
                CriarParametro("codigo", SqlDbType.Int, codigo)
            }

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_cadastro_basico_usuario", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateUsuario(ByVal codigoEmpresa As Integer,
                             ByVal codigoUnidade As Integer,
                             ByVal senha As String,
                             ByVal nome As String,
                             ByVal email As String,
                             ByVal codigo As Integer)

        Try

            ' Define parameters
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, IIf(codigoUnidade = -1, DBNull.Value, codigoUnidade)),
                CriarParametro("nome", SqlDbType.VarChar, nome),
                CriarParametro("email", SqlDbType.VarChar, email),
                CriarParametro("senha", SqlDbType.VarChar, Cripitografar(senha.ToUpper())),
                CriarParametro("codigo", SqlDbType.Int, codigo)
            }

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_cadastro_basico_usuario", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateUsuario(ByVal codigoEmpresa As Integer,
                             ByVal senha As String,
                             ByVal codigo As Integer)

        Try

            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo", SqlDbType.Int, codigo),
                CriarParametro("senha", SqlDbType.VarChar, Cripitografar(senha.ToUpper()))
            }

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_cadastro_basico_usuario", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub DeleteUsuario(ByVal codigoEmpresa As Integer,
                             ByVal codigo As Integer)

        Try

            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo", SqlDbType.Int, codigo)
            }

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_cadastro_basico_usuario", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InfoUsuario(ByVal codigoEmpresa As Integer,
                           ByVal codigo As Integer,
                           ByVal codigoUsuario As Integer,
                           ByRef oUsuario As Usuario)

        Try

            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo", SqlDbType.Int, codigo)
            }

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_usuario_dados", oSqlParameter)

                While oSqlDataReader.Read

                    oUsuario = New Usuario
                    oUsuario.codigo = SafeGetLong(oSqlDataReader, "codigo")
                    oUsuario.codigoUnidade = SafeGetLong(oSqlDataReader, "codigo_unidade")
                    oUsuario.unidade = SafeGetString(oSqlDataReader, "unidade")
                    oUsuario.codigoPerfil = SafeGetLong(oSqlDataReader, "codigo_perfil")
                    oUsuario.perfil = SafeGetString(oSqlDataReader, "perfil")
                    oUsuario.codigoDepartamento = SafeGetLong(oSqlDataReader, "codigo_departamento")
                    oUsuario.departamento = SafeGetString(oSqlDataReader, "departamento")
                    oUsuario.email = SafeGetString(oSqlDataReader, "email")
                    oUsuario.emailSenha = SafeGetString(oSqlDataReader, "email_senha")
                    oUsuario.nome = SafeGetString(oSqlDataReader, "nome")
                    oUsuario.apelido = SafeGetString(oSqlDataReader, "apelido")
                    oUsuario.telefone = SafeGetString(oSqlDataReader, "telefone")
                    oUsuario.ativo = SafeGetBoolean(oSqlDataReader, "ativo")
                    oUsuario.aplicativo = SafeGetBoolean(oSqlDataReader, "aplicativo")
                    oUsuario.website = SafeGetBoolean(oSqlDataReader, "website")
                    oUsuario.modulo = SafeGetString(oSqlDataReader, "modulo")
                    oUsuario.codigoModuloDefault = SafeGetLong(oSqlDataReader, "codigo_modulo_default")
                    oUsuario.colaborador = SafeGetBoolean(oSqlDataReader, "colaborador")
                    oUsuario.contabilizaHoras = SafeGetBoolean(oSqlDataReader, "contabiliza_hora")
                    oUsuario.valorHora = SafeGetFloat(oSqlDataReader, "valor_hora")
                    oUsuario.codigoTipoFuncionario = SafeGetLong(oSqlDataReader, "codigo_tipo_funcionario")
                    oUsuario.codigoFuncao = SafeGetLong(oSqlDataReader, "codigo_funcao")
                    oUsuario.unidades = IndexUsuarioUnidade(codigoUsuario:=codigoUsuario,
                                                            codigoUsuarioUpdate:=codigo,
                                                            codigoEmpresa:=codigoEmpresa)

                End While

            End Using

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function IndexUsuario(ByVal codigoEmpresa As Integer,
                                 ByVal codigoUsuario As Integer,
                                 ByVal codigoUnidade As Integer,
                                 ByVal nome As String,
                                 ByVal codigoDepartamento As Integer,
                                 ByVal codigoModulo As Integer,
                                 ByVal ativo As Integer) As List(Of Usuario)

        Try

            Dim oUsuario As New List(Of Usuario)

            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_usuario", SqlDbType.Int, codigoUsuario),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("codigo_modulo", SqlDbType.Int, codigoModulo),
                CriarParametro("nome", SqlDbType.VarChar, nome),
                CriarParametro("codigo_departamento", SqlDbType.Int, codigoDepartamento),
                CriarParametro("ativo", SqlDbType.Bit, IIf(ativo = 1, True, IIf(ativo = 0, False, DBNull.Value)))
            }

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_usuario", oSqlParameter)

                While oSqlDataReader.Read

                    Dim oInfo As New Usuario

                    oInfo.codigo = SafeGetLong(oSqlDataReader, "codigo")
                    oInfo.modulo = SafeGetString(oSqlDataReader, "modulo")
                    oInfo.unidade = SafeGetString(oSqlDataReader, "unidade")
                    oInfo.perfil = SafeGetString(oSqlDataReader, "perfil")
                    oInfo.departamento = SafeGetString(oSqlDataReader, "departamento")
                    oInfo.nome = SafeGetString(oSqlDataReader, "nome")
                    oInfo.apelido = SafeGetString(oSqlDataReader, "apelido")
                    oInfo.telefone = SafeGetString(oSqlDataReader, "telefone")
                    oInfo.email = SafeGetString(oSqlDataReader, "email")
                    oInfo.emailSenha = SafeGetString(oSqlDataReader, "email_senha")
                    oInfo.departamento = SafeGetString(oSqlDataReader, "departamento")
                    oInfo.ativo = SafeGetBoolean(oSqlDataReader, "ativo")

                    oUsuario.Add(oInfo)

                End While

            End Using

            'Retorno da Função
            Return oUsuario

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ValidaUsuario(ByVal codigoUsuario As Integer,
                                  ByVal email As String) As Boolean

        Try

            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo", SqlDbType.Int, codigoUsuario),
                CriarParametro("email", SqlDbType.VarChar, email)
            }

            'Executa Query
            Dim iReturn As Integer = ExecuteScalar(sConnection, CommandType.StoredProcedure, "sp_validate_cadastro_basico_usuario", oSqlParameter)

            'Retorno da Função
            Return (IIf(iReturn > 0, False, True))

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function IndexUsuarioUnidade(ByVal codigoUsuario As Integer,
                                        ByVal codigoUsuarioUpdate As Integer,
                                        ByVal codigoEmpresa As Integer) As List(Of UsuarioUnidade)


        Try

            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_usuario", SqlDbType.Int, codigoUsuario),
                CriarParametro("codigo_usuario_update", SqlDbType.Int, codigoUsuarioUpdate)
            }

            Dim oReturn As New List(Of UsuarioUnidade)

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_usuario_unidade", oSqlParameter)

                While oSqlDataReader.Read

                    Dim oInfo As New UsuarioUnidade

                    oInfo.codigoUsuario = SafeGetLong(oSqlDataReader, "codigo_usuario")
                    oInfo.codigoUnidade = SafeGetLong(oSqlDataReader, "codigo_unidade")
                    oInfo.unidade = SafeGetString(oSqlDataReader, "unidade")
                    oInfo.selecionado = SafeGetBoolean(oSqlDataReader, "selecionado")

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

    Public Sub InsertUsuarioUnidade(ByVal codigoEmpresa As Integer,
                                    ByVal codigoUnidade As Integer,
                                    ByVal codigoUsuario As Integer)

        Try

            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_usuario", SqlDbType.Int, codigoUsuario),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade)
            }

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_cadastro_basico_usuario_unidade", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region

#Region "::: PERFIL :::"

    Public Sub InsertPerfil(ByVal iCodigoEmpresa As Integer,
                            ByVal iCodigoUsuario As Integer,
                            ByVal sDescricao As String,
                            ByVal bAtivo As Boolean,
                            ByRef iCodigo As Integer)

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

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sDescricao.ToUpper : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Output
            oSqlParameter(i).SqlDbType = SqlDbType.Int

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_cadastro_basico_perfil", oSqlParameter)

            iCodigo = oSqlParameter(i).Value

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InsertPerfilDireito(ByVal iCodigoEmpresa As Integer,
                                   ByVal iCodigoUsuario As Integer,
                                   ByVal iCodigoPerfil As Integer,
                                   ByVal sCodigoFormulario As String,
                                   ByVal bVisualizar As Boolean,
                                   ByVal bInserir As Boolean,
                                   ByVal bEditar As Boolean,
                                   ByVal bExcluir As Boolean,
                                   ByVal bImprimir As Boolean,
                                   ByVal bAdministrador As Boolean)

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

            'Seta Parametros - Código Perfil
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_perfil"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoPerfil : i += 1

            'Seta Parametros - Código Formulário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_formulario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sCodigoFormulario : i += 1

            'Seta Parametros - Visualizar
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "visualizar"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bVisualizar : i += 1

            'Seta Parametros - Inserir
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "inserir"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bInserir : i += 1

            'Seta Parametros - Editar
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "editar"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bEditar : i += 1

            'Seta Parametros - Excluir
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "excluir"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bExcluir : i += 1

            'Seta Parametros - Imprimir
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "imprimir"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bImprimir : i += 1

            'Seta Parametros - Administrador
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "administrador"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAdministrador

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_cadastro_basico_perfil_direito", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub UpdatePerfil(ByVal iCodigoEmpresa As Integer,
                             ByVal iCodigoUsuario As Integer,
                             ByVal sDescricao As String,
                             ByVal bAtivo As Boolean,
                             ByVal iCodigo As Integer)

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

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sDescricao.ToUpper : i += 1

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
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_cadastro_basico_perfil", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function UpdatePerfilHierarquia(ByVal iCodigoEmpresa As Integer,
                                           ByVal iHierarquia As Integer,
                                           ByVal sOpcao As String) As List(Of PerfilHierarquia)

        'Variaveis Locais
        Dim oSqlParameter(2) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New List(Of PerfilHierarquia)
        Dim i As Integer = 0

        Try

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Hierarquia
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "hierarquia"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iHierarquia : i += 1

            'Seta Parametros - Opção
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "opcao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 1
            oSqlParameter(i).Value = sOpcao

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_update_cadastro_basico_perfil_hierarquia", oSqlParameter)

            While oSqlDataReader.Read

                Dim oPerfilHierarquiaInfo As New PerfilHierarquia
                Dim oColuna As String() = oSqlDataReader.Item("coluna").ToString.Split("|")

                oPerfilHierarquiaInfo.linha = oSqlDataReader.Item("linha")
                oPerfilHierarquiaInfo.coluna = New List(Of String)
                For Each sColuna As String In oColuna
                    oPerfilHierarquiaInfo.coluna.Add(sColuna)
                Next
                oReturn.Add(oPerfilHierarquiaInfo)

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Seta Retorno
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Sub DeletePerfil(ByVal iCodigoEmpresa As Integer,
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
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_cadastro_basico_perfil", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub InfoPerfil(ByVal iCodigoEmpresa As Integer,
                           ByVal iCodigo As Integer,
                           ByRef oPerfil As Perfil)

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
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_perfil_dados", oSqlParameter)

            While oSqlDataReader.Read

                oPerfil = New Perfil
                oPerfil.codigo = oSqlDataReader.Item("codigo")
                oPerfil.descricao = oSqlDataReader.Item("descricao")
                oPerfil.ativo = oSqlDataReader.Item("ativo")

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub
    Public Function IndexPerfil(ByVal iCodigoEmpresa As Integer,
                                ByVal iCodigoUsuario As Integer) As List(Of Perfil)

        Try

            'Váriaveis Locais
            Dim oPerfil As New List(Of Perfil)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(2) As SqlParameter
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
            oSqlParameter(i).Value = iCodigoUsuario

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_perfil", oSqlParameter)

            While oSqlDataReader.Read

                Dim oPerfilInfo As New Perfil

                oPerfilInfo.descricao = oSqlDataReader.Item("descricao")
                oPerfilInfo.ativo = oSqlDataReader.Item("ativo")
                oPerfilInfo.hierarquia = oSqlDataReader.Item("hierarquia")
                oPerfilInfo.codigo = oSqlDataReader.Item("codigo")

                oPerfil.Add(oPerfilInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oPerfil

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function IndexPerfilDireito(ByVal iCodigoEmpresa As Integer,
                                       ByVal iCodigoPerfil As Integer) As List(Of PerfilDireito)

        Try

            'Váriaveis Locais
            Dim oPerfilDireito As New List(Of PerfilDireito)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(1) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Perfil
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_perfil"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoPerfil

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_perfil_direito", oSqlParameter)

            While oSqlDataReader.Read

                Dim oPerfilDireitoInfo As New PerfilDireito

                oPerfilDireitoInfo.formulario = oSqlDataReader.Item("formulario")
                oPerfilDireitoInfo.codigo_formulario = oSqlDataReader.Item("codigo_formulario")
                oPerfilDireitoInfo.visualizar = oSqlDataReader.Item("visualizar")                
                oPerfilDireitoInfo.inserir = oSqlDataReader.Item("inserir")
                oPerfilDireitoInfo.editar = oSqlDataReader.Item("editar")
                oPerfilDireitoInfo.excluir = oSqlDataReader.Item("excluir")
                oPerfilDireitoInfo.imprimir = oSqlDataReader.Item("imprimir")
                oPerfilDireitoInfo.direito = oSqlDataReader.Item("direito")
                oPerfilDireitoInfo.administrador = oSqlDataReader.Item("administrador")

                oPerfilDireito.Add(oPerfilDireitoInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oPerfilDireito

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ValidaPerfil(ByVal iCodigoEmpresa As Integer,
                                 ByVal iCodigoPerfil As Integer,
                                 ByVal sDescricao As String) As Boolean

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

            'Seta Parametros - Código Perfil
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_perfil"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoPerfil : i += 1

            'Seta Parametros - Descrição
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "descricao"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sDescricao

            'Executa Query
            iReturn = ExecuteScalar(sConnection, CommandType.StoredProcedure, "sp_validate_cadastro_basico_perfil", oSqlParameter)

            'Retorno da Função
            Return (IIf(iReturn > 0, False, True))

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: EMPRESA :::"

    Public Function IndexEmpresa(ByVal iCodigoEmpresa As Integer,
                                 ByVal iCodigoUsuario As Integer) As List(Of Empresa)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of Empresa)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(1) As SqlParameter
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
            oSqlParameter(i).Value = iCodigoUsuario

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_empresa", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New Empresa

                oInfo.nome_fantasia = oSqlDataReader.Item("nome_fantasia")
                oInfo.ativo = oSqlDataReader.Item("ativo")
                oInfo.codigo = oSqlDataReader.Item("codigo")

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

    Public Sub UpdateEmpresa(ByVal iCodigo As Integer,
                             ByVal bAtivo As Boolean)

        Try

            'Váriaveis Locais
            Dim oSqlParameter(1) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigo : i += 1

            'Seta Parametros - Ativo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ativo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Bit
            oSqlParameter(i).Value = bAtivo

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_cadastro_basico_empresa_ativo", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region

#Region "::: CONFIGURAÇÃO - DESEMPENHO DAS UNIDADE :::"

    Public Function ConfiguracaoDesempenhoUnidade(ByVal iCodigoEmpresa As Integer) As ConfiguracaoDesempenhoUnidades

        Try

            'Váriaveis Locais
            Dim oReturn As New ConfiguracaoDesempenhoUnidades
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(0) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_configuracao_desempenho_unidade", oSqlParameter)

            While oSqlDataReader.Read

                oReturn.laudo = oSqlDataReader.Item("laudo")
                oReturn.preventiva = oSqlDataReader.Item("preventiva")
                oReturn.rotina = oSqlDataReader.Item("rotina")
                oReturn.pmoc = oSqlDataReader.Item("pmoc")
                oReturn.uh_dia = oSqlDataReader.Item("uh_dia")
                oReturn.os_atendimento_dia = oSqlDataReader.Item("os_atendimento_dia")
                oReturn.hh_nao_apontado = oSqlDataReader.Item("hh_nao_apontado")
                oReturn.os_pendente = oSqlDataReader.Item("os_pendente")
                oReturn.hh_extra = oSqlDataReader.Item("hh_extra")
                oReturn.preventiva_corretiva = oSqlDataReader.Item("preventiva_corretiva")
                oReturn.green_planet = oSqlDataReader.Item("green_planet")

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

    Public Sub UpdateConfiguracaoDesempenhoUnidade(ByVal iCodigoEmpresa As Integer,
                                                   ByVal sLaudo As String,
                                                   ByVal sPreventiva As String,
                                                   ByVal sRotina As String,
                                                   ByVal sPMOC As String,
                                                   ByVal sUHDia As String,
                                                   ByVal sOSAtendimentoDia As String,
                                                   ByVal sHHNaoApontado As String,
                                                   ByVal sOSPendente As String,
                                                   ByVal sHHExtra As String,
                                                   ByVal sPreventivaCorretiva As String,
                                                   ByVal sGreenPlanet As String)

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

            'Seta Parametros - Laudo
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "laudo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = CDbl(sLaudo.Replace(".", ",")) : i += 1

            'Seta Parametros - Preventiva
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "preventiva"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = CDbl(sPreventiva.Replace(".", ",")) : i += 1

            'Seta Parametros - Rotina
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "rotina"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = CDbl(sRotina.Replace(".", ",")) : i += 1

            'Seta Parametros - PMOC
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "pmoc"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = CDbl(sPMOC.Replace(".", ",")) : i += 1

            'Seta Parametros - UH Dia
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "uh_dia"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = CDbl(sUHDia.Replace(".", ",")) : i += 1

            'Seta Parametros - OS Atendimento Dia
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "os_atendimento_dia"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = CDbl(sOSAtendimentoDia.Replace(".", ",")) : i += 1

            'Seta Parametros - H.H Não Apontado
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "hh_nao_apontado"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = CDbl(sHHNaoApontado.Replace(".", ",")) : i += 1

            'Seta Parametros - OS Pendente
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "os_pendente"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = CDbl(sOSPendente.Replace(".", ",")) : i += 1

            'Seta Parametros - HH Extra
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "hh_extra"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = CDbl(sHHExtra.Replace(".", ",")) : i += 1

            'Seta Parametros - Preventiva x Corretiva
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "preventiva_corretiva"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = CDbl(sPreventivaCorretiva.Replace(".", ",")) : i += 1

            'Seta Parametros - Green Planet
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "green_planet"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Float
            oSqlParameter(i).Value = CDbl(sGreenPlanet.Replace(".", ","))

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_configuracao_desempenho_unidade", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region

End Class
