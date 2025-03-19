

using Database;
using Database.Entities;
using DatabaseOperations.Interface;
using Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace DatabaseOperations.Implementation
{
    public class BookDatabaseOperations : IBookDatabaseOperations
    {
        private readonly AppDbContext _context;

        public BookDatabaseOperations(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Book>> GetAllAsync()
        {
            try
            {
                return await _context.Books.Include(b => b.Genres).ToListAsync();
            }
            catch (Exception ex)
            {
               
                throw new InvalidOperationException("An error occurred while retrieving all books.", ex);
            }
        }

        public async Task<Book> GetByISBNAsync(string isbn)
        {
            try
            {
                return await _context.Books
                    .Include(b => b.Genres)
                    .FirstOrDefaultAsync(b => b.ISBN == isbn);
            }
            catch (Exception ex)
            {
               
                throw new InvalidOperationException($"An error occurred while retrieving the book with ISBN: {isbn}.", ex);
            }
        }

        public async Task<List<Book>> GetBooksByDynamicQueryAsync(string dsql)
        {
            try
            {
                IQueryable<Book> query = _context.Books.Include(b => b.Genres);

                if (!string.IsNullOrWhiteSpace(dsql))
                {
                    string dynamicQuery = DsqlDynamicQueryTransformer.Transform(dsql);

                    try
                    {
                        query = query.Where(dynamicQuery);
                    }
                    catch
                    {
                        throw new Exception("Invalid query syntax.");
                    }
                }

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                
                throw new InvalidOperationException("An error occurred while retrieving books by dynamic query.", ex);
            }
        }

        public async Task AddAsync(Book book)
        {
            try
            {
               
                var genreIds = book.Genres.Select(g => g.Id).ToList();
                var existingGenres = await _context.Genres.Where(g => genreIds.Contains(g.Id)).ToListAsync();
                book.Genres = existingGenres;

                await _context.Books.AddAsync(book);
            }
            catch (Exception ex)
            {
                
                throw new InvalidOperationException("An error occurred while adding the book.", ex);
            }
        }

        public async Task UpdateAsync(Book book)
        {
            try
            {
                var existingBook = await _context.Books
                    .Include(b => b.Genres)
                    .FirstOrDefaultAsync(b => b.ISBN == book.ISBN);

                if (existingBook != null)
                {
                    existingBook.Title = book.Title;
                    existingBook.Description = book.Description;
                    existingBook.Author = book.Author;
                    existingBook.Publisher = book.Publisher;
                    existingBook.PublicationYear = book.PublicationYear;

                    
                    var genreIds = book.Genres.Select(g => g.Id).ToList();
                    var existingGenres = await _context.Genres.Where(g => genreIds.Contains(g.Id)).ToListAsync();
                    existingBook.Genres.Clear();
                    existingBook.Genres = existingGenres;

                    _context.Books.Update(existingBook);
                }
                else
                {
                    throw new KeyNotFoundException($"Book with ISBN: {book.ISBN} not found.");
                }
            }
            catch (Exception ex)
            {
               
                throw new InvalidOperationException($"An error occurred while updating the book with ISBN: {book.ISBN}.", ex);
            }
        }

        public async Task DeleteAsync(string isbn)
        {
            try
            {
                var book = await _context.Books.Include(b => b.Genres).FirstOrDefaultAsync(b => b.ISBN == isbn);
                if (book != null)
                {
                    book.Genres.Clear(); 
                    _context.Books.Remove(book);
                }
                else
                {
                    throw new KeyNotFoundException($"Book with ISBN: {isbn} not found.");
                }
            }
            catch (Exception ex)
            {
                
                throw new InvalidOperationException($"An error occurred while deleting the book with ISBN: {isbn}.", ex);
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
