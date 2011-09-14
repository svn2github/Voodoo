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
        public static string GetValues(this CheckBoxList self)
        {
            StringBuilder sb=new StringBuilder();
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

        public static string GetValue(this System.Web.UI.Control ctrl)
        {
            

            return "";
        }

        public static string GetTexts(this CheckBoxList self)
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

        #region DataTable转换为Xml字符串，为图表功能提供支持
        /// <summary>
        /// DataTable转换为Xml字符串，为图表功能提供支持
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="KeyColumn">主键，如:id</param>
        /// <param name="colors">颜色字符串，使用“,”分开。如：red,green.blue  或者#FF0000,#00FFFF,#0000FF</param>
        /// <returns></returns>
        public static string ToXML(this DataTable dt,string KeyColumn,string colors)
        {
            StringBuilder sb = new StringBuilder();
            //sb.Append("<?xml version='1.0' encoding='utf-8'?>");
            string[] color=colors.Split(',');
            sb.Append("<chart>");

            sb.Append("<series>");
            for (int i = 0; i < dt.Rows.Count;i++ )
            {
                sb.Append("<value xid='" + i + "'>" + dt.Rows[i][KeyColumn].ToString() + "</value>");
            }
            sb.Append("</series>");

            sb.Append("	<graphs>");
            for (int i = 0; i < dt.Columns.Count ;i++ )
            {
                if (dt.Columns[i].ColumnName.ToLower()==KeyColumn.ToLower())
                {
                    continue;
                }
                string colorset = color.Length >= i ? " color='"+color[i]+"'" : "";
                sb.Append("<graph gid='" + i + "' title='" + dt.Columns[i].ColumnName + "' " + colorset + ">");
                for (int j=0;j<dt.Rows.Count;j++)
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


    }
}
