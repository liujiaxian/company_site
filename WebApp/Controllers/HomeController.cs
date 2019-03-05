using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Model;
using Newtonsoft.Json;
using Utility;

namespace WebApp.Controllers
{
    public class HomeController : DefaultController
    {
        //
        // GET: /Home/

        #region 首页
        public ActionResult Index()
        {
            //轮播
            var bannerlist = db.guanwang_banner_index.Where(c => c.isShow == true).OrderBy(c => c.weight).ToList();

            ViewData["bannerlist"] = bannerlist.Count <= 0 ? null : bannerlist;

            //标题
            var model = db.guanwang_title.Where(c => c.typeid == (int)Enum_Title_Type.首页).FirstOrDefault();
            ViewData["model"] = model;

            //服务领域
            var servicelist = db.guanwang_service.Where(c => true).ToList();
            ViewData["servicelist"] = servicelist;

            //最新项目
            var caselist = db.guanwang_case.Where(c => true).OrderBy(c => c.weight).Take(8).ToList();
            ViewData["caselist"] = caselist;

            //擅长领域
            var goodlist = db.guanwang_good.Where(c => true).ToList();
            ViewData["goodlist"] = goodlist;

            //技能
            var skilllist = db.guanwang_skill.Where(c => true).ToList();
            ViewData["skilllist"] = skilllist;

            //喜欢我们
            var likelist = db.guanwang_like.Where(c => true).ToList();
            ViewData["likelist"] = likelist;

            //产品类型
            var producttypelist = db.guanwang_product_type.Where(c => true).ToList();
            ViewData["producttypelist"] = producttypelist;

            //产品介绍
            var productlist = db.guanwang_product.Where(c => true).ToList();
            ViewData["productlist"] = productlist;

            //推荐项目
            var peoplelist = db.guanwang_evaluation.Where(c => c.isRecommend == true).OrderByDescending(c => c.evaluationID).Take(2).ToList();
            ViewData["peoplelist"] = peoplelist;

            //合作伙伴
            var partnerlist = db.guanwang_partner.Where(c => c.isShow == true).OrderByDescending(c => c.partnerID).Take(5).ToList();
            ViewData["partnerlist"] = partnerlist;

            return View();
        }
        #endregion

        #region 关于
        public ActionResult AboutUS()
        {
            //轮播
            var bannerlist = db.guanwang_banner_about.Where(c => c.isShow == true).OrderBy(c => c.weight).ToList();

            ViewData["bannerlist"] = bannerlist.Count <= 0 ? null : bannerlist;

            //标题
            var model = db.guanwang_title.Where(c => c.typeid == (int)Enum_Title_Type.关于我们).FirstOrDefault();
            ViewData["model"] = model;

            //技能
            var skilllist = db.guanwang_skill.Where(c => true).ToList();
            ViewData["skilllist"] = skilllist;

            //团队
            var teamlist = db.guanwang_user.Where(c => c.userStatus == true).OrderBy(c => c.userID).Take(4).ToList();
            ViewData["teamlist"] = teamlist;
            return View();
        }
        #endregion

        #region 服务
        public ActionResult Services()
        {
            //标题
            var model = db.guanwang_title.Where(c => c.typeid == (int)Enum_Title_Type.服务领域).FirstOrDefault();
            ViewData["model"] = model;

            var list = db.guanwang_service.Where(c => true).ToList();
            ViewData["list"] = list;

            var clientlist = db.guanwang_evaluation.Where(c => true).ToList();
            ViewData["clientlist"] = clientlist;
            return View();
        }
        #endregion

        #region 项目案例
        public ActionResult Cases()
        {
            //标题
            var model = db.guanwang_title.Where(c => c.typeid == (int)Enum_Title_Type.项目案例).FirstOrDefault();
            ViewData["model"] = model;

            //类型
            var typelist = db.guanwang_case_type.Where(c => true).ToList();
            ViewData["typelist"] = typelist;

            //项目
            var list = db.guanwang_case.Where(c => true).OrderBy(c => c.weight).ToList();
            ViewData["list"] = list;

            return View();
        }
        #endregion

