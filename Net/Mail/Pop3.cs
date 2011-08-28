using System;
using System.Net.Sockets;
using System.Net;
using System.Security.Cryptography;
using System.IO;

namespace Voodoo.Net.Mail
{
    public class Pop3
    {
        private string mstrHost = null;  //主机名称或IP地址
        private int mintPort = 110;  //主机的端口号（默认为110）
        private TcpClient mtcpClient = null;  //客户端
        private NetworkStream mnetStream = null;  //网络基础数据流
        private StreamReader m_stmReader = null;  //读取字节流
        private string mstrStatMessage = null;  //执行STAT命令后得到的消息（从中得到邮件数）

        #region 初始化类
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <remarks>一个邮件接收对象</remarks>
        public Pop3()
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="host">主机名称或IP地址</param>
        public Pop3(string host)
        {
            mstrHost = host;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="host">主机名称或IP地址</param>
        /// <param name="port">主机的端口号</param>
        /// <remarks>一个邮件接收对象</remarks>
        public Pop3(string host, int port)
        {
            mstrHost = host;
            mintPort = port;
        }
        #endregion

        #region 属性
        /// <summary>
        /// 主机名称或IP地址
        /// </summary>
        /// <remarks>主机名称或IP地址</remarks>
        public string HostName
        {
            get { return mstrHost; }
            set { mstrHost = value; }
        }
        /// <summary>
        /// 主机的端口号
        /// </summary>
        /// <remarks>主机的端口号</remarks>
        public int Port
        {
            get { return mintPort; }
            set { mintPort = value; }
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 向网络访问的基础数据流中写数据（发送命令码）
        /// </summary>
        /// <param name="netStream">可以用于网络访问的基础数据流</param>
        /// <param name="command">命令行</param>
        /// <remarks>向网络访问的基础数据流中写数据（发送命令码）</remarks>
        private void WriteToNetStream(ref NetworkStream netStream, String command)
        {
            string strToSend = command + "rn";
            byte[] arrayToSend = System.Text.Encoding.ASCII.GetBytes(strToSend.ToCharArray());
            netStream.Write(arrayToSend, 0, arrayToSend.Length);
        }
        /// <summary>
        /// 检查命令行结果是否正确
        /// </summary>
        /// <param name="message">命令行的执行结果</param>
        /// <param name="check">正确标志</param>
        /// <returns>
        /// 类型：布尔
        /// 内容：true表示没有错误，false为有错误
        /// </returns>
        /// <remarks>检查命令行结果是否有错误</remarks>
        private bool CheckCorrect(string message, string check)
        {
            if (message.IndexOf(check) == -1)
                return false;
            else
                return true;
        }
        /// <summary>
        /// 邮箱中的未读邮件数
        /// </summary>
        /// <param name="message">执行完LIST命令后的结果</param>
        /// <returns>
        /// 类型：整型
        /// 内容：邮箱中的未读邮件数
        /// </returns>
        /// <remarks>邮箱中的未读邮件数</remarks>
        private int GetMailNumber(string message)
        {
            string[] strMessage = message.Split(' ');
            return Int32.Parse(strMessage[1]);
        }
        /// <summary>
        /// 得到经过解码后的邮件的内容
        /// </summary>
        /// <param name="encodingContent">解码前的邮件的内容</param>
        /// <returns>
        /// 类型：字符串
        /// 内容：解码后的邮件的内容
        /// </returns>
        /// <remarks>得到解码后的邮件的内容</remarks>
        private string GetDecodeMailContent(string encodingContent)
        {
            string strContent = encodingContent.Trim();
            string strEncode = null;
            int iStart = strContent.IndexOf("Base64");
            if (iStart == -1)
                throw new Exception("邮件内容不是Base64编码，请检查");
            else
            {
                strEncode = strContent.Substring(iStart + 6, strContent.Length - iStart - 6);
                try
                {
                    return strEncode.ToUnBase64();
                }
                catch (Exception exc)
                {
                    throw new Exception(exc.Message);
                }
            }
        }
        #endregion

        #region 与主机建立连接
        /// <summary>
        /// 与主机建立连接
        /// </summary>
        /// <returns>
        /// 类型：布尔
        /// 内容：连接结果（true为连接成功，false为连接失败）
        /// </returns>
        /// <remarks>与主机建立连接</remarks>
        public bool Connect()
        {
            if (mstrHost == null)
                throw new Exception("请提供SMTP主机名称或IP地址！");
            if (mintPort == 0)
                throw new Exception("请提供SMTP主机的端口号");
            try
            {
                mtcpClient = new TcpClient(mstrHost, mintPort);
                mnetStream = mtcpClient.GetStream();
                m_stmReader = new StreamReader(mtcpClient.GetStream());
                string strMessage = m_stmReader.ReadLine();
                if (CheckCorrect(strMessage, "+OK") == true)
                    return true;
                else
                    return false;
            }
            catch (SocketException exc)
            {
                throw new Exception(exc.ToString());
            }
            catch (NullReferenceException exc)
            {
                throw new Exception(exc.ToString());
            }
        }
        #endregion

        #region Pop3命令
        /// <summary>
        /// 执行Pop3命令，并检查执行的结果
        /// </summary>
        /// <param name="command">Pop3命令行</param>
        /// <returns>
        /// 类型：字符串
        /// 内容：Pop3命令的执行结果
        /// </returns>
        private string ExecuteCommand(string command)
        {
            string strMessage = null;  //执行Pop3命令后返回的消息
            try
            {
                //发送命令
                WriteToNetStream(ref mnetStream, command);
                //读取多行
                if (command.Substring(0, 4).Equals("LIST") || command.Substring(0, 4).Equals("RETR") || command.Substring(0, 4).Equals("UIDL")) //记录STAT后的消息（其中包含邮件数）
                {
                    strMessage = ReadMultiLine();
                    if (command.Equals("LIST")) //记录LIST后的消息（其中包含邮件数）
                        mstrStatMessage = strMessage;
                }
                //读取单行
                else
                    strMessage = m_stmReader.ReadLine();
                //判断执行结果是否正确
                if (CheckCorrect(strMessage, "+OK"))
                    return strMessage;
                else
                    return "Error";
            }
            catch (IOException exc)
            {
                throw new Exception(exc.ToString());
            }
        }
        /// <summary>
        /// 在Pop3命令中，LIST、RETR和UIDL命令的结果要返回多行，以点号（.）结尾，
        /// 所以如果想得到正确的结果，必须读取多行
        /// </summary>
        /// <returns>
        /// 类型：字符串
        /// 内容：执行Pop3命令后的结果
        /// </returns>
        private string ReadMultiLine()
        {
            string strMessage = m_stmReader.ReadLine();
            string strTemp = null;
            while (strMessage != ".")
            {
                strTemp = strTemp + strMessage;
                strMessage = m_stmReader.ReadLine();
            }
            return strTemp;
        }
        //USER命令
        private string USER(string user)
        {
            return ExecuteCommand("USER " + user) + "rn";
        }
        //PASS命令
        private string PASS(string password)
        {
            return ExecuteCommand("PASS " + password) + "rn";
        }
        //LIST命令
        private string LIST()
        {
            return ExecuteCommand("LIST") + "rn";
        }
        //UIDL命令
        private string UIDL()
        {
            return ExecuteCommand("UIDL") + "rn";
        }
        //NOOP命令
        private string NOOP()
        {
            return ExecuteCommand("NOOP") + "rn";
        }
        //STAT命令
        private string STAT()
        {
            return ExecuteCommand("STAT") + "rn";
        }
        //RETR命令
        private string RETR(int number)
        {
            return ExecuteCommand("RETR " + number.ToString()) + "rn";
        }
        //DELE命令
        private string DELE(int number)
        {
            return ExecuteCommand("DELE " + number.ToString()) + "rn";
        }
        //QUIT命令
        private void Quit()
        {
            WriteToNetStream(ref mnetStream, "QUIT");
        }
        /// <summary>
        /// 收取邮件
        /// </summary>
        /// <param name="user">用户名</param>
        /// <param name="password">口令</param>
        /// <returns>
        /// 类型：字符串数组
        /// 内容：解码前的邮件内容
        /// </returns>
        private string[] ReceiveMail(string user, string password)
        {
            int iMailNumber = 0;  //邮件数
            if (USER(user).Equals("Error"))
                throw new Exception("用户名不正确！");
            if (PASS(password).Equals("Error"))
                throw new Exception("用户口令不正确！");
            if (STAT().Equals("Error"))
                throw new Exception("准备接收邮件时发生错误！");
            if (LIST().Equals("Error"))
                throw new Exception("得到邮件列表时发生错误！");
            try
            {
                iMailNumber = GetMailNumber(mstrStatMessage);
                //没有新邮件
                if (iMailNumber == 0)
                    return null;
                else
                {
                    string[] strMailContent = new string[iMailNumber];
                    for (int i = 1; i <= iMailNumber; i++)
                    {
                        //读取邮件内容
                        strMailContent[i - 1] = GetDecodeMailContent(RETR(i));
                    }
                    return strMailContent;
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.ToString());
            }
        }
        #endregion

        #region 收取邮件
        /// <summary>
        /// 收取邮件    
        /// </summary>
        /// <param name="user">用户名</param>
        /// <param name="password">口令</param>
        /// <returns>
        /// 类型：字符串数组
        /// 内容：解码前的邮件内容
        /// </returns>
        ///<remarks>收取邮箱中的未读邮件</remarks>
        public string[] Receive(string user, string password)
        {
            try
            {
                return ReceiveMail(user, password);
            }
            catch (Exception exc)
            {
                throw new Exception(exc.ToString());
            }
        }
        #endregion

        #region 断开连接
        /// <summary>
        /// 断开所有与服务器的会话
        /// </summary>
        /// <remarks>断开所有与服务器的会话</remarks>
        public void DisConnect()
        {
            try
            {
                Quit();
                if (m_stmReader != null)
                    m_stmReader.Close();
                if (mnetStream != null)
                    mnetStream.Close();
                if (mtcpClient != null)
                    mtcpClient.Close();
            }
            catch (SocketException exc)
            {
                throw new Exception(exc.ToString());
            }
        }
        #endregion

        #region 删除邮件
        /// <summary>
        /// 删除邮件
        /// </summary>
        /// <param name="number">邮件号</param>
        public void DeleteMail(int number)
        {
            //删除邮件
            int iMailNumber = number + 1;
            if (DELE(iMailNumber).Equals("Error"))
                throw new Exception("删除第" + iMailNumber.ToString() + "时出现错误！");
        }
        #endregion
    }
}
