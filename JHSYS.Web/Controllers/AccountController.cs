using JHSYS.Web.Models;
using JHSYS.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Net.Mail;
using JHSYS.BLL;
using System.Data;
using System.Data.SqlClient;

namespace JHSYS.Web.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        
        // 注册
        public ActionResult Register()
        {
           // DataTable ds = s.GetTable("select * from UserInfos");
            //SqlParameter[] sp = { new SqlParameter("@Id","1")};
            //DataTable dt = JSQL.GetDataTable("UserInfos","*","Id=@Id",  sp,"");
            //string sql = "insert into UserInfos (Name,Password,Email,RegisterOn,AuthCode,_ItemType) values(@Name,@Password,@Email,@RegisterOn,@AuthCode,@_ItemType)";
            //SqlParameter[] paras = new SqlParameter[] { new SqlParameter("@Name", "黄俊浩"), new SqlParameter("@Password", "123456"), new SqlParameter("@Email", "111"), new SqlParameter("@RegisterOn", DateTime.Now), new SqlParameter("@AuthCode", "akjdh19829"), new SqlParameter("@_ItemType", "1") };

            ////bool id = s.ExecuteNoQuery(sql, paras);
            //UserInfo us = new UserInfo();
            //us.Name = "黄俊浩";
            //us.Password = MD5Encrypt32.GetMD5("123456");
            //us.Email = "JumHoo@126.com";
            //us.RegisterOn = DateTime.Now;
            //us.AuthCode = "9283HKJH87ghgFHGFTRJK";
            //us._ItemType = 1;
            //string id2 = DataOperator.InsertListData<UserInfo>(us);

            return View();
        }
        //写入会员资料
        [HttpPost]
        public ActionResult Register([Bind(Exclude = "RegisterOn,AuthCode")] UserInfo User)
        {
            // 检查会员是否存在
            //var chk_User = db.UserInfos.Where(p => p.Email == User.Email).FirstOrDefault();
            SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@Email", User.Email) };
            var chk_User = JSQL.CheckFile("UserInfos", "Email", "Email=@Email", sp);
            if (chk_User)
            {
                ModelState.AddModelError("Email", "您输入的 Email 已经有人注册过了!");
            }

            if (ModelState.IsValid)
            {
                //MD5密码加密
                User.Password = MD5Encrypt32.GetMD5(User.Password);
                // 会员注册时间
                User.RegisterOn = DateTime.Now;
                // 会员验证码，采用 Guid 当成验证码內容，避免有会员使用到重复的验证码
                User.AuthCode = Guid.NewGuid().ToString();

                //db.UserInfos.Add(User);
                //db.SaveChanges();

                string id2 = DataOperator.InsertListData<UserInfo>(User);

                if (String.IsNullOrEmpty(id2))
                {
                    return View();
                }

                //发送邮箱
                SendAuthCodeToUser(User);

                return RedirectToAction("Register", "Account");
            }
            else
            {
                return View();
            }
        }

        //邮箱检测
        [HttpPost]
        public ActionResult CheckDup(string Email)
        {
            SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@Email", Email) };
            var user = JSQL.CheckFile("UserInfos","Email","Email=@Email",sp);
            if (user)
                return Json(false);
            else
                return Json(true);
        }

        //发送邮件
        private void SendAuthCodeToUser(UserInfo User)
        {
            // 准备邮件內容
            string mailBody = System.IO.File.ReadAllText(Server.MapPath("~/App_Data/UserRegisterEMailTemplate.htm"));

            mailBody = mailBody.Replace("{{Name}}", User.Name);
            mailBody = mailBody.Replace("{{RegisterOn}}", User.RegisterOn.ToString("F"));
            var auth_url = new UriBuilder(Request.Url)
            {
                Path = Url.Action("ValidateRegister", new { id = User.AuthCode }),
                Query = ""
            };
            mailBody = mailBody.Replace("{{AUTH_URL}}", auth_url.ToString());

            // 发送邮件
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("jumhoo@126.com");
                mail.To.Add(User.Email);
                mail.Subject = "【JumHoo个人网站】会员注册确认信";
                mail.Body = mailBody;
                mail.IsBodyHtml = true;

                SmtpClient SmtpServer = new SmtpClient();
                SmtpServer.Host = "smtp.126.com";
                SmtpServer.Port = 25;
                SmtpServer.EnableSsl = true;
                SmtpServer.Credentials = new System.Net.NetworkCredential("jumhoo@126.com", "izngqrnxjahfbd00");


                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                throw ex;
                // 可记录异常
            }
        }

        public ActionResult ValidateRegister(string id)
        {
            if (String.IsNullOrEmpty(id))
                return HttpNotFound();

            //var User = db.UserInfos.Where(p => p.AuthCode == id).FirstOrDefault();
            //User = JSQL.GetDataTable("UserInfos",);
            SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@AuthCode", id) };
            var User = DataOperator.GetDataList<UserInfo>("*", "AuthCode=@AuthCode","",sp);
            if (User != null)
            {
                foreach (UserInfo item in User)
                {
                    if (!string.IsNullOrEmpty(item.AuthCode))
                    {
                        // 验证成功后 item.AuthCode="1"
                        item.AuthCode = "1";
                        string[] file = { "AuthCode" };
                        string[] value = { item.AuthCode};
                        //string s=DataOperator.UpdateData("UserInfo", item, "AuthCode=@AuthCode", sp);
                        string s = JSQL.Update("UserInfos", file, value,"AuthCode=@AuthCode",sp);
                        if (s.StartsWith("Err"))
                        {
                            TempData["LastTempMessage"] = s;
                        }
                    }
                }
            }
            else
            {
                TempData["LastTempMessage"] = "查无此会员验证码，您可能已经通过验证了!";
            }
            return RedirectToAction("Login", "Account");
        }

        // 显示登录页面
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        // 会员登录
        [HttpPost]
        public ActionResult Login(string email, string password, string verify, string returnUrl)
        {
            string v = WebHelper.GetSession("jumhoo_session_verifycode").ToString();
            if (string.IsNullOrEmpty(v) || Md5.md5(verify.ToLower(), 16) != v)
            {
                ModelState.AddModelError("verify", "验证码输入有误，请重新输入!");
            }
            else
            {
                if (ValidateUser(email, password))
                {
                    if (ModelState.IsValid)
                    {
                        FormsAuthentication.SetAuthCookie(email, false);

                        if (String.IsNullOrEmpty(returnUrl))
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            return Redirect(returnUrl);
                        }
                    }

                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult GetAuthCode()
        {
            return File(new VerifyCode().GetVerifyCode(), @"image/Gif");
        }

        ////初始化Cookie
        //private void operatorModel(string email, string password)
        //{
        //    var hash_pw = MD5Encrypt32.GetMD5(password);

        //    var userEntity = (from p in db.UserInfos
        //                      where p.Email == email && p.Password == hash_pw
        //                      select p).FirstOrDefault();
        //    if (userEntity != null)
        //    {
        //        if (userEntity.AuthCode == null)
        //        {
        //            OperatorModel operatorModel = new OperatorModel();
        //            operatorModel.UserId = userEntity.Id.ToString();
        //            operatorModel.UserName = userEntity.Name;
        //            operatorModel.RoleId = userEntity._ItemType.ToString();
        //            operatorModel.LoginIPAddress = Net.Ip;
        //            operatorModel.LoginIPAddressName = Net.GetLocation(operatorModel.LoginIPAddress);
        //            operatorModel.LoginTime = DateTime.Now;
        //            operatorModel.LoginToken = DESEncrypt.Encrypt(Guid.NewGuid().ToString());
        //        }
        //    }
        //}

        /// <summary>
        /// 邮箱验证，验证通过
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private bool ValidateUser(string email, string password)
        {
            var hash_pw = MD5Encrypt32.GetMD5(password);
            SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@Email", email), new SqlParameter("@Password", hash_pw) };
            var user = JSQL.GetDataTable("UserInfos","*", "Email=@Email and Password=@Password", sp,"");
            // 如果 user 为 null 则代表会员的账号、密码输入不正确
            if (user != null&&user.Rows.Count>0)
            {
                if (user.Rows[0]["AuthCode"].ToString() == "1")
                {
                    return true;
                }
                else
                {
                    ModelState.AddModelError("Email", "您尚未通过有效验证!");
                    return false;
                }
            }
            else
            {
                ModelState.AddModelError("Password", "您输入的账号密码有误！");
                return false;
            }
        }

        // 退出
        public ActionResult Logout()
        {
            // 清除表单验证的 Cookies
            FormsAuthentication.SignOut();

            // 清除所有 Session 
            Session.Clear();

            return RedirectToAction("Login", "Account");
        }
    }
}