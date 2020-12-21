using LibraryManagement.CustomControl;
using LibraryManagement.Model;
using System.Windows;

namespace LibraryManagement.ViewModel
{
   public class EditPublisherWindowViewModel : EditWindowBaseViewModel<Publisher>
   {
      public string ErrorPublisherName { get => errorPublisherName; set { errorPublisherName = value; OnPropertyChanged(); } }
      public string ErrorPhoneNumber { get => errorPhoneNumber; set { errorPhoneNumber = value; OnPropertyChanged(); } }
      public string ErrorAddress { get => errorAddress; set { errorAddress = value; OnPropertyChanged(); } }
      public string ErrorEmail { get => errorEmail; set { errorEmail = value; OnPropertyChanged(); } }
      public string ErrorWebsite { get => errorWebsite; set { errorWebsite = value; OnPropertyChanged(); } }

      public EditPublisherWindowViewModel(Publisher editPublisher = null) : base(editPublisher)
      {
      }

      private bool CheckInput()
      {
         bool missingInput = false;

         EditObject.PublisherName = EditObject.PublisherName.Trim();

         if (!string.IsNullOrEmpty(EditObject.PublisherName)) { ErrorPublisherName = ""; }
         else { ErrorPublisherName = "Tên nhà xuất bản không được để trống"; missingInput = true; }

         if (Mode == EditMode.Add)
         {
            if (!PublisherDAL.Instance.CheckNameExits(EditObject.PublisherName)) { ErrorPublisherName = ""; }
            else { ErrorPublisherName = "Nhà xuất bản đã tồn tại"; missingInput = true; }
         }

         return !missingInput;
      }

      protected override void Load(Publisher editObject)
      {
         if (editObject != null)
         {
            Mode = EditMode.Update;
            EditObject = editObject;
            EditTitleText = "CHỈNH SỬA NHÀ XUẤT BẢN";
         }
         else
         {
            Mode = EditMode.Add;
            EditObject = new Publisher()
            {
               PublisherStatus = true
            };
            EditTitleText = "THÊM NHÀ XUẤT BẢN MỚI";
         }
         OnPropertyChanged();
      }

      protected override void SaveChange(Window p)
      {
         if (CheckInput() == false) { return; }

         if (Mode == EditMode.Add)
         {
            if (PublisherDAL.Instance.Add(EditObject) != 0)
            {
               EditResult = true;
               p.Close();
            }
            else { CustomMessageBox.Show("Thêm nhà xuất bản thất bại", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error); }
         }
         else
         {
            if (PublisherDAL.Instance.Update(EditObject))
            {
               EditResult = true;
               p.Close();
            }
            else { CustomMessageBox.Show("Cập nhật thông tin thất bại", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error); }
         }
      }

      string errorPublisherName;
      string errorPhoneNumber;
      string errorAddress;
      string errorEmail;
      string errorWebsite;
   }
}