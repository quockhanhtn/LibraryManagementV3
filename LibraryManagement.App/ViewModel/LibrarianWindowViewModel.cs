using LibraryManagement.Model;
using LibraryManagement.View.Page;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LibraryManagement.ViewModel
{
   public class LibrarianWindowViewModel : BaseViewModel
   {
      public ICommand LoadedWindow { get; set; }
      public ICommand NavSelectionChangedCommand { get; set; }

      public string WindowTitle { get => windowTitle; set { windowTitle = value; OnPropertyChanged(nameof(WindowTitle)); } }
      public Grid GridMain { get; set; }
      public Grid GridCursor { get; set; }

      public UserControl PageUserInfo { get; set; }
      public UserControl PageManagerMember { get; set; }
      public UserControl PageManagerBook { get; set; }
      public UserControl PageManagerPublisher { get; set; }
      public UserControl PageManagerBookCategory { get; set; }
      public UserControl PageManagerAuthor { get; set; }
      public UserControl PageAbout { get; set; }

      public LibrarianWindowViewModel(User userLogin)
      {
         LoadedWindow = new RelayCommand<Window>((p) => { return (p != null); }, (p) =>
         {
            this.WindowTitle = ("Library Management - Librarian: " + userLogin.LastName + " " + userLogin.FirstName).Trim();
            InitPage();

            GridCursor = p.FindName("gridCursor") as Grid;
            GridMain = p.FindName("gridMain") as Grid;
            // set default page
            GridCursor.Margin = new Thickness(0, 60 * 2, 0, 0);
            GridMain.Children.Add(this.PageManagerBook);
         });

         NavSelectionChangedCommand = new RelayCommand<Window>((p) => { return p != null; }, (p) =>
         {
            var lvNavigationMenu = p.FindName("lvNavigationMenu") as ListView;
            var lvNavigationMenuSelectedItem = lvNavigationMenu.SelectedItem as ListViewItem;

            GridCursor.Margin = new Thickness(0, 60 * lvNavigationMenu.SelectedIndex, 0, 0);
            GridMain.Children.Clear();
            switch (lvNavigationMenuSelectedItem.Name)
            {
               case "mnuUserInfo":
                  GridMain.Children.Add(this.PageUserInfo);
                  break;

               case "mnuManagerMember":
                  GridMain.Children.Add(this.PageManagerMember);
                  break;

               case "mnuManagerBook":
                  GridMain.Children.Add(this.PageManagerBook);
                  break;

               case "mnuManagerPublisher":
                  GridMain.Children.Add(this.PageManagerPublisher);
                  break;

               case "mnuManagerBookCategory":
                  GridMain.Children.Add(this.PageManagerBookCategory);
                  break;

               case "mnuManagerAuthor":
                  GridMain.Children.Add(this.PageManagerAuthor);
                  break;

               case "mnuAbout":
                  GridMain.Children.Add(this.PageAbout);
                  break;

               case "mnuLogout":
                  var messageResult = CustomControl.CustomMessageBox.Show("Bạn có thực sự muốn đăng xuất ?", "Cảnh báo", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
                  if (messageResult == MessageBoxResult.Yes)
                  {
                     System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                     Application.Current.Shutdown();
                  }
                  else
                  {
                     lvNavigationMenu.SelectedIndex = 0;
                     NavSelectionChangedCommand.Execute(p);
                  }
                  break;
            }
         });
      }

      private void InitPage()
      {
         // Thông tin tài khoản
         this.PageUserInfo = new PageUserInfo()
         {
            DataContext = new PageUserInfoViewModel(),
         };
         // Trang Quản lý độc giả
         this.PageManagerMember = new PageManagerMember()
         {
            DataContext = new PageManagerMemberViewModel(),
         };
         // Trang Quản lý sách
         this.PageManagerBook = new PageManagerBook()
         {
            DataContext = new PageManagerBookViewModel(),
         };
         // Trang quản lý Nhà xuất bản
         this.PageManagerPublisher = new PageManagerPublisher()
         {
            DataContext = new PageManagerPublisherViewModel(),
         };
         // Trang quản lý chuyên mục sách
         this.PageManagerBookCategory = new PageManagerBookCategory()
         {
            DataContext = new PageManagerBookCategoryViewModel(),
         };
         // Trang quản lý tác giả
         this.PageManagerAuthor = new PageManagerAuthor()
         {
            DataContext = new PageManagerAuthorViewModel(),
         };
         // Trang thông tin phần mềm
         this.PageAbout = new PageAbout()
         {
            DataContext = new PageAboutViewModel(),
         };
      }

      private string windowTitle;
   }
}