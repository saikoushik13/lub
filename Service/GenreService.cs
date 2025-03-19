
using AutoMapper;
using Database.Entities;
using DatabaseOperations.Interface;
using Models.DTO;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service
{
    public class GenreService : IGenreService
    {
        private readonly IGenreDatabaseOperations _genreDbOperations;
        private readonly IMapper _mapper;

        public GenreService(IGenreDatabaseOperations genreDbOperations, IMapper mapper)
        {
            _genreDbOperations = genreDbOperations;
            _mapper = mapper;
        }

        public async Task<List<GenreDto>> GetAllGenresAsync()
        {
            try
            {
                var genres = await _genreDbOperations.GetAllAsync();
                return _mapper.Map<List<GenreDto>>(genres);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while retrieving all genres.", ex);
            }
        }

        public async Task<GenreDto> GetGenreByIdAsync(int id)
        {
            try
            {
                var genre = await _genreDbOperations.GetByIdAsync(id);
                return _mapper.Map<GenreDto>(genre);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An error occurred while retrieving the genre with ID: {id}.", ex);
            }
        }

        public async Task AddGenreAsync(GenreDto genreDto)
        {
            try
            {
                var genre = _mapper.Map<Genre>(genreDto);
                await _genreDbOperations.AddAsync(genre);
                await _genreDbOperations.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while adding the genre.", ex);
            }
        }

        public async Task UpdateGenreAsync(GenreDto genreDto)
        {
            try
            {
                var genre = _mapper.Map<Genre>(genreDto);
                await _genreDbOperations.UpdateAsync(genre);
                await _genreDbOperations.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An error occurred while updating the genre with ID: {genreDto.Id}.", ex);
            }
        }

        public async Task DeleteGenreAsync(int id)
        {
            try
            {
                await _genreDbOperations.DeleteAsync(id);
                await _genreDbOperations.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An error occurred while deleting the genre with ID: {id}.", ex);
            }
        }
    }
}
