using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Model;
using Newtonsoft.Json;

namespace WebApp.Controllers
{
    public class CenterController : DefaultController
    {
        //
        // GET: /Center/

        public ActionResult Index()
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
            ViewData["templatejson"] = sb.ToString();
            return View();
        }


        public ActionResult Template(int? id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return RedirectToAction("Index");
            }

            var model = db.guanwang_template.Where(c => c.templateID == id).FirstOrDefault();
            if (model==null)
            {
                return RedirectToAction("Index");
            }

            ViewData["model"] = model.html;
            return View();
        }
    }
}
