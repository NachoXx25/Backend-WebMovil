using CloudinaryDotNet.Actions;

namespace taller1WebMovil.Src.Services.Interfaces
{
    public interface IPhotoService
    {
        Task <ImageUploadResult> AddPhoto(IFormFile photo);
    }
}