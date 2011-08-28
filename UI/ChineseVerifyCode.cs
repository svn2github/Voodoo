using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Drawing;
using Voodoo;
using System.Web.SessionState;
using System.Text;

namespace Voodoo.UI
{
    public class ChineseVerifyCode : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext Context)
        {
            //获取GB2312编码页（表）
            /**
             * 生成中文验证验码所要使用的方法
             * 注，生成中文验证码时要改变一下生成验证码图片的宽度
             * var imageCode = new System.Drawing.Bitmap((int)Math.Ceiling((code.Length * 22.5)), 23); //定义图片的宽度和高度
             **/

            var gb = Encoding.GetEncoding("gb2312");

            //调用函数产生4个随机中文汉字编码
            object[] bytes = CreateRegionCode(4);

            //根据汉字编码的字节数组解码出中文汉字
            var sbCode = new StringBuilder().Append(gb.GetString((byte[])Convert.ChangeType(bytes[0], typeof(byte[]))))
                //.Append(gb.GetString((byte[])Convert.ChangeType(bytes[1], typeof(byte[]))))
                //.Append(gb.GetString((byte[])Convert.ChangeType(bytes[2], typeof(byte[]))))
                .Append(gb.GetString((byte[])Convert.ChangeType(bytes[3], typeof(byte[]))));
            CreateCheckCodeImage(sbCode.ToString());

            // CreateCheckCodeImage(GenerateCheckCode());      //生成数字英文所要使用的方法
        }


        #region 生成汉字验证码
        /// <summary>
        /// 此函数在汉字编码范围内随机创建含两个元素的十六进制字节数组，每个字节数组代表一个汉字，并将四个字节数组存储在object数组中。 
        /// </summary>
        /// <param name="strLength">代表需要产生的汉字个数</param>
        /// <returns></returns>
        static object[] CreateRegionCode(int strLength)
        {
            var rBase = new[]
            {
                "0","1","2","3","4","5","6","7","8","9","a","b","c","d","e","f"
            };

            var random = new Random();
            var bytes = new object[strLength];

            /*每循环一次产生一个含两个元素的十六进制字节数组，并将其放入bject数组中
             每个汉字有四个区位码组成
             区位码第1位和区位码第2位作为字节数组第一个元素
             区位码第3位和区位码第4位作为字节数组第二个元素
            */

            for (int i = 0; i < strLength; i++)
            {
                //区位码第1位
                var r1 = random.Next(11, 14);
                var str_r1 = rBase[r1].Trim();

                random = new Random(r1 * unchecked((int)DateTime.Now.Ticks) + i);   //更换随机数发生器的种子避免产生重复值 

                var r2 = 0;
                if (r1 == 13)
                    r2 = random.Next(0, 7);
                else
                    r2 = random.Next(0, 16);

                var str_r2 = rBase[r2].Trim();

                //区位码第3位
                random = new Random(r2 * unchecked((int)DateTime.Now.Ticks) + i);
                var r3 = random.Next(10, 16);
                var str_r3 = rBase[r3].Trim();

                //区位码第4位
                random = new Random(r3 * unchecked((int)DateTime.Now.Ticks) + i);
                var r4 = 0;
                if (r3 == 10)
                    r4 = random.Next(1, 16);
                else if (r3 == 15)
                    r4 = random.Next(0, 15);
                else
                    r4 = random.Next(0, 16);

                var str_r4 = rBase[r4].Trim();

                var byte1 = Convert.ToByte(str_r1 + str_r2, 16);
                var byte2 = Convert.ToByte(str_r3 + str_r4, 16);
                //将两个字节变量存储在字节数组中
                var str_r = new[] { byte1, byte2 };

                //将产生的一个汉字的字节数组放入object数组中
                bytes.SetValue(str_r, i);
            }
            return bytes;
        }
        #endregion

        #region 生成图片
        void CreateCheckCodeImage(string code)
        {
            var imageCode = new System.Drawing.Bitmap((int)Math.Ceiling((code.Length * 32.5)), 30); //定义图片的宽度和高度
            var g = Graphics.FromImage(imageCode);  //加载图片到画布上

            try
            {
                var random = new Random();
                g.Clear(Color.White); //清空图片背景色

                //画图片的背景噪音线
                for (int i = 0; i < 25; i++)
                {
                    var x1 = random.Next(imageCode.Width);
                    var x2 = random.Next(imageCode.Width);
                    var y1 = random.Next(imageCode.Height);
                    var y2 = random.Next(imageCode.Height);

                    g.DrawLine(new Pen(Color.Silver), new Point(x1, y1), new Point(x2, y2));
                }

                var font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic);
                var brush = new System.Drawing.Drawing2D.LinearGradientBrush(
                    new Rectangle(0, 0, imageCode.Width, imageCode.Height),
                    Color.Blue, Color.DarkRed, 1.2F, true);
                g.DrawString(code, font, brush, 2, 2);

                //画图片的前景噪音点
                for (int i = 0; i < 100; i++)
                {
                    var x = random.Next(imageCode.Width);
                    var y = random.Next(imageCode.Height);
                    imageCode.SetPixel(x, y, Color.FromArgb(random.Next()));
                }

                //画图片的边框线
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, imageCode.Width - 1, imageCode.Height - 1);
                var ms = new System.IO.MemoryStream();
                imageCode.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

                System.Web.HttpContext.Current.Response.ClearContent();
                System.Web.HttpContext.Current.Response.ContentType = "image/Jpeg";
                System.Web.HttpContext.Current.Response.BinaryWrite(ms.ToArray());
            }
            finally
            {
                g.Dispose();
                imageCode.Dispose();
                System.Web.HttpContext.Current.Session["CCode"] = code;
            }

        }
        #endregion

        #region 生成数据验证码
        private string GenerateCheckCode()
        {
            int number;
            char code;
            string checkCode = String.Empty;

            var random = new Random();

            for (int i = 0; i < 5; i++)
            {
                number = random.Next();

                if (number % 2 == 0)
                    code = (char)('0' + (char)(number % 10));
                else
                    code = (char)('A' + (char)(number % 26));

                checkCode += code.ToString();
            }
            return checkCode;
        }
        #endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
