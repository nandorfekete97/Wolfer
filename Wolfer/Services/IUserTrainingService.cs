using Wolfer.Data.Entities;

namespace Wolfer.Services;

public interface IUserTrainingService
{
    public Task<List<TrainingEntity>> GetByUserId(int userId);
    public Task<List<UserEntity>> GetByTrainingId(int trainingId);
}