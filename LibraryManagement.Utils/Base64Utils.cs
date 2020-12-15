using System.Drawing;
using System.Windows.Media;

namespace LibraryManagement.Utils
{
   public class Base64Utils
   {
      public static class Encode
      {
         public static string FromString(string plainText)
         {
            byte[] byteArr = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(byteArr);
         }

         public static string FromImage(string imageFilePath)
         {
            byte[] imageArray = System.IO.File.ReadAllBytes(imageFilePath);
            return System.Convert.ToBase64String(imageArray);
         }
      }

      public static class Decode
      {
         public static string ToString(string base64Text)
         {
            byte[] byteArr = System.Convert.FromBase64String(base64Text);
            return System.Text.Encoding.UTF8.GetString(byteArr);
         }

         public static Image ToImage(string base64ImageRepresentation)
         {
            try
            {
               byte[] bytes = System.Convert.FromBase64String(base64ImageRepresentation);

               using (var memoryStream = new System.IO.MemoryStream(bytes))
               {
                  return Image.FromStream(memoryStream);
               }
            }
            catch (System.Exception) { }
            return null;
         }

         public static ImageSource ToImageSource(string base64ImageRepresentation)
         {
            var image = ToImage(base64ImageRepresentation);
            if (image != null)
            {
               return new Bitmap(image).ToImageSource();
            }
            return null;
         }
      }
   }
}