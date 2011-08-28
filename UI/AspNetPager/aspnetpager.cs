//AspNetPager分页控件源代码：
//版权所有：陕西省延安市吴起县博杨计算机有限公司 杨涛（Webdiyer）
//此源代码仅供学习参考，不得用于任何商业用途；
//您可以修改并重新编译该控件，但源代码中的版权信息必须原样保留！
//有关控件升级及新控件发布信息，请留意 www.webdiyer.com 。


using System;
using System.ComponentModel;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web;
using System.Security.Permissions;

namespace Voodoo.UI
{
	#region AspNetPager Server Control

	/// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Class[@name="AspNetPager"]/*'/>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [DefaultProperty("PageSize")]
    [DefaultEvent("PageChanged")]
    [ParseChildren(false)]
    [PersistChildren(false)]
    [ANPDescription("desc_AspNetPager")]
    [Designer(typeof(PagerDesigner))]
    [ToolboxData("<{0}:AspNetPager runat=server></{0}:AspNetPager>")]
    [System.Drawing.ToolboxBitmap(typeof(AspNetPager),"AspNetPager.bmp")]
    public partial class AspNetPager : Panel, INamingContainer, IPostBackEventHandler, IPostBackDataHandler
    {

        #region Control Rendering Logic

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Method[@name="OnInit"]/*'/>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (null != CloneFrom && string.Empty != CloneFrom.Trim())
            {
                AspNetPager ctrl = Parent.FindControl(CloneFrom) as AspNetPager;
                if (null == ctrl)
                {
                    string errStr = SR.GetString("clonefromexeption") ??
                                    "The control \" %controlID% \" does not exist or is not of type Voodoo.UI.AspNetPager!";
                    throw new ArgumentException(errStr.Replace("%controlID%", CloneFrom), "CloneFrom");
                }
                if (null != ctrl.cloneFrom && this == ctrl.cloneFrom)
                {
                    string errStr = SR.GetString("recusiveclonefrom") ??
                                    "Invalid value for the CloneFrom property, AspNetPager controls can not to be cloned recursively!";
                    throw new ArgumentException(errStr, "CloneFrom");
                }
                cloneFrom = ctrl;
                CssClass = cloneFrom.CssClass;
                Width = cloneFrom.Width;
                Height = cloneFrom.Height;
                HorizontalAlign = cloneFrom.HorizontalAlign;
                BackColor = cloneFrom.BackColor;
                BackImageUrl = cloneFrom.BackImageUrl;
                BorderColor = cloneFrom.BorderColor;
                BorderStyle = cloneFrom.BorderStyle;
                BorderWidth = cloneFrom.BorderWidth;
                Font.CopyFrom(cloneFrom.Font);
                ForeColor = cloneFrom.ForeColor;
                EnableViewState = cloneFrom.EnableViewState;
                Enabled = cloneFrom.Enabled;
            }
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Method[@name="OnLoad"]/*'/>
        protected override void OnLoad(EventArgs e)
        {
            if (UrlPaging)
            {
                currentUrl = Page.Request.Path;
                queryString = Page.Request.ServerVariables["Query_String"];
                if (!Page.IsPostBack && cloneFrom == null)
                {
                    int index;
                    int.TryParse(Page.Request.QueryString[UrlPageIndexName],out index);
                    if (index <= 0)
                        index = 1;
                    else if(ReverseUrlPageIndex)
                        index = PageCount - index + 1;
                    PageChangingEventArgs args = new PageChangingEventArgs(index);
                    OnPageChanging(args);
                }
            }
            else
            {
                inputPageIndex = Page.Request.Form[UniqueID + "_input"];
            }
            base.OnLoad(e);
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Method[@name="OnPreRender"]/*'/>
        protected override void OnPreRender(EventArgs e)
        {
            if (PageCount > 1)
            {
                if ((ShowPageIndexBox == ShowPageIndexBox.Always) || (ShowPageIndexBox == ShowPageIndexBox.Auto && PageCount >= ShowBoxThreshold))
                {
                    StringBuilder sb = new StringBuilder("<script language=\"Javascript\" type=\"text/javascript\"><!--\n");
                    if (UrlPaging)
                    {
                        sb.Append("function ANP_goToPage(boxEl){if(boxEl!=null){var pi;if(boxEl.tagName==\"SELECT\")");
                        sb.Append("{pi=boxEl.options[boxEl.selectedIndex].value;}else{pi=boxEl.value;}");
                        if(string.IsNullOrEmpty(UrlPagingTarget))
                            sb.Append("location.href=\"").Append(GetHrefString(-1)).Append("\"");
                        else
                            sb.Append("window.open(\"").Append(GetHrefString(-1)).Append("\",\"").Append(UrlPagingTarget).Append("\")");
                        sb.Append(";}}\n");
                    }
                    if (PageIndexBoxType == PageIndexBoxType.TextBox)
                    {
                        string ciscript = SR.GetString("checkinputscript");
                        if (ciscript != null)
                        {
                            ciscript = ciscript.Replace("%PageIndexOutOfRangeErrorMessage%", PageIndexOutOfRangeErrorMessage);
                            ciscript = ciscript.Replace("%InvalidPageIndexErrorMessage%", InvalidPageIndexErrorMessage);
                        }
                        sb.Append(ciscript).Append("\n");
                        string keyScript = SR.GetString("keydownscript");
                        sb.Append(keyScript);
                    }
                    sb.Append("\n--></script>");
                    Type ctype = GetType();
                    ClientScriptManager cs = Page.ClientScript;
                    if (!cs.IsClientScriptBlockRegistered(ctype, "anp_script"))
                        cs.RegisterClientScriptBlock(ctype, "anp_script", sb.ToString());
                }
            }
            base.OnPreRender(e);
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Method[@name="AddAttributesToRender"]/*'/>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            if (Page != null && !UrlPaging)
                Page.VerifyRenderingInServerForm(this);
            base.AddAttributesToRender(writer);
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Method[@name="RenderBeginTag"]/*'/>
        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            bool showPager = (PageCount > 1 || (PageCount <= 1 && AlwaysShow));
            writer.WriteLine();
            writer.Write("<!-- ");
            writer.Write("AspNetPager " + VersionNumber + "  Copyright:2003-2010 Webdiyer (www.webdiyer.com)");
            writer.WriteLine(" -->");
            if (!showPager)
            {
                writer.Write("<!--");
                writer.Write(SR.GetString("autohideinfo"));
                writer.Write("-->");
            }
            else
                base.RenderBeginTag(writer);
        }

	    /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Method[@name="RenderEndTag"]/*'/>
        public override void RenderEndTag(HtmlTextWriter writer)
        {
            if (PageCount > 1 || (PageCount <= 1 && AlwaysShow))
                base.RenderEndTag(writer);
            writer.WriteLine();
            writer.Write("<!-- ");
            writer.Write("AspNetPager " + VersionNumber + "  Copyright:2003-2010 Webdiyer (www.webdiyer.com)");
            writer.WriteLine(" -->");
            writer.WriteLine();
        }


        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Method[@name="RenderContents"]/*'/>
        protected override void RenderContents(HtmlTextWriter writer)
        {
            if (PageCount <= 1 && !AlwaysShow)
                return;
            writer.Indent = 0;
            if (ShowCustomInfoSection != ShowCustomInfoSection.Never)
            {
                if (LayoutType == LayoutType.Table)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
                    writer.AddAttribute(HtmlTextWriterAttribute.Style, Style.Value);
                    if (Height != Unit.Empty)
                        writer.AddStyleAttribute(HtmlTextWriterStyle.Height, Height.ToString());
                    writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
                    writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
                    writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
                    writer.RenderBeginTag(HtmlTextWriterTag.Table); //<table>
                    writer.RenderBeginTag(HtmlTextWriterTag.Tr); //<tr>
                }
                if (ShowCustomInfoSection == ShowCustomInfoSection.Left)
                {
                    RenderCustomInfoSection(writer);
                    RenderNavigationSection(writer);
                }
                else
                {
                    RenderNavigationSection(writer);
                    RenderCustomInfoSection(writer);
                }
                if (LayoutType == LayoutType.Table)
                {
                    writer.RenderEndTag(); //</tr>
                    writer.RenderEndTag(); //</table>
                }
            }
            else
                RenderPagingElements(writer);
        }


        #endregion


    }

    #endregion


    #region PageChangingEventHandler Delegate
    /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Delegate[@name="PageChangingEventHandler"]/*'/>
	public delegate void PageChangingEventHandler(object src,PageChangingEventArgs e);

	#endregion

}