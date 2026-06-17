Imports System.Data
Imports System.Data.SqlClient
Imports PCM.WEB.OS.MODELS
Imports PCM.WEB.OS.DAL.SQLHelper

Public Class AtivoFixo

    Private sConnection As String

    Public Sub New(ByVal sConnectionString)
        sConnection = sConnectionString
    End Sub

#Region "::: ASSET INVENTORY :::"

    Public Function InfoInventario(ByVal uniqueId As String) As AssetInventory

        Dim oSqlParameter As List(Of SqlParameter) = New List(Of SqlParameter)
        Dim _result As New AssetInventory

        Try
            AddSqlParameter(oSqlParameter, "uniqueId", SqlDbType.VarChar, 100, uniqueId)

            Using _sqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_asset_inventario_by_id", oSqlParameter.ToArray())

                While _sqlDataReader.Read

                    _result.codigoEmpresa = SafeGetLong(_sqlDataReader, "codigo_empresa")
                    _result.codigoUnidade = SafeGetLong(_sqlDataReader, "codigo_unidade")

                End While

            End Using

            Return _result

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function GetInventarioAtivo(ByVal codigoEmpresa As Integer,
                                       ByVal codigoUnidade As Integer) As Long

        Dim oSqlParameter As List(Of SqlParameter) = New List(Of SqlParameter)

        Try
            AddSqlParameter(oSqlParameter, "codigo_empresa", SqlDbType.Int, 0, codigoEmpresa)
            AddSqlParameter(oSqlParameter, "codigo_unidade", SqlDbType.Int, 0, codigoUnidade)

            Dim result As Object = ExecuteScalar(sConnection, CommandType.StoredProcedure, "sp_select_asset_inventario_ativo", oSqlParameter.ToArray())

            Return If(result Is Nothing OrElse IsDBNull(result), -1, CLng(result))

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ExistsAsset(ByVal codigoEmpresa As Integer,
                                ByVal codigoUnidade As Integer,
                                ByVal assetCode As String) As Boolean

        Dim oSqlParameter As List(Of SqlParameter) = New List(Of SqlParameter)

        Try

            AddSqlParameter(oSqlParameter, "codigo_empresa", SqlDbType.Int, 0, codigoEmpresa)
            AddSqlParameter(oSqlParameter, "codigo_unidade", SqlDbType.Int, 0, codigoUnidade)
            AddSqlParameter(oSqlParameter, "asset_code", SqlDbType.VarChar, 50, assetCode)

            Dim result As Object = ExecuteScalar(sConnection, CommandType.StoredProcedure, "sp_validate_inventario_asset_code", oSqlParameter.ToArray())

            Return If(result IsNot Nothing AndAlso Not IsDBNull(result), CInt(result) > 0, False)

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
                                    ByVal ativoCadastrado As Boolean,
                                    ByVal descricaoInformada As String,
                                    ByVal codigoUsuario As String,
                                    Optional ByVal statusOk As Boolean = True,
                                    Optional ByVal observacao As String = "",
                                    Optional ByVal fotoPath As String = "")

        Dim oSqlParameter As List(Of SqlParameter) = New List(Of SqlParameter)

        Try

            AddSqlParameter(oSqlParameter, "codigo_inventario", SqlDbType.BigInt, 0, codigoInventario)
            AddSqlParameter(oSqlParameter, "codigo_empresa", SqlDbType.Int, 0, codigoEmpresa)
            AddSqlParameter(oSqlParameter, "codigo_unidade", SqlDbType.Int, 0, codigoUnidade)
            AddSqlParameter(oSqlParameter, "codigo_setor", SqlDbType.Int, 0, codigoSetor)
            AddSqlParameter(oSqlParameter, "codigo_apartamento", SqlDbType.Int, 0, codigoApartamento)
            AddSqlParameter(oSqlParameter, "asset_code", SqlDbType.VarChar, 50, assetCode)
            AddSqlParameter(oSqlParameter, "codigo_usuario", SqlDbType.VarChar, 50, codigoUsuario)
            AddSqlParameter(oSqlParameter, "ativo_cadastrado", SqlDbType.Bit, 0, ativoCadastrado)
            AddSqlParameter(oSqlParameter, "descricao_informada", SqlDbType.VarChar, 250, If(descricaoInformada, ""))
            AddSqlParameter(oSqlParameter, "status_ok", SqlDbType.Bit, 0, statusOk)
            AddSqlParameter(oSqlParameter, "observacao", SqlDbType.VarChar, 500, If(String.IsNullOrWhiteSpace(observacao), DBNull.Value, CObj(observacao)))
            AddSqlParameter(oSqlParameter, "foto_path", SqlDbType.VarChar, 500, If(String.IsNullOrWhiteSpace(fotoPath), DBNull.Value, CObj(fotoPath)))

            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_asset_inventory_count", oSqlParameter.ToArray())

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function LoadAssetInventory(ByVal codigoInventario As Long,
                                       ByVal codigoEmpresa As Integer,
                                       ByVal codigoUnidade As Integer,
                                       ByVal codigoSetor As Integer,
                                       ByVal codigoApartamento As Integer) As List(Of AssetInventoryItem)

        Dim oSqlParameter As List(Of SqlParameter) = New List(Of SqlParameter)

        Try

            AddSqlParameter(oSqlParameter, "codigo_inventario", SqlDbType.BigInt, 0, codigoInventario)
            AddSqlParameter(oSqlParameter, "codigo_empresa", SqlDbType.Int, 0, codigoEmpresa)
            AddSqlParameter(oSqlParameter, "codigo_unidade", SqlDbType.Int, 0, codigoUnidade)
            AddSqlParameter(oSqlParameter, "codigo_setor", SqlDbType.Int, 0, codigoSetor)
            AddSqlParameter(oSqlParameter, "codigo_apartamento", SqlDbType.Int, 0, codigoApartamento)

            Dim oReturn As New List(Of AssetInventoryItem)

            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_asset_inventario_count", oSqlParameter.ToArray())

                While oSqlDataReader.Read

                    Dim oItem As New AssetInventoryItem
                    oItem.asset = SafeGetString(oSqlDataReader, "asset")
                    oItem.descricao = SafeGetString(oSqlDataReader, "descricao")
                    oItem.cssClass = SafeGetString(oSqlDataReader, "css_class")
                    oReturn.Add(oItem)

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