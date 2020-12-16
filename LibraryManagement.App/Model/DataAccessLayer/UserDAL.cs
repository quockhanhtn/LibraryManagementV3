using LibraryManagement.Utils;
using System;
using System.Data.Entity;
using System.Linq;

namespace LibraryManagement.Model
{
   public class UserDAL : IDataUpdate<User, long>, IUserLogin<User>
   {
      public bool CheckEmail(long userId, string email) => EFProvider.Instance.DbEntities.Users.Where(u => u.Email == email && u.UserId != userId).FirstOrDefault() == null;
      public bool CheckSsn(long userId, string ssn) => EFProvider.Instance.DbEntities.Users.Where(u => u.Ssn == ssn && u.UserId != userId).FirstOrDefault() == null;
      public bool CheckPhonenumber(long userId, string phoneNumber) => EFProvider.Instance.DbEntities.Users.Where(u => u.PhoneNumber == phoneNumber && u.UserId != userId).FirstOrDefault() == null;
      public bool CheckUsername(long userId, string username) => EFProvider.Instance.DbEntities.Users.Where(u => u.Username == username && u.UserId != userId).FirstOrDefault() == null;

      public User Login(string userInput, string passwordPlantext)
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

         #region Inner method

         User LoginWithEmail(string email, string pwEncode)
         {
            return EFProvider.Instance.DbEntities.Users.Where(a => a.Email == email && a.Password == pwEncode).FirstOrDefault();
         }

         User LoginWithUsername(string username, string pwEncode)
         {
            return EFProvider.Instance.DbEntities.Users.Where(a => a.Username == username && a.Password == pwEncode).FirstOrDefault();
         }

         User LoginWithPhoneNumber(string phoneNumber, string pwEncode)
         {
            return EFProvider.Instance.DbEntities.Users.Where(a => a.PhoneNumber == phoneNumber && a.Password == pwEncode).FirstOrDefault();
         }

         #endregion
      }

      public bool ChangePassword(string username, string oldPassword, string newPassword)
      {
         return false;
      }

      public long Add(User newObject)
      {
         try
         {
            newObject.UserStatus = true;
            EFProvider.Instance.SaveEntity(newObject, EntityState.Added, true);
            return newObject.UserId;
         }
         catch (Exception e)
         {
            Logger.Log(e.Message);
            return 0;
         }
      }

      public bool Update(User objectUpdate)
      {
         try
         {
            var user = EFProvider.Instance.DbEntities.Users.Where(x => x.UserId == objectUpdate.UserId).SingleOrDefault();

            if (user != null)
            {
               user.LastName = objectUpdate.LastName;
               user.FirstName = objectUpdate.FirstName;
               user.Gender = objectUpdate.Gender;
               user.DateOfBirth = objectUpdate.DateOfBirth;
               user.Ssn = objectUpdate.Ssn;

               user.Address = objectUpdate.Address;
               user.Email = objectUpdate.Email;
               user.PhoneNumber = objectUpdate.PhoneNumber;

               EFProvider.Instance.SaveEntity(user, EntityState.Modified);
               return true;
            }
         }
         catch (Exception e) { Logger.Log(e.Message); }

         return false;
      }

      public bool ChangeStatus(long objectId)
      {
         try
         {
            var user = EFProvider.Instance.DbEntities.Users.Where(x => x.UserId == objectId).SingleOrDefault();
            if (user != null)
            {
               user.UserStatus = user.UserStatus != true;
               EFProvider.Instance.SaveEntity(user, EntityState.Modified);
               return true;
            }
         }
         catch (Exception e) { Logger.Log(e.Message); }
         
         return false;
      }

      public bool Delete(long objectId) => false;

      #region Singleton Declare

      public static UserDAL Instance
      {
         get
         {
            instance = instance ?? new UserDAL();
            return instance;
         }
         set { instance = value; }
      }

      private UserDAL()
      {
      }

      private static UserDAL instance;

      #endregion
   }
}