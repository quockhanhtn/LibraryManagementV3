using System.Collections.ObjectModel;

namespace LibraryManagement.Model
{
   /// <summary>
   /// Lấy dữ liệu từ database thông qua EF và chuyển sang DTO
   /// </summary>
   /// <typeparam name="ObjectDTOType">Kiểu dữ liệu object lấy ra</typeparam>
   /// <typeparam name="IdType">Kiểu dữ liệu Id của object</typeparam>
   public interface IDataAccessLayer<ObjectDTOType, IdType>
   {
      /// <summary>
      /// Lấy dữ liệu từ database, chuyển đổi thành ObservableCollection<>
      /// </summary>
      /// <param name="fillter"></param>
      /// <returns></returns>
      ObservableCollection<ObjectDTOType> Gets(EStatusFillter fillter = EStatusFillter.AllStatus);
      /// <summary>
      /// Tìm kiếm gần đúng
      /// </summary>
      /// <param name="keyWord"></param>
      /// <returns></returns>
      ObservableCollection<ObjectDTOType> FindSimilar(string keyWord, EStatusFillter fillter = EStatusFillter.AllStatus);
      /// <summary>
      /// Add object to Database
      /// </summary>
      /// <param name="newObject"></param>
      /// <returns>
      /// 0 if failed <br/>
      /// <b>id</b> of object if successed
      /// </returns>
      object Add(ObjectDTOType newObject);
      /// <summary>
      /// Cập nhật datase theo "objectUpdate"
      /// </summary>
      /// <param name="objectUpdate"></param>
      bool Update(ObjectDTOType objectUpdate);
      /// <summary>
      /// Thay đổi trạng thái (Status)
      /// </summary>
      /// <param name="objectId">Mã của object</param>
      bool ChangeStatus(IdType objectId);
      /// <summary>
      /// Xóa object
      /// </summary>
      /// <param name="objectId">Mã của object</param>
      bool Delete(IdType objectId);
   }
}
