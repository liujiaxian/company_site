using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Model;
using Newtonsoft.Json;
using Utility;

namespace WebApp.Areas.GuanWang.Controllers
{
    public class ManagerHomeController : BaseManagerController
    {
        //
        // GET: /GuanWang/ManagerHome/


        public ActionResult Index()
        {

            if (UserInfo == null)
            {
                return RedirectToAction("Login", "ManagerAccount");
            }
            guanwang_user user = db.guanwang_user.Where(c => c.userID == UserInfo.userID).FirstOrDefault();
            ViewData["name"] = user.userName;
            return View();
        }

        public ActionResult ManagerInfo()
        {
            return View();
        }

        #region 修改密码
        public ActionResult ResetPwd()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ResetPwd(FormCollection collection)
        {
            string pwd = Request["pwd"];
            if (string.IsNullOrEmpty(pwd))
            {
                return Content(ReturnMsg(Enum_Return.失败, "登录密码不能为空", null));
            }
            string md5pwd = pwd.Md5();
            var model = db.guanwang_user.Where(c => c.userName == UserInfo.userName && c.userPwd == md5pwd).FirstOrDefault();
            if (model == null)
            {
                return Content(ReturnMsg(Enum_Return.失败, "登录密码不正确", null));
            }
            string newpwd = Request["newpwd"];
            if (string.IsNullOrEmpty(newpwd))
            {
                return Content(ReturnMsg(Enum_Return.失败, "新的密码不能为空", null));
            }
            if (newpwd.Trim().Length < 6)
            {
                return Content(ReturnMsg(Enum_Return.失败, "新的密码不能小于6个字符", null));
            }
            model.userPwd = newpwd.Md5();
            model.updateTime = DateTime.Now;
            db.SaveChanges();

            return Content(ReturnMsg(Enum_Return.成功, "更新成功", null));
        }
        #endregion

        #region 项目类型管理
        public ActionResult CaseType()
        {
            var list = db.guanwang_case_type.Where(c => true).ToList();
            ViewData["list"] = list;
            return View();
        }

        #region 编辑
        public ActionResult CaseTypeEdit(int? id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return RedirectToAction("CaseType");
            }
            var model = db.guanwang_case_type.Where(c => c.typeID == id).FirstOrDefault();
            if (model == null)
            {
                return RedirectToAction("CaseType");
            }
            ViewData["model"] = model;
            return View();
        }
        [HttpPost]
        public ActionResult CaseTypeEdit()
        {
            int id;
            if (!int.TryParse(Request["id"], out id))
            {
                return Content(ReturnMsg(Enum_Return.失败, "参数错误", null));
            }

            var model = db.guanwang_case_type.Where(c => c.typeID == id).FirstOrDefault();
            if (model == null)
            {
                return Content(ReturnMsg(Enum_Return.失败, "该数据不存在或已被删除", null));
            }

            string name = Request["name"];
            if (string.IsNullOrEmpty(name))
            {
                return Content(ReturnMsg(Enum_Return.失败, "类型名称不能为空", null));
            }

            model.name = name;
            model.updateTime = DateTime.Now;
            db.SaveChanges();

            return Content(ReturnMsg(Enum_Return.成功, "更新成功", null));
        }
        #endregion
        #endregion

        #region 产品类型管理
        public ActionResult ProductType()
        {
            var list = db.guanwang_product_type.Where(c => true).ToList();
            ViewData["list"] = list;
            return View();
        }

