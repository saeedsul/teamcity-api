using System.ComponentModel.DataAnnotations;

namespace People.Api.DTOs
{
    public class UpdatePersonDto
    { 
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;
        public DateOnly DateOfBirth { get; set; }
    }
}
