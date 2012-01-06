using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using System.Data.OleDb;
using System.Runtime.CompilerServices;

namespace Voodoo.Data.DbHelper
{
    public class OleDbHelper : Voodoo.Data.IDbHelper
    {

         #region 预定义对象
        /// <summary>
        /// 数据库连接
        /// </summary>
        public OleDbConnection Conn;

        /// <summary>
        /// 命令对象
        /// </summary>
        public OleDbCommand Cmd;
        #endregion

        #region 实例化
        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="connStr"></param>
        public OleDbHelper(string connStr)
        {
            this.Conn = new OleDbConnection(connStr);
            this.Cmd = new OleDbCommand();
        }

        /// <summary>
        /// 实例化 带超时时间
        /// </summary>
        /// <param name="connStr"></param>
        public OleDbHelper(string connStr, int Times)
        {
            this.Conn = new OleDbConnection(connStr);
            this.Cmd.CommandTimeout = Times;
            this.Cmd = new OleDbCommand();
        }
        #endregion

        #region 枚举:DataReader及其相关联的连接是否在DataReader完成数据读取时自动关闭
        /// <summary>
        /// DataReader及其相关联的连接是否在DataReader完成数据读取时自动关闭
        /// </summary>
        public enum ConnClose
        {
            No,
            Yes
        }
        #endregion

        #region 开启数据源的连接
        /// <summary>
        /// 开启数据源的连接
        /// </summary>
        private void Open()
        {
            if (this.Conn.State == ConnectionState.Closed)
            {
                this.Conn.Open();
            }
        }
        #endregion

        #region 关闭数据源的连接
        /// <summary>
        /// 关闭数据源的连接
        /// </summary>
        private void Close()
        {
            if (this.Conn.State == ConnectionState.Open)
            {
                this.Conn.Dispose();
            }
        }
        #endregion

        #region 释放使用的所有资源
        /// <summary>
        /// 释放使用的所有资源
        /// </summary>
        public void Dispose()
        {
            if ((this.Conn != null) && (this.Conn.State == ConnectionState.Open))
            {
                this.Conn.Close();
            }
            this.Conn.Dispose();
            this.Conn = null;
            this.Cmd.Parameters.Clear();
            this.Cmd.CommandTimeout = 0;
            this.Cmd.Dispose();
        }
        #endregion

        #region 添加SqlCommand 的参数
        /// <summary>
        /// 添加SqlCommand 的参数
        /// </summary>
        /// <param name="CmdParameters">SqlCommand 的参数</param>
        public void Add(DbParameter[] CmdParameters)
        {
            foreach (OleDbParameter parameter in CmdParameters)
            {
                if ((parameter.Direction == ParameterDirection.InputOutput) & Convert.IsDBNull(RuntimeHelpers.GetObjectValue(parameter.Value)))
                {
                    parameter.Value = DBNull.Value;
                }
                this.Cmd.Parameters.Add(parameter);
            }
        }

        public void Add(string ParamName, object Value)
        {
            Cmd.Parameters.Add(new OleDbParameter(ParamName, Value));
        }

