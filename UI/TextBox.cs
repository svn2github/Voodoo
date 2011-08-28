using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Voodoo.Web.UI.Controls
{
    public class TextBox : System.Web.UI.WebControls.TextBox
    {
        // Fields
        private string _ErrText = "";
        private RegularExpressionValidator Rev = new RegularExpressionValidator();
        private RequiredFieldValidator Rfv = new RequiredFieldValidator();

        // Methods
        public TextBox()
        {
            base.Attributes.Add("onfocus", "this.className='InputFocus';");
            base.Attributes.Add("onblur", "this.className='InputBlur';");
            base.CssClass = "InputBlur";
        }

        protected override void EnsureChildControls()
        {
            this.Rfv.ErrorMessage = " * 必填项";
            this.Rfv.Display = ValidatorDisplay.Dynamic;
            this.Rfv.EnableViewState = true;
            this.Rfv.ControlToValidate = base.ID;
            this.Rev.ErrorMessage = " * 输入格式错误";
            this.Rev.Display = ValidatorDisplay.Dynamic;
            this.Rev.EnableViewState = true;
            this.Rev.ControlToValidate = base.ID;
            this.Controls.Add(this.Rfv);
            this.Controls.Add(this.Rev);
        }

        private string GetValidRegex()
        {
            string validRegExp = @"(\S)";
            switch (this.ValidType)
            {
                case DataType.No:
                    this._ErrText = "";
                    validRegExp = "";
                    break;

                case DataType.Str:
                    this._ErrText = " * 不能包含字符：',&quot;,>,<";
                    validRegExp = "[^'^<^>^\"]+";
                    break;

                case DataType.DateTime:
                    this._ErrText = " * 日期格式错误";
                    validRegExp = @"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-))$";
                    break;

                case DataType.Money:
                    this._ErrText = " * 金额格式错误";
                    validRegExp = @"^(\d*)\.?\d{1,2}";
                    break;

                case DataType.Int:
                    this._ErrText = " * 必须为7位整数";
                    validRegExp = @"(-)?(\d{1,7})";
                    break;

                case DataType.IntPostive:
                    this._ErrText = " * 必须为大于0的整数";
                    validRegExp = @"([1-9]{1}\d{1,7})";
                    break;

                case DataType.IntZeroPostive:
                    this._ErrText = " * 必须为不小于0的整数";
                    validRegExp = @"(\d{1,7})";
                    break;

                case DataType.BigInt:
                    this._ErrText = " * 必须为整数";
                    validRegExp = @"(-)?(\d{1,20})";
                    break;

                case DataType.BigIntPostive:
                    this._ErrText = " * 必须为大于0的整数";
                    validRegExp = @"([1-9]{1}\d{1,20})";
                    break;

                case DataType.BigIntZeroPostive:
                    this._ErrText = " * 必须为不小于0的整数";
                    validRegExp = @"(\d{1,20})";
                    break;

                case DataType.Float:
                    this._ErrText = " * 必须为数字";
                    validRegExp = @"(-)?(\d+)(((\.)(\d)+))?";
                    break;

                case DataType.FloatPostive:
                    this._ErrText = " * 必须为大于0的数字";
                    validRegExp = @"(\d+)(((\.)(\d)+))?";
                    break;

                case DataType.FloatZeroPostive:
                    this._ErrText = " * 必须为不小于0的数字";
                    validRegExp = @"(\d+)(((\.)(\d)+))?";
                    break;

                case DataType.EngChar:
                    this._ErrText = " * 只能输入英文";
                    validRegExp = "[a-zA-Z]*";
                    break;

                case DataType.EngNum:
                    this._ErrText = " * 只能输入英文和数字";
                    validRegExp = "[a-zA-Z0-9]*";
                    break;

                case DataType.EngNumUnLine:
                    this._ErrText = " * 只能输入英文、数字和下划线";
                    validRegExp = "[a-zA-Z0-9_]*";
                    break;

                case DataType.Mail:
                    this._ErrText = " * 邮件地址错误";
                    validRegExp = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
                    break;

                case DataType.Url:
                    this._ErrText = " * Url格式错误";
                    validRegExp = @"(http://)?([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?";
                    break;

                case DataType.Phone:
                    this._ErrText = " * 电话号码格式错误";
                    validRegExp = @"(86)?(-)?(0\d{2,3})?(-)?(\d{7,8})(-)?(\d{1,5})?";
                    break;

                case DataType.Mob:
                    this._ErrText = " * 手机号码格式错误";
                    validRegExp = @"^(1[358])\d{9}$";                    
                    break;

                case DataType.Post:
                    this._ErrText = " * 邮编格式错误";
                    validRegExp = @"\d{6}";
                    break;

                case DataType.ChnChar:
                    this._ErrText = " * 只能输入中文";
                    validRegExp = @"[^\x00-\xff]";
                    break;

                case DataType.Custom:
                    this._ErrText = " * 格式错误";
                    validRegExp = this.ValidRegExp;
                    break;
                case DataType.IdCard:
                    this._ErrText = "* 身份证号码无效";
                    validRegExp = @"^(\d{15}$|^\d{18}$|^\d{17}(\d|X|x))$";  
                    break;
            }
            if (this.ValidError.Trim() != "")
            {
                this._ErrText = this.ValidError;
            }
            return validRegExp;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            this.TextareaScript();
        }

        protected override void Render(HtmlTextWriter output)
        {
            if (this.TextMode == TextBoxMode.MultiLine)
            {
                output.Write("&nbsp;");
                output.Write(this.ZoomImg());
            }
            if (this.SumLength)
            {
                base.Attributes.Add("onKeyDown", "chklength('" + this.ClientID + "');");
                base.Attributes.Add("onKeyUp", "chklength('" + this.ClientID + "');");
                base.Attributes.Add("onMouseMove", "chklength('" + this.ClientID + "');");
            }
            base.Render(output);
            output.Write("&nbsp;");
            if (!this.ValidNull)
            {
                this.Rfv.ID = "rfv" + base.ID;
                this.Rfv.ControlToValidate = base.ID;
                this.Rfv.RenderControl(output);
            }
            if ((this.ValidType != DataType.No))
            {
                this.Rev.ID = "rev" + base.ID;
                this.Rev.ControlToValidate = base.ID;
                this.Rev.ValidationExpression = this.GetValidRegex();
                this.Rev.ErrorMessage = this._ErrText;
                this.Rev.RenderControl(output);
            }
        }


        private void TextareaScript()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<script language='JavaScript'>");
            builder.Append("<!--");
            builder.Append(string.Format("\r\n", new object[0]));
            builder.Append("function zoomtextarea(objname, zoom) ");
            builder.Append("{");
            builder.Append("zoomsize = zoom ? 10 : -10;");
            builder.Append("obj = document.getElementById(objname);");
            builder.Append("if(obj.rows + zoomsize > 0) {");
            builder.Append("obj.rows += zoomsize;");
            builder.Append("}}");
            builder.Append(string.Format("\r\n", new object[0]));
            builder.Append("//-->");
            builder.Append("</script>");
            this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "WsTextBox_ZoomTextarea", builder.ToString());
        }

        private string ZoomImg()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("<img src=\"/Images/zoomin.gif\" onMouseOver=\"this.style.cursor='hand'\" align=\"absMiddle\" onClick=\"zoomtextarea('{0}', 1)\" title=\"扩大\">&nbsp;&nbsp;", this.ClientID);
            builder.AppendFormat("<img src=\"/Images/zoomout.gif\" onMouseOver=\"this.style.cursor='hand'\" align=\"absMiddle\" onClick=\"zoomtextarea('{0}', 0)\" title=\"缩小\">", this.ClientID);
            if (this.SumLength)
            {
                builder.Append("&nbsp;<img src=\"/Images/RichTextBox/toolbar.separator.gif\" width=\"5\" height=\"21\" align=\"absMiddle\">&nbsp;内容长度 <span id=\"sumtext\">0</span>");
            }
            builder.Append("<br>");
            return builder.ToString();
        }

        // Properties
        [Description("传入值"), Category("控件验证"), DefaultValue(""), Bindable(false)]
        public string ImportType
        {
            get
            {
                if (this.ViewState["ImportType"] == null)
                {
                    return "";
                }
                return (string)this.ViewState["ImportType"];
            }
            set
            {
                this.ViewState["ImportType"] = value;
            }
        }

        [DefaultValue("False"), Category("控件验证"), Description("统计内容长度"), Bindable(false)]
        public bool SumLength
        {
            get
            {
                if (this.ViewState["SumLength"] == null)
                {
                    return false;
                }
                return Convert.ToBoolean(this.ViewState["SumLength"]);
            }
            set
            {
                this.ViewState["SumLength"] = value;
            }
        }

        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
            }
        }

        [DefaultValue(""), Bindable(false), Category("控件验证"), Description("自定义验证错误提示")]
        public string ValidError
        {
            get
            {
                if (this.ViewState["ValidError"] == null)
                {
                    return "";
                }
                return (string)this.ViewState["ValidError"];
            }
            set
            {
                this.ViewState["ValidError"] = value;
            }
        }

        [Description("验证空值"), Bindable(false), Category("控件验证"), DefaultValue("True")]
        public bool ValidNull
        {
            get
            {
                return ((this.ViewState["ValidNull"] == null) || Convert.ToBoolean(this.ViewState["ValidNull"]));
            }
            set
            {
                this.ViewState["ValidNull"] = value;
            }
        }

        [Description("自定义验证类型"), Category("控件验证"), DefaultValue(""), Bindable(false)]
        public string ValidRegExp
        {
            get
            {
                if (this.ViewState["ValidRegExp"] == null)
                {
                    return "";
                }
                return (string)this.ViewState["ValidRegExp"];
            }
            set
            {
                this.ViewState["ValidRegExp"] = value;
            }
        }

        public enum DataType
        {
            BigInt = 7,
            BigIntPostive = 8,
            BigIntZeroPostive = 9,
            ChnChar = 21,
            Custom = 99,
            DateTime = 2,
            EngChar = 13,
            EngNum = 14,
            EngNumUnLine = 15,
            Float = 10,
            FloatPostive = 11,
            FloatZeroPostive = 12,
            Int = 4,
            IntPostive = 5,
            IntZeroPostive = 6,
            Mail = 16,
            Mob = 19,
            Money = 3,
            No = 0,
            Phone = 18,
            Post = 20,
            Str = 1,
            Url = 17,
            IdCard=22
        }



        [Bindable(false), DefaultValue(0), Category("控件验证"), Description("验证类型")]
        public DataType ValidType
        {
            get
            {
                if (this.ViewState["ValidType"] == null)
                {
                    return DataType.No;
                }
                return (DataType)Convert.ToInt32(this.ViewState["ValidType"]);
            }
            set
            {
                this.ViewState["ValidType"] = value;
            }
        }
    }
}
