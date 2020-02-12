using System.Collections.Generic;
using System.Threading.Tasks;
using UploadFile.WebApi.DTOs;
using UploadFile.WebApi.Models;

namespace UploadFile.WebApi.Services.Interfaces
{
    public interface IUserService
    {
        Task<object> Auth(LoginDTO login);
        Task<User> Save(User user);
        Task Update(User user);
        Task Delete(long userId);
        Task<User> Get(long userId);
        Task<List<User>> GetAll();
    }
}