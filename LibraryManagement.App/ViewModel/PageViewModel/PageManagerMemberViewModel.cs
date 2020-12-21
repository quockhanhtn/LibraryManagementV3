using LibraryManagement.CustomControl;
using LibraryManagement.Model;
using LibraryManagement.Utils;
using LibraryManagement.View;
using OfficeOpenXml;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LibraryManagement.ViewModel
{
   public class PageManagerMemberViewModel : PageManagerBaseViewModel<Member>
   {
      public ICommand SendEmailCommand { get; set; }

      public ICommand CopyIdCommand { get; set; }
      public ICommand CopyNameCommand { get; set; }
      public ICommand CopyPhoneNumberCommand { get; set; }
      public ICommand CopyAddressCommand { get; set; }

      public int StatusFillter { get => (int)statusFillter; set { statusFillter = (EStatusFillter)value; ReloadList(); OnPropertyChanged(); } }

      public PageManagerMemberViewModel()
      {
         ReloadList();

         SearchCommand = new RelayCommand<TextBox>((p) => p != null, (p) => ListDTO = MemberDAL.Instance.FindSimilar(p.Text, (EStatusFillter)StatusFillter));

         ExportToExcelCommand = new RelayCommand<object>((p) => true, (p) =>
         {
            string filePath = DialogUtils.ShowSaveFileDialog("Xuất danh sách độc giả trong thư viện", "Excel | *.xlsx | Excel 2003 | *.xls");
            if (string.IsNullOrEmpty(filePath)) { return; }

            try
            {
               using (var excelPackage = ExcelHelper.CreateExcelPackage("Library Mangement", "Danh sách độc giả", new string[] { "List Member Sheet" }))
               {
                  string[] columnHeaders = { "Mã số", "Họ và tên đệm", "Tên", "Giới tính", "Ngày sinh", "CCCD", "Địa chỉ", "Điện thoại", "Ngày đăng ký", "Ngày hết hạn", "Trạng thái" };
                  double[] columnWidths = new double[] { 6, 20, 8, 7, 10, 15, 42, 12, 10, 10, 10 };
                  ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[0];
                  StyleExcelWorkSheet(worksheet, "Thống kê danh sách Độc giả", columnHeaders, columnWidths);

                  int colIndex = 1, rowIndex = 4;  // bắt đầu từ cột A, hàng 3 (do hàng 1 ghi tiêu đề, hàng 2 ghi ngày giờ, hàng 3 ghi header)
                  foreach (var item in ListDTO)
                  {
                     worksheet.WriteCell(rowIndex, colIndex++, item.UserId);
                     worksheet.WriteCell(rowIndex, colIndex++, item.User.LastName);
                     worksheet.WriteCell(rowIndex, colIndex++, item.User.FirstName);
                     worksheet.WriteCell(rowIndex, colIndex++, item.User.GenderDisplay);
                     worksheet.WriteCell(rowIndex, colIndex++, item.User.DateOfBirth.ToString("dd/MM/yyyy"));
                     worksheet.WriteCell(rowIndex, colIndex++, item.User.Ssn);
                     worksheet.WriteCell(rowIndex, colIndex++, item.User.Address);
                     worksheet.WriteCell(rowIndex, colIndex++, item.User.PhoneNumber);
                     worksheet.WriteCell(rowIndex, colIndex++, item.RegisterDate.ToString("dd/MM/yyyy"));
                     worksheet.WriteCell(rowIndex, colIndex++, item.ExpDate.ToString("dd/MM/yyyy"));
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

         AddCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
         {
            if (EditMemberWindow.Show())
            {
               ShowSnackbarMessage("Thêm độc giả thành công !");
               ReloadList();
            }
            else { ShowSnackbarMessage("Không có thay đổi"); }
         });

         UpdateCommand = new RelayCommand<object>((p) => DTOSelected != null && DTOSelected.User != null, (p) =>
         {
            if (EditMemberWindow.Show(DTOSelected))
            {
               ShowSnackbarMessage("Chỉnh sửa thông tin độc giả thành công !");
               ReloadList();
            }
            else { ShowSnackbarMessage("Không có thay đổi"); }
         });

         SendEmailCommand = new RelayCommand<object>((p) => DTOSelected != null && DTOSelected.User.Email != null, (p) => { WebHelper.SendEmail(DTOSelected.User.Email); });

         StatusOnCommand = new RelayCommand<object>((p) => DTOSelected != null && DTOSelected.User.UserStatus != true, (p) => ChangeStatus());

         StatusOffCommand = new RelayCommand<object>((p) => DTOSelected != null && DTOSelected.User.UserStatus == true, (p) => ChangeStatus());

         CopyIdCommand = new RelayCommand<object>((p) => DTOSelected != null, (p) => { Clipboard.SetText(DTOSelected.UserId.ToString()); });

         CopyNameCommand = new RelayCommand<object>((p) => DTOSelected != null, (p) => { Clipboard.SetText(DTOSelected.User.FullName); });

         CopyPhoneNumberCommand = new RelayCommand<object>((p) => DTOSelected != null, (p) => { Clipboard.SetText(DTOSelected.User.PhoneNumber); });

         CopyAddressCommand = new RelayCommand<object>((p) => DTOSelected != null, (p) => { Clipboard.SetText(DTOSelected.User.Address); });
      }

      private void ReloadList() => ListDTO = MemberDAL.Instance.Gets(statusFillter);

      private void ChangeStatus()
      {
         if (MemberDAL.Instance.ChangeStatus(DTOSelected.UserId))
         {
            ShowSnackbarMessage("Cập nhật thành công !");
            ReloadList();
         }
         else { ShowSnackbarMessage("Cập nhật thất bại"); }
      }

      #region Feild
      private EStatusFillter statusFillter = EStatusFillter.Active;
      #endregion Feild
   }
}