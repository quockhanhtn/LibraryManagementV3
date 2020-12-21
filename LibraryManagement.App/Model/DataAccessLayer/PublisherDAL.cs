using LibraryManagement.Utils;
using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;

namespace LibraryManagement.Model
{
   public class PublisherDAL : IDataGet<Publisher, long>, IDataUpdate<Publisher, long>
   {
      public ObservableCollection<Publisher> FindSimilar(string keyWord, EStatusFillter fillter = EStatusFillter.AllStatus)
      {
         keyWord = keyWord.TrimCheck().RemoveUnicode().ToLower();

         if (string.IsNullOrEmpty(keyWord)) { return Gets(fillter); }
         else
         {
            return Gets(fillter).Where(x => x.PublisherName.Like(keyWord)).ToObservableCollection();
         }
      }

      public Publisher GetById(long id) => EFProvider.Instance.DbEntities.Publishers.Where(p => p.PublisherId == id).FirstOrDefault();

      public ObservableCollection<Publisher> Gets(EStatusFillter fillter = EStatusFillter.AllStatus)
      {
         switch (fillter)
         {
            case EStatusFillter.Active:
               return EFProvider.Instance.DbEntities.Publishers.Where(x => x.PublisherStatus == true).ToObservableCollection();

            case EStatusFillter.InActive:
               return EFProvider.Instance.DbEntities.Publishers.Where(x => x.PublisherStatus != true).ToObservableCollection();
         }
         return EFProvider.Instance.DbEntities.Publishers.ToObservableCollection();
      }

      public long Add(Publisher newObject)
      {
         try
         {
            EFProvider.Instance.SaveEntity(newObject, EntityState.Added, true);
            return newObject.PublisherId;
         }
         catch (Exception e)
         {
            Logger.Log(e.Message);
            return 0;
         }
      }

      public bool Update(Publisher objectUpdate)
      {
         try
         {
            Publisher publisherUpdate = GetById(objectUpdate.PublisherId);
            if (publisherUpdate != null)
            {
               publisherUpdate.PublisherName = objectUpdate.PublisherName;
               publisherUpdate.PhoneNumber = objectUpdate.PhoneNumber;
               publisherUpdate.Address = objectUpdate.Address;
               publisherUpdate.Email = objectUpdate.Email;
               publisherUpdate.Website = objectUpdate.Website;

               EFProvider.Instance.SaveEntity(publisherUpdate, EntityState.Modified, true);
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
            Publisher publisherUpdate = GetById(objectId);
            if (publisherUpdate != null)
            {
               publisherUpdate.PublisherStatus = (publisherUpdate.PublisherStatus != true);
               EFProvider.Instance.SaveEntity(publisherUpdate, EntityState.Modified, true);
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
            Publisher publisherDelete = GetById(objectId);

            if (publisherDelete != null)
            {
               EFProvider.Instance.DbEntities.Publishers.Remove(publisherDelete);
               EFProvider.Instance.DbEntities.SaveChanges();
               return true;
            }
         }
         catch (Exception e) { Logger.Log(e.Message); }
         
         return false;
      }

      public bool CheckNameExits(string name) => EFProvider.Instance.DbEntities.Publishers.Where(p => p.PublisherName.Equals(name)).FirstOrDefault() != null;

      #region Singleton Declare

      public static PublisherDAL Instance
      {
         get
         {
            instance = instance ?? new PublisherDAL();
            return instance;
         }
         set { instance = value; }
      }

      private PublisherDAL()
      {
      }

      private static PublisherDAL instance;

      #endregion Singleton Declare
   }
}
