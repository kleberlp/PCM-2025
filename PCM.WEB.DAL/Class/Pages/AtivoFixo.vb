Imports System.Data.SqlClient
Imports OracleInternal.Json
Imports PCM.WEB.DAL.SQLHelper
Imports PCM.WEB.MODELS

Public Class AtivoFixo

    Private sConnection As String

    Sub New(ByVal sCon As String)
        sConnection = sCon
    End Sub

#Region "::: ASSET :::"

    Public Sub InsertAsset(ByVal oModel As AssetModel)

        Try

            Dim oSqlParameter As SqlParameter() = {
            CriarParametro("codigo_empresa", SqlDbType.SmallInt, oModel.codigoEmpresa),
            CriarParametro("codigo_unidade", SqlDbType.Int, oModel.codigoUnidade),
            CriarParametro("asset_code", SqlDbType.VarChar, oModel.assetCode),
            CriarParametro("descricao", SqlDbType.VarChar, oModel.descricao),
            CriarParametro("numero_serie", SqlDbType.VarChar, oModel.numeroSerie),
            CriarParametro("tag", SqlDbType.VarChar, oModel.tag),
            CriarParametro("data_compra", SqlDbType.Date, IIf(IsDate(oModel.dataCompra), oModel.dataCompra, DBNull.Value)),
            CriarParametro("valor_compra", SqlDbType.Decimal, oModel.valorCompra),
            CriarParametro("codigo_status", SqlDbType.SmallInt, oModel.codigoStatus),
            CriarParametro("codigo_setor", SqlDbType.Int, oModel.codigoSetor),
            CriarParametro("codigo_apartamento", SqlDbType.Int, oModel.codigoApartamento),
            CriarParametro("codigo_usuario_responsavel", SqlDbType.Int, oModel.codigoUsuarioResponsavel),
            CriarParametro("codigo_usuario_input", SqlDbType.Int, oModel.codigoUsuarioInput)
        }

            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_cadastro_basico_asset", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateAsset(ByVal oModel As AssetModel)

        Try

            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo", SqlDbType.BigInt, oModel.codigo),
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, oModel.codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, oModel.codigoUnidade),
                CriarParametro("asset_code", SqlDbType.VarChar, oModel.assetCode),
                CriarParametro("descricao", SqlDbType.VarChar, oModel.descricao),
                CriarParametro("numero_serie", SqlDbType.VarChar, oModel.numeroSerie),
                CriarParametro("tag", SqlDbType.VarChar, oModel.tag),
                CriarParametro("data_compra", SqlDbType.Date, IIf(IsDate(oModel.dataCompra), oModel.dataCompra, DBNull.Value)),
                CriarParametro("valor_compra", SqlDbType.Decimal, oModel.valorCompra),
                CriarParametro("codigo_status", SqlDbType.SmallInt, oModel.codigoStatus),
                CriarParametro("codigo_setor", SqlDbType.Int, oModel.codigoSetor),
                CriarParametro("codigo_apartamento", SqlDbType.Int, oModel.codigoApartamento),
                CriarParametro("codigo_usuario_responsavel", SqlDbType.Int, oModel.codigoUsuarioResponsavel),
                CriarParametro("codigo_usuario_update", SqlDbType.Int, oModel.codigoUsuarioUpdate)
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
                    oReturn.codigoStatus = SafeGetInt16(oReader, "codigo_status")
                    oReturn.codigoSetor = SafeGetNullableInt32(oReader, "codigo_setor")
                    oReturn.codigoApartamento = SafeGetNullableInt32(oReader, "codigo_apartamento")
                    oReturn.codigoUsuarioResponsavel = SafeGetNullableInt32(oReader, "codigo_usuario_responsavel")
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
                                      ByVal codigo As String,
                                      ByVal descricao As String,
                                      ByVal status As Integer,
                                      ByVal setor As Integer,
                                      ByVal apartamento As Integer) As Object

        Try

            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("codigo", SqlDbType.VarChar, codigo),
                CriarParametro("descricao", SqlDbType.VarChar, descricao),
                CriarParametro("status", SqlDbType.SmallInt, status),
                CriarParametro("setor", SqlDbType.Int, setor),
                CriarParametro("apartamento", SqlDbType.Int, apartamento)
            }

            Return LoadDynamicGrid(sConnection, "sp_select_asset_movement", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: ASSET INVENTORY :::"

    Public Function CreateAssetInventory(ByVal codigoEmpresa As Integer,
                                         ByVal codigoUnidade As Integer,
                                         ByVal codigoUsuario As Integer) As defaultResponse


        Dim _response As New defaultResponse

        Try

            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("codigo_usuario", SqlDbType.Int, codigoUsuario),
                CriarParametro("numero_documento", SqlDbType.VarChar, codigoUsuario, ParameterDirection.Output)
            }

            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_asset_inventory", oSqlParameter)

            _response.success = True
            _response.documentNumber = oSqlParameter(3).Value.ToString()

        Catch SqlEx As SqlException
            _response.success = True
            _response.message = SqlEx.Message.ToString()
        Catch ex As Exception
            _response.success = True
            _response.message = ex.Message.ToString()
        End Try

        Return _response

    End Function

    Public Sub CloseAssetInventory(ByVal inventoryId As Long,
                                   ByVal status As Integer,
                                   ByVal codigoUsuario As Integer)

        Try

            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("inventory_id", SqlDbType.BigInt, inventoryId),
                CriarParametro("status", SqlDbType.Int, status),
                CriarParametro("codigo_usuario", SqlDbType.Int, codigoUsuario)
            }

            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_asset_inventory_status", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region

End Class
