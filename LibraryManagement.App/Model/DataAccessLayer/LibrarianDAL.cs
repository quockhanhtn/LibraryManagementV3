using LibraryManagement.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;

namespace LibraryManagement.Model
{
   public class LibrarianDAL : IDataGet<Librarian, long>, IDataUpdate<Librarian, long>
   {
      public long Add(Librarian newLibraian)
      {
         try
         {
            var newUserId = UserDAL.Instance.Add(newLibraian.User);
            if (newUserId != 0)
            {
               newLibraian.UserId = newUserId;
               newLibraian.User = EFProvider.Instance.DbEntities.Users.Where(u => u.UserId == newUserId).FirstOrDefault();
               EFProvider.Instance.SaveEntity(newLibraian, EntityState.Added, true);
               return newUserId;
            }
            else { return 0; }
         }
         catch (Exception e)
         {
            _ = e.Message;
            return 0;
         }
      }

      public bool Update(Librarian objectUpdate)
      {
         if (UserDAL.Instance.Update(objectUpdate.User))
         {
            var librarian = GetById(objectUpdate.UserId);
            if (librarian != null)
            {
               librarian.StartDate = objectUpdate.StartDate;
               librarian.Salary = objectUpdate.Salary;
               EFProvider.Instance.SaveEntity(librarian, EntityState.Modified);
               return true;
            }
         }
         return false;
      }

      public bool ChangeStatus(long objectId) => UserDAL.Instance.ChangeStatus(objectId);

      public bool Delete(long objectId) => false;

      public ObservableCollection<Librarian> FindSimilar(string keyWord, EStatusFillter fillter = EStatusFillter.AllStatus)
      {
         keyWord = keyWord.TrimCheck().RemoveUnicode().ToLower();

         if (string.IsNullOrEmpty(keyWord))
         {
            return Gets(fillter);
         }
         else
         {
            if (char.IsDigit(keyWord, 0))
            {
               return Gets(fillter).Where(l => l.User.PhoneNumber.Contains(keyWord)).ToObservableCollection();
            }
            if (keyWord.Contains("@"))
            {
               return Gets(fillter).Where(l => l.User.Email.Like(keyWord)).ToObservableCollection();
            }
            return Gets(fillter).Where(x => x.User.FullName.Like(keyWord)).ToObservableCollection();
         }
      }

      public ObservableCollection<Librarian> Gets(EStatusFillter fillter = EStatusFillter.AllStatus)
      {
         var entities = new List<Librarian>();
         switch (fillter)
         {
            case EStatusFillter.AllStatus:
               entities = EFProvider.Instance.DbEntities.Librarians.ToList();
               break;

            case EStatusFillter.Active:
               entities = EFProvider.Instance.DbEntities.Librarians.Where(x => x.User.UserStatus == true).ToList();
               break;

            case EStatusFillter.InActive:
               entities = EFProvider.Instance.DbEntities.Librarians.Where(x => x.User.UserStatus != true).ToList();
               break;
         }
         return entities.ToObservableCollection();
      }

      public Librarian GetById(long id) => EFProvider.Instance.DbEntities.Librarians.Where(l => l.UserId == id).FirstOrDefault();

      #region Singleton Declare
      public static LibrarianDAL Instance
      {
         get
         {
            instance = instance ?? new LibrarianDAL();
            return instance;
         }
         set { instance = value; }
      }

      private LibrarianDAL()
      {
      }

      private static LibrarianDAL instance;
      #endregion
   }
}