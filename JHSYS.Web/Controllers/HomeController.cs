using JHSYS.BLL;
using JHSYS.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace JHSYS.Web.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Menu()
        {
            SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@MenuState", "1") };
            var dt = DataOperator.GetDataList<SysMenu>("*","MenuState=@MenuState"," MenuSort",sp);
            string jsondt = ToMenuJson(dt,"0");
            return Json(new { state = true, data = jsondt });
        }

        private string ToMenuJson(List<SysMenu> data, string parentId)
        {
            StringBuilder sbJson = new StringBuilder();
            sbJson.Append("[");
            //var dt = HomeDB.MenuParent(parentId);
            List<SysMenu> entitys = data.FindAll(t => t.ParentID == Int32.Parse(parentId));
            if (entitys.Count > 0)
            {
                foreach (var item in entitys)
                {
                    string strJson = JsonConvert.SerializeObject(item);
                    strJson = strJson.Insert(strJson.Length - 1, ",\"ChildNodes\":" + ToMenuJson(data, item.Id.ToString()) + "");
                    sbJson.Append(strJson + ",");
                }
                sbJson = sbJson.Remove(sbJson.Length - 1, 1);
            }
            sbJson.Append("]");
            return sbJson.ToString();
        }
    }
}