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
                columns[i] = sourceTable.Columns[i].ColumnName;
            }
            return columns;
        }

        /// <summary>
        /// 创建新DataTable
        /// </summary>
        /// <param name="sourceTable"></param>
        /// <param name="colomns"></param>
        /// <returns></returns>
        public static DataTable NewTable(this DataTable sourceTable, params string[] colomns)
        {
            DataTable newTable = new DataTable();

            var oldColumns = sourceTable.GetColumnNames();
            foreach (string name in colomns)
            {
                newTable.Columns.Add(name);
            }
            foreach (DataRow row in sourceTable.Rows)
            {
                DataRow r = newTable.NewRow();

                foreach (string name in colomns)
                {
                    if (oldColumns.Contains(name))
                    {
                        r[name] = row[name];
                    }
                    else
                    {
                        r[name] = null;
                    }

                }
                newTable.Rows.Add(r);
            }

            ////交集
            //var leftColumns = (from l in colomns where oldColumns.Contains(l) select l).ToList();

            //var nonColumns = (from l in colomns where leftColumns.Contains(l) == false select l).ToList();

            //foreach(string str in leftColumns)
            //{
            //    newTable.Columns.Add(str);
            //}

            //foreach (DataRow row in sourceTable.Rows)
            //{
            //    DataRow newRow = newTable.NewRow();
            //    foreach (string str in leftColumns)
            //    {
            //        newRow[str] = row[str];
            //    }
            //    newTable.Rows.Add(newRow);
            //}

            //foreach (string str in nonColumns)
            //{
            //    newTable.Columns.Add(str);
            //}

            return newTable;
        }
    }
}
