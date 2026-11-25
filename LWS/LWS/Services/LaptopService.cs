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
            try
            {
                if (laptop == null)
                {
                    return new Tuple<int, string>(0, "Fillup Laptop Details.");
                }

                await _context.Laptops.AddAsync(new Entities.Laptop
                {
                    SerialNo = laptop.SerialNo,
                    Brand = laptop.Brand,
                    Model = laptop.Model,
                    Processor = laptop.Processor,
                    Ram = laptop.Ram,
                    Storage = laptop.Storage,
                    Gpu = laptop.Gpu,
                    Display = laptop.Display,
                    Price = laptop.Price,
                    CreatedDate = DateTime.Now,
                    IsAvailable = laptop.IsAvailable,
                    ImageUrl = laptop.ImageUrl,
                    ExtraImages = laptop.ExtraImages,

                });

                await _context.SaveChangesAsync();

                return new Tuple<int, string>(1, "Laptop Added Successfully.");
            }
            catch (Exception)
            {
                throw;
            }
        }



        public async Task<Tuple<int, string>> UpdateLaptop(LaptopDto laptop)
        {
            try
            {
                var existing = await _context.Laptops.FirstOrDefaultAsync(x => x.SerialNo == laptop.SerialNo);

                if (existing == null)
                {
                    return new Tuple<int, string>(0, "Laptop Not Found.");
                }

                existing.SerialNo = string.IsNullOrWhiteSpace(laptop.SerialNo) ? existing.SerialNo : laptop.SerialNo;
                existing.Brand = string.IsNullOrWhiteSpace(laptop.Brand) ? existing.Brand : laptop.Brand;
                existing.Model = string.IsNullOrWhiteSpace(laptop.Model) ? existing.Model : laptop.Model;
                existing.Processor = string.IsNullOrWhiteSpace(laptop.Processor) ? existing.Processor : laptop.Processor;
                existing.Ram = string.IsNullOrWhiteSpace(laptop.Ram) ? existing.Ram : laptop.Ram;
                existing.Storage = string.IsNullOrWhiteSpace(laptop.Storage) ? existing.Storage : laptop.Storage;
                existing.Gpu = string.IsNullOrWhiteSpace(laptop.Gpu) ? existing.Gpu : laptop.Gpu;
                existing.Display = string.IsNullOrWhiteSpace(laptop.Display) ? existing.Display : laptop.Display;
                existing.Price = string.IsNullOrWhiteSpace(laptop.Price.ToString()) ? existing.Price : laptop.Price;
                existing.IsAvailable = string.IsNullOrWhiteSpace(laptop.IsAvailable.ToString()) ? existing.IsAvailable : laptop.IsAvailable;
                existing.LastModified = DateTime.Now;

                _context.Laptops.Update(existing);
                await _context.SaveChangesAsync();

                return new Tuple<int, string>(1, "Laptop Updated Successfully.");
            }
            catch (Exception)
            {
                throw;
            }
        }



        public async Task<Tuple<int, string>> DeleteLaptop(string serialNo)
        {
            try
            {
                var existing = await _context.Laptops.FirstOrDefaultAsync(x => x.SerialNo == serialNo);
                if (existing == null)
                {
                    return new Tuple<int, string>(0, "Laptop Not Found.");
                }
                _context.Laptops.Remove(existing);
                await _context.SaveChangesAsync();
                return new Tuple<int, string>(1, "Laptop Deleted Successfully.");
            }
            catch (Exception)
            {
                throw;
            }
        }



        public async Task<Tuple<int, List<LaptopDto>>> GetAllLaptops()
        {
            try
            {
                return new Tuple<int, List<LaptopDto>>(1, await _context.Laptops.AsNoTracking().Select(x => new LaptopDto
                {
                    Id = x.Id,
                    SerialNo = x.SerialNo,
                    Brand = x.Brand,
                    Model = x.Model,
                    Processor = x.Processor,
                    Ram = x.Ram,
                    Storage = x.Storage,
                    Gpu = x.Gpu,
                    Display = x.Display,
                    Price = x.Price,
                    IsAvailable = x.IsAvailable,
                    ImageUrl = x.ImageUrl,
                    ExtraImages = x.ExtraImages
                }).ToListAsync());
            }
            catch (Exception)
            {
                throw;
            }
        }



        public async Task<Tuple<int, List<LaptopDto>>> GetAllLaptopsByBrand(string brand)
        {
            try
            {
                var laptops = await _context.Laptops.AsNoTracking().Where(x => x.Brand == brand).Select(x => new LaptopDto
                    {
                        Id = x.Id,
                        SerialNo = x.SerialNo,
                        Brand = x.Brand,
                        Model = x.Model,
                        Processor = x.Processor,
                        Ram = x.Ram,
                        Storage = x.Storage,
                        Gpu = x.Gpu,
                        Display = x.Display,
                        Price = x.Price,
                        IsAvailable = x.IsAvailable,
                        ImageUrl = x.ImageUrl,
                        ExtraImages = x.ExtraImages
                    }).ToListAsync();

                return new Tuple<int, List<LaptopDto>>(1, laptops);

            }
            catch (Exception)
            {
                throw;
            }
        }




        public async Task<Tuple<int, List<LaptopDto>>> GetAllLaptopsByModel(string model)
        {
            try
            {
                var laptops = await _context.Laptops.AsNoTracking().Where(x => x.Model == model).Select(x => new LaptopDto
                    {
                        Id = x.Id,
                        SerialNo = x.SerialNo,
                        Brand = x.Brand,
                        Model = x.Model,
                        Processor = x.Processor,
                        Ram = x.Ram,
                        Storage = x.Storage,
                        Gpu = x.Gpu,
                        Display = x.Display,
                        Price = x.Price,
                        IsAvailable = x.IsAvailable,
                        ImageUrl = x.ImageUrl,
                        ExtraImages = x.ExtraImages
                    }).ToListAsync();

                return new Tuple<int, List<LaptopDto>>(1, laptops);

            }
            catch (Exception)
            {
                throw;
            }
        }



        public async Task<Tuple<int, LaptopDto>> GetLaptopBySerialNo(string serialNo)
        {
            try
            {
                var existing = await _context.Laptops.AsNoTracking().FirstOrDefaultAsync(x => x.SerialNo == serialNo);

                if (existing == null)
                {
                    return new Tuple<int, LaptopDto>(0, null);
                }

                var laptop = new LaptopDto
                {
                    Id = existing.Id,
                    SerialNo = existing.SerialNo,
                    Brand = existing.Brand,
                    Model = existing.Model,
                    Processor = existing.Processor,
                    Ram = existing.Ram,
                    Storage = existing.Storage,
                    Gpu = existing.Gpu,
                    Display = existing.Display,
                    Price = existing.Price,
                    IsAvailable = existing.IsAvailable,
                    ImageUrl = existing.ImageUrl,
                    ExtraImages = existing.ExtraImages
                };

                return new Tuple<int, LaptopDto>(1, laptop);
            }
            catch (Exception)
            {
                throw;
            }
        }



        public async Task<Tuple<int, List<LaptopDto>>> GetAllLaptopsByPriceRange(decimal minPrice, decimal maxPrice)
        {
            try
            {
                var laptops = await _context.Laptops.AsNoTracking().Where(x => x.Price >= minPrice && x.Price <= maxPrice).Select(x => new LaptopDto
                    {
                        Id = x.Id,
                        SerialNo = x.SerialNo,
                        Brand = x.Brand,
                        Model = x.Model,
                        Processor = x.Processor,
                        Ram = x.Ram,
                        Storage = x.Storage,
                        Gpu = x.Gpu,
                        Display = x.Display,
                        Price = x.Price,
                        IsAvailable = x.IsAvailable,
                        ImageUrl = x.ImageUrl,
                        ExtraImages = x.ExtraImages
                    }).ToListAsync();

                return new Tuple<int, List<LaptopDto>>(1, laptops);

            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
