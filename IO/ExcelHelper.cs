using System.Data;
using Voodoo.Data;
using Voodoo.Data.DbHelper;
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using NPOI.POIFS.FileSystem;
using NPOI.SS.UserModel;
using System.IO;
using System.Web;
using System.Collections.Generic;

namespace Voodoo.IO
{
    public class ExcelHelper
    {


        public static System.Data.DataTable GetDataFromXLS(string s_FileName)
        {
            IDbHelper Helper = new OleDbHelper("Provider=Microsoft.Jet.OLEDB.4.0;Data Source = " + s_FileName + ";Extended Properties = Excel 8.0");
            return Helper.ExecuteDataTable(CommandType.Text, "select * from [Sheet1$]");
        }

        //从Grid生成Excel
        public static bool SetExcelFromData(System.Data.DataTable dtData, string FileName)
        {
            System.Web.UI.WebControls.GridView dgExport = null;
            //当前对话 
            System.Web.HttpContext curContext = System.Web.HttpContext.Current;
            //IO用于导出并返回excel文件 
            System.IO.StringWriter strWriter = null;
            System.Web.UI.HtmlTextWriter htmlWriter = null;
            if (dtData != null)
            {
                //设置编码和附件格式 
                //System.Web.HttpUtility.UrlEncode(FileName, System.Text.Encoding.UTF8)作用是方式中文文件名乱码
                curContext.Response.AddHeader("content-disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(FileName, System.Text.Encoding.UTF8) + ".xls");
                curContext.Response.ContentType = "application nd.ms-excel";
                curContext.Response.ContentEncoding = System.Text.Encoding.UTF8;
                curContext.Response.Charset = "UTF-8";
                //导出Excel文件 
                strWriter = new System.IO.StringWriter();
                htmlWriter = new System.Web.UI.HtmlTextWriter(strWriter);
                //为了解决dgData中可能进行了分页的情况,需要重新定义一个无分页的GridView 
                dgExport = new System.Web.UI.WebControls.GridView();
                dgExport.DataSource = dtData.DefaultView;
                dgExport.AllowPaging = false;
                dgExport.DataBind();
                //下载到客户端 
                dgExport.RenderControl(htmlWriter);
                curContext.Response.Clear();
                curContext.Response.Write("<meta http-equiv=Content-Type content=text/HTml;charset=UTF-8>"); 
                curContext.Response.Write(strWriter.ToString());
                return true;
                //curContext.Response.End();
                //return true;
            }
            return false;
        }

        #region 私有方法 生成SHEET列表
        /// <summary>
        /// 私有方法 生成SHEET列表
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="dtData"></param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        private static List<Sheet> GetSheet(HSSFWorkbook workbook,System.Data.DataTable dtData,string sheetName)
        {

            List<Sheet> list = new List<Sheet>();

            Sheet sheet = workbook.CreateSheet(sheetName);


            //头样式
            HSSFCellStyle headStyle = (HSSFCellStyle)workbook.CreateCellStyle();

            HSSFFont font = (HSSFFont)workbook.CreateFont();
            font.FontHeightInPoints = 10;
            font.Boldweight = 700;
            headStyle.SetFont(font);

            //填充表头
            HSSFRow header = (HSSFRow)sheet.CreateRow(0);
            foreach (DataColumn column in dtData.Columns)
            {
                header.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                header.GetCell(column.Ordinal).CellStyle = headStyle;
            }

           

 

            //填充内容  
            DataTable dt = dtData.Copy();

            int i = 0;
            foreach (DataRow row in dtData.Rows)
            {
                if (i < 60000)
                {
                    Row dataRow = sheet.CreateRow(i + 1);
                    int j = 0;
                    foreach (DataColumn c in dtData.Columns)
                    {
                        dataRow.CreateCell(j).SetCellValue(dtData.Rows[i][j].ToString());
                        j++;
                    }
                    dt.Rows.RemoveAt(0);
                    i++;
                    
                }

            }
            list.Add(sheet);
            if (dt.Rows.Count > 0)
            {
                List<Sheet> sheetLeft = GetSheet(workbook, dt, sheetName + " 剩余");
                foreach(Sheet s in sheetLeft)
                {
                    list.Add(s);
                }
            }
            return list;

        }
        #endregion

        #region 私有方法 把DataTable转换成WorkBook
        /// <summary>
        /// 私有方法 把DataTable转换成WorkBook
        /// </summary>
        /// <param name="dtData"></param>
        /// <returns></returns>
        private static HSSFWorkbook GetWorkBook(System.Data.DataTable dtData, string FileName)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = "kuibono@163.com";
            dsi.Manager = "kuibono";


            workbook.DocumentSummaryInformation = dsi;

            //创建sheet
            //Sheet sheet = workbook.CreateSheet(dtData.TableName);

            GetSheet(workbook, dtData, FileName);


            return workbook;
        }
        #endregion

        #region DataTable转换成Excel并且保存成文件
        /// <summary>
        /// DataTable转换成Excel并且保存成文件
        /// </summary>
        /// <param name="dtData">需要转换的DataTable</param>
        /// <param name="FileName">保存文件的绝对路径</param>
        public static void Export(System.Data.DataTable dtData, string FileName)
        {
            HSSFWorkbook workbook = GetWorkBook(dtData,FileName);
            //保存  
            using (FileStream fs = new FileStream(FileName, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(fs);
            }
            workbook.Dispose();
        }
        #endregion

        #region 将DataTable转换成Excel并输出到页面
        /// <summary>
        /// 将DataTable转换成Excel并输出到页面
        /// </summary>
        /// <param name="dtData"></param>
        public static void ResponseExcel(System.Data.DataTable dtData,string FileName)
        {
            string fileName = FileName;
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}.xls", HttpContext.Current.Server.UrlEncode(fileName)));
            HttpContext.Current.Response.Clear();

            MemoryStream file = new MemoryStream();
            GetWorkBook(dtData, FileName).Write(file);

            HttpContext.Current.Response.BinaryWrite(file.GetBuffer());
            HttpContext.Current.Response.End();
        }

        #endregion
    }
}
