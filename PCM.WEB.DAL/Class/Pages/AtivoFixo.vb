Imports System.Data.SqlClient
Imports System.Windows
Imports OracleInternal.Json
Imports PCM.WEB.DAL.SQLHelper
Imports PCM.WEB.MODELS

Public Class AtivoFixo

    Private sConnection As String

    Sub New(ByVal sCon As String)
        sConnection = sCon
    End Sub

#Region "::: ASSET :::"

    Public Sub InsertAsset(ByVal codigoEmpresa As Integer,
                           ByVal codigoUnidade As Integer,
                           ByVal assetCode As String,
                           ByVal descricao As String,
                           ByVal numeroSerie As String,
                           ByVal tag As String,
                           ByVal contaContabil As String,
                           ByVal dataCompra As String,
                           ByVal valorCompra As Double,
                           ByVal tempoDepreciacaoMes As Integer,
                           ByVal notaFiscal As String,
                           ByVal codigoStatus As Integer,
                           ByVal codigoSetor As Integer,
                           ByVal codigoApartamento As Integer,
                           ByVal codigoUsuarioResponsavel As Integer,
                           ByVal arquivo As String,
                           ByVal codigoUsuario As Integer)

        Try

            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("asset_code", SqlDbType.VarChar, assetCode),
                CriarParametro("descricao", SqlDbType.VarChar, descricao),
                CriarParametro("numero_serie", SqlDbType.VarChar, numeroSerie),
                CriarParametro("tag", SqlDbType.VarChar, tag),
                CriarParametro("conta_contabil", SqlDbType.VarChar, contaContabil),
                CriarParametro("nota_fiscal", SqlDbType.VarChar, notaFiscal),
                CriarParametro("data_compra", SqlDbType.Date, IIf(IsDate(dataCompra), dataCompra, DBNull.Value)),
                CriarParametro("valor_compra", SqlDbType.Decimal, valorCompra),
                CriarParametro("tempo_depreciacao_mes", SqlDbType.SmallInt, tempoDepreciacaoMes),
                CriarParametro("codigo_status", SqlDbType.SmallInt, IIf(codigoStatus = -1, DBNull.Value, codigoStatus)),
                CriarParametro("codigo_setor", SqlDbType.Int, IIf(codigoSetor = -1, DBNull.Value, codigoSetor)),
                CriarParametro("codigo_apartamento", SqlDbType.Int, IIf(codigoApartamento = -1, DBNull.Value, codigoApartamento)),
                CriarParametro("codigo_usuario_responsavel", SqlDbType.Int, IIf(codigoUsuarioResponsavel = -1, DBNull.Value, codigoUsuarioResponsavel)),
                CriarParametro("arquivo", SqlDbType.VarChar, arquivo),
                CriarParametro("codigo_usuario_input", SqlDbType.Int, codigoUsuario)
            }

            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_cadastro_basico_asset", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateAsset(ByVal codigoEmpresa As Integer,
                           ByVal codigoUnidade As Integer,
                           ByVal assetCode As String,
                           ByVal descricao As String,
                           ByVal numeroSerie As String,
                           ByVal tag As String,
                           ByVal contaContabil As String,
                           ByVal dataCompra As String,
                           ByVal valorCompra As Double,
                           ByVal tempoDepreciacaoMes As Integer,
                           ByVal notaFiscal As String,
                           ByVal codigoStatus As Integer,
                           ByVal codigoSetor As Integer,
                           ByVal codigoApartamento As Integer,
                           ByVal codigoUsuarioResponsavel As Integer,
                           ByVal arquivo As String,
                           ByVal codigoUsuario As Integer,
                           ByVal codigo As Long)

        Try

            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("asset_code", SqlDbType.VarChar, assetCode),
                CriarParametro("descricao", SqlDbType.VarChar, descricao),
                CriarParametro("numero_serie", SqlDbType.VarChar, numeroSerie),
                CriarParametro("tag", SqlDbType.VarChar, tag),
                CriarParametro("conta_contabil", SqlDbType.VarChar, contaContabil),
                CriarParametro("nota_fiscal", SqlDbType.VarChar, notaFiscal),
                CriarParametro("data_compra", SqlDbType.Date, IIf(IsDate(dataCompra), dataCompra, DBNull.Value)),
                CriarParametro("valor_compra", SqlDbType.Decimal, valorCompra),
                CriarParametro("tempo_depreciacao_mes", SqlDbType.SmallInt, tempoDepreciacaoMes),
                CriarParametro("codigo_status", SqlDbType.SmallInt, IIf(codigoStatus = -1, DBNull.Value, codigoStatus)),
                CriarParametro("codigo_setor", SqlDbType.Int, IIf(codigoSetor = -1, DBNull.Value, codigoSetor)),
                CriarParametro("codigo_apartamento", SqlDbType.Int, IIf(codigoApartamento = -1, DBNull.Value, codigoApartamento)),
                CriarParametro("codigo_usuario_responsavel", SqlDbType.Int, IIf(codigoUsuarioResponsavel = -1, DBNull.Value, codigoUsuarioResponsavel)),
                CriarParametro("arquivo", SqlDbType.VarChar, arquivo),
                CriarParametro("codigo_usuario_update", SqlDbType.Int, codigoUsuario),
                CriarParametro("codigo", SqlDbType.BigInt, codigo)
            }

            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_cadastro_basico_asset", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub DeleteAsset(ByVal codigo As Long,
                       ByVal codigoEmpresa As Integer,
                       ByVal codigoUsuario As Integer)

        Try

            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo", SqlDbType.BigInt, codigo),
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_usuario_update", SqlDbType.Int, codigoUsuario)
            }

            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_cadastro_basico_asset", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function LoadAsset(ByVal codigoEmpresa As Integer,
                              ByVal codigoUnidade As Integer,
                              ByVal codigo As String,
                              ByVal descricao As String,
                              ByVal status As Integer,
                              ByVal localizacao As String) As Object

        Try

            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("codigo", SqlDbType.VarChar, codigo),
                CriarParametro("descricao", SqlDbType.VarChar, descricao),
                CriarParametro("status", SqlDbType.SmallInt, status),
                CriarParametro("localizacao", SqlDbType.VarChar, localizacao)
            }

            Return LoadDynamicGrid(sConnection, "sp_select_cadastro_basico_asset", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function LoadAssetInfo(ByVal codigo As Long,
                                  ByVal codigoEmpresa As Integer) As AssetModel

        Try

            Dim oReturn As New AssetModel

            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo", SqlDbType.BigInt, codigo),
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa)
            }

            Using oReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_asset_info", oSqlParameter)

                If oReader.HasRows Then

                    oReader.Read()

                    oReturn.codigo = SafeGetInt64(oReader, "codigo")
                    oReturn.codigoEmpresa = SafeGetInt16(oReader, "codigo_empresa")
                    oReturn.codigoUnidade = SafeGetInt32(oReader, "codigo_unidade")
                    oReturn.assetCode = SafeGetString(oReader, "asset_code")
                    oReturn.descricao = SafeGetString(oReader, "descricao")
                    oReturn.numeroSerie = SafeGetString(oReader, "numero_serie")
                    oReturn.tag = SafeGetString(oReader, "tag")
                    oReturn.contaContabil = SafeGetString(oReader, "conta_contabil")
                    oReturn.notaFiscal = SafeGetString(oReader, "nota_fiscal")
                    oReturn.dataCompra = SafeGetString(oReader, "data_compra")
                    oReturn.valorCompra = SafeGetFloat(oReader, "valor_compra")
                    oReturn.tempoDepreciacaoMes = SafeGetInt16(oReader, "tempo_depreciacao_mes")
                    oReturn.codigoStatus = SafeGetInt16(oReader, "codigo_status")
                    oReturn.codigoSetor = SafeGetInt32(oReader, "codigo_setor")
                    oReturn.codigoApartamento = SafeGetInt32(oReader, "codigo_apartamento")
                    oReturn.codigoUsuarioResponsavel = SafeGetInt32(oReader, "codigo_usuario_responsavel")
                    oReturn.ativo = SafeGetBoolean(oReader, "ativo")

                End If

            End Using

            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ValidaAsset(ByVal codigoEmpresa As Integer,
                                ByVal assetCode As String,
                                ByVal codigo As Long) As Boolean

        Try

            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("asset_code", SqlDbType.VarChar, assetCode),
                CriarParametro("codigo", SqlDbType.BigInt, codigo)
            }

            Dim result = ExecuteScalar(sConnection, CommandType.StoredProcedure, "sp_validate_cadastro_basico_asset", oSqlParameter)

            Return IIf(Convert.ToInt32(result) > 0, False, True)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: ASSET MOVEMENT :::"

    Public Function LoadAssetMovement(ByVal codigoEmpresa As Integer,
                                      ByVal codigoUnidade As Integer,
                                      ByVal codigoTipoMovimentacao As Integer,
                                      ByVal assetCode As String,
                                      ByVal documento As String,
                                      ByVal dataInicio As String,
                                      ByVal dataTermino As String,
                                      ByVal origem As String,
                                      ByVal destino As String) As Object

        Try

            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("asset_code", SqlDbType.VarChar, assetCode),
                CriarParametro("codigo_tipo_movimentacao", SqlDbType.SmallInt, codigoTipoMovimentacao),
                CriarParametro("documento", SqlDbType.VarChar, documento),
                CriarParametro("data_inicio", SqlDbType.VarChar, IIf(IsDate(dataInicio), dataInicio, DBNull.Value)),
                CriarParametro("data_termino", SqlDbType.VarChar, IIf(IsDate(dataTermino), dataTermino, DBNull.Value)),
                CriarParametro("origem", SqlDbType.VarChar, origem),
                CriarParametro("destino", SqlDbType.VarChar, destino)
            }

            Return LoadDynamicGrid(sConnection, "sp_select_asset_movement", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function InsertAssetMovement(ByVal codigoEmpresa As Integer,
                                        ByVal codigoUnidade As Integer,
                                        ByVal codigoTipoMovimentacao As Integer,
                                        ByVal dataMovimentacao As String,
                                        ByVal documento As String,
                                        ByVal codigoAsset As Long,
                                        ByVal codigoSetorDestino As Integer,
                                        ByVal codigoApartamentoDestino As Long,
                                        ByVal codigoFornecedorDestino As Long,
                                        ByVal valor As Double,
                                        ByVal observacao As String,
                                        ByVal arquivo As String,
                                        ByVal codigoUsuario As Integer) As defaultResponse


        Dim _response As New defaultResponse

        Try

            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("codigo_tipo_movimentacao", SqlDbType.Int, codigoTipoMovimentacao),
                CriarParametro("data_movimentacao", SqlDbType.Date, dataMovimentacao),
                CriarParametro("documento", SqlDbType.VarChar, documento),
                CriarParametro("codigo_asset", SqlDbType.BigInt, codigoAsset),
                CriarParametro("codigo_setor", SqlDbType.BigInt, IIf(codigoSetorDestino = -1, DBNull.Value, codigoSetorDestino)),
                CriarParametro("codigo_apartamento", SqlDbType.BigInt, IIf(codigoApartamentoDestino = -1, DBNull.Value, codigoApartamentoDestino)),
                CriarParametro("codigo_fornecedor", SqlDbType.BigInt, IIf(codigoFornecedorDestino = -1, DBNull.Value, codigoFornecedorDestino)),
                CriarParametro("valor", SqlDbType.Float, valor),
                CriarParametro("observacao", SqlDbType.VarChar, observacao),
                CriarParametro("arquivo", SqlDbType.VarChar, arquivo),
                CriarParametro("codigo_usuario", SqlDbType.Int, codigoUsuario)
            }

            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_asset_movement", oSqlParameter)

            _response.success = True

        Catch SqlEx As SqlException
            _response.success = True
            _response.message = SqlEx.Message.ToString()
        Catch ex As Exception
            _response.success = True
            _response.message = ex.Message.ToString()
        End Try

        Return _response

    End Function

    Public Function LoadConfiguracaoTipoMovimentacao(ByVal codigoTipoMovimentacao As Integer) As AssetTipoMovimentacaoConfig

        Dim _return As New AssetTipoMovimentacaoConfig

        Try

            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_tipo_movimentacao", SqlDbType.SmallInt, codigoTipoMovimentacao)
            }

            Using _sqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_static_tipo_movimentacao_asset_config", oSqlParameter)

                While _sqlDataReader.Read

                    _return.documento = SafeGetBoolean(_sqlDataReader, "documento")
                    _return.valor = SafeGetBoolean(_sqlDataReader, "valor")
                    _return.setor = SafeGetBoolean(_sqlDataReader, "setor")
                    _return.apartamento = SafeGetBoolean(_sqlDataReader, "apartamento")
                    _return.fornecedor = SafeGetBoolean(_sqlDataReader, "fornecedor")

                End While

            End Using

        Catch SqlEx As SqlException
            _return.success = False
            _return.message = SqlEx.Message.ToString()
        Catch ex As Exception
            _return.success = False
            _return.message = ex.Message.ToString()
        End Try

        Return _return

    End Function

