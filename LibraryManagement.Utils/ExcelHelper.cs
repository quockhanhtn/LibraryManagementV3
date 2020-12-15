using OfficeOpenXml;

namespace LibraryManagement.Utils
{
   public class ExcelHelper
   {
      public static ExcelPackage CreateExcelPackage(string author = null, string title = null, string[] workSheets = null)
      {
         ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // set license
         ExcelPackage excelPackage = new ExcelPackage();

         // set extra info
         excelPackage.Workbook.Properties.Author = author ?? "";
         excelPackage.Workbook.Properties.Title = title ?? "";

         if (workSheets != null)
         {
            for (int i = 0; i < workSheets.GetLength(0); i++)
            {
               excelPackage.Workbook.Worksheets.Add(workSheets[i]);
            }
         }

         return excelPackage;
      }

      public static void SaveExcelPackage(ExcelPackage excelPackage, string filePath)
      {
         byte[] bin = excelPackage.GetAsByteArray();
         System.IO.File.WriteAllBytes(filePath, bin);
      }

      /// <summary>
      /// Mở file Excel
      /// </summary>
      /// <param name="path">Đường dẫn đến file</param>
      public static void OpenFile(string path)
      {
         var excelApp = new Microsoft.Office.Interop.Excel.Application
         {
            Visible = true
         };
         Microsoft.Office.Interop.Excel.Workbooks workbooks = excelApp.Workbooks;
         Microsoft.Office.Interop.Excel.Workbook sheet = workbooks.Open(path);
      }
   }
}