        /// <summary> 
        /// 传入存储过程参数和值,设置传入值类型 
        /// </summary> 
        /// <param name="ParamName"></param> 
        /// <param name="OleDbType"></param> 
        /// <param name="Value"></param> 
        /// <remarks></remarks> 
        public void Add(string ParamName,OleDbType OleDbType, object Value)
        {
            Cmd.Parameters.Add(new OleDbParameter(ParamName, OleDbType)).Value = Value == null ? "" : Value;
        }
        /// <summary> 
        /// 传入存储过程参数和值,设置传入值类型,值可为空 
        /// </summary> 
        /// <param name="ParamName"></param> 
        /// <param name="OleDbType"></param> 
        /// <param name="Value"></param> 
        /// <param name="NoNull"></param> 
        /// <remarks></remarks> 
        public void Add(string ParamName, OleDbType OleDbType, object Value, bool NoNull)
        {
            Cmd.Parameters.Add(new OleDbParameter(ParamName, OleDbType)).Value = Value;
        }
        /// <summary> 
        /// 传入存储过程参数和值,设置传入值类型,限定值的长度 
        /// </summary> 
        /// <param name="ParamName"></param> 
        /// <param name="OleDbType"></param> 
        /// <param name="Value"></param> 
        /// <param name="Size"></param> 
        /// <remarks></remarks> 
        public void Add(string ParamName, OleDbType OleDbType, object Value, int Size)
        {
            Cmd.Parameters.Add(new OleDbParameter(ParamName, OleDbType, Size)).Value = Value == null ? "" : Value;
        }
        /// <summary> 
        /// 传入存储过程参数和值,设置传入值类型,限定值的长度,可为空 
        /// </summary> 
        /// <param name="ParamName"></param> 
        /// <param name="OleDbType"></param> 
        /// <param name="Value"></param> 
        /// <param name="Size"></param> 
        /// <param name="NoNull"></param> 
        /// <remarks></remarks> 
        public void Add(string ParamName, OleDbType OleDbType, object Value, int Size, bool NoNull)
        {
            Cmd.Parameters.Add(new OleDbParameter(ParamName, OleDbType, Size)).Value = Value;
        }
        /// <summary> 
        /// 传入存储过程参数和值,专用于Decimal类型传入 
        /// </summary> 
        /// <param name="ParamName"></param> 
        /// <param name="OleDbType"></param> 
        /// <param name="Precision">长度</param> 
        /// <param name="Scale">精度</param> 
        /// <remarks></remarks> 
        public void Add(string ParamName, OleDbType OleDbType, object Value, int Precision, int Scale)
        {
            OleDbParameter aa = new OleDbParameter(ParamName, OleDbType);
            aa.Precision = Convert.ToByte(Precision);
            aa.Scale = Convert.ToByte(Scale);
            aa.Value = Value;
            Cmd.Parameters.Add(aa);
        }

        #endregion

        #region SqlCommand 的此实例使用的SqlConnection开启
        /// <summary>
        /// SqlCommand 的此实例使用的SqlConnection开启
        /// </summary>
        /// <param name="CmdType">指定如何解释命令字符串</param>
        /// <param name="CmdText">Transact-SQL 语句</param>
        /// <param name="CmdParameters">SqlCommand 的参数</param>
        /// <param name="Transaction">Transact-SQL 事务</param>
        private void CreateCmd(CommandType CmdType, string CmdText, DbParameter[] CmdParameters, OleDbTransaction Transaction)
        {
            this.Cmd.Connection = this.Conn;
            this.Cmd.CommandText = CmdText;
            //this.Cmd.CommandTimeout = 20;
            if (Transaction != null)
            {
                this.Cmd.Transaction = Transaction;
            }
            this.Cmd.CommandType = CmdType;
            if (CmdParameters != null)
            {
                this.Add(CmdParameters);
            }
            this.Cmd.Connection.Open();
        }
        #endregion

        #region ExecuteDataSet
        #region 对连接执行 Transact-SQL 语句并返回DataSet/参数：string CmdText
        /// <summary>
        /// 对连接执行 Transact-SQL 语句并返回DataSet
        /// </summary>
        /// <param name="CmdText">Transact-SQL 语句</param>
        /// <returns>DataSet</returns>
        public DataSet ExecuteDataSet(string CmdText)
        {
            return this.ExecuteDataSet(CommandType.StoredProcedure, CmdText, null, null);
        }
        #endregion

        #region 对连接执行 Transact-SQL 语句并返回DataSet/参数：CommandType CmdType, string CmdText
        /// <summary>
        /// 对连接执行 Transact-SQL 语句并返回DataSet
        /// </summary>
        /// <param name="CmdType">指定如何解释命令字符串</param>
        /// <param name="CmdText">Transact-SQL 语句</param>
        /// <returns>DataSet</returns>
        public DataSet ExecuteDataSet(CommandType CmdType, string CmdText)
        {
            return this.ExecuteDataSet(CmdType, CmdText, null, null);
        }
        #endregion

