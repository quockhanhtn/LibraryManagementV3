namespace LibraryManagement
{
   /// <summary>
   ///
   /// </summary>
   /// <typeparam name="T">Datatype of User account</typeparam>
   public interface IUserLogin<T>
   {
      /// <summary>
      /// Login
      /// </summary>
      /// <param name="username">user name</param>
      /// <param name="password">plan text password</param>
      /// <returns>
      /// <b>null</b> if login failed <br/>
      /// <b>User object</b> if login succesed
      /// </returns>
      T Login(string username, string password);

      /// <summary>
      ///
      /// </summary>
      /// <param name="username">Username</param>
      /// <param name="oldPassword">Old password</param>
      /// <param name="newPassword">New password</param>
      /// <returns>
      /// <b>false</b> if change password<br/>
      /// <b>true</b> if change password succesed
      /// </returns>
      bool ChangePassword(string username, string oldPassword, string newPassword);
   }
}