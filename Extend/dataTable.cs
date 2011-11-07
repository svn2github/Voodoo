using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Voodoo
{
    public static class dataTable
    {
        /// <summary>
        /// 获取所有列名
        /// </summary>
        /// <param name="sourceTable"></param>
        /// <returns></returns>
        public static string[] GetColumnNames(this DataTable sourceTable)
        {
            string[] columns = new string[sourceTable.Columns.Count];

            for (int i = 0; i < sourceTable.Columns.Count; i++)
            {
                columns[i] = sourceTable.Columns[i].ColumnName.Trim();
            }
            return columns;
        }

        public static DataColumn GetLastColumn(this DataTable SourceTable)
        {
            return SourceTable.Columns[SourceTable.Columns.Count - 1];
        }

        /// <summary>
        /// 创建新DataTable
        /// </summary>
        /// <param name="sourceTable"></param>
        /// <param name="colomns"></param>
        /// <returns></returns>
        public static DataTable NewTable(this DataTable sourceTable, params DataColumn[] colomns)
        {
            DataTable newTable = new DataTable();

            var oldColumns = sourceTable.GetColumnNames();
            foreach (DataColumn name in colomns)
            {
                newTable.Columns.Add(name);
            }
            foreach (DataRow row in sourceTable.Rows)
            {
                DataRow r = newTable.NewRow();

                foreach (DataColumn name in colomns)
                {
                    if (oldColumns.Contains(name.ColumnName))
                    {
                        try
                        {
                            r[name.ColumnName] = row[name.ColumnName];
                        }
                        catch
                        {
                            r[name.ColumnName] = GetDefaultValue(name.DataType);
                        }
                    }
                    else
                    {
                        r[name.ColumnName] = GetDefaultValue(name.DataType);
                    }

                }
                newTable.Rows.Add(r);
            }



            return newTable;
        }

        public static object GetDefaultValue(Type type)
        {
            switch (type.Name)
            {
                case "Int32":
                    return  0;
                    break;
                case "String":
                    return  "";
                    break;
                case "DateTime":
                    return  new DateTime(2000, 1, 1);
                    break;
                case "Boolean":
                    return  false;
                    break;
                case "Single":
                    return 0.00;
                    break;
                default:
                    return null;
                    break;

            }
        }
    }
}
