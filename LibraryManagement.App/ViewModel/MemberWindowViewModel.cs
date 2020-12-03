using LibraryManagement.Model;
using LibraryManagement.View.Page;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LibraryManagement.ViewModel
{
   public class MemberWindowViewModel : BaseViewModel
   {
      public ICommand LoadedWindow { get; set; }
      public ICommand NavSelectionChangedCommand { get; set; }

      public string WindowTitle { get => windowTitle; set { windowTitle = value; OnPropertyChanged(nameof(WindowTitle)); } }
      public Grid GridMain { get; set; }
      public Grid GridCursor { get; set; }

      public UserControl PageUserInfo { get; set; }
      public UserControl PageViewListBookBorrow { get; set; }
      public UserControl PageAbout { get; set; }

      public MemberWindowViewModel(User userLogin)
      {
         LoadedWindow = new RelayCommand<Window>((p) => { return (p != null); }, (p) =>
         {
            this.WindowTitle = ("Library Management - Member: " + userLogin.LastName + " " + userLogin.FirstName).Trim();
            InitPage();

            GridCursor = p.FindName("gridCursor") as Grid;
            GridMain = p.FindName("gridMain") as Grid;
            // set default page
            GridCursor.Margin = new Thickness(0, 60, 0, 0);
            GridMain.Children.Add(this.PageViewListBookBorrow);
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

               case "mnuListBookBorrow":
                  GridMain.Children.Add(this.PageViewListBookBorrow);
                  break;

               case "mnuAbout":
                  GridMain.Children.Add(this.PageAbout);
                  break;

               case "mnuLogout":
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

         // Trang xem các sách đăng mượn
         this.PageViewListBookBorrow = new PageViewListBorrowBook()
         {
            DataContext = new PageViewListBorrowBookViewModel(),
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