        #region 对连接执行 Transact-SQL 语句并返回DataSet/参数：CommandType CmdType, string CmdText, SqlTransaction Transaction
        /// <summary>
        /// 对连接执行 Transact-SQL 语句并返回DataSet
        /// </summary>
        /// <param name="CmdType">指定如何解释命令字符串</param>
        /// <param name="CmdText">Transact-SQL 语句</param>
        /// <param name="Transaction">Transact-SQL 事务</param>
        /// <returns>DataSet</returns>
        public DataSet ExecuteDataSet(CommandType CmdType, string CmdText, DbTransaction Transaction)
        {
            return this.ExecuteDataSet(CmdType, CmdText, null, (OleDbTransaction)Transaction);
        }
        #endregion

        #region 对连接执行 Transact-SQL 语句并返回DataSet/参数：CommandType CmdType, string CmdText, SqlParameter[] CmdParameters
        /// <summary>
        /// 对连接执行 Transact-SQL 语句并返回DataSet
        /// </summary>
        /// <param name="CmdType">指定如何解释命令字符串</param>
        /// <param name="CmdText">Transact-SQL 语句</param>
        /// <param name="CmdParameters">SqlCommand 的参数</param>
        /// <returns>DataSet</returns>
        public DataSet ExecuteDataSet(CommandType CmdType, string CmdText, DbParameter[] CmdParameters)
        {
            return this.ExecuteDataSet(CmdType, CmdText, CmdParameters, null);
        }
        #endregion

        #region 对连接执行 Transact-SQL 语句并返回DataSet/参数：CommandType CmdType, string CmdText, SqlParameter[] CmdParameters, SqlTransaction Transaction
        /// <summary>
        /// 对连接执行 Transact-SQL 语句并返回DataSet
        /// </summary>
        /// <param name="CmdType">指定如何解释命令字符串</param>
        /// <param name="CmdText">Transact-SQL 语句</param>
        /// <param name="CmdParameters">SqlCommand 的参数</param>
        /// <param name="Transaction">Transact-SQL 事务</param>
        /// <returns>DataSet</returns>
        public DataSet ExecuteDataSet(CommandType CmdType, string CmdText, DbParameter[] CmdParameters, DbTransaction Transaction)
        {
            this.CreateCmd(CmdType, CmdText.SqlStrTopToLimit(), CmdParameters, (OleDbTransaction)Transaction);
            OleDbDataAdapter adapter = new OleDbDataAdapter(this.Cmd);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);
            this.Dispose();
            return dataSet;
        }
        #endregion
        #endregion

        #region ExecuteDataTable

        #region 对连接执行 Transact-SQL 语句并返回DataTable/参数：string CmdText
        /// <summary>
        /// 对连接执行 Transact-SQL 语句并返回DataTable
        /// </summary>
        /// <param name="CmdText">Transact-SQL 语句</param>
        /// <returns>DataTable</returns>
        public DataTable ExecuteDataTable(string CmdText)
        {
            return this.ExecuteDataTable((CommandType)0, CmdText, null, null);
        }
        #endregion

        #region 对连接执行 Transact-SQL 语句并返回DataTable/参数：CommandType CmdType, string CmdText
        /// <summary>
        /// 对连接执行 Transact-SQL 语句并返回DataTable
        /// </summary>
        /// <param name="CmdType">指定如何解释命令字符串</param>
        /// <param name="CmdText">Transact-SQL 语句</param>
        /// <returns>DataTable</returns>
        public DataTable ExecuteDataTable(CommandType CmdType, string CmdText)
        {
            return this.ExecuteDataTable(CmdType, CmdText, null, null);
        }
        #endregion

        #region 对连接执行 Transact-SQL 语句并返回DataTable/参数：CommandType CmdType, string CmdText, SqlTransaction Transaction
        /// <summary>
        /// 对连接执行 Transact-SQL 语句并返回DataTable
        /// </summary>
        /// <param name="CmdType">指定如何解释命令字符串</param>
        /// <param name="CmdText">Transact-SQL 语句</param>
        /// <param name="Transaction">Transact-SQL 事务</param>
        /// <returns>DataTable</returns>
        public DataTable ExecuteDataTable(CommandType CmdType, string CmdText, DbTransaction Transaction)
        {
            return this.ExecuteDataTable(CmdType, CmdText, null, (OleDbTransaction)Transaction);
        }
        #endregion

