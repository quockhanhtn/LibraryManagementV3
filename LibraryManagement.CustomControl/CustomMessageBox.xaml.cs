using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LibraryManagement.CustomControl
{
   /// <summary>
   /// Interaction logic for CustomMessageBox.xaml
   /// </summary>
   public partial class CustomMessageBox : Window
   {
      private MessageBoxResult ShowResult;

      public CustomMessageBox()
      {
         InitializeComponent();
      }

      private void SetIcon(MessageBoxImage icon)
      {
         switch (icon)
         {
            case MessageBoxImage.None:
               icoBox.Visibility = Visibility.Hidden;
               break;

            case MessageBoxImage.Question:
               icoBox.Kind = PackIconKind.QuestionMarkRhombus;
               icoBox.Foreground = (Brush)(new BrushConverter()).ConvertFrom("#007acc");
               break;

            case MessageBoxImage.Error:
               icoBox.Kind = PackIconKind.MultiplyBox;
               icoBox.Foreground = (Brush)(new BrushConverter()).ConvertFrom("#f38b76");
               break;

            case MessageBoxImage.Warning:
               icoBox.Kind = PackIconKind.WarningBox;
               icoBox.Foreground = (Brush)(new BrushConverter()).ConvertFrom("#d9b172");
               break;

            case MessageBoxImage.Information:
               icoBox.Kind = PackIconKind.InformationOutline;
               icoBox.Foreground = (Brush)(new BrushConverter()).ConvertFrom("#66b158");
               break;

            default:
               break;
         }
      }

      private void SetButtons(MessageBoxButton buttons)
      {
         switch (buttons)
         {
            case MessageBoxButton.OK:
               btnleft.Visibility = Visibility.Visible;
               txtLeft.Text = "OK";
               icoLeft.Kind = PackIconKind.HandOkay;
               btnleft.Tag = "1";   // OK

               btnMid.Visibility = Visibility.Collapsed;
               btnRight.Visibility = Visibility.Collapsed;
               break;

            case MessageBoxButton.OKCancel:
               btnleft.Visibility = Visibility.Visible;
               txtLeft.Text = "OK";
               icoLeft.Kind = PackIconKind.HandOkay;
               btnleft.Tag = "1";   // OK

               btnMid.Visibility = Visibility.Visible;
               txtMid.Text = "Hủy";
               icoMid.Kind = PackIconKind.Cancel;
               btnMid.Tag = "2";    // Cancel

               btnRight.Visibility = Visibility.Collapsed;
               break;

            case MessageBoxButton.YesNoCancel:
               btnleft.Visibility = Visibility.Visible;
               txtLeft.Text = "Có";
               icoLeft.Kind = PackIconKind.Check;
               btnleft.Tag = "6";   // Yes

               btnMid.Visibility = Visibility.Visible;
               txtMid.Text = "Không";
               icoMid.Kind = PackIconKind.Close;
               btnMid.Tag = "7";    // No

               btnRight.Visibility = Visibility.Visible;
               txtRight.Text = "Hủy";
               icoRight.Kind = PackIconKind.Cancel;
               btnRight.Tag = "2";    // Cancel
               break;

            case MessageBoxButton.YesNo:
               btnleft.Visibility = Visibility.Visible;
               txtLeft.Text = "Có";
               icoLeft.Kind = PackIconKind.Check;
               btnleft.Tag = "6";   // Yes

               btnMid.Visibility = Visibility.Visible;
               txtMid.Text = "Không";
               icoMid.Kind = PackIconKind.Close;
               btnMid.Tag = "7";    // No

               btnRight.Visibility = Visibility.Collapsed;
               break;
         }
      }

      public static MessageBoxResult Show(string messageText, string caption, MessageBoxButton buttons, MessageBoxImage icon, MessageBoxResult defaultResult = MessageBoxResult.OK)
      {
         CustomMessageBox customMessageBox = new CustomMessageBox();
         customMessageBox.titleBar.Tag = caption;
         customMessageBox.tblContent.Text = messageText;
         customMessageBox.SetButtons(buttons);
         customMessageBox.SetIcon(icon);
         customMessageBox.ShowResult = defaultResult;

         customMessageBox.ShowDialog();

         return customMessageBox.ShowResult;
      }

      private void BtnClick(object sender, RoutedEventArgs e)
      {
         switch ((sender as Button).Tag)
         {
            case "0":
               ShowResult = MessageBoxResult.None;
               break;

            case "1":
               ShowResult = MessageBoxResult.OK;
               break;

            case "2":
               ShowResult = MessageBoxResult.Cancel;
               break;

            case "6":
               ShowResult = MessageBoxResult.Yes;
               break;

            case "7":
               ShowResult = MessageBoxResult.No;
               break;

            default:
               ShowResult = MessageBoxResult.None;
               break;
         }
         this.Close();
      }
   }
}