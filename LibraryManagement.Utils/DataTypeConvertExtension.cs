using System;

namespace LibraryManagement.Utils
{
   public static class DataTypeConvertExtension
   {
      public static double ToDouble(this object obj)
      {
         double result;
         try
         {
            result = Convert.ToDouble(obj);
         }
         catch (Exception) { result = 0; }

         return result;
      }

      public static decimal ToDecimal(this object obj)
      {
         decimal result;
         try
         {
            result = Convert.ToDecimal(obj);
         }
         catch (Exception) { result = 0; }

         return result;
      }
   }
}