        #region 对连接执行 Transact-SQL 语句并返回DataTable/参数：CommandType CmdType, string CmdText, SqlParameter[] CmdParameters
        /// <summary>
        /// 对连接执行 Transact-SQL 语句并返回DataTable
        /// </summary>
        /// <param name="CmdType">指定如何解释命令字符串</param>
        /// <param name="CmdText">Transact-SQL 语句</param>
        /// <param name="CmdParameters">SqlCommand 的参数</param>
        /// <returns>DataTable</returns>
        public DataTable ExecuteDataTable(CommandType CmdType, string CmdText, DbParameter[] CmdParameters)
        {
            return this.ExecuteDataTable(CmdType, CmdText, CmdParameters, null);
        }
        #endregion

        #region 对连接执行 Transact-SQL 语句并返回DataTable/参数：CommandType CmdType, string CmdText, SqlParameter[] CmdParameters, SqlTransaction Transaction
        /// <summary>
        /// 对连接执行 Transact-SQL 语句并返回DataTable
        /// </summary>
        /// <param name="CmdType">指定如何解释命令字符串</param>
        /// <param name="CmdText">Transact-SQL 语句</param>
        /// <param name="CmdParameters">SqlCommand 的参数</param>
        /// <param name="Transaction">Transact-SQL 事务</param>
        /// <returns>DataTable</returns>
        public DataTable ExecuteDataTable(CommandType CmdType, string CmdText, DbParameter[] CmdParameters, DbTransaction Transaction)
        {
            this.CreateCmd(CmdType, CmdText.SqlStrTopToLimit(), CmdParameters, (OleDbTransaction)Transaction);
            OleDbDataAdapter adapter = new OleDbDataAdapter(this.Cmd);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            this.Dispose();
            return dataTable;
        }
        #endregion
        #endregion

        #region ExecuteNonQuery
        #region 对连接执行 Transact-SQL 语句并返回受影响的行数/参数：string CmdText
        /// <summary>
        /// 对连接执行 Transact-SQL 语句并返回受影响的行数
        /// </summary>
        /// <param name="CmdText">Transact-SQL 语句</param>
        /// <returns>返回受影响的行数</returns>
        public int ExecuteNonQuery(string CmdText)
        {
            return this.ExecuteNonQuery(CommandType.StoredProcedure, CmdText, null, null);
        }
        #endregion

        #region 对连接执行 Transact-SQL 语句并返回受影响的行数/参数：CommandType CmdType, string CmdText
        /// <summary>
        /// 对连接执行 Transact-SQL 语句并返回受影响的行数
        /// </summary>
        /// <param name="CmdType">指定如何解释命令字符串</param>
        /// <param name="CmdText">Transact-SQL 语句</param>
        /// <returns>返回受影响的行数</returns>
        public int ExecuteNonQuery(CommandType CmdType, string CmdText)
        {
            return this.ExecuteNonQuery(CmdType, CmdText, null, null);
        }
        #endregion

        #region 对连接执行 Transact-SQL 语句并返回受影响的行数/参数：CommandType CmdType, string CmdText, SqlParameter[] CmdParameters
        /// <summary>
        /// 对连接执行 Transact-SQL 语句并返回受影响的行数
        /// </summary>
        /// <param name="CmdType">指定如何解释命令字符串</param>
        /// <param name="CmdText">Transact-SQL 语句</param>
        /// <param name="CmdParameters">SqlCommand 的参数</param>
        /// <returns>返回受影响的行数</returns>
        public int ExecuteNonQuery(CommandType CmdType, string CmdText, DbParameter[] CmdParameters)
        {
            return this.ExecuteNonQuery(CmdType, CmdText, CmdParameters, null);
        }
        #endregion

