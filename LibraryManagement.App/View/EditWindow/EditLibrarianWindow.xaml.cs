using LibraryManagement.Model;
using LibraryManagement.ViewModel;
using System.Windows;

namespace LibraryManagement.View.EditWindow
{
   /// <summary>
   /// Interaction logic for EditLibrarianWindow.xaml
   /// </summary>
   public partial class EditLibrarianWindow : Window
   {
      public EditLibrarianWindow()
      {
         InitializeComponent();
      }

      public static bool Show(Librarian editLibrarian = null)
      {
         var editDataContext = new EditLibrarianWindowViewModel(editLibrarian);
         var addWindow = new EditLibrarianWindow()
         {
            DataContext = editDataContext
         };
         addWindow.ShowDialog();

         return editDataContext.EditResult;
      }
   }
}