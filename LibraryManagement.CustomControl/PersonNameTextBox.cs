using LibraryManagement.Utils;
using System.Windows.Controls;

namespace LibraryManagement.CustomControl
{
   public class PersonNameTextBox : TextBox
   {
      public PersonNameTextBox() : base()
      {
         this.SourceUpdated += PersonNameTextBox_SourceUpdated;
      }

      private void PersonNameTextBox_SourceUpdated(object sender, System.Windows.Data.DataTransferEventArgs e)
      {
         var textBox = sender as TextBox;
         textBox.Text = textBox.Text.RemoveMutilSpace().RemoveNonLetter().CapitalizeEachWord();
      }
   }
}