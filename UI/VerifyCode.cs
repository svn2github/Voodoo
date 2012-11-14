

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
                code = CreateVerifyCode(3);
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
                "我是春哥",
                "李毅大帝",
                "屌丝男士",
                "屌丝女士",
                "和谐社会",
                "油价下调",
                "房价下调",
                "人见人爱",
                "花见花开",
                "司法公正",
                "言论自由",
                "中央委员",
                "人肉搜索",
                "网络传闻",
                "生命之环",
                "造节运动",
                "东方之门",
                "长江大桥",
                "肌肉萝莉",
                "政局常委",
                "北京条约",
                "纪委产生",
                "五星酒店",
                "路易威登",
                "犯罪心理",
                "邪恶力量",
                "乐不思蜀",
                "网络管制",
                "孕育希望",
                "美国之声",
                "为人父母",
                "另类家庭",
                "私人诊所",
                "谍影迷情",
                "不要考研",
                "大连实德",
                "美女野兽",
                "洪水猛兽",
                "刷卡消费",
                "傲骨贤妻",
                "妙警贼探",
                "都市侠盗",
                "无耻之徒",
                "单身毒妈",
                "死亡地带",
                "舞林争霸",
                "生活大爆炸",
                "绯闻女孩",
                "绝望主妇",
                "破产姐妹",
                "触摸未来",
                "实习医生",
                "国土安全",
                "摩登家庭",
                "终极审判",
                "行尸走肉",
                "广告狂人",
                "逝者之证",
                "疑犯追踪",
                "福尔摩斯",
                "侠胆雄狮",
                "陨落星辰",
                "鬼楼契约",
                "外星邻居",
                "妖女迷行",
                "危机边缘",
                "脱狱之王",
                "替身姐妹",
                "名媛望族",
                "法证先锋",
                "潜行阻击",
                "倚天屠龙记",
                "护花危情",
                "天下第一",
                "白发魔女",
                "公主嫁到",
                "阿旺新传",
                "万凰之王",
                "功夫足球",
                "新不了情",
                "圆月弯刀",
                "通天干探",
                "陀枪师姐",
                "溏心风暴",
                "谈判专家",
                "耀舞长安",
                "甜言蜜语",
                "万家灯火",
                "东方之珠",
                "纵横天下",
                "纵横四海",
                "英雄本色",
                "日月神剑",
                "与敌同行",
                "缺宅男女",
                "如来神掌",
                "酒店风云",
                "美味情缘",
                "天地男儿",
                "天子寻龙",
                "剑啸江湖",
                "精武陈真",
                "人龙传说",
                "新楚留香",
                "萧十一郎",
                "隔世追凶",
                "香帅传奇",
                "非常岳母",
                "疑情别恋",
                "争分夺秒",
                "老婆大人",
                "突围行动",
                "其乐无穷",
                "盛世仁杰",
                "别无选择",
                "紫禁之巅",
                "汇通天下",
                "乱世佳人",
                "野蛮奶奶",
                "桌球天王",
                "梅艳芳菲",
                "功夫状元",
                "恋爱女王",
                "雷霆扫毒",
                "回到三国",
                "读心神探",
                "怒火街头",
                "雪山飞狐",
                "天龙八部",
                "射雕英雄传",
                "笑傲江湖",
                "书剑恩仇录",
                "神雕侠侣",
                "北京青年",
                "爱情公寓",
                "武林外传",
                "康熙王朝",
                "超级厉害"
            };

            string result = xx[@int.GetRandomNumber(0, xx.Length)];
            HttpContext.Current.Session["SafeCode"] = result;
            return result;
        }
        #endregion
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