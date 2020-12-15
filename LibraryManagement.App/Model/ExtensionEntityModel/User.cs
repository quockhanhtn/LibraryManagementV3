using LibraryManagement.Utils;
using System.Windows;
using System.Windows.Media;

namespace LibraryManagement.Model
{
   public partial class User
   {
      public string FullName { get => (this.LastName + " " + this.FirstName).Trim(); }
      public string GenderDisplay
      {
         get => FormatUtils.Gender(this.Gender);
         set => Gender = ConvertGender(value);
      }

      public ImageSource ImageDisplay { get => Base64Utils.Decode.ToImageSource(this.Image) ?? Application.Current.TryFindResource("DefaultUserImage") as ImageSource; }

      public static string ConvertGender(string gender)
      {
         if (gender.Equals("Nam")) { return "M"; }
         else if (gender.Equals("Nữ")) { return "F"; }
         else { return "O"; }
      }
   }
}