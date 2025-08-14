using Microsoft.EntityFrameworkCore;
using Wolfer.Data.Context;
using Wolfer.Data.Entities;

namespace Wolfer.Repositories;

public class ProfilePhotoRepository : IProfilePhotoRepository
{
    private WolferContext _dbContext;

    public ProfilePhotoRepository(WolferContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ProfilePhotoEntity> GetByUserId(string userId)
    {
        return await _dbContext.ProfilePhotos.FirstOrDefaultAsync(entity => entity.UserId == userId);
    }

    public async Task AddProfilePhoto(ProfilePhotoEntity profilePhoto)
    {
        await _dbContext.ProfilePhotos.AddAsync(profilePhoto);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateProfilePhoto(ProfilePhotoEntity profilePhoto)
    {
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> DeleteProfilePhoto(string userId)
    {
        var profilePhotoToDelete =
            await _dbContext.ProfilePhotos.FirstOrDefaultAsync(entity => entity.UserId == userId);

        if (profilePhotoToDelete is not null)
        {
            _dbContext.Remove(profilePhotoToDelete);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        return false;
    }
}