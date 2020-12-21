using LibraryManagement.CustomControl;
using LibraryManagement.Model;
using System.Windows;

namespace LibraryManagement.ViewModel
{
   public class EditBookWindowViewModel: EditWindowBaseViewModel<BookInfo>
   {
      public EditBookWindowViewModel(BookInfo editBookInfo = null) : base(editBookInfo)
      {
      }

      private bool CheckInput()
      {
         bool missingInput = false;

         /*if (!string.IsNullOrEmpty(EditObject.BookCategoryName)) { ErrorBookCategoryName = ""; }
         else { ErrorBookCategoryName = "Tên chuyên mục không được để trống"; missingInput = true; }

         if (EditObject.LimitDays != 0) { ErrorLimitDays = ""; }
         else { ErrorLimitDays = "Nhập số ngày cho mượn"; missingInput = true; }*/

         return !missingInput;
      }

      protected override void Load(BookInfo editObject)
      {
         if (editObject != null)
         {
            Mode = EditMode.Update;
            EditObject = editObject;
            EditTitleText = "CHỈNH SỬA CHUYÊN MỤC";
         }
         else
         {
            Mode = EditMode.Add;
            EditObject = new BookInfo()
            {
               BookInfoStatus = true
            };
            EditTitleText = "THÊM CHUYÊN MỤC MỚI";
         }
         OnPropertyChanged();
      }

      protected override void SaveChange(Window p)
      {
         if (CheckInput() == false) { return; }

         if (Mode == EditMode.Add)
         {
            if (BookInfoDAL.Instance.Add(EditObject) != 0)
            {
               EditResult = true;
               p.Close();
            }
            else { CustomMessageBox.Show("Thêm chuyên mục thất bại", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error); }
         }
         else
         {
            if (BookInfoDAL.Instance.Update(EditObject))
            {
               EditResult = true;
               p.Close();
            }
            else { CustomMessageBox.Show("Cập nhật thông tin thất bại", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error); }
         }
      }
   }
}