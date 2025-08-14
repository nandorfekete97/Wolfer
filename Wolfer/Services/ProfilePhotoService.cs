using Wolfer.Data.DTOs;
using Wolfer.Data.Entities;
using Wolfer.Repositories;

namespace Wolfer.Services;

public class ProfilePhotoService : IProfilePhotoService
{
    private IProfilePhotoRepository _profilePhotoRepository;

    public ProfilePhotoService(IProfilePhotoRepository profilePhotoRepository)
    {
        _profilePhotoRepository = profilePhotoRepository;
    }
    
    public async Task<ProfilePhotoEntity> GetPhotoByUserId(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new ArgumentException("Invalid user ID.");
        }

        return await _profilePhotoRepository.GetByUserId(userId);
    }

    public async Task AddProfilePhoto(ProfilePhotoUploadDTO profilePhoto)
    {
        if (string.IsNullOrWhiteSpace(profilePhoto.UserId))
        {
            throw new ArgumentException("Profile photo entity already exists.");
        }

        if (profilePhoto.Photo == null || profilePhoto.Photo.Length == 0)
        {
            throw new ArgumentException("Photo data is required.");
        }

        // isn't this an unnecessary duplicate check for an already existing photo for a user?
        var existingPhoto = await _profilePhotoRepository.GetByUserId(profilePhoto.UserId);

        if (existingPhoto != null)
        {
            throw new InvalidOperationException("A profile photo already exists for this user.");
        }

        var entity = new ProfilePhotoEntity
        {
            UserId = profilePhoto.UserId,
            Photo = await ConvertFormFileToByteArray(profilePhoto.Photo),
            ContentType = profilePhoto.Photo.ContentType
        };
        
        await _profilePhotoRepository.AddProfilePhoto(entity);
    }

    public async Task UpdateProfilePhoto(ProfilePhotoUploadDTO profilePhoto)
    {
        if (string.IsNullOrWhiteSpace(profilePhoto.UserId))
        {
            throw new ArgumentException("Invalid user ID.");
        }

        if (profilePhoto.Photo == null || profilePhoto.Photo.Length == 0)
        {
            throw new ArgumentException("Photo data is required.");
        }

        var existingPhoto = await _profilePhotoRepository.GetByUserId(profilePhoto.UserId);
        if (existingPhoto == null)
        {
            throw new InvalidOperationException("No existing photo found for this user.");
        }

        existingPhoto.Photo = await ConvertFormFileToByteArray(profilePhoto.Photo);
        existingPhoto.ContentType = profilePhoto.Photo.ContentType;
        
        await _profilePhotoRepository.UpdateProfilePhoto(existingPhoto);
    }

    public async Task DeleteProfilePhoto(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new ArgumentException("Invalid ID.");
        }

        await _profilePhotoRepository.DeleteProfilePhoto(userId);
    }

    private ProfilePhotoEntity ConvertDtoToEntity(ProfilePhotoDTO dto)
    {
        ProfilePhotoEntity entity = new ProfilePhotoEntity
        {
            UserId = dto.UserId,
            Photo = dto.Photo,
            ContentType = dto.ContentType
        };
        return entity;
    }
    
    private async Task<byte[]> ConvertFormFileToByteArray(IFormFile file)
    {
        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }
}