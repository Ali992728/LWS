using LWS.Dto;

namespace LWS.IService
{
    public interface ILaptopService
    {
        Task<Tuple<int, string>> AddLaptop(LaptopDto laptop);

    }
}
