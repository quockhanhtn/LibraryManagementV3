using LibraryManagement.Utils;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LibraryManagement.CustomControl
{
   /// <summary>
   /// Interaction logic for WaitingDialog.xaml
   /// </summary>
   public partial class WaitingDialog : Window
   {
      public Task DoingTask { get; set; }
      public CancellationTokenSource TokenSource { get; set; }
      public Action DoingAction { get; set; }
      public Action CancelAction { get; set; }

      public WaitingDialog(Action doingAcction, Action cancelAction = null)
      {
         DoingAction = doingAcction;
         CancelAction = cancelAction;

         InitializeComponent();
      }

      public override void EndInit()
      {
         base.EndInit();
         TokenSource = new CancellationTokenSource();

         DoingTask = Task.Factory.StartNew(DoingAction, TokenSource.Token).ContinueWith(
            (t) => { this.Close(); },
            TaskScheduler.FromCurrentSynchronizationContext()
            );
      }

      private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
      {
         var window = this.GetRootParent() as Window;
         if (window != null)
         {
            try { window.DragMove(); }
            catch (Exception) { }
         }
      }

      private void ButtonCancel_Click(object sender, RoutedEventArgs e)
      {
         TokenSource.Cancel();
         CancelAction();
         this.Close();
      }

      public static void Show(Action doingAcction, Action cancelAction = null)
      {
         WaitingDialog waitWindow = new WaitingDialog(doingAcction, cancelAction);
         waitWindow.ShowDialog();
      }
   }
}