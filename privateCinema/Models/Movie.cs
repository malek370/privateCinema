using System.ComponentModel.DataAnnotations.Schema;

namespace Ath.Models
{
    public class Movie
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Genres { get; set; }
        public string? Overview { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int Runtime { get; set; }
        public string? Keywords { get; set; }
        public string? ProdCompanies { get; set; }
        public float Rate { get; set; }

    }
}
