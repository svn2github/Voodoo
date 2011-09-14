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
        #region 注册css和脚本
        /// <summary>
        /// 注册css和脚本
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            string jquery_1_6 = Page.ClientScript.GetWebResourceUrl(this.GetType(), "Voodoo.UI.resource.jquery-1.6.min.js");
            Page.ClientScript.RegisterClientScriptInclude("jquery", jquery_1_6);

            string script = "$(function () {\n    $(\"#" + this.ID + "\").bind('submit',function () {\n        try{if(jQuery('#"+this.ID+"').validationEngine('validate')==false){return false;}}catch(e){}\n        $.post($(this).attr(\"action\") + \"?ajax=1\",\n                    $(this).serialize(),\n                    function (data) {\n                        var j = eval(data);\n                        for (var i = 0; i < j.length; i++) {\n                            $(\"#\" + j[i][\"name\"]).prop(j[i][\"attr\"],j[i][\"val\"]);\n                        }\n                    })\n        return false;\n    });\n})";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "onsubmit", "<script type='text/javascript'>" + script + "</script>");
            if (!Page.IsStartupScriptRegistered("__dopostback"))
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "__dopostback", "<script type='text/javascript'>function __doPostBack(eventTarget, eventArgument){$('#__EVENTTARGET').val(eventTarget);$('#__EVENTARGUMENT').val(eventArgument); $('#" + this.ID + "').submit(); return void(0);}</script>");
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
