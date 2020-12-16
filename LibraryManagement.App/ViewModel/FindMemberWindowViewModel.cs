using LibraryManagement.Model;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace LibraryManagement.ViewModel
{
   public class FindMemberWindowViewModel : BaseViewModel
   {
      public bool IsBorrowBook { get; set; }

      public ICommand OKCommand { get; set; }
      public ICommand CancelCommand { get; set; }

      public Member MemberSelected
      {
         get => memberSelected;
         set
         {
            memberSelected = value;
            OnPropertyChanged(nameof(MemberSelected));
            if (value != null && value.User.UserStatus != true && IsBorrowBook)
            {
               WarningText = "Độc giả bị khóa ! Không thể mượn thêm sách";
            }
            else { WarningText = ""; }
         }
      }

      public ObservableCollection<Member> ListMember { get => listMember; set { listMember = value; OnPropertyChanged(nameof(ListMember)); } }

      public string WarningText { get => warningText; set { warningText = value; OnPropertyChanged(); } }

      public FindMemberWindowViewModel(bool isBorrowBook)
      {
         ListMember = MemberDAL.Instance.Gets();

         IsBorrowBook = isBorrowBook;

         OKCommand = new RelayCommand<Window>((p) => p != null && MemberSelected != null && (IsBorrowBook != true || MemberSelected.User.UserStatus), (p) => p.Close());

         CancelCommand = new RelayCommand<Window>((p) => p != null, (p) => { p.Close(); MemberSelected = null; });
      }

      private Member memberSelected;
      private ObservableCollection<Member> listMember;
      private string warningText;
   }
}