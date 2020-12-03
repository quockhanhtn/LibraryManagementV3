using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;

namespace LibraryManagement.Utils
{
   public static class ExcelWorksheetExtension
   {
      public static void SetTitleAndDateTime(this ExcelWorksheet excelWorksheet, string title, DateTime dateTime, int noOfColumns)
      {
         excelWorksheet.Cells[1, 1].Value = title.ToUpper();
         excelWorksheet.MergeAndCenter(1, 1, 1, noOfColumns, true);

         excelWorksheet.Cells[2, 1].Value = "Ngày " + dateTime.ToString("dd/MM/yyyy");
         excelWorksheet.MergeAndCenter(2, 1, 2, noOfColumns, true);
      }

      /// <summary>
      /// Tạo boder xung quanh một Cell
      /// </summary>
      /// <param name="excelWorksheet">Sheet cần tạo boder</param>
      /// <param name="rowIndex">Chỉ số hàng</param>
      /// <param name="colIndex">Chỉ số cột</param>
      /// <param name="excelBorderStyle">Kiểu đường viền</param>
      public static void FormatCellBorder(this ExcelWorksheet excelWorksheet, int rowIndex, int colIndex, ExcelBorderStyle excelBorderStyle = ExcelBorderStyle.Thin)
      {
         var cell = excelWorksheet.Cells[rowIndex, colIndex];

         //căn chỉnh các border
         var border = cell.Style.Border;
         border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = excelBorderStyle;
      }

      public static void WriteCell(this ExcelWorksheet excelWorksheet, int rowIndex, int colIndex, object value, ExcelBorderStyle excelBorderStyle = ExcelBorderStyle.Thin)
      {
         excelWorksheet.Cells[rowIndex, colIndex].Value = value;
         excelWorksheet.FormatCellBorder(rowIndex, colIndex);
      }

      public static void SetHeader(this ExcelWorksheet excelWorksheet, string[] columnHeaders, int rowIndex = 3, int colIndex = 1)
      {
         //tạo các header từ column header đã tạo từ bên trên
         foreach (var header in columnHeaders)
         {
            var cell = excelWorksheet.Cells[rowIndex, colIndex];

            //set màu thành gray
            var fill = cell.Style.Fill;
            fill.PatternType = ExcelFillStyle.Solid;
            fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);

            //căn chỉnh các border
            var border = cell.Style.Border;
            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            //gán giá trị
            cell.Value = header;

            colIndex++;
         }
      }

      /// <summary>
      /// Gộp các ô lại thành một và căn giữa
      /// </summary>
      /// <param name="excelWorksheet">Sheet chứa các cell cần gộp</param>
      /// <param name="fromRow">Hàng bắt đầu</param>
      /// <param name="formCol">Cột bắt đầu</param>
      /// <param name="toRow">Hàng kết thúc</param>
      /// <param name="toCol">Cột kết trúc</param>
      /// <param name="fontBold">In đâm hay không</param>
      /// <param name="fontItalic">In nghiêng hay không</param>
      public static void MergeAndCenter(this ExcelWorksheet excelWorksheet, int fromRow, int formCol, int toRow, int toCol, bool fontBold = false, bool fontItalic = false)
      {
         excelWorksheet.Cells[fromRow, formCol, toRow, toCol].Merge = true;
         excelWorksheet.Cells[fromRow, formCol, toRow, toCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
         excelWorksheet.Cells[fromRow, formCol, toRow, toCol].Style.Font.Bold = fontBold;
         excelWorksheet.Cells[fromRow, formCol, toRow, toCol].Style.Font.Italic = fontItalic;
      }

      /// <summary>
      /// Set Column width cho sheet
      /// </summary>
      /// <param name="excelWorksheet">Sheet cần gán</param>
      /// <param name="columnWidth">List colum width, bắt đầu từ column 1</param>
      public static void SetColumWidth(this ExcelWorksheet excelWorksheet, double[] columnWidth)
      {
         for (int i = 0; i < columnWidth.Length; i++)
         {
            excelWorksheet.Column(i + 1).Width = columnWidth[i];
         }
      }
   }
}