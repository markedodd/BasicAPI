using BasicApp.Models.Dtos;
using BasicApp.Infrastructure.Repositories.Interfaces;
using BasicApp.Models.Entities;
using BasicApp.Infrastructure.Services.Interfaces;

namespace BasicApp.Infrastructure.Services.Implementations;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return users.Select(u => new UserDto
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email
        });
    }

    public async Task<UserDto?> GetByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return null;
        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        };
    }

    public async Task<UserDto> AddAsync(UserDto userDto)
    {
        var entity = new User
        {
            Id = userDto.Id,
            Name = userDto.Name,
            Email = userDto.Email
        };
        var created = await _userRepository.AddAsync(entity);
        return new UserDto
        {
            Id = created.Id,
            Name = created.Name,
            Email = created.Email
        };
    }
}
