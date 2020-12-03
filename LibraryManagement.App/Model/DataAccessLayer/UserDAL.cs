using LibraryManagement.Utils;
using System.Linq;

namespace LibraryManagement.Model
{
   public class UserDAL
   {
      /// <summary>
      ///
      /// </summary>
      /// <param name="username"></param>
      /// <param name="password"></param>
      /// <returns>
      /// <b>null</b> if login Failed <br/>
      /// <b>User object</b> if login succesed
      /// </returns>
      public static User Login(string userInput, string passwordPlantext)
      {
         userInput = userInput.TrimCheck();
         passwordPlantext = passwordPlantext.TrimCheck();

         if (userInput == "" || passwordPlantext == "") { return null; }
         string passwordEncode = passwordPlantext.Base64Encode().ToMD5Hash();

         if (ValidationUtils.IsEmail(userInput))
         {
            return LoginWithEmail(userInput, passwordEncode);
         }
         else if (ValidationUtils.IsPhoneNumber(userInput))
         {
            return LoginWithPhoneNumber(userInput, passwordEncode);
         }
         else
         {
            return LoginWithUsername(userInput, passwordEncode);
         }
      }

      private static User LoginWithEmail(string email, string passwordEncode)
      {
         return EFProvider.Instance.DbEntities.Users.Where(a => a.Email == email && a.Password == passwordEncode).FirstOrDefault();
      }

      private static User LoginWithUsername(string username, string passwordEncode)
      {
         return EFProvider.Instance.DbEntities.Users.Where(a => a.Username == username && a.Password == passwordEncode).FirstOrDefault();
      }

      private static User LoginWithPhoneNumber(string phoneNumber, string passwordEncode)
      {
         return EFProvider.Instance.DbEntities.Users.Where(a => a.PhoneNumber == phoneNumber && a.Password == passwordEncode).FirstOrDefault();
      }
   }
}