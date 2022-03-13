﻿
namespace Movies.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public AuthController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto dto)
    {
        if(!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _unitOfWork.Auth.RegisterAsync(dto);

        if (!user.IsAuthenticated)
            return BadRequest(user.Message);

        return Ok(user);
    }

    [HttpPost("token")]
    public async Task<IActionResult> GetTokenAsync([FromBody] TokenRequestDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _unitOfWork.Auth.GetTokenAsync(dto);

        if (!user.IsAuthenticated)
            return BadRequest(user.Message);

        return Ok(user);
    }

    [HttpPost("addrole"), Authorize(Roles = SD.Role_Admin)]
    public async Task<IActionResult> AddRoleAsync([FromBody] AddRoleDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _unitOfWork.Auth.AddRoleAsync(dto);

        if (!string.IsNullOrEmpty(result))
            return BadRequest(result);

        return Ok(dto);
    }
}
