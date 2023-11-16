using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Cascade_Country.Models
{
    public class State
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string StateName { get; set; }
        [ForeignKey("Country")]
        public int CountryId { get; set; }
        public virtual Country Country { get; set; }
    }
}
