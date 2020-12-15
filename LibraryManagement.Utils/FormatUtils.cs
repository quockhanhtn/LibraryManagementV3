namespace LibraryManagement.Utils
{
   public class FormatUtils
   {
      public static string Gender(string gender)
      {
         if (string.IsNullOrEmpty(gender)) { return "Khác"; }

         if (gender.ToLower().StartsWith("f"))
         {
            return "Nữ";
         }
         else if (gender.ToLower().StartsWith("m"))
         {
            return "Nam";
         }
         else { return "Khác"; }
      }
   }
}