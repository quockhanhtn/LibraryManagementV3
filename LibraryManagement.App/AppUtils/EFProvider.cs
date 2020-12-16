using LibraryManagement.Model;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Windows;

namespace LibraryManagement
{
   public class EFProvider
   {
      public static EFProvider Instance
      {
         get
         {
            instance = instance ?? new EFProvider();
            return instance;
         }
         set => instance = value;
      }

      public LibraryManagementEntities DbEntities { get; set; }

      public void Reload()
      {
         DbEntities.Dispose();
         DbEntities = new LibraryManagementEntities();
      }

      public void SaveEntity(object entity, EntityState entityState, bool reloadDatabase = false)
      {
         if (entity == null) { return; }

         try
         {
            DbEntities.Entry(entity).State = entityState;
            Instance.DbEntities.SaveChanges();

            Instance.DbEntities.Entry(entity).State = EntityState.Detached;
            if (reloadDatabase) { Reload(); }
         }
         catch (DbEntityValidationException e)
         {
            string exceptMessage = "";
            foreach (var entityValidationError in e.EntityValidationErrors)
            {
               exceptMessage += $"Entity of type \"{entityValidationError.Entry.Entity.GetType().Name}\" in state \"{entityValidationError.Entry.State}\" has the following validation errors:\r\n";
               foreach (var validationError in entityValidationError.ValidationErrors)
               {
                  exceptMessage += $"\t- Property: \"{validationError.PropertyName}\", Error: \"{validationError.ErrorMessage}\"";
               }
            }

            Reload();
            throw new System.Exception(exceptMessage);
         }
      }

      private EFProvider()
      {
         DbEntities = new LibraryManagementEntities();
      }

      private static EFProvider instance;
   }
}