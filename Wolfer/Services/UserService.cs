using Wolfer.Data.DTOs;
using Wolfer.Data.Entities;
using Wolfer.Repositories;

namespace Wolfer.Services;

public class UserService : IUserService
{
    private IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserEntity> GetById(int id)
    {
        // if (id <= 0)
        // {
        //     throw new ArgumentException("ID must be positive integer.");
        // }

        return await _userRepository.GetUserById();
    }

    public async Task<UserEntity> GetByFirstName(string firstName)
    {
        if (string.IsNullOrEmpty(firstName))
        {
            throw new ArgumentException("First name cannot by empty.");
        }

        return await _userRepository.GetUserByFirstName(firstName);
    }

    public async Task<UserEntity> GetByUserName(string userName)
    {
        if (string.IsNullOrEmpty(userName))
        {
            throw new ArgumentException("Username cannot by empty.");
        }

        return await _userRepository.GetUserByUserName(userName);
    }

    public async Task CreateUser(UserDTO userDto)
    {
        // if (userDto.Id != 0)
        // {
        //     throw new ArgumentException("User ID must be null.");
        // }
        
        if (userDto.FirstName == "" || userDto.LastName == "" || userDto.Email == "" ||
            userDto.Password == "")
        {
            throw new ArgumentException("All properties must be filled out.");
        }

        UserEntity newUserEntity = ConvertDtoToEntity(userDto);

        await _userRepository.CreateUser(newUserEntity);
    }

    public async Task UpdateUser(UserDTO userDto)
    {
        if (!await _userRepository.IsUserPresent(userDto.Id))
        {
            throw new ArgumentException("Invalid ID.");
        }
        
        if (userDto.FirstName == "" || userDto.LastName == "" || userDto.Email == "" ||
            userDto.Password == "")
        {
            throw new ArgumentException("All properties must be filled out.");
        }
        
        UserEntity newUserEntity = ConvertDtoToEntity(userDto);

        await _userRepository.UpdateUser(newUserEntity);
    }

    public async Task DeleteUser(int userId)
    {
        // if (userId <= 0)
        // {
        //     throw new ArgumentException("Invalid ID."); 
        // }

        await _userRepository.DeleteUserById(userId);
    }

    private UserEntity ConvertDtoToEntity(UserDTO userDto)
    {
        UserEntity userEntity = new UserEntity
        {
            FirstName = userDto.FirstName,
            LastName = userDto.LastName,
            Username = userDto.Username,
            Email = userDto.Email,
            Password = userDto.Password,
            Id = userDto.Id
        };
        return userEntity;
    }
}