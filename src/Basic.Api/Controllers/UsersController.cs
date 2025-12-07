using Microsoft.AspNetCore.Mvc;
using UserDto = BasicApp.Models.Dtos.UserDto;
//using BasicApp.Models.Dtos;
using BasicApp.Infrastructure.Services.Interfaces;

namespace BasicApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    // GET: api/users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
    {
        var dtos = await _userService.GetAllAsync();
        return Ok(dtos);
    }

    // GET: api/users/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDto>> GetById(int id)
    {
        var dto = await _userService.GetByIdAsync(id);
        if (dto == null)
        {
            return NotFound();
        }
        return Ok(dto);
    }

    // POST: api/users
    [HttpPost]
    public async Task<ActionResult<UserDto>> Create([FromBody] UserDto request)
    {
        var dto = await _userService.AddAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
    }
}
