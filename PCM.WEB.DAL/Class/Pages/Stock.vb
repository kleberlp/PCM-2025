Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.Net.NetworkInformation
Imports System.Windows
Imports PCM.WEB.DAL.SQLHelper
Imports PCM.WEB.MODELS
Imports SYSPACK.WEB.MODELS

Public Class Stock

    Private sConnection As String

    Sub New(ByVal sCon As String)
        sConnection = sCon
    End Sub

    Public Function LoadStock(ByVal sProduct As String,
                              ByVal sStartDate As String,
                              ByVal sEndDate As String,
                              ByVal iStatus As Integer,
                              ByVal sLanguageID As String) As List(Of stockInfo)

        'Variaveis Locais
        Dim oReturn As New List(Of stockInfo)
        Dim i As Integer = 0

        Try

            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("product", SqlDbType.VarChar, sProduct.ToUpper()),
                CriarParametro("start_date", SqlDbType.Date, IIf(IsDate(sStartDate), sStartDate, DBNull.Value)),
                CriarParametro("end_date", SqlDbType.Date, IIf(IsDate(sEndDate), sEndDate, DBNull.Value)),
                CriarParametro("status", SqlDbType.Int, iStatus),
                CriarParametro("language_id", SqlDbType.VarChar, sLanguageID.ToUpper())
            }

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_stock", oSqlParameter)

                While oSqlDataReader.Read

                    Dim oInfo As New stockInfo

                    oInfo.product = oSqlDataReader("product")
                    oInfo.description = oSqlDataReader("description")
                    oInfo.batch = oSqlDataReader("batch")
                    oInfo.quantity = oSqlDataReader("quantity")
                    oInfo.spare_point = oSqlDataReader("spare_point")
                    oInfo.status = oSqlDataReader("status")
                    oInfo.unitary_value = oSqlDataReader("unitary_value")
                    oInfo.total_value = oSqlDataReader("total_value")

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

    Public Function LoadStockMovement(ByVal sProduct As String,
                                      ByVal sStartDate As String,
                                      ByVal sEndDate As String,
                                      ByVal iStatus As Integer,
                                      ByVal sLanguageID As String) As List(Of stock_movement_report)

        'Variaveis Locais
        Dim oReturn As New List(Of stock_movement_report)
        Dim i As Integer = 0

        Try

            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("product", SqlDbType.VarChar, sProduct.ToUpper()),
                CriarParametro("start_date", SqlDbType.Date, IIf(IsDate(sStartDate), sStartDate, DBNull.Value)),
                CriarParametro("end_date", SqlDbType.Date, IIf(IsDate(sEndDate), sEndDate, DBNull.Value)),
                CriarParametro("status", SqlDbType.Int, iStatus),
                CriarParametro("language_id", SqlDbType.VarChar, sLanguageID.ToUpper())
            }

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_stock_movement_report", oSqlParameter)

                While oSqlDataReader.Read

                    Dim oInfo As New stock_movement_report

                    oInfo.product = oSqlDataReader("product")
                    oInfo.description = oSqlDataReader("description")
                    oInfo.batch = oSqlDataReader("batch")
                    oInfo.quantity = oSqlDataReader("quantity")
                    oInfo.document = oSqlDataReader("document")
                    oInfo.date = oSqlDataReader("date")
                    oInfo.type = oSqlDataReader("type")
                    oInfo.username = oSqlDataReader("username")
                    oInfo.unitary_value = oSqlDataReader("unitary_value")
                    oInfo.total_value = oSqlDataReader("total_value")
                    oInfo.file = oSqlDataReader.Item("path_file")
                    oInfo.css_class = oSqlDataReader.Item("css_class")

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

    Public Function LoadStockMovement(ByVal sProduct As String,
                                      ByVal sLanguageID As String) As List(Of stock_movement)

        'Variaveis Locais
        Dim oReturn As New List(Of stock_movement)
        Dim i As Integer = 0

        Try

            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("product", SqlDbType.VarChar, sProduct.ToUpper()),
                CriarParametro("language_id", SqlDbType.VarChar, sLanguageID.ToUpper())
            }

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_stock_movement", oSqlParameter)

                While oSqlDataReader.Read

                    Dim oInfo As New stock_movement
                    oInfo.document = oSqlDataReader("document")
                    oInfo.date = oSqlDataReader("date")
                    oInfo.quantity = oSqlDataReader("quantity")
                    oInfo.type = oSqlDataReader("type")
                    oInfo.username = oSqlDataReader("username")

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

    Public Function GetDocumentNumberPicking() As String

        Try

            'Executa Query
            Return CType(ExecuteScalar(sConnection, CommandType.StoredProcedure, "sp_select_stock_picking_number"), String)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Sub InsertStockProduct(ByVal sDocumentNumber As String,
                                  ByVal sDocumentDate As String,
                                  ByVal sDocumentType As String,
                                  ByVal sPurchaseOrder As String,
                                  ByVal sPathFile As String,
                                  ByVal sRequester As String,
                                  ByVal lProductID As Long,
                                  ByVal sBatch As String,
                                  ByVal iUOMID As Integer,
                                  ByVal dQuantity As String,
                                  ByVal dProductValue As String,
                                  ByVal sCurrentUser As String,
                                  Optional ByVal iCostCenter As Integer = -1,
                                  Optional ByVal sBinPosition As String = "")

        Try
            ' Define parameters
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("document_number", SqlDbType.VarChar, sDocumentNumber.ToUpper()),
                CriarParametro("document_date", SqlDbType.Date, IIf(IsDate(sDocumentDate), sDocumentDate, DBNull.Value)),
                CriarParametro("document_type", SqlDbType.VarChar, sDocumentType),
                CriarParametro("purchase_order_id", SqlDbType.BigInt,
                               IIf(String.IsNullOrEmpty(sPurchaseOrder), DBNull.Value,
                                   IIf(String.IsNullOrEmpty(sPurchaseOrder.Split(";"c)(0)), DBNull.Value, sPurchaseOrder.Split(";"c)(0).ToUpper()))),
                CriarParametro("path_file", SqlDbType.VarChar, IIf(String.IsNullOrEmpty(sPathFile), DBNull.Value, sPathFile.ToUpper())),
                CriarParametro("requester", SqlDbType.VarChar, IIf(String.IsNullOrEmpty(sRequester), DBNull.Value, sRequester.ToUpper())),
                CriarParametro("product_id", SqlDbType.BigInt, lProductID),
                CriarParametro("batch", SqlDbType.VarChar, IIf(IsNothing(sBatch), DBNull.Value, sBatch)),
                CriarParametro("uom_id", SqlDbType.Int, iUOMID),
                CriarParametro("quantity", SqlDbType.Float, dQuantity.Replace(".", "")),
                CriarParametro("unitary_value", SqlDbType.Float, CDbl(dProductValue.Replace("R$ ", "").Replace(".", ""))),
                CriarParametro("cost_center_id", SqlDbType.BigInt, IIf(iCostCenter = -1, DBNull.Value, iCostCenter)),
                CriarParametro("bin_position", SqlDbType.VarChar, IIf(String.IsNullOrEmpty(sBinPosition), DBNull.Value, sBinPosition)),
                CriarParametro("current_user", SqlDbType.VarChar, sCurrentUser.ToUpper())
            }

            ' Execute the query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_stock_movement", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function ProductStockInfo(ByVal sBarcode As String,
                                     ByVal lProduct As Long,
                                     ByVal lPurcharOrderId As Long) As product_stock_info

        ' Local variable
        Dim oReturn As New product_stock_info

        Try
            ' Define parameters
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("barcode", SqlDbType.VarChar, sBarcode),
                CriarParametro("product", SqlDbType.BigInt, lProduct),
                CriarParametro("purchase_order_id", SqlDbType.BigInt, lPurcharOrderId)
            }

            ' Execute query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_register_product_stock_info", oSqlParameter)

                If oSqlDataReader.Read() Then
                    oReturn.description = oSqlDataReader("description")
                    oReturn.uom = oSqlDataReader("uom")
                    oReturn.batch_control = oSqlDataReader("batch_control")
                    oReturn.economic_batch = oSqlDataReader("economic_batch")
                    oReturn.expiration_date_control = oSqlDataReader("expiration_date_control")
                    oReturn.quality_control = oSqlDataReader("quality_control")
                    oReturn.receive_more = oSqlDataReader("receive_more")
                    oReturn.quantity = oSqlDataReader("quantity")
                    oReturn.quantity_pending = oSqlDataReader("quantity_pending")
                    oReturn.percentual = oSqlDataReader("percentual")
                End If

            End Using

            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ProductInfo(ByVal sProduct As String) As product_info

        ' Local variable
        Dim oReturn As New product_info

        Try
            ' Define parameters
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("product", SqlDbType.VarChar, sProduct.ToUpper())
            }

            ' Execute query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_register_product_stock_find", oSqlParameter)

                If oSqlDataReader.Read() Then
                    oReturn.product = oSqlDataReader("product")
                    oReturn.description = oSqlDataReader("description")
                    oReturn.uom = oSqlDataReader("uom")
                    oReturn.batch_control = oSqlDataReader("batch_control")
                End If

            End Using

            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function UOMInfo(ByVal iUOM As Integer) As uom_info

        ' Local variable
        Dim oReturn As New uom_info

        Try

            'Define parameters
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("id", SqlDbType.Int, iUOM)
            }

            ' Execute query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_register_uom_find", oSqlParameter)

                If oSqlDataReader.Read() Then
                    oReturn.decimal_places = oSqlDataReader("decimal_places")
                End If

            End Using

            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function


