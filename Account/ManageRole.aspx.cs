using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sibin.Utilities.Web.Messengers;
using System.Web.Security;
using e_Travel.Class;

namespace e_Travel.Account
{
    public partial class ManageRole : AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ucManageUsersInRole1.AddUserToRole += new SIBINUtility.UserControls.Account.ucManageUsersInRole.AddToRole(AddToRoleEmployee_Click);
        }
        protected void AddToRoleEmployee_Click(object sender, EventArgs e)
        {
            try
            {
                if (ucManageUsersInRole1.RoleListDropDown.SelectedIndex <= 0)
                {
                    BRMessengers.BRInformation(this, "Please Select a Role..");
                    return;
                }
                Button chkUserInRole = (Button)sender;
                GridViewRow gvr = (GridViewRow)(chkUserInRole.NamingContainer);

                Roles.AddUserToRole(gvr.Cells[0].Text, ucManageUsersInRole1.RoleListDropDown.Text);
                BRMessengers.BRSuccess(this, "User has been added to role succcessfully");

            }
            catch (Exception ex)
            {
                BRMessengers.BRError(this,ex.Message);
                return;
            }
        }
    }
}