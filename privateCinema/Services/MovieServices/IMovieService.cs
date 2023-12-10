﻿using privateCinema.DTOs.MovieDTO;
using privateCinema.Models;

namespace privateCinema.Services.MovieServices
{
    public interface IMovieService
    {
        Task<Response<object>> Creat(CreatMovieDTO creatmovie);
        Task<Response<object>> Delete(int movieId);
        Task<Response<GetMovieDTO>> GetByID(int movieId);
        Task<Response<List<GetMovieDTO>>> GetByKeywords(string keywords);
        Task<Response<List<GetMovieDTO>>> GetByTitle(string? name);
    }
}