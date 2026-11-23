using LWS.Dto;

namespace LWS.IService
{
    public interface IAuthService
    {
        Task<Tuple<int, TokenDto>> LoginUser(UserDto dto);
        Task<Tuple<int, string>> RegisterUser(UserDto dto);
    }
}
