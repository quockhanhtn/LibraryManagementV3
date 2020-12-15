using LibraryManagement.Utils;
using System.Windows.Controls;

namespace LibraryManagement.CustomControl
{
   public class DigitTextBox : TextBox
   {
      public DigitTextBox() : base()
      {
         this.SourceUpdated += DigitTextBox_SourceUpdated;
      }

      private void DigitTextBox_SourceUpdated(object sender, System.Windows.Data.DataTransferEventArgs e)
      {
         var textBox = sender as TextBox;
         textBox.Text = textBox.Text.RemoveNonDigit();
      }
   }
}