using Microsoft.AspNetCore.Identity;
using Wolfer.Data.Entities;
using Wolfer.Repositories;

namespace Wolfer.Services;

public class UserTrainingService : IUserTrainingService
{
    private IUserTrainingRepository _userTrainingRepository;
    private IUserRepository _userRepository;
    private ITrainingRepository _trainingRepository;

    public UserTrainingService(IUserTrainingRepository userTrainingRepository, IUserRepository userRepository, ITrainingRepository trainingRepository)
    {
        _userTrainingRepository = userTrainingRepository;
        _userRepository = userRepository;
        _trainingRepository = trainingRepository;
    }

    public async Task<List<TrainingEntity>> GetFutureTrainingsByUserId(Guid userId)
    {
        IdentityUser user = await _userRepository.GetUserById(userId);

        if (user == null)
        {
            throw new ArgumentException("Invalid ID.");
        }
        
        List<UserTrainingEntity> userTrainingEntities = await _userTrainingRepository.GetByUserId(userId);
        
        List<int> trainingIds = userTrainingEntities.Select(entity => entity.TrainingId).ToList();

        List<TrainingEntity> trainingEntities = await _trainingRepository.GetByIds(trainingIds);

        return trainingEntities.Where(entity => DateOnly.FromDateTime(entity.Date) >= DateOnly.FromDateTime(DateTime.Now)).ToList();
    }
    
    public async Task<List<TrainingEntity>> GetPastTrainingsByUserId(Guid userId)
    {
        IdentityUser user = await _userRepository.GetUserById(userId);

        if (user == null)
        {
            throw new ArgumentException("Invalid ID.");
        }
    
        List<UserTrainingEntity> userTrainingEntities = await _userTrainingRepository.GetByUserId(userId);
    
        List<int> trainingIds = userTrainingEntities.Select(entity => entity.TrainingId).ToList();

        List<TrainingEntity> allTrainings = await _trainingRepository.GetByIds(trainingIds);
    
        var pastTrainings = allTrainings.Where(training => training.Date < DateTime.Now).ToList();
    
        return pastTrainings;
    }

    public async Task<List<IdentityUser>> GetByTrainingId(int trainingId)
    {
        if (trainingId <= 0)
        {
            throw new ArgumentException("ID must be positive integer.");
        }

        TrainingEntity training = await _trainingRepository.GetById(trainingId);

        if (training == null)
        {
            throw new ArgumentException("Invalid training ID.");
        }

        List<UserTrainingEntity> userTrainingEntities = await _userTrainingRepository.GetByTrainingId(trainingId);

        List<Guid> userIds = userTrainingEntities.Select(entity => entity.UserId).ToList();

        return await _userRepository.GetByIds(userIds);
    }
    
    public async Task SignUpUserToTraining(Guid userId, int trainingId)
    {
        var user = await _userRepository.GetUserById(userId);
        var training = await _trainingRepository.GetById(trainingId);

        if (user == null || training == null)
        {
            throw new ArgumentException("User or Training not found.");
        }

        var existing = await _userTrainingRepository.GetByUserIdAndTrainingId(userId, trainingId);
        if (existing != null)
            throw new InvalidOperationException("User already signed up for this training.");

        var userTraining = new UserTrainingEntity
        {
            UserId = userId,
            TrainingId = trainingId
        };

        await _userTrainingRepository.Create(userTraining);
    }

    public async Task SignOffUserFromTraining(Guid userId, int trainingId)
    {
        var userTraining = await _userTrainingRepository.GetByUserIdAndTrainingId(userId, trainingId);

        if (userTraining == null)
        {
            throw new ArgumentException("UserTraining record or Training not found.");
        }

        await _userTrainingRepository.Delete(userId, trainingId);
    }
}