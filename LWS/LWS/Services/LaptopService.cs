using LWS.Data;
using LWS.Dto;
using LWS.IService;
using Microsoft.EntityFrameworkCore;

namespace LWS.Services
{
    public class LaptopService(AppDbContext _context):ILaptopService
    {
        public async Task<Tuple<int,string>> AddLaptop(LaptopDto laptop)
        {
            if(laptop==null)
            {
                return new Tuple<int, string>(0,"Fillup Laptop Details.");
            }

            await _context.Laptops.AddAsync(new Entities.Laptop
            {
                SerialNo=laptop.SerialNo,
                Brand=laptop.Brand,
                Model=laptop.Model,
                Processor=laptop.Processor,
                Ram=laptop.Ram,
                Storage=laptop.Storage,
                Gpu=laptop.Gpu,
                Display=laptop.Display,
                Price=laptop.Price,
                CreatedDate=DateTime.Now,
                IsAvailable=laptop.IsAvailable,
                ImageUrl=laptop.ImageUrl,
                ExtraImages=laptop.ExtraImages,

            });

            await _context.SaveChangesAsync();

            return new Tuple<int, string>(1,"Laptop Added Successfully.");
        }
    }
}
