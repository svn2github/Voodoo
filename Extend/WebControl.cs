using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Data;
//using System.Windows.Forms;

namespace Voodoo
{
    //  2010年4月16日 17:34:22 
    //  Kuibono创建
    /// <summary>
    /// 页面空间处理相关类
    /// </summary>
    public static class WebControl
    {
        #region 选择CheckBoxList的所有项
        /// <summary>
        /// 选择CheckBoxList的所有项
        /// </summary>
        /// <param name="self"></param>
        public static void SelectAll(this CheckBoxList self)
        {
            foreach (ListItem lt in self.Items)
            {
                lt.Selected = true;
            }
        }
        #endregion

        #region 撤销选择CheckBoxList的所有项
        /// <summary>
        /// 撤销选择CheckBoxList的所有项
        /// </summary>
        /// <param name="self"></param>
        public static void UnSelectAll(this CheckBoxList self)
        {
            foreach (ListItem lt in self.Items)
            {
                lt.Selected = false;
            }
        }
        #endregion

        #region 选中CheckBox 的指定值
        /// <summary>
        /// 选中CheckBox 的指定值
        /// </summary>
        /// <example>如传入1,2,3,4 则选中值分别为1,2,3,4的项</example>
        /// <param name="self"></param>
        /// <param name="values"></param>
        public static void CheckItem(this CheckBoxList self, string values)
        {
            string[] v = values.Split(',');
            self.CheckItem(v);
        }
        /// <summary>
        /// 选中CheckBox 的指定值
        /// </summary>
        /// <example>如传入1,2,3,4 则选中值分别为1,2,3,4的项</example>
        /// <param name="self"></param>
        /// <param name="v">要选中的数组</param>
        public static void CheckItem(this CheckBoxList self, string[] v)
        {
            foreach (ListItem lt in self.Items)
            {
                if (lt.Value.IsInArray(v))
                {
                    lt.Selected = true;
                }
            }
        }

        #endregion

        #region 获取CheckBoxList的选中值
        /// <summary>
        /// 获取CheckBoxList的选中值
        /// </summary>
        /// <param name="self"></param>
        /// <returns>返回带逗号的字符串</returns>
        /// <example>返回1,2,3,4这种字符串</example>
        public static string GetValues(this ListControl self)
        {
            StringBuilder sb = new StringBuilder();
            foreach (ListItem lt in self.Items)
            {
                if (lt.Selected)
                {
                    sb.Append(lt.Value + ",");
                }
            }
            return sb.TrimEnd(',').ToString();
        }
        #endregion

        #region 获取CheckBoxList选中项的序号
        /// <summary>
        /// 获取CheckBoxList选中项的序号
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static string GetCheckedIndex(this ListControl self)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < self.Items.Count; i++)
            {
                if (self.Items[i].Selected)
                {
                    sb.Append(i.ToString() + ",");
                }
            }

