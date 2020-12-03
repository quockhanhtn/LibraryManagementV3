using System.Windows;

namespace LibraryManagement.View
{
   /// <summary>
   /// Interaction logic for LibrarianWindow.xaml
   /// </summary>
   public partial class LibrarianWindow : Window
   {
      public LibrarianWindow()
      {
         InitializeComponent();
      }

      private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
      {
         btnCloseMenu.Visibility = Visibility.Visible;
         btnOpenMenu.Visibility = Visibility.Collapsed;
      }

      private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
      {
         btnCloseMenu.Visibility = Visibility.Collapsed;
         btnOpenMenu.Visibility = Visibility.Visible;
      }
   }
}