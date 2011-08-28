using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Caching;
using System.Runtime.CompilerServices;
using System.Web;
using System.Security.Policy;
using Voodoo.IO;
using Voodoo;

namespace Voodoo.Cache
{
    /// <summary>
    /// 缓存处理相关类,本类中所有路径请赋予相对路径
    /// 2010年4月9日 16:50:19
    /// </summary>
    public class Cache
    {
        private static System.Web.Caching.Cache ObjCache = HttpRuntime.Cache;
        private static short TimeOut = 720;

        #region 清除指定键值的缓存
        /// <summary>
        /// 清除指定键值的缓存
        /// </summary>
        /// <param name="Key">要清除的缓存的key值</param>
        public static void Clear(string Key)
        {
            if (ObjCache[Key] != null)
            {
                ObjCache.Remove(Key);
            }
        }
        #endregion

        #region 返回系统中缓存的个数
        /// <summary>
        /// 返回系统中缓存的个数
        /// </summary>
        /// <returns>缓存个数</returns>
        public static int Count()
        {
            return ObjCache.Count;
        }
        #endregion

        #region 获取当前指定Key的Cache值
        /// <summary>
        /// 获取当前指定Key的Cache值
        /// </summary>
        /// <param name="Key">缓存Key</param>
        /// <returns>缓存的值</returns>
        public static object GetCache(string Key)
        {
            return ObjCache[Key];
        }
        #endregion

        #region 设置当前指定Key的Cache值
        /// <summary>
        /// 设置当前指定Key的Cache值 
        /// </summary>
        /// <param name="Key">缓存Key</param>
        /// <param name="Obj">缓存的值</param>
        public static void SetCache(string Key, object Obj)
        {
            ObjCache.Insert(Key, Obj);
        }
        #endregion

        #region 设置当前指定Key的Cache值,并限定过期时间
        /// <summary>
        /// 设置当前指定Key的Cache值,并限定过期时间 
        /// </summary>
        /// <param name="Key">缓存Key</param>
        /// <param name="Obj">缓存的值</param>
        /// <param name="TimeOuts">超时时间（分钟）</param>
        public static void SetCache(string Key, object Obj, int TimeOuts)
        {
            ObjCache.Insert(Key, Obj, null, System.DateTime.Now.AddMinutes((double)TimeOuts), TimeSpan.Zero);
        }
        #endregion

        #region 设置当前指定Key的Cache值,依赖文件过期
        /// <summary>
        /// 设置当前指定Key的Cache值,依赖文件过期 
        /// </summary>
        /// <param name="Key">缓存Key</param>
        /// <param name="Obj">缓存的值</param>
        /// <param name="Files">绝对地址，例如"D:\Web\files.xml"</param>
        public static void SetCache(string Key, object Obj, string Files)
        {
            CacheDependency cacheDep = new CacheDependency(Files,System.DateTime.Now);
            SetCache(Key, Obj, TimeOut, cacheDep, CacheItemPriority.High);
        }
        #endregion

        #region 设置当前指定Key的Cache值
        /// <summary>
        /// 设置当前指定Key的Cache值
        /// </summary>
        /// <param name="Key">缓存Key</param>
        /// <param name="Obj">缓存的值</param>
        /// <param name="Priority">撤销缓存的优先值，此参数的值取自枚举变量“CacheItemPriority”，优先级低的数据项将先被删除。此参数主要用在缓存退出对象时.</param>
        public static void SetCache(string Key, object Obj, CacheItemPriority Priority)
        {
            SetCache(Key, Obj, TimeOut, null, Priority);
        }
        #endregion

        #region 设置当前指定Key的Cache值
        /// <summary>
        /// 设置当前指定Key的Cache值
        /// </summary>
        /// <param name="Key">缓存Key</param>
        /// <param name="Obj">缓存的值</param>
        /// <param name="TimeOuts">一个TimeSpan，表示缓存参数将在多长时间以后被删除</param>
        /// <param name="CacheDep">缓存的依赖项,需要一个CacheDependency，可初始化一个</param>
        /// <param name="Priority">撤销缓存的优先值，此参数的值取自枚举变量“CacheItemPriority”，优先级低的数据项将先被删除。此参数主要用在缓存退出对象时</param>
        public static void SetCache(string Key, object Obj, int TimeOuts, CacheDependency CacheDep, CacheItemPriority Priority)
        {
            ObjCache.Insert(Key, Obj, CacheDep, System.DateTime.MaxValue, TimeSpan.FromHours((double)TimeOuts), Priority, null);
        }
        #endregion

        #region 获取依赖文件过期的Cache内容
        /// <summary>
        /// 获取依赖文件过期的Cache内容
        /// </summary>
        /// <param name="Key">缓存Key</param>
        /// <param name="Obj">实体类型，例如typeof(Ws_blockSet)</param>
        /// <param name="FilePath">相对地址，例如"~/files.xml"</param>
        /// <returns>缓存的值</returns>
        public static object GetCache(string Key, object Obj, string FilePath)
        {
            FilePath = System.Web.HttpContext.Current.Server.MapPath(FilePath);
            return GetCache(Key, Obj, FilePath, FileType.Model);
        }
        #endregion

        #region  获取依赖文件过期的Cache内容
        /// <summary>
        /// 获取依赖文件过期的Cache内容
        /// </summary>
        /// <param name="Key">缓存Key</param>
        /// <param name="FilePath">相对地址，例如"~/files.xml"</param>
        /// <param name="Ft">文件类型枚举 例如FileType.XML</param>
        /// <returns>缓存的值</returns>
        public static object GetCache(string Key, string FilePath, FileType Ft)
        {
            return GetCache(Key, System.Web.HttpContext.Current.Server.MapPath(FilePath), Ft);
        }
        #endregion

        #region 获取依赖文件过期的Cache内容 重写
        /// <summary>
        /// 获取依赖文件过期的Cache内容 重写
        /// </summary>
        /// <param name="Key">缓存Key</param>
        /// <param name="Obj">实体类型，例如typeof(Ws_blockSet)</param>
        /// <param name="FilePath">相对地址，例如"~/files.xml"</param>
        /// <param name="Ft">文件类型枚举 FileType.XML</param>
        /// <returns>缓存的值</returns>
        public static object GetCache(string Key, object Obj, string FilePath, FileType Ft)
        {
            if (GetCache(Key) == null)
            {
                FilePath = System.Web.HttpContext.Current.Server.MapPath(FilePath);
                switch (Ft)
                {
                    case FileType.Xml:
                        SetCache(Key,Voodoo.IO.XML.Read(FilePath), FilePath);
                        break;

                    case FileType.Html:
                        SetCache(Key, File.Read(FilePath), FilePath);
                        break;

                    case FileType.Model:
                        SetCache(Key,Voodoo.IO.XML.Read((Type)Obj, FilePath), FilePath);
                        break;
                }
            }
            return GetCache(Key);
        }
        #endregion

        #region 文件类型枚举
        /// <summary>
        /// 文件类型枚举
        /// </summary>
        public enum FileType
        {
            Xml,
            Html,
            Model
        }
#endregion
    }
}
