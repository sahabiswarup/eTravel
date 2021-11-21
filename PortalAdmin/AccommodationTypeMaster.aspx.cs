using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using e_TravelBLL.TourPackage;
using Sibin.ExceptionHandling.ExceptionHandler;
using SIBINUtility.ValidatorClass;
using e_Travel.Class;

namespace e_Travel.PortalAdmin
{
    public partial class AccommodationTypeMaster : AdminBasePage
    {
       
        #region Private Variables
        AccommodationTypeMstBLL objPtlAdmAccommodationTypeMstBLL;   
        #endregion

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            BindEvents();
            if (!IsPostBack)
            {
                AccommodationTypeMasterCustomGridView.AddGridViewColumns("AccTypeName|Description", "AccTypeName|AccTypeDesc", "300|450", "AccTypeID", true, true);
                AccommodationTypeMasterCustomGridView.BindData();
                DisableInputControls();
                ClearInputControls();
            }
        }
        #endregion

        #region Gridview Events and Load GridView
        private void BindEvents()
        {
            AccommodationTypeMasterCustomGridView.ucGridView_SelectedIndexChanging += new GridViewSelectEventHandler(AccommodationTypeMasterCustomGridView_SelectedIndexChanging);
            AccommodationTypeMasterCustomGridView.ucGridView_RowDeleting += new GridViewDeleteEventHandler(AccommodationTypeMasterCustomGridView_RowDeleting);
            AccommodationTypeMasterCustomGridView.DataSource += new SIBINUtility.UserControl.GridView.ucGridView.GridViewDataSource(LoadGridView);
        }

