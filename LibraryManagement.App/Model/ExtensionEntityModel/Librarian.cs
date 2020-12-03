namespace LibraryManagement.Model
{
   public partial class Librarian : BaseEntityModel
   {
      public string DisplayStatus { get => User.UserStatus ? "Đang làm" : "Đã nghỉ"; }
   }
}