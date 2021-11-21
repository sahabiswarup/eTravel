using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using e_Travel.Class;
using Sibin.Utilities.Web.ExceptionHandling;
using e_TravelBLL.TourPackage;
using SIBINUtility.ValidatorClass;
using Sibin.FrameworkExtensions.DotNet.Web;
using System.Data;
using Sibin.Utilities.Imaging.TwoD;

namespace e_Travel.Public
{

    public partial class HomePage : AdminBasePage
    {
        TpkPackageMst objBusAdmTpkPackageMst;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadTopTenPackageType();
            }
        }

        #region Load Top Ten Package Type
        private void LoadTopTenPackageType()
        {
            objBusAdmTpkPackageMst = new TpkPackageMst();
            try
            {
                DataTable dt = new DataTable();
                dt = objBusAdmTpkPackageMst.GetTopTenPackages();
                if (dt.Rows.Count > 0)
                {
                    GridViewPackage.DataSource = dt;
                    GridViewPackage.DataBind();
                }
            }
            catch (Exception)
            {
                WebMessenger.Show(this.Page, "Error Occured", "Error", DialogTypes.Error);
                return;
            }
            finally
            {
                objBusAdmTpkPackageMst = null;
            }
        }
        #endregion
    }
}