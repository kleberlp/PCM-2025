Imports PCM.WEB.MODELS
Imports System.DirectoryServices
Imports System.DirectoryServices.ActiveDirectory
Imports PCM.WEB.DAL.SQLHelper
Imports System.Data.SqlClient
Imports System.Text
Imports OfficeOpenXml
Imports System.IO
Imports OfficeOpenXml.Style

Public Class Excel

    Private sConnection As String

    Sub New(ByVal sCon As String)
        sConnection = sCon
    End Sub

#Region "::: ORDEM DE SERVIÇO :::"

    Public Function OrdemServico(ByVal codigoEmpresa As Integer,
                                 ByVal codigoUsuario As Integer,
                                 Optional ByVal codigoUnidade As Integer = 0,
                                 Optional ByVal dataInicio As String = "",
                                 Optional ByVal dataTermino As String = "",
                                 Optional ByVal numeroOrdemServico As String = "",
                                 Optional ByVal ordemServicoCliente As String = "",
                                 Optional ByVal codigoSetor As Integer = 0,
                                 Optional ByVal codigoPrioridade As Integer = 0,
                                 Optional ByVal codigoEquipamento As Long = 0,
                                 Optional ByVal codigoSolicitante As Integer = 0,
                                 Optional ByVal codigoApartamento As Integer = 0,
                                 Optional ByVal executor As String = "",
                                 Optional ByVal status As Integer = 0) As MemoryStream

        Try

            'Váriavies Locais
            Dim stream As New MemoryStream

            Dim oSqlParameter As SqlParameter() = {
                CriarParametro("codigo_empresa", SqlDbType.Int, codigoEmpresa),
                CriarParametro("codigo_usuario", SqlDbType.Int, codigoUsuario),
                CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade),
                CriarParametro("ordem_servico", SqlDbType.VarChar, numeroOrdemServico),
                CriarParametro("ordem_servico_cliente", SqlDbType.VarChar, ordemServicoCliente),
                CriarParametro("data_inicio", SqlDbType.Date, dataInicio),
                CriarParametro("data_termino", SqlDbType.Date, dataTermino),
                CriarParametro("codigo_setor", SqlDbType.Int, codigoSetor),
                CriarParametro("codigo_apartamento", SqlDbType.Int, codigoApartamento),
                CriarParametro("codigo_prioridade", SqlDbType.Int, codigoPrioridade),
                CriarParametro("codigo_equipamento", SqlDbType.BigInt, codigoEquipamento),
                CriarParametro("codigo_solicitante", SqlDbType.BigInt, codigoSolicitante),
                CriarParametro("executor", SqlDbType.VarChar, executor),
                CriarParametro("status", SqlDbType.SmallInt, status)
            }

            'Executa Query
            Dim oDataset As DataSet = ExecuteDataset(sConnection, CommandType.StoredProcedure, "sp_select_excel_ordem_servico", oSqlParameter)

            Using oExcelPackage As New ExcelPackage()

                Dim oExcelExport = oExcelPackage.Workbook.Worksheets.Add("DETALHADO")

                oExcelExport.Cells("A1").LoadFromDataTable(oDataset.Tables(1), True)
                oExcelExport.Cells("A1:" & NumberToColumnExcel(oDataset.Tables(1).Columns.Count) & "1").Style.Font.Bold = True
                oExcelExport.Cells("A1:" & NumberToColumnExcel(oDataset.Tables(1).Columns.Count) & "1").Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium)
                oExcelExport.Cells("A1:" & NumberToColumnExcel(oDataset.Tables(1).Columns.Count) & "1").Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid
                oExcelExport.Cells("A1:" & NumberToColumnExcel(oDataset.Tables(1).Columns.Count) & "1").Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Blue)
                oExcelExport.Cells("A1:" & NumberToColumnExcel(oDataset.Tables(1).Columns.Count) & "1").Style.Font.Color.SetColor(System.Drawing.Color.White)
                oExcelExport.Cells("A1:" & NumberToColumnExcel(oDataset.Tables(1).Columns.Count) & "1").Style.VerticalAlignment = ExcelVerticalAlignment.Center

                oExcelExport.Cells("A:" & NumberToColumnExcel(oDataset.Tables(1).Columns.Count)).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center
                oExcelExport.Cells.AutoFitColumns()

                oExcelExport = oExcelPackage.Workbook.Worksheets.Add("RESUMIDO")

                oExcelExport.Cells("A1").LoadFromDataTable(oDataset.Tables(0), True)
                oExcelExport.Cells("A1:" & NumberToColumnExcel(oDataset.Tables(0).Columns.Count) & "1").Style.Font.Bold = True
                oExcelExport.Cells("A1:" & NumberToColumnExcel(oDataset.Tables(0).Columns.Count) & "1").Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium)
                oExcelExport.Cells("A1:" & NumberToColumnExcel(oDataset.Tables(0).Columns.Count) & "1").Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid
                oExcelExport.Cells("A1:" & NumberToColumnExcel(oDataset.Tables(0).Columns.Count) & "1").Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Blue)
                oExcelExport.Cells("A1:" & NumberToColumnExcel(oDataset.Tables(0).Columns.Count) & "1").Style.Font.Color.SetColor(System.Drawing.Color.White)
                oExcelExport.Cells("A1:" & NumberToColumnExcel(oDataset.Tables(0).Columns.Count) & "1").Style.VerticalAlignment = ExcelVerticalAlignment.Center

                oExcelExport.Cells("A:" & NumberToColumnExcel(oDataset.Tables(0).Columns.Count)).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center
                oExcelExport.Cells.AutoFitColumns()


                oExcelPackage.SaveAs(stream)
                stream.Position = 0

            End Using

            Return stream

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: PLANO DE AÇÃO - QA :::"

    Public Function PlanoAcaoQA(ByVal iCodigoEmpresa As Integer,
                                ByVal iCodigoUsuario As Integer,
                                Optional ByVal iCodigoUnidade As Integer = 0,
                                Optional ByVal sDataInicio As String = "",
                                Optional ByVal sDataTermino As String = "",
                                Optional ByVal sOrdemServico As String = "",
                                Optional ByVal iCodigoAuditoria As Integer = 0,
                                Optional ByVal iCodigoPrioridade As Integer = 0,
                                Optional ByVal iCodigoDepartamento As Integer = 0,
                                Optional ByVal iCodigoSolicitante As Integer = 0,
                                Optional ByVal sExecutor As String = "",
                                Optional ByVal iStatus As Integer = 0,
                                Optional ByVal lCodigoAuditoria As Long = -1) As List(Of MODELS.PlanoAcaoQA)

        Try

            'Váriaveis Locais
            Dim oOrdemServico As New List(Of MODELS.PlanoAcaoQA)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(12) As SqlParameter
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

            'Seta Parametros - Ordem Serviço
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "ordem_servico"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Size = 20
            oSqlParameter(i).Value = sOrdemServico : i += 1

            'Seta Parametros - CadastroBasico Interna
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_auditoria_interna"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoAuditoria : i += 1

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

            'Seta Parametros - Código Prioridade
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_prioridade"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoPrioridade : i += 1

            'Seta Parametros - Código Departamento
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_departamento"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = iCodigoDepartamento : i += 1

            'Seta Parametros - Código Solicitante
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_solicitante"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoSolicitante : i += 1

            'Seta Parametros - Executor
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "executor"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Value = 100
            oSqlParameter(i).Value = sExecutor : i += 1

            'Seta Parametros - Status
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "status"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iStatus : i += 1

            'Seta Parametros - Código CadastroBasico
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_auditoria"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = lCodigoAuditoria

            'Executa Query
            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_excel_ordem_servico_plano_acao", oSqlParameter)


            While oSqlDataReader.Read

                Dim oInfo As New MODELS.PlanoAcaoQA

                oInfo.unidade = oSqlDataReader("unidade")
                oInfo.categoria = oSqlDataReader("categoria")
                oInfo.setor = oSqlDataReader("setor")
                oInfo.equipamento = oSqlDataReader("equipamento")
                oInfo.departamento = oSqlDataReader("departamento")
                oInfo.local = oSqlDataReader("local")
                oInfo.solicitante = oSqlDataReader("solicitante")
                oInfo.executor = oSqlDataReader("executor")
                oInfo.data_execucao = oSqlDataReader("data_execucao")
                oInfo.numero_documento = oSqlDataReader("numero_documento")
                oInfo.data = Format(oSqlDataReader("data"), "dd/MM/yyyy HH:mm")
                oInfo.data_necessidade = Format(oSqlDataReader("data_necessidade"), "dd/MM/yyyy")
                oInfo.dias = Format(oSqlDataReader("dias"))
                oInfo.descricao = oSqlDataReader("descricao")
                oInfo.prioridade = oSqlDataReader("prioridade")
                oInfo.tipo_servico = oSqlDataReader("tipo_servico")
                oInfo.tipo_ordem_servico = oSqlDataReader("tipo_ordem_servico")
                oInfo.status_descricao = oSqlDataReader("status_descricao")
                oInfo.justificativa_apontamento = oSqlDataReader("justificativa_apontamento")
                oInfo.auditoria = oSqlDataReader("auditoria")

                oOrdemServico.Add(oInfo)

            End While

            'Fecha o oSqlDataReader
            If oSqlDataReader.IsClosed = False Then oSqlDataReader.Close() : oSqlDataReader = Nothing

            'Retorno da Função
            Return oOrdemServico

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "::: CHECKLIST :::"

    Public Function Checklist(ByVal iCodigoEmpresa As Integer,
                              ByVal iCodigoTipoChecklist As Integer,
                              ByVal sFile As String) As List(Of ChecklistItem)

        Try

            'Váriaveis Locais
            Dim oReturn As New List(Of ChecklistItem)
            Dim oSqlDataReader As SqlDataReader
            Dim oSqlParameter(2) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Tipo de Checklist
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_tipo_checklist"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoTipoChecklist : i += 1

            'Seta Parametros - File
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "file"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.VarChar
            oSqlParameter(i).Value = sFile

            'Executa Query

            oSqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, "sp_select_excel_checklist", oSqlParameter)

            While oSqlDataReader.Read

                Dim oInfo As New ChecklistItem

                oInfo.codigo = oSqlDataReader.Item("codigo")
                oInfo.checklist = oSqlDataReader.Item("checklist")
                oInfo.tipo_item_checklist = oSqlDataReader.Item("tipo_item_checklist")
                oInfo.codigo_tipo_item_checklist = oSqlDataReader.Item("codigo_tipo_item_checklist")
                oInfo.grupo = oSqlDataReader.Item("grupo")
                oInfo.peso_grupo = oSqlDataReader.Item("peso_grupo")
                oInfo.subgrupo = oSqlDataReader.Item("subgrupo")
                oInfo.peso_subgrupo = oSqlDataReader.Item("peso_subgrupo")
                oInfo.descricao = oSqlDataReader.Item("descricao")
                oInfo.allow_picture = oSqlDataReader.Item("allow_picture")
                oInfo.valor_minimo = oSqlDataReader.Item("valor_minimo")
                oInfo.valor_maximo = oSqlDataReader.Item("valor_maximo")
                oInfo.unidade_medida = oSqlDataReader.Item("unidade_medida")
                oInfo.tempo_estimado = oSqlDataReader.Item("tempo_estimado")
                oInfo.auditado = oSqlDataReader.Item("auditado")
                oInfo.codigo_periodicidade = oSqlDataReader.Item("codigo_periodicidade")
                oInfo.periodicidade = oSqlDataReader.Item("periodicidade")
                oInfo.intervalo = oSqlDataReader.Item("intervalo")
                oInfo.excluido = oSqlDataReader.Item("excluido")
                oInfo.peso = oSqlDataReader.Item("peso")
                oInfo.codigo_departamento = oSqlDataReader.Item("codigo_departamento")
                oInfo.ordem_servico = oSqlDataReader.Item("ordem_servico")

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

    Public Sub Checklist(ByVal iCodigoEmpresa As Integer,
                         ByVal lCodigoChecklist As Long,
                         ByVal iCodigoTipoChecklist As Integer,
                         ByVal sFile As String,
                         ByVal sPath As String,
                         ByVal sFileName As String)

        Try

            'Váriaveis Locais
            Dim oDataSet As DataSet
            Dim oSqlParameter(2) As SqlParameter
            Dim i As Integer = 0

            'Seta Parametros - Código Empresa
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_empresa"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.SmallInt
            oSqlParameter(i).Value = iCodigoEmpresa : i += 1

            'Seta Parametros - Código Tipo Checklist
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo_tipo_checklist"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.Int
            oSqlParameter(i).Value = iCodigoTipoChecklist : i += 1

            'Seta Parametros - Código
            oSqlParameter(i) = New SqlParameter
            oSqlParameter(i).ParameterName = "codigo"
            oSqlParameter(i).Direction = ParameterDirection.Input
            oSqlParameter(i).SqlDbType = SqlDbType.BigInt
            oSqlParameter(i).Value = lCodigoChecklist

            'Executa Query
            oDataSet = ExecuteDataset(sConnection, CommandType.StoredProcedure, "sp_select_excel_checklist_download", oSqlParameter)

            Using oExcelPackage As ExcelPackage = New ExcelPackage

                Using oFileStream As FileStream = New FileStream(sFile, FileMode.Open)

                    oExcelPackage.Load(oFileStream)

                    Dim oWorkSheet As ExcelWorksheet = oExcelPackage.Workbook.Worksheets("CHECKLIST")

                    oWorkSheet.Cells("A2").LoadFromDataTable(oDataSet.Tables(0), False)

                    Dim oWorkSheetPlan1 As ExcelWorksheet = oExcelPackage.Workbook.Worksheets("Planilha1")

                    oWorkSheetPlan1.Cells("G1").LoadFromDataTable(oDataSet.Tables(1), False)

                End Using

                Dim oFileInfo As New FileInfo(sFileName)

                oExcelPackage.SaveAs(oFileInfo)

            End Using

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region

#Region "::: EXCEL HELPER :::"

    Public Function LoadValidacaoExcel(ByVal codigoEmpresa As Integer,
                                       ByVal codigoUnidade As Integer,
                                       ByVal storedProcedure As String) As List(Of String)

        Dim oReturn As New List(Of String)

        Dim oSqlParameter As SqlParameter() = {
            CriarParametro("codigo_empresa", SqlDbType.SmallInt, codigoEmpresa),
            CriarParametro("codigo_unidade", SqlDbType.Int, codigoUnidade)
        }

        Using oSqlDataReader As SqlDataReader = ExecuteReader(sConnection, CommandType.StoredProcedure, storedProcedure, oSqlParameter)

            While oSqlDataReader.Read()

                oReturn.Add(oSqlDataReader.Item("descricao"))

            End While

        End Using

        Return oReturn

    End Function

#End Region

End Class