#End Region

#Region "::: ASSET INVENTORY MANAGER:::"

    Public Function LoadAssetInventoryMng(ByVal codigoEmpresa As Integer,
                                          ByVal codigoUnidade As Integer,
                                          ByVal descricao As String,
                                          ByVal dataInicio As String,
                                          ByVal dataTermino As String,
                                          ByVal statusInventario As Integer) As Object

        Try

            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("descricao", SqlDbType.VarChar, descricao),
                CriarParametro("data_inicio", SqlDbType.VarChar, IIf(IsDate(dataInicio), dataInicio, DBNull.Value)),
                CriarParametro("data_termino", SqlDbType.VarChar, IIf(IsDate(dataTermino), dataTermino, DBNull.Value)),
                CriarParametro("status", SqlDbType.SmallInt, statusInventario)
            }

            Return LoadDynamicGrid(sConnection, "sp_select_asset_inventory_manager", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function LoadAssetInventoryInfo(ByVal codigo As Long) As AssetInventoryInfo

        Dim _return As New AssetInventoryInfo()

        Try

            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo", SqlDbType.BigInt, codigo)
            }

            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_asset_inventario_info", oSqlParameter)

                If oSqlDataReader.HasRows Then

                    While oSqlDataReader.Read()
                        _return.descricao = SafeGetString(oSqlDataReader, "descricao")
                        _return.codigoInventario = SafeGetLong(oSqlDataReader, "codigo")
                        _return.codigoUnidade = SafeGetInt32(oSqlDataReader, "codigo_unidade")
                        _return.unidade = SafeGetString(oSqlDataReader, "unidade")
                    End While

                End If

            End Using

            Return _return

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function LoadAssetInventoryMngDetails(ByVal codigoInventario As Long) As List(Of AssetInventoryDetails)

        Try

            Dim oReturn As New List(Of AssetInventoryDetails)
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_inventario", SqlDbType.BigInt, codigoInventario)
            }

            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_asset_inventory_manager_details", oSqlParameter)

                While oSqlDataReader.Read

                    Dim oInfo As New AssetInventoryDetails With {
                    .assetCode = SafeGetString(oSqlDataReader, "asset_code"),
                    .descricao = SafeGetString(oSqlDataReader, "descricao"),
                    .origem = SafeGetString(oSqlDataReader, "origem"),
                    .destino = SafeGetString(oSqlDataReader, "destino"),
                    .usuario = SafeGetString(oSqlDataReader, "usuario"),
                    .data = SafeGetString(oSqlDataReader, "data"),
                    .ativoCadastrado = SafeGetBooleanSimNao(oSqlDataReader, "ativo_cadastrado")
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

    Public Function InsertInventory(ByVal codigoEmpresa As Integer,
                                    ByVal codigoUnidade As Integer,
                                    ByVal descricao As String,
                                    ByVal codigoUsuario As Integer) As defaultResponse


        Dim _response As New defaultResponse

        Try

            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("codigo_usuario", SqlDbType.Int, codigoUsuario),
                CriarParametro("descricao", SqlDbType.VarChar, descricao)
            }

            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_asset_inventario", oSqlParameter)

            _response.success = True

        Catch SqlEx As SqlException
            _response.success = True
            _response.message = SqlEx.Message.ToString()
        Catch ex As Exception
            _response.success = True
            _response.message = ex.Message.ToString()
        End Try

        Return _response

    End Function

    Public Function HasInventoryOpened(ByVal codigoEmpresa As Integer,
                                       ByVal codigoUnidade As Integer) As Integer


        Try

            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade)
            }

            Dim iResult As Integer = ExecuteScalar(sConnection, CommandType.StoredProcedure, "sp_select_asset_inventario_opened", oSqlParameter)

            Return iResult

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Sub CloseAssetInventory(ByVal codigoEmpresa As Long,
                                   ByVal codigoUnidade As Integer,
                                   ByVal codigoUsuario As Integer)

        Try

            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("codigo_usuario", SqlDbType.Int, codigoUsuario)
            }

            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_asset_inventory_close", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function LoadAssetInventoried(ByVal codigo As Long,
                                         ByVal type As Integer) As Object

        Try

            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo", SqlDbType.BigInt, codigo),
                CriarParametro("type", SqlDbType.SmallInt, type)
            }

            Return LoadDynamicGrid(sConnection, "sp_select_asset_manager_movement", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function InsertInventoryAssetMovement(ByVal codigoInventario As Long,
                                                 ByVal codigoEmpresa As Integer,
                                                 ByVal codigoTipoMovimentacao As Integer,
                                                 ByVal dataMovimentacao As String,
                                                 ByVal documento As String,
                                                 ByVal assetCode As String,
                                                 ByVal codigoSetorDestino As Integer,
                                                 ByVal codigoApartamentoDestino As Long,
                                                 ByVal codigoFornecedorDestino As Long,
                                                 ByVal valor As Double,
                                                 ByVal observacao As String,
                                                 ByVal arquivo As String,
                                                 ByVal codigoUsuario As Integer) As defaultResponse


        Dim _response As New defaultResponse

        Try

            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_inventario", SqlDbType.BigInt, codigoInventario),
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_tipo_movimentacao", SqlDbType.Int, codigoTipoMovimentacao),
                CriarParametro("data_movimentacao", SqlDbType.Date, dataMovimentacao),
                CriarParametro("documento", SqlDbType.VarChar, documento),
                CriarParametro("asset_code", SqlDbType.BigInt, assetCode),
                CriarParametro("codigo_setor", SqlDbType.BigInt, IIf(codigoSetorDestino = -1, DBNull.Value, codigoSetorDestino)),
                CriarParametro("codigo_apartamento", SqlDbType.BigInt, IIf(codigoApartamentoDestino = -1, DBNull.Value, codigoApartamentoDestino)),
                CriarParametro("codigo_fornecedor", SqlDbType.BigInt, IIf(codigoFornecedorDestino = -1, DBNull.Value, codigoFornecedorDestino)),
                CriarParametro("valor", SqlDbType.Float, valor),
                CriarParametro("observacao", SqlDbType.VarChar, observacao),
                CriarParametro("arquivo", SqlDbType.VarChar, arquivo),
                CriarParametro("codigo_usuario", SqlDbType.Int, codigoUsuario)
            }

            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_asset_inventory_movement", oSqlParameter)

            _response.success = True

        Catch SqlEx As SqlException
            _response.success = True
            _response.message = SqlEx.Message.ToString()
        Catch ex As Exception
            _response.success = True
            _response.message = ex.Message.ToString()
        End Try

        Return _response

    End Function

