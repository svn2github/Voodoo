using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace Voodoo
{
    public class NetExt
    {
        #region 发送电子邮件
        /// <summary>
        /// 发送电子邮件
        /// </summary>
        /// <param name="from">发件邮箱地址</param>
        /// <param name="password">邮箱密码</param>
        /// <param name="to">收件人地址</param>
        /// <param name="subject">邮件标题</param>
        /// <param name="body">邮件正文</param>
        /// <param name="smtpHost">SMTP服务器</param>
        /// <returns>发送邮件是否成功</returns>
        public static void SentMail(string from, string loginName, string password, string to, string subject, string body, string smtpHost, string toUser)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress(from, "浩奇广告联盟", Encoding.GetEncoding("UTF-8"));

            message.Subject = subject;
            message.IsBodyHtml = true;
            message.BodyEncoding = System.Text.Encoding.UTF8;

            message.To.Add(new MailAddress(to));


            #region 读取模版
            StringBuilder sb = new StringBuilder();
            string t = Voodoo.IO.File.Read("~/Config/mail.htm", Voodoo.IO.File.EnCode.UTF8);
            if (string.IsNullOrEmpty(t))
            {
                t = "";
            }
            sb.Append(t);
            sb = sb.Replace("{Title}", subject);
            sb = sb.Replace("{Content}", body);
            sb = sb.Replace("{ToUser}", toUser);
            sb = sb.Replace("{Time}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            sb = sb.Replace("{ToMail}", to);
            message.Body = sb.ToString();
            #endregion

            //message.Body = body;

            SmtpClient client = new SmtpClient();
            client.Host = smtpHost;
            client.Credentials = new System.Net.NetworkCredential(loginName, password);
            client.Send(message);


        }
        #endregion

    }
}
