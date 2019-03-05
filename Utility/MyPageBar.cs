using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public class MyPageBar
    {
        //会员中心
        public static string PageSortManager(int pageIndex, int pageCount, string urlparams)
        {
            if (pageCount == 1)
            {
                return string.Empty;
            }
            int start = pageIndex - 5;
            start = start < 1 ? 1 : start;
            int end = start + 9;
            end = end > pageCount ? pageCount : end;
            StringBuilder sb = new StringBuilder();
            if (pageIndex > 1)
            {
                sb.Append(string.Format("<a href='" + urlparams + "{0}'>上一页</a>", pageIndex - 1));
            }
            for (int i = start; i <= end; i++)
            {
                if (pageIndex == i)
                {
                    sb.Append(string.Format("<span class='current'>{0}</span>", i));
                }
                else
                {
                    sb.Append(string.Format("<a href='" + urlparams + "{0}'>{0}</a>", i));
                }
            }

            if (pageIndex < pageCount)
            {
                sb.Append(string.Format("<a href='"+urlparams+"{0}'>下一页</a>", pageIndex + 1));
            }
            return sb.ToString();
        }
        //模板中心
        public static string PageSortTemplate(int pageIndex, int pageCount, string urlparams)
        {
            if (pageCount == 1)
            {
                return string.Empty;
            }
            int start = pageIndex - 5;
            start = start < 1 ? 1 : start;
            int end = start + 9;
            end = end > pageCount ? pageCount : end;
            StringBuilder sb = new StringBuilder();
            if (pageIndex > 1)
            {
                sb.Append(string.Format("<a href='/guanwang/managerhome/templatecontent?page={0}&typeid=" + urlparams + "'>上一页</a>", pageIndex - 1));
            }
            for (int i = start; i <= end; i++)
            {
                if (pageIndex == i)
                {
                    sb.Append(string.Format("<span class='current'>{0}</span>", i));
                }
                else
                {
                    sb.Append(string.Format("<a href='/guanwang/managerhome/templatecontent?page={0}&typeid=" + urlparams + "'>{0}</a>", i));
                }
            }

            if (pageIndex < pageCount)
            {
                sb.Append(string.Format("<a href='/guanwang/managerhome/templatecontent?page={0}&typeid=" + urlparams + "'>下一页</a>", pageIndex + 1));
            }
            return sb.ToString();
        }
        //博客分页
        public static string PageSort(int pageIndex, int pageCount, string urlparams)
        {
            if (pageCount == 1)
            {
                return string.Empty;
            }
            int start = pageIndex - 5;
            start = start < 1 ? 1 : start;
            int end = start + 9;
            end = end > pageCount ? pageCount : end;
            StringBuilder sb = new StringBuilder();
            if (pageIndex > 1)
            {
                sb.Append(string.Format("<li><a href='"+urlparams+"{0}.shtml'><i class='fa fa-long-arrow-left'></i>上一页</a></li>", pageIndex - 1));
            }
            for (int i = start; i <= end; i++)
            {
                if (pageIndex == i)
                {
                    sb.Append(string.Format("<li class='active'><a href='javascript:;'>{0}</a></li>", i));
                }
                else
                {
                    sb.Append(string.Format("<li><a href='" + urlparams + "{0}.shtml'>{0}</a></li>", i));
                }
            }

            if (pageIndex < pageCount)
            {
                sb.Append(string.Format("<li><a href='" + urlparams + "{0}.shtml'>下一页<i class='fa fa-long-arrow-right'></i></a></li>", pageIndex + 1));
            }
            return sb.ToString();
        }

        //搜索分页
        public static string PageSortSearch(int pageIndex, int pageCount, string searchparams, string searchvalue)
        {
            if (pageCount == 1)
            {
                return string.Empty;
            }
            int start = pageIndex - 5;
            start = start < 1 ? 1 : start;
            int end = start + 9;
            end = end > pageCount ? pageCount : end;
            StringBuilder sb = new StringBuilder();
            if (pageIndex > 1)
            {
                sb.Append(string.Format("<li><a href='/home/blogsearch.shtml?" + searchparams + "=" + searchvalue + "&page={0}'><i class='fa fa-long-arrow-left'></i>上一页</a></li>", pageIndex - 1));
            }
            for (int i = start; i <= end; i++)
            {
                if (pageIndex == i)
                {
                    sb.Append(string.Format("<li class='active'><a href='javascript:;'>{0}</a></li>", i));
                }
                else
                {
                    sb.Append(string.Format("<li><a href='/home/blogsearch.shtml?" + searchparams + "=" + searchvalue + "&page={0}'>{0}</a></li>", i));
                }
            }

            if (pageIndex < pageCount)
            {
                sb.Append(string.Format("<li><a href='/home/blogsearch.shtml?" + searchparams + "=" + searchvalue + "&page={0}'>下一页<i class='fa fa-long-arrow-right'></i></a></li>", pageIndex + 1));
            }
            return sb.ToString();
        }
    }
}
