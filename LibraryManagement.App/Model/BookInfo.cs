//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LibraryManagement.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class BookInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BookInfo()
        {
            this.BookItems = new HashSet<BookItem>();
            this.Authors = new HashSet<Author>();
        }
    
        public long BookInfoId { get; set; }
        public string Title { get; set; }
        public Nullable<long> BookCategoryId { get; set; }
        public long PublisherId { get; set; }
        public Nullable<int> YearPublished { get; set; }
        public Nullable<int> PageNumber { get; set; }
        public string Size { get; set; }
        public Nullable<decimal> Price { get; set; }
        public int Count { get; set; }
        public bool BookInfoStatus { get; set; }
        public string Image { get; set; }
    
        public virtual BookCategory BookCategory { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BookItem> BookItems { get; set; }
        public virtual Publisher Publisher { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Author> Authors { get; set; }
    }
}
