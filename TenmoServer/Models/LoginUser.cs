using System.ComponentModel.DataAnnotations;

namespace TenmoServer.Models
{
    /// <summary>
    /// Model to accept login parameters
    /// </summary>
    public class LoginUser
    {
    [Required]
        public string Username { get; set; }
        [Required]
        [StringLength(150, MinimumLength = 1)]
        public string Password { get; set; }
    }
}