#End Region

#Region "::: ASSET INVENTORY MANAGER:::"

    Public Function GetInventarioAtivo(ByVal codigoEmpresa As Integer,
                                       ByVal codigoUnidade As Integer) As AssetInventoryInfo

        Try


            Dim oReturn As New AssetInventoryInfo
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade)
            }

            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_asset_inventario_ativo", oSqlParameter)

                While oSqlDataReader.Read

                    oReturn.descricao = SafeGetString(oSqlDataReader, "descricao")
                    oReturn.codigoInventario = SafeGetLong(oSqlDataReader, "codigo")

                End While

            End Using

            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function LoadInventory(ByVal codigoEmpresa As Integer,
                                  ByVal codigoUnidade As Integer,
                                  ByVal codigoSetor As Integer,
                                  ByVal codigoApartamento As Integer,
                                  ByVal codigoInventario As Long) As List(Of AssetInventory)

        Try

            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_inventario", SqlDbType.BigInt, codigoInventario),
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("codigo_setor", SqlDbType.Int, codigoSetor),
                CriarParametro("codigo_apartamento", SqlDbType.Int, codigoApartamento)
            }
            Dim oList As New List(Of AssetInventory)

            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_asset_inventario_count", oSqlParameter)

                If oSqlDataReader.HasRows Then

                    While oSqlDataReader.Read()
                        Dim oItem As New AssetInventory
                        oItem.asset = SafeGetString(oSqlDataReader, "asset")
                        oItem.descricao = SafeGetString(oSqlDataReader, "descricao")
                        oItem.ativoCadastrado = SafeGetBoolean(oSqlDataReader, "ativo_cadastrado")
                        oItem.cssClass = SafeGetString(oSqlDataReader, "cssClass")
                        oList.Add(oItem)
                    End While

                End If

            End Using

            Return oList

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Sub InsertInventoryAsset(ByVal codigoInventario As Long,
                                    ByVal codigoEmpresa As Integer,
                                    ByVal codigoUnidade As Integer,
                                    ByVal codigoSetor As Integer,
                                    ByVal codigoApartamento As Integer,
                                    ByVal assetCode As String,
                                    ByVal codigoUsuario As Integer,
                                    ByVal ativoCadastrado As Boolean,
                                    ByVal descricaoInformada As String)

        Try

            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_inventario", SqlDbType.BigInt, codigoInventario),
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("codigo_setor", SqlDbType.Int, codigoSetor),
                CriarParametro("codigo_apartamento", SqlDbType.Int, IIf(codigoApartamento = -1, DBNull.Value, codigoApartamento)),
                CriarParametro("codigo_usuario", SqlDbType.Int, codigoUsuario),
                CriarParametro("asset_code", SqlDbType.VarChar, assetCode),
                CriarParametro("ativo_cadastrado", SqlDbType.Bit, ativoCadastrado),
                CriarParametro("descricao_informada", SqlDbType.VarChar, If(descricaoInformada, ""))
            }

            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_asset_inventory_count", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function ExistsAsset(ByVal codigoEmpresa As Integer,
                                ByVal codigoUnidade As Integer,
                                ByVal assetCode As String) As Boolean

        Try

            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("asset_code", SqlDbType.VarChar, assetCode)
            }

            Using oReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_validate_inventario_asset_code", oSqlParameter)

                If oReader.Read() Then
                    Return Convert.ToInt32(oReader("exists_flag")) = 1
                End If

            End Using

            Return False

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

End Class
