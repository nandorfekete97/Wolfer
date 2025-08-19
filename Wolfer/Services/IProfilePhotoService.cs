using Wolfer.Data.DTOs;
using Wolfer.Data.Entities;

namespace Wolfer.Services;

public interface IProfilePhotoService
{
    Task<ProfilePhotoEntity> GetPhotoByUserId(string userId);
    Task AddProfilePhoto(ProfilePhotoUploadDTO profilePhoto);
    //Task UpdateProfilePhoto(ProfilePhotoUploadDTO profilePhoto);
    Task DeleteProfilePhoto(string userId);
}