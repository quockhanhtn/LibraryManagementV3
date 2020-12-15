using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace LibraryManagement.Utils
{
   public static class CollectionExtension
   {
      static public ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> collection)
      {
         var result = new ObservableCollection<T>();
         if (collection != null)
         {
            foreach (var item in collection)
            {
               result.Add(item);
            }
         }
         return result;
      }
   }
}