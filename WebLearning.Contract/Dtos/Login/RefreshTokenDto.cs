using System.ComponentModel.DataAnnotations.Schema;
using WebLearning.Contract.Dtos.Account;

namespace WebLearning.Contract.Dtos.Login
{
    public class RefreshTokenDto
    {
        public Guid Id { get; set; }
        [ForeignKey("Account")]
        public Guid UserId { get; set; }
        public AccountDto accountDto { get; set; }

        public string Token { get; set; }
        public string JwtId { get; set; }
        public bool IsUsed { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime IssuedAt { get; set; }
        public DateTime ExpiredAt { get; set; }
    }
}