#Region "::: INVENTÁRIO :::"

    Public Sub UploadInventarioExcel(ByVal codigoEmpresa As Integer,
                                     ByVal codigoUnidade As Integer,
                                     ByVal codigoUsuario As Integer,
                                     ByVal file As String,
                                     ByVal worksheet As String,
                                     ByRef oDetails As List(Of estoqueInventarioDetalhe),
                                     ByRef oDetailsError As List(Of estoqueInventarioDetalheError))

        ' Local variables
        Dim sQuery As String
        Dim oOleDbConnection As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & file & ";Extended Properties='Excel 12.0 Xml;HDR=YES';")

        Try

            ' Step 1: Delete temporary inventory data
            Dim oDeleteParameters As SqlParameter() = {
                CriarParametro("codigo_usuario", SqlDbType.Int, codigoUsuario),
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade)
            }

            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_estoque_inventario_tmp", oDeleteParameters)

            ' Step 2: Get inventory query
            Dim oQueryParameters As SqlParameter() = {
                CriarParametro("tipo", SqlDbType.VarChar, "INVENTARIO"),
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("codigo_usuario", SqlDbType.Int, codigoUsuario),
                CriarParametro("worksheet", SqlDbType.VarChar, worksheet)
            }

            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_pcm_query", oQueryParameters)

                While oSqlDataReader.Read

                    sQuery = SafeGetString(oSqlDataReader, "query")
                    sQuery = sQuery.Replace(vbCrLf, "").Replace(vbTab, "").Replace(vbLf, "")

                End While

            End Using

            ' Step 3: Perform bulk copy from Excel to temporary SQL table
            oOleDbConnection.Open()
            Using oSqlConnection As New SqlConnection(sConnection),
              oOleDbCommand As New OleDbCommand(sQuery, oOleDbConnection),
              oOleDbDataReader As OleDbDataReader = oOleDbCommand.ExecuteReader(),
              oSqlBulkCopy As New SqlBulkCopy(oSqlConnection)

                oSqlConnection.Open()
                oSqlBulkCopy.BulkCopyTimeout = 500
                oSqlBulkCopy.DestinationTableName = "dbo.tb_est_inventario_tmp"
                oSqlBulkCopy.WriteToServer(oOleDbDataReader)

            End Using

        Catch ex As Exception
            Throw ex
        Finally
            oOleDbConnection.Close()
        End Try

        Try

            ' Step 4: Retrieve and process inventory details
            Dim oDetailsParameters As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("codigo_usuario", SqlDbType.Int, codigoUsuario)
            }

            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_estoque_inventario_tmp", oDetailsParameters)

                While oSqlDataReader.Read()

                    If IsDBNull(oSqlDataReader("erro")) Then
                        Dim oInfo As New EstoqueInventarioDetalhe With {
                        .produto = oSqlDataReader("produto"),
                        .descricao = oSqlDataReader("descricao"),
                        .lote = oSqlDataReader("lote"),
                        .quantidade = oSqlDataReader("quantidade"),
                        .quantidadeInventario = oSqlDataReader("quantidade_inventario"),
                        .divergencia = oSqlDataReader("divergencia")
                    }
                        oDetails.Add(oInfo)
                    Else
                        Dim oInfo As New estoqueInventarioDetalheError With {
                        .produto = oSqlDataReader("produto"),
                        .erro = oSqlDataReader("erro")
                    }
                        oDetailsError.Add(oInfo)
                    End If

                End While

            End Using

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateInventario(ByVal codigoEmpresa As Integer,
                                ByVal codigoUnidade As Integer,
                                ByVal codigoUsuario As Integer)

        Try

            ' Define parameters
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("codigo_usuario", SqlDbType.Int, codigoUsuario)
            }

            ' Execute query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_estoque_inventario_tmp", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region

