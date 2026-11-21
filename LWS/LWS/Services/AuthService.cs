using LWS.Data;
using LWS.Dto;
using LWS.Entities;
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
        private string PasswordHasing(UserDto dto)
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
    }
}
