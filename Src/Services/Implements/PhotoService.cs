using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using taller1WebMovil.Src.Helpers;
using taller1WebMovil.Src.Services.Interfaces;

namespace taller1WebMovil.Src.Services.Implements
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;
        public PhotoService(IOptions<CloudinarySettings> config)
        {
            var account = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            ); 
            _cloudinary = new Cloudinary(account);
        }
        public async Task<ImageUploadResult> AddPhoto(IFormFile photo)
        {
            var updateResult = new ImageUploadResult();
            if(photo.Length > 0){
                if(photo.ContentType != "image/jpeg" && photo.ContentType != "image/png"){
                    throw new Exception("Formato de imagen no soportado, debe ser jpeg o png");
                }
                if(photo.Length > 10 * 1024 * 1024){
                    throw new Exception("Tamaño de imagen excede el límite de 10MB");
                }
                using var stream = photo.OpenReadStream();
                var uploadParams = new ImageUploadParams{
                    File = new FileDescription(photo.FileName, stream),
                    Transformation = new Transformation()
                        .Height(500)
                        .Width(500)
                        .Crop("fill")
                        .Gravity("face"),
                    Folder = "taller1WebMovil"
                };
                updateResult = await _cloudinary.UploadAsync(uploadParams);
            }
            return updateResult;
        }
    }
}