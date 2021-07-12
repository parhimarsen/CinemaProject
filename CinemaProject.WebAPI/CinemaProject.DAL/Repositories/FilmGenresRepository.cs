using CinemaProject.DAL.Contexts;
using CinemaProject.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CinemaProject.DAL.Repositories
{
    public class FilmGenresRepository
    {
        private readonly CinemaContext _context;

        public FilmGenresRepository(CinemaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FilmGenreEntity>> GetAllAsync()
        {
            return await _context.Set<FilmGenreEntity>().ToListAsync();
        }

        public async Task<FilmGenreEntity> GetAsync(Guid filmId, Guid genreId)
        {
            return await _context.Set<FilmGenreEntity>().FindAsync(filmId, genreId);
        }

        public async Task<FilmGenreEntity> InsertAsync(FilmGenreEntity filmGenreEntity)
        {
            await _context.Set<FilmGenreEntity>().AddAsync(filmGenreEntity);

            return filmGenreEntity;
        }

        public async Task RemoveAsync(Guid filmId, Guid genreId)
        {
            FilmGenreEntity filmGenreEntity = await GetAsync(filmId, genreId);

            _context.Set<FilmGenreEntity>().Remove(filmGenreEntity);
        }

        public async Task<bool> ExistsAsync(FilmGenreEntity filmGenreEntity)
        {
            FilmGenreEntity filmGenre = await GetAsync(filmGenreEntity.FilmId, filmGenreEntity.GenreId);

            if (filmGenre == null)
            {
                return false;
            }

            return true;
        }
    }
}
