using LWS.Data;
using LWS.Dto;
using LWS.IService;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LWS.Services
{
    public class AuthService(AppDbContext _context, IConfiguration _configuration) :IAuthService
    {
        private string PasswordHashing(UserDto dto)
        {
            var passwordHasher = new PasswordHasher<string>();
            var hash = passwordHasher.HashPassword(dto.Email,dto.Password);
            return hash;
        }

        private string GetJwtToken(UserDto dto,string role)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, dto.FirstName + " " + dto.LastName),
                new Claim(ClaimTypes.Email,dto.Email),
                new Claim(ClaimTypes.NameIdentifier,dto.Id.ToString()),
                new Claim(ClaimTypes.Role,role) 
            };

            var key= new SymmetricSecurityKey(Encoding.UTF8.GetBytes("h<#@B2SRZ<?M.@&/Aa^pfz.)0(PunsODR)!$*El$)qlhurFt{AOJG3$r!*y:e0F@"));

            var creds= new SigningCredentials(key, SecurityAlgorithms.HmacSha384);

            var token = new JwtSecurityToken(
                issuer:"LWS",
                audience:"LWS",
                claims:claims,
                expires:DateTime.UtcNow.AddMinutes(5),
                signingCredentials:creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public async Task<Tuple<int,TokenDto>> LoginUser(UserDto dto)
        {
            try
            {
                var tokenDto=new TokenDto();
                
                if(dto==null || string.IsNullOrEmpty(dto.Email) || string.IsNullOrEmpty(dto.Password))
                {
                    tokenDto.Messege="Please fill all details.";
                    return new Tuple<int, TokenDto>(0,tokenDto);
                }

                //Admin Login

                var adminEmail = _configuration["Admin:Email"];
                var adminPasswordHash=_configuration["Admin:Password"];

                if(dto.Email==adminEmail)
                {
                    if (string.IsNullOrEmpty(adminPasswordHash))
                    {
                        tokenDto.Messege = "Admin credentials are not configured.";
                        return new Tuple<int, TokenDto>(2, tokenDto);
                    }

                    var passwordAdminHasher = new PasswordHasher<string>();
                    var verifyAdminPassword = passwordAdminHasher.VerifyHashedPassword("Admin", adminPasswordHash, dto.Password);

                    if(verifyAdminPassword==PasswordVerificationResult.Success || verifyAdminPassword==PasswordVerificationResult.SuccessRehashNeeded)
                    {
                        UserDto admin = new();
                        admin.FirstName = "Admin";
                        admin.LastName = "";
                        admin.Id = Guid.Empty;
                        admin.Email = adminEmail;

                        var token = GetJwtToken(admin, "Admin");
                        tokenDto.Token = token;
                        tokenDto.Messege = "Admin login successful.";

                        return new Tuple<int, TokenDto>(1, tokenDto);
                    }

                    tokenDto.Messege = "Invalid credentials.";
                    return new Tuple<int, TokenDto>(2, tokenDto);

                }

                //Normal User Login

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

                    var token = GetJwtToken(user,"User");
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

                    var token = GetJwtToken(user,"User");
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



        public async Task<Tuple<int,string>> RegisterUser(UserDto dto)
        {
            try
            {
                if (dto == null ||string.IsNullOrEmpty(dto.FirstName) ||string.IsNullOrEmpty(dto.LastName) ||string.IsNullOrEmpty(dto.PhoneNumber) ||string.IsNullOrEmpty(dto.Email) ||string.IsNullOrEmpty(dto.Password))
                {
                    return new Tuple<int, string>(0, "Please fill all required details.");
                }


                var existingUser = await _context.AccountUsers.AnyAsync(x => x.Email == dto.Email);

                if(existingUser)
                {
                    return new Tuple<int, string>(1, "This user already exists!");
                }

                _context.AccountUsers.Add(new Entities.User
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    PhoneNumber = dto.PhoneNumber,
                    Email = dto.Email,
                    Password = PasswordHashing(dto)
                });

                await _context.SaveChangesAsync();

                return new Tuple<int, string>(2, "User registered successfully.");
            }

            catch(Exception)
            {
                throw;
            }
        }

    }
}
