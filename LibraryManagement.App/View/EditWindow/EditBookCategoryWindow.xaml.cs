using LibraryManagement.Model;
using LibraryManagement.ViewModel;
using System.Windows;

namespace LibraryManagement.View
{
   /// <summary>
   /// Interaction logic for EditBookCategoryWindow.xaml
   /// </summary>
   public partial class EditBookCategoryWindow : Window
   {
      public EditBookCategoryWindow()
      {
         InitializeComponent();
         txtName.Focus();
      }

      public static bool Show(BookCategory editBookCategory = null)
      {
         var editDataContext = new EditBookCategoryWindowViewModel(editBookCategory);
         var editWindow = new EditBookCategoryWindow()
         {
            DataContext = editDataContext
         };
         editWindow.ShowDialog();

         return editDataContext.EditResult;
      }
   }
}