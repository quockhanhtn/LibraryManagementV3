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

      private void BtnMinimizeWindow_Click(object sender, RoutedEventArgs e)
      {
         if (this.GetRootParent() is Window window)
         {
            if (window.WindowState != WindowState.Minimized)
            {
               window.WindowState = WindowState.Minimized;
            }
         }
      }

      private void BtnMaximizeWindow_Click(object sender, RoutedEventArgs e)
      {
         if (this.GetRootParent() is Window window)
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

      private void BtnCloseWindow_Click(object sender, RoutedEventArgs e)
      {
         if (this.GetRootParent() is Window window)
         {
            window.Close();
         }
      }

      private void UcTitleBar_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
      {
         if (this.GetRootParent() is System.Windows.Window window)
         {
            try { window.DragMove(); }
            catch (Exception) { }
         }
      }
   }
}