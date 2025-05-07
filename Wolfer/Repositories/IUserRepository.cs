using Wolfer.Data.Entities;

namespace Wolfer.Repositories;

public interface IUserRepository
{
    Task<UserEntity?> GetUserById();
    Task<UserEntity?> GetUserByFirstName(string firstName);
    Task<List<UserEntity>> GetByIds();
    Task<UserEntity?> GetUserByUserName(string userName);
    Task CreateUser(UserEntity user);
    Task UpdateUser(UserEntity user);
    Task<bool> DeleteUserById(int id);
    Task<bool> IsUserPresent(int id);
}