using LibraryManagement.Utils;
using System.Windows;
using System.Windows.Media;

namespace LibraryManagement.Model
{
   public partial class User
   {
      public string FullName { get => (this.LastName + " " + this.FirstName).TrimCheck(); }
      public string GenderDisplay { get => Definition.User.GenderToString(this.Gender); set => Gender = Definition.User.StringToGender(value); }
      public ImageSource ImageDisplay { get => Base64Utils.Decode.ToImageSource(this.Image) ?? Application.Current.TryFindResource("DefaultUserImage") as ImageSource; }
   }
}