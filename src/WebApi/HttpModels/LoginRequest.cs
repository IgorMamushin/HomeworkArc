using System.ComponentModel.DataAnnotations;

namespace WebApi.HttpModels
{
    public class LoginRequest
    {
        [Required]
        public Guid Id { get;set; }
        
        [Required]
        public string Password { get;set; } = default!;
    }
}
