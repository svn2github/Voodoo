using System.Text;
using System.Runtime.InteropServices;

namespace Voodoo.Config
{
    /// <summary>
    /// ini文件操作类，调用windows API
    /// <example>
    /// 
    /// 获取配置：
    /// StringBuilder stringBud = new StringBuilder(50);
    /// OperateIniFile.GetPrivateProfileString("DBServer", "ServerName", "", stringBud, 50, AppDomain.CurrentDomain.BaseDirectory + "\\Config.ini");
    /// 
    /// 保存设置：
    /// OperateIniFile.WritePrivateProfileString("DBServer", "ServerName", cmbServer.Text, AppDomain.CurrentDomain.BaseDirectory + "\\Config.ini");
    /// </example>
    /// </summary>
    public class IniFileHelper
    {
        [DllImport("kernel32")]//返回0表示失败，非0为成功
        public static extern long WritePrivateProfileString(string section, string key,
            string val, string filePath);

        [DllImport("kernel32")]//返回取得字符串缓冲区的长度
        public static extern long GetPrivateProfileString(string section, string key,
            string def, StringBuilder retVal, int size, string filePath);
    }
}
