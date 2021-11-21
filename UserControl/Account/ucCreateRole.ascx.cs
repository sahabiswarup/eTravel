using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;
using System.IO;
using Sibin.Utilities.Web.Messengers;

namespace SIBINUtility.UserControls.Account
{
    public partial class ucCreateRole : System.Web.UI.UserControl
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Page.IsPostBack == false)
            {
                LoadRoles();
            }
        }

        private void LoadRoles()
        {
            DataTable rolesTable = new DataTable("UserRoles");
            rolesTable.Columns.Add("RoleName", typeof(String));

            foreach (var item in Roles.GetAllRoles())
            {
                rolesTable.Rows.Add(item);
            }

            RolesGridView.DataSource = rolesTable;
            RolesGridView.DataBind();
        }

        protected void CreateRoleButton_Click(object sender, EventArgs e)
        {
            Roles.CreateRole(RoleName.Text.Trim());
            BRMessengers.BRSaveSuccess(this);
            RoleName.Text = "";
            LoadRoles();
        }

        protected void RolesGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                Roles.DeleteRole(RolesGridView.Rows[e.RowIndex].Cells[0].Text);
                LoadRoles();
            }
            catch(Exception ex)
            {
                ErrorMessage.Text = ex.Message;
            }
        }
    }


}