using System.Threading.Tasks;
using UploadFile.WebApi.Models;

namespace UploadFile.WebApi.Repositories.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> GetUserByEmail(string email, string password);
        Task UserDelete(long userId);
    }
}