

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
    public class ReviewDatabaseOperations : IReviewDatabaseOperations
    {
        private readonly AppDbContext _context;

        public ReviewDatabaseOperations(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Review>> GetReviewsByBookAsync(string bookISBN)
        {
            try
            {
                return await _context.Reviews
                    .Include(r => r.User)
                    .Where(r => r.BookISBN == bookISBN)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An error occurred while retrieving reviews for the book with ISBN: {bookISBN}.", ex);
            }
        }

        public async Task<Review> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Reviews.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An error occurred while retrieving the review with ID: {id}.", ex);
            }
        }

        public async Task AddAsync(Review review)
        {
            try
            {
                await _context.Reviews.AddAsync(review);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while adding the review.", ex);
            }
        }

        public async Task UpdateAsync(Review review)
        {
            try
            {
                _context.Reviews.Update(review);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An error occurred while updating the review with ID: {review.Id}.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var review = await _context.Reviews.FindAsync(id);
                if (review != null)
                {
                    _context.Reviews.Remove(review);
                }
                else
                {
                    throw new KeyNotFoundException($"Review with ID: {id} not found.");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An error occurred while deleting the review with ID: {id}.", ex);
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
