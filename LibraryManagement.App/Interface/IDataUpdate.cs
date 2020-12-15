namespace LibraryManagement
{
   public interface IDataUpdate<ModelType, IdType>
   {
      /// <summary>
      /// Add object to Database
      /// </summary>
      /// <param name="newObject"></param>
      /// <returns>
      /// 0 if failed <br/>
      /// <b>id</b> of object if successed
      /// </returns>
      IdType Add(ModelType newObject);

      /// <summary>
      /// Cập nhật datase theo "objectUpdate"
      /// </summary>
      /// <param name="objectUpdate"></param>
      bool Update(ModelType objectUpdate);

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