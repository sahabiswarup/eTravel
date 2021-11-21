using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Configuration;
//using SchoolWebSiteBLL.MasterDataBLL;
using System.Data;
using Sibin.FrameworkExtensions.DotNet;
using Sibin.ExceptionHandling.ExceptionHandler;
using Sibin.Security.UserManagement;
using System.Web.Security;
using Sibin.DataStructure;
using Sibin.Utilities.Web.Messengers;
using System.Web.Profile;
using e_Travel.Class;

namespace SchoolWebSite.Account
{
    public partial class RegisterBulkUser : AdminBasePage
    {
       // SchoolSpecificMasterBLL objSchoolSpecificMasterBLL;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadAllRoleInDropDown(ddlNodeUserRole);
            }
        }
        protected void LoadAllRoleInDropDown(DropDownList ddl)
        {
            try
            {
                ddl.Items.Clear();
                ddl.Items.Add(new ListItem("-- Select --", "0"));
                ddl.DataSource = Roles.GetAllRoles();
                ddl.DataBind();
            }
            catch (Exception ex)
            {
                BRMessengers.BRError(this, ex.Message);
                return;
            }
        }
        protected void btnCreateUser_Click(object sender, EventArgs e)
        {
           // objSchoolSpecificMasterBLL = new SchoolSpecificMasterBLL();

            List<SibinMemberShipError> MembershipError = new List<SibinMemberShipError>();
            List<SibinMembership> mes = new List<SibinMembership>();
            //Call the bulk user creater

            DataTable dt = null;
                  //objSchoolSpecificMasterBLL.GetAllSchoolForUser().Tables[0];
              string[] arr2 = { ddlNodeUserRole.SelectedItem.Text };
                if (dt != null)
                {
                    try
                    {
                        Sibin.Security.UserManagement.UserManagement.RegisterBulkUser(dt, "DomainName", "DomainName", true,arr2, ref mes, ref MembershipError);
                        foreach (DataRow dr in dt.Rows)
                        {
                            ProfileBase objPB = ProfileBase.Create(dr["DomainName"].ToString());
                            objPB.SetPropertyValue("AccessTypeID", dr["EstablishmentCode"].ToString());
                            objPB.SetPropertyValue("AccessTypeName", dr["SchoolName"].ToString());
                            objPB.Save();
                        }
                        BRMessengers.BRSuccess(this, "User Created Successfully.");
                        

                    }
                    catch (Exception ex)
                    {
                        UserInterfaceExceptionHandler.HandleException(ref ex);
                        Session["EncounteredException"] = ex.Message;
                        Response.Redirect("~/ErrorPageForAdminSection.aspx");
                    }
                }
        }

    }
}