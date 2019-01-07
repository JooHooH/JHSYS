using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace JHSYS.Web.Models
{
    public class UserLoginViewModel
    {
        [DisplayName("账号")]
        [DataType(DataType.EmailAddress, ErrorMessage = "请输入您的 Email 地址")]
        [Required(ErrorMessage = "请输入{0}")]
        public string email { get; set; }

        [DisplayName("密码")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "请输入{0}")]
        public string password { get; set; }

        [DisplayName("验证码")]
        [DataType(DataType.Text, ErrorMessage = "请输入验证码")]
        [Required(ErrorMessage = "请输入{0}")]
        public string verify { get; set; }
    }
}