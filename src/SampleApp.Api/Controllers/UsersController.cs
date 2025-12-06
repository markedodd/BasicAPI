using Microsoft.AspNetCore.Mvc;
using BasicApp.Api.Mapping;
using BasicApp.Api.Models.Dtos;
using BasicApp.Infrastructure.Repositories.Interfaces;
using BasicApp.Models.Entities;

namespace BasicApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UsersController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    // GET: api/users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
    {
        var users = await _userRepository.GetAllAsync();

        var dtos = users
            .Select(u => SimpleMapper.Map<User, UserDto>(u))
            .ToList();

        return Ok(dtos);
    }

    // GET: api/users/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserDto>> GetById(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        var dto = SimpleMapper.Map<User, UserDto>(user);
        return Ok(dto);
    }

    // POST: api/users
    [HttpPost]
    public async Task<ActionResult<UserDto>> Create([FromBody] UserDto request)
    {
        var entity = new User
        {
            Id = request.Id == Guid.Empty ? Guid.NewGuid() : request.Id,
            Name = request.Name,
            Email = request.Email
        };

        var created = await _userRepository.AddAsync(entity);
        var dto = SimpleMapper.Map<User, UserDto>(created);

        return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
    }
}
