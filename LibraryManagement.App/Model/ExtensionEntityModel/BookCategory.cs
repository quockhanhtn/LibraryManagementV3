namespace LibraryManagement.Model
{
   public partial class BookCategory
   {
      public string StatusDisplay { get { return (this.BookCategoryStatus != true) ? "Đã ẩn" : "Hiển thị"; } }
      public int NumberOfBook { get => BookInfoes.Count; }
   }
}