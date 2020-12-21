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
   public class PageManagerPublisherViewModel : PageManagerBaseViewModel<Publisher>
      {
         public ICommand CopyIdCommand { get; set; }
         public ICommand CopyNameCommand { get; set; }

         public bool IsShowHiddenPublisher
      {
            get => isShowHiddenPublisher;
            set
            {
               isShowHiddenPublisher = value;
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

         public PageManagerPublisherViewModel()
         {
            ReloadList();

            SearchCommand = new RelayCommand<TextBox>((p) => { return p != null; }, (p) =>
            {
               ListDTO = PublisherDAL.Instance.FindSimilar(p.Text, IsShowHiddenPublisher ? EStatusFillter.AllStatus : EStatusFillter.Active);
            });

            ExportToExcelCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
               string filePath = DialogUtils.ShowSaveFileDialog("Xuất danh nhà xuất bản sách", "Excel | *.xlsx | Excel 2003 | *.xls");
               if (string.IsNullOrEmpty(filePath)) { return; }

               try
               {
                  using (var excelPackage = ExcelHelper.CreateExcelPackage("Library Manger", "Danh sách nhà xuất bản sách", new string[] { "List Publisher Sheet" }))
                  {
                     string[] columnHeaders = { "Mã NXB", "Tên nhà xuất bản", "Số điện thoại", "Địa chỉ", "Email", "Trang web", "Số lượng đầu sách", "Trạng thái" };
                     double[] columnWidths = new double[] { 13, 30, 20, 50, 30, 32, 10, 10 };
                     ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[0];
                     StyleExcelWorkSheet(worksheet, "Thống kê nhà xuất bản sách", columnHeaders, columnWidths);

                     int colIndex = 1, rowIndex = 4;  // bắt đầu từ cột A, hàng 3 (do hàng 1 ghi tiêu đề, hàng 2 ghi ngày giờ, hàng 3 ghi header)
                     foreach (var item in ListDTO)
                     {
                        worksheet.WriteCell(rowIndex, colIndex++, item.PublisherId);
                        worksheet.WriteCell(rowIndex, colIndex++, item.PublisherName);
                        worksheet.WriteCell(rowIndex, colIndex++, item.PhoneNumber);
                        worksheet.WriteCell(rowIndex, colIndex++, item.Address);
                        worksheet.WriteCell(rowIndex, colIndex++, item.Email);
                        worksheet.WriteCell(rowIndex, colIndex++, item.Website);
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
               if (EditPublisherWindow.Show())
               {
                  ShowSnackbarMessage("Thêm nhà xuất bản thành công !");
                  ReloadList();
               }
               else { ShowSnackbarMessage("Không có thay đổi"); }
            });

            UpdateCommand = new RelayCommand<object>((p) => DTOSelected != null, (p) =>
            {
               if (EditPublisherWindow.Show(DTOSelected))
               {
                  ShowSnackbarMessage("Cập nhật nhà xuất bản thành công !");
                  ReloadList();
               }
               else { ShowSnackbarMessage("Không có thay đổi"); }
            });

            StatusOnCommand = new RelayCommand<object>((p) => DTOSelected != null && DTOSelected.PublisherStatus != true, (p) => ChangeStatus());

            StatusOffCommand = new RelayCommand<object>((p) => DTOSelected != null && DTOSelected.PublisherStatus == true, (p) => ChangeStatus());

            DeleteCommand = new RelayCommand<object>((p) => DTOSelected != null && DTOSelected.NumberOfBook == 0, (p) =>
            {
               var deleteSucceed = PublisherDAL.Instance.Delete(DTOSelected.PublisherId);

               if (deleteSucceed)
               {
                  ReloadList();
                  ShowSnackbarMessage("Xóa nhà xuất bản thành công !");
               }
               else
               {
                  ReloadList();
                  ShowSnackbarMessage("Không có thay đổi !");
               }
            });

            CopyIdCommand = new RelayCommand<object>((p) => { return DTOSelected != null; }, (p) =>
            {
               Clipboard.SetText(DTOSelected.PublisherId.ToString());
               ShowSnackbarMessage("Copy 'Mã nhà xuất bản' thành công !");
            });

            CopyNameCommand = new RelayCommand<object>((p) => { return DTOSelected != null; }, (p) =>
            {
               Clipboard.SetText(DTOSelected.PublisherName);
               ShowSnackbarMessage("Copy 'Tên nhà xuất bản' thành công !");
            });
         }

         private void ChangeStatus()
         {
            if (PublisherDAL.Instance.ChangeStatus(DTOSelected.PublisherId))
            {
               ShowSnackbarMessage("Cập nhật thành công !");
               ReloadList();
            }
            else { ShowSnackbarMessage("Cập nhật thất bại"); }
         }

         private void ReloadList()
         {
            if (IsShowHiddenPublisher) { ListDTO = PublisherDAL.Instance.Gets(); }
            else { ListDTO = PublisherDAL.Instance.Gets(EStatusFillter.Active); }
         }

         private bool isShowHiddenPublisher = false;
      }
   
}