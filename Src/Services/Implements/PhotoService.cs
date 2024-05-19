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
            ); //Se crea una cuenta con los datos de configuración
            _cloudinary = new Cloudinary(account); //Se crea una instancia de Cloudinary con la cuenta creada
        }
        public async Task<ImageUploadResult> AddPhoto(IFormFile photo)
        {
            var updateResult = new ImageUploadResult();
            if(photo.Length > 0){
                if(photo.ContentType != "image/jpeg" && photo.ContentType != "image/png"){ //Se verifica que el formato de la imagen sea jpeg o png
                    throw new Exception("Formato de imagen no soportado, debe ser jpeg o png"); //Se verifica que el formato de la imagen sea jpeg o png
                }
                if(photo.Length > 10 * 1024 * 1024){ //Se verifica que el tamaño de la imagen no exceda el límite
                    throw new Exception("Tamaño de imagen excede el límite de 10MB"); //Se verifica que el tamaño de la imagen no exceda el límite
                }
                using var stream = photo.OpenReadStream(); //Se abre un stream con la imagen
                var uploadParams = new ImageUploadParams{
                    File = new FileDescription(photo.FileName, stream),
                    Transformation = new Transformation()
                        .Height(500)
                        .Width(500)
                        .Crop("fill")
                        .Gravity("face"),
                    Folder = "taller1WebMovil"
                }; //Se crean los parámetros para subir la imagen
                updateResult = await _cloudinary.UploadAsync(uploadParams); //Se sube la imagen a Cloudinary
            }
            return updateResult; //Se retorna el resultado de la subida
        }
    }
}