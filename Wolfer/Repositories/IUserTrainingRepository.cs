using Wolfer.Data.Entities;

namespace Wolfer.Repositories;

public interface IUserTrainingRepository
{
    Task<List<UserTrainingEntity>> GetByUserId(string userId);
    Task<List<UserTrainingEntity>> GetByTrainingId(int trainingId);
    Task<UserTrainingEntity> GetByUserIdAndTrainingId(string userId, int trainingId);
    Task Create(UserTrainingEntity userTrainingEntity);
    Task<bool> Delete(Guid userId, int trainingId);
}