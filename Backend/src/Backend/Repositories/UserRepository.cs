using Backend.Data;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class UserRepository(AppDbContext dbContext) : IUserRepository
    {
        public async Task<User?> AddUserAsync(User user)
        {
            try
            {
                var createUser = await dbContext.Users.AddAsync(user);
                await dbContext.SaveChangesAsync();

                return createUser.Entity;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var deleteUser = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (deleteUser is null) return false;

            dbContext.Users.Remove(deleteUser);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await dbContext.Users.AsNoTracking().ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            var user = await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (user is null) return null;
            return user;
        }

        public async Task<User?> UpdateUserAsync(int id, User user)
        {
            var userToUpdate = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (userToUpdate is null) return null;

            userToUpdate.FirstName = !string.IsNullOrEmpty(user.FirstName) ? user.FirstName : userToUpdate.FirstName;
            userToUpdate.LastName = !string.IsNullOrEmpty(user.LastName) ? user.LastName : userToUpdate.LastName;

            await dbContext.SaveChangesAsync();
            return userToUpdate;
        }
    }
}
