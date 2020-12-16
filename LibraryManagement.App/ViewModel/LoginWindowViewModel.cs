using LibraryManagement.CustomControl;
using LibraryManagement.Model;
using LibraryManagement.View;
using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LibraryManagement.ViewModel
{
   public class LoginWindowViewModel : BaseViewModel
   {
      public ICommand TabControlChangedCommand { get; set; }
      public ICommand LoginPasswordChangedCommand { get; set; }
      public ICommand LoginCommand { get; set; }
      public ICommand SignUpCommand { get; set; }
      public ICommand LostPasswordCommand { get; set; }
      public ICommand ShowPasswordCommand { get; set; }

      #region Binding Property

      public string LoginUsername
      {
         get => loginUsername;
         set
         {
            loginUsername = value;
            OnPropertyChanged(nameof(LoginUsername));
         }
      }

      public string LoginPassword
      {
         get => loginPassword;
         set
         {
            loginPassword = value;
            OnPropertyChanged(nameof(LoginPassword));
         }
      }

      public Visibility LoginPasswordBoxVisibility
      {
         get => loginPasswordBoxVisibility;
         set
         {
            loginPasswordBoxVisibility = value;
            OnPropertyChanged(nameof(LoginPasswordBoxVisibility));
         }
      }

      public Visibility LoginPasswordTextboxVisibility
      {
         get => loginPasswordTextboxVisibility;
         set
         {
            loginPasswordTextboxVisibility = value;
            OnPropertyChanged(nameof(LoginPasswordTextboxVisibility));
         }
      }

      public Visibility LoginFailedVisibility
      {
         get => loginFailedVisibility;
         set
         {
            loginFailedVisibility = value;
            OnPropertyChanged(nameof(LoginFailedVisibility));
         }
      }

      #endregion Binding Property

      public LoginWindowViewModel()
      {
         LoginPasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return p != null; }, (p) => { LoginPassword = p.Password; });

         ShowPasswordCommand = new RelayCommand<PackIcon>((p) => { return p != null; }, (p) =>
         {
            if (LoginPasswordBoxVisibility == Visibility.Visible)
            {
               p.Kind = PackIconKind.VisibilityOff;
               LoginPasswordBoxVisibility = Visibility.Collapsed;
               LoginPasswordTextboxVisibility = Visibility.Visible;
            }
            else
            {
               p.Kind = PackIconKind.Visibility;
               LoginPasswordBoxVisibility = Visibility.Visible;
               LoginPasswordTextboxVisibility = Visibility.Collapsed;
            }
         });

         LoginCommand = new RelayCommand<Window>((p) => { return p != null; }, (p) =>
         {
            FindMemberWindow.Show(true);
            FindMemberWindow.Show(false);

            User userLogin = null;

            WaitingDialog.Show(() =>
            {
               userLogin = UserDAL.Instance.Login(LoginUsername, LoginPassword);
            });

            if (userLogin == null)  // login failed
            {
               LoginFailedVisibility = Visibility.Visible;
            }
            else                    // login susseced
            {
               LoginFailedVisibility = Visibility.Collapsed;
               p.Hide();
               switch (userLogin.UserType)
               {
                  case string w when w == Definition.User.Type.Admin:
                     AdminWindow adminWindow = new AdminWindow()
                     {
                        DataContext = new AdminWindowViewModel(userLogin),
                     };
                     adminWindow.Show();
                     break;

                  case string w when w == Definition.User.Type.Librarian:
                     LibrarianWindow librarianWindow = new LibrarianWindow()
                     {
                        DataContext = new LibrarianWindowViewModel(userLogin),
                     };
                     librarianWindow.Show();
                     break;

                  case string w when w == Definition.User.Type.Member:
                     MemberWindow memberWindow = new MemberWindow()
                     {
                        DataContext = new MemberWindowViewModel(userLogin),
                     };
                     memberWindow.Show();
                     break;
               }
               p.Close();
            }
         });

         SignUpCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
         {
         });

         LostPasswordCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
         {
         });

         LoginUsername = "admin";
         LoginPassword = "12";
      }

      private string loginUsername;
      private string loginPassword;
      private Visibility loginPasswordBoxVisibility = Visibility.Visible;
      private Visibility loginPasswordTextboxVisibility = Visibility.Collapsed;
      private Visibility loginFailedVisibility = Visibility.Collapsed;
   }
}