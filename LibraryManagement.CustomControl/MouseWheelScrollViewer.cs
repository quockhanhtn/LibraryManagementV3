namespace LibraryManagement.CustomControl
{
   /// <summary>
   /// Derived from <see cref="System.Windows.Controls.ScrollViewer">System.Windows.Controls.ScrollViewer</see> <br/>
   /// Add <b>PreviewMouseWheel</b> event and set auto for <i>HorizontalScrollBarVisibility</i> and <i>VerticalScrollBarVisibility</i>
   /// </summary>
   public class MouseWheelScrollViewer : System.Windows.Controls.ScrollViewer
   {
      public MouseWheelScrollViewer() : base()
      {
         this.Loaded += MouseWheelScrollViewer_Loaded;

         HorizontalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto;
         VerticalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto;
      }

      private void MouseWheelScrollViewer_Loaded(object sender, System.Windows.RoutedEventArgs e)
      {
         this.PreviewMouseWheel += MouseWheelScrollViewer_PreviewMouseWheel;
      }

      private void MouseWheelScrollViewer_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
      {
         this.ScrollToVerticalOffset(this.VerticalOffset - e.Delta);
         e.Handled = true;
      }
   }
}
