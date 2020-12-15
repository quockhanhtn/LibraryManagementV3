using LibraryManagement.Utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;

namespace LibraryManagement.Model
{
   public class BookCategoryDAL : IDataGet<BookCategory, long>, IDataUpdate<BookCategory, long>
   {
      public ObservableCollection<BookCategory> Gets(EStatusFillter fillter = EStatusFillter.AllStatus)
      {
         var entities = new List<BookCategory>();
         switch (fillter)
         {
            case EStatusFillter.AllStatus:
               entities = EFProvider.Instance.DbEntities.BookCategories.ToList();
               break;

            case EStatusFillter.Active:
               entities = EFProvider.Instance.DbEntities.BookCategories.Where(x => x.BookCategoryStatus == true).ToList();
               break;

            case EStatusFillter.InActive:
               entities = EFProvider.Instance.DbEntities.BookCategories.Where(x => x.BookCategoryStatus == false).ToList();
               break;
         }
         return entities.ToObservableCollection();
      }

      public BookCategory GetById(long id) => EFProvider.Instance.DbEntities.BookCategories.Where(bc => bc.BookCategoryId == id).FirstOrDefault();

      public ObservableCollection<BookCategory> FindSimilar(string keyWord, EStatusFillter fillter = EStatusFillter.AllStatus)
      {
         keyWord = keyWord.TrimCheck().RemoveUnicode().ToLower();

         if (string.IsNullOrEmpty(keyWord))
         {
            return Gets(fillter);
         }
         else
         {
            return Gets(fillter).Where(x => x.BookCategoryName.Like(keyWord)).ToObservableCollection();
         }
      }

      public long Add(BookCategory newObject)
      {
         EFProvider.Instance.SaveEntity(newObject, EntityState.Added, true);
         return newObject.BookCategoryId;
      }

      public bool Update(BookCategory objectUpdate)
      {
         var bookCategoryUpdate = GetById(objectUpdate.BookCategoryId);
         if (bookCategoryUpdate != null)
         {
            bookCategoryUpdate.BookCategoryName = objectUpdate.BookCategoryName;
            bookCategoryUpdate.LimitDays = objectUpdate.LimitDays;

            EFProvider.Instance.SaveEntity(bookCategoryUpdate, EntityState.Modified, true);
            return true;
         }
         else { return false; }
      }

      public bool ChangeStatus(long objectId)
      {
         var bookCategoryUpdate = EFProvider.Instance.DbEntities.BookCategories.Where(x => x.BookCategoryId == objectId).SingleOrDefault();
         if (bookCategoryUpdate != null)
         {
            bookCategoryUpdate.BookCategoryStatus = (bookCategoryUpdate.BookCategoryStatus != true);
            EFProvider.Instance.SaveEntity(bookCategoryUpdate, EntityState.Modified, true);
            return true;
         }
         else { return false; }
      }

      public bool Delete(long objectId)
      {
         var bookCategoryDelete = EFProvider.Instance.DbEntities.BookCategories.Where(x => x.BookCategoryId == objectId).SingleOrDefault();

         if (bookCategoryDelete != null)
         {
            EFProvider.Instance.DbEntities.BookCategories.Remove(bookCategoryDelete);
            EFProvider.Instance.DbEntities.SaveChanges();
            return true;
         }
         else { return false; }
      }

      #region Singleton Declare
      public static BookCategoryDAL Instance
      {
         get
         {
            instance = instance ?? new BookCategoryDAL();
            return instance;
         }
         set { instance = value; }
      }

      private BookCategoryDAL()
      {
      }

      private static BookCategoryDAL instance;
      #endregion
   }
}