using LibraryManagement.Utils;
using MaterialDesignThemes.Wpf;
using System;
using System.Windows;
using System.Windows.Controls;

namespace LibraryManagement.CustomControl
{
   /// <summary>
   /// Interaction logic for TitleBar.xaml
   /// </summary>
   public partial class TitleBar : UserControl
   {
      public Visibility MaximinButtonVisibility
      {
         get => btnMaximizeWindow.Visibility;
         set => btnMaximizeWindow.Visibility = value;
      }

      public Visibility MinimizeButtonVisibility
      {
         get => btnMinimizeWindow.Visibility;
         set => btnMinimizeWindow.Visibility = value;
      }

      public TitleBar()
      {
         InitializeComponent();
      }

      private void btnMinimizeWindow_Click(object sender, RoutedEventArgs e)
      {
         var window = this.GetRootParent() as Window;
         if (window != null)
         {
            if (window.WindowState != WindowState.Minimized)
            {
               window.WindowState = WindowState.Minimized;
            }
         }
      }

      private void btnMaximizeWindow_Click(object sender, RoutedEventArgs e)
      {
         var window = this.GetRootParent() as Window;
         if (window != null)
         {
            if (window.WindowState != WindowState.Maximized)
            {
               window.WindowState = WindowState.Maximized;
               iconWindowMaximize.Kind = PackIconKind.WindowRestore;
               btnMaximizeWindow.ToolTip = "Restore";
            }
            else
            {
               window.WindowState = WindowState.Normal;
               iconWindowMaximize.Kind = PackIconKind.WindowMaximize;
               btnMaximizeWindow.ToolTip = "Maximize";
            }
         }
      }

      private void btnCloseWindow_Click(object sender, RoutedEventArgs e)
      {
         var window = this.GetRootParent() as Window;
         if (window != null)
         {
            window.Close();
         }
      }

      private void ucTitleBar_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
      {
         var window = this.GetRootParent() as System.Windows.Window;
         if (window != null)
         {
            try { window.DragMove(); }
            catch (Exception) { }
         }
      }
   }
}
