namespace LibraryManagement.Utils
{
   public class FormatUtils
   {
      public static string Gender(string gender)
      {
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