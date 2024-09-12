using Chat.Api.Exceptions;
using System.Security.Claims;

namespace Chat.Api.Helper
{
    public static class StaticHelper
    {
    


        public static string GetName(string firstName, string lastName)
        {
            return $"{firstName}, {lastName}";
        }

        public static void IsFile(IFormFile file)
        {
            var check = file.ContentType != Constants.PngType ||
                file.ContentType != Constants.JpgType;

            if (check)
                throw new PhotoNotFound();
        }


        public static async Task<byte[]> GetBytes(IFormFile file)
        {

            var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            var data = ms.ToArray();
            var checkData = data is null || data.Length == 0;
            if (checkData)
                return null;
            return data;
        }
  

   

       
    }
}
