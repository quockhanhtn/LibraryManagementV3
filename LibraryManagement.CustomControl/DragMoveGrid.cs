using LibraryManagement.Utils;
using System;

namespace LibraryManagement.CustomControl
{
   /// <summary>
   /// Derived from <see cref="System.Windows.Controls.Grid">System.Windows.Controls.Grid</see> <br/>
   /// Add <b>DragMove</b> when <i>MouseLeftButtonDown</i>
   /// </summary>
   public class DragMoveGrid : System.Windows.Controls.Grid
   {
      public DragMoveGrid()
      {
         this.Loaded += DragMoveGrid_Loaded;
      }

      private void DragMoveGrid_Loaded(object sender, System.Windows.RoutedEventArgs e)
      {
         this.MouseLeftButtonDown += DragMoveGrid_MouseLeftButtonDown;
      }

      private void DragMoveGrid_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
      {
         if (this.GetRootParent() is System.Windows.Window window)
         {
            try { window.DragMove(); }
            catch (Exception) { }
         }
      }
   }
}