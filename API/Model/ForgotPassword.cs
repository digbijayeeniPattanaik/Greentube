using System.ComponentModel.DataAnnotations;

namespace API.Model
{
    public class ForgotPassword
    {
        [RegularExpression("^[A-Za-z0-9._%+-]*@[A-Za-z0-9.-]*\\.[A-Za-z0-9-]{2,}$",
                ErrorMessage = "Email is mandatory and must be properly formatted.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email is mandatory")]
        public string Email { get; set; }
    }
}
