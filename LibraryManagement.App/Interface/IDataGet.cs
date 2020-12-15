using System.Collections.ObjectModel;

namespace LibraryManagement
{
   /// <summary>
   /// Lấy dữ liệu từ database thông qua EF và chuyển sang DTO
   /// </summary>
   /// <typeparam name="ModelType">Kiểu dữ liệu object lấy ra</typeparam>
   /// <typeparam name="IdType">Kiểu dữ liệu Id của object</typeparam>
   public interface IDataGet<ModelType, IdType>
   {
      /// <summary>
      /// Lấy dữ liệu từ database, chuyển đổi thành ObservableCollection<>
      /// </summary>
      /// <param name="fillter"></param>
      /// <returns></returns>
      ObservableCollection<ModelType> Gets(EStatusFillter fillter = EStatusFillter.AllStatus);

      ModelType GetById(IdType id);

      /// <summary>
      /// Tìm kiếm gần đúng
      /// </summary>
      /// <param name="keyWord"></param>
      /// <returns></returns>
      ObservableCollection<ModelType> FindSimilar(string keyWord, EStatusFillter fillter = EStatusFillter.AllStatus);
   }
}