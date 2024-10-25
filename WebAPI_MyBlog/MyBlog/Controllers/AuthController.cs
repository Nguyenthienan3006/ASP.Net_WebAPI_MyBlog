using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.IdentityModel.Tokens;
using MyBlog.Dto;
using MyBlog.Handlers;
using MyBlog.Interfaces;
using MyBlog.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MyBlog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;

        public AuthController(IConfiguration configuration, IUserRepository userRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;
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
        public IActionResult Login([FromBody] LoginDto loginDto)
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
            var token = GenerateJwtToken(user);

            return Ok(new { token });
        }

        // Phương thức tạo JWT token
        private string GenerateJwtToken(User user)
        {
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var key = _configuration["Jwt:Key"];
            var tokenValidityMins = _configuration.GetValue<int>("Jwt:TokenValidityMins");
            var tokenExpiryTimeStamp = DateTime.UtcNow.AddMinutes(tokenValidityMins);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Name, user.UserName)
                }),
                Issuer = issuer,
                Expires = tokenExpiryTimeStamp,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                SecurityAlgorithms.HmacSha512Signature),
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(securityToken);

            return accessToken;

        }
    }

}

