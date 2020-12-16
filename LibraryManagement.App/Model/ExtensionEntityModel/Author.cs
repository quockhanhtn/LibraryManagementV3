using LibraryManagement.Utils;
using System.Collections.ObjectModel;
using System.Linq;

namespace LibraryManagement.Model
{
   public partial class Author
   {
      public string StatusDisplay { get { return (this.AuthorStatus != true) ? "Đã ẩn" : "Hiển thị"; } }
      public int NumberOfBook { get => BookInfoes.Count; }

      public ObservableCollection<string> ListBookTitle { get => BookInfoes.Select(x => x.Title).ToObservableCollection(); }
   }
}