using LibraryManagement.CustomControl;
using LibraryManagement.Model;
using LibraryManagement.Utils;
using OfficeOpenXml;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LibraryManagement.ViewModel
{
   public class PageManagerLibrarianViewModel : BasePageManagerViewModel<Librarian>
   {
      public ICommand ObjectSelectedChangedCommand { get; set; }
      public ICommand SendEmailCommand { get; set; }

      public ICommand CopyIdCommand { get; set; }
      public ICommand CopyNameCommand { get; set; }
      public ICommand CopyPhoneNumberCommand { get; set; }
      public ICommand CopyAddressCommand { get; set; }

      public int StatusFillter { get => (int)statusFillter; set { statusFillter = (EStatusFillter)value; ReloadList(); OnPropertyChanged(); } }

      #region Input property
      public string InputLastName { get => inputLastName; set { inputLastName = value; OnPropertyChanged(); } }
      public string InputFirstName { get => inputFirstName; set { inputFirstName = value; OnPropertyChanged(); } }
      public string InputGender { get => inputGender; set { inputGender = value; OnPropertyChanged(); } }
      public DateTime InputDateOfBirth { get => inputDateOfBirth; set { inputDateOfBirth = value; OnPropertyChanged(); } }
      public string InputSSN { get => inputSSN; set { inputSSN = value; OnPropertyChanged(); } }
      public string InputAddress { get => inputAddress; set { inputAddress = value; OnPropertyChanged(); } }
      public string InputPhoneNumber { get => inputPhoneNumber; set { inputPhoneNumber = value; OnPropertyChanged(); } }
      public string InputEmail { get => inputEmail; set { inputEmail = value; OnPropertyChanged(); } }
      public DateTime InputStartDate { get => inputStartDate; set { inputStartDate = value; OnPropertyChanged(); } }
      public string InputSlary { get => inputSlary; set { inputSlary = value; OnPropertyChanged(); } }
      #endregion

      #region Error text
      public string ErrorLastName { get => errorLastName; set { errorLastName = value; OnPropertyChanged(); } }
      public string ErrorFirstName { get => errorFirstName; set { errorFirstName = value; OnPropertyChanged(); } }
      public string ErrorGender { get => errorGender; set { errorGender = value; OnPropertyChanged(); } }
      public string ErrorDateOfBirth { get => errorDateOfBirth; set { errorDateOfBirth = value; OnPropertyChanged(); } }
      public string ErrorSSN { get => errorSSN; set { errorSSN = value; OnPropertyChanged(); } }
      public string ErrorAddress { get => errorAddress; set { errorAddress = value; OnPropertyChanged(); } }
      public string ErrorPhoneNumber { get => errorPhoneNumber; set { errorPhoneNumber = value; OnPropertyChanged(); } }
      public string ErrorEmail { get => errorEmail; set { errorEmail = value; OnPropertyChanged(); } }
      public string ErrorStartDate { get => errorStartDate; set { errorStartDate = value; OnPropertyChanged(); } }
      public string ErrorSlary { get => errorSlary; set { errorSlary = value; OnPropertyChanged(); } }

      #endregion

      public PageManagerLibrarianViewModel()
      {
         ReloadList();

         SearchCommand = new RelayCommand<TextBox>((p) => { return p != null; }, (p) =>
         {
            ReloadList();
            if (p.Text == "" || p.Text == " ")
            {
               p.Text = "";
               
            }
            else
            {
               // Tìm kiếm theo email
               if (p.Text.Contains("@"))
               {
                  ListDTO = ListDTO.Where(x=> x.User.Email.ToLower().Contains(p.Text.ToLower())).ToObservableCollection();
               }
               // Tìm kiếm theo số điện thoại
               else if (p.Text[0] >= '0' && p.Text[0] <= '9')
               {
                  ListDTO = ListDTO.Where(x => x.User.PhoneNumber.Contains(p.Text)).ToObservableCollection();
               }
               // Tìm theo họ tên
               else
               {
                  ListDTO = ListDTO.Where(x => x.User.FullName.Like(p.Text)).ToObservableCollection();
               }
            }
         });

         ObjectSelectedChangedCommand = new RelayCommand<UserControl>((p) => { return p != null && DTOSelected != null; }, (p) =>
         {
            var btnStatusChange = p.FindName("btnStatusChange") as Button;
            var mnuStatusChange = p.FindName("mnuStatusChange") as MenuItem;
            //var tblStatusChange = p.FindName("tblStatusChange") as TextBlock;
            //var icoStatusChange = p.FindName("icoStatusChange") as PackIcon;
            if (DTOSelected.User.UserStatus == true)
            {
               //tblStatusChange.Text = "THÔI VIỆC";
               //icoStatusChange.Kind = PackIconKind.BlockHelper;
               btnStatusChange.Content = "THÔI VIỆC";
               //btnStatusChange.ToolTip = "Nhân viên " + DTOSelected.FullName + " nghỉ việc";
               mnuStatusChange.Header = "Thôi việc";
            }
            else
            {
               //tblStatusChange.Text = "ĐI LÀM LẠI";
               //icoStatusChange.Kind = PackIconKind.Restore;
               btnStatusChange.Content = "ĐI LÀM LẠI";
               //btnStatusChange.ToolTip = "Nhân viên " + DTOSelected.FullName + " làm viêc lại";
               mnuStatusChange.Header = "Đi làm lại";
            }
         });

         ExportToExcelCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
         {
            string filePath = DialogUtils.ShowSaveFileDialog("Xuất danh sách thủ thư trong thư viện", "Excel | *.xlsx | Excel 2003 | *.xls");
            if (string.IsNullOrEmpty(filePath)) { return; }

            try
            {
               using (var excelPackage = ExcelHelper.CreateExcelPackage("Library Mangement", "Danh sách thủ thư thư viện", new string[] { "List Librarian Sheet" }))
               {
                  string[] columnHeaders = { "Mã số", "Họ và tên đệm", "Tên", "Giới tính", "Ngày sinh", "CCCD", "Địa chỉ", "Điện thoại", "Ngày bắt đầu", "Lương" };
                  double[] columnWidths = new double[] { 30, 30, 30, 30, 30 };
                  ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[0];
                  StyleExcelWorkSheet(worksheet, "Thống kê chuyên mục sách", columnHeaders, columnWidths);

                  int colIndex = 1, rowIndex = 4;  // bắt đầu từ cột A, hàng 3 (do hàng 1 ghi tiêu đề, hàng 2 ghi ngày giờ, hàng 3 ghi header)
                  foreach (var item in ListDTO)
                  {
                     worksheet.WriteCell(rowIndex, colIndex++, item.UserId);
                     worksheet.WriteCell(rowIndex, colIndex++, item.User.FirstName);
                     worksheet.WriteCell(rowIndex, colIndex++, item.User.LastName);
                     worksheet.WriteCell(rowIndex, colIndex++, item.User.GenderDisplay);
                     worksheet.WriteCell(rowIndex, colIndex++, item.User.DateOfBirth?.ToString("dd/MM/yyyy"));
                     worksheet.WriteCell(rowIndex, colIndex++, item.User.Ssn);
                     worksheet.WriteCell(rowIndex, colIndex++, item.User.Address);
                     worksheet.WriteCell(rowIndex, colIndex++, item.User.PhoneNumber);
                     worksheet.WriteCell(rowIndex, colIndex++, item.StartDate.ToString("dd/MM/yyyy"));
                     worksheet.WriteCell(rowIndex, colIndex++, item.Salary);

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
            //var addDataContext = new AddLibrarianWindowVM();
            //var addLibrarianWindow = new AddLibrarianWindow() { DataContext = addDataContext };
            //addLibrarianWindow.ShowDialog();

            //var mySnackbar = p.FindName("mySnackbar") as Snackbar;
            //if (addDataContext.Result != null)
            //{
            //   mySnackbar.MessageQueue.Enqueue("Thêm nhân viên \"" + addDataContext.Result.FullName + "\" thành công");
            //   ReloadList();
            //}
            //else { mySnackbar.MessageQueue.Enqueue("Không có thay đổi"); }
         });

         UpdateCommand = new RelayCommand<UserControl>((p) => { return p != null && DTOSelected != null && DTOSelected.User != null; }, (p) =>
         {
            bool missingInput = false;

            #region Validate input
            if (ValidationUtils.IsName(InputLastName)) { ErrorLastName = ""; }
            else { ErrorLastName = "Họ không hợp lệ"; missingInput = true; }

            if (ValidationUtils.IsName(InputFirstName)) { ErrorFirstName = ""; }
            else { ErrorFirstName = "Tên không hợp lệ"; missingInput = true; }

            if (!string.IsNullOrEmpty(InputGender)) { ErrorGender = ""; }
            else { ErrorGender = "Vui lòng chọn giới tính"; missingInput = true; }

            if (ValidationUtils.IsDateOfBirth(InputDateOfBirth, Definition.User.MinAge)) { ErrorDateOfBirth = ""; }
            else { ErrorDateOfBirth = "Chọn ngày sinh không hợp lệ"; missingInput = true; }

            if (ValidationUtils.IsSsn(InputSSN)) { ErrorSSN = ""; }
            else { ErrorSSN = "Nhập căn cước công dân có 9 hoặc 12 chữ số"; missingInput = true; }

            #endregion

            if (missingInput)
            {
               return;
            }
            else
            {
               DTOSelected.User.LastName = InputLastName;
               DTOSelected.User.FirstName = InputFirstName;
               DTOSelected.User.Gender = User.ConvertGender(InputGender);
               DTOSelected.User.DateOfBirth = InputDateOfBirth;
               DTOSelected.User.Ssn = InputSSN.RemoveMutilSpace();

               DTOSelected.User.Address = InputAddress.RemoveMutilSpace();
               DTOSelected.User.PhoneNumber = InputPhoneNumber.RemoveMutilSpace();
               DTOSelected.User.Email = InputEmail;

               DTOSelected.StartDate = InputStartDate;
               DTOSelected.Salary = Convert.ToDecimal(InputSlary);

               if (LibrarianDAL.Instance.Update(DTOSelected))
               {
                  ShowSnackbarMessage("Cập nhật thông tin thủ thư thành công !");
               }
               else
               {
                  ShowSnackbarMessage("Cập nhật thông tin thủ thư thất bại");
               }
               ReloadList();
            }

            //DTOSelected.LastName = StringHelper.CapitalizeEachWord(txtLastName.Text);
            //DTOSelected.FirstName = StringHelper.CapitalizeEachWord(txtFirstName.Text);
            //DTOSelected.Sex = cmbSex.SelectedValue.ToString();
            //DTOSelected.Birthday = dtpkBirthday.SelectedDate;
            //DTOSelected.SSN = txtSSN.Text;
            //DTOSelected.Address = txtAddress.Text;
            //DTOSelected.Email = txtEmail.Text;
            //DTOSelected.PhoneNumber = txtPhone.Text;
            //DTOSelected.Salary = StringHelper.ToDecimal(txtSalary.Text);
            //DTOSelected.StartDate = dtpkStartDate.SelectedDate;

            //LibrarianDAL.Instance.Update(DTOSelected);
            //var mySnackbar = p.FindName("mySnackbar") as Snackbar;
            //mySnackbar.MessageQueue.Enqueue("Cập nhật thông tin nhân viên \"" + DTOSelected.FullName + "\" thành công");
            //ReloadList();
         });

         SendEmailCommand = new RelayCommand<object>((p) => { return DTOSelected != null && DTOSelected.User.Email != null; }, (p) => { WebHelper.SendEmail(DTOSelected.User.Email); });

         StatusChangeCommand = new RelayCommand<object>((p) => { return DTOSelected != null; }, (p) =>
         {
            LibrarianDAL.Instance.ChangeStatus(DTOSelected.UserId);
            ReloadList();
         });

         CopyIdCommand = new RelayCommand<object>((p) => { return DTOSelected != null; }, (p) => { Clipboard.SetText(DTOSelected.UserId.ToString()); });

         CopyNameCommand = new RelayCommand<object>((p) => { return DTOSelected != null; }, (p) => { Clipboard.SetText(DTOSelected.User.FullName); });

         CopyPhoneNumberCommand = new RelayCommand<object>((p) => { return DTOSelected != null; }, (p) => { Clipboard.SetText(DTOSelected.User.PhoneNumber); });

         CopyAddressCommand = new RelayCommand<object>((p) => { return DTOSelected != null; }, (p) => { Clipboard.SetText(DTOSelected.User.Address); });
      }

      protected override void ObjectSelectedChange()
      {
         InputLastName = DTOSelected?.User?.LastName ?? "";
         InputFirstName = DTOSelected?.User?.FirstName ?? "";
         InputGender = DTOSelected?.User?.GenderDisplay ?? "Khác";
         InputDateOfBirth = DTOSelected?.User?.DateOfBirth ?? DateTime.Now;
         InputSSN = DTOSelected?.User?.Ssn ?? "";
         InputAddress = DTOSelected?.User?.Address ?? "";
         InputPhoneNumber = DTOSelected?.User?.PhoneNumber ?? "";
         InputEmail = DTOSelected?.User?.Email ?? "";
         InputStartDate = DTOSelected?.StartDate ?? DateTime.Now;
         InputSlary = DTOSelected?.Salary?.ToString() ?? "";
      }

      void ReloadList()
      {
         ListDTO = LibrarianDAL.Instance.Gets(statusFillter);
      }

      #region Feild
      EStatusFillter statusFillter = EStatusFillter.Active;

      string inputLastName = "";
      string inputFirstName = "";
      string inputGender = "";
      DateTime inputDateOfBirth;
      string inputSSN = "";
      string inputAddress = "";
      string inputPhoneNumber = "";
      string inputEmail = "";
      DateTime inputStartDate;
      string inputSlary = "";

      string errorLastName;
      string errorFirstName;
      string errorGender;
      string errorDateOfBirth;
      string errorSSN;
      string errorAddress;
      string errorPhoneNumber;
      string errorEmail;
      string errorStartDate;
      string errorSlary;
      #endregion
   }
}