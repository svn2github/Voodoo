

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
namespace Voodoo.UI
{
    ///<summary>
    /// VerifyCode 的摘要说明
    ///</summary>
    public class VerifyCode : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext Context)
        {
            VerifyCode v = new VerifyCode();
            string code = v.CreateVerifyCode(5);//验证码长度
            v.CreateImageOnPage(code, Context);
        }

        #region  验证码长度(默认6个验证码的长度)
        int length = 6;
        public int Length
        {
            get { return length; }
            set { length = value; }
        }
        #endregion

        #region 验证码字体大小(为了显示扭曲效果，默认40像素，可以自行修改)
        int fontSize = 14;
        public int FontSize
        {
            get { return fontSize; }
            set { fontSize = value; }
        }
        #endregion

        #region 边框补(默认1像素)
        int padding = 1;
        public int Padding
        {
            get { return padding; }
            set { padding = value; }
        }
        #endregion

        #region 是否输出燥点(默认不输出)
        bool chaos = false;
        public bool Chaos
        {
            get { return chaos; }
            set { chaos = value; }
        }
        #endregion

        #region 输出燥点的颜色(默认灰色)
        Color chaosColor = Color.LightGray;
        public Color ChaosColor
        {
            get { return chaosColor; }
            set { chaosColor = value; }
        }
        #endregion

        #region 自定义背景色(默认白色)
        Color backgroundColor = Color.Transparent;
        public Color BackgroundColor
        {
            get { return backgroundColor; }
            set { backgroundColor = value; }
        }
        #endregion

        #region 自定义随机颜色数组
        Color[] colors = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple };
        public Color[] Colors
        {
            get { return colors; }
            set { colors = value; }
        }
        #endregion

        #region 自定义字体数组
        string[] fonts = { "Arial", "courier  new  ", "微软雅黑", "幼圆" };
        public string[] Fonts
        {
            get { return fonts; }
            set { fonts = value; }
        }
        #endregion

        #region 自定义随机码字符串序列(使用逗号分隔)
        string codeSerial = "2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,j,k,m,n,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,J,K,L,M,N,P,Q,R,S,T,U,V,W,X,Y,Z";
        public string CodeSerial
        {
            get { return codeSerial; }
            set { codeSerial = value; }
        }
        #endregion

        #region 产生波形滤镜效果
        private const double PI = 3.1415926535897932384626433832795;
        private const double PI2 = 6.283185307179586476925286766559;
        ///<summary>
        ///正弦曲线Wave扭曲图片（Edit By 51aspx.com）
        ///</summary>
        ///<param name="srcBmp">图片路径</param>
        ///<param name="bXDir">如果扭曲则选择为True</param>
        ///<param name="nMultValue">波形的幅度倍数，越大扭曲的程度越高，一般为3</param>
        ///<param name="dPhase">波形的起始相位，取值区间[0-2*PI)</param>
        ///<returns></returns>
        public System.Drawing.Bitmap TwistImage(Bitmap srcBmp, bool bXDir, double dMultValue, double dPhase)
        {
            System.Drawing.Bitmap destBmp = new Bitmap(srcBmp.Width, srcBmp.Height);
            // 将位图背景填充为白色
            System.Drawing.Graphics graph = System.Drawing.Graphics.FromImage(destBmp);
            graph.FillRectangle(new SolidBrush(System.Drawing.Color.White), 0, 0, destBmp.Width, destBmp.Height);
            graph.Dispose();
            double dBaseAxisLen = bXDir ? (double)destBmp.Height : (double)destBmp.Width;
            for (int i = 0; i < destBmp.Width; i++)
            {
                for (int j = 0; j < destBmp.Height; j++)
                {
                    double dx = 0;
                    dx = bXDir ? (PI2 * (double)j) / dBaseAxisLen : (PI2 * (double)i) / dBaseAxisLen;
                    dx += dPhase;
                    double dy = Math.Sin(dx);
                    // 取得当前点的颜色
                    int nOldX = 0, nOldY = 0;
                    nOldX = bXDir ? i + (int)(dy * dMultValue) : i;
                    nOldY = bXDir ? j : j + (int)(dy * dMultValue);
                    System.Drawing.Color color = srcBmp.GetPixel(i, j);
                    if (nOldX >= 0 && nOldX < destBmp.Width
                     && nOldY >= 0 && nOldY < destBmp.Height)
                    {
                        destBmp.SetPixel(nOldX, nOldY, color);
                    }
                }
            }
            return destBmp;
        }
        #endregion

        #region 生成校验码图片
        public Bitmap CreateImageCode(string code)
        {
            if (code == null)
            {
                code = RndNum(5);
            }
            int fSize = FontSize;
            int fWidth = fSize + Padding;
            int imageWidth = (int)(code.Length * fWidth) + 4 + Padding * code.Length;
            int imageHeight = fSize * 2;
            System.Drawing.Bitmap image = new System.Drawing.Bitmap(imageWidth, imageHeight);
            Graphics g = Graphics.FromImage(image);
            g.Clear(BackgroundColor);
            Random rand = new Random();
            //给背景添加随机生成的燥点
            if (this.Chaos)
            {
                Pen pen = new Pen(ChaosColor, 0);
                int c = Length * 5;
                for (int i = 0; i < c; i++)
                {
                    int x = rand.Next(image.Width);
                    int y = rand.Next(image.Height);
                    g.DrawRectangle(pen, x, y, 1, 1);
                }
            }
            int left = 0, top = 0, top1 = 1, top2 = 1;
            int n1 = (imageHeight - FontSize - Padding * 2);
            int n2 = n1 / 4;
            top1 = n2;
            top2 = n2 * 2;
            Font f;
            Brush b;
            int cindex, findex;
            //随机字体和颜色的验证码字符
            for (int i = 0; i < code.Length; i++)
            {
                cindex = @int.GetRandomNumber(0, Colors.Length);
                findex = @int.GetRandomNumber(0, Fonts.Length);
                f = new System.Drawing.Font(Fonts[findex], fSize);
                b = new System.Drawing.SolidBrush(Colors[cindex]);
                if (i % 2 == 1)
                {
                    top = top2;
                }
                else
                {
                    top = top1;
                }
                left = i * fWidth;
                if (i > 0)
                {
                    left += i * Padding;
                }
                g.DrawString(code.Substring(i, 1), f, b, left, Padding);
            }
            //画一个边框边框颜色为Color.Gainsboro
            //g.DrawRectangle(new Pen(Color.Transparent, 0), 0, 0, image.Width - 1, image.Height - 1);
            g.Dispose();
            //产生波形（Add By 51aspx.com）
            //image = TwistImage(image, true, 3, 0);
            return image;
        }
        #endregion

        #region 将创建好的图片输出到页面
        public void CreateImageOnPage(string code, HttpContext context)
        {
            if (code.IsNullOrEmpty())
            {
                code = RndNum(4);
            }
            //this.backgroundColor = Color.Transparent;
            //this.FontSize = 11;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            Bitmap image = this.CreateImageCode(code);
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            context.Response.ClearContent();
            context.Response.ContentType = "image/Png";
            context.Response.BinaryWrite(ms.GetBuffer());
            ms.Close();
            ms = null;
            image.Dispose();
            image = null;
        }
        #endregion

        #region 生成随机加减法字符码
        public string CreateVerifyCode(int codeLen)
        {
            //if (codeLen == 0)
            //{
            //    codeLen = Length;
            //}
            //string[] arr = CodeSerial.Split(',');
            //string code = "";
            //int randValue = -1;
            //Random rand =  new  Random(unchecked((int)DateTime.Now.Ticks));
            //for (int i = 0; i < codeLen; i++)
            //{
            //    randValue = rand.Next(0, arr.Length - 1);
            //    code += arr[randValue];
            //}
            //return code;
            string[] sym = new string[3];
            sym[0] = "+";
            sym[1] = "×";
            sym[2] = "-";
            int index = @int.GetRandomNumber(0, 3);

            int num = 0;
            int num2 = 0;
            int result = 0;

            if (index == 0)//加法
            {
                num = @int.GetRandomNumber(0, 9);
                System.Threading.Thread.Sleep(13);
                num2 = @int.GetRandomNumber(10, 99);
                result = num + num2;
            }
            else if (index == 1)//乘法 
            {
                num = @int.GetRandomNumber(1, 10);
                System.Threading.Thread.Sleep(13);
                num2 = @int.GetRandomNumber(1, 10);
                result = num * num2;
            }
            else if (index == 2) //减法
            {
                num = @int.GetRandomNumber(10, 100);
                System.Threading.Thread.Sleep(13);
                num2 = @int.GetRandomNumber(0, 10);
                result = num - num2;
            }



            HttpContext.Current.Session["SafeCode"] = result;

            return num.ToString() + sym[index] + num2 + "=?";
        }

        public string CreateVerifyCode()
        {
            return CreateVerifyCode(0);
        }

        #region 生成随机成语字符串
        /// <summary>
        /// 生成随机成语字符串
        /// </summary>
        /// <returns></returns>
        public string CreateForWordCode()
        {
            string[] xx = new string[] { 
                "平易近人",
                "宽宏大度",
                "冰清玉洁",
                "持之以恒",
                "锲而不舍",
                "废寝忘食",
                "大义凛然",
                "临危不俱",
                "光明磊落",
                "不屈不挠",
                "料事如神",
                "足智多谋",
                "融会贯通",
                "学贯中西",
                "博古通今",
                "才华横溢",
                "出类拔萃",
                "博大精深",
                "集思广益",
                "举一反三",
                "憨态可掬",
                "文质彬彬",
                "风度翩翩",
                "相貌堂堂",
                "落落大方",
                "斗志昂扬",
                "意气风发",
                "威风凛凛",
                "容光焕发",
                "神采奕奕",
                "三顾茅庐",
                "铁杵成针",
                "望梅止渴",
                "完璧归赵",
                "四面楚歌",
                "负荆请罪",
                "精忠报国",
                "手不释卷",
                "悬梁刺股",
                "凿壁偷光",
                "走马观花",
                "欢呼雀跃",
                "扶老携幼",
                "手舞足蹈",
                "促膝谈心",
                "前俯后仰",
                "奔走相告",
                "跋山涉水",
                "前赴后继",
                "张牙舞爪",
                "恩重如山",
                "深情厚谊",
                "手足情深",
                "形影不离",
                "血浓于水",
                "志同道合",
                "风雨同舟",
                "赤诚相待",
                "肝胆相照",
                "生死相依",
                "春寒料峭",
                "春意盎然",
                "春暖花开",
                "满园春色",
                "春华秋实",
                "春风化雨",
                "骄阳似火",
                "暑气蒸人",
                "烈日炎炎",
                "秋风送爽",
                "秋高气爽",
                "秋色宜人",
                "冰天雪地",
                "寒气袭人",
                "寒冬腊月",
                "日积月累",
                "温故知新",
                "勤能补拙",
                "笨鸟先飞",
                "学无止境",
                "学海无涯",
                "滴水穿石",
                "发奋图强",
                "开卷有益",
                "济济一堂",
                "热火朝天",
                "门庭若市",
                "万人空巷",
                "座无虚席",
                "高朋满座",
                "如火如荼",
                "蒸蒸日上",
                "欣欣向荣",
                "川流不息"

            };

            string result = xx[@int.GetRandomNumber(0, xx.Length)];
            HttpContext.Current.Session["SafeCode"] = result;
            return result;
        }
        #endregion
        #endregion

        private string RndNum(int VcodeNum)
        {
            string[] strArray = "1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,m,n,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,J,K,L,M,N,P,Q,R,S,T,U,W,V,X,Y,Z".Split(new char[] { ',' });
            string str3 = null;
            short num2 = (short)VcodeNum;

            for (short i = 1; i <= num2; i = (short)(i + 1))
            {

                Random rd = new Random(i * ((int)DateTime.Now.Ticks));

                str3 = str3 + strArray[(int)Math.Round((double)((float)(56f * rd.NextDouble())).ToInt32())];
            }
            HttpContext.Current.Session["SafeCode"] = str3;
            return str3;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}