
using Microsoft.AspNetCore.Http;

namespace ConcertBooking.Application.Services.Interfaces
{
    public interface IUtilityService
    {
        Task<string> SaveImage(string ContainerName, IFormFile file);
        Task<string> EditImage(string ContainerName, IFormFile file, string dbpath);
        Task DeleteImage(string ContainerName, string dbpath);

    }
}
