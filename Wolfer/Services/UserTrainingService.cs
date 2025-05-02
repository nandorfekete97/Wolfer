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

    public async Task<List<TrainingEntity>> GetByUserId(int userId)
    {
        if (userId <= 0)
        {
            throw new ArgumentException("ID must be positive integer.");
        }

        UserEntity user = await _userRepository.GetUserById(userId);

        if (user == null)
        {
            throw new ArgumentException("Invalid ID.");
        }
        
        List<UserTrainingEntity> userTrainingEntities = await _userTrainingRepository.GetByUserId(userId);
        
        List<int> trainingIds = userTrainingEntities.Select(entity => entity.TrainingId).ToList();

        return await _trainingRepository.GetByIds(trainingIds);
    }
    
    public async Task<List<TrainingEntity>> GetPastTrainingsByUserId(int userId)
    {
        if (userId <= 0)
        {
            throw new ArgumentException("ID must be positive integer.");
        }

        UserEntity user = await _userRepository.GetUserById(userId);

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

    public async Task<List<UserEntity>> GetByTrainingId(int trainingId)
    {
        if (trainingId <= 0)
        {
            throw new ArgumentException("ID must be positive integer.");
        }

        TrainingEntity training = await _trainingRepository.GetById(trainingId);

        if (training == null)
        {
            throw new ArgumentException("Invalid ID.");
        }

        List<UserTrainingEntity> userTrainingEntities = await _userTrainingRepository.GetByTrainingId(trainingId);

        List<int> userIds = userTrainingEntities.Select(entity => entity.UserId).ToList();

        return await _userRepository.GetByIds(userIds);
    }
    
    public async Task SignUpUserToTraining(int userId, int trainingId)
    {
        if (userId <= 0 || trainingId <= 0)
            throw new ArgumentException("User ID and Training ID must be valid.");

        var user = await _userRepository.GetUserById(userId);
        var training = await _trainingRepository.GetById(trainingId);

        if (user == null || training == null)
            throw new ArgumentException("User or Training not found.");

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
}