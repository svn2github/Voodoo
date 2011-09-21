using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Data.Common;

namespace Voodoo.UI
{
    public class AutoCompleleTextBox : VTextBox
    {
        public DataSourceControl Conn
        {
            get
            {
                return (DataSourceControl)this.ViewState["Conn"];
            }
            set
            {
                this.ViewState["Conn"] = value;
            }
        }
    }
}
