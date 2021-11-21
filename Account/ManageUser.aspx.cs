using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Sibin.ExceptionHandling.ExceptionHandler;
using System.Web.Security;

using Sibin.Utilities.Web.Messengers;
using e_TravelBLL.TourPackage;
using System.Web.Profile;
using System.Data.SqlClient;
using e_Travel.Class;



namespace e_Travel.Account
{
    public partial class ManageUser : AdminBasePage 
    {
        #region Private Veriables
        BusinessMstBLL objBusinessMstBLL;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            BindEvents();
            if (!IsPostBack)
            {
                LoadAllRoles();
                LoadAllRoleInDropDown();
                UserListCustomGridView.AddGridViewColumns("UserName|Email|Role|IsApproved|BusinessName", "UserName|Email|Role|IsApproved|BusinessName", "200|300|100|100|100", "UserName|BusinessID", true, false);
                UserListCustomGridView.BindData();
            }
        }
        private void BindEvents()
        {
            UserListCustomGridView.ucGridView_SelectedIndexChanging += new GridViewSelectEventHandler(UserListCustomGridView_SelectedIndexChanging);
            UserListCustomGridView.ucGridView_RowDeleting += new GridViewDeleteEventHandler(UserListCustomGridView_RowDeleting);
            UserListCustomGridView.DataSource += new SIBINUtility.UserControl.GridView.ucGridView.GridViewDataSource(LoadGridView);
            
            BusinessCustomGridView.ucGridView_SelectedIndexChanging += new GridViewSelectEventHandler(BusinessCustomGridView_SelectedIndexChanging);
            BusinessCustomGridView.DataSource += new SIBINUtility.UserControl.GridView.ucGridView.GridViewDataSource(LoadBusinessGridView);
        }

