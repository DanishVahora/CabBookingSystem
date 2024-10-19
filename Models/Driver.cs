using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CabBookingSystem.Models
{
    public class Driver
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public int CabId { get; set; }
        public Cab? Cab { get; set; }  

        public bool IsAvailable { get; set; }

        [Required]
        public string PhoneNumber { get; set; }  

        [Required]
        public string Location { get; set; }
    }
}
