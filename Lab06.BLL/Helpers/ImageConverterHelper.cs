using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace Lab06.BLL.Helpers
{
    public static class ImageConverterHelper
    {
        public static IFormFile FromByteArrayToFormFile(byte[] byteArray)
        {
            var stream = new MemoryStream(byteArray);
            IFormFile file = new FormFile(stream, 0, byteArray.Length, "name", "fileName");
            return file;
        }

        public static string FromByteArrayToString(byte[] file)
        {
            string imreBase64Data = Convert.ToBase64String(file);
            return $"data:image/png;base64,{imreBase64Data}";
        }

        public static byte[] FromFormFileToByteArray(IFormFile file)
        {
            if (file.Length > 0)
            {
                using var ms = new MemoryStream();
                file.CopyTo(ms);
                var fileBytes = ms.ToArray();

                return fileBytes;
            }

            return null;
        }

        public static IFormFile FromStringToFormFile(string data)
        {
            var offset = data.IndexOf(',') + 1;
            var byteArray = Convert.FromBase64String(data[offset..^0]);

            var stream = new MemoryStream(byteArray);
            IFormFile file = new FormFile(stream, 0, byteArray.Length, "name", "fileName");
            return file;
        }
    }
}
