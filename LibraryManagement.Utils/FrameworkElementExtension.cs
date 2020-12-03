using System.Windows;

namespace LibraryManagement.Utils
{
   public static class FrameworkElementExtension
   {
      public static FrameworkElement GetRootParent(this FrameworkElement frameworkElement)
      {
         FrameworkElement parent = frameworkElement;
         while (parent.Parent != null)
         {
            parent = parent.Parent as FrameworkElement;
         }
         return parent;
      }
   }
}