#Region "::: ENTRADA :::"

    Public Sub InsertEntrada(ByVal codigoEmpresa As Integer,
                             ByVal codigoUnidade As Integer,
                             ByVal numeroDocumento As String,
                             ByVal codigoOrdemCompra As Long,
                             ByVal codigoFornecedor As Integer,
                             ByVal data As String,
                             ByVal codigoUsuario As Integer,
                             ByVal pathFile As String,
                             ByVal produtosJson As String)

        Try
            ' Define parameters
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("numero_documento", SqlDbType.VarChar, numeroDocumento.ToUpper()),
                CriarParametro("codigo_ordem_compra", SqlDbType.BigInt, IIf(codigoOrdemCompra = -1, DBNull.Value, codigoOrdemCompra)),
                CriarParametro("codigo_fornecedor", SqlDbType.Int, IIf(codigoFornecedor = -1, DBNull.Value, codigoFornecedor)),
                CriarParametro("data", SqlDbType.Date, data),
                CriarParametro("codigo_usuario", SqlDbType.Int, codigoUsuario),
                CriarParametro("path_file", SqlDbType.VarChar, IIf(String.IsNullOrEmpty(pathFile), DBNull.Value, pathFile.ToUpper())),
                CriarParametro("produtos_json", SqlDbType.NVarChar, produtosJson)
            }

            ' Execute query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_estoque_entrada", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function LoadProdutoInfo(ByVal codigoEmpresa As Integer,
                                    ByVal codigoOrdemCompra As Long,
                                    ByVal codigoProduto As Long) As Produto

        Try

            'Váriaveis Locais
            Dim oReturn As New Produto
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_ordem_compra", SqlDbType.BigInt, codigoOrdemCompra),
                CriarParametro("codigo_produto", SqlDbType.BigInt, codigoProduto)
            }

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_cadastro_basico_produto_info", oSqlParameter)

                While oSqlDataReader.Read

                    oReturn = New Produto

                    oReturn.descricao = SafeGetString(oSqlDataReader, "descricao")
                    oReturn.codigo = SafeGetLong(oSqlDataReader, "codigo")
                    oReturn.unidadeMedida = SafeGetString(oSqlDataReader, "unidade_medida")
                    oReturn.controlaLote = SafeGetBoolean(oSqlDataReader, "controla_lote")
                    oReturn.ativo = SafeGetBoolean(oSqlDataReader, "ativo")
                    oReturn.controlaDataValidade = SafeGetBoolean(oSqlDataReader, "controla_data_validade")
                    oReturn.quantidadePendente = SafeGetFloat(oSqlDataReader, "quantidade_pendente")

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

