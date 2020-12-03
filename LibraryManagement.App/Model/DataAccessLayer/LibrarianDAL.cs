using LibraryManagement.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;

namespace LibraryManagement.Model
{
   public class LibrarianDAL : IDataAccessLayer<Librarian, long>
   {
      public object Add(Librarian newObject)
      {
         var newLibraian = newObject as Librarian;
         try
         {
            EFProvider.Instance.SaveEntity(newLibraian.User, EntityState.Added);
            EFProvider.Instance.SaveEntity(newLibraian, EntityState.Added, true);
         }
         catch (Exception) { return null; }

         return newLibraian.UserId;
      }

      public bool ChangeStatus(long objectId)
      {
         throw new NotImplementedException();
      }

      public bool Delete(long objectId)
      {
         throw new NotImplementedException();
      }

      public ObservableCollection<Librarian> FindSimilar(string keyWord, EStatusFillter fillter = EStatusFillter.AllStatus)
      {
         throw new NotImplementedException();
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

      public bool Update(Librarian objectUpdate)
      {
         bool updateUserResult =  EFProvider.Instance.SaveEntity(objectUpdate.User, EntityState.Modified);
         bool updateLibrarianResult =  EFProvider.Instance.SaveEntity(objectUpdate, EntityState.Modified);

         return updateUserResult && updateLibrarianResult;
      }

      public static LibrarianDAL Instance
      {
         get
         {
            instance = instance ?? new LibrarianDAL();
            return instance;
         }
         set { instance = value; }
      }

      private LibrarianDAL() { }

      private static LibrarianDAL instance;
   }
}