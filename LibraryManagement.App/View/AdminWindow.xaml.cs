using System.Windows;

namespace LibraryManagement.View
{
   /// <summary>
   /// Interaction logic for AdminWindow.xaml
   /// </summary>
   public partial class AdminWindow : Window
   {
      public AdminWindow()
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