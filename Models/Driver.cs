using System.ComponentModel;

namespace CabBookingSystem.Models
{
    public class Driver
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int CabId { get; set; }
        public Cab Cab { get; set; }  // Assuming a Cab relation
        public bool IsAvailable { get; set; }
        public int PhoneNumber { get; set; }
        public string Location {  get; set; }   
       
    }

}