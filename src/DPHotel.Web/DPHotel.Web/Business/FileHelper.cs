using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DPHotel.Web.Business
{
    public class FileHelper
    {
        public string CreateFilename(string fileType)
        {
            // Filename format: gsite_randomstring.ext
            string fileName = "img"; // image prefix. Might want something more imaginative later on

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[10];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);
            fileName = fileName + finalString + "." + fileType; //add the file extension.

            return fileName;
        }

        public string GetExtension(string contentType)
        {
            if (contentType != null)
            {
                string fileExtension = "";
                if (contentType == "image/jpeg")
                {
                    fileExtension = "jpg";
                    return fileExtension;
                }
                if (contentType == "image/gif")
                {
                    fileExtension = "gif";
                    return fileExtension;
                }
                if (contentType == "image/png")
                {
                    fileExtension = "png";
                    return fileExtension;
                }
                if (contentType == "image/bmp")
                {
                    fileExtension = "bmp";
                    return fileExtension;
                }
            }
            return "Error";
        }
    }
}