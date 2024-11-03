using Ath.DataAccess;
using Ath.DTOs.MovieDTO;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Ath.Models;

namespace Ath.Services.MovieServices
{
    public class MovieService : IMovieService
    {
        private readonly AuthDbContext _context;
        public MovieService(AuthDbContext cinemaDb)
        {
            _context = cinemaDb;

        }

        public async Task<Response<object>> Creat(CreatMovieDTO creatmovie)
        {
            var result = new Response<object>();
            try
            {
                var movie = new Movie()
                {
                    Title = creatmovie.Title,
                    Genres = creatmovie.Genres,
                    Overview = creatmovie.Overview,
                    ReleaseDate = creatmovie.ReleaseDate.ToDateTime(TimeOnly.MinValue),
                    Runtime = creatmovie.Runtime,
                    Keywords = creatmovie.Keywords,
                    ProdCompanies = creatmovie.ProdCompanies,
                    Rate = creatmovie.Rate,
                };
                await _context.Movies.AddAsync(movie);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) { result.message = ex.Message; result.success = false; }
            return result;
        }
        public async Task<Response<object>> Delete(int movieId)
        {
            var result = new Response<object>();
            try
            {
                var movie = await _context.Movies.FirstOrDefaultAsync(u => u.Id == movieId);
                if (movie == null) { throw new Exception("movie not found"); }
                _context.Movies.Remove(movie);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) { result.message = ex.Message; result.success = false; }
            return result;
        }
        public async Task<Response<GetMovieDTO>> GetByID(int movieId)
        {
            var result = new Response<GetMovieDTO>();
            try
            {
                var movie = await _context.Movies.Select(u => new GetMovieDTO()
                {
                    Id = u.Id,
                    Title = u.Title,
                    Genres = u.Genres,
                    Overview = u.Overview,
                    Runtime = u.Runtime,
                    ProdCompanies = u.ProdCompanies,
                    Rate = u.Rate,
                    ReleaseDate = DateOnly.FromDateTime(u.ReleaseDate)
                }).FirstOrDefaultAsync(u => u.Id == movieId);
                if (movie == null) { throw new Exception("movie not found"); }
                result.data = movie;
            }
            catch (Exception ex) { result.message = ex.Message; result.success = false; }
            return result;
        }
        public async Task<Response<List<GetMovieDTO>>> GetByTitle(string? name)
        {
            var result = new Response<List<GetMovieDTO>>();
            try
            {
                List<GetMovieDTO> movies;
                if (string.IsNullOrEmpty(name))
                {
                    movies = await _context.Movies.Select(u => new GetMovieDTO()
                    {
                        Id = u.Id,
                        Title = u.Title,
                        Genres = u.Genres,
                        Overview = u.Overview,
                        Runtime = u.Runtime,
                        ProdCompanies = u.ProdCompanies,
                        Rate = u.Rate,
                        ReleaseDate = DateOnly.FromDateTime(u.ReleaseDate)
                    }).ToListAsync();
                }
                else movies = await _context.Movies.Where(m => m.Title!.ToLower().Contains(name.ToLower())).Select(u => new GetMovieDTO()
                {
                    Id = u.Id,
                    Title = u.Title,
                    Genres = u.Genres,
                    Overview = u.Overview,
                    Runtime = u.Runtime,
                    ProdCompanies = u.ProdCompanies,
                    Rate = u.Rate,
                    ReleaseDate = DateOnly.FromDateTime(u.ReleaseDate)
                }).ToListAsync();
                result.data = movies;
            }
            catch (Exception ex) { result.message = ex.Message; result.success = false; }
            return result;
        }
        public async Task<Response<List<GetMovieDTO>>> GetByKeywords(string keyword)
        {
            //to be repaired
            var result = new Response<List<GetMovieDTO>>();
            try
            {

                var movies = await _context.Movies.Where(m => m.Keywords!.Contains(keyword)).Select(u => new GetMovieDTO()
                {
                    Id = u.Id,
                    Title = u.Title,
                    Genres = u.Genres,
                    Overview = u.Overview,
                    Runtime = u.Runtime,
                    ProdCompanies = u.ProdCompanies,
                    Rate = u.Rate,
                    ReleaseDate = DateOnly.FromDateTime(u.ReleaseDate)
                }).ToListAsync();
                result.data = movies;
            }
            catch (Exception ex) { result.message = ex.Message; result.success = false; }
            return result;
        }
        private static bool CheckKeyword(string s, List<string> l)
        {
            foreach (var item in l)
            {
                if (s.ToLower().Contains(item.ToLower()))
                    return true;
            }
            return false;
        }
    }
}
