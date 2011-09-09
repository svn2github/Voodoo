using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Voodoo.UI
{
    public class VTextBox : System.Web.UI.WebControls.TextBox
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public VTextBox()
        {

        }

        #region 私有方法
        private void AddDouhao(StringBuilder sb)
        {
            if (sb.Length > 0)
            {
               sb.Append(",");
            }
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


                //验证空
                if (this.EnableNull == false)
                {
                    sb.Append("required");
                }

                //验证最大长度
                if (this.MaxSize > 0)
                {
                    AddDouhao(sb);
                    sb.Append("maxSize[" + MaxSize + "]");
                }

                //验证最小长度
                if (this.MinSize > 0)
                {
                    AddDouhao(sb);
                    sb.Append("minSize[" + MinSize + "]");
                }

                //验证最大值
                if (!this.Max.IsNullOrEmpty())
                {
                    AddDouhao(sb);
                    sb.Append("max[" + Max + "]");
                }

                //验证最小值
                if (!this.Min.IsNullOrEmpty())
                {
                    AddDouhao(sb);
                    sb.Append("min[" + Min + "]");
                }

                //对比值
                if (!this.Equal.IsNullOrEmpty())
                {
                    AddDouhao(sb);
                    sb.Append("equals[" + Equal + "]");
                }

                if (this.VType != ValidateType.notValidate)
                {
                    AddDouhao(sb);
                    sb.Append("custom[" + VType.ToString() + "]");
                }

                //验证结束
                sb.Append("]");
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

        #region 最大长度
        [Description("最大长度"), Category("控件验证"), DefaultValue(0), Bindable(false)]
        public int MaxSize
        {
            get
            {
                return this.ViewState["MaxSize"].ToInt32();
            }
            set
            {
                this.ViewState["MaxSize"] = value;
            }
        }
        #endregion

        #region 最小长度
        [Description("最小长度"), Category("控件验证"), DefaultValue(0), Bindable(false)]
        public int MinSize
        {
            get
            {
                return this.ViewState["MinSize"].ToInt32();
            }
            set
            {
                this.ViewState["MinSize"] = value;
            }
        }
        #endregion

        #region 最大值
        [Description("最大值"), Category("控件验证"), DefaultValue(""), Bindable(false)]
        public string Max
        {
            get
            {
                return this.ViewState["Max"].ToS();
            }
            set
            {
                this.ViewState["Max"] = value;
            }
        }
        #endregion

        #region 最小值
        [Description("最小值"), Category("控件验证"), DefaultValue(""), Bindable(false)]
        public string Min
        {
            get
            {
                return this.ViewState["Min"].ToS();
            }
            set
            {
                this.ViewState["Min"] = value;
            }
        }
        #endregion

        #region 对比值
        [Description("对比值"), Category("控件验证"), DefaultValue(""), Bindable(false)]
        public string Equal
        {
            get
            {
                return this.ViewState["Equal"].ToS();
            }
            set
            {
                this.ViewState["Equal"] = value;
            }
        }
        #endregion

        #region 验证类型
        [Description("验证类型"), Category("控件验证"), DefaultValue(ValidateType.notValidate), Bindable(false)]
        public ValidateType VType
        {
            get
            {
                try
                {
                    return (ValidateType)this.ViewState["ValidateType"];
                }
                catch
                {
                    return ValidateType.notValidate;
                }
            }
            set
            {
                this.ViewState["ValidateType"] = value;
            }
        }
        #endregion

        #region 验证类型 enum
        /// <summary>
        /// 验证类型 enum
        /// </summary>
        public enum ValidateType
        {
            notValidate,
            phone,
            url,
            email,
            ipv4,
            date,
            number,
            integer,
            onlyLetterNumber,
            onlyNumberSp

        }
        #endregion
    }
}
