using LibraryManagement.Model;
using LibraryManagement.ViewModel;
using System.Windows;

namespace LibraryManagement.View
{
   /// <summary>
   /// Interaction logic for EditMemberWindow.xaml
   /// </summary>
   public partial class EditMemberWindow : Window
   {
      public EditMemberWindow()
      {
         InitializeComponent();
      }

      public static bool Show(Member editMember = null)
      {
         var editDataContext = new EditMemberWindowViewModel(editMember);
         var addWindow = new EditMemberWindow()
         {
            DataContext = editDataContext
         };
         addWindow.ShowDialog();

         return editDataContext.EditResult;
      }
   }
}