        #region 对连接执行 Transact-SQL 语句并返回受影响的行数/参数：CommandType CmdType, string CmdText, SqlTransaction Transaction
        /// <summary>
        /// 对连接执行 Transact-SQL 语句并返回受影响的行数
        /// </summary>
        /// <param name="CmdType">指定如何解释命令字符串</param>
        /// <param name="CmdText">Transact-SQL 语句</param>
        /// <param name="Transaction">Transact-SQL 事务</param>
        /// <returns>返回受影响的行数</returns>
        public int ExecuteNonQuery(CommandType CmdType, string CmdText, DbTransaction Transaction)
        {
            return this.ExecuteNonQuery(CmdType, CmdText, null, (OleDbTransaction)Transaction);
        }
        #endregion

        #region 对连接执行 Transact-SQL 语句并返回受影响的行数/参数：CommandType CmdType, string CmdText, SqlParameter[] CmdParameters, SqlTransaction Transaction
        /// <summary>
        /// 对连接执行 Transact-SQL 语句并返回受影响的行数
        /// </summary>
        /// <param name="CmdType">指定如何解释命令字符串</param>
        /// <param name="CmdText">Transact-SQL 语句</param>
        /// <param name="CmdParameters">SqlCommand 的参数</param>
        /// <param name="Transaction">Transact-SQL 事务</param>
        /// <returns>返回受影响的行数</returns>
        public int ExecuteNonQuery(CommandType CmdType, string CmdText, DbParameter[] CmdParameters, DbTransaction Transaction)
        {
            this.CreateCmd(CmdType, CmdText, CmdParameters, (OleDbTransaction)Transaction);
            int num2 = 0;
            if (Transaction != null)
            {
                try
                {
                    num2 = this.Cmd.ExecuteNonQuery();
                    Transaction.Commit();
                }
                catch (Exception exception1)
                {
                    Exception exception = exception1;
                    Transaction.Rollback();
                }
            }
            else
            {
                num2 = this.Cmd.ExecuteNonQuery();
            }
            this.Close();
            this.Dispose();
            return num2;
        }
        #endregion
        #endregion

        #region ExecuteReader
        #region 将CommandText发送到Connection，并使用CommandBehavior值之一生成一个SqlDataReader/参数：string CmdText
        /// <summary> 
        /// 将CommandText发送到Connection，并使用CommandBehavior值之一生成一个SqlDataReader
        /// </summary>
        /// <param name="CmdText">Transact-SQL 语句</param>
        /// <returns>SqlDataReader</returns>
        public DbDataReader ExecuteReader(string CmdText)
        {
            return this.ExecuteReader(CommandType.StoredProcedure, CmdText, null, null, false);
        }
        #endregion

        #region 将CommandText发送到Connection，并使用CommandBehavior值之一生成一个SqlDataReader/参数：CommandType CmdType, string CmdText
        /// <summary>
        /// 将CommandText发送到Connection，并使用CommandBehavior值之一生成一个SqlDataReader
        /// </summary>
        /// <param name="CmdType">指定如何解释命令字符串</param>
        /// <param name="CmdText">Transact-SQL 语句</param>
        /// <returns>SqlDataReader</returns>
        public DbDataReader ExecuteReader(CommandType CmdType, string CmdText)
        {
            return this.ExecuteReader(CmdType, CmdText, null, null, false);
        }

        /// <summary>
        /// 将CommandText发送到Connection，并使用CommandBehavior值之一生成一个SqlDataReader
        /// </summary>
        /// <param name="CmdText">Transact-SQL 语句</param>
        /// <param name="ConnOwnership">DataReader完成数据读取时是否关闭</param>
        /// <returns>SqlDataReader</returns>
        public DbDataReader ExecuteReader(string CmdText, bool ConnOwnership)
        {
            return this.ExecuteReader(CommandType.StoredProcedure, CmdText, null, null, ConnOwnership);
        }
        #endregion

