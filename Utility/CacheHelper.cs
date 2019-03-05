using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

public class CacheHelper
{
    /// <summary>
    /// 获取缓存中的数据
    /// </summary>
    /// <param name="key">键</param>
    /// <returns></returns>
    public static object Get(string key)
    {
        System.Web.Caching.Cache cache = HttpRuntime.Cache;
        return cache[key];
    }
    /// <summary>
    /// 设置缓存的值
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="value">值</param>
    public static void Set(string key, object value)
    {
        System.Web.Caching.Cache cache = HttpRuntime.Cache;
        cache[key] = value;
    }
    /// <summary>
    /// 移除缓存中的值
    /// </summary>
    /// <param name="key">键</param>
    public static void Remove(string key)
    {
        System.Web.Caching.Cache cache = HttpRuntime.Cache;
        cache.Remove(key);
    }
}
