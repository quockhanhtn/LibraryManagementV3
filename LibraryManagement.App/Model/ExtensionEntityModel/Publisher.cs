namespace LibraryManagement.Model
{
   public partial class Publisher
   {
      public string StatusDisplay { get { return (this.PublisherStatus != true) ? "Đã ẩn" : "Hiển thị"; } }
      public int NumberOfBook { get => BookInfoes.Count; }
   }
}