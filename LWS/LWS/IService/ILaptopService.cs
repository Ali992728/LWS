using LWS.Dto;

namespace LWS.IService
{
    public interface ILaptopService
    {
        Task<Tuple<int, string>> AddLaptop(LaptopDto laptop);
        Task<Tuple<int, string>> UpdateLaptop(LaptopDto laptop);
        Task<Tuple<int, string>> DeleteLaptop(string serialNo);

    }
}
