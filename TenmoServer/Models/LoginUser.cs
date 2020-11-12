using System.ComponentModel.DataAnnotations;

namespace TenmoServer.Models
{
    /// <summary>
    /// Model to accept login parameters
    /// </summary>
    public class LoginUser
<<<<<<< HEAD
    { 
=======
    {
>>>>>>> 77161be2034ad7214800b53807a894cb40f16038
    [Required]
        public string Username { get; set; }
        [Required]
        [StringLength(150, MinimumLength = 1)]
        public string Password { get; set; }
    }
}