        #region 将CommandText发送到Connection，并使用CommandBehavior值之一生成一个SqlDataReader/参数：CommandType CmdType, string CmdText, SqlParameter[] CmdParameters
        /// <summary>
        /// 将CommandText发送到Connection，并使用CommandBehavior值之一生成一个SqlDataReader
        /// </summary>
        /// <param name="CmdType">指定如何解释命令字符串</param>
        /// <param name="CmdText">Transact-SQL 语句</param>
        /// <param name="CmdParameters">SqlCommand 的参数</param>
        /// <returns>SqlDataReader</returns>
        public DbDataReader ExecuteReader(CommandType CmdType, string CmdText, DbParameter[] CmdParameters)
        {
            return this.ExecuteReader(CmdType, CmdText, CmdParameters, null, false);
        }
        #endregion

        #region 将CommandText发送到Connection，并使用CommandBehavior值之一生成一个SqlDataReader/参数CommandType CmdType, string CmdText, SqlTransaction Transaction
        /// <summary>
        /// 将CommandText发送到Connection，并使用CommandBehavior值之一生成一个SqlDataReader
        /// </summary>
        /// <param name="CmdType">指定如何解释命令字符串</param>
        /// <param name="CmdText">Transact-SQL 语句</param>
        /// <param name="Transaction">Transact-SQL 事务</param>
        /// <returns>SqlDataReader</returns>
        public DbDataReader ExecuteReader(CommandType CmdType, string CmdText, DbTransaction Transaction)
        {
            return this.ExecuteReader(CmdType, CmdText, null, null, false);
        }
        #endregion

        #region 将CommandText发送到Connection，并使用CommandBehavior值之一生成一个SqlDataReader/参数：CommandType CmdType, string CmdText, ConnClose ConnOwnership
        /// <summary>
        /// 将CommandText发送到Connection，并使用CommandBehavior值之一生成一个SqlDataReader
        /// </summary>
        /// <param name="CmdType">指定如何解释命令字符串</param>
        /// <param name="CmdText">Transact-SQL 语句</param>
        /// <param name="ConnOwnership">DataReader完成数据读取时是否关闭</param>
        /// <returns>SqlDataReader</returns>
        public DbDataReader ExecuteReader(CommandType CmdType, string CmdText, bool ConnOwnership)
        {
            return this.ExecuteReader(CmdType, CmdText, null, null, ConnOwnership);
        }
        #endregion

        #region 将CommandText发送到Connection，并使用CommandBehavior值之一生成一个SqlDataReader/参数：CommandType CmdType, string CmdText, SqlTransaction Transaction, ConnClose ConnOwnership
        /// <summary>
        /// 将CommandText发送到Connection，并使用CommandBehavior值之一生成一个SqlDataReader
        /// </summary>
        /// <param name="CmdType">指定如何解释命令字符串</param>
        /// <param name="CmdText">Transact-SQL 语句</param>
        /// <param name="Transaction">Transact-SQL 事务</param>
        /// <param name="ConnOwnership">DataReader完成数据读取时是否关闭</param>
        /// <returns>SqlDataReader</returns>
        public DbDataReader ExecuteReader(CommandType CmdType, string CmdText, DbTransaction Transaction, bool ConnOwnership)
        {
            return this.ExecuteReader(CmdType, CmdText, null, (OleDbTransaction)Transaction, ConnOwnership);
        }
        #endregion

        #region 将CommandText发送到Connection，并使用CommandBehavior值之一生成一个SqlDataReader/参数：CommandType CmdType, string CmdText, SqlParameter[] CmdParameters, ConnClose ConnOwnership
        /// <summary>
        /// 将CommandText发送到Connection，并使用CommandBehavior值之一生成一个SqlDataReader
        /// </summary>
        /// <param name="CmdType">指定如何解释命令字符串</param>
        /// <param name="CmdText">Transact-SQL 语句</param>
        /// <param name="CmdParameters">SqlCommand 的参数</param>
        /// <param name="ConnOwnership">DataReader完成数据读取时是否关闭</param>
        /// <returns>SqlDataReader</returns>
        public DbDataReader ExecuteReader(CommandType CmdType, string CmdText, DbParameter[] CmdParameters, bool ConnOwnership)
        {
            return this.ExecuteReader(CmdType, CmdText, CmdParameters, null, ConnOwnership);
        }
        #endregion

