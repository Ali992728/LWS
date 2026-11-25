using LWS.Dto;

namespace LWS.IService
{
    public interface ILaptopService
    {
        Task<Tuple<int, string>> AddLaptop(LaptopDto laptop);
        Task<Tuple<int, string>> UpdateLaptop(LaptopDto laptop);
        Task<Tuple<int, string>> DeleteLaptop(string serialNo);
        Task<Tuple<int, List<LaptopDto>>> GetAllLaptops();
        Task<Tuple<int, List<LaptopDto>>> GetAllLaptopsByBrand(string brand);
        Task<Tuple<int, List<LaptopDto>>> GetAllLaptopsByModel(string model);
        Task<Tuple<int, LaptopDto>> GetLaptopBySerialNo(string serialNo);
        Task<Tuple<int, List<LaptopDto>>> GetAllLaptopsByPriceRange(decimal minPrice, decimal maxPrice);

    }
}
