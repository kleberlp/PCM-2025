using NPOI.SS.Formula.Functions;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using PCM.WEB.DAL;
using PCM.WEB.MODELS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;

public static class ExcelHelper
{

    public static byte[] GerarExcelChecklist(int codigoEmpresa, 
                                             int codigoUnidade, 
                                             List<InterfaceExcelColumn> estrutura,
                                             DataSet dataSet = null)
    {

        ExcelPackage.License.SetNonCommercialOrganization("<ACTI>");

        using (var package = new ExcelPackage())
        {
            var ws = package.Workbook.Worksheets.Add("Checklist");

            int headerRow = 1;
            int lastColumn = estrutura.Max(x => x.Coluna);

            // Cabeçalho
            foreach (var col in estrutura.Where(x => x.Visivel))
            {
                ws.Cells[headerRow, col.Coluna].Value = col.ColunaExcel;

                AplicarEstiloCabecalho(ws, headerRow, col.Coluna);

                ws.Column(col.Coluna).Width =
                    string.IsNullOrEmpty(col.Largura)
                        ? 20
                        : Convert.ToDouble(col.Largura);

                ws.Cells[headerRow, col.Coluna].Style.Locked = true;
            }

            // Liberar edição apenas linhas 2+ 
            ws.Cells[2, 1, 2000, lastColumn].Style.Locked = false;

            // Filtro
            ws.Cells[1, 1, 1, lastColumn].AutoFilter = true;

            // Congelar primeira linha
            ws.View.FreezePanes(2, 1);

            // Validações
            foreach (var col in estrutura.Where(x => x.Visivel))
            {
                if (col.Obrigatorio && string.IsNullOrEmpty(col.TipoValidacao))
                    AplicarValidacaoObrigatorio(ws, col);

                if (!string.IsNullOrEmpty(col.TipoValidacao))
                    AplicarValidacaoLista(package, ws, col, codigoEmpresa, codigoUnidade);
            }

            if (dataSet != null)
            {
                ws.Cells[2, 1].LoadFromDataTable(dataSet.Tables[0], false);
            }

            // Proteção com Senha
            ws.Protection.SetPassword("P@ssw0rd013459");
            ws.Protection.AllowSelectLockedCells = false;
            ws.Protection.AllowSelectUnlockedCells = true;
            ws.Protection.AllowAutoFilter = true;
            ws.Protection.AllowSort = true;
            ws.Protection.AllowFormatColumns = true;
            ws.Protection.AllowFormatRows = true;

            ws.Protection.IsProtected = true;

            return package.GetAsByteArray();
        }
    }

    private static void AplicarEstiloCabecalho(ExcelWorksheet ws, 
                                               int row, 
                                               int col)
    {
        var cell = ws.Cells[row, col];

        cell.Style.Font.Bold = true;
        cell.Style.Font.Color.SetColor(System.Drawing.Color.White);

        cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
        cell.Style.Fill.BackgroundColor.SetColor(
            System.Drawing.Color.FromArgb(0, 70, 140)); // Azul corporativo

        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        ws.Row(row).Height = 25;
    }

    private static void AplicarValidacaoObrigatorio(ExcelWorksheet ws, 
                                                    InterfaceExcelColumn col)
    {
        var validation = ws.DataValidations.AddCustomValidation(
            ws.Cells[2, col.Coluna, 2000, col.Coluna].Address
        );

        validation.Formula.ExcelFormula = $"LEN(TRIM({GetColumnLetter(col.Coluna)}2))>0";

        validation.ShowErrorMessage = true;
        validation.ErrorTitle = "Campo obrigatório";
        validation.Error = "Este campo deve ser preenchido.";
    }

    private static string GetColumnLetter(int colNumber)
    {
        int dividend = colNumber;
        string columnName = String.Empty;
        int modulo;

        while (dividend > 0)
        {
            modulo = (dividend - 1) % 26;
            columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
            dividend = (int)((dividend - modulo) / 26);
        }

        return columnName;
    }

    private static void AplicarValidacaoLista(ExcelPackage package,
                                          ExcelWorksheet ws,
                                          InterfaceExcelColumn col,
                                          int codigoEmpresa,
                                          int codigoUnidade)
    {
        if (col.TipoValidacao == "LISTA_FIXA")
        {
            var validation = ws.DataValidations.AddListValidation(
                ws.Cells[2, col.Coluna, 2000, col.Coluna].Address
            );

            AplicarListaFixa(validation, col);
        }
        else if (col.TipoValidacao == "LISTA_BANCO")
        {
            Excel excel = new Excel(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

            var valores = excel.LoadValidacaoExcel(
                codigoEmpresa,
                codigoUnidade,
                col.FonteLista
            );

            AplicarListaBanco(package, ws, col, valores);
        }
    }

    private static void AplicarListaFixa(OfficeOpenXml.DataValidation.Contracts.IExcelDataValidationList validation,
                                         InterfaceExcelColumn col)
    {
        if (col.FonteLista == "SIM_NAO")
        {
            validation.Formula.Values.Add("SIM");
            validation.Formula.Values.Add("NÃO");
        }

        validation.ShowErrorMessage = true;
        validation.ErrorTitle = "Valor inválido";
        validation.Error = "Selecione um valor da lista.";
    }

    private static void AplicarListaBanco(ExcelPackage package,
                                          ExcelWorksheet ws,
                                          InterfaceExcelColumn col,
                                          List<string> valores)
    {
        var wsList = package.Workbook.Worksheets["_listas"] ?? package.Workbook.Worksheets.Add("_listas");

        int startRow = wsList.Dimension?.End.Row + 1 ?? 1;

        for (int i = 0; i < valores.Count; i++)
        {
            wsList.Cells[startRow + i, 1].Value = valores[i];
        }

        var range = wsList.Cells[startRow, 1, startRow + valores.Count - 1, 1];

        string namedRange = "Lista_" + col.DataMember;

        package.Workbook.Names.Add(namedRange, range);

        var validation = ws.DataValidations.AddListValidation(
            ws.Cells[2, col.Coluna, 2000, col.Coluna].Address
        );

        validation.Formula.ExcelFormula = namedRange;

        wsList.Hidden = eWorkSheetHidden.VeryHidden;
    }


}
