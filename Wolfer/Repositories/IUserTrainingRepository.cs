using Wolfer.Data.Entities;

namespace Wolfer.Repositories;

public interface IUserTrainingRepository
{
    Task<List<UserTrainingEntity>> GetByUserId(int userId);
    Task<List<UserTrainingEntity>> GetByTrainingId(int trainingId);
    Task<UserTrainingEntity> GetByUserIdAndTrainingId(int userId, int trainingId);
    Task Create(UserTrainingEntity userTrainingEntity);
}