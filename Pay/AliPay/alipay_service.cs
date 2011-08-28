using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Voodoo.Pay.AliPay;
using System.Collections;

namespace Voodoo.Pay.AliPay
{
    /// <summary>
    /// 类名：alipay_service
    /// 功能：支付宝外部服务接口控制
    /// 详细：该页面是请求参数核心处理文件，不需要修改
    /// 版本：3.0
    /// 修改日期：2010-06-13
    /// 说明：
    /// 以下代码只是为了方便商户测试而提供的样例代码，商户可以根据自己网站的需要，按照技术文档编写,并非一定要使用该代码。
    /// 该代码仅供学习和研究支付宝接口使用，只是提供一个参考
    /// </summary>
    public class Alipay_Service
    {
        private string gateway = "";                //网关地址
        private string _key = "";                    //交易安全校验码
        private string _input_charset = "";         //编码格式
        private string _sign_type = "";              //加密方式（签名方式）
        private string mysign = "";                 //加密结果（签名结果）
        private ArrayList sPara = new ArrayList();  //需要加密的已经过滤后的参数数组

        /// <summary>
        /// 构造函数
        /// 从配置文件及入口文件中初始化变量
        /// </summary>
        /// <param name="partner">合作身份者ID</param>
        /// <param name="seller_email">签约支付宝账号或卖家支付宝帐户</param>
        /// <param name="return_url">付完款后跳转的页面 要用 以http开头格式的完整路径，不允许加?id=123这类自定义参数</param>
        /// <param name="notify_url">交易过程中服务器通知的页面 要用 以http开格式的完整路径，不允许加?id=123这类自定义参数</param>
        /// <param name="show_url">网站商品的展示地址，不允许加?id=123这类自定义参数</param>
        /// <param name="out_trade_no">请与贵网站订单系统中的唯一订单号匹配</param>
        /// <param name="subject">订单名称，显示在支付宝收银台里的“商品名称”里，显示在支付宝的交易管理的“商品名称”的列表里。</param>
        /// <param name="body">订单描述、订单详细、订单备注，显示在支付宝收银台里的“商品描述”里</param>
        /// <param name="total_fee">订单总金额，显示在支付宝收银台里的“应付总额”里</param>
        /// <param name="paymethod">默认支付方式，四个值可选：bankPay(网银); cartoon(卡通); directPay(余额); CASH(网点支付)</param>
        /// <param name="defaultbank">默认网银代号，代号列表见club.alipay.com/read.php?tid=8681379</param>
        /// <param name="encrypt_key">防钓鱼时间戳</param>
        /// <param name="exter_invoke_ip">买家本地电脑的IP地址</param>
        /// <param name="extra_common_param">自定义参数，可存放任何内容（除等特殊字符外），不会显示在页面上</param>
        /// <param name="buyer_email">默认买家支付宝账号</param>
        /// <param name="royalty_type">提成类型，该值为固定值：10，不需要修改</param>
        /// <param name="royalty_parameters">提成信息集，与需要结合商户网站自身情况动态获取每笔交易的各分润收款账号、各分润金额、各分润说明。最多只能设置10条</param>
        /// <param name="it_b_pay">超时时间，不填默认是15天。八个值可选：1h(1小时),2h(2小时),3h(3小时),1d(1天),3d(3天),7d(7天),15d(15天),1c(当天)</param>
        /// <param name="key">安全检验码</param>
        /// <param name="input_charset">字符编码格式 目前支持 gb2312 或 utf-8</param>
        /// <param name="sign_type">加密方式 不需修改</param>
        public Alipay_Service(string partner,
            string seller_email,
            string return_url,
            string notify_url,
            string show_url,
            string out_trade_no,
            string subject,
            string body,
            string total_fee,
            string paymethod,
            string defaultbank,
            string encrypt_key,
            string exter_invoke_ip,
            string extra_common_param,
            string buyer_email,
            string royalty_type,
            string royalty_parameters,
            string it_b_pay,
            string key,
            string input_charset,
            string sign_type)
        {
            gateway = "https://www.alipay.com/cooperate/gateway.do?";
            _key = key.Trim();
            _input_charset = input_charset.ToLower();
            _sign_type = sign_type.ToUpper();

            //构造加密参数数组
            sPara.Add("service=create_direct_pay_by_user");
            sPara.Add("payment_type=1");
            sPara.Add("partner=" + partner);
            sPara.Add("seller_email=" + seller_email);
            sPara.Add("return_url=" + return_url);
            sPara.Add("notify_url=" + notify_url);
            sPara.Add("_input_charset=" + _input_charset);
            sPara.Add("show_url=" + show_url);
            sPara.Add("out_trade_no=" + out_trade_no);
            sPara.Add("subject=" + subject);
            sPara.Add("body=" + body);
            sPara.Add("total_fee=" + total_fee);
            sPara.Add("paymethod=" + paymethod);
            sPara.Add("defaultbank=" + defaultbank);
            sPara.Add("anti_phishing_key=" + encrypt_key);
            sPara.Add("exter_invoke_ip=" + exter_invoke_ip);
            sPara.Add("extra_common_param=" + extra_common_param);
            sPara.Add("buyer_email=" + buyer_email);
            sPara.Add("royalty_type=" + royalty_type);
            sPara.Add("royalty_parameters=" + royalty_parameters);
            sPara.Add("it_b_pay=" + it_b_pay);

            sPara = Alipay_Function.Para_filter(sPara);
            sPara.Sort();   //得到从字母a到z排序后的加密参数数组
            //获得签名结果
            mysign = Alipay_Function.Build_mysign(sPara, _key, _sign_type, _input_charset);
        }

        /// <summary>
        /// 构造请求URL（GET方式请求）
        /// </summary>
        /// <returns>请求url</returns>
        public string Create_url()
        {
            string strUrl = gateway;
            string arg = Alipay_Function.Create_linkstring_urlencode(sPara);	//把数组所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串
            strUrl = strUrl + arg + "sign=" + mysign + "&sign_type=" + _sign_type;
            return strUrl;
        }

        /// <summary>
        /// 构造Post表单提交HTML（POST方式请求）
        /// </summary>
        /// <returns>输出 表单提交HTML文本</returns>
        public string Build_postform()
        {
            StringBuilder sbHtml = new StringBuilder();
            sbHtml.Append("<form id=\"alipaysubmit\" name=\"alipaysubmit\" action=\"" + gateway + "_input_charset=" + _input_charset + "\" method=\"post\">");

            int nCount = sPara.Count;
            int i;
            for (i = 0; i < nCount; i++)
            {
                //把sArray的数组里的元素格式：变量名=值，分割开来
                int nPos = sPara[i].ToString().IndexOf('=');              //获得=字符的位置
                int nLen = sPara[i].ToString().Length;                    //获得字符串长度
                string itemName = sPara[i].ToString().Substring(0, nPos); //获得变量名
                string itemValue = "";                                    //获得变量的值
                if (nPos + 1 < nLen)
                {
                    itemValue = sPara[i].ToString().Substring(nPos + 1);
                }

                sbHtml.Append("<input type=\"hidden\" name=\"" + itemName + "\" value=\"" + itemValue + "\"/>");
            }

            sbHtml.Append("<input type=\"hidden\" name=\"sign\" value=\"" + mysign + "\"/>");
            sbHtml.Append("<input type=\"hidden\" name=\"sign_type\" value=\"" + _sign_type + "\"/></form>");

            sbHtml.Append("<input type=\"button\" name=\"v_action\" value=\"支付宝确认付款\" onClick=\"document.forms['alipaysubmit'].submit();\">");

            return sbHtml.ToString();
        }
    }
}
