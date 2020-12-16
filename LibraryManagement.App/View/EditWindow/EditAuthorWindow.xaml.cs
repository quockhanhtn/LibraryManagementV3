using LibraryManagement.Model;
using LibraryManagement.ViewModel;
using System.Windows;

namespace LibraryManagement.View.EditWindow
{
   /// <summary>
   /// Interaction logic for EditAuthorWindow.xaml
   /// </summary>
   public partial class EditAuthorWindow : Window
   {
      public EditAuthorWindow()
      {
         InitializeComponent();
      }

      public static bool Show(Author editAuthor = null)
      {
         var editDataContext = new EditAuthorWindowViewModel(editAuthor);
         var editWindow = new EditBookCategoryWindow()
         {
            DataContext = editDataContext
         };
         editWindow.ShowDialog();

         return editDataContext.EditResult;
      }
   }
}