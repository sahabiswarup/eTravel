using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace SIBINUtility.UserControl.SearchGridView
{
    public partial class ucSearchGridView : System.Web.UI.UserControl
    {
        public delegate DataTable GridViewDataSource();

        #region Public GridView Event Decleration
        public event GridViewDeleteEventHandler ucGridView_RowDeleting;
        public event GridViewSelectEventHandler ucGridView_SelectedIndexChanging;
        public event GridViewPageEventHandler ucGridView_PageIndexChanging;
        public event EventHandler ucGridView_SearchButtonClick;
        public event GridViewDataSource DataSource;
        #endregion

        #region Private Variables and Public Properties

        public string SearchLabelText
        {
            set { SearchLabel.Text=value; }
        }

        public System.Web.UI.WebControls.TextBox TextBoxRef
        {
            get { return SearchTextBox; }
        }

        public System.Web.UI.WebControls.GridView GridViewRef
        {
            get { return refGridView; }
        }

        public string DataSourceID
        {
            set { refGridView.DataSourceID = value; }
            get { return refGridView.DataSourceID; }
        }
        public string TextValue
        {

            get { return SearchTextBox.Text.Trim(); }
        }

        public bool AutoGenerateColumns
        {
            set { refGridView.AutoGenerateColumns = value; }
            get { return refGridView.AutoGenerateColumns; }
        }

        public int PageSize
        {
            set { refGridView.PageSize = value; }
            get { return refGridView.PageSize; }
        }

        public int PageIndex
        {
            set { refGridView.PageIndex = value; }
            get { return refGridView.PageIndex; }
        }

        public GridLines GridLines
        {
            set { refGridView.GridLines = value; }
            get { return refGridView.GridLines; }
        }

        public bool AllowSorting
        {
            set { refGridView.AllowSorting = value; }
            get { return refGridView.AllowSorting; }
        }

        public bool AllowPaging
        {
            set { refGridView.AllowPaging = value; }
            get { return refGridView.AllowPaging; }
        }

        public DataControlFieldCollection Columns
        {
            get { return refGridView.Columns; }
            set
            {
                foreach (DataControlField item in value)
                {
                    refGridView.Columns.Add(item);
                }
            }
        }

        public Unit Width
        {
            get { return refGridView.Width; }
            set { refGridView.Width = value; }
        }

        private string _dtFormatStr = "dd-MMM-yyyy";

        public string DateTimeFormat
        {
            get { return _dtFormatStr; }
            set { _dtFormatStr = value; }
        }

        public DataKeyArray DataKeys
        {
            get { return refGridView.DataKeys; }
        }

        public GridViewRowCollection Rows
        {
            get { return refGridView.Rows; }
        }

        public GridViewRow SelectedRow
        {
            get
            {
                return refGridView.SelectedRow;
            }
        }

        public DataKey SelectedDataKey
        {
            get
            {
                return refGridView.SelectedDataKey;
            }
        }

        public int TotalRecordCount
        {
            get
            {
                return int.Parse(TotalPageCountHiddenField.Value);
            }
            set
            {
                TotalPageCountHiddenField.Value = value.ToString();
            }
        }

        public int SetPageIndex
        {
            set
            {
                PageIndexHiddenField.Value = value.ToString();
            }
        }
        #endregion

        #region Gridview Event Implementation
        public void BindData()
        {
            if (DataSource != null)
            {
                refGridView.PageIndex = int.Parse(PageIndexHiddenField.Value);
                refGridView.DataSource = DataSource();
                refGridView.DataBind();
            }
        }

        protected void PageSizeDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            PageIndexHiddenField.Value = "0";
            refGridView.PageSize = int.Parse(((DropDownList)sender).SelectedValue);
            BindData();
        }

        protected void PageIndexDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            PageIndexHiddenField.Value = (int.Parse(((DropDownList)sender).SelectedValue) - 1).ToString();
            BindData();
        }

        protected void refGridView_DataBound(object sender, EventArgs e)
        {
            GridViewRow gvrPager = refGridView.BottomPagerRow;
            //Check if pager is available 
            if (gvrPager == null)
            {
                return;
            }

            DropDownList ddlPageIndex = (DropDownList)gvrPager.FindControl("PageIndexDropDownList");
            Literal TotalPageCount = (Literal)gvrPager.FindControl("NoOfPagesLiteral");
            DropDownList ddlPageSize = (DropDownList)gvrPager.FindControl("PageSizeDropDown");
            Literal TotalRecCountLiteral = (Literal)gvrPager.FindControl("gridViewTotalRecordLiteral");

            TotalRecCountLiteral.Text = TotalRecordCount + " Record(s) Found...";
            int TotalDataPageCount = (int)Math.Ceiling((decimal)TotalRecordCount / (decimal)PageSize);

            if (TotalDataPageCount == 0)
            {
                TotalDataPageCount = 1;
            }

            //Literal TotalNoOfRecords = (Literal)gvrPager.FindControl("NoOfRecordsLiteral");
            //Bind the Items Pagable Drop down
            if (ddlPageIndex != null)
            {
                // populate pager
                for (int i = 0; i < TotalDataPageCount; i++)
                {
                    int intPageNumber = i + 1;
                    ListItem lstItem = new ListItem(intPageNumber.ToString());
                    if (intPageNumber == int.Parse(PageIndexHiddenField.Value) + 1)
                    {
                        lstItem.Selected = true;
                    }
                    ddlPageIndex.Items.Add(lstItem);
                }
            }

            // Render Paging
            if (TotalPageCount != null)
            {
                TotalPageCount.Text = TotalDataPageCount.ToString();
            }

            if (ddlPageSize != null)
            {
                foreach (ListItem item in ddlPageSize.Items)
                {
                    if (item.Value == PageSize.ToString())
                    {
                        item.Selected = true;
                    }
                }
            }
            #region Manage Button States

            Button btnFirst = (Button)gvrPager.FindControl("FirstButton");
            Button btnPrev = (Button)gvrPager.FindControl("PreviousButton");
            Button btnNext = (Button)gvrPager.FindControl("NextButton");
            Button btnLast = (Button)gvrPager.FindControl("LastButton");

            //now figure out what page we're on
            //if (int.Parse(PageIndexHiddenField.Value) == 0)
            //{
            //    btnPrev.Enabled = false;
            //    btnFirst.Enabled = false;
            //}
            //else if (int.Parse(PageIndexHiddenField.Value) + 1 >= TotalDataPageCount)
            //{
            //    btnLast.Enabled = false;
            //    btnNext.Enabled = false;
            //}
            //else
            //{
            //    btnLast.Enabled = true;
            //    btnNext.Enabled = true;
            //    btnPrev.Enabled = true;
            //    btnFirst.Enabled = true;
            //}
            #endregion

            refGridView.SelectedIndex = -1;

            #region Enable pager and make it visible
            gvrPager.Enabled = true;
            gvrPager.Visible = true;
            #endregion
        }

        protected void refGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DateTime objColValue;
                for (int i = 1; i < e.Row.Cells.Count; i++)
                {
                    if (e.Row.Cells[i].Text.Contains(".") == false || (e.Row.Cells[i].Text.Contains(".") == true && ((e.Row.Cells[i].Text.Length - e.Row.Cells[i].Text.Replace(".", "").Length) > 1)))
                    {
                        if (DateTime.TryParse(e.Row.Cells[i].Text, out objColValue) == true)
                        {
                            e.Row.Cells[i].Text = objColValue.ToString(DateTimeFormat);
                        }
                    }
                }
            }
        }

        protected void refGridView_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && refGridView.Columns[refGridView.Columns.Count - 1] is CommandField)
            {
                ImageButton ib = new ImageButton();
                ib.ID = "ucGridDeleteButton";
                ib.OnClientClick = "return confirm('Are you sure you want to delete the selected record...?');";
                ib.ToolTip = "Delete";
                ib.ImageUrl = "~/UserControl/GridView/GridViewIcons.ashx?T=D";
                ib.CommandName = "Delete";
                ib.CommandArgument = "Delete";
                e.Row.Cells[refGridView.Columns.Count - 1].Controls.Add(ib);
            }
        }

        protected void refGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (ucGridView_RowDeleting != null)
            {
                ucGridView_RowDeleting(sender, e);
            }
            else
            {
                e.Cancel = true;
            }
        }

        protected void refGridView_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            if (ucGridView_SelectedIndexChanging != null)
            {
                ucGridView_SelectedIndexChanging(sender, e);
            }
            else
            {
                e.Cancel = true;
            }
        }

        protected void refGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (ucGridView_PageIndexChanging != null)
            {
                ucGridView_PageIndexChanging(sender, e);
            }
        }

        protected void PageButton_Click(object sender, EventArgs e)
        {
            GridViewRow gvrPager = refGridView.BottomPagerRow;
            //Check if pager is available 
            if (gvrPager == null)
            {
                return;
            }
            DropDownList ddlPageIndex = (DropDownList)gvrPager.FindControl("PageIndexDropDownList");
            DropDownList ddlPageSize = (DropDownList)gvrPager.FindControl("PageSizeDropDown");

            refGridView.PageSize = int.Parse(ddlPageSize.SelectedValue);

            switch (((Button)sender).CommandArgument)
            {
                case "Prev":
                    if (ddlPageIndex.SelectedIndex - 1 >= 0)
                    {
                        PageIndexHiddenField.Value = (ddlPageIndex.SelectedIndex - 1).ToString();
                    }
                    break;
                case "Next":
                    if (int.Parse(((Literal)gvrPager.FindControl("NoOfPagesLiteral")).Text.ToString()) > int.Parse(ddlPageIndex.SelectedItem.Text.ToString()))
                    {
                        PageIndexHiddenField.Value = (ddlPageIndex.SelectedIndex + 1).ToString();
                    }
                    break;
                case "First":
                    if (ddlPageIndex.Items.Count > 0)
                    {
                        PageIndexHiddenField.Value = "0";
                    }
                    break;
                case "Last":
                    if (ddlPageIndex.Items.Count > 0)
                    {
                        PageIndexHiddenField.Value = (ddlPageIndex.Items.Count - 1).ToString();
                    }
                    break;
            }

            BindData();
        }
        #endregion

        #region Add GridView Columns
        public void AddGridViewColumns(string HeaderTexts, string DataFields, string ColumnWidths, string DataKeysNames, bool ShowSelectButton, bool ShowDeleteButton)
        {
            #region Validate ParameterValue
            if (string.IsNullOrEmpty(HeaderTexts))
            {
                throw (new Exception("Header text should not be null or empty."));
            }
            if (string.IsNullOrEmpty(DataFields))
            {
                throw (new Exception("Data fields should not be null or empty."));
            }
            #endregion

            #region Split Parameter Values
            string[] HeaderText = HeaderTexts.Split('|');
            string[] ColumnWidth = null;
            if (!string.IsNullOrEmpty(ColumnWidths))
            {
                ColumnWidth = ColumnWidths.Split('|');
            }
            string[] DataField = DataFields.Split('|');
            string[] DataKeysName = null;
            if (!string.IsNullOrEmpty(DataKeysNames))
            {
                DataKeysName = DataKeysNames.Split('|');
                refGridView.DataKeyNames = DataKeysName;
            }
            #endregion

            refGridView.Columns.Clear();
            refGridView.SelectedIndex = -1;

            if (HeaderText.Length == DataField.Length)
            {
                if (ShowSelectButton == true)
                {
                    CommandField cf = new CommandField();
                    cf.ShowSelectButton = true;
                    cf.ButtonType = ButtonType.Image;
                    cf.ItemStyle.Width = 30;
                    cf.HeaderStyle.Width = 30;
                    cf.SelectImageUrl = "~/UserControl/GridView/GridViewIcons.ashx?T=S";
                    refGridView.Columns.Add(cf);
                }

                for (int i = 0; i < DataField.Length; i++)
                {
                    BoundField bf = new BoundField();
                    bf.DataField = DataField[i];
                    bf.HeaderText = HeaderText[i];

                    if (ColumnWidth != null && ColumnWidth.Length > i)
                    {
                        int colWidth;
                        if (int.TryParse(ColumnWidth[i], out colWidth))
                        {
                            bf.ItemStyle.Width = colWidth;
                            bf.HeaderStyle.Width = colWidth;
                        }
                    }

                    refGridView.Columns.Add(bf);
                }

                if (ShowDeleteButton == true)
                {
                    CommandField cfd = new CommandField();
                    cfd.ItemStyle.Width = 30;
                    cfd.HeaderStyle.Width = 30;
                    refGridView.Columns.Add(cfd);
                }
            }
            else
            {
                throw (new Exception("No. of header text specified is not same with the no. of datafield specified."));
            }
        }
        #endregion

        //public void refGridView_TextChanged(object sender, EventArgs e)
        //{

        //    if (ucGridView_UserTextChanged != null)
        //    {
        //        ucGridView_UserTextChanged(sender, e);
        //    }


        //}

        public void SearchButton_Click(object sender, EventArgs e)
        {

             if (ucGridView_SearchButtonClick != null)
             {
                 ucGridView_SearchButtonClick(sender, e);
             }


        }

    }
}