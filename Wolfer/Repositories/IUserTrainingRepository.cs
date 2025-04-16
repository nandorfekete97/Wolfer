using Wolfer.Data.Entities;

namespace Wolfer.Repositories;

public interface IUserTrainingRepository
{
    Task<List<UserTrainingEntity>> GetByUserId(int userId);
    Task<List<UserTrainingEntity>> GetByTrainingId(int trainingId);
}