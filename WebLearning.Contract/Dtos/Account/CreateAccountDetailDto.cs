using System.ComponentModel.DataAnnotations;

namespace WebLearning.Contract.Dtos.Account
{
    public class CreateAccountDetailDto
    {
        public Guid Id = Guid.NewGuid();
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Avatar { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Address { get; set; }
        public CreateAccountDto CreateAccountDto { get; set; }
    }
}