#Region "::: SAIDA :::"

    Public Sub InsertSaida(ByVal codigoEmpresa As Integer,
                           ByVal codigoUnidade As Integer,
                           ByVal codigoUsuarioRequisitante As Long,
                           ByVal codigoOrdemServico As Long,
                           ByVal data As String,
                           ByVal codigoUsuario As Integer,
                           ByVal pathFile As String,
                           ByVal produtosJson As String)

        Try
            ' Define parameters
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("codigo_usuario_requisitante", SqlDbType.BigInt, codigoUsuarioRequisitante),
                CriarParametro("codigo_ordem_servico", SqlDbType.Int, IIf(codigoOrdemServico = -1, DBNull.Value, codigoOrdemServico)),
                CriarParametro("data", SqlDbType.Date, data),
                CriarParametro("codigo_usuario", SqlDbType.Int, codigoUsuario),
                CriarParametro("path_file", SqlDbType.VarChar, IIf(String.IsNullOrEmpty(pathFile), DBNull.Value, pathFile.ToUpper())),
                CriarParametro("produtos_json", SqlDbType.NVarChar, produtosJson)
            }

            ' Execute query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_estoque_saida", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function LoadProdutoInfoSaida(ByVal codigoEmpresa As Integer,
                                         ByVal codigoProduto As Long,
                                         ByVal lote As String) As Produto

        Try

            'Váriaveis Locais
            Dim oReturn As New Produto
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_produto", SqlDbType.BigInt, codigoProduto),
                CriarParametro("lote", SqlDbType.VarChar, lote)
            }

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_estoque_produto_saida_info", oSqlParameter)

                While oSqlDataReader.Read

                    oReturn = New Produto

                    oReturn.descricao = SafeGetString(oSqlDataReader, "descricao")
                    oReturn.codigo = SafeGetLong(oSqlDataReader, "codigo")
                    oReturn.controlaLote = SafeGetBoolean(oSqlDataReader, "controla_lote")
                    oReturn.ativo = SafeGetBoolean(oSqlDataReader, "ativo")
                    oReturn.controlaDataValidade = SafeGetBoolean(oSqlDataReader, "controla_data_validade")
                    oReturn.saldo = SafeGetFloat(oSqlDataReader, "saldo")

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