        #region 官方博客
        public int count = 0;
        public static int pageSize = 10;
        public ActionResult Blog(int? id)
        {
            //标题
            //var model = db.guanwang_title.Where(c => c.typeid == (int)Enum_Title_Type.官方博客).FirstOrDefault();
            //ViewData["model"] = model;

            //博客
            if (string.IsNullOrEmpty(id.ToString()))
            {
                id = 1;
            }
            count = db.guanwang_vyw_blog_type_user.Where(c => c.isDelete == false).Count();
            int pageCount = (int)Math.Ceiling(count * 1.0 / pageSize);
            id = id < 1 ? 1 : id;
            id = id > pageCount ? pageCount : id;
            List<guanwang_vyw_blog_type_user> list = null;
            if (count > 0)
            {
                list = db.guanwang_vyw_blog_type_user.Where(c => c.isDelete == false).OrderByDescending(c => c.blogID).Skip((Convert.ToInt32(id) - 1) * pageSize).Take(pageSize).ToList();
            }

            ViewData["list"] = list;

            ViewBag.PageIndex = id;
            ViewBag.PageCount = pageCount;
            ViewBag.UrlParams = "/home/blog/";

            return View();
        }
        //博客详细
        public ActionResult BlogDetail(int? id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return RedirectToAction("Blog");
            }

            var model = db.guanwang_vyw_blog_type_user.Where(c => c.blogID == id && c.isDelete == false).FirstOrDefault();
            if (model == null)
            {
                return RedirectToAction("Blog");
            }

            //添加浏览记录
            AddIPAddress((int)id);

            ViewData["model"] = model;

            return View();
        }
        #endregion

        #region 定价
        public ActionResult Pricing()
        {
            //标题
            var model = db.guanwang_title.Where(c => c.typeid == (int)Enum_Title_Type.项目定价).FirstOrDefault();
            ViewData["model"] = model;


            //定价
            var list = db.guanwang_pricing.Where(c => true).ToList();
            ViewData["list"] = list;
            return View();
        }
        #endregion

        #region 联系我们
        public ActionResult ContactUS()
        {
            //标题
            var model = db.guanwang_title.Where(c => c.typeid == (int)Enum_Title_Type.联系我们).FirstOrDefault();
            ViewData["model"] = model;

            //联系方式
            var list = db.guanwang_user.Where(c => c.userStatus == true).OrderBy(c => c.userID).Take(4).ToList();
            ViewData["list"] = list;
            return View();
        }
        [HttpPost]
        public ActionResult ContactUS(FormCollection collection)
        {
            string name = Request["name"];
            if (string.IsNullOrEmpty(name))
            {
                return Content(ReturnMsg(Enum_Return.失败, "姓名不能为空", null));
            }
            string email = Request["email"];
            if (string.IsNullOrEmpty(email))
            {
                return Content(ReturnMsg(Enum_Return.失败, "邮箱不能为空", null));
            }
            string phone = Request["phone"];
            string company = Request["company"];

            string subject = Request["subject"];
            if (string.IsNullOrEmpty(subject))
            {
                return Content(ReturnMsg(Enum_Return.失败, "主题不能为空", null));
            }
            string message = Request["message"];
            if (string.IsNullOrEmpty(message))
            {
                return Content(ReturnMsg(Enum_Return.失败, "信息不能为空", null));
            }

            string ipaddress = GetData.GetIPAddress();

            var ismodel = db.guanwang_feedback.Where(c => c.IPAddress == ipaddress).FirstOrDefault();
            if (ismodel != null)
            {
                if (Convert.ToDateTime(ismodel.addTime).AddDays(1) > DateTime.Now)
                {
                    return Content(ReturnMsg(Enum_Return.失败, "对不起！您今天已经留言过了，请明天再来，谢谢您的支持！", null));
                }
            }

            guanwang_feedback model = new guanwang_feedback();
            model.name = name;
            model.email = email;
            model.phone = phone;
            model.company = company;
            model.subject = subject;
            model.message = message;
            model.IPAddress = ipaddress;
            model.addTime = DateTime.Now;
            db.guanwang_feedback.Add(model);
            db.SaveChanges();

            return Content(ReturnMsg(Enum_Return.成功, "非常感谢您的留言！", null));
        }
        #endregion

        #region 添加ip地址
        public void AddIPAddress(int id)
        {
            string ip = GetData.GetIPAddress();
            if (!string.IsNullOrEmpty(ip))
            {
                var isip = db.guanwang_hot.Where(c => c.ipAddress == ip && c.blogID == id).FirstOrDefault();
                if (isip == null)
                {
                    guanwang_hot hot = new guanwang_hot();
                    hot.blogID = (int)id;
                    hot.ipAddress = ip;
                    hot.addTime = DateTime.Now;
                    db.guanwang_hot.Add(hot);

                    #region 同时更新该记录的数据
                    var blog = db.guanwang_blog.Where(c => c.blogID == id).FirstOrDefault();
                    blog.likeCount = blog.likeCount + 1;
                    #endregion
                }
                else
                {
                    #region 统计误差时更正
                    var newcount = db.guanwang_hot.Where(c => c.blogID == id).Count();
                    var blog = db.guanwang_blog.Where(c => c.blogID == id).FirstOrDefault();
                    if (newcount != blog.likeCount)
                    {
                        blog.likeCount = newcount;
                    }
                    #endregion
                }
            }

            db.SaveChanges();
        }
        #endregion

