using Microsoft.Win32;

namespace LibraryManagement.Utils
{
   public class DialogUtils
   {
      public static string ShowSaveFileDialog(string dialogTitle, string dialogFilter)
      {
         string filePath = "";

         SaveFileDialog dialog = new SaveFileDialog()
         {
            Title = dialogTitle,
            Filter = dialogFilter,
         };

         // Nếu mở file và chọn nơi lưu file thành công sẽ lưu đường dẫn lại dùng
         if (dialog.ShowDialog() == true)
         {
            filePath = dialog.FileName;
         }
         return filePath;
      }
   }
}