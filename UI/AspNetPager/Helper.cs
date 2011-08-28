//AspNetPager分页控件源代码：
//版权所有：陕西省延安市吴起县博杨计算机有限公司 杨涛（Webdiyer）
//此源代码仅供学习参考，不得用于任何商业用途；
//您可以修改并重新编译该控件，但源代码中的版权信息必须原样保留！
//有关控件升级及新控件发布信息，请留意 www.webdiyer.com 。

using System;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

namespace Voodoo.UI
{
    public partial class AspNetPager
    {
        #region Private Helper Functions

        static void addMoreListItem(HtmlTextWriter writer, int pageIndex)
        {
            writer.Write("<option value=\"");
            writer.Write(pageIndex);
            writer.Write("\">......</option>");
        }

        void listPageIndices(HtmlTextWriter writer, int startIndex, int endIndex)
        {
            for (int i = startIndex; i <= endIndex; i++)
            {
                writer.Write("<option value=\"");
                writer.Write(i);
                writer.Write("\"");
                if (i == CurrentPageIndex)
                    writer.Write(" selected=\"true\"");
                writer.Write(">");
                writer.Write(i);
                writer.Write("</option>");
            }
        }

        private void RenderCustomInfoSection(HtmlTextWriter writer)
        {
            if (Height != Unit.Empty)
                writer.AddStyleAttribute(HtmlTextWriterStyle.Height, Height.ToString());
            string customUnit = CustomInfoSectionWidth.ToString();
            if (CustomInfoClass != null && CustomInfoClass.Trim().Length > 0)
                writer.AddAttribute(HtmlTextWriterAttribute.Class, CustomInfoClass);
            if (CustomInfoStyle != null && CustomInfoStyle.Trim().Length > 0)
                writer.AddAttribute(HtmlTextWriterAttribute.Style, CustomInfoStyle);
            writer.AddStyleAttribute(HtmlTextWriterStyle.Width, customUnit);
            if (CustomInfoTextAlign != HorizontalAlign.NotSet)
                writer.AddAttribute(HtmlTextWriterAttribute.Align, CustomInfoTextAlign.ToString().ToLower());
            if (LayoutType == LayoutType.Div)
            {
                writer.AddStyleAttribute("float", "left");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
            }
            else
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Valign, "bottom");
                writer.AddAttribute(HtmlTextWriterAttribute.Nowrap, "true");
                writer.RenderBeginTag(HtmlTextWriterTag.Td);
            }
            writer.Write(GetCustomInfoHtml(CustomInfoHTML));
            writer.RenderEndTag();
        }

        private void RenderNavigationSection(HtmlTextWriter writer)
        {
            if (CustomInfoSectionWidth.Type == UnitType.Percentage)
            {
                writer.AddStyleAttribute(HtmlTextWriterStyle.Width,
                                         (Unit.Percentage(100 - CustomInfoSectionWidth.Value)).ToString());
            }
            if (HorizontalAlign != HorizontalAlign.NotSet)
                writer.AddAttribute(HtmlTextWriterAttribute.Align, HorizontalAlign.ToString().ToLower());
            if (!string.IsNullOrEmpty(CssClass))
                writer.AddAttribute(HtmlTextWriterAttribute.Class, CssClass);
            if (LayoutType == LayoutType.Div)
            {
                writer.AddStyleAttribute("float", "left");
                writer.RenderBeginTag(HtmlTextWriterTag.Div); //<div>
            }
            else
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Valign, "bottom");
                writer.AddAttribute(HtmlTextWriterAttribute.Nowrap, "true");
                writer.RenderBeginTag(HtmlTextWriterTag.Td); //<td>
            }
            RenderPagingElements(writer);
            writer.RenderEndTag(); //</div> or </td>
        }

        private void RenderPagingElements(HtmlTextWriter writer)
        {
            int startIndex = ((CurrentPageIndex - 1)/NumericButtonCount)*NumericButtonCount;
                //this is an important trick, it's not the same as CurrentPageIndex-1
            if (PageCount > NumericButtonCount && CurrentPageButtonPosition != PagingButtonPosition.Fixed)
            {
                switch (CurrentPageButtonPosition)
                {
                    case PagingButtonPosition.End:
                        if (CurrentPageIndex > NumericButtonCount)
                            startIndex = CurrentPageIndex - NumericButtonCount;
                        break;
                    case PagingButtonPosition.Center:
                        int startOffset = CurrentPageIndex - (int) (Math.Ceiling((double) NumericButtonCount/2));
                        if (startOffset > 0)
                        {
                            startIndex = startOffset;
                            if (startIndex > (PageCount - NumericButtonCount))
                                startIndex = PageCount - NumericButtonCount;
                        }
                        break;
                    case PagingButtonPosition.Beginning:
                        startIndex = CurrentPageIndex - 1;
                        if (startIndex + NumericButtonCount > PageCount)
                            startIndex = PageCount - NumericButtonCount;
                        break;
                }
            }

            int endIndex = ((startIndex + NumericButtonCount) > PageCount)
                               ? PageCount
                               : (startIndex + NumericButtonCount);

            if (PagingButtonLayoutType == PagingButtonLayoutType.UnorderedList)
                writer.RenderBeginTag(HtmlTextWriterTag.Ul); //<ul>

            if (NavigationButtonsPosition == NavigationButtonPosition.Left ||
                NavigationButtonsPosition == NavigationButtonPosition.BothSides)
            {
                CreateNavigationButton(writer, NavigationButton.First);
                CreateNavigationButton(writer, NavigationButton.Prev);
                if (NavigationButtonsPosition == NavigationButtonPosition.Left)
                {
                    CreateNavigationButton(writer, NavigationButton.Next);
                    CreateNavigationButton(writer, NavigationButton.Last);
                }
            }

            if (AlwaysShowFirstLastPageNumber && startIndex > 1)
                CreateNumericButton(writer, 1);

            if (ShowMoreButtons && startIndex > 0)
                CreateMoreButton(writer, startIndex);
            if (ShowPageIndex)
            {
                for (int i = startIndex + 1; i <= endIndex; i++)
                {
                    CreateNumericButton(writer, i);
                }
            }
            if (ShowMoreButtons && PageCount > NumericButtonCount && endIndex < PageCount)
                CreateMoreButton(writer, endIndex + 1);

            if (AlwaysShowFirstLastPageNumber && endIndex < PageCount)
                CreateNumericButton(writer, PageCount);

            if (NavigationButtonsPosition == NavigationButtonPosition.Right ||
                NavigationButtonsPosition == NavigationButtonPosition.BothSides)
            {
                if (NavigationButtonsPosition == NavigationButtonPosition.Right)
                {
                    CreateNavigationButton(writer, NavigationButton.First);
                    CreateNavigationButton(writer, NavigationButton.Prev);
                }
                CreateNavigationButton(writer, NavigationButton.Next);
                CreateNavigationButton(writer, NavigationButton.Last);
            }

            if (PagingButtonLayoutType == PagingButtonLayoutType.UnorderedList)
                writer.RenderEndTag(); //</ul>


            if ((ShowPageIndexBox == ShowPageIndexBox.Always) ||
                (ShowPageIndexBox == ShowPageIndexBox.Auto && PageCount >= ShowBoxThreshold))
            {
                string boxClientId = UniqueID + "_input";
                writer.Write("&nbsp;&nbsp;");
                if (!string.IsNullOrEmpty(TextBeforePageIndexBox))
                    writer.Write(TextBeforePageIndexBox);
                if (PageIndexBoxType == PageIndexBoxType.TextBox) //TextBox
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
                    writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "30px");
                    writer.AddAttribute(HtmlTextWriterAttribute.Value, CurrentPageIndex.ToString());
                    if (!string.IsNullOrEmpty(PageIndexBoxStyle))
                        writer.AddAttribute(HtmlTextWriterAttribute.Style, PageIndexBoxStyle);
                    if (!string.IsNullOrEmpty(PageIndexBoxClass))
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, PageIndexBoxClass);
                    if (!Enabled || (PageCount <= 1 && AlwaysShow))
                        writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "disabled");
                    writer.AddAttribute(HtmlTextWriterAttribute.Name, boxClientId);
                    writer.AddAttribute(HtmlTextWriterAttribute.Id, boxClientId);
                    string chkInputScript = "ANP_checkInput(\'" + boxClientId + "\'," + PageCount + ")";
                    string keydownScript = "ANP_keydown(event,\'" + UniqueID + "_btn\');";
                    string clickScript = "if(" + chkInputScript + "){ANP_goToPage(document.getElementById(\'" +
                                         boxClientId + "\'));};return false;";

                    writer.AddAttribute("onkeydown", keydownScript);
                    writer.RenderBeginTag(HtmlTextWriterTag.Input);
                    writer.RenderEndTag();
                    //Text after page index box
                    if (!string.IsNullOrEmpty(TextAfterPageIndexBox))
                        writer.Write(TextAfterPageIndexBox);

                    //button
                    if (!string.IsNullOrEmpty(SubmitButtonImageUrl))
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Type, "image");
                        writer.AddAttribute(HtmlTextWriterAttribute.Src, SubmitButtonImageUrl);
                    }
                    else
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Type, UrlPaging ? "button" : "submit");
                        writer.AddAttribute(HtmlTextWriterAttribute.Value, SubmitButtonText);
                    }
                    writer.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID);
                    writer.AddAttribute(HtmlTextWriterAttribute.Id, UniqueID + "_btn");
                    if (!string.IsNullOrEmpty(SubmitButtonClass))
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, SubmitButtonClass);
                    if (!string.IsNullOrEmpty(SubmitButtonStyle))
                        writer.AddAttribute(HtmlTextWriterAttribute.Style, SubmitButtonStyle);
                    if (!Enabled || (PageCount <= 1 && AlwaysShow))
                        writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "disabled");
                    writer.AddAttribute(HtmlTextWriterAttribute.Onclick,
                                        (UrlPaging)
                                            ? clickScript
                                            : "if(" + chkInputScript + "){" +
                                              Page.ClientScript.GetPostBackEventReference(this, null) +
                                              "} else{return false}");
                    writer.RenderBeginTag(HtmlTextWriterTag.Input);
                    writer.RenderEndTag();
                }
                else //Dropdownlist
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Name, boxClientId);
                    writer.AddAttribute(HtmlTextWriterAttribute.Id, boxClientId);
                    writer.AddAttribute(HtmlTextWriterAttribute.Onchange,
                                        UrlPaging
                                            ? "ANP_goToPage(this)"
                                            : Page.ClientScript.GetPostBackEventReference(this, null));
                    if (!string.IsNullOrEmpty(PageIndexBoxStyle))
                        writer.AddAttribute(HtmlTextWriterAttribute.Style, PageIndexBoxStyle);
                    if (!string.IsNullOrEmpty(PageIndexBoxClass))
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, PageIndexBoxClass);
                    writer.RenderBeginTag(HtmlTextWriterTag.Select);
                    if (PageCount > 80) //list only part of page indices
                    {
                        if (CurrentPageIndex <= 15)
                        {
                            listPageIndices(writer, 1, 15);
                            addMoreListItem(writer, 16);
                            listPageIndices(writer, PageCount - 4, PageCount);
                        }
                        else if (CurrentPageIndex >= PageCount - 14)
                        {
                            listPageIndices(writer, 1, 5);
                            addMoreListItem(writer, PageCount - 15);
                            listPageIndices(writer, PageCount - 14, PageCount);
                        }
                        else
                        {
                            listPageIndices(writer, 1, 5);
                            addMoreListItem(writer, CurrentPageIndex - 6);
                            listPageIndices(writer, CurrentPageIndex - 5, CurrentPageIndex + 5);
                            addMoreListItem(writer, CurrentPageIndex + 6);
                            listPageIndices(writer, PageCount - 4, PageCount);
                        }
                    }
                    else //list all page indices
                        listPageIndices(writer, 1, PageCount);
                    writer.RenderEndTag();
                    if (!string.IsNullOrEmpty(TextAfterPageIndexBox))
                        writer.Write(TextAfterPageIndexBox);
                }
            }
        }

        /// <summary>
        /// Get the navigation url for the paging button.
        /// </summary>
        /// <param name="pageIndex">the page index correspond to the button.</param>
        /// <returns>href string for the paging navigation button.</returns>
        private string GetHrefString(int pageIndex)
        {
            if (UrlPaging)
            {
                int urlPageIndex = pageIndex;
                string jsValue = "pi";
                if (ReverseUrlPageIndex)
                {
                    jsValue = "(" + PageCount + "-pi+1)";
                    urlPageIndex = pageIndex == -1 ? -1 : PageCount - pageIndex + 1;
                }
                if (EnableUrlRewriting)
                {
                    Regex reg = new Regex("(?<p>%(?<m>[^%]+)%)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                    MatchCollection mts = reg.Matches(UrlRewritePattern);
                    string prmValue;
                    NameValueCollection urlParams = ConvertQueryStringToCollection(queryString);
                    string url = UrlRewritePattern;
                    foreach (Match m in mts)
                    {
                        prmValue = urlParams[m.Groups["m"].Value];
                        //if (!string.IsNullOrEmpty(prmValue))
                            url = url.Replace(m.Groups["p"].Value, prmValue);
                    }
                    return ResolveUrl(string.Format(url, (urlPageIndex == -1) ? "\"+" + jsValue + "+\"" : urlPageIndex.ToString()));
                }
                else
                {
                    return BuildUrlString(UrlPageIndexName, (urlPageIndex == -1) ? "\"+" + jsValue + "+\"" : urlPageIndex.ToString());
                }
            }
            return Page.ClientScript.GetPostBackClientHyperlink(this, pageIndex.ToString());
        }

        /// <summary>
        /// Replace the property placeholders in the CustomInfoHTML with the property values repectively
        /// </summary>
        /// <param name="origText">original CustomInfoHTML</param>
        /// <returns></returns>
        private string GetCustomInfoHtml(string origText)
        {
            if (!string.IsNullOrEmpty(origText) && origText.IndexOf('%') >= 0)
            {
                string[] props = new string[] { "recordcount", "pagecount", "currentpageindex", "startrecordindex", "endrecordindex", "pagesize", "pagesremain", "recordsremain" };
                StringBuilder sb = new StringBuilder(origText);
                Regex reg = new Regex("(?<ph>%(?<pname>\\w{8,})%)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                MatchCollection mts = reg.Matches(origText);
                foreach (Match m in mts)
                {
                    string p = m.Groups["pname"].Value.ToLower();
                    if (Array.IndexOf(props, p) >= 0)
                    {
                        string repValue = null;
                        switch (p)
                        {
                            case "recordcount":
                                repValue = RecordCount.ToString(); break;
                            case "pagecount":
                                repValue = PageCount.ToString(); break;
                            case "currentpageindex":
                                repValue = CurrentPageIndex.ToString(); break;
                            case "startrecordindex":
                                repValue = StartRecordIndex.ToString(); break;
                            case "endrecordindex":
                                repValue = EndRecordIndex.ToString(); break;
                            case "pagesize":
                                repValue = PageSize.ToString(); break;
                            case "pagesremain":
                                repValue = PagesRemain.ToString(); break;
                            case "recordsremain":
                                repValue = RecordsRemain.ToString(); break;
                        }
                        if (repValue != null)
                            sb.Replace(m.Groups["ph"].Value, repValue);
                    }
                }
                return sb.ToString();
            }
            return origText;
        }

        /// <summary>
        /// Convert raw query string to NameValueCollection
        /// </summary>
        /// <param name="s">raw query string</param>
        private static NameValueCollection ConvertQueryStringToCollection(string s)
        {
            NameValueCollection prms = new NameValueCollection();
            int num = (s != null) ? s.Length : 0;
            for (int i = 0; i < num; i++)
            {
                int startIndex = i;
                int num4 = -1;
                while (i < num)
                {
                    char ch = s[i];
                    if (ch == '=')
                    {
                        if (num4 < 0)
                        {
                            num4 = i;
                        }
                    }
                    else if (ch == '&')
                    {
                        break;
                    }
                    i++;
                }
                string skey = null;
                string svalue;
                if (num4 >= 0)
                {
                    skey = s.Substring(startIndex, num4 - startIndex);
                    svalue = s.Substring(num4 + 1, (i - num4) - 1);
                }
                else
                {
                    svalue = s.Substring(startIndex, i - startIndex);
                }
                prms.Add(skey, svalue);
                if ((i == (num - 1)) && (s[i] == '&'))
                {
                    prms.Add(null, string.Empty);
                }
            }
            return prms;
        }

        /// <summary>
        /// add paging parameter and value to the current url or change parameter value if it already exists when using url paging mode
        /// </summary>
        /// <param name="sk">name of the url parameter to be added</param>
        /// <param name="sv">value of the url paramter to be added</param>
        /// <returns>href string for the navigattion button</returns>
        private string BuildUrlString(string sk, string sv)
        {
            StringBuilder ubuilder = new StringBuilder(80);
            bool keyFound = false;
            int num = (queryString != null) ? queryString.Length : 0;
            for (int i = 0; i < num; i++)
            {
                int startIndex = i;
                int num4 = -1;
                while (i < num)
                {
                    char ch = queryString[i];
                    if (ch == '=')
                    {
                        if (num4 < 0)
                        {
                            num4 = i;
                        }
                    }
                    else if (ch == '&')
                    {
                        break;
                    }
                    i++;
                }
                string skey = null;
                string svalue;
                if (num4 >= 0)
                {
                    skey = queryString.Substring(startIndex, num4 - startIndex);
                    svalue = queryString.Substring(num4 + 1, (i - num4) - 1);
                }
                else
                {
                    svalue = queryString.Substring(startIndex, i - startIndex);
                }
                ubuilder.Append(skey).Append("=");
                if (skey == sk)
                {
                    keyFound = true;
                    ubuilder.Append(sv);
                }
                else
                    ubuilder.Append(svalue);
                ubuilder.Append("&");
            }
            if (!keyFound)
                ubuilder.Append(sk).Append("=").Append(sv);
            ubuilder.Insert(0, "?").Insert(0, Path.GetFileName(currentUrl));
            return ubuilder.ToString().Trim('&');
        }

        /// <summary>
        /// Create first, prev, next or last button.
        /// </summary>
        /// <param name="writer">A <see cref="System.Web.UI.HtmlTextWriter"/> that represents the output stream to render HTML content on the client.</param>
        /// <param name="btn">the navigation button</param>
        private void CreateNavigationButton(HtmlTextWriter writer, NavigationButton btn)
        {
            if (!ShowFirstLast && (btn == NavigationButton.First || btn == NavigationButton.Last))
                return;
            if (!ShowPrevNext && (btn == NavigationButton.Prev || btn == NavigationButton.Next))
                return;

            if (PagingButtonLayoutType != PagingButtonLayoutType.None) //add css class and style attribute to pager item directly
            {
                if (btn == NavigationButton.First || btn == NavigationButton.Last) //first page or last page button
                    AddClassAndStyle(FirstLastButtonsClass, FirstLastButtonsStyle, writer);
                else
                    AddClassAndStyle(PrevNextButtonsClass, PrevNextButtonsStyle, writer);//next page or prevous page button
            }
            AddPagingButtonLayoutTag(writer); //<li> or <span>

            string linktext;
            bool disabled;
            int pageIndex;
            string btnname = btn.ToString().ToLower();
            bool isImgBtn = (//PagingButtonType == PagingButtonType.Image &&
                              NavigationButtonType == PagingButtonType.Image);
            if (btn == NavigationButton.First || btn == NavigationButton.Prev)
            {
                disabled = (CurrentPageIndex <= 1) | !Enabled;
                if (!ShowDisabledButtons && disabled)
                    return;
                pageIndex = (btn == NavigationButton.First) ? 1 : (CurrentPageIndex - 1);
                writeSpacingStyle(writer);
                if (PagingButtonLayoutType == PagingButtonLayoutType.None) //add css class and style attribute to pager item directly
                {
                    if (btn == NavigationButton.First || btn == NavigationButton.Last) //first page or last page button
                        AddClassAndStyle(FirstLastButtonsClass, FirstLastButtonsStyle, writer);
                    else
                        AddClassAndStyle(PrevNextButtonsClass, PrevNextButtonsStyle, writer);//next page or prevous page button
                }
                if (isImgBtn)
                {
                    if (!disabled)
                    {
                        writer.AddAttribute("href", GetHrefString(pageIndex),false);
                        AddToolTip(writer, pageIndex);
                        AddHyperlinkTarget(writer);
                        writer.RenderBeginTag(HtmlTextWriterTag.A);
                        writer.AddAttribute(HtmlTextWriterAttribute.Src,
                                            String.Concat(ImagePath, btnname, ButtonImageNameExtension,
                                                          ButtonImageExtension));
                        writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
                        if (ButtonImageAlign != ImageAlign.NotSet)
                            writer.AddAttribute(HtmlTextWriterAttribute.Align, ButtonImageAlign.ToString());
                        writer.RenderBeginTag(HtmlTextWriterTag.Img);
                        writer.RenderEndTag();
                        writer.RenderEndTag();
                    }
                    else
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Src,
                                            String.Concat(ImagePath, btnname, DisabledButtonImageNameExtension,
                                                          ButtonImageExtension));
                        writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
                        if (ButtonImageAlign != ImageAlign.NotSet)
                            writer.AddAttribute(HtmlTextWriterAttribute.Align, ButtonImageAlign.ToString());
                        writer.RenderBeginTag(HtmlTextWriterTag.Img);
                        writer.RenderEndTag();
                    }
                }
                else
                {
                    linktext = (btn == NavigationButton.Prev) ? PrevPageText : FirstPageText;
                    if (disabled)
                        writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "disabled");
                    else
                    {
                        //WriteCssClass(writer);
                        AddToolTip(writer, pageIndex);
                        AddHyperlinkTarget(writer);
                        writer.AddAttribute("href", GetHrefString(pageIndex),false);
                    }
                    writer.RenderBeginTag(HtmlTextWriterTag.A);
                    writer.Write(linktext);
                    writer.RenderEndTag();
                }
            }
            else
            {
                disabled = (CurrentPageIndex >= PageCount) | !Enabled;
                if (!ShowDisabledButtons && disabled)
                    return;
                pageIndex = (btn == NavigationButton.Last) ? PageCount : (CurrentPageIndex + 1);
                writeSpacingStyle(writer);
                if (PagingButtonLayoutType == PagingButtonLayoutType.None) //add css class and style attribute to pager item directly
                {
                    if (btn == NavigationButton.First || btn == NavigationButton.Last) //first page or last page button
                        AddClassAndStyle(FirstLastButtonsClass, FirstLastButtonsStyle, writer);
                    else
                        AddClassAndStyle(PrevNextButtonsClass, PrevNextButtonsStyle, writer);//next page or prevous page button
                }
                if (isImgBtn)
                {
                    if (!disabled)
                    {
                        writer.AddAttribute("href", GetHrefString(pageIndex),false);
                        AddToolTip(writer, pageIndex);
                        AddHyperlinkTarget(writer);
                        writer.RenderBeginTag(HtmlTextWriterTag.A);
                        writer.AddAttribute(HtmlTextWriterAttribute.Src,
                                            String.Concat(ImagePath, btnname, ButtonImageNameExtension,
                                                          ButtonImageExtension));
                        writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
                        if (ButtonImageAlign != ImageAlign.NotSet)
                            writer.AddAttribute(HtmlTextWriterAttribute.Align, ButtonImageAlign.ToString());
                        writer.RenderBeginTag(HtmlTextWriterTag.Img);
                        writer.RenderEndTag();
                        writer.RenderEndTag();
                    }
                    else
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Src,
                                            String.Concat(ImagePath, btnname, DisabledButtonImageNameExtension,
                                                          ButtonImageExtension));
                        writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
                        if (ButtonImageAlign != ImageAlign.NotSet)
                            writer.AddAttribute(HtmlTextWriterAttribute.Align, ButtonImageAlign.ToString());
                        writer.RenderBeginTag(HtmlTextWriterTag.Img);
                        writer.RenderEndTag();
                    }
                }
                else
                {
                    linktext = (btn == NavigationButton.Next) ? NextPageText : LastPageText;
                    if (disabled)
                        writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "disabled");
                    else
                    {
                        //WriteCssClass(writer);
                        AddToolTip(writer, pageIndex);
                        writer.AddAttribute("href", GetHrefString(pageIndex),false);
                        AddHyperlinkTarget(writer);
                    }
                    writer.RenderBeginTag(HtmlTextWriterTag.A);
                    writer.Write(linktext);
                    writer.RenderEndTag();
                }
            }
            if (PagingButtonLayoutType != PagingButtonLayoutType.None)
                writer.RenderEndTag(); //</li> or </span>
        }

        /// <summary>
        /// Write css class name.
        /// </summary>
        /// <param name="writer">A <see cref="System.Web.UI.HtmlTextWriter"/> that represents the output stream to render HTML content on the client. </param>
        //private void WriteCssClass(HtmlTextWriter writer)
        //{
        //    if (cssClassName != null && cssClassName.Trim().Length > 0)
        //        writer.AddAttribute(HtmlTextWriterAttribute.Class, cssClassName);
        //}

        /// <summary>
        /// Add tool tip text to navigation button.
        /// </summary>
        private void AddToolTip(HtmlTextWriter writer, int pageIndex)
        {
            if (ShowNavigationToolTip)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Title, String.Format(NavigationToolTipTextFormatString, pageIndex));
            }
        }
        /// <summary>
        /// add paging button layout tag
        /// </summary>
        private void AddPagingButtonLayoutTag(HtmlTextWriter writer)
        {
            if (PagingButtonLayoutType == PagingButtonLayoutType.UnorderedList)
                writer.RenderBeginTag(HtmlTextWriterTag.Li); //<li>
            else if (PagingButtonLayoutType == PagingButtonLayoutType.Span)
                writer.RenderBeginTag(HtmlTextWriterTag.Span); //<span>
        }

        /// <summary>
        /// Create numeric paging button.
        /// </summary>
        /// <param name="writer">A <see cref="System.Web.UI.HtmlTextWriter"/> that represents the output stream to render HTML content on the client.</param>
        /// <param name="index">the page index correspond to the paging button</param>
        private void CreateNumericButton(HtmlTextWriter writer, int index)
        {
            bool isCurrent = (index == CurrentPageIndex);

            if ((!isCurrent && PagingButtonLayoutType != PagingButtonLayoutType.None) || (isCurrent && PagingButtonLayoutType == PagingButtonLayoutType.UnorderedList)) //current page button already wrapped in span
            {
                if (!isCurrent) //exclude current page index button
                    AddClassAndStyle(PagingButtonsClass, PagingButtonsStyle, writer);
                AddPagingButtonLayoutTag(writer); //<li>
            }

            if (/*PagingButtonType == PagingButtonType.Image && */NumericButtonType == PagingButtonType.Image)
            {
                writeSpacingStyle(writer);
                if (!isCurrent)
                {
                    if (Enabled)
                        writer.AddAttribute("href", GetHrefString(index),false);
                    //if (PagingButtonLayoutType == PagingButtonLayoutType.None) //add css class and style attribute to hyper link directly
                        AddClassAndStyle(PagingButtonsClass, PagingButtonsStyle, writer);

                    AddToolTip(writer, index);
                    AddHyperlinkTarget(writer);
                    writer.RenderBeginTag(HtmlTextWriterTag.A);
                    CreateNumericImages(writer, index, false);
                    writer.RenderEndTag();
                }
                else
                {
                    if (!string.IsNullOrEmpty(CurrentPageButtonClass))
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, CurrentPageButtonClass);
                    if (!string.IsNullOrEmpty(CurrentPageButtonStyle))
                        writer.AddAttribute(HtmlTextWriterAttribute.Style, CurrentPageButtonStyle);
                    writer.RenderBeginTag(HtmlTextWriterTag.Span);
                    CreateNumericImages(writer, index, true);
                    writer.RenderEndTag();
                }
            }
            else
            {
                writeSpacingStyle(writer);
                if (isCurrent)
                {
                    if (string.IsNullOrEmpty(CurrentPageButtonClass) && string.IsNullOrEmpty(CurrentPageButtonStyle))
                    {
                        writer.AddStyleAttribute(HtmlTextWriterStyle.FontWeight, "Bold");
                        writer.AddStyleAttribute(HtmlTextWriterStyle.Color, "red");
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(CurrentPageButtonClass))
                            writer.AddAttribute(HtmlTextWriterAttribute.Class, CurrentPageButtonClass);
                        if (!string.IsNullOrEmpty(CurrentPageButtonStyle))
                            writer.AddAttribute(HtmlTextWriterAttribute.Style, CurrentPageButtonStyle);
                    }
                    writer.RenderBeginTag(HtmlTextWriterTag.Span);
                    if (!string.IsNullOrEmpty(CurrentPageButtonTextFormatString))
                        writer.Write(String.Format(CurrentPageButtonTextFormatString, index));
                    else
                        writer.Write(index);
                    writer.RenderEndTag();
                }
                else
                {
                    if (Enabled)
                    {
                        writer.AddAttribute("href", GetHrefString(index),false);
                        AddClassAndStyle(PagingButtonsClass, PagingButtonsStyle, writer); //add css class and style
                    }
                    //WriteCssClass(writer);
                    AddToolTip(writer, index);
                    AddHyperlinkTarget(writer);
                    writer.RenderBeginTag(HtmlTextWriterTag.A);
                    if (!string.IsNullOrEmpty(NumericButtonTextFormatString))
                        writer.Write(String.Format(NumericButtonTextFormatString, index));
                    else
                        writer.Write(index);
                    writer.RenderEndTag();
                }
            }
            if ((!isCurrent && PagingButtonLayoutType != PagingButtonLayoutType.None) || (isCurrent && PagingButtonLayoutType == PagingButtonLayoutType.UnorderedList))
                writer.RenderEndTag(); //</li>
        }

        /// <summary>
        /// Create numeric image button.
        /// </summary>
        /// <param name="writer">A <see cref="System.Web.UI.HtmlTextWriter"/> that represents the output stream to render HTML content on the client.</param>
        /// <param name="index">the page index correspond to the button.</param>
        /// <param name="isCurrent">if the page index correspond to the button is the current page index</param>
        private void CreateNumericImages(HtmlTextWriter writer, int index, bool isCurrent)
        {
            AddPagingButtonLayoutTag(writer); //<li> or <span>

            string indexStr = index.ToString();
            for (int i = 0; i < indexStr.Length; i++)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Src, String.Concat(ImagePath, indexStr[i], (isCurrent) ? CpiButtonImageNameExtension : ButtonImageNameExtension, ButtonImageExtension));
                if (ButtonImageAlign != ImageAlign.NotSet)
                    writer.AddAttribute(HtmlTextWriterAttribute.Align, ButtonImageAlign.ToString());
                writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
                writer.RenderBeginTag(HtmlTextWriterTag.Img);
                writer.RenderEndTag();
            }
            if (PagingButtonLayoutType != PagingButtonLayoutType.None)
                writer.RenderEndTag(); //</li> or </span>
        }

        /// <summary>
        /// create more (...) button.
        /// </summary>
        private void CreateMoreButton(HtmlTextWriter writer, int pageIndex)
        {
            AddClassAndStyle(MoreButtonsClass, MoreButtonsStyle, writer);
            AddPagingButtonLayoutTag(writer); //<li> or <span>

            writeSpacingStyle(writer);
            if (Enabled)
            {
                writer.AddAttribute("href", GetHrefString(pageIndex),false);
                AddToolTip(writer, pageIndex);
                AddHyperlinkTarget(writer);
            }
            writer.RenderBeginTag(HtmlTextWriterTag.A); //<a>
            if ( /*PagingButtonType == PagingButtonType.Image && */MoreButtonType == PagingButtonType.Image)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Src,
                                    String.Concat(ImagePath, "more", ButtonImageNameExtension, ButtonImageExtension));
                writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
                if (ButtonImageAlign != ImageAlign.NotSet)
                    writer.AddAttribute(HtmlTextWriterAttribute.Align, ButtonImageAlign.ToString());
                writer.RenderBeginTag(HtmlTextWriterTag.Img); //<img>
                writer.RenderEndTag(); //</img>
            }
            else
                writer.Write("...");

            writer.RenderEndTag(); //</a>

            if (PagingButtonLayoutType != PagingButtonLayoutType.None)
                writer.RenderEndTag(); //</li> or </span>
        }

        /// <summary>
        /// Add paging button spacing styles to HtmlTextWriter
        /// </summary>
        private void writeSpacingStyle(HtmlTextWriter writer)
        {
            if (PagingButtonSpacing.Value != 0)
                writer.AddStyleAttribute(HtmlTextWriterStyle.MarginRight, PagingButtonSpacing.ToString());
        }

        /// <summary>
        /// add target attribute to paging hyperlink
        /// </summary>
        private void AddHyperlinkTarget(HtmlTextWriter writer)
        {
            if (!string.IsNullOrEmpty(UrlPagingTarget))
                writer.AddAttribute(HtmlTextWriterAttribute.Target, UrlPagingTarget);
        }

        /// <summary>
        /// add css class and style attribute to HtmlTextWriter
        /// </summary>
        private void AddClassAndStyle(string clsname, string style, HtmlTextWriter writer)
        {
            if (!string.IsNullOrEmpty(clsname))
                writer.AddAttribute(HtmlTextWriterAttribute.Class, clsname);
            if (!string.IsNullOrEmpty(style))
                writer.AddAttribute(HtmlTextWriterAttribute.Style, style);
        }

        #endregion
    }
}
