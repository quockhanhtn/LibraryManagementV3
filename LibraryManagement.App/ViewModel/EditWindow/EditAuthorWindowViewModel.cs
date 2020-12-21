using LibraryManagement.CustomControl;
using LibraryManagement.Model;
using System.Windows;

namespace LibraryManagement.ViewModel
{
   public class EditAuthorWindowViewModel : EditWindowBaseViewModel<Author>
   {
      public string ErrorNickName { get => errorNickName; set { errorNickName = value; OnPropertyChanged(); } }
      public string ErrorRealName { get => errorRealName; set { errorRealName = value; OnPropertyChanged(); } }

      public EditAuthorWindowViewModel(Author authorEdit) : base(authorEdit)
      {
      }

      private bool CheckInput()
      {
         bool missingInput = false;

         if (!string.IsNullOrEmpty(EditObject.NickName)) { ErrorNickName = ""; }
         else { ErrorNickName = "Tên tác giả không được để trống"; missingInput = true; }

         //if (EditObject.LimitDays != 0) { ErrorLimitDays = ""; }
         //else { ErrorLimitDays = "Nhập số ngày cho mượn"; missingInput = true; }

         return !missingInput;
      }

      protected override void Load(Author editObject)
      {
         if (editObject != null)
         {
            Mode = EditMode.Update;
            EditObject = editObject;
            EditTitleText = "CHỈNH SỬA TÁC GIẢ";
         }
         else
         {
            Mode = EditMode.Add;
            EditObject = new Author() { AuthorStatus = true };
            EditTitleText = "THÊM TÁC GIẢ MỚI";
         }
         OnPropertyChanged();
      }

      protected override void SaveChange(Window p)
      {
         if (CheckInput() == false) { return; }

         if (Mode == EditMode.Add)
         {
            if (AuthorDAL.Instance.Add(EditObject) != 0)
            {
               EditResult = true;
               p.Close();
            }
            else { CustomMessageBox.Show("Thêm tác giả thất bại", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error); }
         }
         else
         {
            if (AuthorDAL.Instance.Update(EditObject))
            {
               EditResult = true;
               p.Close();
            }
            else { CustomMessageBox.Show("Cập nhật thông tin thất bại", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error); }
         }
      }

      string errorNickName;
      string errorRealName;
   }
}