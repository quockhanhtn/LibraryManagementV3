using LibraryManagement.Utils;
using MaterialDesignThemes.Wpf;
using OfficeOpenXml;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace LibraryManagement.ViewModel
{
   public class PageManagerBaseViewModel<T> : BaseViewModel
   {
      public ICommand SearchCommand { get; set; }
      public ICommand ExportToExcelCommand { get; set; }
      public ICommand AddCommand { get; set; }
      public ICommand UpdateCommand { get; set; }
      public ICommand StatusOnCommand { get; set; }
      public ICommand StatusOffCommand { get; set; }
      public ICommand DeleteCommand { get; set; }
      public SnackbarMessageQueue SnackbarMessageQueue { get; set; }

      public ObservableCollection<T> ListDTO { get => listDTO; set { listDTO = value; OnPropertyChanged(nameof(ListDTO)); } }
      public T DTOSelected { get => _DTOSelected; set { _DTOSelected = value; OnPropertyChanged(nameof(DTOSelected)); } }

      protected PageManagerBaseViewModel()
      {
      }

      protected void StyleExcelWorkSheet(ExcelWorksheet worksheet, string title, string[] columnHeaders, double[] columnWidths)
      {
         worksheet.SetTitleAndDateTime(title.ToUpper(), DateTime.Now, columnHeaders.Length);
         worksheet.SetHeader(columnHeaders);
         worksheet.SetColumWidth(columnWidths);
      }

      protected void ShowSnackbarMessage(object message, double durationSecond = 5)
      {
         this.SnackbarMessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(durationSecond));
         this.SnackbarMessageQueue.Clear();
         this.SnackbarMessageQueue.Enqueue(message);
         OnPropertyChanged(nameof(this.SnackbarMessageQueue));
      }

      protected void ShowSnackbarMessage(object message, object actionContent, Action actionHandler, double durationSecond = 5)
      {
         this.SnackbarMessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(durationSecond));
         this.SnackbarMessageQueue.Clear();
         this.SnackbarMessageQueue.Enqueue(message, actionContent, actionHandler);
         OnPropertyChanged(nameof(this.SnackbarMessageQueue));
      }

      private ObservableCollection<T> listDTO;
      private T _DTOSelected;
   }
}