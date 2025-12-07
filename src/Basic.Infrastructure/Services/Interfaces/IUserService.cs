using BasicApp.Models.Dtos;

namespace BasicApp.Infrastructure.Services.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllAsync();
    Task<UserDto?> GetByIdAsync(int id);
    Task<UserDto> AddAsync(UserDto userDto);
    //Task AddAsync(UserDto request);
}
