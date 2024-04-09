using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LendLoopAPI.Models
{
    [Table("User")]
    public class UserApp
    {
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;    
        public string Adress { get; set; } = string.Empty;
        public string ProfilePicUrl { get; set; } = string.Empty;
    }
}
