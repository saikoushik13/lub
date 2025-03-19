

using Database;
using Database.Entities;
using DatabaseOperations.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DatabaseOperations.Implementation
{
    public class UserDatabaseOperations : IUserDatabaseOperations
    {
        private readonly AppDbContext _context;

        public UserDatabaseOperations(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            try
            {
                return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An error occurred while retrieving the user with email: {email}.", ex);
            }
        }

        public async Task<User> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Users.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An error occurred while retrieving the user with ID: {id}.", ex);
            }
        }

        public async Task AddAsync(User user)
        {
            try
            {
                await _context.Users.AddAsync(user);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while adding the user.", ex);
            }
        }

        public async Task SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while saving changes to the database.", ex);
            }
        }
    }
}