        #region 将CommandText发送到Connection，并使用CommandBehavior值之一生成一个SqlDataReader/参数：CommandType CmdType, string CmdText, SqlParameter[] CmdParameters, SqlTransaction Transaction, ConnClose ConnOwnership
        /// <summary>
        /// 将CommandText发送到Connection，并使用CommandBehavior值之一生成一个SqlDataReader
        /// </summary>
        /// <param name="CmdType">指定如何解释命令字符串</param>
        /// <param name="CmdText">Transact-SQL 语句</param>
        /// <param name="CmdParameters">SqlCommand 的参数</param>
        /// <param name="Transaction">Transact-SQL 事务</param>
        /// <param name="ConnOwnership">DataReader完成数据读取时是否关闭</param>
        /// <returns>SqlDataReader</returns>
        public DbDataReader ExecuteReader(CommandType CmdType, string CmdText, DbParameter[] CmdParameters, DbTransaction Transaction, bool ConnOwnership)
        {
            this.CreateCmd(CmdType, CmdText.SqlStrTopToLimit(), CmdParameters, (OleDbTransaction)Transaction);
            if (ConnOwnership == true)
            {
                return this.Cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            return this.Cmd.ExecuteReader();
        }
        #endregion
        #endregion


        #region ExecuteScalar
        #region 执行查询，并返回查询所返回的结果集中第一行的第一列。忽略其他列或行/参数：string CmdText
        /// <summary>
        /// 执行查询，并返回查询所返回的结果集中第一行的第一列。忽略其他列或行。
        /// </summary>
        /// <param name="CmdText">Transact-SQL 语句</param>
        /// <returns>返回的结果集中第一行的第一列</returns>
        public object ExecuteScalar(string CmdText)
        {
            return this.ExecuteScalar(CommandType.StoredProcedure, CmdText, null, null);
        }
        #endregion

        #region 执行查询，并返回查询所返回的结果集中第一行的第一列。忽略其他列或行/参数：CommandType CmdType, string CmdText
        /// <summary>
        /// 执行查询，并返回查询所返回的结果集中第一行的第一列。忽略其他列或行。
        /// </summary>
        /// <param name="CmdType">指定如何解释命令字符串</param>
        /// <param name="CmdText">Transact-SQL 语句</param>
        /// <returns>返回的结果集中第一行的第一列</returns>
        public object ExecuteScalar(CommandType CmdType, string CmdText)
        {
            return this.ExecuteScalar(CmdType, CmdText, null, null);
        }
        #endregion

        #region 执行查询，并返回查询所返回的结果集中第一行的第一列。忽略其他列或行/参数：CommandType CmdType, string CmdText, SqlTransaction Transaction
        /// <summary>
        /// 执行查询，并返回查询所返回的结果集中第一行的第一列。忽略其他列或行。
        /// </summary>
        /// <param name="CmdType">指定如何解释命令字符串</param>
        /// <param name="CmdText">Transact-SQL 语句</param>
        /// <param name="Transaction">Transact-SQL 事务</param>
        /// <returns>返回的结果集中第一行的第一列</returns>
        public object ExecuteScalar(CommandType CmdType, string CmdText, DbTransaction Transaction)
        {
            return this.ExecuteScalar(CmdType, CmdText, null, (OleDbTransaction)Transaction);
        }
        #endregion

        #region 执行查询，并返回查询所返回的结果集中第一行的第一列。忽略其他列或行/参数：CommandType CmdType, string CmdText, SqlParameter[] CmdParameters
        /// <summary>
        /// 执行查询，并返回查询所返回的结果集中第一行的第一列。忽略其他列或行
        /// </summary>
        /// <param name="CmdType">指定如何解释命令字符串</param>
        /// <param name="CmdText">Transact-SQL 语句</param>
        /// <param name="CmdParameters">SqlCommand 的参数</param>
        /// <returns>返回的结果集中第一行的第一列</returns>
        public object ExecuteScalar(CommandType CmdType, string CmdText, DbParameter[] CmdParameters)
        {
            return this.ExecuteScalar(CmdType, CmdText, CmdParameters, null);
        }
        #endregion

        #region 执行查询，并返回查询所返回的结果集中第一行的第一列。忽略其他列或行/参数：CommandType CmdType, string CmdText, SqlParameter[] CmdParameters, SqlTransaction Transaction
        /// <summary>
        /// 执行查询，并返回查询所返回的结果集中第一行的第一列。忽略其他列或行。
        /// </summary>
        /// <param name="CmdType">指定如何解释命令字符串</param>
        /// <param name="CmdText">Transact-SQL 语句</param>
        /// <param name="CmdParameters">SqlCommand 的参数</param>
        /// <param name="Transaction">Transact-SQL 事务</param>
        /// <returns>返回的结果集中第一行的第一列</returns>
        public object ExecuteScalar(CommandType CmdType, string CmdText, DbParameter[] CmdParameters, DbTransaction Transaction)
        {
            this.CreateCmd(CmdType, CmdText.SqlStrTopToLimit(), CmdParameters, (OleDbTransaction)Transaction);
            object objectValue = Cmd.ExecuteScalar();
            this.Close();
            this.Dispose();
            return objectValue;
        }
        #endregion
        #endregion

        #region 返回分页后的DataTable
        /// <summary>
        /// 返回分页后的DataTable
        /// </summary>
        /// <param name="Tables">表（多个表用“,”分割）</param>
        /// <param name="PrimaryKey">表的主键</param>
        /// <param name="Sort">排序表达式</param>
        /// <param name="CurrentPage">当前页码</param>
        /// <param name="PageSize">每页显示记录数</param>
        /// <param name="Fields">需要查询出的字段</param>
        /// <param name="Filter">where语句，不需要带“where”</param>
        /// <param name="group">Group表达式 不需要“group”</param>
        /// <returns></returns>
        public DataTable PageListViewSort(string Tables, string PrimaryKey, string Sort, int CurrentPage, int PageSize, string Fields, string Filter, string group)
        {
            if (CurrentPage <= 0)
            {
                CurrentPage = 1;
            }
            int strStartRow = (CurrentPage - 1) * PageSize ;
            int strEndRow = CurrentPage * PageSize;

            if (!Filter.IsNullOrEmpty())
            {
                Filter = " where " + Filter + " ";
            }
            else
            {
                Filter = "";
            }

            if (!group.IsNullOrEmpty())
            {
                group = " GROUP BY  " + group + " ";
            }
            else
            {
                group = "";
            }

            if (!Sort.IsNullOrEmpty())
            {
                Sort = " ORDER BY  " + Sort;
            }
            else
            {
                Sort = "";
            }

            //string str_sql = "SELECT " + Fields + " FROM " + Tables + Filter + Sort + " LIMIT " + strStartRow + "," + PageSize;
            string str_sql = "SELECT " + Fields + " FROM " + Tables + Filter + Sort ;
            DataTable dt= ExecuteDataTable(CommandType.Text, str_sql);

            dt.Columns.Add(new DataColumn("rownumber", Type.GetType("System.Int32")));
            for (int i = 0; i < dt.Rows.Count;i++ )
            {
                dt.Rows[i]["rownumber"] = PageSize * (CurrentPage-1) + i+1;
            }

            return dt;

        }
        #endregion

        #region 获取符合条件的记录条数
        /// <summary>
        /// 获取符合条件的记录条数
        /// </summary>
        /// <param name="Tables">表名</param>
        /// <param name="Filter">where语句</param>
        /// <param name="group">group语句</param>
        /// <returns></returns>
        public int PageCountSort(string Tables, string Filter, string group)
        {
            if (!Filter.IsNullOrEmpty())
            {
                Filter = " where " + Filter + " ";
            }
            else
            {
                Filter = "";
            }

            if (!group.IsNullOrEmpty())
            {
                group += " GROUP BY " + group + " ";
            }
            else
            {
                group = "";
            }

            return Convert.ToInt32(ExecuteScalar(CommandType.Text, "select count(0) from " + Tables + Filter + group));

        }
        #endregion
    }
}
