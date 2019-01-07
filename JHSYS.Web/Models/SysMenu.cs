using JHSYS.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JHSYS.Web.Models
{
    /// <summary>
    /// 系统菜单
    /// </summary>
    [TableNames("Sys_Menu")]
    public class SysMenu
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 父级ID
        /// </summary>
        public int ParentID { get; set; }
        /// <summary>
        /// 菜单名
        /// </summary>
        public string MenuName { get; set; }
        /// <summary>
        /// 菜单URL
        /// </summary>
        public string MenuUrl { get; set; }
        /// <summary>
        /// 菜单图标
        /// </summary>
        public string MenuIcon { get; set; }
        /// <summary>
        /// 菜单排序
        /// </summary>
        public int MenuSort { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string Operator { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime? OperatorDT { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int MenuState { get; set; }

        /// <summary>
        /// 子菜单集合
        /// </summary>
        //public List<SysMenu> ChildNo { get; set; } 
    }
}