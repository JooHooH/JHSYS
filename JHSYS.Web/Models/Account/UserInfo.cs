using JHSYS.BLL;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace JHSYS.Web.Models
{
    [TableNames("UserInfos")]
    public class UserInfo
    {
        //[Key]
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [DataBaseColumn]
        [DisplayName("邮箱")]
        [Required(ErrorMessage = "请输入 Email 地址")]
        [Description("我们直接以 Email 当成会员的账号")]
        [MaxLength(250, ErrorMessage = "Email地址长度无法超过250字节")]
        [DataType(DataType.EmailAddress)]
        [Remote("CheckDup", "Account", HttpMethod = "POST", ErrorMessage = "您输入的 Email 已经有人注册了!")]
        public string Email { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [DataBaseColumn]
        [DisplayName("密码")]
        [Required(ErrorMessage = "请输入密码")]
        [MaxLength(40, ErrorMessage = "请输入密码")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [DataBaseColumn]
        [DisplayName("名称")]
        [Required(ErrorMessage = "请输入名称")]
        [MaxLength(8, ErrorMessage = "名称不超过8位数")]
        public string Name { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        [DataBaseColumn]
        public DateTime RegisterOn { get; set; }

        /// <summary>
        /// 权限
        /// </summary>
        [DataBaseColumn]
        [DisplayName("验证")]
        [MaxLength(36)]
        [Description("当 AuthCode 为 null 则代表已经通过邮箱验证")]
        public string AuthCode { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        [DataBaseColumn]
        public int _ItemType { get; set; }
    }
}