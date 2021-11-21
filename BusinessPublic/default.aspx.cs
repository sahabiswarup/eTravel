using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using e_Travel.Class;
using e_TravelBLL.TourPackage;

namespace e_Travel.BusinessPublic
{
    public partial class _default : PublicBasePage
    {
        #region private variables
        ConfigDiscountBLL objDiscount;
        DestinationDtlBLL objDest;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDiscount();
                LoadDestination();
            }
        }

        private void LoadDiscount()
        {
            objDiscount = new ConfigDiscountBLL();
            try
            {
                DiscountListView.DataSource = objDiscount.SelectActiveDiscountDetailsByBusID(Session["BusinessDtl"].ToString().Split('|')[0].ToString());
                DiscountListView.DataBind();
            }
            catch (Exception)
            {

                WebMessenger.Show(this, "Unable To Load Discount/Offer At This Moment", "Error", DialogTypes.Error);
                return;
            }
            finally
            {
                objDiscount = null;
            }
        }

        private void LoadDestination()
        {
            objDest = new DestinationDtlBLL();
            try
            {
                DataTable dt = objDest.GetAllDestinationDtlByPaged(0, 10, Session["BusinessDtl"].ToString().Split('|')[0].ToString());
                StringBuilder sb = new StringBuilder();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sb.Append("<div class='slide'><div style = 'width: 600px; height: 300px;'><img src='data:image/jpg;base64," + Convert.ToBase64String((byte[])dt.Rows[i]["DefaultPhotoNormal"]) + "' /></div><p class='teaser' data-position='295,15' data-delay='500'>" + dt.Rows[i]["Tagline"] + "</p></div>");
                    }
                }
                destLiteral.Text = sb.ToString();
            }
            catch (Exception ex)
            {
                WebMessenger.Show(this, "Unable To Load Destination At This Moment", "Error", DialogTypes.Error);
            }
        }
    }
}
