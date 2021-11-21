using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace e_Travel
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            WebMessenger.Show(this, "Gym Pass Type Could Not Load" + "Error Information", "Information", DialogTypes.Information);
        }
    }
}
