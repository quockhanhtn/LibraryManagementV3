using LibraryManagement.CustomControl;
using LibraryManagement.Model;
using LibraryManagement.Utils;
using LibraryManagement.View;
using OfficeOpenXml;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LibraryManagement.ViewModel
{
   public class PageManagerBookCategoryViewModel : PageManagerBaseViewModel<BookCategory>
   {
      public ICommand CopyIdCommand { get; set; }
      public ICommand CopyNameCommand { get; set; }

      public bool IsShowHiddenCategory
      {
         get => isShowHiddenCategory;
         set
         {
            isShowHiddenCategory = value;
            //OnPropertyChanged(nameof(IsShowHiddenCategory));
            Thread thread = new Thread(() =>
            {
               Application.Current.Dispatcher.Invoke(new Action(delegate () { ReloadList(); }));
            })
            {
               IsBackground = true
            };
            thread.Start();
            //ReloadList();
         }
      }

      public PageManagerBookCategoryViewModel()
      {
         ReloadList();

         SearchCommand = new RelayCommand<TextBox>((p) => { return p != null; }, (p) =>
         {
            ListDTO = BookCategoryDAL.Instance.FindSimilar(p.Text, IsShowHiddenCategory ? EStatusFillter.AllStatus : EStatusFillter.Active);
         });

         ExportToExcelCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
         {
            string filePath = DialogUtils.ShowSaveFileDialog("Xuất danh chuyên mục sách", "Excel | *.xlsx | Excel 2003 | *.xls");
            if (string.IsNullOrEmpty(filePath)) { return; }

            try
            {
               using (var excelPackage = ExcelHelper.CreateExcelPackage("Library Manger", "Danh sách chuyên mục sách", new string[] { "List BookCategory Sheet" }))
               {
                  string[] columnHeaders = { "Mã chuyên mục", "Tên chuyên mục", "Số ngày cho mượn", "Số lượng sách", "Trạng thái" };
                  double[] columnWidths = new double[] { 30, 30, 30, 30, 30 };
                  ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[0];
                  StyleExcelWorkSheet(worksheet, "Thống kê chuyên mục sách", columnHeaders, columnWidths);

                  int colIndex = 1, rowIndex = 4;  // bắt đầu từ cột A, hàng 3 (do hàng 1 ghi tiêu đề, hàng 2 ghi ngày giờ, hàng 3 ghi header)
                  foreach (var item in ListDTO)
                  {
                     worksheet.WriteCell(rowIndex, colIndex++, item.BookCategoryId);
                     worksheet.WriteCell(rowIndex, colIndex++, item.BookCategoryName);
                     worksheet.WriteCell(rowIndex, colIndex++, item.LimitDays);
                     worksheet.WriteCell(rowIndex, colIndex++, item.NumberOfBook);
                     worksheet.WriteCell(rowIndex, colIndex++, item.StatusDisplay);

                     colIndex = 1;
                     rowIndex++;
                  }
                  ExcelHelper.SaveExcelPackage(excelPackage, filePath);
               }

               ShowSnackbarMessage("Xuất file Excel thành công !", "Mở", () => ExcelHelper.OpenFile(filePath), 10);
            }
            catch (Exception e)
            {
               CustomMessageBox.Show($"Có lỗi khi lưu file!\r\n{e.Message}", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
         });

         AddCommand = new RelayCommand<object>((p) => true, (p) =>
         {
            if (EditBookCategoryWindow.Show())
            {
               ShowSnackbarMessage("Thêm chuyên mục thành công !");
               ReloadList();
            }
            else { ShowSnackbarMessage("Không có thay đổi"); }
         });

         UpdateCommand = new RelayCommand<object>((p) => DTOSelected != null, (p) =>
         {
            if (EditBookCategoryWindow.Show(DTOSelected))
            {
               ShowSnackbarMessage("Cập nhật chuyên mục thành công !");
               ReloadList();
            }
            else { ShowSnackbarMessage("Không có thay đổi"); }
         });

         StatusOnCommand = new RelayCommand<object>((p) => DTOSelected != null && DTOSelected.BookCategoryStatus != true, (p) => ChangeStatus());

         StatusOffCommand = new RelayCommand<object>((p) => DTOSelected != null && DTOSelected.BookCategoryStatus == true, (p) => ChangeStatus());

         DeleteCommand = new RelayCommand<object>((p) => DTOSelected != null && DTOSelected.NumberOfBook == 0, (p) =>
         {
            var deleteSucceed = BookCategoryDAL.Instance.Delete(DTOSelected.BookCategoryId);

            if (deleteSucceed)
            {
               ReloadList();
               ShowSnackbarMessage("Xóa chuyên mục thành công !");
            }
            else
            {
               ReloadList();
               ShowSnackbarMessage("Không có thay đổi !");
            }
         });

         CopyIdCommand = new RelayCommand<object>((p) => { return DTOSelected != null; }, (p) =>
         {
            Clipboard.SetText(DTOSelected.BookCategoryId.ToString());
            ShowSnackbarMessage("Copy 'Mã chuyên mục' thành công !");
         });

         CopyNameCommand = new RelayCommand<object>((p) => { return DTOSelected != null; }, (p) =>
         {
            Clipboard.SetText(DTOSelected.BookCategoryName);
            ShowSnackbarMessage("Copy 'Tên chuyên mục' thành công !");
         });
      }

      private void ChangeStatus()
      {
         if (BookCategoryDAL.Instance.ChangeStatus(DTOSelected.BookCategoryId))
         {
            ShowSnackbarMessage("Cập nhật thành công !");
            ReloadList();
         }
         else { ShowSnackbarMessage("Cập nhật thất bại"); }
      }

      private void ReloadList()
      {
         if (IsShowHiddenCategory) { ListDTO = BookCategoryDAL.Instance.Gets(); }
         else { ListDTO = BookCategoryDAL.Instance.Gets(EStatusFillter.Active); }
      }

      private bool isShowHiddenCategory = false;
   }
}