using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using Newtonsoft.Json;
using Utility;

namespace WebApp.Controllers
{
    public class DefaultController : Controller
    {
        public qds11527190_dbEntities db = new qds11527190_dbEntities();

        public string ReturnMsg(Enum_Return type, string msg, object data)
        {
            return JsonConvert.SerializeObject(new { status = (int)type, msg = msg, data = data });
        }
    }
}
