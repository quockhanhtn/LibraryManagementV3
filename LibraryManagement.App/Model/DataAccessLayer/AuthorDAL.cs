using LibraryManagement.Utils;
using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;

namespace LibraryManagement.Model
{
   public class AuthorDAL : IDataGet<Author, long>, IDataUpdate<Author, long>
   {
      public ObservableCollection<Author> FindSimilar(string keyWord, EStatusFillter fillter = EStatusFillter.AllStatus)
      {
         keyWord = keyWord.TrimCheck().RemoveUnicode().ToLower();

         if (string.IsNullOrEmpty(keyWord)) { return Gets(fillter); }
         else
         {
            return Gets(fillter).Where(x => x.RealName.Like(keyWord) || x.NickName.Like(keyWord)).ToObservableCollection();
         }
      }

      public Author GetById(long id) => EFProvider.Instance.DbEntities.Authors.Where(a => a.AuthorId == id).FirstOrDefault();

      public ObservableCollection<Author> Gets(EStatusFillter fillter = EStatusFillter.AllStatus)
      {
         switch (fillter)
         {
            case EStatusFillter.Active:
               return EFProvider.Instance.DbEntities.Authors.Where(x => x.AuthorStatus == true).ToObservableCollection();

            case EStatusFillter.InActive:
               return EFProvider.Instance.DbEntities.Authors.Where(x => x.AuthorStatus != true).ToObservableCollection();
         }
         return EFProvider.Instance.DbEntities.Authors.ToObservableCollection();    // all status
      }

      public long Add(Author newObject)
      {
         try
         {
            EFProvider.Instance.SaveEntity(newObject, EntityState.Added, true);
            return newObject.AuthorId;
         }
         catch (Exception e)
         {
            Logger.Log(e.Message);
            return 0;
         }
      }

      public bool Update(Author objectUpdate)
      {
         try
         {
            Author authorUpdate = GetById(objectUpdate.AuthorId);

            if (authorUpdate != null)
            {
               authorUpdate.RealName = objectUpdate.RealName;
               authorUpdate.NickName = objectUpdate.NickName;

               EFProvider.Instance.SaveEntity(authorUpdate, EntityState.Modified, true);
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
            Author authorUpdate = GetById(objectId);

            if (authorUpdate != null)
            {
               authorUpdate.AuthorStatus = (authorUpdate.AuthorStatus != true);
               EFProvider.Instance.SaveEntity(authorUpdate, EntityState.Modified, true);
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
            EFProvider.Instance.DbEntities.Authors.Remove(GetById(objectId));
            EFProvider.Instance.DbEntities.SaveChanges();
            return true;
         }
         catch (Exception e) { Logger.Log(e.Message); }
         
         return false;
      }

      #region Singleton Declare

      public static AuthorDAL Instance
      {
         get
         {
            instance = instance ?? new AuthorDAL();
            return instance;
         }
         set { instance = value; }
      }

      private AuthorDAL()
      {
      }

      private static AuthorDAL instance;

      #endregion Singleton Declare
   }
}