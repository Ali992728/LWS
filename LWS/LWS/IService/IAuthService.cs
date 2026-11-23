using LWS.Dto;

namespace LWS.IService
{
    public interface IAuthService
    {
        Task<Tuple<int, TokenDto>> LoginUser(UserDto dto);
    }
}
