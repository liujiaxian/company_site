using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utility;
using WebApp.Controllers;

namespace WebApp.Areas.GuanWang.Controllers
{
    public class ManagerAccountController : DefaultManagerController
    {
        //
        // GET: /GuanWang/ManagerAccount/

        #region 登录
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            string name = Request["username"];
            if (string.IsNullOrEmpty(name))
            {
                return Content(ReturnMsg(Enum_Return.失败, "账号不能为空", null));
            }
            string pwd = Request["password"];
            if (string.IsNullOrEmpty(pwd))
            {
                return Content(ReturnMsg(Enum_Return.失败, "密码不能为空", null));
            }
            string verify = Request["verify"];
            if (string.IsNullOrEmpty(verify))
            {
                return Content(ReturnMsg(Enum_Return.失败, "验证码不能为空", null));
            }
            if (Session["vcode"] == null || Session["vcode"].ToString() != verify)
            {
                return Content(ReturnMsg(Enum_Return.失败, "验证码不正确", null));
            }

            string md5pwd = pwd.Md5();

            var model = db.guanwang_user.Where(c => c.userName == name && c.userPwd == md5pwd).FirstOrDefault();
            if (model == null)
            {
                return Content(ReturnMsg(Enum_Return.失败, "用户名或密码不正确", null));
            }

            string check = Request["check"];
            if (check == "true")//记住7天
            {
                HttpCookie username = new HttpCookie("username", name.EncryptStr());
                username.Expires = DateTime.Now.AddDays(7);
                Response.Cookies.Add(username);

                HttpCookie password = new HttpCookie("password", md5pwd.EncryptStr());
                password.Expires = DateTime.Now.AddDays(7);
                Response.Cookies.Add(password);
            }

            Session.Remove("vcode");

            Session["manager_user_model"] = model;
            model.loginTime = DateTime.Now;
            db.SaveChanges();

            return Content(ReturnMsg(Enum_Return.成功, "登录成功", null));
        }
        #endregion

        #region 忘记密码
        public ActionResult ForgetPwd()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgetPwd(FormCollection collection)
        {
            string name = Request["username"];
            if (string.IsNullOrEmpty(name))
            {
                return Content(ReturnMsg(Enum_Return.失败, "账号不能为空", null));
            }
            string email = Request["email"];
            if (string.IsNullOrEmpty(email))
            {
                return Content(ReturnMsg(Enum_Return.失败, "邮箱不能为空", null));
            }
            string verify = Request["verify"];
            if (string.IsNullOrEmpty(verify))
            {
                return Content(ReturnMsg(Enum_Return.失败, "验证码不能为空", null));
            }
            if (Session["vcode"] == null || Session["vcode"].ToString() != verify)
            {
                return Content(ReturnMsg(Enum_Return.失败, "验证码不正确", null));
            }

            var model = db.guanwang_user.Where(c => c.userName == name && c.userEmail == email).FirstOrDefault();
            if (model == null)
            {
                return Content(ReturnMsg(Enum_Return.失败, "验证失败", null));
            }


            #region 重置密码并发送给用户
            //重置密码
            Random rdm = new Random();
            int pwd = rdm.Next(100000,999999);

            string md5pwd = pwd.ToString().Md5();
            model.userPwd = md5pwd;
            model.updateTime = DateTime.Now;
            db.SaveChanges();

            //获取内容
            string htmlbody = MailHelper.CreateHtmlPage(name, pwd.ToString());
            //调用邮件类发送邮件
            bool send = MailHelper.Send(email, "捧起网后台找回密码", htmlbody);
            if (send)//发送成功
            {
                return Content(ReturnMsg(Enum_Return.成功, "新的密码已发送到您的邮箱", email));
            }
            else
            {
                return Content(ReturnMsg(Enum_Return.失败, "重置失败", null));
            }
            #endregion
        }
        #endregion

        #region 验证码
        public ActionResult Vcode()
        {
            var code = CaptchaHelper.CreateRandomCode(4);
            Session["vcode"] = code;
            var img = CaptchaHelper.DrawImage(code, 20, background: Color.White);
            return File(img, "Image/jpeg");
        }
        #endregion
    }
}
