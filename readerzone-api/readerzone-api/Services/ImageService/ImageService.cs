namespace readerzone_api.Services.ImageService
{
    public class ImageService
    {
        private const string UPLOAD_URL = "https://api.imgbb.com/1/upload?key=df4c8bd53c06d450ea53d70528288432";
        //private const string UPLOAD_URL = "https://api.imgbb.com/1/upload";
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public ImageService(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task<string?> UploadImage(byte[] imageData)
        {
            var content = new MultipartFormDataContent();
            content.Headers.Add("X-API-KEY", "df4c8bd53c06d450ea53d70528288432");
            content.Add(new ByteArrayContent(imageData), "image", "image.png");
            var response = await _httpClient.PostAsync(UPLOAD_URL, content);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ImgBBResponse>();
            return result?.Data?.Url;            
        }
    }

    public class ImgBBResponse
    {
        public ImgBBData Data { get; set; } = null!;
    }

    public class ImgBBData
    {
        public string Url { get; set; } = string.Empty;
    }
}
