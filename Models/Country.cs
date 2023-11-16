using System.ComponentModel.DataAnnotations;

namespace Cascade_Country.Models
{
    public class Country
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string CountryName { get; set; }
    }
}
