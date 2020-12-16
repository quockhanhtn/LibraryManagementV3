using LibraryManagement.Utils;
using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;

namespace LibraryManagement.Model
{
   public class BookCategoryDAL : IDataGet<BookCategory, long>, IDataUpdate<BookCategory, long>
   {
      public ObservableCollection<BookCategory> Gets(EStatusFillter fillter = EStatusFillter.AllStatus)
      {
         switch (fillter)
         {
            case EStatusFillter.Active:
               return EFProvider.Instance.DbEntities.BookCategories.Where(x => x.BookCategoryStatus == true).ToObservableCollection();

            case EStatusFillter.InActive:
               return EFProvider.Instance.DbEntities.BookCategories.Where(x => x.BookCategoryStatus != true).ToObservableCollection();
         }
         return EFProvider.Instance.DbEntities.BookCategories.ToObservableCollection();
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
         try
         {
            EFProvider.Instance.SaveEntity(newObject, EntityState.Added, true);
            return newObject.BookCategoryId;
         }
         catch (Exception e)
         {
            Logger.Log(e.Message);
            return 0;
         }
      }

      public bool Update(BookCategory objectUpdate)
      {
         try
         {
            var bookCategoryUpdate = GetById(objectUpdate.BookCategoryId);
            if (bookCategoryUpdate != null)
            {
               bookCategoryUpdate.BookCategoryName = objectUpdate.BookCategoryName;
               bookCategoryUpdate.LimitDays = objectUpdate.LimitDays;

               EFProvider.Instance.SaveEntity(bookCategoryUpdate, EntityState.Modified, true);
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
            var bookCategoryUpdate = GetById(objectId);
            if (bookCategoryUpdate != null)
            {
               bookCategoryUpdate.BookCategoryStatus = (bookCategoryUpdate.BookCategoryStatus != true);
               EFProvider.Instance.SaveEntity(bookCategoryUpdate, EntityState.Modified, true);
               return true;
            }
         }
         catch (Exception e) { Logger.Log(e.Message); }
         
         return false;
      }

      public bool Delete(long objectId)
      {
         try
         {
            EFProvider.Instance.DbEntities.BookCategories.Remove(GetById(objectId));
            EFProvider.Instance.DbEntities.SaveChanges();
            return true;
         }
         catch (Exception e) { Logger.Log(e.Message); }
         
         return false; 
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

      #endregion Singleton Declare
   }
}