        #region 搜索
        public ActionResult BlogSearch()
        {
            string text = Request["text"];
            string type = Request["type"];
            string tags = Request["tags"];
            string time = Request["time"];
            int typeid = 0;
            string searchparams = "", searchvalue = "", searchConetn = "";
            int page;
            if (!int.TryParse(Request["page"], out page))
            {
                page = 1;
            }
            IQueryable<guanwang_vyw_blog_type_user> result = db.guanwang_vyw_blog_type_user;
            if (!string.IsNullOrEmpty(text))//搜索框
            {
                result = result.Where(c => c.isDelete == false && (c.title.Contains(text) || c.describe.Contains(text) || c.content.Contains(text) || c.tags.Contains(text)));
                searchparams = "text";
                searchvalue = text;
                searchConetn = text;
            }
            else if (int.TryParse(type, out typeid))//类型
            {
                result = result.Where(c => c.isDelete == false && c.typeID == typeid);
                searchparams = "type";
                searchvalue = typeid.ToString();
                searchConetn = result.FirstOrDefault().name + "（类型）";
            }
            else if (!string.IsNullOrEmpty(tags))//标签
            {
                tags = Server.UrlEncode(tags);
                result = result.Where(c => c.isDelete == false && (c.title.Contains(tags) || c.describe.Contains(tags) || c.content.Contains(tags) || c.tags.Contains(tags) || c.tags.Contains(tags)));
                searchparams = "tags";
                searchvalue = tags;
                searchConetn = Request["tags"] + "（标签）";
            }
            else if (!string.IsNullOrEmpty(time) && time.IndexOf('-') != -1)
            {
                string[] str = time.Split('-');
                int day = DateTime.DaysInMonth(Convert.ToInt32(str[0]), Convert.ToInt32(str[1]));
                DateTime newtime = Convert.ToDateTime(time);
                DateTime newtime1 = newtime.AddDays(day);
                result = result.Where(c => c.isDelete == false && (c.addTime >= newtime && c.addTime <= newtime1));
                searchparams = "time";
                searchvalue = time;
                searchConetn = time + "（归档）";
            }
            else
            {
                result = result.Where(c => c.isDelete == false);
                searchparams = "text";
                searchvalue = text;
                searchConetn = text;
            }

            count = result.Count();
            int pageCount = (int)Math.Ceiling(count * 1.0 / pageSize);
            page = page < 1 ? 1 : page;
            page = page > pageCount ? pageCount : page;
            List<guanwang_vyw_blog_type_user> list = null;
            if (count > 0)
            {
                list = result.OrderByDescending(c => c.addTime).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            }

            ViewData["list"] = list;
            ViewData["content"] = searchConetn;

            ViewBag.PageIndex = page;
            ViewBag.PageCount = pageCount;
            ViewBag.Count = count;
            ViewBag.SearchParams = searchparams;
            ViewBag.SearchValue = searchvalue;

            return View();
        }
        #endregion

        #region 模板展示
        public ActionResult WebTemplate()
        {
            //类型
            var typelist = db.guanwang_web_type.Where(c => true).OrderBy(c => c.weight).ToList();
            ViewData["typelist"] = typelist;

            //网站
            var list = db.guanwang_vyw_web_type.Where(c => true).OrderByDescending(c => c.websiteID).ToList();
            ViewData["list"] = list;
            return View();
        }

        public ActionResult Template(int? id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return RedirectToAction("WebTemplate");
            }

            var model = db.guanwang_web.Where(c => c.websiteID == id).OrderBy(c=>c.webID).Take(1).FirstOrDefault();
            if (model == null)
            {
                return RedirectToAction("WebTemplate");
            }
            List<guanwang_template> list =new List<guanwang_template>();
            string [] ids = model.template.Split(',');
            for (int i = 0; i < ids.Length-1; i++)
			{
                int curid = Convert.ToInt32(ids[i]);
                var curmodel = db.guanwang_template.Where(c => c.templateID == curid).FirstOrDefault();
                list.Add(curmodel);
			}

            
            ViewData["list"] = list;
            return View();
        }
        #endregion

        public ActionResult Error()
        {
            return View();
        }
    }
}
