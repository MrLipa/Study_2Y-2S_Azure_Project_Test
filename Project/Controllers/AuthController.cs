using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Project.Interfaces;
using Project.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static AppUser user = new AppUser();
        private readonly IConfiguration _configuration;
        private readonly IAppUserRepository _appUserRepository;

        public AuthController(IConfiguration configuration, IAppUserRepository appUserRepository)
        {
            _configuration = configuration;
            _appUserRepository = appUserRepository;
        }

        [HttpPost("register")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult Register(Register request)
        {
            var existingUser = _appUserRepository.GetAppUserByEmail(request.Email);
            if (existingUser != null)
            {
                return BadRequest("User already exists.");
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var newUser = new AppUser
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = passwordHash,
                CreatedAt = DateTime.UtcNow
            };

            _appUserRepository.AddAppUser(newUser);

            return Ok("User registered successfully.");
        }

        [HttpPost("login")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400)]
        public IActionResult Login(Login request)
        {
            var user = _appUserRepository.GetAppUserByEmail(request.Email);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return BadRequest("Wrong password.");
            }

            var accessToken = CreateToken(user, new List<string> { "Admin" }, TimeSpan.FromSeconds(_configuration.GetValue<int>("AppSettings:AccessTokenExpiryInSeconds")), _configuration["AppSettings:AccessToken"]);
            var refreshToken = CreateToken(user, new List<string> { "Admin" }, TimeSpan.FromDays(_configuration.GetValue<int>("AppSettings:RefreshTokenExpiryInDays")), keyString: _configuration["AppSettings:RefreshToken"]);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.Now.AddDays(_configuration.GetValue<int>("AppSettings:RefreshTokenExpiryInDays")),
            };
            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);

            user.RefreshToken = refreshToken;
            _appUserRepository.UpdateAppUser(user);

            return Ok(new { AccessToken = accessToken, RefreshToken = refreshToken });
        }

        [HttpPost("refresh")]
        public IActionResult RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
            {
                return BadRequest("No refresh token provided.");
            }
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(refreshToken, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AppSettings:RefreshToken"])),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userEmail = jwtToken.Claims.First(x => x.Type == ClaimTypes.Email).Value;

                var user = _appUserRepository.GetAppUserByEmail(userEmail);

                if (user == null)
                {
                    return BadRequest("Invalid refresh token.");
                }

                var newAccessToken = CreateToken(user, new List<string> { "Admin" }, TimeSpan.FromSeconds(_configuration.GetValue<int>("AppSettings:AccessTokenExpiryInSeconds")), _configuration["AppSettings:AccessToken"]);
                var newRefreshToken = CreateToken(user, new List<string> { "Admin" }, TimeSpan.FromDays(_configuration.GetValue<int>("AppSettings:RefreshTokenExpiryInDays")), _configuration["AppSettings:RefreshToken"]);

                user.RefreshToken = newRefreshToken;
                _appUserRepository.UpdateAppUser(user);

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.Now.AddDays(_configuration.GetValue<int>("AppSettings:RefreshTokenExpiryInDays")),
                };
                Response.Cookies.Append("refreshToken", newRefreshToken, cookieOptions);

                return Ok(new { AccessToken = newAccessToken });
            } 
            catch
            {
                return BadRequest("Invalid refresh token.");
            }
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
            {
                return BadRequest("No refresh token provided.");
            }
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(refreshToken, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AppSettings:RefreshToken"])),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userEmail = jwtToken.Claims.First(x => x.Type == ClaimTypes.Email).Value;

                var user = _appUserRepository.GetAppUserByEmail(userEmail);

                if (user != null)
                {
                    user.RefreshToken = null;
                    _appUserRepository.UpdateAppUser(user);
                }

                Response.Cookies.Delete("refreshToken");
                return Ok("Logged out successfully.");
            }
            catch
            {
                return BadRequest("Invalid refresh token.");
            }
        }
        
        private string CreateToken(AppUser user, IEnumerable<string> roles, TimeSpan expireTime, string keyString)
        {
            List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.Add(expireTime),
                    signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
