using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace ArticleProject.BL.Helper
{
    public class FileUploader
    {
        public static string UploadFile(IFormFile file, string folderName)
        {
            try
            {
                var fileDir = Directory.GetCurrentDirectory() + "/wwwroot/UpLoadedFiles/" +  folderName ;
                var fileName = Guid.NewGuid() + "-" + Path.GetFileName(file.FileName);
                var filePath = Path.Combine(fileDir, fileName);

                using(var stream = new FileStream(filePath,FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                return fileName;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static string RemoveFile(string folderName, string file)
        {
            try
            {
                var directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UpLoadedFiles", folderName, file);
                if (File.Exists(directory))
                {
                    File.Delete(directory);
                    return "File Deleted";
                }
                return "File Not Deleted";

            }
            catch (Exception ex)
            {

                return ex.Message;
            }
        }
    }
}
