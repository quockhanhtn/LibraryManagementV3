using LibraryManagement.CustomControl;
using LibraryManagement.Model;
using LibraryManagement.Utils;
using LibraryManagement.View;
using OfficeOpenXml;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LibraryManagement.ViewModel
{
   public class PageManagerBookViewModel : PageManagerBaseViewModel<BookInfo>
   {
      public ICommand CopyIdCommand { get; set; }
      public ICommand CopyNameCommand { get; set; }

      public int StatusFillter { get => (int)statusFillter; set { statusFillter = (EStatusFillter)value; ReloadList(); OnPropertyChanged(); } }
      public Publisher PublisherSelected { get => publisherSelected; set { publisherSelected = value; ReloadList(); OnPropertyChanged(); } }
      public ObservableCollection<Publisher> ListPublisher { get => listPublisher; set { listPublisher = value; ReloadList(); OnPropertyChanged(); } }
      public BookCategory BookCategorySelected { get => bookCategorySelected; set { bookCategorySelected = value; ReloadList(); OnPropertyChanged(); } }
      public ObservableCollection<BookCategory> ListBookCategory { get => listBookCategory; set { listBookCategory = value; ReloadList(); OnPropertyChanged(); } }

      public PageManagerBookViewModel()
      {
         bookCategorySelected = new BookCategory() { BookCategoryId = 0, BookCategoryName = "Tất cả chuyên mục" };
         listBookCategory = new ObservableCollection<BookCategory>() { bookCategorySelected };
         listBookCategory.AddRange(BookCategoryDAL.Instance.Gets());

         publisherSelected = new Publisher() { PublisherId = 0, PublisherName = "Tất cả nhà xuất bản" };
         listPublisher = new ObservableCollection<Publisher>() { publisherSelected };
         listPublisher.AddRange(PublisherDAL.Instance.Gets());

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
            }

            var searchKeyWord = p.Text.TrimCheck().RemoveUnicode().ToLower();

            ReloadList();
            ListDTO = ListDTO.Where(x => x.Title.Like(searchKeyWord) || x.AllAuthor.Like(searchKeyWord)).ToObservableCollection();
         });

         ExportToExcelCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
         {
            string filePath = DialogUtils.ShowSaveFileDialog("Xuất danh Đầu sách", "Excel | *.xlsx | Excel 2003 | *.xls");
            if (string.IsNullOrEmpty(filePath)) { return; }

            try
            {
               using (var excelPackage = ExcelHelper.CreateExcelPackage("Library Manger", "Danh sách đầu sách sách", new string[] { "List BookInfo Sheet" }))
               {
                  string[] columnHeaders = { "Mã đầu sách", "Tên đầu sách", "Số ngày cho mượn", "Số lượng sách", "Trạng thái" };
                  double[] columnWidths = new double[] { 30, 30, 30, 30, 30 };
                  ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[0];
                  StyleExcelWorkSheet(worksheet, "Thống kê đầu sách", columnHeaders, columnWidths);

                  int colIndex = 1, rowIndex = 4;  // bắt đầu từ cột A, hàng 3 (do hàng 1 ghi tiêu đề, hàng 2 ghi ngày giờ, hàng 3 ghi header)
                  foreach (var item in ListDTO)
                  {
                     /* worksheet.WriteCell(rowIndex, colIndex++, item.BookInfoId);
                      worksheet.WriteCell(rowIndex, colIndex++, item.BookInfoName);
                      worksheet.WriteCell(rowIndex, colIndex++, item.LimitDays);
                      worksheet.WriteCell(rowIndex, colIndex++, item.NumberOfBook);
                      worksheet.WriteCell(rowIndex, colIndex++, item.StatusDisplay);*/

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
            if (EditBookWindow.Show())
            {
               ShowSnackbarMessage("Thêm đầu sách thành công !");
               ReloadList();
            }
            else { ShowSnackbarMessage("Không có thay đổi"); }
         });

         UpdateCommand = new RelayCommand<object>((p) => DTOSelected != null, (p) =>
         {
            if (EditBookWindow.Show(DTOSelected))
            {
               ShowSnackbarMessage("Cập nhật đầu sách thành công !");
               ReloadList();
            }
            else { ShowSnackbarMessage("Không có thay đổi"); }
         });

         StatusOnCommand = new RelayCommand<object>((p) => DTOSelected != null && DTOSelected.BookInfoStatus != true, (p) => ChangeStatus());

         StatusOffCommand = new RelayCommand<object>((p) => DTOSelected != null && DTOSelected.BookInfoStatus == true, (p) => ChangeStatus());

         DeleteCommand = new RelayCommand<object>((p) => DTOSelected != null && DTOSelected.Count == 0, (p) =>
         {
            var deleteSucceed = BookInfoDAL.Instance.Delete(DTOSelected.BookInfoId);

            if (deleteSucceed)
            {
               ReloadList();
               ShowSnackbarMessage("Xóa đầu sách thành công !");
            }
            else
            {
               ReloadList();
               ShowSnackbarMessage("Không có thay đổi !");
            }
         });

         CopyIdCommand = new RelayCommand<object>((p) => { return DTOSelected != null; }, (p) =>
         {
            Clipboard.SetText(DTOSelected.BookInfoId.ToString());
            ShowSnackbarMessage("Copy 'Mã đầu sách' thành công !");
         });

         CopyNameCommand = new RelayCommand<object>((p) => { return DTOSelected != null; }, (p) =>
         {
            Clipboard.SetText(DTOSelected.Title);
            ShowSnackbarMessage("Copy 'Tên đầu sách' thành công !");
         });
      }

      private void ChangeStatus()
      {
         if (BookInfoDAL.Instance.ChangeStatus(DTOSelected.BookInfoId))
         {
            ShowSnackbarMessage("Cập nhật thành công !");
            ReloadList();
         }
         else { ShowSnackbarMessage("Cập nhật thất bại"); }
      }

      private void ReloadList()
      {
         ListDTO = BookInfoDAL.Instance.Gets(statusFillter);

         if (BookCategorySelected.BookCategoryId != 0)
         {
            ListDTO = ListDTO.Where(b => b.BookCategoryId == BookCategorySelected.BookCategoryId).ToObservableCollection();
         }

         if (PublisherSelected.PublisherId != 0)
         {
            ListDTO = ListDTO.Where(b => b.PublisherId == PublisherSelected.PublisherId).ToObservableCollection();
         }
      }

      #region Feild

      private EStatusFillter statusFillter = EStatusFillter.Active;
      private Publisher publisherSelected;
      private ObservableCollection<Publisher> listPublisher = new ObservableCollection<Publisher>();
      private BookCategory bookCategorySelected;
      private ObservableCollection<BookCategory> listBookCategory = new ObservableCollection<BookCategory>();

      #endregion Feild
   }
}