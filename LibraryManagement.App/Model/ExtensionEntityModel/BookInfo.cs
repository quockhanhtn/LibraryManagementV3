using System.Linq;

namespace LibraryManagement.Model
{
   public partial class BookInfo
   {
      public string StatusDisplay { get => BookInfoStatus ? "Hiển thị" : "Đã ẩn"; }

      public string AllAuthor { get => string.Join(", ", Authors.Select(a => a.NickName)); }

      public int NumberOfBorrow
      {
         get
         {
            int result = 0;
            foreach (var item in BookItems)
            {
               if (item.Borrows.Count(br => br.Status) != 0)
               {
                  result++;
               }
            }
            return result;
         }
      }
   }
}