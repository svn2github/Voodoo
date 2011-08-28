using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace Voodoo.Net.Mail
{
    public class SMTP
    {
        #region 发送电子邮件
        /// <summary>
        /// 发送电子邮件
        /// </summary>
        /// <param name="from">发件邮箱地址</param>
        /// <param name="password">邮箱密码</param>
        /// <param name="to">收件人地址</param>
        /// <param name="FromText">发送者文字</param>
        /// <param name="subject">邮件标题</param>
        /// <param name="body">邮件正文</param>
        /// <param name="smtpHost">SMTP服务器</param>
        /// <returns>发送邮件是否成功</returns>
        public static void SentMail(string from, string loginName, string password, string to,string FromText, string subject, string body, string smtpHost, string toUser)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress(from, FromText, Encoding.GetEncoding("UTF-8"));

            message.Subject = subject;
            message.IsBodyHtml = true;
            message.BodyEncoding = System.Text.Encoding.UTF8;

            message.To.Add(new MailAddress(to));



            message.Body = body;

            //message.Body = body;

            SmtpClient client = new SmtpClient();
            client.Host = smtpHost;
            client.Credentials = new System.Net.NetworkCredential(loginName, password);
            client.Send(message);


        }
        #endregion
    }
}
