using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;


namespace Voodoo.Web.UI.Controls
{

    public class Button : System.Web.UI.WebControls.Button
    {
        private Format _Format = new Format();

        [Description("附加说明"), DefaultValue(0), Category("附加说明"), Bindable(false)]
        public string ReadStr
        {
            get
            {
                if (this.ViewState["ReadStr"] == null)
                {
                    return "";
                }
                return Convert.ToString(this.ViewState["ReadStr"]);
            }
            set
            {
                this.ViewState["ReadStr"] = value;
            }
        }

        [Bindable(false), DefaultValue(0), Description("按钮风格"), Category("按钮风格")]
        public ButtonStyle Styles
        {
            get
            {
                if (this.ViewState["Styles"] == null)
                {
                    return ButtonStyle.HtmlButton;
                }
                return (ButtonStyle)Convert.ToInt32(this.ViewState["Styles"]);
            }
            set
            {
                this.ViewState["Styles"] = value;
            }
        }

        [Description("按钮类型"), Category("按钮类型"), Bindable(false), DefaultValue(0)]
        public ButtonType Type
        {
            get
            {
                if (this.ViewState["Type"] == null)
                {
                    return ButtonType.Ok;
                }
                return (ButtonType)Convert.ToInt32(this.ViewState["Type"]);
            }
            set
            {
                this.ViewState["Type"] = value;
            }
        }

        public Format Format
        {
            get
            {
                return this._Format;
            }
        }

        public Button()
        {
            base.Text = " 保 存 ";
            _Format.Text = " 确 定 ";
            _Format.Title = " 系统提示信息 ";
        }

        private string GenerateJavascript()
        {
            short num = 0;
            switch (this.Type)
            {
                case ButtonType.Ok:
                    num = 0;
                    this._Format.Image = "0.gif";
                    break;

                case ButtonType.OkClose:
                    num = 1;
                    this._Format.Image = "0.gif";
                    break;

                case ButtonType.OkCloseReload:
                    num = 2;
                    this._Format.Image = "0.gif";
                    break;

                case ButtonType.OkCloseReturnValue:
                    num = 3;
                    this._Format.Image = "0.gif";
                    break;

                case ButtonType.OkChangUrl:
                    num = 4;
                    this._Format.Image = "2.gif";
                    break;

                case ButtonType.OkGoback:
                    num = 5;
                    this._Format.Image = "1.gif";
                    break;

                case ButtonType.OkEvent:
                    num = 6;
                    break;

                case ButtonType.OkEventOrCancel:
                    num = 7;
                    break;

                case ButtonType.OkChangUrlOrCancel:
                    num = 8;
                    this._Format.Image = "2.gif";
                    break;

                case ButtonType.OkNewWinUrlOrCancel:
                    num = 9;
                    this._Format.Image = "2.gif";
                    break;

                case ButtonType.OkShowModalDialog:
                    num = 10;
                    this._Format.Image = "2.gif";
                    break;

                case ButtonType.Wait:
                    num = 11;
                    this._Format.Image = "4.gif";
                    break;

                case ButtonType.WaitNoCopy:
                    num = 13;
                    this._Format.Image = "4.gif";
                    break;
            }
            return string.Format("this.disabled=true;tipsWindown('标题','text:提示信息内容','250','150','true','','true','text');this.disabled=false;", new object[] { this.ClientID, this.Format.Title, "SysMeg/" + this.Format.Image, this.Format.Text, num });
        }

