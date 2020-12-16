using LibraryManagement.CustomControl;
using LibraryManagement.Model;
using LibraryManagement.Utils;
using LibraryManagement.View.EditWindow;
using OfficeOpenXml;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LibraryManagement.ViewModel
{
   public class PageManagerAuthorViewModel : PageManagerBaseViewModel<Author>
   {
      public ICommand CopyIdCommand { get; set; }
      public ICommand CopyNameCommand { get; set; }

      public bool IsShowHiddenAuthor
      {
         get => isShowHiddenAuthor;
         set
         {
            isShowHiddenAuthor = value;
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

      public PageManagerAuthorViewModel()
      {
         ReloadList();

         SearchCommand = new RelayCommand<TextBox>((p) => { return p != null; }, (p) =>
         {
            ListDTO = AuthorDAL.Instance.FindSimilar(p.Text, IsShowHiddenAuthor ? EStatusFillter.AllStatus : EStatusFillter.Active);
         });

         ExportToExcelCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
         {
            string filePath = DialogUtils.ShowSaveFileDialog("Xuất danh chuyên mục sách", "Excel|*.xlsx|Excel 2003|*.xls");
            if (string.IsNullOrEmpty(filePath)) { return; }

            try
            {
               using (var excelPackage = ExcelHelper.CreateExcelPackage("Library Mangement", "Danh sách tác giả", new string[] { "List Author Sheet" }))
               {
                  string[] columnHeaders = { "Mã chuyên mục", "Tên tác giả", "Số lượng sách", "Trạng thái" };
                  double[] columnWidths = new double[] { 15, 35, 13, 10 };
                  ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[0];
                  StyleExcelWorkSheet(worksheet, "Thống kê tác giả", columnHeaders, columnWidths);

                  int colIndex = 1, rowIndex = 4;  // bắt đầu từ cột A, hàng 3 (do hàng 1 ghi tiêu đề, hàng 2 ghi ngày giờ, hàng 3 ghi header)
                  foreach (var item in ListDTO)
                  {
                     worksheet.WriteCell(rowIndex, colIndex++, item.AuthorId);
                     worksheet.WriteCell(rowIndex, colIndex++, item.NickName);
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
            if (EditAuthorWindow.Show())
            {
               ShowSnackbarMessage("Thêm chuyên mục thành công !");
               ReloadList();
            }
            else { ShowSnackbarMessage("Không có thay đổi"); }
         });

         UpdateCommand = new RelayCommand<object>((p) => DTOSelected != null, (p) =>
         {
            if (EditAuthorWindow.Show(DTOSelected))
            {
               ShowSnackbarMessage("Cập nhật tác giả thành công !");
               ReloadList();
            }
            else { ShowSnackbarMessage("Không có thay đổi"); }
         });

         StatusOnCommand = new RelayCommand<object>((p) => DTOSelected != null && DTOSelected.AuthorStatus != true, (p) => ChangeStatus());

         StatusOffCommand = new RelayCommand<object>((p) => DTOSelected != null && DTOSelected.AuthorStatus == true, (p) => ChangeStatus());

         DeleteCommand = new RelayCommand<object>((p) => DTOSelected != null && DTOSelected.NumberOfBook == 0, (p) =>
         {
            if (AuthorDAL.Instance.Delete(DTOSelected.AuthorId))
            {
               ReloadList();
               ShowSnackbarMessage("Xóa tác giả thành công !");
            }
            else
            {
               ReloadList();
               ShowSnackbarMessage("Không có thay đổi !");
            }
         });

         CopyIdCommand = new RelayCommand<object>((p) => { return DTOSelected != null; }, (p) =>
         {
            Clipboard.SetText(DTOSelected.AuthorId.ToString());
            ShowSnackbarMessage("Copy 'Mã tác giả' thành công !");
         });

         CopyNameCommand = new RelayCommand<object>((p) => { return DTOSelected != null; }, (p) =>
         {
            Clipboard.SetText(DTOSelected.NickName);
            ShowSnackbarMessage("Copy 'Tên tác giả' thành công !");
         });
      }

      private void ChangeStatus()
      {
         if (AuthorDAL.Instance.ChangeStatus(DTOSelected.AuthorId))
         {
            ShowSnackbarMessage("Cập nhật thành công !");
            ReloadList();
         }
         else { ShowSnackbarMessage("Cập nhật thất bại"); }
      }

      private void ReloadList()
      {
         if (IsShowHiddenAuthor) { ListDTO = AuthorDAL.Instance.Gets(); }
         else { ListDTO = AuthorDAL.Instance.Gets(EStatusFillter.Active); }
      }

      private bool isShowHiddenAuthor = false;
   }
}