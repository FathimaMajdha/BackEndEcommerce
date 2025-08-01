using CloudinaryDotNet;
using CloudinaryDotNet.Actions;


namespace BackendProject.Services.CloudinaryServices
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        private readonly ILogger<CloudinaryService> _logger;

        public CloudinaryService(IConfiguration configuration, ILogger<CloudinaryService> logger)
        {
            _logger = logger;

            var cloudName = configuration["CloudinarySettings:CloudName"];
            var apiKey = configuration["CloudinarySettings:ApiKey"];
            var apiSecret = configuration["CloudinarySettings:ApiSecret"];

            if (string.IsNullOrEmpty(cloudName) || string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(apiSecret))
            {
                throw new Exception("Cloudinary configuration is missing or incomplete.");
            }

            var account = new Account(cloudName, apiKey, apiSecret);
            _cloudinary = new Cloudinary(account);
        }


        public async Task<string> UploadImageAsync(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    _logger.LogError("Upload failed: File is null or empty.");
                    throw new Exception("File is null or empty.");
                }

                using var stream = file.OpenReadStream();

                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill"),
                    Folder = "ecommerce_uploads"
                };

                var result = await _cloudinary.UploadAsync(uploadParams);

                if (result.Error != null)
                {
                    _logger.LogError($"Cloudinary upload error: {result.Error.Message}");
                    throw new Exception($"Cloudinary upload error: {result.Error.Message}");
                }

                _logger.LogInformation($"Image uploaded to Cloudinary: {result.SecureUrl}");
                return result.SecureUrl?.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"UploadImageAsync failed: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteImageAsync(string publicId)
        {
            try
            {
                var deletionParams = new DeletionParams(publicId);
                var result = await _cloudinary.DestroyAsync(deletionParams);

                var success = result.Result == "ok" || result.Result == "deleted";
                _logger.LogInformation($"Image deletion result: {result.Result}");
                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError($"DeleteImageAsync failed: {ex.Message}");
                throw;
            }
        }
    }
}
