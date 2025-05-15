using Microsoft.AspNetCore.Identity;
using Wolfer.Data.Entities;

namespace Wolfer.Services;

public interface IUserTrainingService
{
    public Task<List<TrainingEntity>> GetByUserId(string userId);
    public Task<List<IdentityUser>> GetByTrainingId(int trainingId);
    Task<List<TrainingEntity>> GetPastTrainingsByUserId(string userId);
    Task SignUpUserToTraining(string userId, int trainingId);
}