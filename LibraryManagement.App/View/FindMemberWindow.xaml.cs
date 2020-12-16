using LibraryManagement.Model;
using LibraryManagement.ViewModel;
using System.Windows;

namespace LibraryManagement.View
{
   /// <summary>
   /// Interaction logic for FindMemberWindow.xaml
   /// </summary>
   public partial class FindMemberWindow : Window
   {
      public FindMemberWindow()
      {
         InitializeComponent();
         cmbSsn.Focus();
      }

      public static Member Show(bool isBorrowBook)
      {
         var dataContext = new FindMemberWindowViewModel(isBorrowBook);
         var findWindow = new FindMemberWindow()
         {
            DataContext = dataContext
         };
         findWindow.ShowDialog();

         return dataContext.MemberSelected;
      }
   }
}