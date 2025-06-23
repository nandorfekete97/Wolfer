using Microsoft.AspNetCore.Identity;
using Wolfer.Data.Entities;

namespace Wolfer.Services;

public interface IUserTrainingService
{
    public Task<List<TrainingEntity>> GetFutureTrainingsByUserId(Guid userId);
    public Task<List<IdentityUser>> GetByTrainingId(int trainingId);
    Task<List<TrainingEntity>> GetPastTrainingsByUserId(Guid userId);
    Task SignUpUserToTraining(Guid userId, int trainingId);
    Task SignOffUserFromTraining(Guid userId, int trainingId);
}