#Region "::: ORDEM COMPRA :::"

    Public Sub InsertOrdemCompra(ByVal ordemCompra As String,
                                 ByVal data As String,
                                 ByVal codigoFornecedor As Integer,
                                 ByVal pathFile As String,
                                 ByVal codigoUsuario As String,
                                 ByVal produtosJson As String)

        Try

            ' Define parameters
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("ordem_compra", SqlDbType.VarChar, ordemCompra.ToUpper()),
                CriarParametro("data", SqlDbType.Date, data),
                CriarParametro("codigo_fornecedor", SqlDbType.Int, codigoFornecedor),
                CriarParametro("path_file", SqlDbType.VarChar, IIf(String.IsNullOrEmpty(pathFile), DBNull.Value, pathFile.ToUpper())),
                CriarParametro("codigo_usuario", SqlDbType.Int, codigoUsuario),
                CriarParametro("produtos_json", SqlDbType.NVarChar, produtosJson)
            }

            ' Execute query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_estoque_ordem_compra", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub DeleteOrdemCompra(ByVal codigoOrdemCompra As Long,
                                 ByVal codigoUsuario As String)

        Try
            ' Define parameters
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_ordem_compra", SqlDbType.BigInt, codigoOrdemCompra),
                CriarParametro("codigo_usuario", SqlDbType.Int, codigoUsuario)
            }

            ' Execute query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_stock_ordem_compra", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function FindPurchaseOrder(ByVal codigo As Long) As estoqueOrdemCompra

        ' Local variable
        Dim oReturn As New estoqueOrdemCompra

        Try
            ' Define parameters
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo", SqlDbType.BigInt, codigo)
            }

            ' Execute query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_stock_ordem_compra_find", oSqlParameter)

                If oSqlDataReader.Read() Then
                    oReturn.ordemCompra = oSqlDataReader("ordem_compra")
                    oReturn.dataCriacao = oSqlDataReader("data_criacao")
                    oReturn.codigoFornecedor = oSqlDataReader("codigo_fornecedor")
                    oReturn.usuario = oSqlDataReader("usuario")
                    oReturn.codigoOrdemCompra = oSqlDataReader("codigo")
                End If

            End Using

            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function LoadOrdemCompraProduto(ByVal codigoEmpresa As Integer,
                                           ByVal codigoUnidade As Integer,
                                           ByVal codigoOrdemCompra As Long) As List(Of estoqueOrdemCompraProduto)

        ' Local variable
        Dim oReturn As New List(Of estoqueOrdemCompraProduto)

        Try
            ' Define parameters
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_ordem_compra", SqlDbType.BigInt, codigoOrdemCompra)
            }

            ' Execute query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_estoque_ordem_compra_produto", oSqlParameter)

                While oSqlDataReader.Read()
                    Dim oInfo As New estoqueOrdemCompraProduto With {
                    .codigoProduto = SafeGetLong(oSqlDataReader, "codigo_produto"),
                    .produto = SafeGetString(oSqlDataReader, "produto"),
                    .codigoUnidadeMedida = SafeGetLong(oSqlDataReader, "codigo_unidade_medida"),
                    .uom = SafeGetString(oSqlDataReader, "uom"),
                    .descricao = SafeGetString(oSqlDataReader, "descricao"),
                    .quantidade = SafeGetLong(oSqlDataReader, "quantidade"),
                    .quantidadeRecebida = SafeGetLong(oSqlDataReader, "quantidade_recebida"),
                    .quantidadePendente = SafeGetLong(oSqlDataReader, "quantidade_pendente")
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

    Public Function LoadOrdemCompra(ByVal codigoEmpresa As Integer,
                                    ByVal codigoUnidade As Integer,
                                    ByVal ordemCompra As String,
                                    ByVal codigoFornecedor As Integer,
                                    ByVal status As Integer,
                                    ByVal dataInicio As String,
                                    ByVal dataTermino As String) As List(Of estoqueOrdemCompra)

        ' Local variable
        Dim oReturn As New List(Of estoqueOrdemCompra)

        Try
            ' Define parameters
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("ordem_compra", SqlDbType.VarChar, ordemCompra.ToUpper()),
                CriarParametro("codigo_fornecedor", SqlDbType.Int, codigoFornecedor),
                CriarParametro("status", SqlDbType.Int, status),
                CriarParametro("data_inicio", SqlDbType.Date, IIf(IsDate(dataInicio), dataInicio, DBNull.Value)),
                CriarParametro("data_termino", SqlDbType.Date, IIf(IsDate(dataTermino), dataTermino, DBNull.Value))
            }

            ' Execute query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_estoque_ordem_compra", oSqlParameter)

                While oSqlDataReader.Read()
                    Dim oInfo As New estoqueOrdemCompra With {
                    .ordemCompra = SafeGetString(oSqlDataReader, "ordem_compra"),
                    .codigoUnidade = SafeGetString(oSqlDataReader, "codigo_unidade"),
                    .dataCriacao = SafeGetString(oSqlDataReader, "data"),
                    .fornecedor = SafeGetString(oSqlDataReader, "fornecedor"),
                    .usuario = SafeGetString(oSqlDataReader, "usuario"),
                    .status = SafeGetLong(oSqlDataReader, "status"),
                    .statusDescricao = SafeGetString(oSqlDataReader, "status_descricao"),
                    .quantidade = SafeGetLong(oSqlDataReader, "quantidade"),
                    .quantidadePendente = SafeGetLong(oSqlDataReader, "quantidade_pendente"),
                    .quantidadeRecebida = SafeGetLong(oSqlDataReader, "quantidade_recebida"),
                    .codigoOrdemCompra = SafeGetLong(oSqlDataReader, "codigo"),
                    .cssClass = SafeGetString(oSqlDataReader, "css_class"),
                    .path = SafeGetString(oSqlDataReader, "path_file")
                }

                    oReturn.Add(oInfo)
                End While

            End Using

            ' Return result
            Return oReturn

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function LoadOrdemCompraInfo(ByVal codigoOrdemCompra As Long) As estoqueOrdemCompra

        Try

            'Váriaveis Locais
            Dim oReturn As New estoqueOrdemCompra
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_ordem_compra", SqlDbType.BigInt, codigoOrdemCompra)
            }

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_estoque_ordem_compra_info", oSqlParameter)

                While oSqlDataReader.Read

                    oReturn = New estoqueOrdemCompra
                    oReturn.codigoFornecedor = SafeGetLong(oSqlDataReader, "codigo_fornecedor")
                    oReturn.fornecedor = SafeGetString(oSqlDataReader, "fornecedor")

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