        #region 编辑
        public ActionResult ProductTypeEdit(int? id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return RedirectToAction("ProductType");
            }
            var model = db.guanwang_product_type.Where(c => c.typeID == id).FirstOrDefault();
            if (model == null)
            {
                return RedirectToAction("ProductType");
            }
            ViewData["model"] = model;
            return View();
        }
        [HttpPost]
        public ActionResult ProductTypeEdit()
        {
            int id;
            if (!int.TryParse(Request["id"], out id))
            {
                return Content(ReturnMsg(Enum_Return.失败, "参数错误", null));
            }

            var model = db.guanwang_product_type.Where(c => c.typeID == id).FirstOrDefault();
            if (model == null)
            {
                return Content(ReturnMsg(Enum_Return.失败, "该数据不存在或已被删除", null));
            }

            string name = Request["name"];
            if (string.IsNullOrEmpty(name))
            {
                return Content(ReturnMsg(Enum_Return.失败, "类型名称不能为空", null));
            }

            model.name = name;
            model.updateTime = DateTime.Now;
            db.SaveChanges();

            return Content(ReturnMsg(Enum_Return.成功, "更新成功", null));
        }
        #endregion
        #endregion

        #region 博客类型管理
        public ActionResult BlogType()
        {
            var list = db.guanwang_blog_type.Where(c => true).OrderBy(c => c.weight).ToList();
            ViewData["list"] = list;
            return View();
        }

        #region 新增
        public ActionResult BlogTypeAdd()
        {
            return View();
        }
        [HttpPost]
        public ActionResult BlogTypeAdd(FormCollection collection)
        {
            string name = Request["name"];
            if (string.IsNullOrEmpty(name))
            {
                return Content(ReturnMsg(Enum_Return.失败, "类型名称不能为空", null));
            }

            #region 权重处理
            int weight = 0;
            var list = db.guanwang_blog_type.Where(c => true).ToList();
            if (list.Count > 0)
            {
                weight = list.Max(c => c.weight).Value + 1;
            }
            #endregion

            guanwang_blog_type model = new guanwang_blog_type();
            model.name = name;
            model.weight = weight;
            model.addTime = DateTime.Now;
            db.guanwang_blog_type.Add(model);
            db.SaveChanges();

            return Content(ReturnMsg(Enum_Return.成功, "添加成功", null));
        }
        #endregion

        #region 编辑
        public ActionResult BlogTypeEdit(int? id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return RedirectToAction("BlogType");
            }
            var model = db.guanwang_blog_type.Where(c => c.typeID == id).FirstOrDefault();
            if (model == null)
            {
                return RedirectToAction("BlogType");
            }
            ViewData["model"] = model;
            return View();
        }
        [HttpPost]
        public ActionResult BlogTypeEdit()
        {
            int id;
            if (!int.TryParse(Request["id"], out id))
            {
                return Content(ReturnMsg(Enum_Return.失败, "参数错误", null));
            }

            var model = db.guanwang_blog_type.Where(c => c.typeID == id).FirstOrDefault();
            if (model == null)
            {
                return Content(ReturnMsg(Enum_Return.失败, "该数据不存在或已被删除", null));
            }

            string name = Request["name"];
            if (string.IsNullOrEmpty(name))
            {
                return Content(ReturnMsg(Enum_Return.失败, "类型名称不能为空", null));
            }

            model.name = name;
            model.updateTime = DateTime.Now;
            db.SaveChanges();

            return Content(ReturnMsg(Enum_Return.成功, "更新成功", null));
        }
        #endregion

        #region 关于轮播上移
        public ActionResult BlogTypeUp(int? id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return Content(ReturnMsg(Enum_Return.失败, "参数错误", null));
            }
            var model = db.guanwang_blog_type.Where(c => c.typeID == id).FirstOrDefault();
            if (model == null)
            {
                return Content(ReturnMsg(Enum_Return.失败, "该数据不存在或已被删除！", null));
            }
            //判断是否能上移
            int ismove = db.guanwang_blog_type.Where(c => true).Min(c => c.weight).Value;
            if (ismove == model.weight)
            {
                return Content(ReturnMsg(Enum_Return.失败, "已经最顶层了！", null));
            }
            var list = db.guanwang_blog_type.Where(c => true).OrderBy(c => c.weight).ToList();
            foreach (var item in list)
            {
                if (item.typeID == model.typeID)//当前需要置换
                {
                    int index = list.IndexOf(item); //index 为索引值
                    int flag = (int)model.weight;
                    model.weight = list[index - 1].weight;
                    list[index - 1].weight = flag;
                }
            }
            model.updateTime = DateTime.Now;
            db.SaveChanges();
            return Content(ReturnMsg(Enum_Return.成功, "上移成功", null));
        }
        #endregion

        #region 关于轮播下移
        public ActionResult BlogTypeDown(int? id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return Content(ReturnMsg(Enum_Return.失败, "参数错误", null));
            }
            var model = db.guanwang_blog_type.Where(c => c.typeID == id).FirstOrDefault();
            if (model == null)
            {
                return Content(ReturnMsg(Enum_Return.失败, "该数据不存在或已被删除！", null));
            }
            //判断是否能下移
            int ismove = db.guanwang_blog_type.Where(c => true).Max(c => c.weight).Value;
            if (ismove == model.weight)
            {
                return Content(ReturnMsg(Enum_Return.失败, "已经最底层了！", null));
            }

            var list = db.guanwang_blog_type.Where(c => true).OrderBy(c => c.weight).ToList();
            foreach (var item in list)
            {
                if (item.typeID == model.typeID)//当前需要置换
                {
                    int index = list.IndexOf(item); //index 为索引值
                    int flag = (int)model.weight;
                    model.weight = list[index + 1].weight;
                    list[index + 1].weight = flag;
                }
            }
            model.updateTime = DateTime.Now;
            db.SaveChanges();
            return Content(ReturnMsg(Enum_Return.成功, "下移成功", null));
        }
        #endregion

        #region 关于轮播删除
        public ActionResult BlogTypeDelete(int? id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return Content(ReturnMsg(Enum_Return.失败, "参数错误", null));
            }

            var model = db.guanwang_blog_type.Where(c => c.typeID == id).FirstOrDefault();
            if (model == null)
            {
                return Content(ReturnMsg(Enum_Return.失败, "该数据不存在或已被删除", null));
            }
            var blog = db.guanwang_blog.Where(c => c.typeID == id).Count();
            if (blog > 0)
            {
                return Content(ReturnMsg(Enum_Return.失败, "请先删除该类型下的博客", null));
            }


            db.Entry(model).State = System.Data.EntityState.Deleted;
            db.SaveChanges();

            return Content(ReturnMsg(Enum_Return.成功, "删除成功", null));
        }
        #endregion
        #endregion

        #region 网站类型管理
        public ActionResult WebType()
        {
            var list = db.guanwang_web_type.Where(c => true).OrderBy(c => c.weight).ToList();
            ViewData["list"] = list;
            return View();
        }

        #region 新增
        public ActionResult WebTypeAdd()
        {
            return View();
        }
        [HttpPost]
        public ActionResult WebTypeAdd(FormCollection collection)
        {
            string name = Request["name"];
            if (string.IsNullOrEmpty(name))
            {
                return Content(ReturnMsg(Enum_Return.失败, "类型名称不能为空", null));
            }

            #region 权重处理
            int weight = 0;
            var list = db.guanwang_web_type.Where(c => true).ToList();
            if (list.Count > 0)
            {
                weight = list.Max(c => c.weight).Value + 1;
            }
            #endregion

            guanwang_web_type model = new guanwang_web_type();
            model.name = name;
            model.weight = weight;
            model.addTime = DateTime.Now;
            db.guanwang_web_type.Add(model);
            db.SaveChanges();

            return Content(ReturnMsg(Enum_Return.成功, "添加成功", null));
        }
        #endregion

        #region 编辑
        public ActionResult WebTypeEdit(int? id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return RedirectToAction("WebType");
            }
            var model = db.guanwang_web_type.Where(c => c.typeID == id).FirstOrDefault();
            if (model == null)
            {
                return RedirectToAction("WebType");
            }
            ViewData["model"] = model;
            return View();
        }
        [HttpPost]
        public ActionResult WebTypeEdit()
        {
            int id;
            if (!int.TryParse(Request["id"], out id))
            {
                return Content(ReturnMsg(Enum_Return.失败, "参数错误", null));
            }

            var model = db.guanwang_web_type.Where(c => c.typeID == id).FirstOrDefault();
            if (model == null)
            {
                return Content(ReturnMsg(Enum_Return.失败, "该数据不存在或已被删除", null));
            }

            string name = Request["name"];
            if (string.IsNullOrEmpty(name))
            {
                return Content(ReturnMsg(Enum_Return.失败, "类型名称不能为空", null));
            }

            model.name = name;
            model.updateTime = DateTime.Now;
            db.SaveChanges();

            return Content(ReturnMsg(Enum_Return.成功, "更新成功", null));
        }
        #endregion

        #region 关于网站类型上移
        public ActionResult WebTypeUp(int? id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return Content(ReturnMsg(Enum_Return.失败, "参数错误", null));
            }
            var model = db.guanwang_web_type.Where(c => c.typeID == id).FirstOrDefault();
            if (model == null)
            {
                return Content(ReturnMsg(Enum_Return.失败, "该数据不存在或已被删除！", null));
            }
            //判断是否能上移
            int ismove = db.guanwang_web_type.Where(c => true).Min(c => c.weight).Value;
            if (ismove == model.weight)
            {
                return Content(ReturnMsg(Enum_Return.失败, "已经最顶层了！", null));
            }
            var list = db.guanwang_web_type.Where(c => true).OrderBy(c => c.weight).ToList();
            foreach (var item in list)
            {
                if (item.typeID == model.typeID)//当前需要置换
                {
                    int index = list.IndexOf(item); //index 为索引值
                    int flag = (int)model.weight;
                    model.weight = list[index - 1].weight;
                    list[index - 1].weight = flag;
                }
            }
            model.updateTime = DateTime.Now;
            db.SaveChanges();
            return Content(ReturnMsg(Enum_Return.成功, "上移成功", null));
        }
        #endregion

        #region 关于网站类型下移
        public ActionResult WebTypeDown(int? id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return Content(ReturnMsg(Enum_Return.失败, "参数错误", null));
            }
            var model = db.guanwang_web_type.Where(c => c.typeID == id).FirstOrDefault();
            if (model == null)
            {
                return Content(ReturnMsg(Enum_Return.失败, "该数据不存在或已被删除！", null));
            }
            //判断是否能下移
            int ismove = db.guanwang_web_type.Where(c => true).Max(c => c.weight).Value;
            if (ismove == model.weight)
            {
                return Content(ReturnMsg(Enum_Return.失败, "已经最底层了！", null));
            }

            var list = db.guanwang_web_type.Where(c => true).OrderBy(c => c.weight).ToList();
            foreach (var item in list)
            {
                if (item.typeID == model.typeID)//当前需要置换
                {
                    int index = list.IndexOf(item); //index 为索引值
                    int flag = (int)model.weight;
                    model.weight = list[index + 1].weight;
                    list[index + 1].weight = flag;
                }
            }
            model.updateTime = DateTime.Now;
            db.SaveChanges();
            return Content(ReturnMsg(Enum_Return.成功, "下移成功", null));
        }
        #endregion

        #region 关于网站类型删除
        public ActionResult WebTypeDelete(int? id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return Content(ReturnMsg(Enum_Return.失败, "参数错误", null));
            }

            var model = db.guanwang_web_type.Where(c => c.typeID == id).FirstOrDefault();
            if (model == null)
            {
                return Content(ReturnMsg(Enum_Return.失败, "该数据不存在或已被删除", null));
            }
            var blog = db.guanwang_website.Where(c => c.typeID == id).Count();
            if (blog > 0)
            {
                return Content(ReturnMsg(Enum_Return.失败, "请先删除该类型下的网站", null));
            }


            db.Entry(model).State = System.Data.EntityState.Deleted;
            db.SaveChanges();

            return Content(ReturnMsg(Enum_Return.成功, "删除成功", null));
        }
        #endregion
        #endregion

        #region 内容管理

        #region 项目案例管理

        #region 列表展示
        public int count = 0;
        public static int pageSize = 5;
        public ActionResult Case(int? id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                id = 1;
            }
            IQueryable<guanwang_vyw_case_type> result = db.guanwang_vyw_case_type.Where(c => true);

            int count = result.Count();
            int pageCount = (int)Math.Ceiling(count * 1.0 / pageSize);

            id = id < 1 ? 1 : id;
            id = id > pageCount ? pageCount : id;


            List<guanwang_vyw_case_type> list = null;
            if (count > 0)
            {
                list = result.OrderBy(c => c.weight).Skip((Convert.ToInt32(id) - 1) * pageSize).Take(pageSize).ToList();
            }

            ViewData["list"] = list;

            ViewBag.PageCount = pageCount;
            ViewBag.PageIndex = id;
            ViewBag.Count = count;
            ViewBag.UrlParams = "/GuanWang/ManagerHome/Case/";

            return View();
        }
        #endregion

        #region 添加
        public ActionResult CaseAdd()
        {
            //类型
            var typelist = db.guanwang_case_type.Where(c => true).ToList();
            ViewData["typelist"] = typelist;

            return View();
        }
        [HttpPost]
        public ActionResult CaseAdd(FormCollection collection)
        {
            int typeid;
            if (!int.TryParse(Request["typeid"], out typeid))
            {
                return Content(ReturnMsg(Enum_Return.失败, "参数错误", null));
            }

            string name = Request["name"];
            if (string.IsNullOrEmpty(name))
            {
                return Content(ReturnMsg(Enum_Return.失败, "名称不能为空", null));
            }
            string describe = Request["describe"];
            if (string.IsNullOrEmpty(describe))
            {
                return Content(ReturnMsg(Enum_Return.失败, "描述不能为空", null));
            }

            string img = Request["img"];
            if (string.IsNullOrEmpty(img))
            {
                return Content(ReturnMsg(Enum_Return.失败, "请上传图片", null));
            }

            string link = Request["link"];
            if (string.IsNullOrEmpty(link))
            {
                return Content(ReturnMsg(Enum_Return.失败, "链接地址不能为空", null));
            }

            guanwang_case model = new guanwang_case();

            model.title = name;
            model.describe = describe;
            model.image = img;
            model.link = link;
            model.typeID = typeid;
            model.userID = UserInfo.userID;
            model.addTime = DateTime.Now;
            db.guanwang_case.Add(model);
            db.SaveChanges();

            return Content(ReturnMsg(Enum_Return.成功, "添加成功", null));
        }
        #endregion

        #region 编辑
        public ActionResult CaseEdit(int? id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return RedirectToAction("Case");
            }
            var model = db.guanwang_case.Where(c => c.caseID == id).FirstOrDefault();
            if (model == null)
            {
                return RedirectToAction("Case");
            }
            ViewData["model"] = model;

            //类型
            var typelist = db.guanwang_case_type.Where(c => true).ToList();
            ViewData["typelist"] = typelist;

            return View();
        }
        [HttpPost]
        public ActionResult CaseEdit()
        {
            int id;
            if (!int.TryParse(Request["id"], out id))
            {
                return Content(ReturnMsg(Enum_Return.失败, "参数错误", null));
            }
            var model = db.guanwang_case.Where(c => c.caseID == id).FirstOrDefault();
            if (model == null)
            {
                return Content(ReturnMsg(Enum_Return.失败, "该数据不存在或已被删除", null));
            }
            if (UserInfo.userRole != (int)Enum_User_Role.超级管理员 && id != UserInfo.userID)
            {
                return Content(ReturnMsg(Enum_Return.失败, "对不起！您没有权限！", null));
            }
            int typeid;
            if (!int.TryParse(Request["typeid"], out typeid))
            {
                return Content(ReturnMsg(Enum_Return.失败, "参数错误", null));
            }

            string name = Request["name"];
            if (string.IsNullOrEmpty(name))
            {
                return Content(ReturnMsg(Enum_Return.失败, "名称不能为空", null));
            }
            string describe = Request["describe"];
            if (string.IsNullOrEmpty(describe))
            {
                return Content(ReturnMsg(Enum_Return.失败, "描述不能为空", null));
            }

            string img = Request["img"];

            string link = Request["link"];
            if (string.IsNullOrEmpty(link))
            {
                return Content(ReturnMsg(Enum_Return.失败, "链接地址不能为空", null));
            }

            model.title = name;
            model.describe = describe;
            if (!string.IsNullOrEmpty(img))
            {
                model.image = img;
            }
            model.link = link;
            model.typeID = typeid;
            model.updateTime = DateTime.Now;
            db.SaveChanges();

            return Content(ReturnMsg(Enum_Return.成功, "更新成功", null));
        }
        #endregion

        #region 项目案例上移
        [HttpPost]
        public ActionResult CaseUp(int? id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return Content(ReturnMsg(Enum_Return.失败, "参数错误", null));
            }
            var model = db.guanwang_case.Where(c => c.caseID == id).FirstOrDefault();
            if (model == null)
            {
                return Content(ReturnMsg(Enum_Return.失败, "该数据不存在或已被删除！", null));
            }
            //判断是否能上移
            int ismove = db.guanwang_case.Where(c => true).Min(c => c.weight).Value;
            if (ismove == model.weight)
            {
                return Content(ReturnMsg(Enum_Return.失败, "已经最顶层了！", null));
            }
            var list = db.guanwang_case.Where(c => true).OrderBy(c => c.weight).ToList();
            foreach (var item in list)
            {
                if (item.caseID == model.caseID)//当前需要置换
                {
                    int index = list.IndexOf(item); //index 为索引值
                    int flag = (int)model.weight;
                    model.weight = list[index - 1].weight;
                    list[index - 1].weight = flag;
                }
            }
            model.updateTime = DateTime.Now;
            db.SaveChanges();
            return Content(ReturnMsg(Enum_Return.成功, "上移成功", null));
        }
        #endregion

        #region 项目案例下移
        [HttpPost]
        public ActionResult CaseDown(int? id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return Content(ReturnMsg(Enum_Return.失败, "参数错误", null));
            }
            var model = db.guanwang_case.Where(c => c.caseID == id).FirstOrDefault();
            if (model == null)
            {
                return Content(ReturnMsg(Enum_Return.失败, "该数据不存在或已被删除！", null));
            }
            //判断是否能下移
            int ismove = db.guanwang_case.Where(c => true).Max(c => c.weight).Value;
            if (ismove == model.weight)
            {
                return Content(ReturnMsg(Enum_Return.失败, "已经最底层了！", null));
            }

            var list = db.guanwang_case.Where(c => true).OrderBy(c => c.weight).ToList();
            foreach (var item in list)
            {
                if (item.caseID == model.caseID)//当前需要置换
                {
                    int index = list.IndexOf(item); //index 为索引值
                    int flag = (int)model.weight;
                    model.weight = list[index + 1].weight;
                    list[index + 1].weight = flag;
                }
            }
            model.updateTime = DateTime.Now;
            db.SaveChanges();
            return Content(ReturnMsg(Enum_Return.成功, "下移成功", null));
        }
        #endregion

        #region 删除
        public ActionResult CaseDelete(int? id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return Content(ReturnMsg(Enum_Return.失败, "参数错误", null));
            }

            var model = db.guanwang_case.Where(c => c.caseID == id).FirstOrDefault();
            if (model == null)
            {
                return Content(ReturnMsg(Enum_Return.失败, "该数据不存在或已被删除", null));
            }
            if (UserInfo.userRole != (int)Enum_User_Role.超级管理员 && id != UserInfo.userID)
            {
                return Content(ReturnMsg(Enum_Return.失败, "对不起！您没有权限！", null));
            }

            db.Entry(model).State = System.Data.EntityState.Deleted;
            db.SaveChanges();

            return Content(ReturnMsg(Enum_Return.成功, "删除成功", null));
        }
        #endregion
        #endregion

        #region 客户评价管理
        public ActionResult Evaluation()
        {
            var list = db.guanwang_evaluation.Where(c => true).ToList();
            ViewData["list"] = list;
            return View();
        }

        #region 编辑
        public ActionResult EvaluationEdit(int? id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return RedirectToAction("Evaluation");
            }
            var model = db.guanwang_evaluation.Where(c => c.evaluationID == id).FirstOrDefault();
            if (model == null)
            {
                return RedirectToAction("Evaluation");
            }
            ViewData["model"] = model;
            return View();
        }
        [HttpPost]
        public ActionResult EvaluationEdit()
        {
            int id;
            if (!int.TryParse(Request["id"], out id))
            {
                return Content(ReturnMsg(Enum_Return.失败, "参数错误", null));
            }
            var model = db.guanwang_evaluation.Where(c => c.evaluationID == id).FirstOrDefault();
            if (model == null)
            {
                return Content(ReturnMsg(Enum_Return.失败, "该数据不存在或已被删除", null));
            }

            string name = Request["name"];
            if (string.IsNullOrEmpty(name))
            {
                return Content(ReturnMsg(Enum_Return.失败, "名称不能为空", null));
            }
            string describe = Request["describe"];
            if (string.IsNullOrEmpty(describe))
            {
                return Content(ReturnMsg(Enum_Return.失败, "描述不能为空", null));
            }
            string url = Request["url"];
            if (string.IsNullOrEmpty(url))
            {
                return Content(ReturnMsg(Enum_Return.失败, "url不能为空", null));
            }
            string img = Request["img"];

            model.name = name;
            model.describe = describe;
            model.url = url;
            if (!string.IsNullOrEmpty(img))
            {
                model.image = img;
            }
            model.isRecommend = false;
            model.updateTime = DateTime.Now;
            db.SaveChanges();

            return Content(ReturnMsg(Enum_Return.成功, "更新成功", null));
        }
        #endregion

        #region 客户评价更新状态
        public ActionResult EvaluationStatus(int? id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return Content(ReturnMsg(Enum_Return.失败, "参数错误", null));
            }

            var model = db.guanwang_evaluation.Where(c => c.evaluationID == id).FirstOrDefault();
            if (model == null)
            {
                return Content(ReturnMsg(Enum_Return.失败, "参数错误", null));
            }

            if (model.isRecommend == true)
            {
                model.isRecommend = false;
            }
            else
            {
                model.isRecommend = true;
            }

            model.updateTime = DateTime.Now;
            db.SaveChanges();

            return Content(ReturnMsg(Enum_Return.成功, "更新成功", null));
        }
        #endregion
        #endregion

        #region 擅长领域管理
        public ActionResult Good()
        {
            var list = db.guanwang_good.Where(c => true).ToList();
            ViewData["list"] = list;
            return View();
        }

        #region 编辑
        public ActionResult GoodEdit(int? id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return RedirectToAction("Good");
            }
            var model = db.guanwang_good.Where(c => c.goodID == id).FirstOrDefault();
            if (model == null)
            {
                return RedirectToAction("Good");
            }
            ViewData["model"] = model;
            return View();
        }
        [HttpPost]
        public ActionResult GoodEdit()
        {
            int id;
            if (!int.TryParse(Request["id"], out id))
            {
                return Content(ReturnMsg(Enum_Return.失败, "参数错误", null));
            }
            var model = db.guanwang_good.Where(c => c.goodID == id).FirstOrDefault();
            if (model == null)
            {
                return Content(ReturnMsg(Enum_Return.失败, "该数据不存在或已被删除", null));
            }

            string name = Request["name"];
            if (string.IsNullOrEmpty(name))
            {
                return Content(ReturnMsg(Enum_Return.失败, "名称不能为空", null));
            }
            string describe = Request["describe"];
            if (string.IsNullOrEmpty(describe))
            {
                return Content(ReturnMsg(Enum_Return.失败, "描述不能为空", null));
            }

            string img = Request["img"];

            model.title = name;
            model.describe = describe;
            if (!string.IsNullOrEmpty(img))
            {
                model.image = img;
            }
            model.updateTime = DateTime.Now;
            db.SaveChanges();

            return Content(ReturnMsg(Enum_Return.成功, "更新成功", null));
        }
        #endregion
        #endregion

        #region 喜欢我们管理
        public ActionResult Like()
        {
            var list = db.guanwang_like.Where(c => true).ToList();
            ViewData["list"] = list;
            return View();
        }

        #region 编辑
        public ActionResult LikeEdit(int? id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return RedirectToAction("Like");
            }
            var model = db.guanwang_like.Where(c => c.likeID == id).FirstOrDefault();
            if (model == null)
            {
                return RedirectToAction("Like");
            }
            ViewData["model"] = model;
            return View();
        }
        [HttpPost]
        public ActionResult LikeEdit()
        {
            int id;
            if (!int.TryParse(Request["id"], out id))
            {
                return Content(ReturnMsg(Enum_Return.失败, "参数错误", null));
            }
            var model = db.guanwang_like.Where(c => c.likeID == id).FirstOrDefault();
            if (model == null)
            {
                return Content(ReturnMsg(Enum_Return.失败, "该数据不存在或已被删除", null));
            }

            string name = Request["navname"];
            if (string.IsNullOrEmpty(name))
            {
                return Content(ReturnMsg(Enum_Return.失败, "导航名称不能为空", null));
            }
            string title = Request["title"];
            string describe = Request["describe"];
            if (string.IsNullOrEmpty(describe))
            {
                return Content(ReturnMsg(Enum_Return.失败, "描述不能为空", null));
            }

            string img = Request["img"];

            model.title = title;
            model.navTitle = name;
            model.describe = describe;
            if (!string.IsNullOrEmpty(img))
            {
                model.image = img;
            }
            model.updateTime = DateTime.Now;
            db.SaveChanges();

            return Content(ReturnMsg(Enum_Return.成功, "更新成功", null));
        }
        #endregion
        #endregion

        #region 合作伙伴管理
        public ActionResult Partner()
        {
            var list = db.guanwang_partner.Where(c => true).ToList();
            ViewData["list"] = list;
            return View();
        }

        #region 编辑
        public ActionResult PartnerEdit(int? id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return RedirectToAction("Partner");
            }
            var model = db.guanwang_partner.Where(c => c.partnerID == id).FirstOrDefault();
            if (model == null)
            {
                return RedirectToAction("Partner");
            }
            ViewData["model"] = model;
            return View();
        }
        [HttpPost]
        public ActionResult PartnerEdit()
        {
            int id;
            if (!int.TryParse(Request["id"], out id))
            {
                return Content(ReturnMsg(Enum_Return.失败, "参数错误", null));
            }
            var model = db.guanwang_partner.Where(c => c.partnerID == id).FirstOrDefault();
            if (model == null)
            {
                return Content(ReturnMsg(Enum_Return.失败, "该数据不存在或已被删除", null));
            }

            string name = Request["name"];
            if (string.IsNullOrEmpty(name))
            {
                return Content(ReturnMsg(Enum_Return.失败, "名称不能为空", null));
            }
            string url = Request["url"];
            if (string.IsNullOrEmpty(url))
            {
                return Content(ReturnMsg(Enum_Return.失败, "url不能为空", null));
            }

            string img = Request["img"];
            model.name = name;
            model.url = url;
            if (!string.IsNullOrEmpty(img))
            {
                model.image = img;
            }
            model.updateTime = DateTime.Now;
            db.SaveChanges();

            return Content(ReturnMsg(Enum_Return.成功, "更新成功", null));
        }
        #endregion

        #region 合作伙伴更新状态
        public ActionResult PartnerStatus(int? id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return Content(ReturnMsg(Enum_Return.失败, "参数错误", null));
            }

            var model = db.guanwang_partner.Where(c => c.partnerID == id).FirstOrDefault();
            if (model == null)
            {
                return Content(ReturnMsg(Enum_Return.失败, "参数错误", null));
            }

            if (model.isShow == true)
            {
                model.isShow = false;
            }
            else
            {
                model.isShow = true;
            }

            model.updateTime = DateTime.Now;
            db.SaveChanges();

            return Content(ReturnMsg(Enum_Return.成功, "更新成功", null));
        }
        #endregion
        #endregion

        #region 产品介绍管理
        public ActionResult Product()
        {
            var list = db.guanwang_vyw_product_type.Where(c => true).ToList();
            ViewData["list"] = list;
            return View();
        }

        #region 编辑
        public ActionResult ProductEdit(int? id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return RedirectToAction("Product");
            }
            var model = db.guanwang_product.Where(c => c.productID == id).FirstOrDefault();
            if (model == null)
            {
                return RedirectToAction("Product");
            }
            ViewData["model"] = model;

            //类型
            var typelist = db.guanwang_product_type.Where(c => true).ToList();
            ViewData["typelist"] = typelist;

            return View();
        }
        [HttpPost]
        public ActionResult ProductEdit()
        {
            int id;
            if (!int.TryParse(Request["id"], out id))
            {
                return Content(ReturnMsg(Enum_Return.失败, "参数错误", null));
            }
            var model = db.guanwang_product.Where(c => c.productID == id).FirstOrDefault();
            if (model == null)
            {
                return Content(ReturnMsg(Enum_Return.失败, "该数据不存在或已被删除", null));
            }

            int typeid;
            if (!int.TryParse(Request["typeid"], out typeid))
            {
                return Content(ReturnMsg(Enum_Return.失败, "参数错误", null));
            }

            string name = Request["name"];

            string describe = Request["describe"];
            if (string.IsNullOrEmpty(describe))
            {
                return Content(ReturnMsg(Enum_Return.失败, "描述不能为空", null));
            }

            string img = Request["img"];
            model.typeID = typeid;
            model.title = name;
            model.describe = describe;
            if (!string.IsNullOrEmpty(img))
            {
                model.image = img;
            }
            model.updateTime = DateTime.Now;
            db.SaveChanges();

            return Content(ReturnMsg(Enum_Return.成功, "更新成功", null));
        }
        #endregion
        #endregion

        #region 服务领域管理
        public ActionResult Service()
        {
            var list = db.guanwang_service.Where(c => true).ToList();
            ViewData["list"] = list;
            return View();
        }

        #region 编辑
        public ActionResult ServiceEdit(int? id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return RedirectToAction("Service");
            }
            var model = db.guanwang_service.Where(c => c.serviceID == id).FirstOrDefault();
            if (model == null)
            {
                return RedirectToAction("Service");
            }
            ViewData["model"] = model;
            return View();
        }
        [HttpPost]
        public ActionResult ServiceEdit()
        {
            int id;
            if (!int.TryParse(Request["id"], out id))
            {
                return Content(ReturnMsg(Enum_Return.失败, "参数错误", null));
            }
            var model = db.guanwang_service.Where(c => c.serviceID == id).FirstOrDefault();
            if (model == null)
            {
                return Content(ReturnMsg(Enum_Return.失败, "该数据不存在或已被删除", null));
            }

            string name = Request["name"];
            if (string.IsNullOrEmpty(name))
            {
                return Content(ReturnMsg(Enum_Return.失败, "标题不能为空", null));
            }
            string describe = Request["describe"];
            if (string.IsNullOrEmpty(describe))
            {
                return Content(ReturnMsg(Enum_Return.失败, "描述不能为空", null));
            }

            string icon = Request["icon"];
            if (string.IsNullOrEmpty(icon))
            {
                return Content(ReturnMsg(Enum_Return.失败, "图标类不能为空", null));
            }

            model.title = name;
            model.describe = describe;
            model.iconClass = icon;
            model.updateTime = DateTime.Now;
            db.SaveChanges();

            return Content(ReturnMsg(Enum_Return.成功, "更新成功", null));
        }
        #endregion
        #endregion

        #region 技能介绍管理
        public ActionResult Skill()
        {
            var list = db.guanwang_skill.Where(c => true).ToList();
            ViewData["list"] = list;
            return View();
        }

        #region 编辑
        public ActionResult SkillEdit(int? id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return RedirectToAction("Skill");
            }
            var model = db.guanwang_skill.Where(c => c.skillID == id).FirstOrDefault();
            if (model == null)
            {
                return RedirectToAction("Skill");
            }
            ViewData["model"] = model;
            return View();
        }
        [HttpPost]
        public ActionResult SkillEdit()
        {
            int id;
            if (!int.TryParse(Request["id"], out id))
            {
                return Content(ReturnMsg(Enum_Return.失败, "参数错误", null));
            }
            var model = db.guanwang_skill.Where(c => c.skillID == id).FirstOrDefault();
            if (model == null)
            {
                return Content(ReturnMsg(Enum_Return.失败, "该数据不存在或已被删除", null));
            }

            string name = Request["name"];
            if (string.IsNullOrEmpty(name))
            {
                return Content(ReturnMsg(Enum_Return.失败, "名称不能为空", null));
            }
            int percent;
            if (!int.TryParse(Request["percent"], out percent))
            {
                return Content(ReturnMsg(Enum_Return.失败, "百分比格式不正确", null));
            }

            string color = Request["color"];
            if (string.IsNullOrEmpty(color))
            {
                return Content(ReturnMsg(Enum_Return.失败, "颜色类不能为空", null));
            }

            model.name = name;
            model.percent = percent;
            model.colorClass = color;
            model.updateTime = DateTime.Now;
            db.SaveChanges();

            return Content(ReturnMsg(Enum_Return.成功, "更新成功", null));
        }
        #endregion
        #endregion

        #region 项目定价管理
        public ActionResult Pricing()
        {
            var list = db.guanwang_pricing.Where(c => true).ToList();
            ViewData["list"] = list;
            return View();
        }

        #region 编辑
        public ActionResult PricingEdit(int? id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return RedirectToAction("Pricing");
            }
            var model = db.guanwang_pricing.Where(c => c.pricingID == id).FirstOrDefault();
            if (model == null)
            {
                return RedirectToAction("Pricing");
            }
            ViewData["model"] = model;
            return View();
        }
        [HttpPost]
        public ActionResult PricingEdit()
        {
            int id;
            if (!int.TryParse(Request["id"], out id))
            {
                return Content(ReturnMsg(Enum_Return.失败, "参数错误", null));
            }
            var model = db.guanwang_pricing.Where(c => c.pricingID == id).FirstOrDefault();
            if (model == null)
            {
                return Content(ReturnMsg(Enum_Return.失败, "该数据不存在或已被删除", null));
            }

            string name = Request["name"];
            if (string.IsNullOrEmpty(name))
            {
                return Content(ReturnMsg(Enum_Return.失败, "标题不能为空", null));
            }

            string price = Request["price"];
            if (string.IsNullOrEmpty(price))
            {
                return Content(ReturnMsg(Enum_Return.失败, "价格不能为空", null));
            }

            string remark1 = Request["remark1"];
            if (string.IsNullOrEmpty(remark1))
            {
                return Content(ReturnMsg(Enum_Return.失败, "介绍1不能为空", null));
            }
            string remark2 = Request["remark2"];
            if (string.IsNullOrEmpty(remark2))
            {
                return Content(ReturnMsg(Enum_Return.失败, "介绍2不能为空", null));
            }
            string remark3 = Request["remark3"];
            if (string.IsNullOrEmpty(remark3))
            {
                return Content(ReturnMsg(Enum_Return.失败, "介绍3不能为空", null));
            }
            string remark4 = Request["remark4"];
            if (string.IsNullOrEmpty(remark4))
            {
                return Content(ReturnMsg(Enum_Return.失败, "介绍4不能为空", null));
            }
            string remark5 = Request["remark5"];
            if (string.IsNullOrEmpty(remark5))
            {
                return Content(ReturnMsg(Enum_Return.失败, "介绍5不能为空", null));
            }
            string url = Request["url"];
            string img = Request["img"];
            model.title = name;
            model.price = price;
            model.remark1 = remark1;
            model.remark2 = remark2;
            model.remark3 = remark3;
            model.remark4 = remark4;
            model.remark5 = remark5;
            if (!string.IsNullOrEmpty(img))
            {
                model.image = img;
            }
            model.url = url;
            model.updateTime = DateTime.Now;
            db.SaveChanges();

            return Content(ReturnMsg(Enum_Return.成功, "更新成功", null));
        }
        #endregion
        #endregion

        #region 博客管理
        public ActionResult Blog(int? id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                id = 1;
            }

            int count = db.guanwang_vyw_blog_type_user.Where(c => c.isDelete == false).Count();
            int pageCount = (int)Math.Ceiling(count * 1.0 / pageSize);

            id = id < 1 ? 1 : id;
            id = id > pageCount ? pageCount : id;

            List<guanwang_vyw_blog_type_user> list = null;
            if (count > 0)
            {
                list = db.guanwang_vyw_blog_type_user.Where(c => c.isDelete == false).OrderByDescending(c => c.blogID).Skip((Convert.ToInt32(id) - 1) * pageSize).Take(pageSize).ToList();
            }

            ViewData["list"] = list;

            ViewBag.PageCount = pageCount;
            ViewBag.PageIndex = id;
            ViewBag.Count = count;
            ViewBag.UrlParams = "/GuanWang/ManagerHome/Blog/";

            return View();
        }

        #region 添加
        public ActionResult BlogAdd()
        {
            //类型
            var typelist = db.guanwang_blog_type.Where(c => true).ToList();
            ViewData["typelist"] = typelist;

            return View();
        }
        [HttpPost]
        public ActionResult BlogAdd(FormCollection collection)
        {
            int typeid;
            if (!int.TryParse(Request["typeid"], out typeid))
            {
                return Content(ReturnMsg(Enum_Return.失败, "参数错误", null));
            }

            string name = Request["name"];
            if (string.IsNullOrEmpty(name))
            {
                return Content(ReturnMsg(Enum_Return.失败, "标题不能为空", null));
            }
            string describe = Request["describe"];
            if (string.IsNullOrEmpty(describe))
            {
                return Content(ReturnMsg(Enum_Return.失败, "描述不能为空", null));
            }

            string img = Request["img"];

            string content = Request["content"];
            if (string.IsNullOrEmpty(content))
            {
                return Content(ReturnMsg(Enum_Return.失败, "内容不能为空", null));
            }

            string tags = Request["tags"];

            guanwang_blog model = new guanwang_blog();

            model.title = name;
            model.describe = describe;
            model.image = img;
            model.content = content;
            model.typeID = typeid;
            model.userID = UserInfo.userID;
            model.tags = tags;
            model.likeCount = 0;
            model.isDelete = false;
            model.addTime = DateTime.Now;
            db.guanwang_blog.Add(model);
            db.SaveChanges();

            return Content(ReturnMsg(Enum_Return.成功, "添加成功", null));
        }
        #endregion

        #region 编辑
        public ActionResult BlogEdit(int? id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return RedirectToAction("Blog");
            }
            var model = db.guanwang_blog.Where(c => c.blogID == id).FirstOrDefault();
            if (model == null)
            {
                return RedirectToAction("Blog");
            }
            ViewData["model"] = model;

            //类型
            var typelist = db.guanwang_blog_type.Where(c => true).ToList();
            ViewData["typelist"] = typelist;

            return View();
        }
        [HttpPost]
        public ActionResult BlogEdit()
        {
            int id;
            if (!int.TryParse(Request["id"], out id))
            {
                return Content(ReturnMsg(Enum_Return.失败, "参数错误", null));
            }
            var model = db.guanwang_blog.Where(c => c.blogID == id).FirstOrDefault();
            if (model == null)
            {
                return Content(ReturnMsg(Enum_Return.失败, "该数据不存在或已被删除", null));
            }
            if (UserInfo.userRole != (int)Enum_User_Role.超级管理员 && UserInfo.userID != id)
            {
                return Content(ReturnMsg(Enum_Return.失败, "对不起！您没有权限！", null));
            }
            int typeid;
            if (!int.TryParse(Request["typeid"], out typeid))
            {
                return Content(ReturnMsg(Enum_Return.失败, "参数错误", null));
            }

            string name = Request["name"];
            if (string.IsNullOrEmpty(name))
            {
                return Content(ReturnMsg(Enum_Return.失败, "标题不能为空", null));
            }
            string describe = Request["describe"];
            if (string.IsNullOrEmpty(describe))
            {
                return Content(ReturnMsg(Enum_Return.失败, "描述不能为空", null));
            }

            string img = Request["img"];

            string tags = Request["tags"];

            string content = Request["content"];
            if (string.IsNullOrEmpty(content))
            {
                return Content(ReturnMsg(Enum_Return.失败, "内容不能为空", null));
            }

            model.title = name;
            model.describe = describe;
            if (!string.IsNullOrEmpty(img))
            {
                model.image = img;
            }
            model.content = content;
            model.typeID = typeid;
            model.tags = tags;
            model.updateTime = DateTime.Now;
            db.SaveChanges();

            return Content(ReturnMsg(Enum_Return.成功, "更新成功", null));
        }
        #endregion

        #region 删除
        public ActionResult BlogDelete(int? id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return Content(ReturnMsg(Enum_Return.失败, "参数错误", null));
            }

            var model = db.guanwang_blog.Where(c => c.blogID == id).FirstOrDefault();
            if (model == null)
            {
                return Content(ReturnMsg(Enum_Return.失败, "该数据不存在或已被删除", null));
            }
            if (UserInfo.userRole != (int)Enum_User_Role.超级管理员 && UserInfo.userID != id)
            {
                return Content(ReturnMsg(Enum_Return.失败, "对不起！您没有权限！", null));
            }
            model.isDelete = true;
            model.updateTime = DateTime.Now;
            db.SaveChanges();

            return Content(ReturnMsg(Enum_Return.成功, "删除成功", null));
        }
        #endregion
        #endregion
        #endregion

        #region 模板管理

        #region 模板标题管理

        #region 列表展示
        public ActionResult TemplateTitle()
        {
            var list = db.guanwang_template_title.Where(c => true).ToList();
            ViewData["list"] = list;
            return View();
        }
        #endregion

        #region 添加
        public ActionResult TemplateTitleAdd()
        {
            return View();
        }
        [HttpPost]
        public ActionResult TemplateTitleAdd(FormCollection collection)
        {
            string title = Request["title"];
            if (string.IsNullOrEmpty(title))
            {
                return Content(ReturnMsg(Enum_Return.失败, "标题不能为空", null));
            }

            guanwang_template_title model = new guanwang_template_title();
            model.title = title;
            model.addTime = DateTime.Now;
            db.guanwang_template_title.Add(model);
            db.SaveChanges();

            return Content(ReturnMsg(Enum_Return.成功, "标题添加成功", null));
        }
        #endregion

        #region 编辑
        public ActionResult TemplateTitleEdit(int? id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return RedirectToAction("TemplateTitle");
            }
            var model = db.guanwang_template_title.Where(c => c.pID == id).FirstOrDefault();
            if (model == null)
            {
                return RedirectToAction("TemplateTitle");
            }
            ViewData["model"] = model;
            return View();
        }
        [HttpPost]
        public ActionResult TemplateTitleEdit()
        {
            int id;
            if (!int.TryParse(Request["id"], out id))
            {
                return Content(ReturnMsg(Enum_Return.失败, "参数错误", null));
            }
            var model = db.guanwang_template_title.Where(c => c.pID == id).FirstOrDefault();
            if (model == null)
            {
                return Content(ReturnMsg(Enum_Return.失败, "该数据不存在或已被删除", null));
            }
            string title = Request["title"];
            if (string.IsNullOrEmpty(title))
            {
                return Content(ReturnMsg(Enum_Return.失败, "标题不能为空", null));
            }

            model.title = title;
            model.updateTime = DateTime.Now;
            db.SaveChanges();

            return Content(ReturnMsg(Enum_Return.成功, "标题更新成功", null));
        }
        #endregion

        #region 删除
        public ActionResult TemplateTitleDelete(int? id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return Content(ReturnMsg(Enum_Return.失败, "参数错误", null));
            }

            var model = db.guanwang_template_title.Where(c => c.pID == id).FirstOrDefault();
            if (model == null)
            {
                return Content(ReturnMsg(Enum_Return.失败, "该数据不存在或已被删除", null));
            }
            if (UserInfo.userRole != (int)Enum_User_Role.超级管理员 && UserInfo.userID != id)
            {
                return Content(ReturnMsg(Enum_Return.失败, "对不起！您没有权限！", null));
            }

            var count = db.guanwang_template.Where(c => c.pID == id).Count();
            if (count > 0)
            {
                return Content(ReturnMsg(Enum_Return.失败, "对不起！请先删除模板！", null));
            }

            db.Entry(model).State = System.Data.EntityState.Deleted;
            db.SaveChanges();

            return Content(ReturnMsg(Enum_Return.成功, "删除成功", null));
        }
        #endregion
        #endregion

        #region 模板内容管理

        #region 展示列表
        public ActionResult TemplateContent()
        {
            int page;
            if (!int.TryParse(Request["page"], out page))
            {
                page = 1;
            }
            int typeid;
            if (!int.TryParse(Request["typeid"], out typeid))
            {
                typeid = 0;
            }

            IQueryable<guanwang_vyw_template_title> newlist = db.guanwang_vyw_template_title.Where(c => true);
            List<guanwang_vyw_template_title> list = null;
            if (typeid > 0)
            {
                newlist = newlist.Where(c => c.pID == typeid);
            }

            int count = newlist.Count();
            int pageCount = (int)Math.Ceiling(count * 1.0 / pageSize);
            page = page < 1 ? 1 : page;
            page = page > pageCount ? pageCount : page;

            if (count > 0)
            {
                list = newlist.OrderByDescending(c => c.addTime).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            }

            ViewData["list"] = list;

            //类型
            var typelist = db.guanwang_template_title.Where(c => true).ToList();
            ViewData["typelist"] = typelist;

            ViewBag.PageCount = pageCount;
            ViewBag.PageIndex = page;
            ViewBag.Count = count;
            ViewBag.UrlParams = typeid.ToString();

            return View();
        }
        #endregion

        #region 添加
        public ActionResult TemplateContentAdd()
        {
            //类型
            var typelist = db.guanwang_template_title.Where(c => true).ToList();
            ViewData["list"] = typelist;
            return View();
        }
        [HttpPost]
        public ActionResult TemplateContentAdd(FormCollection collection)
        {
            string height = Request["height"];
            int typeid = Convert.ToInt32(Request["typeid"]);

            string imgurl = Request["imgurl"];
            if (string.IsNullOrEmpty(imgurl))
            {
                return Content(ReturnMsg(Enum_Return.失败, "请上传图片", null));
            }
            string html = Request["html"];
            if (string.IsNullOrEmpty(html))
            {
                return Content(ReturnMsg(Enum_Return.失败, "html不能为空", null));
            }


            guanwang_template model = new guanwang_template();
            model.height = height;
            model.pID = typeid;
            model.html = html;
            model.thumbnail = imgurl;
            model.addTime = DateTime.Now;
            db.guanwang_template.Add(model);
            db.SaveChanges();

            return Content(ReturnMsg(Enum_Return.成功, "添加成功", null));
        }
        #endregion

        #region 编辑
        public ActionResult TemplateContentEdit(int? id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return RedirectToAction("TemplateContent");
            }
            var model = db.guanwang_template.Where(c => c.templateID == id).FirstOrDefault();
            if (model == null)
            {
                return RedirectToAction("TemplateContent");
            }
            ViewData["model"] = model;

            //类型
            var typelist = db.guanwang_template_title.Where(c => true).ToList();
            ViewData["list"] = typelist;
            return View();
        }
        [HttpPost]
        public ActionResult TemplateContentEdit()
        {
            int id;
            if (!int.TryParse(Request["id"], out id))
            {
                return Content(ReturnMsg(Enum_Return.失败, "参数错误", null));
            }
            var model = db.guanwang_template.Where(c => c.templateID == id).FirstOrDefault();
            if (model == null)
            {
                return Content(ReturnMsg(Enum_Return.失败, "该数据不存在或已被删除", null));
            }
            string height = Request["height"];
            int typeid = Convert.ToInt32(Request["typeid"]);

            string imgurl = Request["imgurl"];

            string html = Request["html"];
            if (string.IsNullOrEmpty(html))
            {
                return Content(ReturnMsg(Enum_Return.失败, "html不能为空", null));
            }

            model.height = height;
            model.pID = typeid;
            model.html = html;
            if (!string.IsNullOrEmpty(imgurl))
            {
                model.thumbnail = imgurl;
            }
            model.updateTime = DateTime.Now;
            db.SaveChanges();

            return Content(ReturnMsg(Enum_Return.成功, "更新成功", null));
        }
        #endregion

        #endregion

        #region 网站管理

        #region 列表展示
        public ActionResult WebSite()
        {
            int page;
            if (!int.TryParse(Request["page"], out page))
            {
                page = 1;
            }
            int typeid;
            if (!int.TryParse(Request["typeid"], out typeid))
            {
                typeid = 0;
            }

            IQueryable<guanwang_vyw_web_type> newlist = db.guanwang_vyw_web_type.Where(c => true);
            List<guanwang_vyw_web_type> list = null;
            if (typeid > 0)
            {
                newlist = newlist.Where(c => c.typeID == typeid);
            }

            int count = newlist.Count();
            int pageCount = (int)Math.Ceiling(count * 1.0 / pageSize);
            page = page < 1 ? 1 : page;
            page = page > pageCount ? pageCount : page;

            if (count > 0)
            {
                list = newlist.OrderByDescending(c => c.addTime).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            }

            ViewData["list"] = list;

            //类型
            var typelist = db.guanwang_web_type.Where(c => true).OrderBy(c => c.weight).ToList();
            ViewData["typelist"] = typelist;

            ViewBag.PageCount = pageCount;
            ViewBag.PageIndex = page;
            ViewBag.Count = count;
            ViewBag.UrlParams = typeid.ToString();

            return View();
        }
        #endregion

        #region 添加
        public ActionResult WebSiteAdd()
        {
            //类型
            var typelist = db.guanwang_web_type.Where(c => true).OrderBy(c => c.weight).ToList();
            ViewData["list"] = typelist;
            return View();
        }
        [HttpPost]
        public ActionResult WebSiteAdd(FormCollection collection)
        {
            int typeid = Convert.ToInt32(Request["typeid"]);
            string title = Request["name"];
            if (string.IsNullOrEmpty(title))
            {
                return Content(ReturnMsg(Enum_Return.失败, "网站名称不能为空", null));
            }
            string imgurl = Request["imgurl"];
            if (string.IsNullOrEmpty(imgurl))
            {
                return Content(ReturnMsg(Enum_Return.失败, "请上传图片", null));
            }

            guanwang_website model = new guanwang_website();
            model.title = title;
            model.image = imgurl;
            model.typeID = typeid;
            model.addTime = DateTime.Now;
            db.guanwang_website.Add(model);
            db.SaveChanges();

            return Content(ReturnMsg(Enum_Return.成功, "网站添加成功", null));
        }
        #endregion

        #region 编辑
        public ActionResult WebSiteEdit(int? id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return RedirectToAction("WebSite");
            }
            var model = db.guanwang_website.Where(c => c.websiteID == id).FirstOrDefault();
            if (model == null)
            {
                return RedirectToAction("WebSite");
            }
            ViewData["model"] = model;

            //类型
            var typelist = db.guanwang_web_type.Where(c => true).OrderBy(c => c.weight).ToList();
            ViewData["list"] = typelist;
            return View();
        }
        [HttpPost]
        public ActionResult WebSiteEdit()
        {
            int id;
            if (!int.TryParse(Request["id"], out id))
            {
                return Content(ReturnMsg(Enum_Return.失败, "参数错误", null));
            }
            var model = db.guanwang_website.Where(c => c.websiteID == id).FirstOrDefault();
            if (model == null)
            {
                return Content(ReturnMsg(Enum_Return.失败, "该数据不存在或已被删除", null));
            }
            if (UserInfo.userRole != (int)Enum_User_Role.超级管理员 && UserInfo.userID != model.userID)
            {
                return Content(ReturnMsg(Enum_Return.失败, "对不起！您没有权限改其它管理员的信息！", null));
            }
            int typeid = Convert.ToInt32(Request["typeid"]);
            string title = Request["name"];
            if (string.IsNullOrEmpty(title))
            {
                return Content(ReturnMsg(Enum_Return.失败, "网站名称不能为空", null));
            }
            string imgurl = Request["imgurl"];

            model.title = title;
            if (!string.IsNullOrEmpty(imgurl))
            {
                model.image = imgurl;
            }
            model.typeID = typeid;
            model.updateTime = DateTime.Now;
            db.SaveChanges();

            return Content(ReturnMsg(Enum_Return.成功, "更新成功", null));
        }
        #endregion

        #region 删除
        public ActionResult WebSiteDelete(int? id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return Content(ReturnMsg(Enum_Return.失败, "参数错误", null));
            }

            var model = db.guanwang_website.Where(c => c.websiteID == id).FirstOrDefault();
            if (model == null)
            {
                return Content(ReturnMsg(Enum_Return.失败, "该数据不存在或已被删除", null));
            }
            if (UserInfo.userRole != (int)Enum_User_Role.超级管理员 && UserInfo.userID != model.userID)
            {
                return Content(ReturnMsg(Enum_Return.失败, "对不起！您没有权限删除其它管理员的信息！", null));
            }

            var count = db.guanwang_web.Where(c => c.websiteID == id).Count();
            if (count > 0)
            {
                return Content(ReturnMsg(Enum_Return.失败, "对不起！请先删除该网站下的页面！", null));
            }

            db.Entry(model).State = System.Data.EntityState.Deleted;
            db.SaveChanges();

            return Content(ReturnMsg(Enum_Return.成功, "删除成功", null));
        }
        #endregion
        #endregion

        #region 网站页面管理

        #region 列表展示
        public ActionResult WebSitePage(int? id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return RedirectToAction("WebSite");
            }
           
            var list = db.guanwang_web.Where(c => c.websiteID == id).ToList();
            ViewData["list"] = list;
            return View();
        }
        #endregion

        #region 添加
        public ActionResult WebSitePageAdd()
        {
            ViewData["templatejson"] = GetTemplateType();

            return View();
        }
        [HttpPost]
        public ActionResult WebSitePageAdd(FormCollection collection)
        {
            int websiteid = Convert.ToInt32(Request["websiteid"]);

            string title = Request["title"];
            if (string.IsNullOrEmpty(title))
            {
                return Content(ReturnMsg(Enum_Return.失败, "网站名称不能为空", null));
            }

            string ids = Request["ids"];
            if (string.IsNullOrEmpty(ids))
            {
                return Content(ReturnMsg(Enum_Return.失败, "网站页面元素不能为空", null));
            }

            guanwang_web model = new guanwang_web();
            model.webName = title;
            model.websiteID = websiteid;
            model.template = ids;
            model.userID = UserInfo.userID;
            model.addTime = DateTime.Now;
            db.guanwang_web.Add(model);
            db.SaveChanges();

            return Content(ReturnMsg(Enum_Return.成功, "页面添加成功", null));
        }
        #endregion

        #region 编辑
        public ActionResult WebSitePageEdit(int? id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return RedirectToAction("WebSitePage");
            }
            var model = db.guanwang_web.Where(c => c.webID == id).FirstOrDefault();
            if (model == null)
            {
                return RedirectToAction("WebSitePage");
            }
            List<guanwang_vyw_template_title> list = new List<guanwang_vyw_template_title>();
            if (model.template.IndexOf(',')!=-1)
            {
                string[] str = model.template.Split(',');
                for (int i = 0; i < str.Length-1; i++)
                {
                    int tid = Convert.ToInt32(str[i]);
                    var tmodel = db.guanwang_vyw_template_title.Where(c => c.templateID == tid).FirstOrDefault();
                    list.Add(tmodel);
                }
            }


            ViewData["list"] = list;
            ViewData["webid"] = model.webID;
            ViewData["name"] = model.webName;
            ViewData["templatejson"] = GetTemplateType();
            return View();
        }
        [HttpPost]
        public ActionResult WebSitePageEdit()
        {
            int id;
            if (!int.TryParse(Request["webid"], out id))
            {
                return Content(ReturnMsg(Enum_Return.失败, "参数错误", null));
            }
            var model = db.guanwang_web.Where(c => c.webID == id).FirstOrDefault();
            if (model == null)
            {
                return Content(ReturnMsg(Enum_Return.失败, "该数据不存在或已被删除", null));
            }

            string title = Request["title"];
            if (string.IsNullOrEmpty(title))
            {
                return Content(ReturnMsg(Enum_Return.失败, "网站名称不能为空", null));
            }

            string ids = Request["ids"];
            if (string.IsNullOrEmpty(ids))
            {
                return Content(ReturnMsg(Enum_Return.失败, "网站页面元素不能为空", null));
            }

            model.webName = title;
            model.template = ids;
            model.updateTime = DateTime.Now;
            db.SaveChanges();

            return Content(ReturnMsg(Enum_Return.成功, "更新成功", null));
        }
        #endregion

        #region 删除
        public ActionResult WebSitePageDelete(int? id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return Content(ReturnMsg(Enum_Return.失败, "参数错误", null));
            }

            var model = db.guanwang_web.Where(c => c.webID == id).FirstOrDefault();
            if (model == null)
            {
                return Content(ReturnMsg(Enum_Return.失败, "该数据不存在或已被删除", null));
            }
            if (UserInfo.userRole != (int)Enum_User_Role.超级管理员 && UserInfo.userID != id)
            {
                return Content(ReturnMsg(Enum_Return.失败, "对不起！您没有权限！", null));
            }

            db.Entry(model).State = System.Data.EntityState.Deleted;
            db.SaveChanges();

            return Content(ReturnMsg(Enum_Return.成功, "删除成功", null));
        }
        #endregion

        #region 获取模板类型数据
        public string GetTemplateType()
        {
            var list = db.guanwang_template_title.Where(c => true).ToList();
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"elements\": {");
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    sb.Append("\"" + item.title + "\":[");

                    var list1 = db.guanwang_template.Where(c => c.pID == item.pID).ToList();
                    if (list1.Count > 0)
                    {
                        foreach (var item1 in list1)
                        {
                            sb.Append("{\"url\":\"/center/template/" + item1.templateID + ".shtml\", ");
                            if (!string.IsNullOrEmpty(item1.height))
                            {
                                sb.Append("\"height\":" + item1.height + ",");
                            }
                            sb.Append("\"thumbnail\":\"" + item1.thumbnail + "\"}");
                            if (list1.IndexOf(item1) + 1 != list1.Count)
                            {
                                sb.Append(",");
                            }
                        }
                    }
                    sb.Append("]");
                    if (list.IndexOf(item) + 1 != list.Count)
                    {
                        sb.Append(",");
                    }
                }
            }
            sb.Append("}}");

            return sb.ToString();
        }
        #endregion

        #endregion

        #endregion

        #region 用户管理

        #region 用户列表
        public ActionResult Users()
        {
            List<guanwang_user> list = db.guanwang_user.Where(c => c.isDelete == false).ToList(); ;
            ViewData["list"] = list;
            ViewData["roleid"] = UserInfo.userRole;
            return View();
        }
        #endregion

        #region 添加用户
        public ActionResult UsersAdd()
        {
            if (UserInfo.userRole != (int)Enum_User_Role.超级管理员)
            {
                return RedirectToAction("Users");
            }
            return View();
        }
        [HttpPost]
        public ActionResult UsersAdd(FormCollection collection)
        {
            if (UserInfo.userRole != (int)Enum_User_Role.超级管理员)
            {
                return Content(ReturnMsg(Enum_Return.失败, "对不起！您没有权限！", null));
            }
            string name = Request["name"];
            if (string.IsNullOrEmpty(name))
            {
                return Content(ReturnMsg(Enum_Return.失败, "登录名称不能为空", null));
            }
            if (name.Trim().Length < 6)
            {
                return Content(ReturnMsg(Enum_Return.失败, "登录名称不能小于6个字符", null));
            }
            string pwd = Request["pwd"];
            if (string.IsNullOrEmpty(pwd))
            {
                return Content(ReturnMsg(Enum_Return.失败, "登录密码不能为空", null));
            }
            if (pwd.Trim().Length < 6)
            {
                return Content(ReturnMsg(Enum_Return.失败, "登录密码不能小于6个字符", null));
            }

            string email = Request["email"];
            if (string.IsNullOrEmpty(email))
            {
                return Content(ReturnMsg(Enum_Return.失败, "用户邮箱不能为空", null));
            }


            //查询是否存在此登录名称
            var model = db.guanwang_user.Where(c => c.userName == name).FirstOrDefault();
            if (model != null)
            {
                return Content(ReturnMsg(Enum_Return.失败, "该登录名称已存在！", null));
            }

            var ismodel = db.guanwang_user.Where(c => c.userEmail == email).FirstOrDefault();
            if (ismodel != null)
            {
                return Content(ReturnMsg(Enum_Return.失败, "该邮箱已存在！", null));
            }

            string realname = Request["realname"];
            string phone = Request["phone"];
            string position = Request["position"];
            string tags = Request["tags"];
            string imagevalue = Request["imagevalue"];
            string describe = Request["describe"];
            string address = Request["address"];
            string area = Request["area"];
            string facebook = Request["facebook"];
            string twitter = Request["twitter"];
            string googleplus = Request["googleplus"];
            string userweb = Request["userweb"];

            string md5pwd = pwd.Md5();

            guanwang_user user = new guanwang_user();
            user.userEmail = email;
            user.userName = name;
            user.userPwd = md5pwd;
            user.userRole = (int)Enum_User_Role.管理员;
            user.userStatus = true;
            user.realName = realname;
            user.phone = phone;
            user.userTags = tags;
            user.position = position;
            user.userImage = imagevalue;
            user.userDescribe = describe;
            user.address = address;
            user.area = area;
            user.facebook = facebook;
            user.twitter = twitter;
            user.googlePlus = googleplus;
            user.userWeb = userweb;

            user.addTime = DateTime.Now;

            db.guanwang_user.Add(user);
            db.SaveChanges();

            return Content(ReturnMsg(Enum_Return.成功, "添加成功！", null));
        }
        #endregion

        #region 编辑用户
        public ActionResult UsersEdit(int? id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return RedirectToAction("Users");
            }
            var model = db.guanwang_user.Where(c => c.userID == id).FirstOrDefault();
            if (model == null)
            {
                return RedirectToAction("Users");
            }
            ViewData["model"] = model;
            return View();
        }
        [HttpPost]
        public ActionResult UsersEdit()
        {
            int id;
            if (!int.TryParse(Request["id"], out id))
            {
                return Content(ReturnMsg(Enum_Return.失败, "参数错误", null));
            }
            var model = db.guanwang_user.Where(c => c.userID == id).FirstOrDefault();
            if (model == null)
            {
                return Content(ReturnMsg(Enum_Return.失败, "该数据不存在或已被删除", null));
            }
            if (UserInfo.userRole != (int)Enum_User_Role.超级管理员 && id != UserInfo.userID)
            {
                return Content(ReturnMsg(Enum_Return.失败, "对不起！您没有权限！", null));
            }
            string name = Request["name"];
            if (string.IsNullOrEmpty(name))
            {
                return Content(ReturnMsg(Enum_Return.失败, "登录名称不能为空", null));
            }
            if (name.Trim().Length < 6)
            {
                return Content(ReturnMsg(Enum_Return.失败, "登录名称不能小于6个字符", null));
            }

            string pwd = Request["pwd"];
            if (!string.IsNullOrEmpty(pwd))
            {
                if (pwd.Trim().Length < 6)
                {
                    return Content(ReturnMsg(Enum_Return.失败, "登录密码不能小于6个字符", null));
                }
            }

            string email = Request["email"];
            if (string.IsNullOrEmpty(email))
            {
                return Content(ReturnMsg(Enum_Return.失败, "用户邮箱不能为空", null));
            }


            //查询是否存在此登录名称
            var ismodel = db.guanwang_user.Where(c => c.userName == name && c.userName != model.userName).FirstOrDefault();
            if (ismodel != null)
            {
                return Content(ReturnMsg(Enum_Return.失败, "该登录名称已存在！", null));
            }

            //查询是否存在此邮箱
            var emodel = db.guanwang_user.Where(c => c.userEmail == email && c.userEmail != model.userEmail).FirstOrDefault();
            if (emodel != null)
            {
                return Content(ReturnMsg(Enum_Return.失败, "该邮箱已存在！", null));
            }

            string realname = Request["realname"];
            string phone = Request["phone"];
            string position = Request["position"];
            string tags = Request["tags"];
            string imagevalue = Request["imagevalue"];
            string describe = Request["describe"];
            string address = Request["address"];
            string area = Request["area"];
            string facebook = Request["facebook"];
            string twitter = Request["twitter"];
            string googleplus = Request["googleplus"];
            string userweb = Request["userweb"];

            model.userEmail = email;
            model.userName = name;

            model.realName = realname;
            model.phone = phone;
            model.position = position;
            model.userTags = tags;
            model.userDescribe = describe;
            model.address = address;
            model.area = area;
            model.facebook = facebook;
            model.twitter = twitter;
            model.googlePlus = googleplus;
            model.userWeb = userweb;

            if (!string.IsNullOrEmpty(pwd))
            {
                string md5pwd = pwd.Md5();
                model.userPwd = md5pwd;
            }
            if (!string.IsNullOrEmpty(imagevalue))
            {
                model.userImage = imagevalue;
            }

            model.updateTime = DateTime.Now;
            db.SaveChanges();

            return Content(ReturnMsg(Enum_Return.成功, "更新成功！", null));
        }
        #endregion

        #region 用户更新状态
        public ActionResult UsersStatus(int? id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return Content(ReturnMsg(Enum_Return.失败, "参数错误", null));
            }

            var model = db.guanwang_user.Where(c => c.userID == id).FirstOrDefault();
            if (model == null)
            {
                return Content(ReturnMsg(Enum_Return.失败, "参数错误", null));
            }
            if (!IsStartUser())
            {
                return Content(ReturnMsg(Enum_Return.失败, "对不起！请先完善相关信息！", null));
            }
            if (model.userStatus == true)
            {
                model.userStatus = false;
            }
            else
            {
                model.userStatus = true;
            }

            model.updateTime = DateTime.Now;
            db.SaveChanges();

            return Content(ReturnMsg(Enum_Return.成功, "更新成功", null));
        }

        public bool IsStartUser()
        {
            bool flag = true;
            if (string.IsNullOrEmpty(UserInfo.realName))
            {
                flag = false;
            }
            if (string.IsNullOrEmpty(UserInfo.userImage))
            {
                flag = false;
            }
            if (string.IsNullOrEmpty(UserInfo.position))
            {
                flag = false;
            }
            if (string.IsNullOrEmpty(UserInfo.userTags))
            {
                flag = false;
            }
            if (string.IsNullOrEmpty(UserInfo.address))
            {
                flag = false;
            }
            if (string.IsNullOrEmpty(UserInfo.area))
            {
                flag = false;
            }
            if (string.IsNullOrEmpty(UserInfo.phone))
            {
                flag = false;
            }
            if (string.IsNullOrEmpty(UserInfo.userDescribe))
            {
                flag = false;
            }
            return flag;
        }
        #endregion

        #region 用户删除
        public ActionResult UsersDelete(int? id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return Content(ReturnMsg(Enum_Return.失败, "参数错误", null));
            }

            var model = db.guanwang_user.Where(c => c.userID == id).FirstOrDefault();
            if (model == null)
            {
                return Content(ReturnMsg(Enum_Return.失败, "该数据不存在或已被删除", null));
            }
            if (UserInfo.userRole != (int)Enum_User_Role.超级管理员 && UserInfo.userID != id)
            {
                return Content(ReturnMsg(Enum_Return.失败, "对不起！您没有权限！", null));
            }

            model.isDelete = true;
            model.updateTime = DateTime.Now;
            db.SaveChanges();

            return Content(ReturnMsg(Enum_Return.成功, "删除成功", null));
        }
        #endregion

        #endregion

        #region 标题管理
        public ActionResult Title()
        {
            var list = db.guanwang_title.Where(c => true).ToList();
            ViewData["list"] = list;
            return View();
        }

        public ActionResult TitleEdit(int? id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return RedirectToAction("Title");
            }
            var model = db.guanwang_title.Where(c => c.titleID == id).FirstOrDefault();
            if (model == null)
            {
                return RedirectToAction("Title");
            }
            ViewData["model"] = model;
            return View();
        }
        [HttpPost]
        public ActionResult TitleEdit()
        {
            int id;
            if (!int.TryParse(Request["id"], out id))
            {
                return Content(ReturnMsg(Enum_Return.失败, "参数错误", null));
            }

            var model = db.guanwang_title.Where(c => c.titleID == id).FirstOrDefault();
            if (model == null)
            {
                return Content(ReturnMsg(Enum_Return.失败, "该数据不存在或已被删除", null));
            }

            string title1 = Request["title1"];
            string title2 = Request["title2"];
            string title3 = Request["title3"];
            string title4 = Request["title4"];
            string title5 = Request["title5"];
            string title6 = Request["title6"];

            string describe1 = Request["describe1"];
            string describe2 = Request["describe2"];
            string describe3 = Request["describe3"];
            string describe4 = Request["describe4"];
            string describe5 = Request["describe5"];
            string describe6 = Request["describe6"];

            model.title1 = title1;
            model.title2 = title2;
            model.title3 = title3;
            model.title4 = title4;
            model.title5 = title5;
            model.title6 = title6;

            model.describe1 = describe1;
            model.describe2 = describe2;
            model.describe3 = describe3;
            model.describe4 = describe4;
            model.describe5 = describe5;
            model.describe6 = describe6;

            model.updateTime = DateTime.Now;
            db.SaveChanges();

            return Content(ReturnMsg(Enum_Return.成功, "更新成功", null));
        }
        #endregion

        #region 底部链接管理

        #region 链接列表
        public ActionResult Footer()
        {
            List<guanwang_footer> newlist = new List<guanwang_footer>();
            var list = db.guanwang_footer.Where(c => c.type == 0).ToList();
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    newlist.Add(item);
                    var list1 = db.guanwang_footer.Where(c => c.type == item.footerID).ToList();
                    if (list1.Count > 0)
                    {
                        foreach (var item1 in list1)
                        {
                            item1.name = "　　" + item1.name;
                            newlist.Add(item1);
                        }
                    }
                }
            }


            ViewData["list"] = newlist;
            return View();
        }
        #endregion

        #region 链接添加
        public ActionResult FooterAdd()
        {
            var list = db.guanwang_footer.Where(c => c.type == 0).ToList();
            ViewData["list"] = list;
            return View();
        }

        [HttpPost]
        public ActionResult FooterAdd(FormCollection collection)
        {
            int typeid;
            if (!int.TryParse(Request["typeid"], out typeid))
            {
                return Content(ReturnMsg(Enum_Return.失败, "参数错误", null));
            }
            string title = Request["title"];
            if (string.IsNullOrEmpty(title))
            {
                return Content(ReturnMsg(Enum_Return.失败, "标题不能为空", null));
            }
            string link = Request["link"];
            if (string.IsNullOrEmpty(link))
            {
                return Content(ReturnMsg(Enum_Return.失败, "链接不能为空", null));
            }

            guanwang_footer footer = new guanwang_footer();
            footer.name = title;
            footer.url = link;
            footer.type = typeid;
            footer.isShow = true;
            footer.addTime = DateTime.Now;
            db.guanwang_footer.Add(footer);
            db.SaveChanges();

            return Content(ReturnMsg(Enum_Return.成功, "添加成功", null));
        }
        #endregion

        #region 链接编辑
        public ActionResult FooterEdit(int? id)
        {
            var list = db.guanwang_footer.Where(c => c.type == 0).ToList();
            ViewData["list"] = list;

            if (string.IsNullOrEmpty(id.ToString()))
            {
                return RedirectToAction("Footer");
            }
            var model = db.guanwang_footer.Where(c => c.footerID == id).FirstOrDefault();
            if (model == null)
            {
                return RedirectToAction("Footer");
            }
            ViewData["model"] = model;
            return View();
        }

        [HttpPost]
        public ActionResult FooterEdit()
        {
            int id;
            if (!int.TryParse(Request["id"], out id))
            {
                return Content(ReturnMsg(Enum_Return.失败, "参数错误", null));
            }
            var model = db.guanwang_footer.Where(c => c.footerID == id).FirstOrDefault();
            if (model == null)
            {
                return Content(ReturnMsg(Enum_Return.失败, "该数据不存在或已被删除", null));
            }
            int typeid;
            if (!int.TryParse(Request["typeid"], out typeid))
            {
                return Content(ReturnMsg(Enum_Return.失败, "参数错误", null));
            }
            string title = Request["title"];
            if (string.IsNullOrEmpty(title))
            {
                return Content(ReturnMsg(Enum_Return.失败, "标题不能为空", null));
            }
            string link = Request["link"];
            if (string.IsNullOrEmpty(link))
            {
                return Content(ReturnMsg(Enum_Return.失败, "链接不能为空", null));
            }
            model.name = title;
            model.url = link;
            model.type = typeid;
            model.updateTime = DateTime.Now;
            db.SaveChanges();

            return Content(ReturnMsg(Enum_Return.成功, "编辑成功", null));
        }
        #endregion

        #region 链接更新状态
        public ActionResult FooterStatus(int? id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return Content(ReturnMsg(Enum_Return.失败, "参数错误", null));
            }

            var model = db.guanwang_footer.Where(c => c.footerID == id).FirstOrDefault();
            if (model == null)
            {
                return Content(ReturnMsg(Enum_Return.失败, "参数错误", null));
            }

            if (model.type == 0)
            {
                return Content(ReturnMsg(Enum_Return.失败, "父节点不允许操作", null));
            }

            if (model.isShow == true)
            {
                model.isShow = false;
            }
            else
            {
                model.isShow = true;
            }

            model.updateTime = DateTime.Now;
            db.SaveChanges();

            return Content(ReturnMsg(Enum_Return.成功, "更新成功", null));
        }
        #endregion

        #region 链接删除
        public ActionResult FooterDelete(int? id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return Content(ReturnMsg(Enum_Return.失败, "参数错误", null));
            }

            var model = db.guanwang_footer.Where(c => c.footerID == id).FirstOrDefault();
            if (model == null)
            {
                return Content(ReturnMsg(Enum_Return.失败, "该数据不存在或已被删除", null));
            }

            if (model.type == 0)
            {
                return Content(ReturnMsg(Enum_Return.失败, "父节点不允许删除", null));
            }

            db.Entry(model).State = System.Data.EntityState.Deleted;
            db.SaveChanges();

            return Content(ReturnMsg(Enum_Return.成功, "删除成功", null));
        }
        #endregion
        #endregion

        #region 首页轮播管理

        #region 轮播列表
        public ActionResult Banner()
        {
            var list = db.guanwang_banner_index.Where(c => true).OrderBy(c => c.weight).ToList();
            ViewData["list"] = list;
            return View();
        }
        #endregion

        #region 轮播添加
        public ActionResult BannerAdd()
        {
            return View();
        }
        [HttpPost]
        public ActionResult BannerAdd(FormCollection collection)
        {
            string title = Request["title"];
            if (string.IsNullOrEmpty(title))
            {
                return Content(ReturnMsg(Enum_Return.失败, "标题不能为空", null));
            }
            string describe = Request["describe"];
            if (string.IsNullOrEmpty(describe))
            {
                return Content(ReturnMsg(Enum_Return.失败, "描述不能为空", null));
            }

            string bjimg = Request["bjimg"];
            if (string.IsNullOrEmpty(bjimg))
            {
                return Content(ReturnMsg(Enum_Return.失败, "请上传背景图片", null));
            }

            string rwimg = Request["rwimg"];
            if (string.IsNullOrEmpty(rwimg))
            {
                return Content(ReturnMsg(Enum_Return.失败, "请上传人物图片", null));
            }

            string link = Request["link"];
            if (string.IsNullOrEmpty(link))
            {
                return Content(ReturnMsg(Enum_Return.失败, "链接地址不能为空", null));
            }


            #region 权重处理
            int weight = 0;
            var list = db.guanwang_banner_index.Where(c => true).ToList();
            if (list.Count > 0)
            {
                weight = list.Max(c => c.weight).Value + 1;
            }
            #endregion

            guanwang_banner_index model = new guanwang_banner_index();
            model.bannerTitle = title;
            model.bannerDescripe = describe;
            model.bannerImage = bjimg;
            model.peopleImage = rwimg;
            model.isShow = true;
            model.weight = weight;
            model.link = link;
            model.addTime = DateTime.Now;
            db.guanwang_banner_index.Add(model);
            db.SaveChanges();

            return Content(ReturnMsg(Enum_Return.成功, "添加成功", null));
        }
        #endregion

        #region 轮播编辑
        public ActionResult BannerEdit(int? id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return RedirectToAction("Banner");
            }
            var model = db.guanwang_banner_index.Where(c => c.bannerID == id).FirstOrDefault();
            if (model == null)
            {
                return RedirectToAction("Banner");
            }
            ViewData["model"] = model;
            return View();
        }

        [HttpPost]
        public ActionResult BannerEdit()
        {

            int id;
            if (!int.TryParse(Request["id"], out id))
            {
                return Content(ReturnMsg(Enum_Return.失败, "参数错误", null));
            }
            var model = db.guanwang_banner_index.Where(c => c.bannerID == id).FirstOrDefault();
            if (model == null)
            {
                return Content(ReturnMsg(Enum_Return.失败, "该数据不存在或已被删除", null));
            }

            string title = Request["title"];
            if (string.IsNullOrEmpty(title))
            {
                return Content(ReturnMsg(Enum_Return.失败, "标题不能为空", null));
            }
            string describe = Request["describe"];
            if (string.IsNullOrEmpty(describe))
            {
                return Content(ReturnMsg(Enum_Return.失败, "描述不能为空", null));
            }

            string link = Request["link"];
            if (string.IsNullOrEmpty(link))
            {
                return Content(ReturnMsg(Enum_Return.失败, "链接地址不能为空", null));
            }

            model.bannerTitle = title;
            model.bannerDescripe = describe;

            string bjimg = Request["bjimg"];
            if (!string.IsNullOrEmpty(bjimg))
            {
                model.bannerImage = bjimg;
            }

            string rwimg = Request["rwimg"];
            if (!string.IsNullOrEmpty(rwimg))
            {
                model.peopleImage = rwimg;
            }
            model.link = link;
            model.updateTime = DateTime.Now;
            db.SaveChanges();

            return Content(ReturnMsg(Enum_Return.成功, "编辑成功", null));
        }
        #endregion

        #region 关于轮播更新状态
        public ActionResult BannerStatus(int? id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return Content(ReturnMsg(Enum_Return.失败, "参数错误", null));
            }

            var model = db.guanwang_banner_index.Where(c => c.bannerID == id).FirstOrDefault();
            if (model == null)
            {
                return Content(ReturnMsg(Enum_Return.失败, "参数错误", null));
            }

            if (model.isShow == true)
            {
                model.isShow = false;
            }
            else
            {
                model.isShow = true;
            }

            model.updateTime = DateTime.Now;
            db.SaveChanges();

            return Content(ReturnMsg(Enum_Return.成功, "更新成功", null));
        }
        #endregion

        #region 关于轮播上移
        public ActionResult BannerUp(int? id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return Content(ReturnMsg(Enum_Return.失败, "参数错误", null));
            }
            var model = db.guanwang_banner_index.Where(c => c.bannerID == id).FirstOrDefault();
            if (model == null)
            {
                return Content(ReturnMsg(Enum_Return.失败, "该数据不存在或已被删除！", null));
            }
            //判断是否能上移
            int ismove = db.guanwang_banner_index.Where(c => true).Min(c => c.weight).Value;
            if (ismove == model.weight)
            {
                return Content(ReturnMsg(Enum_Return.失败, "已经最顶层了！", null));
            }
            var list = db.guanwang_banner_index.Where(c => true).OrderBy(c => c.weight).ToList();
            foreach (var item in list)
            {
                if (item.bannerID == model.bannerID)//当前需要置换
                {
                    int index = list.IndexOf(item); //index 为索引值
                    int flag = (int)model.weight;
                    model.weight = list[index - 1].weight;
                    list[index - 1].weight = flag;
                }
            }
            model.updateTime = DateTime.Now;
            db.SaveChanges();
            return Content(ReturnMsg(Enum_Return.成功, "上移成功", null));
        }
        #endregion

        #region 关于轮播下移
        public ActionResult BannerDown(int? id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return Content(ReturnMsg(Enum_Return.失败, "参数错误", null));
            }
            var model = db.guanwang_banner_index.Where(c => c.bannerID == id).FirstOrDefault();
            if (model == null)
            {
                return Content(ReturnMsg(Enum_Return.失败, "该数据不存在或已被删除！", null));
            }
            //判断是否能下移
            int ismove = db.guanwang_banner_index.Where(c => true).Max(c => c.weight).Value;
            if (ismove == model.weight)
            {
                return Content(ReturnMsg(Enum_Return.失败, "已经最底层了！", null));
            }

            var list = db.guanwang_banner_index.Where(c => true).OrderBy(c => c.weight).ToList();
            foreach (var item in list)
            {
                if (item.bannerID == model.bannerID)//当前需要置换
                {
                    int index = list.IndexOf(item); //index 为索引值
                    int flag = (int)model.weight;
                    model.weight = list[index + 1].weight;
                    list[index + 1].weight = flag;
                }
            }
            model.updateTime = DateTime.Now;
            db.SaveChanges();
            return Content(ReturnMsg(Enum_Return.成功, "下移成功", null));
        }
        #endregion

        #region 关于轮播删除
        public ActionResult BannerDelete(int? id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return Content(ReturnMsg(Enum_Return.失败, "参数错误", null));
            }

            var model = db.guanwang_banner_index.Where(c => c.bannerID == id).FirstOrDefault();
            if (model == null)
            {
                return Content(ReturnMsg(Enum_Return.失败, "该数据不存在或已被删除", null));
            }

            db.Entry(model).State = System.Data.EntityState.Deleted;
            db.SaveChanges();

            return Content(ReturnMsg(Enum_Return.成功, "删除成功", null));
        }
        #endregion
        #endregion

        #region 关于轮播管理

        #region 管理列表
        public ActionResult BannerAbout()
        {
            var list = db.guanwang_banner_about.Where(c => true).OrderBy(c => c.weight).ToList();
            ViewData["list"] = list;
            return View();
        }
        #endregion

        #region 关于轮播添加
        public ActionResult BannerAboutAdd()
        {
            return View();
        }
        [HttpPost]
        public ActionResult BannerAboutAdd(FormCollection collection)
        {
            string imgurl = Request["imgurl"];
            if (string.IsNullOrEmpty(imgurl))
            {
                return Content(ReturnMsg(Enum_Return.失败, "您还没有上传图片", null));
            }

            #region 权重处理
            int weight = 0;
            var list = db.guanwang_banner_about.Where(c => true).ToList();
            if (list.Count > 0)
            {
                weight = list.Max(c => c.weight).Value + 1;
            }
            #endregion

            guanwang_banner_about about = new guanwang_banner_about();
            about.image = imgurl;
            about.isShow = true;
            about.weight = weight;
            about.addTime = DateTime.Now;
            db.guanwang_banner_about.Add(about);
            db.SaveChanges();
            return Content(ReturnMsg(Enum_Return.成功, "添加成功", null));
        }
        #endregion

        #region 关于轮播编辑
        public ActionResult BannerAboutEdit(int? id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return RedirectToAction("BannerAbout");
            }
            var model = db.guanwang_banner_about.Where(c => c.bannerID == id).FirstOrDefault();
            if (model == null)
            {
                return RedirectToAction("BannerAbout");
            }
            ViewData["model"] = model;
            return View();
        }
        [HttpPost]
        public ActionResult BannerAboutEdit()
        {
            int id;
            if (!int.TryParse(Request["id"], out id))
            {
                return Content(ReturnMsg(Enum_Return.失败, "参数错误", null));
            }

            var model = db.guanwang_banner_about.Where(c => c.bannerID == id).FirstOrDefault();
            if (model == null)
            {
                return Content(ReturnMsg(Enum_Return.失败, "该数据不存在或已被删除", null));
            }

            string imgurl = Request["imgurl"];
            if (string.IsNullOrEmpty(imgurl))
            {
                return Content(ReturnMsg(Enum_Return.失败, "请上传图片", null));
            }

            model.image = imgurl;
            model.updateTime = DateTime.Now;
            db.SaveChanges();

            return Content(ReturnMsg(Enum_Return.成功, "更新成功", null));
        }
        #endregion

        #region 关于轮播更新状态
        public ActionResult BannerAboutStatus(int? id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return Content(ReturnMsg(Enum_Return.失败, "参数错误", null));
            }

            var model = db.guanwang_banner_about.Where(c => c.bannerID == id).FirstOrDefault();
            if (model == null)
            {
                return Content(ReturnMsg(Enum_Return.失败, "参数错误", null));
            }

            if (model.isShow == true)
            {
                model.isShow = false;
            }
            else
            {
                model.isShow = true;
            }

            model.updateTime = DateTime.Now;
            db.SaveChanges();

            return Content(ReturnMsg(Enum_Return.成功, "更新成功", null));
        }
        #endregion

        #region 关于轮播上移
        public ActionResult BannerAboutUp(int? id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return Content(ReturnMsg(Enum_Return.失败, "参数错误", null));
            }
            var model = db.guanwang_banner_about.Where(c => c.bannerID == id).FirstOrDefault();
            if (model == null)
            {
                return Content(ReturnMsg(Enum_Return.失败, "该数据不存在或已被删除！", null));
            }
            //判断是否能上移
            int ismove = db.guanwang_banner_about.Where(c => true).Min(c => c.weight).Value;
            if (ismove == model.weight)
            {
                return Content(ReturnMsg(Enum_Return.失败, "已经最顶层了！", null));
            }
            var list = db.guanwang_banner_about.Where(c => true).OrderBy(c => c.weight).ToList();
            foreach (var item in list)
            {
                if (item.bannerID == model.bannerID)//当前需要置换
                {
                    int index = list.IndexOf(item); //index 为索引值
                    int flag = (int)model.weight;
                    model.weight = list[index - 1].weight;
                    list[index - 1].weight = flag;
                }
            }
            model.updateTime = DateTime.Now;
            db.SaveChanges();
            return Content(ReturnMsg(Enum_Return.成功, "上移成功", null));
        }
        #endregion

        #region 关于轮播下移
        public ActionResult BannerAboutDown(int? id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return Content(ReturnMsg(Enum_Return.失败, "参数错误", null));
            }
            var model = db.guanwang_banner_about.Where(c => c.bannerID == id).FirstOrDefault();
            if (model == null)
            {
                return Content(ReturnMsg(Enum_Return.失败, "该数据不存在或已被删除！", null));
            }
            //判断是否能下移
            int ismove = db.guanwang_banner_about.Where(c => true).Max(c => c.weight).Value;
            if (ismove == model.weight)
            {
                return Content(ReturnMsg(Enum_Return.失败, "已经最底层了！", null));
            }

            var list = db.guanwang_banner_about.Where(c => true).OrderBy(c => c.weight).ToList();
            foreach (var item in list)
            {
                if (item.bannerID == model.bannerID)//当前需要置换
                {
                    int index = list.IndexOf(item); //index 为索引值
                    int flag = (int)model.weight;
                    model.weight = list[index + 1].weight;
                    list[index + 1].weight = flag;
                }
            }
            model.updateTime = DateTime.Now;
            db.SaveChanges();
            return Content(ReturnMsg(Enum_Return.成功, "下移成功", null));
        }
        #endregion

        #region 关于轮播删除
        public ActionResult BannerAboutDelete(int? id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return Content(ReturnMsg(Enum_Return.失败, "参数错误", null));
            }

            var model = db.guanwang_banner_about.Where(c => c.bannerID == id).FirstOrDefault();
            if (model == null)
            {
                return Content(ReturnMsg(Enum_Return.失败, "该数据不存在或已被删除", null));
            }

            db.Entry(model).State = System.Data.EntityState.Deleted;
            db.SaveChanges();

            return Content(ReturnMsg(Enum_Return.成功, "删除成功", null));
        }
        #endregion
        #endregion

        #region 上传图片
        public ActionResult Upload()
        {
            string imgurl = "";
            foreach (string key in Request.Files)
            {
                //这里只测试上传第一张图片file[0]
                HttpPostedFileBase file0 = Request.Files[key];

                //转换成byte,读取图片MIME类型
                Stream stream;
                int size = file0.ContentLength / 1024; //文件大小KB

                if (size > 1024)
                {
                    return Content(ReturnMsg(Enum_Return.失败, "图片不能超过1M：", null));
                }

                byte[] fileByte = new byte[2];//contentLength，这里我们只读取文件长度的前两位用于判断就好了，这样速度比较快，剩下的也用不到。
                stream = file0.InputStream;
                stream.Read(fileByte, 0, 2);//contentLength，还是取前两位

                //获取图片宽和高
                //System.Drawing.Image image = System.Drawing.Image.FromStream(stream);
                //int width = image.Width;
                //int height = image.Height;


                string fileFlag = "";
                if (fileByte != null && fileByte.Length > 0)//图片数据是否为空
                {
                    fileFlag = fileByte[0].ToString() + fileByte[1].ToString();
                }
                string[] fileTypeStr = { "255216", "7173", "6677", "13780" };//对应的图片格式jpg,gif,bmp,png
                if (fileTypeStr.Contains(fileFlag))
                {
                    string action = Request["action"];
                    string fullpath = "/uploads/" + action + "/";
                    if (!Directory.Exists(Server.MapPath(fullpath)))
                    {
                        Directory.CreateDirectory(Server.MapPath(fullpath));
                    }

                    string Str = Request.Files[key].FileName.Split('.')[1];//获取上传文件的后缀
                    string NewName = DateTime.Now.ToString("yyyyMMddhhmmss") + "." + Str;//重命名上传文件

                    Request.Files[key].SaveAs(Server.MapPath(fullpath + NewName));
                    imgurl = fullpath + NewName;
                }
                else
                {
                    return Content(ReturnMsg(Enum_Return.失败, "图片格式不正确：" + fileFlag, null));
                }

                stream.Close();
            }

            return Content(ReturnMsg(Enum_Return.成功, "上传成功", imgurl));
        }
        #endregion

        #region 富文本上传图片
        public ActionResult RichTextUpload()
        {
            string imgurl = "";
            foreach (string key in Request.Files)
            {
                //这里只测试上传第一张图片file[0]
                HttpPostedFileBase file0 = Request.Files[key];

                //转换成byte,读取图片MIME类型
                Stream stream;
                int size = file0.ContentLength / 1024; //文件大小KB

                if (size > 1024)
                {
                    return Content(JsonConvert.SerializeObject(new { success = 0, message = "图片不能超过1M", url = "" }));
                }


                byte[] fileByte = new byte[2];//contentLength，这里我们只读取文件长度的前两位用于判断就好了，这样速度比较快，剩下的也用不到。
                stream = file0.InputStream;
                stream.Read(fileByte, 0, 2);//contentLength，还是取前两位

                //获取图片宽和高
                //System.Drawing.Image image = System.Drawing.Image.FromStream(stream);
                //int width = image.Width;
                //int height = image.Height;


                string fileFlag = "";
                if (fileByte != null && fileByte.Length > 0)//图片数据是否为空
                {
                    fileFlag = fileByte[0].ToString() + fileByte[1].ToString();
                }
                string[] fileTypeStr = { "255216", "7173", "6677", "13780" };//对应的图片格式jpg,gif,bmp,png
                if (fileTypeStr.Contains(fileFlag))
                {
                    string fullpath = "/uploads/richtext/";
                    if (!Directory.Exists(Server.MapPath(fullpath)))
                    {
                        Directory.CreateDirectory(Server.MapPath(fullpath));
                    }
                    string Str = Request.Files[key].FileName.Split('.')[1];//获取上传文件的后缀
                    string NewName = DateTime.Now.ToString("yyyyMMddhhmmss") + "." + Str;//重命名上传文件

                    Request.Files[key].SaveAs(Server.MapPath(fullpath + NewName));
                    imgurl = fullpath + NewName;
                }
                else
                {
                    return Content(JsonConvert.SerializeObject(new { success = 0, message = "图片格式不正确" + fileFlag, url = "" }));
                }

                stream.Close();
            }
            return Content(JsonConvert.SerializeObject(new { success = 1, message = "上传成功", url = imgurl }));
        }
        #endregion

        #region 注销
        public ActionResult Layout()
        {
            if (Session["manager_user_model"] != null)
            {
                if (Request.Cookies["username"] != null && Request.Cookies["password"] != null)
                {
                    HttpCookie cp1 = Request.Cookies["username"];
                    cp1.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(cp1);

                    HttpCookie cp2 = Request.Cookies["password"];
                    cp2.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(cp2);
                }
                Session.Remove("manager_user_model");
            }
            return Content(ReturnMsg(Enum_Return.成功, "退出成功", null));
        }
        #endregion
    }
}
