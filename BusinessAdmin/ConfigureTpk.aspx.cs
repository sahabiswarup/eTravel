using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Net;
using System.Net.Mail;
using System.Drawing;
using System.Web.Profile;

using e_Travel.Class;
using e_TravelBLL.TourPackage;

using Sibin.FrameworkExtensions.DotNet.Web;
//For Converting Json Object
using Newtonsoft.Json;
//For PDF
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using SIBINUtility.ValidatorClass;



namespace e_Travel.BusinessAdmin
{
    public partial class ConfigureTpk : AdminBasePage
    {
        #region Private Variables
        TpkPackageMst objPackage;
        IncludingExcludingMstBLL objIncludingExcludingMstBLL;
        TpkTransactionDtls objTpkTransactionDtls;
        #endregion

        #region PageLoad Event
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["BusDtl"] == null)
            {
                Response.Redirect("~/PortalAdmin/RegisterBusiness.aspx");
            }
            if (!IsPostBack)
            {
                if (Request.QueryString["TPKID"] == null)
                {
                    Response.Redirect("~/BusinessAdmin/SearchTpk.aspx");
                }
                else
                {
                    LoadPackageBasicDetails();
                    LoadPackageItinerayDetails();
                    LoadAccomodationType();
                    LoadRoomPlanName();
                    AutoSuggestVehicleType();
                    LoadPhotoGallery();
                    GetAllItems();
                    LoadOverNightDesAcc();
                   
                   
                }
            }
        }

        #endregion

        #region Load Package Details
        private void LoadPackageBasicDetails()
        {
            objPackage = new TpkPackageMst();
            try
            {
                DataTable dt = new DataTable();
                if (Request.QueryString["TPKID"] != null)
                {
                    string ID = Request.QueryString["TPKID"];
                    dt = objPackage.GetPackageDetailsByTpkID(ID);
                    ViewState["PackageDetails"] = dt;
                }
                else
                {
                    Response.Redirect("~/BusinessAdmin/TourPackage.aspx");
                }
                ViewState["PackageDetails"] = dt;
                PackageDetailsFormView.DataSource = dt;
                PackageDetailsFormView.DataBind();
                if (dt.Rows.Count > 0)
                {
                    PackageNameLabel.Text = dt.Rows[0]["TpkName"].ToString();
                    PackageDurationLabel.Text = " (" + dt.Rows[0]["TotalDays"].ToString() + " Days " + dt.Rows[0]["TotalNights"].ToString() + " Nights)";
                }
            }
            catch (Exception)
            {
                WebMessenger.Show(this, "Unable To Load Package Details At this Moment", "Package Details", DialogTypes.Error);
            }
            finally
            {
                objPackage = null;
            }
        }

        private void LoadPackageItinerayDetails()
        {
            objPackage = new TpkPackageMst();
            try
            {
                DataTable dt = new DataTable();
                if (Request.QueryString["TPKID"] != null)
                {
                    string ID = Request.QueryString["TPKID"];
                    dt = objPackage.GetTpkPackagaeItineraryDtlByTpkID(ID);
                }
                else
                {
                    Response.Redirect("~/BusinessAdmin/TourPackage.aspx");
                }
                ItineraryDetailsFGridView.DataSource = dt;
                ItineraryDetailsFGridView.DataBind();
            }
            catch (Exception ex)
            {
                WebMessenger.Show(this, "Unable To Load Package Itinerary Details At this Moment", "Package Itinerary Details", DialogTypes.Error);
                return;
            }
            finally
            {
                objPackage = null;
            }

        }

        private void LoadPhotoGallery()
        {
            objPackage = new TpkPackageMst();
            try
            {
                DataTable dt = objPackage.GetAllTpkPackagePhotoDtlPaged(1, 100, Session["BusDtl"].ToString().Split('|')[0].ToString(), Request.QueryString["TPKID"].ToString());
                Session["Image"] = dt;

                if (dt.Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<ul>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        //sb.Append("<img src='../WebHandler/DisplayTpkPhotoGallery.ashx?IT=PT&ID=" + dt.Rows[i]["TpkPhotoID"] + " ' width=\"110\" height=\"80\" />");
                        //sb.Append("<li><a href = '../WebHandler/DisplayTpkPhotoGallery.ashx?IT=PT&ID=" + dt.Rows[i]["TpkPhotoID"] + "'><img src='../WebHandler/DisplayTpkPhotoGallery.ashx?IT=PT&ID=" + dt.Rows[i]["TpkPhotoID"] + " ' /><span>Image1</span></a></li>");
                        sb.Append("<li><img src='data:image/png;base64," + Convert.ToBase64String((byte[])dt.Rows[i]["PhotoThumb"]) + "' /></li>");
                    }
                    sb.Append("</ul>");
                    TpPhotoLiteral.Text = sb.ToString();
                }
            }
            catch (Exception ex)
            {
                WebMessenger.Show(this, "Unable To Load Photo Gallery At This Moment", "Error", DialogTypes.Error);
            }
        }

        protected void PhotoAlbumRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater PhotoAlbumRepeater = sender as Repeater;
            if (PhotoAlbumRepeater.Items.Count < 1)
            {
                if (e.Item.ItemType == ListItemType.Footer)
                {
                    Label lblFooter = (Label)e.Item.FindControl("EmptyDataLabel");
                    lblFooter.Visible = true;
                }
            }
        }

        #endregion

        #region Load Configuration Details
        private void LoadAccomodationType()
        {
            objPackage = new TpkPackageMst();
            try
            {
                DataTable dt = objPackage.GetAccTypeByTpkID(Session["BusDtl"].ToString().Split('|')[0].ToString(), Request.QueryString["TPKID"].ToString());
                
                AccommodationTypeDropDownList.Items.Clear();
                AccommodationTypeDropDownList.Items.Add(new System.Web.UI.WebControls.ListItem("-----Select Accomodation Type -------", "0"));
                AccommodationTypeDropDownList.DataSource = dt;
                AccommodationTypeDropDownList.DataTextField = "AccTypeName";
                AccommodationTypeDropDownList.DataValueField = "AccTypeID";
                AccommodationTypeDropDownList.DataBind();

                //AdvanceAccTypeDropDownList.Items.Clear();
                //AdvanceAccTypeDropDownList.Items.Add(new System.Web.UI.WebControls.ListItem("-----Select Accomodation Type -------", "0"));
                //AdvanceAccTypeDropDownList.DataSource = dt;
                //AdvanceAccTypeDropDownList.DataTextField = "AccTypeName";
                //AdvanceAccTypeDropDownList.DataValueField = "AccTypeID";
                //AdvanceAccTypeDropDownList.DataBind();
               
            }
            catch (Exception)
            {
                WebMessenger.Show(this, "Error While Loading Accomodation Type", "Error", DialogTypes.Error);
                return;
            }
            finally
            {
                objPackage = null;
            }

        }

        private void LoadRoomPlanName()
        {
            objPackage = new TpkPackageMst();
            try
            {
                DataTable dt = objPackage.GetRoomPlanByTpkID(Session["BusDtl"].ToString().Split('|')[0].ToString(), Request.QueryString["TPKID"].ToString());
                RoomPlanDropDownList.Items.Clear();
                RoomPlanDropDownList.Items.Add(new System.Web.UI.WebControls.ListItem("-----Select Room Plan------", "0"));
                RoomPlanDropDownList.DataSource = dt;
                RoomPlanDropDownList.DataValueField = "RoomPlanID";
                RoomPlanDropDownList.DataTextField = "RoomPlanName";
                RoomPlanDropDownList.DataBind();

                AdvanceRoomPlanDropDownList.Items.Clear();
                AdvanceRoomPlanDropDownList.Items.Add(new System.Web.UI.WebControls.ListItem("-----Select Room Plan------", "0"));
                AdvanceRoomPlanDropDownList.DataSource = dt;
                AdvanceRoomPlanDropDownList.DataValueField = "RoomPlanID";
                AdvanceRoomPlanDropDownList.DataTextField = "RoomPlanName";
                AdvanceRoomPlanDropDownList.DataBind();

            }
            catch (Exception)
            {
                WebMessenger.Show(this.Page, "Error Occured While Loading Room Plan Name", "Error", DialogTypes.Error);
                return;
            }
            finally
            {
                objPackage = null;
            }
        }

        public void AutoSuggestVehicleType()
        {
            objPackage = new TpkPackageMst();
            try
            {
                DataTable dt = objPackage.GetVehicleTypeByTpkID(Session["BusDtl"].ToString().Split('|')[0].ToString(), Request.QueryString["TPKID"].ToString());
                string json = Server.HtmlDecode(JsonConvert.SerializeObject(dt));
                VehicleTypeListHiddenField.Value = json;
            }
            catch (Exception ex)
            {
                WebMessenger.Show(this.Page, "Error Occured While Loading VehicleType", "Error", DialogTypes.Error);
                return;
            }
            finally
            {
                objPackage = null;
            }
        }

       
        protected void LoadOverNightDesAcc()
        {
            objPackage = new TpkPackageMst();
            try
            {
                int RecordCount = 0;
                DataTable dt = objPackage.GetDayWiseAccTypeAndAccByTpkID(Request.QueryString["TPKID"].ToString(), Session["BusDtl"].ToString().Split('|')[0].ToString());
                ViewState["AccList"] = dt;
                string[] DistinctDay = { "DayNo", "DestinationName", "DestinationID" };
                DataTable dtDayList = dt.DefaultView.ToTable(true, DistinctDay);
                DestinationWiseAccomodationGridView.DataSource = dtDayList;
                DestinationWiseAccomodationGridView.DataBind();

                foreach (GridViewRow row in DestinationWiseAccomodationGridView.Rows)
                {
                    Int64 DestinationID = Int64.Parse(((Label)row.FindControl("DayDestinationLabel")).Text);
                    DropDownList ddl = (DropDownList)row.FindControl("DayAccTypeDropDownList");
                    DropDownList accddl = (DropDownList)row.FindControl("AccDropDownList");
                    accddl.Items.Add(new System.Web.UI.WebControls.ListItem("-----Select-----", "0"));
                    ddl.Items.Clear();
                    if (dt.Select("DestinationID=" + DestinationID).Length > 0)
                    {
                        DataTable dtNew=  dt.Select("DestinationID=" + DestinationID).CopyToDataTable();
                        string[] DistinctAccType = { "AccTypeID", "AccTypeName" };
                        ddl.DataSource =  dtNew.DefaultView.ToTable(true, DistinctAccType);
                        ddl.DataTextField = "AccTypeName";
                        ddl.DataValueField = "AccTypeID";
                        ddl.Items.Add(new System.Web.UI.WebControls.ListItem("------Select------","0"));
                        ddl.DataBind();
                    }
                }
               

            }
            catch (Exception ex)
            {
                WebMessenger.Show(this, "Unable To Load  Destination Accomadation At this Moment", "Accomation", DialogTypes.Error);
            }
            finally
            {
                objPackage = null;
            }
        }

        protected void DayAccTypeDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ResultTabOpenHiddenField.Value = "";
                string[] DistinctAcc = { "AccName", "AccID" };
                DataTable dt = (DataTable)ViewState["AccList"];
                DropDownList ddl = (DropDownList)sender;
                int iRowIndex = ((ddl).Parent.Parent as GridViewRow).RowIndex;
                if (ddl.SelectedValue != "0")
                {
                    DropDownList Accddl = (DropDownList)DestinationWiseAccomodationGridView.Rows[iRowIndex].FindControl("AccDropDownList");
                    Int64 DestinationID = Int64.Parse(((Label)DestinationWiseAccomodationGridView.Rows[iRowIndex].FindControl("DayDestinationLabel")).Text);
                    Accddl.Items.Clear();
                    if (dt.Select("AccTypeID ='" + ddl.SelectedValue + "'And DestinationID=" + DestinationID.ToString()).Length > 0)
                    {
                        Accddl.DataSource = dt.Select("DestinationID =" + DestinationID + "AND AccTypeID ='" + ddl.SelectedValue + "'").CopyToDataTable().DefaultView.ToTable(true, DistinctAcc) ;
                        Accddl.DataTextField = "AccName";
                        Accddl.DataValueField = "AccID";
                        Accddl.Items.Add(new System.Web.UI.WebControls.ListItem("-----Select-----", "0"));
                        Accddl.DataBind();

                    }

                }
            }
            catch (Exception ex)
            {
                WebMessenger.Show(this, ex.Message, "Error", DialogTypes.Error);
            }
            

        }

        #endregion

        #region GridView Functions
        protected string ItineraryTypeText(string ItineraryType)
        {
            if (ItineraryType == "NA")
            {
                return null;
            }
            else if (ItineraryType == "TR")
            {
                return "Transfer ";
            }
            else
            {
                return "Sight Seen";
            }
        }
        
        protected bool ItineraryTypeVisible(string ItineraryType)
        {
            if (ItineraryType == "NA")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion

        #region Button Click Event
        protected void BacisPackageConfireButton_Click(object sender, EventArgs e)
        {
            
            objPackage = new TpkPackageMst();
            try
            {
                BasicAccPanel.Visible = true;
                AdvanceAccPanel.Visible = false;
                DataSet VehicleCost, AccCost;
                int totalUser = int.Parse(NoOfAdultDropDownList.SelectedValue.ToString()) + int.Parse(NoOfChildDropDownList.SelectedValue.ToString());

                #region Retrieve Data From Database In DataSet
                AccCost = objPackage.GetPackageAccomodationCostDetailsByAccType(Request.QueryString["TPKID"].ToString(), DateTime.Parse(string.Format("{0:dd-MMM-yyyy}", DateTime.Parse(ArrivalDateTextBox.Text.Trim()))), Int64.Parse(AccommodationTypeDropDownList.SelectedValue), Int64.Parse(RoomPlanDropDownList.SelectedValue.ToString()), SelectedRoomListAdultHiddenField.Value, SelectedRoomListChildHiddenField.Value, SelectedVehicleTypeHiddenField.Value, PaxListForVehicleHiddenField.Value, totalUser, Session["BusDtl"].ToString().Split('|')[0].ToString());
                VehicleCost = objPackage.GetPackageItineraryVehicleCostDetails(Request.QueryString["TPKID"].ToString(), DateTime.Parse(string.Format("{0:dd-MMM-yyyy}", DateTime.Parse(ArrivalDateTextBox.Text.Trim()))), SelectedVehicleTypeHiddenField.Value, PaxListForVehicleHiddenField.Value, Session["BusDtl"].ToString().Split('|')[0].ToString(), totalUser);

                #endregion

                #region Check Whether All Destination Rate Is Configured
                string DestinationName = AccCost.Tables[3].Rows[0]["DestinationName"].ToString();
                if (DestinationName.Trim() == "")
                {
                    ResultTabOpenHiddenField.Value = "Open";
                }
                else
                {
                    ResultTabOpenHiddenField.Value = "Close";
                    WebMessenger.Show(this, "Rate For The Package Cannot Be Calculated As Rate For Destination " + DestinationName + " Has  Not Been Configured", "Information", DialogTypes.Information);
                    return;
                }

                #endregion


                #region Optional SightSeen Option
                DataTable dtr = objPackage.GetSupplementPointByTpkID(Request.QueryString["TPKID"].ToString(), DateTime.Parse(string.Format("{0:dd-MMM-yyyy}", DateTime.Parse(ArrivalDateTextBox.Text.Trim()))));
                OptionalGridView.DataSource = dtr;
                OptionalGridView.DataBind();
                ViewState["OptionalDT"] = dtr;
                #endregion

                #region  Accomadation
                DataTable dtAccRate = AccCost.Tables[0];
                DataTable dtAccDis = AccCost.Tables[1];
                string[] TobeDistinct = { "DestinationID", "DestinationName", "RoomPlanName", "AccTypeName","Remarks" };
                DataTable dtAcc = AccCost.Tables[2];
                DataTable dtAccList = AccCost.Tables[2].DefaultView.ToTable(true, TobeDistinct);
                AccomadationGridView.DataSource = dtAccList;
                AccomadationGridView.DataBind();

                foreach (GridViewRow row in AccomadationGridView.Rows)
                {
                    Int64 DestinationID = Int64.Parse(((Label)row.FindControl("DestinationIDLabel")).Text);
                    DataList dr = ((DataList)row.FindControl("AccListDataList"));
                    dr.DataSource = dtAcc.Select("DestinationID=" + DestinationID).CopyToDataTable(); ;
                    dr.DataBind();
                }

                #endregion

                #region Vehicle Dtls
                DataTable dtVehicleCost = VehicleCost.Tables[0];
                DataTable dtVehicleDis = VehicleCost.Tables[1];

                string[] DayDistinct = { "DayNo" };
                DataTable dtDayList = dtVehicleCost.DefaultView.ToTable(true, DayDistinct);
                VehicleGridView.DataSource = dtDayList;
                VehicleGridView.DataBind();

                foreach (GridViewRow row in VehicleGridView.Rows)
                {
                    Int64 DayNo = Int64.Parse(((Label)row.FindControl("DayNoLabel")).Text);
                    GridView gr = ((GridView)row.FindControl("VehicleDtlsGridView"));
                    string[] TypeDistinct = { "ItineraryType" };
                    DataTable drf = dtVehicleCost.Select("DayNo=" + DayNo).CopyToDataTable();
                    gr.DataSource = dtVehicleCost.Select("DayNo=" + DayNo).CopyToDataTable().DefaultView.ToTable(true, TypeDistinct);
                    gr.DataBind();
                    VehicleCountHiddenField.Value = gr.Rows.Count.ToString();
                    foreach (GridViewRow grow in gr.Rows)
                    {
                        string ItineraryType = ((Label)grow.FindControl("OrgItineraryTypeLabel")).Text;
                        GridView grinner = ((GridView)grow.FindControl("VehicleFinalDtlsGridView"));
                        grinner.DataSource = dtVehicleCost.Select("DayNo=" + DayNo + "AND ItineraryType='" + ItineraryType + "'").CopyToDataTable();
                        grinner.DataBind();
                    }
                }

                #endregion

                #region Cost Details

                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("Type", typeof(string)));
                dt.Columns.Add(new DataColumn("TotalAmount", typeof(string)));
                dt.Columns.Add(new DataColumn("Discount", typeof(string)));
                dt.Columns.Add(new DataColumn("AmountAfterDiscount", typeof(string)));


                DataRow drd = dt.NewRow();
                drd["Type"] = "Accomdation Cost For "+ totalUser +" person";
                drd["TotalAmount"] = Decimal.Round(Decimal.Parse(dtAccDis.Rows[0]["TotalCost"].ToString() == "" ? "0.0000" : dtAccDis.Rows[0]["TotalCost"].ToString()), 2);
                drd["Discount"] = Decimal.Round(Decimal.Parse(dtAccDis.Rows[0]["Discount"].ToString() == "" ? "0.0000" : dtAccDis.Rows[0]["Discount"].ToString()), 2);
                drd["AmountAfterDiscount"] = Decimal.Round(Decimal.Parse(dtAccDis.Rows[0]["AmountAfterDiscount"].ToString() == "" ? "0.0000" : dtAccDis.Rows[0]["AmountAfterDiscount"].ToString()), 2);
                dt.Rows.Add(drd);


                DataRow drd2 = dt.NewRow();
                drd2["Type"] = "Vehicle Cost For Entire Package";
                drd2["TotalAmount"] = Decimal.Round(Decimal.Parse(dtVehicleDis.Rows[0]["TotalCost"].ToString() == "" ? "0.0000" : dtVehicleDis.Rows[0]["TotalCost"].ToString()), 2);
                drd2["Discount"] = Decimal.Round(Decimal.Parse(dtVehicleDis.Rows[0]["Discount"].ToString() == "" ? "0.0000" : dtVehicleDis.Rows[0]["Discount"].ToString()), 2);
                drd2["AmountAfterDiscount"] = Decimal.Round(Decimal.Parse(dtVehicleDis.Rows[0]["AmountAfterDiscount"].ToString() == "" ? "0.0000" : dtVehicleDis.Rows[0]["AmountAfterDiscount"].ToString()), 2);
                dt.Rows.Add(drd2);
                RapCostLabel.Text = Decimal.Round(Decimal.Parse(((DataTable)VehicleCost.Tables[2]).Rows[0]["RAPCOST"].ToString()), 2).ToString();
                if (RapCostLabel.Text == "0.00")
                {
                    RapCostPanel.Visible = false;
                }
                else
                {
                    RapCostPanel.Visible = true;
                }
                CostDetailsGridView.DataSource = dt;
                CostDetailsGridView.DataBind();
                TotalAmountLabel.Text = Decimal.Round(Decimal.Parse(dt.Rows[0]["AmountAfterDiscount"].ToString()) + Decimal.Parse(dt.Rows[1]["AmountAfterDiscount"].ToString())).ToString();
                if (VehicleCost.Tables[3].Rows.Count > 0)
                {
                    PackageDiscountLabel.Text = ((Decimal.Parse(VehicleCost.Tables[3].Rows[0]["DiscountPercent"].ToString()) * decimal.Parse(TotalAmountLabel.Text.Trim())) / 100).ToString();
                }
                if (PackageDiscountLabel.Text == "0.00")
                {
                    PackageDiscountPanel.Visible = false;
                }
                else
                {
                    PackageDiscountPanel.Visible = true;
                }
                if (VehicleCost.Tables[4].Rows[0]["AdditionalCost"].ToString() != "")
                {
                    PackageAdditionalCostPanel.Visible = true;
                    PackageAdditonialRemarksLabel.Text = VehicleCost.Tables[4].Rows[0]["AdditionalRemarks"].ToString();
                    PackageAdditionlCostLabel.Text = decimal.Round(decimal.Parse(VehicleCost.Tables[4].Rows[0]["AdditionalCost"].ToString()), 2).ToString();
                }
                else
                {
                    PackageAdditionalCostPanel.Visible = false;
                    PackageAdditonialRemarksLabel.Text = string.Empty;
                    PackageAdditionlCostLabel.Text = "0.00";
                }
                GrandTotalAmountLabel.Text = (decimal.Parse(TotalAmountLabel.Text) - decimal.Parse(PackageDiscountLabel.Text) + decimal.Parse(PackageAdditionlCostLabel.Text.Trim())+decimal.Parse(RapCostLabel.Text.Trim())).ToString();
               // AmountInWordsLabel.Text = retWord(int.Parse(GrandTotalAmountLabel.Text.Split('.')[0].ToString()));
                #endregion

                #region Configuration Details
                ArrivalDateLabel.Text = "Arrival Date Is : "+ArrivalDateTextBox.Text.Trim();
                AdultNumberLabel.Text = "No Of Adult Is : "+ NoOfAdultDropDownList.SelectedValue;
                ChildNumberLabel.Text = "No Of Child Is : " + NoOfChildDropDownList.SelectedValue;
                InfantNumberLabel.Text = "No Of Infant Is : " + NoOfInfantDropDownList.SelectedValue;
                #endregion

            }
            catch (Exception ex)
            {
                WebMessenger.Show(this, ex.Message, "Error", DialogTypes.Error);
                this.RegisterJS("$.fancybox.close();");
            }
            finally
            {
                objPackage = null;
            }
        }

        protected void AdvancePackageConfireButton_Click(object sender, EventArgs e)
        {
            objPackage = new TpkPackageMst();
            int TotalGuest = 0;
            TotalGuest = TotalGuest + int.Parse(AdvanceNoOfAdultDropDownList.SelectedValue);
            TotalGuest = TotalGuest + int.Parse(AdvanceNoOfChildDropDownList.SelectedValue);
            BasicAccPanel.Visible = false;
            AdvanceAccPanel.Visible = true;
            try
            {
                #region Validation 
                SelectAccomadationListHiddenField.Value = null;
                DayCountHiddenField.Value = null;
                foreach (GridViewRow row in DestinationWiseAccomodationGridView.Rows)
                {
                    string DestinationName = ((Label)row.FindControl("DestinationNameLabel")).Text.Trim();
                    DropDownList ddl = (DropDownList)row.FindControl("AccDropDownList");
                    Label DayNo = (Label)row.FindControl("DayNoLabel");
                    if (ddl.SelectedValue == "0")
                    {
                        WebMessenger.Show(this, "Accomadation Name Is Required For Destination " + DestinationName, "Information", DialogTypes.Information);
                        return;
                    }
                    SelectAccomadationListHiddenField.Value = SelectAccomadationListHiddenField.Value + ddl.SelectedValue + ",";
                    DayCountHiddenField.Value = DayCountHiddenField.Value +  "1,";
                }

                #endregion

                #region Retrieve Data From Database In DataSet
                ResultTabOpenHiddenField.Value = "Open";
                DataSet AccCost = objPackage.GetPackageAccomodationCostDetailsByAccID(Request.QueryString["TPKID"].ToString(), DateTime.Parse(string.Format("{0:dd-MMM-yyyy}", DateTime.Parse(AdvanceArrivalDateTextBox.Text.Trim()))), SelectAccomadationListHiddenField.Value, DayCountHiddenField.Value, AdvanceSelectedRoomListAdultHiddenField.Value, AdvanceSelectedRoomListChildHiddenField.Value, Int64.Parse(AdvanceRoomPlanDropDownList.SelectedValue), Session["BusDtl"].ToString().Split('|')[0].ToString());
                DataSet VehicleCost = objPackage.GetPackageItineraryVehicleCostDetails(Request.QueryString["TPKID"].ToString(), DateTime.Parse(string.Format("{0:dd-MMM-yyyy}", DateTime.Parse(AdvanceArrivalDateTextBox.Text.Trim()))), SelectedVehicleTypeHiddenField.Value, PaxListForVehicleHiddenField.Value, Session["BusDtl"].ToString().Split('|')[0].ToString(), TotalGuest);

                #endregion

                #region Optional SightSeen Option
                DataTable dtr = objPackage.GetSupplementPointByTpkID(Request.QueryString["TPKID"].ToString(), DateTime.Parse(string.Format("{0:dd-MMM-yyyy}", DateTime.Parse(AdvanceArrivalDateTextBox.Text.Trim()))));
                OptionalGridView.DataSource = dtr;
                OptionalGridView.DataBind();
                ViewState["OptionalDT"] = dtr;
                #endregion

                #region  Accomadation

                DataTable dtAccDis = AccCost.Tables[1];
                AdvanceAccPanel.Visible = true;
                DayAccDtlsGridView.DataSource = AccCost.Tables[2];
                DayAccDtlsGridView.DataBind();

                #endregion

                #region Vehicle

                DataTable dtVehicleCost = VehicleCost.Tables[0];
                DataTable dtVehicleDis = VehicleCost.Tables[1];

                string[] DayDistinct = { "DayNo" };
                DataTable dtDayList = dtVehicleCost.DefaultView.ToTable(true, DayDistinct);
                VehicleGridView.DataSource = dtDayList;
                VehicleGridView.DataBind();

                foreach (GridViewRow row in VehicleGridView.Rows)
                {
                    Int64 DayNo = Int64.Parse(((Label)row.FindControl("DayNoLabel")).Text);
                    GridView gr = ((GridView)row.FindControl("VehicleDtlsGridView"));
                    string[] TypeDistinct = { "ItineraryType" };
                    gr.DataSource = dtVehicleCost.Select("DayNo=" + DayNo).CopyToDataTable().DefaultView.ToTable(true, TypeDistinct);
                    gr.DataBind();
                    VehicleCountHiddenField.Value = gr.Rows.Count.ToString();
                    foreach (GridViewRow grow in gr.Rows)
                    {
                        string ItineraryType = ((Label)grow.FindControl("OrgItineraryTypeLabel")).Text;
                        GridView grinner = ((GridView)grow.FindControl("VehicleFinalDtlsGridView"));
                        grinner.DataSource = dtVehicleCost.Select("DayNo=" + DayNo + "AND ItineraryType='" + ItineraryType + "'").CopyToDataTable();
                        grinner.DataBind();
                    }
                }

                #endregion

                #region Package Additional Cost

                if (VehicleCost.Tables[4].Rows[0]["AdditionalCost"].ToString()!="" )
                {
                    PackageAdditionalCostPanel.Visible = true;
                    PackageAdditonialRemarksLabel.Text = VehicleCost.Tables[4].Rows[0]["AdditionalRemarks"].ToString();
                    PackageAdditionlCostLabel.Text = decimal.Round(decimal.Parse(VehicleCost.Tables[4].Rows[0]["AdditionalCost"].ToString()),2).ToString();
                }
                else
                {
                    PackageAdditionalCostPanel.Visible = false;
                    PackageAdditonialRemarksLabel.Text = string.Empty;
                    PackageAdditionlCostLabel.Text = "0.00";
                }
                #endregion



                #region Cost Details

                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("Type", typeof(string)));
                dt.Columns.Add(new DataColumn("TotalAmount", typeof(string)));
                dt.Columns.Add(new DataColumn("Discount", typeof(string)));
                dt.Columns.Add(new DataColumn("AmountAfterDiscount", typeof(string)));


                DataRow drd = dt.NewRow();
                drd["Type"] = "Accomadation  Cost For "+ TotalGuest +" Person";
                drd["TotalAmount"] = Decimal.Round(Decimal.Parse(dtAccDis.Rows[0]["TotalCost"].ToString() == "" ? "0.0000" : dtAccDis.Rows[0]["TotalCost"].ToString()), 2);
                drd["Discount"] = Decimal.Round(Decimal.Parse(dtAccDis.Rows[0]["Discount"].ToString() == "" ? "0.0000" : dtAccDis.Rows[0]["Discount"].ToString()), 2);
                drd["AmountAfterDiscount"] = Decimal.Round(Decimal.Parse(dtAccDis.Rows[0]["AmountAfterDiscount"].ToString() == "" ? "0.0000" : dtAccDis.Rows[0]["AmountAfterDiscount"].ToString()), 2);
                dt.Rows.Add(drd);


                DataRow drd2 = dt.NewRow();
                drd2["Type"] = "Vehicle Cost For Entire Package";
                drd2["TotalAmount"] = Decimal.Round(Decimal.Parse(dtVehicleDis.Rows[0]["TotalCost"].ToString() == "" ? "0.0000" : dtVehicleDis.Rows[0]["TotalCost"].ToString()), 2);
                drd2["Discount"] = Decimal.Round(Decimal.Parse(dtVehicleDis.Rows[0]["Discount"].ToString() == "" ? "0.0000" : dtVehicleDis.Rows[0]["Discount"].ToString()), 2);
                drd2["AmountAfterDiscount"] = Decimal.Round(Decimal.Parse(dtVehicleDis.Rows[0]["AmountAfterDiscount"].ToString() == "" ? "0.0000" : dtVehicleDis.Rows[0]["AmountAfterDiscount"].ToString()), 2);
                dt.Rows.Add(drd2);

                RapCostLabel.Text = Decimal.Round(Decimal.Parse(((DataTable)VehicleCost.Tables[2]).Rows[0]["RAPCOST"].ToString()), 2).ToString();
                if (RapCostLabel.Text.Trim() == "0.00")
                {
                    RapCostPanel.Visible = false;
                }
                else
                {
                    RapCostPanel.Visible = true;
                }
                CostDetailsGridView.DataSource = dt;
                CostDetailsGridView.DataBind();
                TotalAmountLabel.Text = Decimal.Round(Decimal.Parse(dt.Rows[0]["AmountAfterDiscount"].ToString()) + Decimal.Parse(dt.Rows[1]["AmountAfterDiscount"].ToString())).ToString();
                if (VehicleCost.Tables[3].Rows.Count > 0)
                {
                    PackageDiscountLabel.Text = ((Decimal.Parse(VehicleCost.Tables[3].Rows[0]["DiscountPercent"].ToString()) * decimal.Parse(TotalAmountLabel.Text.Trim())) / 100).ToString();
                }
                if (PackageDiscountLabel.Text.Trim() == "0.00")
                {
                    PackageDiscountPanel.Visible = false;
                }
                else
                {
                    PackageDiscountPanel.Visible = true;
                }
                GrandTotalAmountLabel.Text = (decimal.Parse(TotalAmountLabel.Text) - decimal.Parse(PackageDiscountLabel.Text) + Decimal.Parse(RapCostLabel.Text.Trim())+decimal.Parse(PackageAdditionlCostLabel.Text.Trim())).ToString();
                
               // AmountInWordsLabel.Text = retWord(int.Parse(GrandTotalAmountLabel.Text.Split('.')[0].ToString()));
                #endregion


                #region Configuration Details
                ArrivalDateLabel.Text = "Arrival Date Is : " + AdvanceArrivalDateTextBox.Text.Trim();
                AdultNumberLabel.Text = "No Of Adult Is : " + AdvanceNoOfAdultDropDownList.SelectedValue;
                ChildNumberLabel.Text = "No Of Child Is : " + AdvanceNoOfChildDropDownList.SelectedValue;
                InfantNumberLabel.Text = "No Of Infant Is : " + AdvanceNoOfInfantDropDownList.SelectedValue;
                #endregion
            }
            catch (Exception ex)
            {
                WebMessenger.Show(this.Page, ex.Message, "Error", DialogTypes.Error);
                this.RegisterJS("$.fancybox.close();");
            }
            finally
            {
                objPackage = null;
            }
        }

        protected void SendMailButton_Click(object sender, EventArgs e)
        {
            if (MailAddressHiddenField.Value.Trim() == "")
            {
                WebMessenger.Show(this, "Email ID Is Required ", "Information ", DialogTypes.Information);
                return;
            }
            else if (MessageHiddenField.Value.Trim() == "")
            {
                WebMessenger.Show(this, "Message  Is Required ", "Information ", DialogTypes.Information);
                return;
            }
            else if (ValidatorClass.IsValidEmail(MailAddressHiddenField.Value.Trim()) == false)
            {
                WebMessenger.Show(this, "Invalid EmailID ", "Information ", DialogTypes.Information);
                return;
            }
            ConvertToPdfDeatils();
        }

        #endregion

        #region Convert To Pdf ITextSharp 

        public override void VerifyRenderingInServerForm(Control control)
        //  this Event is must for Rendring
        {

        }

        protected void ConvertToPdfDeatils()
        {
            Document document = new Document();
            objPackage = new TpkPackageMst();
            objTpkTransactionDtls = new TpkTransactionDtls();
            try
            {
                iTextSharp.text.Font totalFont = FontFactory.GetFont("Helvetica", 20, new BaseColor(203, 57, 4));
                iTextSharp.text.Font headerFont = FontFactory.GetFont("Helvetica", 20, new BaseColor(27, 150, 186));
                MemoryStream memoryStream = new MemoryStream();
                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);

                document.Open();

                #region Logo
                iTextSharp.text.Image logo = null;
                logo = iTextSharp.text.Image.GetInstance(Server.MapPath(@"~/App_Themes/AdminThemeDefault/Images/Logo.jpg"));
                document.Add(logo);

                #endregion

                #region Table for Package Photo Gallery Only Three Photos Allowed

                string header = "<div style='background-color: #5C5C5C; height: 40px; padding-left: 10px; font-size: 17px;font-weight: bold; width: 625px; line-height: 36px; margin: 10px;'> <div style='height: 26px; margin: 5px 0 9px; width: 26px; float: left;'></div><div style='height: 40px; width: 599px; float: left; line-height: 36px;'>" + PackageNameLabel.Text.Trim() + PackageDurationLabel.Text.Trim() + "</div> </div>";
                List<IElement> headerhtmlarraylist = HTMLWorker.ParseToList(new StringReader(header), null);
                for (int k = 0; k < headerhtmlarraylist.Count; k++)
                {
                    document.Add((IElement)headerhtmlarraylist[k]);
                }
                string html = HttpUtility.HtmlDecode(MailQueryHiddenField.Value);
                DataTable dt = (DataTable)Session["Image"];

                //Table for Package Photo Gallery Only Three Photos Allowed

                PdfPTable table = new PdfPTable(3);
                table.SpacingAfter = 15;
                PdfPCell cell1 = new PdfPCell();
                PdfPCell cell2 = new PdfPCell();
                PdfPCell cell3 = new PdfPCell();

                iTextSharp.text.Image gif1 = null, gif2 = null, gif3 = null;
                Byte[] bytes1 = (Byte[])dt.Rows[0]["PhotoThumb"];
                Byte[] bytes2 = (Byte[])dt.Rows[1]["PhotoThumb"];
                Byte[] bytes3 = (Byte[])dt.Rows[2]["PhotoThumb"];
                gif1 = iTextSharp.text.Image.GetInstance(bytes1);
                gif2 = iTextSharp.text.Image.GetInstance(bytes2);
                gif3 = iTextSharp.text.Image.GetInstance(bytes3);

                cell1.AddElement(gif1);
                cell2.AddElement(gif2);
                cell3.AddElement(gif3);
                table.AddCell(cell1);
                table.AddCell(cell2);
                table.AddCell(cell3);

                document.Add(table);
                #endregion

                #region Package Details

                string itinearyHeader = "<div style='width: 625px;height:30px; margin: 0px 10px 10px 5px; font-weight: bold; font-size: 14pt;color: #1b96ba; height: 25px;'> Tour Package Details</div>";
                List<IElement> itineararyheaderhtmlarraylist = HTMLWorker.ParseToList(new StringReader(itinearyHeader), null);
                for (int k = 0; k < itineararyheaderhtmlarraylist.Count; k++)
                {
                    document.Add((IElement)itineararyheaderhtmlarraylist[k]);
                }

                PdfPTable tablePackage = new PdfPTable(2);
                tablePackage.DefaultCell.Border = 0;
                tablePackage.SpacingBefore = 20;
                tablePackage.SpacingAfter = 15;
                DataTable dtPackage = (DataTable)ViewState["PackageDetails"];
                foreach (DataRow dr in dtPackage.Rows)
                {
                    PdfPCell Packagecell1 = new PdfPCell();
                    Packagecell1.Border = 0;
                    PdfPCell Packagecell2 = new PdfPCell();
                    Packagecell2.Border = 0;
                    PdfPCell Packagecell3 = new PdfPCell();
                    Packagecell3.Border = 0;
                    PdfPCell Packagecell4 = new PdfPCell();
                    Packagecell4.Border = 0;
                    PdfPCell Packagecell5 = new PdfPCell();
                    Packagecell5.Border = 0;
                    PdfPCell Packagecell6 = new PdfPCell();
                    Packagecell6.Border = 0;
                    PdfPCell Packagecell7 = new PdfPCell();
                    Packagecell7.Border = 0;
                    PdfPCell Packagecell8 = new PdfPCell();
                    Packagecell8.Border = 0;
                    Packagecell1.AddElement(new Chunk("Package Description"));
                    Packagecell2.AddElement(new Chunk(dr["TpkDesc"].ToString()));
                    tablePackage.AddCell(Packagecell1);
                    tablePackage.AddCell(Packagecell2);

                    Packagecell3.AddElement(new Chunk("Destinations Covered"));
                    Packagecell4.AddElement(new Chunk(dr["TpkDestCovered"].ToString()));
                    tablePackage.AddCell(Packagecell3);
                    tablePackage.AddCell(Packagecell4);

                    Packagecell5.AddElement(new Chunk("PickUp Point"));
                    Packagecell6.AddElement(new Chunk(dr["TpkPickUpPoint"].ToString()));
                    tablePackage.AddCell(Packagecell5);
                    tablePackage.AddCell(Packagecell6);

                    Packagecell7.AddElement(new Chunk("Drop Point"));
                    Packagecell8.AddElement(new Chunk(dr["TpkDropPoint"].ToString()));
                    tablePackage.AddCell(Packagecell7);
                    tablePackage.AddCell(Packagecell8);
                }
                document.Add(tablePackage);
                #endregion

                document.NewPage();

                #region  Day-Wise Itinerary Details

                document.Add(new Paragraph("Day - Wise Itinerary Details", headerFont));
                document.Add(new Paragraph("\n", headerFont));

                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                ItineraryDetailsFGridView.RenderControl(hw);
                StringReader sr = new StringReader(sw.ToString());

                HTMLWorker htmlparser = new HTMLWorker(document);
                htmlparser.Parse(sr);

                writer.CloseStream = false;
                #endregion

                document.NewPage();

                #region Supplement Points

                if (SelectedSupplementHiddenField.Value != "")
                {
                    document.Add(new Paragraph("Supplement Points Selected  In Our Package", headerFont));
                    document.Add(new Paragraph("\n", headerFont));
                    int length = SelectedSupplementHiddenField.Value.Split(',').Length;
                    DataTable dtOP = (DataTable)ViewState["OptionalDT"];
                    PdfPTable tableOpt = new PdfPTable(2);
                    tableOpt.DefaultCell.Border = 1;
                    tableOpt.AddCell(new Paragraph(new Chunk("Supplement Point")));
                    tableOpt.AddCell(new Paragraph(new Chunk("Supplement Description")));
                    for (int i = 1; i <= length - 1; i++)
                    {

                        DataTable hdt = dtOP.Select("SupplementID='" + SelectedSupplementHiddenField.Value.Split(',')[i] + "'").CopyToDataTable();
                        tableOpt.AddCell(new Paragraph(new Chunk(hdt.Rows[0]["SupplementName"].ToString())));
                        tableOpt.AddCell(new Paragraph(new Chunk(hdt.Rows[0]["SupplementDesc"].ToString())));
                    }
                    document.Add(tableOpt);
                }
                #endregion

                #region Accomadation

                document.Add(new Paragraph("Hotels Provided In Our Package", headerFont));
                document.Add(new Paragraph("\n", headerFont));

                DataTable dtAcc = new DataTable();
                int totalUser = 0;
                DateTime arrivalDate = DateTime.Now;
                if (BasicAccPanel.Visible == true)
                {
                    arrivalDate = DateTime.Parse(ArrivalDateTextBox.Text.Trim());
                    totalUser = int.Parse(NoOfAdultDropDownList.SelectedValue.ToString()) + int.Parse(NoOfChildDropDownList.SelectedValue.ToString());
                    dtAcc = objPackage.GetPackageAccomodationCostDetailsByAccType(Request.QueryString["TPKID"].ToString(), DateTime.Parse(string.Format("{0:dd-MMM-yyyy}", DateTime.Parse(ArrivalDateTextBox.Text.Trim()))), Int64.Parse(AccommodationTypeDropDownList.SelectedValue), Int64.Parse(RoomPlanDropDownList.SelectedValue.ToString()), SelectedRoomListAdultHiddenField.Value, SelectedRoomListChildHiddenField.Value, SelectedVehicleTypeHiddenField.Value, PaxListForVehicleHiddenField.Value, totalUser, Session["BusDtl"].ToString().Split('|')[0].ToString()).Tables[2];
                    string[] TobeDistinct = { "DestinationID", "DestinationName", "RoomPlanName", "AccTypeName", "Remarks" };
                    DataTable dtAccList = dtAcc.DefaultView.ToTable(true, TobeDistinct);
                    foreach (DataRow dr in dtAccList.Rows)
                    {
                        PdfPTable tableAcc = new PdfPTable(2);
                        tableAcc.DefaultCell.Border = 0;
                        Int64 destinationID = Int64.Parse(dr["DestinationID"].ToString());
                        PdfPCell AcccellDes = new PdfPCell();
                        PdfPCell AcccellPlan = new PdfPCell();
                        AcccellDes.AddElement(new Chunk("Destination :" + dr["DestinationName"].ToString() + "(" + dr["AccTypeName"].ToString() + ")"));
                        tableAcc.AddCell(AcccellDes);
                        AcccellPlan.AddElement(new Chunk("Plan Name :" + dr["RoomPlanName"].ToString()));
                        tableAcc.AddCell(AcccellPlan);
                        DataTable dtAccLoop = dtAcc.Select("DestinationID =" + destinationID).CopyToDataTable();
                        document.Add(tableAcc);
                        PdfPTable dtAccLoopPDF = new PdfPTable(3);
                        foreach (DataRow drd in dtAccLoop.Rows)
                        {
                            PdfPCell cellPdf1 = new PdfPCell();
                            PdfPCell cellPdf2 = new PdfPCell();
                            PdfPCell cellPdf3 = new PdfPCell();
                            iTextSharp.text.Image gifAcc = null;
                            gifAcc = iTextSharp.text.Image.GetInstance((Byte[])drd["ThumbImage"]);
                            cellPdf1.AddElement(gifAcc);
                            dtAccLoopPDF.AddCell(cellPdf1);
                            cellPdf2.AddElement(new Chunk(drd["AccName"].ToString()));
                            dtAccLoopPDF.AddCell(cellPdf2);
                            cellPdf3.AddElement(new Chunk(drd["AccAddress"].ToString()));
                            dtAccLoopPDF.AddCell(cellPdf3);
                        }
                        document.Add(dtAccLoopPDF);
                    }
                }
                else if (AdvanceAccPanel.Visible == true)
                {
                    arrivalDate = DateTime.Parse(AdvanceArrivalDateTextBox.Text.Trim());
                    totalUser = int.Parse(AdvanceNoOfAdultDropDownList.SelectedValue.ToString()) + int.Parse(AdvanceNoOfChildDropDownList.SelectedValue.ToString());
                    //dtAcc = objPackage.GetPackageAccomodationCostDetailsByAccID(Request.QueryString["TPKID"].ToString(), DateTime.Parse(string.Format("{0:dd-MMM-yyyy}", DateTime.Parse(AdvanceArrivalDateTextBox.Text.Trim()))), SelectAccomadationListHiddenField.Value, DayCountHiddenField.Value, AdvanceSelectedRoomListAdultHiddenField.Value, AdvanceSelectedRoomListChildHiddenField.Value, Int64.Parse(AdvanceRoomPlanDropDownList.SelectedValue), Session["BusinessID"].ToString()).Tables[2];
                    foreach (GridViewRow gr in DayAccDtlsGridView.Rows)
                    {
                        PdfPTable grDayTable = new PdfPTable(2);
                        grDayTable.DefaultCell.Border = 0;
                        grDayTable.AddCell(new Paragraph("Day " + ((Label)gr.FindControl("AccDayNoLabel")).Text.ToString()));
                        grDayTable.AddCell(new Paragraph("Plan Name :" + ((Label)gr.FindControl("PlanNameLabel")).Text.ToString()));
                        document.Add(grDayTable);
                        PdfPTable grAccTable = new PdfPTable(3);
                        iTextSharp.text.Image gifAcc = iTextSharp.text.Image.GetInstance((Byte[])Convert.FromBase64String(((Label)gr.FindControl("AccImageLabel")).Text));

                        grAccTable.AddCell(gifAcc);
                        grAccTable.AddCell(new Paragraph("Accommadation Name :" + ((Label)gr.FindControl("AccNameLabel")).Text.ToString()));
                        grAccTable.AddCell(new Paragraph("Details :" + ((Label)gr.FindControl("AccDescLabel")).Text.ToString()));
                        document.Add(grAccTable);
                    }
                }

                #endregion

                document.NewPage();

                #region Day -Wise Vehicle Details

                document.Add(new Paragraph("Day - Wise Vehicle Details", headerFont));
                document.Add(new Paragraph("\n", headerFont));

                DataTable dtVeh = objPackage.GetPackageItineraryVehicleCostDetails(Request.QueryString["TPKID"].ToString(), DateTime.Parse(string.Format("{0:dd-MMM-yyyy}", arrivalDate)), SelectedVehicleTypeHiddenField.Value, PaxListForVehicleHiddenField.Value, Session["BusDtl"].ToString().Split('|')[0].ToString(), totalUser).Tables[0];
                string[] DayDistinct = { "DayNo" };
                DataTable dtDayList = dtVeh.DefaultView.ToTable(true, DayDistinct);

                foreach (DataRow row in dtDayList.Rows)
                {
                    PdfPTable tableDay = new PdfPTable(1);
                    PdfPCell tbCell = new PdfPCell();
                    int DayNo = int.Parse(row["DayNo"].ToString());
                    tbCell.AddElement(new Chunk("Day " + row["DayNo"].ToString()));
                    tableDay.AddCell(tbCell);
                    document.Add(tableDay);
                    string[] TypeDistinct = { "ItineraryType" };
                    DataTable dtType = dtVeh.Select("DayNo=" + DayNo).CopyToDataTable().DefaultView.ToTable(true, TypeDistinct);
                    foreach (DataRow row1 in dtType.Rows)
                    {
                        string itinearyType = row1["ItineraryType"].ToString();
                        PdfPTable tableType = new PdfPTable(1);
                        PdfPCell tabCell = new PdfPCell();
                        if (row1["ItineraryType"].ToString() == "SS")
                        {
                            tabCell.AddElement(new Chunk("Sight Seen"));
                        }
                        else if (row1["ItineraryType"].ToString() == "TR")
                        {
                            tabCell.AddElement(new Chunk("Transfer"));
                        }
                        else
                        {
                            tabCell.AddElement(new Chunk(""));
                        }
                        tableType.AddCell(tabCell);
                        document.Add(tableType);
                        DataTable dtVehicle = dtVeh.Select("DayNo=" + DayNo + "AND ItineraryType='" + itinearyType + "'").CopyToDataTable();
                        PdfPTable vehicleTable = new PdfPTable(3);
                        foreach (DataRow row2 in dtVehicle.Rows)
                        {
                            PdfPCell cellPdf1 = new PdfPCell();
                            PdfPCell cellPdf2 = new PdfPCell();
                            PdfPCell cellPdf3 = new PdfPCell();
                            iTextSharp.text.Image gifAcc = null;
                            gifAcc = iTextSharp.text.Image.GetInstance((Byte[])row2["VehicleImgThumb"]);
                            cellPdf1.AddElement(gifAcc);
                            vehicleTable.AddCell(cellPdf1);
                            cellPdf2.AddElement(new Chunk("VehicleType   :" + row2["VehicleName"].ToString()));
                            vehicleTable.AddCell(cellPdf2);
                            cellPdf3.AddElement(new Chunk("Remarks  :" + row2["Remarks"].ToString()));
                            vehicleTable.AddCell(cellPdf3);
                        }
                        document.Add(vehicleTable);
                    }
                }

                #endregion

                document.NewPage();

                #region Cost Details

                document.Add(new Paragraph("Cost Details", headerFont));

                document.Add(new Paragraph("\n", headerFont));



                PdfPTable headerTable = new PdfPTable(2);
                headerTable.DefaultCell.Border = 0;
                headerTable.AddCell(new Paragraph("Particulars"));
                headerTable.AddCell(new Paragraph("Total Amount"));
                //document.Add(headerTable);
                foreach (GridViewRow rw in CostDetailsGridView.Rows)
                {
                    headerTable.AddCell(new Paragraph(((Label)rw.FindControl("TypeLabel")).Text));
                    headerTable.AddCell(new Paragraph(((Label)rw.FindControl("AmountAfterDiscountLabel")).Text));

                }
                document.Add(headerTable);


                PdfPTable costTable = new PdfPTable(2);

                costTable.DefaultCell.Border = 0;
                PdfPCell RapCost = new PdfPCell();
                RapCost.Border = 0;
                RapCost.HorizontalAlignment = Element.ALIGN_RIGHT;
                RapCost.AddElement(new Paragraph(RapCostLabel.Text));
                costTable.AddCell(new Paragraph("Rap Cost: Rs."));
                costTable.AddCell(RapCost);

                PdfPCell PakAddCost = new PdfPCell();
                PakAddCost.Border = 0;
                PakAddCost.HorizontalAlignment = Element.ALIGN_RIGHT;
                PakAddCost.AddElement(new Paragraph(PackageAdditionlCostLabel.Text));
                costTable.AddCell(new Paragraph("Extras/Additional Cost Rs."));
                costTable.AddCell(PakAddCost);

                PdfPCell PakDis = new PdfPCell();
                PakDis.Border = 0;
                PakDis.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                PakDis.AddElement(new Paragraph(PackageDiscountLabel.Text.Trim()));
                costTable.AddCell(new Paragraph("Package Discount: Rs."));
                costTable.AddCell(PakDis);

                PdfPCell PakSupp = new PdfPCell();
                PakSupp.Border = 0;
                PakSupp.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                PakSupp.AddElement(new Paragraph(AdditonalCostHiddenField.Value.Trim()));
                costTable.AddCell(new Paragraph("Supplement Points Cost: Rs."));
                costTable.AddCell(PakSupp);

                PdfPCell grandTotal = new PdfPCell();
                grandTotal.Border = 0;
                grandTotal.HorizontalAlignment = Element.ALIGN_RIGHT;
                grandTotal.AddElement(new Paragraph(GrandTotalHiddenField.Value.Trim(), totalFont));
                costTable.AddCell(new Paragraph("Grand Total: Rs."));
                costTable.AddCell(grandTotal);

                document.Add(costTable);
                #endregion

                document.NewPage();
                #region  Business Information
                DataTable dP = (DataTable)ViewState["PackageDetails"];
                string ContactHeader = "<div style='width: 625px;height:30px; margin: 0px 10px 10px 5px; font-weight: bold; font-size: 14pt;color: #1b96ba; height: 25px;'> Contact Details </div>";
                List<IElement> contactheaderhtmlarraylist = HTMLWorker.ParseToList(new StringReader(ContactHeader), null);
                for (int k = 0; k < contactheaderhtmlarraylist.Count; k++)
                {
                    document.Add((IElement)contactheaderhtmlarraylist[k]);
                }


                PdfPTable dptable = new PdfPTable(2);
                if (dP.Rows.Count > 0)
                {
                    PdfPCell dpCell1 = new PdfPCell(new Phrase("Travel Agent Name"));
                    dpCell1.Border = 0;
                    PdfPCell dpCell2 = new PdfPCell(new Phrase(dP.Rows[0]["BusName"].ToString()));
                    dpCell2.Border = 0;
                    PdfPCell dpCell3 = new PdfPCell(new Phrase("Address"));
                    PdfPCell dpCell4 = new PdfPCell(new Phrase(dP.Rows[0]["BusAddress"].ToString() + dP.Rows[0]["BusCity"].ToString()));
                    PdfPCell dpCell5 = new PdfPCell(new Phrase("Website: "));
                    PdfPCell dpCell6 = new PdfPCell(new Phrase(dP.Rows[0]["BusWebsite"].ToString()));
                    PdfPCell dpCell7 = new PdfPCell(new Phrase("Contact Person Name "));
                    PdfPCell dpCell8 = new PdfPCell(new Phrase(dP.Rows[0]["ContactPersonName"].ToString()));
                    PdfPCell dpCell9 = new PdfPCell(new Phrase("Contact Number"));
                    PdfPCell dpCell10 = new PdfPCell(new Phrase(dP.Rows[0]["ContactMobileNo"].ToString()));
                    dpCell3.Border = 0;
                    dpCell4.Border = 0;
                    dpCell5.Border = 0;
                    dpCell6.Border = 0;
                    dpCell7.Border = 0;
                    dpCell8.Border = 0;
                    dpCell9.Border = 0;
                    dpCell10.Border = 0;
                    dptable.AddCell(dpCell1);
                    dptable.AddCell(dpCell2);
                    dptable.AddCell(dpCell3);
                    dptable.AddCell(dpCell4);
                    dptable.AddCell(dpCell5);
                    dptable.AddCell(dpCell6);
                    dptable.AddCell(dpCell7);
                    dptable.AddCell(dpCell8);
                    dptable.AddCell(dpCell9);
                    dptable.AddCell(dpCell10);
                    dptable.SpacingBefore = 20;
                    document.Add(dptable);
                    document.Close();
                }
                #endregion

                memoryStream.Position = 0;
                string TransactionID = null;
                if (AdvanceAccPanel.Visible == true)
                {
                    objTpkTransactionDtls.AddTpkTransactionDtls(DateTime.Now, 1, Request.QueryString["TPKID"].ToString(), CustomerNameHiddenField.Value, MailAddressHiddenField.Value, CustomerNumberHiddenField.Value, DateTime.Parse(AdvanceArrivalDateTextBox.Text.Trim()), Int16.Parse(AdvanceNoOfAdultDropDownList.SelectedValue), Int16.Parse(AdvanceNoOfChildDropDownList.SelectedValue), Int16.Parse(AdvanceNoOfInfantDropDownList.SelectedValue.ToString()), 1, 2, decimal.Parse(GrandTotalHiddenField.Value), Page.User.Identity.Name, ref TransactionID);
                }
                else
                {
                    objTpkTransactionDtls.AddTpkTransactionDtls(DateTime.Now, 1, Request.QueryString["TPKID"].ToString(), CustomerNameHiddenField.Value, MailAddressHiddenField.Value, CustomerNumberHiddenField.Value, DateTime.Parse(ArrivalDateTextBox.Text.Trim()), Int16.Parse(NoOfAdultDropDownList.SelectedValue), Int16.Parse(NoOfChildDropDownList.SelectedValue), Int16.Parse(NoOfInfantDropDownList.SelectedValue.ToString()), 1, 1, decimal.Parse(GrandTotalHiddenField.Value), Page.User.Identity.Name, ref TransactionID);
                }

                FileStream file = new FileStream(Server.MapPath(@"~/ConfiguredPackages/" + TransactionID + ".pdf"), FileMode.Create, FileAccess.Write);
                memoryStream.WriteTo(file);
                file.Close();



                SendEmail.SendMail(MailAddressHiddenField.Value, "eTravel", MessageHiddenField.Value, new Attachment(memoryStream, TransactionID + ".pdf"));
                WebMessenger.Show(this, "Tour Package Details Email To The Client Successfully", "Mail Sent Successfully", DialogTypes.Success);
            }
            catch (SmtpFailedRecipientException se)
            {
                WebMessenger.Show(this, "Unable To Send Mail At This Moment", "Error", DialogTypes.Error);
            }

            catch (Exception ex)
            {
                WebMessenger.Show(this, "Error While Sending Mail ,Unable To Connect With Server", "Error", DialogTypes.Error);
            }
            finally
            {
                objTpkTransactionDtls = null;
                objPackage = null;
                ResultTabOpenHiddenField.Value = "";
                this.RegisterJS("$.fancybox.close();");
                this.RegisterJS("$('[id$=ucGridUpdateProgress]').css('display','none');");
            }

        }

        #endregion

        #region Get Inclusion And Exclusion

        protected void GetAllItems()
        {
            objIncludingExcludingMstBLL = new IncludingExcludingMstBLL();
            try
            {
                DataTable dt = objIncludingExcludingMstBLL.GetAllItemByBusinessID(Session["BusDtl"].ToString().Split('|')[0].ToString());
              //  DataTable dtNew = dt.Select("IsIncluding='True'").CopyToDataTable();
                if (dt.Rows.Count > 0)
                {
                    if (dt.Select("IsIncluding='True'").Length>0)
                    {
                        InclusionGridView.DataSource = dt.Select("IsIncluding='True'").CopyToDataTable();
                        InclusionGridView.DataBind();
                    }
                    else
                    {
                        InclusionGridView.DataSource = null;
                        InclusionGridView.DataBind();
                    }
                    if (dt.Select("IsIncluding='False'").Length > 0)
                    {
                        ExclusionGridView.DataSource = dt.Select("IsIncluding='False'").CopyToDataTable();
                        ExclusionGridView.DataBind();
                    }
                    else
                    {
                        ExclusionGridView.DataSource = null;
                        ExclusionGridView.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {

                WebMessenger.Show(this, "Unable To Load Inclusions And Exclsuions Details At this Moment","Error", DialogTypes.Error);
                return;
            }
        }


        #endregion

        protected void AddButton_Click(object sender, EventArgs e)
        {
            int iRowIndex = (((Button)sender).Parent.Parent as GridViewRow).RowIndex;
            Panel panel = (Panel)OptionalGridView.Rows[iRowIndex].FindControl("SupplementDtlPanel");
            Label perPax = (Label)OptionalGridView.Rows[iRowIndex].FindControl("IsPerPaxLabel");
            panel.Visible = true;
            if (perPax.Text == "True")
            {
                ((TextBox)OptionalGridView.Rows[iRowIndex].FindControl("CountPersonTextBox")).Visible = true;
            }
            else
            {
                ((TextBox)OptionalGridView.Rows[iRowIndex].FindControl("CountPersonTextBox")).Visible = false;
            }
        }

        protected void AddSupplementImageButton_Click(object sender, ImageClickEventArgs e)
        {
              int iRowIndex = (((ImageButton)sender).Parent.Parent as GridViewRow).RowIndex;
              Panel panel =  (Panel)OptionalGridView.Rows[iRowIndex].FindControl("SupplementDtlPanel");
              panel.Visible = true;
             
        }
        protected void AddToPackageButton_Click(object sender, EventArgs e)
        {
            int iRowIndex = (((Button)sender).Parent.Parent as GridViewRow).RowIndex;
            Label perPax = (Label)OptionalGridView.Rows[iRowIndex].FindControl("IsPerPaxLabel");
            decimal AddtionalCost=0;
            if (perPax.Text == "True")
            {
                TextBox countNumber = (TextBox)OptionalGridView.Rows[iRowIndex].FindControl("CountPersonTextBox");
                if (countNumber.Text.Trim() == "")
                {
                    WebMessenger.Show(this, "Person Count Is Required", "Information", DialogTypes.Information);
                    return;
                }
                else if(ValidatorClass.IsNumeric(countNumber.Text.Trim())== false)
                {
                    WebMessenger.Show(this,"Invalid Character In Person Count","Information",DialogTypes.Information);
                    return;
                }
                AddtionalCost = decimal.Parse(((Label)OptionalGridView.Rows[iRowIndex].FindControl("SellingPriceLabel")).Text.Trim()) * decimal.Parse(countNumber.Text.Trim());
            }
            else
            {
               GridView firstGrid = (GridView)VehicleGridView.Rows[0].FindControl("VehicleDtlsGridView");
                GridView secondGrid = (GridView)firstGrid.Rows[0].FindControl("VehicleFinalDtlsGridView");
                int vehiclecount = secondGrid.Rows.Count;
               AddtionalCost = decimal.Parse(((Label)OptionalGridView.Rows[iRowIndex].FindControl("SellingPriceLabel")).Text.Trim()) * vehiclecount;
            }
            AdditionalCostLabel.Text = (decimal.Parse(AdditionalCostLabel.Text.Trim()) + decimal.Parse(AddtionalCost.ToString())).ToString();

        }

      


        public string retWord(int number)
        {

            if (number == 0) return "Zero";
            if (number == -2147483648) return "Minus Two Hundred and Fourteen Crore Seventy Four Lakh Eighty Three Thousand Six Hundred and Forty Eight";
            int[] num = new int[4];
            int first = 0;
            int u, h, t;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            if (number < 0)
            {
                sb.Append("Minus");
                number = -number;
            }
            string[] words0 = { "", "One ", "Two ", "Three ", "Four ", "Five ", "Six ", "Seven ", "Eight", "Nine " };
            string[] words = { "Ten ", "Eleven ", "Twelve ", "Thirteen ", "Fourteen ", "Fifteen ", "Sixteen ", "Seventeen ", "Eighteen ", "Nineteen " };

            string[] words2 = { "Twenty", "Thirty ", "Forty ", "Fifty ", "Sixty ", "Seventy ", "Eighty", "Ninety " };

            string[] words3 = { "Thousand ", "Lakh ", "Crore " };

            num[0] = number % 1000; // units

            num[1] = number / 1000;

            num[2] = number / 100000;

            num[1] = num[1] - 100 * num[2]; // thousands

            num[3] = number / 10000000; // crores

            num[2] = num[2] - 100 * num[3]; // lakhs
            for (int i = 3; i > 0; i--)
            {
                if (num[i] != 0)
                {
                    first = i;
                    break;
                }
            }
            for (int i = first; i >= 0; i--)
            {
                if (num[i] == 0) continue;

                u = num[i] % 10; // ones

                t = num[i] / 10;

                h = num[i] / 100; // hundreds

                t = t - 10 * h; // tens

                if (h > 0) sb.Append(words0[h] + "Hundred ");

                if (u > 0 || t > 0)
                {
                    if (h > 0 || i == 0) sb.Append("and ");
                    if (t == 0)
                        sb.Append(words0[u]);

                    else if (t == 1)
                        sb.Append(words[u]);
                    else
                        sb.Append(words2[t - 2] + words0[u]);
                }

                if (i != 0) sb.Append(words3[i - 1]);
            }
            return sb.ToString().TrimEnd();

        }   
    }
}