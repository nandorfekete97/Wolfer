using Wolfer.Data.Entities;

namespace Wolfer.Services;

public interface IUserTrainingService
{
    public Task<List<TrainingEntity>> GetByUserId(Guid userId);
    public Task<List<UserEntity>> GetByTrainingId(int trainingId);
    Task<List<TrainingEntity>> GetPastTrainingsByUserId(Guid userId);
    Task SignUpUserToTraining(Guid userId, int trainingId);
}