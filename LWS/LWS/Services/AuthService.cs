using LWS.Data;
using LWS.Dto;
using LWS.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LWS.Services
{
    public class AuthService(AppDbContext _context):IAuthService
    {
        private string PasswordHashing(UserDto dto)
        {
            var passwordHasher = new PasswordHasher<string>();
            var hash = passwordHasher.HashPassword(dto.Email,dto.Password);
            return hash;
        }

        private string GetJwtToken(UserDto dto)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, dto.FirstName + " " + dto.LastName),
                new Claim(ClaimTypes.Email,dto.Email),
                new Claim(ClaimTypes.NameIdentifier,dto.Id.ToString())
            };

            var key= new SymmetricSecurityKey(Encoding.UTF8.GetBytes("h<#@B2SRZ<?M.@&/Aa^pfz.)0(PunsODR)!$*El$)qlhurFt{AOJG3$r!*y:e0F@"));

            var creds= new SigningCredentials(key, SecurityAlgorithms.HmacSha384);

            var token = new JwtSecurityToken(
                issuer:"LWS",
                audience:"LWS",
                claims:claims,
                expires:DateTime.Now.AddMinutes(1),
                signingCredentials:creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public async Task<Tuple<int,TokenDto>> LoginUser(UserDto dto)
        {
            try
            {
                var tokenDto=new TokenDto();
                
                if(dto==null)
                {
                    tokenDto.Messege="Please fill all details.";
                    return new Tuple<int, TokenDto>(0,tokenDto);
                }

                var existingUser = await _context.AccountUsers.FirstOrDefaultAsync(x => x.Email == dto.Email);
                
                if(existingUser==null)
                {
                    tokenDto.Messege = "User not found.";
                    return new Tuple<int, TokenDto>(0, tokenDto);
                }

                if (string.IsNullOrEmpty(existingUser.Password))
                {
                    tokenDto.Messege = "Invalid credentials.";
                    return new Tuple<int, TokenDto>(2, tokenDto);
                }

                var passwordHasher = new PasswordHasher<string>();
                var verifyPassword = passwordHasher.VerifyHashedPassword(dto.Email, existingUser.Password, dto.Password);

                if(verifyPassword==PasswordVerificationResult.Success)
                {
                    UserDto user = new();
                    user.FirstName= existingUser.FirstName;
                    user.LastName= existingUser.LastName;
                    user.Id= existingUser.Id;
                    user.Email= existingUser.Email;

                    var token = GetJwtToken(user);
                    tokenDto.Token = token;
                    tokenDto.Messege = "Login successful.";

                    return new Tuple<int, TokenDto>(1, tokenDto);
                }

                else if(verifyPassword==PasswordVerificationResult.SuccessRehashNeeded)
                {
                    UserDto user = new();
                    user.FirstName = existingUser.FirstName;
                    user.LastName = existingUser.LastName;
                    user.Id = existingUser.Id;
                    user.Email = existingUser.Email;

                    existingUser.Password = PasswordHashing(dto);
                    _context.AccountUsers.Update(existingUser);
                    await _context.SaveChangesAsync();

                    var token = GetJwtToken(user);
                    tokenDto.Token = token;
                    tokenDto.Messege = "Login successful.";

                    return new Tuple<int, TokenDto>(1, tokenDto);
                }


                else if(verifyPassword==PasswordVerificationResult.Failed)
                {
                    tokenDto.Messege = "Invalid password.";

                    return new Tuple<int, TokenDto>(2, tokenDto);
                }

                tokenDto.Messege = "Invalid credentials.";
                return new Tuple<int, TokenDto>(2, tokenDto);

            }

            catch(Exception)
            {
                throw;
            }
        }

    }
}
