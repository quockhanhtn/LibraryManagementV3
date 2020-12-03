namespace LibraryManagement.Model
{
   public class BaseEntityModel : System.ICloneable
   {
      public object Clone() { return this.MemberwiseClone(); }
   }
}