namespace LibraryManagement
{
   public static class Definition
   {
      /// <summary>
      /// Constain for table USER
      /// </summary>
      public static class User
      {
         public static int MinAge = 8;

         public static class Gender
         {
            public static string Male = "M";
            public static string Female = "F";
            public static string Orther = "O";
         }

         public static class Type
         {
            public static string Admin = "ADMIN";
            public static string Librarian = "LIBRARIAN";
            public static string Member = "MEMBER";
            public static string UnVerified = "UN_VERIFIED";
         }
      }
   }
}