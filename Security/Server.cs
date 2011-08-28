using System;
using System.Collections;
using System.Collections.Generic;
using System.Management;
using System.Linq;
using System.Text;

namespace Voodoo.Security
{
    /// <summary>
    /// 服务器相关信息
    /// </summary>
    public static class Server
    {
        #region IpList 服务器IP地址

        /// <summary>
        /// IpList 服务器IP地址
        /// </summary>
        public static ArrayList IpList
        {
            get
            {
                ArrayList iplist = new ArrayList();
                for (int i = 0; i < System.Net.Dns.GetHostAddresses(System.Net.Dns.GetHostName()).Length; i++)
                {
                    iplist.Add(System.Net.Dns.GetHostAddresses(System.Net.Dns.GetHostName())[i].ToString());
                }

                return iplist;
            }
        }
        #endregion IpList 服务器IP地址

        #region BaseBoardInfo 主板信息
        /// <summary>
        /// BaseBoardInfo 主板信息
        /// </summary>
        public static string BaseBoardInfo
        {
            get
            {
                try
                {
                    ManagementObjectSearcher searcher = new ManagementObjectSearcher("Select * From Win32_BaseBoard");
                    string baseboardinfostr = "";
                    foreach (ManagementObject mo in searcher.Get())
                    {
                        baseboardinfostr += "制造商:" + mo["Manufacturer"].ToString();
                        baseboardinfostr += "型号:" + mo["Product"].ToString();
                        baseboardinfostr += "序列号:" + mo["SerialNumber"].ToString();
                    }
                    return baseboardinfostr;
                }
                catch
                {
                    return "";
                }

            }
        }
        #endregion BaseBoardInfo 主板信息

        #region BiosInfo BIOS信息
        /// <summary>
        /// BiosInfo BIOS信息
        /// </summary>
        public static string BiosInfo
        {
            get
            {
                try
                {
                    ManagementObjectSearcher searcher = new ManagementObjectSearcher("Select * From Win32_BIOS");
                    string biosinfostr = "";
                    foreach (ManagementObject mo in searcher.Get())
                    {
                        biosinfostr += "制造商:" + mo["Manufacturer"].ToString();
                        biosinfostr += "序列号:" + mo["SerialNumber"].ToString();
                    }
                    return biosinfostr;
                }
                catch
                {
                    return "";
                }
            }

        }
        #endregion BiosInfo BIOS信息

        #region PhysicalDiskInfo 获取物理盘信息
        /// <summary>
        /// PhysicalDiskInfo 获取物理盘信息
        /// </summary>
        public static string PhysicalDiskInfo
        {
            get
            {
                try
                {
                    ManagementObjectSearcher searcher = new ManagementObjectSearcher("Select * From Win32_DiskDrive");
                    string physicaldiskinfostr = "";
                    foreach (ManagementObject mo in searcher.Get())
                    {
                        physicaldiskinfostr += "类型:" + mo["MediaType"].ToString();
                        physicaldiskinfostr += "容量:" + mo["SizeBytes "].ToString();
                    }

                    return physicaldiskinfostr;
                }
                catch
                {
                    return "";
                }
            }
        }
        #endregion PhysicalDiskInfo 获取物理盘信息

        #region NAConfigurationInfo 网卡配置信息
        /// <summary>
        /// NAConfigurationInfo 网卡配置信息
        /// </summary>
        public static string NAConfigurationInfo
        {
            get
            {
                try
                {
                    ManagementObjectSearcher searcher = new ManagementObjectSearcher("Select * From Win32_NetworkAdapterConfiguration");
                    string naconfigurationinfostr = "";
                    foreach (ManagementObject mo in searcher.Get())
                    {
                        naconfigurationinfostr += "IP地址:" + mo["IPAddress"].ToString();
                        naconfigurationinfostr += "默认网关:" + mo["DefaultIPGateway"].ToString();
                        naconfigurationinfostr += "DNS地址:" + mo["DNSServerSearchOrder"].ToString();
                        naconfigurationinfostr += "子网掩码:" + mo["IPSubnet"].ToString();
                        naconfigurationinfostr += "MAC地址:" + mo["MACAddress"].ToString();
                    }

                    return naconfigurationinfostr;
                }
                catch
                {
                    return "";
                }
            }
        }
        #endregion NAConfigurationInfo 网卡配置信息

        #region ProcessorInfo CPU信息
        /// <summary>
        /// ProcessorInfo CPU信息
        /// </summary>
        public static string ProcessorInfo
        {
            get
            {
                try
                {
                    ManagementObjectSearcher searcher = new ManagementObjectSearcher("Select * From Win32_Processor");
                    string processorinfostr = "";
                    foreach (ManagementObject mo in searcher.Get())
                    {
                        processorinfostr += "二级缓存:" + mo["L2CacheSize"].ToString();
                        processorinfostr += "制造商:" + mo["Manufacturer"].ToString();
                        processorinfostr += "主频:" + mo["MaxClockSpeed"].ToString();
                        processorinfostr += "ID:" + mo["ProcessorId"].ToString();
                    }

                    return processorinfostr;
                }
                catch
                {
                    return "";
                }

            }
        }
        #endregion ProcessorInfo CPU信息

        #region LogicalDiskInfo 逻辑磁盘信息
        /// <summary>
        /// LogicalDiskInfo 逻辑磁盘信息
        /// </summary>
        public static string LogicalDiskInfo
        {
            get
            {
                try
                {
                    ManagementObjectSearcher searcher = new ManagementObjectSearcher("Select * From Win32_LogicalDisk");
                    string logicaldiskinfo = "";
                    foreach (ManagementObject mo in searcher.Get())
                    {
                        logicaldiskinfo += "文件系统:" + mo["FileSystem"].ToString();
                        logicaldiskinfo += "剩余空间:" + mo["FreeSpace"].ToString();
                        logicaldiskinfo += "卷标:" + mo["Name"].ToString();
                        logicaldiskinfo += "大小:" + mo["Size"].ToString();
                    }
                    return logicaldiskinfo;
                }
                catch
                {
                    return "";
                }

            }
        }
        #endregion LogicalDiskInfo 逻辑磁盘信息

        #region 服务器名称
        public static string GetMachineName()
        {
            return System.Web.HttpContext.Current.Server.MachineName;
        }
        #endregion

        #region 服务器域名
        public static string GetYuMing()
        {
            return System.Web.HttpContext.Current.Request.ServerVariables["SERVER_NAME"];// 服务器域名
        }
        

        public static string GetHost()
        {
            return System.Web.HttpContext.Current.Request.Url.Host.ToString();
        }
        #endregion

        #region 服务软件
        public static string GetServerSoftware()
        {
            return System.Web.HttpContext.Current.Request.ServerVariables["SERVER_SOFTWARE"].ToString();
        }
        #endregion

        #region .NET版本
        public static string GetDotNetVersion()
        {
            return ".NET CLR " + Environment.Version.ToString();
        }
        #endregion

        #region 脚本超时时间
        public static string ServerTimeOut()
        {
            return System.Web.HttpContext.Current.Server.ScriptTimeout.ToString() + "秒"; // 脚本超时时间
        }
        #endregion

        #region 程序运行时长
        public static string ServerRunTime()
        {
            return ((Double)System.Environment.TickCount / 3600000).ToString("N2") + "小时";// 开机运行时长
        }
        #endregion

        #region 缓存总数
        public static string CacheCount()
        {
            return System.Web.HttpContext.Current.Cache.Count.ToString();
        }
        #endregion
    }
}
