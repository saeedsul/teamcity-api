using System.ComponentModel.DataAnnotations;

namespace People.Api.DTOs
{
    public class PersonDto
    { 
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "DateOfBirth is required")]
        public DateOnly DateOfBirth { get; set; }
    }
}
