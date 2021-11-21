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
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Sibin.FrameworkExtensions.DotNet.Web;
using Sibin.Imaging.TwoD;

namespace e_Travel.MasterPage
{
    public partial class PublicMaster : System.Web.UI.MasterPage
    {
        #region Private Variables
        TravelSegmentMstBLL objPtlAdmTravelSegmentMstBLL;
        TourPackageTypeMstBLL objPtlAdmTourPackageMstBLL;
        AccommodationMstBLL objBusAdmAccommodationMstBLL;//For Accomodation Type
        RoomPlanMstBLL objPtlAdmRoomPlanMstBLL;//For Room Plan Name
        VehicleTypeMasterBLL objPtlAdmVehicleTypeMasterBLL;//For Vehicle Type
        BusinessMstBLL objBusinessMstBLL;//For Destination
        #endregion

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadAllPackageType();
                LoadDestination();
            }
        }
        #endregion

        #region Load TravelSegment
        //private void LoadTravelSegment()
        //{
        //    objPtlAdmTravelSegmentMstBLL = new PtlAdmTravelSegmentMstBLL();
        //    try
        //    {
        //        TravelSegmentDropDown.Items.Clear();
        //        TravelSegmentDropDown.Items.Add(new ListItem("-----------------------Select-----------------------", "0"));
        //        TravelSegmentDropDown.DataTextField = "TravelSegmentName";
        //        TravelSegmentDropDown.DataValueField = "TravelSegmentID";
        //        TravelSegmentDropDown.DataSource = objPtlAdmTravelSegmentMstBLL.GetTravelSegment();
        //        TravelSegmentDropDown.DataBind();
        //        TravelSegmentDropDown.SelectedIndex = 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        WebMessenger.Show(this, "Unable To Load Travel Segment From the server", "Error", DialogTypes.Error);
        //        return;
        //    }
        //    finally
        //    {
        //        objPtlAdmTravelSegmentMstBLL = null;
        //    }
        //}
        #endregion

        #region Load Package Type
        private void LoadAllPackageType()
        {
            objPtlAdmTourPackageMstBLL = new TourPackageTypeMstBLL();
            try
            {
                DataTable dt = objPtlAdmTourPackageMstBLL.GetPackageType();
                PackageTypeDropDown.Items.Clear();
                PackageTypeDropDown.Items.Add(new ListItem("-----------------------Select-----------------------", "0"));
                PackageTypeDropDown.DataTextField = "PackageTypeName";
                PackageTypeDropDown.DataValueField = "PackageTypeID";
                PackageTypeDropDown.DataSource = dt;
                PackageTypeDropDown.DataBind();
            }
            catch (Exception)
            {
               WebMessenger.Show(this, "Error Occured While Loading Package Type From Server", "Error", DialogTypes.Error);
                return;
            }
            finally
            {
                objPtlAdmTourPackageMstBLL = null;
            }

        }
        #endregion       

        #region Load Destination
        private void LoadDestination()
        {
            objBusinessMstBLL = new BusinessMstBLL();
            try
            {
                DestinationDropDown.Items.Clear();
                DestinationDropDown.Items.Add(new ListItem("--- Select Destination ---", "0"));
                DestinationDropDown.DataSource = objBusinessMstBLL.GetDestination(null);
                DestinationDropDown.DataTextField = "DestinationName";
                DestinationDropDown.DataValueField = "DestinationID";
                DestinationDropDown.DataBind();
            }
            catch (Exception)
            {
                WebMessenger.Show(this, "Error Occured While Loading Destination", "Error", DialogTypes.Error);
                return;
            }
            finally
            {
                objBusinessMstBLL = null;
            }
        }
        #endregion

        #region Load Accomodation Type
        //private void LoadAccomodationType()
        //{
        //    objBusAdmAccommodationMstBLL = new BusAdmAccommodationMstBLL();
        //    try
        //    {
        //        AccTypeDropDown.Items.Clear();
        //        AccTypeDropDown.Items.Add(new ListItem("-----------------------Select-----------------------", "0"));
        //        AccTypeDropDown.DataSource = objBusAdmAccommodationMstBLL.GetAllAccTypeList();
        //        AccTypeDropDown.DataTextField = "AccTypeName";
        //        AccTypeDropDown.DataValueField = "AccTypeID";
        //        AccTypeDropDown.DataBind();
        //    }
        //    catch (Exception)
        //    {
        //        WebMessenger.Show(this, "Error While Loading Accomodation Type", "Error", DialogTypes.Error);
        //        return;
        //    }
        //    finally
        //    {
        //        objBusAdmAccommodationMstBLL = null;
        //    }
        //}
        #endregion

        #region Load Room Plan Name
        private void LoadRoomPlanName()
        {
            objPtlAdmRoomPlanMstBLL = new RoomPlanMstBLL();
            try
            {
                RoomPlanDropDown.Items.Clear();
                RoomPlanDropDown.Items.Add(new ListItem("-----------------------Select-----------------------", "0"));
                RoomPlanDropDown.DataSource = objPtlAdmRoomPlanMstBLL.GetRoomPlan();
                RoomPlanDropDown.DataValueField = "RoomPlanID";
                RoomPlanDropDown.DataTextField = "RoomPlanName";
                RoomPlanDropDown.DataBind();
            }
            catch (Exception)
            {
                WebMessenger.Show(this.Page, "Error Occured While Loading Room Plan Name", "Error", DialogTypes.Error);
                return;
            }
            finally
            {
                objPtlAdmRoomPlanMstBLL = null;
            }
        }
        #endregion

        #region Load Vehicle Type
        private void LoadVehicleType()
        {
            objPtlAdmVehicleTypeMasterBLL = new VehicleTypeMasterBLL();
            try
            {
                VehicleTypeDropDown.Items.Clear();
                VehicleTypeDropDown.Items.Add(new ListItem("-----------------------Select-----------------------", "0"));
                VehicleTypeDropDown.DataTextField = "VehicleTypeName";
                VehicleTypeDropDown.DataValueField = "VehicleTypeID";
                VehicleTypeDropDown.DataSource = objPtlAdmVehicleTypeMasterBLL.GetVehicleType();
                VehicleTypeDropDown.DataBind();
                VehicleTypeDropDown.SelectedIndex = 0;
            }
            catch (Exception)
            {
                WebMessenger.Show(this, "Error Occured While Loading Vehicle Type", "Error", DialogTypes.Error);
                return;
            }
            finally
            {
                objPtlAdmVehicleTypeMasterBLL = null;
            }
        }
        #endregion

        private void LoadTotalAdultList()
        {
            TotalAdultDropDown.Items.Clear();
            TotalChildrenDropDown.Items.Clear();
            for (int i = 0; i <= 50; i++)
            {
                TotalAdultDropDown.Items.Add(new ListItem(i.ToString(), i.ToString()));
                TotalChildrenDropDown.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            //if (TravelSegmentDropDown.SelectedIndex > 0)
            //{
            //    Session["TravelSegmentID"] = TravelSegmentDropDown.SelectedValue;
            //}
            //else
            //{
            //    Session["TravelSegmentID"] = null;
            //}
            //if (PackageTypeDropDown.SelectedIndex > 0)
            //{
            //    Session["PackageTypeID"] = PackageTypeDropDown.SelectedValue;
            //}
            //else
            //{
            //    Session["PackageTypeID"] = null;
            //}

        }


    }
}