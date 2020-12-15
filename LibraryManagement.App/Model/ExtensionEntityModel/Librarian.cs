using LibraryManagement.Utils;

namespace LibraryManagement.Model
{
   public partial class Librarian
   {
      public string StatusDisplay { get => User.UserStatus ? "Đang làm" : "Đã nghỉ"; }

      public string SalaryStr { get => Salary.ToString(); set => Salary = value.RemoveNonDigit().ToDecimal(); }
   }
}