#Region "::: REQUISIÇÃO DE COMPRA :::"

    Public Sub InsertRequisicaoOrdemCompra(ByVal codigoEmpresa As Integer,
                                           ByVal codigoUnidade As Integer,
                                           ByVal codigoFornecedor As Integer,
                                           ByVal produtosJson As String,
                                           ByVal codigoUsuario As Integer)

        Try
            ' Define parameters
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("codigo_fornecedor", SqlDbType.Int, If(codigoFornecedor = -1, DBNull.Value, codigoFornecedor)),
                CriarParametro("produtos", SqlDbType.NVarChar, produtosJson),
                CriarParametro("codigo_usuario", SqlDbType.Int, codigoUsuario)
            }

            ' Execute query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_insert_estoque_requisicao_compra", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub DeleteRequisicaoCompra(ByVal codigoEmpresa As Integer,
                                      ByVal codigoUnidade As Integer,
                                      ByVal codigoRequisicaoCompra As Long,
                                      ByVal codigoUsuario As Integer)

        Try
            ' Define parameters
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("codigo_requisicao_compra", SqlDbType.BigInt, codigoRequisicaoCompra),
                CriarParametro("codigo_usuario", SqlDbType.Int, codigoUsuario)
            }

            ' Execute query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_delete_estoque_requisicao_compra", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateRequisicaoCompraStatus(ByVal codigoEmpresa As Integer,
                                            ByVal codigoUnidade As Integer,
                                            ByVal codigoRequisicaoCompra As Long,
                                            ByVal codigoFornecedor As Integer,
                                            ByVal status As Integer,
                                            ByVal ordemCompra As String,
                                            ByVal pathFile As String,
                                            ByVal codigoUsuario As Integer)

        Try
            ' Define parameters
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("codigo_requisicao_compra", SqlDbType.BigInt, codigoRequisicaoCompra),
                CriarParametro("codigo_fornecedor", SqlDbType.Int, IIf(codigoFornecedor = -1, DBNull.Value, codigoFornecedor)),
                CriarParametro("status", SqlDbType.Int, status),
                CriarParametro("ordem_compra", SqlDbType.VarChar, ordemCompra.ToUpper()),
                CriarParametro("path_file", SqlDbType.VarChar, If(String.IsNullOrEmpty(pathFile), DBNull.Value, pathFile.ToUpper())),
                CriarParametro("codigo_usuario", SqlDbType.Int, codigoUsuario)
            }

            ' Execute query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_estoque_requisicao_compra_status", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function LoadRequisicaoCompraProduto(ByVal codigoRequisicaoCompra As Long) As List(Of estoqueRequisicaoCompraProduto)

        Dim oReturn As New List(Of estoqueRequisicaoCompraProduto)

        Try
            ' Define parameters
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_requisicao_compra", SqlDbType.BigInt, codigoRequisicaoCompra)
            }

            ' Execute query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_estoque_requisicao_compra_produto", oSqlParameter)

                While oSqlDataReader.Read()
                    Dim oInfo As New estoqueRequisicaoCompraProduto With {
                        .codigoProduto = SafeGetLong(oSqlDataReader, "codigo_produto"),
                        .produto = SafeGetString(oSqlDataReader, "produto"),
                        .codigoUnidadeMedida = SafeGetLong(oSqlDataReader, "codigo_unidade_medida"),
                        .descricao = SafeGetString(oSqlDataReader, "descricao"),
                        .uom = SafeGetString(oSqlDataReader, "uom"),
                        .quantidade = SafeGetFloat(oSqlDataReader, "quantidade"),
                        .estoque = SafeGetFloat(oSqlDataReader, "estoque"),
                        .transito = SafeGetFloat(oSqlDataReader, "transito"),
                        .consumo = SafeGetFloat(oSqlDataReader, "consumo"),
                        .tempoReposicao = SafeGetFloat(oSqlDataReader, "tempo_reposicao"),
                        .loteEconomico = SafeGetFloat(oSqlDataReader, "lote_economico"),
                        .pontoReposicao = SafeGetFloat(oSqlDataReader, "ponto_reposicao")
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

    Public Function LoadRequisicaoCompra(ByVal codigoEmpresa As Integer,
                                         ByVal codigoUnidade As Integer,
                                         ByVal requisicaoCompra As String,
                                         ByVal codigoFornecedor As Integer,
                                         ByVal dataInicio As String,
                                         ByVal dataTermino As String,
                                         ByVal status As Integer) As List(Of estoqueRequisicaoCompra)

        Dim oReturn As New List(Of estoqueRequisicaoCompra)

        Try
            ' Define parameters
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("requisicao_compra", SqlDbType.VarChar, requisicaoCompra.ToUpper()),
                CriarParametro("codigo_fornecedor", SqlDbType.Int, codigoFornecedor),
                CriarParametro("data_inicio", SqlDbType.Date, IIf(IsDate(dataInicio), dataInicio, DBNull.Value)),
                CriarParametro("data_termino", SqlDbType.Date, IIf(IsDate(dataTermino), dataTermino, DBNull.Value)),
                CriarParametro("status", SqlDbType.Int, status)
            }

            ' Execute query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_estoque_requisicao_compra", oSqlParameter)

                While oSqlDataReader.Read()

                    Dim oInfo As New estoqueRequisicaoCompra With {
                        .codigoRequisicaoCompra = SafeGetLong(oSqlDataReader, "codigo"),
                        .codigoUnidade = SafeGetLong(oSqlDataReader, "codigo_unidade"),
                        .numeroRequisicao = SafeGetString(oSqlDataReader, "numero_requisicao"),
                        .ordemCompra = SafeGetString(oSqlDataReader, "ordem_compra"),
                        .dataCriacao = SafeGetString(oSqlDataReader, "data"),
                        .fornecedor = SafeGetString(oSqlDataReader, "fornecedor"),
                        .codigoFornecedor = SafeGetLong(oSqlDataReader, "codigo_fornecedor"),
                        .statusDescricao = SafeGetString(oSqlDataReader, "status_descricao"),
                        .status = SafeGetLong(oSqlDataReader, "status"),
                        .usuario = SafeGetString(oSqlDataReader, "usuario"),
                        .cssClass = SafeGetString(oSqlDataReader, "css_class")
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

    Public Function LoadRequisicaoCompraSparePart(ByVal codigoEmpresa As Integer,
                                                  ByVal codigoUnidade As Integer) As List(Of estoqueRequisicaoCompraProduto)

        Dim oReturn As New List(Of estoqueRequisicaoCompraProduto)

        Try
            ' Define parameters
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade)
            }

            ' Execute query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_estoque_requisicao_compra_spare_point", oSqlParameter)

                While oSqlDataReader.Read()

                    Dim oInfo As New estoqueRequisicaoCompraProduto With {
                        .codigoProduto = SafeGetLong(oSqlDataReader, "codigo_produto"),
                        .produto = SafeGetString(oSqlDataReader, "produto"),
                        .codigoUnidadeMedida = SafeGetLong(oSqlDataReader, "codigo_unidade_medida"),
                        .descricao = SafeGetString(oSqlDataReader, "descricao"),
                        .uom = SafeGetString(oSqlDataReader, "uom"),
                        .quantidade = SafeGetFloat(oSqlDataReader, "quantidade"),
                        .estoque = SafeGetFloat(oSqlDataReader, "estoque"),
                        .transito = SafeGetFloat(oSqlDataReader, "transito"),
                        .pontoReposicao = SafeGetFloat(oSqlDataReader, "ponto_reposicao")
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

#End Region

#Region "::: APROVAÇÃO - RECEBIMENTO A MAIOR :::"

    Public Function AprovacaoRecebimento(ByVal codigoEmpresa As Integer,
                                         ByVal codigoUnidade As Integer,
                                         ByVal ordemCompra As String,
                                         ByVal documento As String,
                                         ByVal codigoFornecedor As Integer,
                                         ByVal status As Integer,
                                         ByVal dataInicio As String,
                                         ByVal dataTermino As String) As List(Of estoqueEntrada)

        Dim oReturn As New List(Of estoqueEntrada)

        Try

            ' Define parameters
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("ordem_compra", SqlDbType.VarChar, ordemCompra.ToUpper()),
                CriarParametro("documento", SqlDbType.VarChar, documento.ToUpper()),
                CriarParametro("codigo_fornecedor", SqlDbType.Int, IIf(codigoFornecedor = -1, DBNull.Value, codigoFornecedor)),
                CriarParametro("status", SqlDbType.Int, status),
                CriarParametro("data_inicio", SqlDbType.Date, IIf(IsDate(dataInicio), dataInicio, DBNull.Value)),
                CriarParametro("data_termino", SqlDbType.Date, IIf(IsDate(dataTermino), dataTermino, DBNull.Value))
            }

            ' Execute query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_estoque_recebimento_aprovacao", oSqlParameter)

                While oSqlDataReader.Read()
                    Dim oInfo As New estoqueEntrada With {
                    .codigo = oSqlDataReader("codigo"),
                    .codigoProduto = oSqlDataReader("codigo_produto"),
                    .ordemCompra = oSqlDataReader("ordem_compra"),
                    .numeroDocumento = oSqlDataReader("numero_documento"),
                    .dataDocumento = oSqlDataReader("data_documento"),
                    .fornecedor = oSqlDataReader("fornecedor"),
                    .usuario = oSqlDataReader("usuario"),
                    .statusDescricao = oSqlDataReader("status_descricao"),
                    .status = oSqlDataReader("status"),
                    .cssClass = oSqlDataReader("css_class"),
                    .produto = oSqlDataReader("produto"),
                    .descricaoProduto = oSqlDataReader("descricao_produto"),
                    .quantidade = oSqlDataReader("quantidade"),
                    .quantidadePendente = oSqlDataReader("quantidade_pendente"),
                    .quantidadeRecebida = oSqlDataReader("quantidade_recebida")
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

    Public Sub UpdateRecebimentoStatus(ByVal codigoRecebimento As Long,
                                       ByVal status As Integer,
                                       ByVal codigoUsuario As String)

        Try
            ' Define parameters
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo", SqlDbType.BigInt, codigoRecebimento),
                CriarParametro("status", SqlDbType.Int, status),
                CriarParametro("codigo_usuario", SqlDbType.Int, codigoUsuario)
            }

            ' Execute query
            ExecuteNonQuery(sConnection, CommandType.StoredProcedure, "sp_update_estoque_recebimento_status", oSqlParameter)

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region

