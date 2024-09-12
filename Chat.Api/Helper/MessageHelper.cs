namespace Chat.Api.Helper
{
    public class MessageHelper
    {
        private readonly IHostEnvironment _hostEnvironment;
        public MessageHelper(IHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }



    
        public  async Task<string> WriteToFile(IFormFile file)
        {
            if (file == null) return string.Empty;
            var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            var data = ms.ToArray();
            var fileUrl = GetFilePath();
            var checkData = data is null || data.Length == 0;
            if (checkData)
                return string.Empty;

            await File.WriteAllBytesAsync(fileUrl, data);
            return fileUrl;
        }
        
        public string GetFilePath()
        {
            var projectPath = _hostEnvironment.ContentRootPath;
            var name = Guid.NewGuid().ToString();
            var fileUrl = projectPath + "\\wwwroot\\MessageFile" + name;
            return fileUrl;
        }

     

        
    }
}
