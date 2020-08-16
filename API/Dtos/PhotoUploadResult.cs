using Microsoft.AspNetCore.Http;

namespace API.Dtos
{
    public class PhotoUploadResult
    {
        public string PublicId { get; set; }
        public string Url { get; set; }
    }

    public class PhotoParam
    {
        public IFormFile File { get; set; }
    }
       
}