using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace LibraryManagement.Utils
{
   public static class StringExtension
   {
      public static string Base64Decode(this string base64Text) => Base64Utils.Decode.ToString(base64Text);
      public static string Base64Encode(this string plainText) => Base64Utils.Encode.FromString(plainText);

      /// <summary>
      /// Mã hóa chuỗi theo MD5
      /// </summary>
      /// <param name="plainText">Chuỗi cần mã hóa</param>
      /// <returns>Chuỗi đã mã hóa theo MD5 có 32 ký tự</returns>
      public static string ToMD5Hash(this string plainText)
      {
         StringBuilder hash = new StringBuilder();
         MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
         byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(plainText));

         for (int i = 0; i < bytes.Length; i++)
         {
            hash.Append(bytes[i].ToString("x2"));
         }
         return hash.ToString();
      }

      /// <summary>
      /// Gọi hàm <see cref="string.Trim()"/>, kiểm tra null trước khi gọi
      /// </summary>
      /// <param name="str"></param>
      /// <returns></returns>
      public static string TrimCheck(this string str)
      {
         if (string.IsNullOrEmpty(str)) { return ""; }
         else { return str.Trim(); }
      }

      /// <summary>
      /// Xóa dấu tiếng Việt trong chuỗi
      /// </summary>
      /// <param name="str"></param>
      /// <returns></returns>
      public static string RemoveUnicode(this string str)
      {
         Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
         string temp = str.Normalize(NormalizationForm.FormD);
         return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
      }

      /// <summary>
      /// Tìm kiếm gần đúng, không phân biệt hoa thường, tiếng Việt có dấu
      /// </summary>
      /// <param name="str"></param>
      /// <param name="searchKeyWord"></param>
      /// <returns></returns>
      public static bool Like(this string str, string searchKeyWord)
      {
         if (string.IsNullOrEmpty(str)) { return false; }
         else
         {
            searchKeyWord = searchKeyWord.RemoveUnicode().ToLower();
            return str.RemoveUnicode().ToLower().Contains(searchKeyWord);
         }
      }

      public static string RemoveMutilSpace(this string str)
      {
         if (string.IsNullOrEmpty(str)) { return ""; }
         while (str.IndexOf("  ") >= 0) { str = str.Replace("  ", " "); }
         return str.TrimCheck();
      }

      public static string CapitalizeEachWord(this string str)
      {
         str = str.RemoveMutilSpace();
         if (string.IsNullOrEmpty(str)) { return ""; }

         var words = str.Split(' ').ToList();
         for (int i = words.Count - 1; i >= 0; i--)
         {
            if (string.IsNullOrEmpty(words[i])) { words.RemoveAt(i); }
            if (words[i].Length > 1)
            {
               words[i] = words[i].Substring(0, 1).ToUpper() + words[i].Substring(1).ToLower();
            }
         }

         return string.Join(" ", words);
      }

      public static string RemoveNonDigit(this string str)
      {
         if (string.IsNullOrEmpty(str)) { return ""; }
         else
         {
            for (int i = str.Length - 1; i >= 0; i--)
            {
               if (!char.IsDigit(str, i)) { str = str.Remove(i, 1); }
            }
            return str;
         }
      }

      public static string RemoveNonLetter(this string str)
      {
         if (string.IsNullOrEmpty(str)) { return ""; }
         else
         {
            for (int i = str.Length - 1; i >= 0; i--)
            {
               if (!(char.IsLetter(str, i) || char.IsWhiteSpace(str, i))) { str = str.Remove(i, 1); }
            }
            return str;
         }
      }
   }
}