        private string ModifyJavaScriptOnClick(string sHtml)
        {
            string str3;
            string str2 = this.GenerateJavascript();
            Regex regex = new Regex("onclick=\"(?<onclick>[^\"]*)");
            Match match = regex.Match(sHtml);
            if (match.Success)
            {
                string left = match.Groups["onclick"].Value;
                str3 = left + Convert.ToString(left.EndsWith(";") ? "" : "; ");
                str3 = str3.Replace("javascript:", "");
                if (base.CausesValidation)
                {
                    str3 = "if (typeof(Page_ClientValidate) == 'function') Page_ClientValidate(); if (Page_IsValid) {" + str2 + str3 + "} return Page_IsValid;";
                }
                else
                {
                    str3 = str2 + str3;
                }
                return regex.Replace(sHtml, "class=\"ButtonBlur\" onmouseover=\"this.className='ButtonFocus'\" onmouseout=\"this.className='ButtonBlur'\" onclick=\"" + str3);
            }
            int startIndex = sHtml.Trim().Length - 2;
            if (base.CausesValidation)
            {
                str3 = " class=\"ButtonBlur\" onmouseover=\"this.className='ButtonFocus'\" onmouseout=\"this.className='ButtonBlur'\" onclick=\"if (typeof(Page_ClientValidate) == 'function') Page_ClientValidate(); if (Page_IsValid) {" + str2 + "} return Page_IsValid;\"";
            }
            else
            {
                str3 = " class=\"ButtonBlur\" onmouseover=\"this.className='ButtonFocus'\" onmouseout=\"this.className='ButtonBlur'\" onclick=\"" + str2 + "\"";
            }
            return sHtml.Insert(startIndex, str3);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!this.Page.ClientScript.IsClientScriptIncludeRegistered(this.GetType(), "WsButton_RunScript"))
            {
                this.Page.ClientScript.RegisterClientScriptInclude(this.GetType(), "WsButton_RunScript", "/script/Button.js");
            }
        }

        protected override void Render(HtmlTextWriter output)
        {
            string sHtml = "";
            switch (this.Styles)
            {
                case ButtonStyle.HtmlButton:
                    sHtml = "<input type=\"button\" class=\"ButtonBlur\" onmouseover=\"this.className='ButtonFocus'\" onmouseout=\"this.className='ButtonBlur'\" name=\"" + this.ClientID + "\" value=\"" + this.Text + "\" id=\"" + this.ClientID + "\" />";
                    sHtml = this.ModifyJavaScriptOnClick(sHtml);
                    break;

                case ButtonStyle.WebButton:
                    {
                        StringWriter writer2 = new StringWriter();
                        HtmlTextWriter writer = new HtmlTextWriter(writer2);
                        base.Render(writer);
                        sHtml = writer2.ToString();
                        writer.Close();
                        writer2.Close();
                        sHtml = this.ModifyJavaScriptOnClick(sHtml);
                        break;
                    }
                case ButtonStyle.LinkString:
                    if (!base.CausesValidation)
                    {
                        sHtml = "<span class=\"linkstring\" onclick=\"" + this.GenerateJavascript() + "\">" + this.ClientID + "</span>";
                        break;
                    }
                    sHtml = "<span class=\"linkstring\" onclick=\"if (typeof(Page_ClientValidate) == 'function') Page_ClientValidate(); if (Page_IsValid) {" + this.GenerateJavascript() + "} return Page_IsValid;\">" + this.ClientID + "</span>";
                    break;

                case ButtonStyle.CanelButton:
                    sHtml = "<input type=\"button\" class=\"ButtonBlur\" onmouseover=\"this.className='ButtonFocus'\" onmouseout=\"this.className='ButtonBlur'\" onclick=\"window.location='" + this.Format.Content + "';\" name=\"" + this.ClientID + "\" value=\"" + this.Text + "\" id=\"" + this.ClientID + "\" />";
                    break;

                case ButtonStyle.JsButton:
                    sHtml = "<input type=\"button\" class=\"ButtonBlur\" onmouseover=\"this.className='ButtonFocus'\" onmouseout=\"this.className='ButtonBlur'\" onclick=\"" + this.Format.Content + "\" name=\"" + this.ClientID + "\" value=\"" + this.Text + "\" id=\"" + this.ClientID + "\" />";
                    break;
            }
            output.Write(string.Format("<span id='WsButtonDiv_{0}'>", this.ClientID));
            output.Write(sHtml + this.ReadStr);
            output.Write("</span>");
        }

        public enum ButtonStyle
        {
            HtmlButton = 1,
            WebButton = 2,
            LinkString = 3,
            CanelButton = 4,
            JsButton = 5
        }

        public enum ButtonType
        {
            Ok = 0,
            OkChangUrl = 4,
            OkChangUrlOrCancel = 8,
            OkClose = 1,
            OkCloseReload = 2,
            OkCloseReturnValue = 3,
            OkEvent = 6,
            OkEventOrCancel = 7,
            OkGoback = 5,
            OkNewWinUrlOrCancel = 9,
            OkShowModalDialog = 10,
            Wait = 11,
            WaitNoCopy = 13
        }
    }
}