        protected void AccommodationTypeMasterCustomGridView_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            Session["OpernType"] = null;
            DisableInputControls();
            AccommodationTypeNameTextBox.Text = HttpUtility.HtmlDecode(AccommodationTypeMasterCustomGridView.Rows[e.NewSelectedIndex].Cells[1].Text);
            AccTypeDescTextBox.Text = HttpUtility.HtmlDecode(AccommodationTypeMasterCustomGridView.Rows[e.NewSelectedIndex].Cells[2].Text);
        }

        protected void AccommodationTypeMasterCustomGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            objPtlAdmAccommodationTypeMstBLL = new  AccommodationTypeMstBLL();
            try
            {
                int status =0;
                objPtlAdmAccommodationTypeMstBLL.DeleteAccommodationType(int.Parse(AccommodationTypeMasterCustomGridView.DataKeys[e.RowIndex].Values["AccTypeID"].ToString()),Page.User.Identity.Name,ref status);

                if (status == 0)
                {
                    AccommodationTypeMasterCustomGridView.BindData();
                    WebMessenger.Show(this, "Accommodation Type Deleted Successfully", "Deleted", DialogTypes.Success);
                }
                else
                {
                    WebMessenger.Show(this, "Unable To Delete Selected Record As It Is Refence By Other Record", "Relation Exist", DialogTypes.Information);
                }
            }
            catch (Exception ex)
            {
                UserInterfaceExceptionHandler.HandleException(ref ex);
                Session["OpernType"] = null;
                WebMessenger.Show(this, "Unable To Delete Accommodation Type At This Moment", "Error", DialogTypes.Error);
            }
            finally
            {
                objPtlAdmAccommodationTypeMstBLL = null;
            }
        }

        private DataTable LoadGridView()
        {
            objPtlAdmAccommodationTypeMstBLL = new AccommodationTypeMstBLL();
            DataTable dtRec = new DataTable();
            try
            {
                int startRowIndex = (AccommodationTypeMasterCustomGridView.PageIndex * AccommodationTypeMasterCustomGridView.PageSize) + 1;
                dtRec = objPtlAdmAccommodationTypeMstBLL.GetAllAccommodationTypePaged(startRowIndex, AccommodationTypeMasterCustomGridView.PageSize);
                if (dtRec.Rows.Count > 0)
                {
                    AccommodationTypeMasterCustomGridView.TotalRecordCount = int.Parse(dtRec.Rows[0]["TotalRows"].ToString());
                }
            }
            catch (Exception)
            {
                WebMessenger.Show(this, "Unable To Load Accommodation Type List At This Moment", "Error", DialogTypes.Error);
            }
            finally
            {
                objPtlAdmAccommodationTypeMstBLL = null;
            }
            return dtRec;
        }
        #endregion
        
        #region Button Click Events
        protected void AddButton_Click(object sender, EventArgs e)
        {
            EnableInputControls();
            ClearInputControls();
            AccommodationTypeNameTextBox.Focus();
            this.Session["OpernType"] = "Add";
        }

        protected void EditButton_Click(object sender, EventArgs e)
        {
            if (AccommodationTypeMasterCustomGridView.SelectedDataKey == null)
            {
                WebMessenger.Show(this, "Select A Record From List First", "Information", DialogTypes.Information);
                return;
            }
            else
            {
                EnableInputControls();
                AccommodationTypeNameTextBox.Focus();
                this.Session["OpernType"] = "Edit";
            }
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            objPtlAdmAccommodationTypeMstBLL = new AccommodationTypeMstBLL();
            //Validation
            if (AccommodationTypeNameTextBox.Text.Trim() == "")
            {
                WebMessenger.Show(this, "  Accommodation Type Name Is Required", "Information", DialogTypes.Information);
                AccommodationTypeNameTextBox.Focus();
                return;
            }
            if (ValidatorClass.ValidateText(AccommodationTypeNameTextBox.Text.Trim()) == false)
            {
                WebMessenger.Show(this, " Invalid Accommodation Type Name", "Information", DialogTypes.Information);
                AccommodationTypeNameTextBox.Focus();
                return;
            }
            if (AccTypeDescTextBox.Text.Trim() == "")
            {
                WebMessenger.Show(this, " Accommodation Type Description Is Required", "Information", DialogTypes.Error);
                AccTypeDescTextBox.Focus();
                return;
            }
            if (ValidatorClass.ValidateText(AccTypeDescTextBox.Text.Trim()) == false)
            {
                WebMessenger.Show(this, "Invalid Accommodation Type Desc", "Information", DialogTypes.Error);
                AccTypeDescTextBox.Focus();
                return;
            }

            try
            {
                if (Page.IsValid)
                {
                    if (Session["OpernType"] != null)
                    {
                        if (Session["OpernType"].ToString() == "Add")
                        {
                            objPtlAdmAccommodationTypeMstBLL.AddAccommodationType(AccommodationTypeNameTextBox.Text.Trim(), AccTypeDescTextBox.Text.Trim(), false, Page.User.Identity.Name);
                            AccommodationTypeMasterCustomGridView.BindData();
                            ClearInputControls();
                            DisableInputControls();
                            Session["OpernType"] = null;
                            WebMessenger.Show(this, "Accommodation Type Added Sucessfully", "Success", DialogTypes.Success);
                        }
                        else if (Session["OpernType"].ToString() == "Edit")
                        {
                            objPtlAdmAccommodationTypeMstBLL.EditAccommodationType(long.Parse(AccommodationTypeMasterCustomGridView.SelectedDataKey.Values["AccTypeID"].ToString()), AccommodationTypeNameTextBox.Text.Trim(), AccTypeDescTextBox.Text.Trim(), false, Page.User.Identity.Name);
                            AccommodationTypeMasterCustomGridView.BindData();
                            ClearInputControls();
                            DisableInputControls();
                            Session["OpernType"] = null;
                            WebMessenger.Show(this, "Accommodation Type Updated Sucessfully", "Success", DialogTypes.Success);
                        }
                        else
                        {
                            WebMessenger.Show(this, "Invalid Operation", "Operation", DialogTypes.Information);
                            ClearInputControls();
                            DisableInputControls();
                            Session["OpernType"] = null;
                        }
                    }
                    else
                    {
                        WebMessenger.Show(this, "Select A Opeartion First ", "Information", DialogTypes.Information);
                        return;
                    }
                }
                else
                {
                    WebMessenger.Show(this, "Invalid Operation", "Operation", DialogTypes.Information);
                    ClearInputControls();
                    DisableInputControls();
                    Session["OpernType"] = null;
                }
            }
            catch (Exception ex)
            {
                UserInterfaceExceptionHandler.HandleException(ref ex);
                Session["OpernType"] = null;
                WebMessenger.Show(this, ex.Message, "Error", DialogTypes.Error);
            }
            finally
            {
                objPtlAdmAccommodationTypeMstBLL = null;
            }
        }

        protected void ClearButton_Click(object sender, EventArgs e)
        {
            ClearInputControls();
            DisableInputControls();
            this.Session["OpernType"] = null;
        }
        #endregion

        #region Enable/Disable/Clear Form Elements
        private void EnableInputControls()
        {
            AccommodationTypeNameTextBox.Enabled = true;
            AccTypeDescTextBox.Enabled = true;
        }

        private void DisableInputControls()
        {
            AccommodationTypeNameTextBox.Enabled = false;
            AccTypeDescTextBox.Enabled = false;
        }

        private void ClearInputControls()
        {
            AccommodationTypeNameTextBox.Text = "";
            AccTypeDescTextBox.Text = "";
        }
        #endregion
        }
    }
