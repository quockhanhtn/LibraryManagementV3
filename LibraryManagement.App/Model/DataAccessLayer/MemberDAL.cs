using LibraryManagement.Utils;
using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;

namespace LibraryManagement.Model
{
   public class MemberDAL : IDataGet<Member, long>, IDataUpdate<Member, long>
   {
      public ObservableCollection<Member> Gets(EStatusFillter fillter = EStatusFillter.AllStatus)
      {
         switch (fillter)
         {
            case EStatusFillter.Active:
               return EFProvider.Instance.DbEntities.Members.Where(x => x.User.UserStatus == true).ToObservableCollection();

            case EStatusFillter.InActive:
               return EFProvider.Instance.DbEntities.Members.Where(x => x.User.UserStatus != true).ToObservableCollection();
         }
         return EFProvider.Instance.DbEntities.Members.ToObservableCollection();
      }

      public Member GetById(long id) => EFProvider.Instance.DbEntities.Members.Where(x => x.UserId == id).FirstOrDefault();

      public ObservableCollection<Member> FindSimilar(string keyWord, EStatusFillter fillter = EStatusFillter.AllStatus)
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

      public long Add(Member newMember)
      {
         try
         {
            var newUserId = UserDAL.Instance.Add(newMember.User);
            if (newUserId != 0)
            {
               newMember.UserId = newUserId;
               newMember.User = EFProvider.Instance.DbEntities.Users.Where(u => u.UserId == newUserId).FirstOrDefault();
               EFProvider.Instance.SaveEntity(newMember, EntityState.Added, true);
               return newUserId;
            }
            else { return 0; }
         }
         catch (Exception e)
         {
            Logger.Log(e.Message);
            return 0;
         }
      }

      public bool Update(Member objectUpdate)
      {
         try
         {
            if (UserDAL.Instance.Update(objectUpdate.User))
            {
               Member member = GetById(objectUpdate.UserId);
               if (member != null)
               {
                  member.ExpDate = objectUpdate.ExpDate;
                  member.RegisterDate = objectUpdate.RegisterDate;

                  EFProvider.Instance.SaveEntity(member, EntityState.Modified);
                  return true;
               }
            }
         }
         catch (Exception e) { Logger.Log(e.Message); }
         
         return false;
      }

      public bool ChangeStatus(long objectId) => UserDAL.Instance.ChangeStatus(objectId);

      public bool Delete(long objectId) => false;

      #region Singleton Declare

      public static MemberDAL Instance
      {
         get
         {
            instance = instance ?? new MemberDAL();
            return instance;
         }
         set { instance = value; }
      }

      private MemberDAL()
      {
      }

      private static MemberDAL instance;

      #endregion Singleton Declare
   }
}