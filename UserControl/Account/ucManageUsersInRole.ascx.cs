using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Sibin.Utilities.Web.ExceptionHandling;
using System.Web.Security;
using Sibin.Utilities.Web.Messengers;

namespace SIBINUtility.UserControls.Account
{
    public partial class ucManageUsersInRole : System.Web.UI.UserControl
    {
        public delegate void AddToRole(object sender, EventArgs e);

        public event AddToRole AddUserToRole;

        public DropDownList RoleListDropDown
        {
            get { return RoleListDropDownList; }
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Page.IsPostBack == false)
            {
                LoadRoles();
            }
        }

        private void LoadRoles()
        {
            RoleListDropDownList.Items.Clear();
            RoleListDropDownList.Items.Add(new ListItem("<-- Select -->"));

            string[] userList = Roles.GetAllRoles();
            foreach (string role in userList)
	        {
                RoleListDropDownList.Items.Add(new ListItem(role));
	        }
        }

        private void LoadUsersInRole(byte shortBy)
        {
            string[] userList = {};
            userList = Roles.GetUsersInRole(RoleListDropDownList.Text);

            List<UserList> objUserList = new List<UserList>();
            Array.ForEach(userList, value => objUserList.Add(new UserList(value)));



            if (shortBy == 1) // Search by User Name
            {
                if(SearchByTextBox.Text == "")
                {
                    BRMessengers.BRInformation(this, "Please Enter User Name..");
                    return;
                }
                userList = objUserList.Where(x => x.UserName.ToLower().StartsWith(SearchByTextBox.Text.Trim().ToLower())).Select(x => x.UserName).ToArray<string>();
            }
            else if (shortBy == 2) // Search by User's Email
            {
                if (SearchByTextBox.Text == "")
                {
                    BRMessengers.BRInformation(this, "Please Enter EmailID..");
                    return;
                }
                var matchedUsers = from users in Membership.GetAllUsers().Cast<MembershipUser>().Where(x => (x.Email ?? string.Empty).ToLower() == SearchByTextBox.Text.Trim().ToLower())
                                   join usersInRole in objUserList on users.UserName equals usersInRole.UserName
                                    select new
                                    {
                                        UserName = users.UserName
                                    };

                userList = matchedUsers.Select(x => x.UserName).ToArray<string>();
            }

            DataTable usersTable = new DataTable();
            usersTable.Columns.Add(new DataColumn("UserName", typeof(string)));
           
            foreach (string UserName in userList)
            {
                usersTable.Rows.Add(UserName);
            }


            if (UserTypeDropDownList.SelectedItem.Text == "Employee")
            {

            }
            else
            {
                UsersGridView.Columns.Clear();
                BoundField bfield = new BoundField();
                bfield.DataField = "UserName";
                bfield.HeaderText = "User Name";
                bfield.ItemStyle.Width = 150;

                UsersGridView.Columns.Add(bfield);

                BoundField cfield = new BoundField();
                cfield.HeaderText = "";
                cfield.DataField = "";
                cfield.ItemStyle.Width = 100;
                UsersGridView.Columns.Add(cfield);

                UsersGridView.DataSource = usersTable;
            }
                       
            UsersGridView.DataBind();
        }

        protected void UsersGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void FindUserButton_Click(object sender, EventArgs e)
        {
            if (RoleListDropDownList.SelectedIndex <= 0)
            {
                BRMessengers.BRWarning(this, "Please Select a Role");
                return;
            }

            if (SearchByDropDownList.SelectedValue == "1")
            {
                LoadUsersInRole(1);
            }
            else if (SearchByDropDownList.SelectedValue == "2")
            {
                LoadUsersInRole(2);
            }
            
        }

       

        protected void BackButton_Click(object sender, EventArgs e)
        {
            if (Session["CreateRoleURL"] != null)
            {
                Response.Redirect(Session["CreateRoleURL"].ToString());
            }
        }


        private void LoadNonEmployeeUser()
        {
            try
            {
                UserListGridView.Columns.Clear();
                BoundField bfield = new BoundField();
                bfield.DataField = "UserName";
                bfield.HeaderText = "User Name";
                bfield.ItemStyle.Width = 150;

                UserListGridView.Columns.Add(bfield);

                BoundField cfield = new BoundField();
                cfield.HeaderText="";
                cfield.DataField = "";
                cfield.ItemStyle.Width = 100;
                UserListGridView.Columns.Add(cfield);


                //UserListGridView.DataSource = BllEmployeePicker.GetNonEmployeeUser();
                //UserListGridView.DataBind();
             
            }
            catch (Exception ex)
            {
                Session["OperationType"] = null;
                UserInterfaceExceptionHandler.HandleException(ref ex);
                Session["EncounteredException"] = ex.Message;
                Response.Redirect("~/ErrorPageForAdminSection.aspx");
            }
            finally
            {
                
            }
        }

        protected void UserTypeDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (UserTypeDropDownList.SelectedItem.Text == "Employee")
            {
                               
            }
            else if(UserTypeDropDownList.SelectedItem.Text == "Non-Employee")
            {
                LoadNonEmployeeUser();
               
            }

        }

        protected void AddToRoleEmployee_Click(object sender, EventArgs e)
        {
        
            AddUserToRole(sender, e);
           
        }

 

        protected void UserListGridView_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button AddtoRole = new Button();
                AddtoRole.ToolTip = "Add to Role";
                AddtoRole.Text = "Add To Role";
                AddtoRole.ValidationGroup = "CheckRole";
                AddtoRole.CssClass = "AccountButton";
                AddtoRole.OnClientClick = "return confirm('Are you sure you want to add to role...?');";
                AddtoRole.Click += new EventHandler(this.AddToRoleEmployee_Click);
                e.Row.Cells[UserListGridView.Columns.Count - 1].Controls.Add(AddtoRole);
               
            }
        }

        protected void RemoveUserButton_Click(object sender, EventArgs e)
        {
            try
            {

                if (RoleListDropDownList.SelectedIndex <= 0)
                {
                    BRMessengers.BRInformation(this, " Please Select a Role..");
                    return;
                }
                Button chkUserInRole = (Button)sender;
                GridViewRow gvr = (GridViewRow)(chkUserInRole.NamingContainer);

                Roles.RemoveUserFromRole(gvr.Cells[0].Text, RoleListDropDownList.Text);
                BRMessengers.BRSuccess(this, " User has been removed successfully");

              
                
            }
            catch (Exception ex)
            {
                ErrorMessage.Text = ex.Message;
            }
        }

       

        protected void UsersGridView_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button RemoveUser = new Button();
                RemoveUser.ToolTip = "Remove";
                RemoveUser.Text = "Remove User";
               // RemoveUser.ValidationGroup = "CheckRole";
                RemoveUser.CssClass = "AccountButton";
                RemoveUser.OnClientClick = "return confirm('Are you sure you want to remove user...?');";
                RemoveUser.Click += new EventHandler(this.RemoveUserButton_Click);
                e.Row.Cells[UsersGridView.Columns.Count - 1].Controls.Add(RemoveUser);
               
            }
        }
    }

    internal class UserList
    {
        public UserList()
        {
            UserName = null;
        }
        public UserList(string _userName)
        {
            UserName = _userName;
        }
        public string UserName {get; set;}
    }
}