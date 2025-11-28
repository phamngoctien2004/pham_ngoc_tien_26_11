using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request.Auth
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Mật khẩu không được trống")]
        public string Name { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = string.Empty;

        [Length(10, 10, ErrorMessage = "SDT không hợp lệ")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ngày sinh không được trống")]
        public DateOnly Birth { get; set; }

        [MinLength(6, ErrorMessage = "Mật khẩu yêu cầu lớn hơn 6 kí tự")]
        public string Password { get; set; } = string.Empty;

        [Compare("Password", ErrorMessage = "Mật khẩu không khớp")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
