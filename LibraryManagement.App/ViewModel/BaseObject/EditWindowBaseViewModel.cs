using System.Windows.Input;

namespace LibraryManagement.ViewModel
{
   public enum EditMode
   {
      Add,
      Update
   }

   public class EditWindowBaseViewModel<T> : BaseViewModel
   {
      public ICommand SaveChangeCommand { get; set; }
      public ICommand RetypeCommand { get; set; }
      public ICommand CanncelCommand { get; set; }

      public string EditTitleText { get; set; }
      public EditMode Mode { get; set; }
      public T EditObject { get; set; }
      public bool EditResult { get; set; }

      protected EditWindowBaseViewModel(T editObject)
      {
         Load(editObject);
      }

      protected virtual void Load(T editObject)
      {
      }
   }
}