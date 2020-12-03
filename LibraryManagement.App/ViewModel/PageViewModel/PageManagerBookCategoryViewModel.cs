using LibraryManagement.CustomControl;
using LibraryManagement.Model;
using LibraryManagement.Utils;
using MaterialDesignThemes.Wpf;
using OfficeOpenXml;
using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LibraryManagement.ViewModel
{
   public class PageManagerBookCategoryViewModel : BasePageManagerViewModel<BookCategory>
   {
      public ICommand ObjectSelectedChangedCommand { get; set; }
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
            });
            thread.IsBackground = true;
            thread.Start();
            //ReloadList();
         }
      }
      public PageManagerBookCategoryViewModel()
      {
         ReloadList();

         SearchCommand = new RelayCommand<TextBox>((p) => { return p != null; }, (p) =>
         {
            if (p.Text == "" || p.Text == " ")
            {
               p.Text = "";
               ReloadList();
            }
            else
            {
               ListDTO = BookCategoryDAL.Instance.Gets().Where(x => x.BookCategoryName.Like(p.Text)).ToObservableCollection();
            }
         });

         ObjectSelectedChangedCommand = new RelayCommand<UserControl>((p) => { return p != null && DTOSelected != null; }, (p) =>
         {
            var btnStatusChange = p.FindName("btnStatusChange") as Button;
            var mnuStatusChange = p.FindName("mnuStatusChange") as MenuItem;
            if (DTOSelected.BookCategoryStatus == true)
            {
               btnStatusChange.Content = "ẨN";
               btnStatusChange.ToolTip = "Ẩn chuyên mục \"" + DTOSelected.BookCategoryName + "\"";
               mnuStatusChange.Header = "Ẩn chuyên mục";
            }
            else
            {
               btnStatusChange.Content = "HIỂN THỊ";
               btnStatusChange.ToolTip = "Hiển thị chuyên mục \"" + DTOSelected.BookCategoryName + "\"";
               mnuStatusChange.Header = "Hiển thị chuyên mục";
            }
         });

         ExportToExcelCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
         {
            string filePath = DialogUtils.ShowSaveFileDialog("Xuất danh chuyên mục sách", "Excel | *.xlsx | Excel 2003 | *.xls");
            if (string.IsNullOrEmpty(filePath)) { return; }

            try
            {
               using (var excelPackage = ExcelHelper.CreateExcelPackage("Library Manger", "Danh sách chuyên mục sách", new string[] { "List BookCategory Sheet" }))
               {
                  ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[0];
                  worksheet.SetColumWidth(new double[] { 30, 30, 30, 30, 30 });

                  string[] columnHeaders = { "Mã chuyên mục", "Tên chuyên mục", "Số ngày cho mượn", "Số lượng sách", "Ghi chú" };

                  worksheet.SetTitleAndDateTime("Thống kê chuyên mục sách", DateTime.Now, columnHeaders.Length);
                  worksheet.SetHeader(columnHeaders);

                  int colIndex = 1, rowIndex = 4;  // bắt đầu từ cột A, hàng 3 (do hàng 1 ghi tiêu đề, hàng 2 ghi ngày giờ, hàng 3 ghi header)
                  foreach (var item in ListDTO)
                  {
                     worksheet.WriteCell(rowIndex, colIndex++, item.BookCategoryId);
                     worksheet.WriteCell(rowIndex, colIndex++, item.BookCategoryName);
                     worksheet.WriteCell(rowIndex, colIndex++, item.LimitDays);
                     worksheet.WriteCell(rowIndex, colIndex++, item.NumberOfBook);
                     worksheet.WriteCell(rowIndex, colIndex++, item.Note);

                     colIndex = 1;
                     rowIndex++;
                  }
                  ExcelHelper.SaveExcelPackage(excelPackage, filePath);
               }

               var messageBoxResult = CustomMessageBox.Show("Bạn có muốn mở file excel vừa xuất không ?", "Xuất file Excel thành công !", MessageBoxButton.YesNo, MessageBoxImage.Information, MessageBoxResult.Yes);
               if (messageBoxResult == MessageBoxResult.Yes)
               {
                  ExcelHelper.OpenFile(filePath);
               }
            }
            catch (Exception e)
            {
               CustomMessageBox.Show($"Có lỗi khi lưu file!\r\n{e.Message}", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
         });

         AddCommand = new RelayCommand<UserControl>((p) => { return p != null; }, (p) =>
         {
            //var addDataContext = new AddBookCategoryWindowVM();
            //var addBookCategoryWindow = new AddBookCategoryWindow() { DataContext = addDataContext };
            //addBookCategoryWindow.ShowDialog();

            //var mySnackbar = p.FindName("mySnackbar") as Snackbar;
            //if (addDataContext.Result != null)
            //{
            //   mySnackbar.MessageQueue.Enqueue("Thêm chuyên mục \"" + addDataContext.Result.Name + "\" thành công");
            //   ReloadList();
            //}
            //else { mySnackbar.MessageQueue.Enqueue("Không có thay đổi"); }
         });

         UpdateCommand = new RelayCommand<UserControl>((p) => { return p != null && DTOSelected != null; }, (p) =>
         {
            //var txtName = p.FindName("txtName") as TextBox;
            //var txtLimitDays = p.FindName("txtLimitDays") as TextBox;
            //var tblNameWarning = p.FindName("tblNameWarning") as TextBlock;
            //var tblLimitDaysWarning = p.FindName("tblLimitDaysWarning") as TextBlock;

            //if (txtName.Text == "")
            //{
            //   tblNameWarning.Visibility = Visibility.Visible;
            //   txtName.Focus();
            //   return;
            //}
            //else { tblNameWarning.Visibility = Visibility.Hidden; }

            //if (StringHelper.ToInt(txtLimitDays.Text) == 0)
            //{
            //   tblLimitDaysWarning.Visibility = Visibility.Visible;
            //   txtLimitDays.Focus();
            //   return;
            //}
            //else { tblLimitDaysWarning.Visibility = Visibility.Hidden; }

            //DTOSelected.Name = txtName.Text;
            //DTOSelected.LimitDays = StringHelper.ToInt(txtLimitDays.Text);

            //BookCategoryDAL.Instance.Update(DTOSelected);
            //var mySnackbar = p.FindName("mySnackbar") as Snackbar;
            //mySnackbar.MessageQueue.Enqueue("Cập nhật chuyên mục \"" + DTOSelected.Name + "\" thành công");
            //ReloadList();
         });

         StatusChangeCommand = new RelayCommand<object>((p) => { return DTOSelected != null; }, (p) =>
         {
            BookCategoryDAL.Instance.ChangeStatus(DTOSelected.BookCategoryId);
            ReloadList();
         });

         DeleteCommand = new RelayCommand<object>((p) => { return DTOSelected != null && DTOSelected.NumberOfBook == 0; }, (p) =>
         {
            var deleteSucceed = BookCategoryDAL.Instance.Delete(DTOSelected.BookCategoryId);

            if (deleteSucceed)
            {
            }
            else
            {
            }

            ReloadList();
            ShowSnackbarMessage("Xóa chuyên mục thành công !");
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

      private void ReloadList()
      {
         if (IsShowHiddenCategory) { ListDTO = BookCategoryDAL.Instance.Gets(); }
         else { ListDTO = BookCategoryDAL.Instance.Gets(EStatusFillter.Active); }
      }

      private bool isShowHiddenCategory = false;
   }
}