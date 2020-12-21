using LibraryManagement.Model;
using LibraryManagement.ViewModel;
using System.Windows;

namespace LibraryManagement.View
{
   /// <summary>
   /// Interaction logic for EditPublisherWindow.xaml
   /// </summary>
   public partial class EditPublisherWindow : Window
   {
      public EditPublisherWindow()
      {
         InitializeComponent();
         txtPublisherName.Focus();
      }

      public static bool Show(Publisher editPublisher = null)
      {
         var editDataContext = new EditPublisherWindowViewModel(editPublisher);
         var addWindow = new EditPublisherWindow()
         {
            DataContext = editDataContext
         };
         addWindow.ShowDialog();

         return editDataContext.EditResult;
      }
   }
}