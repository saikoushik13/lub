

using Database;
using Database.Entities;
using DatabaseOperations.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseOperations.Implementation
{
    public class GenreDatabaseOperations : IGenreDatabaseOperations
    {
        private readonly AppDbContext _context;

        public GenreDatabaseOperations(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Genre>> GetAllAsync()
        {
            try
            {
                return await _context.Genres.ToListAsync();
            }
            catch (Exception ex)
            {
         
                throw new InvalidOperationException("An error occurred while retrieving all genres.", ex);
            }
        }

        public async Task<Genre> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Genres.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An error occurred while retrieving the genre with ID: {id}.", ex);
            }
        }

        public async Task AddAsync(Genre genre)
        {
            try
            {
                await _context.Genres.AddAsync(genre);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while adding the genre.", ex);
            }
        }

        public async Task<List<Genre>> GetGenresByIdsAsync(List<int> genreIds)
        {
            try
            {
                return await _context.Genres
                    .Where(g => genreIds.Contains(g.Id))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while retrieving genres by IDs.", ex);
            }
        }

        public async Task UpdateAsync(Genre genre)
        {
            try
            {
                _context.Genres.Update(genre);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An error occurred while updating the genre with ID: {genre.Id}.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var genre = await _context.Genres.FindAsync(id);
                if (genre != null)
                {
                    _context.Genres.Remove(genre);
                }
                else
                {
                    throw new KeyNotFoundException($"Genre with ID: {id} not found.");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An error occurred while deleting the genre with ID: {id}.", ex);
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
