//--------------------------------------------------------------------------------------
//  Copyright (c) Voodoo
// 
//  文件名 :IDbHelper.cs
//
//  功能描述：数据库访问接口类，提供多数据库访问所使用的方法
//
//  创建标识：2010-12-4 16:42:18  cuibing  QQ:363212404
//
//
//  修改标识：2010-12-8 15:02:07  cuibing  QQ:363212404
//  修改说明：增加PageListViewSort和PageCountSort两个方法，提供对数据库的分页查询功能
//  
// 
//  修改标识：2010-12-17 13:27:41 cuibing  QQ:363212404
//  修改说明：规范代码结构，增加代码折叠，同时完善代码注释
//
// 
//  修改标识：2011年1月10日 10:45:29 cuibing  QQ:363212404
//  修改说明：PageListViewSort 方法在SqlHelper和MySqlHelper中增加一列:rownumber
//
//---------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;

namespace Voodoo.Data
{
    /// <summary>
    /// 数据库操作接口类，提供程序和数据库交互接口方法，目前支持SqlServer和MySql
    /// </summary>
    public interface IDbHelper
    {
        #region ExecuteDataSet
        DataSet ExecuteDataSet(string CmdText);

        DataSet ExecuteDataSet(CommandType CmdType, string CmdText);

        DataSet ExecuteDataSet(CommandType CmdType, string CmdText, DbTransaction Transaction);

        DataSet ExecuteDataSet(CommandType CmdType, string CmdText, DbParameter[] CmdParameters);

        DataSet ExecuteDataSet(CommandType CmdType, string CmdText, DbParameter[] CmdParameters, DbTransaction Transaction);
        #endregion

        #region ExecuteDataTable
        DataTable ExecuteDataTable(string CmdText);

        DataTable ExecuteDataTable(CommandType CmdType, string CmdText);

        DataTable ExecuteDataTable(CommandType CmdType, string CmdText, DbTransaction Transaction);

        DataTable ExecuteDataTable(CommandType CmdType, string CmdText, DbParameter[] CmdParameters);

        DataTable ExecuteDataTable(CommandType CmdType, string CmdText, DbParameter[] CmdParameters, DbTransaction Transaction);
        #endregion

        #region ExecuteNonQuery
        int ExecuteNonQuery(string CmdText);

        int ExecuteNonQuery(CommandType CmdType, string CmdText);

        int ExecuteNonQuery(CommandType CmdType, string CmdText, DbParameter[] CmdParameters);

        int ExecuteNonQuery(CommandType CmdType, string CmdText, DbTransaction Transaction);

        int ExecuteNonQuery(CommandType CmdType, string CmdText, DbParameter[] CmdParameters, DbTransaction Transaction);
        #endregion

        #region ExecuteReader
        DbDataReader ExecuteReader(string CmdText);

        DbDataReader ExecuteReader(CommandType CmdType, string CmdText);

        DbDataReader ExecuteReader(string CmdText, bool ConnOwnership);

        DbDataReader ExecuteReader(CommandType CmdType, string CmdText, DbParameter[] CmdParameters);

        DbDataReader ExecuteReader(CommandType CmdType, string CmdText, DbTransaction Transaction);

        DbDataReader ExecuteReader(CommandType CmdType, string CmdText, bool ConnOwnership);

        DbDataReader ExecuteReader(CommandType CmdType, string CmdText, DbTransaction Transaction, bool ConnOwnership);

        DbDataReader ExecuteReader(CommandType CmdType, string CmdText, DbParameter[] CmdParameters, bool ConnOwnership);

        DbDataReader ExecuteReader(CommandType CmdType, string CmdText, DbParameter[] CmdParameters, DbTransaction Transaction, bool ConnOwnership);
        #endregion

        #region ExecuteScalar
        object ExecuteScalar(string CmdText);

        object ExecuteScalar(CommandType CmdType, string CmdText);

        object ExecuteScalar(CommandType CmdType, string CmdText, DbTransaction Transaction);

        object ExecuteScalar(CommandType CmdType, string CmdText, DbParameter[] CmdParameters);

        object ExecuteScalar(CommandType CmdType, string CmdText, DbParameter[] CmdParameters, DbTransaction Transaction);
        #endregion

        #region 分页
        DataTable PageListViewSort(string Tables, string PrimaryKey, string Sort, int CurrentPage, int PageSize, string Fields, string Filter, string group);

        int PageCountSort(string Tables, string Filter, string group);
        #endregion

    }
}
