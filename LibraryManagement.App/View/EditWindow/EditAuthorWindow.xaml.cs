using LibraryManagement.Model;
using LibraryManagement.ViewModel;
using System.Windows;

namespace LibraryManagement.View
{
   /// <summary>
   /// Interaction logic for EditAuthorWindow.xaml
   /// </summary>
   public partial class EditAuthorWindow : Window
   {
      public EditAuthorWindow()
      {
         InitializeComponent();
         this.txtRealName.Focus();
      }

      public static bool Show(Author editAuthor = null)
      {
         var editDataContext = new EditAuthorWindowViewModel(editAuthor);
         var editWindow = new EditAuthorWindow()
         {
            DataContext = editDataContext
         };
         editWindow.ShowDialog();

         return editDataContext.EditResult;
      }
   }
}