            return sb.TrimEnd(',').ToString();
        }
        #endregion

        #region 获取CheckBoxList选中项的文本
        public static string GetTexts(this ListControl self)
        {
            StringBuilder sb = new StringBuilder();
            foreach (ListItem lt in self.Items)
            {
                if (lt.Selected)
                {
                    sb.Append(lt.Text + ",");
                }
            }
            return sb.TrimEnd(',').ToString();
        }
        #endregion

        #region DataTable转换为Xml字符串，为图表功能提供支持
        /// <summary>
        /// DataTable转换为Xml字符串，为图表功能提供支持
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="KeyColumn">主键，如:id</param>
        /// <param name="colors">颜色字符串，使用“,”分开。如：red,green.blue  或者#FF0000,#00FFFF,#0000FF</param>
        /// <returns></returns>
        public static string ToXML(this DataTable dt, string KeyColumn, string colors)
        {
            StringBuilder sb = new StringBuilder();
            //sb.Append("<?xml version='1.0' encoding='utf-8'?>");
            string[] color = colors.Split(',');
            sb.Append("<chart>");

            sb.Append("<series>");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sb.Append("<value xid='" + i + "'>" + dt.Rows[i][KeyColumn].ToString() + "</value>");
            }
            sb.Append("</series>");

            sb.Append("	<graphs>");
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (dt.Columns[i].ColumnName.ToLower() == KeyColumn.ToLower())
                {
                    continue;
                }
                string colorset = color.Length >= i ? " color='" + color[i] + "'" : "";
                sb.Append("<graph gid='" + i + "' title='" + dt.Columns[i].ColumnName + "' " + colorset + ">");
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    sb.Append("<value xid='" + j + "'>" + dt.Rows[j][i].ToString() + "</value>");
                }
                sb.Append("</graph>");
            }
            sb.Append("</graphs>");

            sb.Append("</chart>");
            return sb.ToString();
        }
        #endregion

        ///// <summary>
        ///// 设置ComboBox的值
        ///// </summary>
        ///// <param name="cb"></param>
        ///// <param name="value"></param>
        //public static void SetValue(this ComboBox cb,string value)
        //{
        //    bool _hasValue=false;

        //    foreach (string str in cb.Items)
        //    {
        //        if (str == value)
        //        {
        //            cb.SelectedItem = str;
        //            _hasValue = true;
        //        }
        //    }
        //    if (_hasValue == false)
        //    {
        //        cb.Items.Add(value);
        //        cb.SelectedItem = value;
        //    }

        //}

        /// <summary>
        /// 设置列表控件的值，如checkBoxList等
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="values"></param>
        public static void SetValue(this  System.Web.UI.WebControls.ListControl ctrl, string[] values)
        {
            foreach (string value in values)
            {
                foreach (ListItem item in ctrl.Items)
                {
                    if (item.Value == value)
                    {
                        item.Selected = true;
                    }
                }
            }
        }

        /// <summary>
        /// radioButtonList绑定数据
        /// </summary>
        /// <param name="rbl"></param>
        /// <param name="dt"></param>
        /// <param name="DataTextField"></param>
        /// <param name="DataValueField"></param>
        public static void Bind(this System.Web.UI.WebControls.ListControl rbl, DataTable dt, string DataTextField, string DataValueField)
        {
            rbl.DataSource = dt;
            rbl.DataTextField = DataTextField;
            rbl.DataValueField = DataValueField;
            rbl.DataBind();
        }

        public static void Bind(this System.Web.UI.WebControls.ListControl rbl, ListItemCollection li)
        {
            rbl.Items.Clear();
            foreach (ListItem l in li)
            {
                rbl.Items.Add(new ListItem(l.Text, l.Value));
            }
        }

        #region 输出AjaxForm的结果
        /// <summary>
        /// 输出AjaxForm的结果
        /// </summary>
        /// <param name="form"></param>
        public static void ResponseResult(this System.Web.UI.HtmlControls.HtmlForm form)
        {
            ResponseResult(form, "", "");
        }

        /// <summary>
        /// 输出AjaxForm的结果
        /// </summary>
        /// <param name="form"></param>
        /// <param name="result"></param>
        public static void ResponseResult(this System.Web.UI.HtmlControls.HtmlForm form, string result)
        {
            ResponseResult(form, result, "");
        }
        /// <summary>
        /// 输出AjaxForm的结果 并且进行跳转
        /// </summary>
        /// <param name="form"></param>
        /// <param name="result">结果消息</param>
        /// <param name="Url">跳转地址</param>
        public static void ResponseResult(this System.Web.UI.HtmlControls.HtmlForm form, string result, string Url)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("name", typeof(string));
            dt.Columns.Add("attr", typeof(string));
            dt.Columns.Add("val", typeof(string));

            #region 循环输出每个控件的状态
            foreach (System.Web.UI.Control uc in form.Controls)
            {

                try
                {
                    System.Web.UI.WebControls.WebControl c = (System.Web.UI.WebControls.WebControl)uc;

                    switch (c.GetType().Name)
                    {
                        case "Button":
                        case "VTextBox":
                        case "TextBox":
                            if (((TextBox)c).Enabled == false)
                            {
                                AddRow(dt, c.ID, "disabled", (!((TextBox)c).Enabled).ToString());
                            }
                            if (((TextBox)c).ReadOnly == false)
                            {
                                AddRow(dt, c.ID, "readonly", (!((TextBox)c).Enabled).ToString());
                            }
                            AddRow(dt, c.ID, "value", ((TextBox)c).Text);
                            break;
                        case "VCheckBox":
                        case "CheckBox":
                            if (((CheckBox)c).Enabled == false)
                            {
                                AddRow(dt, c.ID, "disabled", (!c.Attributes["Enabled"].ToBoolean()).ToString());
                            }
                            if (((CheckBox)c).Checked)
                            {
                                AddRow(dt, c.ID, "checked", "checked");
                            }
                            break;
                        case "RadioButton":
                            if (((RadioButton)c).Enabled == false)
                            {
                                AddRow(dt, c.ID, "disabled", (!c.Attributes["Enabled"].ToBoolean()).ToString());
                            }
                            if (((RadioButton)c).Checked)
                            {
                                AddRow(dt, c.ID, "checked", "checked");
                            }
                            break;
                        case "VListBox":
                        case "ListBox":
                        case "VDropDownList":
                        case "DropDownList":
                            AddRow(dt, c.ID, "value", ((ListControl)c).SelectedValue);
                            break;
                        case "LinkButton":
                            if (((LinkButton)c).Enabled == false)
                            {
                                AddRow(dt, c.ID, "disabled", (!c.Attributes["Enabled"].ToBoolean()).ToString());
                            }
                            AddRow(dt, c.ID, "text", ((LinkButton)c).Text);
                            break;
                        case "HyperLink":
                            if (((HyperLink)c).Enabled == false)
                            {
                                AddRow(dt, c.ID, "disabled", (!c.Attributes["Enabled"].ToBoolean()).ToString());
                            }
                            AddRow(dt, c.ID, "href", ((HyperLink)c).NavigateUrl);
                            AddRow(dt, c.ID, "text", ((HyperLink)c).Text);
                            break;
                        case "CheckBoxList":
                            AddRow(dt, c.ID, "checkindex", ((CheckBoxList)c).GetCheckedIndex());
                            break;
                        case "RadioButtonList":
                            AddRow(dt, c.ID, "selectindex", ((RadioButtonList)c).SelectedIndex.ToString());
                            break;

                    }

                }
                catch { }

            }
            #endregion

            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("'url':'" + Url + "','result':'" + result + "',data:" + dt.DataTableToJson(true));
            sb.Append("}");

            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.Write(sb);
            System.Web.HttpContext.Current.Response.End();
        }
        /// <summary>
        /// 私有方法
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <param name="c3"></param>
        public static void AddRow(DataTable dt, string c1, string c2, string c3)
        {
            DataRow r = dt.NewRow();
            r[0] = c1;
            r[1] = c2;
            r[2] = c3;
            dt.Rows.Add(r);
        }

        #endregion

        #region TreeNode节点全选
        /// <summary>
        /// TreeNode节点全选
        /// </summary>
        /// <param name="tn"></param>
        public static void CheckAll(this TreeNode tn)
        {
            tn.Checked = true;
            if (tn.ChildNodes.Count > 0)
            {
                foreach (TreeNode ctn in tn.ChildNodes)
                {
                    ctn.CheckAll();
                }
            }
        }

        public static void CheckAll(this System.Windows.Forms.TreeNode tn)
        {
            tn.Checked = true;
            if (tn.Nodes.Count > 0)
            {
                foreach (TreeNode ctn in tn.Nodes)
                {
                    ctn.CheckAll();
                }
            }
        }
        #endregion

    }
}
