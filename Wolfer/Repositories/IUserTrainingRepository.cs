using Wolfer.Data.Entities;

namespace Wolfer.Repositories;

public interface IUserTrainingRepository
{
    Task<List<UserTrainingEntity>> GetByUserId(Guid userId);
    Task<List<UserTrainingEntity>> GetByTrainingId(int trainingId);
    Task<UserTrainingEntity> GetByUserIdAndTrainingId(Guid userId, int trainingId);
    Task Create(UserTrainingEntity userTrainingEntity);
    Task<bool> Delete(Guid userId, int trainingId);
    Task<bool> DeleteByTrainingId (int trainingId);
}