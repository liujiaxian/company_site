using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Areas.GuanWang.Controllers
{
    public class BaseManagerController : DefaultManagerController
    {

        public Model.guanwang_user UserInfo { get; set; }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["manager_user_model"] == null)
            {
                if (Request.Cookies["username"]!=null&&Request.Cookies["password"]!=null)
                {
                    string username = Request.Cookies["username"].Value.DecryptStr();
                    string password = Request.Cookies["password"].Value.DecryptStr();

                    var model = db.guanwang_user.Where(c => c.userName == username && c.userPwd == password).FirstOrDefault();
                    if (model==null)//非法
                    {
                        HttpCookie cp1 = Request.Cookies["username"];
                        cp1.Expires = DateTime.Now.AddDays(-1);
                        Response.Cookies.Add(cp1);

                        HttpCookie cp2 = Request.Cookies["password"];
                        cp2.Expires = DateTime.Now.AddDays(-1);
                        Response.Cookies.Add(cp2);
                    }
                    else
                    {
                        Session["manager_user_model"] = model;

                        UserInfo = model;
                    }
                }
                else
                {
                    Response.Write("<script type='text/javascript'>window.parent.location='/GuanWang/ManagerAccount/Login'</script>");
                }
            }
            else
            {
                UserInfo = Session["manager_user_model"] as Model.guanwang_user;
            }
        }
    }
}
