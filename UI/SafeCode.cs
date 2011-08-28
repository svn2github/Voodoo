using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.SessionState;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace Voodoo.UI
{
    public class SafeCode : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext Context)
        {
            string str2 = this.RndNum(5);
            string name = "SafeCode";
            if (Context.Request.Params["name"] != null)
            {
                name = Context.Request.Params["name"];
            }
            //Context.Response.Cookies.Add(new HttpCookie(name, Voodoo.Security.Encrypt.Md5(str2.ToLower())));
            Context.Session[name] = str2.ToLower();
            Color[] colorArray = new Color[] { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple };
            string[] strArray = new string[] { "Verdana", "Microsoft Sans Serif", "Comic Sans MS", "Arial", "宋体" };
            Random random = new Random();
            Bitmap image = new Bitmap(str2.Length * 15,23);
            Graphics graphics = Graphics.FromImage(image);
            graphics.Clear(Color.White);
            int num = 1;
            do
            {
                int x = random.Next(image.Width);
                int y = random.Next(image.Height);
                graphics.DrawRectangle(new Pen(Color.LightGray, 0f), x, y, 1, 1);
                num++;
            }
            while (num <= 100);
            int num8 = str2.Length - 1;
            for (int i = 0; i <= num8; i++)
            {
                int index = random.Next(7);
                int num5 = random.Next(5);
                Font font = new Font(strArray[num5], 13f, FontStyle.Bold);
                Brush brush = new SolidBrush(colorArray[index]);
                int num7 = 4;
                if ((i + 1) == 0)
                {
                    num7 = 2;
                }
                graphics.DrawString(str2.Substring(i, 1), font, brush, (float)(3 + (i * 12)), (float)num7);
            }
            Context.Response.Clear();
            Context.Response.ContentType = "image/jpeg";
            image.Save(Context.Response.OutputStream,ImageFormat.Jpeg);
            image.Dispose();
            graphics.Dispose();
        }

        public void ProcessRequest1(HttpContext Context)
        {
            string s = this.RndNum(4);
            string str3 = "SafeCode";
            if (Context.Request.Params["name"] != null)
            {
                str3 = Context.Request.Params["name"];
            }
            Context.Session[str3] = s;
            int width = (int)Math.Round((double)(s.Length * 11.5));
            int height = 0x11;
            string familyName = "Arial";
            if (Context.Request.Params["width"] != null)
            {
                width = Context.Request.Params["width"].ToInt32();
            }
            if (Context.Request.Params["height"] != null)
            {
                height = Context.Request.Params["height"].ToInt32();
            }
            if (Context.Request.Params["font"] != null)
            {
                familyName = Context.Request.Params["font"];
            }
            Bitmap image = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            Graphics graphics = Graphics.FromImage(image);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rect = new Rectangle(0, 0, width, height);
            graphics.FillRectangle(new HatchBrush(HatchStyle.SmallConfetti, Color.LightGray, Color.White), rect);
            graphics.DrawString(s, new Font(familyName, 11.5f), new SolidBrush(Color.Black), (float)1f, (float)1f);
            Context.Response.Clear();
            Context.Response.ContentType = "image/jpeg";
            image.Save(Context.Response.OutputStream, ImageFormat.Jpeg);
            image.Dispose();
            graphics.Dispose();
        }

        private string RndNum(int VcodeNum)
        {
            string[] strArray = "1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,m,n,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,J,K,L,M,N,P,Q,R,S,T,U,W,V,X,Y,Z".Split(new char[] { ',' });
            string str3 = null;
            short num2 = (short)VcodeNum;

            for (short i = 1; i <= num2; i = (short)(i + 1))
            {

                Random rd = new Random(i  * ((int)DateTime.Now.Ticks));

                str3 = str3 + strArray[(int)Math.Round((double)((float)(56f * rd.NextDouble())).ToInt32())];
            }
            return str3;
        }

        // Properties
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

    }
}
