using System.Threading.Tasks;
using BookAPI.Models;

namespace BookAPI.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByUsernameAsync(string username);
        Task<User?> GetUserByEmailAsync(string email);
        Task CreateUserAsync(User user);
        Task<bool> UserExistsAsync(string username);
    }
}
