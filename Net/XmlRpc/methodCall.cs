using System;
using System.Collections.Generic;

namespace Voodoo.Net.XmlRpc
{
    public class methodCall
    {
        public string methodName { get; set; }

        public List<param> @params { get; set; }
    }

    //public class Params
    //{
    //    public List<Param> Param { get; set; }
    //}

    public class param
    {
        public ttring value { get; set; }
    }

    public class ttring
    {
        public string @string { get; set; }
    }

}
