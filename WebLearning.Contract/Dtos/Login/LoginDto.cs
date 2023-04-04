using System.ComponentModel.DataAnnotations;

namespace WebLearning.Contract.Dtos.Login
{
    public class LoginDto
    {
        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }
        [Required]
        [MaxLength(250)]
        public string Password { get; set; }
    }
}
