using Wolfer.Data.DTOs;
using Wolfer.Data.Entities;

namespace Wolfer.Services;

public interface IUserService
{
    public Task<UserEntity> GetById(int id);
    public Task<UserEntity> GetByFirstName(string firstName);
    public Task<UserEntity> GetByUserName(string userName);
    public Task CreateUser(UserDTO userDto);
    public Task UpdateUser(UserDTO userDto);
    public Task DeleteUser(int id);
}