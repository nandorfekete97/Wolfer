using Wolfer.Data.Entities;

namespace Wolfer.Repositories;

public interface IUserRepository
{
    Task<UserEntity?> GetUserById(int id);
    Task<UserEntity?> GetUserByFirstName(string firstName);
    Task<List<UserEntity>> GetByIds(List<int> userIds);
    Task<UserEntity?> GetUserByUserName(string userName);
    Task CreateUser(UserEntity user);
    Task UpdateUser(UserEntity user);
    Task<bool> DeleteUserById(int id);
    Task<bool> IsUserPresent(int id);
}