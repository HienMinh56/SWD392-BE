using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SWD392_BE.Repositories.Entities;
using SWD392_BE.Repositories.ViewModels.ResultModel;
using SWD392_BE.Repositories.ViewModels.TokenViewModel;
using SWD392_BE.Services.Interfaces;
using SWD392_BE.Services.Sercurity;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SWD392_BE.API.Controllers
{
    [Route("api/v1/authorize")]
    [ApiController]
    [EnableCors("app-cors")]
    public class AuthorizeController : ControllerBase
    {
        private readonly IUserService _userServices;
        private readonly IRefreshTokenService _refreshHandler;
        private readonly IConfiguration _configuration;

        //private readonly IRefreshHandler refresh;

        public AuthorizeController(IUserService userServices, IRefreshTokenService refreshHandler, IConfiguration configuration)
        {
            _userServices = userServices;
            _refreshHandler = refreshHandler;
            _configuration = configuration;
        }
        #region GenerateToken
        /// <summary>
        /// Which will generating token accessible for user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [NonAction]
        public TokenViewModel GenerateToken(User user, String? RT)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim("UserId", user.UserId.ToString()),
                new Claim("UserName", user.Name),
                new Claim("Email", user.Email),
                new Claim("Role", user.Role.ToString())
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("c2VydmVwZXJmZWN0bHljaGVlc2VxdWlja2NvYWNoY29sbGVjdHNsb3Bld2lzZWNhbWU="));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "YourIssuer",
                audience: "YourAudience",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            if (RT != null)
            {
                return new TokenViewModel()
                {
                    AccessTokenToken = accessToken,
                    RefreshToken = RT,
                    ExpiredAt = _refreshHandler.GetRefreshTokenByUserID(user.UserId).ExpiredTime
                };
            }
            return new TokenViewModel()
            {
                AccessTokenToken = accessToken,
                RefreshToken = GenerateRefreshToken(user),
                ExpiredAt = _refreshHandler.GetRefreshTokenByUserID(user.UserId).ExpiredTime
            };
        }
        #endregion

        #region GenerateRefreshToken
        // Hàm tạo refresh token
        [NonAction]
        public string GenerateRefreshToken(User user)
        {
            var randomnumber = new byte[32];
            using (var randomnumbergenerator = RandomNumberGenerator.Create())
            {
                randomnumbergenerator.GetBytes(randomnumber);
                string refreshtoken = Convert.ToBase64String(randomnumber);
                var refreshTokenEntity = new Token
                {
                    UserId = user.UserId,
                    AccessToken = new Random().Next().ToString(),
                    RefreshToken = refreshtoken.ToString(),
                    ExpiredTime = DateTime.Now.AddDays(7),
                    Status = 1
                };

                _refreshHandler.GenerateRefreshToken(refreshTokenEntity);
                return refreshtoken;
            }
        }
        #endregion

        #region RefreshAccessToken
        /// <summary>
        /// Refresh Token
        /// </summary>
        /// <returns>New Token and Refresh Token</returns>
        [HttpPost("refresh-access-token")]
        public async Task<ActionResult> RefreshAccessToken(TokenViewModel token)
        {
            try
            {
                var jwtTokenHander = new JwtSecurityTokenHandler();
                var TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("c2VydmVwZXJmZWN0bHljaGVlc2VxdWlja2NvYWNoY29sbGVjdHNsb3Bld2lzZWNhbWU=")),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero,
                    ValidateLifetime = false
                };
                //ResetRefreshToken in DB if token is disable or expired will Remove RT
                _refreshHandler.ResetRefreshToken();
                //check validate of Parameter
                var tokenVerification = jwtTokenHander.ValidateToken(token.AccessTokenToken, TokenValidationParameters, out var validatedToken);
                if (tokenVerification == null)
                {
                    return Ok(new ResultModel
                    {
                        IsSuccess = false,
                        Message = "Invalid Param"
                    });
                }
                //check AccessToken expire?
                var epochTime = long.Parse(tokenVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
                DateTimeOffset dateTimeUtc = DateTimeOffset.FromUnixTimeSeconds(epochTime);
                DateTime dateTimeUtcConverted = dateTimeUtc.UtcDateTime;
                if (dateTimeUtcConverted > DateTime.UtcNow)
                {
                    return Ok(new ResultModel
                    {
                        IsSuccess = false,
                        Message = "AccessToken had not expired",
                        Data = "Expire time: " + dateTimeUtcConverted.ToString()
                    });
                }
                //check RefreshToken exist in DB
                var storedToken = _refreshHandler.GetRefreshToken(token.RefreshToken);
                if (storedToken == null)
                {
                    return Ok(new ResultModel
                    {
                        IsSuccess = false,
                        Message = "RefreshToken had not existed"
                    });
                }
                //check RefreshToken is revoked?
                if (storedToken.Status == 2)
                {
                    return Ok(new ResultModel
                    {
                        IsSuccess = false,
                        Message = "RefreshToken had been revoked"
                    });
                }
                var User = _userServices.GetUserById(storedToken.UserId);
                var newAT = GenerateToken(User, token.RefreshToken);

                return Ok(new ResultModel
                {
                    IsSuccess = true,
                    Message = "Refresh AT success fully",
                    Data = newAT
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultModel
                {
                    IsSuccess = false,
                    Code = 500,
                    Message = "Something go wrong"
                });
            }
        }
        #endregion

        #region Login
        /// <summary>
        /// Login into system
        /// </summary>
        /// <returns>Acces Token and Refresh Token</returns>
        [HttpPost]
        [Route("login")]
        public IActionResult Login(string userName, string password)
        {
            var user = _userServices.GetUserByUserName(userName);
            if (user != null && user.Status == 1)
            {
                // Hash the input password with SHA256
                var hashedInputPasswordString = PasswordHasher.HashPassword(password);

                // Compare the hashed input password with the stored hashed password
                if (hashedInputPasswordString == user.Password)
                {
                    _refreshHandler.ResetRefreshToken();
                    var token = GenerateToken(user, null);
                    return Ok(token);
                }
            }
            return BadRequest(new ResultModel
            {
                IsSuccess = false,
                Message = "Status Code:401 Unauthorized",
                Data = null
            });
        }
        #endregion

        #region Logout
        [HttpPost]
        [Route("logout")]
        public IActionResult Logout()
        {
            try
            {
                string token = HttpContext.Request.Headers["Authorization"];
                token = token.Split(' ')[1];
                var tokenHandler = new JwtSecurityTokenHandler();
                var TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("c2VydmVwZXJmZWN0bHljaGVlc2VxdWlja2NvYWNoY29sbGVjdHNsb3Bld2lzZWNhbWU=")),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero,
                    ValidateLifetime = false
                };
                SecurityToken validatedToken;
                var claimsPrincipal = tokenHandler.ValidateToken(token, TokenValidationParameters, out validatedToken);
                var userIdClaim = claimsPrincipal.FindFirst("UserId");
                var _refreshToken = _refreshHandler.GetRefreshTokenByUserID(userIdClaim.Value);
                _refreshHandler.UpdateRefreshToken(_refreshToken);
                _refreshHandler.ResetRefreshToken();
                if (HttpContext.Request.Headers.ContainsKey("Authorization"))
                {
                    HttpContext.Request.Headers.Remove("Authorization");
                }
                return Ok(new ResultModel
                {
                    IsSuccess = true,
                    Message = "Logout succesfully!"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultModel
                {
                    IsSuccess = false,
                    Message = "Something go wrong" + ex.Message
                });
            }
        }
        #endregion

        #region Who Am I
        /// <summary>
        /// Check infor of user
        /// </summary>
        /// <returns>Infor of user</returns>
        [HttpGet("me")]
        public IActionResult WhoAmI()
        {
            // Kiểm tra xem người dùng đã được xác thực chưa
            if (User.Identity.IsAuthenticated)
            {
                // Lấy thông tin về người dùng từ claims
                var userIdClaim = User.FindFirst("UserId");
                var userNameClaim = User.FindFirst("UserName");
                var userEmailClaim = User.FindFirst("Email");
                var userRoleClaim = User.FindFirst("Role");

                // Kiểm tra xem các claim có tồn tại không
                if (userIdClaim != null && userNameClaim != null && userEmailClaim != null && userRoleClaim != null)
                {
                    var userId = userIdClaim.Value;
                    var userName = userNameClaim.Value;
                    var userEmail = userEmailClaim.Value;
                    var userRole = userRoleClaim.Value;

                    // Trả về thông tin của người dùng cùng với token
                    var user = new User
                    {
                        UserId = userId,
                        Name = userName,
                        Email = userEmail,
                        Role = int.Parse(userRole)
                    };

                    // Tạo token JWT cho người dùng


                    // Trả về thông tin của người dùng cùng với token
                    return Ok(new { UserId = userId, UserName = userName, Email = userEmail, Role = userRole });
                }
                else
                {
                    // Nếu thiếu thông tin trong claims, trả về lỗi 401 Unauthorized
                    return Unauthorized(new { Message = "Missing user information in claims" });
                }
            }
            else
            {
                // Trả về lỗi 401 Unauthorized nếu người dùng chưa được xác thực
                return Unauthorized();
            }
        }
        #endregion
    }
}
