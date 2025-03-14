using Wolfer.Data.Entities;

namespace Wolfer.Repositories;

public interface IUserTrainingRepository
{
    Task<UserTrainingsEntity> GetByUserId(int userId);
    Task<UserTrainingsEntity> GetByTrainingId(int trainingId);
}