using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Dto;
using MyBlog.Handlers;
using MyBlog.Interfaces;
using MyBlog.Models;
using MyBlog_API.Handlers;

namespace MyBlog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthController(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        // Đăng ký tài khoản mới
        [HttpPost("register")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public IActionResult Register([FromBody] RegisterDto registerDto)
        {
            if (registerDto == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid registration request");
            }

            // Kiểm tra người dùng đã tồn tại
            if (_userRepository.GetUserByName(registerDto.Username) != null)
            {
                return BadRequest("Username already exists");
            }

            // Mã hóa mật khẩu
            PasswordHashHandler.CreatePasswordHash(registerDto.Password, out byte[] passwordHash, out byte[] passwordSalt);


            // Tạo người dùng mới
            var user = new User
            {
                UserName = registerDto.Username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                CreatedAt = DateTime.UtcNow
            };

            // Lưu người dùng vào cơ sở dữ liệu
            if (!_userRepository.CreateUser(user))
            {

                return StatusCode(500, "An error occurred while creating the user");
            }

            return StatusCode(201, "User registered successfully");
        }

        // Đăng nhập
        [HttpPost("login")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public object Login([FromBody] LoginDto loginDto)
        {
            if (loginDto == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid login request");
            }

            // Kiểm tra người dùng có tồn tại
            var user = _userRepository.GetUserByName(loginDto.Username);
            if (user == null || !PasswordHashHandler.VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt))
            {
                return Unauthorized("Invalid username or password");
            }

            // Tạo JWT token
            var tokenHandler = new TokenHandler(_configuration);
            var token = tokenHandler.GenerateJwtToken(user);

            return new
            {
                UserName = loginDto.Username,
                AccessToken = token
            };
        }
    }

}

