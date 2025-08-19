using Wolfer.Data.Entities;

namespace Wolfer.Repositories;

public interface IProfilePhotoRepository
{
    Task<ProfilePhotoEntity> GetByUserId(string userId);
    Task AddProfilePhoto(ProfilePhotoEntity profilePhoto);
    Task UpdateProfilePhoto(ProfilePhotoEntity profilePhoto);
    Task<bool> DeleteProfilePhoto(string userId);
}