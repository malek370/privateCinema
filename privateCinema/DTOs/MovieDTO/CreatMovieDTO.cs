namespace Ath.DTOs.MovieDTO
{
    public class CreatMovieDTO
    {
        public string? Title { get; set; }
        public string? Genres { get; set; }
        public string? Overview { get; set; }
        public DateOnly ReleaseDate { get; set; }
        public int Runtime { get; set; }
        public string? Keywords { get; set; }
        public string? ProdCompanies { get; set; }
        public float Rate { get; set; }
    }
}
