using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos;
using WebLearning.Contract.Dtos.Login;
using WebLearning.Persistence.ApplicationContext;

namespace WebLearning.Application.ELearning.Services
{
    public interface ILoginService
    {
        Task<string> Login(LoginDto loginDto);

    }
    public class LoginService : ILoginService
    {
        private readonly WebLearningContext _context;
        private readonly AppSetting _appSettings;

        public LoginService(WebLearningContext context, IMapper mapper, IOptionsMonitor<AppSetting> optionsMonitor)
        {
            _context = context;
            _appSettings = optionsMonitor.CurrentValue;
        }

        public async Task<string> Login(LoginDto loginDto)
        {
            string password = Password.HashedPassword(loginDto.Password);

            var account = await _context.Accounts.Include(x => x.Avatar).SingleOrDefaultAsync(x => x.Email == loginDto.UserName && password == x.PasswordHased);

            if (account != null && account.Active == 1)
            {

                var jwtTokenHandler = new JwtSecurityTokenHandler();

                var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);


                var tokenDescription = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] {
                        new Claim(ClaimTypes.Email, account.Email),
                        new Claim(ClaimTypes.Role, account.AuthorizeRole.ToString()),
                        new Claim(ClaimTypes.Name, account.Email),
                    }),

                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha512Signature)

                };

                account.LastLogin = DateTime.Now;

                _context.Accounts.Update(account);

                await _context.SaveChangesAsync();

                var token = jwtTokenHandler.CreateToken(tokenDescription);

                //var accessToken = jwtTokenHandler.WriteToken(token);

                //var refreshToken =  GenerateRefreshToken();

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            else
            {
                return default;
            }

        }
        //public async Task<TokenModel> GenerateToken(Account account, RoleDto role)
        //{
        //    var jwtTokenHandler = new JwtSecurityTokenHandler();

        //    var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);

        //    var tokenDescription = new SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(new[] {
        //            new Claim(ClaimTypes.Name, account.Email),
        //            new Claim(JwtRegisteredClaimNames.Email, account.Email),
        //            new Claim(JwtRegisteredClaimNames.Sub, account.Email),
        //            new Claim(ClaimTypes.Role, role.RoleName),
        //            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //            new Claim("UserName", account.Email),
        //            new Claim("Id", account.Id.ToString()),

        //            //roles
        //        }),
        //        Expires = DateTime.UtcNow.AddMinutes(5),
        //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha512Signature)
        //    };

        //    var token = jwtTokenHandler.CreateToken(tokenDescription);
        //    var accessToken = jwtTokenHandler.WriteToken(token);
        //    var refreshToken = GenerateRefreshToken();

        //    //Lưu database
        //    var refreshTokenEntity = new RefreshToken
        //    {
        //        Id = Guid.NewGuid(),
        //        JwtId = token.Id,
        //        UserId = account.Id,
        //        Token = refreshToken,
        //        IsUsed = false,
        //        IsRevoked = false,
        //        IssuedAt = DateTime.UtcNow,
        //        ExpiredAt = DateTime.UtcNow.AddHours(1)
        //    };

        //    await _context.AddAsync(refreshTokenEntity);
        //    await _context.SaveChangesAsync();

        //    return new TokenModel
        //    {
        //        AccessToken = accessToken,
        //        RefreshToken = refreshToken
        //    };
        //}

        //public async Task<ApiResponse> RenewToken(TokenModel tokenModel)
        //{
        //    var jwtTokenHandler = new JwtSecurityTokenHandler();
        //    var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);
        //    var tokenValidateParam = new TokenValidationParameters
        //    {
        //        //tự cấp token
        //        ValidateIssuer = false,
        //        ValidateAudience = false,

        //        //ký vào token
        //        ValidateIssuerSigningKey = true,
        //        IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),

        //        ClockSkew = TimeSpan.Zero,

        //        ValidateLifetime = false //ko kiểm tra token hết hạn
        //    };
        //    try
        //    {
        //        //check 1: AccessToken valid format
        //        var tokenInVerification = jwtTokenHandler.ValidateToken(tokenModel.AccessToken, tokenValidateParam, out var validatedToken);

        //        //check 2: Check alg
        //        if (validatedToken is JwtSecurityToken jwtSecurityToken)
        //        {
        //            var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase);
        //            if (!result)//false
        //            {

        //                return (new ApiResponse
        //                {
        //                    Success = false,
        //                    Message = "Invalid token"
        //                });

        //            }
        //        }

        //        //check 3: Check accessToken expire?
        //        var utcExpireDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

        //        var expireDate = ConvertUnixTimeToDateTime(utcExpireDate);
        //        if (expireDate > DateTime.UtcNow)
        //        {
        //            return (new ApiResponse
        //            {
        //                Success = false,
        //                Message = "Access token has not yet expired"
        //            });
        //        }

        //        //check 4: Check refreshtoken exist in DB
        //        var storedToken = _context.RefreshTokens.FirstOrDefault(x => x.Token == tokenModel.RefreshToken);
        //        if (storedToken == null)
        //        {
        //            return (new ApiResponse
        //            {
        //                Success = false,
        //                Message = "Refresh token does not exist"
        //            });
        //        }

        //        //check 5: check refreshToken is used/revoked?
        //        if (storedToken.IsUsed)
        //        {
        //            return (new ApiResponse
        //            {
        //                Success = false,
        //                Message = "Refresh token has been used"
        //            });
        //        }
        //        if (storedToken.IsRevoked)
        //        {
        //            return (new ApiResponse
        //            {
        //                Success = false,
        //                Message = "Refresh token has been revoked"
        //            });
        //        }

        //        //check 6: AccessToken id == JwtId in RefreshToken
        //        var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
        //        if (storedToken.JwtId != jti)
        //        {
        //            return (new ApiResponse
        //            {
        //                Success = false,
        //                Message = "Token doesn't match"
        //            });
        //        }

        //        //Update token is used
        //        storedToken.IsRevoked = true;
        //        storedToken.IsUsed = true;
        //        _context.Update(storedToken);
        //        await _context.SaveChangesAsync();

        //        //create new token
        //        var account = await _context.Accounts.SingleOrDefaultAsync(nd => nd.Id == storedToken.UserId);

        //        var role = _context.Roles.SingleOrDefault(x => x.Id == account.RoleId);

        //        var roleDto = _mapper.Map<RoleDto>(role);

        //        var token = await GenerateToken(account, roleDto);

        //        return (new ApiResponse
        //        {
        //            Success = true,
        //            Message = "Renew token success",
        //            Data = token
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return (new ApiResponse
        //        {
        //            Success = false,
        //            Message = ex.Message
        //        });
        //    }
        //}
        //public string GenerateRefreshToken()
        //{
        //    var random = new byte[32];
        //    using (var rng = RandomNumberGenerator.Create())
        //    {
        //        rng.GetBytes(random);

        //        return Convert.ToBase64String(random);
        //    }
        //}
        //private DateTime ConvertUnixTimeToDateTime(long utcExpireDate)
        //{
        //    var dateTimeInterval = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        //    dateTimeInterval.AddSeconds(utcExpireDate).ToUniversalTime();

        //    return dateTimeInterval;
        //}

    }
}
