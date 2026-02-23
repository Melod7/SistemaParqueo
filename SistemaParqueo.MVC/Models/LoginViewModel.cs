using System.ComponentModel.DataAnnotations;

namespace SistemaParqueo.MVC.Models
{
    public class LoginViewModel
    {
        [Required]
        public string Usuario { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
