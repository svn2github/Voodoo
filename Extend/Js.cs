using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Voodoo;

namespace Voodoo
{
    /// <summary>
    /// JavaScript处理相关方法
    /// </summary>
    public static class Js
    {
        /// <summary>
        /// 弹出警告框
        /// </summary>
        /// <param name="Str1"></param>
        public static void Alert(this string Str1)
        {
            HttpContext.Current.Response.Write(@"<script type='text/javascript\'>alert('系统提示：\n\n" + Str1.TrimBR() + "');</script>");
        }

        /// <summary>
        /// 弹出一个提示框,确定转向新URL
        /// </summary>
        /// <param name="Str1">提示信息</param>
        /// <param name="Url">新的URL</param>
        public static void AlertAndChangUrl(string Str1, string Url)
        {
            HttpContext.Current.Response.Write(@"<script>alert('系统提示：\n\n" + Str1 + "');window.location='" + Url + "';</script>");
        }

        public static void AlertAndReloadParent(string str)
        {
            HttpContext.Current.Response.Write(@"<script>alert('系统提示：\n\n" + str + "');parent.location.href=parent.location.href;</script>");
        }
        public static void AlertAndClose(string Str1)
        {
            HttpContext.Current.Response.Write(@"<script>alert('系统提示：\n\n" + Str1 + "');window.close();</script>");
        }

        public static void AlertAndCloseReload(string Str1)
        {
            HttpContext.Current.Response.Write(@"<script>alert('系统提示：\n\n" + Str1 + "'); if(window.dialogArguments != null){window.dialogArguments.location.reload(true);} window.close();</script>");
        }

        public static void AlertAndCloseReturnValue(string Str1, string Value)
        {
            HttpContext.Current.Response.Write(@"<script>alert('系统提示：\n\n" + Str1 + "');window.returnValue='" + Value + "';window.close();</script>");
        }

        public static void AlertAndGoback(string Str1)
        {
            HttpContext.Current.Response.Write(@"<script>alert('系统提示：\n\n" + Str1 + "');history.go(-1);</script>");
        }

        public static void AutoCloseWin()
        {
            HttpContext.Current.Response.Write("<br/><a href='javascript:window.close();'><font color=\"blue\">关闭窗口</font></a>，或三秒后自动关闭。<script>setTimeout(\"close()\",3000);</script><br /><br />");
        }

        public static void AutoCloseWin(int TimeNum)
        {
            string[] strArray = new string[] { "<br/><a href='javascript:window.close();'><font color=\"blue\">关闭窗口</font></a>，或", TimeNum.ToString(), "秒后自动关闭。<script>setTimeout(\"close()\",", (TimeNum * 0x3e8).ToString(), ");</script><br /><br />" };
            HttpContext.Current.Response.Write(string.Concat(strArray));
        }

        public static void ConfirmChangUrl(string Str1, string Url1, string Url2)
        {
            HttpContext.Current.Response.Write(@"<script>if (confirm('系统提示：\n\n" + Str1 + "')==false) {window.location='" + Url1 + "';} else {window.location='" + Url2 + "';}</script>");
        }

        public static void ConfirmChangUrlOrClose(string Str1, string Url1)
        {
            HttpContext.Current.Response.Write(@"<script>if (confirm('系统提示：\n\n" + Str1 + "')==false) {if(window.dialogArguments != null){window.dialogArguments.location.reload(true);} window.close();} else {if(window.dialogArguments != null){window.dialogArguments.location.reload(true);} window.location='" + Url1 + "';}</script>");
        }

        public static void NoAlertAndCloseReturnValue(string Value)
        {
            HttpContext.Current.Response.Write("<script>window.returnValue='" + Value + "';window.close();</script>");
        }

        #region 确认某操作后转入URL或者刷新父窗口
        /// <summary>
        /// 确认某操作后转入URL或者刷新父窗口
        /// </summary>
        /// <param name="message">弹出的询问信息</param>
        /// <param name="url">选择确定后转入的地址</param>
        public static void ConfirmChangUrlOrRefreshParent(string message,string url)
        {
            HttpContext.Current.Response.Write(@"<script>if (confirm('系统提示：\n\n" + message + "')==true) {window.location='" + url + "';} else {document.parent.location.href=document.parent.location.href;}</script>");
        }
        #endregion

        public static void OpenModalDialog(string Url, int W, int H, string Title)
        {
            HttpContext.Current.Response.Write("<script>function OpenWindow(Url,Width,Height,WindowObj){var ReturnStr=showModalDialog(Url,WindowObj,'dialogWidth:'+Width+'pt;dialogHeight:'+Height+'pt;status:no;help:no;scroll:no;');return ReturnStr;}</script>");
            HttpContext.Current.Response.Write("<script>OpenWindow('" + Url + "'," + W.ToString() + "," + H.ToString() + ",'" + Title + "')</script>");
        }

        public static void OpenWinNoAll(string Str1)
        {
            HttpContext.Current.Response.Write("<script>window.open(\"" + Str1 + "\",\"\",\"resizable=no,scrollbars=no,status=no,toolbar=no,menubar=no,location=no\");window.close();</script>");
        }

        public static void ReFreshParent()
        {
            HttpContext.Current.Response.Write("<script>window.opener.location.reload();</script>");
        }

        public static void ScrollEnd()
        {
            HttpContext.Current.Response.Write("<script>function func(){window.scroll(0,9999999);}</script>");
        }

        public static void ScrollEndStart()
        {
            HttpContext.Current.Response.Write("<script>func();</script>");
        }

        public static void RegScript(string ResourceName, string Key)
        {

        }

        /// <summary>
        /// 页面跳转
        /// </summary>
        /// <param name="Url"></param>
        public static void Jump(string Url)
        {
            HttpContext.Current.Response.Write(@"<script>window.location='" + Url + "';</script>");
        }

        /// <summary>
        /// 后退
        /// </summary>
        public static void GoBack()
        {
            HttpContext.Current.Response.Write(@"<script>history.go(-1);;</script>");
        }

    }
}
