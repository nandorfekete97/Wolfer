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
}