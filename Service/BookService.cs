
using AutoMapper;
using Database.Entities;
using DatabaseOperations.Interface;
using Extensions;
using Models.DTO;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service
{
    public class BookService : IBookService
    {
        private readonly IBookDatabaseOperations _bookDbOperations;
        private readonly IGenreDatabaseOperations _genreDbOperations;
        private readonly IMapper _mapper;

        public BookService(IBookDatabaseOperations bookDbOperations, IGenreDatabaseOperations genreDbOperations, IMapper mapper)
        {
            _bookDbOperations = bookDbOperations;
            _genreDbOperations = genreDbOperations;
            _mapper = mapper;
        }

        public async Task<(List<BookDto> books, int totalCount)> GetBooksPaginatedAsync(int page, int pageSize)
        {
            try
            {
                var booksQuery = (await _bookDbOperations.GetAllAsync()).AsQueryable();
                int totalCount = booksQuery.Count();
                var books = booksQuery.Paginate(page, pageSize).ToList();
                return (_mapper.Map<List<BookDto>>(books), totalCount);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while retrieving paginated books.", ex);
            }
        }

        public async Task<(List<BookDto> books, int totalCount)> GetBooksByDynamicQueryAsync(string dsql, int page, int pageSize)
        {
            try
            {
                var booksQuery = (await _bookDbOperations.GetBooksByDynamicQueryAsync(dsql)).AsQueryable();
                int totalCount = booksQuery.Count();
                var books = booksQuery.Paginate(page, pageSize).ToList();
                return (_mapper.Map<List<BookDto>>(books), totalCount);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while retrieving books by dynamic query.", ex);
            }
        }

        public async Task<BookDto> GetBookByISBNAsync(string isbn)
        {
            try
            {
                var book = await _bookDbOperations.GetByISBNAsync(isbn);
                return _mapper.Map<BookDto>(book);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An error occurred while retrieving the book with ISBN: {isbn}.", ex);
            }
        }

        public async Task AddBookAsync(BookDto bookDto)
        {
            try
            {
                var genres = await _genreDbOperations.GetGenresByIdsAsync(bookDto.GenreIds);
                if (genres.Count != bookDto.GenreIds.Count)
                    throw new InvalidOperationException("One or more genres are invalid.");

                var book = _mapper.Map<Book>(bookDto);
                book.Genres = genres; 

                await _bookDbOperations.AddAsync(book);
                await _bookDbOperations.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while adding the book.", ex);
            }
        }

        public async Task UpdateBookAsync(BookDto bookDto)
        {
            try
            {
                var existingBook = await _bookDbOperations.GetByISBNAsync(bookDto.ISBN);
                if (existingBook == null)
                    throw new KeyNotFoundException("Book not found.");

                existingBook.Title = bookDto.Title;
                existingBook.Author = bookDto.Author;
                existingBook.Description = bookDto.Description;
                existingBook.Publisher = bookDto.Publisher;
                existingBook.PublicationYear = bookDto.PublicationYear;

                // Update genres
                var genres = await _genreDbOperations.GetGenresByIdsAsync(bookDto.GenreIds);
                existingBook.Genres.Clear();
                existingBook.Genres = genres;

                await _bookDbOperations.UpdateAsync(existingBook);
                await _bookDbOperations.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An error occurred while updating the book with ISBN: {bookDto.ISBN}.", ex);
            }
        }

        public async Task DeleteBookAsync(string isbn)
        {
            try
            {
                await _bookDbOperations.DeleteAsync(isbn);
                await _bookDbOperations.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An error occurred while deleting the book with ISBN: {isbn}.", ex);
            }
        }
    }
}
