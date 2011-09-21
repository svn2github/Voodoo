using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace Voodoo.UI
{
    public class AjaxForm : HtmlForm
    {
        /// <summary>
        /// 判断是否是回传时间
        /// </summary>
        /// <returns></returns>
        public bool IsCallBack()
        {
            return WS.RequestInt("ajax") > 0;
        }


        #region 注册css和脚本
        /// <summary>
        /// 注册css和脚本
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            string jquery_1_6 = Page.ClientScript.GetWebResourceUrl(this.GetType(), "Voodoo.UI.resource.jquery-1.6.min.js");
            Page.ClientScript.RegisterClientScriptInclude("jquery", jquery_1_6);

            string jqyery_ajax_form = Page.ClientScript.GetWebResourceUrl(this.GetType(), "Voodoo.UI.resource.jquery.AjaxForm.js");
            Page.ClientScript.RegisterClientScriptInclude("jqyery_ajax_form", jqyery_ajax_form);
            if (!Page.IsStartupScriptRegistered("jqyery_ajax_form_ini"))
            {
                string background_image_url = Page.ClientScript.GetWebResourceUrl(this.GetType(), "Voodoo.UI.resource.ajax-loader.gif");
                Page.ClientScript.RegisterStartupScript(this.GetType(), "jqyery_ajax_form_ini", "<script type='text/javascript'>$(function(){ formsubmit('" + this.ID + "','" + background_image_url + "') })</script>");
            }
            if (!Page.IsStartupScriptRegistered("__dopostback"))
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "__dopostback", "<script type='text/javascript'>\nfunction __doPostBack(eventTarget, eventArgument)\n    {    \n$('#__EVENTTARGET').val(eventTarget);\n    $('#__EVENTARGUMENT').val(eventArgument);\n     $('#" + this.ID + "').submit();\n     return void(0);\n }\n </script>");
            }
            base.OnPreRender(e);
        }
        #endregion

        #region 重写 Render
        protected override void Render(HtmlTextWriter output)
        {


            base.Render(output);
        }
        #endregion
    }
}
