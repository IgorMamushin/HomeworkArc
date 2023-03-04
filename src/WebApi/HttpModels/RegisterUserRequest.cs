using System.ComponentModel.DataAnnotations;

namespace WebApi.HttpModels
{
    public class RegisterUserRequest
    {
        [Required, MinLength(1), MaxLength(50)]
        public string FirstName { get; set; }
        [Required, MinLength(1), MaxLength(50)]
        public string LastName { get; set; }
        [Required]
        public int Age { get; set; }
        [MaxLength(1000)]
        public string Biography { get; set; }
        [Required, MinLength(1), MaxLength(30)]
        public string City { get; set; }

        [Required, MinLength(1)]
        public string Password { get;set; }
    }
}
