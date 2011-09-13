using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Voodoo.other
{
    /// <summary>
    /// 省市区县关系
    /// </summary>
    public class City
    {
        public string AreaName { get; set; }

        public string ZipCode { get; set; }

        public List<City> Children { get; set; }
    }

    public class ChineseProvince
    {
        

        List<City> city = new List<City>{
        new City{
            AreaName="北京市",
            Children=new List<City>
            {
                new City{
                    AreaName="市辖区",
                    Children=new List<City>{
                        new City{AreaName="朝阳区"}
                    }
                },
                new City{AreaName="市辖县"}
            }
        },
        new City{AreaName="上海市"},
        new City{AreaName="天津市"},
        new City{AreaName="重庆市"},
        new City{AreaName="黑龙江省"},
        new City{AreaName="辽宁省"},
        new City{AreaName="吉林省"},
        new City{AreaName="内蒙古自治区"},
        new City{AreaName="河北省"},
        new City{AreaName="河南省"},
        new City{AreaName="山西省"},
        new City{AreaName="甘肃省"},
        new City{AreaName="宁夏回族自治区"},
        new City{AreaName="新疆维吾尔自治区"},
        new City{AreaName="西藏自治区"},
        new City{AreaName="广西壮族自治区"},
        new City{AreaName="青海省"},
        new City{AreaName="云南省"},
        new City{AreaName="贵州省"},
        new City{AreaName="四川省"},
        new City{AreaName="湖北省"},
        new City{AreaName="湖南省"},
        new City{AreaName="山东省"},
        new City{AreaName="安徽省"},
        new City{AreaName="江苏省"},
        new City{AreaName="江西省"},
        new City{AreaName="广东省"},
        new City{AreaName="海南省"},
        new City{AreaName="福建省"},
        new City{AreaName="浙江省"},
        new City{AreaName="香港"},
        new City{AreaName="澳门"},
        new City{AreaName="台湾"},
        };
    }
}
