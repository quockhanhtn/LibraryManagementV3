using LibraryManagement.Utils;
using System.Windows.Controls;
using System.Windows.Data;

namespace LibraryManagement.CustomControl
{
   public class DigitTextBox : TextBox
   {
      public DigitTextBox() : base()
      {
         this.SourceUpdated += DigitTextBox_SourceUpdated;
         this.TextChanged += DigitTextBox_TextChanged;
      }

      private void DigitTextBox_TextChanged(object sender, TextChangedEventArgs e)
      {
         var textBox = sender as TextBox;
         textBox.Text = textBox.Text.RemoveNonDigit();
      }

      private void DigitTextBox_SourceUpdated(object sender, System.Windows.Data.DataTransferEventArgs e)
      {
         var textBox = sender as TextBox;
         textBox.Text = textBox.Text.RemoveNonDigit();
      }
   }
}