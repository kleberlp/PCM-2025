Imports PCM.WEB.MODELS
Imports PCM.WEB.DAL.SQLHelper
Imports System.Data.SqlClient

Public Class Account

    Private sConnection As String

    Sub New(ByVal sCon As String)
        sConnection = sCon
    End Sub

    Public Function LoadImageLogin(ByVal sURL As String) As imagemLogin

        'Variaveis Locais
        Dim oSqlParameter(0) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim oReturn As New imagemLogin
        Dim i As Integer = 0

        Try

            'Seta Parametros - URL
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "url"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 100
            oSqlParameter(i).Value = sURL

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_empresa_imagem", oSqlParameter)

            While oSqlDataReader.Read

                oReturn.imagem_login = oSqlDataReader.Item("imagem_login")
                oReturn.imagem_background = oSqlDataReader.Item("imagem_background")

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

    Public Function ValidaLogin(ByVal sEmail As String) As Boolean

        'Variaveis Locais
        Dim oSqlParameter(0) As SqlParameter
        Dim i As Integer = 0

        Try

            'Seta Parametros - Username
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "email"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 250
            oSqlParameter(i).Value = sEmail

            'Executa Query
            Return CType(ExecuteScalar(sConnection, CommandType.StoredProcedure, "sp_valida_login", oSqlParameter), Boolean)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Sub LoadInfoUser(ByVal sUsername As String,
                            ByRef sLanguage As String,
                            ByRef bActive As Boolean)

        'Variaveis Locais
        Dim oSqlParameter(1) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim i As Integer = 0

        Try

            'Seta Parametros - Username
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "username"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sUsername

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_register_user_find", oSqlParameter)

            While oSqlDataReader.Read

                sLanguage = oSqlDataReader.Item("language_id")
                bActive = oSqlDataReader.Item("active")

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function Login(ByVal sEmail As String,
                          ByVal sSenha As String,
                          ByRef iCodigoEmpresa As Integer,
                          ByRef iCodigoUsuario As Integer,
                          ByRef sImagemSidebar As String,
                          ByRef sImagemSidebarMin As String,
                          ByRef sImagemHeader As String) As Boolean

        'Variaveis Locais
        Dim oSqlParameter(1) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim bFlag As Boolean = False
        Dim i As Integer = 0

        Try

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
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_validate_login", oSqlParameter)

            If oSqlDataReader.Read() Then

                iCodigoEmpresa = oSqlDataReader.Item("codigo_empresa")
                iCodigoUsuario = oSqlDataReader.Item("codigo").ToString
                sEmail = oSqlDataReader.Item("email")
                sImagemSidebar = oSqlDataReader.Item("imagem_sidebar")
                sImagemSidebarMin = oSqlDataReader.Item("imagem_sidebar_min")
                sImagemHeader = oSqlDataReader.Item("imagem_header")

                'Seta Retorno da Função
                bFlag = True

            End If

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            Return bFlag

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function Login(ByVal sEmail As String,
                          ByVal sSenha As String,
                          ByRef bAtivo As Boolean,
                          ByRef iCodigoUsuario As Integer,
                          ByRef sCodigoFuncionario As String,
                          ByRef iCodigoDepartamento As Integer,
                          ByRef bFornecedorPMOC As Boolean,
                          ByRef sFuncionario As String,
                          ByRef sNome As String,
                          ByRef iCodigoUnidade As Integer,
                          ByRef sUnidade As String,
                          ByRef iCodigoEmpresa As Integer,
                          ByRef iCodigoEmpresaPMOC As Integer,
                          ByRef iCodigoUnidadePMOC As Integer,
                          ByRef sImagemSidebar As String,
                          ByRef sImagemSidebarMin As String,
                          ByRef sImagemHeader As String,
                          ByRef sDashboardPage As String,
                          ByRef bManutencao As Boolean,
                          ByRef bQualidade As Boolean,
                          ByRef bAeB As Boolean,
                          ByRef bRotinaPrioridade As Boolean,
                          ByRef bApontaHoras As Boolean,
                          ByRef iCodigoModuloDefault As Integer,
                          ByRef sModulo As String,
                          ByRef sModuloDescricao As String,
                          ByRef sHotelOpera As String) As Boolean

        'Variaveis Locais
        Dim oSqlParameter(2) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim bFlag As Boolean = False
        Dim i As Integer = 0

        Try

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
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_validate_login", oSqlParameter)

            If oSqlDataReader.Read() Then

                bAtivo = oSqlDataReader.Item("ativo")
                iCodigoUsuario = oSqlDataReader.Item("codigo").ToString
                sNome = oSqlDataReader.Item("nome")
                sCodigoFuncionario = oSqlDataReader.Item("codigo_funcionario")
                sFuncionario = oSqlDataReader.Item("funcionario")
                iCodigoDepartamento = oSqlDataReader.Item("codigo_departamento")
                bFornecedorPMOC = oSqlDataReader.Item("fornecedor_pmoc")
                sEmail = oSqlDataReader.Item("email")
                iCodigoUnidade = oSqlDataReader.Item("codigo_unidade")
                sUnidade = oSqlDataReader.Item("unidade")
                iCodigoEmpresa = oSqlDataReader.Item("codigo_empresa")
                iCodigoEmpresaPMOC = oSqlDataReader.Item("codigo_empresa_pmoc")
                iCodigoUnidadePMOC = oSqlDataReader.Item("codigo_unidade_pmoc")
                bFornecedorPMOC = oSqlDataReader.Item("fornecedor_pmoc")
                sImagemSidebar = oSqlDataReader.Item("imagem_sidebar")
                sImagemSidebarMin = oSqlDataReader.Item("imagem_sidebar_min")
                sImagemHeader = oSqlDataReader.Item("imagem_header")
                sDashboardPage = oSqlDataReader.Item("dashboard")
                bManutencao = oSqlDataReader.Item("manutencao")
                bQualidade = oSqlDataReader.Item("qualidade")
                bAeB = oSqlDataReader.Item("aeb")
                bRotinaPrioridade = oSqlDataReader.Item("rotina_prioridade")
                iCodigoModuloDefault = oSqlDataReader.Item("codigo_modulo_default")
                bApontaHoras = oSqlDataReader.Item("aponta_horas")
                sModulo = oSqlDataReader.Item("modulo")
                sModuloDescricao = oSqlDataReader.Item("modulo_descricao")
                sHotelOpera = oSqlDataReader.Item("hotel_opera")

                'Seta Retorno da Função
                bFlag = True

            End If

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            Return bFlag

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function Login(ByVal oLogin As login) As Boolean

        'Variaveis Locais
        Dim oSqlParameter(2) As SqlParameter
        Dim oSqlDataReader As SqlDataReader
        Dim bFlag As Boolean = False
        Dim i As Integer = 0

        Try

            'Seta Parametros - E-mail
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "email"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 150
            oSqlParameter(i).Value = oLogin.email : i += 1

            'Seta Parametros - Senha
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).ParameterName = "senha"
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = Cripitografar(oLogin.senha.ToUpper)

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_validate_login", oSqlParameter)

            If oSqlDataReader.Read() Then

                'Seta Retorno da Função
                bFlag = True

            End If

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            Return bFlag

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Sub UpdatePassword(ByVal sEmail As String,
                              ByVal sPassword As String)

        'Variaveis Locais
        Dim oSqlParameter(1) As SqlParameter
        Dim i As Integer = 0

        Try

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
            oSqlParameter(i).Value = Cripitografar(sPassword)

            'Executa Query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_login_senha", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function LoadEmail(ByVal sEmail As String) As String

        'Variaveis Locais
        Dim oSqlParameter(1) As SqlParameter
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
            Return CType(ExecuteScalar(sConnection, CommandType.StoredProcedure, "sp_select_login_email_senha", oSqlParameter), String)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Sub LoadPerfil(ByVal iCodigoEmpresa As Integer,
                          ByVal iCodigoUsuario AS Integer,
                          ByRef oFormularioVisualizar As FormularioVisualizar)

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

            'Seta Parametros - Código Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_perfil_direito_usuario_visualizar", oSqlParameter)

            If oSqlDataReader.Read() Then

                oFormularioVisualizar = New FormularioVisualizar

                oFormularioVisualizar.adm_cliente = IIf(IsDBNull(oSqlDataReader.Item("adm_cliente")) = False AndAlso oSqlDataReader.Item("adm_cliente") = "S", True, False)
                oFormularioVisualizar.adm_empresa = IIf(IsDBNull(oSqlDataReader.Item("adm_empresa")) = False AndAlso oSqlDataReader.Item("adm_empresa") = "S", True, False)
                oFormularioVisualizar.adm_usuario = IIf(IsDBNull(oSqlDataReader.Item("adm_usuario")) = False AndAlso oSqlDataReader.Item("adm_usuario") = "S", True, False)
                oFormularioVisualizar.adm_perfil = IIf(ISDBNULL(oSqlDataReader.Item("adm_perfil")) = False AndAlso oSqlDataReader.Item("adm_perfil") = "S", True, False)
                oFormularioVisualizar.adm_perfil_hierarquia = IIf(IsDBNull(oSqlDataReader.Item("adm_perfil_hierarquia")) = False AndAlso oSqlDataReader.Item("adm_perfil_hierarquia") = "S", True, False)
                oFormularioVisualizar.audit_externa = IIf(IsDBNull(oSqlDataReader.Item("audit_externa")) = False AndAlso oSqlDataReader.Item("audit_externa") = "S", True, False)
                oFormularioVisualizar.audit_corporativo = IIf(IsDBNull(oSqlDataReader.Item("audit_corporativo")) = False AndAlso oSqlDataReader.Item("audit_corporativo") = "S", True, False)
                oFormularioVisualizar.audit_laudo = IIf(IsDBNull(oSqlDataReader.Item("audit_laudo")) = False AndAlso oSqlDataReader.Item("audit_laudo") = "S", True, False)
                oFormularioVisualizar.audit_normas_procedimentos = IIf(IsDBNull(oSqlDataReader.Item("audit_normas_procedimentos")) = False AndAlso oSqlDataReader.Item("audit_normas_procedimentos") = "S", True, False)
                oFormularioVisualizar.audit_relatorio = IIf(IsDBNull(oSqlDataReader.Item("audit_relatorio")) = False AndAlso oSqlDataReader.Item("audit_relatorio") = "S", True, False)
                oFormularioVisualizar.audit_corporativo_historico = IIf(IsDBNull(oSqlDataReader.Item("audit_corporativo_historico")) = False AndAlso oSqlDataReader.Item("audit_corporativo_historico") = "S", True, False)
                oFormularioVisualizar.audit_relatorio_mensal_pcm = IIf(IsDBNull(oSqlDataReader.Item("audit_relatorio_mensal_pcm")) = False AndAlso oSqlDataReader.Item("audit_relatorio_mensal_pcm") = "S", True, False)
                oFormularioVisualizar.cad_apartamento = IIf(IsDBNull(oSqlDataReader.Item("cad_apartamento")) = False AndAlso oSqlDataReader.Item("cad_apartamento") = "S", True, False)
                oFormularioVisualizar.cad_ar_condicionado = IIf(IsDBNull(oSqlDataReader.Item("cad_ar_condicionado")) = False AndAlso oSqlDataReader.Item("cad_ar_condicionado") = "S", True, False)
                oFormularioVisualizar.cad_atividade = IIf(IsDBNull(oSqlDataReader.Item("cad_atividade")) = False AndAlso oSqlDataReader.Item("cad_atividade") = "S", True, False)
                oFormularioVisualizar.cad_auditoria_corporativo = IIf(IsDBNull(oSqlDataReader.Item("cad_auditoria_corporativo")) = False AndAlso oSqlDataReader.Item("cad_auditoria_corporativo") = "S", True, False)
                oFormularioVisualizar.cad_auditoria_qualidade = IIf(IsDBNull(oSqlDataReader.Item("cad_auditoria_qualidade")) = False AndAlso oSqlDataReader.Item("cad_auditoria_qualidade") = "S", True, False)
                oFormularioVisualizar.cad_categoria = IIf(IsDBNull(oSqlDataReader.Item("cad_categoria")) = False AndAlso oSqlDataReader.Item("cad_categoria") = "S", True, False)
                oFormularioVisualizar.cad_checklist = IIf(IsDBNull(oSqlDataReader.Item("cad_checklist")) = False AndAlso oSqlDataReader.Item("cad_checklist") = "S", True, False)
                oFormularioVisualizar.cad_cliente = IIf(IsDBNull(oSqlDataReader.Item("cad_cliente")) = False AndAlso oSqlDataReader.Item("cad_cliente") = "S", True, False)
                oFormularioVisualizar.cad_cliente_acordo_comercial = IIf(IsDBNull(oSqlDataReader.Item("cad_cliente_acordo_comercial")) = False AndAlso oSqlDataReader.Item("cad_cliente_acordo_comercial") = "S", True, False)
                oFormularioVisualizar.cad_departamento = IIf(IsDBNull(oSqlDataReader.Item("cad_departamento")) = False AndAlso oSqlDataReader.Item("cad_departamento") = "S", True, False)
                oFormularioVisualizar.cad_departamento_gestor = IIf(IsDBNull(oSqlDataReader.Item("cad_departamento_gestor")) = False AndAlso oSqlDataReader.Item("cad_departamento_gestor") = "S", True, False)
                oFormularioVisualizar.cad_equipamento = IIf(IsDBNull(oSqlDataReader.Item("cad_equipamento")) = False AndAlso oSqlDataReader.Item("cad_equipamento") = "S", True, False)
                oFormularioVisualizar.cad_enxoval = iif(ISDBNULL(oSqlDataReader.Item("cad_enxoval")) = False AndAlso oSqlDataReader.Item("cad_enxoval") = "S", True, False)
                oFormularioVisualizar.cad_familia_equipamento = IIf(IsDBNull(oSqlDataReader.Item("cad_familia_equipamento")) = False AndAlso oSqlDataReader.Item("cad_familia_equipamento") = "S", True, False)
                oFormularioVisualizar.cad_fornecedor = IIf(IsDBNull(oSqlDataReader.Item("cad_fornecedor")) = False AndAlso oSqlDataReader.Item("cad_fornecedor") = "S", True, False)
                oFormularioVisualizar.cad_funcao = IIf(IsDBNull(oSqlDataReader.Item("cad_funcao")) = False AndAlso oSqlDataReader.Item("cad_funcao") = "S", True, False)
                oFormularioVisualizar.cad_funcionario = IIf(IsDBNull(oSqlDataReader.Item("cad_funcionario")) = False AndAlso oSqlDataReader.Item("cad_funcionario") = "S", True, False)
                oFormularioVisualizar.cad_dedetizacao = IIf(IsDBNull(oSqlDataReader.Item("cad_dedetizacao")) = False AndAlso oSqlDataReader.Item("cad_dedetizacao") = "S", True, False)
                oFormularioVisualizar.cad_grupo_item_medicao = IIf(IsDBNull(oSqlDataReader.Item("cad_grupo_item_medicao")) = False AndAlso oSqlDataReader.Item("cad_grupo_item_medicao") = "S", True, False)
                oFormularioVisualizar.cad_grupo_produto = IIf(IsDBNull(oSqlDataReader.Item("cad_grupo_produto")) = False AndAlso oSqlDataReader.Item("cad_grupo_produto") = "S", True, False)
                oFormularioVisualizar.cad_item_medicao = IIf(IsDBNull(oSqlDataReader.Item("cad_item_medicao")) = False AndAlso oSqlDataReader.Item("cad_item_medicao") = "S", True, False)
                oFormularioVisualizar.cad_itens_gerais = IIf(IsDBNull(oSqlDataReader.Item("cad_itens_gerais")) = False AndAlso oSqlDataReader.Item("cad_itens_gerais") = "S", True, False)
                oFormularioVisualizar.cad_justificativa_apontamento = IIf(IsDBNull(oSqlDataReader.Item("cad_justificativa_apontamento")) = False AndAlso oSqlDataReader.Item("cad_justificativa_apontamento") = "S", True, False)
                oFormularioVisualizar.cad_justificativa_falta = IIf(IsDBNull(oSqlDataReader.Item("cad_justificativa_falta")) = False AndAlso oSqlDataReader.Item("cad_justificativa_falta") = "S", True, False)
                oFormularioVisualizar.cad_justificativa_cancelar_ordem_servico = IIf(IsDBNull(oSqlDataReader.Item("cad_justificativa_cancelar_ordem_servico")) = False AndAlso oSqlDataReader.Item("cad_justificativa_cancelar_ordem_servico") = "S", True, False)
                oFormularioVisualizar.adm_configuracao_desempenho_unidade = IIf(IsDBNull(oSqlDataReader.Item("adm_configuracao_desempenho_unidade")) = False AndAlso oSqlDataReader.Item("adm_configuracao_desempenho_unidade") = "S", True, False)
                oFormularioVisualizar.cad_prioridade = IIf(IsDBNull(oSqlDataReader.Item("cad_prioridade")) = False AndAlso oSqlDataReader.Item("cad_prioridade") = "S", True, False)
                oFormularioVisualizar.cad_produto = IIf(IsDBNull(oSqlDataReader.Item("cad_produto")) = False AndAlso oSqlDataReader.Item("cad_produto") = "S", True, False)
                oFormularioVisualizar.cad_laudo = IIf(IsDBNull(oSqlDataReader.Item("cad_laudo")) = False AndAlso oSqlDataReader.Item("cad_laudo") = "S", True, False)
                oFormularioVisualizar.cad_preventiva = IIf(IsDBNull(oSqlDataReader.Item("cad_preventiva")) = False AndAlso oSqlDataReader.Item("cad_preventiva") = "S", True, False)
                oFormularioVisualizar.cad_rotina = IIf(IsDBNull(oSqlDataReader.Item("cad_rotina")) = False AndAlso oSqlDataReader.Item("cad_rotina") = "S", True, False)
                oFormularioVisualizar.cad_setor = IIf(IsDBNull(oSqlDataReader.Item("cad_setor")) = False AndAlso oSqlDataReader.Item("cad_setor") = "S", True, False)
                oFormularioVisualizar.cad_tarefa = IIf(IsDBNull(oSqlDataReader.Item("cad_tarefa")) = False AndAlso oSqlDataReader.Item("cad_tarefa") = "S", True, False)
                oFormularioVisualizar.cad_tipo_apartamento = IIf(IsDBNull(oSqlDataReader.Item("cad_tipo_apartamento")) = False AndAlso oSqlDataReader.Item("cad_tipo_apartamento") = "S", True, False)
                oFormularioVisualizar.cad_tipo_ar_condicionado = IIf(IsDBNull(oSqlDataReader.Item("cad_tipo_ar_condicionado")) = False AndAlso oSqlDataReader.Item("cad_tipo_ar_condicionado") = "S", True, False)
                oFormularioVisualizar.cad_tipo_cama = IIf(IsDBNull(oSqlDataReader.Item("cad_tipo_cama")) = False AndAlso oSqlDataReader.Item("cad_tipo_cama") = "S", True, False)
                oFormularioVisualizar.cad_tipo_despesa = IIf(IsDBNull(oSqlDataReader.Item("cad_tipo_despesa")) = False AndAlso oSqlDataReader.Item("cad_tipo_despesa") = "S", True, False)
                oFormularioVisualizar.cad_treinamento = IIf(IsDBNull(oSqlDataReader.Item("cad_treinamento")) = False AndAlso oSqlDataReader.Item("cad_treinamento") = "S", True, False)
                oFormularioVisualizar.cad_unidade = IIf(IsDBNull(oSqlDataReader.Item("cad_unidade")) = False AndAlso oSqlDataReader.Item("cad_unidade") = "S", True, False)
                oFormularioVisualizar.cad_relatorio_itens_auditaveis = IIf(IsDBNull(oSqlDataReader.Item("cad_relatorio_itens_auditaveis")) = False AndAlso oSqlDataReader.Item("cad_relatorio_itens_auditaveis") = "S", True, False)
                oFormularioVisualizar.cad_item_os_hospede = IIf(IsDBNull(oSqlDataReader.Item("cad_item_os_hospede")) = False AndAlso oSqlDataReader.Item("cad_item_os_hospede") = "S", True, False)


                oFormularioVisualizar.cfg_opera = IIf(IsDBNull(oSqlDataReader.Item("cfg_opera")) = False AndAlso oSqlDataReader.Item("cfg_opera") = "S", True, False)
                oFormularioVisualizar.fin_contrato = IIf(IsDBNull(oSqlDataReader.Item("fin_contrato")) = False AndAlso oSqlDataReader.Item("fin_contrato") = "S", True, False)
                oFormularioVisualizar.fin_controle_gasto = IIf(IsDBNull(oSqlDataReader.Item("fin_controle_gasto")) = False AndAlso oSqlDataReader.Item("fin_controle_gasto") = "S", True, False)
                oFormularioVisualizar.fin_input_despesa = IIf(IsDBNull(oSqlDataReader.Item("fin_input_despesa")) = False AndAlso oSqlDataReader.Item("fin_input_despesa") = "S", True, False)
                oFormularioVisualizar.green_lancamento = IIf(IsDBNull(oSqlDataReader.Item("green_lancamento")) = False AndAlso oSqlDataReader.Item("green_lancamento") = "S", True, False)

                oFormularioVisualizar.governanca = IIf(IsDBNull(oSqlDataReader.Item("governanca")) = False AndAlso oSqlDataReader.Item("governanca") = "S", True, False)
                oFormularioVisualizar.gov_funcionario = IIf(IsDBNull(oSqlDataReader.Item("gov_funcionario")) = False AndAlso oSqlDataReader.Item("gov_funcionario") = "S", True, False)
                oFormularioVisualizar.gov_planejamento = IIf(IsDBNull(oSqlDataReader.Item("gov_planejamento")) = False AndAlso oSqlDataReader.Item("gov_planejamento") = "S", True, False)
                oFormularioVisualizar.gov_planejamento_historico = IIf(IsDBNull(oSqlDataReader.Item("gov_planejamento_historico")) = False AndAlso oSqlDataReader.Item("gov_planejamento_historico") = "S", True, False)
                oFormularioVisualizar.gov_historico = IIf(IsDBNull(oSqlDataReader.Item("gov_historico")) = False AndAlso oSqlDataReader.Item("gov_historico") = "S", True, False)
                oFormularioVisualizar.gov_dashboard = IIf(IsDBNull(oSqlDataReader.Item("gov_dashboard")) = False AndAlso oSqlDataReader.Item("gov_dashboard") = "S", True, False)
                oFormularioVisualizar.gov_apontamento = IIf(IsDBNull(oSqlDataReader.Item("gov_apontamento")) = False AndAlso oSqlDataReader.Item("gov_apontamento") = "S", True, False)
                oFormularioVisualizar.gov_status_uh = IIf(IsDBNull(oSqlDataReader.Item("gov_status_uh")) = False AndAlso oSqlDataReader.Item("gov_status_uh") = "S", True, False)
                oFormularioVisualizar.gov_lavanderia = IIf(IsDBNull(oSqlDataReader.Item("gov_lavanderia")) = False AndAlso oSqlDataReader.Item("gov_lavanderia") = "S", True, False)
                oFormularioVisualizar.gov_log_alteracao_status_gov = IIf(IsDBNull(oSqlDataReader.Item("gov_log_alteracao_status_gov")) = False AndAlso oSqlDataReader.Item("gov_log_alteracao_status_gov") = "S", True, False)
                oFormularioVisualizar.gov_inventario_enxoval = IIf(IsDBNull(oSqlDataReader.Item("govInventarioEnxoval")) = False AndAlso oSqlDataReader.Item("govInventarioEnxoval") = "S", True, False)
                oFormularioVisualizar.gov_movimentacao_enxoval = IIf(IsDBNull(oSqlDataReader.Item("govMovimentacaoEnxoval")) = False AndAlso oSqlDataReader.Item("govMovimentacaoEnxoval") = "S", True, False)

                oFormularioVisualizar.log_book = IIf(IsDBNull(oSqlDataReader.Item("log_book")) = False AndAlso oSqlDataReader.Item("log_book") = "S", True, False)
                oFormularioVisualizar.ordem_servico = IIf(IsDBNull(oSqlDataReader.Item("ordem_servico")) = False AndAlso oSqlDataReader.Item("ordem_servico") = "S", True, False)
                oFormularioVisualizar.ordem_servico_atribuir = IIf(IsDBNull(oSqlDataReader.Item("ordem_servico_atribuir")) = False AndAlso oSqlDataReader.Item("ordem_servico_atribuir") = "S", True, False)
                oFormularioVisualizar.uh_atividade = IIf(IsDBNull(oSqlDataReader.Item("uh_atividade")) = False AndAlso oSqlDataReader.Item("uh_atividade") = "S", True, False)
                oFormularioVisualizar.pcm_apontamento_os_edit = IIf(IsDBNull(oSqlDataReader.Item("pcm_apontamento_os_edit")) = False AndAlso oSqlDataReader.Item("pcm_apontamento_os_edit") = "S", True, False)
                oFormularioVisualizar.pcm_apontamento_laudo = IIf(IsDBNull(oSqlDataReader.Item("pcm_apontamento_laudo")) = False AndAlso oSqlDataReader.Item("pcm_apontamento_laudo") = "S", True, False)
                oFormularioVisualizar.pcm_apontamento_preventiva = IIf(IsDBNull(oSqlDataReader.Item("pcm_apontamento_preventiva")) = False AndAlso oSqlDataReader.Item("pcm_apontamento_preventiva") = "S", True, False)
                oFormularioVisualizar.pcm_apontamento_os = IIf(IsDBNull(oSqlDataReader.Item("pcm_apontamento_os")) = False AndAlso oSqlDataReader.Item("pcm_apontamento_os") = "S", True, False)
                oFormularioVisualizar.pcm_apontamento_rotina = IIf(IsDBNull(oSqlDataReader.Item("pcm_apontamento_rotina")) = False AndAlso oSqlDataReader.Item("pcm_apontamento_rotina") = "S", True, False)
                oFormularioVisualizar.pcm_cronograma_semanal = IIf(IsDBNull(oSqlDataReader.Item("pcm_cronograma_semanal")) = False AndAlso oSqlDataReader.Item("pcm_cronograma_semanal") = "S", True, False)
                oFormularioVisualizar.pcm_falta = IIf(IsDBNull(oSqlDataReader.Item("pcm_falta")) = False AndAlso oSqlDataReader.Item("pcm_falta") = "S", True, False)
                oFormularioVisualizar.pcm_manutencao_laudo = IIf(IsDBNull(oSqlDataReader.Item("pcm_manutencao_laudo")) = False AndAlso oSqlDataReader.Item("pcm_manutencao_laudo") = "S", True, False)
                oFormularioVisualizar.pcm_manutencao_preventiva = IIf(IsDBNull(oSqlDataReader.Item("pcm_manutencao_preventiva")) = False AndAlso oSqlDataReader.Item("pcm_manutencao_preventiva") = "S", True, False)
                oFormularioVisualizar.pcm_manutencao_rotina = IIf(IsDBNull(oSqlDataReader.Item("pcm_manutencao_rotina")) = False AndAlso oSqlDataReader.Item("pcm_manutencao_rotina") = "S", True, False)
                oFormularioVisualizar.pcm_historico_programada = IIf(IsDBNull(oSqlDataReader.Item("pcm_historico_programada")) = False AndAlso oSqlDataReader.Item("pcm_historico_programada") = "S", True, False)
                oFormularioVisualizar.pcm_plano_acao = IIf(IsDBNull(oSqlDataReader.Item("pcm_plano_acao")) = False AndAlso oSqlDataReader.Item("pcm_plano_acao") = "S", True, False)
                oFormularioVisualizar.pcm_requisicao = IIf(IsDBNull(oSqlDataReader.Item("pcm_requisicao")) = False AndAlso oSqlDataReader.Item("pcm_requisicao") = "S", True, False)
                oFormularioVisualizar.pcm_requisicao_aprovar_reprovar = IIf(IsDBNull(oSqlDataReader.Item("pcm_requisicao_aprovar_reprovar")) = False AndAlso oSqlDataReader.Item("pcm_requisicao_aprovar_reprovar") = "S", True, False)
                oFormularioVisualizar.pmoc = IIf(IsDBNull(oSqlDataReader.Item("pmoc")) = False AndAlso oSqlDataReader.Item("pmoc") = "S", True, False)
                oFormularioVisualizar.pmoc_andar = IIf(IsDBNull(oSqlDataReader.Item("pmoc_andar")) = False AndAlso oSqlDataReader.Item("pmoc_andar") = "S", True, False)
                oFormularioVisualizar.pmoc_bup = IIf(IsDBNull(oSqlDataReader.Item("pmoc_bup")) = False AndAlso oSqlDataReader.Item("pmoc_bup") = "S", True, False)
                oFormularioVisualizar.pmoc_cronograma = IIf(IsDBNull(oSqlDataReader.Item("pmoc_cronograma_bup")) = False AndAlso oSqlDataReader.Item("pmoc_cronograma_bup") = "S", True, False)
                oFormularioVisualizar.pmoc_apontamento = IIf(IsDBNull(oSqlDataReader.Item("pmoc_apontamento")) = False AndAlso oSqlDataReader.Item("pmoc_apontamento") = "S", True, False)
                oFormularioVisualizar.pmoc_cronograma = IIf(IsDBNull(oSqlDataReader.Item("pmoc_cronograma")) = False AndAlso oSqlDataReader.Item("pmoc_cronograma") = "S", True, False)
                oFormularioVisualizar.pmoc_historico = IIf(IsDBNull(oSqlDataReader.Item("pmoc_historico")) = False AndAlso oSqlDataReader.Item("pmoc_historico") = "S", True, False)
                oFormularioVisualizar.rel_custo_horas_trabalhadas = IIf(IsDBNull(oSqlDataReader.Item("rel_custo_horas_trabalhadas")) = False AndAlso oSqlDataReader.Item("rel_custo_horas_trabalhadas") = "S", True, False)
                oFormularioVisualizar.rel_funcionario_horas_trabalhadas = IIf(IsDBNull(oSqlDataReader.Item("rel_funcionario_horas_trabalhadas")) = False AndAlso oSqlDataReader.Item("rel_funcionario_horas_trabalhadas") = "S", True, False)
                oFormularioVisualizar.rel_funcionario_ociosidade = IIf(IsDBNull(oSqlDataReader.Item("rel_funcionario_ociosidade")) = False AndAlso oSqlDataReader.Item("rel_funcionario_ociosidade") = "S", True, False)
                oFormularioVisualizar.rel_green_planet = IIf(IsDBNull(oSqlDataReader.Item("rel_green_planet")) = False AndAlso oSqlDataReader.Item("rel_green_planet") = "S", True, False)
                oFormularioVisualizar.rel_manutencao_aberto_concluido = IIf(IsDBNull(oSqlDataReader.Item("rel_manutencao_aberto_concluido")) = False AndAlso oSqlDataReader.Item("rel_manutencao_aberto_concluido") = "S", True, False)
                oFormularioVisualizar.rel_manutencao = IIf(IsDBNull(oSqlDataReader.Item("rel_manutencao")) = False AndAlso oSqlDataReader.Item("rel_manutencao") = "S", True, False)
                oFormularioVisualizar.rel_manutencao_categoria = IIf(IsDBNull(oSqlDataReader.Item("rel_manutencao_categoria")) = False AndAlso oSqlDataReader.Item("rel_manutencao_categoria") = "S", True, False)
                oFormularioVisualizar.rel_manutencao_equipamento = IIf(IsDBNull(oSqlDataReader.Item("rel_manutencao_equipamento")) = False AndAlso oSqlDataReader.Item("rel_manutencao_equipamento") = "S", True, False)
                oFormularioVisualizar.rel_manutencao_executor = IIf(IsDBNull(oSqlDataReader.Item("rel_manutencao_executor")) = False AndAlso oSqlDataReader.Item("rel_manutencao_executor") = "S", True, False)
                oFormularioVisualizar.rel_manutencao_setor = IIf(IsDBNull(oSqlDataReader.Item("rel_manutencao_setor")) = False AndAlso oSqlDataReader.Item("rel_manutencao_setor") = "S", True, False)
                oFormularioVisualizar.rel_manutencao_solicitante = IIf(IsDBNull(oSqlDataReader.Item("rel_manutencao_solicitante")) = False AndAlso oSqlDataReader.Item("rel_manutencao_solicitante") = "S", True, False)
                oFormularioVisualizar.rel_manutencao_tempo_medio_atendimento = IIf(IsDBNull(oSqlDataReader.Item("rel_manutencao_tempo_medio_atendimento")) = False AndAlso oSqlDataReader.Item("rel_manutencao_tempo_medio_atendimento") = "S", True, False)
                oFormularioVisualizar.rel_manutencao_tipo_ordem_servico = IIf(IsDBNull(oSqlDataReader.Item("rel_manutencao_tipo_ordem_servico")) = False AndAlso oSqlDataReader.Item("rel_manutencao_tipo_ordem_servico") = "S", True, False)
                oFormularioVisualizar.rel_pmoc_mes = IIf(IsDBNull(oSqlDataReader.Item("rel_pmoc_mes")) = False AndAlso oSqlDataReader.Item("rel_pmoc_mes") = "S", True, False)
                oFormularioVisualizar.rel_pmoc_bimestral = IIf(IsDBNull(oSqlDataReader.Item("rel_pmoc_bimestral")) = False AndAlso oSqlDataReader.Item("rel_pmoc_bimestral") = "S", True, False)
                oFormularioVisualizar.rel_log_book = IIf(IsDBNull(oSqlDataReader.Item("rel_log_book")) = False AndAlso oSqlDataReader.Item("rel_log_book") = "S", True, False)
                oFormularioVisualizar.rel_nao_conformidade = IIf(IsDBNull(oSqlDataReader.Item("rel_nao_conformidade")) = False AndAlso oSqlDataReader.Item("rel_nao_conformidade") = "S", True, False)
                oFormularioVisualizar.rel_preventiva_mes = IIf(IsDBNull(oSqlDataReader.Item("rel_preventiva_mes")) = False AndAlso oSqlDataReader.Item("rel_preventiva_mes") = "S", True, False)
                oFormularioVisualizar.rel_dinamico = IIf(IsDBNull(oSqlDataReader.Item("rel_dinamico")) = False AndAlso oSqlDataReader.Item("rel_dinamico") = "S", True, False)
                oFormularioVisualizar.rel_consumo_enxoval = IIf(IsDBNull(oSqlDataReader.Item("rel_consumo_enxoval")) = False AndAlso oSqlDataReader.Item("rel_consumo_enxoval") = "S", True, False)
                oFormularioVisualizar.rel_horas_trabalhadas_governanca = IIf(IsDBNull(oSqlDataReader.Item("rel_horas_trabalhadas_governanca")) = False AndAlso oSqlDataReader.Item("rel_horas_trabalhadas_governanca") = "S", True, False)
                oFormularioVisualizar.rel_camareira_uh = IIf(IsDBNull(oSqlDataReader.Item("rel_camareira_uh")) = False AndAlso oSqlDataReader.Item("rel_camareira_uh") = "S", True, False)
                oFormularioVisualizar.rel_responsavel_vistoria_uh = IIf(IsDBNull(oSqlDataReader.Item("rel_responsavel_vistoria_uh")) = False AndAlso oSqlDataReader.Item("rel_responsavel_vistoria_uh") = "S", True, False)
                oFormularioVisualizar.rel_camareira_nc = IIf(IsDBNull(oSqlDataReader.Item("rel_camareira_nc")) = False AndAlso oSqlDataReader.Item("rel_camareira_nc") = "S", True, False)
                oFormularioVisualizar.uh_checklist = IIf(IsDBNull(oSqlDataReader.Item("uh_checklist")) = False AndAlso oSqlDataReader.Item("uh_checklist") = "S", True, False)
                oFormularioVisualizar.uh_checklist_historico = IIf(IsDBNull(oSqlDataReader.Item("uh_checklist_historico")) = False AndAlso oSqlDataReader.Item("uh_checklist_historico") = "S", True, False)
                oFormularioVisualizar.uh_dedetizacao = IIf(IsDBNull(oSqlDataReader.Item("uh_dedetizacao")) = False AndAlso oSqlDataReader.Item("uh_dedetizacao") = "S", True, False)
                oFormularioVisualizar.excel_ordem_servico = IIf(IsDBNull(oSqlDataReader.Item("excel_ordem_servico")) = False AndAlso oSqlDataReader.Item("excel_ordem_servico") = "S", True, False)
                oFormularioVisualizar.excel_plano_acao_qa = IIf(IsDBNull(oSqlDataReader.Item("excel_plano_acao_qa")) = False AndAlso oSqlDataReader.Item("excel_plano_acao_qa") = "S", True, False)

                oFormularioVisualizar.est_requisicao_compra = IIf(IsDBNull(oSqlDataReader.Item("est_requisicao_compra")) = False AndAlso oSqlDataReader.Item("est_requisicao_compra") = "S", True, False)
                oFormularioVisualizar.est_ordem_compra = IIf(IsDBNull(oSqlDataReader.Item("est_ordem_compra")) = False AndAlso oSqlDataReader.Item("est_ordem_compra") = "S", True, False)
                oFormularioVisualizar.est_entrada = IIf(IsDBNull(oSqlDataReader.Item("est_entrada")) = False AndAlso oSqlDataReader.Item("est_entrada") = "S", True, False)
                oFormularioVisualizar.est_saida = IIf(IsDBNull(oSqlDataReader.Item("est_saida")) = False AndAlso oSqlDataReader.Item("est_saida") = "S", True, False)
                oFormularioVisualizar.est_inventario = IIf(IsDBNull(oSqlDataReader.Item("est_inventario")) = False AndAlso oSqlDataReader.Item("est_inventario") = "S", True, False)
                oFormularioVisualizar.est_listagem = IIf(IsDBNull(oSqlDataReader.Item("est_listagem")) = False AndAlso oSqlDataReader.Item("est_listagem") = "S", True, False)

                oFormularioVisualizar.aeb_contrato = IIf(IsDBNull(oSqlDataReader.Item("aeb_contrato")) = False AndAlso oSqlDataReader.Item("aeb_contrato") = "S", True, False)
                oFormularioVisualizar.aeb_laudo = IIf(IsDBNull(oSqlDataReader.Item("aeb_laudo")) = False AndAlso oSqlDataReader.Item("aeb_laudo") = "S", True, False)
                oFormularioVisualizar.aeb_normas_procedimentos = IIf(IsDBNull(oSqlDataReader.Item("aeb_normas_procedimentos")) = False AndAlso oSqlDataReader.Item("aeb_normas_procedimentos") = "S", True, False)
                oFormularioVisualizar.aeb_auditoria_externa = IIf(IsDBNull(oSqlDataReader.Item("aeb_auditoria_externa")) = False AndAlso oSqlDataReader.Item("aeb_auditoria_externa") = "S", True, False)

                oFormularioVisualizar.qa_auditoria = IIf(IsDBNull(oSqlDataReader.Item("qa_auditoria")) = False AndAlso oSqlDataReader.Item("qa_auditoria") = "S", True, False)
                oFormularioVisualizar.qa_auditoria_historico = IIf(IsDBNull(oSqlDataReader.Item("qa_auditoria_historico")) = False AndAlso oSqlDataReader.Item("qa_auditoria_historico") = "S", True, False)
                oFormularioVisualizar.qa_auditoria_cronograma = IIf(IsDBNull(oSqlDataReader.Item("qa_auditoria_cronograma")) = False AndAlso oSqlDataReader.Item("qa_auditoria_cronograma") = "S", True, False)
                oFormularioVisualizar.qa_plano_acao = IIf(IsDBNull(oSqlDataReader.Item("qa_plano_acao")) = False AndAlso oSqlDataReader.Item("qa_plano_acao") = "S", True, False)
                oFormularioVisualizar.qa_tarefa = IIf(IsDBNull(oSqlDataReader.Item("qa_tarefa")) = False AndAlso oSqlDataReader.Item("qa_tarefa") = "S", True, False)
                oFormularioVisualizar.qa_tarefa_historico = IIf(IsDBNull(oSqlDataReader.Item("qa_tarefa_historico")) = False AndAlso oSqlDataReader.Item("qa_tarefa_historico") = "S", True, False)

                oFormularioVisualizar.dash_desempenho = IIf(IsDBNull(oSqlDataReader.Item("dash_desempenho")) = False AndAlso oSqlDataReader.Item("dash_desempenho") = "S", True, False)
                oFormularioVisualizar.dash_desempenho_qa = IIf(IsDBNull(oSqlDataReader.Item("dash_desempenho_qa")) = False AndAlso oSqlDataReader.Item("dash_desempenho_qa") = "S", True, False)

                oFormularioVisualizar.cad_area_comum = IIf(IsDBNull(oSqlDataReader.Item("cad_area_comum")) = False AndAlso oSqlDataReader.Item("cad_area_comum") = "S", True, False)
                oFormularioVisualizar.agenda_area_comum = IIf(IsDBNull(oSqlDataReader.Item("agenda_area_comum")) = False AndAlso oSqlDataReader.Item("agenda_area_comum") = "S", True, False)
                oFormularioVisualizar.agenda_entrada = IIf(IsDBNull(oSqlDataReader.Item("agenda_entrada")) = False AndAlso oSqlDataReader.Item("agenda_entrada") = "S", True, False)
                oFormularioVisualizar.agenda_saida = IIf(IsDBNull(oSqlDataReader.Item("agenda_saida")) = False AndAlso oSqlDataReader.Item("agenda_saida") = "S", True, False)

                oFormularioVisualizar.upload_excel = IIf(IsDBNull(oSqlDataReader.Item("upload_excel")) = False AndAlso oSqlDataReader.Item("upload_excel") = "S", True, False)
                oFormularioVisualizar.upload_pmoc = IIf(IsDBNull(oSqlDataReader.Item("upload_pmoc")) = False AndAlso oSqlDataReader.Item("upload_pmoc") = "S", True, False)

                oFormularioVisualizar.lav_historico = IIf(IsDBNull(oSqlDataReader.Item("lav_historico")) = False AndAlso oSqlDataReader.Item("lav_historico") = "S", True, False)
                oFormularioVisualizar.lav_apontamento = IIf(IsDBNull(oSqlDataReader.Item("lav_apontamento")) = False AndAlso oSqlDataReader.Item("lav_apontamento") = "S", True, False)
                oFormularioVisualizar.rel_lav_controle = IIf(IsDBNull(oSqlDataReader.Item("rel_lav_controle")) = False AndAlso oSqlDataReader.Item("rel_lav_controle") = "S", True, False)

            End If

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub LoadPerfil(ByVal iCodigoEmpresa As Integer,
                          ByVal iCodigoUsuario AS Integer,
                          ByVal sFormulario As String,
                          Optional ByRef bInserir As Boolean = False,
                          Optional ByRef bEditar As Boolean = False,
                          Optional ByRef bExcluir As Boolean = False,
                          Optional ByRef bAdministrador As Boolean = False)

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

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Formulário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "formulario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sFormulario

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_perfil_direito_dados", oSqlParameter)

            While oSqlDataReader.Read

                bInserir = oSqlDataReader.Item("inserir")
                bEditar = oSqlDataReader.Item("editar")
                bExcluir = oSqlDataReader.Item("excluir")
                bAdministrador = oSqlDataReader.Item("administrador")

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub LoadPerfil(ByVal iCodigoEmpresa As Integer,
                          ByVal iCodigoUnidade As Integer,
                          ByVal iCodigoUsuario As Integer,
                          ByVal sFormulario As String,
                          Optional ByRef bInserir As Boolean = False,
                          Optional ByRef bEditar As Boolean = False,
                          Optional ByRef bExcluir As Boolean = False,
                          Optional ByRef bAdministrador As Boolean = False,
                          Optional ByRef bImprimir As Boolean = False)

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

            'Seta Parametros - Código Unidade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_unidade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoUnidade : i += 1

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Formulário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "formulario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sFormulario

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_perfil_direito_dados", oSqlParameter)

            While oSqlDataReader.Read

                bInserir = oSqlDataReader.Item("inserir")
                bEditar = oSqlDataReader.Item("editar")
                bExcluir = oSqlDataReader.Item("excluir")
                bAdministrador = oSqlDataReader.Item("administrador")
                bImprimir = oSqlDataReader.Item("imprimir")

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub LoadPerfil(ByVal iCodigoEmpresa As Integer,
                          ByVal iCodigoUsuario AS Integer,
                          ByVal sFormulario As String,
                          Optional ByRef bInserir As Boolean = False,
                          Optional ByRef bEditar As Boolean = False,
                          Optional ByRef bExcluir As Boolean = False,
                          Optional ByRef bAdministrador As Boolean = False,
                          Optional ByRef bImprimir As Boolean = False)

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

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Formulário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "formulario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sFormulario

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_perfil_direito_dados", oSqlParameter)

            While oSqlDataReader.Read

                bInserir = oSqlDataReader.Item("inserir")
                bEditar = oSqlDataReader.Item("editar")
                bExcluir = oSqlDataReader.Item("excluir")
                bAdministrador = oSqlDataReader.Item("administrador")
                bImprimir = oSqlDataReader.Item("imprimir")

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub LoadPerfil(ByVal iCodigoEmpresa As Integer,
                          ByVal iCodigoUsuario As Integer,
                          ByVal sFormulario As String,
                          ByVal sDireito As String,
                          Optional ByRef bReturn As Boolean = False)

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

            'Seta Parametros - Usuário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_usuario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoUsuario : i += 1

            'Seta Parametros - Direito
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "direito"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sDireito : i += 1

            'Seta Parametros - Formulário
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "formulario"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 50
            oSqlParameter(i).Value = sFormulario

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_perfil_funcao_dados", oSqlParameter)

            While oSqlDataReader.Read

                bReturn = oSqlDataReader.Item(sDireito)

            End While

            'Fecha o SqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

End Class
