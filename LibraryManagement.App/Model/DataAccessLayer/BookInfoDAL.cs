using LibraryManagement.Utils;
using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;

namespace LibraryManagement.Model
{
   public class BookInfoDAL : IDataGet<BookInfo, long>, IDataUpdate<BookInfo, long>
   {
      public ObservableCollection<BookInfo> FindSimilar(string keyWord, EStatusFillter fillter = EStatusFillter.AllStatus)
      {
         throw new System.NotImplementedException();
      }

      public BookInfo GetById(long id) => EFProvider.Instance.DbEntities.BookInfoes.Where(b => b.BookInfoId == id).FirstOrDefault();

      public ObservableCollection<BookInfo> Gets(EStatusFillter fillter = EStatusFillter.AllStatus)
      {
         switch (fillter)
         {
            case EStatusFillter.Active:
               return EFProvider.Instance.DbEntities.BookInfoes.Where(x => x.BookInfoStatus == true).ToObservableCollection();

            case EStatusFillter.InActive:
               return EFProvider.Instance.DbEntities.BookInfoes.Where(x => x.BookInfoStatus != true).ToObservableCollection();
         }
         return EFProvider.Instance.DbEntities.BookInfoes.ToObservableCollection();
      }

      public long Add(BookInfo newObject)
      {
         try
         {
            EFProvider.Instance.SaveEntity(newObject, EntityState.Added, true);
            var bookAdded = GetById(newObject.BookInfoId);

            if (bookAdded == null) { return 0; }

            //Cập nhật tác giả
            bookAdded.Authors = newObject.Authors;
            foreach (var author in bookAdded.Authors)
            {
               author.BookInfoes.Add(bookAdded);
            }

            EFProvider.Instance.SaveEntity(bookAdded, EntityState.Modified, true);
            return bookAdded.BookInfoId;
         }
         catch (Exception e)
         {
            Logger.Log(e.Message);
            return 0;
         }
      }

      public bool Update(BookInfo objectUpdate)
      {
         try
         {
            var book = GetById(objectUpdate.BookInfoId);

            book.Title = objectUpdate.Title;
            book.BookCategoryId = objectUpdate.BookCategoryId;
            book.PublisherId = objectUpdate.PublisherId;
            book.YearPublished = objectUpdate.YearPublished;
            book.PageNumber = objectUpdate.PageNumber;
            book.Size = objectUpdate.Size;
            book.Price = objectUpdate.Price;
            book.Authors = objectUpdate.Authors;

            EFProvider.Instance.SaveEntity(book, EntityState.Modified, true);
            return true;
         }
         catch (Exception e) { Logger.Log(e.Message); }

         return false;
      }

      public bool ChangeStatus(long objectId)
      {
         try
         {
            BookInfo bookUpdate = GetById(objectId);
            if (bookUpdate != null)
            {
               bookUpdate.BookInfoStatus = bookUpdate.BookInfoStatus != true;
               EFProvider.Instance.SaveEntity(bookUpdate, EntityState.Modified, true);
               return true;
            }
         }
         catch (Exception e) { Logger.Log(e.Message); }

         return false;
      }

      public bool Delete(long objectId) => false;

      #region Singleton Declare

      public static BookInfoDAL Instance
      {
         get
         {
            instance = instance ?? new BookInfoDAL();
            return instance;
         }
         set { instance = value; }
      }

      private BookInfoDAL()
      {
      }

      private static BookInfoDAL instance;

      #endregion Singleton Declare
   }
}
