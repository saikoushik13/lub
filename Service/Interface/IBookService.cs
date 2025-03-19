using Models.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IBookService
    {
        Task<(List<BookDto> books, int totalCount)> GetBooksPaginatedAsync(int page, int pageSize);
        Task<(List<BookDto> books, int totalCount)> GetBooksByDynamicQueryAsync(string dsql, int page, int pageSize);
        Task<BookDto> GetBookByISBNAsync(string isbn);
        Task AddBookAsync(BookDto bookDto);
        Task UpdateBookAsync(BookDto bookDto);
        Task DeleteBookAsync(string isbn);
    }
}
