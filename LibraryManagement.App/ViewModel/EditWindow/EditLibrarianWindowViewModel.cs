﻿using LibraryManagement.CustomControl;
using LibraryManagement.Model;
using LibraryManagement.Utils;
using System;
using System.Windows;

namespace LibraryManagement.ViewModel
{
   public class EditLibrarianWindowViewModel : EditWindowBaseViewModel<Librarian>
   {
      public string DisplayId { get => EditObject.UserId > 0 ? EditObject.UserId.ToString() : "Auto generate"; }

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
      public string ErrorSalary { get => errorSalary; set { errorSalary = value; OnPropertyChanged(); } }

      #endregion Error text

      public EditLibrarianWindowViewModel(Librarian editLibrarian = null) : base(editLibrarian)
      {
         //RetypeCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
         //{
         //   if (Mode == EditMode.Add) { Load(null); }
         //   else { EditObject = LibrarianDAL.Instance.GetById(EditObject.UserId); }
         //});
      }

      bool CheckInput()
      {
         bool missingInput = false;

         if (ValidationUtils.IsName(EditObject.User.LastName)) { ErrorLastName = ""; }
         else { ErrorLastName = "Họ không hợp lệ"; missingInput = true; }

         if (ValidationUtils.IsName(EditObject.User.FirstName)) { ErrorFirstName = ""; }
         else { ErrorFirstName = "Tên không hợp lệ"; missingInput = true; }

         if (!string.IsNullOrEmpty(EditObject.User.GenderDisplay)) { ErrorGender = ""; }
         else { ErrorGender = "Vui lòng chọn giới tính"; missingInput = true; }

         if (ValidationUtils.IsDateOfBirth((DateTime)EditObject.User.DateOfBirth, Definition.User.MinAge)) { ErrorDateOfBirth = ""; }
         else { ErrorDateOfBirth = "Chọn ngày sinh không hợp lệ"; missingInput = true; }

         if (ValidationUtils.IsSsn(EditObject.User.Ssn))
         {
            if (UserDAL.Instance.CheckSsn(EditObject.UserId, EditObject.User.Ssn)) { ErrorSSN = ""; }
            else { errorSSN = "Căn cước công dân đã có người sử dụng"; missingInput = true; }
         }
         else { ErrorSSN = "Nhập căn cước công dân có 9 hoặc 12 chữ số"; missingInput = true; }

         if (ValidationUtils.IsPhoneNumber(EditObject.User.PhoneNumber))
         {
            if (UserDAL.Instance.CheckPhonenumber(EditObject.UserId, EditObject.User.PhoneNumber)) { ErrorPhoneNumber = ""; }
            else { ErrorPhoneNumber = "Số điện thoại đã có người sử dụng"; missingInput = true; }
         }
         else { ErrorPhoneNumber = "Nhập số điện thoại trống hoặc sai định dạng"; missingInput = true; }

         if (string.IsNullOrEmpty(EditObject.User.Email) || ValidationUtils.IsEmail(EditObject.User.Email))
         {
            if (UserDAL.Instance.CheckEmail(EditObject.UserId, EditObject.User.Email)) { ErrorEmail = ""; }
            else { ErrorEmail = "Email đã có người sử dụng"; missingInput = true; }
         }
         else { ErrorEmail = "Email sai định dạng"; missingInput = true; }

         return !missingInput;
      }

      protected override void Load(Librarian editObject)
      {
         if (editObject != null)
         {
            Mode = EditMode.Update;
            EditObject = editObject;
            EditTitleText = "CHỈNH SỬA THÔNG TIN THỦ THƯ";
         }
         else
         {
            Mode = EditMode.Add;
            EditObject = new Librarian()
            {
               User = new User()
               {
                  UserType = Definition.User.Type.Librarian,
                  DateOfBirth = DateTime.Today.AddDays(-365 * 18),
               },
               StartDate = DateTime.Today
            };
            EditTitleText = "THÊM THỦ THƯ MỚI";
         }
         OnPropertyChanged();
      }

      protected override void SaveChange(Window p)
      {
         if (CheckInput() == false) { return; }

         if (Mode == EditMode.Add)
         {
            EditObject.User.Username = EditObject.User.PhoneNumber;
            EditObject.User.Password = EditObject.User.PhoneNumber.Base64Encode().ToMD5Hash();

            if (LibrarianDAL.Instance.Add(EditObject) != 0)
            {
               EditResult = true;
               p.Close();
            }
            else { CustomMessageBox.Show("Thêm thủ thư thất bại", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error); }
         }
         else
         {
            if (LibrarianDAL.Instance.Update(EditObject))
            {
               EditResult = true;
               p.Close();
            }
            else { CustomMessageBox.Show("Cập nhật thông tin thất bại", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error); }
         }
      }

      #region field
      private string errorLastName;
      private string errorFirstName;
      private string errorGender;
      private string errorDateOfBirth;
      private string errorSSN;
      private string errorAddress;
      private string errorPhoneNumber;
      private string errorEmail;
      private string errorStartDate;
      private string errorSalary;
      #endregion
   }
}