using System;
using System.Collections.Generic;

namespace Voodoo
{
    /// <summary>
    /// 联系人
    /// </summary>
    public class Contact
    {
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 英文名
        /// </summary>
        public string EnglishName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public e_Sex Sex { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public DateTime BirthDay { get; set; }


        /// <summary>
        /// 电子邮件
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 即时通讯
        /// </summary>
        public List<e_Messanger> Messanger { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public List<PhoneNumber> Phone { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public Address Address { get; set; }

        /// <summary>
        /// 组织
        /// </summary>
        public string Organization { get; set; }

        /// <summary>
        /// 公司
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 网站
        /// </summary>
        public List<string> WebSite { get; set; }

        /// <summary>
        /// 照片地址 
        /// </summary>
        public string Photo { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }


    }

}
