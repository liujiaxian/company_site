using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{

    public enum Enum_Banner_Type
    {
        首页 = 0,
        关于我们
    }

    public enum Enum_Title_Type
    {
        首页 = 0,
        关于我们,
        服务领域,
        项目案例,
        官方博客,
        项目定价,
        联系我们

    }

    public enum Enum_Footer_Type
    {
        网站导航 = 0,
        技术支持,
        产品导航,
        友情链接
    }

    public enum Enum_Return
    {
        成功=0,
        失败
    }

    public enum Enum_User_Role
    {
        超级管理员 = 0,
        管理员
    }

    public enum Enum_Pricing_Color
    {
        one = 0,
        two,
        three,
        four,
        five,
        six,
        seven
    }
}
