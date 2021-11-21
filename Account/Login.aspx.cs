using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using e_Travel.Class;
using Sibin.FrameworkExtensions.DotNet.Web;

namespace e_Travel.Account
{
    public partial class Login : AdminBasePage 
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            this.RegisterJS("$('.TopMenuWrapper').hide();");
           // RegisterHyperLink.NavigateUrl = "Register.aspx?ReturnUrl=" + HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
        }
    }
}
