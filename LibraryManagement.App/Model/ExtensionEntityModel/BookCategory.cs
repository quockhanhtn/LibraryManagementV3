namespace LibraryManagement.Model
{
   public partial class BookCategory
   {
      public string Note { get { return (this.BookCategoryStatus != true) ? "Đã ẩn" : ""; } }
      public int NumberOfBook { get => BookInfoes.Count; }
   }
}