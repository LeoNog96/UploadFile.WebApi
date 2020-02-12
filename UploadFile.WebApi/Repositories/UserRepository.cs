using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UploadFile.WebApi.Context;
using UploadFile.WebApi.Models;
using UploadFile.WebApi.Repositories.Interfaces;

namespace UploadFile.WebApi.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(UploadFileDbContext db)
        :base(db)
        {   }

        public async Task<User> GetUserByEmail(string email, string password)
        {
            return await _db.User.Where(x => x.Email == email && x.Password == password)
                                    .FirstOrDefaultAsync();
        }

        public async Task UserDelete(long userId)
        {
            var user = await _db.User.FindAsync(userId);

            user.Removed = true;

            await _db.SaveChangesAsync();
        }
    }
}