#Region "::: LISTAGEM :::"

    Public Function EstoqueListagem(ByVal codigoEmpresa As Integer,
                                    ByVal codigoUnidade As Integer,
                                    ByVal codigoUsuario As Integer,
                                    ByVal codigoProduto As Long,
                                    ByVal status As Integer) As List(Of estoqueListagemInventario)

        Try

            Dim oReturn As New List(Of estoqueListagemInventario)

            ' Define parameters
            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("codigo_produto", SqlDbType.BigInt, codigoProduto),
                CriarParametro("status", SqlDbType.Int, status),
                CriarParametro("codigo_usuario", SqlDbType.Int, codigoUsuario)
            }

            'Executa Query
            Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_estoque_listagem", oSqlParameter)

                While oSqlDataReader.Read

                    Dim oInfo As New estoqueListagemInventario

                    oInfo.codigoProduto = oSqlDataReader.Item("codigo_produto")
                    oInfo.produto = oSqlDataReader.Item("produto").ToString.ToUpper()
                    oInfo.descricaoProduto = oSqlDataReader.Item("descricao").ToString.ToUpper()
                    oInfo.lote = oSqlDataReader.Item("lote").ToString.ToUpper()
                    oInfo.quantidadeEntrada = oSqlDataReader.Item("quantidade_entrada")
                    oInfo.quantidadeSaida = oSqlDataReader.Item("quantidade_saida")
                    oInfo.saldo = oSqlDataReader.Item("saldo")
                    oInfo.localizacao = oSqlDataReader.Item("localizacao").ToString.ToUpper()
                    oInfo.unidadeMedida = oSqlDataReader.Item("unidade_medida").ToString.ToUpper()

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

End Class
