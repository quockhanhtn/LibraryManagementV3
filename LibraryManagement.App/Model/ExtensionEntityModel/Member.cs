namespace LibraryManagement.Model
{
   public partial class Member
   {
      public string StatusDisplay { get => User.UserStatus ? "Còn sử dụng" : "Bị khóa"; }
   }
}