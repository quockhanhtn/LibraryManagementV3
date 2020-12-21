using LibraryManagement.Model;
using LibraryManagement.ViewModel;
using System.Windows;

namespace LibraryManagement.View
{
   /// <summary>
   /// Interaction logic for EditBookWindow.xaml
   /// </summary>
   public partial class EditBookWindow : Window
   {
      public EditBookWindow()
      {
         InitializeComponent();
         txtTitle.Focus();
      }

      public static bool Show(BookInfo editBookInfo = null)
      {
         var editDataContext = new EditBookWindowViewModel(editBookInfo);
         var addWindow = new EditBookWindow()
         {
            DataContext = editDataContext
         };
         addWindow.ShowDialog();

         return editDataContext.EditResult;
      }
   }
}