        protected void UserListCustomGridView_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            ViewState["UserName"] = UserListCustomGridView.DataKeys[e.NewSelectedIndex].Values["UserName"].ToString();
            UserNameLabel.Text = ViewState["UserName"].ToString();
            UserRoleLabel.Text = HttpUtility.HtmlDecode(UserListCustomGridView.Rows[e.NewSelectedIndex].Cells[3].Text);
               
        }
        protected void BusinessCustomGridView_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
           string BusinessID =BusinessCustomGridView.DataKeys[e.NewSelectedIndex].Values["BusinessID"].ToString();
           string BusinessName = HttpUtility.HtmlDecode(BusinessCustomGridView.Rows[e.NewSelectedIndex].Cells[1].Text);
           foreach (var role in Roles.GetRolesForUser(ViewState["UserName"].ToString()))
           {
               Roles.RemoveUserFromRole(ViewState["UserName"].ToString(), role);
           }
           Roles.AddUserToRole(ViewState["UserName"].ToString(), ViewState["Role"].ToString());
           ProfileBase objPB = ProfileBase.Create(ViewState["UserName"].ToString());
           objPB.SetPropertyValue("BusinessID",BusinessID);
           objPB.SetPropertyValue("BusinessName",BusinessName);
           objPB.Save();
           UserListCustomGridView.BindData();
           WebMessenger.Show(this, ViewState["UserName"].ToString() + " has Been Successfully Assigned To Role " + ViewState["Role"].ToString() + ".", "Added To Role", DialogTypes.Success);
        }
        protected void UserListCustomGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            
        }
        private DataTable LoadGridView()
        { 
            DataTable dt = new DataTable();
            try
            {
                int startRowIndex = (UserListCustomGridView.PageIndex * UserListCustomGridView.PageSize);
                
                var result = from User in Membership.GetAllUsers().Cast<MembershipUser>()
                             select new
                             {
                                 UserName = User.UserName,
                                 Email = User.Email==null?"Null" :User.Email,
                                 Role = Roles.GetRolesForUser(User.UserName).Count() == 0 ? null : Roles.GetRolesForUser(User.UserName)[0],
                                 IsApproved = User.IsApproved,
                                 BusinessID = ProfileBase.Create(User.UserName).GetPropertyValue("BusinessID") == null ? "Null" : ProfileBase.Create(User.UserName).GetPropertyValue("BusinessID").ToString(),
                                 BusinessName = ProfileBase.Create(User.UserName).GetPropertyValue("BusinessName") == null ? "Null" : ProfileBase.Create(User.UserName).GetPropertyValue("BusinessName").ToString(),
                             };
                if (!string.IsNullOrWhiteSpace(UserNameTextBox.Text.Trim()))
                {
                    result = result.Where(x => x.UserName.StartsWith(UserNameTextBox.Text.Trim()));
                }
                if (!string.IsNullOrWhiteSpace(UserEmailTextBox.Text))
                {
                    result = result.Where(x => x.Email.StartsWith(UserEmailTextBox.Text.Trim()));
                }
                if (UserRoleDropDownList.SelectedIndex > 0)
                {
                    result = result.Where(x => x.Role == UserRoleDropDownList.SelectedValue && x.Role != null);
                }
                if (!string.IsNullOrWhiteSpace(BusinessNameTextBox.Text.Trim()))
                {
                    result = result.Where(x => x.BusinessName.Contains(BusinessNameTextBox.Text.Trim()));
                }
                dt= result.OrderBy(x => x.UserName).Skip(startRowIndex).Take(UserListCustomGridView.PageSize).CopyToDataTableExt();
                if (dt.Rows.Count > 0)
                {
                    UserListCustomGridView.TotalRecordCount = result.Count();
                }
            }
            catch (Exception)
            {
                WebMessenger.Show(this, "Error Occured While Loading User ", "Error", DialogTypes.Error);
            }
           
            return dt;
        }
        private DataTable LoadBusinessGridView()
        {
            objBusinessMstBLL = new BusinessMstBLL();
            DataTable dt = new DataTable();
            try
            {
                int startRowIndex = (BusinessCustomGridView.PageIndex * BusinessCustomGridView.PageSize) + 1;
                dt = objBusinessMstBLL.GetBusinessListDetails(startRowIndex, BusinessCustomGridView.PageSize, null);
                if (dt.Rows.Count > 0)
                {
                    BusinessCustomGridView.TotalRecordCount = int.Parse(dt.Rows[0]["TotalRows"].ToString());
                }
            }
            catch (Exception)
            {
                WebMessenger.Show(this.Page, "Error Occured While Loading Business Details", "Error", DialogTypes.Error);
            }
            finally
            {
                objBusinessMstBLL = null;
            }
            return dt;
        }
        
        protected void LoadAllRoles()
        {
            DataTable dt = new DataTable();
            string[] RoleList = Roles.GetAllRoles();
            dt.Columns.Add("RoleName", typeof(string));
            foreach (string role in RoleList)
            {
                dt.Rows.Add(role);
            }
            RoleListGridView.DataSource = dt;
            RoleListGridView.DataBind();
        }

        protected void SearchUserButton_Click(object sender, EventArgs e)
        {
            UserListCustomGridView.SetPageIndex = 0;
            UserListCustomGridView.BindData();
        }
        protected void ResetPasswordButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (ViewState["UserName"] != null)
                {
                    MembershipUser usr = Membership.GetUser(ViewState["UserName"].ToString());
                    usr.ChangePassword(usr.ResetPassword(), usr.UserName + "123");
                    Membership.UpdateUser(usr);
                    UserListCustomGridView.BindData();
                    WebMessenger.Show(this, "User Password Updated Successfully", "Password Reset", DialogTypes.Success);

                }
                else
                {
                    WebMessenger.Show(this, "Select A User First", "Password Reset", DialogTypes.Information);
                }
            }
            catch (Exception)
            {
                WebMessenger.Show(this, "Unable to Reset Password at this Moment", "Error", DialogTypes.Error);
                return;
            }
        }
        protected void BackLinkButton_Click(object sender, EventArgs e)
        {
            UserListPanel.Visible = true;
            UserDetailsPanel.Visible = false;
        }

        protected void LockUserButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (ViewState["UserName"] != null)
                {
                    MembershipUser usr = Membership.GetUser(ViewState["UserName"].ToString());
                    usr.IsApproved = !usr.IsApproved;
                    Membership.UpdateUser(usr);
                    UserListCustomGridView.BindData();
                    WebMessenger.Show(this, "User Locked Sucessfully", "User Locked", DialogTypes.Success);
                }
                else
                {
                    WebMessenger.Show(this, "Select A User First", "Lock User", DialogTypes.Information);
                }
            }
            catch (Exception)
            {
                WebMessenger.Show(this, "Unable To Lock User At this Moment", "Error", DialogTypes.Error);
                return;
            }
        }
        protected void AddUserInRoleButton_Click(object sender, EventArgs e)
        {
            try
            {
                UserListPanel.Visible = false;
                UserDetailsPanel.Visible = true;
                LoadAllRoles();
            }
            catch (Exception ex)
            {
                WebMessenger.Show(this, "Unable to Load All Roles", "Error", DialogTypes.Error);
            }
           
        }
       
        protected void LoadAllRoleInDropDown( )
        {
            try
            {
                UserRoleDropDownList.Items.Clear();
                UserRoleDropDownList.Items.Add(new ListItem("-- Select --", "0"));
                UserRoleDropDownList.DataSource = Roles.GetAllRoles();
                UserRoleDropDownList.DataBind();
            }
            catch (Exception ex)
            {
                WebMessenger.Show(this, "Unable To Load All Roles at this Moment", "Error", DialogTypes.Error);
                return;
            }
        }
        protected void RoleListGridView_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            ViewState["Role"] = HttpUtility.HtmlDecode(RoleListGridView.Rows[e.NewSelectedIndex].Cells[1].Text);
            if (ViewState["Role"].ToString() == "BusinessAdmin")
            {
                ConfirmationPanel.Visible = true;
                LoadBusinessGrid();
            }
            else
            {
                foreach (var role in Roles.GetRolesForUser(ViewState["UserName"].ToString()))
                {
                    Roles.RemoveUserFromRole(ViewState["UserName"].ToString(), role);
                }
                Roles.AddUserToRole(ViewState["UserName"].ToString(), ViewState["Role"].ToString());
                ProfileBase objPB = ProfileBase.Create(ViewState["UserName"].ToString());
                objPB.SetPropertyValue("BusinessID", null);
                objPB.SetPropertyValue("BusinessName", null);
                objPB.Save();
                UserListCustomGridView.BindData();
                WebMessenger.Show(this, ViewState["UserName"].ToString() + " has Been Successfully Assigned To Role " + ViewState["Role"].ToString() + ".", "Added To Role", DialogTypes.Success);
            }
        }

        protected void LoadBusinessGrid()
        {
            BusinessCustomGridView.AddGridViewColumns("Business Name|Business Address|Country|Destination", "BusName|BusAddress|CountryName|DestinationName", "200|300|100|100", "BusinessID", true, false);
            BusinessCustomGridView.BindData();
        }
      
        
    }
}