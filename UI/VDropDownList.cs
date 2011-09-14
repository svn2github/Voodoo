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
    /// <summary>
    /// 带验证功能的DropDownList
    /// </summary>
    public class VDropDownList:DropDownList
    {
        #region 注册css和脚本
        /// <summary>
        /// 注册css和脚本
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            string cssUrl = Page.ClientScript.GetWebResourceUrl(this.GetType(), "Voodoo.UI.resource.validationEngine.jquery.css");
            StringBuilder css = new StringBuilder();
            css.Append("<link rel='stylesheet' type='text/css' href='" + cssUrl + "' />");

            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Voodoo.UI.resource.validationEngine.jquery.css", css.ToString());



            string jquery_1_6 = Page.ClientScript.GetWebResourceUrl(this.GetType(), "Voodoo.UI.resource.jquery-1.6.min.js");
            Page.ClientScript.RegisterClientScriptInclude("jquery", jquery_1_6);

            string validationEngine = Page.ClientScript.GetWebResourceUrl(this.GetType(), "Voodoo.UI.resource.jquery.validationEngine.js");
            Page.ClientScript.RegisterClientScriptInclude("Voodoo.UI.resource.jquery.validationEngine.js", validationEngine);

            string language = Page.ClientScript.GetWebResourceUrl(this.GetType(), "Voodoo.UI.resource.languages.jquery.validationEngine-zh_cn.js");
            Page.ClientScript.RegisterClientScriptInclude("Voodoo.UI.resource.languages.jquery.validationEngine-zh_cn.js", language);
            //Page.ClientScript.RegisterClientScriptInclude("SH", jsUrl);

            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "onload", "<script type='text/javascript'>jQuery(document).ready(function () {  jQuery(\"form\").validationEngine(); });</script>");
            base.OnPreRender(e);
        }
        #endregion

        #region 重写 Render
        protected override void Render(HtmlTextWriter output)
        {
            if (this.EnableValidate)
            {
                StringBuilder sb = new StringBuilder();

                //先保留文本框自身的Css
                if (this.CssClass.Length > 0)
                {
                    sb.Append(this.CssClass);
                    sb.Append(" ");
                }

                //验证开始
                sb.Append("validate[");

                StringBuilder sb_va = new StringBuilder();
                //验证空
                if (this.EnableNull == false)
                {
                    sb_va.Append("required");
                }

                //验证结束
                sb.Append(sb_va);
                sb.Append("]");
                //sb = new StringBuilder(Voodoo.Properties.Resources.jquery_1_6_min);
                this.CssClass = sb.ToString();
            }
            base.Render(output);
        }
        #endregion

        #region 启用验证
        /// <summary>
        /// 启用验证
        /// </summary>
        [Description("启用验证"), Category("控件验证"), DefaultValue(false), Bindable(false)]
        public bool EnableValidate
        {
            get
            {
                return (bool)this.ViewState["EnableValidate"].ToBoolean();
            }
            set
            {
                this.ViewState["EnableValidate"] = value;
            }
        }
        #endregion

        #region 允许空
        /// <summary>
        /// 允许内容为空
        /// </summary>
        [Description("允许空"), Category("控件验证"), DefaultValue(true), Bindable(false)]
        public bool EnableNull
        {
            get
            {
                return (bool)this.ViewState["EnableNull"];
            }
            set
            {
                this.ViewState["EnableNull"] = value;
            }
        }
        #endregion
    }
}
