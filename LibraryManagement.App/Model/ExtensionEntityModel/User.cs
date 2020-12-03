namespace LibraryManagement.Model
{
   public partial class User : BaseEntityModel
   {
      public string FullName { get => (this.LastName + " " + this.FirstName).Trim(); }
      public string GenderDisplay { get => Utils.FormatUtils.Gender(this.Gender); set => GenderDisplay = value; }

      public static string ConvertGender(string gender)
      {
         if (gender.Equals("Nam")) { return "M"; }
         else if (gender.Equals("Nữ")) { return "F"; }
         else { return "O"; }
      }
   }
}